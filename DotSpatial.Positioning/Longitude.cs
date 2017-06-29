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
    /// Represents a line of constant distance east or west from the Prime Meridian.
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
    /// These examples create new instances of Longitude objects.
    ///   <code lang="VB" description="Create an angle of 90°">
    /// Dim MyLongitude As New Longitude(90)
    ///   </code>
    ///   <code lang="CS" description="Create an angle of 90°">
    /// Longitude MyLongitude = new Longitude(90);
    ///   </code>
    ///   <code lang="C++" description="Create an angle of 90°">
    /// Longitude MyLongitude = new Longitude(90);
    ///   </code>
    ///   <code lang="VB" description="Create an angle of 105°30'21.4">
    /// Dim MyLongitude1 As New Longitude(105, 30, 21.4)
    ///   </code>
    ///   <code lang="CS" description="Create an angle of 105°30'21.4">
    /// Longitude MyLongitude = new Longitude(105, 30, 21.4);
    ///   </code>
    ///   <code lang="C++" description="Create an angle of 105°30'21.4">
    /// Longitude MyLongitude = new Longitude(105, 30, 21.4);
    ///   </code>
    ///   </example>
    /// <remarks><para>Longitudes measure a distance either East or West from the Prime Meridian, an
    /// imaginary line which passes from the North Pole, through the
    ///   <see href="http://www.nmm.ac.uk/">Royal Observatory in Greenwich, England, and on
    /// to the South Pole</see>. Longitudes can range from -180 to 180°, with the Prime
    /// Meridian at 0°. Latitudes are commonly paired with Longitudes to mark a specific
    /// location on Earth's surface.</para>
    ///   <para>Latitudes are expressed in either of two major formats. The first format uses
    /// only positive numbers and the letter "E" or "W" to indicate the hemisphere (i.e.
    /// "94°E" or "32°W"). The second format allows negative numbers an omits the single
    /// character (i.e. 94 or -32).</para>
    ///   <para>Instances of this class are guaranteed to be thread-safe because the class is
    /// immutable (its properties can only be changed via constructors).</para></remarks>
    [TypeConverter("DotSpatial.Positioning.Design.LongitudeConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=1.0.0.0, PublicKeyToken=b4b0b185210c9dae")]
#endif
    public struct Longitude : IFormattable, IComparable<Longitude>, IEquatable<Longitude>, ICloneable<Longitude>, IXmlSerializable
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
        /// Represents a longitude of 0°.
        /// </summary>
        public static readonly Longitude PrimeMeridian = new Longitude(0.0);
        /// <summary>
        /// Represents a longitude 180°.
        /// </summary>
        public static readonly Longitude InternationalDateline = new Longitude(180.0);
        /// <summary>
        /// Represents a longitude of 0°.
        /// </summary>
        public static readonly Longitude Empty = new Longitude(0.0);
        /// <summary>
        /// Represents the minimum possible longitude of -180°.
        /// </summary>
        public static readonly Longitude Minimum = new Longitude(-180.0);
        /// <summary>
        /// Represents the maximum possible longitude of 180°.
        /// </summary>
        public static readonly Longitude Maximum = new Longitude(180.0);
        /// <summary>
        /// Represents an invalid or unspecified value.
        /// </summary>
        public static readonly Longitude Invalid = new Longitude(double.NaN);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance with the specified decimal degrees.
        /// </summary>
        /// <param name="decimalDegrees">The decimal degrees.</param>
        /// <example>
        /// This example demonstrates how to create an angle with a measurement of 90°.
        ///   <code lang="VB">
        /// Dim MyLongitude As New Longitude(90)
        ///   </code>
        ///   <code lang="CS">
        /// Longitude MyLongitude = new Longitude(90);
        ///   </code>
        ///   </example>
        ///
        /// <returns>An <strong>Longitude</strong> containing the specified value.</returns>
        public Longitude(double decimalDegrees)
        {
            // Set the decimal degrees value
            _decimalDegrees = decimalDegrees;
        }

        /// <summary>
        /// Creates a new instance with the specified decimal degrees and hemisphere.
        /// </summary>
        /// <param name="decimalDegrees">The decimal degrees.</param>
        /// <param name="hemisphere">The hemisphere.</param>
        /// <example>
        /// This example creates a new Longitude of 39°30' north.
        ///   <code lang="VB">
        /// Dim MyLongitude As New Longitude(39.5, LongitudeHemisphere.North)
        ///   </code>
        ///   <code lang="C#">
        /// Longitude MyLongitude = new Longitude(39.5, LongitudeHemisphere.North);
        ///   </code>
        /// This example creates a new Longitude of 39°30 south.
        ///   <code lang="VB">
        /// Dim MyLongitude As New Longitude(39.5, LongitudeHemisphere.South)
        ///   </code>
        ///   <code lang="C#">
        /// Longitude MyLongitude = new Longitude(39.5, LongitudeHemisphere.South);
        ///   </code>
        ///   </example>
        public Longitude(double decimalDegrees, LongitudeHemisphere hemisphere)
        {
            _decimalDegrees = ToDecimalDegrees(decimalDegrees, hemisphere);
        }

        /// <summary>
        /// Creates a new instance with the specified degrees.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <returns>An <strong>Longitude</strong> containing the specified value.</returns>
        public Longitude(int hours)
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
        /// Dim MyLongitude As New Longitude(34, 12, 29.2)
        ///   </code>
        ///   <code lang="CS">
        /// Longitude MyLongitude = new Longitude(34, 12, 29.2);
        ///   </code>
        ///   </example>
        ///
        /// <returns>An <strong>Longitude</strong> containing the specified value.</returns>
        public Longitude(int hours, int minutes, double seconds)
        {
            _decimalDegrees = ToDecimalDegrees(hours, minutes, seconds);
        }

        /// <summary>
        /// Creates a new instance using the specified decimal degrees and
        /// hemisphere.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="hemisphere">The hemisphere.</param>
        /// <remarks><para>This constructor is typically used to create a longitude when decimal degrees
        /// are always expressed as a positive number. Since the hemisphere property is set
        ///   <em>after</em> the DecimalDegrees property is set, the DecimalDegrees is adjusted
        /// automatically to be positive for the eastern hemisphere and negative for the
        /// western hemisphere.</para>
        ///   <para>If the parameters conflict with each other, the <strong>Hemisphere</strong>
        /// parameter takes precedence. Therefore, a value of "-19°E" will become "19°E"
        /// (without the negative sign) with no exception being thrown.</para></remarks>
        public Longitude(int hours, int minutes, double seconds, LongitudeHemisphere hemisphere)
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
        /// Dim MyLongitude As New Longitude(12, 42.345)
        ///   </code>
        ///   <code lang="VB">
        /// Longitude MyLongitude = new Longitude(12, 42.345);
        ///   </code>
        ///   </example>
        /// <remarks>An <strong>Longitude</strong> containing the specified value.</remarks>
        public Longitude(int hours, double decimalMinutes)
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
        /// This example creates a new Longitude of 39°12.34' north.
        ///   <code lang="VB">
        /// Dim MyLongitude As New Longitude(39, 12.34, LongitudeHemisphere.North)
        ///   </code>
        ///   <code lang="C#">
        /// Longitude MyLongitude = new Longitude(39, 12.34, LongitudeHemisphere.North);
        ///   </code>
        /// This example creates a new Longitude of 39°12.34 south.
        ///   <code lang="VB">
        /// Dim MyLongitude As New Longitude(39, 12.34, LongitudeHemisphere.South)
        ///   </code>
        ///   <code lang="C#">
        /// Longitude MyLongitude = new Longitude(39, 12.34, LongitudeHemisphere.South);
        ///   </code>
        ///   </example>
        public Longitude(int hours, double decimalMinutes, LongitudeHemisphere hemisphere)
        {
            _decimalDegrees = ToDecimalDegrees(hours, decimalMinutes, hemisphere);
        }

        /// <summary>
        /// Creates a new instance using the specified string-based measurement.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <seealso cref="Longitude.Parse(string)">Parse Method</seealso>
        ///
        /// <example>
        /// This example creates a new instance by parsing a string. notice: The double-quote is
        /// doubled up to represent a single double-quote in the string.)
        ///   <code lang="VB">
        /// Dim MyLongitude As New Longitude("123°45'67.8""")
        ///   </code>
        ///   <code lang="CS">
        /// Longitude MyLongitude = new Longitude("123°45'67.8\"");
        ///   </code>
        ///   </example>
        ///
        /// <returns>An <strong>Longitude</strong> containing the specified value.</returns>
        ///
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">The Parse method requires a decimal or sexagesimal measurement.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">Only the right-most portion of a sexagesimal measurement can be a fractional value.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">Extra characters were encountered while parsing an angular measurement.  Only hours, minutes, and seconds are allowed.</exception>
        ///
        /// <exception cref="FormatException" caption="FormatException">The specified text was not fully understood as an angular measurement.</exception>
        /// <remarks><para>A <strong>String</strong> in any of the following formats (or variation
        /// depending on the local culture):</para>
        ///   <para>
        ///   <table cellspacing="0" cols="4" cellpadding="2" width="100%">
        ///   <tbody>
        ///   <tr>
        ///   <td>hh</td>
        ///   <td>hh.h</td>
        ///   <td>hh mm</td>
        ///   <td>hh mm.mm</td>
        ///   </tr>
        ///   <tr>
        ///   <td>hh mm ss</td>
        ///   <td>hh mm ss.sss</td>
        ///   <td>hhi</td>
        ///   <td>hh.hi</td>
        ///   </tr>
        ///   <tr>
        ///   <td>hh mmi</td>
        ///   <td>hh mm i</td>
        ///   <td>hh mm.mi</td>
        ///   <td>hh mm.m i</td>
        ///   </tr>
        ///   <tr>
        ///   <td>hh mm ssi</td>
        ///   <td>hh mm ss i</td>
        ///   <td>hh mm ss.si</td>
        ///   <td>hh mm ss.s i</td>
        ///   </tr>
        ///   <tr>
        ///   <td>hhhmmssi</td>
        ///   <td></td>
        ///   <td></td>
        ///   <td></td>
        ///   </tr>
        ///   </tbody>
        ///   </table>
        ///   </para>
        ///   <para>Where <strong>h</strong> represents hours, <strong>m</strong> represents
        /// minutes, <strong>s</strong> represents seconds, and <strong>i</strong> represents a
        /// one-letter hemisphere indicator of "E" or "W." Any non-numeric character between
        /// numbers is considered a delimiter. Thus, a value of <strong>12°34'56.78"</strong>
        /// or even <strong>12A34B56.78C</strong> is treated the same as <strong>12 34
        /// 56.78</strong>.</para></remarks>
        public Longitude(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <summary>
        /// Creates a new instance using the specified string-based measurement.
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
        /// <remarks><para>A <strong>String</strong> in any of the following formats (or variation
        /// depending on the local culture):</para>
        ///   <para>
        ///   <table cellspacing="0" cols="4" cellpadding="2" width="100%">
        ///   <tbody>
        ///   <tr>
        ///   <td>hh</td>
        ///   <td>hh.h</td>
        ///   <td>hh mm</td>
        ///   <td>hh mm.mm</td>
        ///   </tr>
        ///   <tr>
        ///   <td>hh mm ss</td>
        ///   <td>hh mm ss.sss</td>
        ///   <td>hhi</td>
        ///   <td>hh.hi</td>
        ///   </tr>
        ///   <tr>
        ///   <td>hh mmi</td>
        ///   <td>hh mm i</td>
        ///   <td>hh mm.mi</td>
        ///   <td>hh mm.m i</td>
        ///   </tr>
        ///   <tr>
        ///   <td>hh mm ssi</td>
        ///   <td>hh mm ss i</td>
        ///   <td>hh mm ss.si</td>
        ///   <td>hh mm ss.s i</td>
        ///   </tr>
        ///   <tr>
        ///   <td>hhhmmssi</td>
        ///   <td></td>
        ///   <td></td>
        ///   <td></td>
        ///   </tr>
        ///   </tbody>
        ///   </table>
        ///   </para>
        ///   <para>Where <strong>h</strong> represents hours, <strong>m</strong> represents
        /// minutes, <strong>s</strong> represents seconds, and <strong>i</strong> represents a
        /// one-letter hemisphere indicator of "E" or "W." Any non-numeric character between
        /// numbers is considered a delimiter. Thus, a value of <strong>12°34'56.78"</strong>
        /// or even <strong>12A34B56.78C</strong> is treated the same as <strong>12 34
        /// 56.78</strong>.</para></remarks>
        public Longitude(string value, CultureInfo culture)
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

            // We'll be replacing and trimming a lot, so use a Stringbuilder
            StringBuilder newValue = new StringBuilder(value);

            // Try to extract the hemisphere
            LongitudeHemisphere hemisphere = LongitudeHemisphere.None;
            if (value.IndexOf('E') != -1
                // Ignore the "E-002" type scientific notation
                && value.IndexOf('E') != value.IndexOf("E-"))
            {
                hemisphere = LongitudeHemisphere.East;
                newValue.Replace("E", string.Empty);
            }
            else if (value.IndexOf('W') != -1)
            {
                hemisphere = LongitudeHemisphere.West;
                newValue.Replace("W", string.Empty);
            }
            else if (value.IndexOf('e') != -1
                // Ignore the "E-002" type scientific notation
                && value.IndexOf('e') != value.IndexOf("e-"))
            {
                hemisphere = LongitudeHemisphere.East;
                newValue.Replace("e", string.Empty);
            }
            else if (value.IndexOf('w') != -1)
            {
                hemisphere = LongitudeHemisphere.West;
                newValue.Replace("w", string.Empty);
            }
            else if (value.StartsWith("-"))
            {
                hemisphere = LongitudeHemisphere.West;
            }

            // Yes. First, clean up the strings
            try
            {
                // Clean up the string
                newValue.Replace("°", " ").Replace("'", " ").Replace("\"", " ").Replace("  ", " ");
                // Now split the values into an array
                string[] values = newValue.ToString().Trim().Split(' ');
                // How many elements are in the array?
                switch (values.Length)
                {
                    case 0:
                        // Return a blank Longitude
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
                            throw new ArgumentException(Properties.Resources.Longitude_OnlyRightmostIsDecimal, "value");
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
                            throw new ArgumentException(Properties.Resources.Longitude_OnlyRightmostIsDecimal, "value");
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
                    throw new ArgumentException(Properties.Resources.Longitude_InvalidFormat, ex);
#else
                throw new ArgumentException(Properties.Resources.Longitude_InvalidFormat, "value", ex);
#endif
            }
        }

        /// <summary>
        /// Creates a new instance by deserializing the specified XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public Longitude(XmlReader reader)
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
        /// Dim MyLongitude As New Longitude(20, 30)
        /// ' Setting the DecimalMinutes recalculated other properties
        /// Debug.WriteLine(MyLongitude.DecimalDegrees)
        /// ' Output: "20.5"  the same as 20°30'
        ///   </code>
        ///   <code lang="CS">
        /// // Create an angle of 20°30'
        /// Longitude MyLongitude = New Longitude(20, 30);
        /// // Setting the DecimalMinutes recalculated other properties
        /// Console.WriteLine(MyLongitude.DecimalDegrees)
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
        /// Dim MyLongitude As New Longitude(20, 10, 30)
        /// ' The DecimalMinutes property is automatically calculated
        /// Debug.WriteLine(MyLongitude.DecimalMinutes)
        /// ' Output: "10.5"
        ///   </code>
        ///   <code lang="CS">
        /// // Create an angle of 20°10'30"
        /// Longitude MyLongitude = new Longitude(20, 10, 30);
        /// // The DecimalMinutes property is automatically calculated
        /// Console.WriteLine(MyLongitude.DecimalMinutes)
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
        /// Dim MyLongitude As New Longitude(60.5)
        /// Debug.WriteLine(MyLongitude.Hours)
        /// ' Output: 60
        ///   </code>
        ///   <code lang="CS">
        /// Longitude MyLongitude = new Longitude(60.5);
        /// Console.WriteLine(MyLongitude.Hours);
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
        /// Dim MyLongitude As New Longitude(45.5)
        /// Debug.WriteLine(MyLongitude.Minutes)
        /// ' Output: 30
        ///   </code>
        ///   <code lang="CS">
        /// Longitude MyLongitude = new Longitude(45.5);
        /// Console.WriteLine(MyLongitude.Minutes);
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
        /// Dim MyLongitude As New Longitude(45, 10.5)
        /// Debug.WriteLine(MyLongitude.Seconds)
        /// ' Output: 30
        ///   </code>
        ///   <code lang="CS">
        /// Dim MyLongitude As New Longitude(45, 10.5);
        /// Console.WriteLine(MyLongitude.Seconds);
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
        /// Returns whether the longitude is east or west of the Prime Meridian.
        /// </summary>
        /// <remarks>When this property changes, the DecimalDegrees property is adjusted: if the
        /// hemisphere is <strong>West</strong>, a negative sign is placed in front of the
        /// DecimalDegrees value, and vice versa.</remarks>
        public LongitudeHemisphere Hemisphere
        {
            get
            {
                // And set the hemisphere
                if (_decimalDegrees < 0)
                    return LongitudeHemisphere.West;
                return LongitudeHemisphere.East;
            }
        }

        /// <summary>
        /// Returns the Universal Transverse Mercator zone number for this longitude.
        /// </summary>
        public int UtmZoneNumber
        {
            get
            {
#if Framework20 && !PocketPC
                double longitudeTemp = (DecimalDegrees + 180) - (int)Math.Truncate((DecimalDegrees + 180) / 360) * 360 - 180;
#else
            double LongitudeTemp = (DecimalDegrees + 180) - Angle.Truncate((DecimalDegrees + 180) / 360) * 360 - 180;
#endif

                //int ZoneNumber = 0;

                // Adjust for special zone numbers
                if (DecimalDegrees >= 56.0 && DecimalDegrees < 64.0 && longitudeTemp >= 3.0 && longitudeTemp < 12.0)
                {
                    return 32;
                }
                // Special zones for Svalbard
                if (DecimalDegrees >= 72 && DecimalDegrees < 84.0)
                {
                    if (longitudeTemp >= 0.0 && longitudeTemp < 9.0)
                    {
                        return 31;
                    }
                    if (longitudeTemp >= 9.0 && longitudeTemp < 21.0)
                    {
                        return 33;
                    }
                    if (longitudeTemp >= 21.0 && longitudeTemp < 33.0)
                    {
                        return 35;
                    }
                    if (longitudeTemp >= 33.0 && longitudeTemp < 42.0)
                    {
                        return 37;
                    }
                }

                //else
                {
#if Framework20 && !PocketPC
                    return (int)Math.Truncate((longitudeTemp + 180) / 6) + 1;
#else
                    return Angle.Truncate((LongitudeTemp + 180) / 6) + 1;
#endif
                }

                //return ZoneNumber;

                //                int Zone;
                //                if (ToRadians() < Math.PI)
                //#if Framework20 && !PocketPC
                //                    Zone = (int)Math.Truncate(31 + (DecimalDegrees / 6.0));
                //#else
                //                    Zone = Truncate(31 + (DecimalDegrees / 6.0));
                //#endif
                //                else
                //#if Framework20 && !PocketPC
                //                    Zone = (int)Math.Truncate((DecimalDegrees / 6.0) - 29);
                //#else
                //                    Zone = Truncate((DecimalDegrees / 6.0) - 29);
                //#endif
                //                if (Zone > 60)
                //                    Zone = 1;
                //                return Zone;
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
        /// allowed bounds of -180° and 180°.
        /// </summary>
        public bool IsNormalized
        {
            get { return _decimalDegrees >= -180 && _decimalDegrees <= 180; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns the object with the smallest value.
        /// </summary>
        /// <param name="value">An <strong>Longitude</strong> object to compare to the current instance.</param>
        /// <returns>The <strong>Longitude</strong> containing the smallest value.</returns>
        public Longitude LesserOf(Longitude value)
        {
            if (_decimalDegrees < value.DecimalDegrees)
                return this;
            return value;
        }

        /// <summary>
        /// Returns the object with the largest value.
        /// </summary>
        /// <param name="value">An <strong>Longitude</strong> object to compare to the current instance.</param>
        /// <returns>An <strong>Longitude</strong> containing the largest value.</returns>
        public Longitude GreaterOf(Longitude value)
        {
            if (_decimalDegrees > value.DecimalDegrees)
                return this;
            return value;
        }

        /// <summary>
        /// Returns a value indicating the relative order of two objects.
        /// </summary>
        /// <param name="value">An <strong>Longitude</strong> object to compare with.</param>
        /// <returns>A value of -1, 0, or 1 as documented by the IComparable interface.</returns>
        /// <remarks>This method allows collections of <strong>Longitude</strong> objects to be sorted.
        /// The <see cref="DecimalDegrees">DecimalDegrees</see> property of each instance is compared.</remarks>
        public int Compare(double value)
        {
            return _decimalDegrees.CompareTo(value);
        }

        /// <summary>
        /// Returns an angle opposite of the current instance.
        /// </summary>
        /// <returns>An <strong>Longitude</strong> representing the mirrored value.</returns>
        /// <example>
        /// This example creates a new <strong>Longitude</strong> of 45° then calculates its mirror
        /// of 225°. (45 + 180)
        ///   <code lang="VB" title="[New Example]">
        /// Dim Longitude1 As New Longitude(45)
        /// Dim Longitude2 As Longitude = Longitude1.Mirror()
        /// Debug.WriteLine(Longitude2.ToString())
        /// ' Output: 225
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Longitude Longitude1 = new Longitude(45);
        /// Longitude Longitude2 = Longitude1.Mirror();
        /// Console.WriteLine(Longitude2.ToString());
        /// // Output: 225
        ///   </code>
        ///   </example>
        /// <remarks>This method returns the "opposite" of the current instance. The opposite is
        /// defined as the point on the other side of an imaginary circle. For example, if an angle
        /// is 0°, at the top of a circle, this method returns 180°, at the bottom of the
        /// circle.</remarks>
        public Longitude Mirror()
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
        /// Dim MyLongitude As New Longitude(90)
        /// Dim MyRadians As Radian = MyLongitude.ToRadians()
        ///   </code>
        ///   <code lang="CS">
        /// Longitude MyLongitude = new Longitude(90);
        /// Radian MyRadians = MyLongitude.ToRadians();
        ///   </code>
        ///   </example>
        /// <remarks>This function is typically used to convert an angular measurement into
        /// radians before performing a trigonometric function.</remarks>
        public Radian ToRadians()
        {
            return new Radian(_decimalDegrees * Radian.RADIANS_PER_DEGREE);
        }

        /// <summary>
        /// Indicates if the current instance is East of the specified longitude.
        /// </summary>
        /// <param name="longitude">A <strong>Longitude</strong> object to examine.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is more East than the specified instance.</returns>
        public bool IsEastOf(Longitude longitude)
        {
            return IsGreaterThan(longitude);
        }

        /// <summary>
        /// Indicates if the current instance is West of the specified longitude.
        /// </summary>
        /// <param name="longitude">A <strong>Longitude</strong> object to examine.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is more West than the specified instance.</returns>
        public bool IsWestOf(Longitude longitude)
        {
            return IsLessThan(longitude);
        }

        /// <summary>
        /// Outputs the current instance as a string using the specified format.
        /// </summary>
        /// <param name="format"><para>A combination of symbols, spaces, and any of the following case-insensitive
        /// letters: <strong>D</strong> or <strong>H</strong> for hours, <strong>M</strong> for
        /// minutes, <strong>S</strong> for seconds, and <strong>I</strong> to indicate the
        /// hemisphere. Here are some examples:</para>
        ///   <para>
        ///   <table cellspacing="0" cols="3" cellpadding="2" width="100%">
        ///   <tbody>
        ///   <tr>
        ///   <td>HH°MM'SS.SS"</td>
        ///   <td>HHH.H°</td>
        ///   <td>HH MM.MM</td>
        ///   <td>HHHMMSS</td>
        ///   </tr>
        ///   <tr>
        ///   <td>HH°MM'SS.SS"I</td>
        ///   <td>HHH.H°I</td>
        ///   <td>HH MM.MMI</td>
        ///   <td>HHHMMSSI</td>
        ///   </tr>
        ///   </tbody>
        ///   </table>
        ///   </para></param>
        /// <returns>A <strong>String</strong> matching the specified format.</returns>
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
        /// Dim MyLongitude As New Longitude(45, 16.772)
        /// Debug.WriteLine(MyLongitude.ToString("h°m.mm"))
        /// ' Output: 45°16.78
        ///   </code>
        ///   <code lang="CS">
        /// Dim MyLongitude As New Longitude(45, 16.772);
        /// Debug.WriteLine(MyLongitude.ToString("h°m.mm"));
        /// // Output: 45°16.78
        ///   </code>
        ///   </example>
        /// <remarks>This powerful method returns the current angular measurement in a specific
        /// format. If no value for the format is specified, a format of
        /// <strong>hhh°mm'SS.SS"I</strong> (adjusted to the current culture) will be used. The
        /// resulting <strong>String</strong> can be converted back into an
        /// <strong>Longitude</strong> via the
        /// <see href="Angle.Parse">Parse</see> method so long as a delimiter separates each individual
        /// value.</remarks>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Returns the smallest integer greater than the specified value.
        /// </summary>
        /// <returns></returns>
        public Longitude Ceiling()
        {
            return new Longitude(Math.Ceiling(_decimalDegrees));
        }

        /// <summary>
        /// Returns the largest integer which is smaller than the specified value.
        /// </summary>
        /// <returns></returns>
        public Longitude Floor()
        {
            return new Longitude(Math.Floor(_decimalDegrees));
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
        public Longitude Round(int decimals)
        {
            return new Longitude(Math.Round(_decimalDegrees, decimals));
        }

        /// <summary>
        /// Returns a new instance whose Seconds property is evenly divisible by 15.
        /// </summary>
        /// <returns>An <strong>Longitude</strong> containing the rounded value.</returns>
        /// <remarks>This method is used to align or "snap" an angle to a regular interval. For
        /// example, a grid might be easier to read if it were drawn at 30-second intervals instead
        /// of 24.198-second intervals.</remarks>
        public Longitude RoundSeconds()
        {
            return RoundSeconds(15.0);
        }

        /// <summary>
        /// Returns a new angle whose Seconds property is evenly divisible by the specified amount.
        /// </summary>
        /// <param name="interval">A <strong>Double</strong> between 0 and 60 indicating the interval to round
        /// to.</param>
        /// <returns>An <strong>Longitude</strong> containing the rounded value.</returns>
        /// <remarks>This method is used to align or "snap" an angle to a regular interval. For
        /// example, a grid might be easier to read if it were drawn at 30-second intervals instead
        /// of 24.198-second intervals.</remarks>
        public Longitude RoundSeconds(double interval)
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
                return new Longitude(Hours, Minutes, newSeconds);
            }
            // return the new value
            return new Longitude(Hours, Minutes, newSeconds);
        }

        /// <summary>
        /// Normalizes this instance.
        /// </summary>
        /// <returns>A <strong>Longitude</strong> containing the normalized value.</returns>
        /// <remarks>This function is used to ensure that an angular measurement is within the
        /// allowed bounds of 0° and 180°. If a value of 360° or 720° is passed, a value of 0°
        /// is returned since traveling around the Earth 360° or 720° brings you to the same
        /// place you started.</remarks>
        public Longitude Normalize()
        {
            // Is the value not a number, infinity, or already normalized?
            if (double.IsInfinity(_decimalDegrees) || double.IsNaN(_decimalDegrees))
                return this;
            // If we're off the eastern edge (180E) wrap back around from the west
            if (_decimalDegrees > 180)
                return new Longitude(-180 + (_decimalDegrees % 180));
            // If we're off the western edge (180W) wrap back around from the east
            return _decimalDegrees < -180 ? new Longitude(180 + (_decimalDegrees % 180)) : this;

            /*
            // Calculate the number of times the degree value winds completely
            // through a hemisphere
            int HemisphereFlips = Convert.ToInt32(Math.Floor(_DecimalDegrees / 360.0));

            // If the value is in the western hemisphere, apply another flip
            if (_DecimalDegrees < 0)
                HemisphereFlips++;

            // Calculate the new value
            double NewValue = _DecimalDegrees % 360;

            // if the value is > 180, return 360 - X
            if (NewValue > 180)
                NewValue = 360 - NewValue;

            // If the value id < -180, return -360 - X
            else if (NewValue < -180.0)
                NewValue = -360.0 - NewValue;

            if (HemisphereFlips % 2 != 0)
                return new Longitude(-NewValue);
            else
                return new Longitude(NewValue);
             */
        }

        #endregion Public Methods

        #region Overrides

        /// <summary>
        /// Compares the current value to another Longitude object's value.
        /// </summary>
        /// <param name="obj">An <strong>Longitude</strong>, <strong>Double</strong>, or <strong>Integer</strong>
        /// to compare with.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the object's DecimalDegrees
        /// properties match.</returns>
        /// <remarks>This</remarks>
        public override bool Equals(object obj)
        {
            // Only compare the same type
            if (obj is Longitude)
                return Equals((Longitude)obj);
            return false;
        }

        /// <summary>
        /// Returns a unique code for this instance.
        /// </summary>
        /// <returns>An <strong>Integer</strong> representing a unique code for the current
        /// instance.</returns>
        /// <remarks>Since the <strong>Longitude</strong> class is immutable, this property may be used
        /// safely with hash tables.</remarks>
        public override int GetHashCode()
        {
            return _decimalDegrees.GetHashCode();
        }

        /// <summary>
        /// Outputs the current instance as a string using the specified format.
        /// </summary>
        /// <returns>A <strong>String</strong> matching the specified format.</returns>
        /// <seealso cref="Parse(string)">Parse Method</seealso>
        ///
        /// <example>
        /// This example outputs a value of 90 degrees in the default format of ###.#°.
        ///   <code lang="VB">
        /// Dim MyLongitude As New Longitude(90)
        /// Debug.WriteLine(MyLongitude.ToString)
        /// ' Output: "90°"
        ///   </code>
        ///   <code lang="CS">
        /// Longitude MyLongitude = new Longitude(90);
        /// Debug.WriteLine(MyLongitude.ToString());
        /// // Output: "90°"
        ///   </code>
        ///   </example>
        /// <remarks>This powerful method returns the current angular measurement in a specific
        /// format. If no value for the format is specified, a format of
        /// <strong>hhh°mm'SS.SS"I</strong> (adjusted to the current culture) will be used. The
        /// resulting <strong>String</strong> can be converted back into an
        /// <strong>Longitude</strong> via the
        /// <see href="Angle.Parse">Parse</see> method so long as a delimiter separates each individual
        /// value.</remarks>
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
        /// <remarks>This function is used to ensure that an angular measurement is within the
        /// allowed bounds of -180° and 180°. If a value of 360° or 720° is passed, a value of 0°
        /// is returned since traveling around the Earth 360° or 720° brings you to the same
        /// place you started.</remarks>
        public static Longitude Normalize(double decimalDegrees)
        {
            return new Longitude(decimalDegrees).Normalize();
        }

        /// <summary>
        /// Returns the object with the smallest value.
        /// </summary>
        /// <param name="value1">A <strong>Longitude</strong> object to compare to value2.</param>
        /// <param name="value2">A <strong>Longitude</strong> object to compare to value1.</param>
        /// <returns>The <strong>Longitude</strong> containing the smallest value.</returns>
        public static Longitude LesserOf(Longitude value1, Longitude value2)
        {
            return value1.LesserOf(value2);
        }

        /// <summary>
        /// Returns the object with the largest value.
        /// </summary>
        /// <param name="value1">A <strong>Longitude</strong> object to compare to value2.</param>
        /// <param name="value2">A <strong>Longitude</strong> object to compare to value1.</param>
        /// <returns>A <strong>Longitude</strong> containing the largest value.</returns>
        public static Longitude GreaterOf(Longitude value1, Longitude value2)
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
        /// Dim MyRadian As Radian = Longitude.ToRadians(90)
        ///   </code>
        ///   <code lang="CS">
        /// Radian MyRadian = Longitude.ToRadians(90);
        ///   </code>
        ///   </example>
        /// <remarks>This function is typically used to convert an angular measurement into
        /// radians before performing a trigonometric function.</remarks>
        public static Radian ToRadians(Longitude value)
        {
            return value.ToRadians();
        }

        /// <summary>
        /// Converts a value in radians into an angular measurement.
        /// </summary>
        /// <param name="radians">The radians.</param>
        /// <returns></returns>
        /// <seealso cref="Longitude.ToRadians()">ToRadians</seealso>
        ///
        /// <seealso cref="Radian">Radian Class</seealso>
        ///
        /// <example>
        /// This example uses the <strong>FromRadians</strong> method to convert a value of one
        /// radian into an <strong>Longitude</strong> of 57°.
        ///   <code lang="VB">
        /// ' Create a new angle equal to one radian
        /// Dim MyRadians As New Radian(1)
        /// Dim MyLongitude As Longitude = Longitude.FromRadians(MyRadians)
        /// Debug.WriteLine(MyLongitude.ToString())
        /// ' Output: 57°
        ///   </code>
        ///   <code lang="CS">
        /// // Create a new angle equal to one radian
        /// Radian MyRadians = new Radian(1);
        /// Longitude MyLongitude = Longitude.FromRadians(MyRadians);
        /// Console.WriteLine(MyLongitude.ToString());
        /// // Output: 57°
        ///   </code>
        ///   </example>
        /// <remarks>This function is typically used in conjunction with the
        /// <see cref="Longitude.ToRadians()">ToRadians</see>
        /// method after a trigonometric function has completed. The converted value is stored in
        /// the <see cref="DecimalDegrees">DecimalDegrees</see> property.</remarks>
        public static Longitude FromRadians(Radian radians)
        {
            return new Longitude(radians.ToDegrees());
        }

        /// <summary>
        /// Froms the radians.
        /// </summary>
        /// <param name="radians">The radians.</param>
        /// <returns></returns>
        public static Longitude FromRadians(double radians)
        {
            return new Longitude(Radian.ToDegrees(radians));
        }

        /// <summary>
        /// Returns a random longitude.
        /// </summary>
        /// <returns></returns>
        public static Longitude Random()
        {
            return Random(new Random());
        }

        /// <summary>
        /// Returns a random longitude based on the specified seed.
        /// </summary>
        /// <param name="generator">The generator.</param>
        /// <returns></returns>
        public static Longitude Random(Random generator)
        {
            return new Longitude((generator.NextDouble() * 180.0) - 90.0);
        }

        /// <summary>
        /// Returns a random longitude using the specified eastern and western boundaries.
        /// </summary>
        /// <param name="easternmost">A <strong>Longitude</strong> specifying the eastern-most allowed longitude.</param>
        /// <param name="westernmost">A <strong>Longitude</strong> specifying the western-most allowed longitude.</param>
        /// <returns></returns>
        public static Longitude Random(Longitude easternmost, Longitude westernmost)
        {
            return Random(new Random(), easternmost, westernmost);
        }

        /// <summary>
        /// Returns a random longitude between the specified minimum and maximum.
        /// </summary>
        /// <param name="generator">A <strong>Random</strong> object used to generate random values.</param>
        /// <param name="easternmost">A <strong>Longitude</strong> specifying the eastern-most allowed longitude.</param>
        /// <param name="westernmost">A <strong>Longitude</strong> specifying the western-most allowed longitude.</param>
        /// <returns></returns>
        public static Longitude Random(Random generator, Longitude easternmost, Longitude westernmost)
        {
            return new Longitude(((easternmost.DecimalDegrees - westernmost.DecimalDegrees) * generator.NextDouble())
                                + westernmost.DecimalDegrees);
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
        /// Converts arbitrary hour and decimal minutes into decimal degrees.
        /// </summary>
        /// <param name="hours">The hours.</param>
        /// <param name="decimalMinutes">The decimal minutes.</param>
        /// <param name="hemisphere">The hemisphere.</param>
        /// <returns>A <strong>Double</strong> containing the decimal degree version of the specified
        /// values.</returns>
        /// <remarks>The specified value will be converted to decimal degrees, then rounded to thirteen digits, the maximum precision allowed by this type.</remarks>
        public static double ToDecimalDegrees(int hours, double decimalMinutes, LongitudeHemisphere hemisphere)
        {
            //switch (hemisphere)
            //{
            //    case LongitudeHemisphere.West:
            //        return -Math.Abs(hours) - Math.Round(decimalMinutes / 60.0, MaximumPrecisionDigits);
            //    case LongitudeHemisphere.East:
            //        return Math.Abs(hours) + Math.Round(decimalMinutes / 60.0, MaximumPrecisionDigits);
            //    default:
            //        return ToDecimalDegrees(hours, decimalMinutes);
            //}
            switch (hemisphere)
            {
                case LongitudeHemisphere.West:
                    return -Math.Abs(hours) - decimalMinutes / 60.0;
                case LongitudeHemisphere.East:
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
        public static double ToDecimalDegrees(double decimalDegrees, LongitudeHemisphere hemisphere)
        {
            //switch (hemisphere)
            //{
            //    case LongitudeHemisphere.West:
            //        return -Math.Abs(Math.Round(decimalDegrees, MaximumPrecisionDigits));
            //    case LongitudeHemisphere.East:
            //        return Math.Abs(Math.Round(decimalDegrees, MaximumPrecisionDigits));
            //    default:
            //        return ToDecimalDegrees(decimalDegrees);
            //}
            switch (hemisphere)
            {
                case LongitudeHemisphere.West:
                    return -Math.Abs(decimalDegrees);
                case LongitudeHemisphere.East:
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
        public static double ToDecimalDegrees(int hours, int minutes, double seconds, LongitudeHemisphere hemisphere)
        {
            switch (hemisphere)
            {
                case LongitudeHemisphere.West:
                    return -Math.Abs(hours) - minutes / 60.0 - seconds / 3600.0;
                case LongitudeHemisphere.East:
                    return Math.Abs(hours) + minutes / 60.0 + seconds / 3600.0;
                default:
                    return ToDecimalDegrees(hours, minutes, seconds);
            }
            //switch (hemisphere)
            //{
            //    case LongitudeHemisphere.West:
            //        return -Math.Abs(hours) - Math.Round(minutes / 60.0, MaximumPrecisionDigits) - Math.Round(seconds / 3600.0, MaximumPrecisionDigits);
            //    case LongitudeHemisphere.East:
            //        return Math.Abs(hours) + Math.Round(minutes / 60.0, MaximumPrecisionDigits) + Math.Round(seconds / 3600.0, MaximumPrecisionDigits);
            //    default:
            //        return ToDecimalDegrees(hours, minutes, seconds);
            //}
        }

        /// <summary>
        /// Converts the specified string into an Longitude object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A new <strong>Longitude</strong> object populated with the specified
        /// values.</returns>
        /// <seealso cref="ToString()">ToString Method</seealso>
        ///
        /// <example>
        /// This example creates a new angular measurement using the <strong>Parse</strong>
        /// method.
        ///   <code lang="VB">
        /// Dim NewLongitude As Longitude = Longitude.Parse("123.45°")
        ///   </code>
        ///   <code lang="CS">
        /// Longitude NewLongitude = Longitude.Parse("123.45°");
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
        /// <remarks>This method parses the specified string into an <strong>Longitude</strong> object
        /// using the current culture. This constructor can parse any strings created via the
        /// <strong>ToString</strong> method.</remarks>
        public static Longitude Parse(string value)
        {
            return new Longitude(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts the specified string into an <strong>Longitude</strong> object using the
        /// specified culture.
        /// </summary>
        /// <param name="value">A <strong>String</strong> describing an angle in the form of decimal degrees or a
        /// sexagesimal.</param>
        /// <param name="culture">A <strong>CultureInfo</strong> object describing the numeric format to use during
        /// conversion.</param>
        /// <returns>A new <strong>Longitude</strong> object equivalent to the specified string.</returns>
        /// <remarks>This powerful method is typically used to process data from a data store or a
        /// value input by the user in any culture. This function can accept any format which
        /// can be output by the ToString method.</remarks>
        public static Longitude Parse(string value, CultureInfo culture)
        {
            return new Longitude(value, culture);
        }

        #endregion Static Methods

        #region Operators

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Longitude operator +(Longitude left, Longitude right)
        {
            return new Longitude(left.DecimalDegrees + right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Longitude operator +(Longitude left, double right)
        {
            return new Longitude(left.DecimalDegrees + right);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Longitude operator -(Longitude left, Longitude right)
        {
            return new Longitude(left.DecimalDegrees - right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Longitude operator -(Longitude left, double right)
        {
            return new Longitude(left.DecimalDegrees - right);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Longitude operator *(Longitude left, Longitude right)
        {
            return new Longitude(left.DecimalDegrees * right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Longitude operator *(Longitude left, double right)
        {
            return new Longitude(left.DecimalDegrees * right);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Longitude operator /(Longitude left, Longitude right)
        {
            return new Longitude(left.DecimalDegrees / right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Longitude operator /(Longitude left, double right)
        {
            return new Longitude(left.DecimalDegrees / right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Longitude left, Longitude right)
        {
            return left.DecimalDegrees.Equals(right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Longitude left, double right)
        {
            return left.DecimalDegrees.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Longitude left, Longitude right)
        {
            return !left.DecimalDegrees.Equals(right.DecimalDegrees);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Longitude left, double right)
        {
            return !left.DecimalDegrees.Equals(right);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Longitude left, Longitude right)
        {
            return left.DecimalDegrees > right.DecimalDegrees;
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Longitude left, double right)
        {
            return left.DecimalDegrees > right;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(Longitude left, Longitude right)
        {
            return left.DecimalDegrees >= right.DecimalDegrees;
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(Longitude left, double right)
        {
            return left.DecimalDegrees >= right;
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Longitude left, Longitude right)
        {
            return left.DecimalDegrees < right.DecimalDegrees;
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Longitude left, double right)
        {
            return left.DecimalDegrees < right;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(Longitude left, Longitude right)
        {
            return left.DecimalDegrees <= right.DecimalDegrees;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(Longitude left, double right)
        {
            return left.DecimalDegrees <= right;
        }

        /// <summary>
        /// Returns the current instance increased by one.
        /// </summary>
        /// <returns>An <strong>Longitude</strong> object.</returns>
        /// <example>
        /// This example uses the <strong>Increment</strong> method to increase an Longitude's
        /// value. It also demonstrates the subtle error which can be caused if
        ///   <strong>Increment</strong> is called while ignoring the return value.
        ///   <code lang="VB" title="[New Example]">
        /// ' Correct use of Increment
        /// Dim Longitude1 As New Longitude(89)
        /// Longitude1 = Longitude1.Increment()
        /// ' Incorrect use of Increment
        /// Dim Longitude1 = New Longitude(89)
        /// Longitude1.Increment()
        /// ' notice: Longitude1 will still be 89°!
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// // Correct use of Increment
        /// Longitude Longitude1 = new Longitude(89);
        /// Longitude1 = Longitude1.Increment();
        /// // Incorrect use of Increment
        /// Longitude Longitude1 = new Longitude(89);
        /// Longitude1.Increment();
        /// // notice: Longitude1 will still be 89°!
        ///   </code>
        ///   </example>
        /// <remarks><para>This method increases the <strong>DecimalDegrees</strong> property by 1.0,
        /// returned as a new instance.</para>
        ///   <para><font color="red">Since the <strong>Longitude</strong> class is immutable, this
        /// method cannot be used to modify an existing instance.</font></para></remarks>
        public Longitude Increment()
        {
            return new Longitude(_decimalDegrees + 1.0);
        }

        /// <summary>
        /// Increases the current instance by the specified value.
        /// </summary>
        /// <param name="value">A <strong>Double</strong> to add to the current instance.</param>
        /// <returns>A new <strong>Longitude</strong> containing the summed values.</returns>
        /// <example>
        /// This example adds 45° to the current instance of 45°, returning 90°.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Longitude1 As New Longitude(45)
        /// Longitude1 = Longitude1.Add(45)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Longitude Longitude1 = new Longitude(45);
        /// Longitude1 = Longitude1.Add(45);
        ///   </code>
        ///   </example>
        public Longitude Add(double value)
        {
            return new Longitude(_decimalDegrees + value);
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Longitude Add(Longitude value)
        {
            return new Longitude(_decimalDegrees + value.DecimalDegrees);
        }

        /// <summary>
        /// Returns the current instance decreased by one.
        /// </summary>
        /// <returns>An <strong>Longitude</strong> object.</returns>
        /// <example>
        /// This example uses the <strong>Decrement</strong> method to decrease an Longitude's
        /// value. It also demonstrates the subtle error which can be caused if
        ///   <strong>Decrement</strong> is called while ignoring the return value.
        ///   <code lang="VB" title="[New Example]">
        /// ' Correct use of Decrement
        /// Dim Longitude1 As New Longitude(91)
        /// Longitude1 = Longitude1.Decrement()
        /// ' Incorrect use of Decrement
        /// Dim Longitude1 = New Longitude(91)
        /// Longitude1.Increment()
        /// ' notice Longitude1 will still be 91°!
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// // Correct use of Decrement
        /// Longitude Longitude1 = new Longitude(91);
        /// Longitude1 = Longitude1.Decrement();
        /// // Incorrect use of Decrement
        /// Longitude Longitude1 = new Longitude(91);
        /// Longitude1.Decrement();
        /// // notice: Longitude1 will still be 91°!
        ///   </code>
        ///   </example>
        /// <remarks><para>This method decreases the <strong>DecimalDegrees</strong> property by 1.0,
        /// returned as a new instance.</para>
        ///   <para><font color="red">Since the <strong>Longitude</strong> class is immutable, this
        /// method cannot be used to modify an existing instance.</font></para></remarks>
        public Longitude Decrement()
        {
            return new Longitude(_decimalDegrees - 1.0);
        }

        /// <summary>
        /// Decreases the current instance by the specified value.
        /// </summary>
        /// <param name="value">A <strong>Double</strong> to subtract from the current instance.</param>
        /// <returns>A new <strong>Longitude</strong> containing the new value.</returns>
        /// <example>
        /// This example subtracts 30° from the current instance of 90°, returning 60°.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Longitude1 As New Longitude(90)
        /// Longitude1 = Longitude1.Subtract(30)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Longitude Longitude1 = new Longitude(90);
        /// Longitude1 = Longitude1.Subtract(30);
        ///   </code>
        ///   </example>
        public Longitude Subtract(double value)
        {
            return new Longitude(_decimalDegrees - value);
        }

        /// <summary>
        /// Subtracts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Longitude Subtract(Longitude value)
        {
            return new Longitude(_decimalDegrees - value.DecimalDegrees);
        }

        /// <summary>
        /// Multiplies the current instance by the specified value.
        /// </summary>
        /// <param name="value">A <strong>Double</strong> to multiply with the current instance.</param>
        /// <returns>A new <strong>Longitude</strong> containing the product of the two numbers.</returns>
        /// <example>
        /// This example multiplies 30° with three, returning 90°.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Longitude1 As New Longitude(30)
        /// Longitude1 = Longitude1.Multiply(3)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Longitude Longitude1 = new Longitude(30);
        /// Longitude1 = Longitude1.Multiply(3);
        ///   </code>
        ///   </example>
        public Longitude Multiply(double value)
        {
            return new Longitude(_decimalDegrees * value);
        }

        /// <summary>
        /// Multiplies the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Longitude Multiply(Longitude value)
        {
            return new Longitude(_decimalDegrees * value.DecimalDegrees);
        }

        /// <summary>
        /// Divides the current instance by the specified value.
        /// </summary>
        /// <param name="value">A <strong>Double</strong> representing a denominator to divide by.</param>
        /// <returns>An <strong>Longitude</strong> containing the new value.</returns>
        /// <example>
        /// This example divides 90° by three, returning 30°.
        ///   <code lang="VB" title="[New Example]">
        /// Dim Longitude1 As New Longitude(90)
        /// Longitude1 = Longitude1.Divide(3)
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// Longitude Longitude1 = new Longitude(90);
        /// Longitude1 = Longitude1.Divide(3);
        ///   </code>
        ///   </example>
        public Longitude Divide(double value)
        {
            return new Longitude(_decimalDegrees / value);
        }

        /// <summary>
        /// Divides the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Longitude Divide(Longitude value)
        {
            return new Longitude(_decimalDegrees / value.DecimalDegrees);
        }

        /// <summary>
        /// Indicates if the current instance is smaller than the specified value.
        /// </summary>
        /// <param name="value">An <strong>Longitude</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// smaller than the specified value.</returns>
        public bool IsLessThan(Longitude value)
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
        /// <param name="value">An <strong>Longitude</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// smaller than or equal to the specified value.</returns>
        /// <remarks>This method compares the <strong>DecimalDegrees</strong> property with the
        /// specified value. This method is the same as the "&lt;=" operator.</remarks>
        public bool IsLessThanOrEqualTo(Longitude value)
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
        /// <param name="value">An <strong>Longitude</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// greater than the specified value.</returns>
        public bool IsGreaterThan(Longitude value)
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
        /// <param name="value">An <strong>Longitude</strong> to compare with the current instance.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// greater than or equal to the specified value.</returns>
        public bool IsGreaterThanOrEqualTo(Longitude value)
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
        /// Converts a measurement in Radians into an Longitude.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Longitude(Radian value)
        {
            return new Longitude(value.ToDegrees());
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Longitude.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Longitude(double value)
        {
            return new Longitude(value);
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Longitude.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Longitude(float value)
        {
            return new Longitude(Convert.ToDouble(value));
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Longitude.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator double(Longitude value)
        {
            return value.DecimalDegrees;
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Longitude.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator float(Longitude value)
        {
            return Convert.ToSingle(value.DecimalDegrees);
        }

        /// <summary>
        /// Converts a measurement in degrees as an Integer into an Longitude.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Longitude(int value)
        {
            return new Longitude(value);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Angle"/> to <see cref="DotSpatial.Positioning.Longitude"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Longitude(Angle value)
        {
            return new Longitude(value.DecimalDegrees);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Azimuth"/> to <see cref="DotSpatial.Positioning.Longitude"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Longitude(Azimuth value)
        {
            return new Longitude(value.DecimalDegrees);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Elevation"/> to <see cref="DotSpatial.Positioning.Longitude"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Longitude(Elevation value)
        {
            return new Longitude(value.DecimalDegrees);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Latitude"/> to <see cref="DotSpatial.Positioning.Longitude"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Longitude(Latitude value)
        {
            return new Longitude(value.DecimalDegrees);
        }

        /// <summary>
        /// Converts a measurement in the form of a formatted String into an Longitude.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Longitude(string value)
        {
            return new Longitude(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts an Longitude into a String.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        /// <remarks>This operator calls the ToString() method using the current culture.</remarks>
        public static explicit operator string(Longitude value)
        {
            return value.ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion Conversions

        #region ICloneable<Longitude> Members

        /// <summary>
        /// Creates a copy of the current instance.
        /// </summary>
        /// <returns>An <strong>Longitude</strong> of the same value as the current instance.</returns>
        public Longitude Clone()
        {
            return new Longitude(_decimalDegrees);
        }

        #endregion ICloneable<Longitude> Members

        #region IEquatable<Longitude> Members

        /// <summary>
        /// Compares the current instance to another instance using the specified
        /// precision.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <param name="decimals">The decimals.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the
        /// <strong>DecimalDegrees</strong> property of the current instance matches the
        /// specified instance's <strong>DecimalDegrees</strong> property.</returns>
        /// <seealso cref="Equals(Longitude)">Equals Method</seealso>
        ///
        /// <example>
        /// These examples compare two fractional values using specific numbers of digits for
        /// comparison.
        ///   <code lang="VB" title="[New Example]">
        /// ' Equals will return False
        /// Dim Longitude1 As New Longitude(90.15);
        /// Dim Longitude2 As New Longitude(90.12);
        /// If Longitude1.Equals(Longitude2, 2) Then
        /// Debug.WriteLine("The values are the same to two digits of precision.");
        /// ' Equals will return True
        /// Dim Longitude1 As New Longitude(90.15);
        /// Dim Longitude2 As New Longitude(90.12);
        /// If Longitude1.Equals(Longitude2, 1) Then
        /// Debug.WriteLine("The values are the same to one digit of precision.");
        ///   </code>
        ///   <code lang="CS" title="[New Example]">
        /// // Equals will return False
        /// Longitude Longitude1 = new Longitude(90.15);
        /// Longitude Longitude2 = new Longitude(90.12);
        /// if (Longitude1.Equals(Longitude2, 2))
        /// Console.WriteLine("The values are the same to two digits of precision.");
        /// // Equals will return True
        /// Longitude Longitude1 = new Longitude(90.15);
        /// Longitude Longitude2 = new Longitude(90.12);
        /// if (Longitude1.Equals(Longitude2, 1))
        /// Console.WriteLine("The values are the same to one digits of precision.");
        ///   </code>
        ///   </example>
        /// <remarks><para>This is typically used in cases where precision is only significant for a few
        /// digits and exact comparison is not necessary.</para>
        ///   <para><em>notice: This method compares objects by value, not by
        /// reference.</em></para></remarks>
        public bool Equals(Longitude other, int decimals)
        {
            return Math.Round(_decimalDegrees, decimals) == Math.Round(other.DecimalDegrees, decimals);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        public bool Equals(Longitude other)
        {
            return _decimalDegrees.Equals(other.DecimalDegrees);
        }

        #endregion IEquatable<Longitude> Members

        #region IComparable<Longitude> Members

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
        public int CompareTo(Longitude other)
        {
            return _decimalDegrees.CompareTo(other.DecimalDegrees);
        }

        #endregion IComparable<Longitude> Members

        #region IFormattable Members

        /// <summary>
        /// Outputs the current instance as a string using the specified format.
        /// </summary>
        /// <param name="format"><para>A combination of symbols, spaces, and any of the following case-insensitive
        /// letters: <strong>D</strong> or <strong>H</strong> for hours, <strong>M</strong> for
        /// minutes, <strong>S</strong> for seconds, and <strong>I</strong> to indicate the
        /// hemisphere. Here are some examples:</para>
        ///   <para>
        ///   <table cellspacing="0" cols="3" cellpadding="2" width="100%">
        ///   <tbody>
        ///   <tr>
        ///   <td>HH°MM'SS.SS"</td>
        ///   <td>HHH.H°</td>
        ///   <td>HH MM.MM</td>
        ///   <td>HHHMMSS</td>
        ///   </tr>
        ///   <tr>
        ///   <td>HH°MM'SS.SS"I</td>
        ///   <td>HHH.H°I</td>
        ///   <td>HH MM.MMI</td>
        ///   <td>HHHMMSSI</td>
        ///   </tr>
        ///   </tbody>
        ///   </table>
        ///   </para></param>
        /// <param name="formatProvider">A <strong>CultureInfo</strong> object used to properly format the string information.</param>
        /// <returns>A <strong>String</strong> matching the specified format.</returns>
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
        /// Dim MyLongitude As New Longitude(45, 16.772)
        /// Debug.WriteLine(MyLongitude.ToString("h°m.mm", CultureInfo.CurrentCulture))
        /// ' Output: 45°16.78
        ///   </code>
        ///   <code lang="CS">
        /// Dim MyLongitude As New Longitude(45, 16.772);
        /// Debug.WriteLine(MyLongitude.ToString("h°m.mm", CultureInfo.CurrentCulture));
        /// // Output: 45°16.78
        ///   </code>
        ///   </example>
        /// <remarks>This powerful method returns the current angular measurement in a specific
        /// format. If no value for the format is specified, a format of
        /// <strong>hhh°mm'SS.SS"I</strong> (adjusted to the current culture) will be used. The
        /// resulting <strong>String</strong> can be converted back into an
        /// <strong>Longitude</strong> via the
        /// <see href="Angle.Parse">Parse</see> method so long as a delimiter separates each individual
        /// value.</remarks>
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
                    format = "HHH°MM'SS.SSSS\"i";

                // Replace the "d" with "h" since degrees is the same as hours
                format = format.Replace("d", "h")
                    // Convert the format to uppercase
                    .ToUpper(culture);
                // Only one decimal is allowed
                if (format.IndexOf(culture.NumberFormat.NumberDecimalSeparator) !=
                    format.LastIndexOf(culture.NumberFormat.NumberDecimalSeparator))
                    throw new ArgumentException(Properties.Resources.Longitude_OnlyRightmostIsDecimal);
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
                        format = format.Replace(subFormat, format.IndexOf("I") > -1 ? Math.Abs(Hours).ToString(newFormat, culture) : Hours.ToString(newFormat, culture));
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
                            throw new ArgumentException(Properties.Resources.Longitude_OnlyRightmostIsDecimal);
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
                            throw new ArgumentException(Properties.Resources.Longitude_OnlyRightmostIsDecimal);
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
    /// Indicates the position of a longitude measurement relative to the <a href="http://www.greenwichmeridian.com/">Prime Meridian</a>.
    /// </summary>
    /// <seealso cref="Latitude.Hemisphere">Hemisphere Property (Latitude Class)</seealso>
    ///
    /// <seealso cref="LatitudeHemisphere">LatitudeHemisphere Enumeration</seealso>
    /// <remarks><para>This enumeration is used by the <see cref="Longitude.Hemisphere">Hemisphere</see>
    /// property of the <see cref="Longitude">Latitude</see> class. If a longitude is west of the
    /// Prime Meridian, it's value is displayed as a negative number, or with a single letter (but not
    /// both). For example, 105 degrees west can be expressed in either of these
    /// ways:</para>
    ///   <list type="bullet">
    ///   <item>105°W</item>
    ///   <item>-105°</item>
    ///   </list></remarks>
    public enum LongitudeHemisphere
    {
        /// <summary>Missing longitude information.</summary>
        None = 0,
        /// <summary>The longitude is east of the Prime Meridian.</summary>
        East = 1,
        /// <summary>The longitude is west of the Prime Meridian.</summary>
        West = 2
    }
}