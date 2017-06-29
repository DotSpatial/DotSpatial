// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2008 8:49:44 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Globalization;

namespace DotSpatial.Data
{
    /// <summary>
    /// Global has some basic methods that may be useful in lots of places.
    /// TODO: This class needs to be removed and these methods relocated to easier to find places.
    /// </summary>
    public static class Global
    {
        /// <summary>
        /// Converts a double numeric value into the appropriate T data type using a ChangeType process.
        /// </summary>
        /// <typeparam name="T">The numeric output type created from the double value.</typeparam>
        /// <param name="value">The double value to retrieve the equivalent numeric value for.</param>
        /// <returns>A variable of type T with the value of the value parameter.</returns>
        public static T Convert<T>(double value)
        {
            return (T)System.Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// This involves boxing and unboxing as well as a convert to double, but IConvertible was
        /// not CLS Compliant, so we were always getting warnings about it.  Ted was trying to make
        /// all the code CLS Compliant to remove warnings.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble<U>(U value)
        {
            if (typeof(U) == typeof(byte)) return System.Convert.ToDouble((byte)(object)value);
            if (typeof(U) == typeof(short)) return System.Convert.ToDouble((short)(object)value);
            if (typeof(U) == typeof(int)) return System.Convert.ToDouble((int)(object)value);
            if (typeof(U) == typeof(long)) return System.Convert.ToDouble((long)(object)value);

            if (typeof(U) == typeof(float)) return System.Convert.ToDouble((float)(object)value);
            if (typeof(U) == typeof(double)) return (double)(object)value;

            if (typeof(U) == typeof(ushort)) return System.Convert.ToDouble((ushort)(object)value);
            if (typeof(U) == typeof(uint)) return System.Convert.ToDouble((uint)(object)value);
            if (typeof(U) == typeof(ulong)) return System.Convert.ToDouble((ulong)(object)value);

            return 0;
        }

        /// <summary>
        /// For numeric types, this will return the maximum value.
        /// </summary>
        /// <typeparam name="T">The type of the numeric range to find the maximum for.</typeparam>
        /// <returns>The maximum value for the numeric type specified by T.</returns>
        public static T MaximumValue<T>()
        {
            T value = default(T);
            if (value is byte)
            {
                return SafeCastTo<T>(byte.MaxValue);
            }
            if (value is short)
            {
                return SafeCastTo<T>(short.MaxValue);
            }
            if (value is int)
            {
                return SafeCastTo<T>(int.MaxValue);
            }
            if (value is long)
            {
                return SafeCastTo<T>(long.MaxValue);
            }

            if (value is float)
            {
                return SafeCastTo<T>(float.MaxValue);
            }
            if (value is double)
            {
                return SafeCastTo<T>(double.MaxValue);
            }

            if (value is UInt16)
            {
                return SafeCastTo<T>(UInt16.MaxValue);
            }
            if (value is UInt32)
            {
                return SafeCastTo<T>(UInt32.MaxValue);
            }
            if (value is UInt64)
            {
                return SafeCastTo<T>(UInt64.MaxValue);
            }

            return default(T);
        }

        /// <summary>
        /// For Numeric types, this will return the minimum value.
        /// </summary>
        /// <typeparam name="T">The type of the numeric range to return the minimum for.</typeparam>
        /// <returns>The the minimum value possible for a numeric value of type T.</returns>
        public static T MinimumValue<T>()
        {
            T value = default(T);
            if (value is byte)
            {
                return SafeCastTo<T>(byte.MinValue);
            }
            if (value is short)
            {
                return SafeCastTo<T>(short.MinValue);
            }
            if (value is int)
            {
                return SafeCastTo<T>(int.MinValue);
            }
            if (value is long)
            {
                return SafeCastTo<T>(long.MinValue);
            }

            if (value is float)
            {
                return SafeCastTo<T>(float.MinValue);
            }
            if (value is double)
            {
                return SafeCastTo<T>(double.MinValue);
            }

            if (value is UInt16)
            {
                return SafeCastTo<T>(UInt16.MinValue);
            }
            if (value is UInt32)
            {
                return SafeCastTo<T>(UInt32.MinValue);
            }
            if (value is UInt64)
            {
                return SafeCastTo<T>(UInt64.MinValue);
            }

            return default(T);
        }

        /// <summary>
        /// A Generic Safe Casting method that should simply exist as part of the core framework
        /// </summary>
        /// <typeparam name="T">The type of the member to attempt to cast to.</typeparam>
        /// <param name="obj">The original object to attempt to System.Convert.</param>
        /// <returns>An output variable of type T.</returns>
        public static T SafeCastTo<T>(object obj)
        {
            if (obj == null)
            {
                return default(T);
            }
            if (!(obj is T))
            {
                return default(T);
            }
            return (T)obj;
        }

        /// <summary>
        /// Uses the standard enum parsing, but returns it cast as the specified T parameter
        /// </summary>
        /// <typeparam name="T">The type of the enum to use</typeparam>
        /// <param name="text">The string to parse into a copy of the enumeration</param>
        /// <returns>an enumeration of the specified type</returns>
        public static T ParseEnum<T>(string text)
        {
            return SafeCastTo<T>(Enum.Parse(typeof(T), text));
        }

        /// <summary>
        /// This attempts to convert a value into a byte.  If it fails, the byte will be 0.
        /// </summary>
        /// <param name="expression">The expression (like a string) to System.Convert.</param>
        /// <returns>A byte that is 0 if the test fails.</returns>
        public static byte GetByte(object expression)
        {
            byte retNum;
            if (Byte.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum))
            {
                return retNum;
            }
            return 0;
        }

        /// <summary>
        /// This attempts to convert a value into a double.  If it fails, the double will be double.NaN.
        /// </summary>
        /// <param name="expression">The expression (like a string) to System.Convert.</param>
        /// <returns>A double that is double.NAN if the test fails.</returns>
        public static double GetDouble(object expression)
        {
            double retNum;
            return Double.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum) ? retNum : double.NaN;
        }

        /// <summary>
        /// This attempts to convert a value into a float.  If it fails, the float will be 0.
        /// </summary>
        /// <param name="expression">The expression (like a string) to System.Convert.</param>
        /// <returns>A float that is 0 if the test fails.</returns>
        public static float GetFloat(object expression)
        {
            float retNum;
            return Single.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum) ? retNum : 0F;
        }

        /// <summary>
        /// This attempts to convert a value into an integer.  If it fails, it returns 0.
        /// </summary>
        /// <param name="expression">The expression to test</param>
        /// <returns>true if the value could be cast as a double, false otherwise</returns>
        public static int GetInteger(object expression)
        {
            int retNum;
            return Int32.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum) ? retNum : 0;
        }

        /// <summary>
        /// This attempts to convert a value into a short.  If it fails, it returns 0.
        /// </summary>
        /// <param name="expression">The expression (like a string) to System.Convert.</param>
        /// <returns>A short that is 0 if the test fails.</returns>
        public static double GetShort(object expression)
        {
            short retNum;
            if (Int16.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum))
            {
                return retNum;
            }
            return 0;
        }

        /// <summary>
        /// Gets the string form of the number using culture settings
        /// </summary>
        /// <param name="expression">The expression to obtain the string for</param>
        /// <returns>A string</returns>
        public static string GetString(object expression)
        {
            return System.Convert.ToString(expression, CulturePreferences.CultureInformation.NumberFormat);
        }

        /// <summary>
        /// Tests an expression to see if it can be converted into a byte.
        /// </summary>
        /// <param name="expression">The expression to test</param>
        /// <returns>true if the value could be cast as a double, false otherwise</returns>
        public static bool IsByte(object expression)
        {
            byte retNum;
            bool isNum = Byte.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum);
            return isNum;
        }

        /// <summary>
        /// Tests an expression to see if it can be converted into a double.
        /// </summary>
        /// <param name="expression">The expression to test</param>
        /// <returns>true if the value could be cast as a double, false otherwise</returns>
        public static bool IsDouble(object expression)
        {
            double retNum;

            bool isNum = Double.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum);
            return isNum;
        }

        /// <summary>
        /// Tests an expression to see if it can be converted into a float.
        /// </summary>
        /// <param name="expression">The expression to test</param>
        /// <returns>true if the value could be cast as a double, false otherwise</returns>
        public static bool IsFloat(object expression)
        {
            float retNum;
            bool isNum = Single.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum);
            return isNum;
        }

        /// <summary>
        /// Tests an expression to see if it can be converted into an integer.
        /// </summary>
        /// <param name="expression">The expression to test</param>
        /// <returns>true if the value could be cast as an integer, false otherwise</returns>
        public static bool IsInteger(object expression)
        {
            int retNum;
            bool isNum = Int32.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum);
            return isNum;
        }

        /// <summary>
        /// Tests an expression to see if it can be converted into a short.
        /// </summary>
        /// <param name="expression">The expression to test</param>
        /// <returns>true if the value could be cast as a double, false otherwise</returns>
        public static bool IsShort(object expression)
        {
            short retNum;
            bool isNum = Int16.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum);
            return isNum;
        }
    }
}