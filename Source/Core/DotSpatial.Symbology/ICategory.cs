// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for Category.
    /// </summary>
    public interface ICategory : ILegendItem
    {
        #region Properties

        /// <summary>
        /// Gets or sets the maximum. This is a convenient caching tool only, and doesn't control the filter expression at all.
        /// Use ApplyMinMax after setting this to update the filter expression.
        /// </summary>
        double? Maximum { get; set; }

        /// <summary>
        /// Gets or sets the minimum. This is a convenient caching tool only, and doesn't control the filter expression at all.
        /// Use ApplyMinMax after setting this to update the filter expression.
        /// </summary>
        double? Minimum { get; set; }

        /// <summary>
        /// Gets or sets the value range, which overrides any existing min/max setup.
        /// This is only valid for numeric values that can be expressed as doubles.
        /// </summary>
        Range Range { get; set; }

        /// <summary>
        /// Gets or sets a status message for this string.
        /// </summary>
        string Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is actively supporting selection.
        /// </summary>
        bool SelectionEnabled { get; set; }

        /// <summary>
        /// Gets or sets the tag. This is not used by DotSpatial, but is provided for convenient linking for this object
        /// in plugins or other applications.
        /// </summary>
        object Tag { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Applies the minimum and maximum in order to create the filter expression. This will also
        /// count the members that match the specified criteria.
        /// </summary>
        /// <param name="settings">Settings to get the minimum and maximum from.</param>
        void ApplyMinMax(EditorSettings settings);

        /// <summary>
        /// Applies the snapping rule directly to the categories, instead of the breaks.
        /// </summary>
        /// <param name="method">Snapping method that should be applied.</param>
        /// <param name="numDigits">Number of significant digits.</param>
        /// <param name="values">Values to get the min and max from when using IntervalSnapMethod.DataValue as method.</param>
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
    }
}