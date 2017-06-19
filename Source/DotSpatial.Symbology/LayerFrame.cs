// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
    /// shared by multiple displays. For instance, if you have a map control and a print preview control,
    /// you could draw the same data frame property on both, just by passing the graphics object for each.
    /// Be sure to handle any scaling or translation that you require through the Transform property
    /// of the graphics object as it will ultimately be that scale which is used to back-calculate the
    /// appropriate pixel sizes for point-size, line-width and other non-georeferenced characteristics.
    /// </summary>
    public abstract class LayerFrame : Group, IFrame
    {
        #region Fields

        private int _extentChangedSuspensionCount;
        private bool _extentsChanged;

        [Serialize("ViewExtents")]
        private Extent _viewExtents;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerFrame"/> class.
        /// </summary>
        protected LayerFrame()
        {
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerFrame"/> class.
        /// </summary>
        /// <param name="container">The parent of this frame.</param>
        protected LayerFrame(IContainer container)
        {
            Parent = container;
            Configure();
        }

        #endregion

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

        #region Properties

        /// <summary>
        /// Gets or sets the drawing layers. Drawing layers are tracked separately, and do not appear in the legend.
        /// </summary>
        public List<ILayer> DrawingLayers { get; set; }

        /// <summary>
        /// Gets the extent. Overrides the default behavior for groups, which should return null in the
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

        /// <inheritdoc />
        [Serialize("ExtentsInitialized")]
        public bool ExtentsInitialized { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the drawing function will render anything.
        /// Warning! If false this will also prevent any execution of calculations that take place
        /// as part of the drawing methods and will also abort the drawing methods of any
        /// sub-members to this IRenderable.
        /// </summary>
        public override bool IsVisible
        {
            get
            {
                return base.IsVisible;
            }

            set
            {
                if (base.IsVisible == value) return;

                // switching values
                base.IsVisible = value;
                OnItemChanged();
            }
        }

        /// <summary>
        /// Gets the container control that this MapFrame belongs to.
        /// </summary>
        public IContainer Parent { get; }

        /// <summary>
        /// Gets the currently selected layer. This will be an active layer that is used for operations.
        /// </summary>
        public ILayer SelectedLayer => Layers?.SelectedLayer;

        /// <summary>
        /// Gets or sets the smoothing mode. Default or None will have faster performance
        /// at the cost of quality.
        /// </summary>
        public SmoothingMode SmoothingMode { get; set; } = SmoothingMode.AntiAlias;

        /// <summary>
        /// Gets or sets the geographic extents visible in the map.
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

        #region Methods

        /// <summary>
        /// This will create a new layer from the featureset and add it.
        /// </summary>
        /// <param name="featureSet">Any valid IFeatureSet that does not yet have drawing characteristics</param>
        public virtual void Add(IFeatureSet featureSet)
        {
            // this should be overridden in subclasses
        }

        /// <summary>
        /// Draws the layers icon to the legend
        /// </summary>
        /// <param name="g">Graphics object used for drawing.</param>
        /// <param name="box">Rectangle used for drawing.</param>
        public override void LegendSymbolPainted(Graphics g, Rectangle box)
        {
            g.DrawIcon(SymbologyImages.Layers, box);
        }

        /// <summary>
        /// This is responsible for wiring the ZoomToLayer event from any layers
        /// in the map frame whenever the layer collection is changed.
        /// </summary>
        /// <param name="collection">Collection the events get wired to.</param>
        protected override void HandleLayerEvents(ILayerEvents collection)
        {
            if (collection == null) return;
            collection.ZoomToLayer += LayersZoomToLayer;
            collection.LayerAdded += LayersLayerAdded;
            collection.LayerRemoved += LayersLayerRemoved;
            base.HandleLayerEvents(collection);
        }

        /// <summary>
        /// This is responsible for unwiring the ZoomToLayer event.
        /// </summary>
        /// <param name="collection">Collection the events get unwired from.</param>
        protected override void IgnoreLayerEvents(ILayerEvents collection)
        {
            if (collection == null) return;
            collection.ZoomToLayer -= LayersZoomToLayer;
            collection.LayerAdded -= LayersLayerAdded;
            collection.LayerRemoved -= LayersLayerRemoved;
            base.IgnoreLayerEvents(collection);
        }

        /// <summary>
        /// Zooms to the envelope if no envelope has been established for this frame.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        protected virtual void LayersLayerAdded(object sender, LayerEventArgs e)
        {
            if (ExtentsInitialized) return;
            ExtentsInitialized = true;
            if (e.Layer != null)
            {
                ViewExtents = e.Layer.Extent.Copy();
                ViewExtents.ExpandBy(e.Layer.Extent.Width / 10, e.Layer.Extent.Height / 10);
            }
        }

        /// <summary>
        /// This adjusts the extents when ZoomToLayer is called in one of the internal layers.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        protected virtual void LayersZoomToLayer(object sender, EnvelopeArgs e)
        {
            ViewExtents = e.Envelope.ToExtent();
        }

        /// <summary>
        /// Fires the ExtentsChanged event
        /// </summary>
        /// <param name="ext">The new extent.</param>
        protected virtual void OnExtentsChanged(Extent ext)
        {
            if (_extentChangedSuspensionCount > 0) return;
            ViewExtentsChanged?.Invoke(this, new ExtentArgs(ext));
        }

        /// <summary>
        /// Fires the ExtentsChanged event
        /// </summary>
        protected virtual void OnUpdateMap()
        {
            UpdateMap?.Invoke(this, EventArgs.Empty);
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

        /// <summary>
        /// Suspends firing the ExtentsChanges event. It only fires if all suspension is gone.
        /// </summary>
        protected void SuspendExtentChanged()
        {
            if (_extentChangedSuspensionCount == 0) _extentsChanged = false;
            _extentChangedSuspensionCount++;
        }

        private void Configure()
        {
            Layers = new LayerCollection(this);
            LegendText = SymbologyMessageStrings.LayerFrame_Map_Layers;
            ContextMenuItems = new List<SymbologyMenuItem>
                               {
                                   new SymbologyMenuItem(SymbologyMessageStrings.LayerFrame_ZoomToMapFrame, ZoomToMapFrameClick),
                                   new SymbologyMenuItem(SymbologyMessageStrings.LayerFrame_CreateGroup, CreateGroupClick)
                               };

            LegendSymbolMode = SymbolMode.GroupSymbol;
            LegendType = LegendType.Group;
            MapFrame = this;
            ParentGroup = this;
            DrawingLayers = new List<ILayer>();
        }

        private void CreateGroupClick(object sender, EventArgs e)
        {
            OnCreateGroup();
        }

        private void LayersLayerRemoved(object sender, LayerEventArgs e)
        {
            if (GetLayerCount(true) == 0)
            {
                ExtentsInitialized = false;
            }
        }

        private void ZoomToMapFrameClick(object sender, EventArgs e)
        {
            // work item #42
            // to prevent exception when zoom to map with one layer with one point
            const double Eps = 1e-7;
            var maxExtent = Extent.Width < Eps || Extent.Height < Eps ? new Extent(Extent.MinX - Eps, Extent.MinY - Eps, Extent.MaxX + Eps, Extent.MaxY + Eps) : Extent;
            maxExtent.ExpandBy(maxExtent.Width / 10, maxExtent.Height / 10); // work item #84

            ViewExtents = maxExtent;
        }

        #endregion
    }
}