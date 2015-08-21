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

using System.Collections.Generic;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Operation.Distance
{
    /// <summary>
    /// Extracts a single point from each connected element in a Geometry
    /// (e.g. a polygon, linestring or point) and returns them in a list.
    /// </summary>
    public class ConnectedElementPointFilter : IGeometryFilter
    {
        #region Fields

        private readonly IList<Coordinate> _pts;

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pts"></param>
        ConnectedElementPointFilter(IList<Coordinate> pts)
        {
            _pts = pts;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="geom"></param>
        public void Filter(IGeometry geom)
        {
            if (geom is IPoint || geom is ILineString || geom is IPolygon)
                _pts.Add(geom.Coordinate);
        }

        /// <summary>
        /// Returns a list containing a Coordinate from each Polygon, LineString, and Point
        /// found inside the specified point. Thus, if the specified point is
        /// not a GeometryCollection, an empty list will be returned.
        /// </summary>
        public static IList<Coordinate> GetCoordinates(Geometry geom)
        {
            var pts = new List<Coordinate>();
            geom.Apply(new ConnectedElementPointFilter(pts));
            return pts;
        }

        #endregion
    }
}