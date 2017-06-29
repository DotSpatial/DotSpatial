using System;
using System.ComponentModel;
using System.Globalization;

namespace DotSpatial.Positioning.Gps
{
    /// <summary>
    /// Represents a confidence level in the precision of GPS data.
    /// </summary>
    /// <remarks><para>Dilution of Precision (or "DOP" for short) is a very important concept for GPS software
    /// developers.  When GPS devices calculate the current location on Earth's surface, inaccuracies
    /// can cause the calculated position to be incorrect by as much as an American football field!  To
    /// help minimize these effects, the GPS device also reports DOP values.  A low DOP value (such as 2)
    /// indicates excellent precision, whereas a high value (such as 50) indicates that precision is very 
    /// poor.</para>  
    /// <para>As a rule of thumb, a DOP value can be multiplied by the average precision of a GPS device
    /// to get a measurable amount of error.  Most consumer GPS devices are capable of about four to six meters
    /// of precision at their best, or as low as one to three meters if DGPS services such as WAAS or EGNOS are
    /// being utilized.  Or, an average of five meters without DGPS, and an average of two meters with DGPS.
    /// So, if the current Dilution of Precision is four, and WAAS is in effect, the current precision is
    /// "2 meters * four = 8 meters" of precision.</para>
    /// <para>GPS.NET includes features to help you monitor and control the precision of your GPS devices.  Properties
    /// such as <strong>MaximumHorizontalDilutionOfPrecision</strong> will cause a GPS.NET interpreter to throw out
    /// any real-time data until the DOP is at or below a specific number.  To help you determine this maximum number for
    /// your application, an article is available to help you.  You can find it online at the DotSpatial.Positioning web site here:
    /// http://dotspatial.codeplex.com/Articles/WritingApps2_1.aspx. </para>
    /// </remarks>
#if !PocketPC || DesignTime
    [TypeConverter("DotSpatial.Positioning.Design.DilutionOfPrecisionConverter, DotSpatial.Positioning.Gps.Design, Culture=neutral, Version=3.0.0.0, PublicKeyToken=3ed3cdf4fdda3400")]
#endif
    public struct DilutionOfPrecision : IFormattable, IComparable<DilutionOfPrecision>, IEquatable<DilutionOfPrecision>
    {
        #region Private members

        private readonly float _Value;

        #endregion

        #region Fields

        /// <summary>
        /// Represents the typical precision of a consumer-grade GPS device: six meters.
        /// </summary>
        public static readonly Distance TypicalPrecision = Distance.FromMeters(6);
        /// <summary>
        /// Represents the typical precision of a consumer-grade GPS device with WAAS, MSAS or EGNOS: two meters.
        /// </summary>
        public static readonly Distance TypicalPrecisionWithDgps = Distance.FromMeters(2);     
  
        /// <summary>Represents the worst possible DOP value of fifty.</summary>
        public static readonly DilutionOfPrecision Maximum = new DilutionOfPrecision(50.0f);

        /// <summary>Represents the best possible DOP value of one.</summary>
        public static readonly DilutionOfPrecision Minimum = new DilutionOfPrecision(1.0f);

        /// <summary>Represents a DOP reading signifying nearly-ideal precision.</summary>
        public static readonly DilutionOfPrecision Excellent = new DilutionOfPrecision(3.0f);

        /// <summary>Represents a DOP reading signifying inaccurate positional measurements.</summary>
        public static readonly DilutionOfPrecision Fair = new DilutionOfPrecision(20.0f);

        /// <summary>
        /// Represents a FOP reading signifying grossly inaccurate positional
        /// measurements.
        /// </summary>
        public static readonly DilutionOfPrecision Poor = new DilutionOfPrecision(50.0f);

        /// <summary>
        /// Represents a value of 1, where the GPS device is making the most accurate
        /// measurements possible.
        /// </summary>
        public static readonly DilutionOfPrecision Ideal = new DilutionOfPrecision(1.0f);

        /// <summary>Represents a DOP reading signifying inaccurate positional measurements.</summary>
        public static readonly DilutionOfPrecision Moderate = new DilutionOfPrecision(8.0f);

        /// <summary>
        /// Represents a DOP reading signifying fairly accurate positional
        /// measurements.
        /// </summary>
        public static readonly DilutionOfPrecision Good = new DilutionOfPrecision(6.0f);

        /// <summary>Represents an invalid or unspecified DOP value.</summary>
        public static readonly DilutionOfPrecision Invalid = new DilutionOfPrecision(float.NaN);

        #endregion

        #region Private Fields

        // Initalize the current average DOP based on a typical precision value, 
        // but not BEFORE the TypicalPrecision member has been initialized...
        private static Distance _CurrentAverageDevicePrecision = TypicalPrecision;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified value.
        /// </summary>
        /// <param name="value"></param>
        public DilutionOfPrecision(float value)
        {
            if (value <= 0)
                throw new ArgumentException("Dillution of precision value must be > 0");

            _Value = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Returns the numeric value of the rating.
        /// </summary>
        public float Value
        {
            get
            {
                return _Value;
            }
        }

        /// <summary>
        /// Returns whether the value is zero.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _Value.Equals(0.0f);
            }
        }

        /// <summary>
        /// Returns whether the value is invalid or unspecified.
        /// </summary>
        public bool IsInvalid
        {
            get
            {
                return float.IsNaN(_Value) || float.IsInfinity(_Value);
            }
        }

        /// <summary>
        /// Returns the estimated precision as a measurable distance.
        /// </summary>
        /// <remarks>
        /// The precision estimate is a product of this value and the value of the 
        /// CurrentAverageDevicePrecision static property.
        /// </remarks>
        public Distance EstimatedPrecision
        {
            get
            {
                return Distance.FromMeters(_CurrentAverageDevicePrecision.ToMeters().Value * _Value).ToLocalUnitType();
            }
        }

        ///// <summary>
        ///// Obsolete.  See compiler warnings for upgrade help.
        ///// </summary>
        ///// <returns></returns>
        //[Obsolete("Use the 'EstimatedPrecision' property to estimate precision as a measurable distance.")]
        //public Distance GetEstimatedError()
        //{ return EstimatedPrecision; }

        /// <summary>
        /// Returns a friendly name for the level of precision.
        /// </summary>
        public DilutionOfPrecisionRating Rating
        {
            get
            {
                // -Infinity --> 0.0
                if (_Value < 0.0f)
                {
                    return DilutionOfPrecisionRating.Unknown;
                }
                // 0.1 --> 1.0
                else if (_Value > 0.0f && _Value <= 1.0f)
                {
                    return DilutionOfPrecisionRating.Ideal;
                }
                // 1.1 --> 3.0
                else if (_Value <= 3.0f)
                {
                    return DilutionOfPrecisionRating.Excellent;
                }
                else if (_Value <= 6.0f)
                {
                    return DilutionOfPrecisionRating.Good;
                }
                else if (_Value <= 8.0f)
                {
                    return DilutionOfPrecisionRating.Moderate;
                }
                else if (_Value <= 20.0f)
                {
                    return DilutionOfPrecisionRating.Fair;
                }
                else
                {
                    return DilutionOfPrecisionRating.Poor;
                }
            }
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj is DilutionOfPrecision)
                return Equals((DilutionOfPrecision)obj);
            return false;
        }

        public override int GetHashCode()
        {
            return _Value.GetHashCode();
        }

        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Controls the estimated average precision possible by the current GPS device.
        /// </summary>
        /// <remarks>Most consumer GPS devces are capable of about six meters of precision without DGPS features
        /// such as WAAS or EGNOS.  When DGPS features are utilized, a typical cunsumer device is capable of about
        /// two meters of precision.  If you know of a specific amount for your device, you can set this property to
        /// assist GPS.NET in calculating the current estimated measurable amount of error in latitude/longitude reports.</remarks>
        public static Distance CurrentAverageDevicePrecision
        {
            get 
            { 
                return _CurrentAverageDevicePrecision; 
            }
            set
            {
                if (value.Value <= 0)
                    throw new ArgumentOutOfRangeException("CurrentAverageDevicePrecision", "The average precision of the current GPS device must be a value greater than zero.");

                _CurrentAverageDevicePrecision = value;
            }
        }

        #endregion

        #region IEquatable<DilutionOfPrecision> Members

        public bool Equals(DilutionOfPrecision other)
        {
            return _Value == other.Value;
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider;

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            if (format == null || format.Length == 0)
                format = "G";

            // If the format is "G", use the default format
            if(format.Equals("G", StringComparison.OrdinalIgnoreCase))
                format = "0.0 (R)";

            // Replace "R" with the rating
            format = format.Replace("R", Rating.ToString());

            // Now format the value
            return _Value.ToString(format, formatProvider);
        }

        #endregion

        #region IComparable<DilutionOfPrecision> Members

        public int CompareTo(DilutionOfPrecision other)
        {
            return _Value.CompareTo(other.Value);
        }

        #endregion
    }

    /// <summary>
    /// Indicates an interpretation of the accuracy of a measurement made by the GPS
    /// device.
    /// </summary>
    /// <remarks>
    /// This enumeration is used by the
    /// <see cref="DilutionOfPrecision.Rating">Rating</see> property of the
    /// <see cref="DilutionOfPrecision">
    /// DilutionOfPrecision</see> class. This interpretation is subject to discussion as to
    /// what precisely constitutes a "good" versus "bad" DOP. Use your own best judgement based
    /// on the needs of your application. Generally speaking, a DOP of six or less is
    /// recommended for en-route navigation, and three or less for highly-precise measurements.
    /// A rating of <strong>Moderate</strong> corresponds with six or better, a rating of
    /// <strong>Excellent</strong> means three or less, and a rating of <strong>Ideal</strong>
    /// means the best value of one.
    /// </remarks>
    public enum DilutionOfPrecisionRating : int
    {
        /// <summary>The rating is unknown or not yet available.</summary>
        Unknown = 0,
        /// <summary>
        /// Represents a value of 1, where the GPS device is making the most accurate
        /// measurements possible.
        /// </summary>
        Ideal = 1,
        /// <summary>
        /// The GPS device is making high-quality measurements, good enough for applications
        /// requiring higher levels of precision. Represents a value of 2 or 3.
        /// </summary>
        Excellent = 3,
        /// <summary>
        /// The GPS device is making measurements accurate enough for en-route navigation.
        /// Represents a value between 4 and 6.
        /// </summary>
        Good = 6,
        /// <summary>
        /// The GPS device is making measurements good enough to indicate the approximate
        /// location, but should be ignored by applications requiring high accuracy. Represents a
        /// value of 7 or 8.
        /// </summary>
        Moderate = 8,
        /// <summary>
        /// The GPS device is making measurements with an accuracy which should only be used
        /// to indicate the approximate location, but is not accurate enough for en-route
        /// navigation. Represents a value between 9 and 20.
        /// </summary>
        Fair = 20,
        /// <summary>
        /// The GPS device is calculating the current location, but the accuracy is extremely
        /// low and may be off by a factor of fifty. Represents a value between 21 and the maximum
        /// possible of 50.
        /// </summary>
        Poor = 50
    }

    /// <summary>Represents information about a DOP measurement when an DOP-related event is raised.</summary>
    /// <remarks>This class is typically used for events in the <see cref="DilutionOfPrecision">DilutionOfPrecision</see> class to 
    /// provide notification when hours, minutes, decimal minutes or seconds properties have changed.</remarks>
    /// <example>This example demonstrates how to use this class when raising an event.
    /// <code lang="VB">
    /// ' Declare a new event
    /// Dim MyDilutionOfPrecisionEvent As EventHandler
    /// ' Create a DilutionOfPrecision of 30 horizontal
    /// Dim MyDilutionOfPrecision As New DilutionOfPrecision(DilutionOfPrecisionType.Horizontal, 30.0)
    /// 
    /// Sub Main()
    ///   ' Raise our custom event
    ///   RaiseEvent MyDilutionOfPrecisionEvent(Me, New DilutionOfPrecisionEventArgs(MyDilutionOfPrecision))
    /// End Sub
    /// </code>
    /// <code lang="C#">
    /// // Declare a new event
    /// EventHandler MyDilutionOfPrecisionEvent;
    /// // Create a DilutionOfPrecision of 30 horizontal
    /// DilutionOfPrecision MyDilutionOfPrecision = new DilutionOfPrecision(DilutionOfPrecisionType.Horizontal, 30.0);
    /// 
    /// void Main()
    /// {
    ///   // Raise our custom event
    ///   MyDilutionOfPrecisionEvent(this, New DilutionOfPrecisionEventArgs(MyDilutionOfPrecision));
    /// }
    /// </code>
    /// </example>
    /// <seealso cref="DilutionOfPrecision">DilutionOfPrecision Class</seealso>
    /// <seealso cref="EventHandler">EventHandler Delegate</seealso>
    public sealed class DilutionOfPrecisionEventArgs : EventArgs
    {

        private DilutionOfPrecision _DilutionOfPrecision;

        /// <summary>
        /// Creates a new instance with the specified DOP measurement.
        /// </summary>
        /// <param name="DilutionOfPrecision">A DilutionOfPrecision object which is the target of the event.</param>

        public DilutionOfPrecisionEventArgs(DilutionOfPrecision dilutionOfPrecision)
        {
            _DilutionOfPrecision = dilutionOfPrecision;
        }

        /// <summary>
        /// A DilutionOfPrecision object which is the target of the event.
        /// </summary>
        /// <value></value>

        public DilutionOfPrecision DilutionOfPrecision
        {
            get
            {
                return _DilutionOfPrecision;
            }
        }
    }
}
