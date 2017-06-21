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
using DotSpatial.Topology.Algorithm;

namespace DotSpatial.Topology.Noding
{
    /// <summary>
    /// Nodes a set of <see cref="SegmentString" />s completely.
    /// The set of <see cref="SegmentString" />s is fully noded;
    /// i.e. noding is repeated until no further intersections are detected.
    /// <para>
    /// Iterated noding using a <see cref="PrecisionModelType.Floating" /> precision model is not guaranteed to converge,
    /// due to roundoff error. This problem is detected and an exception is thrown.
    /// Clients can choose to rerun the noding using a lower precision model.
    /// </para>
    /// </summary>
    public class IteratedNoder : INoder
    {
        /// <summary>
        ///
        /// </summary>
        public const int MAX_ITERATIONS = 5;

        private readonly LineIntersector _li;
        private int _maxIter = MAX_ITERATIONS;
        private IList _nodedSegStrings;

        /// <summary>
        /// Initializes a new instance of the <see cref="IteratedNoder"/> class.
        /// </summary>
        /// <param name="pm"></param>
        public IteratedNoder(PrecisionModel pm)
        {
            _li = new RobustLineIntersector();
            _li.PrecisionModel = pm;
        }

        /// <summary>
        /// Gets/Sets the maximum number of noding iterations performed before
        /// the noding is aborted. Experience suggests that this should rarely need to be changed
        /// from the default. The default is <see cref="MAX_ITERATIONS" />.
        /// </summary>
        public int MaximumIterations
        {
            get
            {
                return _maxIter;
            }
            set
            {
                _maxIter = value;
            }
        }

        #region INoder Members

        /// <summary>
        /// Returns a <see cref="IList"/> of fully noded <see cref="SegmentString"/>s.
        /// The <see cref="SegmentString"/>s have the same context as their parent.
        /// </summary>
        /// <returns></returns>
        public IList GetNodedSubstrings()
        {
            return _nodedSegStrings;
        }

        /// <summary>
        /// Fully nodes a list of <see cref="SegmentString" />s, i.e. peforms noding iteratively
        /// until no intersections are found between segments.
        /// Maintains labelling of edges correctly through the noding.
        /// </summary>
        /// <param name="segStrings">A collection of SegmentStrings to be noded.</param>
        /// <exception cref="TopologyException">If the iterated noding fails to converge.</exception>
        public void ComputeNodes(IList segStrings)
        {
            int[] numInteriorIntersections = new int[1];
            _nodedSegStrings = segStrings;
            int nodingIterationCount = 0;
            int lastNodesCreated = -1;
            do
            {
                Node(_nodedSegStrings, numInteriorIntersections);
                nodingIterationCount++;
                int nodesCreated = numInteriorIntersections[0];

                /*
                 * Fail if the number of nodes created is not declining.
                 * However, allow a few iterations at least before doing this
                 */
                if (lastNodesCreated > 0
                    && nodesCreated >= lastNodesCreated
                    && nodingIterationCount > _maxIter)
                    throw new TopologyException("Iterated noding failed to converge after "
                                                + nodingIterationCount + " iterations");
                lastNodesCreated = nodesCreated;
            }
            while (lastNodesCreated > 0);
        }

        #endregion

        /// <summary>
        /// Node the input segment strings once
        /// and create the split edges between the nodes.
        /// </summary>
        /// <param name="segStrings"></param>
        /// <param name="numInteriorIntersections"></param>
        private void Node(IList segStrings, int[] numInteriorIntersections)
        {
            IntersectionAdder si = new IntersectionAdder(_li);
            McIndexNoder noder = new McIndexNoder(si);
            noder.ComputeNodes(segStrings);
            _nodedSegStrings = noder.GetNodedSubstrings();
            numInteriorIntersections[0] = si.NumInteriorIntersections;
        }
    }
}