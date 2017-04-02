// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/11/2009 10:24:02 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    public class Category : LegendItem
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of Category.
        /// </summary>
        public Category()
        {
        }

        /// <summary>
        /// Creates a new instance of this category, and tailors the range to the specifeid values.
        /// </summary>
        /// <param name="startValue">The start value</param>
        /// <param name="endValue">The end value</param>
        public Category(double? startValue, double? endValue)
        {
            Range = new Range(startValue, endValue);
        }

        /// <summary>
        /// Creates a category that has the same value for both minimum and maximum.
        /// </summary>
        /// <param name="value">The value to use</param>
        public Category(double value)
        {
            Range = new Range(value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Applies the snapping rule directly to the categories, instead of the breaks.
        /// </summary>
        public void ApplySnapping(IntervalSnapMethod method, int numDigits, List<double> values)
        {
            switch (method)
            {
                case IntervalSnapMethod.None:
                    break;
                case IntervalSnapMethod.SignificantFigures:
                    if (Maximum != null)
                    {
                        Maximum = Utils.SigFig(Maximum.Value, numDigits);
                    }
                    if (Minimum != null)
                    {
                        Minimum = Utils.SigFig(Minimum.Value, numDigits);
                    }
                    break;
                case IntervalSnapMethod.Rounding:
                    if (Maximum != null)
                    {
                        Maximum = Math.Round((double)Maximum, numDigits);
                    }
                    if (Minimum != null)
                    {
                        Minimum = Math.Round((double)Minimum, numDigits);
                    }
                    break;
                case IntervalSnapMethod.DataValue:
                    if (Maximum != null)
                    {
                        Maximum = Utils.GetNearestValue((double)Maximum, values);
                    }
                    if (Minimum != null)
                    {
                        Minimum = Utils.GetNearestValue((double)Minimum, values);
                    }
                    break;
            }
        }

        /// <summary>
        /// Since rasters are numeric and not relying on an SQL expression, this allows
        /// this only sets the legend text using the method and digits to help with
        /// formatting.
        /// </summary>
        /// <param name="settings">An EditorSettings from either a feature scheme or color scheme.</param>
        public virtual void ApplyMinMax(EditorSettings settings)
        {
            LegendText = Range.ToString(settings.IntervalSnapMethod, settings.IntervalRoundingDigits);
        }

        /// <summary>
        /// Tests to see if the specified value falls in the range specified by this ColorCategory
        /// </summary>
        /// <param name="value">The value of type int to test</param>
        /// <returns>Boolean, true if the value was found in the range</returns>
        public bool Contains(double value)
        {
            return Range == null || Range.Contains(value);
        }

        /// <summary>
        /// Returns this Number as a string.  This uses the DotSpatial.Globalization.CulturePreferences and
        /// Controls the number type using the NumberFormat enumeration plus the DecimalCount to create
        /// a number format.
        /// </summary>
        /// <returns>The string created using the specified number format and precision.</returns>
        public override string ToString()
        {
            return Range.ToString();
        }

        /// <summary>
        ///  Returns this Number as a string.
        /// </summary>
        /// <param name="method">Specifies how the numbers are modified so that the numeric text can be cleaned up.</param>
        /// <param name="digits">An integer clarifying digits for rounding or significant figure situations.</param>
        /// <returns>A string with the formatted number.</returns>
        public virtual string ToString(IntervalSnapMethod method, int digits)
        {
            return Range.ToString(method, digits);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Maximum this is a convenient caching tool only, and doesn't control the filter expression at all.
        /// Use ApplyMinMax after setting this to update the filter expression.
        /// </summary>
        [Description("Gets or sets the maximum value for this category using the scheme field.")]
        public double? Maximum
        {
            get
            {
                return Range?.Maximum;
            }
            set
            {
                if (Range == null)
                {
                    Range = new Range(null, value);
                    return;
                }
                Range.Maximum = value;
            }
        }

        /// <summary>
        /// Gets or sets the color to be used for this break.  For
        /// BiValued breaks, this only sets one of the colors.  If
        /// this is higher than the high value, both are set to this.
        /// If this equals the high value, IsBiValue will be false.
        /// </summary>
        [Description("Gets or sets a minimum value for this category using the scheme field.")]
        public double? Minimum
        {
            get
            {
                return Range?.Minimum;
            }
            set
            {
                if (Range == null)
                {
                    Range = new Range(value, null);
                    return;
                }
                Range.Minimum = value;
            }
        }

        /// <summary>
        /// Gets the numeric Range for this color break.
        /// </summary>
        [Serialize("Range")]
        public Range Range { get; set; }

        /// <summary>
        /// Gets or sets a status message for this string.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// This is not used by DotSpatial, but is provided for convenient linking for this object
        /// in plugins or other applications.
        /// </summary>
        public object Tag { get; set; }

        #endregion
    }
}