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

namespace DotSpatial.Topology.Operation.Valid
{
    /// <summary>
    /// Implements the appropriate checks for repeated points
    /// (consecutive identical coordinates) as defined in the
    /// NTS spec.
    /// </summary>
    public class RepeatedPointTester
    {
        // save the repeated coord found (if any)
        private Coordinate _repeatedCoord;

        /// <summary>
        ///
        /// </summary>
        public virtual Coordinate Coordinate
        {
            get
            {
                return _repeatedCoord;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        protected virtual bool HasRepeatedPoint(IGeometry g)
        {
            if (g.IsEmpty) return false;
            if (g is Point) return false;
            if (g is MultiPoint) return false;
            // LineString also handles LinearRings
            if (g is LineString)
                return HasRepeatedPoint(g.Coordinates);
            if (g is Polygon)
                return HasRepeatedPoint((IPolygon)g);
            if (g is GeometryCollection)
                return HasRepeatedPoint((IGeometryCollection)g);
            throw new NotSupportedException(g.GetType().FullName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="coord"></param>
        /// <returns></returns>
        public virtual bool HasRepeatedPoint(IList<Coordinate> coord)
        {
            for (int i = 1; i < coord.Count; i++)
            {
                if (coord[i - 1].Equals(coord[i]))
                {
                    _repeatedCoord = coord[i];
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool HasRepeatedPoint(IPolygon p)
        {
            if (HasRepeatedPoint(p.Shell.Coordinates))
                return true;
            for (int i = 0; i < p.NumHoles; i++)
                if (HasRepeatedPoint(p.GetInteriorRingN(i).Coordinates))
                    return true;
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="gc"></param>
        /// <returns></returns>
        private bool HasRepeatedPoint(IGeometryCollection gc)
        {
            for (int i = 0; i < gc.NumGeometries; i++)
            {
                IGeometry g = gc.GetGeometryN(i);
                if (HasRepeatedPoint(g))
                    return true;
            }
            return false;
        }
    }
}