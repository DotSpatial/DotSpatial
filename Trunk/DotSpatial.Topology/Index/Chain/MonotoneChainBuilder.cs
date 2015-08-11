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

using System.Collections.Generic;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.GeometriesGraph;

namespace DotSpatial.Topology.Index.Chain
{
    /// <summary> 
    /// Constructs <see cref="MonotoneChain"/>s
    /// for sequences of <see cref="Coordinate"/>s.
    /// </summary>
    public static class MonotoneChainBuilder
    {
        #region Methods

        /// <summary>
        /// Finds the index of the last point in a monotone chain starting at a given point.
        /// Any repeated points (0-length segments) will be included in the monotone chain returned.
        /// </summary>
        /// <param name="pts">The coordinates</param>
        /// <param name="start">The start index</param>
        /// <returns> 
        /// The index of the last point in the monotone chain starting at <c>start</c>.
        /// </returns>
        private static int FindChainEnd(IList<Coordinate> pts, int start)
        {
            int safeStart = start;
            // skip any zero-length segments at the start of the sequence
            // (since they cannot be used to establish a quadrant)
            while (safeStart < pts.Count - 1 && pts[safeStart].Equals2D(pts[safeStart + 1]))
            {
                safeStart++;
            }
            // check if there are NO non-zero-length segments
            if (safeStart >= pts.Count - 1) return pts.Count - 1;
            
            // determine overall quadrant for chain (which is the starting quadrant)
            int chainQuad = QuadrantOp.Quadrant(pts[safeStart], pts[safeStart + 1]);
            int last = start + 1;
            while (last < pts.Count)
            {
                // skip zero-length segments, but include them in the chain
                if (!pts[last - 1].Equals2D(pts[last]))
                {
                    // compute quadrant for next possible segment in chain
                    int quad = QuadrantOp.Quadrant(pts[last - 1], pts[last]);
                    if (quad != chainQuad) break;
                }
                last++;
            }
            return last - 1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pts"></param>
        /// <returns></returns>
        public static IList<MonotoneChain> GetChains(IList<Coordinate> pts)
        {
            return GetChains(pts, null);
        }

        /// <summary>
        /// Return a list of the <c>MonotoneChain</c>s
        /// for the given list of coordinates.
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="context"></param>
        public static IList<MonotoneChain> GetChains(IList<Coordinate> pts, object context)
        {
            var mcList = new List<MonotoneChain>();
            var startIndex = GetChainStartIndices(pts);
            for (var i = 0; i < startIndex.Length - 1; i++)
            {
                MonotoneChain mc = new MonotoneChain(pts, startIndex[i], startIndex[i + 1], context);
                mcList.Add(mc);
            }
            return mcList;
        }

        /// <summary>
        /// Return an array containing lists of start/end indexes of the monotone chains
        /// for the given list of coordinates.
        /// The last entry in the array points to the end point of the point array,
        /// for use as a sentinel.
        /// </summary>
        /// <param name="pts"></param>
        public static int[] GetChainStartIndices(IList<Coordinate> pts)
        {
            // find the startpoint (and endpoints) of all monotone chains in this edge
            int start = 0;
            var startIndexList = new List<int> {start};
            do
            {
                int last = FindChainEnd(pts, start);
                startIndexList.Add(last);
                start = last;
            }
            while (start < pts.Count - 1);

            // copy list to an array of ints, for efficiency
            return startIndexList.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int[] ToIntArray(IList<int> list)
        {
            int[] array = new int[list.Count];
            for (int i = 0; i < array.Length; i++)            
                array[i] = list[i];            
            return array;
        }

        #endregion
    }
}