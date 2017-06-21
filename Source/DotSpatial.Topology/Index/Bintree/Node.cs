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

using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Index.Bintree
{
    /// <summary>
    /// A node of a <c>Bintree</c>.
    /// </summary>
    public class Node : NodeBase
    {
        private readonly double _centre;
        private readonly Interval _interval;
        private readonly int _level;

        /// <summary>
        ///
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="level"></param>
        public Node(Interval interval, int level)
        {
            _interval = interval;
            _level = level;
            _centre = (interval.Min + interval.Max) / 2;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Interval Interval
        {
            get
            {
                return _interval;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="itemInterval"></param>
        /// <returns></returns>
        public static Node CreateNode(Interval itemInterval)
        {
            Key key = new Key(itemInterval);

            Node node = new Node(key.Interval, key.Level);
            return node;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="node"></param>
        /// <param name="addInterval"></param>
        /// <returns></returns>
        public static Node CreateExpanded(Node node, Interval addInterval)
        {
            Interval expandInt = new Interval(addInterval);
            if (node != null) expandInt.ExpandToInclude(node._interval);
            Node largerNode = CreateNode(expandInt);
            if (node != null) largerNode.Insert(node);
            return largerNode;
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

        /// <summary>
        /// Returns the subnode containing the envelope.
        /// Creates the node if
        /// it does not already exist.
        /// </summary>
        /// <param name="searchInterval"></param>
        public virtual Node GetNode(Interval searchInterval)
        {
            int subnodeIndex = GetSubnodeIndex(searchInterval, _centre);
            // if index is -1 searchEnv is not contained in a subnode
            if (subnodeIndex != -1)
            {
                // create the node if it does not exist
                Node node = GetSubnode(subnodeIndex);
                // recursively search the found/created node
                return node.GetNode(searchInterval);
            }
            return this;
        }

        /// <summary>
        /// Returns the smallest existing
        /// node containing the envelope.
        /// </summary>
        /// <param name="searchInterval"></param>
        public virtual NodeBase Find(Interval searchInterval)
        {
            int subnodeIndex = GetSubnodeIndex(searchInterval, _centre);
            if (subnodeIndex == -1)
                return this;
            if (Nodes[subnodeIndex] != null)
            {
                // query lies in subnode, so search it
                Node node = Nodes[subnodeIndex];
                return node.Find(searchInterval);
            }
            // no existing subnode, so return this one anyway
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="node"></param>
        public virtual void Insert(Node node)
        {
            Assert.IsTrue(_interval == null || _interval.Contains(node.Interval));
            int index = GetSubnodeIndex(node._interval, _centre);
            if (node._level == _level - 1)
                Nodes[index] = node;
            else
            {
                // the node is not a direct child, so make a new child node to contain it
                // and recursively insert the node
                Node childNode = CreateSubnode(index);
                childNode.Insert(node);
                Nodes[index] = childNode;
            }
        }

        /// <summary>
        /// Get the subnode for the index.
        /// If it doesn't exist, create it.
        /// </summary>
        private Node GetSubnode(int index)
        {
            if (Nodes[index] == null)
                Nodes[index] = CreateSubnode(index);
            return Nodes[index];
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Node CreateSubnode(int index)
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
                default:
                    break;
            }
            Interval subInt = new Interval(min, max);
            Node node = new Node(subInt, _level - 1);
            return node;
        }
    }
}