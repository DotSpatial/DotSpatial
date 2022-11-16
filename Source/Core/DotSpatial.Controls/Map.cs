// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Data.Forms;
using DotSpatial.Projections;
using DotSpatial.Serialization;
using DotSpatial.Symbology;
using NetTopologySuite.Geometries;
using Point = System.Drawing.Point;
using SelectionMode = DotSpatial.Symbology.SelectionMode;

namespace DotSpatial.Controls
{
    /// <summary>
    /// The Map Control for 2D applications.
    /// </summary>
    public partial class Map : UserControl, IMap, IMessageFilter
    {
        #region Fields

        private Dictionary<FunctionMode, IMapFunction> _functionLookup;
        private FunctionMode _functionMode;
        private IMapFrame _geoMapFrame;
        private int _isBusyIndex;

        /// <summary>
        /// This is used to remember the last minimal extent that was set by OnViewExtentsChanged.
        /// It's used to stop a loop that starts if MapFrame.ResetAspectRatio makes the minExt smaller than 1e-7.
        /// </summary>
        private Extent _lastMinExtent;

        private ILegend _legend;
        private Size _oldSize;
        private IProgressHandler _progressHandler;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Map"/> class that can be dropped on a form.
        /// </summary>
        public Map()
        {
            InitializeComponent();
            Configure();
            Application.AddMessageFilter(this);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs after the map refreshes the image
        /// </summary>
        public event EventHandler FinishedRefresh;

        /// <summary>
        /// Occurs when the map function mode has changed
        /// </summary>
        /// <remarks>Example of changing the function mode
        /// is changing from zoom mode to select mode.</remarks>
        public event EventHandler FunctionModeChanged;

        /// <summary>
        /// Public event advertising the mouse movement
        /// </summary>
        public event EventHandler<GeoMouseArgs> GeoMouseMove;

        /// <summary>
        /// Occurs after a layer has been added to the mapframe, or any of the child groups of that mapframe.
        /// </summary>
        public event EventHandler<LayerEventArgs> LayerAdded;

        /// <summary>
        /// Occurs after the projection of the map has been changed
        /// </summary>
        public event EventHandler ProjectionChanged;

        /// <summary>
        /// Occurs after a resize event
        /// </summary>
        public event EventHandler Resized;

        /// <summary>
        /// Occurs after the selection has changed for all the layers
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Fires after the view extents have been altered and the map has redrawn to the new extents.
        /// This is an echo of the MapFrame.ViewExtentsChanged, so you only want one handler.
        /// </summary>
        public event EventHandler<ExtentArgs> ViewExtentsChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the back buffer. The back buffer should be in Format32bbpArgb bitmap.
        /// If it is not, then the image on the back buffer will be copied from the supplied image.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image BufferedImage
        {
            get
            {
                return _geoMapFrame.BufferImage;
            }

            set
            {
                _geoMapFrame.BufferImage = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether point layers in the map will only draw points that are
        /// more than 50% revealed. This should increase drawing speed for layers that have a large number of points.
        /// </summary>
        public bool CollisionDetection { get; set; }

        /// <summary>
        /// Gets or sets the Cursor.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Cursor Cursor
        {
            get
            {
                return base.Cursor;
            }

            set
            {
                base.Cursor = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the drawing layers should cache off-screen data to
        /// the buffer. Panning will be much more elegant, but zooming, selecting and resizing will take a performance penalty.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets a boolean that indicates whether or not zooming will also retrieve content immediately around the map so that panning pulls new content onto the screen.")]
        public bool ExtendBuffer
        {
            get
            {
                return MapFrame.ExtendBuffer;
            }

            set
            {
                MapFrame.ExtendBuffer = value;
            }
        }

        /// <summary>
        /// Gets the geographic bounds of all of the different data layers currently visible on the map.
        /// </summary>
        [Category("Bounds")]
        [Description("Gets the geographic bounds of all of the different data layers currently visible on the map.")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Extent Extent => _geoMapFrame.Extent;

        /// <summary>
        /// Gets or sets the current tool mode. This rapidly enables or disables specific tools to give
        /// a combination of functionality. Selecting None will disable all the tools, which can be
        /// enabled manually by enabling the specific tool in the GeoTools dictionary.
        /// </summary>
        public FunctionMode FunctionMode
        {
            get
            {
                return _functionMode;
            }

            set
            {
                _functionMode = value;
                switch (_functionMode)
                {
                    case FunctionMode.ZoomIn:
                        try
                        {
                            MemoryStream ms = new(Images.cursorZoomIn);
                            Cursor = new Cursor(ms);
                        }
                        catch
                        {
                            Cursor = Cursors.Arrow;
                        }

                        break;
                    case FunctionMode.ZoomOut:
                        try
                        {
                            MemoryStream ms = new(Images.cursorZoomOut);
                            Cursor = new Cursor(ms);
                        }
                        catch
                        {
                            Cursor = Cursors.Arrow;
                        }

                        break;
                    case FunctionMode.Info:
                        Cursor = Cursors.Help;
                        break;
                    case FunctionMode.Label:
                        Cursor = Cursors.IBeam;
                        break;
                    case FunctionMode.ZoomPan:
                    case FunctionMode.None:
                        Cursor = Cursors.Arrow;
                        break;
                    case FunctionMode.Pan:
                        try
                        {
                            MemoryStream ms = new(Images.cursorHand);
                            Cursor = new Cursor(ms);
                        }
                        catch
                        {
                            Cursor = Cursors.SizeAll;
                        }

                        break;

                    case FunctionMode.Select:
                        try
                        {
                            MemoryStream ms = new(Images.cursorSelect);
                            Cursor = new Cursor(ms);
                        }
                        catch
                        {
                            Cursor = Cursors.Hand;
                        }

                        break;
                }

                // Turn off functions that are not "Always on"
                if (_functionMode == FunctionMode.None)
                {
                    foreach (var f in MapFunctions)
                    {
                        if ((f.YieldStyle & YieldStyles.AlwaysOn) != YieldStyles.AlwaysOn)
                            f.Deactivate();
                    }
                }
                else
                {
                    if (_functionMode != FunctionMode.ZoomPan)
                    {
                        // Except for function mode "none" allow scrolling
                        IMapFunction scroll = _functionLookup[FunctionMode.ZoomPan];
                        ActivateMapFunction(scroll);
                    }

                    IMapFunction newMode = _functionLookup[_functionMode];
                    ActivateMapFunction(newMode);
                }

                // function mode changed event
                OnFunctionModeChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a map-function is currently interacting with the map.
        /// If this is true, then any tool-tip like popups or other mechanisms that require lots of re-drawing
        /// should suspend themselves to prevent conflict. Setting this actually increments an internal integer,
        /// so when that integer is 0, the map is "Not" busy, but multiple busy processess can work independantly.
        /// </summary>
        public bool IsBusy
        {
            get
            {
                return _isBusyIndex > 0;
            }

            set
            {
                if (value)
                    _isBusyIndex++;
                else
                    _isBusyIndex--;
                if (_isBusyIndex <= 0)
                {
                    _isBusyIndex = 0;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the map is zoomed to its full extents.
        /// Added by Eric Hullinger 1/3/2013.
        /// </summary>
        public bool IsZoomedToMaxExtent { get; set; }

        /// <summary>
        /// Gets the collection of layers.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IMapLayerCollection Layers => _geoMapFrame?.Layers;

        /// <summary>
        /// Gets or sets the legend to use when showing the layers from this map.
        /// </summary>
        public ILegend Legend
        {
            get
            {
                return _legend;
            }

            set
            {
                _legend = value;
                _legend?.AddMapFrame(_geoMapFrame);
            }
        }

        /// <summary>
        /// Gets or sets the MapFrame that should be displayed in this map.
        /// </summary>
        [Serialize("MapFrame")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IMapFrame MapFrame
        {
            get
            {
                return _geoMapFrame;
            }

            set
            {
                var proj = Projection;
                OnExcludeMapFrame(_geoMapFrame);
                _geoMapFrame = value;
                _geoMapFrame.ProgressHandler = ProgressHandler;
                OnIncludeMapFrame(_geoMapFrame);
                if (proj != null)
                    Projection = proj;
                MapFrame.ResetBuffer();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the dictionary of tools built into this project.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<IMapFunction> MapFunctions { get; set; }

        /// <summary>
        /// Gets or sets the progress handler for this component.
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get
            {
                return _progressHandler;
            }

            set
            {
                _progressHandler = value;
                _geoMapFrame.ProgressHandler = value;
            }
        }

        /// <summary>
        /// Gets or sets the projection. This should reflect the projection of the first data layer loaded.
        /// Loading subsequent, but non-matching projections should throw an alert, and allow reprojection.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public ProjectionInfo Projection
        {
            get
            {
                return _geoMapFrame?.Projection;
            }

            set
            {
                if (_geoMapFrame != null)
                {
                    _geoMapFrame.Projection = value;
                    OnProjectionChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the Projection Esri string of the map. This property is used for serializing
        /// the projection string to the project file.
        /// </summary>
        [Serialize("ProjectionEsriString")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public string ProjectionEsriString
        {
            get
            {
                return Projection?.ToEsriString();
            }

            set
            {
                if (Projection != null)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        Projection = null;
                    }
                    else if (Projection.ToEsriString() != value)
                    {
                        Projection = ProjectionInfo.FromEsriString(value);
                        OnProjectionChanged();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a PromptMode enumeration that controls how users are prompted before adding layers
        /// that have a coordinate system that is different from the map.
        /// </summary>
        public ActionMode ProjectionModeDefine
        {
            get
            {
                if (_geoMapFrame != null)
                    return MapFrame.ProjectionModeDefine;
                return ActionMode.Never;
            }

            set
            {
                if (_geoMapFrame != null)
                    _geoMapFrame.ProjectionModeDefine = value;
            }
        }

        /// <summary>
        /// Gets or sets a PromptMode enumeration that controls how users are prompted before adding layers
        /// that have a coordinate system that is different from the map.
        /// </summary>
        public ActionMode ProjectionModeReproject
        {
            get
            {
                if (_geoMapFrame != null)
                    return MapFrame.ProjectionModeReproject;
                return ActionMode.Never;
            }

            set
            {
                if (_geoMapFrame != null)
                    _geoMapFrame.ProjectionModeReproject = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether layers should draw during the actual resize itself. The
        /// normal behavior is to draw the existing image buffer in the new size and position which is much
        /// faster for large datasets, but is not as visually appealing if you only work with small datasets.
        /// </summary>
        public bool RedrawLayersWhileResizing { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the selection is enabled.
        /// </summary>
        public bool SelectionEnabled
        {
            get
            {
                return MapFrame != null && MapFrame.SelectionEnabled;
            }

            set
            {
                if (MapFrame == null)
                    return;
                MapFrame.SelectionEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets the geographic extents to show in the view.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Extent ViewExtents
        {
            get
            {
                return _geoMapFrame.ViewExtents;
            }

            set
            {
                _geoMapFrame.ViewExtents = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether zooming out farther than the extent of the map is allowed.
        /// This is useful if we have only layers with small extents and want to look at them from farther out.
        /// </summary>
        [Serialize("ZoomOutFartherThanMaxExtent")]
        public bool ZoomOutFartherThanMaxExtent { get; set; }

        /// <summary>
        /// Gets the map frame.
        /// </summary>
        IFrame IBasicMap.MapFrame => _geoMapFrame;

        #endregion

        #region Methods

        /// <summary>
        /// If the specified function is already in the list of functions, this will properly test the yield style of various
        /// map functions that are currently on and then activate the function. If this function is not in the list, then
        /// it will add it to the list. If you need to control the position, then insert the function before using this
        /// method to activate. Be warned that calling "Activate" directly on your function will activate your function
        /// but not disable any other functions. You can set "Map.FunctionMode = FunctionModes.None" first, and then
        /// specifically activate the function that you want.
        /// </summary>
        /// <param name="function">The MapFunction to activate, or add.</param>
        public void ActivateMapFunction(IMapFunction function)
        {
            if (!MapFunctions.Contains(function))
            {
                MapFunctions.Add(function);
            }

            foreach (var f in MapFunctions)
            {
                if ((f.YieldStyle & YieldStyles.AlwaysOn) == YieldStyles.AlwaysOn)
                    continue; // ignore "Always On" functions
                int test = (int)(f.YieldStyle & function.YieldStyle);
                if (test > 0)
                    f.Deactivate(); // any overlap of behavior leads to deactivation
            }

            function.Activate();
        }

        /// <summary>
        /// Allows an open file dialog without multi-select enabled to add a single
        /// raster tot he map as a layer, and returns the added layer.
        /// </summary>
        /// <returns>The IMapFeatureLayer that was added, or null.</returns>
        public virtual IMapFeatureLayer AddFeatureLayer()
        {
            var vector = DataManager.DefaultDataManager.OpenVector();
            return vector == null ? null : Layers.Add(vector);
        }

        /// <summary>
        /// Allows a mult-select open file dialog to specify several fileNames to add.
        /// Only files with supported vector extensions will be shown.
        /// </summary>
        /// <returns>The list of added MapFeatureLayers.</returns>
        public virtual List<IMapFeatureLayer> AddFeatureLayers()
        {
            var sets = DataManager.DefaultDataManager.OpenVectors();
            return sets.Select(featureSet => Layers.Add(featureSet)).ToList();
        }

        /// <summary>
        /// Allows an open dialog without multi-select to specify a single fileName
        /// to be added to the map as a new layer and returns the newly added layer.
        /// </summary>
        /// <returns>The layer that was added to the map, or null.</returns>
        public virtual IMapImageLayer AddImageLayer()
        {
            var image = DataManager.DefaultDataManager.OpenImage();
            return image == null ? null : Layers.Add(image);
        }

        /// <summary>
        /// Allows a mult-select open file dialog to specify several fileNames to add.
        /// Only files with supported image extensions will be shown.
        /// </summary>
        /// <returns>The list of added MapImageLayers.</returns>
        public virtual List<IMapImageLayer> AddImageLayers()
        {
            var sets = DataManager.DefaultDataManager.OpenImages();
            return sets.Select(imageData => Layers.Add(imageData)).ToList();
        }

        /// <summary>
        /// Adds the fileName as a new layer to the map, returning the new layer.
        /// </summary>
        /// <param name="fileName">The string fileName of the layer to add.</param>
        /// <returns>The IMapLayer added to the file.</returns>
        public virtual IMapLayer AddLayer(string fileName)
        {
            return Layers.Add(fileName);
        }

        /// <summary>
        /// Uses the file dialog to allow selection of a fileName for opening the
        /// new layer, but does not allow multiple files to be added at once.
        /// </summary>
        /// <returns>The newly opened IMapLayer.</returns>
        public virtual IMapLayer AddLayer()
        {
            if (DataManager.DefaultDataManager.ProgressHandler == null && ProgressHandler != null)
            {
                DataManager.DefaultDataManager.ProgressHandler = ProgressHandler;
            }

            try
            {
                return Layers.Add(DataManager.DefaultDataManager.OpenFile());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return null;
        }

        /// <summary>
        /// Allows the user to add a new layer to the map using an open file dialog to choose a layer file.
        /// Multi-select is an option, so this return a list with all the layers.
        /// </summary>
        /// <returns>Layers that were added.</returns>
        public virtual List<IMapLayer> AddLayers()
        {
            var results = new List<IMapLayer>();
            foreach (var set in DataManager.DefaultDataManager.OpenFiles())
            {
                if (set is IFeatureSet fs)
                {
                    results.Add(Layers.Add(fs));
                    continue;
                }

                if (set is IImageData id)
                {
                    results.Add(Layers.Add(id));
                    continue;
                }

                if (set is IRaster r)
                {
                    results.Add(Layers.Add(r));
                }

                if (set is ISelfLoadSet ss)
                {
                    results.Add(Layers.Add(ss));
                }
            }

            return results;
        }

        /// <summary>
        /// Allows an open file dialog without multi-select enabled to add a single
        /// raster to the map as a layer, and returns the added layer.
        /// </summary>
        /// <returns>The IMapRasterLayer that was added, or null.</returns>
        public virtual IMapRasterLayer AddRasterLayer()
        {
            var raster = DataManager.DefaultDataManager.OpenRaster();
            return raster == null ? null : Layers.Add(raster);
        }

        /// <summary>
        /// Allows a multi-select file dialog to add raster layers, applying a
        /// filter so that only supported raster formats will appear.
        /// </summary>
        /// <returns>A list of the IMapRasterLayers that were opened.</returns>
        public virtual List<IMapRasterLayer> AddRasterLayers()
        {
            var sets = DataManager.DefaultDataManager.OpenRasters();
            return sets.Select(raster => Layers.Add(raster)).ToList();
        }

        /// <summary>
        /// Instructs the map to clear the layers.
        /// </summary>
        public void ClearLayers()
        {
            MapFrame?.Layers.Clear();
        }

        /// <summary>
        /// Removes any members from existing in the selected state.
        /// </summary>
        /// <param name="affectedArea">The area affected by the clear operation.</param>
        /// <param name="force">Indicates whether the selection should be cleared although SelectionEnabled is false.</param>
        /// <returns>True if members were removed from the selection.</returns>
        public bool ClearSelection(out Envelope affectedArea, bool force)
        {
            affectedArea = new Envelope();
            if (MapFrame == null)
                return false;
            return MapFrame.ClearSelection(out affectedArea, force);
        }

        /// <summary>
        /// Gets all map groups in the map including the nested groups.
        /// </summary>
        /// <returns>the list of the groups.</returns>
        public List<IGroup> GetAllGroups()
        {
            return _geoMapFrame?.GetAllGroups();
        }

        /// <summary>
        /// Gets all layers of the map including layers which are nested
        /// within groups. The group objects themselves are not included in this list,
        /// but all FeatureLayers, RasterLayers, ImageLayers and other layers are included.
        /// </summary>
        /// <returns>The list of the layers.</returns>
        public List<ILayer> GetAllLayers()
        {
            return _geoMapFrame?.GetAllLayers();
        }

        /// <summary>
        /// Gets a list of just the feature layers regardless of whether they are lines, points, or polygons.
        /// </summary>
        /// <returns>An array of IMapFeatureLayers.</returns>
        public IMapFeatureLayer[] GetFeatureLayers()
        {
            return MapFrame.Layers.OfType<IMapFeatureLayer>().ToArray();
        }

        /// <summary>
        /// Gets the subset of layers that are specifically raster layers, allowing
        /// you to control their symbology.
        /// </summary>
        /// <returns>The image layers.</returns>
        public IMapImageLayer[] GetImageLayers()
        {
            return MapFrame.Layers.OfType<IMapImageLayer>().ToArray();
        }

        /// <summary>
        /// Returns a functional list of the ILayer members. This list will be
        /// separate from the actual list stored, but contains a shallow copy
        /// of the members, so the layers themselves can be accessed directly.
        /// </summary>
        /// <returns>All layers of the map.</returns>
        public List<ILayer> GetLayers()
        {
            return _geoMapFrame?.Layers.Cast<ILayer>().ToList() ?? Enumerable.Empty<ILayer>().ToList();
        }

        /// <summary>
        /// Gets a list of just the line layers (and not the general layers).
        /// </summary>
        /// <returns>The line layers.</returns>
        public IMapLineLayer[] GetLineLayers()
        {
            return MapFrame.Layers.OfType<IMapLineLayer>().ToArray();
        }

        /// <summary>
        /// Gets the MapFunction based on the string name.
        /// </summary>
        /// <param name="name">The string name to find.</param>
        /// <returns>The MapFunction with the specified name.</returns>
        public IMapFunction GetMapFunction(string name)
        {
            return MapFunctions.First(f => f.Name == name);
        }

        /// <summary>
        /// Gets the MaxExtent Window of the current Map.
        /// </summary>
        /// <param name="expand">Indicates whether the extent should be expanded by 10% to satisfy issue 84 (Expand target envelope by 10%). </param>
        /// <returns>The maximal extent of the map.</returns>
        public Extent GetMaxExtent(bool expand = false)
        {
            // to prevent exception when zoom to map with one layer with one point
            const double Eps = 1e-7;
            var maxExtent = Extent.Width < Eps || Extent.Height < Eps ? new Extent(Extent.MinX - Eps, Extent.MinY - Eps, Extent.MaxX + Eps, Extent.MaxY + Eps) : Extent;

            if (ExtendBuffer)
                maxExtent.ExpandBy(maxExtent.Width, maxExtent.Height);

            if (expand)
                maxExtent.ExpandBy(maxExtent.Width / 10, maxExtent.Height / 10); // work item #84 (Expand target envelope by 10%)

            return maxExtent;
        }

        /// <summary>
        /// Gets a list of just the point layers (and not the general layers).
        /// </summary>
        /// <returns>The point layers.</returns>
        public IMapPointLayer[] GetPointLayers()
        {
            return MapFrame.Layers.OfType<IMapPointLayer>().ToArray();
        }

        /// <summary>
        /// Gets a list of just the polygon layers (and not the general layers).
        /// </summary>
        /// <returns>The polygon layers.</returns>
        public IMapPolygonLayer[] GetPolygonLayers()
        {
            return MapFrame.Layers.OfType<IMapPolygonLayer>().ToArray();
        }

        /// <summary>
        /// Gets the subset of layers that are specifically raster layers, allowing
        /// you to control their symbology.
        /// </summary>
        /// <returns>The raster layers.</returns>
        public IMapRasterLayer[] GetRasterLayers()
        {
            return MapFrame.Layers.OfType<IMapRasterLayer>().ToArray();
        }

        /// <summary>
        /// Inverts the selected state of any members in the specified region.
        /// </summary>
        /// <param name="tolerant">The geographic region where selection occurs that is tolerant for point or linestrings.</param>
        /// <param name="strict">The tight envelope to use for polygons.</param>
        /// <param name="mode">The selection mode determining how to test for intersection.</param>
        /// <param name="affectedArea">The geographic region encapsulating the changed members.</param>
        /// <returns>boolean, true if members were changed by the selection process.</returns>
        public bool InvertSelection(Envelope tolerant, Envelope strict, SelectionMode mode, out Envelope affectedArea)
        {
            affectedArea = new Envelope();
            if (MapFrame == null)
                return false;
            return MapFrame.InvertSelection(tolerant, strict, mode, out affectedArea);
        }

        /// <summary>
        /// Converts a single point location into an equivalent geographic coordinate.
        /// </summary>
        /// <param name="position">The client coordinate relative to the map control.</param>
        /// <returns>The geographic ICoordinate interface.</returns>
        public Coordinate PixelToProj(Point position)
        {
            return _geoMapFrame.PixelToProj(position);
        }

        /// <summary>
        /// Converts a rectangle in pixel coordinates relative to the map control into
        /// a geographic envelope.
        /// </summary>
        /// <param name="rect">The rectangle to convert.</param>
        /// <returns>An Envelope interface.</returns>
        public Extent PixelToProj(Rectangle rect)
        {
            return _geoMapFrame.PixelToProj(rect);
        }

        /// <inheritdoc />
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == 0x0100)
            {
                if (ContainsFocus)
                    OnKeyDown(new KeyEventArgs((Keys)m.WParam.ToInt32()));
            }
            else if (m.Msg == 0x0101)
            {
                if (ContainsFocus)
                    OnKeyUp(new KeyEventArgs((Keys)m.WParam.ToInt32()));
            }

            return false;
        }

        /// <summary>
        /// Instead of using the usual buffers, this bypasses any buffering and instructs the layers
        /// to draw directly to the specified target rectangle on the graphics object. This is useful
        /// for doing vector drawing on much larger pages. The result will be centered in the
        /// specified target rectangle bounds.
        /// </summary>
        /// <param name="device">The graphics device to print to.</param>
        /// <param name="targetRectangle">the rectangle where the map content should be drawn.</param>
        public void Print(Graphics device, Rectangle targetRectangle)
        {
            MapFrame.Print(device, targetRectangle);
        }

        /// <summary>
        /// Instead of using the usual buffers, this bypasses any buffering and instructs the layers
        /// to draw directly to the specified target rectangle on the graphics object. This is useful
        /// for doing vector drawing on much larger pages. The result will be centered in the
        /// specified target rectangle bounds.
        /// </summary>
        /// <param name="device">The graphics device to print to.</param>
        /// <param name="targetRectangle">the rectangle where the map content should be drawn.</param>
        /// <param name="targetEnvelope">the extents to print in the target rectangle.</param>
        public void Print(Graphics device, Rectangle targetRectangle, Extent targetEnvelope)
        {
            MapFrame.Print(device, targetRectangle, targetEnvelope);
        }

        /// <summary>
        /// Converts a single geographic location into the equivalent point on the
        /// screen relative to the top left corner of the map.
        /// </summary>
        /// <param name="location">The geographic position to transform.</param>
        /// <returns>A Point with the new location.</returns>
        public Point ProjToPixel(Coordinate location)
        {
            return _geoMapFrame.ProjToPixel(location);
        }

        /// <summary>
        /// Converts a single geographic envelope into an equivalent Rectangle
        /// as it would be drawn on the screen.
        /// </summary>
        /// <param name="env">The geographic Envelope.</param>
        /// <returns>A Rectangle.</returns>
        public Rectangle ProjToPixel(Extent env)
        {
            return _geoMapFrame.ProjToPixel(env);
        }

        /// <summary>
        /// This causes all of the data layers to re-draw themselves to the buffer, rather than just drawing
        /// the buffer itself like what happens during "Invalidate".
        /// </summary>
        public override void Refresh()
        {
            _geoMapFrame.Initialize();
            base.Refresh();
            Invalidate();
        }

        /// <summary>
        /// Instructs the map to update the specified clipRectangle by drawing it to the back buffer.
        /// </summary>
        /// <param name="clipRectangle">Rectangle that gets updated.</param>
        public void RefreshMap(Rectangle clipRectangle)
        {
            Extent region = _geoMapFrame.BufferToProj(clipRectangle);
            _geoMapFrame.Invalidate(region);
        }

        /// <summary>
        /// This can be called any time, and is currently being used to capture
        /// the end of a resize event when the actual data should be updated.
        /// </summary>
        public virtual void ResetBuffer()
        {
            _geoMapFrame?.ResetBuffer();
        }

        /// <summary>
        /// Saves the dataset belonging to the layer.
        /// </summary>
        public virtual void SaveLayer()
        {
            var sfd = new SaveFileDialog();
            var layer = _geoMapFrame.Layers[0];
            if (layer is IMapFeatureLayer mfl)
            {
                sfd.Filter = DataManager.DefaultDataManager.VectorWriteFilter;
                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                mfl.DataSet.SaveAs(sfd.FileName, true);
                return;
            }

            if (layer is IMapRasterLayer mrl)
            {
                sfd.Filter = DataManager.DefaultDataManager.RasterWriteFilter;
                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                mrl.DataSet.SaveAs(sfd.FileName);
                return;
            }

            if (layer is IMapImageLayer mil)
            {
                sfd.Filter = DataManager.DefaultDataManager.ImageWriteFilter;
                if (sfd.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                mil.DataSet.SaveAs(sfd.FileName);
                return;
            }

            throw new ArgumentException("The layer chosen did not have a raster, vector or image dataset to save.");
        }

        /// <summary>
        /// Adds any members found in the specified region to the selected state as long as SelectionEnabled is set to true.
        /// </summary>
        /// <param name="tolerant">The geographic region where selection occurs that is tolerant for point or linestrings.</param>
        /// <param name="strict">The tight envelope to use for polygons.</param>
        /// <param name="mode">The selection mode.</param>
        /// <param name="affectedArea">The envelope affected area.</param>
        /// <param name="clear">Indicates whether prior selected features should be cleared.</param>
        /// <returns>Boolean, true if any members were added to the selection.</returns>
        public bool Select(Envelope tolerant, Envelope strict, SelectionMode mode, out Envelope affectedArea, ClearStates clear)
        {
            affectedArea = new Envelope();
            return MapFrame != null && MapFrame.Select(tolerant, strict, mode, out affectedArea, clear);
        }

        /// <summary>
        /// Captures an image of whatever the contents of the back buffer would be at the size of the screen.
        /// </summary>
        /// <returns>A bitmap with the snap shot.</returns>
        public Bitmap SnapShot()
        {
            var clip = ClientRectangle;
            var stencil = new Bitmap(clip.Width, clip.Height, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(stencil))
            {
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, stencil.Width, stencil.Height));

                // Translate the buffer so that drawing occurs in client coordinates, regardless of whether
                // there is a clip rectangle or not.
                using var m = new Matrix();
                m.Translate(-clip.X, -clip.Y);
                g.Transform = m;
                _geoMapFrame.Draw(new PaintEventArgs(g, clip));
            }

            return stencil;
        }

        /// <summary>
        /// Creates a snapshot that is scaled to fit to a bitmap of the specified width.
        /// </summary>
        /// <param name="width">The width of the desired bitmap.</param>
        /// <returns>A bitmap with the specified width.</returns>
        public Bitmap SnapShot(int width)
        {
            var height = (int)(width * (MapFrame.ViewExtents.Height / MapFrame.ViewExtents.Width));
            var bmp = new Bitmap(height, width);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                _geoMapFrame.Print(g, new Rectangle(0, 0, width, height));
            }

            return bmp;
        }

        /// <summary>
        /// Captures an image of whatever the contents of the back buffer would be at the size of the screen.
        /// Since the DotSpatial framework currently does not provide a mechanism for screen referenced labels
        /// the best we can do in the meantime is to allow the developer to pass in Label objects that are controls
        /// placed on top of the map.
        /// </summary>
        /// <param name="labelList">a list of Label controls</param>
        /// <returns>A bitmap with the snap shot plus embedded string text.</returns>
        public Bitmap SnapShot(List<Label> labelList)
        {
            Bitmap bmp = SnapShot();

            // inject the labels into the bitmap
            if (labelList != null && labelList.Count > 0)
            {
                using (Graphics gr = Graphics.FromImage(bmp))
                {
                    gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    // cycle through each label in the list
                    foreach (Label lbl in labelList)
                    {
                        // the label has to be visible and it must contain text
                        if (lbl != null && lbl.Visible && !string.IsNullOrWhiteSpace(lbl.Text))
                        {
                            // convert top left of label to screen coords
                            Point ptScr = lbl.PointToScreen(new Point(0, 0));

                            // take the screen point and determine where that point is on the map control
                            Point ptMap = PointToClient(ptScr);

                            // get the label rectangle
                            Rectangle rct = lbl.ClientRectangle;

                            // setup the background rectangle
                            if (lbl.BackColor != Color.Transparent)
                            {
                                // get the label backcolor
                                Brush brshBak = new SolidBrush(lbl.BackColor);

                                // to create the back area in graphics we can use the rectangle dimensions but we
                                // need to use proper location
                                gr.FillRectangle(brshBak, ptMap.X, ptMap.Y, rct.Width, rct.Height);
                            }

                            // setup the background border
                            if (lbl.BorderStyle != BorderStyle.None)
                            {
                                // get the label border color (assumed to be black)
                                Pen penBdr = new Pen(Color.Black, 1);

                                // to create the back border in graphics we can use the rectangle dimensions but we
                                // need to use proper location
                                gr.DrawRectangle(penBdr, ptMap.X, ptMap.Y, rct.Width, rct.Height);
                            }

                            // set up the text part
                            string txt = lbl.Text;
                            Font fnt = lbl.Font;
                            SolidBrush brsh = new SolidBrush(lbl.ForeColor);

                            // draw the text string directly on the bitmap
                            gr.DrawString(txt, fnt, brsh, ptMap);
                        }
                    }
                }
            }

            // return the modified bitmap
            return bmp;
        }

        /// <summary>
        /// Captures an image of whatever the contents of the back buffer would be at the size of the screen.
        /// Since the DotSpatial framework currently does not provide a mechanism for screen referenced labels
        /// the best we can do in the meantime is to allow the developer to pass in Label objects that are controls
        /// placed on top of the map.
        /// </summary>
        /// <param name="lbl">a Label control</param>
        /// <returns>A bitmap with the snap shot plus embedded string text.</returns>
        public Bitmap SnapShot(Label lbl)
        {
            List<Label> labelList = new List<Label>() { lbl };
            return SnapShot(labelList);
        }

        /// <summary>
        /// Adds any members found in the specified region to the selected state as long as SelectionEnabled is set to true.
        /// </summary>
        /// <param name="tolerant">The geographic region where selection occurs that is tolerant for point or linestrings.</param>
        /// <param name="strict">The tight envelope to use for polygons.</param>
        /// <param name="mode">The selection mode.</param>
        /// <param name="affectedArea">The envelope affected area.</param>
        /// <returns>Boolean, true if any members were added to the selection.</returns>
        public bool UnSelect(Envelope tolerant, Envelope strict, SelectionMode mode, out Envelope affectedArea)
        {
            affectedArea = new Envelope();
            if (MapFrame == null)
                return false;
            return MapFrame.UnSelect(tolerant, strict, mode, out affectedArea);
        }

        /// <summary>
        /// Zooms in one notch, so that the scale becomes larger and the features become larger.
        /// </summary>
        public void ZoomIn()
        {
            MapFrame.ZoomIn();
        }

        /// <summary>
        /// Zooms out one notch so that the scale becomes smaller and the features become smaller.
        /// </summary>
        public void ZoomOut()
        {
            MapFrame.ZoomOut();
        }

        /// <summary>
        /// Instructs the map to change the perspective to include the entire drawing content, and
        /// in the case of 3D maps, changes the perspective to look from directly overhead.
        /// </summary>
        public void ZoomToMaxExtent()
        {
            // to prevent exception when zoom to map with one layer with one point
            ViewExtents = GetMaxExtent(true);
            IsZoomedToMaxExtent = true;
        }

        /// <summary>
        /// Zooms to the next extent.
        /// </summary>
        public void ZoomToNext()
        {
            MapFrame.ZoomToNext();
        }

        /// <summary>
        /// Zooms to the previous extent.
        /// </summary>
        public void ZoomToPrevious()
        {
            MapFrame.ZoomToPrevious();
        }

        /// <summary>
        /// This is so that if you have a basic map interface you can still prompt to add a layer, you just won't get an IMapLayer back.
        /// </summary>
        /// <returns>The added layer.</returns>
        ILayer IBasicMap.AddLayer()
        {
            return AddLayer();
        }

        /// <summary>
        /// Draws the content.
        /// </summary>
        /// <param name="g">Graphics object used for drawing.</param>
        /// <param name="e">The event args.</param>
        protected virtual void Draw(Graphics g, PaintEventArgs e)
        {
            _geoMapFrame.Draw(new PaintEventArgs(g, e.ClipRectangle));
        }

        /// <inheritdoc />
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            string[] s = (string[])drgevent.Data.GetData(DataFormats.FileDrop, false);
            if (s == null)
            {
                base.OnDragDrop(drgevent);
                return;
            }

            int i;
            bool failed = false;
            for (i = 0; i < s.Length; i++)
            {
                try
                {
                    AddLayer(s[i]);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);

                    // failed at least one effort
                    failed = true;
                }
            }

            if (failed)
            {
                MessageBox.Show(MessageStrings.Map_OnDragDrop_Invalid);
            }
        }

        /// <inheritdoc />
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            drgevent.Effect = drgevent.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            base.OnDragEnter(drgevent);
        }

        /// <summary>
        /// Handles removing event handlers for the map frame.
        /// </summary>
        /// <param name="mapFrame">MapFrame the event handlers get removed from.</param>
        protected virtual void OnExcludeMapFrame(IMapFrame mapFrame)
        {
            if (mapFrame == null)
                return;
            mapFrame.UpdateMap -= MapFrameUpdateMap;
            mapFrame.ScreenUpdated -= MapFrameScreenUpdated;
            mapFrame.ItemChanged -= MapFrameItemChanged;
            mapFrame.BufferChanged -= MapFrameBufferChanged;
            mapFrame.SelectionChanged -= MapFrameSelectionChanged;
            mapFrame.LayerAdded -= MapFrameLayerAdded;
            mapFrame.ViewExtentsChanged -= MapFrameViewExtentsChanged;
            Legend?.RemoveMapFrame(mapFrame, true);
        }

        /// <summary>
        /// Raises <see cref="FinishedRefresh"/> event.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnFinishedRefresh(EventArgs e)
        {
            FinishedRefresh?.Invoke(this, e);
        }

        /// <summary>
        /// Fires the FunctionModeChanged event.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnFunctionModeChanged(object sender, EventArgs e)
        {
            FunctionModeChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Handles adding new event handlers to the map frame.
        /// </summary>
        /// <param name="mapFrame">MapFrame the event handler get added to.</param>
        protected virtual void OnIncludeMapFrame(IMapFrame mapFrame)
        {
            if (mapFrame == null)
            {
                Legend?.RefreshNodes();
                return;
            }

            mapFrame.Parent = this;
            mapFrame.UpdateMap += MapFrameUpdateMap;
            mapFrame.ScreenUpdated += MapFrameScreenUpdated;
            mapFrame.ItemChanged += MapFrameItemChanged;
            mapFrame.BufferChanged += MapFrameBufferChanged;
            mapFrame.SelectionChanged += MapFrameSelectionChanged;
            mapFrame.LayerAdded += MapFrameLayerAdded;
            mapFrame.ViewExtentsChanged += MapFrameViewExtentsChanged;
            Legend?.AddMapFrame(mapFrame);
        }

        /// <summary>
        /// Fires the LayerAdded event.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnLayerAdded(object sender, LayerEventArgs e)
        {
            LayerAdded?.Invoke(sender, e);
        }

        /// <summary>
        /// Handles the resizing in the case where the map uses docking, and therefore
        /// needs to be updated whenever the form changes size.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnLoad(EventArgs e)
        {
            KeyUp += MapKeyUp;
            KeyDown += MapKeyDown;

            SizeChanged += OnSizeChanged;
            _oldSize = Size;
        }

        /// <summary>
        /// Fires the DoMouseDoubleClick method on the ActiveTools.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            var args = new GeoMouseArgs(e, this);
            foreach (var tool in MapFunctions.Where(_ => _.Enabled))
            {
                tool.DoMouseDoubleClick(args);
                if (args.Handled)
                    break;
            }

            base.OnMouseDoubleClick(e);
        }

        /// <summary>
        /// Fires the OnMouseDown event on the Active Tools.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            var args = new GeoMouseArgs(e, this);
            foreach (var tool in MapFunctions.Where(_ => _.Enabled))
            {
                tool.DoMouseDown(args);
                if (args.Handled)
                    break;
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Fires the OnMouseMove event on the Active Tools.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            var args = new GeoMouseArgs(e, this);
            foreach (var tool in MapFunctions.Where(_ => _.Enabled))
            {
                tool.DoMouseMove(args);
                if (args.Handled)
                    break;
            }

            GeoMouseMove?.Invoke(this, args);

            base.OnMouseMove(e);
        }

        /// <summary>
        /// Fires the OnMouseUp event on the Active Tools.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            var args = new GeoMouseArgs(e, this);
            foreach (var tool in MapFunctions.Where(_ => _.Enabled))
            {
                tool.DoMouseUp(args);
                if (args.Handled)
                    break;
            }

            base.OnMouseUp(e);
        }

        /// <summary>
        /// Fires the OnMouseWheel event for the active tools.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            var args = new GeoMouseArgs(e, this);
            foreach (var tool in MapFunctions.Where(_ => _.Enabled))
            {
                tool.DoMouseWheel(args);
                if (args.Handled)
                    break;
            }

            base.OnMouseWheel(e);
        }

        /// <summary>
        /// Perform custom drawing.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_geoMapFrame.IsPanning)
                return;

            var clip = e.ClipRectangle;
            if (clip.IsEmpty)
                clip = ClientRectangle;

            // if the area to paint is too small, there's nothing to paint.
            // Added to fix http://dotspatial.codeplex.com/workitem/320
            if (clip.Width < 1 || clip.Height < 1)
                return;

            using var stencil = new Bitmap(clip.Width, clip.Height, PixelFormat.Format32bppArgb);
            using var g = Graphics.FromImage(stencil);
            using (var b = new SolidBrush(BackColor))
                g.FillRectangle(b, new Rectangle(0, 0, stencil.Width, stencil.Height));

            using (var m = new Matrix())
            {
                m.Translate(-clip.X, -clip.Y);
                g.Transform = m;

                Draw(g, e);

                var args = new MapDrawArgs(g, clip, _geoMapFrame);
                foreach (var tool in MapFunctions.Where(_ => _.Enabled))
                {
                    tool.Draw(args);
                }

                var pe = new PaintEventArgs(g, e.ClipRectangle);
                base.OnPaint(pe);
            }

            e.Graphics.DrawImageUnscaled(stencil, clip.X, clip.Y);
        }

        /// <summary>
        /// Occurs when this control tries to paint the background.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // This is done deliberately to prevent flicker.
            // base.OnPaintBackground(e);
        }

        /// <summary>
        /// Fires the ProjectionChanged event.
        /// </summary>
        protected virtual void OnProjectionChanged()
        {
            ProjectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs after this object has been resized.
        /// </summary>
        protected virtual void OnResized()
        {
            Resized?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs after the selection is updated on all the layers.
        /// </summary>
        protected virtual void OnSelectionChanged()
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the ViewExtentsChanged event. Corrects the ViewExtent if it is smaller than 1e-7. If ZoomOutFartherThanMaxExtent is set, it corrects the
        /// ViewExtent if it is bigger then 1e+9. Otherwise it corrects the ViewExtent if it is bigger than the Maps extent + 10%.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="args">The extent args.</param>
        protected virtual void OnViewExtentsChanged(object sender, ExtentArgs args)
        {
            double minExt = 1e-7;
            double maxExt = 1e+9; // if we can zoom out farther than the maps extent we don't zoom out farther than this
            var maxExtent = GetMaxExtent(true);
            if (Math.Max(maxExtent.Height, maxExtent.Width) > maxExt)
                maxExt = Math.Max(maxExtent.Height, maxExtent.Width); // make sure maxExtent isn't bigger than maxExt

            if (!ZoomOutFartherThanMaxExtent && (maxExtent.Width != 0 && maxExtent.Height != 0) && (ViewExtents.Width > (maxExtent.Width + 1)) && (ViewExtents.Height > (maxExtent.Height + 1)))
            {
                // we only want to zoom out to the maps extent
                ZoomToMaxExtent();
            }
            else if (ZoomOutFartherThanMaxExtent && ViewExtents.Width > maxExt)
            {
                // we want to zoom out farther than the maps extent and the width is bigger than maxExt
                double x = ViewExtents.Center.X;
                double y = ViewExtents.Center.Y;
                double add = (maxExt / 2) - minExt;
                ViewExtents = new Extent(x - add, y - 100, x + add, y + 100); // resizes the width to stay below maxExt
            }
            else if (ZoomOutFartherThanMaxExtent && ViewExtents.Height > maxExt)
            {
                // we want to zoom out farther than the maps extent and the height is bigger than maxExt
                double x = ViewExtents.Center.X;
                double y = ViewExtents.Center.Y;
                double add = (maxExt / 2) - minExt;
                ViewExtents = new Extent(x - 100, y - add, x + 100, y + add); // resizes the height to stay below maxExt
            }
            else if (ViewExtents.Width < minExt || ViewExtents.Height < minExt)
            {
                // the current height or width is smaller than minExt
                double x = ViewExtents.Center.X;
                double y = ViewExtents.Center.Y;
                Extent newExtent = new(x - (minExt / 2), y - (minExt / 2), x + (minExt / 2), y + (minExt / 2)); // resize to stay above the minExt

                // changed by jany_ (2016-04-14) Remember the last minimum extent in case MapFrame.ResetAspectRatio decides to resize the ViewExtent to something smaller than this.
                // We don't want to cause a loop between this point and MapFrame.ResetAspectRatio switching ViewExtent between minExt and the corresponding extent that comes from fitting minExt to the maps aspect ratio.
                if (_lastMinExtent == null || !_lastMinExtent.Equals(newExtent))
                {
                    _lastMinExtent = newExtent;
                    ViewExtents = newExtent;
                }
            }
            else
            {
                ViewExtentsChanged?.Invoke(sender, args);
            }
        }

        private void Configure()
        {
            MapFrame = new MapFrame(this, new Extent(0, 0, 0, 0));

            IMapFunction info = new MapFunctionIdentify(this);
            IMapFunction pan = new MapFunctionPan(this);
            IMapFunction label = new MapFunctionLabelSelect(this);
            IMapFunction select = new MapFunctionSelect(this);
            IMapFunction zoomIn = new MapFunctionClickZoom(this);
            IMapFunction zoomOut = new MapFunctionZoomOut(this);
            IMapFunction zoomPan = new MapFunctionZoom(this);
            MapFunctions = new List<IMapFunction>
                           {
                               new MapFunctionKeyNavigation(this),
                               pan,
                               select,
                               zoomIn,
                               zoomOut,
                               zoomPan,
                               label,
                               info,
                           };
            _functionLookup = new Dictionary<FunctionMode, IMapFunction>
                              {
                                  { FunctionMode.Pan, pan },
                                  { FunctionMode.Info, info },
                                  { FunctionMode.Label, label },
                                  { FunctionMode.Select, select },
                                  { FunctionMode.ZoomIn, zoomIn },
                                  { FunctionMode.ZoomOut, zoomOut },
                                  { FunctionMode.ZoomPan, zoomPan }
                              };

            CollisionDetection = false;

            var keyNavigation = MapFunctions.Find(f => f.GetType() == typeof(MapFunctionKeyNavigation));

            if (keyNavigation != null)
            {
                ActivateMapFunction(keyNavigation);
            }

            // changed by Jiri Kadlec - default function mode is none
            FunctionMode = FunctionMode.None;
        }

        private void MapKeyDown(object sender, KeyEventArgs e)
        {
            foreach (var tool in MapFunctions.Where(_ => _.Enabled))
            {
                tool.DoKeyDown(e);
                if (e.Handled)
                    break;
            }
        }

        private void MapKeyUp(object sender, KeyEventArgs e)
        {
            foreach (var tool in MapFunctions.Where(_ => _.Enabled))
            {
                tool.DoKeyUp(e);
                if (e.Handled)
                    break;
            }
        }

        private void MapFrameBufferChanged(object sender, ClipArgs e)
        {
            Rectangle view = MapFrame.View;
            foreach (Rectangle clip in e.ClipRectangles)
            {
                if (clip.IsEmpty == false)
                {
                    var mapClip = new Rectangle(clip.X - view.X, clip.Y - view.Y, clip.Width, clip.Height);
                    Invalidate(mapClip);
                }
            }
        }

        private void MapFrameLayerAdded(object sender, LayerEventArgs e)
        {
            OnLayerAdded(sender, e);
        }

        private void MapFrameSelectionChanged(object sender, EventArgs e)
        {
            OnSelectionChanged();
        }

        private void MapFrameViewExtentsChanged(object sender, ExtentArgs args)
        {
            OnViewExtentsChanged(sender, args);
        }

        private void MapFrameItemChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void MapFrameScreenUpdated(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void MapFrameUpdateMap(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void OnSizeChanged(object sender, EventArgs eventArgs)
        {
            if (_geoMapFrame != null)
            {
                var diff = new Point
                {
                    X = Size.Width - _oldSize.Width,
                    Y = Size.Height - _oldSize.Height
                };
                var newView = new Rectangle(_geoMapFrame.View.X, _geoMapFrame.View.Y, _geoMapFrame.View.Width + diff.X, _geoMapFrame.View.Height + diff.Y);

                // Check for minimal size of view.
                if (newView.Width < 5)
                    newView.Width = 5;
                if (newView.Height < 5)
                    newView.Height = 5;

                _geoMapFrame.View = newView;
                _geoMapFrame.ResetExtents();
                Invalidate();
            }

            _oldSize = Size;
            OnResized();
        }

        #endregion
    }
}