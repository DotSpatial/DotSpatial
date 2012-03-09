// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 7/25/2008 9:37:01 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------
// Jiri Kadlec        |  2/18/2010         |  Added zoom out button and custom mouse cursors
// Kyle Ellison       | 12/15/2010         |  Fixed Issue #190 (Deactivated MapFunctions still involved)
// ********************************************************************************************************

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
using DotSpatial.Topology;
using Point = System.Drawing.Point;
using SelectionMode = DotSpatial.Symbology.SelectionMode;

namespace DotSpatial.Controls
{
    /// <summary>
    /// The Map Control for 2D applications.
    /// </summary>
    public partial class Map : UserControl, IMap
    {
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
        /// Occurs after a resize event
        /// </summary>
        public event EventHandler Resized;

        /// <summary>
        /// Occurs after the selection has changed for all the layers
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Occurs after a layer has been added to the mapframe, or any of the child groups of that mapframe.
        /// </summary>
        public event EventHandler<LayerEventArgs> LayerAdded;

        /// <summary>
        /// Public event advertising the mouse movement
        /// </summary>
        public event EventHandler<GeoMouseArgs> GeoMouseMove;

        /// <summary>
        /// Fires after the view extents have been altered and the map has redrawn to the new extents.
        /// This is an echo of the MapFrame.ViewExtentsChanged, so you only want one handler.
        /// </summary>
        public event EventHandler<ExtentArgs> ViewExtentsChanged;

        /// <summary>
        /// Occurs after the projection of the map has been changed
        /// </summary>
        public event EventHandler ProjectionChanged;

        #endregion

        #region Private Variables

        private Dictionary<FunctionMode, IMapFunction> _functionLookup;
        private FunctionMode _functionMode;
        private IMapFrame _geoMapFrame;
        private int _isBusyIndex;
        private bool _isResizing;
        private ILegend _legend;
        private Size _oldParentSize;
        private bool _parentFormSizeChanging;
        private IProgressHandler _progressHandler;
        private bool isPanningTemporarily;
        private FunctionMode previousFunction = FunctionMode.None;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a map component that can be dropped on a form.
        /// </summary>
        public Map()
        {
            InitializeComponent();
            Configure();
        }

        /// <summary>
        /// Handles the resizing in the case where the map uses docking, and therefore
        /// needs to be updated whenever the form changes size.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            KeyUp += Map_KeyUp;
            KeyDown += Map_KeyDown;

            if (ParentForm != null)
            {
                ParentForm.ResizeEnd += ParentFormResizeEnd;
                ParentForm.KeyDown += ParentFormKeyDown;
                ParentForm.KeyUp += ParentFormKeyUp;
                //ParentForm.Resize += ParentResize;
                ParentForm.ResizeBegin += ParentForm_ResizeBegin;
                _oldParentSize = ParentForm.Size;
            }
        }

        private void ParentForm_ResizeBegin(object sender, EventArgs e)
        {
            _parentFormSizeChanging = true;
            _isResizing = true;
        }

        private void Map_KeyUp(object sender, KeyEventArgs e)
        {
            // Allow panning if the space is pressed.
            if (e.KeyCode == Keys.Space && isPanningTemporarily)
            {
                this.FunctionMode = previousFunction;
                isPanningTemporarily = false;
            }

            foreach (IMapFunction tool in MapFunctions)
            {
                if (tool.Enabled) tool.DoKeyUp(e);
            }
        }

        private void Map_KeyDown(object sender, KeyEventArgs e)
        {
            // Allow panning if the space is pressed.
            if (e.KeyCode == Keys.Space && !isPanningTemporarily)
            {
                previousFunction = this.FunctionMode;
                this.FunctionMode = FunctionMode.Pan;
                isPanningTemporarily = true;
            }

            foreach (IMapFunction tool in MapFunctions)
            {
                if (tool.Enabled) tool.DoKeyDown(e);
            }
        }

        private void ParentFormResizeEnd(object sender, EventArgs e)
        {
            _parentFormSizeChanging = false;
            _isResizing = false;
            //_resizeEndTimer.Stop();  // parent form resize end is good, but fails on splitters.
            // The splitter doesn't require a timer, but if some other control setup exists that
            // has a progressive resize, the timer helps prevent jerky behavior.
            if (MapFrame != null)
            {
                if (ParentForm != null)
                {
                    if (ParentForm.Size == _oldParentSize) return;
                    _oldParentSize = ParentForm.Size;
                }
                ResetExtents();
            }
            OnResized();
        }

        private void ResetExtents()
        {
            if (MapFrame != null)
            {
                _geoMapFrame.ResetExtents();
                Invalidate();
            }
        }

        private void Configure()
        {
            MapFrame = new MapFrame(this, new Extent(-180, -90, 180, 90));
            //_resizeEndTimer = new Timer {Interval = 100};
            //_resizeEndTimer.Tick += _resizeEndTimer_Tick;

            IMapFunction info = new MapFunctionIdentify(this);
            IMapFunction pan = new MapFunctionPan(this);
            IMapFunction label = new MapFunctionLabelSelect(this);
            IMapFunction select = new MapFunctionSelect(this);
            IMapFunction zoomIn = new MapFunctionClickZoom(this);
            IMapFunction zoomOut = new MapFunctionZoomOut(this);
            MapFunctions = new List<IMapFunction>
                               {
                                   new MapFunctionZoom(this),
                                   pan,
                                   select,
                                   zoomIn,
                                   zoomOut,
                                   label,
                                   info,
                               };
            _functionLookup = new Dictionary<FunctionMode, IMapFunction>
                                  {
                                      {FunctionMode.Pan, pan},
                                      {FunctionMode.Info, info},
                                      {FunctionMode.Label, label},
                                      {FunctionMode.Select, select},
                                      {FunctionMode.ZoomIn, zoomIn},
                                      {FunctionMode.ZoomOut, zoomOut}
                                  };

            CollisionDetection = false;

            //changed by Jiri Kadlec - default function mode is none
            FunctionMode = FunctionMode.None;
        }

        private void ParentFormKeyUp(object sender, KeyEventArgs e)
        {
            foreach (IMapFunction tool in MapFunctions)
            {
                if (tool.Enabled) tool.DoKeyUp(e);
            }
        }

        private void ParentFormKeyDown(object sender, KeyEventArgs e)
        {
            foreach (IMapFunction tool in MapFunctions)
            {
                if (tool.Enabled) tool.DoKeyDown(e);
            }
        }

        private void MapFrameItemChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void MapFrameUpdateMap(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void MapFrameScreenUpdated(object sender, EventArgs e)
        {
            Invalidate();
        }

        /// <inheritdoc />
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            drgevent.Effect = drgevent.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            base.OnDragEnter(drgevent);
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

        #endregion

        #region Methods

        /// <summary>
        /// This will add a new label category that will only apply to the specified filter expression.
        /// This will not remove any existing categories.
        /// </summary>
        /// <param name="featureLayer">The feature layer that the labels should be applied to</param>
        /// <param name="expression">The string expression where field names are in square brackets</param>
        /// <param name="filterExpression">The string filter expression that controls which features are labeled.
        /// Field names are in square brackets, strings in single quotes.</param>
        /// <param name="symbolizer">The label symbolizer that controls the basic appearance of the labels in this
        /// category.</param>
        /// <param name="name">The name of the category.</param>
        public void AddLabels(IFeatureLayer featureLayer, string expression, string filterExpression,
                              ILabelSymbolizer symbolizer, string name)
        {
            if (featureLayer.LabelLayer == null) featureLayer.LabelLayer = new MapLabelLayer();
            featureLayer.ShowLabels = true;
            ILabelCategory lc = new LabelCategory
                                    {
                                        Expression = expression,
                                        FilterExpression = filterExpression,
                                        Symbolizer = symbolizer,
                                        Name = name,
                                    };
            featureLayer.LabelLayer.Symbology.Categories.Add(lc);
            featureLayer.LabelLayer.CreateLabels();
        }

        /// <summary>
        /// This will add a new label category that will only apply to the specified filter expression.  This will
        /// not remove any existing categories.
        /// </summary>
        /// <param name="featureLayer">The feature layer that the labels should be applied to</param>
        /// <param name="expression">The string expression where field names are in square brackets</param>
        /// <param name="filterExpression">The string filter expression that controls which features are labeled.
        /// Field names are in square brackets, strings in single quotes.</param>
        /// <param name="symbolizer">The label symbolizer that controls the basic appearance of the labels in this
        ///  category.</param>
        /// <param name="width">A geographic width, so that if the map is zoomed to a geographic width smaller than
        /// this value, labels should appear.</param>
        public void AddLabels(IFeatureLayer featureLayer, string expression, string filterExpression,
                              ILabelSymbolizer symbolizer, double width)
        {
            if (featureLayer.LabelLayer == null) featureLayer.LabelLayer = new MapLabelLayer();
            featureLayer.ShowLabels = true;
            ILabelCategory lc = new LabelCategory
                                    {
                                        Expression = expression,
                                        FilterExpression = filterExpression,
                                        Symbolizer = symbolizer
                                    };
            featureLayer.LabelLayer.UseDynamicVisibility = true;
            featureLayer.LabelLayer.DynamicVisibilityWidth = width;
            featureLayer.LabelLayer.Symbology.Categories.Add(lc);
            featureLayer.LabelLayer.CreateLabels();
        }

        /// <summary>
        /// Gets the subset of layers that are specifically raster layers, allowing
        /// you to control their symbology.
        /// </summary>
        /// <returns></returns>
        public IMapImageLayer[] GetImageLayers()
        {
            return MapFrame.Layers.OfType<IMapImageLayer>().ToArray();
        }

        /// <summary>
        /// Gets the subset of layers that are specifically raster layers, allowing
        /// you to control their symbology.
        /// </summary>
        /// <returns></returns>
        public IMapRasterLayer[] GetRasterLayers()
        {
            return MapFrame.Layers.OfType<IMapRasterLayer>().ToArray();
        }

        /// <summary>
        /// Gets a list of just the line layers (and not the general layers)
        /// </summary>
        /// <returns></returns>
        public IMapLineLayer[] GetLineLayers()
        {
            return MapFrame.Layers.OfType<IMapLineLayer>().ToArray();
        }

        /// <summary>
        /// Gets a list of just the line layers (and not the general layers)
        /// </summary>
        /// <returns></returns>
        public IMapPolygonLayer[] GetPolygonLayers()
        {
            return MapFrame.Layers.OfType<IMapPolygonLayer>().ToArray();
        }

        /// <summary>
        /// Gets a list of just the line layers (and not the general layers)
        /// </summary>
        /// <returns></returns>
        public IMapPointLayer[] GetPointLayers()
        {
            return MapFrame.Layers.OfType<IMapPointLayer>().ToArray();
        }

        /// <summary>
        /// Gets a list of just the feature layers regardless of whether they are lines, points, or polygons
        /// </summary>
        /// <returns>An array of IMapFeatureLayers</returns>
        public IMapFeatureLayer[] GetFeatureLayers()
        {
            return MapFrame.Layers.OfType<IMapFeatureLayer>().ToArray();
        }

        /// <summary>
        /// Gets the MapFunction based on the string name
        /// </summary>
        /// <param name="name">The string name to find</param>
        /// <returns>The MapFunction with the specified name</returns>
        public IMapFunction GetMapFunction(string name)
        {
            return MapFunctions.First(f => f.Name == name);
        }

        /// <summary>
        /// Removes any members from existing in the selected state
        /// </summary>
        public bool ClearSelection(out IEnvelope affectedArea)
        {
            affectedArea = new Envelope();
            if (MapFrame == null) return false;
            return MapFrame.ClearSelection(out affectedArea);
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
            if (MapFrame == null) return false;
            return MapFrame.Select(tolerant, strict, mode, out affectedArea);
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
            if (MapFrame == null) return false;
            return MapFrame.InvertSelection(tolerant, strict, mode, out affectedArea);
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
            if (MapFrame == null) return false;
            return MapFrame.UnSelect(tolerant, strict, mode, out affectedArea);
        }

        /// <summary>
        /// Allows the user to add a new layer to the map using an open file dialog to choose a layer file.
        /// Multi-select is an option, so this return a list with all the layers.
        /// </summary>
        public virtual List<IMapLayer> AddLayers()
        {
            List<IDataSet> sets = DataManager.DefaultDataManager.OpenFiles();
            if (sets == null || sets.Count == 0) return null;
            List<IMapLayer> results = new List<IMapLayer>();
            foreach (IDataSet set in sets)
            {
                if (set == null) return null;

                IFeatureSet fs = set as IFeatureSet;
                if (fs != null)
                {
                    results.Add(Layers.Add(fs));
                    continue;
                }
                IImageData id = set as IImageData;
                if (id != null)
                {
                    results.Add(Layers.Add(id));
                    continue;
                }
                IRaster r = set as IRaster;
                if (r != null)
                {
                    results.Add(Layers.Add(r));
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
        public virtual IMapLayer AddLayer(string fileName)
        {
            return Layers.Add(fileName);
        }

        /// <summary>
        /// This is so that if you have a basic map interface you can still prompt
        /// to add a layer, you just won't get an IMapLayer back.
        /// </summary>
        ILayer IBasicMap.AddLayer()
        {
            return AddLayer();
        }

        /// <summary>
        /// Uses the file dialog to allow selection of a fileName for opening the
        /// new layer, but does not allow multiple files to be added at once.
        /// </summary>
        /// <returns>The newly opened IMapLayer</returns>
        public virtual IMapLayer AddLayer()
        {
            if (DataManager.DefaultDataManager.ProgressHandler == null)
            {
                if (ProgressHandler != null)
                {
                    DataManager.DefaultDataManager.ProgressHandler = ProgressHandler;
                }
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
        /// Allows a multi-select file dialog to add raster layers, applying a
        /// filter so that only supported raster formats will appear.
        /// </summary>
        /// <returns>A list of the IMapRasterLayers that were opened.</returns>
        public virtual List<IMapRasterLayer> AddRasterLayers()
        {
            List<IRaster> sets = DataManager.DefaultDataManager.OpenRasters();
            return sets.Select(raster => Layers.Add(raster)).ToList();
        }

        /// <summary>
        /// Allows an open file dialog without multi-select enabled to add a single
        /// raster to the map as a layer, and returns the added layer.
        /// </summary>
        /// <returns>The IMapRasterLayer that was added</returns>
        public virtual IMapRasterLayer AddRasterLayer()
        {
            return Layers.Add(DataManager.DefaultDataManager.OpenRaster());
        }

        /// <summary>
        /// Allows a mult-select open file dialog to specify several fileNames to add.
        /// Only files with supported vector extensions will be shown.
        /// </summary>
        /// <returns>The list of added MapFeatureLayers</returns>
        public virtual List<IMapFeatureLayer> AddFeatureLayers()
        {
            List<IFeatureSet> sets = DataManager.DefaultDataManager.OpenVectors();
            return sets.Select(featureSet => Layers.Add(featureSet)).ToList();
        }

        /// <summary>
        /// Allows an open file dialog without multi-select enabled to add a single
        /// raster tot he map as a layer, and returns the added layer.
        /// </summary>
        /// <returns></returns>
        public virtual IMapFeatureLayer AddFeatureLayer()
        {
            return Layers.Add(DataManager.DefaultDataManager.OpenVector());
        }

        /// <summary>
        /// Allows a mult-select open file dialog to specify several fileNames to add.
        /// Only files with supported image extensions will be shown.
        /// </summary>
        /// <returns>The list of added MapImageLayers</returns>
        public virtual List<IMapImageLayer> AddImageLayers()
        {
            List<IImageData> sets = DataManager.DefaultDataManager.OpenImages();
            return sets.Select(imageData => Layers.Add(imageData)).ToList();
        }

        /// <summary>
        /// Allows an open dialog without multi-select to specify a single fileName
        /// to be added to the map as a new layer and returns the newly added layer.
        /// </summary>
        /// <returns>The layer that was added to the map.</returns>
        public virtual IMapImageLayer AddImageLayer()
        {
            return Layers.Add(DataManager.DefaultDataManager.OpenImage());
        }

        /// <summary>
        /// This can be called any time, and is currently being used to capture
        /// the end of a resize event when the actual data should be updated.
        /// </summary>
        public virtual void ResetBuffer()
        {
            _geoMapFrame.ResetBuffer();
        }

        /// <summary>
        /// Saves the dataset belonging to the layer.
        /// </summary>
        public virtual void SaveLayer()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            IMapLayer layer = _geoMapFrame.Layers[0];
            IMapFeatureLayer mfl = layer as IMapFeatureLayer;
            if (mfl != null)
            {
                sfd.Filter = DataManager.DefaultDataManager.VectorWriteFilter;
                if (sfd.ShowDialog() != DialogResult.OK) { return; }
                mfl.DataSet.SaveAs(sfd.FileName, true);
                return;
            }
            IMapRasterLayer mrl = layer as IMapRasterLayer;
            if (mrl != null)
            {
                sfd.Filter = DataManager.DefaultDataManager.RasterWriteFilter;
                if (sfd.ShowDialog() != DialogResult.OK) { return; }
                mrl.DataSet.SaveAs(sfd.FileName);
                return;
            }
            IMapImageLayer mil = layer as IMapImageLayer;
            if (mil != null)
            {
                sfd.Filter = DataManager.DefaultDataManager.ImageWriteFilter;
                if (sfd.ShowDialog() != DialogResult.OK) { return; }
                mil.DataSet.SaveAs(sfd.FileName);
                return;
            }
            throw new ArgumentException("The layer chosen did not have a raster, vector or image dataset to save.");
        }

        /// <summary>
        /// Instructs the map to change the perspective to include the entire drawing content, and
        /// in the case of 3D maps, changes the perspective to look from directly overhead.
        /// </summary>
        public void ZoomToMaxExtent()
        {
            // to prevent exception when zoom to map with one layer with one point
            const double eps = 1e-7;
            if (Extent.Width < eps || Extent.Height < eps)
            {
                Extent newExtent = new Extent(Extent.MinX - eps, Extent.MinY - eps, Extent.MaxX + eps, Extent.MaxY + eps);
                ViewExtents = newExtent;
            }
            else
            {
                ViewExtents = Extent;
            }
        }

        /// <summary>
        /// This activates the labels for the specified feature layer that will be the specified expression
        /// where field names are in square brackets like "[Name]: [Value]".  This will label all the features,
        /// and remove any previous labeling.
        /// </summary>
        /// <param name="featureLayer">The FeatureLayer to apply the labels to.</param>
        /// <param name="expression">The string label expression to use where field names are in square brackets like
        /// [Name]</param>
        /// <param name="font">The font to use for these labels</param>
        /// <param name="fontColor">The color for the labels</param>
        public void AddLabels(IFeatureLayer featureLayer, string expression, Font font, Color fontColor)
        {
            featureLayer.ShowLabels = true;

            MapLabelLayer ll = new MapLabelLayer();
            ll.Symbology.Categories.Clear();
            LabelCategory lc = new LabelCategory { Expression = expression };
            ll.Symbology.Categories.Add(lc);

            ILabelSymbolizer ls = ll.Symbolizer;
            ls.Orientation = ContentAlignment.MiddleCenter;
            ls.BackColorEnabled = false;
            ls.BackColorEnabled = false;
            ls.FontColor = fontColor;
            ls.FontFamily = font.FontFamily.ToString();
            ls.FontSize = font.Size;
            ls.FontStyle = font.Style;
            ls.PartsLabelingMethod = PartLabelingMethod.LabelLargestPart;
            featureLayer.LabelLayer = ll;
        }

        /// <summary>
        /// Removes any existing label categories
        /// </summary>
        public void ClearLabels(IFeatureLayer featureLayer)
        {
            featureLayer.ShowLabels = false;
            if (featureLayer.LabelLayer == null) return;
        }

        /// <summary>
        /// Opens a new layer and automatically adds it to the map.
        /// </summary>
        [Obsolete("Use AddLayers() with no parameters")]
        public virtual void OpenLayer()
        {
            AddLayers();
        }

        /// <summary>
        /// Instead of using the usual buffers, this bypasses any buffering and instructs the layers
        /// to draw directly to the specified target rectangle on the graphics object.  This is useful
        /// for doing vector drawing on much larger pages.  The result will be centered in the
        /// specified target rectangle bounds.
        /// </summary>
        /// <param name="device">The graphics device to print to</param>
        /// <param name="targetRectangle">the rectangle where the map content should be drawn.</param>
        public void Print(Graphics device, Rectangle targetRectangle)
        {
            MapFrame.Print(device, targetRectangle);
        }

        /// <summary>
        /// Instead of using the usual buffers, this bypasses any buffering and instructs the layers
        /// to draw directly to the specified target rectangle on the graphics object.  This is useful
        /// for doing vector drawing on much larger pages.  The result will be centered in the
        /// specified target rectangle bounds.
        /// </summary>
        /// <param name="device">The graphics device to print to</param>
        /// <param name="targetRectangle">the rectangle where the map content should be drawn.</param>
        /// <param name="targetEnvelope">the extents to print in the target rectangle</param>
        public void Print(Graphics device, Rectangle targetRectangle, Extent targetEnvelope)
        {
            MapFrame.Print(device, targetRectangle, targetEnvelope);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a boolean that indicates whether or not
        /// the drawing layers should cache off-screen data to
        /// the buffer.  Panning will be much more elegant,
        /// but zooming, selecting and resizing will take a
        /// performance penalty.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets a boolean that indicates whether or not zooming will also retrieve content immediately around the map so that panning pulls new content onto the screen.")]
        public bool ExtendBuffer
        {
            get { return MapFrame.ExtendBuffer; }
            set { MapFrame.ExtendBuffer = value; }
        }

        /// <summary>
        /// Gets or sets the Projection Esri string of the map. This property is used for serializing
        /// the projection string to the project file.
        /// </summary>
        [Serialize("ProjectionEsriString")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public string ProjectionEsriString
        {
            get
            {
                return Projection != null ? Projection.ToEsriString() : null;
            }
            set
            {
                if (Projection != null)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        Projection = null;
                    }
                    else
                    {
                        if (Projection.ToEsriString() != value)
                        {
                            Projection = ProjectionInfo.FromEsriString(value);
                            OnProjectionChanged();
                        }
                    }
                }
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
                if (_geoMapFrame != null) return MapFrame.ProjectionModeReproject;
                return ActionMode.Never;
            }
            set
            {
                if (_geoMapFrame != null) _geoMapFrame.ProjectionModeReproject = value;
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
                if (_geoMapFrame != null) return MapFrame.ProjectionModeDefine;
                return ActionMode.Never;
            }
            set
            {
                if (_geoMapFrame != null) _geoMapFrame.ProjectionModeDefine = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether layers should draw during the actual resize itself.  The
        /// normal behavior is to draw the existing image buffer in the new size and position which is much
        /// faster for large datasets, but is not as visually appealing if you only work with small datasets.
        /// </summary>
        public bool RedrawLayersWhileResizing { get; set; }

        /// <summary>
        /// Cursor hiding from designer
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
        /// Gets or sets a boolean indicating whether or not the sel
        /// </summary>
        public bool SelectionEnabled
        {
            get
            {
                if (MapFrame == null) return false;
                return MapFrame.SelectionEnabled;
            }
            set
            {
                if (MapFrame == null) return;
                MapFrame.SelectionEnabled = value;
            }
        }

        /// <summary>
        /// Instructs the map to clear the layers.
        /// </summary>
        public void ClearLayers()
        {
            if (MapFrame == null) return;
            MapFrame.Layers.Clear();
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether the Garbage collector should collect after drawing.
        /// This can be disabled for fast-action panning, but should normally be enabled.
        /// </summary>
        public bool CollectAfterDraw { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of tools built into this project
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<IMapFunction> MapFunctions { get; set; }

        /// <summary>
        /// Gets or sets the current tool mode.  This rapidly enables or disables specific tools to give
        /// a combination of functionality.  Selecting None will disable all the tools, which can be
        /// enabled manually by enabling the specific tool in the GeoTools dictionary.
        /// </summary>
        public FunctionMode FunctionMode
        {
            get { return _functionMode; }
            set
            {
                _functionMode = value;
                switch (_functionMode)
                {
                    case FunctionMode.ZoomIn:
                        try
                        {
                            MemoryStream ms = new MemoryStream(Images.cursorZoomIn);
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
                            MemoryStream ms = new MemoryStream(Images.cursorZoomOut);
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
                    case FunctionMode.None:
                        Cursor = Cursors.Arrow;
                        break;
                    case FunctionMode.Pan:
                        try
                        {
                            MemoryStream ms = new MemoryStream(Images.cursorHand);
                            Cursor = new Cursor(ms);
                        }
                        catch
                        {
                            Cursor = Cursors.SizeAll;
                            break;
                        }
                        break;

                    case FunctionMode.Select:
                        try
                        {
                            MemoryStream ms = new MemoryStream(Images.cursorSelect);
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
                    foreach (MapFunction f in MapFunctions)
                    {
                        if ((f.YieldStyle & YieldStyles.AlwaysOn) != YieldStyles.AlwaysOn) f.Deactivate();
                    }
                }
                else
                {
                    IMapFunction newMode = _functionLookup[_functionMode];
                    ActivateMapFunction(newMode);
                    // Except for function mode "none" allow scrolling
                    IMapFunction scroll = MapFunctions.Find(f => f.GetType() == typeof(MapFunctionZoom));
                    ActivateMapFunction(scroll);
                }

                //function mode changed event
                OnFunctionModeChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// If the specified function is already in the list of functions, this will properly test the yield style of various
        /// map functions that are currently on and then activate the function.  If this function is not in the list, then
        /// it will add it to the list.  If you need to control the position, then insert the function before using this
        /// method to activate.  Be warned that calling "Activate" directly on your function will activate your function
        /// but not disable any other functions.  You can set "Map.FunctionMode = FunctionModes.None" first, and then
        /// specifically activate the function that you want.
        /// </summary>
        /// <param name="function">The MapFunction to activate, or add.</param>
        public void ActivateMapFunction(IMapFunction function)
        {
            if (!MapFunctions.Contains(function))
            {
                MapFunctions.Add(function);
            }
            foreach (MapFunction f in MapFunctions)
            {
                if ((f.YieldStyle & YieldStyles.AlwaysOn) == YieldStyles.AlwaysOn) continue; // ignore "Always On" functions
                int test = (int)(f.YieldStyle & function.YieldStyle);
                if (test > 0) f.Deactivate(); // any overlap of behavior leads to deactivation
            }
            function.Activate();
        }

        /// <summary>
        /// Gets or sets a boolean that indicates whether a map-function is currently interacting with the map.
        /// If this is true, then any tool-tip like popups or other mechanisms that require lots of re-drawing
        /// should suspend themselves to prevent conflict.  Setting this actually increments an internal integer,
        /// so when that integer is 0, the map is "Not" busy, but multiple busy processess can work independantly.
        /// </summary>
        public bool IsBusy
        {
            get { return (_isBusyIndex > 0); }
            set
            {
                if (value) _isBusyIndex++;
                else _isBusyIndex--;
            }
        }

        /// <summary>
        /// Gets or sets the back buffer.  The back buffer should be in Format32bbpArgb bitmap.
        /// If it is not, then the image on the back buffer will be copied from the supplied image.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Image BufferedImage
        {
            get { return _geoMapFrame.BufferImage; }
            set { _geoMapFrame.BufferImage = value; }
        }

        /// <summary>
        /// If this is true, then point layers in the map will only draw points that are
        /// more than 50% revealed.  This should increase drawing speed for layers that have
        /// a large number of points.
        /// </summary>
        public bool CollisionDetection { get; set; }

        /// <summary>
        /// Gets the geographic bounds of all of the different data layers currently visible on the map.
        /// </summary>
        [Category("Bounds"),
         Description("Gets the geographic bounds of all of the different data layers currently visible on the map."),
         Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Extent Extent
        {
            get { return _geoMapFrame.Extent; }
        }

        /// <summary>
        /// Gets or sets the geographic extents to show in the view.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Extent ViewExtents
        {
            get { return _geoMapFrame.ViewExtents; }
            set
            {
                _geoMapFrame.ViewExtents = value;
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the MapFrame that should be displayed in this map.
        /// </summary>
        [Serialize("MapFrame"), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IMapFrame MapFrame
        {
            get { return _geoMapFrame; }
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
        /// Gets or sets the collection of layers
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IMapLayerCollection Layers
        {
            get
            {
                if (_geoMapFrame != null) return _geoMapFrame.Layers;
                return null;
            }
        }

        /// <summary>
        /// returns a functional list of the ILayer members.  This list will be
        /// separate from the actual list stored, but contains a shallow copy
        /// of the members, so the layers themselves can be accessed directly.
        /// </summary>
        /// <returns></returns>
        public List<ILayer> GetLayers()
        {
            List<ILayer> result = new List<ILayer>();
            if (_geoMapFrame == null) return result;
            return _geoMapFrame.Layers.Cast<ILayer>().ToList();
        }

        /// <summary>
        /// Gets or sets the projection.  This should reflect the projection of the first data layer loaded.
        /// Loading subsequent, but non-matching projections should throw an alert, and allow reprojection.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ProjectionInfo Projection
        {
            get
            {
                return _geoMapFrame != null ? _geoMapFrame.Projection : null;
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
        /// Gets or sets the legend to use when showing the layers from this map
        /// </summary>
        public ILegend Legend
        {
            get { return _legend; }
            set
            {
                _legend = value;
                if (_legend != null)
                {
                    _legend.AddMapFrame(_geoMapFrame);
                }
            }
        }

        IFrame IBasicMap.MapFrame
        {
            get { return _geoMapFrame; }
        }

        /// <summary>
        /// Gets or sets the progress handler for this component.
        /// </summary>
        public IProgressHandler ProgressHandler
        {
            get { return _progressHandler; }
            set
            {
                _progressHandler = value;
                _geoMapFrame.ProgressHandler = value;
            }
        }

        private void MapFrame_ViewExtentsChanged(object sender, ExtentArgs args)
        {
            OnViewExtentsChanged(sender, args);
        }

        /// <summary>
        /// Handles removing event handlers for the map frame
        /// </summary>
        /// <param name="mapFrame"></param>
        protected virtual void OnExcludeMapFrame(IMapFrame mapFrame)
        {
            if (mapFrame == null) return;
            mapFrame.UpdateMap -= MapFrameUpdateMap;
            mapFrame.ScreenUpdated -= MapFrameScreenUpdated;
            mapFrame.ItemChanged -= MapFrameItemChanged;
            mapFrame.BufferChanged -= MapFrame_BufferChanged;
            mapFrame.SelectionChanged -= MapFrame_SelectionChanged;
            mapFrame.LayerAdded -= MapFrame_LayerAdded;
            mapFrame.ViewExtentsChanged -= MapFrame_ViewExtentsChanged;
            if (Legend != null)
            {
                Legend.RemoveMapFrame(mapFrame, true);
            }
        }

        /// <summary>
        /// Handles adding new event handlers to the map frame
        /// </summary>
        /// <param name="mapFrame"></param>
        protected virtual void OnIncludeMapFrame(IMapFrame mapFrame)
        {
            if (mapFrame == null)
            {
                if (Legend == null) return;
                Legend.RefreshNodes();
                return;
            }
            mapFrame.Parent = this;
            mapFrame.UpdateMap += MapFrameUpdateMap;
            mapFrame.ScreenUpdated += MapFrameScreenUpdated;
            mapFrame.ItemChanged += MapFrameItemChanged;
            mapFrame.BufferChanged += MapFrame_BufferChanged;
            mapFrame.SelectionChanged += MapFrame_SelectionChanged;
            mapFrame.LayerAdded += MapFrame_LayerAdded;
            mapFrame.ViewExtentsChanged += MapFrame_ViewExtentsChanged;
            if (Legend == null) return;
            Legend.AddMapFrame(mapFrame);
        }

        private void MapFrame_LayerAdded(object sender, LayerEventArgs e)
        {
            OnLayerAdded(sender, e);
        }

        /// <summary>
        /// Fires the LayerAdded event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnLayerAdded(object sender, LayerEventArgs e)
        {
            if (LayerAdded != null) LayerAdded(sender, e);
        }

        private void MapFrame_SelectionChanged(object sender, EventArgs e)
        {
            OnSelectionChanged();
        }

        /// <summary>
        /// Occurs after the selection is updated on all the layers
        /// </summary>
        protected virtual void OnSelectionChanged()
        {
            if (SelectionChanged != null) SelectionChanged(this, new EventArgs());
        }

        private void MapFrame_BufferChanged(object sender, ClipArgs e)
        {
            Rectangle view = MapFrame.View;
            foreach (Rectangle clip in e.ClipRectangles)
            {
                if (clip.IsEmpty == false)
                {
                    Rectangle mapClip = new Rectangle(clip.X - view.X, clip.Y - view.Y, clip.Width, clip.Height);
                    Invalidate(mapClip);
                }
            }
        }

        /// <summary>
        /// Gets all layers of the map including layers which are nested
        /// within groups. The group objects themselves are not included in this list,
        /// but all FeatureLayers, RasterLayers, ImageLayers and other layers are included.
        /// </summary>
        /// <returns>The list of the layers</returns>
        public List<ILayer> GetAllLayers()
        {
            return _geoMapFrame != null ? _geoMapFrame.GetAllLayers() : null;
        }

        /// <summary>
        /// Gets all map groups in the map including the nested groups
        /// </summary>
        /// <returns>the list of the groups</returns>
        public List<IMapGroup> GetAllGroups()
        {
            return _geoMapFrame != null ? _geoMapFrame.GetAllGroups() : null;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// This causes all of the data layers to re-draw themselves to the buffer, rather than just drawing
        /// the buffer itself like what happens during "Invalidate"
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
        /// <param name="clipRectangle"></param>
        public void RefreshMap(Rectangle clipRectangle)
        {
            Extent region = _geoMapFrame.BufferToProj(clipRectangle);
            _geoMapFrame.Invalidate(region);
        }

        /// <summary>
        /// Converts a single point location into an equivalent geographic coordinate
        /// </summary>
        /// <param name="position">The client coordinate relative to the map control</param>
        /// <returns>The geographic ICoordinate interface</returns>
        public Coordinate PixelToProj(Point position)
        {
            return _geoMapFrame.PixelToProj(position);
        }

        /// <summary>
        /// Converts a rectangle in pixel coordinates relative to the map control into
        /// a geographic envelope.
        /// </summary>
        /// <param name="rect">The rectangle to convert</param>
        /// <returns>An IEnvelope interface</returns>
        public Extent PixelToProj(Rectangle rect)
        {
            return _geoMapFrame.PixelToProj(rect);
        }

        /// <summary>
        /// Converts a single geographic location into the equivalent point on the
        /// screen relative to the top left corner of the map.
        /// </summary>
        /// <param name="location">The geographic position to transform</param>
        /// <returns>A Point with the new location.</returns>
        public Point ProjToPixel(Coordinate location)
        {
            return _geoMapFrame.ProjToPixel(location);
        }

        /// <summary>
        /// Converts a single geographic envelope into an equivalent Rectangle
        /// as it would be drawn on the screen.
        /// </summary>
        /// <param name="env">The geographic IEnvelope</param>
        /// <returns>A Rectangle</returns>
        public Rectangle ProjToPixel(Extent env)
        {
            return _geoMapFrame.ProjToPixel(env);
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
        /// Zooms to the next extent
        /// </summary>
        public void ZoomToNext()
        {
            MapFrame.ZoomToNext();
        }

        /// <summary>
        /// Zooms to the previous extent
        /// </summary>
        public void ZoomToPrevious()
        {
            MapFrame.ZoomToPrevious();
        }

        /// <summary>
        /// Occurs when this control tries to paint the background.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            //This is done deliberately to prevent flicker.
            //base.OnPaintBackground(e);
        }

        private void GetResizeExtent(Extent newEnv)
        {
            // ---------- Aspect Ratio Handling
            if (newEnv == null) return;
            double h = Height;
            double w = Width;
            // It isn't exactly an exception, but rather just an indication not to do anything here.
            if (h == 0 || w == 0) return;

            double controlAspect = w / h;
            double envelopeAspect = newEnv.Width / newEnv.Height;
            Coordinate center = newEnv.ToEnvelope().Center();

            if (controlAspect > envelopeAspect)
            {
                // The Control is proportionally wider than the envelope to display.
                // If the envelope is proportionately wider than the control, "reveal" more width without
                // changing height If the envelope is proportionately taller than the control,
                // "hide" width without changing height
                newEnv.SetCenter(center, newEnv.Height * controlAspect, newEnv.Height);
            }
            else
            {
                // The control is proportionally taller than the content is
                // If the envelope is proportionately wider than the control,
                // "hide" the extra height without changing width
                // If the envelope is proportionately taller than the control, "reveal" more height without changing width
                newEnv.SetCenter(center, newEnv.Width, newEnv.Width / controlAspect);
            }

            return;
        }

        /// <summary>
        /// Perform custom drawing
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_geoMapFrame.IsPanning) return;

            Rectangle clip = e.ClipRectangle;
            if (clip.IsEmpty || _isResizing) clip = ClientRectangle;

            // if the area to paint is too small, there's nothing to paint.
            // Added to fix http://dotspatial.codeplex.com/workitem/320
            if (clip.Width < 1 || clip.Height < 1) return;

            Bitmap stencil = new Bitmap(clip.Width, clip.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(stencil);
            Brush b = new SolidBrush(BackColor);
            g.FillRectangle(b, new Rectangle(0, 0, stencil.Width, stencil.Height));
            b.Dispose();
            Matrix m = new Matrix();
            m.Translate(-clip.X, -clip.Y);
            g.Transform = m;

            _geoMapFrame.Draw(new PaintEventArgs(g, e.ClipRectangle));

            MapDrawArgs args = new MapDrawArgs(g, clip, _geoMapFrame);
            foreach (IMapFunction tool in MapFunctions)
            {
                if (tool.Enabled) tool.Draw(args);
            }

            PaintEventArgs pe = new PaintEventArgs(g, e.ClipRectangle);
            base.OnPaint(pe);

            g.Dispose();

            if (_isResizing)
            {
                Rectangle v = _geoMapFrame.View;
                Extent target = ViewExtents.Copy();
                GetResizeExtent(target);
                Rectangle nV = _geoMapFrame.ProjToPixel(target);
                const int sourceX = 0;
                const int sourceY = 0;
                int sourceHeight = Height;
                int sourceWidth = Width;
                int destX = 0;
                int destY = 0;
                int destWidth = Width;
                int destHeight = Height;
                if (nV.Height > v.Height)
                {
                    double yRat = nV.Height / (double)v.Height;
                    destHeight = Convert.ToInt32(Height / yRat);
                    destY = (Height - destHeight) / 2;
                }
                else
                {
                    double xRat = nV.Width / (double)v.Width;
                    destWidth = Convert.ToInt32(Width / xRat);
                    destX = (Width - destWidth) / 2;
                }
                using (Bitmap bmp = new Bitmap(Width, Height))
                {
                    using (Graphics gg = Graphics.FromImage(bmp))
                    {
                        using (Brush bb = new SolidBrush(BackColor))
                        {
                            gg.FillRectangle(bb, 0, 0, Width, Height);
                        }

                        Rectangle source = new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);
                        Rectangle dest = new Rectangle(destX, destY, destWidth, destHeight);
                        gg.DrawImage(stencil, dest, source, GraphicsUnit.Pixel);
                    }

                    e.Graphics.DrawImageUnscaled(bmp, 0, 0);
                }
            }
            else
            {
                e.Graphics.DrawImageUnscaled(stencil, clip.X, clip.Y);
            }

            stencil.Dispose();
        }

        /// <summary>
        /// Captures an image of whatever the contents of the back buffer would be at the size of the screen.
        /// </summary>
        /// <returns></returns>
        public Bitmap SnapShot()
        {
            Rectangle clip = ClientRectangle;
            Bitmap stencil = new Bitmap(clip.Width, clip.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(stencil);
            g.FillRectangle(Brushes.White, new Rectangle(0, 0, stencil.Width, stencil.Height));

            // Translate the buffer so that drawing occurs in client coordinates, regardless of whether
            // there is a clip rectangle or not.
            Matrix m = new Matrix();
            m.Translate(-clip.X, -clip.Y);
            g.Transform = m;

            _geoMapFrame.Draw(new PaintEventArgs(g, clip));
            g.Dispose();
            return stencil;
        }

        /// <summary>
        /// Creates a snapshot that is scaled to fit to a bitmap of the specified width.
        /// </summary>
        /// <param name="width">The width of the desired bitmap</param>
        /// <returns>A bitmap with the specified width</returns>
        public Bitmap SnapShot(int width)
        {
            int height = (int)(width * (MapFrame.ViewExtents.Height / MapFrame.ViewExtents.Width));
            Bitmap bmp = new Bitmap(height, width);
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            _geoMapFrame.Print(g, new Rectangle(0, 0, width, height));
            g.Dispose();
            return bmp;
        }

        /// <summary>
        /// Fires the DoMouseDoubleClick method on the ActiveTools
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            GeoMouseArgs args = new GeoMouseArgs(e, this);
            foreach (IMapFunction tool in MapFunctions)
            {
                if (tool.Enabled)
                {
                    tool.DoMouseDoubleClick(args);
                    if (args.Handled) break;
                }
            }

            base.OnMouseDoubleClick(e);
        }

        /// <summary>
        /// Fires the OnMouseDown event on the Active Tools
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            GeoMouseArgs args = new GeoMouseArgs(e, this);
            foreach (IMapFunction tool in MapFunctions)
            {
                if (tool.Enabled)
                {
                    tool.DoMouseDown(args);
                    if (args.Handled) break;
                }
            }

            base.OnMouseDown(e);
        }

        /// <summary>
        /// Fires the OnMouseUp event on the Active Tools
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            GeoMouseArgs args = new GeoMouseArgs(e, this);

            foreach (IMapFunction tool in MapFunctions)
            {
                if (tool.Enabled)
                {
                    tool.DoMouseUp(args);
                    if (args.Handled) break;
                }
            }
            base.OnMouseUp(e);
        }

        /// <summary>
        /// Fires the OnMouseMove event on the Active Tools
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            GeoMouseArgs args = new GeoMouseArgs(e, this);
            foreach (IMapFunction tool in MapFunctions)
            {
                if (tool.Enabled)
                {
                    tool.DoMouseMove(args);
                    if (args.Handled) break;
                }
            }
            if (GeoMouseMove != null)
            {
                GeoMouseMove(this, new GeoMouseArgs(e, this));
            }
            base.OnMouseMove(e);
        }

        /// <summary>
        /// Fires the OnMouseWheel event for the active tools
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            GeoMouseArgs args = new GeoMouseArgs(e, this);
            foreach (IMapFunction tool in MapFunctions)
            {
                if (tool.Enabled)
                {
                    tool.DoMouseWheel(args);
                    if (args.Handled) break;
                }
            }
            base.OnMouseWheel(e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnFinishedRefresh(EventArgs e)
        {
            if (FinishedRefresh != null)
            {
                FinishedRefresh(this, e);
            }
        }

        /// <summary>
        /// Fires the ProjectionChanged event
        /// and also reprojects all map layers
        /// </summary>
        protected virtual void OnProjectionChanged()
        {
            if (ProjectionChanged != null) ProjectionChanged(this, null);
        }

        /// <summary>
        /// Fires the ViewExtentsChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        protected virtual void OnViewExtentsChanged(object sender, ExtentArgs args)
        {
            if (ViewExtentsChanged != null) ViewExtentsChanged(sender, args);
        }

        /// <summary>
        /// Fires the FunctionModeChanged event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnFunctionModeChanged(object sender, EventArgs e)
        {
            if (FunctionModeChanged != null) FunctionModeChanged(this, e);
        }

        /// <summary>
        /// Handles resize of the form.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnResize(EventArgs e)
        {
            if (RedrawLayersWhileResizing)
            {
                // Redraw everything any time the size changes.
                if (_geoMapFrame != null)
                {
                    _geoMapFrame.ResetExtents();
                }
            }
            else
            {
                if (!_parentFormSizeChanging)
                {
                    // This is called after a splitter drop, or after maximize minimize changes.
                    if (_geoMapFrame != null)
                    {
                        _geoMapFrame.ResetExtents();
                    }
                }
            }
            Invalidate();
            base.OnResize(e);
        }

        /// <summary>
        /// Occurs after this object has been resized.
        /// </summary>
        protected virtual void OnResized()
        {
            if (Resized != null) Resized(this, EventArgs.Empty);
        }

        #endregion
    }
}