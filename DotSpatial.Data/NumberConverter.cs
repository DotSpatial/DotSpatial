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
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/7/2008 10:27:12 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

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
        /// <summary>
        /// Numbers can contain ASCII text up till 18 characters long, but no longer
        /// </summary>
        public const int MaximumLength = 18;
        ///<summary>
        /// Format provider to use to convert DBF numbers to strings and characters
        ///</summary>
        public static readonly IFormatProvider NumberConversionFormatProvider = CultureInfo.GetCultureInfo("en-US");

        #region Private Variables

        private static readonly Random _rnd = new Random();
        private int _decimalCount; // when the number is treated like a string, this is the number of recorded values after the decimal, plus one digit in front of the decimal.
        private int _length; // when the number is treated like a string, this is the total length, including minus signs and decimals.

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of NumberConverter where the length and decimal count are known.
        /// </summary>
        public NumberConverter(int inLength, int inDecimalCount)
        {
            _length = inLength;

            _decimalCount = inDecimalCount;
            if (_length < 4)
            {
                _decimalCount = 0;
            }
            else if (_decimalCount > _length - 3)
            {
                _decimalCount = _length - 3;
            }
            UpdateDecimalFormatString();
        }

        /// <summary>
        /// Cycles through the numeric values in the specified column and determines a selection of
        /// length and decimal count can accurately store the data.
        /// </summary>
        public NumberConverter(IList<double> values)
        {
            int maxExp = 0;
            int minExp = 0;
            for (int i = 0; i < values.Count; i++)
            {
                int exp = (int)Math.Log10(Math.Abs(values[i]));
                if (exp > MaximumLength - 1)
                {
                    throw new NumberException(DataStrings.NumberException_TooLarge_S.Replace("%S", values[i].ToString()));
                }
                if (exp < -(MaximumLength - 1))
                {
                    throw new NumberException(DataStrings.NumberException_TooSmall_S.Replace("%S", values[i].ToString()));
                }
                if (exp > maxExp) maxExp = exp;
                if (exp < minExp) minExp = exp;
                if (exp < MaximumLength - 2) continue;
                // If this happens, we know that we need all the characters for values greater than 1, so no characters are left
                // for storing both the decimal itself and the numbers beyond the decimal.
                _length = MaximumLength;
                _decimalCount = 0;
                UpdateDecimalFormatString();
                return;
            }
            UpdateDecimalFormatString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new, random double that is constrained by the specified length and decimal count.
        /// </summary>
        /// <returns></returns>
        public double RandomDouble()
        {
            string test = new string(RandomChars(14));
            return double.Parse(test);
        }

        /// <summary>
        /// Creates a new, random float that is constrained by the specified length and decimal count.
        /// </summary>
        /// <returns>A new float.  Floats can only store about 8 digits of precision, so specifying a high </returns>
        public float RandomFloat()
        {
            string test = new string(RandomChars(6));
            return float.Parse(test);
        }

        /// <summary>
        /// Creates a new, random decimal that is constrained by the specified length and decimal count.
        /// </summary>
        /// <returns></returns>
        public decimal RandomDecimal()
        {
            string test = new string(RandomChars(16));
            return decimal.Parse(test);
        }

        /// <summary>
        /// Creates a new random array of characters that represents a number and is constrained by the specified length and decimal count
        /// </summary>
        /// <param name="numDigits">The integer number of significant (non-zero) digits that should be created as part of the number.</param>
        /// <returns>A character array of that matches the length and decimal count specified by this properties on this number converter</returns>
        public char[] RandomChars(int numDigits)
        {
            if (numDigits > _length - 1)
            {
                numDigits = _length - 1; // crop digits to length (reserve one spot for negative sign)
            }
            else if (numDigits < _length - 2)
            {
                if (_decimalCount > 0 && numDigits >= _decimalCount) numDigits += 1; // extend digits by decimal
            }
            bool isNegative = (_rnd.Next(0, 1) == 0);

            char[] c = new char[_length];

            // i represents the distance from the end, moving backwards
            for (int i = 0; i < _length; i++)
            {
                if (_decimalCount > 0 && i == _decimalCount - 2)
                {
                    c[i] = '.';
                }
                else if (i < numDigits)
                {
                    c[i] = (char)_rnd.Next(48, 57);
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
        /// Converts from a string, or 0 if the parse failed
        /// </summary>
        /// <param name="value">The string value to parse</param>
        /// <returns></returns>
        public double FromString(string value)
        {
            double result;
            double.TryParse(value, out result);
            return result;
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

        private char[] ToCharInternal(object number)
        {
            char[] c = new char[_length];
            string str = String.Format(NumberConversionFormatProvider, DecimalFormatString, number);
            if (str.Length >= _length)
            {
                for (int i = 0; i < _length; i++)
                {
                    c[i] = str[i]; // keep the left characters, and chop off lesser characters
                }
            }
            else
            {
                for (int i = 0; i < _length; i++)
                {
                    int ci = i - (_length - str.Length);
                    c[i] = ci < 0 ? ' ' : str[ci];
                }
            }
            return c;
        }

        private string ToStringInternal(object number)
        {
            var sb = new StringBuilder();
            var str = String.Format(NumberConversionFormatProvider, DecimalFormatString, number);
            for (var i = 0; i < _length - str.Length; i++)
            {
                sb.Append(' ');
            }
            sb.Append(str);
            return sb.ToString();
        }

       
        /// <summary>
        /// Compute and update the DecimalFormatString from the precision specifier
        /// </summary>
        public void UpdateDecimalFormatString()
        {
            string format = "{0:";
            for (int i = 0; i < _decimalCount; i++)
            {
                if (i == 0)
                    format = format + "0.";
                format = format + "0";
            }
            DecimalFormatString = format + "}";
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or set the length
        /// </summary>
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        /// <summary>
        /// Gets or sets the decimal count to use for this number converter
        /// </summary>
        public int DecimalCount
        {
            get { return _decimalCount; }
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

        #endregion
    }
}