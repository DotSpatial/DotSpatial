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

using System.Collections;

namespace DotSpatial.Topology.Index.Quadtree
{
    /// <summary>
    /// The base class for nodes in a <c>Quadtree</c>.
    /// </summary>
    public abstract class NodeBase
    {
        #region Private Variables

        private IList _items = new ArrayList();

        /// <summary>
        /// subquads are numbered as follows:
        /// 2 | 3
        /// --+--
        /// 0 | 1
        /// </summary>
        private Node[] _subnode = new Node[4];

        #endregion

        #region Methods

        /// <summary>
        /// Adds a new item to this node.
        /// </summary>
        /// <param name="item">The item to add to this node</param>
        public virtual void Add(object item)
        {
            _items.Add(item);
        }

        /// <summary>
        /// Insert items in <c>this</c> into the parameter!
        /// </summary>
        /// <param name="resultItems">IList for adding items.</param>
        /// <returns>Parameter IList with <c>this</c> items.</returns>
        public virtual IList AddAllItems(ref IList resultItems)
        {
            // this node may have items as well as subnodes (since items may not
            // be wholely contained in any single subnode
            // resultItems.addAll(this.items);
            foreach (object o in _items)
                resultItems.Add(o);
            for (int i = 0; i < 4; i++)
                if (_subnode[i] != null)
                    _subnode[i].AddAllItems(ref resultItems);
            return resultItems;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <param name="resultItems"></param>
        public virtual void AddAllItemsFromOverlapping(IEnvelope searchEnv, ref IList resultItems)
        {
            if (!IsSearchMatch(searchEnv))
                return;

            // this node may have items as well as subnodes (since items may not
            // be wholely contained in any single subnode
            // resultItems.addAll(items);
            foreach (object o in _items)
                resultItems.Add(o);

            for (int i = 0; i < 4; i++)
                if (_subnode[i] != null)
                    _subnode[i].AddAllItemsFromOverlapping(searchEnv, ref resultItems);
        }

        /// <summary>
        /// Removes a single item from this subtree.
        /// </summary>
        /// <param name="itemEnv">The envelope containing the item.</param>
        /// <param name="item">The item to remove.</param>
        /// <returns><c>true</c> if the item was found and removed.</returns>
        public virtual bool Remove(IEnvelope itemEnv, object item)
        {
            // use envelope to restrict nodes scanned
            if (!IsSearchMatch(itemEnv))
                return false;

            bool found = false;
            for (int i = 0; i < 4; i++)
            {
                if (_subnode[i] != null)
                {
                    found = _subnode[i].Remove(itemEnv, item);
                    if (found)
                    {
                        // trim subtree if empty
                        if (_subnode[i].IsPrunable)
                            _subnode[i] = null;
                        break;
                    }
                }
            }

            // if item was found lower down, don't need to search for it here
            if (found)
                return true;

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
        public virtual void Visit(IEnvelope searchEnv, IItemVisitor visitor)
        {
            if (!IsSearchMatch(searchEnv))
                return;

            // this node may have items as well as subnodes (since items may not
            // be wholely contained in any single subnode
            VisitItems(visitor);

            for (int i = 0; i < 4; i++)
                if (_subnode[i] != null)
                    _subnode[i].Visit(searchEnv, visitor);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Because more than one item can be stored in each node, this returns the total count of
        /// items contained by this node and its child nodes.
        /// </summary>
        public virtual int Count
        {
            get
            {
                int subSize = 0;
                for (int i = 0; i < 4; i++)
                    if (_subnode[i] != null)
                        subSize += _subnode[i].Count;
                return subSize + _items.Count;
            }
        }

        /// <summary>
        /// Gets an integer representing how deem the deepest child of this node extends.
        /// </summary>
        public virtual int Depth
        {
            get
            {
                int maxSubDepth = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (_subnode[i] != null)
                    {
                        int sqd = _subnode[i].Depth;
                        if (sqd > maxSubDepth)
                            maxSubDepth = sqd;
                    }
                }
                return maxSubDepth + 1;
            }
        }

        /// <summary>
        /// Gets or sets the array of 4 nodes represnting the spatial quadrants being used as children
        /// 2 | 3
        /// --+--
        /// 0 | 1
        /// </summary>
        public Node[] Nodes
        {
            get { return _subnode; }
            set { _subnode = value; }
        }

        /// <summary>
        /// Gets a boolean indicating whehter or not this node links to any nodes below it in the tree
        /// </summary>
        public virtual bool HasChildren
        {
            get
            {
                for (int i = 0; i < 4; i++)
                {
                    if (_subnode[i] != null)
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Each node can store multiple items.  This tests whether or not there are items in this node.
        /// </summary>
        public virtual bool HasItems
        {
            get
            {
                // return !items.IsEmpty;
                if (_items.Count == 0)
                    return false;
                return true;
            }
        }

        /// <summary>
        /// If this node has no childern or items, then it can be pruned
        /// </summary>
        public virtual bool IsPrunable
        {
            get
            {
                return !(HasChildren || HasItems);
            }
        }

        /// <summary>
        /// If this node has an item, or if any of the children of this node has an item, then this is false.
        /// </summary>
        public virtual bool IsEmpty
        {
            get
            {
                bool isEmpty = true;
                // if (!items.isEmpty())
                if (_items.Count != 0)
                    isEmpty = false;
                for (int i = 0; i < 4; i++)
                    if (_subnode[i] != null)
                        if (!_subnode[i].IsEmpty)
                            isEmpty = false;
                return isEmpty;
            }
        }

        /// <summary>
        /// Gets or sets the list of items that are stored in this node
        /// </summary>
        public virtual IList Items
        {
            get { return _items; }
            set { _items = value; }
        }

        /// <summary>
        /// Gets an integer representing this node and the count of all of the children in its subnodes
        /// </summary>
        public virtual int NodeCount
        {
            get
            {
                int subSize = 0;
                for (int i = 0; i < 4; i++)
                    if (_subnode[i] != null)
                        subSize += _subnode[i].NodeCount;
                return subSize + 1;
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <returns></returns>
        protected abstract bool IsSearchMatch(IEnvelope searchEnv);

        #endregion

        #region Private Functions

        /// <summary>
        ///
        /// </summary>
        /// <param name="visitor"></param>
        private void VisitItems(IItemVisitor visitor)
        {
            // would be nice to filter items based on search envelope, but can't until they contain an envelope
            for (IEnumerator i = _items.GetEnumerator(); i.MoveNext(); )
                visitor.VisitItem(i.Current);
        }

        #endregion

        #region Static

        /// <summary>
        /// Returns the index of the subquad that wholly contains the given envelope.
        /// If none does, returns -1.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="centre"></param>
        public static int GetSubnodeIndex(IEnvelope env, Coordinate centre)
        {
            int subnodeIndex = -1;
            if (env.Minimum.X >= centre.X)
            {
                if (env.Minimum.Y >= centre.Y)
                    subnodeIndex = 3;
                if (env.Maximum.Y <= centre.Y)
                    subnodeIndex = 1;
            }
            if (env.Maximum.X <= centre.X)
            {
                if (env.Minimum.Y >= centre.Y)
                    subnodeIndex = 2;
                if (env.Maximum.Y <= centre.Y)
                    subnodeIndex = 0;
            }
            return subnodeIndex;
        }

        #endregion
    }
}