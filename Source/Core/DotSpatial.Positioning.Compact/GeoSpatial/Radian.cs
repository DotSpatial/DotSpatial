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
	/// Represents a unit of angular measurement used during trigonometric
	/// equations.
	/// </summary>
	/// <remarks>
	/// 	<para>A radian is a unit of measure of an angle formed by an arc whose length is
	///     the same as the circle's radius, making a shape similar to a slice of pizza.
	///     Radians are typically used during trigonometric calculations such as calculating
	///     the distance between two points on Earth's curved surface.</para>
	/// 	<para>Instances of this class are guaranteed to be thread-safe because the class is
	///     immutable (its properties can only be changed during constructors).</para>
	/// </remarks>
#if !PocketPC || DesignTime
    [TypeConverter("DotSpatial.Positioning.Design.RadianConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=d77afaeb30e3236a")]
#endif
    public struct Radian : IFormattable, IEquatable<Radian>, IComparable<Radian>, IXmlSerializable
    {
        private double _Value;

        #region Constants

        // Public?

        public const double RadiansPerDegree = Math.PI / 180.0; 
        public const double DegreesPerRadian = 180.0 / Math.PI; 

        #endregion

        #region Fields

        /// <summary>Represents a radian with a value of zero.</summary>
		public static readonly Radian Empty = new Radian(0.0);

        #endregion

        #region Constructors

        /// <summary>Creates a new instance with the specified value.</summary>
        /// <remarks>
        /// this constructor is typically used to initialize an instance when the radian
        /// value is already known.
        /// </remarks>
        /// <param name="value">A value to store in the <strong>Value</strong> property.</param>
        public Radian(double value)
        {
            _Value = value;
        }

        public Radian(int value)
        {
            _Value = (double)value;
        }

        public Radian(string value)
            : this(value, CultureInfo.CurrentCulture)
        {}

        public Radian(string value, CultureInfo culture)
        {
            _Value = double.Parse(value, culture);
        }

        /// <summary>
        /// Creates a new instance by deserializing the specified XML.
        /// </summary>
        /// <param name="reader"></param>
        public Radian(XmlReader reader)
        {
            // Initialize all fields
            _Value = Double.NaN;

            // Deserialize the object from XML
            ReadXml(reader);
        }

        #endregion

        #region Public Properties

        // Returns/sets the number of radians
        /// <summary>Represents the numeric portion of a radian measurement.</summary>
        /// <value>
        /// A <strong>Double</strong> value indicating an angular measurement expressed in
        /// radians.
        /// </value>
        /// <remarks>
        /// This property stores the numeric radian measurement. A radian can be converted into a degree
        /// measurements via the <see cref="Radian.ToAngle(Radian)">ToAngle</see> method.
        /// </remarks>
        public double Value
        {
            get
            {
                return _Value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>Returns the cosine of the current instance.</summary>
        public Radian Cosine()
        {
            return new Radian(Math.Cos(_Value));
        }

        public Radian Sine()
        {
            return new Radian(Math.Sin(_Value));
        }

        public Radian Tangent()
        {
            return new Radian(Math.Tan(_Value));
        }

        public Radian SquareRoot()
        {
            return new Radian(Math.Sqrt(_Value));
        }

        /// <summary>Returns the absolute value of the current instance.</summary>
        public Radian AbsoluteValue()
        {
            return new Radian(Math.Abs(_Value));
        }

        /// <summary>Returns the arccosine of the current instance.</summary>
        public Radian ArcCosine()
        {
            return new Radian(Math.Acos(_Value));
        }

        /// <summary>Returns the arcsine of the current instance.</summary>
        public Radian ArcSine()
        {
            return new Radian(Math.Asin(_Value));
        }

        /// <summary>Returns the arctangent of the current instance.</summary>
        public Radian ArcTangent()
        {
            return new Radian(Math.Atan(_Value));
        }

#if !PocketPC
        public Radian Logarithm(double newBase)
        {
            return new Radian(Math.Log(_Value, newBase));
        }
#endif

        public Radian LogarithmBase10()
        {
            return new Radian(Math.Log10(_Value));
        }

        /// <summary>Converts the current instance into an <strong>Angle</strong> object.</summary>
        /// <returns>An <strong>Angle</strong> object.</returns>
        /// <remarks>
        /// This method is typically used to convert a radian measurement back to latitude or
        /// longitude after a trigonometric formula has completed.
        /// </remarks>
        public double ToDegrees()
        {
            return _Value / RadiansPerDegree;
        }

        /// <summary>Converts the current instance into an <strong>Angle</strong> object.</summary>
        /// <returns>An <strong>Angle</strong> object.</returns>
        /// <remarks>
        /// This method is typically used to convert a radian measurement back to latitude or
        /// longitude after a trigonometric formula has completed.
        /// </remarks>
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
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            // Compare only with objects of the same type
            if (obj is Radian)
                return Equals((Radian)obj);
            return false;
        }

        /// <summary>Returns the unique code for this instance used in hash tables.</summary>
        public override int GetHashCode()
        {
            return _Value.GetHashCode();
        }

        public override string ToString()
        {
            return ToString("g", CultureInfo.CurrentCulture); // Always support "g" as a default format
        }

        #endregion

        #region Static Methods


        /// <summary>
        /// Converts the specified value in degrees into radians.
        /// </summary>
        /// <param name="value">A <strong>Double</strong> containing the value to convert.</param>
        public static Radian FromDegrees(double value)
        {
            return new Radian(value * RadiansPerDegree);
        }

        /// <summary>
        /// Converts the specified value in degrees into radians.
        /// </summary>
        /// <param name="value">An <strong>Angle</strong> containing the value to convert.</param>
        public static Radian FromAngle(Angle value)
        {
            return new Radian(value.DecimalDegrees * RadiansPerDegree);
        }

        /// <summary>Converts the specified value from radians to degrees.</summary>
        /// <returns>A <strong>Double</strong> measuring degrees.</returns>
        /// <remarks>
        /// This method is typically used to convert a radian measurement back to latitude or
        /// longitude after a trigonometric formula has completed.
        /// </remarks>
        public static double ToDegrees(double radians)
        {
            return radians / RadiansPerDegree;
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

        public static Radian Parse(string value)
        {
            return new Radian(value, CultureInfo.CurrentCulture);
        }

        public static Radian Parse(string value, CultureInfo culture)
        {
            return new Radian(value, culture);
        }

        #endregion

		#region Operators

		public static Radian operator +(Radian left, Radian right) 
		{
			return left.Add(right);
		}	

		public static Radian operator -(Radian left, Radian right) 
		{
			return left.Subtract(right);
		}

		public static Radian operator *(Radian left, Radian right) 
		{
			return left.Multiply(right);
		}

		public static Radian operator /(Radian left, Radian right) 
		{
			return left.Divide(right);
		}

		public static bool operator <(Radian left, Radian right) 
		{
			return left.CompareTo(right) < 0;
		}

		public static bool operator <=(Radian left, Radian right) 
		{
			return left.CompareTo(right) < 0 || left.Equals(right);
		}

		public static bool operator ==(Radian left, Radian right) 
		{
			return left.Equals(right);
		}

		public static bool operator !=(Radian left, Radian right) 
		{
			return !(left == right);
		}

		public static bool operator >=(Radian left, Radian right) 
		{
			return left.CompareTo(right) > 0 || left.Equals(right);
		}
	
		public static bool operator >(Radian left, Radian right) 
		{
			return left.CompareTo(right) > 0;
		}

        public static Radian operator +(Radian left, double right)
        {
            return left.Add(right);
        }

        public static Radian operator -(Radian left, double right)
        {
            return left.Subtract(right);
        }

        public static Radian operator *(Radian left, double right)
        {
            return left.Multiply(right);
        }

        public static Radian operator /(Radian left, double right)
        {
            return left.Divide(right);
        }

        public static bool operator <(Radian left, double right)
        {
            return left.IsLessThan(right);
        }

        public static bool operator <=(Radian left, double right)
        {
            return left.IsLessThanOrEqualTo(right);
        }

        public static bool operator ==(Radian left, double right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Radian left, double right)
        {
            return !(left == right);
        }

        public static bool operator >=(Radian left, double right)
        {
            return left.IsGreaterThanOrEqualTo(right);
        }

        public static bool operator >(Radian left, double right)
        {
            return left.IsGreaterThan(right);
        }

        /// <summary>Adds the current instance to the specified value.</summary>
        public Radian Add(Radian value)
        {
            return Add(value.Value);
        }

        public Radian Add(double value)
        {
            return new Radian(_Value + value);
        }

        public Radian Subtract(Radian value)
        {
            return Subtract(value.Value);
        }

        public Radian Subtract(double value)
        {
            return new Radian(_Value - value);
        }

        public Radian Multiply(Radian value)
        {
            return Multiply(value.Value);
        }

        public Radian Multiply(double value)
        {
            return new Radian(_Value * value);
        }

        /// <summary>Returns the current value divided by the specified value.</summary>
        public Radian Divide(Radian value)
        {
            return new Radian(Value / value.Value);
        }

        public Radian Divide(double value)
        {
            return new Radian(_Value / value);
        }

        public Radian Increment()
        {
            return new Radian(_Value + 1.0);
        }

        /// <summary>Returns the current value decreased by one.</summary>
        public Radian Decrement()
        {
            return new Radian(_Value - 1.0);
        }

        public bool IsLessThan(Radian value)
        {
            return _Value < value.Value;
        }

        public bool IsLessThan(double value)
        {
            return _Value < value;
        }

        public bool IsLessThanOrEqualTo(Radian value)
        {
            return _Value <= value.Value;
        }

        public bool IsLessThanOrEqualTo(double value)
        {
            return _Value <= value;
        }

        public bool IsGreaterThan(Radian value)
        {
            return _Value > value.Value;
        }

        public bool IsGreaterThan(double value)
        {
            return _Value > value;
        }

        public bool IsGreaterThanOrEqualTo(Radian value)
        {
            return _Value >= value.Value;
        }

        public bool IsGreaterThanOrEqualTo(double value)
        {
            return _Value >= value;
        }

		#endregion

		#region Conversions
		
        public static explicit operator double(Radian value)
		{
			return value.Value;
		}

		public static explicit operator Radian(Longitude value) 
		{
            return value.ToRadians();
		}

        public static explicit operator Radian(Latitude value) 
		{
            return value.ToRadians();
		}
		
        public static explicit operator Radian(Azimuth value) 
		{
            return value.ToRadians();
		}

		public static explicit operator Radian(Angle value) 
		{
            return value.ToRadians();
		}

		public static explicit operator Radian(double value) 
		{
			return new Radian(value);
		}

		public static explicit operator Radian(string value) 
		{
			return new Radian(value, CultureInfo.CurrentCulture);
		}

		#endregion

        #region IEquatable<Radian> Members

        public bool Equals(Radian value)
        {
            return _Value.Equals(value.Value);
        }

        public bool Equals(Radian value, int decimals)
        {
            return Math.Round(_Value, decimals) == Math.Round(value.Value, decimals);
        }

        #endregion

        #region IComparable<Radian> Members

        /// <summary>Compares the current instance with the specified value.</summary>
        public int CompareTo(Radian value)
        {
            return _Value.CompareTo(value.Value);
        }

        #endregion

        #region IFormattable Members

        /// <summary>
        /// Outputs the speed measurement as a formatted string using the specified format
        /// and culture information.
        /// </summary>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _Value.ToString(format, formatProvider);
        }

        #endregion

        #region IXmlSerializable Members

       
        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(_Value.ToString("G17", CultureInfo.InvariantCulture));
        }

        public void ReadXml(XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.Text)
                _Value = reader.ReadContentAsDouble();
            else
                _Value = reader.ReadElementContentAsDouble();
        }

        #endregion
    }
}