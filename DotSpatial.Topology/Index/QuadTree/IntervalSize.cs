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

namespace DotSpatial.Topology.Index.Quadtree
{
    /// <summary>
    /// Provides a test for whether an interval is
    /// so small it should be considered as zero for the purposes of
    /// inserting it into a binary tree.
    /// The reason this check is necessary is that round-off error can
    /// cause the algorithm used to subdivide an interval to fail, by
    /// computing a midpoint value which does not lie strictly between the
    /// endpoints.
    /// </summary>
    public static class IntervalSize
    {
        /// <summary>
        /// This value is chosen to be a few powers of 2 less than the
        /// number of bits available in the double representation (i.e. 53).
        /// This should allow enough extra precision for simple computations to be correct,
        /// at least for comparison purposes.
        /// </summary>
        public const int MIN_BINARY_EXPONENT = -50;

        /// <summary>
        /// Computes whether the interval [min, max] is effectively zero width.
        /// I.e. the width of the interval is so much less than the
        /// location of the interval that the midpoint of the interval cannot be
        /// represented precisely.
        /// </summary>
        public static bool IsZeroWidth(double min, double max)
        {
            double width = max - min;
            if (width == 0.0)
                return true;
            double maxAbs = Math.Max(Math.Abs(min), Math.Abs(max));
            double scaledInterval = width / maxAbs;
            int level = DoubleBits.GetExponent(scaledInterval);
            return level <= MIN_BINARY_EXPONENT;
        }
    }
}