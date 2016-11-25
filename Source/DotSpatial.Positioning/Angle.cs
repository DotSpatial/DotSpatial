﻿// ********************************************************************************************************
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
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using DotSpatial.Positioning.Properties;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents an angular measurement around a circle.
    /// </summary>
    /// <seealso cref="Azimuth">Azimuth Class</seealso>
    ///
    /// <seealso cref="Elevation">Elevation Class</seealso>
    ///
    /// <seealso cref="Latitude">Latitude Class</seealso>
    ///
    /// <seealso cref="Longitude">Longitude Class</seealso>
    ///
    /// <example>
    /// These examples create new instances of Angle objects.
    ///   <code lang="VB" description="Create an angle of 90°">
    /// Dim MyAngle As New Angle(90)
    ///   </code>
    ///   <code lang="CS" description="Create an angle of 90°">
    /// Angle MyAngle = new Angle(90);
    ///   </code>
    ///   <code lang="C++" description="Create an angle of 90°">
    /// Angle MyAngle = new Angle(90);
    ///   </code>
    ///   <code lang="VB" description="Create an angle of 105°30'21.4">
    /// Dim MyAngle1 As New Angle(105, 30, 21.4)
    ///   </code>
    ///   <code lang="CS" description="Create an angle of 105°30'21.4">
    /// Angle MyAngle = new Angle(105, 30, 21.4);
    ///   </code>
    ///   <code lang="C++" description="Create an angle of 105°30'21.4">
    /// Angle MyAngle = new Angle(105, 30, 21.4);
    ///   </code>
    ///   </example>
    /// <remarks><para>This class serves as the base class for angular measurement classes within
    /// the framework, such as the <strong>Azimuth</strong>, <strong>Elevation</strong>,
    ///   <strong>Latitude</strong> and <strong>Longitude</strong> classes. An "angular
    /// measurement" is a measurement around a circle. Typically, angular measurements are
    /// between 0° and 360°.</para>
    ///   <para>Angles can be represented in two forms: decimal and sexagesimal. In decimal
    /// form, angles are represented as a single number. In sexagesimal form, angles are
    /// represented in three components: hours, minutes, and seconds, very much like a
    /// clock.</para>
    ///   <para>Upon creating an <strong>Angle</strong> object, other properties such as
    ///   <strong>DecimalDegrees</strong>, <strong>DecimalMinutes</strong>,
    ///   <strong>Hours</strong>, <strong>Minutes</strong> and <strong>Seconds</strong> are
    /// calculated automatically.</para>
    ///   <para>Instances of this class are guaranteed to be thread-safe because they are
    /// immutable (properties can only be modified via constructors).</para></remarks>
    [TypeConverter("DotSpatial.Positioning.Design.AngleConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=1.0.0.0, PublicKeyToken=b4b0b185210c9dae")]
    public struct Angle : IFormattable, IComparable<Angle>, IEquatable<Angle>, ICloneable<Angle>, IXmlSerializable
    {
        /// <summary>
        ///
        /// </summary>
        private double _decimalDegrees;

        #region Constants

        /// <summary>
        ///
        /// </summary>
        private const int MAXIMUM_PRECISION_DIGITS = 12;

        #endregion Constants

        #region Fields

        /// <summary>
        /// Represents the minimum value of an angle in one turn of a circle.
        /// </summary>
        /// <example>
        /// This example creates an angle representing the minimum allowed value.
        ///   <code lang="VB">
        /// Dim MyAngle As Angle = Angle.Minimum
        ///   </code>
        ///   <code lang="CS">
        /// Angle MyAngle = Angle.Minimum;
        ///   </code>
        ///   <code lang="C++">
        /// Angle MyAngle = Angle.Minimum;
        ///   </code>
        ///   </example>
        ///
        /// <value>An Angle with a value of -359.999999°.</value>
        public static readonly Angle Minimum = new Angle(-359.99999999);

        /// <summary>
        /// Represents an angle with no value.
        /// </summary>
        /// <value>An Angle containing a value of zero (0°).</value>
        ///
        /// <seealso cref="IsEmpty">IsEmpty Property</seealso>
        public static readonly Angle Empty = new Angle(0.0);

        /// <summary>
        /// Represents an angle with infinite value.
        /// </summary>
        public static readonly Angle Infinity = new Angle(double.PositiveInfinity);

        /// <summary>
        /// Represents the maximum value of an angle in one turn of a circle.
        /// </summary>
        /// <example>
        /// This example creates an angle representing the maximum allowed value of 359.9999°.
        ///   <code lang="VB">
        /// Dim MyAngle As Angle = Angle.Maximum
        ///   </code>
        ///   <code lang="CS">
        /// Angle MyAngle = Angle.Maximum;
        ///   </code>
        ///   </example>
        public static readonly Angle Maximum = new Angle(359.99999999);

        /// <summary>
        /// Represents an invalid or unspecified value.
        /// </summary>
        public static readonly Angle Invalid = new Angle(double.NaN);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance with the specified decimal degrees.
        /// </summary>
        /// <param name="decimalDegrees">The decimal degrees.</param>
        /// <example>
        /// This example demonstrates how to create an angle with a measurement of 90°.
        ///   <code lang="VB">
        /// Dim MyAngle As New Angle(90)
        ///   </code>
        ///   <code lang="CS">
        /// Angle MyAngle = new Angle(90);
        ///   </code>
        ///   </example>
        ///
        /// <returns>An <strong>Angle</strong> containing the specified value.</returns>
        public Angle(double decimalDegrees)
        {
            // Set the decimal degrees value
            _decimalDegrees = decimalDegrees;
        }

        /// <summary>
        /// Creates a new instance with the specified degrees.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <returns>An <strong>Angle</strong> containing the specified value.</returns>
        public Angle(int hours)
        {
            _decimalDegrees = ToDecimalDegrees(hours);
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
        /// Dim MyAngle As New Angle(34, 12, 29.2)
        ///   </code>
        ///   <code lang="CS">
        /// Angle MyAngle = new Angle(34, 12, 29.2);
        ///   </code>
        ///   </example>
        ///
        /// <returns>An <strong>Angle</strong> containing the specified value.</returns>
        public Angle(int hours, int minutes, double seconds)
        {
            _decimalDegrees = ToDecimalDegrees(hours, minutes, seconds);
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
        /// Dim MyAngle As New Angle(12, 42.345)
        ///   </code>
        ///   <code lang="VB">
        /// Angle MyAngle = new Angle(12, 42.345);
        ///   </code>
        ///   </example>
        /// <remarks>An <strong>Angle</strong> containing the specified value.</remarks>
        public Angle(int hours, double decimalMinutes)
        {
            _decimalDegrees = ToDecimalDegrees(hours, decimalMinutes);
        }

        /// <summary>
        /// Creates a new instance by converting the specified string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <seealso cref="Angle.Parse(string)">Parse Method</seealso>
        ///
        /// <example>
        /// This example creates a new instance by parsing a string. (Notice The double-quote is
        /// doubled up to represent a single double-quote in the string.)
        ///   <code lang="VB">
        /// Dim MyAngle As New Angle("123°45'67.8""")
        ///   </code>
        ///   <code lang="CS">
        /// Angle MyAngle = new Angle("123°45'67.8\"");
        ///   </code>
        ///   </example>
        ///
        /// <returns>An <strong>Angle</strong> containing the specified value.</returns>
        ///
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">The Parse method requires a decimal or sexagesimal measurement.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">Only the right-most portion of a sexagesimal measurement can be a fractional value.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">Extra characters were encountered while parsing an angular measurement.  Only hours, minutes, and seconds are allowed.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">The specified text was not fully understood as an angular measurement.</exception>
        /// <remarks>This constructor parses the specified string into an <strong>Angle</strong>
        /// object using the current culture. This constructor can parse any strings created via
        /// the <strong>ToString</strong> method.</remarks>
        public Angle(string value)
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
        /// <remarks>This constructor parses the specified string into an <strong>Angle</strong>
        /// object using a specific culture. This constructor can parse any strings created via the
        /// <strong>ToString</strong> method.</remarks>
        public Angle(string value, CultureInfo culture)
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
                        // Return a blank Angle
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
                        if (values[0].Length == 7 && values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator, StringComparison.CurrentCulture) == -1)
                        {
                            _decimalDegrees = ToDecimalDegrees(
                                int.Parse(values[0].Substring(0, 3), culture),
                                int.Parse(values[0].Substring(3, 2), culture),
                                double.Parse(values[0].Substring(5, 2), culture));
                            return;
                        }
                        if (values[0].Length == 8 && values[0][0] == '-' && values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator, StringComparison.CurrentCulture) == -1)
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
                        if (values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator, StringComparison.Ordinal) != -1)
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
                        _decimalDegrees = ToDecimalDegrees(int.Parse(values[0], culture),
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
        public Angle(XmlReader reader)
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
        /// Dim MyAngle As New Angle(20, 30)
        /// ' Setting the DecimalMinutes recalculated other properties
        /// Debug.WriteLine(MyAngle.DecimalDegrees)
        /// ' Output: "20.5"  the same as 20°30'
        ///   </code>
        ///   <code lang="CS">
        /// // Create an angle of 20°30'
        /// Angle MyAngle = New Angle(20, 30);
        /// // Setting the DecimalMinutes recalculated other properties
        /// Console.WriteLine(MyAngle.DecimalDegrees)
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
        /// Dim MyAngle As New Angle(20, 10, 30)
        /// ' The DecimalMinutes property is automatically calculated
        /// Debug.WriteLine(MyAngle.DecimalMinutes)
        /// ' Output: "10.5"
        ///   </code>
        ///   <code lang="CS">
        /// // Create an angle of 20°10'30"
        /// Angle MyAngle = new Angle(20, 10, 30);
        /// // The DecimalMinutes property is automatically calculated
        /// Console.WriteLine(MyAngle.DecimalMinutes)
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
        /// Dim MyAngle As New Angle(60.5)
        /// Debug.WriteLine(MyAngle.Hours)
        /// ' Output: 60
        ///   </code>
        ///   <code lang="CS">
        /// Angle MyAngle = new Angle(60.5);
        /// Console.WriteLine(MyAngle.Hours);
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
        /// Dim MyAngle As New Angle(45.5)
        /// Debug.WriteLine(MyAngle.Minutes)
        /// ' Output: 30
        ///   </code>
        ///   <code lang="CS">
        /// Angle MyAngle = new Angle(45.5);
        /// Console.WriteLine(MyAngle.Minutes);
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
        /// Dim MyAngle As New Angle(45, 10.5)
        /// Debug.WriteLine(MyAngle.Seconds)
        /// ' Output: 30
        ///   </code>
        ///   <code lang="CS">
        /// Dim MyAngle As New Angle(45, 10.5);
        /// Console.WriteLine(MyAngle.Seconds);
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
        /// Indicates whether the value is invalid or unspecified.
        /// </summary>
        public bool IsInvalid
        {
            get { return double.IsNaN(_decimalDegrees); }
        }

        /// <summary>
        /// Indicates whether the value has been normalized and is within the
        /// allowed bounds of 0° and 360°.
        /// </summary>
        public bool IsNormalized
        {
            get { return _decimalDegrees >= 0 && _decimalDegrees < 360; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns the object with the largest value.
        /// </summary>
        /// <param name="value">An <strong>Angle</strong> object to compare to the current instance.</param>
        /// <returns>An <strong>Angle</strong> containing the largest value.</returns>
        public Angle GreaterOf(Angle value)
        {
            if (_decimalDegrees > value.DecimalDegrees)
                return this;
            return value;
        }

        /// <summary>
        /// Returns the object with the smallest value.
        /// </summary>
        /// <param name="value">An <strong>Angle</strong> object to compare to the current instance.</param>
        /// <returns>The <strong>Angle</strong> containing the smallest value.</returns>
        public Angle LesserOf(Angle value)
        {
            if (_decimalDegrees < value.DecimalDegrees)
                return this;
            return value;
        }

        /// <summary>
        /// Returns an angle opposite of the current instance.
        /// </summary>
        /// <returns>An <strong>Angle</strong> representing the mirrored value.</returns>
        /// <example>
        /// This example creates a new <strong>Angle</strong> of 45° then calculates its mirror
        /// of 225°. (45 + 180)
        ///   <code lang="VB" title="[New Example]">
        /// Dim Angle1 As New Angle(45)
        /// Dim Angle2 As Angle = Angle1.Mirror()
        /// Debug.WriteLine(Angle2.ToString())
        /// ' Output: 225
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Angle Angle1 = new Angle(45);
        /// Angle Angle2 = Angle1.Mirror();
        /// Console.WriteLine(Angle2.ToString());
        /// // Output: 225
        ///   </code>
        ///   </example>
        /// <remarks>This method returns the "opposite" of the current instance. The opposite is
        /// defined as the point on the other side of an imaginary circle. For example, if an angle
        /// is 0°, at the top of a circle, this method returns 180°, at the bottom of the
        /// circle.</remarks>
        public Angle Mirror()
        {
            return new Angle(_decimalDegrees + 180.0).Normalize();
        }

        /// <summary>
        /// Modifies a value to its equivalent between 0° and 360°.
        /// </summary>
        /// <returns>An <strong>Angle</strong> representing the normalized angle.</returns>
        /// <seealso cref="Normalize(double)">Normalize(Angle) Method</seealso>
        ///
        /// <example>
        /// This example demonstrates how normalization is used. The Stop statement is hit.
        /// This example demonstrates how the Normalize method can ensure that an angle fits
        /// between 0° and 359.9999°. This example normalizes 725° into 5°.
        ///   <code lang="VB">
        /// Dim MyAngle As New Angle(720)
        /// MyAngle = MyAngle.Normalize()
        ///   </code>
        ///   <code lang="CS">
        /// Angle MyAngle = new Angle(720);
        /// MyAngle = MyAngle.Normalize();
        ///   </code>
        ///   <code lang="VB">
        /// Dim MyValue As New Angle(725)
        /// MyValue = MyValue.Normalize()
        ///   </code>
        ///   <code lang="CS">
        /// Angle MyValue = new Angle(725);
        /// MyValue = MyValue.Normalize();
        ///   </code>
        ///   </example>
        /// <remarks>This function is used to ensure that an angular measurement is within the
        /// allowed bounds of 0° and 360°. If a value of 360° or 720° is passed, a value of 0°
        /// is returned since 360° and 720° represent the same point on a circle. For the Angle
        /// class, this function is the same as "value Mod 360".</remarks>
        public Angle Normalize()
        {
            double value = _decimalDegrees;
            while (value < 0)
            {
                value += 360.0;
            }
            return new Angle(value % 360);
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
        /// Dim MyAngle As New Angle(90)
        /// Dim MyRadians As Radian = MyAngle.ToRadians()
        ///   </code>
        ///   <code lang="CS">
        /// Angle MyAngle = new Angle(90);
        /// Radian MyRadians = MyAngle.ToRadians();
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
        /// Dim MyAngle As New Angle(45, 16.772)
        /// Debug.WriteLine(MyAngle.ToString("h°m.mm"))
        /// ' Output: 45°16.78
        ///   </code>
        ///   <code lang="CS">
        /// Dim MyAngle As New Angle(45, 16.772);
        /// Debug.WriteLine(MyAngle.ToString("h°m.mm"));
        /// // Output: 45°16.78
        ///   </code>
        ///   </example>
        /// <remarks>This method returns the current instance output in a specific format. If no
        /// value for the format is specified, a default format of "d.dddd°" is used. Any
        /// string output by this method can be converted back into an Angle object using the
        /// <strong>Parse</strong> method or <strong>Angle(string)</strong> constructor.</remarks>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns the smallest integer greater than the specified value.
        /// </summary>
        /// <returns></returns>
        public Angle Ceiling()
        {
            return new Angle(Math.Ceiling(_decimalDegrees));
        }

        /// <summary>
        /// Returns the largest integer which is smaller than the specified value.
        /// </summary>
        /// <returns></returns>
        public Angle Floor()
        {
            return new Angle(Math.Floor(_decimalDegrees));
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
        /// Returns a new instance whose Seconds property is evenly divisible by 15.
        /// </summary>
        /// <returns>An <strong>Angle</strong> containing the rounded value.</returns>
        /// <remarks>This method is used to align or "snap" an angle to a regular interval. For
        /// example, a grid might be easier to read if it were drawn at 30-second intervals instead
        /// of 24.198-second intervals.</remarks>
        public Angle RoundSeconds()
        {
            return RoundSeconds(15.0);
        }

        /// <summary>
        /// Returns a new instance whose value is rounded the specified number of decimals.
        /// </summary>
        /// <param name="decimals">An <strong>Integer</strong> specifying the number of decimals to round off to.</param>
        /// <returns></returns>
        public Angle Round(int decimals)
        {
            return new Angle(Math.Round(_decimalDegrees, decimals));
        }

        /// <summary>
        /// Returns a new angle whose Seconds property is evenly divisible by the specified amount.
        /// </summary>
        /// <param name="interval">A <strong>Double</strong> between 0 and 60 indicating the interval to round
        /// to.</param>
        /// <returns>An <strong>Angle</strong> containing the rounded value.</returns>
        /// <remarks>This method is used to align or "snap" an angle to a regular interval. For
        /// example, a grid might be easier to read if it were drawn at 30-second intervals instead
        /// of 24.198-second intervals.</remarks>
        public Angle RoundSeconds(double interval)
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
                return new Angle(Hours, Minutes, newSeconds);
            }
            // return the new value
            return new Angle(Hours, Minutes, newSeconds);
        }

        #endregion Public Methods

        #region Overrides

        /// <summary>
        /// Compares the current value to another Angle object's value.
        /// </summary>
        /// <param name="obj">An <strong>Angle</strong>, <strong>Double</strong>, or <strong>Integer</strong>
        /// to compare with.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the object's DecimalDegrees
        /// properties match.</returns>
        /// <remarks>This</remarks>
        public override bool Equals(object obj)
        {
            // Convert objects to an Angle as needed before comparison
            if (obj is Angle)
                return Equals((Angle)obj);

            // Nothing else will work, so False
            return false;
        }

        /// <summary>
        /// Returns a unique code for this instance.
        /// </summary>
        /// <returns>An <strong>Integer</strong> representing a unique code for the current
        /// instance.</returns>
        /// <remarks>Since the <strong>Angle</strong> class is immutable, this property may be used
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
        /// Dim MyAngle As New Angle(90)
        /// Debug.WriteLine(MyAngle.ToString)
        /// ' Output: "90°"
        ///   </code>
        ///   <code lang="CS">
        /// Angle MyAngle = new Angle(90);
        /// Debug.WriteLine(MyAngle.ToString());
        /// // Output: "90°"
        ///   </code>
        ///   </example>
        /// <remarks>This method formats the current instance using the default format of
        /// "d.dddd°." Any string output by this method can be converted back into an Angle
        /// object using the <strong>Parse</strong> method or <strong>Angle(string)</strong>
        /// constructor.</remarks>
        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion Overrides

        #region Static Methods

        /// <summary>
        /// Converts the specified value to its equivalent between 0° and 360°.
        /// </summary>
        /// <param name="decimalDegrees">A <strong>Double</strong> value to be normalized.</param>
        /// <returns>An Angle containing a value equivalent to the value specified, but between 0° and
        /// 360°.</returns>
        public static Angle Normalize(double decimalDegrees)
        {
            return new Angle(decimalDegrees).Normalize();
        }

        /// <summary>
        /// Converts an angular measurement into radians.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="Radian"><strong>Radian</strong></see> object.</returns>
        /// <example>
        /// This example shows a quick way to convert an angle of 90° into radians.
        ///   <code lang="VB">
        /// Dim MyRadian As Radian = Angle.ToRadians(90)
        ///   </code>
        ///   <code lang="CS">
        /// Radian MyRadian = Angle.ToRadians(90);
        ///   </code>
        ///   </example>
        /// <remarks>This function is typically used to convert an angular measurement into
        /// radians before performing a trigonometric function.</remarks>
        public static Radian ToRadians(Angle value)
        {
            return value.ToRadians();
        }

        /// <summary>
        /// Converts a value in radians into an angular measurement.
        /// </summary>
        /// <param name="radians">The radians.</param>
        /// <returns></returns>
        /// <seealso cref="Angle.ToRadians()">ToRadians</seealso>
        ///
        /// <seealso cref="Radian">Radian Class</seealso>
        ///
        /// <example>
        /// This example uses the <strong>FromRadians</strong> method to convert a value of one
        /// radian into an <strong>Angle</strong> of 57°.
        ///   <code lang="VB">
        /// ' Create a new angle equal to one radian
        /// Dim MyRadians As New Radian(1)
        /// Dim MyAngle As Angle = Angle.FromRadians(MyRadians)
        /// Debug.WriteLine(MyAngle.ToString())
        /// ' Output: 57°
        ///   </code>
        ///   <code lang="CS">
        /// // Create a new angle equal to one radian
        /// Radian MyRadians = new Radian(1);
        /// Angle MyAngle = Angle.FromRadians(MyRadians);
        /// Console.WriteLine(MyAngle.ToString());
        /// // Output: 57°
        ///   </code>
        ///   </example>
        /// <remarks>This function is typically used in conjunction with the
        /// <see cref="Angle.ToRadians()">ToRadians</see>
        /// method after a trigonometric function has completed. The converted value is stored in
        /// the <see cref="DecimalDegrees">DecimalDegrees</see> property.</remarks>
        public static Angle FromRadians(Radian radians)
        {
            return radians.ToAngle();
        }

        /// <summary>
        /// Froms the radians.
        /// </summary>
        /// <param name="radians">The radians.</param>
        /// <returns></returns>
        public static Angle FromRadians(double radians)
        {
            return new Angle(Radian.ToDegrees(radians));
        }

        /// <summary>
        /// Convers a sexagesimal number into an Angle.
        /// </summary>
        /// <param name="dms">A Double value, a number in the form of DDD.MMSSSSS format</param>
        /// <returns>An <strong>Angle</strong> object.</returns>
        public static Angle FromSexagesimal(double dms)
        {
            // Shift the decimal left 2 places and save it.
            double dmsX100 = dms * 100;
            // Get the integral portion for the hours
            int hours = (int)Math.Floor(dms);
            // Subtract the hours from the sexagesimal number, leaving just the fractional portion,
            // shift the decimal left 2 places and truncate for the minutes
            int minutes = (int)Math.Floor(Math.Round((dms - hours) * 100));
            // Subtract the integral portion of the shifted sexagesimal number for the shifted sexagesimal number,
            // leaving just the fractional portion, shift the decimal left 2 places and truncate for the seconds.
            double seconds = (dmsX100 - Math.Floor(dmsX100)) * 100;

            // Create an Angle from the hours, minutes and seconds
            return new Angle(hours, Math.Abs(minutes), seconds);
        }

        /// <summary>
        /// Returns the object with the smallest value.
        /// </summary>
        /// <param name="value1">A <strong>Angle</strong> object to compare to value2.</param>
        /// <param name="value2">A <strong>Angle</strong> object to compare to value1.</param>
        /// <returns>The <strong>Angle</strong> containing the smallest value.</returns>
        public static Angle LesserOf(Angle value1, Angle value2)
        {
            return value1.LesserOf(value2);
        }

        /// <summary>
        /// Returns the object with the largest value.
        /// </summary>
        /// <param name="value1">A <strong>Angle</strong> object to compare to value2.</param>
        /// <param name="value2">A <strong>Angle</strong> object to compare to value1</param>
        /// <returns>A <strong>Angle</strong> containing the largest value.</returns>
        public static Angle GreaterOf(Angle value1, Angle value2)
        {
            return value1.GreaterOf(value2);
        }

        /// <summary>
        /// Returns a random angle between 0° and 360°.
        /// </summary>
        /// <returns>An <strong>Angle</strong> containing a random value.</returns>
        public static Angle Random()
        {
            return Random(new Random(DateTime.Now.Millisecond));
        }

        /// <summary>
        /// Returns a random Angle between 0° and 360°
        /// </summary>
        /// <param name="generator">A <strong>Random</strong> object used to ogenerate random values.</param>
        /// <returns>An <strong>Angle</strong> containing a random value.</returns>
        public static Angle Random(Random generator)
        {
            return new Angle(generator.NextDouble() * 360.0);
        }

        #endregion Static Methods

        #region Conversions

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Angle.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Angle(double value)
        {
            return new Angle(value);
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Single into an Angle.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Angle(float value)
        {
            return new Angle(Convert.ToDouble(value));
        }

        /// <summary>
        /// Converts a measurement in Radians into an Angle.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Angle(Radian value)
        {
            return value.ToAngle();
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Angle into an Double.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator double(Angle value)
        {
            return value.DecimalDegrees;
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Angle into a Single.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator float(Angle value)
        {
            return Convert.ToSingle(value.DecimalDegrees);
        }

        /// <summary>
        /// Converts a measurement in degrees as an Integer into an Angle.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Angle(int value)
        {
            return new Angle(value);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Azimuth"/> to <see cref="DotSpatial.Positioning.Angle"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Angle(Azimuth value)
        {
            return new Angle(value.DecimalDegrees);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Elevation"/> to <see cref="DotSpatial.Positioning.Angle"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Angle(Elevation value)
        {
            return new Angle(value.DecimalDegrees);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Latitude"/> to <see cref="DotSpatial.Positioning.Angle"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Angle(Latitude value)
        {
            return new Angle(value.DecimalDegrees);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Longitude"/> to <see cref="DotSpatial.Positioning.Angle"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Angle(Longitude value)
        {
            return new Angle(value.DecimalDegrees);
        }

        /// <summary>
        /// Converts a measurement in the form of a formatted String into an Angle.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Angle(string value)
        {
            return new Angle(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts an Angle into a String.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks>This operator calls the ToString() method using the current culture.</remarks>
        public static explicit operator string(Angle value)
        {
            return value.ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion Conversions

        #region Operators

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Angle operator +(Angle left, Angle right)
        {
            return new Angle(left.DecimalDegrees + right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Angle operator +(Angle left, double right)
        {
            return new Angle(left.DecimalDegrees + right);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Angle operator -(Angle left, Angle right)
        {
            return new Angle(left.DecimalDegrees - right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Angle operator -(Angle left, double right)
        {
            return new Angle(left.DecimalDegrees - right);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Angle operator *(Angle left, Angle right)
        {
            return new Angle(left.DecimalDegrees * right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Angle operator *(Angle left, double right)
        {
            return new Angle(left.DecimalDegrees * right);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Angle operator /(Angle left, Angle right)
        {
            return new Angle(left.DecimalDegrees / right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Angle operator /(Angle left, double right)
        {
            return new Angle(left.DecimalDegrees / right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Angle left, Angle right)
        {
            return left.DecimalDegrees.Equals(right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Angle left, double right)
        {
            return left.DecimalDegrees.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Angle left, Angle right)
        {
            return !left.DecimalDegrees.Equals(right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Angle left, double right)
        {
            return !left.DecimalDegrees.Equals(right);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Angle left, Angle right)
        {
            return left.DecimalDegrees > right.DecimalDegrees;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Angle left, double right)
        {
            return left.DecimalDegrees > right;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(Angle left, Angle right)
        {
            return left.DecimalDegrees >= right.DecimalDegrees;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(Angle left, double right)
        {
            return left.DecimalDegrees >= right;
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Angle left, Angle right)
        {
            return left.DecimalDegrees < right.DecimalDegrees;
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Angle left, double right)
        {
            return left.DecimalDegrees < right;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(Angle left, Angle right)
        {
            return left.DecimalDegrees <= right.DecimalDegrees;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(Angle left, double right)
        {
            return left.DecimalDegrees <= right;
        }

        /// <summary>
        /// Returns the current instance increased by one.
        /// </summary>
        /// <returns>An <strong>Angle</strong> object.</returns>
        /// <example>
        /// This example uses the <strong>Increment</strong> method to increase an angle's
        /// value. It also demonstrates the subtle error which can be caused if
        ///   <strong>Increment</strong> is called while ignoring the return value.
        ///   <code lang="VB" title="[New Example]">
        /// ' Correct use of Increment
        /// Dim Angle1 As New Angle(89)
        /// Angle1 = Angle1.Increment()
        /// ' Incorrect use of Increment
        /// Dim Angle1 = New Angle(89)
        /// Angle1.Increment()
        /// ' Notice: Angle1 will still be 89°!
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// // Correct use of Increment
        /// Angle Angle1 = new Angle(89);
        /// Angle1 = Angle1.Increment();
        /// // Incorrect use of Increment
        /// Angle Angle1 = new Angle(89);
        /// Angle1.Increment();
        /// // Notice: Angle1 will still be 89°!
        ///   </code>
        ///   </example>
        /// <remarks><para>This method increases the <strong>DecimalDegrees</strong> property by 1.0,
        /// returned as a new instance.</para>
        ///   <para><font color="red">Since the <strong>Angle</strong> class is immutable, this
        /// method cannot be used to modify an existing instance.</font></para></remarks>
        public Angle Increment()
        {
            return new Angle(_decimalDegrees + 1.0);
        }

        /// <summary>
        /// Increases the current instance by the specified value.
        /// </summary>
        /// <param name="value">A <strong>Double</strong> to add to the current instance.</param>
        /// <returns>A new <strong>Angle</strong> containing the summed values.</returns>
        /// <example>
        /// This example adds 45° to the current instance of 45°, returning 90°.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Angle1 As New Angle(45)
        /// Angle1 = Angle1.Add(45)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Angle Angle1 = new Angle(45);
        /// Angle1 = Angle1.Add(45);
        ///   </code>
        ///   </example>
        public Angle Add(double value)
        {
            return new Angle(_decimalDegrees + value);
        }

        /// <summary>
        /// Adds the specified angle.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        public Angle Add(Angle angle)
        {
            return new Angle(_decimalDegrees + angle.DecimalDegrees);
        }

        /// <summary>
        /// Returns the current instance decreased by one.
        /// </summary>
        /// <returns>An <strong>Angle</strong> object.</returns>
        /// <example>
        /// This example uses the <strong>Decrement</strong> method to decrease an angle's
        /// value. It also demonstrates the subtle error which can be caused if
        ///   <strong>Decrement</strong> is called while ignoring the return value.
        ///   <code lang="VB" title="[New Example]">
        /// ' Correct use of Decrement
        /// Dim Angle1 As New Angle(91)
        /// Angle1 = Angle1.Decrement()
        /// ' Incorrect use of Decrement
        /// Dim Angle1 = New Angle(91)
        /// Angle1.Increment()
        /// ' NOTE: Angle1 will still be 91°!
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// // Correct use of Decrement
        /// Angle Angle1 = new Angle(91);
        /// Angle1 = Angle1.Decrement();
        /// // Incorrect use of Decrement
        /// Angle Angle1 = new Angle(91);
        /// Angle1.Decrement();
        /// // NOTE: Angle1 will still be 91°!
        ///   </code>
        ///   </example>
        /// <remarks><para>This method decreases the <strong>DecimalDegrees</strong> property by 1.0,
        /// returned as a new instance.</para>
        ///   <para><font color="red">Since the <strong>Angle</strong> class is immutable, this
        /// method cannot be used to modify an existing instance.</font></para></remarks>
        public Angle Decrement()
        {
            return new Angle(_decimalDegrees - 1.0);
        }

        /// <summary>
        /// Decreases the current instance by the specified value.
        /// </summary>
        /// <param name="value">A <strong>Double</strong> to subtract from the current instance.</param>
        /// <returns>A new <strong>Angle</strong> containing the new value.</returns>
        /// <example>
        /// This example subtracts 30° from the current instance of 90°, returning 60°.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Angle1 As New Angle(90)
        /// Angle1 = Angle1.Subtract(30)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Angle Angle1 = new Angle(90);
        /// Angle1 = Angle1.Subtract(30);
        ///   </code>
        ///   </example>
        public Angle Subtract(double value)
        {
            return new Angle(_decimalDegrees - value);
        }

        /// <summary>
        /// Subtracts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Angle Subtract(Angle value)
        {
            return new Angle(_decimalDegrees - value.DecimalDegrees);
        }

        /// <summary>
        /// Multiplies the current instance by the specified value.
        /// </summary>
        /// <param name="value">A <strong>Double</strong> to multiply with the current instance.</param>
        /// <returns>A new <strong>Angle</strong> containing the product of the two numbers.</returns>
        /// <example>
        /// This example multiplies 30° with three, returning 90°.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Angle1 As New Angle(30)
        /// Angle1 = Angle1.Multiply(3)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Angle Angle1 = new Angle(30);
        /// Angle1 = Angle1.Multiply(3);
        ///   </code>
        ///   </example>
        public Angle Multiply(double value)
        {
            return new Angle(_decimalDegrees * value);
        }

        /// <summary>
        /// Multiplies the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Angle Multiply(Angle value)
        {
            return new Angle(_decimalDegrees * value.DecimalDegrees);
        }

        /// <summary>
        /// Divides the current instance by the specified value.
        /// </summary>
        /// <param name="value">A <strong>Double</strong> representing a denominator to divide by.</param>
        /// <returns>An <strong>Angle</strong> containing the new value.</returns>
        /// <example>
        /// This example divides 90° by three, returning 30°.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Angle1 As New Angle(90)
        /// Angle1 = Angle1.Divide(3)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Angle Angle1 = new Angle(90);
        /// Angle1 = Angle1.Divide(3);
        ///   </code>
        ///   </example>
        public Angle Divide(double value)
        {
            return new Angle(_decimalDegrees / value);
        }

        /// <summary>
        /// Divides the specified angle.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        public Angle Divide(Angle angle)
        {
            return new Angle(_decimalDegrees / angle.DecimalDegrees);
        }

        /// <summary>
        /// Indicates if the current instance is smaller than the specified value.
        /// </summary>
        /// <param name="value">An <strong>Angle</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// smaller than the specified value.</returns>
        public bool IsLessThan(Angle value)
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
        /// <param name="value">An <strong>Angle</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// smaller than or equal to the specified value.</returns>
        /// <remarks>This method compares the <strong>DecimalDegrees</strong> property with the
        /// specified value. This method is the same as the "&lt;=" operator.</remarks>
        public bool IsLessThanOrEqualTo(Angle value)
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
        /// <param name="value">An <strong>Angle</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// greater than the specified value.</returns>
        public bool IsGreaterThan(Angle value)
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
        /// <param name="value">An <strong>Angle</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// greater than or equal to the specified value.</returns>
        public bool IsGreaterThanOrEqualTo(Angle value)
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

        #region Static methods

        /// <summary>
        /// Converts the specified string into an Angle object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A new <strong>Angle</strong> object populated with the specified
        /// values.</returns>
        /// <seealso cref="ToString()">ToString Method</seealso>
        ///
        /// <example>
        /// This example creates a new angular measurement using the <strong>Parse</strong>
        /// method.
        ///   <code lang="VB">
        /// Dim NewAngle As Angle = Angle.Parse("123.45°")
        ///   </code>
        ///   <code lang="CS">
        /// Angle NewAngle = Angle.Parse("123.45°");
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
        /// <remarks>This method parses the specified string into an <strong>Angle</strong> object
        /// using the current culture. This constructor can parse any strings created via the
        /// <strong>ToString</strong> method.</remarks>
        public static Angle Parse(string value)
        {
            return new Angle(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts the specified string into an <strong>Angle</strong> object using the
        /// specified culture.
        /// </summary>
        /// <param name="value">A <strong>String</strong> describing an angle in the form of decimal degrees or a
        /// sexagesimal.</param>
        /// <param name="culture">A <strong>CultureInfo</strong> object describing the numeric format to use during
        /// conversion.</param>
        /// <returns>A new <strong>Angle</strong> object equivalent to the specified string.</returns>
        /// <remarks>This powerful method is typically used to process data from a data store or a
        /// value input by the user in any culture. This function can accept any format which
        /// can be output by the ToString method.</remarks>
        public static Angle Parse(string value, CultureInfo culture)
        {
            return new Angle(value, culture);
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

        #endregion Static methods

        #region ICloneable<Angle> Members

        /// <summary>
        /// Creates a copy of the current instance.
        /// </summary>
        /// <returns>An <strong>Angle</strong> of the same value as the current instance.</returns>
        public Angle Clone()
        {
            return new Angle(_decimalDegrees);
        }

        #endregion ICloneable<Angle> Members

        #region IComparable<Angle> Members

        /// <summary>
        /// Returns a value indicating the relative order of two objects.
        /// </summary>
        /// <param name="other">An <strong>Angle</strong> object to compare with.</param>
        /// <returns>A value of -1, 0, or 1 as documented by the IComparable interface.</returns>
        /// <remarks>This method allows collections of <strong>Azimuth</strong> objects to be sorted.
        /// The <see cref="DecimalDegrees">DecimalDegrees</see> property of each instance is compared.</remarks>
        public int CompareTo(Angle other)
        {
            return _decimalDegrees.CompareTo(other.DecimalDegrees);
        }

        #endregion IComparable<Angle> Members

        #region IEquatable<Angle> Members

        /// <summary>
        /// Compares the current instance to another instance using the specified
        /// precision.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the
        /// <strong>DecimalDegrees</strong> property of the current instance matches the
        /// specified instance's <strong>DecimalDegrees</strong> property.</returns>
        /// <seealso cref="Equals(Angle, int)">Equals Method</seealso>
        ///
        /// <example>
        /// These examples compare two fractional values using specific numbers of digits for
        /// comparison.
        ///   <code lang="VB" title="[New Example]">
        /// ' Equals will return False
        /// Dim Angle1 As New Angle(90.15);
        /// Dim Angle2 As New Angle(90.12);
        /// If Angle1.Equals(Angle2, 2) Then
        /// Debug.WriteLine("The values are the same to two digits of precision.");
        /// ' Equals will return True
        /// Dim Angle1 As New Angle(90.15);
        /// Dim Angle2 As New Angle(90.12);
        /// If Angle1.Equals(Angle2, 1) Then
        /// Debug.WriteLine("The values are the same to one digit of precision.");
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// // Equals will return False
        /// Angle Angle1 = new Angle(90.15);
        /// Angle Angle2 = new Angle(90.12);
        /// if (Angle1.Equals(Angle2, 2))
        /// Console.WriteLine("The values are the same to two digits of precision.");
        /// // Equals will return True
        /// Angle Angle1 = new Angle(90.15);
        /// Angle Angle2 = new Angle(90.12);
        /// if (Angle1.Equals(Angle2, 1))
        /// Console.WriteLine("The values are the same to one digits of precision.");
        ///   </code>
        ///   </example>
        /// <remarks><para>This is typically used in cases where precision is only significant for a few
        /// digits and exact comparison is not necessary.</para>
        ///   <para><em>NOTE: This method compares objects by value, not by
        /// reference.</em></para></remarks>
        public bool Equals(Angle angle)
        {
            return _decimalDegrees.Equals(angle.DecimalDegrees);
        }

        /// <summary>
        /// Compares the current instance to another instance using the specified
        /// precision.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <param name="decimals">The decimals.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the
        /// <strong>DecimalDegrees</strong> property of the current instance matches the
        /// specified instance's <strong>DecimalDegrees</strong> property.</returns>
        /// <seealso cref="Equals(Angle, int)">Equals Method</seealso>
        ///
        /// <example>
        /// These examples compare two fractional values using specific numbers of digits for
        /// comparison.
        ///   <code lang="VB" title="[New Example]">
        /// ' Equals will return False
        /// Dim Angle1 As New Angle(90.15);
        /// Dim Angle2 As New Angle(90.12);
        /// If Angle1.Equals(Angle2, 2) Then
        /// Debug.WriteLine("The values are the same to two digits of precision.");
        /// ' Equals will return True
        /// Dim Angle1 As New Angle(90.15);
        /// Dim Angle2 As New Angle(90.12);
        /// If Angle1.Equals(Angle2, 1) Then
        /// Debug.WriteLine("The values are the same to one digit of precision.");
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// // Equals will return False
        /// Angle Angle1 = new Angle(90.15);
        /// Angle Angle2 = new Angle(90.12);
        /// if (Angle1.Equals(Angle2, 2))
        /// Console.WriteLine("The values are the same to two digits of precision.");
        /// // Equals will return True
        /// Angle Angle1 = new Angle(90.15);
        /// Angle Angle2 = new Angle(90.12);
        /// if (Angle1.Equals(Angle2, 1))
        /// Console.WriteLine("The values are the same to one digits of precision.");
        ///   </code>
        ///   </example>
        /// <remarks><para>This is typically used in cases where precision is only significant for a few
        /// digits and exact comparison is not necessary.</para>
        ///   <para><em>NOTE: This method compares objects by value, not by
        /// reference.</em></para></remarks>
        public bool Equals(Angle angle, int decimals)
        {
            return Math.Round(_decimalDegrees, decimals).Equals(Math.Round(angle.DecimalDegrees, decimals));
        }

        #endregion IEquatable<Angle> Members

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
        /// Dim MyAngle As New Angle(45, 16.772)
        /// Debug.WriteLine(MyAngle.ToString("h°m.mm", CultureInfo.CurrentCulture))
        /// ' Output: 45°16.78
        ///   </code>
        ///   <code lang="CS">
        /// Dim MyAngle As New Angle(45, 16.772);
        /// Debug.WriteLine(MyAngle.ToString("h°m.mm", CultureInfo.CurrentCulture));
        /// // Output: 45°16.78
        ///   </code>
        ///   </example>
        /// <remarks>This method returns the current instance output in a specific format. If no
        /// value for the format is specified, a default format of "d.dddd" is used. Any string
        /// output by this method can be converted back into an Angle object using the
        /// <strong>Parse</strong> method or <strong>Angle(string)</strong> constructor.</remarks>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider ?? CultureInfo.CurrentCulture;

            if (string.IsNullOrEmpty(format))
                format = "G";

            string subFormat;
            string newFormat;
            bool isDecimalHandled = false;
            try
            {
                // Is it infinity?
                if (double.IsPositiveInfinity(_decimalDegrees))
                    return "+" + Resources.Common_Infinity;
                // Is it infinity?
                if (double.IsNegativeInfinity(_decimalDegrees))
                    return "-" + Resources.Common_Infinity;
                if (double.IsNaN(_decimalDegrees))
                    return "NaN";

                // Use the default if "g" is passed
                if (String.Compare(format, "g", StringComparison.OrdinalIgnoreCase) == 0)
                    format = "d.dddd°";

                // Replace the "d" with "h" since degrees is the same as hours
                format = format.ToUpper(CultureInfo.InvariantCulture).Replace("D", "H");

                // Only one decimal is allowed
                if (format.IndexOf(culture.NumberFormat.NumberDecimalSeparator, StringComparison.Ordinal) !=
                    format.LastIndexOf(culture.NumberFormat.NumberDecimalSeparator, StringComparison.Ordinal))
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
            /* This class conforms to GML version 3.0.
             *
             * <gml:angle uom="degrees">123.45678</gml:angle>
             *
             */

            // Write the start element
            writer.WriteStartElement(Xml.GML_XML_PREFIX, "angle", Xml.GML_XML_NAMESPACE);

            // Write the units-of-measure (degrees)
            writer.WriteAttributeString("uom", "degrees");

            // Write the angle value
            writer.WriteString(_decimalDegrees.ToString("G17", CultureInfo.InvariantCulture));

            // Close up the element
            writer.WriteEndElement();
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            /* This class conforms to GML version 3.0.
             *
             * <gml:angle uom="degrees">123.45678</gml:angle>
             *
            */

            // Move to the <gml:angle> element
            if (!reader.IsStartElement("angle", Xml.GML_XML_NAMESPACE))
                reader.ReadToDescendant("angle", Xml.GML_XML_NAMESPACE);

            // Read in the element content
            // I'm going to assume for now that the unit of measure is degrees.
            _decimalDegrees = reader.ReadElementContentAsDouble();

            reader.Read();
        }

        #endregion IXmlSerializable Members
    }
}