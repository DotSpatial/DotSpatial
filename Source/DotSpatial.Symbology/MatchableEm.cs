// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Extension methods for IMatchable.
    /// </summary>
    public static class MatchableEm
    {
        /// <summary>
        /// This tests the public properties from the two objects. If any properties implement
        /// the IMatchable interface, and do not match, this returns false. If any public
        /// properties are value types, and they are not equal, then this returns false.
        /// </summary>
        /// <param name="self">This matchable item. </param>
        /// <param name="other">The other item to compare to this item.</param>
        /// <returns>Boolean, true if there is a match.</returns>
        public static bool Matches(this IMatchable self, IMatchable other)
        {
            List<string> ignoreMe;
            return self.Matches(other, out ignoreMe);
        }
    }
}