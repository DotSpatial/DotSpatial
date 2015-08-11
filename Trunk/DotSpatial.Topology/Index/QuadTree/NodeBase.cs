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
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Index.QuadTree
{
    /// <summary>
    /// The base class for nodes in a <c>Quadtree</c>.
    /// </summary>
    [Serializable]
    public abstract class NodeBase<T>
    {
        #region Fields

        /// <summary>
        /// subquads are numbered as follows:
        /// 2 | 3
        /// --+--
        /// 0 | 1
        /// </summary>
        protected Node<T>[] Subnode = new Node<T>[4];

        /// <summary>
        /// 
        /// </summary>
        private List<T> _items = new List<T>();

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                int subSize = 0;
                for (int i = 0; i < 4; i++)
                    if (Subnode[i] != null)
                        subSize += Subnode[i].Count;
                return subSize + _items.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Depth
        {
            get
            {
                int maxSubDepth = 0;
                for (int i = 0; i < 4; i++)
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
        /// 
        /// </summary>
        public bool HasChildren
        {
            get
            {
                for (int i = 0; i < 4; i++)
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
        public bool HasItems
        {
            get
            {
                return _items.Count != 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                bool isEmpty = _items.Count == 0;
                for (int i = 0; i < 4; i++)
                    if (Subnode[i] != null)
                        if (!Subnode[i].IsEmpty)
                            isEmpty = false;
                return isEmpty;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsPrunable
        {
            get
            {
                return !(HasChildren || HasItems);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int NodeCount
        {
            get
            {
                int subSize = 0;
                for (int i = 0; i < 4; i++)
                    if (Subnode[i] != null)
                        subSize += Subnode[i].Count;
                return subSize + 1;
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
            protected set { _items = (List<T>)value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            _items.Add(item);
        }

        /// <summary>
        /// Insert items in <c>this</c> into the parameter!
        /// </summary>
        /// <param name="resultItems">IList for adding items.</param>
        /// <returns>Parameter IList with <c>this</c> items.</returns>
        public IList<T> AddAllItems(ref IList<T> resultItems)
        {
            // this node may have items as well as subnodes (since items may not
            // be wholely contained in any single subnode
            // resultItems.addAll(this.items);
            foreach (T o in _items)
                resultItems.Add(o);
            for (int i = 0; i < 4; i++)
                if (Subnode[i] != null)
                    Subnode[i].AddAllItems(ref resultItems);
            return resultItems;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <param name="resultItems"></param>
        public void AddAllItemsFromOverlapping(Envelope searchEnv, ref IList<T> resultItems)
        {
            if (!IsSearchMatch(searchEnv))
                return;

            // this node may have items as well as subnodes (since items may not
            // be wholely contained in any single subnode
            foreach (T o in _items)
                resultItems.Add(o);

            for (int i = 0; i < 4; i++)
                if (Subnode[i] != null)
                    Subnode[i].AddAllItemsFromOverlapping(searchEnv, ref resultItems);
        }

        /// <summary> 
        /// Gets the index of the subquad that wholly contains the given envelope.
        /// If none does, returns -1.
        /// </summary>
        /// <returns>The index of the subquad that wholly contains the given envelope <br/>
        /// or -1 if no subquad wholly contains the envelope</returns>
        public static int GetSubnodeIndex(Envelope env, double centreX, double centreY)
        {
            int subnodeIndex = -1;
            if (env.Minimum.X >= centreX)
            {
                if (env.Minimum.Y >= centreY) subnodeIndex = 3;
                if (env.Maximum.Y <= centreY) subnodeIndex = 1;
            }
            if (env.Maximum.X <= centreX)
            {
                if (env.Minimum.Y >= centreY) subnodeIndex = 2;
                if (env.Maximum.Y <= centreY) subnodeIndex = 0;
            }
            return subnodeIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <returns></returns>
        protected abstract bool IsSearchMatch(Envelope searchEnv);

        /// <summary> 
        /// Removes a single item from this subtree.
        /// </summary>
        /// <param name="itemEnv">The envelope containing the item.</param>
        /// <param name="item">The item to remove.</param>
        /// <returns><c>true</c> if the item was found and removed.</returns>
        public bool Remove(Envelope itemEnv, T item)
        {
            // use envelope to restrict nodes scanned
            if (!IsSearchMatch(itemEnv)) return false;

            bool found = false;
            for (int i = 0; i < 4; i++)
            {
                if (Subnode[i] != null)
                {
                    found = Subnode[i].Remove(itemEnv, item);
                    if (found)
                    {
                        // trim subtree if empty
                        if (Subnode[i].IsPrunable) Subnode[i] = null;
                        break;
                    }
                }
            }

            // if item was found lower down, don't need to search for it here
            if (found) return true;

            // otherwise, try and remove the item from the list of items in this node
            if (_items.Contains(item))
            {
                _items.Remove(item);
                found = true;
            }
            return found;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <param name="visitor"></param>
        public void Visit(Envelope searchEnv, IItemVisitor<T> visitor)
        {
            if (!IsSearchMatch(searchEnv)) return;

            // this node may have items as well as subnodes (since items may not
            // be wholely contained in any single subnode
            VisitItems(searchEnv, visitor);

            for (int i = 0; i < 4; i++)
                if (Subnode[i] != null)
                    Subnode[i].Visit(searchEnv, visitor);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <param name="visitor"></param>
        private void VisitItems(Envelope searchEnv, IItemVisitor<T> visitor)
        {
            // would be nice to filter items based on search envelope, but can't until they contain an envelope
            for (IEnumerator<T> i = _items.GetEnumerator(); i.MoveNext(); )
                visitor.VisitItem(i.Current);
        }

        #endregion
    }
}