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
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Index.QuadTree
{
    /// <summary>
    /// Represents a node of a <c>Quadtree</c>.  Nodes contain
    /// items which have a spatial extent corresponding to the node's position
    /// in the quadtree.
    /// </summary>
    [Serializable]
    public class Node<T> : NodeBase<T>
    {
        #region Fields

        //private readonly Coordinate _centre;
        private readonly double _centreX, _centreY;
        private readonly int _level;

        #endregion

        #region Constructors

        /// <summary>
        ///
        /// </summary>
        /// <param name="env"></param>
        /// <param name="level"></param>
        public Node(Envelope env, int level)
        {
            Envelope = env;
            _level = level;
            _centreX = (env.Minimum.X + env.Maximum.X) / 2;
            _centreY = (env.Minimum.Y + env.Maximum.Y) / 2;
        }

        #endregion

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public Envelope Envelope { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="node"></param>
        /// <param name="addEnv"></param>
        /// <returns></returns>
        public static Node<T> CreateExpanded(Node<T> node, Envelope addEnv)
        {
            Envelope expandEnv = new Envelope(addEnv);
            if (node != null)
                expandEnv.ExpandToInclude(node.Envelope);

            var largerNode = CreateNode(expandEnv);
            if (node != null) 
                largerNode.InsertNode(node);
            return largerNode;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public static Node<T> CreateNode(Envelope env)
        {
            Key key = new Key(env);
            var node = new Node<T>(key.Envelope, key.Level);
            return node;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Node<T> CreateSubnode(int index)
        {
            // create a new subquad in the appropriate quadrant
            double minx = 0.0;
            double maxx = 0.0;
            double miny = 0.0;
            double maxy = 0.0;

            switch (index)
            {
                case 0:
                    minx = Envelope.Minimum.X;
                    maxx = _centreX;
                    miny = Envelope.Minimum.Y;
                    maxy = _centreY;
                    break;
                case 1:
                    minx = _centreX;
                    maxx = Envelope.Maximum.X;
                    miny = Envelope.Minimum.Y;
                    maxy = _centreY;
                    break;
                case 2:
                    minx = Envelope.Minimum.X;
                    maxx = _centreX;
                    miny = _centreY;
                    maxy = Envelope.Maximum.Y;
                    break;
                case 3:
                    minx = _centreX;
                    maxx = Envelope.Maximum.X;
                    miny = _centreY;
                    maxy = Envelope.Maximum.Y;
                    break;
                default:
                    break;
            }
            Envelope sqEnv = new Envelope(minx, maxx, miny, maxy);
            var node = new Node<T>(sqEnv, _level - 1);
            return node;
        }

        /// <summary>
        /// Returns the smallest <i>existing</i>
        /// node containing the envelope.
        /// </summary>
        /// <param name="searchEnv"></param>
        public NodeBase<T> Find(Envelope searchEnv)
        {
            int subnodeIndex = GetSubnodeIndex(searchEnv, _centreX, _centreY);
            if (subnodeIndex == -1)
                return this;
            if (Subnode[subnodeIndex] != null) 
            {
                // query lies in subquad, so search it
                var node = Subnode[subnodeIndex];
                return node.Find(searchEnv);
            }
            // no existing subquad, so return this one anyway
            return this;
        }

        /// <summary> 
        /// Returns the subquad containing the envelope <paramref name="searchEnv"/>.
        /// Creates the subquad if
        /// it does not already exist.
        /// </summary>
        /// <param name="searchEnv">The envelope to search for</param>
        /// <returns>The subquad containing the search envelope.</returns>
        public Node<T> GetNode(Envelope searchEnv)
        {
            int subnodeIndex = GetSubnodeIndex(searchEnv, _centreX, _centreY);            
            // if subquadIndex is -1 searchEnv is not contained in a subquad
            if (subnodeIndex != -1) 
            {
                // create the quad if it does not exist
                var node = GetSubnode(subnodeIndex);
                // recursively search the found/created quad
                return node.GetNode(searchEnv);
            }
            return this;
        }

        /// <summary>
        /// Get the subquad for the index.
        /// If it doesn't exist, create it.
        /// </summary>
        /// <param name="index"></param>
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
        public void InsertNode(Node<T> node)
        {
            Assert.IsTrue(Envelope == null || Envelope.Contains(node.Envelope));        
            int index = GetSubnodeIndex(node.Envelope, _centreX, _centreY);        
            if (node._level == _level - 1)             
                Subnode[index] = node;                    
            else 
            {
                // the quad is not a direct child, so make a new child quad to contain it
                // and recursively insert the quad
                var childNode = CreateSubnode(index);
                childNode.InsertNode(node);
                Subnode[index] = childNode;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <returns></returns>
        protected override bool IsSearchMatch(Envelope searchEnv)
        {
            return Envelope.Intersects(searchEnv);
        }

        #endregion
    }
}