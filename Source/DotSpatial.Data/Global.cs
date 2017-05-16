// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
        #region Methods

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
        /// This attempts to convert a value into a byte. If it fails, the byte will be 0.
        /// </summary>
        /// <param name="expression">The expression (like a string) to System.Convert.</param>
        /// <returns>A byte that is 0 if the test fails.</returns>
        public static byte GetByte(object expression)
        {
            byte retNum;
            if (byte.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum))
            {
                return retNum;
            }

            return 0;
        }

        /// <summary>
        /// This attempts to convert a value into a double. If it fails, the double will be double.NaN.
        /// </summary>
        /// <param name="expression">The expression (like a string) to System.Convert.</param>
        /// <returns>A double that is double.NAN if the test fails.</returns>
        public static double GetDouble(object expression)
        {
            double retNum;
            return double.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum) ? retNum : double.NaN;
        }

        /// <summary>
        /// This attempts to convert a value into a float. If it fails, the float will be 0.
        /// </summary>
        /// <param name="expression">The expression (like a string) to System.Convert.</param>
        /// <returns>A float that is 0 if the test fails.</returns>
        public static float GetFloat(object expression)
        {
            float retNum;
            return float.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum) ? retNum : 0F;
        }

        /// <summary>
        /// This attempts to convert a value into an integer. If it fails, it returns 0.
        /// </summary>
        /// <param name="expression">The expression to test</param>
        /// <returns>true if the value could be cast as a double, false otherwise</returns>
        public static int GetInteger(object expression)
        {
            int retNum;
            return int.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum) ? retNum : 0;
        }

        /// <summary>
        /// This attempts to convert a value into a short. If it fails, it returns 0.
        /// </summary>
        /// <param name="expression">The expression (like a string) to System.Convert.</param>
        /// <returns>A short that is 0 if the test fails.</returns>
        public static double GetShort(object expression)
        {
            short retNum;
            if (short.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum))
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
            bool isNum = byte.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum);
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

            bool isNum = double.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum);
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
            bool isNum = float.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum);
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
            bool isNum = int.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum);
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
            bool isNum = short.TryParse(GetString(expression), NumberStyles.Any, CulturePreferences.CultureInformation.NumberFormat, out retNum);
            return isNum;
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

            if (value is ushort)
            {
                return SafeCastTo<T>(ushort.MaxValue);
            }

            if (value is uint)
            {
                return SafeCastTo<T>(uint.MaxValue);
            }

            if (value is ulong)
            {
                return SafeCastTo<T>(ulong.MaxValue);
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

            if (value is ushort)
            {
                return SafeCastTo<T>(ushort.MinValue);
            }

            if (value is uint)
            {
                return SafeCastTo<T>(uint.MinValue);
            }

            if (value is ulong)
            {
                return SafeCastTo<T>(ulong.MinValue);
            }

            return default(T);
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
        /// This involves boxing and unboxing as well as a convert to double, but IConvertible was
        /// not CLS Compliant, so we were always getting warnings about it. Ted was trying to make
        /// all the code CLS Compliant to remove warnings.
        /// </summary>
        /// <param name="value">The value that gets converted to double. this should be a numeric.</param>
        /// <typeparam name="T">Type of the value that gets converted to double.</typeparam>
        /// <returns>0 if the type wasn't found otherwise the result of the conversion.</returns>
        public static double ToDouble<T>(T value)
        {
            if (typeof(T) == typeof(byte)) return System.Convert.ToDouble((byte)(object)value);
            if (typeof(T) == typeof(short)) return System.Convert.ToDouble((short)(object)value);
            if (typeof(T) == typeof(int)) return System.Convert.ToDouble((int)(object)value);
            if (typeof(T) == typeof(long)) return System.Convert.ToDouble((long)(object)value);

            if (typeof(T) == typeof(float)) return System.Convert.ToDouble((float)(object)value);
            if (typeof(T) == typeof(double)) return (double)(object)value;

            if (typeof(T) == typeof(ushort)) return System.Convert.ToDouble((ushort)(object)value);
            if (typeof(T) == typeof(uint)) return System.Convert.ToDouble((uint)(object)value);
            if (typeof(T) == typeof(ulong)) return System.Convert.ToDouble((ulong)(object)value);

            return 0;
        }

        #endregion
    }
}