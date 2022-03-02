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
	/// <summary>
	/// Represents a two-dimensional rectangular area.
	/// </summary>
	/// <remarks>
	/// 	<para>Instances of this class are guaranteed to be thread-safe because the class is
	///     immutable (it's properties can only be set via constructors).</para>
    /// </remarks>
#if !PocketPC || DesignTime
    [TypeConverter("DotSpatial.Positioning.Design.GeographicSizeConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=d77afaeb30e3236a")]
#endif
    public struct GeographicSize : IFormattable, IEquatable<GeographicSize>
    {
        private readonly Distance _Width;
        private readonly Distance _Height;

        #region Fields

        /// <summary>Represents a size with no value.</summary>
        public static readonly GeographicSize Empty = new GeographicSize(Distance.Empty, Distance.Empty);
		/// <summary>Represents a size with no value.</summary>
        public static readonly GeographicSize Minimum = new GeographicSize(Distance.Minimum, Distance.Minimum);
		/// <summary>Represents the largest possible size on Earth's surface.</summary>
		public static readonly GeographicSize Maximum = new GeographicSize(Distance.Maximum, Distance.Maximum);
        /// <summary>Represents an invalid geographic size.</summary>
        public static readonly GeographicSize Invalid = new GeographicSize(Distance.Invalid, Distance.Invalid);

        #endregion

        #region Constructors

        /// <summary>Creates a new instance.</summary>
        public GeographicSize(Distance width, Distance height)
        {
            _Width = width;
            _Height = height;
        }

        /// <summary>
        /// Creates a new instance from the specified string.
        /// </summary>
        /// <param name="value"></param>
        public GeographicSize(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <summary>
        /// Creates a new instance from the specified string in the specified culture.
        /// </summary>
        /// <param name="value">A <strong>String</strong> describing a width and height in degrees (e.g. "1,3").</param>
        /// <param name="culture">A <strong>CultureInfo</strong> object describing how to parse the string.</param>
        /// <remarks>This method will attempt to split the specified string into two values, then parse each value
        /// as an Distance object.  The string must contain two numbers separated by a comma (or other character depending
        /// on the culture).</remarks>
        public GeographicSize(string value, CultureInfo culture)
        {
            // Split out the values
            string[] Values = value.Split(culture.TextInfo.ListSeparator.ToCharArray());

            // There should only be two of them
            switch (Values.Length)
            {
                case 2:
                    _Width = Distance.Parse(Values[0], culture);
                    _Height = Distance.Parse(Values[1], culture);
                    break;
                default:
                    throw new ArgumentException("A GeographicSize could not be created from a string because the string was not in an identifiable format.  The format should be \"(w,h)\" where \"w\" represents a width in degrees, and \"h\" represents a height in degrees.  The values should be separated by a comma (or other character depending on the current culture).");
            }
        }

        #endregion

        #region Public Properties

        /// <summary>Returns the ratio of the size's width to its height.</summary>
		public float AspectRatio
		{
			get
			{
                return Convert.ToSingle(_Width.ToMeters().Value / _Height.ToMeters().Value);
			}
        }

        /// <summary>
        /// Returns the left-to-right size.
        /// </summary>
        public Distance Width
        {
            get
            {
                return _Width;
            }
        }

        /// <summary>Returns the top-to-bottom size.</summary>
        public Distance Height
        {
            get
            {
                return _Height;
            }
        }

        /// <summary>Indicates if the size has zero values.</summary>
        public bool IsEmpty
        {
            get
            {
                return (_Width.IsEmpty && _Height.IsEmpty);
            }
        }

        /// <summary>Returns whether the current instance has invalid values.</summary>
        public bool IsInvalid
        {
            get
            {
                return _Width.IsInvalid && _Height.IsInvalid;
            }
        }

        #endregion

        #region Public Methods

		public GeographicSize ToAspectRatio(Distance width, Distance height)
		{
			// Calculate the aspect ratio
			return ToAspectRatio(Convert.ToSingle(width.Divide(height).Value));
		}

		public GeographicSize ToAspectRatio(float aspectRatio)
		{
			float CurrentAspect = AspectRatio;
			
            // Do the values already match?
			if(CurrentAspect == aspectRatio) 
                return this;

            // Convert to meters first
            Distance WidthMeters = _Width.ToMeters();
            Distance HeightMeters = _Height.ToMeters();
            
            // Is the new ratio higher or lower?
			if(aspectRatio > CurrentAspect)
			{
				// Inflate the GeographicRectDistance to the new height minus the current height				
				return new GeographicSize(
                    WidthMeters.Add(HeightMeters.Multiply(aspectRatio).Subtract(WidthMeters)), 
                    HeightMeters);
			}
			else
			{
				// Inflate the GeographicRectDistance to the new height minus the current height
				return new GeographicSize(
                    WidthMeters, 
                    HeightMeters.Add(WidthMeters.Divide(aspectRatio).Subtract(HeightMeters)));
			}
        }

		/// <summary>Adds the specified size to the current instance.</summary>
		public GeographicSize Add(GeographicSize size)
		{
			return new GeographicSize(_Width.Add(size.Width), _Height.Add(size.Height));
		}

		/// <summary>Subtracts the specified size from the current instance.</summary>
		public GeographicSize Subtract(GeographicSize size)
		{
			return new GeographicSize(_Width.Subtract(size.Width), _Height.Subtract(size.Height));
		}
	
        /// <summary>
        /// Multiplies the width and height by the specified size.
        /// </summary>
        /// <param name="size">A <strong>GeographicSize</strong> specifying how to much to multiply the width and height.</param>
        /// <returns>A <strong>GeographicSize</strong> representing the product of the current instance with the specified size.</returns>
		public GeographicSize Multiply(GeographicSize size)
		{
            return new GeographicSize(_Width.Multiply(size.Width), _Height.Multiply(size.Height));
		}

        /// <summary>
        /// Divides the width and height by the specified size.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
		public GeographicSize Divide(GeographicSize size)
		{
            return new GeographicSize(_Width.Divide(size.Width), _Height.Divide(size.Height));
		}

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj is GeographicSize)
                return Equals((GeographicSize)obj);
            return false;
        }

        /// <summary>
        /// Returns a unique code based on the object's value.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _Width.GetHashCode() ^ _Height.GetHashCode();
        }

        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Returns a GeographicSize whose value matches the specified string.
        /// </summary>
        /// <param name="value">A <strong>String</strong> describing a width, followed by a height.</param>
        /// <returns>A <strong>GeographicSize</strong> whose Width and Height properties match the specified string.</returns>
		public static GeographicSize Parse(string value)
		{
			return Parse(value, CultureInfo.CurrentCulture);
		}

        /// <summary>
        /// Returns a GeographicSize whose value matches the specified string.
        /// </summary>
        /// <param name="value">A <strong>String</strong> describing a width, followed by a height.</param>
        /// <returns>A <strong>GeographicSize</strong> whose Width and Height properties match the specified string.</returns>
        /// <param name="culture">A <strong>CultureInfo</strong> object describing how to parse the specified string.</param>
		public static GeographicSize Parse(string value, CultureInfo culture)
		{
            return new GeographicSize(value, culture);
        }

        #endregion

        #region Conversions

		public static explicit operator GeographicSize(string value)
		{
			return new GeographicSize(value, CultureInfo.CurrentCulture);
		}

		public static explicit operator string(GeographicSize value)
		{
			return value.ToString();
        }

        #endregion

        #region IEquatable<GeographicSize> Members

        /// <summary>
        /// Compares the value of the current instance to the specified GeographicSize.
        /// </summary>
        /// <param name="value">A <strong>GeographicSize</strong> object to compare against.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the values of both objects are precisely the same.</returns>
        public bool Equals(GeographicSize value)
        {
            return Width.Equals(value.Width)
                && Height.Equals(value.Height);
        }

        /// <summary>
        /// Compares the value of the current instance to the specified GeographicSize, to the specified number of decimals.
        /// </summary>
        /// <param name="value">A <strong>GeographicSize</strong> object to compare against.</param>
        /// <param name="decimals">An <strong>Integer</strong> describing how many decimals the values are rounded to before comparison.</param>
        /// <returns>A <strong>Boolean</strong>, <strong>True</strong> if the values of both objects are the same out to the number of decimals specified.</returns>
        public bool Equals(GeographicSize value, int decimals)
        {
            return _Width.Equals(value.Width, decimals)
                && _Height.Equals(value.Height, decimals);
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

            return _Width.ToString(format, culture)
                + culture.TextInfo.ListSeparator + " "
                + _Height.ToString(format, culture);
        }

        #endregion
    }
}
