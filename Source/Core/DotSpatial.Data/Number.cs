// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// Number.
    /// </summary>
    public struct Number : IComparable, IComparable<Number>, IComparable<double>
    {
        #region Private methods

        private bool _hasValue;
        private double _value;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Number"/> struct.
        /// Creates a value from an object.
        /// </summary>
        /// <param name="value">A numeric value that is, or can be parsed to a numeric value.</param>
        /// <exception cref="NonNumericException">Not Numeric.</exception>
        public Number(object value)
        {
            if (Global.IsShort(value))
            {
                _value = Global.GetDouble(value);
                SignificantFigures = 5;
                DecimalsCount = 0;
                Format = NumberFormat.General;
                _hasValue = true;
                return;
            }

            if (Global.IsInteger(value))
            {
                _value = Global.GetDouble(value);
                SignificantFigures = 10;
                DecimalsCount = 0;
                Format = NumberFormat.General;
                _hasValue = true;
                return;
            }

            if (Global.IsFloat(value))
            {
                _value = Global.GetDouble(value);
                SignificantFigures = 8;
                DecimalsCount = 7;
                Format = NumberFormat.General;
                _hasValue = true;
                return;
            }

            if (Global.IsDouble(value))
            {
                // doubles can have 16 digits, so in scientific notation
                _value = Global.GetDouble(value);
                SignificantFigures = 16;
                DecimalsCount = 15;
                Format = NumberFormat.General;
                _hasValue = true;
                return;
            }

            throw new NonNumericException("value");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number"/> struct to fit the specified double value.
        /// </summary>
        /// <param name="value">A double.</param>
        public Number(double value)
        {
            // doubles can have 16 digits, so in scientific notation
            _value = value;
            SignificantFigures = 16;
            DecimalsCount = 15;
            Format = NumberFormat.General;
            _hasValue = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number"/> struct to fit the specified int value.
        /// </summary>
        /// <param name="value">An integer.</param>
        public Number(int value)
        {
            _value = value;
            SignificantFigures = 10;
            DecimalsCount = 0;
            Format = NumberFormat.General;
            _hasValue = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number"/> struct to fit the specified float value.
        /// </summary>
        /// <param name="value">The value to work with.</param>
        public Number(float value)
        {
            _value = value;
            SignificantFigures = 8;
            DecimalsCount = 7;
            Format = NumberFormat.General;
            _hasValue = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Number"/> struct to fit the specified short value.
        /// </summary>
        /// <param name="value">A short.</param>
        public Number(short value)
        {
            _value = value;
            SignificantFigures = 5;
            DecimalsCount = 0;
            Format = NumberFormat.General;
            _hasValue = true;
        }

        /// <summary>
        /// Gets the alphabetical letter code for ToString(-Code-).
        /// </summary>
        public string Code
        {
            get
            {
                return Format switch
                {
                    NumberFormat.Currency => "C",
                    NumberFormat.Exponential => "E",
                    NumberFormat.FixedPoint => "F",
                    NumberFormat.General => "G",
                    NumberFormat.Number => "N",
                    NumberFormat.Percent => "P",
                    NumberFormat.Unspecified => "G",
                    _ => "G",
                };
            }
        }

        /// <summary>
        /// Gets or sets the format. The NumberFormats enumeration contains the various formatting options.
        /// </summary>
        public NumberFormat Format { get; set; }

        /// <summary>
        /// Gets a value indicating whether a value has been assigned to this Number.
        /// </summary>
        public bool IsEmpty => !_hasValue;

        /// <summary>
        /// Gets or sets the number of digits that folow the decimal
        /// when converting this value to string notation.
        /// </summary>
        public int DecimalsCount { get; set; }

        /// <summary>
        /// Gets the precision (determined by the data type) which
        /// effectively describes how many significant figures there are.
        /// </summary>
        public int SignificantFigures { get; }

        /// <summary>
        /// Gets or sets this number as a double.
        /// </summary>
        /// <returns>a double.</returns>
        public double Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
                _hasValue = true;
            }
        }

        #region IComparable Members

        /// <summary>
        /// Compares this with the specified value. Returns 1 if the other
        /// item is greater than this value, 0 if they are equal and -1 if
        /// the other value is less than this value.
        /// </summary>
        /// <param name="other">The value to be tested.</param>
        /// <returns>An integer: 1 if the other
        /// item is greater than this value, 0 if they are equal and -1 if
        /// the other value is less than this value. </returns>
        public int CompareTo(object other)
        {
            if (Global.IsDouble(other) == false)
            {
                throw new NonNumericException("other");
            }

            return _value.CompareTo(Global.GetDouble(other));
        }

        #endregion

        #region IComparable<double> Members

        /// <summary>
        /// Compares this with the specified value. Returns 1 if the other
        /// item is greater than this value, 0 if they are equal and -1 if
        /// the other value is less than this value.
        /// </summary>
        /// <param name="other">The value to be tested.</param>
        /// <returns>An integer: 1 if the other
        /// item is greater than this value, 0 if they are equal and -1 if
        /// the other value is less than this value. </returns>
        public int CompareTo(double other)
        {
            return _value.CompareTo(other);
        }

        #endregion

        #region IComparable<Number> Members

        /// <summary>
        /// Compares this with the specified value. Returns 1 if the other
        /// item is greater than this value, 0 if they are equal and -1 if
        /// the other value is less than this value.
        /// </summary>
        /// <param name="other">The value to be tested.</param>
        /// <returns>An integer: 1 if the other
        /// item is greater than this value, 0 if they are equal and -1 if
        /// the other value is less than this value. </returns>
        public int CompareTo(Number other)
        {
            return _value.CompareTo(other.Value);
        }

        #endregion

        /// <summary>
        /// Returns this Number as a string.
        /// </summary>
        /// <returns>The string created using the specified number format and precision.</returns>
        public override string ToString()
        {
            return _value.ToString(Code + DecimalsCount, CulturePreferences.CultureInformation.NumberFormat);
        }

        /// <summary>
        /// Returns this Number as a string.
        /// </summary>
        /// <param name="provider">An IFormatProvider that provides culture specific formatting information.</param>
        /// <returns>A string with the formatted number.</returns>
        public string ToString(IFormatProvider provider)
        {
            return _value.ToString(Code + DecimalsCount, provider);
        }

        /// <summary>
        /// Returns this Number as a string.
        /// </summary>
        /// <param name="format">A string that controls how this value should be formatted.</param>
        /// <returns>A string with the formatted number.</returns>
        public string ToString(string format)
        {
            return _value.ToString(format, CulturePreferences.CultureInformation.NumberFormat);
        }

        /// <summary>
        ///  Returns this Number as a string.
        /// </summary>
        /// <param name="format">A string that controls how this value should be formatted.</param>
        /// <param name="provider">An IFormatProvider that provides culture specific formatting information.</param>
        /// <returns>A string with the formatted number.</returns>
        public string ToString(string format, IFormatProvider provider)
        {
            return _value.ToString(format, provider);
        }

        /// <summary>
        /// Gets this number as an int 32.
        /// </summary>
        /// <returns>An integer.</returns>
        public int ToInt32()
        {
            if (_value > int.MaxValue) return int.MaxValue;
            if (_value < int.MinValue) return int.MinValue;

            return Convert.ToInt32(_value);
        }

        /// <summary>
        /// Gets this number as a short, or a short.MaxValue/short.MinValue.
        /// </summary>
        /// <returns>A short.</returns>
        public short ToInt16()
        {
            if (_value > short.MaxValue) return short.MaxValue;
            if (_value < short.MinValue) return short.MinValue;

            return Convert.ToInt16(_value);
        }

        /// <summary>
        /// Gets this number as a float.
        /// </summary>
        /// <returns>A float. </returns>
        public float ToFloat()
        {
            if (_value > float.MaxValue) return float.MaxValue;
            if (_value > float.MinValue) return float.MinValue;

            return Convert.ToSingle(_value);
        }
    }
}