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
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.Mathematics;
using DotSpatial.Topology.Noding;
using DotSpatial.Topology.Noding.Snapround;

namespace DotSpatial.Topology.Operation.Buffer
{
    /// <summary>
    /// Computes the buffer of a geometry, for both positive and negative buffer distances.
    /// </summary>
    /// <remarks>
    /// <para>
    /// In GIS, the positive (or negative) buffer of a geometry is defined as
    /// the Minkowski sum (or difference) of the geometry
    /// with a circle of radius equal to the absolute value of the buffer distance.
    /// In the CAD/CAM world buffers are known as <i>offset curves</i>.
    /// In morphological analysis the
    /// operation of positive and negative buffering
    /// is referred to as <i>erosion</i> and <i>dilation</i>
    /// </para>
    /// <para>
    /// The buffer operation always returns a polygonal result.
    /// The negative or zero-distance buffer of lines and points is always an empty <see cref="IPolygon" />.
    /// </para>
    /// <para>
    /// Since true buffer curves may contain circular arcs,
    /// computed buffer polygons are only approximations to the true geometry.
    /// The user can control the accuracy of the approximation by specifying
    /// the number of linear segments used to approximate arcs.
    /// This is specified via <see cref="IBufferParameters.QuadrantSegments"/> 
    /// or <see cref="QuadrantSegments"/>.
    /// </para>
    /// <para>
    /// The <see cref="IBufferParameters.EndCapStyle"/> of a linear buffer may be specified. 
    /// The following end cap styles are supported:
    /// <ul>
    /// <li><see cref="EndCapStyle.Round" /> - the usual round end caps</li>
    /// <li><see cref="EndCapStyle.Flat" /> - end caps are truncated flat at the line ends</li>
    /// <li><see cref="EndCapStyle.Square" /> - end caps are squared off at the buffer distance beyond the line ends</li>
    /// </ul>
    /// </para>
    /// <para>
    /// The <see cref="IBufferParameters.JoinStyle"/> of the corners in a buffer may be specified. 
    /// The following join styles are supported:
    /// <ul>
    /// <li><see cref="JoinStyle.Round" /> - the usual round join</li>
    /// <li><see cref="JoinStyle.Mitre" /> - corners are "sharp" (up to a <see cref="IBufferParameters.MitreLimit"/> distance limit})</li>
    /// <li><see cref="JoinStyle.Bevel" /> - corners are beveled (clipped off)</li>
    /// </ul>
    /// </para>
    /// <para>
    /// The buffer algorithm can perform simplification on the input to increase performance.
    /// The simplification is performed a way that always increases the buffer area 
    /// (so that the simplified input covers the original input).
    /// The degree of simplification can be specified with <see cref="IBufferParameters.SimplifyFactor"/>,
    /// with a <see cref="BufferParameters.DefaultSimplifyFactor"/> used otherwise.
    /// Note that if the buffer distance is zero then so is the computed simplify tolerance, 
    /// no matter what the simplify factor.
    /// </para>
    /// </remarks>
    public class BufferOp
    {
        #region Constant Fields

        /// <summary>
        /// A number of digits of precision which leaves some computational "headroom"
        /// for floating point operations.
        /// </summary>
        /// <remarks>
        /// This value should be less than the decimal precision of double-precision values (16).
        /// </remarks>
        private const int MaxPrecisionDigits = 12;

        #endregion

        #region Fields

        private readonly IGeometry _argGeom;
        private readonly IBufferParameters _bufParams = new BufferParameters();
        private double _distance;
        private IGeometry _resultGeometry;
        private TopologyException _saveException;   // debugging only

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a buffer computation for the given geometry
        /// </summary>
        /// <param name="g"> the geometry to buffer</param>
        public BufferOp(IGeometry g)
        {
            _argGeom = g;
        }

        /// <summary>
        /// Initializes a buffer computation for the given geometry
        /// with the given set of parameters
        /// </summary>
        /// <param name="g"> the geometry to buffer</param>
        /// <param name="bufParams"> the buffer parameters to use</param>
        public BufferOp(IGeometry g, IBufferParameters bufParams)
        {
            _argGeom = g;
            _bufParams = bufParams;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Sets the number of segments used to approximate a angle fillet
        /// </summary>
        public virtual int QuadrantSegments
        {
            get { return _bufParams.QuadrantSegments; }
            set { _bufParams.QuadrantSegments = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the buffer of a geometry for a given buffer distance.
        /// </summary>
        /// <param name="g"> the geometry to buffer</param>
        /// <param name="distance"> the buffer distance</param>
        /// <returns> the buffer of the input geometry</returns>
        public static IGeometry Buffer(IGeometry g, double distance)
        {
            BufferOp gBuf = new BufferOp(g);
            IGeometry geomBuf = gBuf.GetResultGeometry(distance);
            return geomBuf;
        }

        /// <summary>
        /// Comutes the buffer for a geometry for a given buffer distance
        /// and accuracy of approximation.
        /// </summary>
        /// <param name="g"> the geometry to buffer</param>
        /// <param name="distance"> the buffer distance</param>
        /// <param name="parameters"> the buffer parameters to use</param>
        /// <returns> the buffer of the input geometry</returns>
        public static IGeometry Buffer(IGeometry g, double distance, IBufferParameters parameters)
        {
            BufferOp bufOp = new BufferOp(g, parameters);
            IGeometry geomBuf = bufOp.GetResultGeometry(distance);
            return geomBuf;
        }

        /// <summary>
        /// Comutes the buffer for a geometry for a given buffer distance
        /// and accuracy of approximation.
        /// </summary>
        /// <param name="g"> the geometry to buffer</param>
        /// <param name="distance"> the buffer distance</param>
        /// <param name="quadrantSegments"> the number of segments used to approximate a quarter circle</param>
        /// <returns> the buffer of the input geometry</returns>
        public static IGeometry Buffer(IGeometry g, double distance, int quadrantSegments)
        {
            BufferOp bufOp = new BufferOp(g) {QuadrantSegments = quadrantSegments};
            IGeometry geomBuf = bufOp.GetResultGeometry(distance);
            return geomBuf;
        }

        private void BufferFixedPrecision(IPrecisionModel fixedPrecModel)
        {
            INoder noder = new ScaledNoder(new McIndexSnapRounder(new PrecisionModel(1.0)),fixedPrecModel.Scale);

            var bufBuilder = new BufferBuilder(_bufParams);
            bufBuilder.WorkingPrecisionModel = fixedPrecModel;
            bufBuilder.Noder = noder;
            // this may throw an exception, if robustness errors are encountered
            _resultGeometry = bufBuilder.Buffer(_argGeom, _distance);
        }

        private void BufferOriginalPrecision()
        {
            try
            {
                // use fast noding by default
                var bufBuilder = new BufferBuilder(_bufParams);
                _resultGeometry = bufBuilder.Buffer(_argGeom, _distance);
            }
            catch (TopologyException ex)
            {
                _saveException = ex;
                // don't propagate the exception - it will be detected by fact that resultGeometry is null
            }
        }

        private void BufferReducedPrecision()
        {
            // try and compute with decreasing precision
            for (int precDigits = MaxPrecisionDigits; precDigits >= 0; precDigits--)
            {
                try
                {
                    BufferReducedPrecision(precDigits);
                }
                catch (TopologyException ex)
                {
                    // update the saved exception to reflect the new input geometry
                    _saveException = ex;
                    // don't propagate the exception - it will be detected by fact that resultGeometry is null
                }
                if (_resultGeometry != null) return;
            }

            // tried everything - have to bail
            throw _saveException;
        }

        private void BufferReducedPrecision(int precisionDigits)
        {
            double sizeBasedScaleFactor = PrecisionScaleFactor(_argGeom, _distance, precisionDigits);
            //    System.out.println("recomputing with precision scale factor = " + sizeBasedScaleFactor);

            var fixedPrecModel = new PrecisionModel(sizeBasedScaleFactor);
            BufferFixedPrecision(fixedPrecModel);
        }

        private void ComputeGeometry()
        {
            BufferOriginalPrecision();
            if (_resultGeometry != null) return;

            IPrecisionModel argPrecModel = _argGeom.Factory.PrecisionModel;
            if (argPrecModel.PrecisionModelType == PrecisionModelType.Fixed)
                BufferFixedPrecision(argPrecModel);
            else
                BufferReducedPrecision();
        }

        /// <summary>
        /// Returns the buffer computed for a geometry for a given buffer distance.
        ///</summary>
        /// <param name="distance"> the buffer distance</param>
        /// <returns> the buffer of the input geometry</returns>
        public virtual IGeometry GetResultGeometry(double distance)
        {
            _distance = distance;
            ComputeGeometry();
            return _resultGeometry;
        }

        /// <summary>
        /// Compute a scale factor to limit the precision of
        /// a given combination of Geometry and buffer distance.
        /// The scale factor is determined by 
        /// the number of digits of precision in the (geometry + buffer distance),
        /// limited by the supplied <paramref name="maxPrecisionDigits"/> value.
        /// <para/>
        /// The scale factor is based on the absolute magnitude of the (geometry + buffer distance).
        /// since this determines the number of digits of precision which must be handled.
        /// </summary>
        /// <param name="g"> the Geometry being buffered</param>
        /// <param name="distance"> the buffer distance</param>
        /// <param name="maxPrecisionDigits"> the max # of digits that should be allowed by
        ///          the precision determined by the computed scale factor</param>
        /// <returns> a scale factor for the buffer computation</returns>
        private static double PrecisionScaleFactor(IGeometry g, double distance, int maxPrecisionDigits)
        {
            IEnvelope env = g.EnvelopeInternal;
            var envMax = MathUtil.Max(
                Math.Abs(env.Maximum.X), Math.Abs(env.Maximum.Y),
                Math.Abs(env.Minimum.X), Math.Abs(env.Minimum.Y));

            double expandByDistance = distance > 0.0 ? distance : 0.0;
            double bufEnvMax = envMax + 2 * expandByDistance;

            // the smallest power of 10 greater than the buffer envelope
            int bufEnvPrecisionDigits = (int)(Math.Log(bufEnvMax) / Math.Log(10) + 1.0);
            int minUnitLog10 = maxPrecisionDigits - bufEnvPrecisionDigits;

            double scaleFactor = Math.Pow(10.0, minUnitLog10);
            return scaleFactor;
        }

        #endregion
    }
}