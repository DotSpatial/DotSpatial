// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This extends the ChangeEventList by providing methods that allow access by an object Key, and will pass.
    /// </summary>
    public class LayerCollection : LayerEventList<ILayer>, ILayerCollection
    {
        #region Fields
        private IFrame _mapFrame;
        private IGroup _parentGroup;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerCollection"/> class that is blank.
        /// This is especially useful for tracking layers that can draw themselves. This does not concern itself with
        /// view extents like a dataframe, but rather is a grouping of layers that is itself also an IMapLayer.
        /// </summary>
        /// <param name="containerFrame">The map frame.</param>
        public LayerCollection(IFrame containerFrame)
        {
            _mapFrame = containerFrame;
            _parentGroup = containerFrame;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerCollection"/> class
        /// where the parent group is different from the map frame.
        /// </summary>
        /// <param name="frame">The map frame.</param>
        /// <param name="parent">The parent of this.</param>
        public LayerCollection(IFrame frame, IGroup parent)
        {
            _mapFrame = frame;
            _parentGroup = parent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerCollection"/> class that is not confined in a MapFrame.
        /// </summary>
        public LayerCollection()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current count.
        /// </summary>
        public new int Count => base.Count;

        /// <summary>
        /// Gets or sets the map frame of this layer collection.
        /// </summary>
        public IFrame MapFrame
        {
            get
            {
                return _mapFrame;
            }

            // not sure I want to try to implement set. That would force me to change the _mapFrame property
            // on all the layers? Maybe layers don't access
            set
            {
                _mapFrame = value;
                foreach (ILayer item in this)
                {
                    item.MapFrame = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the ParentGroup for this layer collection, even if that parent group
        /// is not actually a map frame.
        /// </summary>
        public IGroup ParentGroup
        {
            get
            {
                return _parentGroup;
            }

            set
            {
                _parentGroup = value;
                foreach (ILayer item in this)
                {
                    item.SetParentItem(_parentGroup);
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a range of legend items.
        /// </summary>
        /// <param name="collection">Legend items that get added.</param>
        public void AddRange(IEnumerable<ILegendItem> collection)
        {
            foreach (ILegendItem item in collection)
            {
                Add(item as ILayer);
            }
        }

        /// <summary>
        /// Gets the index of the specified item in this collection if it is a layer.
        /// </summary>
        /// <param name="item">Item whose index should be returned.</param>
        /// <returns>-1 if the item is no ILayer or not found, otherwise the index.</returns>
        public int IndexOf(ILegendItem item)
        {
            if (item is not ILayer layer) return -1;
            return base.IndexOf(layer);
        }

        /// <summary>
        /// Inserts the specified item into this collection if it is a layer.
        /// </summary>
        /// <param name="index">Index where the item should be added.</param>
        /// <param name="item">Item that should be added.</param>
        public void Insert(int index, ILegendItem item)
        {
            if (item is not ILayer layer) return;
            base.Insert(index, layer);
        }

        /// <summary>
        /// Occurs when unwiring events.
        /// </summary>
        /// <param name="item">Item whose events get unwired.</param>
        protected override void OnExclude(ILayer item)
        {
            item.SetParentItem(null);
            item.MapFrame = null;
            base.OnExclude(item);
        }

        /// <summary>
        /// Occurs when wiring events.
        /// </summary>
        /// <param name="item">Item whose events get wired.</param>
        protected override void OnInclude(ILayer item)
        {
            item.SetParentItem(_parentGroup);
            item.MapFrame = _mapFrame;
            base.OnInclude(item);
        }

        #endregion
    }
}