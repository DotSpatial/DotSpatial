// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Numeric range using doubles.
    /// </summary>
    public class Range
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Range"/> class with undefined interval.
        /// </summary>
        public Range()
        {
            Minimum = null;
            Maximum = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Range"/> class.
        /// </summary>
        /// <param name="value1">Either bound of the range.</param>
        /// <param name="value2">The other bound of the range.</param>
        public Range(double? value1, double? value2)
        {
            Minimum = value1;
            Maximum = value2;
            FixOrder();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Range"/> class where both the minimum and maximum are the
        /// same value and both are inclusive.
        /// </summary>
        /// <param name="value">Value for Minimum and Maximum.</param>
        public Range(double value)
        {
            Minimum = value;
            Maximum = value;
            MinIsInclusive = true;
            MaxIsInclusive = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Range"/> class.
        /// </summary>
        /// <param name="expression">A string expression that can be two separate numbers separated by a dash.</param>
        public Range(string expression)
        {
            string exp = expression ?? "-";
            if (exp.Contains(">=") || exp.Contains(">"))
            {
                MinIsInclusive = exp.Contains(">=");
                Maximum = null;
                double min;
                if (double.TryParse(exp.Replace(">=", string.Empty).Replace(">", string.Empty), out min))
                {
                    Minimum = min;
                }
            }
            else if (exp.Contains("<=") || exp.Contains("<"))
            {
                MaxIsInclusive = exp.Contains("<=");
                Minimum = null;
                double max;
                if (double.TryParse(exp.Replace("<=", string.Empty).Replace("<", string.Empty), out max))
                {
                    Maximum = max;
                }
            }
            else if (exp.Contains("-"))
            {
                // The default is to actually include the maximums, but not the minimums.
                MaxIsInclusive = true;

                // - can mean negative or the break. A minus before any numbers means
                // it is negative. Two dashes in the middle means the second number is negative.
                // If there is only one dash, treat it like a break, not a negative.
                int numDashes = CountOf(exp, "-");
                string[] args = exp.Split('-');
                bool minNegative = false;
                bool maxNegative = false;
                int minIndex = 0;
                int maxIndex = 1;

                if (numDashes > 1)
                {
                    // -10 - 20 |  10 - -20 | -20 - -10
                    if (args[0] == string.Empty)
                    {
                        // -10 - 20
                        minNegative = true;
                        minIndex = 1;
                        maxIndex = 2;
                        if (numDashes > 2)
                        {
                            // -20 - -10
                            maxNegative = true;
                            maxIndex = 3;
                        }
                    }
                    else
                    {
                        // the range could be out of order, like 10 - -20.
                        maxNegative = true;
                        maxIndex = 2;
                    }
                }

                double min;
                double max;
                if (double.TryParse(args[minIndex], out min))
                {
                    Minimum = minNegative ? -min : min;
                }

                if (double.TryParse(args[maxIndex], out max))
                {
                    Maximum = maxNegative ? -max : max;
                }

                FixOrder();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the maximum value. If this is null, the upper range is unbounded.
        /// </summary>
        [Serialize("Maximum")]
        public double? Maximum { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the upper bounds includes the maximum value.
        /// </summary>
        [Serialize("MaxIsInclusive")]
        public bool MaxIsInclusive { get; set; }

        /// <summary>
        /// Gets or sets the Minimum value. If this is null, the lower range is unbounded.
        /// </summary>
        [Serialize("Minimum")]
        public double? Minimum { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the lower bounds includes the minimum value.
        /// </summary>
        [Serialize("MinIsInclusive")]
        public bool MinIsInclusive { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Tests to determine if this range contains the specified double value.
        /// </summary>
        /// <param name="value">the double value to test.</param>
        /// <returns>Boolean, true if the value is within the current bounds.</returns>
        public bool Contains(double value)
        {
            if (Minimum == null && Maximum == null) return true;
            if (Minimum == null)
            {
                return MaxIsInclusive ? (value <= Maximum) : (value < Maximum);
            }

            if (Maximum == null)
            {
                return MinIsInclusive ? (value >= Minimum) : (value > Minimum);
            }

            if (MaxIsInclusive && MinIsInclusive)
            {
                return value >= Minimum && value <= Maximum;
            }

            if (MaxIsInclusive)
            {
                return value > Minimum && value <= Maximum;
            }

            if (MinIsInclusive)
            {
                return value >= Minimum && value < Maximum;
            }

            return value > Minimum && value < Maximum;
        }

        /// <summary>
        /// Generates a valid SQL query expression for this range, using the field string
        /// as the member being compared. The field string should already be bound in
        /// brackets, or put together as a normal composit like "[males]/[pop1990]".
        /// </summary>
        /// <param name="field">The field name to build into an expression. This should already be wrapped in square brackets.</param>
        /// <returns>The string SQL query expression.</returns>
        public string ToExpression(string field)
        {
            if (Minimum == null && Maximum == null) return string.Empty;

            string maxExp = MaxIsInclusive ? field + " <= " + Maximum : field + " < " + Maximum;
            string minExp = MinIsInclusive ? field + " >= " + Minimum : field + " > " + Minimum;
            if (Minimum == null) return maxExp;
            if (Maximum == null) return minExp;

            return minExp + " AND " + maxExp;
        }

        /// <summary>
        /// Expresses this range in string form. By default, ranges include the maximum,
        /// and exclude the minimum. A null value for one expression will result in a
        /// a semi-unbounded range using the greater than or less than symbols. A null
        /// expression for both values is completely unbounded and will result in a string
        /// that reads like [All Values].
        /// </summary>
        /// <returns>A string representing the range.</returns>
        public override string ToString()
        {
            if (Minimum == null && Maximum == null)
            {
                return "[All Values]";
            }

            if (Minimum == null)
            {
                return MaxIsInclusive ? "<= " + Maximum : "< " + Maximum;
            }

            if (Maximum == null)
            {
                return MinIsInclusive ? ">= " + Minimum : "> " + Minimum;
            }

            return Minimum + " - " + Maximum;
        }

        /// <summary>
        /// This is a slightly more complex specification where the numeric formatting
        /// controls how the generated string will appear.
        /// </summary>
        /// <param name="method">The interval snap method.</param>
        /// <param name="digits">This is only used for rounding or significant figures, but controls those options.</param>
        /// <returns>A string equivalent of this range, but using a number format.</returns>
        public string ToString(IntervalSnapMethod method, int digits)
        {
            if (Minimum == null && Maximum == null)
            {
                return "[All Values]";
            }

            if (Minimum == null)
            {
                string max = Format(Maximum.Value, method, digits);
                return MaxIsInclusive ? "<= " + max : "< " + max;
            }

            if (Maximum == null)
            {
                string min = Format(Minimum.Value, method, digits);
                return MinIsInclusive ? ">= " + min : "> " + min;
            }

            return Format(Minimum.Value, method, digits) + " - " + Format(Maximum.Value, method, digits);
        }

        private static int CountOf(string source, string instance)
        {
            int iStart = 0;
            int count = 0;
            while ((iStart = source.IndexOf(instance, iStart + 1)) > 0)
            {
                count++;
            }

            return count;
        }

        private static string Format(double value, IntervalSnapMethod method, int digits)
        {
            if (method == IntervalSnapMethod.None)
            {
                return value.ToString();
            }

            if (method == IntervalSnapMethod.DataValue)
            {
                return value.ToString();
            }

            if (method == IntervalSnapMethod.Rounding)
            {
                return value.ToString("N" + digits);
            }

            if (method == IntervalSnapMethod.SignificantFigures)
            {
                int dig = (int)Math.Ceiling(Math.Log10(Math.Abs(value)));
                dig = digits - dig;
                if (dig < 0) dig = 0;
                if (dig > 10)
                {
                    return value.ToString("E" + digits);
                }

                return value.ToString("N" + dig);
            }

            return value.ToString("N");
        }

        /// <summary>
        /// If the minimum and maximum are out of order, this reverses them.
        /// </summary>
        private void FixOrder()
        {
            if (Maximum == null || Minimum == null || Maximum.Value >= Minimum.Value) return;

            double? temp = Maximum;
            Maximum = Minimum;
            Minimum = temp;
        }

        #endregion
    }
}