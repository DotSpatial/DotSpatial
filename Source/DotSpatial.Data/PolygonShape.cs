// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// Polygon contains algorithms for the case where the current feature type is a polygon. The shape may be single or multi-part.
    /// </summary>
    public static class PolygonShape
    {
        #region Properties

        /// <summary>
        /// Gets or sets the precision for calculating equality, but this is just a re-direction to Vertex.Epsilon
        /// </summary>
        public static double Epsilon
        {
            get
            {
                return Vertex.Epsilon;
            }

            set
            {
                Vertex.Epsilon = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This cycles through all the vertices, which are stored as {X1, Y1, X2, Y2...Xn, Yn} and tests
        /// if any of those vertices falls within the polygon shape.
        /// </summary>
        /// <param name="polygonShape">Polygon shape used for calculation.</param>
        /// <param name="otherShape">Other shape of any feature type.</param>
        /// <returns>True, if the given shape ranges intersect.</returns>
        public static bool ContainsVertex(ShapeRange polygonShape, ShapeRange otherShape)
        {
            foreach (PartRange otherPart in otherShape.Parts)
            {
                if (ContainsVertex(polygonShape, otherPart))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Calculates the intersection of a polygon shape without relying on the NTS geometry
        /// </summary>
        /// <param name="polygonShape">Polygon shape used for calculation.</param>
        /// <param name="otherShape">Other shape of any feature type.</param>
        /// <returns>True, if the given shape ranges intersect.</returns>
        public static bool Intersects(ShapeRange polygonShape, ShapeRange otherShape)
        {
            if (polygonShape.FeatureType != FeatureType.Polygon)
            {
                throw new ArgumentException(string.Format(DataStrings.Shape_WrongFeatureType, "polygonShape", "polygon", polygonShape.FeatureType));
            }

            // Extents will have already been tested by this point.
            // Regardless of what the other shapetype is, a coordinate inside this polygon indicates a hit.
            if (ContainsVertex(polygonShape, otherShape)) return true;

            // There is no other way for this polygon to intersect the other points
            if (otherShape.FeatureType == FeatureType.Point || otherShape.FeatureType == FeatureType.MultiPoint) return false;

            // For lines and polygons, if any segment intersects any segment of this polygon shape, return true.
            // This is essentially looking for the rare case of crossing in and crossing out again with
            // no actual points inside this polygonShape.
            if (LineShape.SegmentsIntersect(polygonShape, otherShape)) return true;

            // There is no other way for this polygon to intersect the other lines
            if (otherShape.FeatureType == FeatureType.Line) return false;

            // If the other polygon completely contains this polygon every other test has returned false,
            // but this will test for that case with only a single point.
            Vertex v = polygonShape.First();
            ShapeRange onlyFirstPoint = new ShapeRange(v);
            return ContainsVertex(otherShape, onlyFirstPoint);
        }

        /// <summary>
        /// For each coordinate in the other part, if it falls in the extent of this polygon, a
        /// ray crossing test is used for point in polygon testing. If it is not in the extent, it is skipped.
        /// </summary>
        /// <param name="polygonShape">The part of the polygon to analyze polygon</param>
        /// <param name="otherPart">The other part</param>
        /// <returns>Boolean, true if any coordinate falls inside the polygon.</returns>
        private static bool ContainsVertex(ShapeRange polygonShape, PartRange otherPart)
        {
            // Create an extent for faster checking in most cases
            Extent ext = polygonShape.Extent;

            foreach (Vertex point in otherPart)
            {
                // This extent check shortcut should help speed things up for large polygon parts
                if (!ext.Intersects(point)) continue;

                // Imagine a ray on the horizontal starting from point.X -> infinity. (In practice this can be ext.XMax)
                // Count the intersections of segments with that line. If the resulting count is odd, the point is inside.
                Segment ray = new Segment(point.X, point.Y, ext.MaxX, point.Y);
                int[] numCrosses = new int[polygonShape.NumParts]; // A cross is a complete cross. Coincident doesn't count because it is either 0 or 2 crosses.
                int totalCrosses = 0;
                int iPart = 0;
                foreach (PartRange ring in polygonShape.Parts)
                {
                    foreach (Segment segment in ring.Segments)
                    {
                        if (segment.IntersectionCount(ray) != 1) continue;

                        numCrosses[iPart]++;
                        totalCrosses++;
                    }

                    iPart++;
                }

                // If we didn't actually have any polygons we cant intersect with anything
                if (polygonShape.NumParts < 1) return false;

                // For shapes with only one part, we don't need to test part-containment.
                if (polygonShape.NumParts == 1 && totalCrosses % 2 == 1) return true;

                // This used to check to see if totalCrosses == 1, but now checks to see if totalCrosses is an odd number.
                // This change was made to solve the issue described in HD Issue 8593 (http://hydrodesktop.codeplex.com/workitem/8593).
                if (totalCrosses % 2 == 1) return true;

                totalCrosses = 0;
                for (iPart = 0; iPart < numCrosses.Length; iPart++)
                {
                    int count = numCrosses[iPart];

                    // If this part does not contain the point, don't bother trying to figure out if the part is a hole or not.
                    if (count % 2 == 0) continue;

                    // If this particular part is a hole, subtract the total crosses by 1,  otherwise add one.
                    // This takes time, so we want to do this as few times as possible.
                    if (polygonShape.Parts[iPart].IsHole())
                    {
                        totalCrosses--;
                    }
                    else
                    {
                        totalCrosses++;
                    }
                }

                return totalCrosses > 0;
            }

            return false;
        }

        #endregion
    }
}