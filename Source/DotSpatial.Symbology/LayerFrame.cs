// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using DotSpatial.Data;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This is a class that organizes a list of renderable layers into a single "view" which might be
    /// shared by multiple displays.  For instance, if you have a map control and a print preview control,
    /// you could draw the same data frame property on both, just by passing the graphics object for each.
    /// Be sure to handle any scaling or translation that you require through the Transform property
    /// of the graphics object as it will ultimately be that scale which is used to back-calculate the
    /// appropriate pixel sizes for point-size, line-width and other non-georeferenced characteristics.
    /// </summary>
    public abstract class LayerFrame : Group, IFrame
    {
        #region Events

        /// <summary>
        /// Occurs when the visible region being displayed on the map should update
        /// </summary>
        public event EventHandler UpdateMap;

        /// <summary>
        /// Occurs after zooming to a specific location on the map and causes a camera recent.
        /// </summary>
        public event EventHandler<ExtentArgs> ViewExtentsChanged;

        #endregion

        #region Private Variables

        readonly IContainer _parent;
        private List<ILayer> _drawingLayers;
        private int _extentChangedSuspensionCount;
        private bool _extentsChanged;

        [Serialize("ExtentsInitialized")]
        private bool _extentsInitialized;

        private SmoothingMode _smoothingMode = SmoothingMode.AntiAlias;
        [Serialize("ViewExtents")]
        private Extent _viewExtents;

        #endregion

        #region Constructors

        /// <summary>
        /// The Constructor for the MapFrame object
        /// </summary>
        protected LayerFrame()
        {
            Configure();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="container"></param>
        protected LayerFrame(IContainer container)
        {
            _parent = container;
            Configure();
        }

        /// <summary>
        /// Drawing layers are tracked separately, and do not appear in the legend.
        /// </summary>
        public List<ILayer> DrawingLayers
        {
            get { return _drawingLayers; }
            set { _drawingLayers = value; }
        }

        /// <summary>
        /// Draws the layers icon to the legend
        /// </summary>
        /// <param name="g"></param>
        /// <param name="box"></param>
        public override void LegendSymbol_Painted(Graphics g, Rectangle box)
        {
            g.DrawIcon(SymbologyImages.Layers, box);
        }

        /// <summary>
        /// Overrides the default behavior for groups, which should return null in the
        /// event that they have no layers, with a more tolerant, getting started
        /// behavior where geographic coordinates are assumed.
        /// </summary>
        public override Extent Extent
        {
            get
            {
                if (base.Extent == null)
                {
                    return new Extent(-180, -90, 180, 90);
                }
                return base.Extent;
            }
        }

        private void Configure()
        {
            Layers = new LayerCollection(this);
            LegendText = SymbologyMessageStrings.LayerFrame_Map_Layers;
            ContextMenuItems = new List<SymbologyMenuItem>
            {
                new SymbologyMenuItem(SymbologyMessageStrings.LayerFrame_ZoomToMapFrame, ZoomToMapFrame_Click),
                new SymbologyMenuItem(SymbologyMessageStrings.LayerFrame_CreateGroup, CreateGroup_Click)
            };

            LegendSymbolMode = SymbolMode.GroupSymbol;
            LegendType = LegendType.Group;
            MapFrame = this;
            ParentGroup = this;
            _drawingLayers = new List<ILayer>();
        }

        private void CreateGroup_Click(object sender, EventArgs e)
        {
            OnCreateGroup();
        }

        /// <summary>
        /// Suspends firing the ExtentsChanges event.  It only fires if all suspension is gone.
        /// </summary>
        protected void SuspendExtentChanged()
        {
            if (_extentChangedSuspensionCount == 0) _extentsChanged = false;
            _extentChangedSuspensionCount++;
        }

        /// <summary>
        /// Resumes firing the method, and will fire it automatically if any changes have occurred.
        /// </summary>
        protected void ResumeExtentChanged()
        {
            _extentChangedSuspensionCount--;
            if (_extentChangedSuspensionCount == 0)
            {
                if (_extentsChanged)
                {
                    OnExtentsChanged(_viewExtents);
                }
            }
        }

        private void ZoomToMapFrame_Click(object sender, EventArgs e)
        {
            // work item #42
            // to prevent exception when zoom to map with one layer with one point
            const double eps = 1e-7;
            var maxExtent = Extent.Width < eps || Extent.Height < eps
                ? new Extent(Extent.MinX - eps, Extent.MinY - eps, Extent.MaxX + eps, Extent.MaxY + eps)
                : Extent;
            maxExtent.ExpandBy(maxExtent.Width / 10, maxExtent.Height / 10); // work item #84

            ViewExtents = maxExtent;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the container control that this MapFrame belongs to.
        /// </summary>
        public IContainer Parent
        {
            get { return _parent; }
        }

        /// <summary>
        /// This will create a new layer from the featureset and add it.
        /// </summary>
        /// <param name="featureSet">Any valid IFeatureSet that does not yet have drawing characteristics</param>
        public virtual void Add(IFeatureSet featureSet)
        {
            // this should be overridden in subclasses
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public bool ExtentsInitialized
        {
            get { return _extentsInitialized; }
            set { _extentsInitialized = value; }
        }

        /// <summary>
        /// If this is false, then the drawing function will not render anything.
        /// Warning!  This will also prevent any execution of calculations that take place
        /// as part of the drawing methods and will also abort the drawing methods of any
        /// sub-members to this IRenderable.
        /// </summary>
        public override bool IsVisible
        {
            get { return base.IsVisible; }
            set
            {
                if (base.IsVisible != value)
                {
                    // switching values
                    base.IsVisible = value;
                    OnItemChanged();
                }
            }
        }

        /// <summary>
        /// Gets the currently selected layer.  This will be an active layer that is used for operations.
        /// </summary>
        public ILayer SelectedLayer
        {
            get
            {
                if (Layers != null)
                {
                    return Layers.SelectedLayer;
                }
                return null;
            }
        }

        /// <summary>
        /// Controls the smoothing mode.  Default or None will have faster performance
        /// at the cost of quality.
        /// </summary>
        public SmoothingMode SmoothingMode
        {
            get { return _smoothingMode; }
            set { _smoothingMode = value; }
        }

        /// <summary>
        /// This should be the geographic extents visible in the map.
        /// </summary>
        public virtual Extent ViewExtents
        {
            get
            {
                return _viewExtents ?? (_viewExtents = Extent != null ? Extent.Copy() : new Extent(-180, -90, 180, 90));
            }
            set
            {
                _viewExtents = value;
                _extentsChanged = true;
                if (_extentChangedSuspensionCount == 0)
                {
                    OnExtentsChanged(_viewExtents);
                }
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// This adjusts the extents when ZoomToLayer is called in one of the internal layers.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Layers_ZoomToLayer(object sender, EnvelopeArgs e)
        {
            ViewExtents = e.Envelope.ToExtent();
        }

        /// <summary>
        /// Zooms to the envelope if no envelope has been established for this frame.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Layers_LayerAdded(object sender, LayerEventArgs e)
        {
            if (ExtentsInitialized) return;
            ExtentsInitialized = true;
            if (e.Layer != null)
            {
                ViewExtents = e.Layer.Extent.Copy();
                ViewExtents.ExpandBy(e.Layer.Extent.Width / 10, e.Layer.Extent.Height / 10);
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Fires the ExtentsChanged event
        /// </summary>
        /// <param name="ext"></param>
        protected virtual void OnExtentsChanged(Extent ext)
        {
            if (_extentChangedSuspensionCount > 0)
            {
                return;
            }
            if (ViewExtentsChanged != null) ViewExtentsChanged(this, new ExtentArgs(ext));
        }

        /// <summary>
        /// Fires the ExtentsChanged event
        /// </summary>
        protected virtual void OnUpdateMap()
        {
            if (UpdateMap != null) UpdateMap(this, EventArgs.Empty);
        }

        /// <summary>
        /// This is responsible for wiring the ZoomToLayer event from any layers
        /// in the map frame whenever the layer collection is changed.
        /// </summary>
        /// <param name="collection"></param>
        protected override void Handle_Layer_Events(ILayerEvents collection)
        {
            if (collection == null) return;
            collection.ZoomToLayer += Layers_ZoomToLayer;
            collection.LayerAdded += Layers_LayerAdded;
            collection.LayerRemoved += Layers_LayerRemoved;
            base.Handle_Layer_Events(collection);
        }

        /// <summary>
        /// This is responsible for unwiring the ZoomToLayer event.
        /// </summary>
        /// <param name="collection"></param>
        protected override void Ignore_Layer_Events(ILayerEvents collection)
        {
            if (collection == null) return;
            collection.ZoomToLayer -= Layers_ZoomToLayer;
            collection.LayerAdded -= Layers_LayerAdded;
            collection.LayerRemoved -= Layers_LayerRemoved;
            base.Ignore_Layer_Events(collection);
        }

        private void Layers_LayerRemoved(object sender, LayerEventArgs e)
        {
            if (GetLayerCount(true) == 0)
            {
                ExtentsInitialized = false;
            }
        }

        #endregion
    }
}