// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Matching is defined as being a different object, but having properties
    /// that would make it indistinguishable from the comparision property.
    /// This is an alternative to overriding the equals behavior in cases
    /// where you might ALSO need to see if the object reference is the same.
    /// </summary>
    public interface IMatchable
    {
        /// <summary>
        /// Tests this object against the comparison object. If any of the
        /// value type members are different, or if any of the properties
        /// are IMatchable and do not match, then this returns false.
        /// </summary>
        /// <param name="other">The other IMatcheable object of the same type</param>
        /// <param name="mismatchedProperties">The list of property names that do not match</param>
        /// <returns>Boolean, true if the properties are comparably equal.</returns>
        bool Matches(IMatchable other, out List<string> mismatchedProperties);
    }
}