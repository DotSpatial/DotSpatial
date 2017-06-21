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
using DotSpatial.Topology.Algorithm;

namespace DotSpatial.Topology.GeometriesGraph.Index
{
    /// <summary>
    ///
    /// </summary>
    public class SegmentIntersector
    {
        private readonly bool _includeProper;
        private readonly LineIntersector _li;
        private readonly bool _recordIsolated;

        /// <summary>
        /// Testing only.
        /// </summary>
        public int NumTests;

        private ICollection[] _bdyNodes;
        private bool _hasIntersection;
        private bool _hasProper;
        private bool _hasProperInterior;
        private int _numIntersections;
        private Coordinate _properIntersectionPoint;

        /// <summary>
        ///
        /// </summary>
        /// <param name="li"></param>
        /// <param name="includeProper"></param>
        /// <param name="recordIsolated"></param>
        public SegmentIntersector(LineIntersector li, bool includeProper, bool recordIsolated)
        {
            _li = li;
            _includeProper = includeProper;
            _recordIsolated = recordIsolated;
        }

        /// <returns>
        /// The proper intersection point, or <c>null</c> if none was found.
        /// </returns>
        public virtual Coordinate ProperIntersectionPoint
        {
            get
            {
                return _properIntersectionPoint;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public virtual bool HasIntersection
        {
            get
            {
                return _hasIntersection;
            }
        }

        /// <summary>
        /// A proper intersection is an intersection which is interior to at least two
        /// line segments.  Notice that a proper intersection is not necessarily
        /// in the interior of the entire Geometry, since another edge may have
        /// an endpoint equal to the intersection, which according to SFS semantics
        /// can result in the point being on the Boundary of the Geometry.
        /// </summary>
        public virtual bool HasProperIntersection
        {
            get
            {
                return _hasProper;
            }
        }

        /// <summary>
        /// A proper interior intersection is a proper intersection which is not
        /// contained in the set of boundary nodes set for this SegmentIntersector.
        /// </summary>
        public virtual bool HasProperInteriorIntersection
        {
            get
            {
                return _hasProperInterior;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i1"></param>
        /// <param name="i2"></param>
        /// <returns></returns>
        public static bool IsAdjacentSegments(int i1, int i2)
        {
            return Math.Abs(i1 - i2) == 1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="bdyNodes0"></param>
        /// <param name="bdyNodes1"></param>
        public virtual void SetBoundaryNodes(ICollection bdyNodes0, ICollection bdyNodes1)
        {
            _bdyNodes = new ICollection[2];
            _bdyNodes[0] = bdyNodes0;
            _bdyNodes[1] = bdyNodes1;
        }

        /// <summary>
        /// A trivial intersection is an apparent self-intersection which in fact
        /// is simply the point shared by adjacent line segments.
        /// Notice that closed edges require a special check for the point shared by the beginning
        /// and end segments.
        /// </summary>
        /// <param name="e0"></param>
        /// <param name="segIndex0"></param>
        /// <param name="e1"></param>
        /// <param name="segIndex1"></param>
        private bool IsTrivialIntersection(Edge e0, int segIndex0, Edge e1, int segIndex1)
        {
            if (ReferenceEquals(e0, e1))
            {
                if (_li.IntersectionNum == 1)
                {
                    if (IsAdjacentSegments(segIndex0, segIndex1))
                        return true;
                    if (e0.IsClosed)
                    {
                        int maxSegIndex = e0.NumPoints - 1;
                        if ((segIndex0 == 0 && segIndex1 == maxSegIndex) ||
                            (segIndex1 == 0 && segIndex0 == maxSegIndex))
                            return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// This method is called by clients of the EdgeIntersector class to test for and add
        /// intersections for two segments of the edges being intersected.
        /// Notice that clients (such as MonotoneChainEdges) may choose not to intersect
        /// certain pairs of segments for efficiency reasons.
        /// </summary>
        /// <param name="e0"></param>
        /// <param name="segIndex0"></param>
        /// <param name="e1"></param>
        /// <param name="segIndex1"></param>
        public virtual void AddIntersections(Edge e0, int segIndex0, Edge e1, int segIndex1)
        {
            // if (e0 == e1 && segIndex0 == segIndex1)
            if (ReferenceEquals(e0, e1) && segIndex0 == segIndex1)
                return;             // Diego Guidi say's: Avoid overload equality, i use references equality, otherwise TOPOLOGY ERROR!

            NumTests++;
            Coordinate p00 = e0.Coordinates[segIndex0];
            Coordinate p01 = e0.Coordinates[segIndex0 + 1];
            Coordinate p10 = e1.Coordinates[segIndex1];
            Coordinate p11 = e1.Coordinates[segIndex1 + 1];
            _li.ComputeIntersection(p00, p01, p10, p11);
            /*
             *  Always record any non-proper intersections.
             *  If includeProper is true, record any proper intersections as well.
             */
            if (_li.HasIntersection)
            {
                if (_recordIsolated)
                {
                    e0.Isolated = false;
                    e1.Isolated = false;
                }
                _numIntersections++;
                // if the segments are adjacent they have at least one trivial intersection,
                // the shared endpoint.  Don't bother adding it if it is the
                // only intersection.
                if (!IsTrivialIntersection(e0, segIndex0, e1, segIndex1))
                {
                    _hasIntersection = true;
                    if (_includeProper || !_li.IsProper)
                    {
                        e0.AddIntersections(_li, segIndex0, 0);
                        e1.AddIntersections(_li, segIndex1, 1);
                    }
                    if (_li.IsProper)
                    {
                        _properIntersectionPoint = (Coordinate)_li.GetIntersection(0).Clone();
                        _hasProper = true;
                        if (!IsBoundaryPoint(_li, _bdyNodes))
                            _hasProperInterior = true;
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="li"></param>
        /// <param name="bdyNodes"></param>
        /// <returns></returns>
        private static bool IsBoundaryPoint(LineIntersector li, ICollection[] bdyNodes)
        {
            if (bdyNodes == null)
                return false;
            if (IsBoundaryPoint(li, bdyNodes[0]))
                return true;
            if (IsBoundaryPoint(li, bdyNodes[1]))
                return true;
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="li"></param>
        /// <param name="bdyNodes"></param>
        /// <returns></returns>
        private static bool IsBoundaryPoint(LineIntersector li, IEnumerable bdyNodes)
        {
            for (IEnumerator i = bdyNodes.GetEnumerator(); i.MoveNext(); )
            {
                Node node = (Node)i.Current;
                Coordinate pt = node.Coordinate;
                if (li.IsIntersection(pt))
                    return true;
            }
            return false;
        }
    }
}