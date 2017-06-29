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
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/3/2009 10:10:33 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Numeric range using doubles
    /// </summary>
    public class Range
    {
        #region Private Variables

        /// <summary>
        /// Boolean, true if the upper bounds includes the maximum value.
        /// </summary>
        private bool _maxIsInclusive;

        /// <summary>
        /// The maximum value.  If this is null, the upper range is unbounded.
        /// </summary>
        private double? _maximum;

        /// <summary>
        /// Boolean, true if the the lower bounds includes the minimum value.
        /// </summary>
        private bool _minIsInclusive;

        /// <summary>
        /// The minimum value.  If this is null, the lower range is unbounded.
        /// </summary>
        private double? _minimum;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Range (with undefined interval)
        /// </summary>
        public Range()
        {
            Minimum = null;
            Maximum = null;
        }

        /// <summary>
        /// Creates a new instance of Range
        /// </summary>
        /// <param name="value1">Either bound of the range</param>
        /// <param name="value2">The other bound of the range</param>
        public Range(double? value1, double? value2)
        {
            Minimum = value1;
            Maximum = value2;
            FixOrder();
        }

        /// <summary>
        /// Creates an equality type range where both the minimum and maximum are the
        /// same value and both are inclusive.
        /// </summary>
        /// <param name="value"></param>
        public Range(double value)
        {
            Minimum = value;
            Maximum = value;
            MinIsInclusive = true;
            MaxIsInclusive = true;
        }

        /// <summary>
        /// A string expression that can be two separate numbers separated by a dash,
        /// </summary>
        /// <param name="expression"></param>
        public Range(string expression)
        {
            string exp = expression ?? "-";
            if (exp.Contains(">=") || exp.Contains(">"))
            {
                _minIsInclusive = exp.Contains(">=");
                _maximum = null;
                double min;
                if (double.TryParse(exp.Replace(">=", string.Empty).Replace(">", string.Empty), out min))
                {
                    _minimum = min;
                }
            }
            else if (exp.Contains("<=") || exp.Contains("<"))
            {
                _maxIsInclusive = exp.Contains("<=");
                _minimum = null;
                double max;
                if (double.TryParse(exp.Replace("<=", string.Empty).Replace("<", string.Empty), out max))
                {
                    _maximum = max;
                }
            }
            else if (exp.Contains("-"))
            {
                // The default is to actually include the maximums, but not the minimums.
                _maxIsInclusive = true;
                // - can mean negative or the break.  A minus before any numbers means
                // it is negative.  Two dashes in the middle means the second number is negative.
                // If there is only one dash, treat it like a break, not a negative.
                int numDashes = CountOf(exp, "-");
                string[] args = exp.Split(new[] { '-' });
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

        #region Methods

        /// <summary>
        /// Expresses this range in string form.  By default, ranges include the maximum,
        /// and exclude the minimum.  A null value for one expression will result in a
        /// a semi-unbounded range using the greater than or less than symbols.  A null
        /// expression for both values is completely unbounded and will result in a string
        /// that reads like [All Values].
        /// </summary>
        /// <returns>A string representing the range.</returns>
        public override string ToString()
        {
            if (_minimum == null && _maximum == null)
            {
                return "[All Values]";
            }
            if (_minimum == null)
            {
                return _maxIsInclusive ? "<= " + _maximum : "< " + _maximum;
            }
            if (_maximum == null)
            {
                return _minIsInclusive ? ">= " + _minimum : "> " + _minimum;
            }
            return _minimum + " - " + _maximum;
        }

        /// <summary>
        /// This is a slightly more complex specification where the numeric formatting
        /// controls how the generated string will appear.
        /// </summary>
        /// <param name="method">The interval snap method</param>
        /// <param name="digits">This is only used for rounding or significant figures, but controls those options</param>
        /// <returns>A string equivalent of this range, but using a number format.</returns>
        public string ToString(IntervalSnapMethod method, int digits)
        {
            if (_minimum == null && _maximum == null)
            {
                return "[All Values]";
            }
            if (_minimum == null)
            {
                string max = Format(_maximum.Value, method, digits);
                return _maxIsInclusive ? "<= " + max : "< " + max;
            }
            if (_maximum == null)
            {
                string min = Format(_minimum.Value, method, digits);
                return _minIsInclusive ? ">= " + min : "> " + min;
            }
            return Format(_minimum.Value, method, digits) + " - " + Format(_maximum.Value, method, digits);
        }

        /// <summary>
        /// Generates a valid SQL query expression for this range, using the field string
        /// as the member being compared.  The field string should already be bound in
        /// brackets, or put together as a normal composit like "[males]/[pop1990]"
        /// </summary>
        /// <param name="field">The field name to build into an expression.  This should already be wrapped in square brackets.</param>
        /// <returns>The string SQL query expression.</returns>
        public string ToExpression(string field)
        {
            if (_minimum == null && _maximum == null) return string.Empty;
            string maxExp = _maxIsInclusive ? field + " <= " + _maximum : field + " < " + _maximum;
            string minExp = _minIsInclusive ? field + " >= " + _minimum : field + " > " + _minimum;
            if (_minimum == null) return maxExp;
            if (_maximum == null) return minExp;
            return minExp + " AND " + maxExp;
        }

        /// <summary>
        /// Tests to determine if this range contains the specified double value.
        /// </summary>
        /// <param name="value">the double value to test</param>
        /// <returns>Boolean, true if the value is within the current bounds.</returns>
        public bool Contains(double value)
        {
            if (_minimum == null && _maximum == null) return true;
            if (_minimum == null)
            {
                return _maxIsInclusive ? (value <= _maximum) : (value < _maximum);
            }
            if (_maximum == null)
            {
                return _minIsInclusive ? (value >= _minimum) : (value > _minimum);
            }
            if (_maxIsInclusive && _minIsInclusive)
            {
                return value >= _minimum && value <= _maximum;
            }
            if (_maxIsInclusive)
            {
                return value > _minimum && value <= _maximum;
            }
            if (_minIsInclusive)
            {
                return value >= _minimum && value < _maximum;
            }
            return value > _minimum && value < _maximum;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets teh Minimum value.  If this is null, the lower range is unbounded.
        /// </summary>
        [Serialize("Minimum")]
        public double? Minimum
        {
            get { return _minimum; }
            set { _minimum = value; }
        }

        /// <summary>
        /// The maximum value.  If this is null, the upper range is unbounded.
        /// </summary>
        [Serialize("Maximum")]
        public double? Maximum
        {
            get { return _maximum; }
            set { _maximum = value; }
        }

        /// <summary>
        /// Boolean, true if the the lower bounds includes the minimum value.
        /// </summary>
        [Serialize("MinIsInclusive")]
        public bool MinIsInclusive
        {
            get { return _minIsInclusive; }
            set { _minIsInclusive = value; }
        }

        /// <summary>
        /// Boolean, true if the upper bounds includes the maximum value.
        /// </summary>
        [Serialize("MaxIsInclusive")]
        public bool MaxIsInclusive
        {
            get { return _maxIsInclusive; }
            set { _maxIsInclusive = value; }
        }

        #endregion

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
            double? temp = _maximum;
            Maximum = Minimum;
            Minimum = temp;
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
    }
}