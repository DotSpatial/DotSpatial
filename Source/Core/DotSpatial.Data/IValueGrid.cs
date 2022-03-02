// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// An interface specification for any of the multiple types of IDataBlock.
    /// </summary>
    public interface IValueGrid
    {
        /// <summary>
        /// Gets or sets a value indicating whether the values have been changed since the last time this flag was set to false.
        /// </summary>
        bool Updated { get; set; }

        /// <summary>
        /// Gets or sets a value at the 0 row, 0 column index.
        /// </summary>
        /// <param name="row">The 0 based vertical row index from the top.</param>
        /// <param name="column">The 0 based horizontal column index from the left.</param>
        /// <returns>An object reference to the actual value in the data member.</returns>
        double this[int row, int column] { get; set; }
    }
}