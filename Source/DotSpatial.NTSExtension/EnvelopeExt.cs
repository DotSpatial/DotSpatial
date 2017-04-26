using System.Collections.Generic;
using GeoAPI.Geometries;
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
        /// <param name="self">The <c>IEnvelope</c> to find the center for</param>
        /// <returns>An ICoordinate</returns>
        public static Coordinate Center(this Envelope self)
        {
            if (self == null || self.IsNull) return null;
            Coordinate result = new Coordinate
                                    {
                                        X = (self.MinX + self.MaxX) / 2,
                                        Y = (self.MinY + self.MaxY) / 2
                                    };

            if (!double.IsNaN(self.Minimum.Z)) result.Z = (self.Minimum.Z + self.Maximum.Z) / 2;
            if (!double.IsNaN(self.Minimum.M)) result.M = (self.Minimum.M + self.Maximum.M) / 2;
            return result;
        }

        /// <summary>
        /// Checks whether the given Envelope has M values.
        /// </summary>
        /// <param name="envelope">Envelope that gets checked.</param>
        /// <returns>False if either envelope.Minimum.M or envelope.Maximum.M is not a number or Minimum.M is bigger than Maximum.M. </returns>
        public static bool HasM(this Envelope envelope)
        {
            if (double.IsNaN(envelope.Minimum.M) || double.IsNaN(envelope.Maximum.M))
                return false;
            return envelope.Minimum.M <= envelope.Maximum.M;
        }

        /// <summary>
        /// Checks whether the given Envelope has Z values.
        /// </summary>
        /// <param name="envelope">Envelope that gets checked.</param>
        /// <returns>False if either envelope.Minimum.Z or envelope.Maximum.Z is not a number or Minimum.Z is bigger than Maximum.Z. </returns>
        public static bool HasZ(this Envelope envelope)
        {
            if (double.IsNaN(envelope.Minimum.Z) || double.IsNaN(envelope.Maximum.Z))
                return false;
            return envelope.Minimum.Z <= envelope.Maximum.Z;
        }

        /// <summary>
        /// Initializes the envelopes Minimum.M with the smaller of the two given m values and the Maximum.M with the bigger of the two given m values.
        /// </summary>
        /// <param name="envelope">Envelope, whos Minimum and Maximum.M should be initialized.</param>
        /// <param name="m1">First m value.</param>
        /// <param name="m2">Second m value.</param>
        public static void InitM(this Envelope envelope, double m1, double m2)
        {
            if (m1 < m2)
            {
                envelope.Minimum.M = m1;
                envelope.Maximum.M = m2;
            }
            else
            {
                envelope.Minimum.M = m2;
                envelope.Maximum.M = m1;
            }
        }

        /// <summary>
        /// Initializes the envelopes Minimum.Z with the smaller of the two given z values and the Maximum.Z with the bigger of the two given z values.
        /// </summary>
        /// <param name="envelope">Envelope, whose Minimum and Maximum.Z should be initialized.</param>
        /// <param name="z1">First z value.</param>
        /// <param name="z2">Second z value.</param>
        public static void InitZ(this Envelope envelope, double z1, double z2)
        {
            if (z1 < z2)
            {
                envelope.Minimum.Z = z1;
                envelope.Maximum.Z = z2;
            }
            else
            {
                envelope.Minimum.Z = z2;
                envelope.Maximum.Z = z1;
            }
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
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <returns>A Linear ring describing the border of this envelope.</returns>
        public static ILinearRing ToLinearRing(this Envelope self)
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
        /// <param name="self">The IEnvelope to use with this method</param>
        /// <returns>A Polygon, which technically qualifies as an IGeometry</returns>
        public static IPolygon ToPolygon(this Envelope self)
        {
            if (self.IsNull) return new Polygon(new LinearRing(new Coordinate[] { }));
            return new Polygon(ToLinearRing(self));
        }

        #endregion
    }
}