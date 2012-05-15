// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebMapPlugin.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Data;
using DotSpatial.Plugins.WebMap.Resources;
using DotSpatial.Plugins.WebMap.Tiling;
using DotSpatial.Plugins.WebMap.WMS;
using DotSpatial.Projections;
using DotSpatial.Topology;
using Microsoft.VisualBasic;

namespace DotSpatial.Plugins.WebMap
{
    public class WebMapPlugin : Extension
    {
        #region Constants and Fields

        private const string Other = "Other";

        private const string PluginName = "FetchBasemap";

        private const string STR_KeyOpacityDropDown = "kOpacityDropDown";

        private const string STR_KeyServiceDropDown = "kServiceDropDown";
        private readonly ProjectionInfo WebMercProj = ProjectionInfo.FromEsriString(KnownCoordinateSystems.Projected.World.WebMercator.ToEsriString());
        private readonly ProjectionInfo Wgs84Proj = ProjectionInfo.FromEsriString(KnownCoordinateSystems.Geographic.World.WGS1984.ToEsriString());

        //reference to the main application and its UI items
        private MapImageLayer _baseMapLayer;

        private Bitmap _basemapImage;

        private BackgroundWorker _bw;

        private ServiceProvider _emptyProvider;

        private IMapFeatureLayer _featureSetLayer;

        private Int16 _opacity = 100;

        private DropDownActionItem _opacityDropDown;

        private ServiceProvider _provider;

        private DropDownActionItem _serviceDropDown;

        private TileManager _tileManager;

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialize the DotSpatial plugin
        /// </summary>
        public override void Activate()
        {
            // Add Menu or Ribbon buttons.
            AddButtons();

            //Create the tile manager
            _tileManager = new TileManager();

            //Add handlers for saving/restoring settings
            App.SerializationManager.Serializing += SerializationManagerSerializing;
            App.SerializationManager.Deserializing += SerializationManagerDeserializing;
            App.SerializationManager.NewProjectCreated += SerializationManagerNewProject;

            //EnsureMapProjectionIsWebMercator();

            //Setup the background worker
            _bw = new BackgroundWorker { WorkerSupportsCancellation = true, WorkerReportsProgress = true };
            _bw.DoWork += BwDoWork;
            _bw.RunWorkerCompleted += BwRunWorkerCompleted;
            _bw.ProgressChanged += BwProgressChanged;

            base.Activate();
        }

        /// <summary>
        /// Fires when the plugin should become inactive
        /// </summary>
        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();

            DisableBasemapLayer();

            //remove handlers for saving/restoring settings
            App.SerializationManager.Serializing -= SerializationManagerSerializing;
            App.SerializationManager.Deserializing -= SerializationManagerDeserializing;
            App.SerializationManager.NewProjectCreated -= SerializationManagerNewProject;

            base.Deactivate();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the input bitmap after making it transparent
        /// </summary>
        /// <returns>Opacity-modified bitmap of the basemap image</returns>
        private static Bitmap GetTransparentBasemapImage(Bitmap originalImage, int opacity)
        {
            if (originalImage == null)
                return null;

            try
            {
                var newImage = new Bitmap(originalImage.Width, originalImage.Height);
                using (var gfxPic = Graphics.FromImage(newImage))
                {
                    using (var iaPic = new ImageAttributes())
                    {
                        var cmxPic = new ColorMatrix { Matrix33 = opacity / 100f };
                        iaPic.SetColorMatrix(cmxPic, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                        gfxPic.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height), 0, 0, originalImage.Width, originalImage.Height, GraphicsUnit.Pixel, iaPic);
                    }
                }

                return newImage;
            }
            catch
            {
                return originalImage;
            }
        }

        /// <summary>
        /// Inserts the _baseMapLayer below point layers and line layers
        /// </summary>
        private void AddBasemapLayerToMap()
        {
            //check all top-level map point layers, line layers
            int groupOrLayerIndex = FindPointOrLineLayerIndex(App.Map.MapFrame);
            if (groupOrLayerIndex >= 0)
                App.Map.Layers.Insert(groupOrLayerIndex, _baseMapLayer);
            else
                App.Map.Layers.Add(_baseMapLayer);
        }

        private int FindPointOrLineLayerIndex(IMapGroup group)
        {
            int index = -1;

            foreach (IMapLayer layer in group.Layers)
            {
                index++;

                IMapGroup childGroup = layer as IMapGroup;
                if (childGroup != null)
                {
                    int groupIndex = FindPointOrLineLayerIndex(childGroup);
                    if (groupIndex != -1) return index;
                }

                if (layer is IMapPointLayer || layer is IMapLineLayer)
                {
                    return index;
                }
            }
            //if no layer was found, return -1
            return -1;
        }

        private void AddButtons()
        {
            var header = App.HeaderControl;
            AddServiceDropDown(header);
            AddOpaticyDropDown(header);
        }

        private void AddOpaticyDropDown(IHeaderControl header)
        {
            string defaultOpacity = null;
            _opacityDropDown = new DropDownActionItem
                                   {
                                       AllowEditingText = true,
                                       Caption = resources.Opacity_Box_Text,
                                       ToolTipText = resources.Opacity_Box_ToolTip,
                                       Width = 45,
                                       Key = STR_KeyOpacityDropDown
                                   };

            //Make some opacity settings
            for (var i = 100; i > 0; i -= 10)
            {
                string opacity = i.ToString();
                if (i == 100)
                {
                    defaultOpacity = opacity;
                }
                _opacityDropDown.Items.Add(opacity);
            }

            _opacityDropDown.GroupCaption = resources.Panel_Name;
            _opacityDropDown.SelectedValueChanged += OpacitySelected;
            _opacityDropDown.RootKey = HeaderControl.HomeRootItemKey;

            //Add it to the Header
            header.Add(_opacityDropDown);
            if (defaultOpacity != null)
                _opacityDropDown.SelectedItem = defaultOpacity;
        }

        private void AddServiceDropDown(IHeaderControl header)
        {
            _serviceDropDown = new DropDownActionItem();
            _serviceDropDown.Key = STR_KeyServiceDropDown;

            //Create "None" Option
            _emptyProvider = new ServiceProvider(resources.None, null);
            _serviceDropDown.Items.Add(_emptyProvider);

            // no option presently for group image.
            // Image = resources.AddOnlineBasemap.GetThumbnailImage(32, 32, null, IntPtr.Zero),

            _serviceDropDown.Width = 145;
            _serviceDropDown.AllowEditingText = false;
            _serviceDropDown.ToolTipText = resources.Service_Box_ToolTip;
            _serviceDropDown.SelectedValueChanged += ServiceSelected;
            _serviceDropDown.GroupCaption = resources.Panel_Name;
            _serviceDropDown.Items.AddRange(ServiceProvider.GetDefaultServiceProviders());
            _serviceDropDown.RootKey = HeaderControl.HomeRootItemKey;

            //Create "Other" Option
            var otherProvider = new ServiceProvider(Other, null);
            _serviceDropDown.Items.Add(otherProvider);

            //Add it to the Header
            header.Add(_serviceDropDown);

            _serviceDropDown.SelectedItem = _emptyProvider;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseMapLayerRemoveItem(object sender, EventArgs e)
        {
            _baseMapLayer = null;
            _serviceDropDown.SelectedItem = _emptyProvider;
            App.Map.MapFrame.ViewExtentsChanged -= MapFrameExtentsChanged;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BwDoWork(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;

            if (worker != null && _baseMapLayer != null)
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                }
                else
                {
                    worker.ReportProgress(50);
                    UpdateStichedBasemap();
                }
        }

        private void BwProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // Do we know what what our progress completion percent is (instead of 50)?
            // TODO: make this localizable
            App.ProgressHandler.Progress("Loading Basemap ...", 50, "Loading Basemap ...");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BwRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var map = App.Map as Map;
            if (map != null) map.MapFrame.Invalidate();
            App.ProgressHandler.Progress(String.Empty, 0, String.Empty);
        }

        /// <summary>
        /// Changes the opacity of the current basemap image and invalidates the map
        /// </summary>
        /// <param name="opacity">The opacity as a value between 0 and 100, inclusive.</param>
        private void ChangeBasemapOpacity(short opacity)
        {
            MapFrameExtentsChanged(this, new ExtentArgs(App.Map.ViewExtents)); //this forces the map layer to refresh

            //using (Bitmap newBasemapImage = GetTransparentBasemapImage(_baseMapLayer.Image.GetBitmap(), opacity))
            //{
            //    _baseMapLayer.Image.SetBitmap(newBasemapImage);
            //}

            ////Calling Refresh() or Invalidate() or ResetBuffer() completely cleared the map and did not redraw
            ////the basemap layer image.
            ////Calling ResetExtents() refreshes the map because it triggers MapFrame.ExtentsChanged() and
            ////re-gets the basemap layer from the web.
            ////TODO: find a faster method here
            //App.Map.MapFrame.ResetExtents();
        }

        /// <summary>
        ///
        /// </summary>
        private void DisableBasemapLayer()
        {
            RemoveBasemapLayer(_baseMapLayer);
            RemoveBasemapLayer(_featureSetLayer);

            _baseMapLayer = null;
            _basemapImage = null;
            _featureSetLayer = null;

            App.Map.MapFrame.ViewExtentsChanged -= MapFrameExtentsChanged;
        }

        private void ForceMaxExtentZoom()
        {
            ProjectionInfo webMerc = KnownCoordinateSystems.Projected.World.WebMercator;

            //special case when there are no other layers in the map. Set map projection to WebMercator and zoom to max ext.
            MapFrameProjectionHelper.ReprojectMapFrame(App.Map.MapFrame, webMerc.ToEsriString());

            // modifying the view extents didn't get the job done, so we are creating a new featureset.
            // App.Map.ViewExtents = new Extent(TileCalculator.MinWebMercX, TileCalculator.MinWebMercY, TileCalculator.MaxWebMercX, TileCalculator.MaxWebMercY);
            var fs = new FeatureSet(FeatureType.Point);
            fs.Features.Add(new Coordinate(TileCalculator.MinWebMercX, TileCalculator.MinWebMercY));
            fs.Features.Add(new Coordinate(TileCalculator.MaxWebMercX, TileCalculator.MaxWebMercY));

            fs.Projection = App.Map.Projection;
            _featureSetLayer = App.Map.Layers.Add(fs);

            // hide the points that we are adding.
            _featureSetLayer.LegendItemVisible = false;

            App.Map.ZoomToMaxExtent();
        }

        private void EnableBasemapFetching(string tileServerName, string tileServerUrl)
        {
            ProjectionInfo webMerc = KnownCoordinateSystems.Projected.World.WebMercator;

            // Zoom out as much as possible if there are no other layers.
            //reproject any existing layer in non-webMercator projection.
            //if (App.Map.Layers.Count == 0 || App.Map.Projection != webMerc)
            if (App.Map.Layers.Count == 0)
            {
                ForceMaxExtentZoom();
            }
            else if (!App.Map.Projection.Equals(webMerc))
            {
                //get original view extents
                App.ProgressHandler.Progress(String.Empty, 0, "Reprojecting Map Layers...");
                double[] viewExtentXY = new[] { App.Map.ViewExtents.MinX, App.Map.ViewExtents.MinY, App.Map.ViewExtents.MaxX, App.Map.ViewExtents.MaxY };
                double[] viewExtentZ = new[] { 0.0, 0.0 };

                //reproject view extents
                Reproject.ReprojectPoints(viewExtentXY, viewExtentZ, App.Map.Projection, webMerc, 0, 2);

                //set the new map extents
                App.Map.ViewExtents = new Extent(viewExtentXY);

                //if projection is not WebMercator - reproject all layers:
                MapFrameProjectionHelper.ReprojectMapFrame(App.Map.MapFrame, webMerc.ToEsriString());

                App.ProgressHandler.Progress(String.Empty, 0, "Loading Basemap...");
            }

            // Special case for WMS
            if (tileServerName.Equals(Properties.Resources.WMSMap, StringComparison.InvariantCultureIgnoreCase))
            {
                using (var wmsDialog = new WMSServerParameters())
                {
                    if (wmsDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    _tileManager.WmsServerInfo = wmsDialog.WmsServerInfo;
                }
            }

            // Other is a custom service
            if (tileServerName.Equals(Other, StringComparison.InvariantCultureIgnoreCase))
            {
                tileServerUrl = Interaction.InputBox("Please provide the Url for the service.",
                                                     DefaultResponse: "http://tiles.virtualearth.net/tiles/h{key}.jpeg?g=461&mkt=en-us&n=z");

                // Let the user cancel...
                if (String.IsNullOrWhiteSpace(tileServerUrl))
                    return;
            }

            EnableBasemapLayer();

            _tileManager.ChangeService(tileServerName, tileServerUrl);

            if (_bw.IsBusy != true)
            {
                _bw.RunWorkerAsync();
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void EnableBasemapLayer()
        {
            if (_baseMapLayer == null)
            {
                //Need to first initialize and add the basemap layer synchronously (it will fail if done in
                // another thread.

                //First create a temporary imageData with an Envelope (otherwise adding to the map will fail)
                var tempImageData = new InRamImageData(resources.NoDataTile, new Extent(1, 1, 2, 2));

                _baseMapLayer = new MapImageLayer(tempImageData)
                                    {
                                        Projection = App.Map.Projection,
                                        LegendText = resources.Legend_Title
                                    };

                _baseMapLayer.RemoveItem += BaseMapLayerRemoveItem;

                AddBasemapLayerToMap();
            }

            App.Map.MapFrame.ViewExtentsChanged += MapFrameExtentsChanged;
        }

        private void EnsureMapProjectionIsWebMercator()
        {
            if (App.Map.Projection != KnownCoordinateSystems.Projected.World.WebMercator)
            {
                App.Map.Projection = KnownCoordinateSystems.Projected.World.WebMercator;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapFrameExtentsChanged(object sender, ExtentArgs e)
        {
            if (_bw.IsBusy != true)
            {
                _bw.RunWorkerAsync();
            }
        }

        private void OpacitySelected(object sender, SelectedValueChangedEventArgs e)
        {
            if (_baseMapLayer == null)
                return;

            Int16 opacityInt;

            //Check to make sure the text in the box is an integer and we are in the range
            if (!Int16.TryParse(e.SelectedItem as string, out opacityInt) || opacityInt > 100 || opacityInt < 0)
            {
                opacityInt = 100;
                _opacityDropDown.SelectedItem = opacityInt;
            }
            _opacity = opacityInt;
            ChangeBasemapOpacity(opacityInt);
        }

        /// <summary>
        /// Finds and removes the online basemap layer
        /// </summary>
        /// <param name="Layer"></param>
        private void RemoveBasemapLayer(IMapLayer Layer)
        {
            //attempt to remove from the top-level
            if (App.Map.Layers.Remove(Layer)) return;

            //Remove from other groups if the user has moved it
            App.Map.Layers.OfType<IMapGroup>().Any(grp => grp.Remove(Layer));
        }

        private void SerializationManagerDeserializing(object sender, SerializingEventArgs e)
        {
            var opacity = App.SerializationManager.GetCustomSetting(PluginName + "_Opacity", "100");
            var basemapName = App.SerializationManager.GetCustomSetting(PluginName + "_BasemapName", resources.None);
            //Set opacity
            _opacityDropDown.SelectedItem = opacity;
            _opacity = Convert.ToInt16(opacity);

            _baseMapLayer = (MapImageLayer)App.Map.MapFrame.GetAllLayers().FirstOrDefault(layer => layer.LegendText == resources.Legend_Title);

            if (basemapName == "None")
            {
                if (_baseMapLayer != null)
                {
                    DisableBasemapLayer();
                    _provider = _emptyProvider;
                    _serviceDropDown.SelectedItem = _provider;
                }
            }
            else
            {
                //hack: need to set provider to original object, not a new one.
                _provider = ServiceProvider.GetDefaultServiceProviders().FirstOrDefault(p => p.Name == basemapName);
                _serviceDropDown.SelectedItem = _provider;
                EnableBasemapFetching(_provider.Name, _provider.Url);
            }
        }

        private void SerializationManagerSerializing(object sender, SerializingEventArgs e)
        {
            App.SerializationManager.SetCustomSetting(PluginName + "_BasemapName", _provider.Name);
            App.SerializationManager.SetCustomSetting(PluginName + "_Opacity", _opacity.ToString());
        }

        private void SerializationManagerNewProject(object sender, SerializingEventArgs e)
        {
            //deactivate the web map
            if (_baseMapLayer != null)
            {
                DisableBasemapLayer();
                _provider = _emptyProvider;
                _serviceDropDown.SelectedItem = _provider;
            }
        }

        private void ServiceSelected(object sender, SelectedValueChangedEventArgs e)
        {
            _provider = e.SelectedItem as ServiceProvider;

            if (_provider.Name == resources.None)
                DisableBasemapLayer();
            else
                EnableBasemapFetching(_provider.Name, _provider.Url);
        }

        /// <summary>
        /// Main method of this plugin: gets the tiles from the TileManager, stitches them together, and adds the layer to the map.
        /// </summary>
        private void UpdateStichedBasemap()
        {
            var map = App.Map as Map;
            if (map != null)
            {
                var rectangle = map.Bounds;
                var webMercExtent = map.ViewExtents;

                //Clip the reported Web Merc Envelope to be within possible Web Merc extents
                //  This fixes an issue with Reproject returning bad results for very large (impossible) web merc extents reported from the Map
                var webMercTopLeftX = TileCalculator.Clip(webMercExtent.MinX, TileCalculator.MinWebMercX, TileCalculator.MaxWebMercX);
                var webMercTopLeftY = TileCalculator.Clip(webMercExtent.MaxY, TileCalculator.MinWebMercY, TileCalculator.MaxWebMercY);
                var webMercBtmRightX = TileCalculator.Clip(webMercExtent.MaxX, TileCalculator.MinWebMercX, TileCalculator.MaxWebMercX);
                var webMercBtmRightY = TileCalculator.Clip(webMercExtent.MinY, TileCalculator.MinWebMercY, TileCalculator.MaxWebMercY);

                //Get the web mercator vertices of the current map view
                var mapVertices = new[] { webMercTopLeftX, webMercTopLeftY, webMercBtmRightX, webMercBtmRightY };

                double[] z = { 0, 0 };

                //Reproject from web mercator to WGS1984 geographic
                Reproject.ReprojectPoints(mapVertices, z, WebMercProj, Wgs84Proj, 0, mapVertices.Length / 2);
                var geogEnv = new Envelope(mapVertices[0], mapVertices[2], mapVertices[1], mapVertices[3]);

                //Grab the tiles
                var tiles = _tileManager.GetTiles(geogEnv, rectangle);

                //Stitch them into a single image
                var stitchedBasemap = TileCalculator.StitchTiles(tiles);

                _basemapImage = stitchedBasemap;

                stitchedBasemap = GetTransparentBasemapImage(stitchedBasemap, _opacity);

                var tileImage = new InRamImageData(stitchedBasemap);

                //Tiles will have often slightly different bounds from what we are displaying on screen
                // so we need to get the top left and bottom right tiles' bounds to get the proper extent
                // of the tiled image
                var topLeftTile = tiles[0, 0];
                var bottomRightTile = tiles[tiles.GetLength(0) - 1, tiles.GetLength(1) - 1];

                var tileVertices = new[]
                                       {
                                           topLeftTile.Envelope.TopLeft().X, topLeftTile.Envelope.TopLeft().Y,
                                           bottomRightTile.Envelope.BottomRight().X,
                                           bottomRightTile.Envelope.BottomRight().Y
                                       };

                //Reproject from WGS1984 geographic coordinates to web mercator so we can show on the map
                Reproject.ReprojectPoints(tileVertices, z, Wgs84Proj, WebMercProj, 0, tileVertices.Length / 2);

                tileImage.Bounds = new RasterBounds(stitchedBasemap.Height, stitchedBasemap.Width,
                                                    new Extent(tileVertices[0], tileVertices[3], tileVertices[2], tileVertices[1]));

                _baseMapLayer.Image = tileImage;
            }
        }

        #endregion
    }
}