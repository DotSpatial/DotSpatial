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
using DotSpatial.Topology.Algorithm;

namespace DotSpatial.Topology.Noding.Snapround
{
    /// <summary>
    /// Uses Snap Rounding to compute a rounded,
    /// fully noded arrangement from a set of <see cref="SegmentString" />s.
    /// Implements the Snap Rounding technique described in Hobby, Guibas and Marimont, and Goodrich et al.
    /// Snap Rounding assumes that all vertices lie on a uniform grid
    /// (hence the precision model of the input must be fixed precision,
    /// and all the input vertices must be rounded to that precision).
    /// <para>
    /// This implementation uses simple iteration over the line segments.
    /// This implementation appears to be fully robust using an integer precision model.
    /// It will function with non-integer precision models, but the
    /// results are not 100% guaranteed to be correctly noded.
    /// </para>
    /// </summary>
    public class SimpleSnapRounder : INoder
    {
        private readonly LineIntersector _li;
        private readonly double _scaleFactor;
        private IList _nodedSegStrings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleSnapRounder"/> class.
        /// </summary>
        /// <param name="pm">The <see cref="PrecisionModel" /> to use.</param>
        public SimpleSnapRounder(PrecisionModel pm)
        {
            _li = new RobustLineIntersector();
            _li.PrecisionModel = pm;
            _scaleFactor = pm.Scale;
        }

        #region INoder Members

        /// <summary>
        /// Returns a <see cref="IList"/> of fully noded <see cref="SegmentString"/>s.
        /// The <see cref="SegmentString"/>s have the same context as their parent.
        /// </summary>
        /// <returns></returns>
        public IList GetNodedSubstrings()
        {
            return SegmentString.GetNodedSubstrings(_nodedSegStrings);
        }

        /// <summary>
        /// Computes the noding for a collection of <see cref="SegmentString" />s.
        /// Some Noders may add all these nodes to the input <see cref="SegmentString" />s;
        /// others may only add some or none at all.
        /// </summary>
        /// <param name="inputSegmentStrings"></param>
        public void ComputeNodes(IList inputSegmentStrings)
        {
            _nodedSegStrings = inputSegmentStrings;
            SnapRound(inputSegmentStrings, _li);
        }

        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <param name="segStrings"></param>
        /// <param name="li"></param>
        private void SnapRound(IList segStrings, LineIntersector li)
        {
            IList intersections = FindInteriorIntersections(segStrings, li);
            ComputeSnaps(segStrings, intersections);
            ComputeVertexSnaps(segStrings);
        }

        /// <summary>
        /// Computes all interior intersections in the collection of <see cref="SegmentString" />s,
        /// and returns their <see cref="Coordinate" />s.
        /// Does NOT node the segStrings.
        /// </summary>
        /// <param name="segStrings"></param>
        /// <param name="li"></param>
        /// <returns>A list of <see cref="Coordinate" />s for the intersections.</returns>
        private static IList FindInteriorIntersections(IList segStrings, LineIntersector li)
        {
            IntersectionFinderAdder intFinderAdder = new IntersectionFinderAdder(li);
            SinglePassNoder noder = new McIndexNoder(intFinderAdder);
            noder.ComputeNodes(segStrings);
            return intFinderAdder.InteriorIntersections;
        }

        /// <summary>
        /// Computes nodes introduced as a result of snapping segments to snap points (hot pixels).
        /// </summary>
        /// <param name="segStrings"></param>
        /// <param name="snapPts"></param>
        private void ComputeSnaps(IList segStrings, IList snapPts)
        {
            foreach (object obj in segStrings)
            {
                SegmentString ss = (SegmentString)obj;
                ComputeSnaps(ss, snapPts);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="snapPts"></param>
        private void ComputeSnaps(SegmentString ss, IList snapPts)
        {
            foreach (object objo in snapPts)
            {
                Coordinate snapPt = (Coordinate)objo;
                HotPixel hotPixel = new HotPixel(snapPt, _scaleFactor, _li);
                for (int i = 0; i < ss.Count - 1; i++)
                    AddSnappedNode(hotPixel, ss, i);
            }
        }

        /// <summary>
        /// Computes nodes introduced as a result of
        /// snapping segments to vertices of other segments.
        /// </summary>
        /// <param name="edges"></param>
        public void ComputeVertexSnaps(IList edges)
        {
            foreach (object obj0 in edges)
            {
                SegmentString edge0 = (SegmentString)obj0;
                foreach (object obj1 in edges)
                {
                    SegmentString edge1 = (SegmentString)obj1;
                    ComputeVertexSnaps(edge0, edge1);
                }
            }
        }

        /// <summary>
        /// Performs a brute-force comparison of every segment in each <see cref="SegmentString" />.
        /// This has n^2 performance.
        /// </summary>
        /// <param name="e0"></param>
        /// <param name="e1"></param>
        private void ComputeVertexSnaps(SegmentString e0, SegmentString e1)
        {
            IList<Coordinate> pts0 = e0.Coordinates;
            IList<Coordinate> pts1 = e1.Coordinates;
            for (int i0 = 0; i0 < pts0.Count - 1; i0++)
            {
                HotPixel hotPixel = new HotPixel(pts0[i0], _scaleFactor, _li);
                for (int i1 = 0; i1 < pts1.Count - 1; i1++)
                {
                    // don't snap a vertex to itself
                    if (e0 == e1)
                        if (i0 == i1)
                            continue;

                    bool isNodeAdded = AddSnappedNode(hotPixel, e1, i1);
                    // if a node is created for a vertex, that vertex must be noded too
                    if (isNodeAdded)
                        e0.AddIntersection(pts0[i0], i0);
                }
            }
        }

        /// <summary>
        /// Adds a new node (equal to the snap pt) to the segment
        /// if the segment passes through the hot pixel.
        /// </summary>
        /// <param name="hotPix"></param>
        /// <param name="segStr"></param>
        /// <param name="segIndex"></param>
        /// <returns></returns>
        public static bool AddSnappedNode(HotPixel hotPix, SegmentString segStr, int segIndex)
        {
            Coordinate p0 = segStr.GetCoordinate(segIndex);
            Coordinate p1 = segStr.GetCoordinate(segIndex + 1);

            if (hotPix.Intersects(p0, p1))
            {
                segStr.AddIntersection(hotPix.Coordinate, segIndex);
                return true;
            }
            return false;
        }
    }
}