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
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Index.Bintree
{
    /// <summary>
    /// A node of a <c>Bintree</c>.
    /// </summary>
    [Serializable]
    public class Node<T> : NodeBase<T>
    {
        #region Fields

        private readonly double _centre;
        private readonly Interval _interval;
        private readonly int _level;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new node instance
        /// </summary>
        /// <param name="interval">The node's interval</param>
        /// <param name="level">The node's level</param>
        public Node(Interval interval, int level)
        {
            _interval = interval;
            _level = level;
            _centre = (interval.Min + interval.Max) / 2;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the node's <see cref="Interval"/>
        /// </summary>
        public virtual Interval Interval
        {
            get
            {
                return _interval;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a larger node, that contains both <paramref name="node.Interval"/> and <paramref name="addInterval"/>
        /// If <paramref name="node"/> is <c>null</c>, a node for <paramref name="addInterval"/> is created.
        /// </summary>
        /// <param name="node">The original node</param>
        /// <param name="addInterval">The additional interval</param>
        /// <returns>A new node</returns>
        public static Node<T> CreateExpanded(Node<T> node, Interval addInterval)
        {
            var expandInt = new Interval(addInterval);
            if (node != null) expandInt.ExpandToInclude(node._interval);
            Node<T> largerNode = CreateNode(expandInt);
            if (node != null) largerNode.Insert(node);
            return largerNode;
        }

        /// <summary>
        /// Creates a node
        /// </summary>
        /// <param name="itemInterval">The interval of the node item</param>
        /// <returns>A new node</returns>
        public static Node<T> CreateNode(Interval itemInterval)
        {
            var key = new Key(itemInterval);
            var node = new Node<T>(key.Interval, key.Level);
            return node;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Node<T> CreateSubnode(int index)
        {
            // create a new subnode in the appropriate interval
            double min = 0.0;
            double max = 0.0;

            switch (index)
            {
                case 0:
                    min = _interval.Min;
                    max = _centre;
                    break;
                case 1:
                    min = _centre;
                    max = _interval.Max;
                    break;
            }
            var subInt = new Interval(min, max);
            //var subInt = Interval.Create(min, max);
            var node = new Node<T>(subInt, _level - 1);
            return node;
        }

        /// <summary>
        /// Returns the smallest existing
        /// node containing the envelope.
        /// </summary>
        /// <param name="searchInterval"></param>
        public NodeBase<T> Find(Interval searchInterval)
        {
            int subnodeIndex = GetSubnodeIndex(searchInterval, _centre);
            if (subnodeIndex == -1)
                return this;
            if (Subnode[subnodeIndex] != null)
            {
                // query lies in subnode, so search it
                Node<T> node = Subnode[subnodeIndex];
                return node.Find(searchInterval);
            }
            // no existing subnode, so return this one anyway
            return this;
        }

        /// <summary>
        /// Returns the subnode containing the envelope.
        /// Creates the node if
        /// it does not already exist.
        /// </summary>
        /// <param name="searchInterval"></param>
        public Node<T> GetNode(Interval searchInterval)
        {
            int subnodeIndex = GetSubnodeIndex(searchInterval, _centre);
            // if index is -1 searchEnv is not contained in a subnode
            if (subnodeIndex != -1)
            {
                // create the node if it does not exist
                Node<T> node = GetSubnode(subnodeIndex);
                // recursively search the found/created node
                return node.GetNode(searchInterval);
            }
            return this;
        }

        /// <summary>
        /// Get the subnode for the index.
        /// If it doesn't exist, create it.
        /// </summary>
        private Node<T> GetSubnode(int index)
        {
            if (Subnode[index] == null)
                Subnode[index] = CreateSubnode(index);
            return Subnode[index];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="node"></param>
        public void Insert(Node<T> node)
        {
            Assert.IsTrue(_interval == null || _interval.Contains(node.Interval));
            int index = GetSubnodeIndex(node._interval, _centre);
            if (node._level == _level - 1)
                Subnode[index] = node;
            else
            {
                // the node is not a direct child, so make a new child node to contain it
                // and recursively insert the node
                Node<T> childNode = CreateSubnode(index);
                childNode.Insert(node);
                Subnode[index] = childNode;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="itemInterval"></param>
        /// <returns></returns>
        protected override bool IsSearchMatch(Interval itemInterval)
        {
            return itemInterval.Overlaps(_interval);
        }

        #endregion
    }
}