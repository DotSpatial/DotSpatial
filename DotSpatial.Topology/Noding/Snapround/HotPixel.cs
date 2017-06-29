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
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Noding.Snapround
{
    /// <summary>
    /// Implements a "hot pixel" as used in the Snap Rounding algorithm.
    /// A hot pixel contains the interior of the tolerance square and the boundary
    /// minus the top and right segments.
    /// The hot pixel operations are all computed in the integer domain
    /// to avoid rounding problems.
    /// </summary>
    public class HotPixel
    {
        private readonly Coordinate[] _corner = new Coordinate[4];
        private readonly LineIntersector _li;

        private readonly Coordinate _originalPt;

        private readonly Coordinate _p0Scaled;
        private readonly Coordinate _p1Scaled;
        private readonly Coordinate _pt;

        private readonly double _scaleFactor;

        private double _maxx;
        private double _maxy;
        private double _minx;
        private double _miny;

        /*
         * The corners of the hot pixel, in the order:
         *  10
         *  23
         */

        private Envelope _safeEnv;

        /// <summary>
        /// Initializes a new instance of the <see cref="HotPixel"/> class.
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="scaleFactor"></param>
        /// <param name="li"></param>
        public HotPixel(Coordinate pt, double scaleFactor, LineIntersector li)
        {
            _originalPt = pt;
            _pt = pt;
            _scaleFactor = scaleFactor;
            _li = li;
            if (scaleFactor != 1.0)
            {
                _pt = new Coordinate(Scale(pt.X), Scale(pt.Y));
                _p0Scaled = new Coordinate();
                _p1Scaled = new Coordinate();
            }
            InitCorners(_pt);
        }

        /// <summary>
        ///
        /// </summary>
        public Coordinate Coordinate
        {
            get
            {
                return _originalPt;
            }
        }

        /// <summary>
        /// Returns a "safe" envelope that is guaranteed to contain the hot pixel.
        /// </summary>
        /// <returns></returns>
        public Envelope GetSafeEnvelope()
        {
            if (_safeEnv == null)
            {
                double safeTolerance = 0.75 / _scaleFactor;
                _safeEnv = new Envelope(_originalPt.X - safeTolerance, _originalPt.X + safeTolerance,
                                       _originalPt.Y - safeTolerance, _originalPt.Y + safeTolerance);
            }
            return _safeEnv;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pt"></param>
        private void InitCorners(Coordinate pt)
        {
            const double tolerance = 0.5;
            _minx = pt.X - tolerance;
            _maxx = pt.X + tolerance;
            _miny = pt.Y - tolerance;
            _maxy = pt.Y + tolerance;

            _corner[0] = new Coordinate(_maxx, _maxy);
            _corner[1] = new Coordinate(_minx, _maxy);
            _corner[2] = new Coordinate(_minx, _miny);
            _corner[3] = new Coordinate(_maxx, _miny);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private double Scale(double val)
        {
            return Math.Round(val * _scaleFactor);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public bool Intersects(Coordinate p0, Coordinate p1)
        {
            if (_scaleFactor == 1.0)
                return IntersectsScaled(p0, p1);

            CopyScaled(p0, _p0Scaled);
            CopyScaled(p1, _p1Scaled);
            return IntersectsScaled(_p0Scaled, _p1Scaled);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <param name="pScaled"></param>
        private void CopyScaled(Coordinate p, Coordinate pScaled)
        {
            pScaled.X = Scale(p.X);
            pScaled.Y = Scale(p.Y);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        public bool IntersectsScaled(Coordinate p0, Coordinate p1)
        {
            double segMinx = Math.Min(p0.X, p1.X);
            double segMaxx = Math.Max(p0.X, p1.X);
            double segMiny = Math.Min(p0.Y, p1.Y);
            double segMaxy = Math.Max(p0.Y, p1.Y);

            bool isOutsidePixelEnv = _maxx < segMinx || _minx > segMaxx ||
                                     _maxy < segMiny || _miny > segMaxy;
            if (isOutsidePixelEnv)
                return false;
            bool intersects = IntersectsToleranceSquare(p0, p1);
            Assert.IsTrue(!intersects, "Found bad envelope test");
            return intersects;
        }

        /// <summary>
        /// Tests whether the segment p0-p1 intersects the hot pixel tolerance square.
        /// Because the tolerance square point set is partially open (along the
        /// top and right) the test needs to be more sophisticated than
        /// simply checking for any intersection.  However, it
        /// can take advantage of the fact that because the hot pixel edges
        /// do not lie on the coordinate grid.  It is sufficient to check
        /// if there is at least one of:
        ///  - a proper intersection with the segment and any hot pixel edge.
        ///  - an intersection between the segment and both the left and bottom edges.
        ///  - an intersection between a segment endpoint and the hot pixel coordinate.
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns></returns>
        private bool IntersectsToleranceSquare(Coordinate p0, Coordinate p1)
        {
            bool intersectsLeft = false;
            bool intersectsBottom = false;

            _li.ComputeIntersection(p0, p1, _corner[0], _corner[1]);
            if (_li.IsProper) return true;

            _li.ComputeIntersection(p0, p1, _corner[1], _corner[2]);
            if (_li.IsProper) return true;
            if (_li.HasIntersection) intersectsLeft = true;

            _li.ComputeIntersection(p0, p1, _corner[2], _corner[3]);
            if (_li.IsProper) return true;
            if (_li.HasIntersection) intersectsBottom = true;

            _li.ComputeIntersection(p0, p1, _corner[3], _corner[0]);
            if (_li.IsProper) return true;

            if (intersectsLeft && intersectsBottom) return true;

            if (p0.Equals(_pt)) return true;
            if (p1.Equals(_pt)) return true;

            return false;
        }
    }
}