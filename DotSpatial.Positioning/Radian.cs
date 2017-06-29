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
    /// Represents a unit of angular measurement used during trigonometric
    /// equations.
    /// </summary>
    /// <remarks><para>A radian is a unit of measure of an angle formed by an arc whose length is
    /// the same as the circle's radius, making a shape similar to a slice of pizza.
    /// Radians are typically used during trigonometric calculations such as calculating
    /// the distance between two points on Earth's curved surface.</para>
    ///   <para>Instances of this class are guaranteed to be thread-safe because the class is
    /// immutable (its properties can only be changed during constructors).</para></remarks>
    [TypeConverter("DotSpatial.Positioning.Design.RadianConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=1.0.0.0, PublicKeyToken=b4b0b185210c9dae")]
#endif
    public struct Radian : IFormattable, IEquatable<Radian>, IComparable<Radian>, IXmlSerializable
    {
        /// <summary>
        ///
        /// </summary>
        private double _value;

        #region Constants

        // Public?

        /// <summary>
        ///
        /// </summary>
        public const double RADIANS_PER_DEGREE = Math.PI / 180.0;
        /// <summary>
        ///
        /// </summary>
        public const double DEGREES_PER_RADIAN = 180.0 / Math.PI;

        #endregion Constants

        #region Fields

        /// <summary>
        /// Represents a radian with a value of zero.
        /// </summary>
        public static readonly Radian Empty = new Radian(0.0);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance with the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <remarks>this constructor is typically used to initialize an instance when the radian
        /// value is already known.</remarks>
        public Radian(double value)
        {
            _value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Radian"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Radian(int value)
        {
            _value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Radian"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Radian(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Radian"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        public Radian(string value, CultureInfo culture)
        {
            _value = double.Parse(value, culture);
        }

        /// <summary>
        /// Creates a new instance by deserializing the specified XML.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public Radian(XmlReader reader)
        {
            // Initialize all fields
            _value = Double.NaN;

            // Deserialize the object from XML
            ReadXml(reader);
        }

        #endregion Constructors

        #region Public Properties

        // Returns/sets the number of radians
        /// <summary>
        /// Represents the numeric portion of a radian measurement.
        /// </summary>
        /// <value>A <strong>Double</strong> value indicating an angular measurement expressed in
        /// radians.</value>
        /// <remarks>This property stores the numeric radian measurement. A radian can be converted into a degree
        /// measurements via the <see cref="Radian.ToAngle(Radian)">ToAngle</see> method.</remarks>
        public double Value
        {
            get
            {
                return _value;
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns the cosine of the current instance.
        /// </summary>
        /// <returns></returns>
        public Radian Cosine()
        {
            return new Radian(Math.Cos(_value));
        }

        /// <summary>
        /// Sines this instance.
        /// </summary>
        /// <returns></returns>
        public Radian Sine()
        {
            return new Radian(Math.Sin(_value));
        }

        /// <summary>
        /// Tangents this instance.
        /// </summary>
        /// <returns></returns>
        public Radian Tangent()
        {
            return new Radian(Math.Tan(_value));
        }

        /// <summary>
        /// Squares the root.
        /// </summary>
        /// <returns></returns>
        public Radian SquareRoot()
        {
            return new Radian(Math.Sqrt(_value));
        }

        /// <summary>
        /// Returns the absolute value of the current instance.
        /// </summary>
        /// <returns></returns>
        public Radian AbsoluteValue()
        {
            return new Radian(Math.Abs(_value));
        }

        /// <summary>
        /// Returns the arccosine of the current instance.
        /// </summary>
        /// <returns></returns>
        public Radian ArcCosine()
        {
            return new Radian(Math.Acos(_value));
        }

        /// <summary>
        /// Returns the arcsine of the current instance.
        /// </summary>
        /// <returns></returns>
        public Radian ArcSine()
        {
            return new Radian(Math.Asin(_value));
        }

        /// <summary>
        /// Returns the arctangent of the current instance.
        /// </summary>
        /// <returns></returns>
        public Radian ArcTangent()
        {
            return new Radian(Math.Atan(_value));
        }

#if !PocketPC

        /// <summary>
        /// Logarithms the specified new base.
        /// </summary>
        /// <param name="newBase">The new base.</param>
        /// <returns></returns>
        public Radian Logarithm(double newBase)
        {
            return new Radian(Math.Log(_value, newBase));
        }

#endif

        /// <summary>
        /// Logarithms the base10.
        /// </summary>
        /// <returns></returns>
        public Radian LogarithmBase10()
        {
            return new Radian(Math.Log10(_value));
        }

        /// <summary>
        /// Converts the current instance into an <strong>Angle</strong> object.
        /// </summary>
        /// <returns>An <strong>Angle</strong> object.</returns>
        /// <remarks>This method is typically used to convert a radian measurement back to latitude or
        /// longitude after a trigonometric formula has completed.</remarks>
        public double ToDegrees()
        {
            return _value / RADIANS_PER_DEGREE;
        }

        /// <summary>
        /// Converts the current instance into an <strong>Angle</strong> object.
        /// </summary>
        /// <returns>An <strong>Angle</strong> object.</returns>
        /// <remarks>This method is typically used to convert a radian measurement back to latitude or
        /// longitude after a trigonometric formula has completed.</remarks>
        public Angle ToAngle()
        {
            return new Angle(ToDegrees());
        }

        /// <summary>
        /// Converts the current instance to a latitude.
        /// </summary>
        /// <returns></returns>
        public Latitude ToLatitude()
        {
            return new Latitude(ToDegrees());
        }

        /// <summary>
        /// Converts the current instance to a longitude.
        /// </summary>
        /// <returns></returns>
        public Longitude ToLongitude()
        {
            return new Longitude(ToDegrees());
        }

        /// <summary>
        /// Outputs the speed measurement as a formatted string using the specified
        /// format.
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
            // Compare only with objects of the same type
            if (obj is Radian)
                return Equals((Radian)obj);
            return false;
        }

        /// <summary>
        /// Returns the unique code for this instance used in hash tables.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture); // Always support "g" as a default format
        }

        #endregion Overrides

        #region Static Methods

        /// <summary>
        /// Converts the specified value in degrees into radians.
        /// </summary>
        /// <param name="value">A <strong>Double</strong> containing the value to convert.</param>
        /// <returns></returns>
        public static Radian FromDegrees(double value)
        {
            return new Radian(value * RADIANS_PER_DEGREE);
        }

        /// <summary>
        /// Converts the specified value in degrees into radians.
        /// </summary>
        /// <param name="value">An <strong>Angle</strong> containing the value to convert.</param>
        /// <returns></returns>
        public static Radian FromAngle(Angle value)
        {
            return new Radian(value.DecimalDegrees * RADIANS_PER_DEGREE);
        }

        /// <summary>
        /// Converts the specified value from radians to degrees.
        /// </summary>
        /// <param name="radians">The radians.</param>
        /// <returns>A <strong>Double</strong> measuring degrees.</returns>
        /// <remarks>This method is typically used to convert a radian measurement back to latitude or
        /// longitude after a trigonometric formula has completed.</remarks>
        public static double ToDegrees(double radians)
        {
            return radians / RADIANS_PER_DEGREE;
        }

        /// <summary>
        /// Converts a Radian object into decimal degrees.
        /// </summary>
        /// <param name="value">A <strong>Radian</strong> object to convert to an <strong>Angle</strong>.</param>
        /// <returns>An <strong>Angle</strong> object containing the converted value.</returns>
        /// <remarks>This method is typically used for trigonometric functions which work with values expressed as radians.  Then the formula has completed, results are converted from radians to decimal degrees to make them easier to use.</remarks>
        public static Angle ToAngle(Radian value)
        {
            return value.ToAngle();
        }

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Radian Parse(string value)
        {
            return new Radian(value, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static Radian Parse(string value, CultureInfo culture)
        {
            return new Radian(value, culture);
        }

        #endregion Static Methods

        #region Operators

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Radian operator +(Radian left, Radian right)
        {
            return left.Add(right);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Radian operator -(Radian left, Radian right)
        {
            return left.Subtract(right);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Radian operator *(Radian left, Radian right)
        {
            return left.Multiply(right);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Radian operator /(Radian left, Radian right)
        {
            return left.Divide(right);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Radian left, Radian right)
        {
            return left.CompareTo(right) < 0;
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(Radian left, Radian right)
        {
            return left.CompareTo(right) < 0 || left.Equals(right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Radian left, Radian right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Radian left, Radian right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(Radian left, Radian right)
        {
            return left.CompareTo(right) > 0 || left.Equals(right);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Radian left, Radian right)
        {
            return left.CompareTo(right) > 0;
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Radian operator +(Radian left, double right)
        {
            return left.Add(right);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Radian operator -(Radian left, double right)
        {
            return left.Subtract(right);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Radian operator *(Radian left, double right)
        {
            return left.Multiply(right);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static Radian operator /(Radian left, double right)
        {
            return left.Divide(right);
        }

        /// <summary>
        /// Implements the operator &lt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(Radian left, double right)
        {
            return left.IsLessThan(right);
        }

        /// <summary>
        /// Implements the operator &lt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <=(Radian left, double right)
        {
            return left.IsLessThanOrEqualTo(right);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Radian left, double right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Radian left, double right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Implements the operator &gt;=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >=(Radian left, double right)
        {
            return left.IsGreaterThanOrEqualTo(right);
        }

        /// <summary>
        /// Implements the operator &gt;.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(Radian left, double right)
        {
            return left.IsGreaterThan(right);
        }

        /// <summary>
        /// Adds the current instance to the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Radian Add(Radian value)
        {
            return Add(value.Value);
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Radian Add(double value)
        {
            return new Radian(_value + value);
        }

        /// <summary>
        /// Subtracts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Radian Subtract(Radian value)
        {
            return Subtract(value.Value);
        }

        /// <summary>
        /// Subtracts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Radian Subtract(double value)
        {
            return new Radian(_value - value);
        }

        /// <summary>
        /// Multiplies the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Radian Multiply(Radian value)
        {
            return Multiply(value.Value);
        }

        /// <summary>
        /// Multiplies the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Radian Multiply(double value)
        {
            return new Radian(_value * value);
        }

        /// <summary>
        /// Returns the current value divided by the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Radian Divide(Radian value)
        {
            return new Radian(Value / value.Value);
        }

        /// <summary>
        /// Divides the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Radian Divide(double value)
        {
            return new Radian(_value / value);
        }

        /// <summary>
        /// Increments this instance.
        /// </summary>
        /// <returns></returns>
        public Radian Increment()
        {
            return new Radian(_value + 1.0);
        }

        /// <summary>
        /// Returns the current value decreased by one.
        /// </summary>
        /// <returns></returns>
        public Radian Decrement()
        {
            return new Radian(_value - 1.0);
        }

        /// <summary>
        /// Determines whether [is less than] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is less than] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsLessThan(Radian value)
        {
            return _value < value.Value;
        }

        /// <summary>
        /// Determines whether [is less than] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is less than] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsLessThan(double value)
        {
            return _value < value;
        }

        /// <summary>
        /// Determines whether [is less than or equal to] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is less than or equal to] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsLessThanOrEqualTo(Radian value)
        {
            return _value <= value.Value;
        }

        /// <summary>
        /// Determines whether [is less than or equal to] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is less than or equal to] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsLessThanOrEqualTo(double value)
        {
            return _value <= value;
        }

        /// <summary>
        /// Determines whether [is greater than] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is greater than] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsGreaterThan(Radian value)
        {
            return _value > value.Value;
        }

        /// <summary>
        /// Determines whether [is greater than] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is greater than] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsGreaterThan(double value)
        {
            return _value > value;
        }

        /// <summary>
        /// Determines whether [is greater than or equal to] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is greater than or equal to] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsGreaterThanOrEqualTo(Radian value)
        {
            return _value >= value.Value;
        }

        /// <summary>
        /// Determines whether [is greater than or equal to] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if [is greater than or equal to] [the specified value]; otherwise, <c>false</c>.</returns>
        public bool IsGreaterThanOrEqualTo(double value)
        {
            return _value >= value;
        }

        #endregion Operators

        #region Conversions

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Radian"/> to <see cref="System.Double"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator double(Radian value)
        {
            return value.Value;
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Longitude"/> to <see cref="DotSpatial.Positioning.Radian"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Radian(Longitude value)
        {
            return value.ToRadians();
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Latitude"/> to <see cref="DotSpatial.Positioning.Radian"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Radian(Latitude value)
        {
            return value.ToRadians();
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Azimuth"/> to <see cref="DotSpatial.Positioning.Radian"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Radian(Azimuth value)
        {
            return value.ToRadians();
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="DotSpatial.Positioning.Angle"/> to <see cref="DotSpatial.Positioning.Radian"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Radian(Angle value)
        {
            return value.ToRadians();
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Double"/> to <see cref="DotSpatial.Positioning.Radian"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Radian(double value)
        {
            return new Radian(value);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.String"/> to <see cref="DotSpatial.Positioning.Radian"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Radian(string value)
        {
            return new Radian(value, CultureInfo.CurrentCulture);
        }

        #endregion Conversions

        #region IEquatable<Radian> Members

        /// <summary>
        /// Equalses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Equals(Radian value)
        {
            return _value.Equals(value.Value);
        }

        /// <summary>
        /// Equalses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="decimals">The decimals.</param>
        /// <returns></returns>
        public bool Equals(Radian value, int decimals)
        {
            return Math.Round(_value, decimals) == Math.Round(value.Value, decimals);
        }

        #endregion IEquatable<Radian> Members

        #region IComparable<Radian> Members

        /// <summary>
        /// Compares the current instance with the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public int CompareTo(Radian value)
        {
            return _value.CompareTo(value.Value);
        }

        #endregion IComparable<Radian> Members

        #region IFormattable Members

        /// <summary>
        /// Outputs the speed measurement as a formatted string using the specified format
        /// and culture information.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider);
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
            writer.WriteString(_value.ToString("G17", CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            _value = reader.NodeType == XmlNodeType.Text ? reader.ReadContentAsDouble() : reader.ReadElementContentAsDouble();
        }

        #endregion IXmlSerializable Members
    }
}