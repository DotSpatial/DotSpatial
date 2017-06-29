// ********************************************************************************************************
// Product Name: DotSpatial.Positioning.dll
// Description:  A library for managing GPS connections.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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

#if !PocketPC || DesignTime

using System.ComponentModel;

#endif

namespace DotSpatial.Positioning
{
#if !PocketPC || DesignTime
    /// <summary>
    /// Represents a highly-precise pixel coordinate.
    /// </summary>
    /// <remarks><para>This class behaves similar to the <strong>PointF</strong> structure in the
    ///   <strong>System.Drawing</strong> namespace, except that it supports double-precision
    /// values and can be converted into a geographic coordinate. This structure is also
    /// supported on the Compact Framework version of the <strong>DotSpatial.Positioning</strong>,
    /// whereas <strong>PointF</strong> is not.</para>
    ///   <para>Instances of this class are guaranteed to be thread-safe because the class is
    /// immutable (its properties can only be changed via constructors).</para></remarks>
    [TypeConverter("DotSpatial.Positioning.Design.PointDConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=1.0.0.0, PublicKeyToken=b4b0b185210c9dae")]
#endif
    public struct PointD : IFormattable, IEquatable<PointD>, IXmlSerializable
    {
        /// <summary>
        ///
        /// </summary>
        private double _x;
        /// <summary>
        ///
        /// </summary>
        private double _y;

        #region Fields

        /// <summary>
        /// Returns a point with no value.
        /// </summary>
        public static readonly PointD Empty = new PointD(0, 0);
        /// <summary>
        /// Represents an invalid coordinate.
        /// </summary>
        public static readonly PointD Invalid = new PointD(double.NaN, double.NaN);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance for the specified coordinates.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public PointD(double x, double y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointD"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public PointD(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointD"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        public PointD(string value, CultureInfo culture)
        {
            // Trim the string and remove double spaces
            value = value.Replace("  ", " ").Trim();

            // And separate it via the list separator
            string[] values = value.Split(Convert.ToChar(culture.TextInfo.ListSeparator, culture));

            // Return the converted values
            _x = double.Parse(values[0], NumberStyles.Any, culture);
            _y = double.Parse(values[1], NumberStyles.Any, culture);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointD"/> struct.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public PointD(XmlReader reader)
        {
            _x = _y = 0.0;
            ReadXml(reader);
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the x-coordinate of this PointD.
        /// </summary>
        /// <value>The X.</value>
        public double X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        /// <summary>
        /// For projected coordinates, this is the factor Lamda or the longitude parameter.
        /// For readability only, the value is X.
        /// </summary>
        /// <value>The lam.</value>
        public double Lam
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of this PointD.
        /// </summary>
        /// <value>The Y.</value>
        public double Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        /// <summary>
        /// For projected coordinates, this is the factor Phi or the latitude parameter.
        /// For readability only, the value is Y.
        /// </summary>
        /// <value>The phi.</value>
        public double Phi
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        /// <summary>
        /// Returns whether the current instance has no value.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (_x == 0 && _y == 0);
            }
        }

        /// <summary>
        /// Returns whether the current instance has an invalid value.
        /// </summary>
        public bool IsInvalid
        {
            get
            {
                double fail = _x * _y;

                return (double.IsNaN(fail) || double.IsInfinity(fail));
            }
        }

        #endregion Public Properties

        #region Public Methods

        /*
        /// <summary>Calculates the direction from one point to another.</summary>
        public Azimuth BearingTo(PointD value)
        {
            double Result = value.Subtract(this).ToPolarCoordinate().Theta.DecimalDegrees;
            return new Azimuth(Result).Normalize();
        }
         */

        /// <summary>
        /// Calculates the distance to another pixel.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public double DistanceTo(PointD value)
        {
            return Math.Sqrt(Math.Abs(value.X - _x) * Math.Abs(value.X - _x)
                + Math.Abs(value.Y - _y) * Math.Abs(value.Y - _y));
        }

        /// <summary>
        /// Indicates if the current instance is closer to the top of the monitor than the
        /// specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is above; otherwise, <c>false</c>.</returns>
        public bool IsAbove(PointD value)
        {
            return _y < value.Y;
        }

        /// <summary>
        /// Indicates if the current instance is closer to the bottom of the monitor than the
        /// specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is below; otherwise, <c>false</c>.</returns>
        public bool IsBelow(PointD value)
        {
            return _y > value.Y;
        }

        /// <summary>
        /// Indicates if the current instance is closer to the left of the monitor than the
        /// specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is left of] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsLeftOf(PointD value)
        {
            return _x < value.X;
        }

        /// <summary>
        /// Indicates if the current instance is closer to the right of the monitor than the
        /// specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is right of] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsRightOf(PointD value)
        {
            return _x > value.X;
        }

        /// <summary>
        /// Returns the current instance with its signs switched.
        /// </summary>
        /// <returns></returns>
        /// <remarks>This method returns a new point where the signs of X and Y are flipped.  For example, if
        /// a point, represents (20, 40), this function will return (-20, -40).</remarks>
        public PointD Mirror()
        {
            return new PointD(-_x, -_y);
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

        /// <summary>
        /// Returns the current instance rotated about (0, 0).
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        public PointD Rotate(Angle angle)
        {
            return Rotate(angle.DecimalDegrees);
        }

        /// <summary>
        /// Returns the current instance rotated about (0, 0).
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        public PointD Rotate(double angle)
        {
            // Convert the angle to radians
            double angleRadians = Radian.FromDegrees(angle).Value;
            double angleCos = Math.Cos(angleRadians);
            double angleSin = Math.Sin(angleRadians);
            // Yes.  Rotate the point about 0, 0
            return new PointD(angleCos * _x - angleSin * _y, angleSin * _x + angleCos * _y);
        }

        /// <summary>
        /// Returns the current instance rotated about the specified point.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <param name="center">The center.</param>
        /// <returns></returns>
        public PointD RotateAt(Angle angle, PointD center)
        {
            return RotateAt(angle.DecimalDegrees, center);
        }

        /// <summary>
        /// Rotates at.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <param name="center">The center.</param>
        /// <returns></returns>
        public PointD RotateAt(double angle, PointD center)
        {
            if (angle == 0)
                return this;

            // Shift the point by its center, rotate, then add the center back in
            return Subtract(center).Rotate(angle).Add(center);
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
            if (obj is PointD)
                return Equals((PointD)obj);
            return false;
        }

        /// <summary>
        /// Returns a unique code used for hash tables.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return _x.GetHashCode() ^ _y.GetHashCode();
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
        public static PointD Parse(string value)
        {
            return new PointD(value);
        }

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static PointD Parse(string value, CultureInfo culture)
        {
            return new PointD(value, culture);
        }

        #endregion Static Methods

        #region Operators

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(PointD left, PointD right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(PointD left, PointD right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static PointD operator +(PointD left, PointD right)
        {
            return new PointD(left.X + right.X, left.Y + right.Y);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static PointD operator -(PointD left, PointD right)
        {
            return new PointD(left.X - right.X, left.Y - right.Y);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static PointD operator *(PointD left, PointD right)
        {
            return new PointD(left.X * right.X, left.Y * right.Y);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static PointD operator /(PointD left, PointD right)
        {
            return new PointD(left.X / right.X, left.Y / right.Y);
        }

        /// <summary>
        /// Returns the sum of two points by adding X and Y values together.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        /// <remarks>This method adds the X and Y coordinates and returns a new point at that location.</remarks>
        public PointD Add(PointD offset)
        {
            return new PointD(_x + offset.X, _y + offset.Y);
            //return Offset(offset.X, offset.Y);
        }

        /// <summary>
        /// Returns the sum of two points by adding X and Y values together.
        /// </summary>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="offsetY">The offset Y.</param>
        /// <returns></returns>
        /// <remarks>This method adds the X and Y coordinates and returns a new point at that location.</remarks>
        public PointD Add(double offsetX, double offsetY)
        {
            return new PointD(_x + offsetX, _y + offsetY);
        }

        /// <summary>
        /// Returns the difference of two points by subtracting the specified X and Y values.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        /// <remarks>This method subtracts the X and Y coordinates and returns a new point at that location.</remarks>
        public PointD Subtract(PointD offset)
        {
            return new PointD(_x - offset.X, _y - offset.Y);
        }

        /// <summary>
        /// Returns the difference of two points by subtracting the specified X and Y values.
        /// </summary>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="offsetY">The offset Y.</param>
        /// <returns></returns>
        /// <remarks>This method subtracts the X and Y coordinates and returns a new point at that location.</remarks>
        public PointD Subtract(double offsetX, double offsetY)
        {
            return new PointD(_x - offsetX, _y - offsetY);
        }

        /// <summary>
        /// Returns the product of two points by multiplying X and Y values together.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        /// <remarks>This method multiplies the X and Y coordinates together and returns a new point at that location.  This
        /// is typically used to scale a point from one coordinate system to another.</remarks>
        public PointD Multiply(PointD offset)
        {
            return new PointD(_x * offset.X, _y * offset.Y);
        }

        /// <summary>
        /// Multiplies the specified offset X.
        /// </summary>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="offsetY">The offset Y.</param>
        /// <returns></returns>
        public PointD Multiply(double offsetX, double offsetY)
        {
            return new PointD(_x * offsetX, _y * offsetY);
        }

        /// <summary>
        /// Divides the specified offset.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public PointD Divide(PointD offset)
        {
            return new PointD(_x / offset.X, _y / offset.Y);
        }

        /// <summary>
        /// Divides the specified offset X.
        /// </summary>
        /// <param name="offsetX">The offset X.</param>
        /// <param name="offsetY">The offset Y.</param>
        /// <returns></returns>
        public PointD Divide(double offsetX, double offsetY)
        {
            return new PointD(_x / offsetX, _y / offsetY);
        }

        #endregion Operators

        #region Conversions

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.PointD"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator string(PointD value)
        {
            return value.ToString();
        }

        #endregion Conversions

        #region IEquatable<PointD> Members

        /// <summary>
        /// Equalses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Equals(PointD value)
        {
            return (_x == value.X) && (_y == value.Y);
        }

        /// <summary>
        /// Equalses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="precision">The precision.</param>
        /// <returns></returns>
        public bool Equals(PointD value, int precision)
        {
            return ((Math.Round(_x, precision) == Math.Round(value.X, precision))
                && (Math.Round(_y, precision) == Math.Round(value.Y, precision)));
        }

        #endregion IEquatable<PointD> Members

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

            return _x.ToString(format, formatProvider)
                + culture.TextInfo.ListSeparator
                + _y.ToString(format, formatProvider);
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
            writer.WriteAttributeString("X",
                        _x.ToString("G17", CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Y",
                        _y.ToString("G", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            double.TryParse(reader.GetAttribute("X"), NumberStyles.Any, CultureInfo.InvariantCulture, out _x);
            double.TryParse(reader.GetAttribute("Y"), NumberStyles.Any, CultureInfo.InvariantCulture, out _y);
        }

        #endregion IXmlSerializable Members
    }
}