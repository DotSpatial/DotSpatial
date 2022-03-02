using System;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
#if !PocketPC || DesignTime
using System.ComponentModel;
#endif

namespace DotSpatial.Positioning
{
	/// <summary>Represents a highly-precise two-dimensional size.</summary>
	/// <remarks>
	/// 	<para>This structure is a <em>DotSpatial.Positioning</em> "parseable type" whose value can
	///     be freely converted to and from <strong>String</strong> objects via the
	///     <strong>ToString</strong> and <strong>Parse</strong> methods.</para>
	/// 	<para>Instances of this structure are guaranteed to be thread-safe because it is
	///     immutable (its properties can only be modified via constructors).</para>
	/// </remarks>
#if !PocketPC || DesignTime
    [TypeConverter("DotSpatial.Positioning.Design.SizeDConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=d77afaeb30e3236a")]
#endif
    public struct SizeD : IFormattable, IEquatable<SizeD>, IXmlSerializable
    {
        private double _Width;
        private double _Height;

        #region Fields

        /// <summary>Represents a size with no value.</summary>
		public static readonly SizeD Empty = new SizeD(0.0, 0.0);
		/// <summary>Represents an infinite size.</summary>
		public static readonly SizeD Infinity = new SizeD(Double.PositiveInfinity, Double.PositiveInfinity);
		/// <summary>Represents the smallest possible size.</summary>
		public static readonly SizeD Minimum = new SizeD(Double.MinValue, Double.MinValue);
		/// <summary>Represents the largest possible size.</summary>
		public static readonly SizeD Maximum = new SizeD(Double.MaxValue, Double.MaxValue);

        #endregion

        #region Constructors

        public SizeD(PointD pt)
		{
			_Width = pt.X;
			_Height = pt.Y;
		}

		public SizeD(SizeD size)
		{
			_Width = size.Width;
			_Height = size.Height;
		}

		/// <summary>Creates a new instance.</summary>
		public SizeD(double width, double height)
		{
			_Width = width;
			_Height = height;
		}

        public SizeD(string value)
            : this(value, CultureInfo.CurrentCulture)
        {}

        public SizeD(string value, CultureInfo culture)
        {
            // Split out the values
            string[] Values = value.Trim().Split(culture.TextInfo.ListSeparator.ToCharArray());
            
            // There should be two values
            if (Values.Length != 2)
                throw new FormatException(Properties.Resources.SizeD_InvalidFormat); 

            // PArse it out
            _Width = double.Parse(Values[0].Trim(), culture);
            _Height = double.Parse(Values[1].Trim(), culture);
        }

        public SizeD(XmlReader reader)
        {
            // Initialize all fields
            _Width = Double.NaN;
            _Height = Double.NaN;

            // Deserialize the object from XML
            ReadXml(reader);
        }

        #endregion

        #region Public Properties

        /// <summary>Returns the horizontal size.</summary>
        public double Width
        {
            get
            {
                return _Width;
            }
        }

        /// <summary>Returns the vertical size.</summary>
        public double Height
        {
            get
            {
                return _Height;
            }
        }

        /// <summary>Returns the ratio width to height.</summary>
		public double AspectRatio
		{
			get
			{
				return _Width / _Height;
			}
        }

        /// <summary>Indicates if the instance has any value.</summary>
        public bool IsEmpty
        {
            get
            {
                return (_Width == 0 && _Height == 0);
            }
        }

        #endregion

        #region Public Methods

        public SizeD ToAspectRatio(SizeD size)
        {
            // Calculate the aspect ratio
            return ToAspectRatio((double)size.Width / (double)size.Height);
        }

        public SizeD ToAspectRatio(double aspectRatio)
        {
            double CurrentAspect = AspectRatio;
            // Do the values already match?
            if (CurrentAspect == aspectRatio) return this;
            // Is the new ratio higher or lower?
            if (aspectRatio > CurrentAspect)
            {
                // Inflate the GeographicRectangle to the new height minus the current height
                // TESTS OK
                return new SizeD(_Width +
                    (aspectRatio * Height - Width), _Height);
            }
            else
            {
                // Inflate the GeographicRectangle to the new height minus the current height
                return new SizeD(_Width,
                    _Height + (Width / aspectRatio - Height));
            }
        }

        /// <summary>Returns a copy of the current instance.</summary>
        public SizeD Clone()
        {
            return new SizeD(_Width, _Height);
        }

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        #endregion

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

        public override int GetHashCode()
        {
            return Convert.ToInt32(_Width) ^ Convert.ToInt32(_Height);
        }

        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        #endregion

        #region Static Methods

        public static SizeD Parse(string value)
        {
            return new SizeD(value, CultureInfo.CurrentCulture);
        }

        public static SizeD Parse(string value, CultureInfo culture)
        {
            return new SizeD(value, culture);
        }

        #endregion

        #region Operators

        public static bool operator ==(SizeD left, SizeD right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SizeD left, SizeD right)
        {
            return !(left.Equals(right));
        }

        public static SizeD operator +(SizeD left, SizeD right)
        {
            return left.Add(right);
        }

        public static SizeD operator -(SizeD left, SizeD right)
        {
            return left.Subtract(right);
        }

        public static SizeD operator *(SizeD left, SizeD right)
        {
            return left.Multiply(right);
        }

        public static SizeD operator /(SizeD left, SizeD right)
        {
            return left.Divide(right);
        }

        /// <summary>Returns the sum of the current instance with the specified size.</summary>
        public SizeD Add(SizeD size)
        {
            return new SizeD(_Width + size.Width, _Height + size.Height);
        }

        /// <summary>Returns the current instance decreased by the specified value.</summary>
        public SizeD Subtract(SizeD size)
        {
            return new SizeD(_Width - size.Width, _Height - size.Height);
        }

        /// <summary>Returns the product of the current instance with the specified value.</summary>
        public SizeD Multiply(SizeD size)
        {
            return new SizeD(_Width * size.Width, _Height * size.Height);
        }

        /// <summary>Returns the current instance divided by the specified value.</summary>
        public SizeD Divide(SizeD size)
        {
            return new SizeD(_Width / size.Width, _Height / size.Height);
        }

        #endregion

        #region IEquatable<SizeD> Members

        /// <summary>
        /// Compares the current instance to the specified object.
        /// </summary>
        /// <param name="other">A <strong>SizeD</strong> object to compare with.</param>
        /// <returns>A <strong>Boolean</strong>, True if the values are equivalent.</returns>
        public bool Equals(SizeD other)
        {
            return _Width.Equals(other.Width) && _Height.Equals(other.Height);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format, IFormatProvider formatProvider)
		{
            CultureInfo culture = (CultureInfo)formatProvider;

            if (culture == null)
                culture = CultureInfo.CurrentCulture;

            if (format == null || format.Length == 0)
                format = "G";

            return Width.ToString(format, culture)
                + culture.TextInfo.ListSeparator + " "
                + Height.ToString(format, culture);
		}
	
		#endregion

        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Width",
                        _Width.ToString("G17", CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Height",
                        _Height.ToString("G17", CultureInfo.InvariantCulture));
        }

        public void ReadXml(XmlReader reader)
        {
            _Width = double.Parse(
                reader.GetAttribute("Width"), CultureInfo.InvariantCulture);
            _Height = double.Parse(
                reader.GetAttribute("Height"), CultureInfo.InvariantCulture);
        }

        #endregion
	}
}
