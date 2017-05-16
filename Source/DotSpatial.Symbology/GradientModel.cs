// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// An enumeration specifying the way that a gradient of color is attributed to the values in the specified range.
    /// </summary>
    public enum GradientModel
    {
        /// <summary>
        /// The values are colored in even steps in each of the Red, Green and Blue bands.
        /// </summary>
        Linear,

        /// <summary>
        /// The even steps between values are used as powers of two, greatly increasing the impact of higher values.
        /// </summary>
        Exponential,

        /// <summary>
        /// The log of the values is used, reducing the relative impact of the higher values in the range.
        /// </summary>
        Logarithmic
    }
}