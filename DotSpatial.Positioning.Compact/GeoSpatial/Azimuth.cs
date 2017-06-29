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
    /// <summary>Represents an angular measurement around the horizon between 0° and 
    /// 360°.</summary>
    /// <remarks>
    /// 	<para>This class is used to indicate a horizontal direction of travel, such as the
    ///     bearing from one point on Earth to another. This class can also be combined with an
    ///     Elevation object to form a three-dimensional direction towards an object in space,
    ///     such as a GPS satellite.</para>
    /// </remarks>
    /// <example>
    ///     These examples create new instances of an Azimuth object using different
    ///     techniques.
    ///     <code lang="VB" description="Create a new instance of 45° (northeast).">
    /// Dim MyAzimuth As New Azimuth(45)
    ///     </code>
    /// 	<code lang="CS" description="Create a new instance of 45° (northeast).">
    /// Azimuth MyAzimuth = new Azimuth(45);
    ///     </code>
    /// 	<code lang="VB" description="Create a new instance of 45°30'15.">
    /// Dim MyAzimuth As New Azimuth(45, 30, 15)
    ///     </code>
    /// 	<code lang="CS" description="Create a new instance of 45°30'15.">
    /// Azimuth MyAzimuth = new Azimuth(45, 30, 15);
    ///     </code>
    /// 	<code lang="VB" description="Create a new instance equal to a known compass direction.">
    /// Dim MyAzimuth As Azimuth = Azimuth.NorthNorthwest
    ///     </code>
    /// 	<code lang="CS" description="Create a new instance equal to a known compass direction.">
    /// Azimuth MyAzimuth = Azimuth.NorthNorthwest;
    ///     </code>
    /// </example>
    /// <remarks>
    /// This class is used to indicate a horizontal direction of travel. In most
    /// situations, instances of this object identify one object's position relative to
    /// another, though an <strong>Azimuth</strong> can also be combined with an
    /// <strong>Elevation</strong> object (which measures a vertical direction), to create a
    /// three-dimensional direction towards an object in space, such as a GPS satellite.
    /// </remarks>
#if !PocketPC || DesignTime
    [TypeConverter("DotSpatial.Positioning.Design.AzimuthConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=d77afaeb30e3236a")]
#endif
    public struct Azimuth : IFormattable, IComparable<Azimuth>, IEquatable<Azimuth>, IEquatable<Direction>, ICloneable<Azimuth>, IXmlSerializable
    {
        private double _DecimalDegrees;
        
        #region Constants

        /// <summary>
        /// Controls the number of digits of precision supported by the class.
        /// </summary>
        /// <remarks>
        /// The underlying type for this class is Double, meaning that a maximum of 15 digits are
        /// supported for any value.  Since up to three of these digits will be occupied by a value
        /// between -360 and +360, that leaves 12 digits left over for floating point values.  A
        /// value greater than 12 will lead to mathematical errors, so the value should be 12 or less.
        /// </remarks>
        private const int MaximumPrecisionDigits = 12;

        #endregion

        #region  Constructors

        /// <summary>Creates a new instance with the specified decimal degrees.</summary>
        /// <example>
        ///     This example demonstrates how to create an angle with a measurement of 90°. 
        ///     <code lang="VB">
        /// Dim MyAzimuth As New Azimuth(90)
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = new Azimuth(90);
        ///     </code>
        /// </example>
        /// <returns>An <strong>Azimuth</strong> containing the specified value.</returns>
        public Azimuth(double decimalDegrees)
        {
            // Set the decimal degrees value
            _DecimalDegrees = decimalDegrees;
        }

        /// <summary>Creates a new instance with the specified degrees.</summary>
        /// <returns>An <strong>Azimuth</strong> containing the specified value.</returns>
        /// <param name="hours">
        /// An <strong>Integer</strong> indicating the amount of degrees, typically between 0
        /// and 360.
        /// </param>
        public Azimuth(int hours)
        {
            _DecimalDegrees = ToDecimalDegrees(hours);
        }

        /// <summary>Creates a new instance with the specified hours, minutes and 
        /// seconds.</summary>
        /// <example>
        ///     This example demonstrates how to create an angular measurement of 34°12'29.2 in
        ///     hours, minutes and seconds. 
        ///     <code lang="VB">
        /// Dim MyAzimuth As New Azimuth(34, 12, 29.2)
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = new Azimuth(34, 12, 29.2);
        ///     </code>
        /// </example>
        /// <returns>An <strong>Azimuth</strong> containing the specified value.</returns>
        public Azimuth(int hours, int minutes, double seconds)
        {
            _DecimalDegrees = ToDecimalDegrees(hours, minutes, seconds);
        }

        /// <summary>Creates a new instance with the specified hours and decimal minutes.</summary>
        /// <example>
        ///     This example demonstrates how an angle can be created when only the hours and
        ///     minutes (in decimal form) are known. This creates a value of 12°42.345'. 
        ///     <code lang="VB">
        /// Dim MyAzimuth As New Azimuth(12, 42.345)
        ///     </code>
        /// 	<code lang="VB">
        /// Azimuth MyAzimuth = new Azimuth(12, 42.345);
        ///     </code>
        /// </example>
        /// <remarks>An <strong>Azimuth</strong> containing the specified value.</remarks>
        public Azimuth(int hours, double decimalMinutes)
        {
            _DecimalDegrees = ToDecimalDegrees(hours, decimalMinutes);
        }

        /// <summary>Creates a new instance by converting the specified string.</summary>
        /// <remarks>
        /// This constructor parses the specified string into an <strong>Azimuth</strong>
        /// object using the current culture. This constructor can parse any strings created via
        /// the <strong>ToString</strong> method.
        /// </remarks>
        /// <seealso cref="Azimuth.Parse(string)">Parse Method</seealso>
        /// <example>
        ///     This example creates a new instance by parsing a string. (NOTE: The double-quote is
        ///     doubled up to represent a single double-quote in the string.) 
        ///     <code lang="VB">
        /// Dim MyAzimuth As New Azimuth("123°45'67.8""")
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = new Azimuth("123°45'67.8\"");
        ///     </code>
        /// </example>
        /// <example>
        ///     This example creates a new <strong>Azimuth</strong> object by converting the string
        ///     "NW," short for Northwest. or 315°.
        ///     <code lang="VB" title="[New Example]">
        /// Dim NewAzimuth As New Azimuth("NW")
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Azimuth NewAzimuth = new Azimuth("NW");
        ///     </code>
        /// </example>
        /// <returns>An <strong>Azimuth</strong> containing the specified value.</returns>
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">The Parse method requires a decimal or sexagesimal measurement.</exception>
        /// <exception cref="FormatException" caption="FormatException">Only the right-most portion of a sexagesimal measurement can be a fractional value.</exception>
        /// <exception cref="FormatException" caption="FormatException">Extra characters were encountered while parsing an angular measurement.  Only hours, minutes, and seconds are allowed.</exception>
        /// <exception cref="FormatException" caption="FormatException">The specified text was not fully understood as an angular measurement.</exception>
        public Azimuth(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <summary>
        /// Creates a new instance by converting the specified string using the specified
        /// culture.
        /// </summary>
        /// <remarks>
        /// This constructor parses the specified string into an <strong>Azimuth</strong>
        /// object using the specified culture. This constructor can parse any strings created via
        /// the <strong>ToString</strong> method.
        /// </remarks>
        /// <example>
        ///     This example creates a new <strong>Azimuth</strong> object by converting the string
        ///     "NW," short for Northwest. or 315°.
        ///     <code lang="VB" title="[New Example]">
        /// Dim NewAzimuth As New Azimuth("NW", CultureInfo.CurrentCulture)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Azimuth NewAzimuth = new Azimuth("NW", CultureInfo.CurrentCulture);
        ///     </code>
        /// </example>
        /// <param name="value">
        /// A <strong>String</strong> describing an angle in the form of decimal degrees or a
        /// sexagesimal.
        /// </param>
        /// <param name="culture">
        /// A <strong>CultureInfo</strong> object describing the numeric format to use during
        /// conversion.
        /// </param>
        public Azimuth(string value, CultureInfo culture)
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

            // Try to see it as a cardinal direction
            switch (value.Trim().ToUpper(CultureInfo.InvariantCulture))
            {
                case "N":
                case "NORTH":
                    _DecimalDegrees = Azimuth.North.DecimalDegrees;
                    return;
                case "NNE":
                case "NORTHNORTHEAST":
                case "NORTH-NORTHEAST":
                case "NORTH NORTHEAST":
                    _DecimalDegrees = Azimuth.NorthNortheast.DecimalDegrees;
                    return;
                case "NE":
                case "NORTHEAST":
                case "NORTH-EAST":
                case "NORTH EAST":
                    _DecimalDegrees = Azimuth.Northeast.DecimalDegrees;
                    return;
                case "ENE":
                case "EASTNORTHEAST":
                case "EAST-NORTHEAST":
                case "EAST NORTHEAST":
                    _DecimalDegrees = Azimuth.EastNortheast.DecimalDegrees;
                    return;
                case "E":
                case "EAST":
                    _DecimalDegrees = Azimuth.East.DecimalDegrees;
                    return;
                case "ESE":
                case "EASTSOUTHEAST":
                case "EAST-SOUTHEAST":
                case "EAST SOUTHEAST":
                    _DecimalDegrees = Azimuth.EastSoutheast.DecimalDegrees;
                    return;
                case "SE":
                case "SOUTHEAST":
                case "SOUTH-EAST":
                case "SOUTH EAST":
                    _DecimalDegrees = Azimuth.Southeast.DecimalDegrees;
                    return;
                case "SSE":
                case "SOUTHSOUTHEAST":
                case "SOUTH-SOUTHEAST":
                case "SOUTH SOUTHEAST":
                    _DecimalDegrees = Azimuth.SouthSoutheast.DecimalDegrees;
                    return;
                case "S":
                case "SOUTH":
                    _DecimalDegrees = Azimuth.South.DecimalDegrees;
                    return;
                case "SSW":
                case "SOUTHSOUTHWEST":
                case "SOUTH-SOUTHWEST":
                case "SOUTH SOUTHWEST":
                    _DecimalDegrees = Azimuth.SouthSouthwest.DecimalDegrees;
                    return;
                case "SW":
                case "SOUTHWEST":
                case "SOUTH-WEST":
                case "SOUTH WEST":
                    _DecimalDegrees = Azimuth.Southwest.DecimalDegrees;
                    return;
                case "WSW":
                case "WESTSOUTHWEST":
                case "WEST-SOUTHWEST":
                case "WEST SOUTHWEST":
                    _DecimalDegrees = Azimuth.WestSouthwest.DecimalDegrees;
                    return;
                case "W":
                case "WEST":
                    _DecimalDegrees = Azimuth.West.DecimalDegrees;
                    return;
                case "WNW":
                case "WESTNORTHWEST":
                case "WEST-NORTHWEST":
                case "WEST NORTHWEST":
                    _DecimalDegrees = Azimuth.WestNorthwest.DecimalDegrees;
                    return;
                case "NW":
                case "NORTHWEST":
                case "NORTH-WEST":
                case "NORTH WEST":
                    _DecimalDegrees = Azimuth.Northwest.DecimalDegrees;
                    return;
                case "NNW":
                case "NORTHNORTHWEST":
                case "NORTH-NORTHWEST":
                case "NORTH NORTHWEST":
                    _DecimalDegrees = Azimuth.NorthNorthwest.DecimalDegrees;
                    return;
            }

            // Yes. First, clean up the strings
            try
            {
                // Clean up the string
                StringBuilder NewValue = new StringBuilder(value);
                NewValue.Replace("°", " ").Replace("'", " ").Replace("\"", " ").Replace("  ", " ");
                // Now split the values into an array
                string[] Values = NewValue.ToString().Trim().Split(' ');
                // How many elements are in the array?
                switch (Values.Length)
                {
                    case 0:
                        // Return a blank Azimuth
                        _DecimalDegrees = 0.0;
                        return;
                    case 1: // Decimal degrees
                        // Is it infinity?
                        if (String.Compare(Values[0], Properties.Resources.Common_Infinity, true, culture) == 0)
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
                                double.Parse(Values[0].Substring(5, 2), culture));
                            return;
                        }
                        else if (Values[0].Length == 8 && Values[0][0] == '-' && Values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) == -1)
                        {
                            _DecimalDegrees = ToDecimalDegrees(
                                int.Parse(Values[0].Substring(0, 4), culture),
                                int.Parse(Values[0].Substring(4, 2), culture),
                                double.Parse(Values[0].Substring(6, 2), culture));
                            return;
                        }
                        else
                        {
                            _DecimalDegrees = double.Parse(Values[0], culture);
                            return;
                        }
                    case 2: // Hours and decimal minutes
                        // If this is a fractional value, remember that it is
                        if (Values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) != -1)
                        {
                            throw new ArgumentException(Properties.Resources.Angle_OnlyRightmostIsDecimal, "value");
                        }
                        // Set decimal degrees
                        _DecimalDegrees = ToDecimalDegrees(
                            int.Parse(Values[0], culture),
                            float.Parse(Values[1], culture));
                        return;
                    default: // Hours, minutes and seconds  (most likely)
                        // If this is a fractional value, remember that it is
                        if (Values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) != -1 || Values[0].IndexOf(culture.NumberFormat.NumberDecimalSeparator) != -1)
                        {
                            throw new ArgumentException(Properties.Resources.Angle_OnlyRightmostIsDecimal, "value");
                        }

                        // Set decimal degrees
                        _DecimalDegrees = ToDecimalDegrees(
                            int.Parse(Values[0], culture),
                            int.Parse(Values[1], culture),
                            double.Parse(Values[2], culture));
                        return;
                }
            }
            catch (Exception ex)
            {
#if PocketPC
                    throw new ArgumentException(Properties.Resources.Angle_InvalidFormat, ex);
#else
                throw new ArgumentException(Properties.Resources.Angle_InvalidFormat, "value", ex);
#endif
            }
        }

        /// <summary>
        /// Creates a new instance by deserializing the specified XML.
        /// </summary>
        /// <param name="reader"></param>
        public Azimuth(XmlReader reader)
        {
            // Initialize all fields
            _DecimalDegrees = Double.NaN;

            // Deserialize the object from XML
            ReadXml(reader);
        }

        #endregion

        #region Fields

        /// <summary>Represents the minimum value of an angle in one turn of a circle.</summary>
        /// <remarks>
        /// This member is typically used for looping through the entire range of possible
        /// angles. It is possible to create angular values below this value, such as -720°.
        /// </remarks>
        /// <example>
        ///     This example creates an angle representing the minimum allowed value. 
        ///     <code lang="VB">
        /// Dim MyAzimuth As Azimuth = Azimuth.Minimum
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = Azimuth.Minimum;
        ///     </code>
        /// 	<code lang="C++">
        /// Azimuth MyAzimuth = Azimuth.Minimum;
        ///     </code>
        /// </example>
        /// <value>An Azimuth with a value of -359.999999°.</value>
        public static readonly Azimuth Minimum = new Azimuth(-359.99999999);
        /// <summary>Represents an angle with no value.</summary>
        /// <remarks>
        /// This member is typically used to initialize an angle variable to zero. When an
        /// angle has a value of zero, its <see cref="IsEmpty">IsEmpty</see> property returns
        /// <strong>True</strong>.
        /// </remarks>
        /// <value>An Azimuth containing a value of zero (0°).</value>
        /// <seealso cref="IsEmpty">IsEmpty Property</seealso>
        public static readonly Azimuth Empty = new Azimuth(0.0);
        /// <summary>
        /// Represents an angle with infinite value.
        /// </summary>
        /// <remarks>
        /// In some cases, the result of an angular calculation may be infinity. This member
        /// is used in such cases. The <see cref="DecimalDegrees">DecimalDegrees</see> property is
        /// set to Double.PositiveInfinity.
        /// </remarks>
        public static readonly Azimuth Infinity = new Azimuth(double.PositiveInfinity);
        /// <summary>Represents the maximum value of an angle in one turn of a circle.</summary>
        /// <remarks>
        /// This member is typically used for looping through the entire range of possible
        /// angles, or to test the range of a value. It is possible to create angular values below
        /// this value, such as 720°.
        /// </remarks>
        /// <example>
        ///     This example creates an angle representing the maximum allowed value of 359.9999°. 
        ///     <code lang="VB">
        /// Dim MyAzimuth As Azimuth = Azimuth.Maximum
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = Azimuth.Maximum;
        ///     </code>
        /// </example>
        public static readonly Azimuth Maximum = new Azimuth(359.99999999);

        /// <summary>Represents a direction of travel of 0°.</summary>
        /// <example>
        ///     This example creates an Azimuth representing North. 
        ///     <code lang="VB">
        /// Dim MyAzimuth As Azimuth = Azimuth.North
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = Azimuth.North;
        ///     </code>
        /// </example>
        public static readonly Azimuth North = new Azimuth(0.0);
        /// <summary>Represents a direction of travel of 22.5°, between north and northeast.</summary>
        /// <example>
        /// 	<code lang="VB">
        /// Dim MyAzimuth As Azimuth = Azimuth.NorthNortheast
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = Azimuth.NorthNortheast;
        ///     </code>
        /// </example>
        public static readonly Azimuth NorthNortheast = new Azimuth(22.5);
        /// <summary>Represents a direction of travel of 45°.</summary>
        /// <example>
        /// 	<code lang="VB">
        /// Dim MyAzimuth As Azimuth = Azimuth.Northeast
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = Azimuth.Northeast;
        ///     </code>
        /// </example>
        public static readonly Azimuth Northeast = new Azimuth(45.0);
        /// <summary>Represents a direction of travel of 67.5°.</summary>
        /// <example>
        /// 	<code lang="VB">
        /// Dim MyAzimuth As Azimuth = Azimuth.EastNortheast
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = Azimuth.EastNortheast;
        ///     </code>
        /// </example>
        public static readonly Azimuth EastNortheast = new Azimuth(67.5);
        /// <summary>Represents a direction of travel of 90°.</summary>
        /// <example>
        /// 	<code lang="VB">
        /// Dim MyAzimuth As Azimuth = Azimuth.East
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = Azimuth.East;
        ///     </code>
        /// </example>
        public static readonly Azimuth East = new Azimuth(90.0);
        /// <summary>Represents a direction of travel of 112.5°, between east and southeast.</summary>
        /// <example>
        /// 	<code lang="VB">
        /// Dim MyAzimuth As Azimuth = Azimuth.EastSoutheast
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = Azimuth.EastSoutheast;
        ///     </code>
        /// </example>
        public static readonly Azimuth EastSoutheast = new Azimuth(112.5);
        /// <summary>Represents a direction of travel of 135°.</summary>
        /// <example>
        /// 	<code lang="VB">
        /// Dim MyAzimuth As Azimuth = Azimuth.Southeast
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = Azimuth.Southeast;
        ///     </code>
        /// </example>
        public static readonly Azimuth Southeast = new Azimuth(135.0);
        /// <summary>Represents a direction of travel of 157.5°, between south and southeast.</summary>
        /// <example>
        /// 	<code lang="VB">
        /// Dim MyAzimuth As Azimuth = Azimuth.SouthSoutheast
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = Azimuth.SouthSoutheast;
        ///     </code>
        /// </example>
        public static readonly Azimuth SouthSoutheast = new Azimuth(157.5);
        /// <summary>Represents a direction of travel of 180°.</summary>
        /// <example>
        /// 	<code lang="VB">
        /// Dim MyAzimuth As Azimuth = Azimuth.South
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = Azimuth.South;
        ///     </code>
        /// </example>
        public static readonly Azimuth South = new Azimuth(180.0);
        /// <summary>Represents a direction of travel of 202.5°, between south and southwest.</summary>
        /// <example>
        /// 	<code lang="VB">
        /// Dim MyAzimuth As Azimuth = Azimuth.SouthSouthwest
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = Azimuth.SouthSouthwest;
        ///     </code>
        /// </example>
        public static readonly Azimuth SouthSouthwest = new Azimuth(202.5);
        /// <summary>Represents a direction of travel of 225°.</summary>
        /// <example>
        /// 	<code lang="VB">
        /// Dim MyAzimuth As Azimuth = Azimuth.Southwest
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = Azimuth.Southwest;
        ///     </code>
        /// </example>
        public static readonly Azimuth Southwest = new Azimuth(225.0);
        /// <summary>Represents a direction of travel of 247.5°, between west and southwest.</summary>
        /// <example>
        /// 	<code lang="VB">
        /// Dim MyAzimuth As Azimuth = Azimuth.WestSouthwest
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = Azimuth.WestSouthwest;
        ///     </code>
        /// </example>
        public static readonly Azimuth WestSouthwest = new Azimuth(247.5);
        /// <summary>Represents a direction of travel of 270°.</summary>
        /// <example>
        /// 	<code lang="VB">
        /// Dim MyAzimuth As Azimuth = Azimuth.West
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = Azimuth.West;
        ///     </code>
        /// </example>
        public static readonly Azimuth West = new Azimuth(270.0);
        /// <summary>Represents a direction of travel of 292.5°, between west and northwest.</summary>
        /// <example>
        /// 	<code lang="VB">
        /// Dim MyAzimuth As Azimuth = Azimuth.WestNorthwest
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = Azimuth.WestNorthwest;
        ///     </code>
        /// </example>
        public static readonly Azimuth WestNorthwest = new Azimuth(292.5);
        /// <summary>Represents a direction of travel of 315°.</summary>
        /// <example>
        /// 	<code lang="VB">
        /// Dim MyAzimuth As Azimuth = Azimuth.Northwest
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = Azimuth.Northwest;
        ///     </code>
        /// </example>
        public static readonly Azimuth Northwest = new Azimuth(315.0);
        /// <summary>Represents a direction of travel of 337.5°, between north and northwest.</summary>
        /// <example>
        /// 	<code lang="VB">
        /// Dim MyAzimuth As Azimuth = Azimuth.NorthNorthwest
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = Azimuth.NorthNorthwest;
        ///     </code>
        /// </example>
        public static readonly Azimuth NorthNorthwest = new Azimuth(337.5);
        /// <summary>
        /// Represents an invalid or unspecified value.
        /// </summary>
        public static readonly Azimuth Invalid = new Azimuth(double.NaN);

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
        /// Dim MyAzimuth As New Azimuth(20, 30)
        /// ' Setting the DecimalMinutes recalculated other properties 
        /// Debug.WriteLine(MyAzimuth.DecimalDegrees)
        /// ' Output: "20.5"  the same as 20°30'
        ///     </code>
        /// 	<code lang="CS">
        /// // Create an angle of 20°30'
        /// Azimuth MyAzimuth = New Azimuth(20, 30);
        /// // Setting the DecimalMinutes recalculated other properties 
        /// Console.WriteLine(MyAzimuth.DecimalDegrees)
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
        /// Dim MyAzimuth As New Azimuth(20, 10, 30)
        /// ' The DecimalMinutes property is automatically calculated
        /// Debug.WriteLine(MyAzimuth.DecimalMinutes)
        /// ' Output: "10.5"
        ///     </code>
        /// 	<code lang="CS">
        /// // Create an angle of 20°10'30"
        /// Azimuth MyAzimuth = new Azimuth(20, 10, 30);
        /// // The DecimalMinutes property is automatically calculated
        /// Console.WriteLine(MyAzimuth.DecimalMinutes)
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
        /// Dim MyAzimuth As New Azimuth(60.5)
        /// Debug.WriteLine(MyAzimuth.Hours)
        /// ' Output: 60
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = new Azimuth(60.5);
        /// Console.WriteLine(MyAzimuth.Hours);
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
        /// Dim MyAzimuth As New Azimuth(45.5)
        /// Debug.WriteLine(MyAzimuth.Minutes)
        /// ' Output: 30
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = new Azimuth(45.5);
        /// Console.WriteLine(MyAzimuth.Minutes);
        /// // Output: 30
        ///     </code>
        /// </example>
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
        /// Dim MyAzimuth As New Azimuth(45, 10.5)
        /// Debug.WriteLine(MyAzimuth.Seconds)
        /// ' Output: 30
        ///     </code>
        /// 	<code lang="CS">
        /// Dim MyAzimuth As New Azimuth(45, 10.5);
        /// Console.WriteLine(MyAzimuth.Seconds);
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

        /// <summary>Returns the current instance expressed as a compass direction.</summary>
        /// <remarks>
        /// This property converts an azimuth to the nearest of sixteen compass directions.
        /// For example, an azimuth of 89° points almost east, therefore a value of
        /// <strong>East</strong> would be returned. This property is typically used for user
        /// interfaces to express an azimuth in a form that is easy to understand.
        /// </remarks>
        /// <example>
        ///     This example outputs the direction associated 272°, which is <strong>West</strong>.
        ///     <code lang="VB">
        /// Dim MyAzimuth As New Azimuth(272)
        /// Debug.WriteLine(MyAzimuth.Direction.ToString())
        /// ' Output: West
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = new Azimuth(272);
        /// Console.WriteLine(MyAzimuth.Direction.ToString());
        /// // Output: West
        ///     </code>
        /// </example>
        /// <value>A <strong>Direction</strong> value.</value>
        public Direction Direction
        {
            get
            {
                if ((DecimalDegrees >= (360 - 11.25) & DecimalDegrees < 360) || (DecimalDegrees >= 0 & DecimalDegrees <= (0 + 11.25))) // North
                    return Direction.North;
                else if (DecimalDegrees >= (22.5 - 11.25) & DecimalDegrees < (22.5 + 11.25)) // North-Northeast
                    return Direction.NorthNortheast;
                else if (DecimalDegrees >= (45 - 11.25) & DecimalDegrees < (45 + 11.25)) // Northeast
                    return Direction.Northeast;
                else if (DecimalDegrees >= (67.5 - 11.25) & DecimalDegrees < (67.5 + 11.25)) // East-Northeast
                    return Direction.EastNortheast;
                else if (DecimalDegrees >= (90 - 11.25) & DecimalDegrees < (90 + 11.25)) // East
                    return Direction.East;
                else if (DecimalDegrees >= (112.5 - 11.25) & DecimalDegrees < (112.5 + 11.25)) // East-Southeast
                    return Direction.EastSoutheast;
                else if (DecimalDegrees >= (135 - 11.25) & DecimalDegrees < (135 + 11.25)) // Southeast
                    return Direction.Southeast;
                else if (DecimalDegrees >= (157.5 - 11.25) & DecimalDegrees < (157.5 + 11.25)) // South-Southeast
                    return Direction.SouthSoutheast;
                else if (DecimalDegrees >= (180 - 11.25) & DecimalDegrees < (180 + 11.25)) // South
                    return Direction.South;
                else if (DecimalDegrees >= (202.5 - 11.25) & DecimalDegrees < (202.5 + 11.25)) // South-Southwest
                    return Direction.SouthSouthwest;
                else if (DecimalDegrees >= (225 - 11.25) & DecimalDegrees < (225 + 11.25)) // South
                    return Direction.Southwest;
                else if (DecimalDegrees >= (247.5 - 11.25) & DecimalDegrees < (247.5 + 11.25)) // West-Southwest
                    return Direction.WestSouthwest;
                else if (DecimalDegrees >= (270 - 11.25) & DecimalDegrees < (270 + 11.25)) // South
                    return Direction.West;
                else if (DecimalDegrees >= (292.5 - 11.25) & DecimalDegrees < (292.5 + 11.25)) // West-Northwest
                    return Direction.WestNorthwest;
                else if (DecimalDegrees >= (315 - 11.25) & DecimalDegrees < (315 + 11.25)) // Northwest
                    return Direction.Northwest;
                else if (DecimalDegrees >= (337.5 - 11.25) & DecimalDegrees < (337.5 + 11.25)) // North-Northwest
                    return Direction.NorthNorthwest;
                return 0;
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
        /// allowed bounds of 0° and 360°.</summary>
        public bool IsNormalized
        {
            get { return _DecimalDegrees >= 0 && _DecimalDegrees < 360; }
        }

        #endregion

        #region Public Methods

        /// <summary>Modifies a value to its equivalent between 0° and 360°.</summary>
        /// <returns>An <strong>Azimuth</strong> representing the normalized angle.</returns>
        /// <remarks>
        /// 	<para>This function is used to ensure that an angular measurement is within the
        ///     allowed bounds of 0° and 360°. If a value of 360° or 720° is passed, a value of 0°
        ///     is returned since 360° and 720° represent the same point on a circle. For the Azimuth
        ///     class, this function is the same as "value Mod 360".</para>
        /// </remarks>
        /// <seealso cref="Normalize()">Normalize(Azimuth) Method</seealso>
        /// <example>
        ///     This example demonstrates how normalization is used. The Stop statement is hit.
        ///     This example demonstrates how the Normalize method can ensure that an angle fits
        ///     between 0° and 359.9999°. This example normalizes 725° into 5°. 
        ///     <code lang="VB">
        /// Dim MyAzimuth As New Azimuth(720)
        /// MyAzimuth = MyAzimuth.Normalize()
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = new Azimuth(720);
        /// MyAzimuth = MyAzimuth.Normalize();
        ///     </code>
        /// 	<code lang="VB">
        /// Dim MyValue As New Azimuth(725)
        /// MyValue = MyValue.Normalize()
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyValue = new Azimuth(725);
        /// MyValue = MyValue.Normalize();
        ///     </code>
        /// </example>
        public Azimuth Normalize()
        {
            double _Value = _DecimalDegrees;
            while (_Value < 0)
            {
                _Value += 360.0;
            }
            return new Azimuth(_Value % 360);
        }

        /// <summary>
        /// Returns whether the current value is between the specified values.
        /// </summary>
        /// <param name="start">An <strong>Azimuth</strong> marking the start of a range.</param>
        /// <param name="end">An <strong>Azimuth</strong> marking the end of a range.</param>
        /// <returns>A <strong>Boolean</strong> value.</returns>
        /// <remarks>This property is used to determine whether a value is within a specified range.  If the
        /// starting value is less than the end value, a basic greater-than or less-than comparison is performed.  
        /// If, however, the end value is greater than the start, it is assumed that the range crosses the 0/360
        /// boundary.  For example, if the start is 270 and the end is 90, a value of <strong>True</strong> is 
        /// returned if the current value is between 270 and 360, or 0 and 90.</remarks>
        public bool IsBetween(Azimuth start, Azimuth end)
        {
            // Is the start value less than the end value?
            if (start.DecimalDegrees <= end.DecimalDegrees)
            {
                // Yes.  This is a simple check
                return _DecimalDegrees >= start.DecimalDegrees && _DecimalDegrees <= end.DecimalDegrees;
            }
            else
            {
                // No, the value crosses the 0/360 line.
                return (_DecimalDegrees >= 0 && _DecimalDegrees <= end.DecimalDegrees)
                          || (_DecimalDegrees <= 360 && _DecimalDegrees >= start.DecimalDegrees);
            }
        }

        /// <summary>Returns the smallest integer greater than the specified value.</summary>
        public Azimuth Ceiling()
        {
            return new Azimuth(Math.Ceiling(_DecimalDegrees));
        }

        /// <summary>Returns the largest integer which is smaller than the specified value.</summary>
        public Azimuth Floor()
        {
            return new Azimuth(Math.Floor(_DecimalDegrees));
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
        public Azimuth Round(int decimals)
        {
            return new Azimuth(Math.Round(_DecimalDegrees, decimals));
        }

        /// <summary>Returns a new instance whose Seconds property is evenly divisible by 15.</summary>
        /// <returns>An <strong>Azimuth</strong> containing the rounded value.</returns>
        /// <remarks>
        /// This method is used to align or "snap" an angle to a regular interval. For
        /// example, a grid might be easier to read if it were drawn at 30-second intervals instead
        /// of 24.198-second intervals.
        /// </remarks>
        public Azimuth RoundSeconds()
        {
            return RoundSeconds(15.0);
        }

        /// <summary>
        /// Returns a new angle whose Seconds property is evenly divisible by the specified amount.
        /// </summary>
        /// <returns>An <strong>Azimuth</strong> containing the rounded value.</returns>
        /// <remarks>
        /// This method is used to align or "snap" an angle to a regular interval. For
        /// example, a grid might be easier to read if it were drawn at 30-second intervals instead
        /// of 24.198-second intervals.
        /// </remarks>
        /// <param name="interval">
        /// A <strong>Double</strong> between 0 and 60 indicating the interval to round
        /// to.
        /// </param>
        public Azimuth RoundSeconds(double interval)
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
                return new Azimuth(Hours, Minutes, NewSeconds);
            }
            // return the new value
            return new Azimuth(Hours, Minutes, NewSeconds);
        }


        /// <summary>Outputs the azimuth as a string using the specified format.</summary>
        /// <returns>A <strong>String</strong> in the specified format.</returns>
        /// <remarks>
        /// 	<para>This method returns the current instance output in a specific format. If no
        ///     value for the format is specified, a default format of "cc" is used. Any string
        ///     output by this method can be converted back into an Azimuth object using the
        ///     <strong>Parse</strong> method or <strong>Azimuth(string)</strong>
        ///     constructor.</para>
        /// </remarks>
        /// <seealso cref="ToString()">ToString Method</seealso>
        /// <seealso cref="Parse(string)">Parse Method</seealso>
        /// <example>
        ///     This example uses the <strong>ToString</strong> method to output an azimuth in a
        ///     custom format. The " <strong>d.dd</strong> " code represents decimal degrees
        ///     rounded to two digits, and " <strong>cc</strong> " represents the direction in
        ///     verbose form. 
        ///     <code lang="VB">
        /// Dim MyAzimuth As New Azimuth(90.946)
        /// Debug.WriteLine(MyAzimuth.ToString("d.dd (cc)"))
        /// ' Output: 90.95 (East)
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = new Azimuth(90.946);
        /// Console.WriteLine(MyAzimuth.ToString("d.dd (cc)"));
        /// // Output: 90.95 (East)
        ///     </code>
        /// </example>
        /// <param name="format">
        /// 	<para>A <strong>String</strong> consisting of any number of the following
        ///     codes:</para>
        /// 	<para>
        /// 		<list type="table">
        /// 			<item>
        /// 				<term><strong>c</strong></term>
        /// 				<description>The object is output as an abbreviated direction.
        ///                 <strong>N</strong>, <strong>NE</strong>,
        ///                 <strong>NNW</strong></description>
        /// 			</item>
        /// 			<item>
        /// 				<term><strong>cc</strong></term>
        /// 				<description>The object is output as a full direction.
        ///                 <strong>North</strong>, <strong>Northeast</strong>,
        ///                 <strong>North-Northwest</strong></description>
        /// 			</item>
        /// 			<item>
        /// 				<term><strong>d</strong></term>
        /// 				<description>Represents one digit from the
        ///                 <strong>DecimalDegrees</strong> property.</description>
        /// 			</item>
        /// 			<item>
        /// 				<term><strong>h</strong></term>
        /// 				<description>Represents one digit from the
        ///                 <strong>Hours</strong> property.</description>
        /// 			</item>
        /// 			<item>
        /// 				<term><strong>m</strong></term>
        /// 				<description>Represents one digit from the <strong>Minutes</strong>
        ///                 property.</description>
        /// 			</item>
        /// 			<item>
        /// 				<term><strong>s</strong></term>
        /// 				<description>Represents one digit from the <strong>Seconds</strong>
        ///                 property.</description>
        /// 			</item>
        /// 		</list>
        /// 	</para>
        /// </param>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns the object with the smallest value.</summary>
        /// <returns>The <strong>Azimuth</strong> containing the smallest value.</returns>
        /// <param name="value">An <strong>Azimuth</strong> object to compare to the current instance.</param>
        public Azimuth LesserOf(Azimuth value)
        {
            if (_DecimalDegrees < value.DecimalDegrees)
                return this;
            else
                return value;
        }

        /// <returns>An <strong>Azimuth</strong> containing the largest value.</returns>
        /// <summary>Returns the object with the largest value.</summary>
        /// <param name="value">An <strong>Azimuth</strong> object to compare to the current instance.</param>
        public Azimuth GreaterOf(Azimuth value)
        {
            if (_DecimalDegrees > value.DecimalDegrees)
                return this;
            else
                return value;
        }

        /// <summary>Returns an angle opposite of the current instance.</summary>
        /// <returns>An <strong>Azimuth</strong> representing the mirrored value.</returns>
        /// <remarks>
        /// This method returns the "opposite" of the current instance. The opposite is
        /// defined as the point on the other side of an imaginary circle. For example, if an angle
        /// is 0°, at the top of a circle, this method returns 180°, at the bottom of the
        /// circle.
        /// </remarks>
        /// <example>
        ///     This example creates a new <strong>Azimuth</strong> of 45° then calculates its mirror
        ///     of 225°. (45 + 180) 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Azimuth1 As New Azimuth(45)
        /// Dim Azimuth2 As Azimuth = Azimuth1.Mirror()
        /// Debug.WriteLine(Azimuth2.ToString())
        /// ' Output: 225
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Azimuth Azimuth1 = new Azimuth(45);
        /// Azimuth Azimuth2 = Azimuth1.Mirror();
        /// Console.WriteLine(Azimuth2.ToString());
        /// // Output: 225
        ///     </code>
        /// </example>
        public Azimuth Mirror()
        {
            return new Azimuth(_DecimalDegrees + 180.0).Normalize();
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
        /// Dim MyAzimuth As New Azimuth(90)
        /// Dim MyRadians As Radian = MyAzimuth.ToRadians()
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = new Azimuth(90);
        /// Radian MyRadians = MyAzimuth.ToRadians();
        ///     </code>
        /// </example>
        public Radian ToRadians()
        {
            return Radian.FromDegrees(_DecimalDegrees);
        }

        #endregion

        #region Overrides

        /// <summary>Compares the current value to another Azimuth object's value.</summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the object's DecimalDegrees
        /// properties match.
        /// </returns>
        /// <remarks>This </remarks>
        /// <param name="obj">
        /// An <strong>Azimuth</strong>, <strong>Double</strong>, or <strong>Integer</strong>
        /// to compare with.
        /// </param>
        public override bool Equals(object obj)
        {
            // Convert objects to an Azimuth as needed before comparison
            if (obj is Azimuth)
                return Equals((Azimuth)obj);

            // Compare degree value
            return _DecimalDegrees.Equals(obj);
        }

        /// <summary>Returns a unique code for this instance.</summary>
        /// <remarks>
        /// Since the <strong>Azimuth</strong> class is immutable, this property may be used
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

        /// <summary>Outputs the current instance as a string using the default format.</summary>
        /// <returns><para>A <strong>String</strong> representing the current instance.</para></returns>
        /// <remarks>
        /// This method formats the current instance using the default format of "cc." Any
        /// string output by this method can be converted back into an Azimuth object using the
        /// <strong>Parse</strong> method or <strong>Azimuth(string)</strong> constructor.
        /// </remarks>
        /// <seealso cref="Parse(string)">Parse Method</seealso>
        /// <example>
        ///     This example outputs a value of 90 degrees in the default format of ###.#°. 
        ///     <code lang="VB">
        /// Dim MyAzimuth As New Azimuth(90)
        /// Debug.WriteLine(MyAzimuth.ToString)
        /// ' Output: "90°"
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = new Azimuth(90);
        /// Debug.WriteLine(MyAzimuth.ToString());
        /// // Output: "90°"
        ///     </code>
        /// </example>
        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion

        #region Static Methods

        /// <summary>Converts the specified value to its equivalent between 0° and 360°.</summary>
        /// <returns>
        /// An Azimuth containing a value equivalent to the value specified, but between 0° and
        /// 360°.
        /// </returns>
        /// <param name="decimalDegrees">A <strong>Double</strong> value to be normalized.</param>
        public static Azimuth Normalize(double decimalDegrees)
        {
            return new Azimuth(decimalDegrees).Normalize();
        }

        /// <summary>
        /// Converts a <strong>Direction</strong> value into an <strong>Azimuth</strong>
        /// object.
        /// </summary>
        /// <returns>An <strong>Azimuth</strong> equivalent to the specified direction.</returns>
        /// <param name="direction">A value from the <strong>Direction</strong> enumeration to convert.</param>
        public static Azimuth FromDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return Azimuth.North;
                case Direction.NorthNortheast:
                    return Azimuth.NorthNortheast;
                case Direction.Northeast:
                    return Azimuth.Northeast;
                case Direction.EastNortheast:
                    return Azimuth.EastNortheast;
                case Direction.East:
                    return Azimuth.East;
                case Direction.EastSoutheast:
                    return Azimuth.EastSoutheast;
                case Direction.Southeast:
                    return Azimuth.Southeast;
                case Direction.SouthSoutheast:
                    return Azimuth.SouthSoutheast;
                case Direction.South:
                    return Azimuth.South;
                case Direction.SouthSouthwest:
                    return Azimuth.SouthSouthwest;
                case Direction.Southwest:
                    return Azimuth.Southwest;
                case Direction.WestSouthwest:
                    return Azimuth.WestSouthwest;
                case Direction.West:
                    return Azimuth.West;
                case Direction.WestNorthwest:
                    return Azimuth.WestNorthwest;
                case Direction.Northwest:
                    return Azimuth.Northwest;
                case Direction.NorthNorthwest:
                    return Azimuth.NorthNorthwest;
                default:
                    return Azimuth.Empty;
            }
        }

        /// <summary>Returns a random angle between 0° and 360°.</summary>
        /// <returns>An <strong>Azimuth</strong> containing a random value.</returns>
        public static Azimuth Random()
        {
            return Random(new Random(DateTime.Now.Millisecond));
        }

        /// <summary>
        /// Returns a random Azimuth between 0° and 360° using the specified random number
        /// seed.
        /// </summary>
        /// <returns>An <strong>Azimuth</strong> containing a random value.</returns>
        /// <param name="generator">A <strong>Random</strong> object used to generate random values.</param>
        public static Azimuth Random(Random generator)
        {
            return new Azimuth(generator.NextDouble() * 360.0);
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

        /// <returns>The <strong>Azimuth</strong> containing the smallest value.</returns>
        /// <summary>Returns the object with the smallest value.</summary>
        /// <param name="value1">A <strong>Azimuth</strong> object to compare to value2.</param>
        /// <param name="value2">A <strong>Azimuth</strong> object to compare to value1.</param>
        public static Azimuth LesserOf(Azimuth value1, Azimuth value2)
        {
            return value1.LesserOf(value2);
        }

        /// <summary>Returns the object with the largest value.</summary>
        /// <returns>A <strong>Azimuth</strong> containing the largest value.</returns>
        /// <param name="value1">A <strong>Azimuth</strong> object to compare to value2.</param>
        /// <param name="value2">A <strong>Azimuth</strong> object to compare to value1.</param>
        public static Azimuth GreaterOf(Azimuth value1, Azimuth value2)
        {
            return value1.GreaterOf(value2);
        }

        /// <summary>Converts the specified string into an Azimuth object.</summary>
        /// <returns>
        /// 	A new <strong>Azimuth</strong> object populated with the specified 
        /// 	values.
        /// </returns>
        /// <remarks>
        /// 	<para>This method parses the specified string into an <strong>Azimuth</strong> object
        ///     using the current culture. This constructor can parse any strings created via the
        ///     <strong>ToString</strong> method.</para>
        /// </remarks>
        /// <seealso cref="ToString()">ToString Method</seealso>
        /// <example>
        ///     This example creates a new angular measurement using the <strong>Parse</strong>
        ///     method. 
        ///     <code lang="VB">
        /// Dim NewAzimuth As Azimuth = Azimuth.Parse("123.45°")
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth NewAzimuth = Azimuth.Parse("123.45°");
        ///     </code>
        /// </example>
        /// <exception cref="ArgumentNullException" caption="ArgumentNullException">The Parse method requires a decimal or sexagesimal measurement.</exception>
        /// <exception cref="FormatException" caption="FormatException">Only the right-most portion of a sexagesimal measurement can be a fractional value.</exception>
        /// <exception cref="FormatException" caption="FormatException">Extra characters were encountered while parsing an angular measurement.  Only hours, minutes, and seconds are allowed.</exception>
        /// <exception cref="FormatException" caption="FormatException">The specified text was not fully understood as an angular measurement.</exception>
        public static Azimuth Parse(string value)
        {
            return new Azimuth(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Converts the specified string into an <strong>Azimuth</strong> object using the
        /// specified culture.
        /// </summary>
        /// <returns>A new <strong>Azimuth</strong> object equivalent to the specified string.</returns>
        /// <remarks>
        /// 	<para>This method parses the specified string into an <strong>Azimuth</strong>
        ///     object using the specified culture. This method can parse any string created via
        ///     the <strong>ToString</strong> method.</para>
        /// </remarks>
        /// <example>
        ///     This example creates a new <strong>Azimuth</strong> object by converting the string
        ///     "NW," short for Northwest. or 315°.
        ///     <code lang="VB" title="[New Example]">
        /// Dim NewAzimuth As Azimuth = Azimuth.Parse("NW", CultureInfo.CurrentCulture)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Azimuth NewAzimuth = Azimuth.Parse("NW", CultureInfo.CurrentCulture);
        ///     </code>
        /// </example>
        /// <param name="value">
        /// A <strong>String</strong> describing an angle in the form of decimal degrees or a
        /// sexagesimal.
        /// </param>
        /// <param name="culture">
        /// A <strong>CultureInfo</strong> object describing the numeric format to use during
        /// conversion.
        /// </param>
        public static Azimuth Parse(string value, CultureInfo culture)
        {
            return new Azimuth(value, culture);
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
        /// Dim MyRadian As Radian = Azimuth.ToRadians(90)
        ///     </code>
        /// 	<code lang="CS">
        /// Radian MyRadian = Azimuth.ToRadians(90);
        ///     </code>
        /// </example>
        public static Radian ToRadians(Azimuth value)
        {
            return Radian.FromDegrees(value.DecimalDegrees);
        }

        /// <summary>Converts a value in radians into an angular measurement.</summary>
        /// <remarks>
        /// 	This function is typically used in conjunction with the
        /// 	<see cref="Azimuth.ToRadians()">ToRadians</see>
        /// 	method after a trigonometric function has completed. The converted value is stored in
        /// 	the <see cref="DecimalDegrees">DecimalDegrees</see> property.
        /// </remarks>
        /// <seealso cref="Azimuth.ToRadians()">ToRadians</seealso>
        /// <seealso cref="Radian">Radian Class</seealso>
        /// <example>
        ///     This example uses the <strong>FromRadians</strong> method to convert a value of one
        ///     radian into an <strong>Azimuth</strong> of 57°. 
        ///     <code lang="VB">
        /// ' Create a new angle equal to one radian
        /// Dim MyRadians As New Radian(1)
        /// Dim MyAzimuth As Azimuth = Azimuth.FromRadians(MyRadians)
        /// Debug.WriteLine(MyAzimuth.ToString())
        /// ' Output: 57°
        ///     </code>
        /// 	<code lang="CS">
        /// // Create a new angle equal to one radian
        /// Radian MyRadians = new Radian(1);
        /// Azimuth MyAzimuth = Azimuth.FromRadians(MyRadians);
        /// Console.WriteLine(MyAzimuth.ToString());
        /// // Output: 57°
        ///     </code>
        /// </example>
        public static Azimuth FromRadians(Radian radians)
        {
            return new Azimuth(radians.ToDegrees());
        }

        public static Azimuth FromRadians(double radians)
        {
            return new Azimuth(Radian.ToDegrees(radians));
        }

        #endregion

		#region Conversions

		/// <summary>
		/// Converts a measurement in Radians into an Azimuth.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static explicit operator Azimuth(Radian value)
		{
			return new Azimuth(value.ToDegrees());
		}

		/// <summary>
		/// Converts a decimal degree measurement as a Double into an Azimuth.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static explicit operator Azimuth(double value)
		{
			return new Azimuth(value);
		}

        /// <summary>
        /// Converts a decimal degree measurement as a Double into an Azimuth.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator Azimuth(float value)
        {
            return new Azimuth(Convert.ToDouble(value));
        }

		/// <summary>
		/// Converts a decimal degree measurement as a Double into an Azimuth.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static explicit operator double(Azimuth value)
		{
			return value.DecimalDegrees;
		}

		/// <summary>
		/// Converts a decimal degree measurement as a Double into an Azimuth.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static explicit operator float(Azimuth value)
		{
			return Convert.ToSingle(value.DecimalDegrees);
		}

        public static explicit operator Direction(Azimuth value) 
        {
            return value.Direction;
        }

        public static explicit operator Azimuth(Direction value)
        {
            return FromDirection(value);
        }


		/// <summary>
		/// Converts a measurement in degrees as an Integer into an Azimuth.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static explicit operator Azimuth(int value)
		{
			return new Azimuth(value);
		}

        public static explicit operator Azimuth(Angle value)
        {
            return new Azimuth(value.DecimalDegrees);
        }

        public static explicit operator Azimuth(Elevation value)
        {
            return new Azimuth(value.DecimalDegrees);
        }

        public static explicit operator Azimuth(Latitude value)
        {
            return new Azimuth(value.DecimalDegrees);
        }

        public static explicit operator Azimuth(Longitude value)
        {
            return new Azimuth(value.DecimalDegrees);
        }

		/// <summary>
		/// Converts a measurement in the form of a formatted String into an Azimuth.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static explicit operator Azimuth(string value)
		{
			return new Azimuth(value, CultureInfo.CurrentCulture);
		}

		/// <summary>
		/// Converts an Azimuth into a String.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <remarks>This operator calls the ToString() method using the current culture.</remarks>
		public static explicit operator string(Azimuth value)
		{
			return value.ToString("g", CultureInfo.CurrentCulture);
		}

		#endregion

        #region Operators

        public static Azimuth operator +(Azimuth left, Azimuth right)
        {
            return new Azimuth(left.DecimalDegrees + right.DecimalDegrees);
        }

        public static Azimuth operator +(Azimuth left, double right)
        {
            return new Azimuth(left.DecimalDegrees + right);
        }

        public static Azimuth operator -(Azimuth left, Azimuth right)
        {
            return new Azimuth(left.DecimalDegrees - right.DecimalDegrees);
        }

        public static Azimuth operator -(Azimuth left, double right)
        {
            return new Azimuth(left.DecimalDegrees - right);
        }

        public static Azimuth operator *(Azimuth left, Azimuth right)
        {
            return new Azimuth(left.DecimalDegrees * right.DecimalDegrees);
        }

        public static Azimuth operator *(Azimuth left, double right)
        {
            return new Azimuth(left.DecimalDegrees * right);
        }

        public static Azimuth operator /(Azimuth left, Azimuth right)
        {
            return new Azimuth(left.DecimalDegrees / right.DecimalDegrees);
        }

        public static Azimuth operator /(Azimuth left, double right)
        {
            return new Azimuth(left.DecimalDegrees / right);
        }

        public static bool operator ==(Azimuth left, Azimuth right)
        {
            return left.DecimalDegrees.Equals(right.DecimalDegrees);
        }

        public static bool operator ==(Azimuth left, double right)
        {
            return left.DecimalDegrees.Equals(right);
        }

        public static bool operator !=(Azimuth left, Azimuth right)
        {
            return !left.DecimalDegrees.Equals(right.DecimalDegrees);
        }

        public static bool operator !=(Azimuth left, double right)
        {
            return !left.DecimalDegrees.Equals(right);
        }

        public static bool operator >(Azimuth left, Azimuth right)
        {
            return left.DecimalDegrees > right.DecimalDegrees;
        }

        public static bool operator >(Azimuth left, double right)
        {
            return left.DecimalDegrees > right;
        }

        public static bool operator >=(Azimuth left, Azimuth right)
        {
            return left.DecimalDegrees >= right.DecimalDegrees;
        }

        public static bool operator >=(Azimuth left, double right)
        {
            return left.DecimalDegrees >= right;
        }

        public static bool operator <(Azimuth left, Azimuth right)
        {
            return left.DecimalDegrees < right.DecimalDegrees;
        }

        public static bool operator <(Azimuth left, double right)
        {
            return left.DecimalDegrees < right;
        }

        public static bool operator <=(Azimuth left, Azimuth right)
        {
            return left.DecimalDegrees <= right.DecimalDegrees;
        }

        public static bool operator <=(Azimuth left, double right)
        {
            return left.DecimalDegrees <= right;
        }


        #region Math methods

        /// <summary>Returns the current instance increased by one.</summary>
        /// <returns>An <strong>Azimuth</strong> object.</returns>
        /// <remarks>
        /// 	<para>This method increases the <strong>DecimalDegrees</strong> property by 1.0,
        ///     returned as a new instance.</para>
        /// 	<para><font color="red">Since the <strong>Azimuth</strong> class is immutable, this
        ///     method cannot be used to modify an existing instance.</font></para>
        /// </remarks>
        /// <example>
        ///     This example uses the <strong>Increment</strong> method to increase an Azimuth's
        ///     value. It also demonstrates the subtle error which can be caused if
        ///     <strong>Increment</strong> is called while ignoring the return value.
        ///     <code lang="VB" title="[New Example]">
        /// ' Correct use of Increment
        /// Dim Azimuth1 As New Azimuth(89)
        /// Azimuth1 = Azimuth1.Increment()
        ///  
        /// ' Incorrect use of Increment
        /// Dim Azimuth1 = New Azimuth(89)
        /// Azimuth1.Increment()
        /// ' NOTE: Azimuth1 will still be 89°!
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// // Correct use of Increment
        /// Azimuth Azimuth1 = new Azimuth(89);
        /// Azimuth1 = Azimuth1.Increment();
        ///  
        /// // Incorrect use of Increment
        /// Azimuth Azimuth1 = new Azimuth(89);
        /// Azimuth1.Increment();
        /// // NOTE: Azimuth1 will still be 89°!
        ///     </code>
        /// </example>
        public Azimuth Increment()
        {
            return new Azimuth(_DecimalDegrees + 1.0);
        }

        /// <summary>Increases the current instance by the specified value.</summary>
        /// <returns>A new <strong>Azimuth</strong> containing the summed values.</returns>
        /// <example>
        ///     This example adds 45° to the current instance of 45°, returning 90°. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Azimuth1 As New Azimuth(45)
        /// Azimuth1 = Azimuth1.Add(45)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Azimuth Azimuth1 = new Azimuth(45);
        /// Azimuth1 = Azimuth1.Add(45);
        ///     </code>
        /// </example>
        /// <param name="value">A <strong>Double</strong> to add to the current instance.</param>
        public Azimuth Add(double value)
        {
            return new Azimuth(_DecimalDegrees + value);
        }

        public Azimuth Add(Azimuth value)
        {
            return new Azimuth(_DecimalDegrees + value.DecimalDegrees);
        }

        /// <summary>Returns the current instance decreased by one.</summary>
        /// <returns>An <strong>Azimuth</strong> object.</returns>
        /// <remarks>
        /// 	<para>This method decreases the <strong>DecimalDegrees</strong> property by 1.0,
        ///     returned as a new instance.</para>
        /// 	<para><font color="red">Since the <strong>Azimuth</strong> class is immutable, this
        ///     method cannot be used to modify an existing instance.</font></para>
        /// </remarks>
        /// <example>
        ///     This example uses the <strong>Decrement</strong> method to decrease an Azimuth's
        ///     value. It also demonstrates the subtle error which can be caused if
        ///     <strong>Decrement</strong> is called while ignoring the return value.
        ///     <code lang="VB" title="[New Example]">
        /// ' Correct use of Decrement
        /// Dim Azimuth1 As New Azimuth(91)
        /// Azimuth1 = Azimuth1.Decrement()
        ///  
        /// ' Incorrect use of Decrement
        /// Dim Azimuth1 = New Azimuth(91)
        /// Azimuth1.Increment()
        /// ' NOTE: Azimuth1 will still be 91°!
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// // Correct use of Decrement
        /// Azimuth Azimuth1 = new Azimuth(91);
        /// Azimuth1 = Azimuth1.Decrement();
        ///  
        /// // Incorrect use of Decrement
        /// Azimuth Azimuth1 = new Azimuth(91);
        /// Azimuth1.Decrement();
        /// // NOTE: Azimuth1 will still be 91°!
        ///     </code>
        /// </example>
        public Azimuth Decrement()
        {
            return new Azimuth(_DecimalDegrees - 1.0);
        }

        /// <summary>Decreases the current instance by the specified value.</summary>
        /// <returns>A new <strong>Azimuth</strong> containing the new value.</returns>
        /// <example>
        ///     This example subtracts 30° from the current instance of 90°, returning 60°. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Azimuth1 As New Azimuth(90)
        /// Azimuth1 = Azimuth1.Subtract(30)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Azimuth Azimuth1 = new Azimuth(90);
        /// Azimuth1 = Azimuth1.Subtract(30);
        ///     </code>
        /// </example>
        /// <param name="value">A <strong>Double</strong> to subtract from the current instance.</param>
        public Azimuth Subtract(double value)
        {
            return new Azimuth(_DecimalDegrees - value);
        }

        public Azimuth Subtract(Azimuth value)
        {
            return new Azimuth(_DecimalDegrees - value.DecimalDegrees);
        }

        /// <summary>Multiplies the current instance by the specified value.</summary>
        /// <returns>A new <strong>Azimuth</strong> containing the product of the two numbers.</returns>
        /// <example>
        ///     This example multiplies 30° with three, returning 90°. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Azimuth1 As New Azimuth(30)
        /// Azimuth1 = Azimuth1.Multiply(3)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Azimuth Azimuth1 = new Azimuth(30);
        /// Azimuth1 = Azimuth1.Multiply(3);
        ///     </code>
        /// </example>
        /// <param name="value">A <strong>Double</strong> to multiply with the current instance.</param>
        public Azimuth Multiply(double value)
        {
            return new Azimuth(_DecimalDegrees * value);
        }

        public Azimuth Multiply(Azimuth value)
        {
            return new Azimuth(_DecimalDegrees * value.DecimalDegrees);
        }

        /// <summary>Divides the current instance by the specified value.</summary>
        /// <returns>An <strong>Azimuth</strong> containing the new value.</returns>
        /// <example>
        ///     This example divides 90° by three, returning 30°. 
        ///     <code lang="VB" title="[New Example]">
        /// Dim Azimuth1 As New Azimuth(90)
        /// Azimuth1 = Azimuth1.Divide(3)
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// Azimuth Azimuth1 = new Azimuth(90);
        /// Azimuth1 = Azimuth1.Divide(3);
        ///     </code>
        /// </example>
        /// <param name="value">A <strong>Double</strong> representing a denominator to divide by.</param>
        public Azimuth Divide(double value)
        {
            return new Azimuth(_DecimalDegrees / value);
        }

        public Azimuth Divide(Azimuth value)
        {
            return new Azimuth(_DecimalDegrees / value.DecimalDegrees);
        }

        /// <summary>Indicates if the current instance is smaller than the specified value.</summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the current instance is
        /// smaller than the specified value.
        /// </returns>
        /// <param name="value">An <strong>Azimuth</strong> to compare with the current instance.</param>
        public bool IsLessThan(Azimuth value)
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
        /// <param name="value">An <strong>Azimuth</strong> to compare with the current instance.</param>
        public bool IsLessThanOrEqualTo(Azimuth value)
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
        /// <param name="value">An <strong>Azimuth</strong> to compare with the current instance.</param>
        public bool IsGreaterThan(Azimuth value)
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
        /// <param name="value">An <strong>Azimuth</strong> to compare with the current instance.</param>
        public bool IsGreaterThanOrEqualTo(Azimuth value)
        {
            return _DecimalDegrees >= value.DecimalDegrees;
        }

        public bool IsGreaterThanOrEqualTo(double value)
        {
            return _DecimalDegrees >= value;
        }

        #endregion

        #endregion

        #region ICloneable<Azimuth> Members

        /// <summary>Creates a copy of the current instance.</summary>
        /// <returns>An <strong>Azimuth</strong> of the same value as the current instance.</returns>
        public Azimuth Clone()
        {
            return new Azimuth(_DecimalDegrees);
        }

        #endregion

        #region IComparable<Azimuth> Members

        /// <summary>Returns a value indicating the relative order of two objects.</summary>
        /// <returns>A value of -1, 0, or 1 as documented by the IComparable interface.</returns>
        /// <remarks>
        ///		This method allows collections of <strong>Azimuth</strong> objects to be sorted.
        ///		The <see cref="DecimalDegrees">DecimalDegrees</see> property of each instance is compared.
        /// </remarks>
        /// <param name="other">An <strong>Azimuth</strong> object to compare with.</param>
        public int CompareTo(Azimuth other)
        {
            return _DecimalDegrees.CompareTo(other.DecimalDegrees);
        }

        #endregion

        #region IEquatable<Azimuth> Members

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
        /// <seealso cref="Equals(Direction)">Equals Method</seealso>
        /// <example>
        ///     These examples compare two fractional values using specific numbers of digits for
        ///     comparison. 
        ///     <code lang="VB" title="[New Example]">
        /// ' Equals will return False
        /// Dim Azimuth1 As New Azimuth(90.15);
        /// Dim Azimuth2 As New Azimuth(90.12);
        /// If Azimuth1.Equals(Azimuth2, 2) Then
        ///      Debug.WriteLine("The values are the same to two digits of precision.");
        ///  
        /// ' Equals will return True
        /// Dim Azimuth1 As New Azimuth(90.15);
        /// Dim Azimuth2 As New Azimuth(90.12);
        /// If Azimuth1.Equals(Azimuth2, 1) Then
        ///      Debug.WriteLine("The values are the same to one digit of precision.");
        ///     </code>
        /// 	<code lang="CS" title="[New Example]">
        /// // Equals will return False
        /// Azimuth Azimuth1 = new Azimuth(90.15);
        /// Azimuth Azimuth2 = new Azimuth(90.12);
        /// if(Azimuth1.Equals(Azimuth2, 2))
        ///      Console.WriteLine("The values are the same to two digits of precision.");
        ///  
        /// // Equals will return True
        /// Azimuth Azimuth1 = new Azimuth(90.15);
        /// Azimuth Azimuth2 = new Azimuth(90.12);
        /// if(Azimuth1.Equals(Azimuth2, 1))
        ///      Console.WriteLine("The values are the same to one digits of precision.");
        ///     </code>
        /// </example>
        public bool Equals(Azimuth value, int decimals)
        {
            return Equals(value.DecimalDegrees, decimals);
        }

        public bool Equals(Azimuth value)
        {
            return Equals(value.DecimalDegrees);
        }

        #endregion

        #region IEquatable<Direction> Members

        /// <summary>
        /// Compares the current instance to the specified compass direction.
        /// </summary>
        /// <returns>
        /// A <strong>Boolean</strong>, <strong>True</strong> if the current instance's
        /// Direction property matches the specified value.
        /// </returns>
        /// <remarks>
        /// This method is typically used to approximate if two directions are equivalent.
        /// For example, if two objects are traveling at a bearing of 41° and 46°, they both could
        /// be considered to be traveling Northeast even though their bearings are not precisely
        /// the same.
        /// </remarks>
        /// <param name="value">A <strong>Direction</strong> value to compare with the current instance.</param>
        public bool Equals(Direction value)
        {
            return Direction == value;
        }

        #endregion

        #region IFormattable Members

        /// <summary>Outputs the azimuth as a string using the specified format.</summary>
        /// <returns>A <strong>String</strong> in the specified format.</returns>
        /// <remarks>
        /// 	<para>This method returns the current instance output in a specific format. If no
        ///     value for the format is specified, a default format of "cc" is used. Any string
        ///     output by this method can be converted back into an Azimuth object using the
        ///     <strong>Parse</strong> method or <strong>Azimuth(string)</strong>
        ///     constructor.</para>
        /// </remarks>
        /// <seealso cref="ToString()">ToString Method</seealso>
        /// <seealso cref="Parse(string)">Parse Method</seealso>
        /// <example>
        ///     This example uses the <strong>ToString</strong> method to output an azimuth in a
        ///     custom format. The " <strong>d.dd</strong> " code represents decimal degrees
        ///     rounded to two digits, and " <strong>cc</strong> " represents the direction in
        ///     verbose form. 
        ///     <code lang="VB">
        /// Dim MyAzimuth As New Azimuth(90.946)
        /// Debug.WriteLine(MyAzimuth.ToString("d.dd (cc)", CultureInfo.CurrentCulture))
        /// ' Output: 90.95 (East)
        ///     </code>
        /// 	<code lang="CS">
        /// Azimuth MyAzimuth = new Azimuth(90.946);
        /// Console.WriteLine(MyAzimuth.ToString("d.dd (cc)", CultureInfo.CurrentCulture));
        /// // Output: 90.95 (East)
        ///     </code>
        /// </example>
        /// <param name="format">
        /// 	<para>A <strong>String</strong> consisting of any number of the following
        ///     codes:</para>
        /// 	<para>
        /// 		<list type="table">
        /// 			<item>
        /// 				<term><strong>c</strong></term>
        /// 				<description>The object is output as an abbreviated direction.
        ///                 <strong>N</strong>, <strong>NE</strong>,
        ///                 <strong>NNW</strong></description>
        /// 			</item>
        /// 			<item>
        /// 				<term><strong>cc</strong></term>
        /// 				<description>The object is output as a full direction.
        ///                 <strong>North</strong>, <strong>Northeast</strong>,
        ///                 <strong>North-Northwest</strong></description>
        /// 			</item>
        /// 			<item>
        /// 				<term><strong>d</strong></term>
        /// 				<description>Represents one digit from the
        ///                 <strong>DecimalDegrees</strong> property.</description>
        /// 			</item>
        /// 			<item>
        /// 				<term><strong>h</strong></term>
        /// 				<description>Represents one digit from the
        ///                 <strong>Hours</strong> property.</description>
        /// 			</item>
        /// 			<item>
        /// 				<term><strong>m</strong></term>
        /// 				<description>Represents one digit from the <strong>Minutes</strong>
        ///                 property.</description>
        /// 			</item>
        /// 			<item>
        /// 				<term><strong>s</strong></term>
        /// 				<description>Represents one digit from the <strong>Seconds</strong>
        ///                 property.</description>
        /// 			</item>
        /// 		</list>
        /// 	</para>
        /// </param>
        /// <param name="formatProvider">
        /// A <strong>CultureInfo</strong> object used to properly format numeric
        /// information.
        /// </param>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider;

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            if (format == null || format.Length == 0)
                format = "G";

            // Convert to upper case
            format = format.ToUpper(culture);
            // Use a default format
            if (String.Compare(format, "G", false, culture) == 0)
                format = "CC";

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
                // Prevent null values
                if (format == null) format = "g";
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
                    throw new ArgumentException(Properties.Resources.Angle_OnlyRightmostIsDecimal);
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
                        format = format.Replace(SubFormat, DecimalDegrees.ToString(NewFormat, culture));
                    }
                    else
                    {
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
                            throw new ArgumentException(Properties.Resources.Angle_OnlyRightmostIsDecimal);
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
                            throw new ArgumentException(Properties.Resources.Angle_OnlyRightmostIsDecimal);
                        }
                        IsDecimalHandled = true;
                        format = format.Replace(SubFormat, Seconds.ToString(NewFormat, culture));
                    }
                    else
                    {
                        format = format.Replace(SubFormat, Seconds.ToString(NewFormat, culture));
                    }
                }
                // If nothing then return zero
                if (String.Compare(format, "°", true, culture) == 0)
                    return "0°";

                // Is there an hours specifier?
                StartChar = format.IndexOf("C");
                if (StartChar > -1)
                {
                    // Yes. Look for subsequent H characters or a period
                    EndChar = format.LastIndexOf("C");
                    // Extract the sub-string
                    SubFormat = format.Substring(StartChar, EndChar - StartChar + 1);
                    // How many characters is this?
                    switch (SubFormat.Length)
                    {
                        case 0:
                            // Do nothing
                            break;
                        case 1:
                            switch (Direction)
                            {
                                case Direction.North:
                                    format = format.Replace(SubFormat, "N");
                                    break;
                                case Direction.NorthNortheast:
                                    format = format.Replace(SubFormat, "NNE");
                                    break;
                                case Direction.Northeast:
                                    format = format.Replace(SubFormat, "NE");
                                    break;
                                case Direction.EastNortheast:
                                    format = format.Replace(SubFormat, "ENE");
                                    break;
                                case Direction.East:
                                    format = format.Replace(SubFormat, "E");
                                    break;
                                case Direction.EastSoutheast:
                                    format = format.Replace(SubFormat, "ESE");
                                    break;
                                case Direction.Southeast:
                                    format = format.Replace(SubFormat, "SE");
                                    break;
                                case Direction.SouthSoutheast:
                                    format = format.Replace(SubFormat, "SSE");
                                    break;
                                case Direction.South:
                                    format = format.Replace(SubFormat, "S");
                                    break;
                                case Direction.SouthSouthwest:
                                    format = format.Replace(SubFormat, "SSW");
                                    break;
                                case Direction.Southwest:
                                    format = format.Replace(SubFormat, "SW");
                                    break;
                                case Direction.WestSouthwest:
                                    format = format.Replace(SubFormat, "WSW");
                                    break;
                                case Direction.West:
                                    format = format.Replace(SubFormat, "W");
                                    break;
                                case Direction.WestNorthwest:
                                    format = format.Replace(SubFormat, "WNW");
                                    break;
                                case Direction.Northwest:
                                    format = format.Replace(SubFormat, "NW");
                                    break;
                                case Direction.NorthNorthwest:
                                    format = format.Replace(SubFormat, "NNW");
                                    break;
                            }
                            break;
                        default:
                            switch (Direction)
                            {
                                case Direction.North:
                                    format = format.Replace(SubFormat, "North");
                                    break;
                                case Direction.NorthNortheast:
                                    format = format.Replace(SubFormat, "North-Northeast");
                                    break;
                                case Direction.Northeast:
                                    format = format.Replace(SubFormat, "Northeast");
                                    break;
                                case Direction.EastNortheast:
                                    format = format.Replace(SubFormat, "East-Northeast");
                                    break;
                                case Direction.East:
                                    format = format.Replace(SubFormat, "East");
                                    break;
                                case Direction.EastSoutheast:
                                    format = format.Replace(SubFormat, "East-Southeast");
                                    break;
                                case Direction.Southeast:
                                    format = format.Replace(SubFormat, "Southeast");
                                    break;
                                case Direction.SouthSoutheast:
                                    format = format.Replace(SubFormat, "South-Southeast");
                                    break;
                                case Direction.South:
                                    format = format.Replace(SubFormat, "South");
                                    break;
                                case Direction.SouthSouthwest:
                                    format = format.Replace(SubFormat, "South-Southwest");
                                    break;
                                case Direction.Southwest:
                                    format = format.Replace(SubFormat, "Southwest");
                                    break;
                                case Direction.WestSouthwest:
                                    format = format.Replace(SubFormat, "West-Southwest");
                                    break;
                                case Direction.West:
                                    format = format.Replace(SubFormat, "West");
                                    break;
                                case Direction.WestNorthwest:
                                    format = format.Replace(SubFormat, "West-Northwest");
                                    break;
                                case Direction.Northwest:
                                    format = format.Replace(SubFormat, "Northwest");
                                    break;
                                case Direction.NorthNorthwest:
                                    format = format.Replace(SubFormat, "North-Northwest");
                                    break;
                            }
                            break;
                    }
                }
                // All done
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

    /// <summary>Represents an approximate direction of motion.</summary>
    /// <remarks>
    /// This enumeration is used primarily by the <strong>Azimuth</strong> class when
    /// converting a numeric angle measurement into a compass direction.
    /// </remarks>
    /// <example>
    ///     This example outputs the direction associated 272Â°, which is <strong>West</strong>
    ///     . 
    ///     <code lang="VB" title="[New Example]">
    /// Dim MyAzimuth As New Azimuth(272)
    /// Debug.WriteLine(MyAzimuth.Direction.ToString())
    /// ' Output: West
    ///     </code>
    /// 	<code lang="CS" title="[New Example]">
    /// Azimuth MyAzimuth = new Azimuth(272);
    /// Console.WriteLine(MyAzimuth.Direction.ToString());
    /// // Output: West
    ///     </code>
    /// </example>
    public enum Direction : int
    {
        /// <summary>An azimuth of approximately 0°</summary>
        North,
        /// <summary>Between north and northeast</summary>
        NorthNortheast,
        /// <summary>Between north and east</summary>
        Northeast,
        /// <summary>Between east and northeast</summary>
        EastNortheast,
        /// <summary>An azimuth of approximately 90°</summary>
        East,
        /// <summary>Between east and southeast</summary>
        EastSoutheast,
        /// <summary>Between south and east</summary>
        Southeast,
        /// <summary>Between south and southeast</summary>
        SouthSoutheast,
        /// <summary>An azimuth of approximately 180°</summary>
        South,
        /// <summary>Between south and southwest</summary>
        SouthSouthwest,
        /// <summary>Between south and west</summary>
        Southwest,
        /// <summary>Between west and southwest</summary>
        WestSouthwest,
        /// <summary>An azimuth of approximately 270°</summary>
        West,
        /// <summary>Between west and northwest</summary>
        WestNorthwest,
        /// <summary>Between north and west</summary>
        Northwest,
        /// <summary>Between north and northwest</summary>
        NorthNorthwest
    }
}
