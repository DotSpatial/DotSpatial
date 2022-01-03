// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data.MiscUtil
{
    /// <summary>
    /// INullOp.
    /// </summary>
    /// <typeparam name="T">Type of the operator.</typeparam>
    // Required Attribution under Creative Commons Attribution-Noncommercial-Share Alike 3.0 United States License;
    // http://www.codeproject.com/KB/cs/GenericArithmeticUtil.aspx
    internal interface INullOp<T>
    {
        #region Methods

        /// <summary>
        /// Increments the accumulator only if the value is non-null. If the accumulator is null,
        /// then the accumulator is given the new value; otherwise the accumulator and value are added.
        /// </summary>
        /// <param name="accumulator">The current total to be incremented (can be null).</param>
        /// <param name="value">The value to be tested and added to the accumulator.</param>
        /// <returns>True if the value is non-null, else false - i.e. "has the accumulator been updated?".</returns>
        bool AddIfNotNull(ref T accumulator, T value);

        /// <summary>
        /// Indicates if the supplied value is non-null, for reference-types or Nullable&lt;T&gt;.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>True for non-null values, else false.</returns>
        bool HasValue(T value);

        #endregion
    }
}