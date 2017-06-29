// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from http://gps3.codeplex.com/ version 3.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GPS.Net 3.0
// | Shade1974 (Ted Dunsford) | 10/22/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************
using System;
using System.Globalization;
using System.Text;

#if !PocketPC

using System.ComponentModel;

#endif

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents an address used to identify a unique Bluetooth device.
    /// </summary>
    /// <remarks>Each Bluetooth device has a unique address, in the form of a six-byte address.</remarks>
    public struct BluetoothAddress : IFormattable, IEquatable<BluetoothAddress>
    {
        /// <summary>
        ///
        /// </summary>
        private readonly byte[] _bytes;

        #region Constructors

        /// <summary>
        /// Creates a new instance from the specified byte array.
        /// </summary>
        /// <param name="address">The address.</param>
        public BluetoothAddress(byte[] address)
        {
            if (address == null)
                throw new ArgumentNullException("address");
            if (address.Length != 8 && address.Length != 6)
            {
                throw new ArgumentOutOfRangeException("address", "The length of a Bluetooth address must be 6 or 8 bytes.");
            }
            _bytes = address;
        }

        /// <summary>
        /// Creates a new instance using the specified unsigned 64-bit integer.
        /// </summary>
        /// <param name="address">The address.</param>
        [CLSCompliant(false)]
        public BluetoothAddress(ulong address)
        {
            _bytes = BitConverter.GetBytes(address);
        }

        /// <summary>
        /// Creates a new instance using the specified 64-bit integer.
        /// </summary>
        /// <param name="address">The address.</param>
        public BluetoothAddress(long address)
        {
            _bytes = BitConverter.GetBytes(address);
        }

        /// <summary>
        /// Creates a new instance using the specified string.
        /// </summary>
        /// <param name="address">The address.</param>
        public BluetoothAddress(string address)
        {
            string[] values = address.Split(':');

            if (values.Length != 6)
                throw new ArgumentException("When creating a BluetoothAddress object from a string, the string must be six bytes, in hexadecimal, separated by a colon (e.g. 00:00:00:00:00:00)");

            _bytes = new byte[6];
            for (int index = 0; index < 6; index++)
                _bytes[5 - index] = Convert.ToByte(values[index], 16);
        }

        #endregion Constructors

        #region Public Properties

#if !PocketPC

        /// <summary>
        /// Returns the bytes of the address.
        /// </summary>
        [Category("Data")]
        [Description("Returns the bytes of the address.")]
        [Browsable(true)]
#endif
        public byte[] Address
        {
            get
            {
                return _bytes;
            }
        }

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
            Array.Copy(_bytes, bytes, 6);
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
            return _bytes.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is BluetoothAddress)
                return Equals((BluetoothAddress)obj);
            return false;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
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
        [CLSCompliant(false)]
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
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            string separator;

            if (string.IsNullOrEmpty(format))
            {
                separator = string.Empty;
            }
            else
            {
                switch (format.ToUpper(CultureInfo.InvariantCulture))
                {
                    case "8":
                    case "N":
                        separator = string.Empty;
                        break;
                    case "G":
                    case "C":
                        separator = ":";
                        break;
                    case "P":
                        separator = ".";
                        break;
                    default:
                        throw new FormatException("Invalid format specified - must be either \"N\", \"C\", \"P\", \"\" or null.");
                }
            }

            StringBuilder result = new StringBuilder(18);

            if (format == "8")
            {
                result.Append(_bytes[7].ToString("X2") + separator);
                result.Append(_bytes[6].ToString("X2") + separator);
            }

            result.Append(_bytes[5].ToString("X2") + separator);
            result.Append(_bytes[4].ToString("X2") + separator);
            result.Append(_bytes[3].ToString("X2") + separator);
            result.Append(_bytes[2].ToString("X2") + separator);
            result.Append(_bytes[1].ToString("X2") + separator);
            result.Append(_bytes[0].ToString("X2"));

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
                if (_bytes[index] != other.Address[index])
                    return false;

            // If we get here, all bytes match
            return true;
        }

        #endregion IEquatable<BluetoothAddress> Members
    }
}