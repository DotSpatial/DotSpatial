// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/1/2010 11:01:12 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    public static class LineShape
    {
        /// <summary>
        /// Gets or sets the precision for calculating equality, but this is just a re-direction to Vertex.Epsilon
        /// </summary>
        public static double Epsilon
        {
            get { return Vertex.Epsilon; }
            set { Vertex.Epsilon = value; }
        }

        /// <summary>
        /// Calculates the intersection of a polygon shape without relying on the NTS geometry.
        /// </summary>
        /// <param name="lineShape">LineShape used for calculation.</param>
        /// <param name="otherShape">Other shape of any feature type.</param>
        /// <returns>True, if the given shape ranges intersect.</returns>
        public static bool Intersects(ShapeRange lineShape, ShapeRange otherShape)
        {
            if (lineShape.FeatureType != FeatureType.Line)
            {
                throw new ArgumentException(string.Format(DataStrings.Shape_WrongFeatureType, "lineShape", "line", lineShape.FeatureType)); 
            }

            // Implemented in PolygonShape
            if (otherShape.FeatureType == FeatureType.Polygon)
            {
                return otherShape.Intersects(lineShape);
            }

            // Test for point on a line
            if (otherShape.FeatureType == FeatureType.Point || otherShape.FeatureType == FeatureType.MultiPoint)
            {
                return IntersectsVertex(lineShape, otherShape);
            }

            // There is no other way for this polygon to intersect the other points
            if (otherShape.FeatureType == FeatureType.Point || otherShape.FeatureType == FeatureType.MultiPoint)
                return false;

            // For lines and polygons, if any segment intersects any segment of this polygon shape, return true.
            // This is essentially looking for the rare case of crossing in and crossing out again with
            // no actual points inside this polygonShape.
            return SegmentsIntersect(lineShape, otherShape);
        }

        /// <summary>
        /// Tests to see if any vertices from the other shape are coincident with a segment from this line shape.
        /// </summary>
        /// <param name="lineShape">LineShape used for calculation.</param>
        /// <param name="otherShape">Other shape of any feature type.</param>
        /// <returns>True, if the shapes intersect.</returns>
        public static bool IntersectsVertex(ShapeRange lineShape, ShapeRange otherShape)
        {
            foreach (PartRange part in lineShape.Parts)
            {
                foreach (Segment segment in part.Segments)
                {
                    foreach (PartRange oPart in otherShape.Parts)
                    {
                        foreach (Vertex v in oPart)
                        {
                            if (segment.IntersectsVertex(v)) return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns true if any segment from a line or polygon shape interesect any segments from the line or polygon shape.
        /// </summary>
        /// <param name="lineShape">A Line or Polygon shape</param>
        /// <param name="otherShape">Another line or polygon shape</param>
        /// <returns></returns>
        public static bool SegmentsIntersect(ShapeRange lineShape, ShapeRange otherShape)
        {
            if (lineShape.FeatureType != FeatureType.Line && lineShape.FeatureType != FeatureType.Polygon)
                throw new ArgumentException(string.Format(DataStrings.Shape_WrongFeatureType, "lineShape", "line or polygon", lineShape.FeatureType));
            if (otherShape.FeatureType != FeatureType.Line && otherShape.FeatureType != FeatureType.Polygon)
                throw new ArgumentException(string.Format(DataStrings.Shape_WrongFeatureType, "otherShape", "line or polygon", otherShape.FeatureType));

            foreach (PartRange part in lineShape.Parts)
            {
                foreach (Segment segment in part.Segments)
                {
                    foreach (PartRange oPart in otherShape.Parts)
                    {
                        foreach (Segment oSegment in oPart.Segments)
                        {
                            if (segment.Intersects(oSegment))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            // If the other segment is a polygon, we need to check the "

            return false;
        }
    }
}