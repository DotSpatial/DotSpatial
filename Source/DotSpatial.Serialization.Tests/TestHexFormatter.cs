// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Globalization;

namespace DotSpatial.Serialization.Tests
{
    /// <summary>
    /// TestHexFormatter
    /// </summary>
    public class TestHexFormatter : SerializationFormatter
    {
        #region Methods

        /// <summary>
        /// Converts the given string to a hex number.
        /// </summary>
        /// <param name="value">String that gets converted.</param>
        /// <returns>The resulting value.</returns>
        public override object FromString(string value)
        {
            if (value.StartsWith("0x")) value = value.Substring(2);

            return int.Parse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the given number to a hex string.
        /// </summary>
        /// <param name="value">The integer that gets converted.</param>
        /// <returns>The resulting string.</returns>
        public override string ToString(object value)
        {
            if (value.GetType() != typeof(int)) throw new NotSupportedException("Only integers are supported by this formatter");

            return "0x" + ((int)value).ToString("X", CultureInfo.InvariantCulture);
        }

        #endregion
    }
}