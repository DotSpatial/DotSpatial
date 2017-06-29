// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/3/2008 5:15:54 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    // We don't support hex or round-trip
    /// <summary>
    /// NumberFormats
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