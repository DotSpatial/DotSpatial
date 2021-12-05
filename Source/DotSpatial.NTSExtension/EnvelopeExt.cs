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