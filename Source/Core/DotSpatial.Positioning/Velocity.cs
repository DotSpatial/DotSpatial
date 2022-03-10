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
    /// Represents a measurement of an object's rate of travel in a particular direction.
    /// </summary>
    /// <remarks>Instances of this class are guaranteed to be thread-safe because the class is
    /// immutable (its properties can only be changed via constructors).</remarks>
    public struct Velocity : IFormattable, IEquatable<Velocity>, ICloneable<Velocity>, IXmlSerializable
    {
        /// <summary>
        ///
        /// </summary>
        private Speed _speed;
        /// <summary>
        ///
        /// </summary>
        private Azimuth _bearing;

        #region Fields

        /// <summary>
        /// Represents a velocity with no speed or direction.
        /// </summary>
        public static readonly Velocity Empty = new(Speed.Empty, Azimuth.Empty);

        /// <summary>
        /// Represents a velocity with an invalid or unspecified speed and direction.
        /// </summary>
        public static readonly Velocity Invalid = new(Speed.Invalid, Azimuth.Invalid);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Velocity"/> struct.
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <param name="bearing">The bearing.</param>
        public Velocity(Speed speed, Azimuth bearing)
        {
            _speed = speed;
            _bearing = bearing;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Velocity"/> struct.
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <param name="bearingDegrees">The bearing degrees.</param>
        public Velocity(Speed speed, double bearingDegrees)
        {
            _speed = speed;
            _bearing = new Azimuth(bearingDegrees);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Velocity"/> struct.
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <param name="speedUnits">The speed units.</param>
        /// <param name="bearing">The bearing.</param>
        public Velocity(double speed, SpeedUnit speedUnits, Azimuth bearing)
        {
            _speed = new Speed(speed, speedUnits);
            _bearing = bearing;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Velocity"/> struct.
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <param name="speedUnits">The speed units.</param>
        /// <param name="bearingDegrees">The bearing degrees.</param>
        public Velocity(double speed, SpeedUnit speedUnits, double bearingDegrees)
        {
            _speed = new Speed(speed, speedUnits);
            _bearing = new Azimuth(bearingDegrees);
        }

        /// <summary>
        /// Creates a new instance by parsing speed and bearing from the specified strings.
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <param name="bearing">The bearing.</param>
        public Velocity(string speed, string bearing)
            : this(speed, bearing, CultureInfo.CurrentCulture)
        { }

        /// <summary>
        /// Creates a new instance by converting the specified strings using the specific culture.
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <param name="bearing">The bearing.</param>
        /// <param name="culture">The culture.</param>
        public Velocity(string speed, string bearing, CultureInfo culture)
        {
            _speed = new Speed(speed, culture);
            _bearing = new Azimuth(bearing, culture);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Velocity"/> struct.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public Velocity(XmlReader reader)
        {
            // Initialize all fields
            _speed = Speed.Invalid;
            _bearing = Azimuth.Invalid;

            // Deserialize the object from XML
            ReadXml(reader);
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets the objects rate of travel.
        /// </summary>
        public Speed Speed
        {
            get { return _speed; }
        }

        /// <summary>
        /// Gets the objects direction of travel.
        /// </summary>
        public Azimuth Bearing
        {
            get { return _bearing; }
        }

        /// <summary>
        /// Indicates whether the speed and bearing are both zero.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _speed.IsEmpty && _bearing.IsEmpty;
            }
        }

        /// <summary>
        /// Indicates whether the speed or bearing is invalid or unspecified.
        /// </summary>
        public bool IsInvalid
        {
            get
            {
                return _speed.IsInvalid || _bearing.IsInvalid;
            }
        }

        #endregion Public Properties

        #region Operators

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Velocity left, Velocity right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Velocity left, Velocity right)
        {
            return !left.Equals(right);
        }

        #endregion Operators

        #region Overrides

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return _speed.GetHashCode() ^ _bearing.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Velocity)
                return Equals((Velocity)obj);
            return false;
        }

        /// <summary>
        /// Outputs the current instance as a string using the default format.
        /// </summary>
        /// <returns>A <strong>String</strong> representing the current instance.</returns>
        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture);
        }

        #endregion Overrides

        #region IEquatable<Velocity>

        /// <summary>
        /// Compares the current instance to the specified velocity.
        /// </summary>
        /// <param name="other">A <strong>Velocity</strong> object to compare with.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the values are identical.</returns>
        public bool Equals(Velocity other)
        {
            return _speed.Equals(other.Speed)
                && _bearing.Equals(other.Bearing);
        }

        /// <summary>
        /// Compares the current instance to the specified velocity using the specified numeric precision.
        /// </summary>
        /// <param name="other">A <strong>Velocity</strong> object to compare with.</param>
        /// <param name="decimals">An <strong>Integer</strong> specifying the number of fractional digits to compare.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the values are identical.</returns>
        public bool Equals(Velocity other, int decimals)
        {
            return _speed.Equals(other.Speed, decimals)
                && _bearing.Equals(other.Bearing, decimals);
        }

        #endregion IEquatable<Velocity>

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

            if (string.IsNullOrEmpty(format)) format = "G";

            // Output as speed and bearing
            return _speed.ToString(format, culture)
                + " "
                + _bearing.ToString(format, culture);
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
            writer.WriteStartElement("Speed");
            _speed.WriteXml(writer);
            writer.WriteEndElement();

            writer.WriteStartElement("Bearing");
            _bearing.WriteXml(writer);
            writer.WriteEndElement();
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            // Move to the <Speed> element
            if (!reader.IsStartElement("Speed"))
                reader.ReadToDescendant("Speed");

            _speed.ReadXml(reader);
            _bearing.ReadXml(reader);

            reader.Read();
        }

        #endregion IXmlSerializable Members

        #region ICloneable<Velocity> Members

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public Velocity Clone()
        {
            return new Velocity(_speed, _bearing);
        }

        #endregion ICloneable<Velocity> Members
    }
}