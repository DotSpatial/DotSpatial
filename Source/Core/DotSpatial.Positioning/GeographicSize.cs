// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Globalization;

namespace DotSpatial.Positioning
{
    /// <summary>
    /// Represents a two-dimensional rectangular area.
    /// </summary>
    /// <remarks>Instances of this class are guaranteed to be thread-safe because the class is
    /// immutable (it's properties can only be set via constructors).</remarks>
    [TypeConverter("DotSpatial.Positioning.Design.GeographicSizeConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=1.0.0.0, PublicKeyToken=b4b0b185210c9dae")]
    public struct GeographicSize : IFormattable, IEquatable<GeographicSize>
    {

        #region Fields

        /// <summary>
        /// Represents a size with no value.
        /// </summary>
        public static readonly GeographicSize Empty = new(Distance.Empty, Distance.Empty);
        /// <summary>
        /// Represents a size with no value.
        /// </summary>
        public static readonly GeographicSize Minimum = new(Distance.Minimum, Distance.Minimum);
        /// <summary>
        /// Represents the largest possible size on Earth's surface.
        /// </summary>
        public static readonly GeographicSize Maximum = new(Distance.Maximum, Distance.Maximum);
        /// <summary>
        /// Represents an invalid geographic size.
        /// </summary>
        public static readonly GeographicSize Invalid = new(Distance.Invalid, Distance.Invalid);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public GeographicSize(Distance width, Distance height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Creates a new instance from the specified string.
        /// </summary>
        /// <param name="value">The value.</param>
        public GeographicSize(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <summary>
        /// Creates a new instance from the specified string in the specified culture.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        /// <remarks>This method will attempt to split the specified string into two values, then parse each value
        /// as an Distance object.  The string must contain two numbers separated by a comma (or other character depending
        /// on the culture).</remarks>
        public GeographicSize(string value, CultureInfo culture)
        {
            // Split out the values
            string[] values = value.Split(culture.TextInfo.ListSeparator.ToCharArray());

            // There should only be two of them
            switch (values.Length)
            {
                case 2:
                    Width = Distance.Parse(values[0], culture);
                    Height = Distance.Parse(values[1], culture);
                    break;
                default:
                    throw new ArgumentException("A GeographicSize could not be created from a string because the string was not in an identifiable format.  The format should be \"(w, h)\" where \"w\" represents a width in degrees, and \"h\" represents a height in degrees.  The values should be separated by a comma (or other character depending on the current culture).");
            }
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Returns the ratio of the size's width to its height.
        /// </summary>
        public float AspectRatio => Convert.ToSingle(Width.ToMeters().Value / Height.ToMeters().Value);

        /// <summary>
        /// Returns the left-to-right size.
        /// </summary>
        public Distance Width { get; }

        /// <summary>
        /// Returns the top-to-bottom size.
        /// </summary>
        public Distance Height { get; }

        /// <summary>
        /// Indicates if the size has zero values.
        /// </summary>
        public bool IsEmpty => (Width.IsEmpty && Height.IsEmpty);

        /// <summary>
        /// Returns whether the current instance has invalid values.
        /// </summary>
        public bool IsInvalid => Width.IsInvalid && Height.IsInvalid;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Toes the aspect ratio.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns></returns>
        public GeographicSize ToAspectRatio(Distance width, Distance height)
        {
            // Calculate the aspect ratio
            return ToAspectRatio(Convert.ToSingle(width.Divide(height).Value));
        }

        /// <summary>
        /// Toes the aspect ratio.
        /// </summary>
        /// <param name="aspectRatio">The aspect ratio.</param>
        /// <returns></returns>
        public GeographicSize ToAspectRatio(float aspectRatio)
        {
            float currentAspect = AspectRatio;

            // Do the values already match?
            if (currentAspect == aspectRatio)
            {
                return this;
            }

            // Convert to meters first
            Distance widthMeters = Width.ToMeters();
            Distance heightMeters = Height.ToMeters();

            // Is the new ratio higher or lower?
            if (aspectRatio > currentAspect)
            {
                // Inflate the GeographicRectDistance to the new height minus the current height
                return new GeographicSize(
                    widthMeters.Add(heightMeters.Multiply(aspectRatio).Subtract(widthMeters)),
                    heightMeters);
            }
            // Inflate the GeographicRectDistance to the new height minus the current height
            return new GeographicSize(
                widthMeters,
                heightMeters.Add(widthMeters.Divide(aspectRatio).Subtract(heightMeters)));
        }

        /// <summary>
        /// Adds the specified size to the current instance.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public GeographicSize Add(GeographicSize size)
        {
            return new GeographicSize(Width.Add(size.Width), Height.Add(size.Height));
        }

        /// <summary>
        /// Subtracts the specified size from the current instance.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public GeographicSize Subtract(GeographicSize size)
        {
            return new GeographicSize(Width.Subtract(size.Width), Height.Subtract(size.Height));
        }

        /// <summary>
        /// Multiplies the width and height by the specified size.
        /// </summary>
        /// <param name="size">A <strong>GeographicSize</strong> specifying how to much to multiply the width and height.</param>
        /// <returns>A <strong>GeographicSize</strong> representing the product of the current instance with the specified size.</returns>
        public GeographicSize Multiply(GeographicSize size)
        {
            return new GeographicSize(Width.Multiply(size.Width), Height.Multiply(size.Height));
        }

        /// <summary>
        /// Divides the width and height by the specified size.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public GeographicSize Divide(GeographicSize size)
        {
            return new GeographicSize(Width.Divide(size.Width), Height.Divide(size.Height));
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        #endregion Public Methods

        #region Overrides

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is GeographicSize size && Equals(size);
        }

        /// <summary>
        /// Returns a unique code based on the object's value.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Width.GetHashCode() ^ Height.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        #endregion Overrides

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
        /// <param name="culture">A <strong>CultureInfo</strong> object describing how to parse the specified string.</param>
        /// <returns>A <strong>GeographicSize</strong> whose Width and Height properties match the specified string.</returns>
        public static GeographicSize Parse(string value, CultureInfo culture)
        {
            return new GeographicSize(value, culture);
        }

        #endregion Static Methods

        #region Conversions

        /// <summary>
        /// Performs an explicit conversion from <see cref="string"/> to <see cref="DotSpatial.Positioning.GeographicSize"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator GeographicSize(string value)
        {
            return new GeographicSize(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.GeographicSize"/> to <see cref="string"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator string(GeographicSize value)
        {
            return value.ToString();
        }

        #endregion Conversions

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
            return Width.Equals(value.Width, decimals)
                && Height.Equals(value.Height, decimals);
        }

        /// <summary>
        /// Compares the left with the right object.
        /// </summary>
        /// <param name="left">The left object to compare.</param>
        /// <param name="right">The right object to compare.</param>
        /// <returns>True if both objects are equal.</returns>
        public static bool operator ==(GeographicSize left, GeographicSize right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares the left with the right object.
        /// </summary>
        /// <param name="left">The left object to compare.</param>
        /// <param name="right">The right object to compare.</param>
        /// <returns>True if both objects are not equal.</returns>
        public static bool operator !=(GeographicSize left, GeographicSize right)
        {
            return !(left == right);
        }

        #endregion IEquatable<GeographicSize> Members

        #region IFormattable Members

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="string"/> that represents this instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider ?? CultureInfo.CurrentCulture;

            if (string.IsNullOrEmpty(format))
            {
                format = "G";
            }

            return Width.ToString(format, culture)
                + culture.TextInfo.ListSeparator + " "
                + Height.ToString(format, culture);
        }

        #endregion IFormattable Members
    }
}