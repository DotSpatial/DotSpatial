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

namespace DotSpatial.Topology.Simplify
{
    /// <summary>
    /// Simplifies a line (sequence of points) using
    /// the standard Douglas-Peucker algorithm.
    /// </summary>
    public class DouglasPeuckerLineSimplifier
    {
        private readonly IList<Coordinate> _pts;
        private readonly LineSegment _seg = new LineSegment();
        private double _distanceTolerance;
        private bool[] _usePt;

        /// <summary>
        ///
        /// </summary>
        /// <param name="pts"></param>
        private DouglasPeuckerLineSimplifier(IList<Coordinate> pts)
        {
            _pts = pts;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pts"></param>
        /// <param name="distanceTolerance"></param>
        /// <returns></returns>
        public static IList<Coordinate> Simplify(IList<Coordinate> pts, double distanceTolerance)
        {
            DouglasPeuckerLineSimplifier simp = new DouglasPeuckerLineSimplifier(pts);
            simp._distanceTolerance = distanceTolerance;
            return simp.Simplify();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private Coordinate[] Simplify()
        {
            _usePt = new bool[_pts.Count];
            for (int i = 0; i < _pts.Count; i++)
                _usePt[i] = true;

            SimplifySection(0, _pts.Count - 1);
            CoordinateList coordList = new CoordinateList();
            for (int i = 0; i < _pts.Count; i++)
                if (_usePt[i])
                    coordList.Add(new Coordinate(_pts[i]));
            return coordList.ToCoordinateArray();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private void SimplifySection(int i, int j)
        {
            if ((i + 1) == j)
                return;
            _seg.P0 = _pts[i];
            _seg.P1 = _pts[j];
            double maxDistance = -1.0;
            int maxIndex = i;
            for (int k = i + 1; k < j; k++)
            {
                double distance = _seg.Distance(_pts[k]);
                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    maxIndex = k;
                }
            }
            if (maxDistance <= _distanceTolerance)
                for (int k = i + 1; k < j; k++)
                    _usePt[k] = false;
            else
            {
                SimplifySection(i, maxIndex);
                SimplifySection(maxIndex, j);
            }
        }
    }
}