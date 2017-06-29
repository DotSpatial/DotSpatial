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
using System.Text;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

#if !PocketPC || DesignTime

using System.ComponentModel;

#endif

namespace DotSpatial.Positioning
{
#if !PocketPC || DesignTime
    /// <summary>
    /// Represents a line of constant distance north or south of the equator.
    /// </summary>
    /// <seealso cref="Longitude">Longitude Class</seealso>
    ///
    /// <seealso cref="Position">Position Class</seealso>
    ///
    /// <seealso cref="Azimuth">Azimuth Class</seealso>
    ///
    /// <seealso cref="Elevation">Elevation Class</seealso>
    ///
    /// <seealso cref="Angle">Angle Class</seealso>
    ///
    /// <example>
    /// These examples create new instances of Latitude objects.
    ///   <code lang="VB" description="Create an angle of 90°">
    /// Dim MyLatitude As New Latitude(90)
    ///   </code>
    ///   <code lang="CS" description="Create an angle of 90°">
    /// Latitude MyLatitude = new Latitude(90);
    ///   </code>
    ///   <code lang="C++" description="Create an angle of 90°">
    /// Latitude MyLatitude = new Latitude(90);
    ///   </code>
    ///   <code lang="VB" description="Create an angle of 105°30'21.4">
    /// Dim MyLatitude1 As New Latitude(105, 30, 21.4)
    ///   </code>
    ///   <code lang="CS" description="Create an angle of 105°30'21.4">
    /// Latitude MyLatitude = new Latitude(105, 30, 21.4);
    ///   </code>
    ///   <code lang="C++" description="Create an angle of 105°30'21.4">
    /// Latitude MyLatitude = new Latitude(105, 30, 21.4);
    ///   </code>
    ///   </example>
    /// <remarks><para>Latitudes measure a distance North or South away from the equator. Latitudes
    /// can range from -90° (at the South pole) to 90° (the North pole), with 0°
    /// representing the equator. Latitudes are commonly paired with Longitudes to mark a
    /// specific location on Earth's surface.</para>
    ///   <para>Latitudes are expressed in either of two major formats. The first format uses
    /// only positive numbers and the letter "N" or "S" to indicate the hemisphere (i.e.
    /// "45°N" or "60°S"). The second format allows negative numbers an omits the single
    /// character (i.e. 45 or -60).</para>
    ///   <para>Instances of this class are guaranteed to be thread-safe because the class is
    /// immutable (its properties can only be changed via constructors).</para></remarks>
    [TypeConverter("DotSpatial.Positioning.Design.LatitudeConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=1.0.0.0, PublicKeyToken=b4b0b185210c9dae")]
#endif
    public struct Latitude : IFormattable, IComparable<Latitude>, IEquatable<Latitude>, ICloneable<Latitude>, IXmlSerializable
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
        /// Represents a latitude of 0°.
        /// </summary>
        public static readonly Latitude Equator = new Latitude(0.0);
        /// <summary>
        /// Represents a latitude of 0°.
        /// </summary>
        public static readonly Latitude Empty = new Latitude(0.0);
        /// <summary>
        /// Represents a latitude of 23.5°S.
        /// </summary>
        public static readonly Latitude TropicOfCapricorn = new Latitude(-23.5);
        /// <summary>
        /// Represents a latitude of 23.5°N.
        /// </summary>
        public static readonly Latitude TropicOfCancer = new Latitude(23.5);
        /// <summary>
        /// Represents a latitude of 90°N.
        /// </summary>
        public static readonly Latitude NorthPole = new Latitude(90.0);
        /// <summary>
        /// Represents a latitude of 90°S.
        /// </summary>
        public static readonly Latitude SouthPole = new Latitude(-90.0);
        /// <summary>
        /// Represents the minimum possible latitude -90°.
        /// </summary>
        public static readonly Latitude Minimum = new Latitude(-90.0);
        /// <summary>
        /// Represents the maximum possible latitude of 90°.
        /// </summary>
        public static readonly Latitude Maximum = new Latitude(90.0);
        /// <summary>
        /// Represents an invalid or unspecified value.
        /// </summary>
        public static readonly Latitude Invalid = new Latitude(double.NaN);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance with the specified decimal degrees.
        /// </summary>
        /// <param name="decimalDegrees">The decimal degrees.</param>
        /// <example>
        /// This example demonstrates how to create an angle with a measurement of 90°.
        ///   <code lang="VB">
        /// Dim MyLatitude As New Latitude(90)
        ///   </code>
        ///   <code lang="CS">
        /// Latitude MyLatitude = new Latitude(90);
        ///   </code>
        ///   </example>
        ///
        /// <returns>An <strong>Latitude</strong> containing the specified value.</returns>
        public Latitude(double decimalDegrees)
        {
            _decimalDegrees = decimalDegrees;
        }

        /// <summary>
        /// Creates a new instance with the specified decimal degrees and hemisphere.
        /// </summary>
        /// <param name="decimalDegrees">The decimal degrees.</param>
        /// <param name="hemisphere">The hemisphere.</param>
        /// <example>
        /// This example creates a new latitude of 39°30' north.
        ///   <code lang="VB">
        /// Dim MyLatitude As New Latitude(39.5, LatitudeHemisphere.North)
        ///   </code>
        ///   <code lang="C#">
        /// Latitude MyLatitude = new Latitude(39.5, LatitudeHemisphere.North);
        ///   </code>
        /// This example creates a new latitude of 39°30 south.
        ///   <code lang="VB">
        /// Dim MyLatitude As New Latitude(39.5, LatitudeHemisphere.South)
        ///   </code>
        ///   <code lang="C#">
        /// Latitude MyLatitude = new Latitude(39.5, LatitudeHemisphere.South);
        ///   </code>
        ///   </example>
        public Latitude(double decimalDegrees, LatitudeHemisphere hemisphere)
        {
            _decimalDegrees = ToDecimalDegrees(decimalDegrees, hemisphere);
        }

        /// <summary>
        /// Creates a new instance with the specified degrees.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <returns>An <strong>Latitude</strong> containing the specified value.</returns>
        public Latitude(int hours)
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
        /// Dim MyLatitude As New Latitude(34, 12, 29.2)
        ///   </code>
        ///   <code lang="CS">
        /// Latitude MyLatitude = new Latitude(34, 12, 29.2);
        ///   </code>
        ///   </example>
        ///
        /// <returns>An <strong>Latitude</strong> containing the specified value.</returns>
        public Latitude(int hours, int minutes, double seconds)
        {
            _decimalDegrees = ToDecimalDegrees(hours, minutes, seconds);
        }

        /// <summary>
        /// Creates a new longitude with the specified hours, minutes, seconds, and hemisphere.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="hemisphere">The hemisphere.</param>
        /// <example>
        /// This example creates a new latitude of 39°12'10" north.
        ///   <code lang="VB">
        /// Dim MyLatitude As New Latitude(39, 12, 10, LatitudeHemisphere.North)
        ///   </code>
        ///   <code lang="C#">
        /// Latitude MyLatitude = new Latitude(39, 12, 10, LatitudeHemisphere.North);
        ///   </code>
        /// This example creates a new latitude of 39°12'10" south.
        ///   <code lang="VB">
        /// Dim MyLatitude As New Latitude(39, 12, 10, LatitudeHemisphere.South)
        ///   </code>
        ///   <code lang="C#">
        /// Latitude MyLatitude = new Latitude(39, 12, 10, LatitudeHemisphere.South);
        ///   </code>
        ///   </example>
        public Latitude(int hours, int minutes, double seconds, LatitudeHemisphere hemisphere)
        {
            _decimalDegrees = ToDecimalDegrees(hours, minutes, seconds, hemisphere);
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
        /// Dim MyLatitude As New Latitude(12, 42.345)
        ///   </code>
        ///   <code lang="VB">
        /// Latitude MyLatitude = new Latitude(12, 42.345);
        ///   </code>
        ///   </example>
        /// <remarks>An <strong>Latitude</strong> containing the specified value.</remarks>
        public Latitude(int hours, double decimalMinutes)
        {
            _decimalDegrees = ToDecimalDegrees(hours, decimalMinutes);
        }

        /// <summary>
        /// Creates a new instance with the specified hours, decimal minutes, and hemisphere.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <param name="decimalMinutes">The decimal minutes.</param>
        /// <param name="hemisphere">The hemisphere.</param>
        /// <example>
        /// This example creates a new latitude of 39°12.34' north.
        ///   <code lang="VB">
        /// Dim MyLatitude As New Latitude(39, 12.34, LatitudeHemisphere.North)
        ///   </code>
        ///   <code lang="C#">
        /// Latitude MyLatitude = new Latitude(39, 12.34, LatitudeHemisphere.North);
        ///   </code>
        /// This example creates a new latitude of 39°12.34 south.
        ///   <code lang="VB">
        /// Dim MyLatitude As New Latitude(39, 12.34, LatitudeHemisphere.South)
        ///   </code>
        ///   <code lang="C#">
        /// Latitude MyLatitude = new Latitude(39, 12.34, LatitudeHemisphere.South);
        ///   </code>
        ///   </example>
        public Latitude(int hours, double decimalMinutes, LatitudeHemisphere hemisphere)
        {
            _decimalDegrees = ToDecimalDegrees(hours, decimalMinutes, hemisphere);
        }

        /// <summary>
        /// Creates a new instance by parsing the specified string value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <example>
        /// This example creates a new instance by parsing a string. (notice The double-quote is
        /// doubled up to represent a single double-quote in the string.)
        ///   <code lang="VB">
        /// Dim MyLatitude As New Latitude("23°45'67.8""N")
        ///   </code>
        ///   <code lang="CS">
        /// Latitude MyLatitude = new Latitude("23°45'67.8\"N");
        ///   </code>
        ///   </example>
        ///
        /// <returns>An <strong>Latitude</strong> containing the specified value.</returns>
        ///
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">The Parse method requires a decimal or sexagesimal measurement.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">Only the right-most portion of a sexagesimal measurement can be a fractional value.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">Extra characters were encountered while parsing an angular measurement.  Only hours, minutes, and seconds are allowed.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">The specified text was not fully understood as an angular measurement.</exception>
        public Latitude(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <summary>
        /// Creates a new instance by parsing the specified string value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        /// <para>A <strong>String</strong> in any of the following formats (or variation
        /// depending on the local culture):</para>
        ///
        /// <seealso cref="Parse(string)">Parse</seealso>
        ///
        /// <example>
        /// This example creates a new instance by parsing a string. (notice: The double-quote is
        /// doubled up to represent a single double-quote in the string.)
        ///   <code lang="VB">
        /// Dim MyLatitude As New Latitude("23°45'67.8""N")
        ///   </code>
        ///   <code lang="CS">
        /// Latitude MyLatitude = new Latitude("23°45'67.8\"N");
        ///   </code>
        ///   </example>
        ///
        /// <returns>An <strong>Latitude</strong> containing the specified value.</returns>
        ///
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">The Parse method requires a decimal or sexagesimal measurement.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">Only the right-most portion of a sexagesimal measurement can be a fractional value.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">Extra characters were encountered while parsing an angular measurement.  Only hours, minutes, and seconds are allowed.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">The specified text was not fully understood as an angular measurement.</exception>
        public Latitude(string value, CultureInfo culture)
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

            // Try to extract the hemisphere
            LatitudeHemisphere hemisphere = LatitudeHemisphere.None;
            if (value.IndexOf("N") != -1)
            {
                hemisphere = LatitudeHemisphere.North;
                value = value.Replace("N", string.Empty);
            }
            else if (value.IndexOf("S") != -1)
            {
                hemisphere = LatitudeHemisphere.South;
                value = value.Replace("S", string.Empty);
            }
            else if (value.IndexOf("n") != -1)
            {
                hemisphere = LatitudeHemisphere.North;
                value = value.Replace("n", string.Empty);
            }
            else if (value.IndexOf("s") != -1)
            {
                hemisphere = LatitudeHemisphere.South;
                value = value.Replace("s", string.Empty);
            }

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
                        // Return a blank Latitude
                        _decimalDegrees = 0.0;
                        return;
                    case 1: // Decimal degrees
                        // Is it nothing?
                        if (values[0].Length == 0)
                        {
                            _decimalDegrees = 0.0;
                            return;
                        }
                        // Is it infinity?
                        if (String.Compare(values[0], Properties.Resources.Common_Infinity, true, culture) == 0)
                        {
                            _decimalDegrees = double.PositiveInfinity;
                            return;
                        }
                        // Is it empty?
                        if (String.Compare(values[0], Properties.Resources.Common_Empty, true, culture) == 0)
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
                                double.Parse(values[0].Substring(5, 2), culture),
                                hemisphere);
                            break;
                        }
                        if (values[0].Length == 8 && values[0][0] == '-' && values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) == -1)
                        {
                            _decimalDegrees = ToDecimalDegrees(
                                int.Parse(values[0].Substring(0, 4), culture),
                                int.Parse(values[0].Substring(4, 2), culture),
                                double.Parse(values[0].Substring(6, 2), culture),
                                hemisphere);
                            break;
                        }
                        _decimalDegrees = ToDecimalDegrees(
                            double.Parse(values[0], culture),
                            hemisphere);
                        break;
                    case 2: // Hours and decimal minutes
                        // If this is a fractional value, remember that it is
                        if (values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) != -1)
                        {
                            throw new ArgumentException(Properties.Resources.Latitude_OnlyRightmostIsDecimal, "value");
                        }

                        // Set decimal degrees
                        _decimalDegrees = ToDecimalDegrees(
                                                int.Parse(values[0], culture),
                                                float.Parse(values[1], culture),
                                                hemisphere);
                        break;
                    default: // Hours, minutes and seconds  (most likely)
                        // If this is a fractional value, remember that it is
                        if (values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) != -1 || values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) != -1)
                        {
                            throw new ArgumentException(Properties.Resources.Latitude_OnlyRightmostIsDecimal, "value");
                        }

                        // Set decimal degrees
                        _decimalDegrees = ToDecimalDegrees(
                                                int.Parse(values[0], culture),
                                                int.Parse(values[1], culture),
                                                double.Parse(values[2], culture),
                                                hemisphere);
                        break;
                }
            }
            catch (Exception ex)
            {
#if PocketPC
                    throw new ArgumentException(Properties.Resources.Latitude_InvalidFormat, ex);
#else
                throw new ArgumentException(Properties.Resources.Latitude_InvalidFormat, "value", ex);
#endif
            }
        }

        /// <summary>
        /// Creates a new instance by deserializing the specified XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public Latitude(XmlReader reader)
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
        /// Dim MyLatitude As New Latitude(20, 30)
        /// ' Setting the DecimalMinutes recalculated other properties
        /// Debug.WriteLine(MyLatitude.DecimalDegrees)
        /// ' Output: "20.5"  the same as 20°30'
        ///   </code>
        ///   <code lang="CS">
        /// // Create an angle of 20°30'
        /// Latitude MyLatitude = New Latitude(20, 30);
        /// // Setting the DecimalMinutes recalculated other properties
        /// Console.WriteLine(MyLatitude.DecimalDegrees)
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
        /// Dim MyLatitude As New Latitude(20, 10, 30)
        /// ' The DecimalMinutes property is automatically calculated
        /// Debug.WriteLine(MyLatitude.DecimalMinutes)
        /// ' Output: "10.5"
        ///   </code>
        ///   <code lang="CS">
        /// // Create an angle of 20°10'30"
        /// Latitude MyLatitude = new Latitude(20, 10, 30);
        /// // The DecimalMinutes property is automatically calculated
        /// Console.WriteLine(MyLatitude.DecimalMinutes)
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
        /// Dim MyLatitude As New Latitude(60.5)
        /// Debug.WriteLine(MyLatitude.Hours)
        /// ' Output: 60
        ///   </code>
        ///   <code lang="CS">
        /// Latitude MyLatitude = new Latitude(60.5);
        /// Console.WriteLine(MyLatitude.Hours);
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
        /// Dim MyLatitude As New Latitude(45.5)
        /// Debug.WriteLine(MyLatitude.Minutes)
        /// ' Output: 30
        ///   </code>
        ///   <code lang="CS">
        /// Latitude MyLatitude = new Latitude(45.5);
        /// Console.WriteLine(MyLatitude.Minutes);
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
                return (int)(
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
        /// Dim MyLatitude As New Latitude(45, 10.5)
        /// Debug.WriteLine(MyLatitude.Seconds)
        /// ' Output: 30
        ///   </code>
        ///   <code lang="CS">
        /// Dim MyLatitude As New Latitude(45, 10.5);
        /// Console.WriteLine(MyLatitude.Seconds);
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
        /// Indicates if the latitude is north or south of the equator.
        /// </summary>
        public LatitudeHemisphere Hemisphere
        {
            get
            {
                // And set the hemisphere
                return DecimalDegrees < 0 ? LatitudeHemisphere.South : LatitudeHemisphere.North;
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
                return _decimalDegrees == 0;
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
        /// allowed bounds of -90° and 90°.
        /// </summary>
        public bool IsNormalized
        {
            get { return _decimalDegrees >= -90 && _decimalDegrees <= 90; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns the object with the smallest value.
        /// </summary>
        /// <param name="value">A <strong>Latitude</strong> object to compare to the current instance.</param>
        /// <returns>The <strong>Latitude</strong> containing the smallest value.</returns>
        public Latitude LesserOf(Latitude value)
        {
            if (_decimalDegrees < value.DecimalDegrees)
                return this;
            return value;
        }

        /// <summary>
        /// Returns the object with the largest value.
        /// </summary>
        /// <param name="value">A <strong>Latitude</strong> object to compare to the current instance.</param>
        /// <returns>A <strong>Latitude</strong> containing the largest value.</returns>
        public Latitude GreaterOf(Latitude value)
        {
            if (_decimalDegrees > value.DecimalDegrees)
                return this;
            return value;
        }

        /// <summary>
        /// Returns a new instance whose value is rounded the specified number of decimals.
        /// </summary>
        /// <param name="decimals">An <strong>Integer</strong> specifying the number of decimals to round off to.</param>
        /// <returns></returns>
        public Latitude Round(int decimals)
        {
            return new Latitude(Math.Round(_decimalDegrees, decimals));
        }

        /// <summary>
        /// Returns an angle opposite of the current instance.
        /// </summary>
        /// <returns>An <strong>Latitude</strong> representing the mirrored value.</returns>
        /// <example>
        /// This example creates a new <strong>Latitude</strong> of 45° then calculates its mirror
        /// of 225°. (45 + 180)
        ///   <code lang="VB" title="[New Example]">
        /// Dim Latitude1 As New Latitude(45)
        /// Dim Latitude2 As Latitude = Latitude1.Mirror()
        /// Debug.WriteLine(Latitude2.ToString())
        /// ' Output: 225
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Latitude Latitude1 = new Latitude(45);
        /// Latitude Latitude2 = Latitude1.Mirror();
        /// Console.WriteLine(Latitude2.ToString());
        /// // Output: 225
        ///   </code>
        ///   </example>
        /// <remarks>This method returns the "opposite" of the current instance. The opposite is
        /// defined as the point on the other side of an imaginary circle. For example, if an angle
        /// is 0°, at the top of a circle, this method returns 180°, at the bottom of the
        /// circle.</remarks>
        public Latitude Mirror()
        {
            return Normalize().Multiply(-1.0);
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
        /// Dim MyLatitude As New Latitude(90)
        /// Dim MyRadians As Radian = MyLatitude.ToRadians()
        ///   </code>
        ///   <code lang="CS">
        /// Latitude MyLatitude = new Latitude(90);
        /// Radian MyRadians = MyLatitude.ToRadians();
        ///   </code>
        ///   </example>
        /// <remarks>This function is typically used to convert an angular measurement into
        /// radians before performing a trigonometric function.</remarks>
        public Radian ToRadians()
        {
            return new Radian(_decimalDegrees * Radian.RADIANS_PER_DEGREE);
        }

        /// <summary>
        /// Causes the value to be adjusted to between -90 and +90.
        /// </summary>
        /// <returns></returns>
        public Latitude Normalize()
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
                return new Latitude(-newValue);
            return new Latitude(newValue);
        }

        /// <summary>
        /// Indicates if the current instance is North of the specified latitude.
        /// </summary>
        /// <param name="latitude">A <strong>Latitude</strong> object to examine.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is more North than the specified instance.</returns>
        public bool IsNorthOf(Latitude latitude)
        {
            return IsGreaterThan(latitude);
        }

        /// <summary>
        /// Indicates if the current instance is South of the specified latitude.
        /// </summary>
        /// <param name="latitude">A <strong>Latitude</strong> object to examine.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is more South than the specified instance.</returns>
        public bool IsSouthOf(Latitude latitude)
        {
            return IsLessThan(latitude);
        }

        /// <summary>
        /// Converts the current instance to the northern or southern hemisphere.
        /// </summary>
        /// <param name="hemisphere">The hemisphere.</param>
        /// <returns></returns>
        public Latitude ToHemisphere(LatitudeHemisphere hemisphere)
        {
            if (hemisphere == LatitudeHemisphere.None)
                throw new ArgumentException(Properties.Resources.Latitude_InvalidHemisphere);

            // IF the degrees is already in the right hemishpere, do nothing
            if ((hemisphere == LatitudeHemisphere.North && _decimalDegrees >= 0)
                || (hemisphere == LatitudeHemisphere.South && _decimalDegrees < 0))
                return this;

            // Switch the hemisphere
            return Mirror();
        }

        /// <summary>
        /// Outputs the angle as a string using the specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>A <strong>String</strong> in the specified format.</returns>
        /// <example>
        /// This example uses the <strong>ToString</strong> method to output an angle in a
        /// custom format. The " <strong>h°</strong> " code represents hours along with a
        /// degree symbol (Alt+0176 on the keypad), and " <strong>m.mm</strong> " represents
        /// the minutes out to two decimals. Mmm.
        ///   <code lang="VB">
        /// Dim MyLatitude As New Latitude(45, 16.772)
        /// Debug.WriteLine(MyLatitude.ToString("h°m.mm"))
        /// ' Output: 45°16.78
        ///   </code>
        ///   <code lang="CS">
        /// Dim MyLatitude As New Latitude(45, 16.772);
        /// Debug.WriteLine(MyLatitude.ToString("h°m.mm"));
        /// // Output: 45°16.78
        ///   </code>
        ///   </example>
        /// <remarks>This method returns the current instance output in a specific format. If no
        /// value for the format is specified, a default format of "d.dddd°" is used. Any
        /// string output by this method can be converted back into an Latitude object using the
        /// <strong>Parse</strong> method or <strong>Latitude(string)</strong> constructor.</remarks>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns the smallest integer greater than the specified value.
        /// </summary>
        /// <returns></returns>
        public Latitude Ceiling()
        {
            return new Latitude(Math.Ceiling(_decimalDegrees));
        }

        /// <summary>
        /// Returns the largest integer which is smaller than the specified value.
        /// </summary>
        /// <returns></returns>
        public Latitude Floor()
        {
            return new Latitude(Math.Floor(_decimalDegrees));
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
        /// <returns>An <strong>Latitude</strong> containing the rounded value.</returns>
        /// <remarks>This method is used to align or "snap" an angle to a regular interval. For
        /// example, a grid might be easier to read if it were drawn at 30-second intervals instead
        /// of 24.198-second intervals.</remarks>
        public Latitude RoundSeconds()
        {
            return RoundSeconds(15.0);
        }

        /// <summary>
        /// Returns a new angle whose Seconds property is evenly divisible by the specified amount.
        /// </summary>
        /// <param name="interval">A <strong>Double</strong> between 0 and 60 indicating the interval to round
        /// to.</param>
        /// <returns>An <strong>Latitude</strong> containing the rounded value.</returns>
        /// <remarks>This method is used to align or "snap" an angle to a regular interval. For
        /// example, a grid might be easier to read if it were drawn at 30-second intervals instead
        /// of 24.198-second intervals.</remarks>
        public Latitude RoundSeconds(double interval)
        {
            // Interval must be > 0
            if (interval == 0)
#if PocketPC
                throw new ArgumentOutOfRangeException(Properties.Resources.Angle_InvalidInterval);
#else
                throw new ArgumentOutOfRangeException("interval", interval, Properties.Resources.Angle_InvalidInterval);
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
                return new Latitude(Hours, Minutes, newSeconds);
            }
            // return the new value
            return new Latitude(Hours, Minutes, newSeconds);
        }

        #endregion Public Methods

        #region Overrides

        /// <summary>
        /// Compares the current value to another Latitude object's value.
        /// </summary>
        /// <param name="obj">An <strong>Latitude</strong>, <strong>Double</strong>, or <strong>Integer</strong>
        /// to compare with.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the object's DecimalDegrees
        /// properties match.</returns>
        /// <remarks>This</remarks>
        public override bool Equals(object obj)
        {
            // Only other Latitude objects are allowed
            if (obj is Latitude)
                return Equals((Latitude)obj);
            return false;
        }

        /// <summary>
        /// Returns a unique code for this instance.
        /// </summary>
        /// <returns>An <strong>Integer</strong> representing a unique code for the current
        /// instance.</returns>
        /// <remarks>Since the <strong>Latitude</strong> class is immutable, this property may be used
        /// safely with hash tables.</remarks>
        public override int GetHashCode()
        {
            return _decimalDegrees.GetHashCode();
        }

        /// <summary>
        /// Outputs the angle as a string using the default format.
        /// </summary>
        /// <returns>A <strong>String</strong> created using the default format.</returns>
        /// <example>
        /// This example outputs a value of 90 degrees in the default format of ###.#°.
        ///   <code lang="VB">
        /// Dim MyLatitude As New Latitude(90)
        /// Debug.WriteLine(MyLatitude.ToString)
        /// ' Output: "90°"
        ///   </code>
        ///   <code lang="CS">
        /// Latitude MyLatitude = new Latitude(90);
        /// Debug.WriteLine(MyLatitude.ToString());
        /// // Output: "90°"
        ///   </code>
        ///   </example>
        /// <remarks>This method formats the current instance using the default format of
        /// "d.dddd°." Any string output by this method can be converted back into an Latitude
        /// object using the <strong>Parse</strong> method or <strong>Latitude(string)</strong>
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
        public static Latitude Normalize(double decimalDegrees)
        {
            return new Latitude(decimalDegrees).Normalize();
        }

        /// <summary>
        /// Returns a random latitude.
        /// </summary>
        /// <returns></returns>
        public static Latitude Random()
        {
            return Random(new Random());
        }

        /// <summary>
        /// Returns a random latitude based on the specified seed.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <returns></returns>
        public static Latitude Random(Random generator)
        {
            return new Latitude((generator.NextDouble() * 180.0) - 90.0);
        }

        /// <summary>
        /// Returns a random latitude using the specified northern and southern boundaries.
        /// </summary>
        /// <param name="northernmost">The northernmost.</param>
        /// <param name="southernmost">The southernmost.</param>
        /// <returns></returns>
        public static Latitude Random(Latitude northernmost, Latitude southernmost)
        {
            return Random(new Random(), northernmost, southernmost);
        }

        /// <summary>
        /// Returns a random latitude between the specified minimum and maximum.
        /// </summary>
        /// <param name="generator">a <strong>Random</strong> object used to generate random values.</param>
        /// <param name="northernmost">A <strong>Latitude</strong> specifying the northern-most allowed latitude.</param>
        /// <param name="southernmost">A <strong>Latitude</strong> specifying the southern-most allowed latitude.</param>
        /// <returns></returns>
        public static Latitude Random(Random generator, Latitude northernmost, Latitude southernmost)
        {
            return new Latitude(((northernmost.DecimalDegrees - southernmost.DecimalDegrees) * generator.NextDouble())
                                + southernmost.DecimalDegrees);
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
        /// Converts arbitrary hour and decimal minutes into decimal degrees.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <param name="decimalMinutes">The decimal minutes.</param>
        /// <param name="hemisphere">The hemisphere.</param>
        /// <returns>A <strong>Double</strong> containing the decimal degree version of the specified
        /// values.</returns>
        /// <remarks>The specified value will be converted to decimal degrees, then rounded to thirteen digits, the maximum precision allowed by this type.</remarks>
        public static double ToDecimalDegrees(int hours, double decimalMinutes, LatitudeHemisphere hemisphere)
        {
            //switch (hemisphere)
            //{
            //    case LatitudeHemisphere.South:
            //        return -Math.Abs(hours) - Math.Round(decimalMinutes / 60.0, MaximumPrecisionDigits);
            //    case LatitudeHemisphere.North:
            //        return Math.Abs(hours) + Math.Round(decimalMinutes / 60.0, MaximumPrecisionDigits);
            //    default:
            //        return ToDecimalDegrees(hours, decimalMinutes);
            //}

            switch (hemisphere)
            {
                case LatitudeHemisphere.South:
                    return -Math.Abs(hours) - decimalMinutes / 60.0;
                case LatitudeHemisphere.North:
                    return Math.Abs(hours) + decimalMinutes / 60.0;
                default:
                    return ToDecimalDegrees(hours, decimalMinutes);
            }
        }

        /// <summary>
        /// Converts arbitrary decrees into well-formed decimal degrees.
        /// </summary>
        /// <param name="decimalDegrees">The decimal degrees.</param>
        /// <param name="hemisphere">The hemisphere.</param>
        /// <returns>A <strong>Double</strong> containing the decimal degree version of the specified
        /// values.</returns>
        /// <remarks>The specified value will be rounded to thirteen digits, the maximum precision allowed by this type.</remarks>
        public static double ToDecimalDegrees(double decimalDegrees, LatitudeHemisphere hemisphere)
        {
            //switch (hemisphere)
            //{
            //    case LatitudeHemisphere.South:
            //        return -Math.Abs(Math.Round(decimalDegrees, MaximumPrecisionDigits));
            //    case LatitudeHemisphere.North:
            //        return Math.Abs(Math.Round(decimalDegrees, MaximumPrecisionDigits));
            //    default:
            //        return ToDecimalDegrees(decimalDegrees);
            //}
            switch (hemisphere)
            {
                case LatitudeHemisphere.South:
                    return -Math.Abs(decimalDegrees);
                case LatitudeHemisphere.North:
                    return Math.Abs(decimalDegrees);
                default:
                    return decimalDegrees;
            }
        }

        /// <summary>
        /// Converts arbitrary hour, minute and seconds into decimal degrees.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="hemisphere">The hemisphere.</param>
        /// <returns>A <strong>Double</strong> containing the decimal degree version of the specified
        /// values.</returns>
        /// <remarks>This function is used to convert three-part measurements into a single value. The
        /// result of this method is typically assigned to the
        /// <see cref="Latitude.DecimalDegrees">
        /// DecimalDegrees</see> property. Values are rounded to thirteen decimal
        /// places, the maximum precision allowed by this type.</remarks>
        public static double ToDecimalDegrees(int hours, int minutes, double seconds, LatitudeHemisphere hemisphere)
        {
            //switch (hemisphere)
            //{
            //    case LatitudeHemisphere.South:
            //        return -Math.Abs(hours) - Math.Round(minutes / 60.0, MaximumPrecisionDigits) - Math.Round(seconds / 3600.0, MaximumPrecisionDigits);
            //    case LatitudeHemisphere.North:
            //        return Math.Abs(hours) + Math.Round(minutes / 60.0, MaximumPrecisionDigits) + Math.Round(seconds / 3600.0, MaximumPrecisionDigits);
            //    default:
            //        return ToDecimalDegrees(hours, minutes, seconds);
            //}

            switch (hemisphere)
            {
                case LatitudeHemisphere.South:
                    return -Math.Abs(hours) - minutes / 60.0 - seconds / 3600.0;
                case LatitudeHemisphere.North:
                    return Math.Abs(hours) + minutes / 60.0 + seconds / 3600.0;
                default:
                    return ToDecimalDegrees(hours, minutes, seconds);
            }
        }

        /// <summary>
        /// Returns the object with the smallest value.
        /// </summary>
        /// <param name="value1">The first <strong>Latitude</strong> object.</param>
        /// <param name="value2">The second <strong>Latitude</strong> object.</param>
        /// <returns>The <strong>Latitude</strong> containing the smallest value.</returns>
        public static Latitude LesserOf(Latitude value1, Latitude value2)
        {
            return value1.LesserOf(value2);
        }

        /// <summary>
        /// Returns the object with the largest value.
        /// </summary>
        /// <param name="value1">The first <strong>Latitude</strong> object.</param>
        /// <param name="value2">The second <strong>Latitude</strong> object.</param>
        /// <returns>A <strong>Latitude</strong> containing the largest value.</returns>
        public static Latitude GreaterOf(Latitude value1, Latitude value2)
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
        /// Dim MyRadian As Radian = Latitude.ToRadians(90)
        ///   </code>
        ///   <code lang="CS">
        /// Radian MyRadian = Latitude.ToRadians(90);
        ///   </code>
        ///   </example>
        /// <remarks>This function is typically used to convert an angular measurement into
        /// radians before performing a trigonometric function.</remarks>
        public static Radian ToRadians(Latitude value)
        {
            return Radian.FromDegrees(value.DecimalDegrees);
        }

        /// <summary>
        /// Converts a value in radians into an angular measurement.
        /// </summary>
        /// <param name="radians">The radians.</param>
        /// <returns></returns>
        /// <seealso cref="Latitude.ToRadians()">ToRadians</seealso>
        ///
        /// <seealso cref="Radian">Radian Class</seealso>
        ///
        /// <example>
        /// This example uses the <strong>FromRadians</strong> method to convert a value of one
        /// radian into an <strong>Latitude</strong> of 57°.
        ///   <code lang="VB">
        /// ' Create a new angle equal to one radian
        /// Dim MyRadians As New Radian(1)
        /// Dim MyLatitude As Latitude = Latitude.FromRadians(MyRadians)
        /// Debug.WriteLine(MyLatitude.ToString())
        /// ' Output: 57°
        ///   </code>
        ///   <code lang="CS">
        /// // Create a new angle equal to one radian
        /// Radian MyRadians = new Radian(1);
        /// Latitude MyLatitude = Latitude.FromRadians(MyRadians);
        /// Console.WriteLine(MyLatitude.ToString());
        /// // Output: 57°
        ///   </code>
        ///   </example>
        /// <remarks>This function is typically used in conjunction with the
        /// <see cref="Latitude.ToRadians()">ToRadians</see>
        /// method after a trigonometric function has completed. The converted value is stored in
        /// the <see cref="DecimalDegrees">DecimalDegrees</see> property.</remarks>
        public static Latitude FromRadians(Radian radians)
        {
            return new Latitude(radians.ToDegrees());
        }

        /// <summary>
        /// Froms the radians.
        /// </summary>
        /// <param name="radians">The radians.</param>
        /// <returns></returns>
        public static Latitude FromRadians(double radians)
        {
            return new Latitude(Radian.ToDegrees(radians));
        }

        /// <summary>
        /// Converts the specified string into an Latitude object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A new <strong>Latitude</strong> object populated with the specified
        /// values.</returns>
        /// <example>
        /// This example creates a new angular measurement using the <strong>Parse</strong>
        /// method.
        ///   <code lang="VB">
        /// Dim NewLatitude As Latitude = Latitude.Parse("123.45°")
        ///   </code>
        ///   <code lang="CS">
        /// Latitude NewLatitude = Latitude.Parse("123.45°");
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
        /// <remarks>This method parses the specified string into an <strong>Latitude</strong> object
        /// using the current culture. This constructor can parse any strings created via the
        /// <strong>ToString</strong> method.</remarks>
        public static Latitude Parse(string value)
        {
            return new Latitude(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts the specified string into an <strong>Latitude</strong> object using the
        /// specified culture.
        /// </summary>
        /// <param name="value">A <strong>String</strong> describing an angle in the form of decimal degrees or a
        /// sexagesimal.</param>
        /// <param name="culture">A <strong>CultureInfo</strong> object describing the numeric format to use during
        /// conversion.</param>
        /// <returns>A new <strong>Latitude</strong> object equivalent to the specified string.</returns>
        /// <remarks>This powerful method is typically used to process data from a data store or a
        /// value input by the user in any culture. This function can accept any format which
        /// can be output by the ToString method.</remarks>
        public static Latitude Parse(string value, CultureInfo culture)
        {
            return new Latitude(value, culture);
        }

        #endregion Static Methods

        #region Operators

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Latitude operator +(Latitude left, Latitude right)
        {
            return new Latitude(left.DecimalDegrees + right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Latitude operator +(Latitude left, double right)
        {
            return new Latitude(left.DecimalDegrees + right);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Latitude operator -(Latitude left, Latitude right)
        {
            return new Latitude(left.DecimalDegrees - right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Latitude operator -(Latitude left, double right)
        {
            return new Latitude(left.DecimalDegrees - right);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Latitude operator *(Latitude left, Latitude right)
        {
            return new Latitude(left.DecimalDegrees * right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Latitude operator *(Latitude left, double right)
        {
            return new Latitude(left.DecimalDegrees * right);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Latitude operator /(Latitude left, Latitude right)
        {
            return new Latitude(left.DecimalDegrees / right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Latitude operator /(Latitude left, double right)
        {
            return new Latitude(left.DecimalDegrees / right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Latitude left, Latitude right)
        {
            return left.DecimalDegrees.Equals(right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Latitude left, double right)
        {
            return left.DecimalDegrees.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Latitude left, Latitude right)
        {
            return !left.DecimalDegrees.Equals(right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Latitude left, double right)
        {
            return !left.DecimalDegrees.Equals(right);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Latitude left, Latitude right)
        {
            return left.DecimalDegrees > right.DecimalDegrees;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Latitude left, double right)
        {
            return left.DecimalDegrees > right;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(Latitude left, Latitude right)
        {
            return left.DecimalDegrees >= right.DecimalDegrees;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(Latitude left, double right)
        {
            return left.DecimalDegrees >= right;
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Latitude left, Latitude right)
        {
            return left.DecimalDegrees < right.DecimalDegrees;
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Latitude left, double right)
        {
            return left.DecimalDegrees < right;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(Latitude left, Latitude right)
        {
            return left.DecimalDegrees <= right.DecimalDegrees;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(Latitude left, double right)
        {
            return left.DecimalDegrees <= right;
        }

        /// <summary>
        /// Returns the current instance increased by one.
        /// </summary>
        /// <returns>An <strong>Latitude</strong> object.</returns>
        /// <example>
        /// This example uses the <strong>Increment</strong> method to increase an Latitude's
        /// value. It also demonstrates the subtle error which can be caused if
        ///   <strong>Increment</strong> is called while ignoring the return value.
        ///   <code lang="VB" title="[New Example]">
        /// ' Correct use of Increment
        /// Dim Latitude1 As New Latitude(89)
        /// Latitude1 = Latitude1.Increment()
        /// ' Incorrect use of Increment
        /// Dim Latitude1 = New Latitude(89)
        /// Latitude1.Increment()
        /// ' notice: Latitude1 will still be 89°!
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// // Correct use of Increment
        /// Latitude Latitude1 = new Latitude(89);
        /// Latitude1 = Latitude1.Increment();
        /// // Incorrect use of Increment
        /// Latitude Latitude1 = new Latitude(89);
        /// Latitude1.Increment();
        /// // notice: Latitude1 will still be 89°!
        ///   </code>
        ///   </example>
        /// <remarks><para>This method increases the <strong>DecimalDegrees</strong> property by 1.0,
        /// returned as a new instance.</para>
        ///   <para><font color="red">Since the <strong>Latitude</strong> class is immutable, this
        /// method cannot be used to modify an existing instance.</font></para></remarks>
        public Latitude Increment()
        {
            return new Latitude(_decimalDegrees + 1.0);
        }

        /// <summary>
        /// Increases the current instance by the specified value.
        /// </summary>
        /// <param name="value">A <strong>Double</strong> to add to the current instance.</param>
        /// <returns>A new <strong>Latitude</strong> containing the summed values.</returns>
        /// <example>
        /// This example adds 45° to the current instance of 45°, returning 90°.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Latitude1 As New Latitude(45)
        /// Latitude1 = Latitude1.Add(45)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Latitude Latitude1 = new Latitude(45);
        /// Latitude1 = Latitude1.Add(45);
        ///   </code>
        ///   </example>
        public Latitude Add(double value)
        {
            return new Latitude(_decimalDegrees + value);
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Latitude Add(Latitude value)
        {
            return new Latitude(_decimalDegrees + value.DecimalDegrees);
        }

        /// <summary>
        /// Returns the current instance decreased by one.
        /// </summary>
        /// <returns>An <strong>Latitude</strong> object.</returns>
        /// <example>
        /// This example uses the <strong>Decrement</strong> method to decrease an Latitude's
        /// value. It also demonstrates the subtle error which can be caused if
        ///   <strong>Decrement</strong> is called while ignoring the return value.
        ///   <code lang="VB" title="[New Example]">
        /// ' Correct use of Decrement
        /// Dim Latitude1 As New Latitude(91)
        /// Latitude1 = Latitude1.Decrement()
        /// ' Incorrect use of Decrement
        /// Dim Latitude1 = New Latitude(91)
        /// Latitude1.Increment()
        /// ' notice Latitude1 will still be 91°!
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// // Correct use of Decrement
        /// Latitude Latitude1 = new Latitude(91);
        /// Latitude1 = Latitude1.Decrement();
        /// // Incorrect use of Decrement
        /// Latitude Latitude1 = new Latitude(91);
        /// Latitude1.Decrement();
        /// // notice Latitude1 will still be 91°!
        ///   </code>
        ///   </example>
        /// <remarks><para>This method decreases the <strong>DecimalDegrees</strong> property by 1.0,
        /// returned as a new instance.</para>
        ///   <para><font color="red">Since the <strong>Latitude</strong> class is immutable, this
        /// method cannot be used to modify an existing instance.</font></para></remarks>
        public Latitude Decrement()
        {
            return new Latitude(_decimalDegrees - 1.0);
        }

        /// <summary>
        /// Decreases the current instance by the specified value.
        /// </summary>
        /// <param name="value">A <strong>Double</strong> to subtract from the current instance.</param>
        /// <returns>A new <strong>Latitude</strong> containing the new value.</returns>
        /// <example>
        /// This example subtracts 30° from the current instance of 90°, returning 60°.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Latitude1 As New Latitude(90)
        /// Latitude1 = Latitude1.Subtract(30)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Latitude Latitude1 = new Latitude(90);
        /// Latitude1 = Latitude1.Subtract(30);
        ///   </code>
        ///   </example>
        public Latitude Subtract(double value)
        {
            return new Latitude(_decimalDegrees - value);
        }

        /// <summary>
        /// Subtracts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Latitude Subtract(Latitude value)
        {
            return new Latitude(_decimalDegrees - value.DecimalDegrees);
        }

        /// <summary>
        /// Multiplies the current instance by the specified value.
        /// </summary>
        /// <param name="value">A <strong>Double</strong> to multiply with the current instance.</param>
        /// <returns>A new <strong>Latitude</strong> containing the product of the two numbers.</returns>
        /// <example>
        /// This example multiplies 30° with three, returning 90°.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Latitude1 As New Latitude(30)
        /// Latitude1 = Latitude1.Multiply(3)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Latitude Latitude1 = new Latitude(30);
        /// Latitude1 = Latitude1.Multiply(3);
        ///   </code>
        ///   </example>
        public Latitude Multiply(double value)
        {
            return new Latitude(_decimalDegrees * value);
        }

        /// <summary>
        /// Multiplies the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Latitude Multiply(Latitude value)
        {
            return new Latitude(_decimalDegrees * value.DecimalDegrees);
        }

        /// <summary>
        /// Divides the current instance by the specified value.
        /// </summary>
        /// <param name="value">A <strong>Double</strong> representing a denominator to divide by.</param>
        /// <returns>An <strong>Latitude</strong> containing the new value.</returns>
        /// <example>
        /// This example divides 90° by three, returning 30°.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Latitude1 As New Latitude(90)
        /// Latitude1 = Latitude1.Divide(3)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Latitude Latitude1 = new Latitude(90);
        /// Latitude1 = Latitude1.Divide(3);
        ///   </code>
        ///   </example>
        public Latitude Divide(double value)
        {
            return new Latitude(_decimalDegrees / value);
        }

        /// <summary>
        /// Divides the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Latitude Divide(Latitude value)
        {
            return new Latitude(_decimalDegrees / value.DecimalDegrees);
        }

        /// <summary>
        /// Indicates if the current instance is smaller than the specified value.
        /// </summary>
        /// <param name="value">An <strong>Latitude</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// smaller than the specified value.</returns>
        public bool IsLessThan(Latitude value)
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
        /// <param name="value">An <strong>Latitude</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// smaller than or equal to the specified value.</returns>
        /// <remarks>This method compares the <strong>DecimalDegrees</strong> property with the
        /// specified value. This method is the same as the "&lt;=" operator.</remarks>
        public bool IsLessThanOrEqualTo(Latitude value)
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
        /// <param name="value">An <strong>Latitude</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// greater than the specified value.</returns>
        public bool IsGreaterThan(Latitude value)
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
        /// <param name="value">An <strong>Latitude</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// greater than or equal to the specified value.</returns>
        public bool IsGreaterThanOrEqualTo(Latitude value)
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
        /// Converts a measurement in Radians into an Latitude.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Latitude(Radian value)
        {
            return new Latitude(value.ToDegrees());
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Latitude.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Latitude(double value)
        {
            return new Latitude(value);
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Latitude.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Latitude(float value)
        {
            return new Latitude(Convert.ToDouble(value));
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Latitude.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator double(Latitude value)
        {
            return value.DecimalDegrees;
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Latitude.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator float(Latitude value)
        {
            return Convert.ToSingle(value.DecimalDegrees);
        }

        /// <summary>
        /// Converts a measurement in degrees as an Integer into an Latitude.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Latitude(int value)
        {
            return new Latitude(value);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Angle"/> to <see cref="DotSpatial.Positioning.Latitude"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Latitude(Angle value)
        {
            return new Latitude(value.DecimalDegrees);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Azimuth"/> to <see cref="DotSpatial.Positioning.Latitude"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Latitude(Azimuth value)
        {
            return new Latitude(value.DecimalDegrees);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Elevation"/> to <see cref="DotSpatial.Positioning.Latitude"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Latitude(Elevation value)
        {
            return new Latitude(value.DecimalDegrees);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Longitude"/> to <see cref="DotSpatial.Positioning.Latitude"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Latitude(Longitude value)
        {
            return new Latitude(value.DecimalDegrees);
        }

        /// <summary>
        /// Converts a measurement in the form of a formatted String into an Latitude.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Latitude(string value)
        {
            return new Latitude(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts an Latitude into a String.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks>This operator calls the ToString() method using the current culture.</remarks>
        public static explicit operator string(Latitude value)
        {
            return value.ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion Conversions

        #region ICloneable<Latitude> Members

        /// <summary>
        /// Creates a copy of the current instance.
        /// </summary>
        /// <returns>An <strong>Latitude</strong> of the same value as the current instance.</returns>
        public Latitude Clone()
        {
            return new Latitude(_decimalDegrees);
        }

        #endregion ICloneable<Latitude> Members

        #region IEquatable<Latitude> Members

        /// <summary>
        /// Compares the current instance to another instance using the specified
        /// precision.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decimals">The decimals.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the
        /// <strong>DecimalDegrees</strong> property of the current instance matches the
        /// specified instance's <strong>DecimalDegrees</strong> property.</returns>
        /// <example>
        /// These examples compare two fractional values using specific numbers of digits for
        /// comparison.
        ///   <code lang="VB" title="[New Example]">
        /// ' Equals will return False
        /// Dim Latitude1 As New Latitude(90.15);
        /// Dim Latitude2 As New Latitude(90.12);
        /// If Latitude1.Equals(Latitude2, 2) Then
        /// Debug.WriteLine("The values are the same to two digits of precision.");
        /// ' Equals will return True
        /// Dim Latitude1 As New Latitude(90.15);
        /// Dim Latitude2 As New Latitude(90.12);
        /// If Latitude1.Equals(Latitude2, 1) Then
        /// Debug.WriteLine("The values are the same to one digit of precision.");
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// // Equals will return False
        /// Latitude Latitude1 = new Latitude(90.15);
        /// Latitude Latitude2 = new Latitude(90.12);
        /// if (Latitude1.Equals(Latitude2, 2))
        /// Console.WriteLine("The values are the same to two digits of precision.");
        /// // Equals will return True
        /// Latitude Latitude1 = new Latitude(90.15);
        /// Latitude Latitude2 = new Latitude(90.12);
        /// if (Latitude1.Equals(Latitude2, 1))
        /// Console.WriteLine("The values are the same to one digits of precision.");
        ///   </code>
        ///   </example>
        /// <remarks><para>This is typically used in cases where precision is only significant for a few
        /// digits and exact comparison is not necessary.</para>
        ///   <para><em>notice This method compares objects by value, not by
        /// reference.</em></para></remarks>
        public bool Equals(Latitude value, int decimals)
        {
            // Round both values then compare
            return Math.Round(_decimalDegrees, decimals)
                .Equals(Math.Round(value.DecimalDegrees, decimals));
        }

        /// <summary>
        /// Equalses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Equals(Latitude value)
        {
            return _decimalDegrees.Equals(value.DecimalDegrees);
        }

        #endregion IEquatable<Latitude> Members

        #region IComparable<Latitude> Members

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public int CompareTo(Latitude obj)
        {
            return _decimalDegrees.CompareTo(obj.DecimalDegrees);
        }

        #endregion IComparable<Latitude> Members

        #region IFormattable Members

        /// <summary>
        /// Outputs the angle as a string using the specified format.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <strong>String</strong> in the specified format.</returns>
        /// <example>
        /// This example uses the <strong>ToString</strong> method to output an angle in a
        /// custom format. The " <strong>h°</strong> " code represents hours along with a
        /// degree symbol (Alt+0176 on the keypad), and " <strong>m.mm</strong> " represents
        /// the minutes out to two decimals. Mmm.
        ///   <code lang="VB">
        /// Dim MyLatitude As New Latitude(45, 16.772)
        /// Debug.WriteLine(MyLatitude.ToString("h°m.mm", CultureInfo.CurrentCulture))
        /// ' Output: 45°16.78
        ///   </code>
        ///   <code lang="CS">
        /// Dim MyLatitude As New Latitude(45, 16.772);
        /// Debug.WriteLine(MyLatitude.ToString("h°m.mm", CultureInfo.CurrentCulture));
        /// // Output: 45°16.78
        ///   </code>
        ///   </example>
        /// <remarks>This method returns the current instance output in a specific format. If no
        /// value for the format is specified, a default format of "d.dddd" is used. Any string
        /// output by this method can be converted back into an Latitude object using the
        /// <strong>Parse</strong> method or <strong>Latitude(string)</strong> constructor.</remarks>
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
                if (double.IsPositiveInfinity(DecimalDegrees))
                    return "+" + Properties.Resources.Common_Infinity;
                // Is it infinity?
                if (double.IsNegativeInfinity(DecimalDegrees))
                    return "-" + Properties.Resources.Common_Infinity;
                if (double.IsNaN(DecimalDegrees))
                    return "NaN";

                // Use the default if "g" is passed
                format = format.ToLower(culture);

                // IF the format is "G", use the default format
                if (String.Compare(format, "g", true, culture) == 0)
                    format = "HH°MM'SS.SSSS\"i";

                // Replace the "d" with "h" since degrees is the same as hours
                format = format.Replace("d", "h")
                    // Convert the format to uppercase
                    .ToUpper(culture);
                // Only one decimal is allowed
                if (format.IndexOf(culture.NumberFormat.NumberDecimalSeparator) !=
                    format.LastIndexOf(culture.NumberFormat.NumberDecimalSeparator))
                    throw new ArgumentException(Properties.Resources.Latitude_OnlyRightmostIsDecimal);
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
                        // If an indicator is present, drop the minus sign
                        format = format.Replace(subFormat, format.IndexOf("I") > -1 ? Math.Abs(_decimalDegrees).ToString(newFormat, culture) : _decimalDegrees.ToString(newFormat, culture));
                    }
                    else
                    {
                        format = format.Replace(subFormat, format.IndexOf("I") > -1 ? Math.Abs((long)Hours).ToString(newFormat, culture) : Hours.ToString(newFormat, culture));
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
                            throw new ArgumentException(Properties.Resources.Latitude_OnlyRightmostIsDecimal);
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
                            throw new ArgumentException(Properties.Resources.Latitude_OnlyRightmostIsDecimal);
                        }
                        format = format.Replace(subFormat, Seconds.ToString(newFormat, culture));
                    }
                    else
                    {
                        format = format.Replace(subFormat, Seconds.ToString(newFormat, culture));
                    }
                }

                // Now add on an indicator if specified
                // Is there an hours specifier°
                startChar = format.IndexOf("I");
                if (startChar > -1)
                {
                    // Yes. Look for subsequent H characters or a period
                    endChar = format.LastIndexOf("I");
                    // Extract the sub-string
                    subFormat = format.Substring(startChar, endChar - startChar + 1);
                    // Convert to a numberic-formattable string
                    switch (subFormat.Length)
                    {
                        case 1: // Double character
                            format = format.Replace("I", Hemisphere.ToString().Substring(0, 1));
                            break;
                        case 3: // multiple character
                            format = format.Replace("III", Hemisphere.ToString());
                            break;
                    }
                }

                // If nothing then return zero
                if (String.Compare(format, "°", true, culture) == 0)
                    return "0°";
                return format;
            }
            catch
            {
                throw new ArgumentException(Properties.Resources.Angle_InvalidToStringFormat);
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

    /// <summary>
    /// Indicates the position of a latitude measurement relative to the equator.
    /// </summary>
    /// <seealso cref="Longitude.Hemisphere">Hemisphere Property (Longitude Class)</seealso>
    ///
    /// <seealso cref="LongitudeHemisphere">LongitudeHemisphere Enumeration</seealso>
    /// <remarks><para>This enumeration is used by the <see cref="Latitude.Hemisphere">Hemisphere</see>
    /// property of the <see cref="Latitude">Latitude</see> class. If a latitude is south of the
    /// equator, it's value is displayed as a negative number, or with a single letter (but not
    /// both). For example, 39 degrees south of the equator can be expressed in either of these
    /// ways:</para>
    ///   <list type="bullet">
    ///   <item>39°S</item>
    ///   <item>-39°</item>
    ///   </list></remarks>
    public enum LatitudeHemisphere
    {
        /// <summary>Missing latitude information.</summary>
        None = 0,
        /// <summary>The latitude is north of the equator.</summary>
        North = 1,
        /// <summary>The latitude is south of the equator.</summary>
        South = 2
    }
}