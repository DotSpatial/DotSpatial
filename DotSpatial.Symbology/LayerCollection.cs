// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in September, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.Linq;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This extends the ChangeEventList by providing methods that allow access by an object Key, and will pass
    /// </summary>
    public class LayerCollection : LayerEventList<ILayer>, ILayerCollection
    {
        #region Private Variables

        private IFrame _mapFrame;

        private IGroup _parentGroup;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new blank instance of a MapLayer collection.  This is especially useful
        /// for tracking layers that can draw themselves.  This does not concern itself with
        /// view extents like a dataframe, but rather is a grouping of layers that is itself
        /// also an IMapLayer.
        /// </summary>
        public LayerCollection(IFrame containerFrame)
        {
            _mapFrame = containerFrame;
            _parentGroup = containerFrame;
        }

        /// <summary>
        /// Creates a new layer collection where the parent group is different from the
        /// map frame.
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="parent"></param>
        public LayerCollection(IFrame frame, IGroup parent)
        {
            _mapFrame = frame;
            _parentGroup = parent;
        }

        /// <summary>
        /// Creates a LayerCollection that is not confined in a MapFrame
        /// </summary>
        public LayerCollection()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Occurs when wiring events
        /// </summary>
        /// <param name="item"></param>
        protected override void OnInclude(ILayer item)
        {
            item.SetParentItem(_parentGroup);
            item.MapFrame = _mapFrame;
            base.OnInclude(item);
        }

        /// <summary>
        /// Occurs when unwiring events
        /// </summary>
        /// <param name="item"></param>
        protected override void OnExclude(ILayer item)
        {
            item.SetParentItem(null);
            item.MapFrame = null;
            base.OnExclude(item);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current count.
        /// </summary>
        public new int Count
        {
            get { return base.Count; }
        }

        /// <summary>
        /// Gets the map frame of this layer collection
        /// </summary>
        public IFrame MapFrame
        {
            get
            {
                return _mapFrame;
            }
            // not sure I want to try to implement set.  That would force me to change the _mapFrame property
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

        #endregion

        #region ILayerCollection Members

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

        /// <summary>
        /// Given a base name, this increments a number for appending
        /// if the name already exists in the collection.
        /// </summary>
        /// <param name="baseName">The string base name to start with</param>
        /// <returns>The base name modified by a number making it unique in the collection</returns>
        public string UnusedName(string baseName)
        {
            int i = 1;
            string name = baseName;
            bool duplicateExists = this.Any(item => item.LegendText == baseName);
            while (duplicateExists)
            {
                duplicateExists = false;
                name = baseName + i;
                foreach (ILayer item in this)
                {
                    if (item.LegendText != name) continue;
                    duplicateExists = true;
                    break;
                }
                i++;
            }
            return name;
        }

        #endregion

        /// <summary>
        /// Gets the index of the specified item in this collection if it is a layer
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(ILegendItem item)
        {
            ILayer layer = item as ILayer;
            if (layer == null) return -1;
            return base.IndexOf(item as ILayer);
        }

        /// <summary>
        /// Inserts the specified item into this collection if it is a layer
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, ILegendItem item)
        {
            ILayer layer = item as ILayer;
            if (layer == null) return;
            base.Insert(index, layer);
        }

        /// <summary>
        /// Adds a range of values
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(IEnumerable<ILegendItem> collection)
        {
            foreach (ILegendItem item in collection)
            {
                Add(item as ILayer);
            }
        }
    }
}