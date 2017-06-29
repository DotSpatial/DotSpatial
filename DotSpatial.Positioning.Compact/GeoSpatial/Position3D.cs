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
	/// <remarks>
	/// 	<para>Instances of this class are guaranteed to be thread-safe because the class is
	///     immutable (its properties can only be changed via constructors).</para>
	/// </remarks>
    public struct Position3D : IFormattable, IEquatable<Position3D>, ICloneable<Position3D>, IXmlSerializable
    {
        private Position _Position;
        private Distance _Altitude;

		#region  Constructors 
        
        public Position3D(Distance altitude, Position location)
        {
            _Position = location;
            _Altitude = altitude;
        }

		public Position3D(Distance altitude, Longitude longitude, Latitude latitude) 	
		{
            _Position = new Position(longitude, latitude);
			_Altitude = altitude;
		}

		/// <overloads>Creates a new instance.</overloads>
		public Position3D(Distance altitude, Latitude latitude, Longitude longitude)
		{
            _Position = new Position(latitude, longitude);
		    _Altitude = altitude;			
        }

        public Position3D(Longitude longitude, Latitude latitude, Distance altitude)
        {
            _Position = new Position(latitude, longitude);
            _Altitude = altitude;
        }

        public Position3D(Latitude latitude, Longitude longitude, Distance altitude)
        {
            _Position = new Position(latitude, longitude);
            _Altitude = altitude;
        }
        
        /// <summary>
		/// Creates a new instance by parsing latitude and longitude from a single string.
		/// </summary>
		/// <param name="location">A <strong>String</strong> containing both a latitude and longitude to parse.</param>
        /// <param name="altitude">A <strong>String</strong> containing an altitude to parse.</param>
		public Position3D(string altitude, string location) 
            : this(altitude, location, CultureInfo.CurrentCulture)
		{ }

        /// <summary>
        /// Creates a new instance by interpreting the specified latitude and longitude.
        /// </summary>
        /// <param name="latitude">A <strong>String</strong> describing a latitude in the current culture.</param>
        /// <param name="longitude">A <strong>String</strong> describing a longitude in the current culture.</param>
        /// <param name="altitude">A <strong>String</strong> containing an altitude to parse.</param>
        /// <remarks>Latitude and longitude values are parsed using the current local culture.  For better support
        /// of international cultures, add a CultureInfo parameter.</remarks>
		public Position3D(string altitude, string latitude, string longitude) 
            : this(altitude, latitude, longitude, CultureInfo.CurrentCulture)
		{ }

        /// <summary>
        /// Creates a new instance by interpreting the specified latitude and longitude.
        /// </summary>
        /// <param name="latitude">A <strong>String</strong> describing a latitude in the current culture.</param>
        /// <param name="longitude">A <strong>String</strong> describing a longitude in the current culture.</param>
        /// <param name="altitude">A <strong>String</strong> containing an altitude to parse.</param>
        /// <param name="culture">A <strong>CultureInfo</strong> describing the specific culture</param>
        /// <remarks>Latitude and longitude values are parsed using the current local culture.  For better support
        /// of international cultures, a CultureInfo parameter should be specified to indicate how numbers should
        /// be parsed.</remarks>
        public Position3D(string altitude, string latitude, string longitude, CultureInfo culture)
        {
            _Position = new Position(latitude, longitude, culture);
            _Altitude = new Distance(altitude, culture);
        }

        /// <summary>
        /// Creates a new instance by converting the specified string using the specific culture.
        /// </summary>
        /// <param name="altitude">A <strong>String</strong> containing an altitude to parse.</param>
        /// <param name="location">A <strong>String</strong> containing both a latitude and longitude to parse.</param>
        /// <param name="culture">A <strong>CultureInfo</strong> describing the specific culture</param>
        public Position3D(string altitude, string location, CultureInfo culture)
        {
            _Position = new Position(location, culture);
            _Altitude = new Distance(altitude, culture);
        }

        /// <summary>
        /// Upgrades a Position object to a Position3D object.
        /// </summary>
        /// <param name="position"></param>
        public Position3D(Position position)
        {
            _Position = position;
            _Altitude = Distance.Empty;
        }

        public Position3D(XmlReader reader)
        {
            // Initialize all fields
            _Position = Position.Invalid;
            _Altitude = Distance.Invalid;

            // Deserialize the object from XML
            ReadXml(reader);
        }

		#endregion

        #region Public Properties

        /// <summary>Returns the location's distance above sea level.</summary>
        public Distance Altitude
        {
            get { return _Altitude; }
        }

        public Latitude Latitude
        {
            get { return _Position.Latitude;  }
        }

        public Longitude Longitude
        {
            get { return _Position.Longitude; }
        }

        #endregion

        #region Public Methods
        
        public CartesianPoint ToCartesianPoint()
        {
            return ToCartesianPoint(Ellipsoid.Default);
        }

        public CartesianPoint ToCartesianPoint(Ellipsoid ellipsoid)
        {
            return _Position.ToCartesianPoint(ellipsoid, _Altitude);
        }

        #endregion

        #region Operators

        public static bool operator ==(Position3D left, Position3D right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Position3D left, Position3D right)
        {
            return !left.Equals(right);
        }

        public static Position3D operator +(Position3D left, Position3D right)
        {
            return left.Add(right);
        }

        public static Position3D operator -(Position3D left, Position3D right)
        {
            return left.Subtract(right);
        }

        public static Position3D operator *(Position3D left, Position3D right)
        {
            return left.Multiply(right);
        }

        public static Position3D operator /(Position3D left, Position3D right)
        {
            return left.Divide(right);
        }

        public Position3D Add(Position3D position)
        {
            return
                new Position3D(this.Latitude.Add(position.Latitude.DecimalDegrees),
                this.Longitude.Add(position.Longitude.DecimalDegrees),
                this._Altitude.Add(position.Altitude));
        }
        public Position3D Subtract(Position3D position)
        {
            return
                new Position3D(this.Latitude.Subtract(position.Latitude.DecimalDegrees),
                this.Longitude.Subtract(position.Longitude.DecimalDegrees),
                this._Altitude.Subtract(position.Altitude));
        }
        public Position3D Multiply(Position3D position)
        {
            return
                new Position3D(this.Latitude.Multiply(position.Latitude.DecimalDegrees),
                this.Longitude.Multiply(position.Longitude.DecimalDegrees),
                this._Altitude.Multiply(position.Altitude));
        }
        public Position3D Divide(Position3D position)
        {
            return
                new Position3D(this.Latitude.Divide(position.Latitude.DecimalDegrees),
                this.Longitude.Divide(position.Longitude.DecimalDegrees),
                this._Altitude.Divide(position.Altitude));
        }

        #endregion



        /// <summary>
        /// Returns whether the latitude, longitude and altitude are zero.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _Altitude.IsEmpty && _Position.IsEmpty;
            }
        }

        #region Overrides

        public override int GetHashCode()
        {
            return _Position.GetHashCode() ^ _Altitude.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is Position3D)
                return Equals((Position3D)obj);
            return false;
        }

        #endregion

        #region Conversions

        public static explicit operator Position3D(CartesianPoint value)
        {
            return value.ToPosition3D();
        }

        public static explicit operator string(Position3D value)
        {
            return value.ToString();
        }

        #endregion

        #region IXmlSerializable

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            /* The position class uses the GML 3.0 specification for XML.
             * 
             * <gml:pos>X Y</gml:pos>
             *
             */
            writer.WriteStartElement(Xml.GmlXmlPrefix, "pos", Xml.GmlXmlNamespace);
            writer.WriteString(Longitude.DecimalDegrees.ToString("G17", CultureInfo.InvariantCulture));
            writer.WriteString(" ");
            writer.WriteString(Latitude.DecimalDegrees.ToString("G17", CultureInfo.InvariantCulture));
            writer.WriteString(" ");
            writer.WriteString(Altitude.ToMeters().Value.ToString("G17", CultureInfo.InvariantCulture));
            writer.WriteEndElement();
        }

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
            _Position = Position.Empty;
            _Altitude = Distance.Empty;
            Longitude longitude = Longitude.Empty;
            Latitude latitude = Latitude.Empty;

            // Move to the <gml:pos> or <gml:coord> element
            if (!reader.IsStartElement("pos", Xml.GmlXmlNamespace)
                && !reader.IsStartElement("coord", Xml.GmlXmlNamespace))
                reader.ReadStartElement();

            switch (reader.LocalName.ToLower(CultureInfo.InvariantCulture))
            {
                case "pos":
                    // Read the "X Y" string, then split by the space between them
                    string[] Values = reader.ReadElementContentAsString().Split(' ');
                    // Deserialize the longitude
                    longitude = new Longitude(Values[0], CultureInfo.InvariantCulture);

                    // Deserialize the latitude
                    if (Values.Length >= 2)
                        latitude = new Latitude(Values[1], CultureInfo.InvariantCulture);

                    // Deserialize the altitude
                    if (Values.Length == 3)
                        _Altitude = Distance.FromMeters(double.Parse(Values[2], CultureInfo.InvariantCulture));

                    // Make the position
                    _Position = new Position(latitude, longitude);
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
                                _Altitude = Distance.FromMeters(reader.ReadElementContentAsDouble());
                                break;
                        }

                        // If we're at an end element, stop
                        if (reader.NodeType == XmlNodeType.EndElement)
                            break;
                    }

                    // Make the position
                    _Position = new Position(latitude, longitude);

                    // Read the </gml:coord> end tag
                    reader.ReadEndElement();
                    break;
            }
        }

        #endregion

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
                && _Altitude.Equals(other.Altitude);
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
                && _Altitude.Equals(other.Altitude, decimals);
        }

        #endregion

        #region IFormattable Members

        /// <summary>
        /// Outputs the current instance as a string using the specified format and culture information.
        /// </summary>
        /// <returns></returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider;

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            if (format == null || format.Length == 0)
                format = "G";

            // Output as latitude and longitude
            return _Position.ToString(format, culture)
                + culture.TextInfo.ListSeparator
                + _Altitude.ToString(format, culture);
        }

        /// <summary>
        /// Returns a coordinate which has been shifted the specified bearing and distance.
        /// </summary>
        /// <param name="bearing"></param>
        /// <param name="distance"></param>
        /// <param name="ellipsoid"></param>
        /// <returns></returns>
        public Position3D TranslateTo(Azimuth bearing, Distance distance, Ellipsoid ellipsoid)
        {
            return new Position3D(_Altitude, _Position.TranslateTo(bearing, distance, ellipsoid));
        }

        #endregion

        #region ICloneable<Position3D> Members

        public Position3D Clone()
        {
            return new Position3D(_Position);
        }

        #endregion
    }
}
