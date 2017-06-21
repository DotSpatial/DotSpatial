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

using System.Collections;

namespace DotSpatial.Topology.Utilities
{
    /// <summary>
    /// Extracts all the 2-dimensional (<c>Polygon</c>) components from a <c>Geometry</c>.
    /// </summary>
    public class PolygonExtracter : IGeometryFilter
    {
        private readonly IList _comps;

        /// <summary>
        /// Constructs a PolygonExtracterFilter with a list in which to store Polygons found.
        /// </summary>
        /// <param name="comps"></param>
        public PolygonExtracter(IList comps)
        {
            _comps = comps;
        }

        #region IGeometryFilter Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        public virtual void Filter(IGeometry geom)
        {
            if (geom is Polygon)
                _comps.Add(geom);
        }

        #endregion

        /// <summary>
        /// Returns the Polygon components from a single point.
        /// If more than one point is to be processed, it is more
        /// efficient to create a single <c>PolygonExtracterFilter</c> instance
        /// and pass it to multiple geometries.
        /// </summary>
        /// <param name="geom"></param>
        public static IList GetPolygons(IGeometry geom)
        {
            IList comps = new ArrayList();
            geom.Apply(new PolygonExtracter(comps));
            return comps;
        }
    }
}