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
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Noding.Snapround
{
    /// <summary>
    /// Uses Snap Rounding to compute a rounded,
    /// fully noded arrangement from a set of <see cref="ISegmentString" />s.
    /// Implements the Snap Rounding technique described in
    /// the papers by Hobby, Guibas & Marimont, and Goodrich et al.
    /// Snap Rounding assumes that all vertices lie on a uniform grid;
    /// hence the precision model of the input must be fixed precision,
    /// and all the input vertices must be rounded to that precision.
    /// <para>
    /// This implementation uses simple iteration over the line segments.
    /// This is not the most efficient approach for large sets of segments.
    /// This implementation appears to be fully robust using an integer precision model.
    /// It will function with non-integer precision models, but the
    /// results are not 100% guaranteed to be correctly noded.
    /// </para>
    /// </summary>
    public class SimpleSnapRounder : INoder
    {
        #region Fields

        private readonly LineIntersector _li;
        private readonly double _scaleFactor;
        private IList<ISegmentString> _nodedSegStrings;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleSnapRounder"/> class.
        /// </summary>
        /// <param name="pm">The <see cref="PrecisionModel" /> to use.</param>
        public SimpleSnapRounder(PrecisionModel pm)
        {
            _li = new RobustLineIntersector { PrecisionModel = pm };
            _scaleFactor = pm.Scale;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Computes the noding for a collection of <see cref="ISegmentString" />s.
        /// Some Noders may add all these nodes to the input <see cref="ISegmentString" />s;
        /// others may only add some or none at all.
        /// </summary>
        /// <param name="inputSegmentStrings">A collection of NodedSegmentStrings</param>
        public void ComputeNodes(IList<ISegmentString> inputSegmentStrings)
        {
            _nodedSegStrings = inputSegmentStrings;
            SnapRound(inputSegmentStrings, _li);
        }

        /// <summary>
        /// Computes nodes introduced as a result of snapping segments to snap points (hot pixels).
        /// </summary>
        /// <param name="segStrings"></param>
        /// <param name="snapPts"></param>
        private void ComputeSnaps(IEnumerable<ISegmentString> segStrings, ICollection<Coordinate> snapPts)
        {
            foreach (INodableSegmentString ss in segStrings)
                ComputeSnaps(ss, snapPts);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="snapPts"></param>
        private void ComputeSnaps(INodableSegmentString ss, IEnumerable<Coordinate> snapPts)
        {
            foreach (Coordinate snapPt in snapPts)
            {
                HotPixel hotPixel = new HotPixel(snapPt, _scaleFactor, _li);
                for (int i = 0; i < ss.Count - 1; i++)
                    hotPixel.AddSnappedNode(ss, i);
            }
        }

        /// <summary>
        /// Computes nodes introduced as a result of
        /// snapping segments to vertices of other segments.
        /// </summary>
        /// <param name="edges">The list of segment strings to snap together</param>
        public void ComputeVertexSnaps(IList<ISegmentString> edges)
        {
            foreach (INodableSegmentString edge0 in edges)
                foreach (INodableSegmentString edge1 in edges)
                    ComputeVertexSnaps(edge0, edge1);
        }

        /// <summary>
        /// Performs a brute-force comparison of every segment in each <see cref="ISegmentString" />.
        /// This has n^2 performance.
        /// </summary>
        /// <param name="e0"></param>
        /// <param name="e1"></param>
        private void ComputeVertexSnaps(INodableSegmentString e0, INodableSegmentString e1)
        {
            IList<Coordinate> pts0 = e0.Coordinates;
            IList<Coordinate> pts1 = e1.Coordinates;
            for (int i0 = 0; i0 < pts0.Count - 1; i0++)
            {
                HotPixel hotPixel = new HotPixel(pts0[i0], _scaleFactor, _li);
                for (int i1 = 0; i1 < pts1.Count - 1; i1++)
                {
                    // don't snap a vertex to itself
                    if (e0 == e1 && i0 == i1) continue;

                    // if a node is created for a vertex, that vertex must be noded too
                    if (hotPixel.AddSnappedNode(e1, i1))
                        e0.AddIntersection(pts0[i0], i0);
                }
            }
        }

        /// <summary>
        /// Computes all interior intersections in the collection of <see cref="ISegmentString" />s,
        /// and returns their <see cref="Coordinate" />s.
        /// Does NOT node the segStrings.
        /// </summary>
        /// <param name="segStrings"></param>
        /// <param name="li"></param>
        /// <returns>A list of <see cref="Coordinate" />s for the intersections.</returns>
        private static IList<Coordinate> FindInteriorIntersections(IList<ISegmentString> segStrings, LineIntersector li)
        {
            InteriorIntersectionFinderAdder intFinderAdder = new InteriorIntersectionFinderAdder(li);
            SinglePassNoder noder = new MCIndexNoder(intFinderAdder);
            noder.ComputeNodes(segStrings);
            return intFinderAdder.InteriorIntersections;
        }

        /// <summary>
        /// Returns a <see cref="IList"/> of fully noded <see cref="ISegmentString"/>s.
        /// The <see cref="ISegmentString"/>s have the same context as their parent.
        /// </summary>
        /// <returns>A Collection of NodedSegmentStrings representing the substrings</returns>
        public IList<ISegmentString> GetNodedSubstrings()
        {
            return NodedSegmentString.GetNodedSubstrings(_nodedSegStrings);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="segStrings"></param>
        /// <param name="li"></param>
        private void SnapRound(IList<ISegmentString> segStrings, LineIntersector li)
        {
            IList<Coordinate> intersections = FindInteriorIntersections(segStrings, li);
            ComputeSnaps(segStrings, intersections);
            ComputeVertexSnaps(segStrings);
        }

        #endregion
    }
}