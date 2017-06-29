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
using System.ComponentModel;
using System.Globalization;

namespace DotSpatial.Positioning
{
#if !PocketPC || DesignTime
    /// <summary>
    /// Represents a measurement of the strength of a received radio signal.
    /// </summary>
    /// <remarks>Signal-to-Noise Ratios (or SNR for short) measure the clarity of
    /// a received signal versus the level at which the signal is being obscured.  For
    /// example, if a GPS device is operating in open sky on a clear day, no signals are
    /// obscured and the SNR will be high.  Conversely, if a device is activated in a room
    /// with no view of the sky, signals may be obscured, resulting in a low SNR.
    /// This class is used frequently by the <see cref="Satellite">Satellite</see>
    /// class to report on the quality of GPS satellite signals.  When three or more
    /// satellite SNR's are strong, the device is able to obtain a fix on the current location.</remarks>
    [TypeConverter("DotSpatial.Positioning.Design.SignalToNoiseRatioConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=1.0.0.0, PublicKeyToken=b4b0b185210c9dae")]
#endif
    public struct SignalToNoiseRatio : IFormattable, IComparable<SignalToNoiseRatio>, IEquatable<SignalToNoiseRatio>, IEquatable<SignalToNoiseRatioRating>
    {
        /// <summary>
        ///
        /// </summary>
        private readonly int _value;

        #region Fields

        /// <summary>
        /// Represents a value signifying a signal which is completely obscured.
        /// </summary>
        public static readonly SignalToNoiseRatio NoSignal = new SignalToNoiseRatio(0);

        /// <summary>
        /// Represents a value of zero.
        /// </summary>
        public static readonly SignalToNoiseRatio Empty = new SignalToNoiseRatio(0);

        /// <summary>
        /// Represents a value signifying a signal which is partially obscured.
        /// </summary>
        public static readonly SignalToNoiseRatio HalfSignal = new SignalToNoiseRatio(25);

        /// <summary>
        /// Represents a value signifying a signal which is not being obscured.
        /// </summary>
        public static readonly SignalToNoiseRatio FullSignal = new SignalToNoiseRatio(50);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="value">The value.</param>
        public SignalToNoiseRatio(int value)
        {
            _value = value > 50 ? 50 : value;
        }

        /// <summary>
        /// Creates a new instance by parsing the specified string.
        /// </summary>
        /// <param name="value">The value.</param>
        public SignalToNoiseRatio(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <summary>
        /// Creates a new instance by parsing the specified string using a specific culture.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        public SignalToNoiseRatio(string value, CultureInfo culture)
        {
            // Remove the decibals "DB"
            value = value.ToUpper(culture).Replace(" DB", string.Empty).Trim();

            // If there's a rating in there, get rid of it
            if (value.IndexOf("(", StringComparison.OrdinalIgnoreCase) != -1)
                value = value.Substring(0, value.IndexOf("(", StringComparison.OrdinalIgnoreCase) - 1).Trim();

            // The remaining text should be a number
            _value = int.Parse(value, culture);

            _value = _value > 50 ? 50 : _value;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Returns the numeric value of the signal strength.
        /// </summary>
        /// <value>An <strong>Integer</strong> value between 0 and 50, where 0 represents no signal and fifty represents a signal at full strength.</value>
        /// <remarks>This property indicates the strength of a received satellite signal.  GPS
        /// satellites always transmit signals at the same strength, but the signal is often
        /// obscured by the atmosphere, buildings and other obstacles such as trees.  A value
        /// must be at about 30 or greater for a satellite to be able to maintain a fix for long.
        /// In the <see cref="Satellite">Satellite</see> class, this property is updated automatically as new information is
        /// received from the GPS device.</remarks>
        public int Value
        {
            get
            {
                return _value;
            }
        }

        /// <summary>
        /// Indicates if the value is zero.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _value == 0;
            }
        }

        /// <summary>
        /// Returns a subjective rating of the signal strength.
        /// </summary>
        /// <value>A value from the <strong>SignalToNoiseRatioRating</strong> enumeration.</value>
        /// <seealso cref="SignalToNoiseRatioRating">SignalToNoiseRatioRating Enumeration</seealso>
        /// <remarks>This property is frequently used to present an SNR reading in a readable format to the user.
        /// This property is updated automatically as new information is received from the GPS device.</remarks>
        public SignalToNoiseRatioRating Rating //'Implements ISignalToNoiseRatio.Rating
        {
            get
            {
                if (_value > 0)
                {
                    if (_value <= 15)
                    {
                        return SignalToNoiseRatioRating.Poor;
                    }
                    if (_value <= 30)
                    {
                        return SignalToNoiseRatioRating.Moderate;
                    }
                    if (_value <= 40)
                    {
                        return SignalToNoiseRatioRating.Good;
                    }
                    return SignalToNoiseRatioRating.Excellent;
                }
                return SignalToNoiseRatioRating.None;
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        /// <overloads>Outputs the current instance as a formatted string.</overloads>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        #endregion Public Methods

        #region Overrides

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is SignalToNoiseRatio)
                return Equals((SignalToNoiseRatio)obj);
            if (obj is SignalToNoiseRatioRating)
                return Equals((SignalToNoiseRatioRating)obj);
            return false;
        }

        /// <summary>
        /// Returns a unique code for the current instance used in hash tables.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion Overrides

        #region Static Methods

        /// <summary>
        /// Returns a <strong>SignalToNoiseRatio</strong> object containing a random
        /// value.
        /// </summary>
        /// <returns></returns>
        public static SignalToNoiseRatio Random()
        {
            return new SignalToNoiseRatio((int)(new Random().NextDouble() * 50.0));
        }

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static SignalToNoiseRatio Parse(string value)
        {
            return new SignalToNoiseRatio(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static SignalToNoiseRatio Parse(string value, CultureInfo culture)
        {
            return new SignalToNoiseRatio(value, culture);
        }

        #endregion Static Methods

        #region IEquatable<SignalToNoiseRatio> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        public bool Equals(SignalToNoiseRatio other)
        {
            return _value == other.Value;
        }

        #endregion IEquatable<SignalToNoiseRatio> Members

        #region IEquatable<SignalToNoiseRatioRating> Members

        /// <summary>
        /// Equalses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Equals(SignalToNoiseRatioRating value)
        {
            return (Rating == value);
        }

        #endregion IEquatable<SignalToNoiseRatioRating> Members

        #region IFormattable Members

        /// <summary>
        /// Outputs the current instance as a string using the specified format and
        /// culture.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = formatProvider as CultureInfo ?? CultureInfo.CurrentCulture;

            if (string.IsNullOrEmpty(format))
                format = "G";

            // IF the format is "G", use the default
            if (format.Equals("G", StringComparison.OrdinalIgnoreCase))
                format = "0 dB (r)";

            // Convert the string to lower-case
            format = format.ToUpper(CultureInfo.InvariantCulture);

            // Replace "r" with the rating
            if (format.IndexOf("R", StringComparison.Ordinal) != -1)
                format = format.Replace("R", Rating.ToString());

            // And return the formatted value
            return _value.ToString(format, culture);
        }

        #endregion IFormattable Members

        #region IComparable<SignalToNoiseRatio> Members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This object is less than the <paramref name="other"/> parameter.
        /// Zero
        /// This object is equal to <paramref name="other"/>.
        /// Greater than zero
        /// This object is greater than <paramref name="other"/>.</returns>
        public int CompareTo(SignalToNoiseRatio other)
        {
            return _value.CompareTo(other.Value);
        }

        #endregion IComparable<SignalToNoiseRatio> Members

        #region Unused Code (Commented Out)

        /* FxCop says this is unused
             *
        internal void SetValue(int value)
        {
            if (_Value == value)
                return;
            if (value < 0 || value > 100)
#if PocketPC
				throw new ArgumentOutOfRangeException("Signal-to-noise ratio must be between 0 and 100.");
#else
                throw new ArgumentOutOfRangeException("value", value, "Signal-to-noise ratio must be between 0 and 100.");
#endif
            else
                _Value = value;
        }
             */

        #endregion Unused Code (Commented Out)
    }

    /// <summary>
    /// Indicates an in-English description of the a satellite's radio signal strength
    /// </summary>
    /// <remarks>This enumeration is used by the <see cref="SignalToNoiseRatio.Rating">Rating</see> property of the
    /// <see cref="SignalToNoiseRatio">SignalToNoiseRatio</see> class to give an in-English interpretation
    /// of satellite radio signal strength.</remarks>
    public enum SignalToNoiseRatioRating
    {
        /// <summary>Represents a value of 0. The radio signal is completely obscured.</summary>
        None,
        /// <summary>Represents a value between 1 and 15.  The radio signal is mostly obscured, such as by a building or tree, but might briefly be part of a fix.</summary>
        Poor,
        /// <summary>Represents a value between 16 and 30.  The radio signal is partially obscured, but could be part of a sustained fix.</summary>
        Moderate,
        /// <summary>Represents a value between 31 and 40.  The radio signal is being received with little interferance and could maintain a reliable fix.</summary>
        Good,
        /// <summary>Represents a value of 41 or above.  The satellite is in direct line of sight from the receiver and can sustain a fix.</summary>
        Excellent
    }

    /// <summary>
    /// Signal to noise ratio event args
    /// </summary>
    public sealed class SignalToNoiseRatioEventArgs : EventArgs
    {
        /// <summary>
        ///
        /// </summary>
        private readonly SignalToNoiseRatio _signalToNoiseRatio;

        /// <summary>
        /// Creates a new instance of the event args
        /// </summary>
        /// <param name="signalToNoiseRatio">The signal to noise ratio.</param>
        public SignalToNoiseRatioEventArgs(SignalToNoiseRatio signalToNoiseRatio)
        {
            _signalToNoiseRatio = signalToNoiseRatio;
        }

        /// <summary>
        /// The signal to noise ratio
        /// </summary>
        public SignalToNoiseRatio SignalToNoiseRatio
        {
            get
            {
                return _signalToNoiseRatio;
            }
        }
    }
}