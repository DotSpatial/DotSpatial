using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using DotSpatial.Data;
using DotSpatial.Data.Forms;
using DotSpatial.Projections;
using DotSpatial.Serialization;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using Point = System.Drawing.Point;
using SelectionMode = DotSpatial.Symbology.SelectionMode;

namespace DotSpatial.Controls
{
    /// <summary>
    /// The Map Control for 2D applications.
    /// </summary>
    public class MapCore
    {
        #region Constructors

        public Size oldSize;
        private IMap _map;

        public MapCore(IMap map)
        {
            _map = map;
        }

        public void Configure()
        {
            _map.MapFrame = new MapFrame(_map, new Extent(0, 0, 0, 0));

            //_resizeEndTimer = new Timer {Interval = 100};
            //_resizeEndTimer.Tick += _resizeEndTimer_Tick;

            IMapFunction info = new MapFunctionIdentify(_map);
            IMapFunction pan = new MapFunctionPan(_map);
            IMapFunction label = new MapFunctionLabelSelect(_map);
            IMapFunction select = new MapFunctionSelect(_map);
            IMapFunction zoomIn = new MapFunctionClickZoom(_map);
            IMapFunction zoomOut = new MapFunctionZoomOut(_map);
            _map.MapFunctions = new List<IMapFunction>
                               {
                                   new MapFunctionKeyNavigation(_map),
                                   pan,
                                   select,
                                   zoomIn,
                                   zoomOut,
                                   label,
                                   info,
                               };
            _map.FunctionLookup = new Dictionary<FunctionMode, IMapFunction>
                                  {
                                      {FunctionMode.Pan, pan},
                                      {FunctionMode.Info, info},
                                      {FunctionMode.Label, label},
                                      {FunctionMode.Select, select},
                                      {FunctionMode.ZoomIn, zoomIn},
                                      {FunctionMode.ZoomOut, zoomOut}
                                  };

            _map.CollisionDetection = false;

            IMapFunction KeyNavigation = _map.MapFunctions.Find(f => f.GetType() == typeof(MapFunctionKeyNavigation));
            _map.ActivateMapFunction(KeyNavigation);
            //changed by Jiri Kadlec - default function mode is none
            _map.FunctionMode = FunctionMode.None;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Removes any members from existing in the selected state
        /// </summary>
        public bool ClearSelection(out IEnvelope affectedArea)
        {
            affectedArea = new Envelope();
            if (_map.MapFrame == null) return false;
            return _map.MapFrame.ClearSelection(out affectedArea);
        }

        /// <summary>
        /// Adds any members found in the specified region to the selected state as long as SelectionEnabled is set to true.
        /// </summary>
        /// <param name="tolerant">The geographic region where selection occurs that is tolerant for point or linestrings.</param>
        /// <param name="strict">The tight envelope to use for polygons.</param>
        /// <param name="mode">The selection mode.</param>
        /// <param name="affectedArea">The envelope affected area.</param>
        /// <returns>Boolean, true if any members were added to the selection.</returns>
        public bool Select(IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea)
        {
            affectedArea = new Envelope();
            if (_map.MapFrame == null) return false;
            return _map.MapFrame.Select(tolerant, strict, mode, out affectedArea);
        }

        /// <summary>
        /// Inverts the selected state of any members in the specified region.
        /// </summary>
        /// <param name="tolerant">The geographic region where selection occurs that is tolerant for point or linestrings.</param>
        /// <param name="strict">The tight envelope to use for polygons.</param>
        /// <param name="mode">The selection mode determining how to test for intersection.</param>
        /// <param name="affectedArea">The geographic region encapsulating the changed members.</param>
        /// <returns>boolean, true if members were changed by the selection process.</returns>
        public bool InvertSelection(IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea)
        {
            affectedArea = new Envelope();
            if (_map.MapFrame == null) return false;
            return _map.MapFrame.InvertSelection(tolerant, strict, mode, out affectedArea);
        }

        /// <summary>
        /// Adds any members found in the specified region to the selected state as long as SelectionEnabled is set to true.
        /// </summary>
        /// <param name="tolerant">The geographic region where selection occurs that is tolerant for point or linestrings.</param>
        /// <param name="strict">The tight envelope to use for polygons.</param>
        /// <param name="mode">The selection mode.</param>
        /// <param name="affectedArea">The envelope affected area.</param>
        /// <returns>Boolean, true if any members were added to the selection.</returns>
        public bool UnSelect(IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea)
        {
            affectedArea = new Envelope();
            if (_map.MapFrame == null) return false;
            return _map.MapFrame.UnSelect(tolerant, strict, mode, out affectedArea);
        }

        /// <summary>
        /// Allows the user to add a new layer to the map using an open file dialog to choose a layer file.
        /// Multi-select is an option, so this return a list with all the layers.
        /// </summary>
        public List<IMapLayer> AddLayers()
        {
            var results = new List<IMapLayer>();
            foreach (var set in DataManager.DefaultDataManager.OpenFiles())
            {
                var fs = set as IFeatureSet;
                if (fs != null)
                {
                    results.Add(_map.Layers.Add(fs));
                    continue;
                }
                var id = set as IImageData;
                if (id != null)
                {
                    results.Add(_map.Layers.Add(id));
                    continue;
                }
                var r = set as IRaster;
                if (r != null)
                {
                    results.Add(_map.Layers.Add(r));
                    continue;
                }
            }
            return results;
        }

        /// <summary>
        /// Adds the fileName as a new layer to the map, returning the new layer.
        /// </summary>
        /// <param name="fileName">The string fileName of the layer to add</param>
        /// <returns>The IMapLayer added to the file.</returns>
        public IMapLayer AddLayer(string fileName)
        {
            return _map.Layers.Add(fileName);
        }

        /// <summary>
        /// Uses the file dialog to allow selection of a fileName for opening the
        /// new layer, but does not allow multiple files to be added at once.
        /// </summary>
        /// <returns>The newly opened IMapLayer</returns>
        public IMapLayer AddLayer()
        {
            if (DataManager.DefaultDataManager.ProgressHandler == null)
            {
                if (_map.ProgressHandler != null)
                {
                    DataManager.DefaultDataManager.ProgressHandler = _map.ProgressHandler;
                }
            }
            try
            {
                return _map.Layers.Add(DataManager.DefaultDataManager.OpenFile());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Allows a multi-select file dialog to add raster layers, applying a
        /// filter so that only supported raster formats will appear.
        /// </summary>
        /// <returns>A list of the IMapRasterLayers that were opened.</returns>
        public List<IMapRasterLayer> AddRasterLayers()
        {
            var sets = DataManager.DefaultDataManager.OpenRasters();
            return sets.Select(raster => _map.Layers.Add(raster)).ToList();
        }

        /// <summary>
        /// Allows an open file dialog without multi-select enabled to add a single
        /// raster to the map as a layer, and returns the added layer.
        /// </summary>
        /// <returns>The IMapRasterLayer that was added, or null.</returns>
        public IMapRasterLayer AddRasterLayer()
        {
            var raster = DataManager.DefaultDataManager.OpenRaster();
            return raster == null ? null : _map.Layers.Add(raster);
        }

        /// <summary>
        /// Allows a mult-select open file dialog to specify several fileNames to add.
        /// Only files with supported vector extensions will be shown.
        /// </summary>
        /// <returns>The list of added MapFeatureLayers</returns>
        public List<IMapFeatureLayer> AddFeatureLayers()
        {
            var sets = DataManager.DefaultDataManager.OpenVectors();
            return sets.Select(featureSet => _map.Layers.Add(featureSet)).ToList();
        }

        /// <summary>
        /// Allows an open file dialog without multi-select enabled to add a single
        /// raster tot he map as a layer, and returns the added layer.
        /// </summary>
        /// <returns>The IMapFeatureLayer that was added, or null.</returns>
        public IMapFeatureLayer AddFeatureLayer()
        {
            var vector = DataManager.DefaultDataManager.OpenVector();
            return vector == null? null: _map.Layers.Add(vector);
        }

        /// <summary>
        /// Allows a mult-select open file dialog to specify several fileNames to add.
        /// Only files with supported image extensions will be shown.
        /// </summary>
        /// <returns>The list of added MapImageLayers</returns>
        public List<IMapImageLayer> AddImageLayers()
        {
            var sets = DataManager.DefaultDataManager.OpenImages();
            return sets.Select(imageData => _map.Layers.Add(imageData)).ToList();
        }

        /// <summary>
        /// Allows an open dialog without multi-select to specify a single fileName
        /// to be added to the map as a new layer and returns the newly added layer.
        /// </summary>
        /// <returns>The layer that was added to the map, or null.</returns>
        public IMapImageLayer AddImageLayer()
        {
            var image = DataManager.DefaultDataManager.OpenImage();
            return image == null? null : _map.Layers.Add(image);
        }

        /// <summary>
        /// This can be called any time, and is currently being used to capture
        /// the end of a resize event when the actual data should be updated.
        /// </summary>
        public void ResetBuffer()
        {
            if (_map.MapFrame != null)
            {
                _map.MapFrame.ResetBuffer();
            }
        }

        /// <summary>
        /// Saves the dataset belonging to the layer.
        /// </summary>
        public void SaveLayer()
        {
            var sfd = new SaveFileDialog();
            var layer = _map.MapFrame.Layers[0];
            var mfl = layer as IMapFeatureLayer;
            if (mfl != null)
            {
                sfd.Filter = DataManager.DefaultDataManager.VectorWriteFilter;
                if (sfd.ShowDialog() != DialogResult.OK) { return; }
                mfl.DataSet.SaveAs(sfd.FileName, true);
                return;
            }

            var mrl = layer as IMapRasterLayer;
            if (mrl != null)
            {
                sfd.Filter = DataManager.DefaultDataManager.RasterWriteFilter;
                if (sfd.ShowDialog() != DialogResult.OK) { return; }
                mrl.DataSet.SaveAs(sfd.FileName);
                return;
            }

            var mil = layer as IMapImageLayer;
            if (mil != null)
            {
                sfd.Filter = DataManager.DefaultDataManager.ImageWriteFilter;
                if (sfd.ShowDialog() != DialogResult.OK) { return; }
                mil.DataSet.SaveAs(sfd.FileName);
                return;
            }
            throw new ArgumentException("The layer chosen did not have a raster, vector or image dataset to save.");
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the ViewExtentsChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnViewExtentsChanged()
        {
            var maxExtent = _map.GetMaxExtent();
            if (maxExtent.Width != 0 && maxExtent.Height != 0)
            {
                if ((_map.ViewExtents.Width > (maxExtent.Width + 1)) && (_map.ViewExtents.Height > (maxExtent.Height + 1)))
                {
                    _map.ZoomToMaxExtent();
                }
            }
        }

        public void OnSizeChanged()
        {
            if (_map.MapFrame != null)
            {
                var diff = new Point { X = _map.Width - oldSize.Width, Y = _map.Height - oldSize.Height };
                var newView = new Rectangle(_map.MapFrame.View.X,
                    _map.MapFrame.View.Y,
                    _map.MapFrame.View.Width + diff.X,
                    _map.MapFrame.View.Height + diff.Y);
                // Check for minimal size of view.
                if (newView.Width < 5) newView.Width = 5;
                if (newView.Height < 5) newView.Height = 5;

                _map.MapFrame.View = newView;
                _map.MapFrame.ResetExtents();
                _map.Invalidate();
            }
            oldSize = new Size(_map.Width, _map.Height);
        }

        public void Draw(Graphics g, PaintEventArgs e)
        {
            _map.MapFrame.Draw(new PaintEventArgs(g, e.ClipRectangle));
        }

        public void MapFrameItemChanged(object sender, EventArgs e)
        {
            _map.Invalidate();
        }

        public void MapFrameUpdateMap(object sender, EventArgs e)
        {
            _map.Invalidate();
        }

        public void MapFrameScreenUpdated(object sender, EventArgs e)
        {
            _map.Invalidate();
        }

        #endregion
    }
}