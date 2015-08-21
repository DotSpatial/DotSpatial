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
using DotSpatial.Topology.Index;
using DotSpatial.Topology.Index.QuadTree;

namespace DotSpatial.Topology.Simplify
{
    /// <summary>
    /// An index of LineSegments.
    /// </summary>
    public class LineSegmentIndex
    {
        #region Fields

        private readonly ISpatialIndex<LineSegment>_index = new Quadtree<LineSegment>();

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="line"></param>
        public void Add(TaggedLineString line) 
        {
            TaggedLineSegment[] segs = line.Segments;
            for (int i = 0; i < segs.Length; i++) 
            {
                TaggedLineSegment seg = segs[i];
                Add(seg);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="seg"></param>
        public void Add(LineSegment seg)
        {
            _index.Insert(new Envelope(seg.P0, seg.P1), seg);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="querySeg"></param>
        /// <returns></returns>
        public IList<LineSegment> Query(LineSegment querySeg)
        {
            Envelope env = new Envelope(querySeg.P0, querySeg.P1);

            LineSegmentVisitor visitor = new LineSegmentVisitor(querySeg);
            _index.Query(env, visitor);
            IList<LineSegment> itemsFound = visitor.Items;        

            return itemsFound;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="seg"></param>
        public void Remove(LineSegment seg)
        {
            _index.Remove(new Envelope(seg.P0, seg.P1), seg);
        }

        #endregion
    }

    /// <summary>
    /// ItemVisitor subclass to reduce volume of query results.
    /// </summary>
    public class LineSegmentVisitor : IItemVisitor<LineSegment>
    {
        #region Fields

        private readonly IList<LineSegment> _items = new List<LineSegment>();
        // MD - only seems to make about a 10% difference in overall time.
        private readonly LineSegment _querySeg;

        #endregion

        #region Constructors

        /// <summary>
        ///
        /// </summary>
        /// <param name="querySeg"></param>
        public LineSegmentVisitor(LineSegment querySeg)
        {
            _querySeg = querySeg;
        }

        #endregion

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public IList<LineSegment> Items 
        {
            get
            {
                return _items;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        public void VisitItem(LineSegment item)
        {
            LineSegment seg = item;
            if (Envelope.Intersects(seg.P0, seg.P1, _querySeg.P0, _querySeg.P1))
                _items.Add(seg);
        }

        #endregion
    }
}