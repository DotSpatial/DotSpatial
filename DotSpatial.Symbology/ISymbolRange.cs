// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Characterizes
    /// </summary>
    [Obsolete("Do not use it. This interface is not used in DotSpatial anymore.")] // Marked in 1.7
    public interface ISymbolRange
    {
        /// <summary>
        /// The minimum value, inclusive, in a range of values.  If
        /// the value being tested is less than this value,
        /// then these properties should not be applied.
        /// </summary>
        object Minimum
        {
            get;
            set;
        }

        /// <summary>
        /// Describes the maximum value, inclusive, in a range of values.  If
        /// the value being tested is greater than this value, then these
        /// properties should not be applied.
        /// </summary>
        object Maximum
        {
            get;
            set;
        }

        /// <summary>
        /// The classification for this symbol.  This is the same as
        /// specifying the MinValue and MaxValue to be equal and testing
        /// the IsInRange property.
        /// </summary>
        object Classification
        {
            get;
            set;
        }

        /// <summary>
        /// Boolean Tests a value against the minimum and maximum values.
        /// If the value is greater than or equal to the MinValue and
        /// is less than or equal to the MaxValue then it will be included.
        /// </summary>
        /// <param name="value">The IComparable value to test.  This is usually a numeric value.</param>
        /// <returns>True if value is between MinValue and MaxValue</returns>
        bool IsInRange(IComparable value);

        /// <summary>
        /// The classification must implement IComparable.  The normal value types like
        /// strings, doubles and integers already do this.  If the CompareTo method
        /// on value returns 0 when compared to the Classification property of this
        /// instance, this method will return true.  Otherwise, it returns false.
        /// </summary>
        /// <param name="value">Any valid implementation of IComparable.  This can be either
        /// the default value types like integer or string, or else it can be a custom
        /// implementation of IComparable.</param>
        /// <returns>Boolean, true if value.CompareTo returns 0 when compared against this classification.</returns>
        bool MatchesClassification(IComparable value);
    }
}