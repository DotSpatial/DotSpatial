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
using System.ComponentModel;
using System.Globalization;

namespace DotSpatial.Positioning
{
#if !PocketPC || DesignTime
    /// <summary>
    /// Represents a number as a fraction of one hundred.
    /// </summary>
    /// <example>
    /// These examples create <strong>Percent</strong> objects using different
    /// constructors.
    ///   <code lang="CS" description="This example creates a percentage of 25% using the decimal value 0.25.">
    /// // Create a percentage of 25%
    /// Percent twentyFivePercent = new Percent(0.25f);
    ///   </code>
    ///   <code lang="VB" description="This example creates a percentage of 25% using the decimal value 0.25.">
    /// ' Create a percentage of 25%
    /// Dim TwentyFivePercent As New Percent(0.25)
    ///   </code>
    ///   <code lang="CS" description="This example creates a percentage of 25.4% using a string. Since not all cultures use a period (.) as a separator, a CultureInfo object is passed.">
    /// // Create a percentage of 25%
    /// Percent twentyFivePercent = New Percent("25.4%", CultureInfo.InvariantCulture);
    ///   </code>
    ///   <code lang="VB" description="This example creates a percentage of 25.4% using a string. Since not all cultures use a period (.) as a separator, a CultureInfo object is passed.">
    /// ' Create a percentage of 25%
    /// Dim TwentyFivePercent As New Percent("25.5%", CultureInfo.InvariantCulture)
    ///   </code>
    ///   </example>
    /// <remarks><para>This class is used to express one quantity relative to another quantity.
    /// Percentage values are presented in string form using the percent symbol of the
    /// local culture (usually the percent symbol "%"). When percentage values are
    /// expressed in decimal form the value is divided by one hundred. In other words, the
    /// value "25%" is equivalent to 0.25.</para>
    ///   <para>This class is culture-sensitive, meaning that both the percent symbol and the
    /// numeric format is interpreted and presented differently depending on the local
    /// culture. As a result, the <strong>CultureInfo</strong> object should be used any
    /// time a value is parsed from a <strong>String</strong> or output as a String using
    /// the <strong>ToString</strong> method.</para></remarks>
    [TypeConverter("DotSpatial.Positioning.Design.PercentConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=d77afaeb30e3236a")]
#endif
    public struct Percent : IFormattable, IComparable<Percent>, IEquatable<Percent>
    {
        /// <summary>
        ///
        /// </summary>
        private readonly float _value;

        #region Fields

        /// <summary>
        /// Represents a percentage value of zero.
        /// </summary>
        /// <value>
        /// A <strong>Percentage</strong> value, representing <strong>0%</strong> and
        ///   <strong>0.0</strong>.
        ///   </value>
        public static readonly Percent Zero = new Percent(0.0f);
        /// <summary>
        /// Represents a value of one hundred percent.
        /// </summary>
        /// <value>
        /// A <strong>Percentage</strong> value, meaning <strong>100%</strong> or
        ///   <strong>1.0</strong>.
        ///   </value>
        public static readonly Percent OneHundredPercent = new Percent(1.0f);
        /// <summary>
        /// Represents a percentage of fifty percent.
        /// </summary>
        /// <value>
        /// A <strong>Percentage</strong> value, representing <strong>50%</strong> or
        ///   <strong>0.5</strong>.
        ///   </value>
        public static readonly Percent FiftyPercent = new Percent(0.5f);
        /// <summary>
        ///
        /// </summary>
        public static readonly Percent TenPercent = new Percent(0.1f);
        /// <summary>
        ///
        /// </summary>
        public static readonly Percent TwentyPercent = new Percent(0.2f);
        /// <summary>
        ///
        /// </summary>
        public static readonly Percent ThirtyPercent = new Percent(0.3f);
        /// <summary>
        ///
        /// </summary>
        public static readonly Percent FortyPercent = new Percent(0.4f);
        /// <summary>
        ///
        /// </summary>
        public static readonly Percent SixtyPercent = new Percent(0.6f);
        /// <summary>
        ///
        /// </summary>
        public static readonly Percent SeventyPercent = new Percent(0.7f);
        /// <summary>
        ///
        /// </summary>
        public static readonly Percent EightyPercent = new Percent(0.8f);
        /// <summary>
        ///
        /// </summary>
        public static readonly Percent NinetyPercent = new Percent(0.9f);
        /// <summary>
        /// Represents a value of twenty-five percent.
        /// </summary>
        /// <value>
        /// A <strong>Percentage</strong> value, representing <strong>25%</strong> or
        ///   <strong>0.25</strong>.
        ///   </value>
        public static readonly Percent TwentyFivePercent = new Percent(0.25f);

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public Percent(float value)
        {
            _value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Percent"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        public Percent(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Percent"/> struct.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="culture">The culture.</param>
        public Percent(string value, CultureInfo culture)
        {
            // Replace the "%" percent sign cuz .Net formating does EVERYTHING else a
            // number string could have in it but not a freekun % sign.
            value = value.Trim().Replace(culture.NumberFormat.PercentSymbol, string.Empty);

            /* Parse the value as a float, then divide by 100.  In other words,
             * "15%" will become 0.15f
             */
            _value = float.Parse(value, NumberStyles.Any, culture.NumberFormat) * 0.01f;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Returns the decimal value of the percentage.
        /// </summary>
        /// <remarks>The value of a <strong>Percent</strong> object is 1/100th of the
        /// percentage.  In other words, if the percentage is "15%" then the <strong>Value</strong>
        /// property will return <strong>0.15</strong>, and a percentage of "100%" means a
        /// <strong>Value</strong> of <strong>1.0</strong>.</remarks>
        public float Value
        {
            get
            {
                return _value;
            }
        }

        /// <summary>
        /// Returns whether the value equals zero.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _value.Equals(0);
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Returns the percentage of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public double PercentageOf(double value)
        {
            return value * _value;
        }

        /// <summary>
        /// Returns the percentage of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public float PercentageOf(float value)
        {
            return value * _value;
        }

        /// <summary>
        /// Returns the percentage of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public float PercentageOf(int value)
        {
            return value * _value;
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
            if (ReferenceEquals(obj, null))
                return false;
            if (!(obj is Percent))
                return false;

            return ((Percent)obj).Value.Equals(_value);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Returns the percentage formatted as a <strong>String</strong>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString("P", CultureInfo.CurrentCulture);
        }

        #endregion Overrides

        #region Operators

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Percent left, Percent right)
        {
            return left.Value.Equals(right.Value);
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Percent left, float right)
        {
            return left.Value.Equals(right);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Percent left, Percent right)
        {
            return !left.Value.Equals(right.Value);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Percent left, float right)
        {
            return !left.Value.Equals(right);
        }

        /// <summary>
        /// Returns the percentage of the specified value.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static double operator *(Percent left, double right)
        {
            return left.PercentageOf(right);
        }

        /// <summary>
        /// Returns the percentage of the specified value.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static float operator *(Percent left, float right)
        {
            return left.PercentageOf(right);
        }

        /// <summary>
        /// Returns the percentage of the specified value.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>The result of the operator.</returns>
        public static float operator *(Percent left, int right)
        {
            return left.PercentageOf(right);
        }

        /// <summary>
        /// Returns the percentage of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Percent Multiply(double value)
        {
            return new Percent(Convert.ToSingle(_value * value));
        }

        /// <summary>
        /// Returns the percentage of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Percent Multiply(float value)
        {
            return new Percent(_value * value);
        }

        /// <summary>
        /// Returns the percentage of the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public Percent Multiply(int value)
        {
            return new Percent(_value * value);
        }

        #endregion Operators

        #region Conversions

        /// <summary>
        /// Converts the specified value to a <strong>Percent</strong> object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Percent(float value)
        {
            return new Percent(value);
        }

        /// <summary>
        /// Converts the specified value to a <strong>Percent</strong> object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Percent(double value)
        {
            return new Percent(Convert.ToSingle(value));
        }

        /// <summary>
        /// Converts the specified value to a <strong>Percent</strong> object.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Percent(int value)
        {
            return new Percent(Convert.ToSingle(value));
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.String"/> to <see cref="DotSpatial.Positioning.Percent"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Percent(string value)
        {
            return new Percent(value, CultureInfo.CurrentCulture);
        }

        #endregion Conversions

        #region IFormattable Members

        /// <summary>
        /// Returns the percentage formatted as a <strong>String</strong>.
        /// </summary>
        /// <param name="format">The format to use.-or- A null reference (Nothing in Visual Basic) to use the default format defined for the type of the <see cref="T:System.IFormattable"/> implementation.</param>
        /// <param name="formatProvider">The provider to use to format the value.-or- A null reference (Nothing in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
        /// <returns>A <see cref="System.String"/> that represents this instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider).Replace(" ", string.Empty);
        }

        #endregion IFormattable Members

        #region IEquatable<Percent> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.</returns>
        public bool Equals(Percent other)
        {
            return _value.Equals(other.Value);
        }

        #endregion IEquatable<Percent> Members

        #region IComparable<Percent> Members

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This object is less than the <paramref name="other"/> parameter.
        /// Zero
        /// This object is equal to <paramref name="other"/>.
        /// Greater than zero
        /// This object is greater than <paramref name="other"/>.</returns>
        public int CompareTo(Percent other)
        {
            return _value.CompareTo(other.Value);
        }

        #endregion IComparable<Percent> Members
    }
}