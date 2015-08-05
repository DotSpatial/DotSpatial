// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;
using System.Collections.Generic;

namespace DotSpatial.Topology.Index.Bintree
{
    /// <summary>
    /// The base class for nodes in a <c>Bintree</c>.
    /// </summary>
    [Serializable]
    public abstract class NodeBase<T>
    {
        #region Fields

        /// <summary>
        /// Subnodes are numbered as follows:
        /// 0 | 1
        /// </summary>
        protected Node<T>[] Subnode = new Node<T>[2];

        private IList<T> _items = new List<T>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the count of all the items in this node, plus all the items in all of the child nodes
        /// </summary>
        public virtual int Count
        {
            get
            {
                int subSize = 0;
                for (int i = 0; i < 2; i++)
                    if (Subnode[i] != null)
                        subSize += Subnode[i].Count;
                return subSize + _items.Count;
            }
        }

        /// <summary>
        /// Gets an integer representing the maximum levels needed to be decended to account for all the child nodes
        /// </summary>
        public virtual int Depth
        {
            get
            {
                int maxSubDepth = 0;
                for (int i = 0; i < 2; i++)
                {
                    if (Subnode[i] != null)
                    {
                        int sqd = Subnode[i].Depth;
                        if (sqd > maxSubDepth)
                            maxSubDepth = sqd;
                    }
                }
                return maxSubDepth + 1;
            }
        }

        /// <summary>
        /// Gets whether this node has any children
        /// </summary>
        public bool HasChildren
        {
            get
            {
                for (int i = 0; i < 2; i++)
                {
                    if (Subnode[i] != null)
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool HasItems { get { return _items.Count != 0; }
        }

        /// <summary>
        /// Gets whether this node is prunable
        /// </summary>
        public bool IsPrunable
        {
            get { return !(HasChildren || HasItems); }
        }

        /// <summary>
        /// 
        /// </summary>
        public int NodeCount
        {
            get
            {
                int subCount = 0;
                for (int i = 0; i < 2; i++)
                    if (Subnode[i] != null)
                        subCount += Subnode[i].NodeCount;
                return subCount + 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IList<T> Items
        {
            get
            {
                return _items;
            }
            protected set { _items = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified object to the items list for this node.  This will not affect child nodes.
        /// </summary>
        /// <param name="item">The object item to add to the list.</param>
        public void Add(T item)
        {
            _items.Add(item);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public IList<T> AddAllItems(IList<T> items)
        {
            // items.addAll(this.items);
            foreach (T o in _items)
                items.Add(o);
            for (int i = 0; i < 2; i++)
                if (Subnode[i] != null)
                    Subnode[i].AddAllItems(items);
            return items;
        }

        /// <summary>
        /// Adds items in the tree which potentially overlap the query interval
        /// to the given collection.
        /// If the query interval is <tt>null</tt>, add all items in the tree.
        /// </summary>
        /// <param name="interval">A query interval, or <c>null</c></param>
        /// <param name="resultItems">The candidate items found</param>
        public void AddAllItemsFromOverlapping(Interval interval, ICollection<T> resultItems)
        {
            if (interval != null && !IsSearchMatch(interval))
                return;

            // some of these may not actually overlap - this is allowed by the bintree contract
            //resultItems.AddAll(items);
            foreach (T o in _items)
                resultItems.Add(o);

            if (Subnode[0] != null) Subnode[0].AddAllItemsFromOverlapping(interval, resultItems);
            if (Subnode[1] != null) Subnode[1].AddAllItemsFromOverlapping(interval, resultItems);
        }

        /// <summary>
        /// Returns the index of the subnode that wholely contains the given interval.
        /// If none does, returns -1.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="centre"></param>
        public static int GetSubnodeIndex(Interval interval, double centre)
        {
            int subnodeIndex = -1;
            if (interval.Min >= centre)
                subnodeIndex = 1;
            if (interval.Max <= centre)
                subnodeIndex = 0;
            return subnodeIndex;
        }

        /// <summary>
        /// Removes a single item from this subtree.
        /// </summary>
        /// <param name="itemInterval">The envelope containing the item</param>
        /// <param name="item">The item to remove</param>
        /// <returns><c>true</c> if the item was found and removed</returns>
        public bool Remove(Interval itemInterval, T item)
        {
            // use interval to restrict nodes scanned
            if (!IsSearchMatch(itemInterval))
                return false;

            bool found = false;
            for (int i = 0; i < 2; i++)
            {
                if (Subnode[i] != null)
                {
                    found = Subnode[i].Remove(itemInterval, item);
                    if (found)
                    {
                        // trim subtree if empty
                        if (Subnode[i].IsPrunable)
                            Subnode[i] = null;
                        break;
                    }
                }
            }
            // if item was found lower down, don't need to search for it here
            if (found) return true;

            // otherwise, try and remove the item from the list of items in this node
            found = _items.Remove(item);
            return found;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        protected abstract bool IsSearchMatch(Interval interval);

        #endregion
    }
}