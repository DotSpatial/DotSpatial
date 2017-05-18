// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace DotSpatial.Data
{
    /// <summary>
    /// Numbers are based on the old school dbf definitions of data formats, and so can only store
    /// a very limited range of values.
    /// </summary>
    public class NumberConverter
    {
        #region Fields

        /// <summary>
        /// Numbers can contain ASCII text up till 18 characters long, but no longer.
        /// </summary>
        public const int MaximumLength = 18;

        /// <summary>
        /// Format provider to use to convert DBF numbers to strings and characters
        /// </summary>
        public static readonly IFormatProvider NumberConversionFormatProvider = CultureInfo.GetCultureInfo("en-US");

        private static readonly Random Rnd = new Random();
        private int _decimalCount; // when the number is treated like a string, this is the number of recorded values after the decimal, plus one digit in front of the decimal.

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberConverter"/> class.
        /// Creates a new instance of NumberConverter where the length and decimal count are known.
        /// </summary>
        /// <param name="inLength">The length.</param>
        /// <param name="inDecimalCount">The decimal count.</param>
        public NumberConverter(int inLength, int inDecimalCount)
        {
            Length = inLength;

            _decimalCount = inDecimalCount;
            if (Length < 4)
            {
                _decimalCount = 0;
            }
            else if (_decimalCount > Length - 3)
            {
                _decimalCount = Length - 3;
            }

            UpdateDecimalFormatString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberConverter"/> class.
        /// Cycles through the numeric values in the specified column and determines a selection of
        /// length and decimal count can accurately store the data.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <exception cref="NumberException">If the value was to small or lage to be encode with 18 ASCII characters.</exception>
        public NumberConverter(IList<double> values)
        {
            int maxExp = 0;
            int minExp = 0;
            foreach (double value in values)
            {
                int exp = (int)Math.Log10(Math.Abs(value));
                if (exp > MaximumLength - 1)
                {
                    throw new NumberException(string.Format(DataStrings.NumberException_TooLarge_S, value));
                }

                if (exp < -(MaximumLength - 1))
                {
                    throw new NumberException(string.Format(DataStrings.NumberException_TooSmall_S, value));
                }

                if (exp > maxExp) maxExp = exp;
                if (exp < minExp) minExp = exp;
                if (exp < MaximumLength - 2) continue;

                // If this happens, we know that we need all the characters for values greater than 1, so no characters are left
                // for storing both the decimal itself and the numbers beyond the decimal.
                Length = MaximumLength;
                _decimalCount = 0;
                UpdateDecimalFormatString();
                return;
            }

            UpdateDecimalFormatString();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the decimal count to use for this number converter
        /// </summary>
        public int DecimalCount
        {
            get
            {
                return _decimalCount;
            }

            set
            {
                _decimalCount = value;
                UpdateDecimalFormatString();
            }
        }

        /// <summary>
        /// Gets or sets the format string used to convert doubles, floats, and decimals to strings
        /// </summary>
        public string DecimalFormatString { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public int Length { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Converts from a string, or 0 if the parse failed.
        /// </summary>
        /// <param name="value">The string value to parse</param>
        /// <returns>The parse result.</returns>
        public double FromString(string value)
        {
            double result;
            double.TryParse(value, out result);
            return result;
        }

        /// <summary>
        /// Creates a new random array of characters that represents a number and is constrained by the specified length and decimal count
        /// </summary>
        /// <param name="numDigits">The integer number of significant (non-zero) digits that should be created as part of the number.</param>
        /// <returns>A character array of that matches the length and decimal count specified by this properties on this number converter</returns>
        public char[] RandomChars(int numDigits)
        {
            if (numDigits > Length - 1)
            {
                numDigits = Length - 1; // crop digits to length (reserve one spot for negative sign)
            }
            else if (numDigits < Length - 2)
            {
                if (_decimalCount > 0 && numDigits >= _decimalCount) numDigits += 1; // extend digits by decimal
            }

            bool isNegative = Rnd.Next(0, 1) == 0;

            char[] c = new char[Length];

            // i represents the distance from the end, moving backwards
            for (int i = 0; i < Length; i++)
            {
                if (_decimalCount > 0 && i == _decimalCount - 2)
                {
                    c[i] = '.';
                }
                else if (i < numDigits)
                {
                    c[i] = (char)Rnd.Next(48, 57);
                }
                else if (i < _decimalCount - 2)
                {
                    c[i] = '0';
                }
                else
                {
                    c[i] = ' ';
                }
            }

            if (isNegative)
            {
                c[numDigits] = '-';
            }

            Array.Reverse(c);
            return c;
        }

        /// <summary>
        /// Creates a new, random decimal that is constrained by the specified length and decimal count.
        /// </summary>
        /// <returns>The created random decimal.</returns>
        public decimal RandomDecimal()
        {
            string test = new string(RandomChars(16));
            return decimal.Parse(test);
        }

        /// <summary>
        /// Creates a new, random double that is constrained by the specified length and decimal count.
        /// </summary>
        /// <returns>The created random double.</returns>
        public double RandomDouble()
        {
            string test = new string(RandomChars(14));
            return double.Parse(test);
        }

        /// <summary>
        /// Creates a new, random float that is constrained by the specified length and decimal count.
        /// </summary>
        /// <returns>A new float. Floats can only store about 8 digits of precision, so specifying a high </returns>
        public float RandomFloat()
        {
            string test = new string(RandomChars(6));
            return float.Parse(test);
        }

        /// <summary>
        /// Converts the specified decimal value to a string that can be used for the number field
        /// </summary>
        /// <param name="number">The decimal value to convert to a string</param>
        /// <returns>A string version of the specified number</returns>
        public char[] ToChar(double number)
        {
            return ToCharInternal(number);
        }

        /// <summary>
        /// Converts the specified decimal value to a string that can be used for the number field
        /// </summary>
        /// <param name="number">The decimal value to convert to a string</param>
        /// <returns>A string version of the specified number</returns>
        public char[] ToChar(float number)
        {
            return ToCharInternal(number);
        }

        /// <summary>
        /// Converts the specified decimal value to a string that can be used for the number field
        /// </summary>
        /// <param name="number">The decimal value to convert to a string</param>
        /// <returns>A string version of the specified number</returns>
        public char[] ToChar(decimal number)
        {
            return ToCharInternal(number);
        }

        /// <summary>
        /// Converts the specified double value to a string that can be used for the number field
        /// </summary>
        /// <param name="number">The double precision floating point value to convert to a string</param>
        /// <returns>A string version of the specified number</returns>
        public string ToString(double number)
        {
            return ToStringInternal(number);
        }

        /// <summary>
        /// Converts the specified decimal value to a string that can be used for the number field
        /// </summary>
        /// <param name="number">The decimal value to convert to a string</param>
        /// <returns>A string version of the specified number</returns>
        public string ToString(decimal number)
        {
            return ToStringInternal(number);
        }

        /// <summary>
        /// Converts the specified float value to a string that can be used for the number field
        /// </summary>
        /// <param name="number">The floating point value to convert to a string</param>
        /// <returns>A string version of the specified number</returns>
        public string ToString(float number)
        {
            return ToStringInternal(number);
        }

        /// <summary>
        /// Compute and update the DecimalFormatString from the precision specifier
        /// </summary>
        public void UpdateDecimalFormatString()
        {
            string format = "{0:";
            for (int i = 0; i < _decimalCount; i++)
            {
                if (i == 0) format = format + "0.";
                format = format + "0";
            }

            DecimalFormatString = format + "}";
        }

        private char[] ToCharInternal(object number)
        {
            char[] c = new char[Length];
            string str = string.Format(NumberConversionFormatProvider, DecimalFormatString, number);
            if (str.Length >= Length)
            {
                for (int i = 0; i < Length; i++)
                {
                    c[i] = str[i]; // keep the left characters, and chop off lesser characters
                }
            }
            else
            {
                for (int i = 0; i < Length; i++)
                {
                    int ci = i - (Length - str.Length);
                    c[i] = ci < 0 ? ' ' : str[ci];
                }
            }

            return c;
        }

        private string ToStringInternal(object number)
        {
            var sb = new StringBuilder();
            var str = string.Format(NumberConversionFormatProvider, DecimalFormatString, number);
            for (var i = 0; i < Length - str.Length; i++)
            {
                sb.Append(' ');
            }

            sb.Append(str);
            return sb.ToString();
        }

        #endregion
    }
}