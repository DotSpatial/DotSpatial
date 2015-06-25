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
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/1/2010 11:43:08 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Linq;
using DotSpatial.Topology;

namespace DotSpatial.Data
{
    public static class PointShape
    {
        /// <summary>
        /// Calculates the intersection of a polygon shape without relying on the NTS geometry
        /// </summary>
        /// <param name="pointShape"></param>
        /// <param name="otherShape"></param>
        /// <returns></returns>
        public static bool Intersects(ShapeRange pointShape, ShapeRange otherShape)
        {
            if (pointShape.FeatureType != FeatureType.Point && pointShape.FeatureType != FeatureType.MultiPoint)
            {
                throw new ArgumentException("The First parameter should be a point shape, but it was featuretype:" + pointShape.FeatureType);
            }

            // Implmented in PolygonShape or line shape.  Point shape is the simplest and just looks for overlapping coordinates.
            if (otherShape.FeatureType == FeatureType.Polygon || otherShape.FeatureType == FeatureType.Line)
            {
                return otherShape.Intersects(pointShape);
            }

            // For two point-type shapes, test if any vertex from one overlaps with any vertex of the other within Epsilon tollerance
            return VerticesIntersect(pointShape, otherShape);
        }

        /// <summary>
        /// Returns true if any vertices overlap
        /// </summary>
        /// <returns></returns>
        public static bool VerticesIntersect(ShapeRange pointShape, ShapeRange otherPointShape)
        {
            return pointShape.Parts
                .Any(part => otherPointShape.Parts
                    .Any(oPart => part
                        .Any(v1 => oPart
                            .Any(v2 => v1 == v2))));
        }
    }
}