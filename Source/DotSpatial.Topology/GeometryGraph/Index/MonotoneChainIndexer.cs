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
using System.Collections;
using System.Collections.Generic;

namespace DotSpatial.Topology.GeometriesGraph.Index
{
    /// <summary>
    /// MonotoneChains are a way of partitioning the segments of an edge to
    /// allow for fast searching of intersections.
    /// They have the following properties:
    /// the segments within a monotone chain will never intersect each other, and
    /// the envelope of any contiguous subset of the segments in a monotone chain
    /// is simply the envelope of the endpoints of the subset.
    /// Property 1 means that there is no need to test pairs of segments from within
    /// the same monotone chain for intersection.
    /// Property 2 allows
    /// binary search to be used to find the intersection points of two monotone chains.
    /// For many types of real-world data, these properties eliminate a large number of
    /// segment comparisons, producing substantial speed gains.
    /// </summary>
    public class MonotoneChainIndexer
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int[] ToIntArray(IList list)
        {
            int[] array = new int[list.Count];
            for (int i = 0; i < array.Length; i++)
                array[i] = Convert.ToInt32(list[i]);
            return array;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public virtual int[] GetChainStartIndices(IList<Coordinate> pts)
        {
            // find the startpoint (and endpoints) of all monotone chains in this edge
            int start = 0;
            IList startIndexList = new ArrayList();
            startIndexList.Add(start);
            do
            {
                int last = FindChainEnd(pts, start);
                startIndexList.Add(last);
                start = last;
            }
            while (start < pts.Count - 1);
            // copy list to an array of ints, for efficiency
            int[] startIndex = ToIntArray(startIndexList);
            return startIndex;
        }

        /// <returns>
        /// The index of the last point in the monotone chain.
        /// </returns>
        private static int FindChainEnd(IList<Coordinate> pts, int start)
        {
            // determine quadrant for chain
            int chainQuad = QuadrantOp.Quadrant(pts[start], pts[start + 1]);
            int last = start + 1;
            while (last < pts.Count)
            {
                // compute quadrant for next possible segment in chain
                int quad = QuadrantOp.Quadrant(pts[last - 1], pts[last]);
                if (quad != chainQuad)
                    break;
                last++;
            }
            return last - 1;
        }
    }
}