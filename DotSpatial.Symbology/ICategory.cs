// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/11/2009 10:48:40 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ICategory
    /// </summary>
    public interface ICategory : ILegendItem
    {
        #region Methods

        /// <summary>
        /// Applies the minimum and maximum in order to create the filter expression.  This will also
        /// count the members that match the specified criteria.
        /// </summary>
        void ApplyMinMax(EditorSettings settings);

        /// <summary>
        /// Applies the snapping rule directly to the categories, instead of the breaks.
        /// </summary>
        void ApplySnapping(IntervalSnapMethod method, int numDigits, List<double> values);

        /// <summary>
        /// For quantitative categories, this simply tests to see if the specified value can be
        /// found between the minimum and maximum range values.
        /// </summary>
        /// <param name="value">The double value to check against the minimum and maximum values.</param>
        /// <returns>Boolean, true if the value is inside the range.</returns>
        bool Contains(double value);

        /// <summary>
        ///  Returns this Number as a string.
        /// </summary>
        /// <param name="method">Specifies how the numbers are modified so that the numeric text can be cleaned up.</param>
        /// <param name="digits">An integer clarifying digits for rounding or significant figure situations.</param>
        /// <returns>A string with the formatted number.</returns>
        string ToString(IntervalSnapMethod method, int digits);

        #endregion

        #region Properties

        /// <summary>
        /// Minimum this is a convenient caching tool only, and doesn't control the filter expression at all.
        /// Use ApplyMinMax after setting this to update the filter expression.
        /// </summary>
        double? Minimum
        {
            get;
            set;
        }

        /// <summary>
        /// Maximum this is a convenient caching tool only, and doesn't control the filter expression at all.
        /// Use ApplyMinMax after setting this to update the filter expression.
        /// </summary>
        double? Maximum
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the value range, which overrides any existing min/max setup.
        /// This is only valid for numeric values that can be expressed as doubles.
        /// </summary>
        Range Range
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a status message for this string.
        /// </summary>
        string Status
        {
            get;
            set;
        }

        /// <summary>
        /// This is not used by DotSpatial, but is provided for convenient linking for this object
        /// in plugins or other applications.
        /// </summary>
        object Tag
        {
            get;
            set;
        }

        #endregion
    }
}