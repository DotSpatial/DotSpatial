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

namespace DotSpatial.Topology.Noding
{
    /// <summary>
    /// Computes the possible intersections between two line segments in <see cref="ISegmentString" />s
    /// and adds them to each string
    /// using <see cref="NodedSegmentString.AddIntersection(LineIntersector,int,int,int)"/>.
    /// </summary>
    public class IntersectionAdder : ISegmentIntersector
    {
        #region Fields

        /// <summary>
        ///
        /// </summary>
        public int NumInteriorIntersections = 0;

        /// <summary>
        /// 
        /// </summary>
        public int NumIntersections = 0;

        /// <summary>
        /// 
        /// </summary>
        public int NumProperIntersections = 0;

        /// <summary>
        /// 
        /// </summary>
        public int NumTests = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IntersectionAdder"/> class.
        /// </summary>
        /// <param name="li"></param>
        public IntersectionAdder(LineIntersector li)
        {
            ProperIntersectionPoint = null;
            LineIntersector = li;
        }

        #endregion

        #region Properties

        /// <summary>
        /// An interior intersection is an intersection which is
        /// in the interior of some segment.
        /// </summary>
        public bool HasInteriorIntersection { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public bool HasIntersection { get; private set; }

        /// <summary>
        /// A proper interior intersection is a proper intersection which is not
        /// contained in the set of boundary nodes set for this <see cref="ISegmentIntersector" />.
        /// </summary>
        public bool HasProperInteriorIntersection { get; private set; }

        /// <summary>
        /// A proper intersection is an intersection which is interior to at least two
        /// line segments.  Note that a proper intersection is not necessarily
        /// in the interior of the entire <see cref="Geometry" />, since another edge may have
        /// an endpoint equal to the intersection, which according to SFS semantics
        /// can result in the point being on the Boundary of the <see cref="Geometry" />.
        /// </summary>
        public bool HasProperIntersection { get; private set; }

        ///<summary>
        /// Always process all intersections
        ///</summary>
        public bool IsDone { get { return false; } }

        /// <summary>
        /// 
        /// </summary>
        public LineIntersector LineIntersector { get; private set; }

        /// <summary>
        /// Returns the proper intersection point, or <c>null</c> if none was found.
        /// </summary>
        public Coordinate ProperIntersectionPoint { get; private set; }

        #endregion

        #region Methods

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
        /// A trivial intersection is an apparent self-intersection which in fact
        /// is simply the point shared by adjacent line segments.
        /// Note that closed edges require a special check for the point shared by the beginning and end segments.
        /// </summary>
        /// <param name="e0"></param>
        /// <param name="segIndex0"></param>
        /// <param name="e1"></param>
        /// <param name="segIndex1"></param>
        /// <returns></returns>
        private bool IsTrivialIntersection(ISegmentString e0, int segIndex0, ISegmentString e1, int segIndex1)
        {
            if (e0 != e1 || LineIntersector.IntersectionNum != 1) return false;
            if (IsAdjacentSegments(segIndex0, segIndex1)) return true;
            if (!e0.IsClosed) return false;
            int maxSegIndex = e0.Count - 1;
            return (segIndex0 == 0 && segIndex1 == maxSegIndex) || (segIndex1 == 0 && segIndex0 == maxSegIndex);
        }

        /// <summary>
        /// This method is called by clients
        /// of the <see cref="ISegmentIntersector" /> class to process
        /// intersections for two segments of the <see cref="ISegmentString" /> being intersected.<br/>
        /// Note that some clients (such as <c>MonotoneChain</c>") may optimize away
        /// this call for segment pairs which they have determined do not intersect
        /// (e.g. by an disjoint envelope test).
        /// </summary>
        /// <param name="e0"></param>
        /// <param name="segIndex0"></param>
        /// <param name="e1"></param>
        /// <param name="segIndex1"></param>
        public void ProcessIntersections(ISegmentString e0, int segIndex0, ISegmentString e1, int segIndex1)
        {
            if (e0 == e1 && segIndex0 == segIndex1) return;

            NumTests++;
            IList<Coordinate> coordinates0 = e0.Coordinates;
            Coordinate p00 = coordinates0[segIndex0];
            Coordinate p01 = coordinates0[segIndex0 + 1];
            IList<Coordinate> coordinates1 = e1.Coordinates;
            Coordinate p10 = coordinates1[segIndex1];
            Coordinate p11 = coordinates1[segIndex1 + 1];

            LineIntersector.ComputeIntersection(p00, p01, p10, p11);
            if (!LineIntersector.HasIntersection) return;
            NumIntersections++;
            if (LineIntersector.IsInteriorIntersection())
            {
                NumInteriorIntersections++;
                HasInteriorIntersection = true;
            }
            // if the segments are adjacent they have at least one trivial intersection,
            // the shared endpoint.  Don't bother adding it if it is the
            // only intersection.
            if (IsTrivialIntersection(e0, segIndex0, e1, segIndex1)) return;
            HasIntersection = true;
            ((NodedSegmentString)e0).AddIntersections(LineIntersector, segIndex0, 0);
            ((NodedSegmentString)e1).AddIntersections(LineIntersector, segIndex1, 1);

            if (!LineIntersector.IsProper) return;
            NumProperIntersections++;
            HasProperIntersection = true;
            HasProperInteriorIntersection = true;
        }

        #endregion
    }
}