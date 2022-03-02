// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// PointShape.
    /// </summary>
    public static class PointShape
    {
        #region Properties

        /// <summary>
        /// Gets or sets the precision for calculating equality, but this is just a re-direction to Vertex.Epsilon.
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
        /// Calculates the intersection of a point shape without relying on the NTS geometry.
        /// </summary>
        /// <param name="pointShape">Point shape used for calculation.</param>
        /// <param name="otherShape">Other shape of any feature type.</param>
        /// <returns>True, if the given shape ranges intersect.</returns>
        public static bool Intersects(ShapeRange pointShape, ShapeRange otherShape)
        {
            if (pointShape.FeatureType != FeatureType.Point && pointShape.FeatureType != FeatureType.MultiPoint)
            {
                throw new ArgumentException(string.Format(DataStrings.Shape_WrongFeatureType, "pointShape", "point or multi point", pointShape.FeatureType));
            }

            // Implemented in PolygonShape or line shape. Point shape is the simplest and just looks for overlapping coordinates.
            if (otherShape.FeatureType == FeatureType.Polygon || otherShape.FeatureType == FeatureType.Line)
            {
                return otherShape.Intersects(pointShape);
            }

            // For two point-type shapes, test if any vertex from one overlaps with any vertex of the other within Epsilon tollerance
            return VerticesIntersect(pointShape, otherShape);
        }

        /// <summary>
        /// Returns true if any vertices overlap.
        /// </summary>
        /// <param name="pointShape">First point shape used for checking.</param>
        /// <param name="otherPointShape">Second point shape used for checking.</param>
        /// <returns>True if any vertices overlap.</returns>
        public static bool VerticesIntersect(ShapeRange pointShape, ShapeRange otherPointShape)
        {
            foreach (PartRange part in pointShape.Parts)
            {
                foreach (PartRange oPart in otherPointShape.Parts)
                {
                    foreach (Vertex v1 in part)
                    {
                        foreach (Vertex v2 in oPart)
                        {
                            if (v1 == v2) return true;
                        }
                    }
                }
            }

            return false;
        }

        #endregion
    }
}