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
using System.Collections;
using DotSpatial.Topology.Index;
using DotSpatial.Topology.Index.Quadtree;

namespace DotSpatial.Topology.Simplify
{
    /// <summary>
    /// An index of LineSegments.
    /// </summary>
    public class LineSegmentIndex
    {
        private readonly Quadtree _index = new Quadtree();

        /// <summary>
        ///
        /// </summary>
        /// <param name="line"></param>
        public virtual void Add(TaggedLineString line)
        {
            TaggedLineSegment[] segs = line.Segments;
            for (int i = 0; i < segs.Length - 1; i++)
            {
                TaggedLineSegment seg = segs[i];
                Add(seg);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="seg"></param>
        public virtual void Add(LineSegment seg)
        {
            _index.Insert(new Envelope(seg.P0, seg.P1), seg);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="seg"></param>
        public virtual void Remove(LineSegment seg)
        {
            _index.Remove(new Envelope(seg.P0, seg.P1), seg);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="querySeg"></param>
        /// <returns></returns>
        public virtual IList Query(LineSegment querySeg)
        {
            Envelope env = new Envelope(querySeg.P0, querySeg.P1);

            LineSegmentVisitor visitor = new LineSegmentVisitor(querySeg);
            _index.Query(env, visitor);
            IList itemsFound = visitor.Items;

            return itemsFound;
        }
    }

    /// <summary>
    /// ItemVisitor subclass to reduce volume of query results.
    /// </summary>
    public class LineSegmentVisitor : IItemVisitor
    {
        // MD - only seems to make about a 10% difference in overall time.
        private readonly ArrayList _items = new ArrayList();
        private readonly LineSegment _querySeg;

        /// <summary>
        ///
        /// </summary>
        /// <param name="querySeg"></param>
        public LineSegmentVisitor(LineSegment querySeg)
        {
            _querySeg = querySeg;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual ArrayList Items
        {
            get
            {
                return _items;
            }
        }

        #region IItemVisitor Members

        /// <summary>
        ///
        /// </summary>
        /// <param name="item"></param>
        public virtual void VisitItem(Object item)
        {
            LineSegment seg = (LineSegment)item;
            if (Envelope.Intersects(seg.P0, seg.P1, _querySeg.P0, _querySeg.P1))
                _items.Add(item);
        }

        #endregion
    }
}