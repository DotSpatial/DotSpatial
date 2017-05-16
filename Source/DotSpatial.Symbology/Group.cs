// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DotSpatial.Data;
using GeoAPI.Geometries;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Group of layers.
    /// </summary>
    public class Group : Layer, IGroup
    {
        #region Fields

        private ILayerCollection _layers;
        private bool _selectionEnabled;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        public Group()
        {
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class that sits in a layer list and uses the specified progress handler.
        /// </summary>
        /// <param name="frame">The map frame the group belongs to.</param>
        /// <param name="progressHandler">the progress handler</param>
        public Group(IFrame frame, IProgressHandler progressHandler)
            : base(progressHandler)
        {
            MapFrame = frame;
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class that sits in a layer list and uses the specified progress handler.
        /// </summary>
        /// <param name="container">the layer list</param>
        /// <param name="frame">The map frame the group belongs to.</param>
        /// <param name="progressHandler">the progress handler</param>
        public Group(ICollection<ILayer> container, IFrame frame, IProgressHandler progressHandler)
            : base(container, progressHandler)
        {
            MapFrame = frame;
            Configure();
        }

        #endregion

        #region Events

        /// <summary>
        /// This occurs when a new layer is added either to this group, or one of the child groups within this group.
        /// </summary>
        public event EventHandler<LayerEventArgs> LayerAdded;

        /// <summary>
        /// This occurs when a layer is removed from this group.
        /// </summary>
        public event EventHandler<LayerEventArgs> LayerRemoved;

        #endregion

        #region Properties

        /// <inheritdoc />
        public virtual int Count => _layers.Count;

        /// <inheritdoc />
        public virtual bool EventsSuspended => _layers.EventsSuspended;

        /// <summary>
        /// Gets the envelope that contains all of the layers for this data frame. Essentially this would be
        /// the extents to use if you want to zoom to the world view.
        /// </summary>
        public override Extent Extent
        {
            get
            {
                var layers = GetLayers();
                if (layers == null) return null;

                Extent ext = null;

                // changed by jany (2015-07-17) don't add extents of empty layers, because they cause a wrong overall extent
                foreach (var extent in layers.Select(layer => layer.Extent).Where(extent => extent != null && !extent.IsEmpty()))
                {
                    if (ext == null)
                        ext = (Extent)extent.Clone();
                    else
                        ext.ExpandToInclude(extent);
                }

                return ext;
            }
        }

        /// <summary>
        /// Gets or sets the integer handle for this group.
        /// </summary>
        public int Handle { get; protected set; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        public Image Icon { get; set; }

        /// <summary>
        /// Gets the currently invalidated region as a union of all the
        /// invalidated regions of individual layers in this group.
        /// </summary>
        public override Extent InvalidRegion
        {
            get
            {
                Extent result = new Extent();
                foreach (ILayer lyr in GetLayers())
                {
                    if (lyr.InvalidRegion != null) result.ExpandToInclude(lyr.InvalidRegion);
                }

                return result;
            }
        }

        /// <inheritdoc />
        public virtual bool IsReadOnly => _layers.IsReadOnly;

        /// <summary>
        /// Gets or sets a value indicating whether any sub-layers in the group are visible. Setting this
        /// will force all the layers in this group to become visible.
        /// </summary>
        public override bool IsVisible
        {
            get
            {
                return GetLayers().Any(lyr => lyr.IsVisible);
            }

            set
            {
                foreach (var lyr in GetLayers())
                {
                    lyr.IsVisible = value;
                }

                base.IsVisible = value;
            }
        }

        /// <summary>
        /// Gets the count of the layers that are currently stored in this map frame.
        /// </summary>
        public int LayerCount => _layers.Count;

        /// <summary>
        /// Gets or sets a value indicating whether any of the immediate layers or groups contained within this
        /// control are visible. Setting this will set the visibility for each of the members of this
        /// map frame.
        /// </summary>
        public bool LayersVisible
        {
            get
            {
                return GetLayers().Any(layer => layer.IsVisible);
            }

            set
            {
                foreach (var layer in GetLayers())
                {
                    layer.IsVisible = value;
                }
            }
        }

        /// <summary>
        /// Gets the layers cast as legend items. This allows easier cycling in recursive legend code.
        /// </summary>
        public override IEnumerable<ILegendItem> LegendItems => _layers;

        /// <summary>
        /// Gets or sets the parent group of this group.
        /// </summary>
        public IGroup ParentGroup { get; protected set; }

        /// <summary>
        /// Gets or sets the progress handler to use. Setting this will set the progress handler for each of the layers in this map frame.
        /// </summary>
        public override IProgressHandler ProgressHandler
        {
            get
            {
                return base.ProgressHandler;
            }

            set
            {
                base.ProgressHandler = value;
                foreach (ILayer layer in GetLayers())
                {
                    layer.ProgressHandler = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the groups state is locked. This prevents the user from changing the visual state
        /// except layer by layer.
        /// </summary>
        public bool StateLocked { get; set; }

        /// <summary>
        /// gets or sets the list of layers.
        /// </summary>
        [ShallowCopy]
        protected ILayerCollection Layers
        {
            get
            {
                return _layers;
            }

            set
            {
                if (Layers != null)
                {
                    IgnoreLayerEvents(_layers);
                }

                _layers = value;
                if (_layers != null)
                {
                    HandleLayerEvents(value);
                    _layers.ParentGroup = this;
                    _layers.MapFrame = MapFrame;
                }
            }
        }

        #endregion

        #region Indexers

        /// <inheritdoc />
        public virtual ILayer this[int index]
        {
            get
            {
                return _layers[index];
            }

            set
            {
                _layers[index] = value;
            }
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public virtual void Add(ILayer layer)
        {
            _layers.Add(layer);
        }

        /// <inheritdoc />
        public override bool CanReceiveItem(ILegendItem item)
        {
            ILayer lyr = item as ILayer;
            if (lyr != null)
            {
                if (lyr != this) return true; // don't allow groups to add to themselves
            }

            return false;
        }

        /// <inheritdoc />
        public virtual void Clear()
        {
            _layers.Clear();
        }

        /// <inheritdoc />
        public override bool ClearSelection(out Envelope affectedAreas)
        {
            affectedAreas = new Envelope();
            bool changed = false;
            if (!_selectionEnabled) return false;
            MapFrame.SuspendEvents();
            foreach (ILayer layer in GetLayers())
            {
                Envelope layerArea;
                if (layer.ClearSelection(out layerArea))
                {
                    changed = true;
                    affectedAreas.ExpandToInclude(layerArea);
                }
            }

            MapFrame.ResumeEvents();

            OnSelectionChanged(); // fires only AFTER the individual layers have fired their events.

            return changed;
        }

        /// <inheritdoc />
        public virtual bool Contains(ILayer item)
        {
            return _layers.Contains(item);
        }

        /// <inheritdoc />
        public virtual void CopyTo(ILayer[] array, int arrayIndex)
        {
            _layers.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public virtual IEnumerator<ILayer> GetEnumerator()
        {
            return _layers.GetEnumerator();
        }

        /// <inheritdoc />
        public int GetLayerCount(bool recursive)
        {
            // if this is not overridden, this just looks at the Layers collection
            int count = 0;
            IList<ILayer> layers = GetLayers();
            foreach (ILayer item in layers)
            {
                IGroup grp = item as IGroup;
                if (grp != null)
                {
                    if (recursive) count += grp.GetLayerCount(true);
                }
                else
                {
                    count++;
                }
            }

            return count;
        }

        /// <inheritdoc />
        public virtual IList<ILayer> GetLayers()
        {
            return _layers.ToList();
        }

        /// <inheritdoc />
        public override Size GetLegendSymbolSize()
        {
            return new Size(16, 16);
        }

        /// <inheritdoc />
        public virtual int IndexOf(ILayer item)
        {
            return _layers.IndexOf(item);
        }

        /// <inheritdoc />
        public virtual void Insert(int index, ILayer layer)
        {
            _layers.Insert(index, layer);
        }

        /// <inheritdoc />
        public override void Invalidate(Extent region)
        {
            foreach (ILayer layer in GetLayers())
            {
                layer.Invalidate(region);
            }
        }

        /// <inheritdoc />
        public override void Invalidate()
        {
            foreach (ILayer layer in GetLayers())
            {
                layer.Invalidate();
            }
        }

        /// <inheritdoc />
        public override bool InvertSelection(Envelope tolerant, Envelope strict, SelectionMode mode, out Envelope affectedArea)
        {
            affectedArea = new Envelope();
            if (!_selectionEnabled) return false;
            bool somethingChanged = false;
            MapFrame.SuspendEvents();
            foreach (ILayer s in GetLayers())
            {
                if (s.SelectionEnabled == false) continue;
                Envelope layerArea;
                if (s.InvertSelection(tolerant, strict, mode, out layerArea))
                {
                    somethingChanged = true;
                    affectedArea.ExpandToInclude(layerArea);
                }
            }

            MapFrame.ResumeEvents();
            OnSelectionChanged(); // fires only AFTER the individual layers have fired their events.
            return somethingChanged;
        }

        /// <summary>
        /// Gets the layer handle of the specified layer
        /// </summary>
        /// <param name="positionInGroup">0 based index into list of layers</param>
        /// <returns>Layer's handle on success, -1 on failure</returns>
        public int LayerHandle(int positionInGroup)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public Bitmap LegendSnapShot(int imgWidth)
        {
            // bool TO_DO_GROUP_LEGEND_SNAPSHOT;
            // return new Bitmap(100, 100);
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual bool Remove(ILayer layer)
        {
            return _layers.Remove(layer);
        }

        /// <inheritdoc />
        public virtual void RemoveAt(int index)
        {
            _layers.RemoveAt(index);
        }

        /// <inheritdoc />
        public virtual void ResumeEvents()
        {
            _layers.ResumeEvents();
        }

        /// <inheritdoc />
        public override bool Select(Envelope tolerant, Envelope strict, SelectionMode mode, out Envelope affectedArea)
        {
            affectedArea = new Envelope();
            if (!_selectionEnabled) return false;
            bool somethingChanged = false;
            MapFrame.SuspendEvents();

            foreach (var s in GetLayers().Reverse().Where(_ => _.SelectionEnabled && _.IsVisible))
            {
                Envelope layerArea;
                if (s.Select(tolerant, strict, mode, out layerArea))
                {
                    somethingChanged = true;
                    affectedArea.ExpandToInclude(layerArea);
                }

                // removed by jany_: this selected only features of the first layer with features in the selected area, if user wanted to select features of another layer too they get ignored
                // added SelectPlugin enables user to choose the layers in which he wants to select features
                // if (somethingChanged)
                // {
                //    MapFrame.ResumeEvents();
                //    OnSelectionChanged(); // fires only AFTER the individual layers have fired their events.
                //    return somethingChanged;
                // }
            }

            MapFrame.ResumeEvents();
            OnSelectionChanged(); // fires only AFTER the individual layers have fired their events.
            return somethingChanged;
        }

        /// <inheritdoc />
        public virtual void SuspendEvents()
        {
            _layers.SuspendEvents();
        }

        /// <inheritdoc />
        public override bool UnSelect(Envelope tolerant, Envelope strict, SelectionMode mode, out Envelope affectedArea)
        {
            affectedArea = new Envelope();
            if (!_selectionEnabled) return false;
            bool somethingChanged = false;
            SuspendEvents();
            foreach (ILayer s in GetLayers())
            {
                Envelope layerArea;
                if (s.UnSelect(tolerant, strict, mode, out layerArea))
                {
                    somethingChanged = true;
                    affectedArea.ExpandToInclude(layerArea);
                }
            }

            ResumeEvents();
            OnSelectionChanged(); // fires only AFTER the individual layers have fired their events.
            return somethingChanged;
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _layers.GetEnumerator();
        }

        /// <summary>
        /// Zoom to group
        /// </summary>
        internal void ZoomToGroup()
        {
            var extent = Extent;
            if (extent != null)
            {
                OnZoomToLayer(extent.ToEnvelope());
            }
        }

        /// <summary>
        /// Disposes the unmanaged resourced of this group. If disposeManagedResources is true, then this will
        /// also dispose the resources of the child layers and groups unless they are dispose locked.
        /// </summary>
        /// <param name="disposeManagedResources">Boolean, true to dispose child objects and set managed members to null.</param>
        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                ParentGroup = null;
                _layers?.Dispose();
            }

            Icon?.Dispose();

            base.Dispose(disposeManagedResources);
        }

        /// <summary>
        /// Given a new LayerCollection, we need to be sensitive to certain events.
        /// </summary>
        /// <param name="collection">Collection events get added to.</param>
        protected virtual void HandleLayerEvents(ILayerEvents collection)
        {
            if (collection == null) return;
            collection.LayerVisibleChanged += LayersLayerVisibleChanged;
            collection.ItemChanged += LayersItemChanged;
            collection.ZoomToLayer += LayersZoomToLayer;
            collection.SelectionChanging += CollectionSelectionChanging;
            collection.LayerSelected += CollectionLayerSelected;
            collection.LayerAdded += LayersLayerAdded;
            collection.LayerRemoved += LayersLayerRemoved;

            collection.SelectionChanged += CollectionSelectionChanged;
        }

        /// <summary>
        /// When setting an old layer collection it is advisable to not only add
        /// new handlers to the new collection, but remove the handlers related to the old collection.
        /// </summary>
        /// <param name="collection">Collection events get removed from.</param>
        protected virtual void IgnoreLayerEvents(ILayerEvents collection)
        {
            if (collection == null) return;
            collection.LayerVisibleChanged -= LayersLayerVisibleChanged;
            collection.ItemChanged -= LayersItemChanged;
            collection.ZoomToLayer -= LayersZoomToLayer;
            collection.SelectionChanging -= CollectionSelectionChanging;
            collection.LayerSelected -= CollectionLayerSelected;
            collection.LayerAdded -= LayersLayerAdded;
            collection.LayerRemoved -= LayersLayerRemoved;

            collection.SelectionChanged -= CollectionSelectionChanged;
        }

        /// <summary>
        /// Creates a virtual method when sub-groups are being created
        /// </summary>
        protected virtual void OnCreateGroup()
        {
            Group grp = new Group(Layers, MapFrame, ProgressHandler)
            {
                LegendText = "New Group"
            };
        }

        /// <summary>
        /// Simply echo this event out to members above this group that might be listening to it.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnLayerAdded(object sender, LayerEventArgs e)
        {
            LayerAdded?.Invoke(sender, e);
        }

        /// <summary>
        /// Occurs when removing a layer. This also fires the LayerRemoved event.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        protected virtual void OnLayerRemoved(object sender, LayerEventArgs e)
        {
            LayerRemoved?.Invoke(sender, e);
        }

        private void CollectionLayerSelected(object sender, LayerSelectedEventArgs e)
        {
            OnLayerSelected(e.Layer, e.IsSelected);
        }

        private void CollectionSelectionChanged(object sender, EventArgs e)
        {
            OnSelectionChanged();
        }

        private void CollectionSelectionChanging(object sender, FeatureLayerSelectionEventArgs e)
        {
            OnSelectionChanged();
        }

        private void Configure()
        {
            Layers = new LayerCollection(MapFrame, this);
            StateLocked = false;
            IsDragable = true;
            IsExpanded = true;
            ContextMenuItems = new List<SymbologyMenuItem>
                               {
                                   new SymbologyMenuItem("Remove Group", RemoveClick),
                                   new SymbologyMenuItem("Zoom to Group", (sender, args) => ZoomToGroup()),
                                   new SymbologyMenuItem("Create new Group", CreateGroupClick)
                               };
            _selectionEnabled = true;
        }

        private void CreateGroupClick(object sender, EventArgs e)
        {
            OnCreateGroup();
        }

        private void LayersItemChanged(object sender, EventArgs e)
        {
            OnItemChanged(sender);
        }

        private void LayersLayerAdded(object sender, LayerEventArgs e)
        {
            OnLayerAdded(sender, e);
        }

        private void LayersLayerRemoved(object sender, LayerEventArgs e)
        {
            OnLayerRemoved(sender, e);
        }

        private void LayersLayerVisibleChanged(object sender, EventArgs e)
        {
            OnVisibleChanged(sender, e);
        }

        private void LayersZoomToLayer(object sender, EnvelopeArgs e)
        {
            OnZoomToLayer(e.Envelope);
        }

        private void RemoveClick(object sender, EventArgs e)
        {
            OnRemoveItem();
        }

        #endregion
    }
}