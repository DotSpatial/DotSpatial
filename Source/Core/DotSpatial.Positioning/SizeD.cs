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
using DotSpatial.Positioning;
#if !PocketPC || DesignTime

using System.ComponentModel;

#endif

namespace DotSpatial.Positioning
{
#if !PocketPC || DesignTime
    /// <summary>
    /// Represents a highly-precise two-dimensional size.
    /// </summary>
    /// <remarks><para>This structure is a <em>DotSpatial.Positioning</em> "parseable type" whose value can
    /// be freely converted to and from <strong>String</strong> objects via the
    ///   <strong>ToString</strong> and <strong>Parse</strong> methods.</para>
    ///   <para>Instances of this structure are guaranteed to be thread-safe because it is
    /// immutable (its properties can only be modified via constructors).</para></remarks>
    [TypeConverter("DotSpatial.Positioning.Design.SizeDConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=1.0.0.0, PublicKeyToken=b4b0b185210c9dae")]
#endif
    public struct SizeD : IFormattable, IEquatable<SizeD>, IXmlSerializable
    {
        /// <summary>
        ///
        /// </summary>
        private double _width;
        /// <summary>
        ///
        /// </summary>
        private double _height;

        #region Fields

        /// <summary>
        /// Represents a size with no value.
        /// </summary>
        public static readonly SizeD Empty = new(0.0, 0.0);
        /// <summary>
        /// Represents an infinite size.
        /// </summary>
        public static readonly SizeD Infinity = new(Double.PositiveInfinity, Double.PositiveInfinity);
        /// <summary>
        /// Represents the smallest possible size.
        /// </summary>
        public static readonly SizeD Minimum = new(Double.MinValue, Double.MinValue);
        /// <summary>
        /// Represents the largest possible size.
        /// </summary>
        public static readonly SizeD Maximum = new(Double.MaxValue, Double.MaxValue);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeD"/> struct.
        /// </summary>
        /// <param name="pt">The pt.</param>
        public SizeD(PointD pt)
        {
            _width = pt.X;
            _height = pt.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeD"/> struct.
        /// </summary>
        /// <param name="size">The size.</param>
        public SizeD(SizeD size)
        {
            _width = size.Width;
            _height = size.Height;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public SizeD(double width, double height)
        {
            _width = width;
            _height = height;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeD"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public SizeD(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeD"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        public SizeD(string value, CultureInfo culture)
        {
            // Split out the values
            string[] values = value.Trim().Split(culture.TextInfo.ListSeparator.ToCharArray());

            // There should be two values
            if (values.Length != 2)
                throw new FormatException(Resources.SizeD_InvalidFormat);

            // PArse it out
            _width = double.Parse(values[0].Trim(), culture);
            _height = double.Parse(values[1].Trim(), culture);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeD"/> struct.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public SizeD(XmlReader reader)
        {
            // Initialize all fields
            _width = Double.NaN;
            _height = Double.NaN;

            // Deserialize the object from XML
            ReadXml(reader);
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Returns the horizontal size.
        /// </summary>
        public double Width
        {
            get
            {
                return _width;
            }
        }

        /// <summary>
        /// Returns the vertical size.
        /// </summary>
        public double Height
        {
            get
            {
                return _height;
            }
        }

        /// <summary>
        /// Returns the ratio width to height.
        /// </summary>
        public double AspectRatio
        {
            get
            {
                return _width / _height;
            }
        }

        /// <summary>
        /// Indicates if the instance has any value.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (_width == 0 && _height == 0);
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Toes the aspect ratio.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public SizeD ToAspectRatio(SizeD size)
        {
            // Calculate the aspect ratio
            return ToAspectRatio(size.Width / size.Height);
        }

        /// <summary>
        /// Toes the aspect ratio.
        /// </summary>
        /// <param name="aspectRatio">The aspect ratio.</param>
        /// <returns></returns>
        public SizeD ToAspectRatio(double aspectRatio)
        {
            double currentAspect = AspectRatio;
            // Do the values already match?
            if (currentAspect == aspectRatio) return this;
            // Is the new ratio higher or lower?
            if (aspectRatio > currentAspect)
            {
                // Inflate the GeographicRectangle to the new height minus the current height
                // TESTS OK
                return new SizeD(_width +
                    (aspectRatio * Height - Width), _height);
            }
            // Inflate the GeographicRectangle to the new height minus the current height
            return new SizeD(_width,
                             _height + (Width / aspectRatio - Height));
        }

        /// <summary>
        /// Returns a copy of the current instance.
        /// </summary>
        /// <returns></returns>
        public SizeD Clone()
        {
            return new SizeD(_width, _height);
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
        /// Compares the current instance to the specified object.
        /// </summary>
        /// <param name="obj">An <strong>Object</strong> to compare with.</param>
        /// <returns>A <strong>Boolean</strong>, True if the values are equivalent.</returns>
        public override bool Equals(object obj)
        {
            // Only compare similar objects
            if (obj is SizeD)
                return Equals((SizeD)obj);
            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Convert.ToInt32(_width) ^ Convert.ToInt32(_height);
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

        #region Static Methods

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static SizeD Parse(string value)
        {
            return new SizeD(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static SizeD Parse(string value, CultureInfo culture)
        {
            return new SizeD(value, culture);
        }

        #endregion Static Methods

        #region Operators

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(SizeD left, SizeD right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(SizeD left, SizeD right)
        {
            return !(left.Equals(right));
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static SizeD operator +(SizeD left, SizeD right)
        {
            return left.Add(right);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static SizeD operator -(SizeD left, SizeD right)
        {
            return left.Subtract(right);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static SizeD operator *(SizeD left, SizeD right)
        {
            return left.Multiply(right);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static SizeD operator /(SizeD left, SizeD right)
        {
            return left.Divide(right);
        }

        /// <summary>
        /// Returns the sum of the current instance with the specified size.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public SizeD Add(SizeD size)
        {
            return new SizeD(_width + size.Width, _height + size.Height);
        }

        /// <summary>
        /// Returns the current instance decreased by the specified value.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public SizeD Subtract(SizeD size)
        {
            return new SizeD(_width - size.Width, _height - size.Height);
        }

        /// <summary>
        /// Returns the product of the current instance with the specified value.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public SizeD Multiply(SizeD size)
        {
            return new SizeD(_width * size.Width, _height * size.Height);
        }

        /// <summary>
        /// Returns the current instance divided by the specified value.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public SizeD Divide(SizeD size)
        {
            return new SizeD(_width / size.Width, _height / size.Height);
        }

        #endregion Operators

        #region IEquatable<SizeD> Members

        /// <summary>
        /// Compares the current instance to the specified object.
        /// </summary>
        /// <param name="other">A <strong>SizeD</strong> object to compare with.</param>
        /// <returns>A <strong>Boolean</strong>, True if the values are equivalent.</returns>
        public bool Equals(SizeD other)
        {
            return _width.Equals(other.Width) && _height.Equals(other.Height);
        }

        #endregion IEquatable<SizeD> Members

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

            return Width.ToString(format, culture)
                + culture.TextInfo.ListSeparator + " "
                + Height.ToString(format, culture);
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
            writer.WriteAttributeString("Width",
                        _width.ToString("G17", CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Height",
                        _height.ToString("G17", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            double.TryParse(reader.GetAttribute("Width"), NumberStyles.Any, CultureInfo.InvariantCulture, out _width);
            double.TryParse(reader.GetAttribute("Height"), NumberStyles.Any, CultureInfo.InvariantCulture, out _height);
            reader.Read();
        }

        #endregion IXmlSerializable Members
    }
}