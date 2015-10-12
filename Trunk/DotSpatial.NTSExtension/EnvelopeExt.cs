using System.Collections.Generic;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace DotSpatial.NTSExtension
{
    public static class EnvelopeExt
    {
        /// <summary>
        /// Gets the minY, which is Y - Height.
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> that this calculation is for.</param>
        /// <returns></returns>
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

            if (!double.IsNaN(self.MinZ)) result.Z = (self.MinZ + self.MaxZ) / 2;
            if (!double.IsNaN(self.MinM)) result.M = (self.MinM + self.MaxM) / 2;
            return result;
        }

        /// <summary>
        /// Gets the right value, which is X + Width.
        /// </summary>
        /// <param name="self">The <c>IEnvelope</c> that this calculation is for.</param>
        /// <returns></returns>
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
                new Coordinate(self.MaxX, self.MinX),
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
            return new Polygon(ToLinearRing(self));
        }
    }
}
