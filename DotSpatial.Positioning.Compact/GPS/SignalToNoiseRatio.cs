using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace DotSpatial.Positioning.Gps
{
    /// <summary>Represents a measurement of the strength of a received radio signal.</summary>
    /// <remarks>Signal-to-Noise Ratios (or SNR for short) measure the clarity of
    /// a received signal versus the level at which the signal is being obscured.  For 
    /// example, if a GPS device is operating in open sky on a clear day, no signals are
    /// obscured and the SNR will be high.  Conversely, if a device is activated in a room 
    /// with no view of the sky, signals may be obscured, resulting in a low SNR.  
    /// This class is used frequently by the <see cref="Satellite">Satellite</see>
    /// class to report on the quality of GPS satellite signals.  When three or more 
    /// satellite SNR's are strong, the device is able to obtain a fix on the current location.
    /// </remarks>
#if !PocketPC || DesignTime
    [TypeConverter("DotSpatial.Positioning.Design.SignalToNoiseRatioConverter, DotSpatial.Positioning.Gps.Design, Culture=neutral, Version=3.0.0.0, PublicKeyToken=3ed3cdf4fdda3400")]
#endif
    public struct SignalToNoiseRatio : IFormattable, IComparable<SignalToNoiseRatio>, IEquatable<SignalToNoiseRatio>, IEquatable<SignalToNoiseRatioRating>
    {
        private readonly int _Value;

        #region Fields

        /// <summary>Represents a value signifying a signal which is completely obscured.</summary>
        /// <remarks>If a GPS satellite reports an SNR of zero, it is likely below the
        /// horizon or completely blocked by an obstacle such as a building or tree.</remarks>
        public static readonly SignalToNoiseRatio NoSignal = new SignalToNoiseRatio(0);

        /// <summary>Represents a value of zero.</summary>
        /// <remarks>If a GPS satellite reports an SNR of zero, it is likely below the
        /// horizon or completely blocked by an obstacle such as a building or tree.</remarks>
        public static readonly SignalToNoiseRatio Empty = new SignalToNoiseRatio(0);

        /// <summary>Represents a value signifying a signal which is partially obscured.</summary>
        /// <remarks>GPS satellite signals with an SNR of 25 are not likely to become
        /// involved in a fix.</remarks>
        public static readonly SignalToNoiseRatio HalfSignal = new SignalToNoiseRatio(25);

        /// <summary>Represents a value signifying a signal which is not being obscured.</summary>
        /// <remarks>GPS satellite signals with an SNR of 50 are in direct line-of-sight
        /// and certain to become involved in obtaining a fix on the current location.</remarks>
        public static readonly SignalToNoiseRatio FullSignal = new SignalToNoiseRatio(50);

        #endregion

        #region  Constructors

        /// <summary>Creates a new instance.</summary>
        public SignalToNoiseRatio(int value)
        {
            _Value = value > 50 ? 50 : value;
        }

        /// <summary>Creates a new instance by parsing the specified string.</summary>
        public SignalToNoiseRatio(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <summary>Creates a new instance by parsing the specified string using a specific culture.</summary>
        public SignalToNoiseRatio(string value, CultureInfo culture)
        {
            // Remove the decibals "DB" 
            value = value.ToUpper(culture).Replace(" DB", string.Empty).Trim();

            // If there's a rating in there, get rid of it
            if (value.IndexOf("(", StringComparison.OrdinalIgnoreCase) != -1)
                value = value.Substring(0, value.IndexOf("(", StringComparison.OrdinalIgnoreCase) - 1).Trim();

            // The remaining text should be a number
            _Value = int.Parse(value, culture);

            _Value = _Value > 50 ? 50 : _Value;
        }

        #endregion

        #region Public Properties

        /// <summary>Returns the numeric value of the signal strength.</summary>
        /// <value>An <strong>Integer</strong> value between 0 and 50, where 0 represents no signal and fifty represents a signal at full strength.</value>
        /// <remarks>
        /// This property indicates the strength of a received satellite signal.  GPS 
        /// satellites always transmit signals at the same strength, but the signal is often
        /// obscured by the atmosphere, buildings and other obstacles such as trees.  A value
        /// must be at about 30 or greater for a satellite to be able to maintain a fix for long.
        /// In the <see cref="Satellite">Satellite</see> class, this property is updated automatically as new information is 
        /// received from the GPS device.
        /// </remarks>
        public int Value
        {
            get
            {
                return _Value;
            }
        }

        /// <summary>Indicates if the value is zero.</summary>
        public bool IsEmpty
        {
            get
            {
                return _Value == 0;
            }
        }
        
        /// <summary>Returns a subjective rating of the signal strength.</summary>
        /// <value>A value from the <strong>SignalToNoiseRatioRating</strong> enumeration.</value>
        /// <remarks>
        /// This property is frequently used to present an SNR reading in a readable format to the user.
        /// This property is updated automatically as new information is received from the GPS device.
        /// </remarks>
        /// <seealso cref="SignalToNoiseRatioRating">SignalToNoiseRatioRating Enumeration</seealso>
        public SignalToNoiseRatioRating Rating //'Implements ISignalToNoiseRatio.Rating
        {
            get
            {
                if (_Value > 0)
                {
                    if (_Value <= 15)
                    {
                        return SignalToNoiseRatioRating.Poor;
                    }
                    else if (_Value <= 30)
                    {
                        return SignalToNoiseRatioRating.Moderate;
                    }
                    else if (_Value <= 40)
                    {
                        return SignalToNoiseRatioRating.Good;
                    }
                    else
                    {
                        return SignalToNoiseRatioRating.Excellent;
                    }
                }
                else
                {
                    return SignalToNoiseRatioRating.None;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <overloads>Outputs the current instance as a formatted string.</overloads>
        public string ToString(string format)
        {
            return this.ToString(format, CultureInfo.CurrentCulture);
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj is SignalToNoiseRatio)
                return this.Equals((SignalToNoiseRatio)obj);
            else if (obj is SignalToNoiseRatioRating)
                return this.Equals((SignalToNoiseRatioRating)obj);
            return false;
        }

        /// <summary>Returns a unique code for the current instance used in hash tables.</summary>
        public override int GetHashCode()
        {
            return _Value.GetHashCode();
        }
        
        public override string ToString()
        {
            return this.ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Returns a <strong>SignalToNoiseRatio</strong> object containing a random
        /// value.
        /// </summary>
        public static SignalToNoiseRatio Random()
        {
            return new SignalToNoiseRatio((int)(new Random().NextDouble() * 50.0));
        }

        public static SignalToNoiseRatio Parse(string value)
        {
            return new SignalToNoiseRatio(value, CultureInfo.CurrentCulture);
        }

        public static SignalToNoiseRatio Parse(string value, CultureInfo culture)
        {
            return new SignalToNoiseRatio(value, culture);
        }
        
        #endregion

        #region IEquatable<SignalToNoiseRatio> Members

        public bool Equals(SignalToNoiseRatio other)
        {
            return _Value == other.Value;
        }

        #endregion

        #region IEquatable<SignalToNoiseRatioRating> Members

        public bool Equals(SignalToNoiseRatioRating value)
        {
            return (this.Rating == value);
        }

        #endregion

        #region IFormattable Members

        /// <summary>
        /// Outputs the current instance as a string using the specified format and
        /// culture.
        /// </summary>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider;

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            if (format == null || format.Length == 0)
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
            return _Value.ToString(format, formatProvider);
        }

        #endregion

        #region IComparable<SignalToNoiseRatio> Members

        public int CompareTo(SignalToNoiseRatio other)
        {
            return _Value.CompareTo(other.Value);
        }

        #endregion


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

        #endregion
    }

    /// <summary>Indicates an in-English description of the a satellite's radio signal strength</summary>
    /// <remarks>This enumeration is used by the <see cref="SignalToNoiseRatio.Rating">Rating</see> property of the 
    /// <see cref="SignalToNoiseRatio">SignalToNoiseRatio</see> class to give an in-English interpretation 
    /// of satellite radio signal strength.
    /// </remarks>
    public enum SignalToNoiseRatioRating : int
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

    public sealed class SignalToNoiseRatioEventArgs : EventArgs
    {

        private SignalToNoiseRatio _SignalToNoiseRatio;

        public SignalToNoiseRatioEventArgs(SignalToNoiseRatio signalToNoiseRatio)
        {
            _SignalToNoiseRatio = signalToNoiseRatio;
        }

        public SignalToNoiseRatio SignalToNoiseRatio 
        {
            get
            {
                return _SignalToNoiseRatio;
            }
        }
    }
}
