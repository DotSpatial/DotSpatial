// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/9/2009 5:59:10 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DotSpatial.Data;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Group
    /// </summary>
    public class Group : Layer, IGroup
    {
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

        #region Private Variables

        private int _handle;
        private Image _image;
        private ILayerCollection _layers;
        //private ILegend _legend;
        private IGroup _parentGroup;
        private bool _selectionEnabled;
        private bool _stateLocked;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Group
        /// </summary>
        public Group()
        {
            Configure();
        }

        /// <summary>
        /// Creates a group that sits in a layer list and uses the specified progress handler
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="progressHandler">the progress handler</param>
        public Group(IFrame frame, IProgressHandler progressHandler)
            : base(progressHandler)
        {
            MapFrame = frame;
            Configure();
        }

        /// <summary>
        /// Creates a group that sits in a layer list and uses the specified progress handler
        /// </summary>
        /// <param name="container">the layer list</param>
        /// <param name="frame"></param>
        /// <param name="progressHandler">the progress handler</param>
        public Group(ICollection<ILayer> container, IFrame frame, IProgressHandler progressHandler)
            : base(container, progressHandler)
        {
            MapFrame = frame;
            Configure();
        }

        private void Configure()
        {
            Layers = new LayerCollection(MapFrame, this);
            _stateLocked = false;
            IsDragable = true;
            base.IsExpanded = true;
            ContextMenuItems = new List<SymbologyMenuItem>();
            ContextMenuItems.Add(new SymbologyMenuItem("Remove Group", Remove_Click));
            ContextMenuItems.Add(new SymbologyMenuItem("Zoom to Group", ZoomToGroupClick));
            ContextMenuItems.Add(new SymbologyMenuItem("Create new Group", CreateGroupClick));
            _selectionEnabled = true;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public virtual void Add(ILayer layer)
        {
            _layers.Add(layer);
        }

        /// <inheritdoc />
        public virtual bool Remove(ILayer layer)
        {
            return _layers.Remove(layer);
        }

        /// <inheritdoc />
        public virtual void Insert(int index, ILayer layer)
        {
            _layers.Insert(index, layer);
        }

        /// <inheritdoc />
        public override Size GetLegendSymbolSize()
        {
            return new Size(16, 16);
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
        public Bitmap LegendSnapShot(int imgWidth)
        {
            //bool TO_DO_GROUP_LEGEND_SNAPSHOT;
            //return new Bitmap(100, 100);
            throw new NotImplementedException();
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
        public override bool ClearSelection(out IEnvelope affectedAreas)
        {
            affectedAreas = new Envelope();
            bool changed = false;
            if (!_selectionEnabled) return false;
            MapFrame.SuspendEvents();
            foreach (ILayer layer in GetLayers())
            {
                IEnvelope layerArea;
                if (layer.ClearSelection(out layerArea)) changed = true;
                affectedAreas.ExpandToInclude(layerArea);
            }
            MapFrame.ResumeEvents();

            OnSelectionChanged(); // fires only AFTER the individual layers have fired their events.

            return changed;
        }

        /// <inheritdoc />
        public override bool Select(IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea)
        {
            affectedArea = new Envelope();
            if (!_selectionEnabled) return false;
            bool somethingChanged = false;
            MapFrame.SuspendEvents();

            List<ILayer> layers = (List<ILayer>)GetLayers();
            layers.Reverse();

            foreach (ILayer s in layers)
            {
                if (s.SelectionEnabled == false) continue;
                if (s.IsVisible == false) continue;
                IEnvelope layerArea;
                if (s.Select(tolerant, strict, mode, out layerArea)) somethingChanged = true;
                affectedArea.ExpandToInclude(layerArea);
                if (somethingChanged == true)
                {
                    MapFrame.ResumeEvents();
                    OnSelectionChanged(); // fires only AFTER the individual layers have fired their events.
                    return somethingChanged;
                }
                
            }
            MapFrame.ResumeEvents();
            OnSelectionChanged(); // fires only AFTER the individual layers have fired their events.
            return somethingChanged;
        }

        /// <inheritdoc />
        public virtual IList<ILayer> GetLayers()
        {
            return _layers.Cast<ILayer>().ToList();
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
        public override bool InvertSelection(IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea)
        {
            affectedArea = new Envelope();
            if (!_selectionEnabled) return false;
            bool somethingChanged = false;
            MapFrame.SuspendEvents();
            foreach (ILayer s in GetLayers())
            {
                if (s.SelectionEnabled == false) continue;
                IEnvelope layerArea;
                if (s.InvertSelection(tolerant, strict, mode, out layerArea)) somethingChanged = true;
                affectedArea.ExpandToInclude(layerArea);
            }
            MapFrame.ResumeEvents();
            OnSelectionChanged(); // fires only AFTER the individual layers have fired their events.
            return somethingChanged;
        }

        /// <inheritdoc />
        public override bool UnSelect(IEnvelope tolerant, IEnvelope strict, SelectionMode mode, out IEnvelope affectedArea)
        {
            affectedArea = new Envelope();
            if (!_selectionEnabled) return false;
            bool somethingChanged = false;
            SuspendEvents();
            foreach (ILayer s in GetLayers())
            {
                IEnvelope layerArea;
                if (s.UnSelect(tolerant, strict, mode, out layerArea)) somethingChanged = true;
                affectedArea.ExpandToInclude(layerArea);
            }
            ResumeEvents();
            OnSelectionChanged(); // fires only AFTER the individual layers have fired their events.
            return somethingChanged;
        }

        /// <inheritdoc />
        public virtual void SuspendEvents()
        {
            _layers.SuspendEvents();
        }

        /// <inheritdoc />
        public virtual void ResumeEvents()
        {
            _layers.ResumeEvents();
        }

        /// <inheritdoc />
        public virtual bool EventsSuspended
        {
            get { return _layers.EventsSuspended; }
        }

        #endregion

        #region Properties

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
                    Ignore_Layer_Events(_layers);
                }
                _layers = value;
                if (_layers != null)
                {
                    Handle_Layer_Events(value);
                    _layers.ParentGroup = this;
                    _layers.MapFrame = MapFrame;
                }
            }
        }

        /// <summary>
        /// Boolean, true if any sub-layers in the group are visible.  Setting this
        /// will force all the layers in this group to become visible.
        /// </summary>
        public override bool IsVisible
        {
            get
            {
                foreach (ILayer lyr in GetLayers())
                {
                    if (lyr.IsVisible) return true;
                }
                return false;
            }
            set
            {
                foreach (ILayer lyr in GetLayers())
                {
                    lyr.IsVisible = value;
                }
                base.IsVisible = value;
            }
        }

        /// <summary>
        /// The envelope that contains all of the layers for this data frame.  Essentially this would be
        /// the extents to use if you want to zoom to the world view.
        /// </summary>
        public override Extent Extent
        {
            get
            {
                Extent ext = null;
                IList<ILayer> layers = GetLayers();
                if (layers != null)
                {
                    foreach (ILayer layer in layers)
                    {
                        if (layer.Extent != null)
                        {
                            if (ext == null)
                            {
                                ext = (Extent)layer.Extent.Clone();
                            }
                            else
                            {
                                ext.ExpandToInclude(layer.Extent);
                            }
                        }
                    }
                }
                return ext;
            }
        }

        /// <summary>
        /// Gets the integer handle for this group
        /// </summary>
        public int Handle
        {
            get { return _handle; }
            protected set { _handle = value; }
        }

        /// <summary>
        /// Gets or sets the icon
        /// </summary>
        public Image Icon
        {
            get { return _image; }
            set { _image = value; }
        }

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

        /// <summary>
        /// Gets the layer handle of the specified layer
        /// </summary>
        /// <param name="positionInGroup">0 based index into list of layers</param>
        /// <returns>Layer's handle on success, -1 on failure</returns>
        public int LayerHandle(int positionInGroup)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns the count of the layers that are currently stored in this map frame.
        /// </summary>
        public int LayerCount
        {
            get { return _layers.Count; }
        }

        /// <summary>
        /// Gets a boolean that is true if any of the immediate layers or groups contained within this
        /// control are visible.  Setting this will set the visibility for each of the members of this
        /// map frame.
        /// </summary>
        public bool LayersVisible
        {
            get
            {
                foreach (ILayer layer in GetLayers())
                {
                    if (layer.IsVisible) return true;
                }
                return false;
            }
            set
            {
                foreach (ILayer layer in GetLayers())
                {
                    layer.IsVisible = value;
                }
            }
        }

        /// <summary>
        /// This is a different view of the layers cast as legend items.  This allows
        /// easier cycling in recursive legend code.
        /// </summary>
        public override IEnumerable<ILegendItem> LegendItems
        {
            get
            {
                return _layers.Cast<ILegendItem>();
            }
        }

        /// <summary>
        /// Gets the parent group of this group.
        /// </summary>
        public IGroup ParentGroup
        {
            get { return _parentGroup; }
            protected set { _parentGroup = value; }
        }

        /// <summary>
        /// Gets or sets the progress handler to use.  Setting this will set the progress handler for
        /// each of the layers in this map frame.
        /// </summary>
        public override IProgressHandler ProgressHandler
        {
            get { return base.ProgressHandler; }
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
        /// gets or sets the locked property, which prevents the user from changing the visual state
        /// except layer by layer
        /// </summary>
        public bool StateLocked
        {
            get { return _stateLocked; }
            set { _stateLocked = value; }
        }

        #endregion

        #region Private Methods

        private void Remove_Click(object sender, EventArgs e)
        {
            OnRemoveItem();
        }

        private void ZoomToGroupClick(object sender, EventArgs e)
        {
            OnZoomToLayer(Extent.ToEnvelope());
        }

        private void CreateGroupClick(object sender, EventArgs e)
        {
            OnCreateGroup();
        }

        /// <summary>
        /// Creates a virtual method when sub-groups are being created
        /// </summary>
        protected virtual void OnCreateGroup()
        {
            Group grp = new Group(Layers, MapFrame, ProgressHandler);
            grp.LegendText = "New Group";
        }

        private void LayersItemChanged(object sender, EventArgs e)
        {
            OnItemChanged(sender);
        }

        private void LayersLayerVisibleChanged(object sender, EventArgs e)
        {
            OnVisibleChanged(sender, e);
        }

        private void Layers_ZoomToLayer(object sender, EnvelopeArgs e)
        {
            OnZoomToLayer(e.Envelope);
        }

        /// <summary>
        /// Given a new LayerCollection, we need to be sensitive to certain events
        /// </summary>
        protected virtual void Handle_Layer_Events(ILayerEvents collection)
        {
            if (collection == null) return;
            collection.LayerVisibleChanged += LayersLayerVisibleChanged;
            collection.ItemChanged += LayersItemChanged;
            collection.ZoomToLayer += Layers_ZoomToLayer;
            collection.SelectionChanging += collection_SelectionChanging;
            collection.LayerSelected += collection_LayerSelected;
            collection.LayerAdded += Layers_LayerAdded;
            collection.LayerRemoved += Layers_LayerRemoved;

            collection.SelectionChanged += collection_SelectionChanged;
        }

        private void collection_LayerSelected(object sender, LayerSelectedEventArgs e)
        {
            OnLayerSelected(e.Layer, e.IsSelected);
        }

        private void collection_SelectionChanging(object sender, FeatureLayerSelectionEventArgs e)
        {
            OnSelectionChanged();
        }

        private void collection_SelectionChanged(object sender, EventArgs e)
        {
            OnSelectionChanged();
        }

        private void Layers_LayerRemoved(object sender, LayerEventArgs e)
        {
            OnLayerRemoved(sender, e);
        }

        /// <summary>
        /// Occurs when removing a layer.  This also fires the LayerRemoved event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnLayerRemoved(object sender, LayerEventArgs e)
        {
            if (LayerRemoved != null) LayerRemoved(sender, e);
        }

        private void Layers_LayerAdded(object sender, LayerEventArgs e)
        {
            OnLayerAdded(sender, e);
        }

        /// <summary>
        /// Simply echo this event out to members above this group that might be listening to it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnLayerAdded(object sender, LayerEventArgs e)
        {
            if (LayerAdded != null) LayerAdded(sender, e);
        }

        /// <summary>
        /// When setting an old layer collection it is advisable to not only add
        /// new handlers to the new collection, but remove the handlers related
        /// to the old collection.
        /// </summary>
        /// <param name="collection"></param>
        protected virtual void Ignore_Layer_Events(ILayerEvents collection)
        {
            if (collection == null) return;
            collection.LayerVisibleChanged -= LayersLayerVisibleChanged;
            collection.ItemChanged -= LayersItemChanged;
            collection.ZoomToLayer -= Layers_ZoomToLayer;
            collection.SelectionChanging -= collection_SelectionChanging;
            collection.LayerSelected -= collection_LayerSelected;
            collection.LayerAdded -= Layers_LayerAdded;
            collection.LayerRemoved -= Layers_LayerRemoved;

            collection.SelectionChanged -= collection_SelectionChanged;
        }

        /// <summary>
        /// Disposes the unmanaged resourced of this group.  If disposeManagedResources is true, then this will
        /// also dispose the resources of the child layers and groups unless they are dispose locked.
        /// </summary>
        /// <param name="disposeManagedResources">Boolean, true to dispose child objects and set managed members to null.</param>
        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                _parentGroup = null;
                if (_layers != null)
                {
                    _layers.Dispose();
                }
            }
            if (_image != null)
            {
                _image.Dispose();
            }
            base.Dispose(disposeManagedResources);
        }

        #endregion

        #region IGroup Members

        /// <inheritdoc />
        public virtual int IndexOf(ILayer item)
        {
            return _layers.IndexOf(item);
        }

        /// <inheritdoc />
        public virtual void RemoveAt(int index)
        {
            _layers.RemoveAt(index);
        }

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

        /// <inheritdoc />
        public virtual void Clear()
        {
            _layers.Clear();
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
        public virtual int Count
        {
            get { return _layers.Count; }
        }

        /// <inheritdoc />
        public virtual bool IsReadOnly
        {
            get { return _layers.IsReadOnly; }
        }

        /// <inheritdoc />
        public virtual IEnumerator<ILayer> GetEnumerator()
        {
            return _layers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _layers.GetEnumerator();
        }

        #endregion
    }
}