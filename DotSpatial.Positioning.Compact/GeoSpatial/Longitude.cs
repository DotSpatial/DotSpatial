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
	/// <summary>
	/// Represents a line of constant distance east or west from the Prime Meridian.
	/// </summary>
	/// <remarks>
	/// 	<para>Longitudes measure a distance either East or West from the Prime Meridian, an
	///     imaginary line which passes from the North Pole, through the
	///     <see href="http://www.nmm.ac.uk/">Royal Observatory in Greenwich, England, and on
	///     to the South Pole</see>. Longitudes can range from -180 to 180°, with the Prime
	///     Meridian at 0°. Latitudes are commonly paired with Longitudes to mark a specific
	///     location on Earth's surface.</para>
	/// 	<para>Latitudes are expressed in either of two major formats. The first format uses
	///     only positive numbers and the letter "E" or "W" to indicate the hemisphere (i.e.
	///     "94°E" or "32°W"). The second format allows negative numbers an omits the single
	///     character (i.e. 94 or -32).</para>
	/// 	<para>Instances of this class are guaranteed to be thread-safe because the class is
	///     immutable (its properties can only be changed via constructors).</para>
	/// </remarks>
	/// <seealso cref="Azimuth">Azimuth Class</seealso>
	/// <seealso cref="Elevation">Elevation Class</seealso>
	/// <seealso cref="Latitude">Latitude Class</seealso>
	/// <seealso cref="Longitude">Longitude Class</seealso>
	/// <example>
	///     These examples create new instances of Longitude objects. 
	///     <code lang="VB" description="Create an angle of 90°">
	/// Dim MyLongitude As New Longitude(90)
	///     </code>
	/// 	<code lang="CS" description="Create an angle of 90°">
	/// Longitude MyLongitude = new Longitude(90);
	///     </code>
	/// 	<code lang="C++" description="Create an angle of 90°">
	/// Longitude MyLongitude = new Longitude(90);
	///     </code>
	/// 	<code lang="VB" description="Create an angle of 105°30'21.4">
	/// Dim MyLongitude1 As New Longitude(105, 30, 21.4)
	///     </code>
	/// 	<code lang="CS" description="Create an angle of 105°30'21.4">
	/// Longitude MyLongitude = new Longitude(105, 30, 21.4);
	///     </code>
	/// 	<code lang="C++" description="Create an angle of 105°30'21.4">
	/// Longitude MyLongitude = new Longitude(105, 30, 21.4);
	///     </code>
	/// </example>
#if !PocketPC || DesignTime
    [TypeConverter("DotSpatial.Positioning.Design.LongitudeConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=d77afaeb30e3236a")]
#endif
    public struct Longitude : IFormattable, IComparable<Longitude>, IEquatable<Longitude>, ICloneable<Longitude>, IXmlSerializable
    {
        private double _DecimalDegrees;

        #region Constants

        private const int MaximumPrecisionDigits = 12;

        #endregion

        #region Fields

 		/// <summary>Represents a longitude of 0°.</summary>
		/// <remarks>
		/// 	<para>The Prime Meridian, located at 0°E (and 0°W), also known as the "Greenwich" Meridian was chosen as the
		///  Prime Meridian of the World in 1884. Forty-one delegates from 25 nations met in
		///  Washington, D.C. for the International Meridian Conference. By the end of the
		///  conference, Greenwich had won the prize of Longitude 0° by a vote of 22 to 1
		///  against (San Domingo), with 2 abstentions (France and Brazil).</para>
		/// 	<para>The Prime Meridian is also significant in that it marks the location from
		///  which all time zones are measured. Times displayed as "Zulu," "UTC," or "GMT" are
		///  all talking about times adjusted to the Greenwich time zone.</para>
		/// 	<para>Before the Prime Meridian, almost every town in the world kept its own local
		///  time. There were no national or international conventions which set how time should
		///  be measured, or when the day would begin and end, or even what length an hour might
		///  be!</para>
		/// </remarks>
		public static readonly Longitude PrimeMeridian = new Longitude(0.0);
		/// <summary>Represents a longitude 180°.</summary>
		/// <remarks>
		/// This value of 180°W (also 180°E) marks the longitude located on the opposite side of the Earth from the
		/// Prime Meridian. It runs approximately through the
		/// <a href="http://greenwichmeridian.com/date-line.htm">International Date Line</a>
		/// (between Alaska and Russia).
		/// </remarks>
		public static readonly Longitude InternationalDateline = new Longitude(180.0);
		/// <summary>Represents a longitude of 0°.</summary>
		public static readonly Longitude Empty = new Longitude(0.0);
		/// <summary>Represents the minimum possible longitude of -180°.</summary>
		/// <remarks>
		/// This member is provided for completeness and is equivalent to the
		/// <see cref="PrimeMeridian">PrimeMeridian</see> shared field.
		/// </remarks>
		public static readonly Longitude Minimum = new Longitude(-180.0);
		/// <summary>Represents the maximum possible longitude of 180°.</summary>
		/// <remarks>
		/// This value of 180°W (also 180°E) marks the longitude located on the opposite side of the Earth from the
		/// Prime Meridian. It runs approximately through the
		/// <a href="http://greenwichmeridian.com/date-line.htm">International Date Line</a>
		/// (between Alaska and Russia).
		/// </remarks>
		public static readonly Longitude Maximum = new Longitude(180.0);
        /// <summary>
        /// Represents an invalid or unspecified value.
        /// </summary>
        public static readonly Longitude Invalid = new Longitude(double.NaN);

        #endregion

        #region  Constructors

        /// <summary>Creates a new instance with the specified decimal degrees.</summary>
        /// <example>
        ///     This example demonstrates how to create an angle with a measurement of 90°. 
        ///     <code lang="VB">
        /// Dim MyLongitude As New Longitude(90)
        ///     </code>
        /// 	<code lang="CS">
        /// Longitude MyLongitude = new Longitude(90);
        ///     </code>
        /// </example>
        /// <returns>An <strong>Longitude</strong> containing the specified value.</returns>
        public Longitude(double decimalDegrees)
        {
            // Set the decimal degrees value
            _DecimalDegrees = decimalDegrees;
        }

        /// <summary>
        /// Creates a new instance with the specified decimal degrees and hemisphere.
        /// </summary>
        /// <param name="decimalDegrees">A <strong>Double</strong> specifying the number of hours.</param>
        /// <param name="hemisphere">A value from the <strong>LongitudeHemisphere</strong> enumeration.</param>
        /// <example>
        /// This example creates a new Longitude of 39°30' north.
        /// <code lang="VB">
        /// Dim MyLongitude As New Longitude(39.5, LongitudeHemisphere.North)
        /// </code>
        /// <code lang="C#">
        /// Longitude MyLongitude = new Longitude(39.5, LongitudeHemisphere.North);
        /// </code>
        /// This example creates a new Longitude of 39°30 south.
        /// <code lang="VB">
        /// Dim MyLongitude As New Longitude(39.5, LongitudeHemisphere.South)
        /// </code>
        /// <code lang="C#">
        /// Longitude MyLongitude = new Longitude(39.5, LongitudeHemisphere.South);
        /// </code>
        /// </example>
        public Longitude(double decimalDegrees, LongitudeHemisphere hemisphere)
        {
            _DecimalDegrees = ToDecimalDegrees(decimalDegrees, hemisphere);
        }

        /// <summary>Creates a new instance with the specified degrees.</summary>
        /// <returns>An <strong>Longitude</strong> containing the specified value.</returns>
        /// <param name="hours">
        /// An <strong>Integer</strong> indicating the amount of degrees, typically between 0
        /// and 360.
        /// </param>
        public Longitude(int hours)
        {
            _DecimalDegrees = ToDecimalDegrees(hours);
        }

        /// <summary>Creates a new instance with the specified hours, minutes and 
        /// seconds.</summary>
        /// <example>
        ///     This example demonstrates how to create an angular measurement of 34°12'29.2 in
        ///     hours, minutes and seconds. 
        ///     <code lang="VB">
        /// Dim MyLongitude As New Longitude(34, 12, 29.2)
        ///     </code>
        /// 	<code lang="CS">
        /// Longitude MyLongitude = new Longitude(34, 12, 29.2);
        ///     </code>
        /// </example>
        /// <returns>An <strong>Longitude</strong> containing the specified value.</returns>
        public Longitude(int hours, int minutes, double seconds)
        {
            _DecimalDegrees = ToDecimalDegrees(hours, minutes, seconds);
        }

        /// <summary>Creates a new instance using the specified decimal degrees and 
        /// hemisphere.</summary>
        /// <param name="hours">An <strong>Integer</strong> specifying the number of hours.</param>
        /// <param name="minutes">An <strong>Integer</strong> specifying the number of minutes.</param>
        /// <param name="seconds">An <strong>double</strong> specifying the whole and fractional seconds.</param>
        /// <remarks>
        ///	 <para>This constructor is typically used to create a longitude when decimal degrees
        ///	 are always expressed as a positive number. Since the hemisphere property is set
        ///	 <em>after</em> the DecimalDegrees property is set, the DecimalDegrees is adjusted
        ///	 automatically to be positive for the eastern hemisphere and negative for the
        ///	 western hemisphere.</para>
        /// 
        ///	 <para>If the parameters conflict with each other, the <strong>Hemisphere</strong>
        ///	 parameter takes precedence. Therefore, a value of "-19°E" will become "19°E"
        ///	 (without the negative sign) with no exception being thrown.</para>
        /// </remarks>
        /// <param name="hemisphere">A value from the <strong>LatitudeHemisphere</strong> 
        /// enumeration.</param>
        public Longitude(int hours, int minutes, double seconds, LongitudeHemisphere hemisphere)
        {
            _DecimalDegrees = ToDecimalDegrees(hours, minutes, seconds, hemisphere);
        }

        /// <summary>Creates a new instance with the specified hours and decimal minutes.</summary>
        /// <param name="hours">An <strong>Integer</strong> specifying the number of hours.</param>
        /// <param name="decimalMinutes">An <strong>Integer</strong> specifying the number of minutes.</param>
        /// <example>
        ///     This example demonstrates how an angle can be created when only the hours and
        ///     minutes (in decimal form) are known. This creates a value of 12°42.345'. 
        ///     <code lang="VB">
        /// Dim MyLongitude As New Longitude(12, 42.345)
        ///     </code>
        /// 	<code lang="VB">
        /// Longitude MyLongitude = new Longitude(12, 42.345);
        ///     </code>
        /// </example>
        /// <remarks>An <strong>Longitude</strong> containing the specified value.</remarks>
        public Longitude(int hours, double decimalMinutes)
        {
            _DecimalDegrees = ToDecimalDegrees(hours, decimalMinutes);
        }

        /// <summary>
        /// Creates a new instance with the specified hours, decimal minutes, and hemisphere.
        /// </summary>
        /// <param name="hours">An <strong>Integer</strong> specifying the number of hours.</param>
        /// <param name="decimalMinutes">An <strong>Integer</strong> specifying the number of minutes.</param>
        /// <param name="hemisphere">A value from the <strong>LongitudeHemisphere</strong> enumeration.</param>
        /// <example>
        /// This example creates a new Longitude of 39°12.34' north.
        /// <code lang="VB">
        /// Dim MyLongitude As New Longitude(39, 12.34, LongitudeHemisphere.North)
        /// </code>
        /// <code lang="C#">
        /// Longitude MyLongitude = new Longitude(39, 12.34, LongitudeHemisphere.North);
        /// </code>
        /// This example creates a new Longitude of 39°12.34 south.
        /// <code lang="VB">
        /// Dim MyLongitude As New Longitude(39, 12.34, LongitudeHemisphere.South)
        /// </code>
        /// <code lang="C#">
        /// Longitude MyLongitude = new Longitude(39, 12.34, LongitudeHemisphere.South);
        /// </code>
        /// </example>
        public Longitude(int hours, double decimalMinutes, LongitudeHemisphere hemisphere)
        {
            _DecimalDegrees = ToDecimalDegrees(hours, decimalMinutes, hemisphere);
        }

        /// <summary>Creates a new instance using the specified string-based measurement.</summary>
        /// <remarks>
        ///	 <para>A <strong>String</strong> in any of the following formats (or variation
        ///	 depending on the local culture):</para>
        /// 
        ///	 <para>
        ///		 <table cellspacing="0" cols="4" cellpadding="2" width="100%">
        ///			 <tbody>
        ///				 <tr>
        ///					 <td>hh</td>
        ///					 <td>hh.h</td>
        ///					 <td>hh mm</td>
        ///					 <td>hh mm.mm</td>
        ///				 </tr>
        ///				 <tr>
        ///					 <td>hh mm ss</td>
        ///					 <td>hh mm ss.sss</td>
        ///					 <td>hhi</td>
        ///					 <td>hh.hi</td>
        ///				 </tr>
        ///				 <tr>
        ///					 <td>hh mmi</td>
        ///					 <td>hh mm i</td>
        ///					 <td>hh mm.mi</td>
        ///					 <td>hh mm.m i</td>
        ///				 </tr>
        ///				 <tr>
        ///					 <td>hh mm ssi</td>
        ///					 <td>hh mm ss i</td>
        ///					 <td>hh mm ss.si</td>
        ///					 <td>hh mm ss.s i</td>
        ///				 </tr>
        ///				 <tr>
        ///					 <td>hhhmmssi</td>
        ///					 <td></td>
        ///					 <td></td>
        ///					 <td></td>
        ///				 </tr>
        ///			 </tbody>
        ///		 </table>
        ///	 </para>
        /// 
        ///	 <para>Where <strong>h</strong> represents hours, <strong>m</strong> represents
        ///	 minutes, <strong>s</strong> represents seconds, and <strong>i</strong> represents a
        ///	 one-letter hemisphere indicator of "E" or "W." Any non-numeric character between
        ///	 numbers is considered a delimiter. Thus, a value of <strong>12°34'56.78"</strong>
        ///	 or even <strong>12A34B56.78C</strong> is treated the same as <strong>12 34
        ///	 56.78</strong>.</para>
        /// </remarks>
        /// <seealso cref="Longitude.Parse(string)">Parse Method</seealso>
        /// <example>
        ///     This example creates a new instance by parsing a string. (NOTE: The double-quote is
        ///     doubled up to represent a single double-quote in the string.) 
        ///     <code lang="VB">
        /// Dim MyLongitude As New Longitude("123°45'67.8""")
        ///     </code>
        /// 	<code lang="CS">
        /// Longitude MyLongitude = new Longitude("123°45'67.8\"");
        ///     </code>
        /// </example>
        /// <returns>An <strong>Longitude</strong> containing the specified value.</returns>
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">The Parse method requires a decimal or sexagesimal measurement.</exception>
        /// <exception cref="FormatException" caption="FormatException">Only the right-most portion of a sexagesimal measurement can be a fractional value.</exception>
        /// <exception cref="FormatException" caption="FormatException">Extra characters were encountered while parsing an angular measurement.  Only hours, minutes, and seconds are allowed.</exception>
        /// <exception cref="FormatException" caption="FormatException">The specified text was not fully understood as an angular measurement.</exception>
        public Longitude(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <summary>Creates a new instance using the specified string-based measurement.</summary>
        /// <remarks>
        ///	 <para>A <strong>String</strong> in any of the following formats (or variation
        ///	 depending on the local culture):</para>
        /// 
        ///	 <para>
        ///		 <table cellspacing="0" cols="4" cellpadding="2" width="100%">
        ///			 <tbody>
        ///				 <tr>
        ///					 <td>hh</td>
        ///					 <td>hh.h</td>
        ///					 <td>hh mm</td>
        ///					 <td>hh mm.mm</td>
        ///				 </tr>
        ///				 <tr>
        ///					 <td>hh mm ss</td>
        ///					 <td>hh mm ss.sss</td>
        ///					 <td>hhi</td>
        ///					 <td>hh.hi</td>
        ///				 </tr>
        ///				 <tr>
        ///					 <td>hh mmi</td>
        ///					 <td>hh mm i</td>
        ///					 <td>hh mm.mi</td>
        ///					 <td>hh mm.m i</td>
        ///				 </tr>
        ///				 <tr>
        ///					 <td>hh mm ssi</td>
        ///					 <td>hh mm ss i</td>
        ///					 <td>hh mm ss.si</td>
        ///					 <td>hh mm ss.s i</td>
        ///				 </tr>
        ///				 <tr>
        ///					 <td>hhhmmssi</td>
        ///					 <td></td>
        ///					 <td></td>
        ///					 <td></td>
        ///				 </tr>
        ///			 </tbody>
        ///		 </table>
        ///	 </para>
        /// 
        ///	 <para>Where <strong>h</strong> represents hours, <strong>m</strong> represents
        ///	 minutes, <strong>s</strong> represents seconds, and <strong>i</strong> represents a
        ///	 one-letter hemisphere indicator of "E" or "W." Any non-numeric character between
        ///	 numbers is considered a delimiter. Thus, a value of <strong>12°34'56.78"</strong>
        ///	 or even <strong>12A34B56.78C</strong> is treated the same as <strong>12 34
        ///	 56.78</strong>.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">The Parse method requires a decimal or sexagesimal measurement.</exception>
        /// <exception cref="FormatException" caption="FormatException">Only the right-most portion of a sexagesimal measurement can be a fractional value.</exception>
        /// <exception cref="FormatException" caption="FormatException">Extra characters were encountered while parsing an angular measurement.  Only hours, minutes, and seconds are allowed.</exception>
        /// <exception cref="FormatException" caption="FormatException">The specified text was not fully understood as an angular measurement.</exception>
        /// <summary>
        /// Creates a new instance by converting the specified string using the specified
        /// culture.
        /// </summary>
        /// <param name="value">
        /// A <strong>String</strong> describing an angle in the form of decimal degrees or a
        /// sexagesimal.
        /// </param>
        /// <param name="culture">
        /// A <strong>CultureInfo</strong> object describing the numeric format to use during
        /// conversion.
        /// </param>
        public Longitude(string value, CultureInfo culture)
        {
            // Is the value null or empty?
            if (value == null || value.Length == 0)
            {
                // Yes. Set to zero
                _DecimalDegrees = 0;
                return;
            }

            // Default to the current culture
            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            // We'll be replacing and trimming a lot, so use a Stringbuilder
            StringBuilder NewValue = new StringBuilder(value);

            // Try to extract the hemisphere
            LongitudeHemisphere hemisphere = LongitudeHemisphere.None;
            if (value.IndexOf('E') != -1
                // Ignore the "E-002" type scientific notation
                && value.IndexOf('E') != value.IndexOf("E-"))
            {
                hemisphere = LongitudeHemisphere.East;
                NewValue.Replace("E", "");
            }
            else if (value.IndexOf('W') != -1)
            {
                hemisphere = LongitudeHemisphere.West;
                NewValue.Replace("W", "");
            }
            else if (value.IndexOf('e') != -1
                // Ignore the "E-002" type scientific notation
                && value.IndexOf('e') != value.IndexOf("e-"))
            {
                hemisphere = LongitudeHemisphere.East;
                NewValue.Replace("e", "");
            }
            else if (value.IndexOf('w') != -1)
            {
                hemisphere = LongitudeHemisphere.West;
                NewValue.Replace("w", "");
            }
            else if (value.StartsWith("-"))
            {
                hemisphere = LongitudeHemisphere.West;
            }

            // Yes. First, clean up the strings
            try
            {
                // Clean up the string
                NewValue.Replace("°", " ").Replace("'", " ").Replace("\"", " ").Replace("  ", " ");
                // Now split the values into an array
                string[] Values = NewValue.ToString().Trim().Split(' ');
                // How many elements are in the array?
                switch (Values.Length)
                {
                    case 0:
                        // Return a blank Longitude
                        _DecimalDegrees = 0.0;
                        return;
                    case 1: // Decimal degrees
                        // Is it nothing?
                        if (Values[0].Length == 0)
                        {
                            _DecimalDegrees = 0.0;
                            return;
                        }
                        // Is it infinity?
                        else if (String.Compare(Values[0], Properties.Resources.Common_Infinity, true, culture) == 0)
                        {
                            _DecimalDegrees = double.PositiveInfinity;
                            return;
                        }
                        // Is it empty?
                        else if (String.Compare(Values[0], Properties.Resources.Common_Empty, true, culture) == 0)
                        {
                            _DecimalDegrees = 0.0;
                            return;
                        }
                        // Look at the number of digits, this might be HHHMMSS format.
                        else if (Values[0].Length == 7 && Values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) == -1)
                        {
                            _DecimalDegrees = ToDecimalDegrees(
                                int.Parse(Values[0].Substring(0, 3), culture),
                                int.Parse(Values[0].Substring(3, 2), culture),
                                double.Parse(Values[0].Substring(5, 2), culture),
                                hemisphere);
                            break;
                        }
                        else if (Values[0].Length == 8 && Values[0][0] == '-' && Values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) == -1)
                        {
                            _DecimalDegrees = ToDecimalDegrees(
                                int.Parse(Values[0].Substring(0, 4), culture),
                                int.Parse(Values[0].Substring(4, 2), culture),
                                double.Parse(Values[0].Substring(6, 2), culture),
                                hemisphere);
                            break;
                        }
                        else
                        {
                            _DecimalDegrees = ToDecimalDegrees(
                                double.Parse(Values[0], culture),
                                hemisphere);
                            break;
                        }
                    case 2: // Hours and decimal minutes
                        // If this is a fractional value, remember that it is
                        if (Values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) != -1)
                        {
                            throw new ArgumentException(Properties.Resources.Longitude_OnlyRightmostIsDecimal, "value");
                        }
                        // Set decimal degrees
                        _DecimalDegrees = ToDecimalDegrees(
                            int.Parse(Values[0], culture),
                            float.Parse(Values[1], culture),
                            hemisphere);

                        break;
                    default: // Hours, minutes and seconds  (most likely)
                        // If this is a fractional value, remember that it is
                        if (Values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) != -1 || Values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) != -1)
                        {
                            throw new ArgumentException(Properties.Resources.Longitude_OnlyRightmostIsDecimal, "value");
                        }

                        // Set decimal degrees
                        _DecimalDegrees = ToDecimalDegrees(
                            int.Parse(Values[0], culture),
                            int.Parse(Values[1], culture),
                            double.Parse(Values[2], culture),
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
        /// <param name="reader"></param>
        public Longitude(XmlReader reader)
        {
            // Initialize all fields
            _DecimalDegrees = Double.NaN;

            // Deserialize the object from XML
            ReadXml(reader);
        }

        #endregion

        #region Public Properties

        /// <summary>Returns the value of the angle as decimal degrees.</summary>
        /// <value>A <strong>Double</strong> value.</value>
        /// <remarks>This property returns the value of the angle as a single number.</remarks>
        /// <seealso cref="Hours">Hours Property</seealso>
        /// <seealso cref="Minutes">Minutes Property</seealso>
        /// <seealso cref="Seconds">Seconds Property</seealso>
        /// <example>
        ///     This example demonstrates how the
        ///     <see cref="DecimalDegrees"><strong>DecimalDegrees</strong></see> property is
        ///     calculated automatically when creating an angle using hours, minutes and seconds. 
        ///     <code lang="VB">
        /// ' Create an angle of 20°30'
        /// Dim MyLongitude As New Longitude(20, 30)
        /// ' Setting the DecimalMinutes recalculated other properties 
        /// Debug.WriteLine(MyLongitude.DecimalDegrees)
        /// ' Output: "20.5"  the same as 20°30'
        ///     </code>
        /// 	<code lang="CS">
        /// // Create an angle of 20°30'
        /// Longitude MyLongitude = New Longitude(20, 30);
        /// // Setting the DecimalMinutes recalculated other properties 
        /// Console.WriteLine(MyLongitude.DecimalDegrees)
        /// // Output: "20.5"  the same as 20°30'
        ///     </code>
        /// </example>
        public double DecimalDegrees
        {
            get
            {
                return _DecimalDegrees;
            }
        }

        /// <summary>Returns the minutes and seconds as a single numeric value.</summary>
        /// <seealso cref="Minutes">Minutes Property</seealso>
        /// <seealso cref="DecimalDegrees">DecimalDegrees Property</seealso>
        /// <value>A <strong>Double</strong> value.</value>
        /// <remarks>
        /// This property is used when minutes and seconds are represented as a single
        /// decimal value.
        /// </remarks>
        /// <example>
        ///     This example demonstrates how the <strong>DecimalMinutes</strong> property is
        ///     automatically calculated when creating a new angle. 
        ///     <code lang="VB">
        /// ' Create an angle of 20°10'30"
        /// Dim MyLongitude As New Longitude(20, 10, 30)
        /// ' The DecimalMinutes property is automatically calculated
        /// Debug.WriteLine(MyLongitude.DecimalMinutes)
        /// ' Output: "10.5"
        ///     </code>
        /// 	<code lang="CS">
        /// // Create an angle of 20°10'30"
        /// Longitude MyLongitude = new Longitude(20, 10, 30);
        /// // The DecimalMinutes property is automatically calculated
        /// Console.WriteLine(MyLongitude.DecimalMinutes)
        /// // Output: "10.5"
        ///     </code>
        /// </example>
        public double DecimalMinutes
        {
            get
            {
#if Framework20 && !PocketPC
                return Math.Round(
                    (Math.Abs(
                        _DecimalDegrees - Math.Truncate(_DecimalDegrees)) * 60.0),
                    // Apparently we must round to two less places to preserve accuracy
                        MaximumPrecisionDigits - 2);
#else
                return Math.Round(
                    (Math.Abs(
                        _DecimalDegrees - Truncate(_DecimalDegrees)) * 60.0), MaximumPrecisionDigits - 2);
#endif
            }
        }


        /// <summary>Returns the integer hours (degrees) portion of an angular 
        /// measurement.</summary>
        /// <seealso cref="Minutes">Minutes Property</seealso>
        /// <seealso cref="Seconds">Seconds Property</seealso>
        /// <value>An <strong>Integer</strong> value.</value>
        /// <remarks>
        /// This property is used in conjunction with the <see cref="Minutes">Minutes</see>
        /// and <see cref="Seconds">Seconds</see> properties to create a full angular measurement.
        /// This property is the same as <strong>DecimalDegrees</strong> without any fractional
        /// value.
        /// </remarks>
        /// <example>
        ///     This example creates an angle of 60.5° then outputs the value of the
        ///     <strong>Hours</strong> property, 60. 
        ///     <code lang="VB">
        /// Dim MyLongitude As New Longitude(60.5)
        /// Debug.WriteLine(MyLongitude.Hours)
        /// ' Output: 60
        ///     </code>
        /// 	<code lang="CS">
        /// Longitude MyLongitude = new Longitude(60.5);
        /// Console.WriteLine(MyLongitude.Hours);
        /// // Output: 60
        ///     </code>
        /// </example>
        public int Hours
        {
            get
            {
#if Framework20 && !PocketPC
                return (int)Math.Truncate(_DecimalDegrees);
#else
				return Truncate(_DecimalDegrees);
#endif
            }
        }


        /// <summary>Returns the integer minutes portion of an angular measurement.</summary>
        /// <seealso cref="Hours">Hours Property</seealso>
        /// <seealso cref="Seconds">Seconds Property</seealso>
        /// <remarks>
        /// This property is used in conjunction with the <see cref="Hours">Hours</see> and
        /// <see cref="Seconds">Seconds</see> properties to create a sexagesimal
        /// measurement.
        /// </remarks>
        /// <value>An <strong>Integer</strong>.</value>
        /// <example>
        ///     This example creates an angle of 45.5° then outputs the value of the
        ///     <strong>Minutes</strong> property, 30. 
        ///     <code lang="VB">
        /// Dim MyLongitude As New Longitude(45.5)
        /// Debug.WriteLine(MyLongitude.Minutes)
        /// ' Output: 30
        ///     </code>
        /// 	<code lang="CS">
        /// Longitude MyLongitude = new Longitude(45.5);
        /// Console.WriteLine(MyLongitude.Minutes);
        /// // Output: 30
        ///     </code>
        /// </example>
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
                                (_DecimalDegrees - Hours) * 60.0, MaximumPrecisionDigits - 1))));
            }
        }


        /// <summary>Returns the seconds minutes portion of an angular measurement.</summary>
        /// <remarks>
        /// This property is used in conjunction with the <see cref="Hours">Hours</see> and
        /// <see cref="Minutes">Minutes</see> properties to create a sexagesimal
        /// measurement.
        /// </remarks>
        /// <seealso cref="Hours">Hours Property</seealso>
        /// <seealso cref="Minutes">Minutes Property</seealso>
        /// <value>A <strong>Double</strong> value.</value>
        /// <example>
        ///     This example creates an angle of 45°10.5' then outputs the value of the
        ///     <strong>Seconds</strong> property, 30. 
        ///     <code lang="VB">
        /// Dim MyLongitude As New Longitude(45, 10.5)
        /// Debug.WriteLine(MyLongitude.Seconds)
        /// ' Output: 30
        ///     </code>
        /// 	<code lang="CS">
        /// Dim MyLongitude As New Longitude(45, 10.5);
        /// Console.WriteLine(MyLongitude.Seconds);
        /// // Output: 30
        ///     </code>
        /// </example>
        public double Seconds
        {
            get
            {
                return Math.Round(
                                (Math.Abs(_DecimalDegrees - Hours) * 60.0 - Minutes) * 60.0,
                    // This property appears to support one less digit than the maximum allowed
                                MaximumPrecisionDigits - 4);
            }
        }

        /// <summary>Returns whether the longitude is east or west of the Prime Meridian.</summary>
        /// <remarks>
        /// 	<para>When this property changes, the DecimalDegrees property is adjusted: if the
        ///  hemisphere is <strong>West</strong>, a negative sign is placed in front of the
        ///  DecimalDegrees value, and vice versa.</para>
        /// </remarks>
        public LongitudeHemisphere Hemisphere
        {
            get
            {
                // And set the hemisphere
                if (_DecimalDegrees < 0)
                    return LongitudeHemisphere.West;
                else
                    return LongitudeHemisphere.East;
            }
        }

        /// <summary>Returns the Universal Transverse Mercator zone number for this longitude.</summary>
        public int UtmZoneNumber
        {
            get
            {
#if Framework20 && !PocketPC
                double LongitudeTemp = (DecimalDegrees + 180) - (int)Math.Truncate((DecimalDegrees + 180) / 360) * 360 - 180;
#else
			double LongitudeTemp = (DecimalDegrees + 180) - Angle.Truncate((DecimalDegrees + 180) / 360) * 360 - 180;
#endif

                //int ZoneNumber = 0;

                // Adjust for special zone numbers
                if (DecimalDegrees >= 56.0 && DecimalDegrees < 64.0 && LongitudeTemp >= 3.0 && LongitudeTemp < 12.0)
                {
                    return 32;
                }
                // Special zones for Svalbard
                else if (DecimalDegrees >= 72 && DecimalDegrees < 84.0)
                {
                    if (LongitudeTemp >= 0.0 && LongitudeTemp < 9.0)
                    {
                        return 31;
                    }
                    else if (LongitudeTemp >= 9.0 && LongitudeTemp < 21.0)
                    {
                        return 33;
                    }
                    else if (LongitudeTemp >= 21.0 && LongitudeTemp < 33.0)
                    {
                        return 35;
                    }
                    else if (LongitudeTemp >= 33.0 && LongitudeTemp < 42.0)
                    {
                        return 37;
                    }
                }

                //else
                {
#if Framework20 && !PocketPC
                    return (int)Math.Truncate((LongitudeTemp + 180) / 6) + 1;
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


        /// <summary>Indicates if the current instance has a non-zero value.</summary>
        /// <value>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the
        /// <strong>DecimalDegrees</strong> property is zero.
        /// </value>
        /// <seealso cref="Empty">Empty Field</seealso>
        public bool IsEmpty
        {
            get
            {
                return (_DecimalDegrees == 0);
            }
        }

        /// <summary>Indicates if the current instance represents an infinite value.</summary>
        public bool IsInfinity
        {
            get
            {
                return double.IsInfinity(_DecimalDegrees);
            }
        }

        /// <summary>
        /// Indicates whether the value is invalid or unspecified.
        /// </summary>
        public bool IsInvalid
        {
            get { return double.IsNaN(_DecimalDegrees); }
        }

        /// <summary>Indicates whether the value has been normalized and is within the 
        /// allowed bounds of -180° and 180°.</summary>
        public bool IsNormalized
        {
            get { return _DecimalDegrees >= -180 && _DecimalDegrees <= 180; }
        }

        #endregion

        #region Public Methods

        /// <summary>Returns the object with the smallest value.</summary>
        /// <returns>The <strong>Longitude</strong> containing the smallest value.</returns>
        /// <param name="value">An <strong>Longitude</strong> object to compare to the current instance.</param>
        public Longitude LesserOf(Longitude value)
        {
            if (_DecimalDegrees < value.DecimalDegrees)
                return this;
            else
                return value;
        }

        /// <returns>An <strong>Longitude</strong> containing the largest value.</returns>
        /// <summary>Returns the object with the largest value.</summary>
        /// <param name="value">An <strong>Longitude</strong> object to compare to the current instance.</param>
        public Longitude GreaterOf(Longitude value)
        {
            if (_DecimalDegrees > value.DecimalDegrees)
                return this;
            else
                return value;
        }

        /// <summary>Returns a value indicating the relative order of two objects.</summary>
        /// <returns>A value of -1, 0, or 1 as documented by the IComparable interface.</returns>
        /// <remarks>
        ///		This method allows collections of <strong>Longitude</strong> objects to be sorted.
        ///		The <see cref="DecimalDegrees">DecimalDegrees</see> property of each instance is compared.
        /// </remarks>
        /// <param name="value">An <strong>Longitude</strong> object to compare with.</param>
        public int Compare(double value)
        {
            return _DecimalDegrees.CompareTo(value);
        }

        /// <summary>Returns an angle opposite of the current instance.</summary>
        /// <returns>An <strong>Longitude</strong> representing the mirrored value.</returns>
        /// <remarks>
        /// This method returns the "opposite" of the current instance. The opposite is
        /// defined as the point on the other side of an imaginary circle. For example, if an angle
        /// is 0°, at the top of a circle, this method returns 180°, at the bottom of the
        /// circle.
        /// </remarks>
        /// <example>
        ///     This example creates a new <strong>Longitude</strong> of 45° then calculates its mirror
        ///     of 225°. (45 + 180) 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Longitude1 As New Longitude(45)
        /// Dim Longitude2 As Longitude = Longitude1.Mirror()
        /// Debug.WriteLine(Longitude2.ToString())
        /// ' Output: 225
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Longitude Longitude1 = new Longitude(45);
        /// Longitude Longitude2 = Longitude1.Mirror();
        /// Console.WriteLine(Longitude2.ToString());
        /// // Output: 225
        ///     </code>
        /// </example>
        public Longitude Mirror()
        {
            return Normalize().Multiply(-1.0);
        }

        /// <summary>Converts the current instance into radians.</summary>
        /// <returns>A <see cref="Radian">Radian</see> object.</returns>
        /// <remarks>
        /// 	<para>This function is typically used to convert an angular measurement into
        ///  radians before performing a trigonometric function.
        /// 		</para>
        /// </remarks>
        /// <seealso cref="Radian">Radian Class</seealso>
        /// <overloads>Converts an angular measurement into radians before further processing.</overloads>
        /// <example>
        ///     This example converts a measurement of 90° into radians. 
        ///     <code lang="VB">
        /// Dim MyLongitude As New Longitude(90)
        /// Dim MyRadians As Radian = MyLongitude.ToRadians()
        ///     </code>
        /// 	<code lang="CS">
        /// Longitude MyLongitude = new Longitude(90);
        /// Radian MyRadians = MyLongitude.ToRadians();
        ///     </code>
        /// </example>
        public Radian ToRadians()
        {
            return new Radian(_DecimalDegrees * Radian.RadiansPerDegree);
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

        /// <remarks>
        ///	 <para>This powerful method returns the current angular measurement in a specific
        ///	 format. If no value for the format is specified, a format of
        ///	 <strong>hhh°mm'SS.SS"I</strong> (adjusted to the current culture) will be used. The
        ///	 resulting <strong>String</strong> can be converted back into an
        ///	 <strong>Longitude</strong> via the
        ///	 <see href="Angle.Parse">Parse</see> method so long as a delimiter separates each individual
        ///	 value.</para>
        /// </remarks>
        /// <param name="format">
        ///	 <para>A combination of symbols, spaces, and any of the following case-insensitive
        ///	 letters: <strong>D</strong> or <strong>H</strong> for hours, <strong>M</strong> for
        ///	 minutes, <strong>S</strong> for seconds, and <strong>I</strong> to indicate the
        ///	 hemisphere. Here are some examples:</para>
        /// 
        ///	 <para>
        ///		 <table cellspacing="0" cols="3" cellpadding="2" width="100%">
        ///			 <tbody>
        ///				 <tr>
        ///					 <td>HH°MM'SS.SS"</td>
        /// 
        ///					 <td>HHH.H°</td>
        /// 
        ///					 <td>HH MM.MM</td>
        /// 
        ///					 <td>HHHMMSS</td>
        ///				 </tr>
        /// 
        ///				 <tr>
        ///					 <td>HH°MM'SS.SS"I</td>
        /// 
        ///					 <td>HHH.H°I</td>
        /// 
        ///					 <td>HH MM.MMI</td>
        /// 
        ///					 <td>HHHMMSSI</td>
        ///				 </tr>
        ///			 </tbody>
        ///		 </table>
        ///	 </para>
        /// </param>
        /// <summary>Outputs the current instance as a string using the specified format.</summary>
        /// <returns>A <strong>String</strong> matching the specified format.</returns>
        /// <remarks>
        /// 	<para>This method returns the current instance output in a specific format. If no
        ///     value for the format is specified, a default format of "d.dddd°" is used. Any
        ///     string output by this method can be converted back into an Longitude object using the
        ///     <strong>Parse</strong> method or <strong>Longitude(string)</strong> constructor.</para>
        /// </remarks>
        /// <seealso cref="ToString()">ToString Method</seealso>
        /// <seealso cref="Parse(string)">Parse Method</seealso>
        /// <example>
        ///     This example uses the <strong>ToString</strong> method to output an angle in a
        ///     custom format. The " <strong>h°</strong> " code represents hours along with a
        ///     degree symbol (Alt+0176 on the keypad), and " <strong>m.mm</strong> " represents
        ///     the minutes out to two decimals. Mmm. 
        ///     <code lang="VB">
        /// Dim MyLongitude As New Longitude(45, 16.772)
        /// Debug.WriteLine(MyLongitude.ToString("h°m.mm"))
        /// ' Output: 45°16.78
        ///     </code>
        /// 	<code lang="CS">
        /// Dim MyLongitude As New Longitude(45, 16.772);
        /// Debug.WriteLine(MyLongitude.ToString("h°m.mm"));
        /// // Output: 45°16.78
        ///     </code>
        /// </example>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns the smallest integer greater than the specified value.</summary>
        public Longitude Ceiling()
        {
            return new Longitude(Math.Ceiling(_DecimalDegrees));
        }

        /// <summary>Returns the largest integer which is smaller than the specified value.</summary>
        public Longitude Floor()
        {
            return new Longitude(Math.Floor(_DecimalDegrees));
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
            return new Longitude(Math.Round(_DecimalDegrees, decimals));
        }


        /// <summary>Returns a new instance whose Seconds property is evenly divisible by 15.</summary>
        /// <returns>An <strong>Longitude</strong> containing the rounded value.</returns>
        /// <remarks>
        /// This method is used to align or "snap" an angle to a regular interval. For
        /// example, a grid might be easier to read if it were drawn at 30-second intervals instead
        /// of 24.198-second intervals.
        /// </remarks>
        public Longitude RoundSeconds()
        {
            return RoundSeconds(15.0);
        }

        /// <summary>
        /// Returns a new angle whose Seconds property is evenly divisible by the specified amount.
        /// </summary>
        /// <returns>An <strong>Longitude</strong> containing the rounded value.</returns>
        /// <remarks>
        /// This method is used to align or "snap" an angle to a regular interval. For
        /// example, a grid might be easier to read if it were drawn at 30-second intervals instead
        /// of 24.198-second intervals.
        /// </remarks>
        /// <param name="interval">
        /// A <strong>Double</strong> between 0 and 60 indicating the interval to round
        /// to.
        /// </param>
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
            double NewSeconds = Seconds;
            //double HalfInterval = interval * 0.5;
            // Loop through all intervals to find the right rounding
            for (double value = 0; value < 60; value += interval)
            {
                // Calculate the value of the next interval
                double NextInterval = value + interval;
                // Is the seconds value greater than the next interval?
                if (NewSeconds > NextInterval)
                    // Yes.  Continue on
                    continue;
                // Is the seconds value closer to the current or next interval?
                if (NewSeconds < (value + NextInterval) * 0.5)
                    // Closer to the current interval, so adjust it
                    NewSeconds = value;
                else
                    NewSeconds = NextInterval;
                // Is the new value 60?  If so, make it zero
                if (NewSeconds == 60)
                    NewSeconds = 0;
                // Return the new value
                return new Longitude(Hours, Minutes, NewSeconds);
            }
            // return the new value
            return new Longitude(Hours, Minutes, NewSeconds);
        }

        /// <remarks>
        ///	 <para>This function is used to ensure that an angular measurement is within the
        ///	 allowed bounds of 0° and 180°. If a value of 360° or 720° is passed, a value of 0°
        ///	 is returned since traveling around the Earth 360° or 720° brings you to the same
        ///	 place you started.</para>
        /// </remarks>
        /// <returns>A <strong>Longitude</strong> containing the normalized value.</returns>
        public Longitude Normalize()
        {
            // Is the value not a number, infinity, or already normalized?
            if (double.IsInfinity(_DecimalDegrees) || double.IsNaN(_DecimalDegrees))
                return this;
            // If we're off the eastern edge (180E) wrap back around from the west 
            else if (_DecimalDegrees > 180)
                return new Longitude(-180 + (_DecimalDegrees % 180));
            // If we're off the western edge (180W) wrap back around from the east
            else if (_DecimalDegrees < -180)
                return new Longitude(180 + (_DecimalDegrees % 180));
            // We're in bounds already, so just return the current instance
            else
                return this;

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

            if(HemisphereFlips % 2 != 0)
                return new Longitude(-NewValue);
            else
                return new Longitude(NewValue);          
             */
        }

        #endregion

        #region Overrides

        /// <summary>Compares the current value to another Longitude object's value.</summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the object's DecimalDegrees
        /// properties match.
        /// </returns>
        /// <remarks>This </remarks>
        /// <param name="obj">
        /// An <strong>Longitude</strong>, <strong>Double</strong>, or <strong>Integer</strong>
        /// to compare with.
        /// </param>
        public override bool Equals(object obj)
        {
            // Only compare the same type
            if (obj is Longitude)
                return Equals((Longitude)obj);
            return false;
        }

        /// <summary>Returns a unique code for this instance.</summary>
        /// <remarks>
        /// Since the <strong>Longitude</strong> class is immutable, this property may be used
        /// safely with hash tables.
        /// </remarks>
        /// <returns>
        /// An <strong>Integer</strong> representing a unique code for the current
        /// instance.
        /// </returns>
        public override int GetHashCode()
        {
            return _DecimalDegrees.GetHashCode();
        }

        /// <remarks>
        ///	 <para>This powerful method returns the current angular measurement in a specific
        ///	 format. If no value for the format is specified, a format of
        ///	 <strong>hhh°mm'SS.SS"I</strong> (adjusted to the current culture) will be used. The
        ///	 resulting <strong>String</strong> can be converted back into an
        ///	 <strong>Longitude</strong> via the
        ///	 <see href="Angle.Parse">Parse</see> method so long as a delimiter separates each individual
        ///	 value.</para>
        /// </remarks>
        /// <summary>Outputs the current instance as a string using the specified format.</summary>
        /// <returns>A <strong>String</strong> matching the specified format.</returns>
        /// <remarks>
        /// 	<para>This method formats the current instance using the default format of
        ///     "d.dddd°." Any string output by this method can be converted back into an Longitude
        ///     object using the <strong>Parse</strong> method or <strong>Longitude(string)</strong>
        ///     constructor.</para>
        /// </remarks>
        /// <seealso cref="Parse(string)">Parse Method</seealso>
        /// <example>
        ///     This example outputs a value of 90 degrees in the default format of ###.#°. 
        ///     <code lang="VB">
        /// Dim MyLongitude As New Longitude(90)
        /// Debug.WriteLine(MyLongitude.ToString)
        /// ' Output: "90°"
        ///     </code>
        /// 	<code lang="CS">
        /// Longitude MyLongitude = new Longitude(90);
        /// Debug.WriteLine(MyLongitude.ToString());
        /// // Output: "90°"
        ///     </code>
        /// </example>
        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion

        #region Static Methods

        /// <remarks>
        ///	<para>This function is used to ensure that an angular measurement is within the
        ///	allowed bounds of -180° and 180°. If a value of 360° or 720° is passed, a value of 0°
        ///	is returned since traveling around the Earth 360° or 720° brings you to the same
        ///	place you started.</para>
        /// </remarks>
        public static Longitude Normalize(double decimalDegrees)
        {
            return new Longitude(decimalDegrees).Normalize();
        }

        /// <returns>The <strong>Longitude</strong> containing the smallest value.</returns>
        /// <summary>Returns the object with the smallest value.</summary>
        /// <param name="value1">A <strong>Longitude</strong> object to compare to value2.</param>
        /// <param name="value2">A <strong>Longitude</strong> object to compare to value1.</param>
        public static Longitude LesserOf(Longitude value1, Longitude value2)
        {
            return value1.LesserOf(value2);
        }

        /// <summary>Returns the object with the largest value.</summary>
        /// <returns>A <strong>Longitude</strong> containing the largest value.</returns>
        /// <param name="value1">A <strong>Longitude</strong> object to compare to value2.</param>
        /// <param name="value2">A <strong>Longitude</strong> object to compare to value1.</param>
        public static Longitude GreaterOf(Longitude value1, Longitude value2)
        {
            return value1.GreaterOf(value2);
        }


        /// <remarks>
        /// 	<para>This function is typically used to convert an angular measurement into
        ///  radians before performing a trigonometric function.</para>
        /// </remarks>
        /// <returns>A <see cref="Radian"><strong>Radian</strong></see> object.</returns>
        /// <summary>Converts an angular measurement into radians.</summary>
        /// <example>
        ///     This example shows a quick way to convert an angle of 90° into radians. 
        ///     <code lang="VB">
        /// Dim MyRadian As Radian = Longitude.ToRadians(90)
        ///     </code>
        /// 	<code lang="CS">
        /// Radian MyRadian = Longitude.ToRadians(90);
        ///     </code>
        /// </example>
        public static Radian ToRadians(Longitude value)
        {
            return value.ToRadians();
        }

        /// <summary>Converts a value in radians into an angular measurement.</summary>
        /// <remarks>
        /// 	This function is typically used in conjunction with the
        /// 	<see cref="Longitude.ToRadians()">ToRadians</see>
        /// 	method after a trigonometric function has completed. The converted value is stored in
        /// 	the <see cref="DecimalDegrees">DecimalDegrees</see> property.
        /// </remarks>
        /// <seealso cref="Longitude.ToRadians()">ToRadians</seealso>
        /// <seealso cref="Radian">Radian Class</seealso>
        /// <example>
        ///     This example uses the <strong>FromRadians</strong> method to convert a value of one
        ///     radian into an <strong>Longitude</strong> of 57°. 
        ///     <code lang="VB">
        /// ' Create a new angle equal to one radian
        /// Dim MyRadians As New Radian(1)
        /// Dim MyLongitude As Longitude = Longitude.FromRadians(MyRadians)
        /// Debug.WriteLine(MyLongitude.ToString())
        /// ' Output: 57°
        ///     </code>
        /// 	<code lang="CS">
        /// // Create a new angle equal to one radian
        /// Radian MyRadians = new Radian(1);
        /// Longitude MyLongitude = Longitude.FromRadians(MyRadians);
        /// Console.WriteLine(MyLongitude.ToString());
        /// // Output: 57°
        ///     </code>
        /// </example>
        public static Longitude FromRadians(Radian radians)
        {
            return new Longitude(radians.ToDegrees());
        }

        public static Longitude FromRadians(double radians)
        {
            return new Longitude(Radian.ToDegrees(radians));
        }

        /// <summary>Returns a random longitude.</summary>
        public static Longitude Random()
        {
            return Random(new Random());
        }

        /// <summary>Returns a random longitude based on the specified seed.</summary>
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


        /// <summary>Converts arbitrary hour, minute and seconds into decimal degrees.</summary>
        /// <returns>
        /// A <strong>Double</strong> containing the decimal degree version of the specified
        /// values.
        /// </returns>
        /// <remarks>
        /// This function is used to convert three-part measurements into a single value. The
        /// result of this method is typically assigned to the
        /// <see cref="Latitude.DecimalDegrees">
        /// DecimalDegrees</see> property. Values are rounded to thirteen decimal 
        /// places, the maximum precision allowed by this type.
        /// </remarks>
        /// <seealso cref="Latitude.DecimalDegrees">DecimalDegrees Property</seealso>
        /// <seealso cref="Latitude.Normalize()">Normalize Method</seealso>
        /// <example>
        ///     This example converts a value of 10°30'0" into decimal degrees (10.5). 
        ///     <code lang="VB" title="ToDecimalDegrees Example (VB)">
        /// Dim MyValue As Double = Latitude.ToDecimalDegrees(10, 30, 0)
        ///     </code>
        /// 	<code lang="CS" title="ToDecimalDegrees Example (C#)">
        /// double MyValue = Latitude.ToDecimalDegrees(10, 30, 0);
        ///     </code>
        /// </example>
        public static double ToDecimalDegrees(int hours, int minutes, double seconds)
        {
            //return hours < 0
            //    ? -Math.Round(-hours + minutes / 60.0 + seconds / 3600.0, MaximumPrecisionDigits)
            //    : Math.Round(hours + minutes / 60.0 + seconds / 3600.0, MaximumPrecisionDigits);
            return hours < 0
                ? -(-hours + minutes / 60.0 + seconds / 3600.0)
                : (hours + minutes / 60.0 + seconds / 3600.0);
        }

        /// <summary>Converts arbitrary hour and decimal minutes into decimal degrees.</summary>
        /// <returns>
        /// A <strong>Double</strong> containing the decimal degree version of the specified
        /// values.
        /// </returns>
        /// <remarks>
        /// This function is used to convert three-part measurements into a single value. The
        /// result of this method is typically assigned to the
        /// <see cref="Latitude.DecimalDegrees">
        /// DecimalDegrees</see> property. Values are rounded to thirteen decimal 
        /// places, the maximum precision allowed by this type.
        /// </remarks>
        public static double ToDecimalDegrees(int hours, double decimalMinutes)
        {
            //return hours < 0
            //    ? -Math.Round(-hours + decimalMinutes / 60.0, MaximumPrecisionDigits)
            //    : Math.Round(hours + decimalMinutes / 60.0, MaximumPrecisionDigits);
            return hours < 0
                ? -(-hours + decimalMinutes / 60.0)
                : (hours + decimalMinutes / 60.0);
        }

        /// <summary>Converts an hour value into decimal degrees.</summary>
        /// <returns>
        /// A <strong>Double</strong> containing the decimal degree version of the specified
        /// values.
        /// </returns>
        /// <remarks>
        /// The specified value will be converted to a double value.
        /// </remarks>
        public static double ToDecimalDegrees(int hours)
        {
            return Convert.ToDouble(hours);
        }

        /// <summary>Converts arbitrary hour and decimal minutes into decimal degrees.</summary>
        /// <returns>
        /// A <strong>Double</strong> containing the decimal degree version of the specified
        /// values.
        /// </returns>
        /// <remarks>
        /// The specified value will be converted to decimal degrees, then rounded to thirteen digits, the maximum precision allowed by this type.
        /// </remarks>
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

        /// <summary>Converts arbitrary decrees into well-formed decimal degrees.</summary>
        /// <returns>
        /// A <strong>Double</strong> containing the decimal degree version of the specified
        /// values.
        /// </returns>
        /// <remarks>
        /// The specified value will be rounded to thirteen digits, the maximum precision allowed by this type.
        /// </remarks>
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

        /// <summary>Converts arbitrary hour, minute and seconds into decimal degrees.</summary>
        /// <returns>
        /// A <strong>Double</strong> containing the decimal degree version of the specified
        /// values.
        /// </returns>
        /// <remarks>
        /// This function is used to convert three-part measurements into a single value. The
        /// result of this method is typically assigned to the
        /// <see cref="Latitude.DecimalDegrees">
        /// DecimalDegrees</see> property. Values are rounded to thirteen decimal 
        /// places, the maximum precision allowed by this type.
        /// </remarks>
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

        /// <summary>Converts the specified string into an Longitude object.</summary>
        /// <returns>
        /// 	A new <strong>Longitude</strong> object populated with the specified 
        /// 	values.
        /// </returns>
        /// <remarks>
        /// 	<para>This method parses the specified string into an <strong>Longitude</strong> object
        ///     using the current culture. This constructor can parse any strings created via the
        ///     <strong>ToString</strong> method.</para>
        /// </remarks>
        /// <seealso cref="ToString()">ToString Method</seealso>
        /// <example>
        ///     This example creates a new angular measurement using the <strong>Parse</strong>
        ///     method. 
        ///     <code lang="VB">
        /// Dim NewLongitude As Longitude = Longitude.Parse("123.45°")
        ///     </code>
        /// 	<code lang="CS">
        /// Longitude NewLongitude = Longitude.Parse("123.45°");
        ///     </code>
        /// </example>
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">The Parse method requires a decimal or sexagesimal measurement.</exception>
        /// <exception cref="FormatException" caption="FormatException">Only the right-most portion of a sexagesimal measurement can be a fractional value.</exception>
        /// <exception cref="FormatException" caption="FormatException">Extra characters were encountered while parsing an angular measurement.  Only hours, minutes, and seconds are allowed.</exception>
        /// <exception cref="FormatException" caption="FormatException">The specified text was not fully understood as an angular measurement.</exception>
        public static Longitude Parse(string value)
        {
            return new Longitude(value, CultureInfo.CurrentCulture);
        }

        /// <remarks>
        /// 	<para>This powerful method is typically used to process data from a data store or a
        ///     value input by the user in any culture. This function can accept any format which
        ///     can be output by the ToString method.</para>
        /// </remarks>
        /// <returns>A new <strong>Longitude</strong> object equivalent to the specified string.</returns>
        /// <summary>
        /// Converts the specified string into an <strong>Longitude</strong> object using the
        /// specified culture.
        /// </summary>
        /// <param name="value">
        /// A <strong>String</strong> describing an angle in the form of decimal degrees or a
        /// sexagesimal.
        /// </param>
        /// <param name="culture">
        /// A <strong>CultureInfo</strong> object describing the numeric format to use during
        /// conversion.
        /// </param>
        public static Longitude Parse(string value, CultureInfo culture)
        {
            return new Longitude(value, culture);
        }

        #endregion

        #region Operators

        public static Longitude operator +(Longitude left, Longitude right)
        {
            return new Longitude(left.DecimalDegrees + right.DecimalDegrees);
        }

        public static Longitude operator +(Longitude left, double right)
        {
            return new Longitude(left.DecimalDegrees + right);
        }

        public static Longitude operator -(Longitude left, Longitude right)
        {
            return new Longitude(left.DecimalDegrees - right.DecimalDegrees);
        }

        public static Longitude operator -(Longitude left, double right)
        {
            return new Longitude(left.DecimalDegrees - right);
        }

        public static Longitude operator *(Longitude left, Longitude right)
        {
            return new Longitude(left.DecimalDegrees * right.DecimalDegrees);
        }

        public static Longitude operator *(Longitude left, double right)
        {
            return new Longitude(left.DecimalDegrees * right);
        }

        public static Longitude operator /(Longitude left, Longitude right)
        {
            return new Longitude(left.DecimalDegrees / right.DecimalDegrees);
        }

        public static Longitude operator /(Longitude left, double right)
        {
            return new Longitude(left.DecimalDegrees / right);
        }

        public static bool operator ==(Longitude left, Longitude right)
        {
            return left.DecimalDegrees.Equals(right.DecimalDegrees);
        }

        public static bool operator ==(Longitude left, double right)
        {
            return left.DecimalDegrees.Equals(right);
        }

        public static bool operator !=(Longitude left, Longitude right)
        {
            return !left.DecimalDegrees.Equals(right.DecimalDegrees);
        }

        public static bool operator !=(Longitude left, double right)
        {
            return !left.DecimalDegrees.Equals(right);
        }

        public static bool operator >(Longitude left, Longitude right)
        {
            return left.DecimalDegrees > right.DecimalDegrees;
        }

        public static bool operator >(Longitude left, double right)
        {
            return left.DecimalDegrees > right;
        }

        public static bool operator >=(Longitude left, Longitude right)
        {
            return left.DecimalDegrees >= right.DecimalDegrees;
        }

        public static bool operator >=(Longitude left, double right)
        {
            return left.DecimalDegrees >= right;
        }

        public static bool operator <(Longitude left, Longitude right)
        {
            return left.DecimalDegrees < right.DecimalDegrees;
        }

        public static bool operator <(Longitude left, double right)
        {
            return left.DecimalDegrees < right;
        }

        public static bool operator <=(Longitude left, Longitude right)
        {
            return left.DecimalDegrees <= right.DecimalDegrees;
        }

        public static bool operator <=(Longitude left, double right)
        {
            return left.DecimalDegrees <= right;
        }

        /// <summary>Returns the current instance increased by one.</summary>
        /// <returns>An <strong>Longitude</strong> object.</returns>
        /// <remarks>
        /// 	<para>This method increases the <strong>DecimalDegrees</strong> property by 1.0,
        ///     returned as a new instance.</para>
        /// 	<para><font color="red">Since the <strong>Longitude</strong> class is immutable, this
        ///     method cannot be used to modify an existing instance.</font></para>
        /// </remarks>
        /// <example>
        ///     This example uses the <strong>Increment</strong> method to increase an Longitude's
        ///     value. It also demonstrates the subtle error which can be caused if
        ///     <strong>Increment</strong> is called while ignoring the return value.
        ///     <code lang="VB" title="[New Example]">
        /// ' Correct use of Increment
        /// Dim Longitude1 As New Longitude(89)
        /// Longitude1 = Longitude1.Increment()
        ///  
        /// ' Incorrect use of Increment
        /// Dim Longitude1 = New Longitude(89)
        /// Longitude1.Increment()
        /// ' NOTE: Longitude1 will still be 89°!
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// // Correct use of Increment
        /// Longitude Longitude1 = new Longitude(89);
        /// Longitude1 = Longitude1.Increment();
        ///  
        /// // Incorrect use of Increment
        /// Longitude Longitude1 = new Longitude(89);
        /// Longitude1.Increment();
        /// // NOTE: Longitude1 will still be 89°!
        ///     </code>
        /// </example>
        public Longitude Increment()
        {
            return new Longitude(_DecimalDegrees + 1.0);
        }

        /// <summary>Increases the current instance by the specified value.</summary>
        /// <returns>A new <strong>Longitude</strong> containing the summed values.</returns>
        /// <example>
        ///     This example adds 45° to the current instance of 45°, returning 90°. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Longitude1 As New Longitude(45)
        /// Longitude1 = Longitude1.Add(45)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Longitude Longitude1 = new Longitude(45);
        /// Longitude1 = Longitude1.Add(45);
        ///     </code>
        /// </example>
        /// <param name="value">A <strong>Double</strong> to add to the current instance.</param>
        public Longitude Add(double value)
        {
            return new Longitude(_DecimalDegrees + value);
        }

        public Longitude Add(Longitude value)
        {
            return new Longitude(_DecimalDegrees + value.DecimalDegrees);
        }

        /// <summary>Returns the current instance decreased by one.</summary>
        /// <returns>An <strong>Longitude</strong> object.</returns>
        /// <remarks>
        /// 	<para>This method decreases the <strong>DecimalDegrees</strong> property by 1.0,
        ///     returned as a new instance.</para>
        /// 	<para><font color="red">Since the <strong>Longitude</strong> class is immutable, this
        ///     method cannot be used to modify an existing instance.</font></para>
        /// </remarks>
        /// <example>
        ///     This example uses the <strong>Decrement</strong> method to decrease an Longitude's
        ///     value. It also demonstrates the subtle error which can be caused if
        ///     <strong>Decrement</strong> is called while ignoring the return value.
        ///     <code lang="VB" title="[New Example]">
        /// ' Correct use of Decrement
        /// Dim Longitude1 As New Longitude(91)
        /// Longitude1 = Longitude1.Decrement()
        ///  
        /// ' Incorrect use of Decrement
        /// Dim Longitude1 = New Longitude(91)
        /// Longitude1.Increment()
        /// ' NOTE: Longitude1 will still be 91°!
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// // Correct use of Decrement
        /// Longitude Longitude1 = new Longitude(91);
        /// Longitude1 = Longitude1.Decrement();
        ///  
        /// // Incorrect use of Decrement
        /// Longitude Longitude1 = new Longitude(91);
        /// Longitude1.Decrement();
        /// // NOTE: Longitude1 will still be 91°!
        ///     </code>
        /// </example>
        public Longitude Decrement()
        {
            return new Longitude(_DecimalDegrees - 1.0);
        }

        /// <summary>Decreases the current instance by the specified value.</summary>
        /// <returns>A new <strong>Longitude</strong> containing the new value.</returns>
        /// <example>
        ///     This example subtracts 30° from the current instance of 90°, returning 60°. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Longitude1 As New Longitude(90)
        /// Longitude1 = Longitude1.Subtract(30)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Longitude Longitude1 = new Longitude(90);
        /// Longitude1 = Longitude1.Subtract(30);
        ///     </code>
        /// </example>
        /// <param name="value">A <strong>Double</strong> to subtract from the current instance.</param>
        public Longitude Subtract(double value)
        {
            return new Longitude(_DecimalDegrees - value);
        }

        public Longitude Subtract(Longitude value)
        {
            return new Longitude(_DecimalDegrees - value.DecimalDegrees);
        }

        /// <summary>Multiplies the current instance by the specified value.</summary>
        /// <returns>A new <strong>Longitude</strong> containing the product of the two numbers.</returns>
        /// <example>
        ///     This example multiplies 30° with three, returning 90°. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Longitude1 As New Longitude(30)
        /// Longitude1 = Longitude1.Multiply(3)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Longitude Longitude1 = new Longitude(30);
        /// Longitude1 = Longitude1.Multiply(3);
        ///     </code>
        /// </example>
        /// <param name="value">A <strong>Double</strong> to multiply with the current instance.</param>
        public Longitude Multiply(double value)
        {
            return new Longitude(_DecimalDegrees * value);
        }

        public Longitude Multiply(Longitude value)
        {
            return new Longitude(_DecimalDegrees * value.DecimalDegrees);
        }

        /// <summary>Divides the current instance by the specified value.</summary>
        /// <returns>An <strong>Longitude</strong> containing the new value.</returns>
        /// <example>
        ///     This example divides 90° by three, returning 30°. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Longitude1 As New Longitude(90)
        /// Longitude1 = Longitude1.Divide(3)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Longitude Longitude1 = new Longitude(90);
        /// Longitude1 = Longitude1.Divide(3);
        ///     </code>
        /// </example>
        /// <param name="value">A <strong>Double</strong> representing a denominator to divide by.</param>
        public Longitude Divide(double value)
        {
            return new Longitude(_DecimalDegrees / value);
        }

        public Longitude Divide(Longitude value)
        {
            return new Longitude(_DecimalDegrees / value.DecimalDegrees);
        }

        /// <summary>Indicates if the current instance is smaller than the specified value.</summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// smaller than the specified value.
        /// </returns>
        /// <param name="value">An <strong>Longitude</strong> to compare with the current instance.</param>
        public bool IsLessThan(Longitude value)
        {
            return _DecimalDegrees < value.DecimalDegrees;
        }

        public bool IsLessThan(double value)
        {
            return _DecimalDegrees < value;
        }

        /// <remarks>
        /// This method compares the <strong>DecimalDegrees</strong> property with the
        /// specified value. This method is the same as the "&lt;=" operator.
        /// </remarks>
        /// <summary>
        /// Indicates if the current instance is smaller than or equal to the specified
        /// value.
        /// </summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// smaller than or equal to the specified value.
        /// </returns>
        /// <param name="value">An <strong>Longitude</strong> to compare with the current instance.</param>
        public bool IsLessThanOrEqualTo(Longitude value)
        {
            return _DecimalDegrees <= value.DecimalDegrees;
        }

        public bool IsLessThanOrEqualTo(double value)
        {
            return _DecimalDegrees <= value;
        }

        /// <summary>Indicates if the current instance is larger than the specified value.</summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// greater than the specified value.
        /// </returns>
        /// <param name="value">An <strong>Longitude</strong> to compare with the current instance.</param>
        public bool IsGreaterThan(Longitude value)
        {
            return _DecimalDegrees > value.DecimalDegrees;
        }

        public bool IsGreaterThan(double value)
        {
            return _DecimalDegrees > value;
        }

        /// <summary>
        /// Indicates if the current instance is larger than or equal to the specified
        /// value.
        /// </summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// greater than or equal to the specified value.
        /// </returns>
        /// <param name="value">An <strong>Longitude</strong> to compare with the current instance.</param>
        public bool IsGreaterThanOrEqualTo(Longitude value)
        {
            return _DecimalDegrees >= value.DecimalDegrees;
        }

        public bool IsGreaterThanOrEqualTo(double value)
        {
            return _DecimalDegrees >= value;
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts a measurement in Radians into an Longitude.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator Longitude(Radian value)
        {
            return new Longitude(value.ToDegrees());
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Longitude.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator Longitude(double value)
        {
            return new Longitude(value);
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Longitude.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator Longitude(float value)
        {
            return new Longitude(Convert.ToDouble(value));
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Longitude.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator double(Longitude value)
        {
            return value.DecimalDegrees;
        }

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Longitude.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator float(Longitude value)
        {
            return Convert.ToSingle(value.DecimalDegrees);
        }

        /// <summary>
        /// Converts a measurement in degrees as an Integer into an Longitude.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator Longitude(int value)
        {
            return new Longitude(value);
        }

        public static explicit operator Longitude(Angle value)
        {
            return new Longitude(value.DecimalDegrees);
        }

        public static explicit operator Longitude(Azimuth value)
        {
            return new Longitude(value.DecimalDegrees);
        }

        public static explicit operator Longitude(Elevation value)
        {
            return new Longitude(value.DecimalDegrees);
        }

        public static explicit operator Longitude(Latitude value)
        {
            return new Longitude(value.DecimalDegrees);
        }

        /// <summary>
        /// Converts a measurement in the form of a formatted String into an Longitude.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator Longitude(string value)
        {
            return new Longitude(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts an Longitude into a String.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks>This operator calls the ToString() method using the current culture.</remarks>
        public static explicit operator string(Longitude value)
        {
            return value.ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion

        #region ICloneable<Longitude> Members

        /// <summary>Creates a copy of the current instance.</summary>
        /// <returns>An <strong>Longitude</strong> of the same value as the current instance.</returns>
        public Longitude Clone()
        {
            return new Longitude(_DecimalDegrees);
        }

        #endregion

        #region IEquatable<Longitude> Members


        /// <summary>
        /// Compares the current instance to another instance using the specified
        /// precision.
        /// </summary>
        /// <returns>
        /// 	<para>A <strong>Boolean</strong>, <strong>True</strong> if the
        ///     <strong>DecimalDegrees</strong> property of the current instance matches the
        ///     specified instance's <strong>DecimalDegrees</strong> property.</para>
        /// </returns>
        /// <remarks>
        /// 	<para>This is typically used in cases where precision is only significant for a few
        ///     digits and exact comparison is not necessary.</para>
        /// 	<para><em>NOTE: This method compares objects by value, not by
        ///     reference.</em></para>
        /// </remarks>
        /// <seealso cref="Equals(Longitude)">Equals Method</seealso>
        /// <example>
        ///     These examples compare two fractional values using specific numbers of digits for
        ///     comparison. 
        ///     <code lang="VB" title="[New Example]">
        /// ' Equals will return False
        /// Dim Longitude1 As New Longitude(90.15);
        /// Dim Longitude2 As New Longitude(90.12);
        /// If Longitude1.Equals(Longitude2, 2) Then
        ///      Debug.WriteLine("The values are the same to two digits of precision.");
        ///  
        /// ' Equals will return True
        /// Dim Longitude1 As New Longitude(90.15);
        /// Dim Longitude2 As New Longitude(90.12);
        /// If Longitude1.Equals(Longitude2, 1) Then
        ///      Debug.WriteLine("The values are the same to one digit of precision.");
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// // Equals will return False
        /// Longitude Longitude1 = new Longitude(90.15);
        /// Longitude Longitude2 = new Longitude(90.12);
        /// if(Longitude1.Equals(Longitude2, 2))
        ///      Console.WriteLine("The values are the same to two digits of precision.");
        ///  
        /// // Equals will return True
        /// Longitude Longitude1 = new Longitude(90.15);
        /// Longitude Longitude2 = new Longitude(90.12);
        /// if(Longitude1.Equals(Longitude2, 1))
        ///      Console.WriteLine("The values are the same to one digits of precision.");
        ///     </code>
        /// </example>
        public bool Equals(Longitude other, int decimals)
        {
            return Math.Round(_DecimalDegrees, decimals) == Math.Round(other.DecimalDegrees, decimals);
        }

        public bool Equals(Longitude other)
        {
            return _DecimalDegrees.Equals(other.DecimalDegrees);
        }

        #endregion

        #region IComparable<Longitude> Members

        public int CompareTo(Longitude other)
		{
            return _DecimalDegrees.CompareTo(other.DecimalDegrees);
		}

		#endregion

		#region IFormattable Members

        /// <remarks>
        ///	 <para>This powerful method returns the current angular measurement in a specific
        ///	 format. If no value for the format is specified, a format of
        ///	 <strong>hhh°mm'SS.SS"I</strong> (adjusted to the current culture) will be used. The
        ///	 resulting <strong>String</strong> can be converted back into an
        ///	 <strong>Longitude</strong> via the
        ///	 <see href="Angle.Parse">Parse</see> method so long as a delimiter separates each individual
        ///	 value.</para>
        /// </remarks>
        /// <param name="format">
        ///	 <para>A combination of symbols, spaces, and any of the following case-insensitive
        ///	 letters: <strong>D</strong> or <strong>H</strong> for hours, <strong>M</strong> for
        ///	 minutes, <strong>S</strong> for seconds, and <strong>I</strong> to indicate the
        ///	 hemisphere. Here are some examples:</para>
        /// 
        ///	 <para>
        ///		 <table cellspacing="0" cols="3" cellpadding="2" width="100%">
        ///			 <tbody>
        ///				 <tr>
        ///					 <td>HH°MM'SS.SS"</td>
        /// 
        ///					 <td>HHH.H°</td>
        /// 
        ///					 <td>HH MM.MM</td>
        /// 
        ///					 <td>HHHMMSS</td>
        ///				 </tr>
        /// 
        ///				 <tr>
        ///					 <td>HH°MM'SS.SS"I</td>
        /// 
        ///					 <td>HHH.H°I</td>
        /// 
        ///					 <td>HH MM.MMI</td>
        /// 
        ///					 <td>HHHMMSSI</td>
        ///				 </tr>
        ///			 </tbody>
        ///		 </table>
        ///	 </para>
        /// </param>
        /// <param name="formatProvider"> A <strong>CultureInfo</strong> object used to properly format the string information.
        /// </param>
        /// <summary>Outputs the current instance as a string using the specified format.</summary>
        /// <returns>A <strong>String</strong> matching the specified format.</returns>
        /// <remarks>
        /// 	<para>This method returns the current instance output in a specific format. If no
        ///     value for the format is specified, a default format of "d.dddd" is used. Any string
        ///     output by this method can be converted back into an Longitude object using the
        ///     <strong>Parse</strong> method or <strong>Longitude(string)</strong> constructor.</para>
        /// </remarks>
        /// <seealso cref="ToString()">ToString Method</seealso>
        /// <seealso cref="Parse(string)">Parse Method</seealso>
        /// <example>
        ///     This example uses the <strong>ToString</strong> method to output an angle in a
        ///     custom format. The " <strong>h°</strong> " code represents hours along with a
        ///     degree symbol (Alt+0176 on the keypad), and " <strong>m.mm</strong> " represents
        ///     the minutes out to two decimals. Mmm. 
        ///     <code lang="VB">
        /// Dim MyLongitude As New Longitude(45, 16.772)
        /// Debug.WriteLine(MyLongitude.ToString("h°m.mm", CultureInfo.CurrentCulture))
        /// ' Output: 45°16.78
        ///     </code>
        /// 	<code lang="CS">
        /// Dim MyLongitude As New Longitude(45, 16.772);
        /// Debug.WriteLine(MyLongitude.ToString("h°m.mm", CultureInfo.CurrentCulture));
        /// // Output: 45°16.78
        ///     </code>
        /// </example>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider;

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            if (format == null || format.Length == 0)
                format = "G";

            int StartChar = 0;
            int EndChar = 0;
            string SubFormat = null;
            string NewFormat = null;
            bool IsDecimalHandled = false;
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
                StartChar = format.IndexOf("H");
                if (StartChar > -1)
                {
                    // Yes. Look for subsequent H characters or a period
                    EndChar = format.LastIndexOf("H");
                    // Extract the sub-string
                    SubFormat = format.Substring(StartChar, EndChar - StartChar + 1);
                    // Convert to a numberic-formattable string
                    NewFormat = SubFormat.Replace("H", "0");
                    // Replace the hours
                    if (NewFormat.IndexOf(culture.NumberFormat.NumberDecimalSeparator) > -1)
                    {
                        IsDecimalHandled = true;
                        // If an indicator is present, drop the minus sign
                        if (format.IndexOf("I") > -1)
                            format = format.Replace(SubFormat, Math.Abs(_DecimalDegrees).ToString(NewFormat, culture));
                        else
                            format = format.Replace(SubFormat, _DecimalDegrees.ToString(NewFormat, culture));
                    }
                    else
                    {
                        if (format.IndexOf("I") > -1)
                            format = format.Replace(SubFormat, Math.Abs(Hours).ToString(NewFormat, culture));
                        else
                            format = format.Replace(SubFormat, Hours.ToString(NewFormat, culture));
                    }
                }
                // Is there an hours specifier°
                StartChar = format.IndexOf("M");
                if (StartChar > -1)
                {
                    // Yes. Look for subsequent H characters or a period
                    EndChar = format.LastIndexOf("M");
                    // Extract the sub-string
                    SubFormat = format.Substring(StartChar, EndChar - StartChar + 1);
                    // Convert to a numberic-formattable string
                    NewFormat = SubFormat.Replace("M", "0");
                    // Replace the hours
                    if (NewFormat.IndexOf(culture.NumberFormat.NumberDecimalSeparator) > -1)
                    {
                        if (IsDecimalHandled)
                        {
                            throw new ArgumentException(Properties.Resources.Longitude_OnlyRightmostIsDecimal);
                        }
                        IsDecimalHandled = true;
                        format = format.Replace(SubFormat, DecimalMinutes.ToString(NewFormat, culture));
                    }
                    else
                    {
                        format = format.Replace(SubFormat, Minutes.ToString(NewFormat, culture));
                    }
                }
                // Is there an hours specifier°
                StartChar = format.IndexOf("S");
                if (StartChar > -1)
                {
                    // Yes. Look for subsequent H characters or a period
                    EndChar = format.LastIndexOf("S");
                    // Extract the sub-string
                    SubFormat = format.Substring(StartChar, EndChar - StartChar + 1);
                    // Convert to a numberic-formattable string
                    NewFormat = SubFormat.Replace("S", "0");
                    // Replace the hours
                    if (NewFormat.IndexOf(culture.NumberFormat.NumberDecimalSeparator) > -1)
                    {
                        if (IsDecimalHandled)
                        {
                            throw new ArgumentException(Properties.Resources.Longitude_OnlyRightmostIsDecimal);
                        }
                        IsDecimalHandled = true;
                        format = format.Replace(SubFormat, Seconds.ToString(NewFormat, culture));
                    }
                    else
                    {
                        format = format.Replace(SubFormat, Seconds.ToString(NewFormat, culture));
                    }
                }

                // Now add on an indicator if specified
                // Is there an hours specifier°
                StartChar = format.IndexOf("I");
                if (StartChar > -1)
                {
                    // Yes. Look for subsequent H characters or a period
                    EndChar = format.LastIndexOf("I");
                    // Extract the sub-string
                    SubFormat = format.Substring(StartChar, EndChar - StartChar + 1);
                    // Convert to a numberic-formattable string
                    switch (SubFormat.Length)
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

		#endregion

        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(_DecimalDegrees.ToString("G17", CultureInfo.InvariantCulture));
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.Text)
                _DecimalDegrees = reader.ReadContentAsDouble();
            else
                _DecimalDegrees = reader.ReadElementContentAsDouble();
        }

        #endregion
    }

    /// <summary>Indicates the position of a longitude measurement relative to the <a href="http://www.greenwichmeridian.com/">Prime Meridian</a>.</summary>
    /// <remarks>
    /// <para>This enumeration is used by the <see cref="Longitude.Hemisphere">Hemisphere</see> 
    /// property of the <see cref="Longitude">Latitude</see> class. If a longitude is west of the 
    /// Prime Meridian, it's value is displayed as a negative number, or with a single letter (but not 
    /// both). For example, 105 degrees west can be expressed in either of these
    /// ways:</para>
    /// 
    /// <list type="bullet">
    ///	 <item>105°W</item>
    ///	 <item>-105°</item>
    /// </list>
    /// </remarks>
    /// <seealso cref="Latitude.Hemisphere">Hemisphere Property (Latitude Class)</seealso>
    /// <seealso cref="LatitudeHemisphere">LatitudeHemisphere Enumeration</seealso>
    public enum LongitudeHemisphere : int
    {
        /// <summary>Missing longitude information.</summary>
        None = 0,
        /// <summary>The longitude is east of the Prime Meridian.</summary>
        East = 1,
        /// <summary>The longitude is west of the Prime Meridian.</summary>
        West = 2
    }	
}