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

namespace DotSpatial.Topology.Simplify
{
    /// <summary>
    /// A LineSegment which is tagged with its location in a <c>Geometry</c>.
    /// Used to index the segments in a point and recover the segment locations
    /// from the index.
    /// </summary>
    public class TaggedLineSegment : LineSegment
    {
        private readonly int _index;
        private readonly IGeometry _parent;

        /// <summary>
        ///
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="parent"></param>
        /// <param name="index"></param>
        public TaggedLineSegment(Coordinate p0, Coordinate p1, IGeometry parent, int index)
            : base(p0, p1)
        {
            _parent = parent;
            _index = index;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        public TaggedLineSegment(Coordinate p0, Coordinate p1) : this(p0, p1, null, -1) { }

        /// <summary>
        ///
        /// </summary>
        public virtual IGeometry Parent
        {
            get
            {
                return _parent;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int Index
        {
            get
            {
                return _index;
            }
        }
    }
}