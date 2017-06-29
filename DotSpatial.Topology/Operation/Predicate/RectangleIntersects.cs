// ********************************************************************************************************
// Product Name: MapWindow.dll Alpha
// Description:  The basic module for MapWindow version 6.0
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
using System.Collections.Generic;
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Utilities;

namespace DotSpatial.Topology.Operation.Predicate
{
    /// <summary>
    /// Optimized implementation of spatial predicate "intersects"
    /// for cases where the first {@link Geometry} is a rectangle.
    /// As a further optimization,
    /// this class can be used directly to test many geometries against a single
    /// rectangle.
    /// </summary>
    public class RectangleIntersects
    {
        /// <summary>
        /// Crossover size at which brute-force intersection scanning
        /// is slower than indexed intersection detection.
        /// Must be determined empirically.  Should err on the
        /// safe side by making value smaller rather than larger.
        /// </summary>
        public const int MAXIMUM_SCAN_SEGMENT_COUNT = 200;

        private readonly IEnvelope _rectEnv;
        private readonly IPolygon _rectangle;

        /// <summary>
        /// Create a new intersects computer for a rectangle.
        /// </summary>
        /// <param name="rectangle">A rectangular geometry.</param>
        public RectangleIntersects(IPolygon rectangle)
        {
            _rectangle = rectangle;
            _rectEnv = rectangle.EnvelopeInternal;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Intersects(Polygon rectangle, IGeometry b)
        {
            RectangleIntersects rp = new RectangleIntersects(rectangle);
            return rp.Intersects(b);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public bool Intersects(IGeometry geom)
        {
            if (!_rectEnv.Intersects(geom.EnvelopeInternal))
                return false;
            // test envelope relationships
            EnvelopeIntersectsVisitor visitor = new EnvelopeIntersectsVisitor(_rectEnv);
            visitor.ApplyTo(geom);
            if (visitor.Intersects())
                return true;

            // test if any rectangle corner is contained in the target
            ContainsPointVisitor ecpVisitor = new ContainsPointVisitor(_rectangle);
            ecpVisitor.ApplyTo(geom);
            if (ecpVisitor.ContainsPoint())
                return true;

            // test if any lines intersect
            LineIntersectsVisitor liVisitor = new LineIntersectsVisitor(_rectangle);
            liVisitor.ApplyTo(geom);
            if (liVisitor.Intersects())
                return true;

            return false;
        }
    }

    /// <summary>
    ///
    /// </summary>
    internal class EnvelopeIntersectsVisitor : ShortCircuitedGeometryVisitor
    {
        private readonly IEnvelope _rectEnv;
        private bool _intersects;

        /// <summary>
        ///
        /// </summary>
        /// <param name="rectEnv"></param>
        public EnvelopeIntersectsVisitor(IEnvelope rectEnv)
        {
            _rectEnv = rectEnv;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool Intersects() { return _intersects; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="element"></param>
        protected override void Visit(IGeometry element)
        {
            IEnvelope elementEnv = element.EnvelopeInternal;
            // disjoint
            if (!_rectEnv.Intersects(elementEnv))
                return;
            // fully contained - must intersect
            if (_rectEnv.Contains(elementEnv))
            {
                _intersects = true;
                return;
            }
            /*
            * Since the envelopes intersect and the test element is connected,
            * if its envelope is completely bisected by an edge of the rectangle
            * the element and the rectangle must touch.
            * (Notice it is NOT possible to make this conclusion
            * if the test envelope is "on a corner" of the rectangle
            * envelope)
            */
            if (elementEnv.Minimum.X >= _rectEnv.Minimum.X && elementEnv.Maximum.X <= _rectEnv.Maximum.X)
            {
                _intersects = true;
                return;
            }
            if (elementEnv.Minimum.Y >= _rectEnv.Minimum.Y && elementEnv.Maximum.Y <= _rectEnv.Maximum.Y)
            {
                _intersects = true;
                return;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override bool IsDone()
        {
            return _intersects;
        }
    }

    /// <summary>
    ///
    /// </summary>
    internal class ContainsPointVisitor : ShortCircuitedGeometryVisitor
    {
        private readonly IEnvelope _rectEnv;
        private readonly IList<Coordinate> _rectSeq;
        private bool _containsPoint;

        /// <summary>
        ///
        /// </summary>
        /// <param name="rectangle"></param>
        public ContainsPointVisitor(IPolygon rectangle)
        {
            _rectSeq = rectangle.Shell.Coordinates;
            _rectEnv = rectangle.EnvelopeInternal;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool ContainsPoint() { return _containsPoint; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        protected override void Visit(IGeometry geom)
        {
            if (!(geom is Polygon))
                return;
            IEnvelope elementEnv = geom.EnvelopeInternal;
            if (!_rectEnv.Intersects(elementEnv))
                return;
            // test each corner of rectangle for inclusion
            for (int i = 0; i < 4; i++)
            {
                Coordinate rectPt = _rectSeq[i];
                if (!elementEnv.Contains(rectPt))
                    continue;
                // check rect point in poly (rect is known not to touch polygon at this point)
                if (!SimplePointInAreaLocator.ContainsPointInPolygon(rectPt, (Polygon)geom)) continue;
                _containsPoint = true;
                return;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override bool IsDone()
        {
            return _containsPoint;
        }
    }

    /// <summary>
    ///
    /// </summary>
    internal class LineIntersectsVisitor : ShortCircuitedGeometryVisitor
    {
        private readonly IEnvelope _rectEnv;
        private readonly IList<Coordinate> _rectSeq;
        private readonly IPolygon _rectangle;
        private bool _intersects;

        /// <summary>
        ///
        /// </summary>
        /// <param name="rectangle"></param>
        public LineIntersectsVisitor(IPolygon rectangle)
        {
            _rectangle = rectangle;
            _rectSeq = rectangle.Shell.Coordinates;
            _rectEnv = rectangle.EnvelopeInternal;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool Intersects() { return _intersects; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        protected override void Visit(IGeometry geom)
        {
            IEnvelope elementEnv = geom.EnvelopeInternal;
            if (!_rectEnv.Intersects(elementEnv))
                return;
            // check if general relate algorithm should be used, since it's faster for large inputs
            if (geom.NumPoints > RectangleIntersects.MAXIMUM_SCAN_SEGMENT_COUNT)
            {
                _intersects = _rectangle.Relate(geom).IsIntersects();
                return;
            }
            ComputeSegmentIntersection(geom);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        private void ComputeSegmentIntersection(IGeometry geom)
        {
            // check segment intersection
            // get all lines from geom (e.g. if it's a multi-ring polygon)
            IList lines = LinearComponentExtracter.GetLines(geom);
            SegmentIntersectionTester si = new SegmentIntersectionTester();
            bool hasIntersection = si.HasIntersectionWithLineStrings(_rectSeq, lines);
            if (hasIntersection)
            {
                _intersects = true;
                return;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected override bool IsDone()
        {
            return _intersects;
        }
    }
}