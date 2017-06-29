// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/1/2010 11:01:12 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    /// <summary>
    /// LineShape
    /// </summary>
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
        /// Calculates the intersection of a polygon shape without relying on the NTS geometry
        /// </summary>
        /// <param name="lineShape"></param>
        /// <param name="otherShape"></param>
        /// <returns></returns>
        public static bool Intersects(ShapeRange lineShape, ShapeRange otherShape)
        {
            if (lineShape.FeatureType != FeatureType.Line)
            {
                throw new ArgumentException("The First parameter should be a point shape, but it was featuretype:" + lineShape.FeatureType);
            }

            // Implmented in PolygonShape
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
        /// <param name="lineShape"></param>
        /// <param name="otherShape"></param>
        /// <returns></returns>
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
                throw new ArgumentException("Expected lineShape to be a line or polygon feature type, got " + lineShape.FeatureType);
            if (otherShape.FeatureType != FeatureType.Line && otherShape.FeatureType != FeatureType.Polygon)
                throw new ArgumentException("Expected otherShape to be a line or polygon feature type, got " + otherShape.FeatureType);

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