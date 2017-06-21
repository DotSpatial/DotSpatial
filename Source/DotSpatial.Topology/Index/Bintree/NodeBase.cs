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

namespace DotSpatial.Topology.Index.Bintree
{
    /// <summary>
    /// The base class for nodes in a <c>Bintree</c>.
    /// </summary>
    public abstract class NodeBase
    {
        #region Private Variables

        private readonly IList _items = new ArrayList();

        /// <summary>
        /// Subnodes are numbered as follows:
        /// 0 | 1
        /// </summary>
        private Node[] _subnode = new Node[2];

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified object to the items list for this node.  This will not affect child nodes.
        /// </summary>
        /// <param name="item">The object item to add to the list.</param>
        public virtual void Add(object item)
        {
            _items.Add(item);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public virtual IList AddAllItems(IList items)
        {
            // items.addAll(this.items);
            foreach (object o in _items)
                items.Add(o);
            for (int i = 0; i < 2; i++)
                if (_subnode[i] != null)
                    _subnode[i].AddAllItems(items);
            return items;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="resultItems"></param>
        /// <returns></returns>
        public virtual IList AddAllItemsFromOverlapping(Interval interval, IList resultItems)
        {
            if (!IsSearchMatch(interval))
                return _items;
            // resultItems.addAll(items);
            foreach (object o in _items)
                resultItems.Add(o);
            for (int i = 0; i < 2; i++)
                if (_subnode[i] != null)
                    _subnode[i].AddAllItemsFromOverlapping(interval, resultItems);
            return _items;
        }

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
                    if (_subnode[i] != null)
                        subSize += _subnode[i].Count;
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
        /// Gets a list of all the items currently stored in this node.  This does not include
        /// any items from child nodes.
        /// </summary>
        public virtual IList Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Gets the count of this node plus all of the child nodes
        /// </summary>
        public virtual int NodeCount
        {
            get
            {
                int subCount = 0;
                for (int i = 0; i < 2; i++)
                    if (_subnode[i] != null)
                        subCount += _subnode[i].NodeCount;
                return subCount + 1;
            }
        }

        /// <summary>
        /// Gets the array of all the sub-nodes below this node.
        /// </summary>
        public virtual Node[] Nodes
        {
            get { return _subnode; }
            protected set { _subnode = value; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="interval"></param>
        /// <returns></returns>
        protected abstract bool IsSearchMatch(Interval interval);

        #endregion

        #region Static

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

        #endregion
    }
}