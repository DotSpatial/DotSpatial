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

namespace DotSpatial.Topology.Operation.Predicate
{
    /// <summary>
    /// Tests if any line segments in two sets of CoordinateSequences intersect.
    /// Optimized for small geometry size.
    /// Short-circuited to return as soon an intersection is found.
    /// </summary>
    public class SegmentIntersectionTester
    {
        // for purposes of intersection testing, don't need to set precision model
        private readonly LineIntersector _li = new RobustLineIntersector();

        private bool _hasIntersection;
        private Coordinate _pt00;
        private Coordinate _pt01;
        private Coordinate _pt10;
        private Coordinate _pt11;

        /// <summary>
        ///
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public bool HasIntersectionWithLineStrings(IList<Coordinate> seq, IList lines)
        {
            for (IEnumerator i = lines.GetEnumerator(); i.MoveNext(); )
            {
                LineString line = (LineString)i.Current;
                HasIntersection(seq, line.Coordinates);
                if (_hasIntersection)
                    break;
            }
            return _hasIntersection;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="seq0"></param>
        /// <param name="seq1"></param>
        private void HasIntersection(IList<Coordinate> seq0, IList<Coordinate> seq1)
        {
            for (int i = 1; i < seq0.Count && !_hasIntersection; i++)
            {
                _pt00 = seq0[i - 1];
                _pt01 = seq0[i];
                for (int j = 1; j < seq1.Count && !_hasIntersection; j++)
                {
                    _pt10 = seq1[j - 1];
                    _pt11 = seq1[j];
                    _li.ComputeIntersection(_pt00, _pt01, _pt10, _pt11);
                    if (_li.HasIntersection)
                        _hasIntersection = true;
                }
            }
            return;
        }
    }
}