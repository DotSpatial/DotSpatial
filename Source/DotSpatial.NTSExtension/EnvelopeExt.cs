// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace DotSpatial.NTSExtension
{
    /// <summary>
    /// This contains extension methods for GeoApi.Geometries.Envelope.
    /// </summary>
    public static class EnvelopeExt
    {
        #region Methods

        /// <summary>
        /// Gets the minY, which is Y - Height.
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> that this calculation is for.</param>
        /// <returns>The bottom value.</returns>
        public static double Bottom(this Envelope self)
        {
            return self.MinY;
        }

        /// <summary>
        /// Gets the coordinate defining the center of this envelope
        /// in all of the dimensions of this envelope.
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> to find the center for.</param>
        /// <returns>An ICoordinate.</returns>
        public static Coordinate Center(this Envelope self)
        {
            if (self == null || self.IsNull) return null;
            Coordinate result = new Coordinate
                                    {
                                        X = (self.MinX + self.MaxX) / 2,
                                        Y = (self.MinY + self.MaxY) / 2
                                    };

            if (!double.IsNaN(self.MinZ)) result.Z = (self.MinZ + self.MaxZ) / 2;
            if (!double.IsNaN(self.MinM)) result.M = (self.MinM + self.MaxM) / 2;
            return result;
        }

        /// <summary>
        /// Checks whether the given Envelope has M values.
        /// </summary>
        /// <param name="envelope">Envelope that gets checked.</param>
        /// <returns>False if either envelope.Minimum.M or envelope.Maximum.M is not a number or Minimum.M is bigger than Maximum.M. </returns>
        public static bool HasM(this Envelope envelope)
        {
            if (double.IsNaN(envelope.MinM) || double.IsNaN(envelope.MaxM))
                return false;
            return envelope.MinM <= envelope.MaxM;
        }

        /// <summary>
        /// Checks whether the given Envelope has Z values.
        /// </summary>
        /// <param name="envelope">Envelope that gets checked.</param>
        /// <returns>False if either envelope.Minimum.Z or envelope.Maximum.Z is not a number or Minimum.Z is bigger than Maximum.Z. </returns>
        public static bool HasZ(this Envelope envelope)
        {
            if (double.IsNaN(envelope.MinZ) || double.IsNaN(envelope.MaxZ))
                return false;
            return envelope.MinZ <= envelope.MaxZ;
        }

        /// <summary>
        /// Gets the right value, which is X + Width.
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> that this calculation is for.</param>
        /// <returns>The right value.</returns>
        public static double Right(this Envelope self)
        {
            return self.MaxX;
        }

        /// <summary>
        /// Converts this envelope into a linear ring.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method.</param>
        /// <returns>A Linear ring describing the border of this envelope.</returns>
        public static LinearRing ToLinearRing(this Envelope self)
        {
            // Holes are counter clockwise, so this should create a clockwise linear ring.
            var coords = new List<Coordinate>
                             {
                                 new Coordinate(self.MinX, self.MaxY),
                                 new Coordinate(self.MaxX, self.MaxY),
                                 new Coordinate(self.MaxX, self.MinY),
                                 new Coordinate(self.MinX, self.MinY),
                                 new Coordinate(self.MinX, self.MaxY)
                             };

            // close the polygon
            return new LinearRing(coords.ToArray());
        }

        /// <summary>
        /// Technically an Evelope object is not actually a geometry.
        /// This creates a polygon from the extents.
        /// </summary>
        /// <param name="self">The IEnvelope to use with this method.</param>
        /// <returns>A Polygon, which technically qualifies as an IGeometry.</returns>
        public static Polygon ToPolygon(this Envelope self)
        {
            if (self.IsNull) return new Polygon(new LinearRing(new Coordinate[] { }));
            return new Polygon(ToLinearRing(self));
        }

        #endregion
    }
}