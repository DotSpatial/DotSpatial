using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace DotSpatial.Positioning
{
    /// <summary>Represents a number as a fraction of one hundred.</summary>
    /// <example>
    ///     These examples create <strong>Percent</strong> objects using different
    ///     constructors.
    ///     <code lang="CS" description="This example creates a percentage of 25% using the decimal value 0.25.">
    /// // Create a percentage of 25%
    /// Percent twentyFivePercent = new Percent(0.25f);
    /// </code>
    /// 	<code lang="VB" description="This example creates a percentage of 25% using the decimal value 0.25.">
    /// ' Create a percentage of 25%
    /// Dim TwentyFivePercent As New Percent(0.25)
    /// </code>
    /// 	<code lang="CS" description="This example creates a percentage of 25.4% using a string. Since not all cultures use a period (.) as a separator, a CultureInfo object is passed.">
    /// // Create a percentage of 25%
    /// Percent twentyFivePercent = New Percent("25.4%", CultureInfo.InvariantCulture);
    /// </code>
    /// 	<code lang="VB" description="This example creates a percentage of 25.4% using a string. Since not all cultures use a period (.) as a separator, a CultureInfo object is passed.">
    /// ' Create a percentage of 25%
    /// Dim TwentyFivePercent As New Percent("25.5%", CultureInfo.InvariantCulture)
    /// </code>
    /// </example>
    /// <remarks>
    /// 	<para>This class is used to express one quantity relative to another quantity.
    ///     Percentage values are presented in string form using the percent symbol of the
    ///     local culture (usually the percent symbol "%"). When percentage values are
    ///     expressed in decimal form the value is divided by one hundred. In other words, the
    ///     value "25%" is equivalent to 0.25.</para>
    /// 	<para>This class is culture-sensitive, meaning that both the percent symbol and the
    ///     numeric format is interpreted and presented differently depending on the local
    ///     culture. As a result, the <strong>CultureInfo</strong> object should be used any
    ///     time a value is parsed from a <strong>String</strong> or output as a String using
    ///     the <strong>ToString</strong> method.</para>
    /// </remarks>
#if !PocketPC || DesignTime
    [TypeConverter("DotSpatial.Positioning.Design.PercentConverter, DotSpatial.Positioning.Design, Culture=neutral, Version=2.0.0.0, PublicKeyToken=d77afaeb30e3236a")]
#endif
    public struct Percent : IFormattable, IComparable<Percent>, IEquatable<Percent>
    {
        private readonly float _Value;

        #region Fields

        /// <summary>Represents a percentage value of zero.</summary>
        /// <value>
        /// A <strong>Percentage</strong> value, representing <strong>0%</strong> and
        /// <strong>0.0</strong>.
        /// </value>
        public static readonly Percent Zero = new Percent(0.0f);
        /// <summary>Represents a value of one hundred percent.</summary>
        /// <value>
        /// A <strong>Percentage</strong> value, meaning <strong>100%</strong> or
        /// <strong>1.0</strong>.
        /// </value>
        public static readonly Percent OneHundredPercent = new Percent(1.0f);
        /// <summary>Represents a percentage of fifty percent.</summary>
        /// <value>
        /// A <strong>Percentage</strong> value, representing <strong>50%</strong> or
        /// <strong>0.5</strong>.
        /// </value>
        public static readonly Percent FiftyPercent = new Percent(0.5f);
        public static readonly Percent TenPercent = new Percent(0.1f);
        public static readonly Percent TwentyPercent = new Percent(0.2f);
        public static readonly Percent ThirtyPercent = new Percent(0.3f);
        public static readonly Percent FortyPercent = new Percent(0.4f);
        public static readonly Percent SixtyPercent = new Percent(0.6f);
        public static readonly Percent SeventyPercent = new Percent(0.7f);
        public static readonly Percent EightyPercent = new Percent(0.8f);
        public static readonly Percent NinetyPercent = new Percent(0.9f);
        /// <summary>Represents a value of twenty-five percent.</summary>
        /// <value>
        /// A <strong>Percentage</strong> value, representing <strong>25%</strong> or
        /// <strong>0.25</strong>.
        /// </value>
        public static readonly Percent TwentyFivePercent = new Percent(0.25f);

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance using the specified value.
        /// </summary>
        /// <param name="value">A <strong>Single</strong> value.  A value of ".15" indicates 15%</param>
        public Percent(float value)
        {
            _Value = value;
        }

        public Percent(string value)
            : this(value, CultureInfo.CurrentCulture)
        { }

        public Percent(string value, CultureInfo culture)
        {
            // Replace the "%" percent sign cuz .Net formating does EVERYTHING else a 
            // number string could have in it but not a freekun % sign.
            value = value.Trim().Replace(culture.NumberFormat.PercentSymbol, string.Empty); 

            /* Parse the value as a float, then divide by 100.  In other words,
             * "15%" will become 0.15f
             */
            _Value = float.Parse(value, NumberStyles.Any, culture.NumberFormat) * 0.01f;
        }

        #endregion

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
                return _Value;
            }
        }

        /// <summary>
        /// Returns whether the value equals zero.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return _Value.Equals(0);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>Returns the percentage of the specified value.</summary>
        public double PercentageOf(double value)
        {
            return value * _Value;
        }

        /// <summary>Returns the percentage of the specified value.</summary>
        public float PercentageOf(float value)
        {
            return value * _Value;
        }

        /// <summary>Returns the percentage of the specified value.</summary>
        public float PercentageOf(int value)
        {
            return value * _Value;
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, null))
                return false;
            if (!(obj is Percent))
                return false;

            return ((Percent)obj).Value.Equals(_Value);
        }

        public override int GetHashCode()
        {
            return _Value.GetHashCode();
        }

        /// <summary>Returns the percentage formatted as a <strong>String</strong>.</summary>
        public override string ToString()
        {
            return ToString("P", CultureInfo.CurrentCulture);
        }

        #endregion

        #region Operators

        public static bool operator ==(Percent left, Percent right)
        {
            return left.Value.Equals(right.Value);
        }

        public static bool operator ==(Percent left, float right)
        {
            return left.Value.Equals(right);
        }

        public static bool operator !=(Percent left, Percent right)
        {
            return !left.Value.Equals(right.Value);
        }

        public static bool operator !=(Percent left, float right)
        {
            return !left.Value.Equals(right);
        }

        /// <summary>Returns the percentage of the specified value.</summary>
        public static double operator *(Percent left, double right)
        {
            return left.PercentageOf(right);
        }

        /// <summary>Returns the percentage of the specified value.</summary>
        public static float operator *(Percent left, float right)
        {
            return left.PercentageOf(right);
        }

        /// <summary>Returns the percentage of the specified value.</summary>
        public static float operator *(Percent left, int right)
        {
            return left.PercentageOf(right);
        }

        /// <summary>Returns the percentage of the specified value.</summary>
        public Percent Multiply(double value)
        {
            return new Percent(Convert.ToSingle(_Value * value));
        }

        /// <summary>Returns the percentage of the specified value.</summary>
        public Percent Multiply(float value)
        {
            return new Percent(_Value * value);
        }

        /// <summary>Returns the percentage of the specified value.</summary>
        public Percent Multiply(int value)
        {
            return new Percent(_Value * value);
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Converts the specified value to a <strong>Percent</strong> object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator Percent(float value)
        {
            return new Percent(value);
        }

        /// <summary>
        /// Converts the specified value to a <strong>Percent</strong> object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator Percent(double value)
        {
            return new Percent(Convert.ToSingle(value));
        }

        /// <summary>
        /// Converts the specified value to a <strong>Percent</strong> object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static explicit operator Percent(int value)
        {
            return new Percent(Convert.ToSingle(value));
        }

        public static explicit operator Percent(string value)
        {
            return new Percent(value, CultureInfo.CurrentCulture);
        }

        #endregion

        #region IFormattable Members

        /// <summary>Returns the percentage formatted as a <strong>String</strong>.</summary>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _Value.ToString(format, formatProvider).Replace(" ", "");
        }

        #endregion

        #region IEquatable<Percent> Members

        public bool Equals(Percent other)
        {
            return _Value.Equals(other.Value);
        }

        #endregion

        #region IComparable<Percent> Members

        public int CompareTo(Percent other)
        {
            return _Value.CompareTo(other.Value);
        }

        #endregion
    }
}
