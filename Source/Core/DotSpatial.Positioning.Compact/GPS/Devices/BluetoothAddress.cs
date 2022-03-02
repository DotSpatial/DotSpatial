using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
#if !PocketPC
using System.ComponentModel;
#endif

namespace DotSpatial.Positioning.Gps.IO
{
    /// <summary>
    /// Represents an address used to identify a unique Bluetooth device.
    /// </summary>
    /// <remarks>Each Bluetooth device has a unique address, in the form of a six-byte address.  </remarks>
    public struct BluetoothAddress : IFormattable, IEquatable<BluetoothAddress>
    {
        private readonly byte[] _Bytes;

        #region Constructors

        /// <summary>
        /// Creates a new instance from the specified byte array.
        /// </summary>
        /// <param name="address"></param>
        public BluetoothAddress(byte[] address)
        {
            if (address == null)
                throw new ArgumentNullException("address");
            if (address.Length != 8 && address.Length != 6)
            {
#if !PocketPC
                throw new ArgumentOutOfRangeException("address", address, "The length of a Bluetooth address must be 6 or 8 bytes.");
#else
                throw new ArgumentOutOfRangeException("address", "The length of a Bluetooth address must be 6 or 8 bytes.");
#endif
            }

            _Bytes = address;
        }

        /// <summary>
        /// Creates a new instance using the specified unsigned 64-bit integer.
        /// </summary>
        /// <param name="address"></param>
        [CLSCompliant(false)]
        public BluetoothAddress(ulong address)
        {
            _Bytes = BitConverter.GetBytes(address);
        }

        /// <summary>
        /// Creates a new instance using the specified 64-bit integer.
        /// </summary>
        /// <param name="address"></param>
        public BluetoothAddress(long address)
        {
            _Bytes = BitConverter.GetBytes(address);
        }

        /// <summary>
        /// Creates a new instance using the specified string.
        /// </summary>
        /// <param name="address"></param>
        public BluetoothAddress(string address)
        {
            string[] values = address.Split(':');

            if(values.Length != 6)
                throw new ArgumentException("When creating a BluetoothAddress object from a string, the string must be six bytes, in hexadecimal, separated by a colon (e.g. 00:00:00:00:00:00)");

            _Bytes = new byte[6];
            for (int index = 0; index < 6; index++)
                _Bytes[5 - index] = Convert.ToByte(values[index], 16);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns the bytes of the address.
        /// </summary>
#if !PocketPC
        [Category("Data")]
        [Description("Returns the bytes of the address.")]
        [Browsable(true)]
#endif
        public byte[] Address
        {
            get
            {
                return _Bytes;
            }
        }

        #endregion

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
            Array.Copy(_Bytes, bytes, 6);
            return BitConverter.ToInt64(bytes, 0);
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return _Bytes.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is BluetoothAddress)
                return Equals((BluetoothAddress)obj);
            return false;
        }

        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Converts the specified string into an address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static BluetoothAddress Parse(string address)
        {
            return new BluetoothAddress(address);
        }

        #endregion

        #region Conversions

        public static explicit operator BluetoothAddress(string address)
        {
            return new BluetoothAddress(address);
        }

        public static explicit operator BluetoothAddress(long address)
        {
            return new BluetoothAddress(address);
        }

        [CLSCompliant(false)]
        public static explicit operator BluetoothAddress(ulong address)
        {
            return new BluetoothAddress(address);
        }

        public static explicit operator string(BluetoothAddress address)
        {
            return address.ToString();
        }

        #endregion

        #region IFormattable Members

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
                result.Append(_Bytes[7].ToString("X2") + separator);
                result.Append(_Bytes[6].ToString("X2") + separator);
            }

            result.Append(_Bytes[5].ToString("X2") + separator);
            result.Append(_Bytes[4].ToString("X2") + separator);
            result.Append(_Bytes[3].ToString("X2") + separator);
            result.Append(_Bytes[2].ToString("X2") + separator);
            result.Append(_Bytes[1].ToString("X2") + separator);
            result.Append(_Bytes[0].ToString("X2"));

            return result.ToString();
        }

        #endregion

        #region IEquatable<BluetoothAddress> Members

        public bool Equals(BluetoothAddress other)
        {
            // compare the address byte-by-byte
            for (int index = 0; index < 6; index++)
                if (_Bytes[index] != other.Address[index])
                    return false;

            // If we get here, all bytes match
            return true;
        }

        #endregion
    }
}
