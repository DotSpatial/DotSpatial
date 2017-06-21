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

namespace DotSpatial.Topology.Noding
{
    /// <summary>
    /// Implements a robust method of comparing the relative position of two points along the same segment.
    /// The coordinates are assumed to lie "near" the segment.
    /// This means that this algorithm will only return correct results
    /// if the input coordinates have the same precision and correspond to rounded values
    /// of exact coordinates lying on the segment.
    /// </summary>
    public class SegmentPointComparator
    {
        /// <summary>
        ///  Compares two <see cref="Coordinate" />s for their relative position along a segment
        /// lying in the specified <see cref="Octant" />.
        /// </summary>
        /// <param name="octant"></param>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <returns>
        /// -1 if node0 occurs first, or
        ///  0 if the two nodes are equal, or
        ///  1 if node1 occurs first.
        /// </returns>
        public static int Compare(OctantDirection octant, Coordinate p0, Coordinate p1)
        {
            // nodes can only be equal if their coordinates are equal
            if (p0.Equals2D(p1))
                return 0;

            int xSign = RelativeSign(p0.X, p1.X);
            int ySign = RelativeSign(p0.Y, p1.Y);

            switch (octant)
            {
                case OctantDirection.Zero:
                    return CompareValue(xSign, ySign);
                case OctantDirection.One:
                    return CompareValue(ySign, xSign);
                case OctantDirection.Two:
                    return CompareValue(ySign, -xSign);
                case OctantDirection.Three:
                    return CompareValue(-xSign, ySign);
                case OctantDirection.Four:
                    return CompareValue(-xSign, -ySign);
                case OctantDirection.Five:
                    return CompareValue(-ySign, -xSign);
                case OctantDirection.Six:
                    return CompareValue(-ySign, xSign);
                case OctantDirection.Seven:
                    return CompareValue(xSign, -ySign);
            }
            throw new InvalidOctantException(octant.ToString());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="x1"></param>
        /// <returns></returns>
        public static int RelativeSign(double x0, double x1)
        {
            if (x0 < x1)
                return -1;
            if (x0 > x1)
                return 1;
            return 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="compareSign0"></param>
        /// <param name="compareSign1"></param>
        /// <returns></returns>
        private static int CompareValue(int compareSign0, int compareSign1)
        {
            if (compareSign0 < 0)
                return -1;
            if (compareSign0 > 0)
                return 1;
            if (compareSign1 < 0)
                return -1;
            if (compareSign1 > 0)
                return 1;
            return 0;
        }
    }
}