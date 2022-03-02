// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// The NumberFormat enumeration contains the various formatting options for numbers.
    /// </summary>
    public enum NumberFormat
    {
        /// <summary>
        /// Currency - C
        /// </summary>
        Currency,

        /// <summary>
        /// Scientific Notation Exponential - E
        /// </summary>
        Exponential,

        /// <summary>
        /// Fixed point - F
        /// The number is converted to a string of the form "-ddd.ddd…" where each 'd'
        /// indicates a digit (0-9). The string starts with a minus sign if the number
        /// is negative.
        /// </summary>
        FixedPoint,

        /// <summary>
        /// Shortest text - G
        /// </summary>
        General,

        /// <summary>
        /// Number - N, The number is converted to a string of the form "-d, ddd, ddd.ddd…",
        /// where '-' indicates a negative number symbol if required, 'd' indicates a digit
        /// (0-9), ',' indicates a thousand separator between number groups, and '.' indicates
        /// a decimal point symbol
        /// </summary>
        Number,

        /// <summary>
        /// Percent, value is multiplied by 100 and shown with a % symbol (cultural specific)
        /// </summary>
        Percent,

        /// <summary>
        /// No format specified.
        /// </summary>
        Unspecified,
    }
}