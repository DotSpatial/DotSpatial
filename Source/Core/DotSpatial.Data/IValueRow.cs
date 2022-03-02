// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// A public interface definition for a single row of values that should be supported
    /// by any of the generic data row types.
    /// </summary>
    public interface IValueRow
    {
        /// <summary>
        /// Gets or sets the value in the position of column.
        /// </summary>
        /// <param name="cell">The 0 based integer column index to access on this row.</param>
        /// <returns>An object reference to the actual data value, which can be many types.</returns>
        double this[int cell] { get; set; }
    }
}