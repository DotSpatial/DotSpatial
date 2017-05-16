// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Diagnostics;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This extends the ChangeEventList by providing methods that allow access by an object Key, and will pass
    /// </summary>
    /// <typeparam name="T">Type of the items in the event list.</typeparam>
    public class LayerEventList<T> : ChangeEventList<T>, ILayerEventList<T>, IDisposable, IDisposeLock
        where T : class, ILayer
    {
        #region Fields

        private int _disposeLockCount;
        private bool _isInitialized; // True if layers already existed before a member change
        private ILayer _selectedLayer;

        #endregion

        #region Constructors

        /// <summary>
        /// Finalizes an instance of the <see cref="LayerEventList{T}"/> class.
        /// </summary>
        ~LayerEventList()
        {
            Dispose(false);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a layer is added to this item.
        /// </summary>
        public event EventHandler<LayerEventArgs> LayerAdded;

        /// <summary>
        /// Occurs when a layer is moved.
        /// </summary>
        public event EventHandler<LayerMovedEventArgs> LayerMoved;

        /// <summary>
        /// Occurs when a layer is removed from this item.
        /// </summary>
        public event EventHandler<LayerEventArgs> LayerRemoved;

        /// <summary>
        /// Occurs immediately after a layer is selected.
        /// </summary>
        public event EventHandler<LayerSelectedEventArgs> LayerSelected;

        /// <summary>
        /// Occurs when one of the layers in this collection changes visibility.
        /// </summary>
        public event EventHandler LayerVisibleChanged;

        /// <summary>
        /// Occurs after the selection has changed, and all the layers have had their selection information updated.
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        /// Occurs when the selection on a feature layer changes.
        /// </summary>
        public event EventHandler<FeatureLayerSelectionEventArgs> SelectionChanging;

        /// <summary>
        /// Occurs if the maps should zoom to this layer.
        /// </summary>
        public event EventHandler<EnvelopeArgs> ZoomToLayer;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the envelope that contains all of the layers for this data frame. Essentially this would be
        /// the extents to use if you want to zoom to the world view.
        /// </summary>
        public virtual Extent Extent
        {
            get
            {
                Extent env = new Extent();
                if (Count == 0) return env;

                foreach (T lyr in this)
                {
                    env.ExpandToInclude(lyr.Extent);
                }

                return env;
            }
        }

        /// <summary>
        /// Gets a value indicating whether dispose has already been called on this object.
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Gets a value indicating whether there are requests to prevent disposal of this object.
        /// </summary>
        public bool IsDisposeLocked => _disposeLockCount > 0;

        /// <summary>
        /// Gets or sets the currently selected layer in this collection.
        /// </summary>
        public ILayer SelectedLayer
        {
            get
            {
                return _selectedLayer;
            }

            set
            {
                _selectedLayer = value;
                if (value != null)
                {
                    value.IsSelected = true;
                    OnLayerSelected(value, true);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Standard Dispose implementation since layers are now disposable.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Adds one request to prevent disposal of this object and its children.
        /// </summary>
        public void LockDispose()
        {
            _disposeLockCount++;
        }

        /// <summary>
        /// This selects the layer with the specified integer index
        /// </summary>
        /// <param name="index">THe zero based integer index</param>
        public void SelectLayer(int index)
        {
            SelectedLayer = this[index];
        }

        /// <summary>
        /// Removes one request to prevent disposal of this object and its children.
        /// </summary>
        public void UnlockDispose()
        {
            _disposeLockCount--;
        }

        /// <summary>
        /// Overrides the dispose behavior. If disposeManagedMemory objects is true, then managed objects
        /// should be set to null, effectively removing reference counts. If it is false, then only
        /// unmanaged memory objects should be removed.
        /// </summary>
        /// <param name="disposeManagedMemory">Boolean, true if managed memory objects should be set to null, and
        /// if the Dispose method should be called on contained objects.</param>
        protected virtual void Dispose(bool disposeManagedMemory)
        {
            if (!IsDisposed)
            {
                // In debug mode, try to find any calls to dispose that will conflict with locks.
                Debug.Assert(_disposeLockCount == 0, "_disposeLockCount == 0");
                if (disposeManagedMemory)
                {
                    foreach (var layer in this)
                    {
                        if (layer != null)
                        {
                            layer.UnlockDispose(); // indicate this group no longer needs to lock the layer.
                            if (!layer.IsDisposeLocked) layer.Dispose();
                        }
                    }
                }
            }

            IsDisposed = true;
        }

        /// <summary>
        /// Removes the extended event listeners once a layer is removed from this list.
        /// </summary>
        /// <param name="item">The item that got excluded.</param>
        protected override void OnExclude(T item)
        {
            item.UnlockDispose();
            item.ZoomToLayer -= LayerZoomToLayer;
            item.VisibleChanged -= OnLayerVisibleChanged;
            item.FinishedLoading -= ItemFinishedLoading;
            item.SelectionChanged -= SelectableSelectionChanged;
            item.LayerSelected -= ItemLayerSelected;

            // if the layer being removed is selected, ensure that the SelectedLayer property is cleared
            if (item.Equals(SelectedLayer))
            {
                SelectedLayer = null;
            }

            base.OnExclude(item);
            OnLayerRemoved(item);
            OnListChanged();
            if (!item.IsDisposeLocked)
            {
                item.Dispose();
            }
        }

        /// <summary>
        /// Extends the event listeners to include events like ZoomToLayer and VisibleChanged.
        /// </summary>
        /// <param name="item">The item that got included.</param>
        protected override void OnInclude(T item)
        {
            item.LockDispose();
            item.ZoomToLayer += LayerZoomToLayer;
            item.VisibleChanged += OnLayerVisibleChanged;
            item.FinishedLoading += ItemFinishedLoading;
            item.SelectionChanged += SelectableSelectionChanged;
            item.LayerSelected += ItemLayerSelected;
            base.OnInclude(item);
            OnListChanged();
        }

        /// <inheritdoc />
        protected override void OnIncludeComplete(T item)
        {
            OnLayerAdded(this, new LayerEventArgs(item));
        }

        /// <summary>
        /// Fires the LayerAdded event
        /// </summary>
        /// <param name="sender">The layer that was added</param>
        /// <param name="e">LayerEventArgs</param>
        protected virtual void OnLayerAdded(object sender, LayerEventArgs e)
        {
            if (EventsSuspended) return;

            LayerAdded?.Invoke(sender, e);
        }

        /// <summary>
        /// Fires the LayerMoved event.
        /// </summary>
        /// <param name="sender">The layer that was moved.</param>
        /// <param name="e">LayerEventArgs</param>
        protected virtual void OnLayerMoved(object sender, LayerMovedEventArgs e)
        {
            if (EventsSuspended) return;

            LayerMoved?.Invoke(sender, e);
        }

        /// <summary>
        /// Fires the LayerRemoved event.
        /// </summary>
        /// <param name="item">Layer that was removed.</param>
        protected virtual void OnLayerRemoved(T item)
        {
            if (EventsSuspended) return;

            LayerRemoved?.Invoke(this, new LayerEventArgs(item));
        }

        /// <summary>
        /// Fires the LayerSelected event and adjusts the selected state of the layer.
        /// </summary>
        /// <param name="layer">The layer to select</param>
        /// <param name="selected">Boolean, true if the specified layer is selected</param>
        protected virtual void OnLayerSelected(ILayer layer, bool selected)
        {
            LayerSelected?.Invoke(this, new LayerSelectedEventArgs(layer, selected));
        }

        /// <summary>
        /// Fires the ItemChanged event and the MembersChanged event and resets any cached lists
        /// </summary>
        protected override void OnListChanged()
        {
            if (EventsSuspended)
            {
                base.OnListChanged();
                return;
            }

            if (_isInitialized == false)
            {
                if (Count > 0)
                {
                    _isInitialized = true;
                }
            }
            else
            {
                if (Count == 0)
                {
                    _isInitialized = false;
                }
            }

            // reset cached extra lists if any
            base.OnListChanged();
        }

        /// <inheritdoc />
        protected override void OnMoved(T item, int newPosition)
        {
            base.OnListChanged();
            OnLayerMoved(this, new LayerMovedEventArgs(item, newPosition));
        }

        /// <summary>
        /// Raises <see cref="SelectionChanged"/> event.
        /// </summary>
        protected virtual void OnSelectionChanged()
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the selection changed event
        /// </summary>
        /// <param name="sender">the object sender of the event</param>
        /// <param name="args">The FeatureLayerSelectionEventArgs of the layer</param>
        protected virtual void OnSelectionChanging(object sender, FeatureLayerSelectionEventArgs args)
        {
            SelectionChanging?.Invoke(sender, args);
        }

        /// <summary>
        /// Fires the ZoomToLayer method when one of the layers fires its ZoomTo event
        /// </summary>
        /// <param name="sender">The layer to zoom to</param>
        /// <param name="e">The extent of the layer</param>
        protected virtual void OnZoomToLayer(object sender, EnvelopeArgs e)
        {
            // Just forward the event
            ZoomToLayer?.Invoke(sender, e);
        }

        private void ItemFinishedLoading(object sender, EventArgs e)
        {
            OnLayerAdded(this, new LayerEventArgs(sender as ILayer));
        }

        private void ItemLayerSelected(object sender, LayerSelectedEventArgs e)
        {
            if (e.IsSelected)
            {
                SelectedLayer = e.Layer;
            }
        }

        private void OnLayerVisibleChanged(object sender, EventArgs e)
        {
            LayerVisibleChanged?.Invoke(sender, e);
        }

        private void LayerZoomToLayer(object sender, EnvelopeArgs e)
        {
            OnZoomToLayer(sender, e);
        }

        private void SelectableSelectionChanged(object sender, EventArgs e)
        {
            OnSelectionChanged();
        }

        #endregion
    }
}