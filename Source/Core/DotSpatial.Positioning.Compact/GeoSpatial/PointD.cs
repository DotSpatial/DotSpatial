using System;
using System.IO;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Security.Permissions;
#if !PocketPC || DesignTime
using System.Runtime.Serialization;
using System.ComponentModel;
#endif
using System.Reflection;

namespace DotSpatial.Positioning
{
	/// <summary>Represents a highly-precise pixel coordinate.</summary>
	/// <remarks>
	/// 	<para>This class behaves similar to the <strong>PointF</strong> structure in the
	///     <strong>System.Drawing</strong> namespace, except that it supports double-precision
	///     values and can be converted into a geographic coordinate. This structure is also
	///     supported on the Compact Framework version of the <strong>DotSpatial.Positioning</strong>,
	///     whereas <strong>PointF</strong> is not.</para>
	/// 	<para>Instances of this class are guaranteed to be thread-safe because the class is
	///     immutable (its properties can only be changed via constructors).</para>
	/// </remarks>
#if !PocketPC || DesignTime
    [TypeConverter("DotSpatial.Positioning.Design.PointDConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=d77afaeb30e3236a")]
#endif
    public struct PointD : IFormattable, IEquatable<PointD>, IXmlSerializable
    {
        private double _X;
        private double _Y;

        #region Fields

        /// <summary>Returns a point with no value.</summary>
        public static readonly PointD Empty = new PointD(0, 0);
        /// <summary>Represents an invalid coordinate.</summary>
        public static readonly PointD Invalid = new PointD(double.NaN, double.NaN);

        #endregion

        #region Constructors

        /// <summary>Creates a new instance for the specified coordinates.</summary>
        public PointD(double x, double y)
        {
            _X = x;
            _Y = y;
        }

        public PointD(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        public PointD(string value, CultureInfo culture)
        {
            // Trim the string and remove double spaces
            value = value.Replace("  ", " ").Trim();

            // And separate it via the list separator
            string[] Values = value.Split(Convert.ToChar(culture.TextInfo.ListSeparator, culture));

            // Return the converted values
            _X = double.Parse(Values[0], NumberStyles.Any, culture);
            _Y = double.Parse(Values[1], NumberStyles.Any, culture);
        }

        public PointD(XmlReader reader)
        {
            _X = _Y = 0.0;
            ReadXml(reader);
        }

        #endregion

        #region Public Properties


        /// <summary>Gets or sets the x-coordinate of this PointD.</summary>
        public double X
        {
            get
            {
                return _X;
            }
            set
            {
                _X = value;
            }
        }
        /// <summary>
        /// For projected coordinates, this is the factor Lamda or the longitude parameter. 
        /// For readability only, the value is X.  
        /// </summary>
        public double Lam
        {
            get
            {
                return _X;
            }
            set
            {
                _X = value;
            }
        }

        /// <summary>Gets or sets the x-coordinate of this PointD.</summary>
        public double Y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
            }
        }
        /// <summary>
        /// For projected coordinates, this is the factor Phi or the latitude parameter. 
        /// For readability only, the value is Y.  
        /// </summary>
        public double Phi
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
            }
        }

        /// <summary>Returns whether the current instance has no value.</summary>
        public bool IsEmpty
        {
            get
            {
                return (_X == 0 && _Y == 0);
            }
        }

        /// <summary>Returns whether the current instance has an invalid value.</summary>
        public bool IsInvalid
        {
            get
            {
                double fail = _X * _Y;

                return (double.IsNaN(fail) || double.IsInfinity(fail));
            }
        }

        #endregion

        #region Public Methods

        /*
        /// <summary>Calculates the direction from one point to another.</summary>
        public Azimuth BearingTo(PointD value)
        {
            double Result = value.Subtract(this).ToPolarCoordinate().Theta.DecimalDegrees;
            return new Azimuth(Result).Normalize();
        }
         */

        /// <summary>Calculates the distance to another pixel.</summary>
        public double DistanceTo(PointD value)
        {
            return Math.Sqrt(Math.Abs(value.X - _X) * Math.Abs(value.X - _X)
                + Math.Abs(value.Y - _Y) * Math.Abs(value.Y - _Y));
        }

        /// <summary>
        /// Indicates if the current instance is closer to the top of the monitor than the
        /// specified value.
        /// </summary>
        public bool IsAbove(PointD value)
        {
            return _Y < value.Y;
        }

        /// <summary>
        /// Indicates if the current instance is closer to the bottom of the monitor than the
        /// specified value.
        /// </summary>
        public bool IsBelow(PointD value)
        {
            return _Y > value.Y;
        }

        /// <summary>
        /// Indicates if the current instance is closer to the left of the monitor than the
        /// specified value.
        /// </summary>
        public bool IsLeftOf(PointD value)
        {
            return _X < value.X;
        }

        /// <summary>
        /// Indicates if the current instance is closer to the right of the monitor than the
        /// specified value.
        /// </summary>
        public bool IsRightOf(PointD value)
        {
            return _X > value.X;
        }

        /// <summary>Returns the current instance with its signs switched.</summary>
        /// <remarks>This method returns a new point where the signs of X and Y are flipped.  For example, if
        /// a point, represents (20, 40), this function will return (-20, -40).</remarks>
        public PointD Mirror()
        {
            return new PointD(-_X, -_Y);
        }

        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        /// <summary>Returns the current instance rotated about (0, 0).</summary>
        public PointD Rotate(Angle angle)
        {
            return Rotate(angle.DecimalDegrees);
        }

        /// <summary>Returns the current instance rotated about (0, 0).</summary>
        public PointD Rotate(double angle)
        {
            // Convert the angle to radians
            double AngleRadians = Radian.FromDegrees(angle).Value;
            double AngleCos = Math.Cos(AngleRadians);
            double AngleSin = Math.Sin(AngleRadians);
            // Yes.  Rotate the point about 0,0
            return new PointD(AngleCos * _X - AngleSin * _Y, AngleSin * _X + AngleCos * _Y);
        }

        /// <summary>Returns the current instance rotated about the specified point.</summary>
        public PointD RotateAt(Angle angle, PointD center)
        {
            return RotateAt(angle.DecimalDegrees, center);
        }

        public PointD RotateAt(double angle, PointD center)
        {
            if (angle == 0)
                return this;

            // Shift the point by its center, rotate, then add the center back in
            return Subtract(center).Rotate(angle).Add(center);
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (obj is PointD)
                return Equals((PointD)obj);
            return false;
        }

        /// <summary>Returns a unique code used for hash tables.</summary>
        public override int GetHashCode()
        {
            return _X.GetHashCode() ^ _Y.GetHashCode();
        }

        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        #endregion

        #region Static Methods

        public static PointD Parse(string value)
        {
            return new PointD(value);
        }

        public static PointD Parse(string value, CultureInfo culture)
        {
            return new PointD(value, culture);
        }

        #endregion

        #region Operators

        public static bool operator ==(PointD left, PointD right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PointD left, PointD right)
        {
            return !left.Equals(right);
        }

        public static PointD operator +(PointD left, PointD right)
        {
            return new PointD(left.X + right.X, left.Y + right.Y);
        }

        public static PointD operator -(PointD left, PointD right)
        {
            return new PointD(left.X - right.X, left.Y - right.Y);
        }

        public static PointD operator *(PointD left, PointD right)
        {
            return new PointD(left.X * right.X, left.Y * right.Y);
        }

        public static PointD operator /(PointD left, PointD right)
        {
            return new PointD(left.X / right.X, left.Y / right.Y);
        }

        /// <summary>
        /// Returns the sum of two points by adding X and Y values together.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        /// <remarks>This method adds the X and Y coordinates and returns a new point at that location.</remarks>
        public PointD Add(PointD offset)
        {
            return new PointD(_X + offset.X, _Y + offset.Y);
            //return Offset(offset.X, offset.Y);
        }

        /// <summary>
        /// Returns the sum of two points by adding X and Y values together.
        /// </summary>
        /// <returns></returns>
        /// <remarks>This method adds the X and Y coordinates and returns a new point at that location.</remarks>
        public PointD Add(double offsetX, double offsetY)
        {
            return new PointD(_X + offsetX, _Y + offsetY);
        }

        /// <summary>
        /// Returns the difference of two points by subtracting the specified X and Y values.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        /// <remarks>This method subtracts the X and Y coordinates and returns a new point at that location.</remarks>
        public PointD Subtract(PointD offset)
        {
            return new PointD(_X - offset.X, _Y - offset.Y);
        }

        /// <summary>
        /// Returns the difference of two points by subtracting the specified X and Y values.
        /// </summary>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <returns></returns>
        /// <remarks>This method subtracts the X and Y coordinates and returns a new point at that location.</remarks>
        public PointD Subtract(double offsetX, double offsetY)
        {
            return new PointD(_X - offsetX, _Y - offsetY);
        }

        /// <summary>
        /// Returns the product of two points by multiplying X and Y values together.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        /// <remarks>This method multiplies the X and Y coordinates together and returns a new point at that location.  This
        /// is typically used to scale a point from one coordinate system to another.</remarks>
        public PointD Multiply(PointD offset)
        {
            return new PointD(_X * offset.X, _Y * offset.Y);
        }

        public PointD Multiply(double offsetX, double offsetY)
        {
            return new PointD(_X * offsetX, _Y * offsetY);
        }

        public PointD Divide(PointD offset)
        {
            return new PointD(_X / offset.X, _Y / offset.Y);
        }

        public PointD Divide(double offsetX, double offsetY)
        {
            return new PointD(_X / offsetX, _Y / offsetY);
        }

        #endregion

        #region Conversions

        public static explicit operator string(PointD value)
        {
            return value.ToString();
        }

        #endregion

        #region IEquatable<PointD> Members

        public bool Equals(PointD value)
        {
            return (_X == value.X) && (_Y == value.Y);
        }

        public bool Equals(PointD value, int precision)
        {
            return ((Math.Round(_X, precision) == Math.Round(value.X, precision))
                && (Math.Round(_Y, precision) == Math.Round(value.Y, precision)));
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

            return _X.ToString(format, formatProvider)
                + culture.TextInfo.ListSeparator
                + _Y.ToString(format, formatProvider);
        }

        #endregion

        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("X",
                        _X.ToString("G17", CultureInfo.InvariantCulture));
            writer.WriteAttributeString("Y",
                        _Y.ToString("G", CultureInfo.InvariantCulture));
        }

        public void ReadXml(XmlReader reader)
        {
            _X = double.Parse(
                reader.GetAttribute("X"), CultureInfo.InvariantCulture);
            _Y = double.Parse(
                reader.GetAttribute("Y"), CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
