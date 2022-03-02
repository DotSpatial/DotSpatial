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
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents a position on Earth marked by latitude, longitude, and altitude.
    /// </summary>
    /// <remarks>Instances of this class are guaranteed to be thread-safe because the class is
    /// immutable (its properties can only be changed via constructors).</remarks>
    public struct Position3D : IFormattable, IEquatable<Position3D>, ICloneable<Position3D>, IXmlSerializable
    {
        /// <summary>
        ///
        /// </summary>
        private Position _position;
        /// <summary>
        ///
        /// </summary>
        private Distance _altitude;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Position3D"/> struct.
        /// </summary>
        /// <param name="altitude">The altitude.</param>
        /// <param name="location">The location.</param>
        public Position3D(Distance altitude, Position location)
        {
            _position = location;
            _altitude = altitude;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Position3D"/> struct.
        /// </summary>
        /// <param name="altitude">The altitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="latitude">The latitude.</param>
        public Position3D(Distance altitude, Longitude longitude, Latitude latitude)
        {
            _position = new Position(longitude, latitude);
            _altitude = altitude;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Position3D"/> struct.
        /// </summary>
        /// <param name="altitude">The altitude.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <overloads>Creates a new instance.</overloads>
        public Position3D(Distance altitude, Latitude latitude, Longitude longitude)
        {
            _position = new Position(latitude, longitude);
            _altitude = altitude;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Position3D"/> struct.
        /// </summary>
        /// <param name="longitude">The longitude.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="altitude">The altitude.</param>
        public Position3D(Longitude longitude, Latitude latitude, Distance altitude)
        {
            _position = new Position(latitude, longitude);
            _altitude = altitude;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Position3D"/> struct.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="altitude">The altitude.</param>
        public Position3D(Latitude latitude, Longitude longitude, Distance altitude)
        {
            _position = new Position(latitude, longitude);
            _altitude = altitude;
        }

        /// <summary>
        /// Creates a new instance by parsing latitude and longitude from a single string.
        /// </summary>
        /// <param name="altitude">The altitude.</param>
        /// <param name="location">The location.</param>
        public Position3D(string altitude, string location)
            : this(altitude, location, CultureInfo.CurrentCulture)
        { }

        /// <summary>
        /// Creates a new instance by interpreting the specified latitude and longitude.
        /// </summary>
        /// <param name="altitude">The altitude.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <remarks>Latitude and longitude values are parsed using the current local culture.  For better support
        /// of international cultures, add a CultureInfo parameter.</remarks>
        public Position3D(string altitude, string latitude, string longitude)
            : this(altitude, latitude, longitude, CultureInfo.CurrentCulture)
        { }

        /// <summary>
        /// Creates a new instance by interpreting the specified latitude and longitude.
        /// </summary>
        /// <param name="altitude">The altitude.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="culture">The culture.</param>
        /// <remarks>Latitude and longitude values are parsed using the current local culture.  For better support
        /// of international cultures, a CultureInfo parameter should be specified to indicate how numbers should
        /// be parsed.</remarks>
        public Position3D(string altitude, string latitude, string longitude, CultureInfo culture)
        {
            _position = new Position(latitude, longitude, culture);
            _altitude = new Distance(altitude, culture);
        }

        /// <summary>
        /// Creates a new instance by converting the specified string using the specific culture.
        /// </summary>
        /// <param name="altitude">The altitude.</param>
        /// <param name="location">The location.</param>
        /// <param name="culture">The culture.</param>
        public Position3D(string altitude, string location, CultureInfo culture)
        {
            _position = new Position(location, culture);
            _altitude = new Distance(altitude, culture);
        }

        /// <summary>
        /// Upgrades a Position object to a Position3D object.
        /// </summary>
        /// <param name="position">The position.</param>
        public Position3D(Position position)
        {
            _position = position;
            _altitude = Distance.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Position3D"/> struct.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public Position3D(XmlReader reader)
        {
            // Initialize all fields
            _position = Position.Invalid;
            _altitude = Distance.Invalid;

            // Deserialize the object from XML
            ReadXml(reader);
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Returns the location's distance above sea level.
        /// </summary>
        public Distance Altitude
        {
            get { return _altitude; }
        }

        /// <summary>
        /// Gets the latitude.
        /// </summary>
        public Latitude Latitude
        {
            get { return _position.Latitude; }
        }

        /// <summary>
        /// Gets the longitude.
        /// </summary>
        public Longitude Longitude
        {
            get { return _position.Longitude; }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Toes the cartesian point.
        /// </summary>
        /// <returns></returns>
        public CartesianPoint ToCartesianPoint()
        {
            return ToCartesianPoint(Ellipsoid.Default);
        }

        /// <summary>
        /// Toes the cartesian point.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns></returns>
        public CartesianPoint ToCartesianPoint(Ellipsoid ellipsoid)
        {
            return _position.ToCartesianPoint(ellipsoid, _altitude);
        }

        #endregion Public Methods

        #region Operators

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Position3D left, Position3D right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Position3D left, Position3D right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Position3D operator +(Position3D left, Position3D right)
        {
            return left.Add(right);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Position3D operator -(Position3D left, Position3D right)
        {
            return left.Subtract(right);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Position3D operator *(Position3D left, Position3D right)
        {
            return left.Multiply(right);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Position3D operator /(Position3D left, Position3D right)
        {
            return left.Divide(right);
        }

        /// <summary>
        /// Adds the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public Position3D Add(Position3D position)
        {
            return
                new Position3D(Latitude.Add(position.Latitude.DecimalDegrees),
                Longitude.Add(position.Longitude.DecimalDegrees),
                _altitude.Add(position.Altitude));
        }

        /// <summary>
        /// Subtracts the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public Position3D Subtract(Position3D position)
        {
            return
                new Position3D(Latitude.Subtract(position.Latitude.DecimalDegrees),
                Longitude.Subtract(position.Longitude.DecimalDegrees),
                _altitude.Subtract(position.Altitude));
        }

        /// <summary>
        /// Multiplies the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public Position3D Multiply(Position3D position)
        {
            return
                new Position3D(Latitude.Multiply(position.Latitude.DecimalDegrees),
                Longitude.Multiply(position.Longitude.DecimalDegrees),
                _altitude.Multiply(position.Altitude));
        }

        /// <summary>
        /// Divides the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public Position3D Divide(Position3D position)
        {
            return
                new Position3D(Latitude.Divide(position.Latitude.DecimalDegrees),
                Longitude.Divide(position.Longitude.DecimalDegrees),
                _altitude.Divide(position.Altitude));
        }

        #endregion Operators

        /// <summary>
        /// Returns whether the latitude, longitude and altitude are zero.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _altitude.IsEmpty && _position.IsEmpty;
            }
        }

        #region Overrides

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return _position.GetHashCode() ^ _altitude.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Position3D)
                return Equals((Position3D)obj);
            return false;
        }

        #endregion Overrides

        #region Conversions

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.CartesianPoint"/> to <see cref="DotSpatial.Positioning.Position3D"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Position3D(CartesianPoint value)
        {
            return value.ToPosition3D();
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Position3D"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator string(Position3D value)
        {
            return value.ToString();
        }

        #endregion Conversions

        #region IXmlSerializable

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
            /* The position class uses the GML 3.0 specification for XML.
             *
             * <gml:pos>X Y</gml:pos>
             *
             */
            writer.WriteStartElement(Xml.GML_XML_PREFIX, "pos", Xml.GML_XML_NAMESPACE);
            writer.WriteString(Longitude.DecimalDegrees.ToString("G17", CultureInfo.InvariantCulture));
            writer.WriteString(" ");
            writer.WriteString(Latitude.DecimalDegrees.ToString("G17", CultureInfo.InvariantCulture));
            writer.WriteString(" ");
            writer.WriteString(Altitude.ToMeters().Value.ToString("G17", CultureInfo.InvariantCulture));
            writer.WriteEndElement();
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            /* The position class uses the GML 3.0 specification for XML.
             *
             * <gml:pos>X Y</gml:pos>
             *
             * ... but it is also helpful to be able to READ older versions
             * of GML, such as this one for GML 2.0:
             *
             * <gml:coord>
             *      <gml:X>double</gml:X>
             *      <gml:Y>double</gml:Y>  // optional
             *      <gml:Z>double</gml:Z>  // optional
             * </gml:coord>
             *
             */

            // .NET Complains if we don't assign values
            _position = Position.Empty;
            _altitude = Distance.Empty;
            Longitude longitude = Longitude.Empty;
            Latitude latitude = Latitude.Empty;

            // Move to the <gml:pos> or <gml:coord> element
            if (!reader.IsStartElement("pos", Xml.GML_XML_NAMESPACE)
                && !reader.IsStartElement("coord", Xml.GML_XML_NAMESPACE))
                reader.ReadStartElement();

            switch (reader.LocalName.ToLower(CultureInfo.InvariantCulture))
            {
                case "pos":
                    // Read the "X Y" string, then split by the space between them
                    string[] values = reader.ReadElementContentAsString().Split(' ');
                    // Deserialize the longitude
                    longitude = new Longitude(values[0], CultureInfo.InvariantCulture);

                    // Deserialize the latitude
                    if (values.Length >= 2)
                        latitude = new Latitude(values[1], CultureInfo.InvariantCulture);

                    // Deserialize the altitude
                    if (values.Length == 3)
                        _altitude = Distance.FromMeters(double.Parse(values[2], CultureInfo.InvariantCulture));

                    // Make the position
                    _position = new Position(latitude, longitude);
                    break;
                case "coord":
                    // Read the <gml:coord> start tag
                    reader.ReadStartElement();

                    // Now read up to 3 elements: X, and optionally Y or Z
                    for (int index = 0; index < 3; index++)
                    {
                        switch (reader.LocalName.ToLower(CultureInfo.InvariantCulture))
                        {
                            case "x":
                                longitude = new Longitude(reader.ReadElementContentAsDouble());
                                break;
                            case "y":
                                latitude = new Latitude(reader.ReadElementContentAsDouble());
                                break;
                            case "z":
                                // Read Z as meters (there's no unit type in the spec :P morons)
                                _altitude = Distance.FromMeters(reader.ReadElementContentAsDouble());
                                break;
                        }

                        // If we're at an end element, stop
                        if (reader.NodeType == XmlNodeType.EndElement)
                            break;
                    }

                    // Make the position
                    _position = new Position(latitude, longitude);

                    // Read the </gml:coord> end tag
                    reader.ReadEndElement();
                    break;
            }

            reader.Read();
        }

        #endregion IXmlSerializable

        #region IEquatable<Position3D>

        /// <summary>
        /// Compares the current instance to the specified position.
        /// </summary>
        /// <param name="other">A <strong>Position</strong> object to compare with.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the values are identical.</returns>
        /// <remarks>The two objects are compared at up to four digits of precision.</remarks>
        public bool Equals(Position3D other)
        {
            return Latitude.Equals(other.Latitude)
                && Longitude.Equals(other.Longitude)
                && _altitude.Equals(other.Altitude);
        }

        /// <summary>
        /// Compares the current instance to the specified position using the specified numeric precision.
        /// </summary>
        /// <param name="other">A <strong>Position</strong> object to compare with.</param>
        /// <param name="decimals">An <strong>Integer</strong> specifying the number of fractional digits to compare.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the values are identical.</returns>
        /// <remarks>This method is typically used when positions do not mark the same location unless they are
        /// extremely close to one another.  Conversely, a low or even negative value for <strong>Precision</strong>
        /// allows positions to be considered equal even when they do not precisely match.</remarks>
        public bool Equals(Position3D other, int decimals)
        {
            return Latitude.Equals(other.Latitude, decimals)
                && Longitude.Equals(other.Longitude, decimals)
                && _altitude.Equals(other.Altitude, decimals);
        }

        #endregion IEquatable<Position3D>

        #region IFormattable Members

        /// <summary>
        /// Outputs the current instance as a string using the specified format and culture information.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider ?? CultureInfo.CurrentCulture;

            if (string.IsNullOrEmpty(format))
                format = "G";

            // Output as latitude and longitude
            return _position.ToString(format, culture)
                + culture.TextInfo.ListSeparator
                + _altitude.ToString(format, culture);
        }

        /// <summary>
        /// Returns a coordinate which has been shifted the specified bearing and distance.
        /// </summary>
        /// <param name="bearing">The bearing.</param>
        /// <param name="distance">The distance.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns></returns>
        public Position3D TranslateTo(Azimuth bearing, Distance distance, Ellipsoid ellipsoid)
        {
            return new Position3D(_altitude, _position.TranslateTo(bearing, distance, ellipsoid));
        }

        #endregion IFormattable Members

        #region ICloneable<Position3D> Members

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public Position3D Clone()
        {
            return new Position3D(_position);
        }

        #endregion ICloneable<Position3D> Members
    }
}