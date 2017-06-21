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

using DotSpatial.Topology.Index.Quadtree;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Index.Bintree
{
    /// <summary>
    /// The root node of a single <c>Bintree</c>.
    /// It is centred at the origin,
    /// and does not have a defined extent.
    /// </summary>
    public class Root : NodeBase
    {
        // the singleton root node is centred at the origin.
        private const double ORIGIN = 0.0;

        /// <summary>
        /// Insert an item into the tree this is the root of.
        /// </summary>
        /// <param name="itemInterval"></param>
        /// <param name="item"></param>
        public virtual void Insert(Interval itemInterval, object item)
        {
            int index = GetSubnodeIndex(itemInterval, ORIGIN);
            // if index is -1, itemEnv must contain the origin.
            if (index == -1)
            {
                Add(item);
                return;
            }
            /*
            * the item must be contained in one interval, so insert it into the
            * tree for that interval (which may not yet exist)
            */
            Node node = Nodes[index];
            /*
            *  If the subnode doesn't exist or this item is not contained in it,
            *  have to expand the tree upward to contain the item.
            */

            if (node == null || !node.Interval.Contains(itemInterval))
            {
                Node largerNode = Node.CreateExpanded(node, itemInterval);
                Nodes[index] = largerNode;
            }
            /*
            * At this point we have a subnode which exists and must contain
            * contains the env for the item.  Insert the item into the tree.
            */
            InsertContained(Nodes[index], itemInterval, item);
        }

        /// <summary>
        /// Insert an item which is known to be contained in the tree rooted at
        /// the given Node.  Lower levels of the tree will be created
        /// if necessary to hold the item.
        /// </summary>
        /// <param name="tree"></param>
        /// <param name="itemInterval"></param>
        /// <param name="item"></param>
        private static void InsertContained(Node tree, Interval itemInterval, object item)
        {
            Assert.IsTrue(tree.Interval.Contains(itemInterval));
            /*
            * Do NOT create a new node for zero-area intervals - this would lead
            * to infinite recursion. Instead, use a heuristic of simply returning
            * the smallest existing node containing the query
            */
            bool isZeroArea = IntervalSize.IsZeroWidth(itemInterval.Min, itemInterval.Max);
            NodeBase node;
            if (isZeroArea)
                node = tree.Find(itemInterval);
            else node = tree.GetNode(itemInterval);
            node.Add(item);
        }

        /// <summary>
        /// The root node matches all searches.
        /// </summary>
        /// <param name="interval"></param>
        protected override bool IsSearchMatch(Interval interval)
        {
            return true;
        }
    }
}