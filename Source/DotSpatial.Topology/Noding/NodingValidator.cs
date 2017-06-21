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
using DotSpatial.Topology.Algorithm;

namespace DotSpatial.Topology.Noding
{
    /// <summary>
    /// Validates that a collection of <see cref="SegmentString" />s is correctly noded.
    /// Throws an appropriate exception if an noding error is found.
    /// </summary>
    public class NodingValidator
    {
        private readonly LineIntersector _li = new RobustLineIntersector();

        private readonly IList _segStrings;

        /// <summary>
        /// Initializes a new instance of the <see cref="NodingValidator"/> class.
        /// </summary>
        /// <param name="segStrings">The seg strings.</param>
        public NodingValidator(IList segStrings)
        {
            _segStrings = segStrings;
        }

        /// <summary>
        ///
        /// </summary>
        public void CheckValid()
        {
            CheckEndPtVertexIntersections();
            CheckInteriorIntersections();
            CheckCollapses();
        }

        /// <summary>
        /// Checks if a segment string contains a segment pattern a-b-a (which implies a self-intersection).
        /// </summary>
        private void CheckCollapses()
        {
            foreach (object obj in _segStrings)
            {
                SegmentString ss = (SegmentString)obj;
                CheckCollapses(ss);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ss"></param>
        private static void CheckCollapses(SegmentString ss)
        {
            IList<Coordinate> pts = ss.Coordinates;
            for (int i = 0; i < pts.Count - 2; i++)
                CheckCollapse(pts[i], pts[i + 1], pts[i + 2]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        private static void CheckCollapse(Coordinate p0, Coordinate p1, Coordinate p2)
        {
            if (p0.Equals(p2))
                throw new Exception("found non-noded collapse at: " + p0 + ", " + p1 + " " + p2);
        }

        /// <summary>
        /// Checks all pairs of segments for intersections at an interior point of a segment.
        /// </summary>
        private void CheckInteriorIntersections()
        {
            foreach (object obj0 in _segStrings)
            {
                SegmentString ss0 = (SegmentString)obj0;
                foreach (object obj1 in _segStrings)
                {
                    SegmentString ss1 = (SegmentString)obj1;
                    CheckInteriorIntersections(ss0, ss1);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ss0"></param>
        /// <param name="ss1"></param>
        private void CheckInteriorIntersections(SegmentString ss0, SegmentString ss1)
        {
            IList<Coordinate> pts0 = ss0.Coordinates;
            IList<Coordinate> pts1 = ss1.Coordinates;
            for (int i0 = 0; i0 < pts0.Count - 1; i0++)
                for (int i1 = 0; i1 < pts1.Count - 1; i1++)
                    CheckInteriorIntersections(ss0, i0, ss1, i1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="e0"></param>
        /// <param name="segIndex0"></param>
        /// <param name="e1"></param>
        /// <param name="segIndex1"></param>
        private void CheckInteriorIntersections(SegmentString e0, int segIndex0, SegmentString e1, int segIndex1)
        {
            if (e0 == e1 && segIndex0 == segIndex1)
                return;

            Coordinate p00 = e0.Coordinates[segIndex0];
            Coordinate p01 = e0.Coordinates[segIndex0 + 1];
            Coordinate p10 = e1.Coordinates[segIndex1];
            Coordinate p11 = e1.Coordinates[segIndex1 + 1];

            _li.ComputeIntersection(p00, p01, p10, p11);
            if (_li.HasIntersection)
                if (_li.IsProper || HasInteriorIntersection(_li, p00, p01) || HasInteriorIntersection(_li, p10, p11))
                    throw new Exception("found non-noded intersection at " + p00 + "-" + p01
                                               + " and " + p10 + "-" + p11);
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
            for (int i = 0; i < li.IntersectionNum; i++)
            {
                Coordinate intPt = li.GetIntersection(i);
                if (!(intPt.Equals(p0) || intPt.Equals(p1)))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks for intersections between an endpoint of a segment string
        /// and an interior vertex of another segment string
        /// </summary>
        private void CheckEndPtVertexIntersections()
        {
            foreach (object obj in _segStrings)
            {
                SegmentString ss = (SegmentString)obj;
                IList<Coordinate> pts = ss.Coordinates;
                CheckEndPtVertexIntersections(pts[0], _segStrings);
                CheckEndPtVertexIntersections(pts[pts.Count - 1], _segStrings);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="testPt"></param>
        /// <param name="segStrings"></param>
        private static void CheckEndPtVertexIntersections(Coordinate testPt, IList segStrings)
        {
            foreach (object obj in segStrings)
            {
                SegmentString ss = (SegmentString)obj;
                IList<Coordinate> pts = ss.Coordinates;
                for (int j = 1; j < pts.Count - 1; j++)
                    if (pts[j].Equals(testPt))
                        throw new Exception("found endpt/interior pt intersection at index " + j + " :pt " + testPt);
            }
        }
    }
}