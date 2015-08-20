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
using System.Linq;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.GeometriesGraph;

namespace DotSpatial.Topology.Operation.Buffer
{
    /// <summary>
    /// Computes the raw offset curve for a
    /// single <see cref="IGeometry"/> component (ring, line or point).
    /// A raw offset curve line is not noded -
    /// it may contain self-intersections (and usually will).
    /// The final buffer polygon is computed by forming a topological graph
    /// of all the noded raw curves and tracing outside contours.
    /// The points in the raw curve are rounded
    /// to a given <see cref="IPrecisionModel"/>.
    /// </summary>
    public class OffsetCurveBuilder
    {
        #region Fields

        private readonly IBufferParameters _bufParams;
        private readonly IPrecisionModel _precisionModel;
        private double _distance;

        #endregion

        #region Constructors

        public OffsetCurveBuilder(IPrecisionModel precisionModel,IBufferParameters bufParams)
        {
            _precisionModel = precisionModel;
            _bufParams = bufParams;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the buffer parameters being used to generate the curve.
        /// </summary>
        public IBufferParameters BufferParameters
        {
            get { return _bufParams; }
        }

        #endregion

        #region Methods

        private void ComputeLineBufferCurve(IList<Coordinate> inputPts, OffsetSegmentGenerator segGen)
        {
            var distTol = SimplifyTolerance(_distance);

            //--------- compute points for left side of line
            // Simplify the appropriate side of the line before generating
            var simp1 = BufferInputLineSimplifier.Simplify(inputPts, distTol);

            var n1 = simp1.Count - 1;
            segGen.InitSideSegments(simp1[0], simp1[1], PositionType.Left);
            for (int i = 2; i <= n1; i++)
            {
                segGen.AddNextSegment(simp1[i], true);
            }
            segGen.AddLastSegment();
            // add line cap for end of line
            segGen.AddLineEndCap(simp1[n1 - 1], simp1[n1]);

            //---------- compute points for right side of line
            // Simplify the appropriate side of the line before generating
            var simp2 = BufferInputLineSimplifier.Simplify(inputPts, -distTol);
            var n2 = simp2.Count - 1;

            // since we are traversing line in opposite order, offset position is still LEFT
            segGen.InitSideSegments(simp2[n2], simp2[n2 - 1], PositionType.Left);
            for (var i = n2 - 2; i >= 0; i--)
            {
                segGen.AddNextSegment(simp2[i], true);
            }
            segGen.AddLastSegment();
            // add line cap for start of line
            segGen.AddLineEndCap(simp2[1], simp2[0]);

            segGen.CloseRing();
        }

        private void ComputeOffsetCurve(IList<Coordinate> inputPts, Boolean isRightSide, OffsetSegmentGenerator segGen)
        {
            var distTol = SimplifyTolerance(_distance);

            if (isRightSide)
            {
                //---------- compute points for right side of line
                // Simplify the appropriate side of the line before generating
                var simp2 = BufferInputLineSimplifier.Simplify(inputPts, -distTol);
                // MD - used for testing only (to eliminate simplification)
                // Coordinate[] simp2 = inputPts;
                var n2 = simp2.Count - 1;

                // since we are traversing line in opposite order, offset position is still LEFT
                segGen.InitSideSegments(simp2[n2], simp2[n2 - 1], PositionType.Left);
                segGen.AddFirstSegment();
                for (var i = n2 - 2; i >= 0; i--)
                {
                    segGen.AddNextSegment(simp2[i], true);
                }
            }
            else
            {
                //--------- compute points for left side of line
                // Simplify the appropriate side of the line before generating
                var simp1 = BufferInputLineSimplifier.Simplify(inputPts, distTol);
                // MD - used for testing only (to eliminate simplification)
                // Coordinate[] simp1 = inputPts;

                var n1 = simp1.Count - 1;
                segGen.InitSideSegments(simp1[0], simp1[1], PositionType.Left);
                segGen.AddFirstSegment();
                for (var i = 2; i <= n1; i++)
                {
                    segGen.AddNextSegment(simp1[i], true);
                }
            }
            segGen.AddLastSegment();
        }

        private void ComputePointCurve(Coordinate pt, OffsetSegmentGenerator segGen)
        {
            switch (_bufParams.EndCapStyle)
            {
                case EndCapStyle.Round:
                    segGen.CreateCircle(pt);
                    break;
                case EndCapStyle.Square:
                    segGen.CreateSquare(pt);
                    break;
                // otherwise curve is empty (e.g. for a butt cap);
            }
        }

        private void ComputeRingBufferCurve(IList<Coordinate> inputPts, PositionType side, OffsetSegmentGenerator segGen)
        {
            // simplify input line to improve performance
            var distTol = SimplifyTolerance(_distance);
            // ensure that correct side is simplified
            if (side == PositionType.Right)
                distTol = -distTol;
            var simp = BufferInputLineSimplifier.Simplify(inputPts, distTol);
            // MD - used for testing only (to eliminate simplification)
            // Coordinate[] simp = inputPts;

            var n = simp.Count - 1;
            segGen.InitSideSegments(simp[n - 1], simp[0], side);
            for (var i = 1; i <= n; i++)
            {
                var addStartPoint = i != 1;
                segGen.AddNextSegment(simp[i], addStartPoint);
            }
            segGen.CloseRing();
        }

        private void ComputeSingleSidedBufferCurve(IList<Coordinate> inputPts, bool isRightSide, OffsetSegmentGenerator segGen)
        {
            var distTol = SimplifyTolerance(_distance);

            if (isRightSide)
            {
                // add original line
                segGen.AddSegments(inputPts, true);

                //---------- compute points for right side of line
                // Simplify the appropriate side of the line before generating
                var simp2 = BufferInputLineSimplifier.Simplify(inputPts, -distTol);
                // MD - used for testing only (to eliminate simplification)
                // Coordinate[] simp2 = inputPts;
                var n2 = simp2.Count - 1;

                // since we are traversing line in opposite order, offset position is still LEFT
                segGen.InitSideSegments(simp2[n2], simp2[n2 - 1], PositionType.Left);
                segGen.AddFirstSegment();
                for (var i = n2 - 2; i >= 0; i--)
                {
                    segGen.AddNextSegment(simp2[i], true);
                }
            }
            else
            {
                // add original line
                segGen.AddSegments(inputPts, false);

                //--------- compute points for left side of line
                // Simplify the appropriate side of the line before generating
                var simp1 = BufferInputLineSimplifier.Simplify(inputPts, distTol);
                // MD - used for testing only (to eliminate simplification)
                //      Coordinate[] simp1 = inputPts;

                var n1 = simp1.Count - 1;
                segGen.InitSideSegments(simp1[0], simp1[1], PositionType.Left);
                segGen.AddFirstSegment();
                for (var i = 2; i <= n1; i++)
                {
                    segGen.AddNextSegment(simp1[i], true);
                }
            }
            segGen.AddLastSegment();
            segGen.CloseRing();
        }

        private static IList<Coordinate> CopyCoordinates(IList<Coordinate> pts)
        {
            var copy = new Coordinate[pts.Count];
            for (int i = 0; i < copy.Length; i++)
            {
                copy[i] = new Coordinate(pts[i]);
            }
            return copy;
        }

        /// <summary>
        /// This method handles single points as well as LineStrings.
        /// LineStrings are assumed <b>not</b> to be closed (the function will not
        /// fail for closed lines, but will generate superfluous line caps).
        /// </summary>
        /// <param name="inputPts">The vertices of the line to offset</param>
        /// <param name="distance">The offset distance</param>
        /// <returns>A Coordinate array representing the curve <br/>
        /// or <c>null</c> if the curve is empty
        /// </returns>
        public virtual IList<Coordinate> GetLineCurve(IList<Coordinate> inputPts, double distance)
        {
            _distance = distance;

            // a zero or negative width buffer of a line/point is empty
            if (distance < 0.0 && !_bufParams.IsSingleSided) return null;
            if (distance == 0.0) return null;

            double posDistance = Math.Abs(distance);
            var segGen = GetSegmentGenerator(posDistance);
            if (inputPts.Count <= 1)
            {
                ComputePointCurve(inputPts[0], segGen);
            }
            else
            {
                if (_bufParams.IsSingleSided)
                {
                    var isRightSide = distance < 0.0;
                    ComputeSingleSidedBufferCurve(inputPts, isRightSide, segGen);
                }
                else
                    ComputeLineBufferCurve(inputPts, segGen);
            }

            var lineCoord = segGen.GetCoordinates();
            return lineCoord;
        }

        public IList<Coordinate> GetOffsetCurve(Coordinate[] inputPts, double distance)
        {
            _distance = distance;

            // a zero width offset curve is empty
            if (distance == 0.0) return null;

            var isRightSide = distance < 0.0;
            double posDistance = Math.Abs(distance);
            OffsetSegmentGenerator segGen = GetSegmentGenerator(posDistance);
            if (inputPts.Length <= 1)
            {
                ComputePointCurve(inputPts[0], segGen);
            }
            else
            {
                ComputeOffsetCurve(inputPts, isRightSide, segGen);
            }
            IList<Coordinate> curvePts = segGen.GetCoordinates();
            // for right side line is traversed in reverse direction, so have to reverse generated line
            if (isRightSide)
                curvePts = curvePts.Reverse().ToList();
            return curvePts;
        }

        /// <summary>
        /// This method handles the degenerate cases of single points and lines,
        /// as well as rings.
        /// </summary>
        /// <returns>A List of Coordinate or <c>null</c> if the curve is empty.</returns>
        public virtual IList<Coordinate> GetRingCurve(IList<Coordinate> inputPts, PositionType side, double distance)
        {
            _distance = distance;
            if (inputPts.Count <= 2)
                return GetLineCurve(inputPts, distance);
            // optimize creating ring for for zero distance
            if (distance == 0.0)
            {
                return CopyCoordinates(inputPts);
            }
            OffsetSegmentGenerator segGen = GetSegmentGenerator(distance);
            ComputeRingBufferCurve(inputPts, side, segGen);
            return segGen.GetCoordinates();
        }

        private OffsetSegmentGenerator GetSegmentGenerator(double distance)
        {
            return new OffsetSegmentGenerator(_precisionModel, _bufParams, distance);
        }

        /// <summary>
        /// Computes the distance tolerance to use during input line simplification.
        /// </summary>
        /// <param name="bufDistance">The buffer distance</param>
        /// <returns>The simplification tolerance</returns>
        private double SimplifyTolerance(double bufDistance)
       {
           return bufDistance * _bufParams.SimplifyFactor;
       }

        #endregion
    }
}