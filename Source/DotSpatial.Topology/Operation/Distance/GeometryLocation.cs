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

namespace DotSpatial.Topology.Operation.Distance
{
    /// <summary>
    /// Represents the location of a point on a Geometry.
    /// Maintains both the actual point location (which of course
    /// may not be exact) as well as information about the component
    /// and segment index where the point occurs.
    /// Locations inside area Geometrys will not have an associated segment index,
    /// so in this case the segment index will have the sentinel value of InsideArea.
    /// </summary>
    public class GeometryLocation
    {
        /// <summary>
        /// Special value of segment-index for locations inside area geometries. These
        /// locations do not have an associated segment index.
        /// </summary>
        public const int INSIDE_AREA = -1;

        private readonly IGeometry _component;
        private readonly Coordinate _pt;
        private readonly int _segIndex;

        /// <summary>
        /// Constructs a GeometryLocation specifying a point on a point, as well as the
        /// segment that the point is on (or InsideArea if the point is not on a segment).
        /// </summary>
        /// <param name="component"></param>
        /// <param name="segIndex"></param>
        /// <param name="pt"></param>
        public GeometryLocation(IGeometry component, int segIndex, Coordinate pt)
        {
            _component = component;
            _segIndex = segIndex;
            _pt = new Coordinate(pt);
        }

        /// <summary>
        /// Constructs a GeometryLocation specifying a point inside an area point.
        /// </summary>
        public GeometryLocation(IGeometry component, Coordinate pt) : this(component, INSIDE_AREA, pt) { }

        /// <summary>
        /// Returns the point associated with this location.
        /// </summary>
        public virtual IGeometry GeometryComponent
        {
            get
            {
                return _component;
            }
        }

        /// <summary>
        /// Returns the segment index for this location. If the location is inside an
        /// area, the index will have the value InsideArea;
        /// </summary>
        public virtual int SegmentIndex
        {
            get
            {
                return _segIndex;
            }
        }

        /// <summary>
        /// Returns the location.
        /// </summary>
        public virtual Coordinate Coordinate
        {
            get
            {
                return _pt;
            }
        }

        /// <summary>
        /// Returns whether this GeometryLocation represents a point inside an area point.
        /// </summary>
        public virtual bool IsInsideArea
        {
            get
            {
                return _segIndex == INSIDE_AREA;
            }
        }
    }
}