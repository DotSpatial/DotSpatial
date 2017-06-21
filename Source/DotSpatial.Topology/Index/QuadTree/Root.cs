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
    /// QuadRoot is the root of a single Quadtree.
    /// It is centred at the origin,
    /// and does not have a defined extent.
    /// </summary>
    public class Root : NodeBase
    {
        // the singleton root quad is centred at the origin.
        private static readonly Coordinate Origin = new Coordinate(0.0, 0.0);

        /// <summary>
        /// Insert an item into the quadtree this is the root of.
        /// </summary>
        public virtual void Insert(IEnvelope itemEnv, object item)
        {
            int index = GetSubnodeIndex(itemEnv, Origin);
            // if index is -1, itemEnv must cross the X or Y axis.
            if (index == -1)
            {
                Add(item);
                return;
            }
            /*
            * the item must be contained in one quadrant, so insert it into the
            * tree for that quadrant (which may not yet exist)
            */
            Node node = Nodes[index];
            /*
            *  If the subquad doesn't exist or this item is not contained in it,
            *  have to expand the tree upward to contain the item.
            */
            if (node == null || !node.Envelope.Contains(itemEnv))
            {
                Node largerNode = Node.CreateExpanded(node, itemEnv);
                Nodes[index] = largerNode;
            }
            /*
            * At this point we have a subquad which exists and must contain
            * contains the env for the item.  Insert the item into the tree.
            */
            InsertContained(Nodes[index], itemEnv, item);
        }

        /// <summary>
        /// Insert an item which is known to be contained in the tree rooted at
        /// the given QuadNode root.  Lower levels of the tree will be created
        /// if necessary to hold the item.
        /// </summary>
        private static void InsertContained(Node tree, IEnvelope itemEnv, object item)
        {
            Assert.IsTrue(tree.Envelope.Contains(itemEnv));
            /*
            * Do NOT create a new quad for zero-area envelopes - this would lead
            * to infinite recursion. Instead, use a heuristic of simply returning
            * the smallest existing quad containing the query
            */
            bool isZeroX = IntervalSize.IsZeroWidth(itemEnv.Minimum.X, itemEnv.Maximum.X);
            bool isZeroY = IntervalSize.IsZeroWidth(itemEnv.Minimum.X, itemEnv.Maximum.X);
            NodeBase node;
            if (isZeroX || isZeroY)
                node = tree.Find(itemEnv);
            else node = tree.GetNode(itemEnv);
            node.Add(item);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <returns></returns>
        protected override bool IsSearchMatch(IEnvelope searchEnv)
        {
            return true;
        }
    }
}