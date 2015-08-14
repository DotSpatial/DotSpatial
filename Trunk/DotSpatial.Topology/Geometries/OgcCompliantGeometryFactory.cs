using System;
using System.Collections.Generic;
using DotSpatial.Topology.Algorithm;

namespace DotSpatial.Topology.Geometries
{
    /// <summary>
    /// OGC compliant geometry factory
    /// </summary>
    public class OgcCompliantGeometryFactory : GeometryFactory
    {
        #region Constructors

        /// <summary>
        /// Creates an instance of this class using the default 
        /// values for <see cref="GeometryFactory.SRID"/>, 
        /// <see cref="GeometryFactory.PrecisionModel"/> and
        /// <see cref="GeometryFactory.CoordinateSequenceFactory"/>.
        /// </summary>
        public OgcCompliantGeometryFactory()
        { }

        /// <summary>
        /// Creates an instance of this class using the default 
        /// values for <see cref="GeometryFactory.SRID"/>, 
        /// <see cref="GeometryFactory.PrecisionModel"/>, 
        /// but the specified <paramref name="factory"/>.
        /// </summary>
        public OgcCompliantGeometryFactory(ICoordinateSequenceFactory factory)
            : base(factory) { }

        /// Creates an instance of this class using the default 
        /// values for <see cref="GeometryFactory.SRID"/>, 
        /// <see cref="GeometryFactory.CoordinateSequenceFactory"/> but the
        /// specified <paramref name="pm"/>.
        public OgcCompliantGeometryFactory(IPrecisionModel pm)
            : base(pm) { }

        public OgcCompliantGeometryFactory(IPrecisionModel pm, int srid)
            : base(pm, srid) { }

        public OgcCompliantGeometryFactory(IPrecisionModel pm, int srid, ICoordinateSequenceFactory factory)
            : base(pm, srid, factory) { }

        #endregion

        #region Methods

        private ILinearRing CreateLinearRing(Coordinate[] coordinates, bool ccw)
        {
            if (coordinates != null && CgAlgorithms.IsCounterClockwise(coordinates) != ccw)
                Array.Reverse(coordinates);
            return CreateLinearRing(coordinates);
        }

        private ILinearRing CreateLinearRing(ICoordinateSequence coordinates, bool ccw)
        {
            if (coordinates != null && CgAlgorithms.IsCounterClockwise(coordinates) != ccw)
            {
                //CoordinateSequences.Reverse(coordinates);
                coordinates = coordinates.Reversed();
            }
            return CreateLinearRing(coordinates);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The <see cref="IPolygon.ExteriorRing"/> is guaranteed to be orientated counter-clockwise.
        /// </remarks>
        public override IPolygon CreatePolygon(IEnumerable<Coordinate> coordinates)
        {
            var ring = CreateLinearRing(coordinates, true);
            return base.CreatePolygon(ring);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The <see cref="IPolygon.ExteriorRing"/> is guaranteed to be orientated counter-clockwise.
        /// </remarks>
        public override IPolygon CreatePolygon(ICoordinateSequence coordinates)
        {
            var ring = CreateLinearRing(coordinates, true);
            return base.CreatePolygon(ring);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The <see cref="IPolygon.ExteriorRing"/> is guaranteed to be orientated counter-clockwise.
        /// </remarks>
        public override IPolygon CreatePolygon(ILinearRing shell)
        {
            return CreatePolygon(shell, null);
        }

        /// <inheritdoc/>
        /// <remarks>
        /// The <see cref="IPolygon.ExteriorRing"/> is guaranteed to be orientated counter-clockwise.
        /// <br/>The <see cref="IPolygon.InteriorRings"/> are guaranteed to be orientated clockwise.
        /// </remarks>
        public override IPolygon CreatePolygon(ILinearRing shell, ILinearRing[] holes)
        {
            if (shell != null)
            {
                if (!shell.IsCounterClockwise)
                    shell = ReverseRing(shell);
            }

            if (holes != null)
            {
                for (var i = 0; i < holes.Length; i++)
                {
                    if (holes[i].IsCounterClockwise)
                        holes[i] = ReverseRing(holes[i]);
                }
            }

            return base.CreatePolygon(shell, holes);
        }

        private static ILinearRing ReverseRing(ILinearRing ring)
        {
            return (ILinearRing)ring.Reverse();
        }

        /// <inheritdoc/>
        public override IGeometry ToGeometry(IEnvelope envelope)
        {
            // null envelope - return empty point geometry
            if (envelope.IsNull)
                return CreatePoint((ICoordinateSequence)null);

            // point?
            if (envelope.Minimum.X == envelope.Maximum.X && envelope.Minimum.Y == envelope.Maximum.Y)
                return CreatePoint(new Coordinate(envelope.Minimum.X, envelope.Minimum.Y));

            // vertical or horizontal line?
            if (envelope.Minimum.X == envelope.Maximum.X
                    || envelope.Minimum.Y == envelope.Maximum.Y)
            {
                return CreateLineString(new[] 
                    {
                        new Coordinate(envelope.Minimum.X, envelope.Minimum.Y),
                        new Coordinate(envelope.Maximum.X, envelope.Maximum.Y)
                    });
            }

            // return CCW polygon
            var ring = CreateLinearRing(new[]
            {
                new Coordinate(envelope.Minimum.X, envelope.Minimum.Y),
                new Coordinate(envelope.Maximum.X, envelope.Minimum.Y),
                new Coordinate(envelope.Maximum.X, envelope.Maximum.Y),
                new Coordinate(envelope.Minimum.X, envelope.Maximum.Y),
                new Coordinate(envelope.Minimum.X, envelope.Minimum.Y)
            });

            //this is ccw so no need to check that again
            return base.CreatePolygon(ring, null);
        }

        #endregion
    }
}