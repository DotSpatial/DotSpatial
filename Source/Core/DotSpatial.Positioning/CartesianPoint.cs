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
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DotSpatial.Positioning
{
#if !PocketPC || DesignTime
    /// <summary>
    /// Represents an Earth-centered, Earth-fixed (ECEF) Cartesian coordinate.
    /// </summary>
    [TypeConverter("DotSpatial.Positioning.Design.CartesianPointConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=1.0.0.0, PublicKeyToken=b4b0b185210c9dae")]
#endif
    public struct CartesianPoint : IFormattable, IEquatable<CartesianPoint>, IXmlSerializable
    {
        /// <summary>
        ///
        /// </summary>
        private Distance _x;
        /// <summary>
        ///
        /// </summary>
        private Distance _y;
        /// <summary>
        ///
        /// </summary>
        private Distance _z;

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified X, Y and Z values.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        public CartesianPoint(Distance x, Distance y, Distance z)
        {
            _x = x.ToMeters();
            _y = y.ToMeters();
            _z = z.ToMeters();
        }

        /// <summary>
        /// Creates a new instance from the specified block of GML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public CartesianPoint(XmlReader reader)
        {
            // Initialize all fields
            _x = Distance.Invalid;
            _y = Distance.Invalid;
            _z = Distance.Invalid;

            // Deserialize the object from XML
            ReadXml(reader);
        }

        #endregion Constructors

        #region Fields

        /// <summary>
        /// Returns a cartesian coordinate with empty values.
        /// </summary>
        public static readonly CartesianPoint Empty = new(Distance.Empty, Distance.Empty, Distance.Empty);
        /// <summary>
        /// Returns a cartesian point with infinite values.
        /// </summary>
        public static readonly CartesianPoint Infinity = new(Distance.Infinity, Distance.Infinity, Distance.Infinity);
        /// <summary>
        /// Represents an invalid or unspecified value.
        /// </summary>
        public static readonly CartesianPoint Invalid = new(Distance.Invalid, Distance.Invalid, Distance.Invalid);

        #endregion Fields

        #region Public Properties

        /// <summary>
        /// Returns the horizontal (longitude) portion of a Cartesian coordinate.
        /// </summary>
        public Distance X
        {
            get
            {
                return _x;
            }
        }

        /// <summary>
        /// Returns the vertical (latitude) portion of a Cartesian coordinate.
        /// </summary>
        public Distance Y
        {
            get
            {
                return _y;
            }
        }

        /// <summary>
        /// Returns the altitude portion of a Cartesian coordinate.
        /// </summary>
        public Distance Z
        {
            get
            {
                return _z;
            }
        }

        /// <summary>
        /// Indicates whether the current instance has no value.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _x.IsEmpty && _y.IsEmpty && _z.IsEmpty;
            }
        }

        /// <summary>
        /// Indicates whether the current instance is invalid or unspecified.
        /// </summary>
        public bool IsInvalid
        {
            get
            {
                return _x.IsInvalid && _y.IsInvalid && _z.IsInvalid;
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Converts the current instance to a geodetic (latitude/longitude) coordinate.
        /// </summary>
        /// <returns>A <strong>Position</strong> object containing the converted result.</returns>
        /// <remarks>The conversion formula will convert the Cartesian coordinate to
        /// latitude and longitude using the WGS1984 ellipsoid (the default ellipsoid for
        /// GPS coordinates).</remarks>
        public Position3D ToPosition3D()
        {
            return ToPosition3D(Ellipsoid.Wgs1984);
        }

        /// <summary>
        /// Converts the current instance to a geodetic (latitude/longitude) coordinate using the specified ellipsoid.
        /// </summary>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>A <strong>Position</strong> object containing the converted result.</returns>
        /// <remarks>The conversion formula will convert the Cartesian coordinate to
        /// latitude and longitude using the WGS1984 ellipsoid (the default ellipsoid for
        /// GPS coordinates).  The resulting three-dimensional coordinate is accurate to within two millimeters
        /// (2 mm).</remarks>
        public Position3D ToPosition3D(Ellipsoid ellipsoid)
        {
            if (ellipsoid == null)
                throw new ArgumentNullException("ellipsoid");

            #region New code

            /*
             * % ECEF2LLA - convert earth-centered earth-fixed (ECEF)
%            cartesian coordinates to latitude, longitude,
%            and altitude
%
% USAGE:
% [lat, lon, alt] = ecef2lla(x, y, z)
%
% lat = geodetic latitude (radians)
% lon = longitude (radians)
% alt = height above WGS84 ellipsoid (m)
% x = ECEF X-coordinate (m)
% y = ECEF Y-coordinate (m)
% z = ECEF Z-coordinate (m)
%
% Notes: (1) This function assumes the WGS84 model.
%        (2) Latitude is customary geodetic (not geocentric).
%        (3) Inputs may be scalars, vectors, or matrices of the same
%            size and shape. Outputs will have that same size and shape.
%        (4) Tested but no warranty; use at your own risk.
%        (5) Michael Kleder, April 2006

function [lat, lon, alt] = ecef2lla(x, y, z)

% WGS84 ellipsoid constants:
a = 6378137;
e = 8.1819190842622e-2;

% calculations:
b   = sqrt(a^2*(1-e^2));
ep  = sqrt((a^2-b^2)/b^2);
p   = sqrt(x.^2+y.^2);
th  = atan2(a*z, b*p);
lon = atan2(y, x);
lat = atan2((z+ep^2.*b.*sin(th).^3), (p-e^2.*a.*cos(th).^3));
N   = a./sqrt(1-e^2.*sin(lat).^2);
alt = p./cos(lat)-N;

% return lon in range [0, 2*pi)
lon = mod(lon, 2*pi);

% correct for numerical instability in altitude near exact poles:
% (after this correction, error is about 2 millimeters, which is about
% the same as the numerical precision of the overall function)

k=abs(x)<1 & abs(y)<1;
alt(k) = abs(z(k))-b;

return
             */

            double x = _x.ToMeters().Value;
            double y = _y.ToMeters().Value;
            double z = _z.ToMeters().Value;

            //% WGS84 ellipsoid constants:
            // a = 6378137;

            double a = ellipsoid.EquatorialRadius.ToMeters().Value;

            // e = 8.1819190842622e-2;

            double e = ellipsoid.Eccentricity;

            //% calculations:
            // b   = sqrt(a^2*(1-e^2));

            double b = Math.Sqrt(Math.Pow(a, 2) * (1 - Math.Pow(e, 2)));

            // ep  = sqrt((a^2-b^2)/b^2);

            double ep = Math.Sqrt((Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(b, 2));

            // p   = sqrt(x.^2+y.^2);

            double p = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));

            // th  = atan2(a*z, b*p);

            double th = Math.Atan2(a * z, b * p);

            // lon = atan2(y, x);

            double lon = Math.Atan2(y, x);

            // lat = atan2((z+ep^2.*b.*sin(th).^3), (p-e^2.*a.*cos(th).^3));

            double lat = Math.Atan2((z + Math.Pow(ep, 2) * b * Math.Pow(Math.Sin(th), 3)), (p - Math.Pow(e, 2) * a * Math.Pow(Math.Cos(th), 3)));

            // N   = a./sqrt(1-e^2.*sin(lat).^2);

            double n = a / Math.Sqrt(1 - Math.Pow(e, 2) * Math.Pow(Math.Sin(lat), 2));

            // alt = p./cos(lat)-N;

            double alt = p / Math.Cos(lat) - n;

            //% return lon in range [0, 2*pi)
            // lon = mod(lon, 2*pi);

            lon %= (2 * Math.PI);

            //% correct for numerical instability in altitude near exact poles:
            //% (after this correction, error is about 2 millimeters, which is about
            //% the same as the numerical precision of the overall function)

            // k=abs(x)<1 & abs(y)<1;

            bool k = Math.Abs(x) < 1.0 && Math.Abs(y) < 1.0;

            // alt(k) = abs(z(k))-b;

            if (k)
                alt = Math.Abs(z) - b;

            // return

            return new Position3D(
                    Distance.FromMeters(alt),
                    Latitude.FromRadians(lat),
                    Longitude.FromRadians(lon));

            #endregion New code
        }

        /// <summary>
        /// Returns the distance from the current instance to the specified cartesian point.
        /// </summary>
        /// <param name="point">A <strong>CartesianPoint</strong> object representing the end of a segment.</param>
        /// <returns></returns>
        public Distance DistanceTo(CartesianPoint point)
        {
            return new Distance(
                Math.Sqrt(Math.Pow(point.X.Value - _x.Value, 2)
                        + Math.Pow(point.Y.Value - _y.Value, 2)),
                        DistanceUnit.Meters).ToLocalUnitType();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        #endregion Public Methods

        #region Overrides

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is CartesianPoint)
                return Equals((CartesianPoint)obj);
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return _x.GetHashCode() ^ _y.GetHashCode() ^ _z.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        #endregion Overrides

        #region IFormattable Members

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider ?? CultureInfo.CurrentCulture;

            if (string.IsNullOrEmpty(format))
                format = "G";

            return _x.ToString(format, culture) + culture.TextInfo.ListSeparator
                + _y.ToString(format, culture) + culture.TextInfo.ListSeparator
                + _z.ToString(format, culture);
        }

        #endregion IFormattable Members

        #region IEquatable<CartesianPoint> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        public bool Equals(CartesianPoint other)
        {
            return _x.Equals(other.X)
                && _y.Equals(other.Y)
                && _z.Equals(other.Z);
        }

        #endregion IEquatable<CartesianPoint> Members

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
            /* The position class uses the GML 3.0 specification for XML.
             *
             * <gml:pos>X Y Z</gml:pos>
             *
             */
            writer.WriteStartElement(Xml.GML_XML_PREFIX, "pos", Xml.GML_XML_NAMESPACE);
            writer.WriteString(_x.ToMeters().Value.ToString("G17", CultureInfo.InvariantCulture));
            writer.WriteString(" ");
            writer.WriteString(_y.ToMeters().Value.ToString("G17", CultureInfo.InvariantCulture));
            writer.WriteString(" ");
            writer.WriteString(_z.ToMeters().Value.ToString("G17", CultureInfo.InvariantCulture));
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
             * <gml:pos>X Y Z</gml:pos>
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

            // .NET whines if we don't fully assign all values
            _x = Distance.Empty;
            _y = Distance.Empty;
            _z = Distance.Empty;

            // Move to the <gml:pos> or <gml:coord> element
            if (!reader.IsStartElement("pos", Xml.GML_XML_NAMESPACE)
                && !reader.IsStartElement("coord", Xml.GML_XML_NAMESPACE))
                reader.ReadStartElement();

            switch (reader.LocalName.ToLower(CultureInfo.InvariantCulture))
            {
                case "pos":
                    // Read the "X Y" string, then split by the space between them
                    string[] values = reader.ReadElementContentAsString().Split(' ');
                    // Deserialize the X
                    _x = Distance.FromMeters(double.Parse(values[0], CultureInfo.InvariantCulture));

                    // Deserialize the Y
                    if (values.Length >= 2)
                        _y = Distance.FromMeters(double.Parse(values[1], CultureInfo.InvariantCulture));

                    // Deserialize the Z
                    if (values.Length == 3)
                        _z = Distance.FromMeters(double.Parse(values[2], CultureInfo.InvariantCulture));

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
                                // Read X as meters (there's no unit type in the spec :P morons)
                                _x = Distance.FromMeters(reader.ReadElementContentAsDouble());
                                break;
                            case "y":
                                // Read Y as meters (there's no unit type in the spec :P morons)
                                _y = Distance.FromMeters(reader.ReadElementContentAsDouble());
                                break;
                            case "z":
                                // Read Z as meters (there's no unit type in the spec :P morons)
                                _z = Distance.FromMeters(reader.ReadElementContentAsDouble());
                                break;
                        }

                        // If we're at an end element, stop
                        if (reader.NodeType == XmlNodeType.EndElement)
                            break;
                    }
                    // Read the </gml:coord> end tag
                    reader.ReadEndElement();
                    break;
            }
            reader.Read();
        }

        #endregion IXmlSerializable Members

        #region Operators

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static CartesianPoint operator +(CartesianPoint a, CartesianPoint b)
        {
            return new CartesianPoint(a.X.Add(b.X), a.Y.Add(b.Y), a.Z.Add(b.Z));
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static CartesianPoint operator -(CartesianPoint a, CartesianPoint b)
        {
            return new CartesianPoint(a.X.Subtract(b.X), a.Y.Subtract(b.Y), a.Z.Subtract(b.Z));
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static CartesianPoint operator *(CartesianPoint a, CartesianPoint b)
        {
            return new CartesianPoint(a.X.Multiply(b.X), a.Y.Multiply(b.Y), a.Z.Multiply(b.Z));
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static CartesianPoint operator /(CartesianPoint a, CartesianPoint b)
        {
            return new CartesianPoint(a.X.Divide(b.X), a.Y.Divide(b.Y), a.Z.Divide(b.Z));
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Position"/> to <see cref="DotSpatial.Positioning.CartesianPoint"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator CartesianPoint(Position value)
        {
            return value.ToCartesianPoint();
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Position3D"/> to <see cref="DotSpatial.Positioning.CartesianPoint"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator CartesianPoint(Position3D value)
        {
            return value.ToCartesianPoint();
        }

        #endregion Operators
    }
}