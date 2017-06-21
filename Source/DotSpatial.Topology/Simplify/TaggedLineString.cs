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
using System.Collections.Generic;

namespace DotSpatial.Topology.Simplify
{
    /// <summary>
    ///
    /// </summary>
    public class TaggedLineString
    {
        private readonly int _minimumSize;
        private readonly LineString _parentLine;
        private readonly IList _resultSegs = new ArrayList();
        private TaggedLineSegment[] _segs;

        /// <summary>
        ///
        /// </summary>
        /// <param name="parentLine"></param>
        public TaggedLineString(LineString parentLine) : this(parentLine, 2) { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="parentLine"></param>
        /// <param name="minimumSize"></param>
        public TaggedLineString(LineString parentLine, int minimumSize)
        {
            _parentLine = parentLine;
            _minimumSize = minimumSize;
            Init();
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int MinimumSize
        {
            get
            {
                return _minimumSize;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual LineString Parent
        {
            get
            {
                return _parentLine;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual IList<Coordinate> ParentCoordinates
        {
            get
            {
                return _parentLine.Coordinates;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual Coordinate[] ResultCoordinates
        {
            get
            {
                return ExtractCoordinates(_resultSegs);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual int ResultSize
        {
            get
            {
                int resultSegsSize = _resultSegs.Count;
                return resultSegsSize == 0 ? 0 : resultSegsSize + 1;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual TaggedLineSegment[] Segments
        {
            get
            {
                return _segs;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual TaggedLineSegment GetSegment(int i)
        {
            return _segs[i];
        }

        /// <summary>
        ///
        /// </summary>
        private void Init()
        {
            IList<Coordinate> pts = _parentLine.Coordinates;
            _segs = new TaggedLineSegment[pts.Count - 1];
            for (int i = 0; i < pts.Count - 1; i++)
            {
                TaggedLineSegment seg
                         = new TaggedLineSegment(pts[i], pts[i + 1], _parentLine, i);
                _segs[i] = seg;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="seg"></param>
        public virtual void AddToResult(LineSegment seg)
        {
            _resultSegs.Add(seg);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual ILineString AsLineString()
        {
            return _parentLine.Factory.CreateLineString(ExtractCoordinates(_resultSegs));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual ILinearRing AsLinearRing()
        {
            return _parentLine.Factory.CreateLinearRing(ExtractCoordinates(_resultSegs));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="segs"></param>
        /// <returns></returns>
        private static Coordinate[] ExtractCoordinates(IList segs)
        {
            Coordinate[] pts = new Coordinate[segs.Count + 1];
            LineSegment seg = null;
            for (int i = 0; i < segs.Count; i++)
            {
                seg = (LineSegment)segs[i];
                pts[i] = seg.P0;
            }
            // add last point
            if (seg != null) pts[pts.Length - 1] = seg.P1;
            return pts;
        }
    }
}