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
using DotSpatial.Topology.Algorithm;
using DotSpatial.Topology.Geometries;
using DotSpatial.Topology.GeometriesGraph;

namespace DotSpatial.Topology.Operation.Buffer
{
    /// <summary>
    /// Locates a subgraph inside a set of subgraphs,
    /// in order to determine the outside depth of the subgraph.
    /// The input subgraphs are assumed to have had depths
    /// already calculated for their edges.
    /// </summary>
    public class SubgraphDepthLocater
    {
        #region Fields

        private readonly LineSegment _seg = new LineSegment();
        private readonly IList<BufferSubgraph> _subgraphs;

        #endregion

        #region Constructors

        /// <summary>
        ///
        /// </summary>
        /// <param name="subgraphs"></param>
        public SubgraphDepthLocater(IList<BufferSubgraph> subgraphs)
        {
            _subgraphs = subgraphs;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds all non-horizontal segments intersecting the stabbing line.
        /// The stabbing line is the ray to the right of stabbingRayLeftPt.
        /// </summary>
        /// <param name="stabbingRayLeftPt">The left-hand origin of the stabbing line.</param>
        /// <returns>A List of {DepthSegments} intersecting the stabbing line.</returns>
        private IList<DepthSegment> FindStabbedSegments(Coordinate stabbingRayLeftPt)
        {
            IList<DepthSegment> stabbedSegments = new List<DepthSegment>();
            foreach (var bsg in _subgraphs)
            {
                FindStabbedSegments(stabbingRayLeftPt, bsg.DirectedEdges, stabbedSegments);
            }
            return stabbedSegments;
        }

        /// <summary>
        /// Finds all non-horizontal segments intersecting the stabbing line
        /// in the list of dirEdges.
        /// The stabbing line is the ray to the right of stabbingRayLeftPt.
        /// </summary>
        /// <param name="stabbingRayLeftPt">The left-hand origin of the stabbing line.</param>
        /// <param name="dirEdges"></param>
        /// <param name="stabbedSegments">The current list of DepthSegments intersecting the stabbing line.</param>
        private void FindStabbedSegments(Coordinate stabbingRayLeftPt, IEnumerable<DirectedEdge> dirEdges, IList<DepthSegment> stabbedSegments)
        {
            /*
            * Check all forward DirectedEdges only.  This is still general,
            * because each Edge has a forward DirectedEdge.
            */
            foreach (DirectedEdge de in dirEdges)
            {
                if (!de.IsForward)
                    continue;
                FindStabbedSegments(stabbingRayLeftPt, de, stabbedSegments);
            }
        }

        /// <summary>
        /// Finds all non-horizontal segments intersecting the stabbing line
        /// in the input dirEdge.
        /// The stabbing line is the ray to the right of stabbingRayLeftPt.
        /// </summary>
        /// <param name="stabbingRayLeftPt">The left-hand origin of the stabbing line.</param>
        /// <param name="dirEdge"></param>
        /// <param name="stabbedSegments">The current list of DepthSegments intersecting the stabbing line.</param>
        private void FindStabbedSegments(Coordinate stabbingRayLeftPt, DirectedEdge dirEdge, IList<DepthSegment> stabbedSegments)
        {
            IList<Coordinate> pts = dirEdge.Edge.Coordinates;
            for (int i = 0; i < pts.Count - 1; i++)
            {
                _seg.P0 = pts[i];
                _seg.P1 = pts[i + 1];
                // ensure segment always points upwards
                if (_seg.P0.Y > _seg.P1.Y)
                    _seg.Reverse();

                // skip segment if it is left of the stabbing line
                var maxx = Math.Max(_seg.P0.X, _seg.P1.X);
                if (maxx < stabbingRayLeftPt.X) continue;

                // skip horizontal segments (there will be a non-horizontal one carrying the same depth info
                if (_seg.IsHorizontal) continue;

                // skip if segment is above or below stabbing line
                if (stabbingRayLeftPt.Y < _seg.P0.Y || stabbingRayLeftPt.Y > _seg.P1.Y) continue;

                // skip if stabbing ray is right of the segment
                if (CgAlgorithms.ComputeOrientation(_seg.P0, _seg.P1, stabbingRayLeftPt) == CgAlgorithms.Right) continue;

                // stabbing line cuts this segment, so record it
                int depth = dirEdge.GetDepth(PositionType.Left);
                // if segment direction was flipped, use RHS depth instead
                if (!_seg.P0.Equals(pts[i]))
                    depth = dirEdge.GetDepth(PositionType.Right);
                DepthSegment ds = new DepthSegment(_seg, depth);
                stabbedSegments.Add(ds);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual int GetDepth(Coordinate p)
        {
            //ArrayList stabbedSegments = new ArrayList(FindStabbedSegments(p).CastPlatform());
            var stabbedSegments = new List<DepthSegment>(FindStabbedSegments(p));
            // if no segments on stabbing line subgraph must be outside all others.
            if (stabbedSegments.Count == 0)
                return 0;
            stabbedSegments.Sort();
            var ds = stabbedSegments[0];
            return ds.LeftDepth;
        }

        #endregion

        #region Classes

        /// <summary>
        /// A segment from a directed edge which has been assigned a depth value
        /// for its sides.
        /// </summary>
        private class DepthSegment : IComparable
        {
            #region Fields

            private readonly LineSegment _upwardSeg;

            #endregion

            #region Constructors

            /// <summary>
            ///
            /// </summary>
            /// <param name="seg"></param>
            /// <param name="depth"></param>
            public DepthSegment(ILineSegmentBase seg, int depth)
            {
                // input seg is assumed to be normalized
                _upwardSeg = new LineSegment(seg);
                this.LeftDepth = depth;
            }

            #endregion

            #region Properties

            /// <summary>
            ///
            /// </summary>
            public int LeftDepth { get; set; }

            #endregion

            #region Methods

            /// <summary>
            /// Defines a comparision operation on DepthSegments
            /// which orders them left to right:
            /// DS1 smaller DS2   if   DS1.seg is left of DS2.seg.
            /// DS1 bigger  DS2   if   DS1.seg is right of DS2.seg.
            /// </summary>
            /// <param name="obj"></param>
            /// <returns>The comparison value</returns>
            public int CompareTo(Object obj)
            {
                var other = (DepthSegment)obj;

                /*
                * try and compute a determinate orientation for the segments.
                * Test returns 1 if other is left of this (i.e. this > other)
                */
                int orientIndex = _upwardSeg.OrientationIndex(other._upwardSeg);

                /*
                * If comparison between this and other is indeterminate,
                * try the opposite call order.
                * orientationIndex value is 1 if this is left of other,
                * so have to flip sign to get proper comparison value of
                * -1 if this is leftmost
                */
                if (orientIndex == 0)
                    orientIndex = -1 * other._upwardSeg.OrientationIndex(_upwardSeg);

                // if orientation is determinate, return it
                if (orientIndex != 0)
                    return orientIndex;

                // otherwise, segs must be collinear - sort based on minimum X value
                return CompareX(_upwardSeg, other._upwardSeg);
            }

            /// <summary>
            /// Compare two collinear segments for left-most ordering.
            /// If segs are vertical, use vertical ordering for comparison.
            /// If segs are equal, return 0.
            /// Segments are assumed to be directed so that the second coordinate is >= to the first
            /// (e.g. up and to the right).
            /// </summary>
            /// <param name="seg0">The left hand side segment to compare.</param>
            /// <param name="seg1">The riht hand side segment to compare.</param>
            /// <returns>An integer, -1 if seg0 is less, 0 if they are the same, and 1 if seg0 is greater.</returns>
            private static int CompareX(ILineSegmentBase seg0, ILineSegmentBase seg1)
            {
                int compare0 = seg0.P0.CompareTo(seg1.P0);
                return compare0 != 0 ? compare0 : seg0.P1.CompareTo(seg1.P1);
            }

            #endregion
        }

        #endregion
    }
}