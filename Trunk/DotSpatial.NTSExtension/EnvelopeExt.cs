using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;

namespace DotSpatial.NTSExtension
{
   public static class EnvelopeExt
    {

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
