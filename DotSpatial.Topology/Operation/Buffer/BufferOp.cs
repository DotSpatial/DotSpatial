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
using DotSpatial.Topology.Precision;

namespace DotSpatial.Topology.Operation.Buffer
{
    /// <summary>
    /// Computes the buffer of a point, for both positive and negative buffer distances.
    /// In GIS, the buffer of a point is defined as
    /// the Minkowski sum or difference of the point
    /// with a circle with radius equal to the absolute value of the buffer distance.
    /// In the CAD/CAM world buffers are known as offset curves.
    /// Since true buffer curves may contain circular arcs,
    /// computed buffer polygons can only be approximations to the true point.
    /// The user can control the accuracy of the curve approximation by specifying
    /// the number of linear segments with which to approximate a curve.
    /// The end cap endCapStyle of a linear buffer may be specified. The
    /// following end cap styles are supported:
    /// <para>
    /// {CAP_ROUND} - the usual round end caps
    /// {CAP_BUTT} - end caps are truncated flat at the line ends
    /// {CAP_SQUARE} - end caps are squared off at the buffer distance beyond the line ends
    /// </para>
    /// The computation uses an algorithm involving iterated noding and precision reduction
    /// to provide a high degree of robustness.
    /// </summary>
    public class BufferOp
    {
        // Notice: modified for "safe" assembly in Sql 2005
        // Const added!
        private const int MAX_PRECISION_DIGITS = 12;

        private readonly IGeometry _argGeom;
        private double _distance;
        private BufferStyle _endCapStyle = BufferStyle.CapRound;
        private int _quadrantSegments = OffsetCurveBuilder.DEFAULT_QUADRANT_SEGMENTS;
        private IGeometry _resultGeometry;
        private TopologyException _saveException;   // debugging only

        /// <summary>
        /// Initializes a buffer computation for the given point.
        /// </summary>
        /// <param name="g">The point to buffer.</param>
        public BufferOp(IGeometry g)
        {
            _argGeom = g;
        }

        /// <summary>
        /// Specifies the end cap endCapStyle of the generated buffer.
        /// The styles supported are CapRound, CapButt, and CapSquare.
        /// The default is CapRound.
        /// </summary>
        public virtual BufferStyle EndCapStyle
        {
            get
            {
                return _endCapStyle;
            }
            set
            {
                _endCapStyle = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int QuadrantSegments
        {
            get
            {
                return _quadrantSegments;
            }
            set
            {
                _quadrantSegments = value;
            }
        }

        /// <summary>
        /// Compute a reasonable scale factor to limit the precision of
        /// a given combination of Geometry and buffer distance.
        /// The scale factor is based on a heuristic.
        /// </summary>
        /// <param name="g">The Geometry being buffered.</param>
        /// <param name="distance">The buffer distance.</param>
        /// <param name="maxPrecisionDigits">The mzx # of digits that should be allowed by
        /// the precision determined by the computed scale factor.</param>
        /// <returns>A scale factor that allows a reasonable amount of precision for the buffer computation.</returns>
        private static double PrecisionScaleFactor(IGeometry g, double distance, int maxPrecisionDigits)
        {
            IEnvelope env = g.EnvelopeInternal;
            double envSize = Math.Max(env.Height, env.Width);
            double expandByDistance = distance > 0.0 ? distance : 0.0;
            double bufEnvSize = envSize + 2 * expandByDistance;

            // the smallest power of 10 greater than the buffer envelope
            int bufEnvLog10 = (int)(Math.Log(bufEnvSize) / Math.Log(10) + 1.0);
            int minUnitLog10 = bufEnvLog10 - maxPrecisionDigits;

            // scale factor is inverse of min Unit size, so flip sign of exponent
            double scaleFactor = Math.Pow(10.0, -minUnitLog10);
            return scaleFactor;
        }

        /// <summary>
        /// Computes the buffer of a point for a given buffer distance.
        /// </summary>
        /// <param name="g">The point to buffer.</param>
        /// <param name="distance">The buffer distance.</param>
        /// <returns> The buffer of the input point.</returns>
        public static IGeometry Buffer(IGeometry g, double distance)
        {
            BufferOp gBuf = new BufferOp(g);
            IGeometry geomBuf = gBuf.GetResultGeometry(distance);
            return geomBuf;
        }

        /// <summary>
        /// Computes the buffer of a point for a given buffer distance,
        /// using the given Cap Style for borders of the point.
        /// </summary>
        /// <param name="g">The point to buffer.</param>
        /// <param name="distance">The buffer distance.</param>
        /// <param name="endCapStyle">Cap Style to use for compute buffer.</param>
        /// <returns> The buffer of the input point.</returns>
        public static IGeometry Buffer(Geometry g, double distance, BufferStyle endCapStyle)
        {
            BufferOp gBuf = new BufferOp(g);
            gBuf.EndCapStyle = endCapStyle;
            IGeometry geomBuf = gBuf.GetResultGeometry(distance);
            return geomBuf;
        }

        /// <summary>
        /// Computes the buffer for a point for a given buffer distance
        /// and accuracy of approximation.
        /// </summary>
        /// <param name="g">The point to buffer.</param>
        /// <param name="distance">The buffer distance.</param>
        /// <param name="quadrantSegments">The number of segments used to approximate a quarter circle.</param>
        /// <returns>The buffer of the input point.</returns>
        public static IGeometry Buffer(Geometry g, double distance, int quadrantSegments)
        {
            BufferOp bufOp = new BufferOp(g);
            bufOp.QuadrantSegments = quadrantSegments;
            IGeometry geomBuf = bufOp.GetResultGeometry(distance);
            return geomBuf;
        }

        /// <summary>
        /// Computes the buffer for a point for a given buffer distance
        /// and accuracy of approximation.
        /// </summary>
        /// <param name="g">The point to buffer.</param>
        /// <param name="distance">The buffer distance.</param>
        /// <param name="quadrantSegments">The number of segments used to approximate a quarter circle.</param>
        /// <param name="endCapStyle">Cap Style to use for compute buffer.</param>
        /// <returns>The buffer of the input point.</returns>
        public static IGeometry Buffer(IGeometry g, double distance, int quadrantSegments, BufferStyle endCapStyle)
        {
            BufferOp bufOp = new BufferOp(g);
            bufOp.EndCapStyle = endCapStyle;
            bufOp.QuadrantSegments = quadrantSegments;
            IGeometry geomBuf = bufOp.GetResultGeometry(distance);
            return geomBuf;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public virtual IGeometry GetResultGeometry(double distance)
        {
            _distance = distance;
            ComputeGeometry();
            return _resultGeometry;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="quadrantSegments"></param>
        /// <returns></returns>
        public virtual IGeometry GetResultGeometry(double distance, int quadrantSegments)
        {
            _distance = distance;
            QuadrantSegments = quadrantSegments;
            ComputeGeometry();
            return _resultGeometry;
        }

        /// <summary>
        ///
        /// </summary>
        private void ComputeGeometry()
        {
            BufferOriginalPrecision();
            if (_resultGeometry != null)
                return;

            // try and compute with decreasing precision
            for (int precDigits = MAX_PRECISION_DIGITS; precDigits >= 0; precDigits--)
            {
                try
                {
                    BufferFixedPrecision(precDigits);
                }
                catch (TopologyException ex)
                {
                    _saveException = ex;
                    // don't propagate the exception - it will be detected by fact that resultGeometry is null
                }
                if (_resultGeometry != null)
                    return;
            }

            // tried everything - have to bail
            throw _saveException;
        }

        /// <summary>
        ///
        /// </summary>
        private void BufferOriginalPrecision()
        {
            try
            {
                BufferBuilder bufBuilder = new BufferBuilder();
                bufBuilder.QuadrantSegments = _quadrantSegments;
                bufBuilder.EndCapStyle = _endCapStyle;
                _resultGeometry = bufBuilder.Buffer(_argGeom, _distance);
            }
            catch (TopologyException ex)
            {
                _saveException = ex;
                // don't propagate the exception - it will be detected by fact that resultGeometry is null
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="precisionDigits"></param>
        private void BufferFixedPrecision(int precisionDigits)
        {
            double sizeBasedScaleFactor = PrecisionScaleFactor(_argGeom, _distance, precisionDigits);

            PrecisionModel fixedPm = new PrecisionModel(sizeBasedScaleFactor);

            // don't change the precision model of the Geometry, just reduce the precision
            SimpleGeometryPrecisionReducer reducer = new SimpleGeometryPrecisionReducer(fixedPm);
            IGeometry reducedGeom = reducer.Reduce(_argGeom);

            BufferBuilder bufBuilder = new BufferBuilder();
            bufBuilder.WorkingPrecisionModel = fixedPm;
            bufBuilder.QuadrantSegments = _quadrantSegments;

            // this may throw an exception, if robustness errors are encountered
            _resultGeometry = bufBuilder.Buffer(reducedGeom, _distance);
        }
    }
}