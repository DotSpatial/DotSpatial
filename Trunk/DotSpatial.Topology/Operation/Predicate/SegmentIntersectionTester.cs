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

namespace DotSpatial.Topology.Operation.Predicate
{
    /// <summary>
    /// Tests if any line segments in two sets of <see cref="CoordinateSequences"/> intersect.
    /// Optimized for use when at least one input is of small size.
    /// Short-circuited to return as soon an intersection is found.
    /// </summary>
    public class SegmentIntersectionTester
    {
        #region Fields

        // for purposes of intersection testing, don't need to set precision model
        private readonly LineIntersector li = new RobustLineIntersector();
        private readonly Coordinate pt00 = new Coordinate();
        private readonly Coordinate pt01 = new Coordinate();
        private readonly Coordinate pt10 = new Coordinate();
        private readonly Coordinate pt11 = new Coordinate();
        private bool _hasIntersection;

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="seq0"></param>
        /// <param name="seq1"></param>
        /// <returns></returns>
        public bool HasIntersection(ICoordinateSequence seq0, ICoordinateSequence seq1) 
        {
            for (int i = 1; i < seq0.Count && ! _hasIntersection; i++) 
            {
                seq0.GetCoordinate(i - 1, pt00);
                seq0.GetCoordinate(i, pt01);
                for (int j = 1; j < seq1.Count && ! _hasIntersection; j++) 
                {
                    seq1.GetCoordinate(j - 1, pt10);
                    seq1.GetCoordinate(j, pt11);
                    li.ComputeIntersection(pt00, pt01, pt10, pt11);
                    if (li.HasIntersection)
                        _hasIntersection = true;
                }
            }
            return _hasIntersection;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public bool HasIntersectionWithLineStrings(ICoordinateSequence seq, ICollection<IGeometry> lines)
        {
            foreach (ILineString line in lines)
            {
                HasIntersection(seq, line.CoordinateSequence);
                if (_hasIntersection)
                    break;
            }
            return _hasIntersection;
        }

        #endregion
    }
}