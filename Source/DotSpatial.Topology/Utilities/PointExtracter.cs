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
    /// Extracts all the 0-dimensional (<c>Point</c>) components from a <c>Geometry</c>.
    /// </summary>
    public class PointExtracter : IGeometryFilter
    {
        private readonly IList _pts;

        /// <summary>
        /// Constructs a PointExtracterFilter with a list in which to store Points found.
        /// </summary>
        /// <param name="pts"></param>
        public PointExtracter(IList pts)
        {
            _pts = pts;
        }

        #region IGeometryFilter Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="geom"></param>
        public virtual void Filter(IGeometry geom)
        {
            if (geom is Point)
                _pts.Add(geom);
        }

        #endregion

        /// <summary>
        /// Returns the Point components from a single point.
        /// If more than one point is to be processed, it is more
        /// efficient to create a single <c>PointExtracterFilter</c> instance
        /// and pass it to multiple geometries.
        /// </summary>
        /// <param name="geom"></param>
        public static IList GetPoints(IGeometry geom)
        {
            IList pts = new ArrayList();
            geom.Apply(new PointExtracter(pts));
            return pts;
        }
    }
}