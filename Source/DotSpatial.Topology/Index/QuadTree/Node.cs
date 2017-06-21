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

namespace DotSpatial.Topology.Index.Quadtree
{
    /// <summary>
    /// Represents a node of a <c>Quadtree</c>.  Nodes contain
    /// items which have a spatial extent corresponding to the node's position
    /// in the quadtree.
    /// </summary>
    public class Node : NodeBase
    {
        private readonly Coordinate _centre;
        private readonly IEnvelope _env;
        private readonly int _level;

        /// <summary>
        ///
        /// </summary>
        /// <param name="env"></param>
        /// <param name="level"></param>
        public Node(IEnvelope env, int level)
        {
            _env = env;
            _level = level;
            _centre = new Coordinate();
            _centre.X = (env.Minimum.X + env.Maximum.X) / 2;
            _centre.Y = (env.Minimum.Y + env.Maximum.Y) / 2;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IEnvelope Envelope
        {
            get
            {
                return _env;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public static Node CreateNode(IEnvelope env)
        {
            Key key = new Key(env);
            Node node = new Node(key.Envelope, key.Level);
            return node;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="node"></param>
        /// <param name="addEnv"></param>
        /// <returns></returns>
        public static Node CreateExpanded(Node node, IEnvelope addEnv)
        {
            Envelope expandEnv = new Envelope(addEnv);
            if (node != null)
                expandEnv.ExpandToInclude(node._env);

            Node largerNode = CreateNode(expandEnv);
            if (node != null)
                largerNode.InsertNode(node);
            return largerNode;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <returns></returns>
        protected override bool IsSearchMatch(IEnvelope searchEnv)
        {
            return _env.Intersects(searchEnv);
        }

        /// <summary>
        /// Returns the subquad containing the envelope.
        /// Creates the subquad if
        /// it does not already exist.
        /// </summary>
        /// <param name="searchEnv"></param>
        public virtual Node GetNode(IEnvelope searchEnv)
        {
            int subnodeIndex = GetSubnodeIndex(searchEnv, _centre);
            // if subquadIndex is -1 searchEnv is not contained in a subquad
            if (subnodeIndex != -1)
            {
                // create the quad if it does not exist
                Node node = GetSubnode(subnodeIndex);
                // recursively search the found/created quad
                return node.GetNode(searchEnv);
            }
            return this;
        }

        /// <summary>
        /// Returns the smallest <i>existing</i>
        /// node containing the envelope.
        /// </summary>
        /// <param name="searchEnv"></param>
        public virtual NodeBase Find(IEnvelope searchEnv)
        {
            int subnodeIndex = GetSubnodeIndex(searchEnv, _centre);
            if (subnodeIndex == -1)
                return this;
            if (Nodes[subnodeIndex] != null)
            {
                // query lies in subquad, so search it
                Node node = Nodes[subnodeIndex];
                return node.Find(searchEnv);
            }
            // no existing subquad, so return this one anyway
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="node"></param>
        public virtual void InsertNode(Node node)
        {
            Assert.IsTrue(_env == null || _env.Contains(node.Envelope));
            int index = GetSubnodeIndex(node._env, _centre);
            if (node._level == _level - 1)
                Nodes[index] = node;
            else
            {
                // the quad is not a direct child, so make a new child quad to contain it
                // and recursively insert the quad
                Node childNode = CreateSubnode(index);
                childNode.InsertNode(node);
                Nodes[index] = childNode;
            }
        }

        /// <summary>
        /// Get the subquad for the index.
        /// If it doesn't exist, create it.
        /// </summary>
        /// <param name="index"></param>
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
            // create a new subquad in the appropriate quadrant
            double minx = 0.0;
            double maxx = 0.0;
            double miny = 0.0;
            double maxy = 0.0;

            switch (index)
            {
                case 0:
                    minx = _env.Minimum.X;
                    maxx = _centre.X;
                    miny = _env.Minimum.Y;
                    maxy = _centre.Y;
                    break;
                case 1:
                    minx = _centre.X;
                    maxx = _env.Maximum.X;
                    miny = _env.Minimum.Y;
                    maxy = _centre.Y;
                    break;
                case 2:
                    minx = _env.Minimum.X;
                    maxx = _centre.X;
                    miny = _centre.Y;
                    maxy = _env.Maximum.Y;
                    break;
                case 3:
                    minx = _centre.X;
                    maxx = _env.Maximum.X;
                    miny = _centre.Y;
                    maxy = _env.Maximum.Y;
                    break;
                default:
                    break;
            }
            Envelope sqEnv = new Envelope(minx, maxx, miny, maxy);
            Node node = new Node(sqEnv, _level - 1);
            return node;
        }
    }
}