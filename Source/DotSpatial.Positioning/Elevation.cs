// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
//
// The Original Code is from http://geoframework.codeplex.com/ version 2.0
//
// The Initial Developer of this original code is Jon Pearson. Submitted Oct. 21, 2010 by Ben Tombs (tidyup)
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// -------------------------------------------------------------------------------------------------------
// |    Developer             |    Date    |                             Comments
// |--------------------------|------------|--------------------------------------------------------------
// | Tidyup  (Ben Tombs)      | 10/21/2010 | Original copy submitted from modified GeoFrameworks 2.0
// | Shade1974 (Ted Dunsford) | 10/21/2010 | Added file headers reviewed formatting with resharper.
// ********************************************************************************************************

using System;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using DotSpatial.Positioning.Properties;
#if !PocketPC || DesignTime

using System.ComponentModel;

#endif

namespace DotSpatial.Positioning
{
#if !PocketPC || DesignTime
    /// <summary>
    /// Represents a vertical angular measurement between -90° and 90°.
    /// </summary>
    /// <example>
    /// These examples create new instances of <strong>Elevation</strong> objects.
    ///   </example>
    ///
    /// <seealso cref="Angle">Angle Class</seealso>
    ///
    /// <seealso cref="Azimuth">Azimuth Class</seealso>
    ///
    /// <seealso cref="Latitude">Latitude Class</seealso>
    ///
    /// <seealso cref="Longitude">Longitude Class</seealso>
    ///
    /// <example>
    /// These examples create new instances of Elevation objects.
    ///   <code lang="VB" description="Create an angle of 90°">
    /// Dim MyElevation As New Elevation(90)
    ///   </code>
    ///   <code lang="CS" description="Create an angle of 90°">
    /// Elevation MyElevation = new Elevation(90);
    ///   </code>
    ///   <code lang="C++" description="Create an angle of 90°">
    /// Elevation MyElevation = new Elevation(90);
    ///   </code>
    ///   <code lang="VB" description="Create an angle of 105°30'21.4">
    /// Dim MyElevation1 As New Elevation(105, 30, 21.4)
    ///   </code>
    ///   <code lang="CS" description="Create an angle of 105°30'21.4">
    /// Elevation MyElevation = new Elevation(105, 30, 21.4);
    ///   </code>
    ///   <code lang="C++" description="Create an angle of 105°30'21.4">
    /// Elevation MyElevation = new Elevation(105, 30, 21.4);
    ///   </code>
    ///   </example>
    /// <remarks>This class is used to indicate a vertical angle where 90° represents a point
    /// directly overhead, 0° represents the horizon (striaght ahead), and -90° represents a
    /// point straight down. This class is typically combined with an <strong>Elevation</strong>
    /// object (which measures a horizontal angle) to form a three-dimensional direction to an
    /// object in space, such as a GPS satellite.</remarks>
    [TypeConverter("DotSpatial.Positioning.Design.ElevationConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=1.0.0.0, PublicKeyToken=b4b0b185210c9dae")]
#endif
    public struct Elevation : IFormattable, IComparable<Elevation>, IEquatable<Elevation>, ICloneable<Elevation>, IXmlSerializable
    {
        /// <summary>
        ///
        /// </summary>
        private double _decimalDegrees;

        #region Constants

        /// <summary>
        ///
        /// </summary>
        private const int MAXIMUM_PRECISION_DIGITS = 13;

        #endregion Constants

        #region Fields

        /// <summary>
        /// Represents the minimum value of an angle in one turn of a circle.
        /// </summary>
        /// <example>
        /// This example creates an angle representing the minimum allowed value.
        ///   <code lang="VB">
        /// Dim MyElevation As Elevation = Elevation.Minimum
        ///   </code>
        ///   <code lang="CS">
        /// Elevation MyElevation = Elevation.Minimum;
        ///   </code>
        ///   <code lang="C++">
        /// Elevation MyElevation = Elevation.Minimum;
        ///   </code>
        ///   </example>
        ///
        /// <value>An Elevation with a value of -359.999999°.</value>
        public static readonly Elevation Minimum = new Elevation(-90);
        /// <summary>
        /// Represents an angle with no value.
        /// </summary>
        /// <value>An Elevation containing a value of zero (0°).</value>
        ///
        /// <seealso cref="IsEmpty">IsEmpty Property</seealso>
        public static readonly Elevation Empty = new Elevation(0.0);
        /// <summary>
        /// Represents an angle with infinite value.
        /// </summary>
        public static readonly Elevation Infinity = new Elevation(double.PositiveInfinity);
        /// <summary>
        /// Represents an invalid or unspecified value.
        /// </summary>
        public static readonly Elevation Invalid = new Elevation(double.NaN);
        /// <summary>
        /// Represents the maximum value of an angle in one turn of a circle.
        /// </summary>
        /// <example>
        /// This example creates an angle representing the maximum allowed value of 359.9999°.
        ///   <code lang="VB">
        /// Dim MyElevation As Elevation = Elevation.Maximum
        ///   </code>
        ///   <code lang="CS">
        /// Elevation MyElevation = Elevation.Maximum;
        ///   </code>
        ///   </example>
        public static readonly Elevation Maximum = new Elevation(90.0);
        /// <summary>
        /// Represents the point directly overhead.
        /// </summary>
        /// <value>An <strong>Elevation</strong> object.</value>
        public static readonly Elevation Zenith = new Elevation(90.0);
        /// <summary>
        /// Represents a vertical direction halfway up from the horizon.
        /// </summary>
        /// <value>An <strong>Elevation</strong> object.</value>
        public static readonly Elevation HalfwayAboveHorizon = new Elevation(45.0);
        /// <summary>
        /// Represents a vertical direction halfway below the horizon.
        /// </summary>
        /// <value>An <strong>Elevation</strong> object.</value>
        public static readonly Elevation HalfwayBelowHorizon = new Elevation(-45.0);
        /// <summary>
        ///
        /// </summary>
        /// <value>An <strong>Elevation</strong> object.</value>
        public static readonly Elevation Horizon = new Elevation(0.0);
        /// <summary>
        /// Represents the point directly below one's feet.
        /// </summary>
        /// <value>An <strong>Elevation</strong> object.</value>
        public static readonly Elevation Nadir = new Elevation(-90.0);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance with the specified decimal degrees.
        /// </summary>
        /// <param name="decimalDegrees">The decimal degrees.</param>
        /// <example>
        /// This example demonstrates how to create an angle with a measurement of 90°.
        ///   <code lang="VB">
        /// Dim MyElevation As New Elevation(90)
        ///   </code>
        ///   <code lang="CS">
        /// Elevation MyElevation = new Elevation(90);
        ///   </code>
        ///   </example>
        ///
        /// <returns>An <strong>Elevation</strong> containing the specified value.</returns>
        public Elevation(double decimalDegrees)
        {
            // Set the decimal degrees value
            _decimalDegrees = decimalDegrees;
        }

        /// <summary>
        /// Creates a new instance with the specified degrees.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <returns>An <strong>Elevation</strong> containing the specified value.</returns>
        public Elevation(int hours)
        {
            _decimalDegrees = ToDecimalDegrees(hours);
        }

        /// <summary>
        /// Creates a new instance with the specified hours and decimal minutes.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <param name="decimalMinutes">The decimal minutes.</param>
        /// <example>
        /// This example demonstrates how an angle can be created when only the hours and
        /// minutes (in decimal form) are known. This creates a value of 12°42.345'.
        ///   <code lang="VB">
        /// Dim MyElevation As New Elevation(12, 42.345)
        ///   </code>
        ///   <code lang="VB">
        /// Elevation MyElevation = new Elevation(12, 42.345);
        ///   </code>
        ///   </example>
        /// <remarks>An <strong>Elevation</strong> containing the specified value.</remarks>
        public Elevation(int hours, double decimalMinutes)
        {
            _decimalDegrees = ToDecimalDegrees(hours, decimalMinutes);
        }

        /// <summary>
        /// Creates a new instance with the specified hours, minutes and
        /// seconds.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <example>
        /// This example demonstrates how to create an angular measurement of 34°12'29.2 in
        /// hours, minutes and seconds.
        ///   <code lang="VB">
        /// Dim MyElevation As New Elevation(34, 12, 29.2)
        ///   </code>
        ///   <code lang="CS">
        /// Elevation MyElevation = new Elevation(34, 12, 29.2);
        ///   </code>
        ///   </example>
        ///
        /// <returns>An <strong>Elevation</strong> containing the specified value.</returns>
        public Elevation(int hours, int minutes, double seconds)
        {
            _decimalDegrees = ToDecimalDegrees(hours, minutes, seconds);
        }

        /// <summary>
        /// Creates a new instance by converting the specified string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <seealso cref="Elevation.Parse(string, CultureInfo)">Parse Method</seealso>
        ///
        /// <example>
        /// This example creates a new instance by parsing a string. (notice The double-quote is
        /// doubled up to represent a single double-quote in the string.)
        ///   <code lang="VB">
        /// Dim MyElevation As New Elevation("123°45'67.8""")
        ///   </code>
        ///   <code lang="CS">
        /// Elevation MyElevation = new Elevation("123°45'67.8\"");
        ///   </code>
        ///   </example>
        ///
        /// <returns>An <strong>Elevation</strong> containing the specified value.</returns>
        ///
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">The Parse method requires a decimal or sexagesimal measurement.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">Only the right-most portion of a sexagesimal measurement can be a fractional value.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">Extra characters were encountered while parsing an angular measurement.  Only hours, minutes, and seconds are allowed.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">The specified text was not fully understood as an angular measurement.</exception>
        /// <remarks>This constructor parses the specified string into an <strong>Elevation</strong>
        /// object using the current culture. This constructor can parse any strings created via
        /// the <strong>ToString</strong> method.</remarks>
        public Elevation(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <summary>
        /// Creates a new instance by converting the specified string using the specified
        /// culture.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">The Parse method requires a decimal or sexagesimal measurement.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">Only the right-most portion of a sexagesimal measurement can be a fractional value.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">Extra characters were encountered while parsing an angular measurement.  Only hours, minutes, and seconds are allowed.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">The specified text was not fully understood as an angular measurement.</exception>
        /// <remarks>This constructor parses the specified string into an <strong>Elevation</strong>
        /// object using a specific culture. This constructor can parse any strings created via the
        /// <strong>ToString</strong> method.</remarks>
        public Elevation(string value, CultureInfo culture)
        {
            // Is the value null or empty?
            if (string.IsNullOrEmpty(value))
            {
                // Yes. Set to zero
                _decimalDegrees = 0;
                return;
            }

            // Default to the current culture
            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            // Yes. First, clean up the strings
            try
            {
                // Clean up the string
                StringBuilder newValue = new StringBuilder(value);
                newValue.Replace("°", " ").Replace("'", " ").Replace("\"", " ").Replace("  ", " ");
                // Now split the values into an array
                string[] values = newValue.ToString().Trim().Split(' ');
                // How many elements are in the array?
                switch (values.Length)
                {
                    case 0:
                        // Return a blank Elevation
                        _decimalDegrees = 0.0;
                        return;
                    case 1: // Decimal degrees
                        // Is it infinity?
                        if (String.Compare(values[0], Resources.Common_Infinity, true, culture) == 0)
                        {
                            _decimalDegrees = double.PositiveInfinity;
                            return;
                        }
                        // Is it empty?
                        if (String.Compare(values[0], Resources.Common_Empty, true, culture) == 0)
                        {
                            _decimalDegrees = 0.0;
                            return;
                        }
                        // Look at the number of digits, this might be HHHMMSS format.
                        if (values[0].Length == 7 && values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) == -1)
                        {
                            _decimalDegrees = ToDecimalDegrees(
                                int.Parse(values[0].Substring(0, 3), culture),
                                int.Parse(values[0].Substring(3, 2), culture),
                                double.Parse(values[0].Substring(5, 2), culture));
                            return;
                        }
                        if (values[0].Length == 8 && values[0][0] == '-' && values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) == -1)
                        {
                            _decimalDegrees = ToDecimalDegrees(
                                int.Parse(values[0].Substring(0, 4), culture),
                                int.Parse(values[0].Substring(4, 2), culture),
                                double.Parse(values[0].Substring(6, 2), culture));
                            return;
                        }
                        _decimalDegrees = double.Parse(values[0], culture);
                        return;
                    case 2: // Hours and decimal minutes
                        // If this is a fractional value, remember that it is
                        if (values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) != -1)
                        {
                            throw new ArgumentException(Resources.Angle_OnlyRightmostIsDecimal, "value");
                        }

                        // Set decimal degrees
                        _decimalDegrees = ToDecimalDegrees(
                            int.Parse(values[0], culture),
                            float.Parse(values[1], culture));
                        return;
                    default: // Hours, minutes and seconds  (most likely)
                        // If this is a fractional value, remember that it is
                        if (values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) != -1 || values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) != -1)
                        {
                            throw new ArgumentException(Resources.Angle_OnlyRightmostIsDecimal, "value");
                        }

                        // Set decimal degrees
                        _decimalDegrees = ToDecimalDegrees(
                            int.Parse(values[0], culture),
                            int.Parse(values[1], culture),
                            double.Parse(values[2], culture));
                        return;
                }
            }
            catch (Exception ex)
            {
#if PocketPC
                throw new ArgumentException(Properties.Resources.Angle_InvalidFormat, ex);
#else
                throw new ArgumentException(Resources.Angle_InvalidFormat, "value", ex);
#endif
            }
        }

        /// <summary>
        /// Creates a new instance by deserializing the specified XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public Elevation(XmlReader reader)
        {
            // Initialize all fields
            _decimalDegrees = Double.NaN;

            // Deserialize the object from XML
            ReadXml(reader);
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Returns the value of the angle as decimal degrees.
        /// </summary>
        /// <value>A <strong>Double</strong> value.</value>
        /// <seealso cref="Hours">Hours Property</seealso>
        ///
        /// <seealso cref="Minutes">Minutes Property</seealso>
        ///
        /// <seealso cref="Seconds">Seconds Property</seealso>
        ///
        /// <example>
        /// This example demonstrates how the
        ///   <see cref="DecimalDegrees"><strong>DecimalDegrees</strong></see> property is
        /// calculated automatically when creating an angle using hours, minutes and seconds.
        ///   <code lang="VB">
        /// ' Create an angle of 20°30'
        /// Dim MyElevation As New Elevation(20, 30)
        /// ' Setting the DecimalMinutes recalculated other properties
        /// Debug.WriteLine(MyElevation.DecimalDegrees)
        /// ' Output: "20.5"  the same as 20°30'
        ///   </code>
        ///   <code lang="CS">
        /// // Create an angle of 20°30'
        /// Elevation MyElevation = New Elevation(20, 30);
        /// // Setting the DecimalMinutes recalculated other properties
        /// Console.WriteLine(MyElevation.DecimalDegrees)
        /// // Output: "20.5"  the same as 20°30'
        ///   </code>
        ///   </example>
        /// <remarks>This property returns the value of the angle as a single number.</remarks>
        public double DecimalDegrees
        {
            get
            {
                return _decimalDegrees;
            }
        }

        /// <summary>
        /// Returns the minutes and seconds as a single numeric value.
        /// </summary>
        /// <value>A <strong>Double</strong> value.</value>
        /// <seealso cref="Minutes">Minutes Property</seealso>
        ///
        /// <seealso cref="DecimalDegrees">DecimalDegrees Property</seealso>
        ///
        /// <example>
        /// This example demonstrates how the <strong>DecimalMinutes</strong> property is
        /// automatically calculated when creating a new angle.
        ///   <code lang="VB">
        /// ' Create an angle of 20°10'30"
        /// Dim MyElevation As New Elevation(20, 10, 30)
        /// ' The DecimalMinutes property is automatically calculated
        /// Debug.WriteLine(MyElevation.DecimalMinutes)
        /// ' Output: "10.5"
        ///   </code>
        ///   <code lang="CS">
        /// // Create an angle of 20°10'30"
        /// Elevation MyElevation = new Elevation(20, 10, 30);
        /// // The DecimalMinutes property is automatically calculated
        /// Console.WriteLine(MyElevation.DecimalMinutes)
        /// // Output: "10.5"
        ///   </code>
        ///   </example>
        /// <remarks>This property is used when minutes and seconds are represented as a single
        /// decimal value.</remarks>
        public double DecimalMinutes
        {
            get
            {
#if Framework20 && !PocketPC
                return Math.Round(
                    (Math.Abs(
                        _decimalDegrees - Math.Truncate(_decimalDegrees)) * 60.0),
                    // Apparently we must round to two less places to preserve accuracy
                        MAXIMUM_PRECISION_DIGITS - 2);
#else
                return Math.Round(
                    (Math.Abs(
                        _DecimalDegrees - Truncate(_DecimalDegrees)) * 60.0), MaximumPrecisionDigits - 2);
#endif
            }
        }

        /// <summary>
        /// Returns the integer hours (degrees) portion of an angular
        /// measurement.
        /// </summary>
        /// <value>An <strong>Integer</strong> value.</value>
        /// <seealso cref="Minutes">Minutes Property</seealso>
        ///
        /// <seealso cref="Seconds">Seconds Property</seealso>
        ///
        /// <example>
        /// This example creates an angle of 60.5° then outputs the value of the
        ///   <strong>Hours</strong> property, 60.
        ///   <code lang="VB">
        /// Dim MyElevation As New Elevation(60.5)
        /// Debug.WriteLine(MyElevation.Hours)
        /// ' Output: 60
        ///   </code>
        ///   <code lang="CS">
        /// Elevation MyElevation = new Elevation(60.5);
        /// Console.WriteLine(MyElevation.Hours);
        /// // Output: 60
        ///   </code>
        ///   </example>
        /// <remarks>This property is used in conjunction with the <see cref="Minutes">Minutes</see>
        /// and <see cref="Seconds">Seconds</see> properties to create a full angular measurement.
        /// This property is the same as <strong>DecimalDegrees</strong> without any fractional
        /// value.</remarks>
        public int Hours
        {
            get
            {
#if Framework20 && !PocketPC
                return (int)Math.Truncate(_decimalDegrees);
#else
                return Truncate(_DecimalDegrees);
#endif
            }
        }

        /// <summary>
        /// Returns the integer minutes portion of an angular measurement.
        /// </summary>
        /// <value>An <strong>Integer</strong>.</value>
        /// <seealso cref="Hours">Hours Property</seealso>
        ///
        /// <seealso cref="Seconds">Seconds Property</seealso>
        ///
        /// <example>
        /// This example creates an angle of 45.5° then outputs the value of the
        ///   <strong>Minutes</strong> property, 30.
        ///   <code lang="VB">
        /// Dim MyElevation As New Elevation(45.5)
        /// Debug.WriteLine(MyElevation.Minutes)
        /// ' Output: 30
        ///   </code>
        ///   <code lang="CS">
        /// Elevation MyElevation = new Elevation(45.5);
        /// Console.WriteLine(MyElevation.Minutes);
        /// // Output: 30
        ///   </code>
        ///   </example>
        /// <remarks>This property is used in conjunction with the <see cref="Hours">Hours</see> and
        /// <see cref="Seconds">Seconds</see> properties to create a sexagesimal
        /// measurement.</remarks>
        public int Minutes
        {
            get
            {
                return Convert.ToInt32(
                    Math.Abs(
#if Framework20 && !PocketPC
Math.Truncate(
#else
                        Truncate(
#endif
Math.Round(
                    // Calculations appear to support one less digit than the maximum allowed precision
                                (_decimalDegrees - Hours) * 60.0, MAXIMUM_PRECISION_DIGITS - 1))));
            }
        }

        /// <summary>
        /// Returns the seconds minutes portion of an angular measurement.
        /// </summary>
        /// <value>A <strong>Double</strong> value.</value>
        /// <seealso cref="Hours">Hours Property</seealso>
        ///
        /// <seealso cref="Minutes">Minutes Property</seealso>
        ///
        /// <example>
        /// This example creates an angle of 45°10.5' then outputs the value of the
        ///   <strong>Seconds</strong> property, 30.
        ///   <code lang="VB">
        /// Dim MyElevation As New Elevation(45, 10.5)
        /// Debug.WriteLine(MyElevation.Seconds)
        /// ' Output: 30
        ///   </code>
        ///   <code lang="CS">
        /// Dim MyElevation As New Elevation(45, 10.5);
        /// Console.WriteLine(MyElevation.Seconds);
        /// // Output: 30
        ///   </code>
        ///   </example>
        /// <remarks>This property is used in conjunction with the <see cref="Hours">Hours</see> and
        /// <see cref="Minutes">Minutes</see> properties to create a sexagesimal
        /// measurement.</remarks>
        public double Seconds
        {
            get
            {
                return Math.Round(
                                (Math.Abs(_decimalDegrees - Hours) * 60.0 - Minutes) * 60.0,
                    // This property appears to support one less digit than the maximum allowed
                                MAXIMUM_PRECISION_DIGITS - 4);
            }
        }

        /// <summary>
        /// Indicates if the current instance has a non-zero value.
        /// </summary>
        /// <value>A <strong>Boolean</strong>, <strong>True</strong> if the
        /// <strong>DecimalDegrees</strong> property is zero.</value>
        /// <seealso cref="Empty">Empty Field</seealso>
        public bool IsEmpty
        {
            get
            {
                return (_decimalDegrees == 0);
            }
        }

        /// <summary>
        /// Indicates if the current instance represents an infinite value.
        /// </summary>
        public bool IsInfinity
        {
            get
            {
                return double.IsInfinity(_decimalDegrees);
            }
        }

        /// <summary>
        /// Indicates if the current instance is invalid or unspecified.
        /// </summary>
        public bool IsInvalid
        {
            get { return double.IsNaN(_decimalDegrees); }
        }

        /// <summary>
        /// Indicates whether the current instance has been normalized and is within the
        /// allowed bounds of -90° and 90°.
        /// </summary>
        public bool IsNormalized
        {
            get { return _decimalDegrees >= -90 && _decimalDegrees <= 90; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Normalizes this instance.
        /// </summary>
        /// <returns></returns>
        /// <overloads>Converts a measurement to its equivalent value between -90 and
        /// 90 degrees.</overloads>
        public Elevation Normalize()
        {
            // Is the value not a number, infinity, or already normalized?
            if (double.IsInfinity(_decimalDegrees)
                || double.IsNaN(_decimalDegrees)
                || IsNormalized)
                return this;

            // Calculate the number of times the degree value winds completely
            // through a hemisphere
            int hemisphereFlips = Convert.ToInt32(Math.Floor(_decimalDegrees / 180.0));

            // If the value is in the southern hemisphere, apply another flip
            if (_decimalDegrees < 0)
                hemisphereFlips++;

            // Calculate the new value
            double newValue = _decimalDegrees % 180;

            // if the value is > 90, return 180 - X
            if (newValue > 90)
                newValue = 180 - newValue;

            // If the value id < -180, return -180 - X
            else if (newValue < -90.0)
                newValue = -180.0 - newValue;

            // Account for flips around hemispheres by flipping the sign
            if (hemisphereFlips % 2 != 0)
                return new Elevation(-newValue);
            return new Elevation(newValue);
        }

        /// <summary>
        /// Returns the smallest integer greater than the specified value.
        /// </summary>
        /// <returns></returns>
        public Elevation Ceiling()
        {
            return new Elevation(Math.Ceiling(_decimalDegrees));
        }

        /// <summary>
        /// Returns the largest integer which is smaller than the specified value.
        /// </summary>
        /// <returns></returns>
        public Elevation Floor()
        {
            return new Elevation(Math.Floor(_decimalDegrees));
        }

#if !Framework20 || PocketPC
        internal static int Truncate(double value)
        {
            return value > 0
                ? (int)(value - (value - Math.Floor(value)))
                : (int)(value - (value - Math.Ceiling(value)));
        }
#endif

        /// <summary>
        /// Returns a new instance whose value is rounded the specified number of decimals.
        /// </summary>
        /// <param name="decimals">An <strong>Integer</strong> specifying the number of decimals to round off to.</param>
        /// <returns></returns>
        public Elevation Round(int decimals)
        {
            return new Elevation(Math.Round(_decimalDegrees, decimals));
        }

        /// <summary>
        /// Returns a new instance whose Seconds property is evenly divisible by 15.
        /// </summary>
        /// <returns>An <strong>Elevation</strong> containing the rounded value.</returns>
        /// <remarks>This method is used to align or "snap" an angle to a regular interval. For
        /// example, a grid might be easier to read if it were drawn at 30-second intervals instead
        /// of 24.198-second intervals.</remarks>
        public Elevation RoundSeconds()
        {
            return RoundSeconds(15.0);
        }

        /// <summary>
        /// Returns a new angle whose Seconds property is evenly divisible by the specified amount.
        /// </summary>
        /// <param name="interval">A <strong>Double</strong> between 0 and 60 indicating the interval to round
        /// to.</param>
        /// <returns>An <strong>Elevation</strong> containing the rounded value.</returns>
        /// <remarks>This method is used to align or "snap" an angle to a regular interval. For
        /// example, a grid might be easier to read if it were drawn at 30-second intervals instead
        /// of 24.198-second intervals.</remarks>
        public Elevation RoundSeconds(double interval)
        {
            // Interval must be > 0
            if (interval == 0)
#if PocketPC
                throw new ArgumentOutOfRangeException(Properties.Resources.Angle_InvalidInterval);
#else
                throw new ArgumentOutOfRangeException("interval", interval, Resources.Angle_InvalidInterval);
#endif
            // Get the amount in seconds
            double newSeconds = Seconds;
            //double HalfInterval = interval * 0.5;
            // Loop through all intervals to find the right rounding
            for (double value = 0; value < 60; value += interval)
            {
                // Calculate the value of the next interval
                double nextInterval = value + interval;
                // Is the seconds value greater than the next interval?
                if (newSeconds > nextInterval)
                    // Yes.  Continue on
                    continue;
                // Is the seconds value closer to the current or next interval?
                newSeconds = newSeconds < (value + nextInterval) * 0.5 ? value : nextInterval;
                // Is the new value 60?  If so, make it zero
                if (newSeconds == 60)
                    newSeconds = 0;
                // Return the new value
                return new Elevation(Hours, Minutes, newSeconds);
            }
            // return the new value
            return new Elevation(Hours, Minutes, newSeconds);
        }

        /// <summary>
        /// Returns the object with the smallest value.
        /// </summary>
        /// <param name="value">An <strong>Elevation</strong> object to compare to the current instance.</param>
        /// <returns>The <strong>Elevation</strong> containing the smallest value.</returns>
        public Elevation LesserOf(Elevation value)
        {
            if (_decimalDegrees < value.DecimalDegrees)
                return this;
            return value;
        }

        /// <summary>
        /// Returns the object with the largest value.
        /// </summary>
        /// <param name="value">An <strong>Elevation</strong> object to compare to the current instance.</param>
        /// <returns>An <strong>Elevation</strong> containing the largest value.</returns>
        public Elevation GreaterOf(Elevation value)
        {
            if (_decimalDegrees > value.DecimalDegrees)
                return this;
            return value;
        }

        /// <summary>
        /// Returns an angle opposite of the current instance.
        /// </summary>
        /// <returns>An <strong>Elevation</strong> representing the mirrored value.</returns>
        /// <example>
        /// This example creates a new <strong>Elevation</strong> of 45° then calculates its mirror
        /// of 225°. (45 + 180)
        ///   <code lang="VB" title="[New Example]">
        /// Dim Elevation1 As New Elevation(45)
        /// Dim Elevation2 As Elevation = Elevation1.Mirror()
        /// Debug.WriteLine(Elevation2.ToString())
        /// ' Output: 225
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Elevation Elevation1 = new Elevation(45);
        /// Elevation Elevation2 = Elevation1.Mirror();
        /// Console.WriteLine(Elevation2.ToString());
        /// // Output: 225
        ///   </code>
        ///   </example>
        /// <remarks>This method returns the "opposite" of the current instance. The opposite is
        /// defined as the point on the other side of an imaginary circle. For example, if an angle
        /// is 0°, at the top of a circle, this method returns 180°, at the bottom of the
        /// circle.</remarks>
        public Elevation Mirror()
        {
            return new Elevation(_decimalDegrees + 180.0).Normalize();
        }

        /// <summary>
        /// Converts the current instance into radians.
        /// </summary>
        /// <returns>A <see cref="Radian">Radian</see> object.</returns>
        /// <seealso cref="Radian">Radian Class</seealso>
        ///
        /// <overloads>Converts an angular measurement into radians before further processing.</overloads>
        ///
        /// <example>
        /// This example converts a measurement of 90° into radians.
        ///   <code lang="VB">
        /// Dim MyElevation As New Elevation(90)
        /// Dim MyRadians As Radian = MyElevation.ToRadians()
        ///   </code>
        ///   <code lang="CS">
        /// Elevation MyElevation = new Elevation(90);
        /// Radian MyRadians = MyElevation.ToRadians();
        ///   </code>
        ///   </example>
        /// <remarks>This function is typically used to convert an angular measurement into
        /// radians before performing a trigonometric function.</remarks>
        public Radian ToRadians()
        {
            return Radian.FromDegrees(_decimalDegrees);
        }

        /// <summary>
        /// Outputs the angle as a string using the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>A <strong>String</strong> in the specified format.</returns>
        /// <seealso cref="ToString(string, IFormatProvider)">ToString Method</seealso>
        ///
        /// <seealso cref="Parse(string)">Parse Method</seealso>
        ///
        /// <example>
        /// This example uses the <strong>ToString</strong> method to output an angle in a
        /// custom format. The " <strong>h°</strong> " code represents hours along with a
        /// degree symbol (Alt+0176 on the keypad), and " <strong>m.mm</strong> " represents
        /// the minutes out to two decimals. Mmm.
        ///   <code lang="VB">
        /// Dim MyElevation As New Elevation(45, 16.772)
        /// Debug.WriteLine(MyElevation.ToString("h°m.mm"))
        /// ' Output: 45°16.78
        ///   </code>
        ///   <code lang="CS">
        /// Dim MyElevation As New Elevation(45, 16.772);
        /// Debug.WriteLine(MyElevation.ToString("h°m.mm"));
        /// // Output: 45°16.78
        ///   </code>
        ///   </example>
        /// <remarks>This method returns the current instance output in a specific format. If no
        /// value for the format is specified, a default format of "d.dddd°" is used. Any
        /// string output by this method can be converted back into an Elevation object using the
        /// <strong>Parse</strong> method or <strong>Elevation(string)</strong> constructor.</remarks>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        #endregion Public Methods

        #region Overrides

        /// <summary>
        /// Compares the current value to another Elevation object's value.
        /// </summary>
        /// <param name="obj">An <strong>Elevation</strong>, <strong>Double</strong>, or <strong>Integer</strong>
        /// to compare with.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the object's DecimalDegrees
        /// properties match.</returns>
        /// <remarks>This</remarks>
        public override bool Equals(object obj)
        {
#if !PocketPC || Framework20
            // ?
            if (ReferenceEquals(obj, null))
                return false;
#endif
            // Convert objects to an Elevation as needed before comparison
            if (obj is Elevation || obj is string)
                return _decimalDegrees.Equals(((Elevation)obj).DecimalDegrees);
            if (obj is double)
            {
                return _decimalDegrees.Equals(((Elevation)(double)obj).DecimalDegrees);
            }
            if (obj is int)
            {
                return _decimalDegrees.Equals(((Elevation)(int)obj).DecimalDegrees);
            }
            if (obj is float)
            {
                return _decimalDegrees.Equals(((Elevation)(float)obj).DecimalDegrees);
            }

            // Nothing else will work, so False
            return false;
        }

        /// <summary>
        /// Returns a unique code for this instance.
        /// </summary>
        /// <returns>An <strong>Integer</strong> representing a unique code for the current
        /// instance.</returns>
        /// <remarks>Since the <strong>Elevation</strong> class is immutable, this property may be used
        /// safely with hash tables.</remarks>
        public override int GetHashCode()
        {
            return _decimalDegrees.GetHashCode();
        }

        /// <summary>
        /// Outputs the angle as a string using the default format.
        /// </summary>
        /// <returns>A <strong>String</strong> created using the default format.</returns>
        /// <seealso cref="Parse(string)">Parse Method</seealso>
        ///
        /// <example>
        /// This example outputs a value of 90 degrees in the default format of ###.#°.
        ///   <code lang="VB">
        /// Dim MyElevation As New Elevation(90)
        /// Debug.WriteLine(MyElevation.ToString)
        /// ' Output: "90°"
        ///   </code>
        ///   <code lang="CS">
        /// Elevation MyElevation = new Elevation(90);
        /// Debug.WriteLine(MyElevation.ToString());
        /// // Output: "90°"
        ///   </code>
        ///   </example>
        /// <remarks>This method formats the current instance using the default format of
        /// "d.dddd°." Any string output by this method can be converted back into an Elevation
        /// object using the <strong>Parse</strong> method or <strong>Elevation(string)</strong>
        /// constructor.</remarks>
        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion Overrides

        #region Static Methods

        /// <summary>
        /// Normalizes the specified decimal degrees.
        /// </summary>
        /// <param name="decimalDegrees">The decimal degrees.</param>
        /// <returns></returns>
        /// <overloads>Converts a measurement to its equivalent value between -90 and
        /// 90 degrees.</overloads>
        public static Elevation Normalize(double decimalDegrees)
        {
            return new Elevation(decimalDegrees).Normalize();
        }

        /// <summary>
        /// Converts arbitrary hour, minute and seconds into decimal degrees.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <returns>A <strong>Double</strong> containing the decimal degree version of the specified
        /// values.</returns>
        /// <seealso cref="Latitude.DecimalDegrees">DecimalDegrees Property</seealso>
        ///
        /// <seealso cref="Latitude.Normalize()">Normalize Method</seealso>
        ///
        /// <example>
        /// This example converts a value of 10°30'0" into decimal degrees (10.5).
        ///   <code lang="VB" title="ToDecimalDegrees Example (VB)">
        /// Dim MyValue As Double = Latitude.ToDecimalDegrees(10, 30, 0)
        ///   </code>
        ///   <code lang="CS" title="ToDecimalDegrees Example (C#)">
        /// double MyValue = Latitude.ToDecimalDegrees(10, 30, 0);
        ///   </code>
        ///   </example>
        /// <remarks>This function is used to convert three-part measurements into a single value. The
        /// result of this method is typically assigned to the
        /// <see cref="Latitude.DecimalDegrees">
        /// DecimalDegrees</see> property. Values are rounded to thirteen decimal
        /// places, the maximum precision allowed by this type.</remarks>
        public static double ToDecimalDegrees(int hours, int minutes, double seconds)
        {
            //return hours < 0
            //    ? -Math.Round(-hours + minutes / 60.0 + seconds / 3600.0, MaximumPrecisionDigits)
            //    : Math.Round(hours + minutes / 60.0 + seconds / 3600.0, MaximumPrecisionDigits);
            return hours < 0
                ? -(-hours + minutes / 60.0 + seconds / 3600.0)
                : (hours + minutes / 60.0 + seconds / 3600.0);
        }

        /// <summary>
        /// Converts arbitrary hour and decimal minutes into decimal degrees.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <param name="decimalMinutes">The decimal minutes.</param>
        /// <returns>A <strong>Double</strong> containing the decimal degree version of the specified
        /// values.</returns>
        /// <remarks>This function is used to convert three-part measurements into a single value. The
        /// result of this method is typically assigned to the
        /// <see cref="Latitude.DecimalDegrees">
        /// DecimalDegrees</see> property. Values are rounded to thirteen decimal
        /// places, the maximum precision allowed by this type.</remarks>
        public static double ToDecimalDegrees(int hours, double decimalMinutes)
        {
            //return hours < 0
            //    ? -Math.Round(-hours + decimalMinutes / 60.0, MaximumPrecisionDigits)
            //    : Math.Round(hours + decimalMinutes / 60.0, MaximumPrecisionDigits);
            return hours < 0
                ? -(-hours + decimalMinutes / 60.0)
                : (hours + decimalMinutes / 60.0);
        }

        /// <summary>
        /// Converts an hour value into decimal degrees.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <returns>A <strong>Double</strong> containing the decimal degree version of the specified
        /// values.</returns>
        /// <remarks>The specified value will be converted to a double value.</remarks>
        public static double ToDecimalDegrees(int hours)
        {
            return Convert.ToDouble(hours);
        }

        /// <summary>
        /// Returns a random angle between 0° and 360°.
        /// </summary>
        /// <returns>An <strong>Elevation</strong> containing a random value.</returns>
        public static Elevation Random()
        {
            return Random(new Random(DateTime.Now.Millisecond));
        }

        /// <summary>
        /// Returns a random Elevation between 0° and 360° using the specified random number
        /// seed.
        /// </summary>
        /// <param name="generator">A <strong>Random</strong> object used to ogenerate random values.</param>
        /// <returns>An <strong>Elevation</strong> containing a random value.</returns>
        public static Elevation Random(Random generator)
        {
            return new Elevation(generator.NextDouble() * 360.0);
        }

        /// <summary>
        /// Returns the object with the smallest value.
        /// </summary>
        /// <param name="value1">A <strong>Elevation</strong> object to compare to value2.</param>
        /// <param name="value2">A <strong>Elevation</strong> object to compare to value1.</param>
        /// <returns>The <strong>Elevation</strong> containing the smallest value.</returns>
        public static Elevation LesserOf(Elevation value1, Elevation value2)
        {
            return value1.LesserOf(value2);
        }

        /// <summary>
        /// Returns the object with the largest value.
        /// </summary>
        /// <param name="value1">A <strong>Elevation</strong> object to compare to value2.</param>
        /// <param name="value2">A <strong>Elevation</strong> object to compare to value1.</param>
        /// <returns>A <strong>Elevation</strong> containing the largest value.</returns>
        public static Elevation GreaterOf(Elevation value1, Elevation value2)
        {
            return value1.GreaterOf(value2);
        }

        /// <summary>
        /// Converts an angular measurement into radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="Radian"><strong>Radian</strong></see> object.</returns>
        /// <example>
        /// This example shows a quick way to convert an angle of 90° into radians.
        ///   <code lang="VB">
        /// Dim MyRadian As Radian = Elevation.ToRadians(90)
        ///   </code>
        ///   <code lang="CS">
        /// Radian MyRadian = Elevation.ToRadians(90);
        ///   </code>
        ///   </example>
        /// <remarks>This function is typically used to convert an angular measurement into
        /// radians before performing a trigonometric function.</remarks>
        public static Radian ToRadians(Elevation value)
        {
            return value.ToRadians();
        }

        /// <summary>
        /// Converts a value in radians into an angular measurement.
        /// </summary>
        /// <param name="radians">The radians.</param>
        /// <returns></returns>
        /// <seealso cref="Elevation.ToRadians(Elevation)">ToRadians</seealso>
        ///
        /// <seealso cref="Radian">Radian Class</seealso>
        ///
        /// <example>
        /// This example uses the <strong>FromRadians</strong> method to convert a value of one
        /// radian into an <strong>Elevation</strong> of 57°.
        ///   <code lang="VB">
        /// ' Create a new angle equal to one radian
        /// Dim MyRadians As New Radian(1)
        /// Dim MyElevation As Elevation = Elevation.FromRadians(MyRadians)
        /// Debug.WriteLine(MyElevation.ToString())
        /// ' Output: 57°
        ///   </code>
        ///   <code lang="CS">
        /// // Create a new angle equal to one radian
        /// Radian MyRadians = new Radian(1);
        /// Elevation MyElevation = Elevation.FromRadians(MyRadians);
        /// Console.WriteLine(MyElevation.ToString());
        /// // Output: 57°
        ///   </code>
        ///   </example>
        /// <remarks>This function is typically used in conjunction with the
        /// <see cref="Elevation.ToRadians()">ToRadians</see>
        /// method after a trigonometric function has completed. The converted value is stored in
        /// the <see cref="DecimalDegrees">DecimalDegrees</see> property.</remarks>
        public static Elevation FromRadians(Radian radians)
        {
            return new Elevation(radians.ToDegrees());
        }

        /// <summary>
        /// Froms the radians.
        /// </summary>
        /// <param name="radians">The radians.</param>
        /// <returns></returns>
        public static Elevation FromRadians(double radians)
        {
            return new Elevation(Radian.ToDegrees(radians));
        }

        /// <summary>
        /// Converts the specified string into an Elevation object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A new <strong>Elevation</strong> object populated with the specified
        /// values.</returns>
        /// <seealso cref="ToString()">ToString Method</seealso>
        ///
        /// <example>
        /// This example creates a new angular measurement using the <strong>Parse</strong>
        /// method.
        ///   <code lang="VB">
        /// Dim NewElevation As Elevation = Elevation.Parse("123.45°")
        ///   </code>
        ///   <code lang="CS">
        /// Elevation NewElevation = Elevation.Parse("123.45°");
        ///   </code>
        ///   </example>
        ///
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">The Parse method requires a decimal or sexagesimal measurement.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">Only the right-most portion of a sexagesimal measurement can be a fractional value.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">Extra characters were encountered while parsing an angular measurement.  Only hours, minutes, and seconds are allowed.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">The specified text was not fully understood as an angular measurement.</exception>
        /// <remarks>This method parses the specified string into an <strong>Elevation</strong> object
        /// using the current culture. This constructor can parse any strings created via the
        /// <strong>ToString</strong> method.</remarks>
        public static Elevation Parse(string value)
        {
            return new Elevation(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts the specified string into an <strong>Elevation</strong> object using the
        /// specified culture.
        /// </summary>
        /// <param name="value">A <strong>String</strong> describing an angle in the form of decimal degrees or a
        /// sexagesimal.</param>
        /// <param name="culture">A <strong>CultureInfo</strong> object describing the numeric format to use during
        /// conversion.</param>
        /// <returns>A new <strong>Elevation</strong> object equivalent to the specified string.</returns>
        /// <remarks>This powerful method is typically used to process data from a data store or a
        /// value input by the user in any culture. This function can accept any format which
        /// can be output by the ToString method.</remarks>
        public static Elevation Parse(string value, CultureInfo culture)
        {
            return new Elevation(value, culture);
        }

        #endregion Static Methods

        #region Operators

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Elevation operator +(Elevation left, Elevation right)
        {
            return new Elevation(left.DecimalDegrees + right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Elevation operator +(Elevation left, double right)
        {
            return new Elevation(left.DecimalDegrees + right);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Elevation operator -(Elevation left, Elevation right)
        {
            return new Elevation(left.DecimalDegrees - right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Elevation operator -(Elevation left, double right)
        {
            return new Elevation(left.DecimalDegrees - right);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Elevation operator *(Elevation left, Elevation right)
        {
            return new Elevation(left.DecimalDegrees * right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Elevation operator *(Elevation left, double right)
        {
            return new Elevation(left.DecimalDegrees * right);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Elevation operator /(Elevation left, Elevation right)
        {
            return new Elevation(left.DecimalDegrees / right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Elevation operator /(Elevation left, double right)
        {
            return new Elevation(left.DecimalDegrees / right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Elevation left, Elevation right)
        {
            return left.DecimalDegrees.Equals(right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Elevation left, double right)
        {
            return left.DecimalDegrees.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Elevation left, Elevation right)
        {
            return !left.DecimalDegrees.Equals(right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Elevation left, double right)
        {
            return !left.DecimalDegrees.Equals(right);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Elevation left, Elevation right)
        {
            return left.DecimalDegrees > right.DecimalDegrees;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Elevation left, double right)
        {
            return left.DecimalDegrees > right;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(Elevation left, Elevation right)
        {
            return left.DecimalDegrees >= right.DecimalDegrees;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(Elevation left, double right)
        {
            return left.DecimalDegrees >= right;
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Elevation left, Elevation right)
        {
            return left.DecimalDegrees < right.DecimalDegrees;
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Elevation left, double right)
        {
            return left.DecimalDegrees < right;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(Elevation left, Elevation right)
        {
            return left.DecimalDegrees <= right.DecimalDegrees;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(Elevation left, double right)
        {
            return left.DecimalDegrees <= right;
        }

        /// <summary>
        /// Returns the current instance increased by one.
        /// </summary>
        /// <returns>An <strong>Elevation</strong> object.</returns>
        /// <example>
        /// This example uses the <strong>Increment</strong> method to increase an Elevation's
        /// value. It also demonstrates the subtle error which can be caused if
        ///   <strong>Increment</strong> is called while ignoring the return value.
        ///   <code lang="VB" title="[New Example]">
        /// ' Correct use of Increment
        /// Dim Elevation1 As New Elevation(89)
        /// Elevation1 = Elevation1.Increment()
        /// ' Incorrect use of Increment
        /// Dim Elevation1 = New Elevation(89)
        /// Elevation1.Increment()
        /// 'notice Elevation1 will still be 89°!
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// // Correct use of Increment
        /// Elevation Elevation1 = new Elevation(89);
        /// Elevation1 = Elevation1.Increment();
        /// // Incorrect use of Increment
        /// Elevation Elevation1 = new Elevation(89);
        /// Elevation1.Increment();
        /// //notice Elevation1 will still be 89°!
        ///   </code>
        ///   </example>
        /// <remarks><para>This method increases the <strong>DecimalDegrees</strong> property by 1.0,
        /// returned as a new instance.</para>
        ///   <para><font color="red">Since the <strong>Elevation</strong> class is immutable, this
        /// method cannot be used to modify an existing instance.</font></para></remarks>
        public Elevation Increment()
        {
            return new Elevation(_decimalDegrees + 1.0);
        }

        /// <summary>
        /// Increases the current instance by the specified value.
        /// </summary>
        /// <param name="value">A <strong>Double</strong> to add to the current instance.</param>
        /// <returns>A new <strong>Elevation</strong> containing the summed values.</returns>
        /// <example>
        /// This example adds 45° to the current instance of 45°, returning 90°.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Elevation1 As New Elevation(45)
        /// Elevation1 = Elevation1.Add(45)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Elevation Elevation1 = new Elevation(45);
        /// Elevation1 = Elevation1.Add(45);
        ///   </code>
        ///   </example>
        public Elevation Add(double value)
        {
            return new Elevation(_decimalDegrees + value);
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Elevation Add(Elevation value)
        {
            return new Elevation(_decimalDegrees + value.DecimalDegrees);
        }

        /// <summary>
        /// Returns the current instance decreased by one.
        /// </summary>
        /// <returns>An <strong>Elevation</strong> object.</returns>
        /// <example>
        /// This example uses the <strong>Decrement</strong> method to decrease an Elevation's
        /// value. It also demonstrates the subtle error which can be caused if
        ///   <strong>Decrement</strong> is called while ignoring the return value.
        ///   <code lang="VB" title="[New Example]">
        /// ' Correct use of Decrement
        /// Dim Elevation1 As New Elevation(91)
        /// Elevation1 = Elevation1.Decrement()
        /// ' Incorrect use of Decrement
        /// Dim Elevation1 = New Elevation(91)
        /// Elevation1.Increment()
        /// ' notice Elevation1 will still be 91°!
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// // Correct use of Decrement
        /// Elevation Elevation1 = new Elevation(91);
        /// Elevation1 = Elevation1.Decrement();
        /// // Incorrect use of Decrement
        /// Elevation Elevation1 = new Elevation(91);
        /// Elevation1.Decrement();
        /// // notice Elevation1 will still be 91°!
        ///   </code>
        ///   </example>
        /// <remarks><para>This method decreases the <strong>DecimalDegrees</strong> property by 1.0,
        /// returned as a new instance.</para>
        ///   <para><font color="red">Since the <strong>Elevation</strong> class is immutable, this
        /// method cannot be used to modify an existing instance.</font></para></remarks>
        public Elevation Decrement()
        {
            return new Elevation(_decimalDegrees - 1.0);
        }

        /// <summary>
        /// Decreases the current instance by the specified value.
        /// </summary>
        /// <param name="value">A <strong>Double</strong> to subtract from the current instance.</param>
        /// <returns>A new <strong>Elevation</strong> containing the new value.</returns>
        /// <example>
        /// This example subtracts 30° from the current instance of 90°, returning 60°.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Elevation1 As New Elevation(90)
        /// Elevation1 = Elevation1.Subtract(30)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Elevation Elevation1 = new Elevation(90);
        /// Elevation1 = Elevation1.Subtract(30);
        ///   </code>
        ///   </example>
        public Elevation Subtract(double value)
        {
            return new Elevation(_decimalDegrees - value);
        }

        /// <summary>
        /// Subtracts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Elevation Subtract(Elevation value)
        {
            return new Elevation(_decimalDegrees - value.DecimalDegrees);
        }

        /// <summary>
        /// Multiplies the current instance by the specified value.
        /// </summary>
        /// <param name="value">A <strong>Double</strong> to multiply with the current instance.</param>
        /// <returns>A new <strong>Elevation</strong> containing the product of the two numbers.</returns>
        /// <example>
        /// This example multiplies 30° with three, returning 90°.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Elevation1 As New Elevation(30)
        /// Elevation1 = Elevation1.Multiply(3)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Elevation Elevation1 = new Elevation(30);
        /// Elevation1 = Elevation1.Multiply(3);
        ///   </code>
        ///   </example>
        public Elevation Multiply(double value)
        {
            return new Elevation(_decimalDegrees * value);
        }

        /// <summary>
        /// Multiplies the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Elevation Multiply(Elevation value)
        {
            return new Elevation(_decimalDegrees * value.DecimalDegrees);
        }

        /// <summary>
        /// Divides the current instance by the specified value.
        /// </summary>
        /// <param name="value">A <strong>Double</strong> representing a denominator to divide by.</param>
        /// <returns>An <strong>Elevation</strong> containing the new value.</returns>
        /// <example>
        /// This example divides 90° by three, returning 30°.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Elevation1 As New Elevation(90)
        /// Elevation1 = Elevation1.Divide(3)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Elevation Elevation1 = new Elevation(90);
        /// Elevation1 = Elevation1.Divide(3);
        ///   </code>
        ///   </example>
        public Elevation Divide(double value)
        {
            return new Elevation(_decimalDegrees / value);
        }

        /// <summary>
        /// Divides the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Elevation Divide(Elevation value)
        {
            return new Elevation(_decimalDegrees / value.DecimalDegrees);
        }

        /// <summary>
        /// Indicates if the current instance is smaller than the specified value.
        /// </summary>
        /// <param name="value">An <strong>Elevation</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// smaller than the specified value.</returns>
        public bool IsLessThan(Elevation value)
        {
            return _decimalDegrees < value.DecimalDegrees;
        }

        /// <summary>
        /// Determines whether [is less than] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is less than] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsLessThan(double value)
        {
            return _decimalDegrees < value;
        }

        /// <summary>
        /// Indicates if the current instance is smaller than or equal to the specified
        /// value.
        /// </summary>
        /// <param name="value">An <strong>Elevation</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// smaller than or equal to the specified value.</returns>
        /// <remarks>This method compares the <strong>DecimalDegrees</strong> property with the
        /// specified value. This method is the same as the "&lt;=" operator.</remarks>
        public bool IsLessThanOrEqualTo(Elevation value)
        {
            return _decimalDegrees <= value.DecimalDegrees;
        }

        /// <summary>
        /// Determines whether [is less than or equal to] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is less than or equal to] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsLessThanOrEqualTo(double value)
        {
            return _decimalDegrees <= value;
        }

        /// <summary>
        /// Indicates if the current instance is larger than the specified value.
        /// </summary>
        /// <param name="value">An <strong>Elevation</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// greater than the specified value.</returns>
        public bool IsGreaterThan(Elevation value)
        {
            return _decimalDegrees > value.DecimalDegrees;
        }

        /// <summary>
        /// Determines whether [is greater than] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is greater than] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsGreaterThan(double value)
        {
            return _decimalDegrees > value;
        }

        /// <summary>
        /// Indicates if the current instance is larger than or equal to the specified
        /// value.
        /// </summary>
        /// <param name="value">An <strong>Elevation</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// greater than or equal to the specified value.</returns>
        public bool IsGreaterThanOrEqualTo(Elevation value)
        {
            return _decimalDegrees >= value.DecimalDegrees;
        }

        /// <summary>
        /// Determines whether [is greater than or equal to] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is greater than or equal to] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsGreaterThanOrEqualTo(double value)
        {
            return _decimalDegrees >= value;
        }

        #endregion Operators

        #region Conversions

        /// <summary>
        /// Converts a measurement in Radians into an Elevation.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Elevation(Radian value)
        {
            return new Elevation(value.ToDegrees());
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Elevation.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Elevation(double value)
        {
            return new Elevation(value);
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Elevation.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Elevation(float value)
        {
            return new Elevation(Convert.ToDouble(value));
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Elevation.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator double(Elevation value)
        {
            return value.DecimalDegrees;
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Elevation.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator float(Elevation value)
        {
            return Convert.ToSingle(value.DecimalDegrees);
        }

        /// <summary>
        /// Converts a measurement in degrees as an Integer into an Elevation.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Elevation(int value)
        {
            return new Elevation(value);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Angle"/> to <see cref="DotSpatial.Positioning.Elevation"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Elevation(Angle value)
        {
            return new Elevation(value.DecimalDegrees);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Azimuth"/> to <see cref="DotSpatial.Positioning.Elevation"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Elevation(Azimuth value)
        {
            return new Elevation(value.DecimalDegrees);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Latitude"/> to <see cref="DotSpatial.Positioning.Elevation"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Elevation(Latitude value)
        {
            return new Elevation(value.DecimalDegrees);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Longitude"/> to <see cref="DotSpatial.Positioning.Elevation"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Elevation(Longitude value)
        {
            return new Elevation(value.DecimalDegrees);
        }

        /// <summary>
        /// Converts a measurement in the form of a formatted String into an Elevation.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Elevation(string value)
        {
            return new Elevation(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts an Elevation into a String.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks>This operator calls the ToString() method using the current culture.</remarks>
        public static explicit operator string(Elevation value)
        {
            return value.ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion Conversions

        #region ICloneable<Elevation> Members

        /// <summary>
        /// Creates a copy of the current instance.
        /// </summary>
        /// <returns>An <strong>Elevation</strong> of the same value as the current instance.</returns>
        public Elevation Clone()
        {
            return new Elevation(_decimalDegrees);
        }

        #endregion ICloneable<Elevation> Members

        #region IFormattable Members

        /// <summary>
        /// Outputs the angle as a string using the specified format.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <strong>String</strong> in the specified format.</returns>
        /// <seealso cref="ToString()">ToString Method</seealso>
        ///
        /// <seealso cref="Parse(string)">Parse Method</seealso>
        ///
        /// <example>
        /// This example uses the <strong>ToString</strong> method to output an angle in a
        /// custom format. The " <strong>h°</strong> " code represents hours along with a
        /// degree symbol (Alt+0176 on the keypad), and " <strong>m.mm</strong> " represents
        /// the minutes out to two decimals. Mmm.
        ///   <code lang="VB">
        /// Dim MyElevation As New Elevation(45, 16.772)
        /// Debug.WriteLine(MyElevation.ToString("h°m.mm", CultureInfo.CurrentCulture))
        /// ' Output: 45°16.78
        ///   </code>
        ///   <code lang="CS">
        /// Dim MyElevation As New Elevation(45, 16.772);
        /// Debug.WriteLine(MyElevation.ToString("h°m.mm", CultureInfo.CurrentCulture));
        /// // Output: 45°16.78
        ///   </code>
        ///   </example>
        /// <remarks>This method returns the current instance output in a specific format. If no
        /// value for the format is specified, a default format of "d.dddd" is used. Any string
        /// output by this method can be converted back into an Elevation object using the
        /// <strong>Parse</strong> method or <strong>Elevation(string)</strong> constructor.</remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider ?? CultureInfo.CurrentCulture;

            if (string.IsNullOrEmpty(format)
                || format == "g" || format == "G")
                format = "e";

            if (format == "e")
            {
                if (DecimalDegrees < 0)
                    return "Below the horizon";
                if (DecimalDegrees < 30)
                    return "Near the horizon";
                if (DecimalDegrees < 60)
                    return "Halfway up from the horizon";
                if (DecimalDegrees < 80)
                    return "Almost directly overhead";
                return "Directly overhead";
            }

            // Parse as a normal angle
            string subFormat;
            string newFormat;
            bool isDecimalHandled = false;
            try
            {
                // Is it infinity?
                if (double.IsPositiveInfinity(DecimalDegrees))
                    return "+" + Resources.Common_Infinity;
                // Is it infinity?
                if (double.IsNegativeInfinity(DecimalDegrees))
                    return "-" + Resources.Common_Infinity;
                if (double.IsNaN(DecimalDegrees))
                    return "NaN";
                // Use the default if "g" is passed
                format = format.ToLower(culture);
                if (format == "g")
                    format = "d.dddd°";
                // Replace the "d" with "h" since degrees is the same as hours
                format = format.Replace("d", "h")
                    // Convert the format to uppercase
                    .ToUpper(culture);
                // Only one decimal is allowed
                if (format.IndexOf(culture.NumberFormat.NumberDecimalSeparator) !=
                    format.LastIndexOf(culture.NumberFormat.NumberDecimalSeparator))
                    throw new ArgumentException(Resources.Angle_OnlyRightmostIsDecimal);
                // Is there an hours specifier?
                int startChar = format.IndexOf("H");
                int endChar;
                if (startChar > -1)
                {
                    // Yes. Look for subsequent H characters or a period
                    endChar = format.LastIndexOf("H");
                    // Extract the sub-string
                    subFormat = format.Substring(startChar, endChar - startChar + 1);
                    // Convert to a numberic-formattable string
                    newFormat = subFormat.Replace("H", "0");
                    // Replace the hours
                    if (newFormat.IndexOf(culture.NumberFormat.NumberDecimalSeparator) > -1)
                    {
                        isDecimalHandled = true;
                        format = format.Replace(subFormat, DecimalDegrees.ToString(newFormat, culture));
                    }
                    else
                    {
                        format = format.Replace(subFormat, Hours.ToString(newFormat, culture));
                    }
                }
                // Is there an hours specifier°
                startChar = format.IndexOf("M");
                if (startChar > -1)
                {
                    // Yes. Look for subsequent H characters or a period
                    endChar = format.LastIndexOf("M");
                    // Extract the sub-string
                    subFormat = format.Substring(startChar, endChar - startChar + 1);
                    // Convert to a numberic-formattable string
                    newFormat = subFormat.Replace("M", "0");
                    // Replace the hours
                    if (newFormat.IndexOf(culture.NumberFormat.NumberDecimalSeparator) > -1)
                    {
                        if (isDecimalHandled)
                        {
                            throw new ArgumentException(Resources.Angle_OnlyRightmostIsDecimal);
                        }
                        isDecimalHandled = true;
                        format = format.Replace(subFormat, DecimalMinutes.ToString(newFormat, culture));
                    }
                    else
                    {
                        format = format.Replace(subFormat, Minutes.ToString(newFormat, culture));
                    }
                }
                // Is there an hours specifier°
                startChar = format.IndexOf("S");
                if (startChar > -1)
                {
                    // Yes. Look for subsequent H characters or a period
                    endChar = format.LastIndexOf("S");
                    // Extract the sub-string
                    subFormat = format.Substring(startChar, endChar - startChar + 1);
                    // Convert to a numberic-formattable string
                    newFormat = subFormat.Replace("S", "0");
                    // Replace the hours
                    if (newFormat.IndexOf(culture.NumberFormat.NumberDecimalSeparator) > -1)
                    {
                        if (isDecimalHandled)
                        {
                            throw new ArgumentException(Resources.Angle_OnlyRightmostIsDecimal);
                        }
                        format = format.Replace(subFormat, Seconds.ToString(newFormat, culture));
                    }
                    else
                    {
                        format = format.Replace(subFormat, Seconds.ToString(newFormat, culture));
                    }
                }
                // If nothing then return zero
                if (String.Compare(format, "°", true, culture) == 0)
                    return "0°";
                return format;
            }
            catch
            {
                throw new ArgumentException(Resources.Angle_InvalidToStringFormat);
            }
        }

        #endregion IFormattable Members

        #region IEquatable<Elevation> Members

        /// <summary>
        /// Compares the current instance to another instance using the specified
        /// precision.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decimals">The decimals.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the
        /// <strong>DecimalDegrees</strong> property of the current instance matches the
        /// specified instance's <strong>DecimalDegrees</strong> property.</returns>
        /// <seealso cref="Equals(Elevation)">Equals Method</seealso>
        ///
        /// <example>
        /// These examples compare two fractional values using specific numbers of digits for
        /// comparison.
        ///   <code lang="VB" title="[New Example]">
        /// ' Equals will return False
        /// Dim Elevation1 As New Elevation(90.15);
        /// Dim Elevation2 As New Elevation(90.12);
        /// If Elevation1.Equals(Elevation2, 2) Then
        /// Debug.WriteLine("The values are the same to two digits of precision.");
        /// ' Equals will return True
        /// Dim Elevation1 As New Elevation(90.15);
        /// Dim Elevation2 As New Elevation(90.12);
        /// If Elevation1.Equals(Elevation2, 1) Then
        /// Debug.WriteLine("The values are the same to one digit of precision.");
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// // Equals will return False
        /// Elevation Elevation1 = new Elevation(90.15);
        /// Elevation Elevation2 = new Elevation(90.12);
        /// if (Elevation1.Equals(Elevation2, 2))
        /// Console.WriteLine("The values are the same to two digits of precision.");
        /// // Equals will return True
        /// Elevation Elevation1 = new Elevation(90.15);
        /// Elevation Elevation2 = new Elevation(90.12);
        /// if (Elevation1.Equals(Elevation2, 1))
        /// Console.WriteLine("The values are the same to one digits of precision.");
        ///   </code>
        ///   </example>
        /// <remarks><para>This is typically used in cases where precision is only significant for a few
        /// digits and exact comparison is not necessary.</para>
        ///   <para><em>notice This method compares objects by value, not by
        /// reference.</em></para></remarks>
        public bool Equals(Elevation value, int decimals)
        {
            return Equals(value.DecimalDegrees, decimals);
        }

        /// <summary>
        /// Equalses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Whether the two elevations are equivalent.</returns>
        public bool Equals(Elevation value)
        {
            return Equals(value.DecimalDegrees);
        }

        #endregion IEquatable<Elevation> Members

        #region IComparable<Elevation> Members

        /// <summary>
        /// Returns a value indicating the relative order of two objects.
        /// </summary>
        /// <param name="other">An <strong>Elevation</strong> object to compare with.</param>
        /// <returns>A value of -1, 0, or 1 as documented by the IComparable interface.</returns>
        /// <remarks>This method allows collections of <strong>Azimuth</strong> objects to be sorted.
        /// The <see cref="DecimalDegrees">DecimalDegrees</see> property of each instance is compared.</remarks>
        public int CompareTo(Elevation other)
        {
            return _decimalDegrees.CompareTo(other.DecimalDegrees);
        }

        #endregion IComparable<Elevation> Members

        #region IXmlSerializable Members

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.</returns>
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(_decimalDegrees.ToString("G17", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            _decimalDegrees = reader.NodeType == XmlNodeType.Text ? reader.ReadContentAsDouble() : reader.ReadElementContentAsDouble();
        }

        #endregion IXmlSerializable Members
    }
}