// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents an address used to identify a unique Bluetooth device.
    /// </summary>
    /// <remarks>Each Bluetooth device has a unique address, in the form of a six-byte address.</remarks>
    public struct BluetoothAddress : IFormattable, IEquatable<BluetoothAddress>
    {

        #region Constructors

        /// <summary>
        /// Creates a new instance from the specified byte array.
        /// </summary>
        /// <param name="address">The address.</param>
        public BluetoothAddress(byte[] address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (address.Length is not 8 and not 6)
            {
                throw new ArgumentOutOfRangeException(nameof(address), "The length of a Bluetooth address must be 6 or 8 bytes.");
            }

            Address = address;
        }

        /// <summary>
        /// Creates a new instance using the specified unsigned 64-bit integer.
        /// </summary>
        /// <param name="address">The address.</param>
        public BluetoothAddress(ulong address)
        {
            Address = BitConverter.GetBytes(address);
        }

        /// <summary>
        /// Creates a new instance using the specified 64-bit integer.
        /// </summary>
        /// <param name="address">The address.</param>
        public BluetoothAddress(long address)
        {
            Address = BitConverter.GetBytes(address);
        }

        /// <summary>
        /// Creates a new instance using the specified string.
        /// </summary>
        /// <param name="address">The address.</param>
        public BluetoothAddress(string address)
        {
            string[] values = address.Split(':');

            if (values.Length != 6)
            {
                throw new ArgumentException("When creating a BluetoothAddress object from a string, the string must be six bytes, in hexadecimal, separated by a colon (e.g. 00:00:00:00:00:00)");
            }

            Address = new byte[6];
            for (int index = 0; index < 6; index++)
            {
                Address[5 - index] = Convert.ToByte(values[index], 16);
            }
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Returns the bytes of the address.
        /// </summary>
        [Category("Data")]
        [Description("Returns the bytes of the address.")]
        [Browsable(true)]

        public byte[] Address { get; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns the address as a 64-bit integer.
        /// </summary>
        /// <returns></returns>
        public long ToInt64()
        {
            /* Bluetooth addresses are 6 bytes, but are typically passed as a long (e.g. two extra bytes).
             * So, make an 8-byte array.
             */
            byte[] bytes = new byte[8];
            Array.Copy(Address, bytes, 6);
            return BitConverter.ToInt64(bytes, 0);
        }

        #endregion Public Methods

        #region Overrides

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Address.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is BluetoothAddress address)
            {
                return Equals(address);
            }

            return false;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        #endregion Overrides

        #region Static Methods

        /// <summary>
        /// Converts the specified string into an address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static BluetoothAddress Parse(string address)
        {
            return new BluetoothAddress(address);
        }

        #endregion Static Methods

        #region Conversions

        /// <summary>
        /// Bluetooth Address
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator BluetoothAddress(string address)
        {
            return new BluetoothAddress(address);
        }

        /// <summary>
        /// Bluetooth address
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator BluetoothAddress(long address)
        {
            return new BluetoothAddress(address);
        }

        /// <summary>
        /// Bluetooth address
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator BluetoothAddress(ulong address)
        {
            return new BluetoothAddress(address);
        }

        /// <summary>
        /// Bluetooth address
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator string(BluetoothAddress address)
        {
            return address.ToString();
        }

        #endregion Conversions

        #region IFormattable Members

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            string separator;

            if (string.IsNullOrEmpty(format))
            {
                separator = string.Empty;
            }
            else
            {
                separator = format.ToUpper(CultureInfo.InvariantCulture) switch
                {
                    "8" or "N" => string.Empty,
                    "G" or "C" => ":",
                    "P" => ".",
                    _ => throw new FormatException("Invalid format specified - must be either \"N\", \"C\", \"P\", \"\" or null."),
                };
            }

            StringBuilder result = new(18);

            if (format == "8")
            {
                result.Append(Address[7].ToString("X2") + separator);
                result.Append(Address[6].ToString("X2") + separator);
            }

            result.Append(Address[5].ToString("X2") + separator);
            result.Append(Address[4].ToString("X2") + separator);
            result.Append(Address[3].ToString("X2") + separator);
            result.Append(Address[2].ToString("X2") + separator);
            result.Append(Address[1].ToString("X2") + separator);
            result.Append(Address[0].ToString("X2"));

            return result.ToString();
        }

        #endregion IFormattable Members

        #region IEquatable<BluetoothAddress> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        public bool Equals(BluetoothAddress other)
        {
            // compare the address byte-by-byte
            for (int index = 0; index < 6; index++)
            {
                if (Address[index] != other.Address[index])
                {
                    return false;
                }
            }

            // If we get here, all bytes match
            return true;
        }

        /// <summary>
        /// Indicates whether both objects are equal.
        /// </summary>
        /// <param name="left">The left object to compare.</param>
        /// <param name="right">The right object to compare.</param>
        /// <returns>true if both objects are equal.</returns>
        public static bool operator ==(BluetoothAddress left, BluetoothAddress right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Indicates whether both objects are not equal.
        /// </summary>
        /// <param name="left">The left object to compare.</param>
        /// <param name="right">The right object to compare.</param>
        /// <returns>true if both objects are not equal.</returns>
        public static bool operator !=(BluetoothAddress left, BluetoothAddress right)
        {
            return !(left == right);
        }

        #endregion IEquatable<BluetoothAddress> Members
    }
}