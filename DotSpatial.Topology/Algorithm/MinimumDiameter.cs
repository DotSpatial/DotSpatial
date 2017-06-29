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

namespace DotSpatial.Topology.Algorithm
{
    /// <summary>
    /// Computes the minimum diameter of a <c>Geometry</c>.
    /// The minimum diameter is defined to be the
    /// width of the smallest band that contains the point,
    /// where a band is a strip of the plane defined
    /// by two parallel lines.
    /// This can be thought of as the smallest hole that the point can be
    /// moved through, with a single rotation.
    /// The first step in the algorithm is computing the convex hull of the Geometry.
    /// If the input Geometry is known to be convex, a hint can be supplied to
    /// avoid this computation.
    /// </summary>
    public class MinimumDiameter
    {
        private readonly IGeometry _inputGeom;
        private readonly bool _isConvex;

        private LineSegment _minBaseSeg = new LineSegment();
        private int _minPtIndex;
        private double _minWidth;
        private Coordinate _minWidthPt = new Coordinate(0, 0, 0, 0);

        /// <summary>
        /// Compute a minimum diameter for a giver <c>Geometry</c>.
        /// </summary>
        /// <param name="inputGeom">a Geometry.</param>
        public MinimumDiameter(IGeometry inputGeom) : this(inputGeom, false) { }

        /// <summary>
        /// Compute a minimum diameter for a giver <c>Geometry</c>,
        /// with a hint if
        /// the Geometry is convex
        /// (e.g. a convex Polygon or LinearRing,
        /// or a two-point LineString, or a Point).
        /// </summary>
        /// <param name="inputGeom">a Geometry which is convex.</param>
        /// <param name="isConvex"><c>true</c> if the input point is convex.</param>
        public MinimumDiameter(IGeometry inputGeom, bool isConvex)
        {
            _inputGeom = inputGeom;
            _isConvex = isConvex;
        }

        /// <summary>
        /// Gets the length of the minimum diameter of the input Geometry.
        /// </summary>
        /// <returns>The length of the minimum diameter.</returns>
        public virtual double Length
        {
            get
            {
                ComputeMinimumDiameter();
                return _minWidth;
            }
        }

        /// <summary>
        /// Gets the <c>Coordinate</c> forming one end of the minimum diameter.
        /// </summary>
        /// <returns>A coordinate forming one end of the minimum diameter.</returns>
        public virtual Coordinate WidthCoordinate
        {
            get
            {
                ComputeMinimumDiameter();
                return _minWidthPt;
            }
        }

        /// <summary>
        /// Gets the segment forming the base of the minimum diameter.
        /// </summary>
        /// <returns>The segment forming the base of the minimum diameter.</returns>
        public virtual ILineString SupportingSegment
        {
            get
            {
                ComputeMinimumDiameter();
                return _inputGeom.Factory.CreateLineString(new[] { _minBaseSeg.P0, _minBaseSeg.P1 });
            }
        }

        /// <summary>
        /// Gets a <c>LineString</c> which is a minimum diameter.
        /// </summary>
        /// <returns>A <c>LineString</c> which is a minimum diameter.</returns>
        public virtual ILineString Diameter
        {
            get
            {
                ComputeMinimumDiameter();

                // return empty linearRing if no minimum width calculated
                if (_minWidthPt.IsEmpty())
                    return _inputGeom.Factory.CreateLineString(null);

                Coordinate basePt = new Coordinate(_minBaseSeg.Project(_minWidthPt));
                return _inputGeom.Factory.CreateLineString(new[] { basePt, _minWidthPt });
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void ComputeMinimumDiameter()
        {
            // check if computation is cached
            if (_minWidthPt.IsEmpty())
                return;

            if (_isConvex) ComputeWidthConvex(_inputGeom);
            else
            {
                IGeometry convexGeom = (new ConvexHull(_inputGeom)).GetConvexHull();
                ComputeWidthConvex(convexGeom);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        private void ComputeWidthConvex(IGeometry geom)
        {
            IList<Coordinate> pts;
            if (geom is Polygon)
                pts = ((Polygon)geom).Shell.Coordinates;
            else pts = geom.Coordinates;

            // special cases for lines or points or degenerate rings
            if (pts.Count == 0)
            {
                _minWidth = 0.0;
                _minWidthPt = Coordinate.Empty;
                _minBaseSeg = null;
            }
            else if (pts.Count == 1)
            {
                _minWidth = 0.0;
                _minWidthPt = pts[0];
                _minBaseSeg.P0 = pts[0];
                _minBaseSeg.P1 = pts[0];
            }
            else if (pts.Count == 2 || pts.Count == 3)
            {
                _minWidth = 0.0;
                _minWidthPt = pts[0];
                _minBaseSeg.P0 = pts[0];
                _minBaseSeg.P1 = pts[1];
            }
            else ComputeConvexRingMinDiameter(pts);
        }

        /// <summary>
        /// Compute the width information for a ring of <c>Coordinate</c>s.
        /// Leaves the width information in the instance variables.
        /// </summary>
        /// <param name="pts"></param>
        private void ComputeConvexRingMinDiameter(IList<Coordinate> pts)
        {
            // for each segment in the ring
            _minWidth = Double.MaxValue;
            int currMaxIndex = 1;

            LineSegment seg = new LineSegment();
            // compute the max distance for all segments in the ring, and pick the minimum
            for (int i = 0; i < pts.Count - 1; i++)
            {
                seg.P0 = pts[i];
                seg.P1 = pts[i + 1];
                currMaxIndex = FindMaxPerpDistance(pts, seg, currMaxIndex);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="seg"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        private int FindMaxPerpDistance(IList<Coordinate> pts, ILineSegment seg, int startIndex)
        {
            double maxPerpDistance = seg.DistancePerpendicular(pts[startIndex]);
            double nextPerpDistance = maxPerpDistance;
            int maxIndex = startIndex;
            int nextIndex = maxIndex;
            while (nextPerpDistance >= maxPerpDistance)
            {
                maxPerpDistance = nextPerpDistance;
                maxIndex = nextIndex;

                nextIndex = NextIndex(pts, maxIndex);
                nextPerpDistance = seg.DistancePerpendicular(pts[nextIndex]);
            }

            // found maximum width for this segment - update global min dist if appropriate
            if (maxPerpDistance < _minWidth)
            {
                _minPtIndex = maxIndex;
                _minWidth = maxPerpDistance;
                _minWidthPt = pts[_minPtIndex];
                _minBaseSeg = new LineSegment(seg);
            }
            return maxIndex;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static int NextIndex(ICollection<Coordinate> pts, int index)
        {
            index++;
            if (index >= pts.Count) index = 0;
            return index;
        }
    }
}