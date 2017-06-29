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
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System;

namespace DotSpatial.Projections.Transforms
{
    /// <summary>
    /// Extensions can be used after the namespace is included as follows. _w = projInfo.W.ToRadians();
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Obtains the double valued parameter after converting from degrees to radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The double valued parameter in radians.
        /// </returns>
        public static double FromDegreesToRadians(this double? value)
        {
            if (value.HasValue)
                return FromDegreesToRadians(value.Value);
            else
                throw new ArgumentNullException();
        }

        /// <summary>
        /// Obtains the double valued parameter after converting from degrees to radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The double valued parameter in radians.
        /// </returns>
        public static double FromDegreesToRadians(this double value)
        {
            return value * Math.PI / 180;
        }
    }
}