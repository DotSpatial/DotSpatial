// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Algorithm
{
    /// <summary> 
    /// Computes the centroid of a linear point.
    /// Algorithm:
    /// Compute the average of the midpoints
    /// of all line segments weighted by the segment length.
    /// </summary>
    [Obsolete("Use Centroid instead")]
    public class CentroidLine
    {
        #region Fields

        private readonly Coordinate _centSum = new Coordinate();
        private double _totalLength;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Coordinate Centroid
        {
            get
            {
                Coordinate cent = new Coordinate();
                cent.X = _centSum.X / _totalLength;
                cent.Y = _centSum.Y / _totalLength;
                return cent;
            }
        }

        #endregion

        #region Methods

        /// <summary> 
        /// Adds the linear components of by a Geometry to the centroid total.
        /// If the geometry has no linear components it does not contribute to the centroid.
        /// </summary>
        /// <param name="geom">The point to add.</param>
        public void Add(IGeometry geom)
        {
            if (geom is ILineString)             
                Add(geom.Coordinates);

            else if (geom is IPolygon)
            {
                var poly = (IPolygon) geom;
                // add linear components of a polygon
                Add(poly.ExteriorRing.Coordinates);
                for (int i = 0; i < poly.NumInteriorRings; i++)
                {
                    Add(poly.GetInteriorRingN(i).Coordinates);
                }
            }

            else if (geom is IGeometryCollection)
            {
                var gc = (IGeometryCollection)geom;
                foreach (IGeometry geometry in gc.Geometries)
                    Add(geometry);
            }
        }

        /// <summary> 
        /// Adds the length defined by an array of coordinates.
        /// </summary>
        /// <param name="pts">An array of <c>Coordinate</c>s.</param>
        public void Add(IList<Coordinate> pts)
        {
            for (int i = 0; i < pts.Count - 1; i++)
            {
                double segmentLen = pts[i].Distance(pts[i + 1]);
                _totalLength += segmentLen;

                double midx = (pts[i].X + pts[i + 1].X) / 2;
                _centSum.X += segmentLen * midx;
                double midy = (pts[i].Y + pts[i + 1].Y) / 2;
                _centSum.Y += segmentLen * midy;
            }
        }

        #endregion
    }
}
