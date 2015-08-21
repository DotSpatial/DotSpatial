// ********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
// The contents of this file are subject to the Mozilla Public License Version 1.1 (the "License");
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is protected by the GNU Lesser Public License
// http://dotspatial.codeplex.com/license and the sourcecode for the Net Topology Suite
// can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using DotSpatial.Topology.Geometries;
using Wintellect.PowerCollections;

namespace DotSpatial.Topology.Planargraph
{
    /// <summary>
    /// A map of <see cref="Node">nodes</see>, indexed by the coordinate of the node.
    /// </summary>   
    public class NodeMap
    {
        #region Fields

        private readonly IDictionary<Coordinate, Node> _nodeMap = new OrderedDictionary<Coordinate, Node>();

        #endregion

        #region Properties

        /// <summary>
        /// Returns the number of Nodes in this NodeMap.
        /// </summary>
        public int Count
        {
            get { return _nodeMap.Count; }
        }

        /// <summary>
        /// Returns the Nodes in this NodeMap, sorted in ascending order
        /// by angle with the positive x-axis.
        /// </summary>
        public ICollection<Node> Values
        {
            get { return _nodeMap.Values; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a node to the map, replacing any that is already at that location.
        /// </summary>
        /// <param name="n"></param>
        /// <returns>The added node.</returns>
        public Node Add(Node n)
        {
            _nodeMap[n.Coordinate] = n;            
            return n;
        }

        /// <summary>
        /// Returns the Node at the given location, or null if no Node was there.
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public Node Find(Coordinate coord)
        {
            Node res;
            if (_nodeMap.TryGetValue(coord, out res))
                return res;
            return null; 
        }

        /// <summary>
        /// Returns an Iterator over the Nodes in this NodeMap, sorted in ascending order
        /// by angle with the positive x-axis.
        /// </summary>
        public IEnumerator<Node> GetEnumerator()
        {
            return _nodeMap.Values.GetEnumerator();
        }

        /// <summary>
        /// Removes the Node at the given location, and returns it (or null if no Node was there).
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public Node Remove(Coordinate pt)
        {
            if (!_nodeMap.ContainsKey(pt))
                return null;
            Node node = _nodeMap[pt];
            _nodeMap.Remove(pt);
            return node;
        }

        #endregion
    }
}