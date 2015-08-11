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
using System.Collections.Generic;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Noding
{
    /// <summary>
    /// Validates that a collection of <see cref="ISegmentString" />s is correctly noded.
    /// Throws an appropriate exception if an noding error is found.
    /// </summary>
    public class NodingValidator
    {
        #region Fields

        private readonly LineIntersector _li = new RobustLineIntersector();
        private readonly IList<ISegmentString> _segStrings;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new validator for the given collection of <see cref="ISegmentString"/>s.
        /// </summary>
        /// <param name="segStrings">The seg strings.</param>
        public NodingValidator(IList<ISegmentString> segStrings)
        {
            _segStrings = segStrings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Checks whether the supplied segment strings are correctly noded.
        /// Throws an exception if they are not.
        /// </summary>
        public void CheckValid()
        {
            CheckEndPtVertexIntersections();
            CheckInteriorIntersections();
            CheckCollapses();
        }

        private static void CheckCollapse(Coordinate p0, Coordinate p1, Coordinate p2)
        {
            if (p0.Equals(p2))
                throw new ApplicationException(string.Format(TopologyText.NodingValidator_FoundNonNodedCollapse, p0, p1, p2));
        }

        private static void CheckCollapses(ISegmentString ss)
        {
            var pts = ss.Coordinates;
            for (var i = 0; i < pts.Count - 2; i++)
                CheckCollapse(pts[i], pts[i + 1], pts[i + 2]);            
        }

        /// <summary>
        /// Checks if a segment string contains a segment pattern a-b-a (which implies a self-intersection).
        /// </summary>
        private void CheckCollapses()
        {
            foreach (ISegmentString ss in _segStrings)
                CheckCollapses(ss);            
        }

        private static void CheckEndPtVertexIntersections(Coordinate testPt, IEnumerable<ISegmentString> segStrings)
        {
            foreach (ISegmentString ss in segStrings)
            {
                var pts = ss.Coordinates;
                for (var j = 1; j < pts.Count - 1; j++)
                    if (pts[j].Equals(testPt))
                        throw new ApplicationException(string.Format(TopologyText.NodingValidator_FoundEndPointInteriorPointIntersection, j, testPt));                
            }
        }

        /// <summary>
        /// Checks for intersections between an endpoint of a segment string
        /// and an interior vertex of another segment string
        /// </summary>
        private void CheckEndPtVertexIntersections()
        {
            foreach(ISegmentString ss in _segStrings)
            {
                var pts = ss.Coordinates;
                CheckEndPtVertexIntersections(pts[0], _segStrings);
                CheckEndPtVertexIntersections(pts[pts.Count - 1], _segStrings);
            }
        }

        /// <summary>
        /// Checks all pairs of segments for intersections at an interior point of a segment.
        /// </summary>
        private void CheckInteriorIntersections()
        {
            foreach (ISegmentString ss0 in _segStrings)
                foreach (ISegmentString ss1 in _segStrings)
                    CheckInteriorIntersections(ss0, ss1);
        }

        private void CheckInteriorIntersections(ISegmentString ss0, ISegmentString ss1)
        {
            var pts0 = ss0.Coordinates;
            var pts1 = ss1.Coordinates;
            for (var i0 = 0; i0 < pts0.Count - 1; i0++)
                for (var i1 = 0; i1 < pts1.Count - 1; i1++)
                    CheckInteriorIntersections(ss0, i0, ss1, i1);            
        }

        private void CheckInteriorIntersections(ISegmentString e0, int segIndex0, ISegmentString e1, int segIndex1)
        {
            if (e0 == e1 && segIndex0 == segIndex1) return;

            var p00 = e0.Coordinates[segIndex0];
            var p01 = e0.Coordinates[segIndex0 + 1];
            var p10 = e1.Coordinates[segIndex1];
            var p11 = e1.Coordinates[segIndex1 + 1];

            _li.ComputeIntersection(p00, p01, p10, p11);
            if (!_li.HasIntersection) return;
            if (_li.IsProper || HasInteriorIntersection(_li, p00, p01) || HasInteriorIntersection(_li, p10, p11))
                throw new ApplicationException(string.Format(TopologyText.NodingValidator_FoundNonNodedIntersection, p00, p01, p10, p11));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="li"></param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns><c>true</c> if there is an intersection point which is not an endpoint of the segment p0-p1.</returns>
        private static bool HasInteriorIntersection(LineIntersector li, Coordinate p0, Coordinate p1)
        {
            for (var i = 0; i < li.IntersectionNum; i++)
            {
                var intPt = li.GetIntersection(i);
                if (!(intPt.Equals(p0) || intPt.Equals(p1))) return true;
            }
            return false;
        }

        #endregion
    }
}