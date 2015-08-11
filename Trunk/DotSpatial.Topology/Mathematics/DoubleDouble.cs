using System;
using System.Globalization;
using System.Text;

namespace DotSpatial.Topology.Mathematics
{
    /// <summary>
    /// Implements extended-precision floating-point numbers 
    /// which maintain 106 bits (approximately 30 decimal digits) of precision. 
    /// <para/>
    /// A DoubleDouble uses a representation containing two double-precision values.
    /// A number x is represented as a pair of doubles, x.hi and x.lo,
    /// such that the number represented by x is x.hi + x.lo, where
    /// <pre>
    ///     |x.lo| &lt;= 0.5*ulp(x.hi)
    /// </pre>
    /// and ulp(y) means "unit in the last place of y".  
    /// The basic arithmetic operations are implemented using 
    /// convenient properties of IEEE-754 floating-point arithmetic.
    /// <para/>
    /// The range of values which can be represented is the same as in IEEE-754.  
    /// The precision of the representable numbers 
    /// is twice as great as IEEE-754 double precision.
    /// <para/>
    /// The correctness of the arithmetic algorithms relies on operations
    /// being performed with standard IEEE-754 double precision and rounding.
    /// This is the Java standard arithmetic model, but for performance reasons 
    /// Java implementations are not
    /// constrained to using this standard by default.  
    /// Some processors (notably the Intel Pentium architecure) perform
    /// floating point operations in (non-IEEE-754-standard) extended-precision.
    /// A JVM implementation may choose to use the non-standard extended-precision
    /// as its default arithmetic mode.
    /// To prevent this from happening, this code uses the
    /// Java <tt>strictfp</tt> modifier, 
    /// which forces all operations to take place in the standard IEEE-754 rounding model. 
    /// <para/>
    /// The API provides both a set of value-oriented operations 
    /// and a set of mutating operations.
    /// Value-oriented operations treat DoubleDouble values as 
    /// immutable; operations on them return new objects carrying the result
    /// of the operation.  This provides a simple and safe semantics for
    /// writing DoubleDouble expressions.  However, there is a performance
    /// penalty for the object allocations required.
    /// The mutable interface updates object values in-place.
    /// It provides optimum memory performance, but requires
    /// care to ensure that aliasing errors are not created
    /// and constant values are not changed.
    /// <para/>
    /// This implementation uses algorithms originally designed variously by 
    /// Knuth, Kahan, Dekker, and Linnainmaa.  
    /// Douglas Priest developed the first C implementation of these techniques. 
    /// Other more recent C++ implementation are due to Keith M. Briggs and David Bailey et al.
    /// <h3>References</h3>
    /// <list type="Bullet">
    /// <item>Priest, D., <i>Algorithms for Arbitrary Precision Floating Point Arithmetic</i>,
    /// in P. Kornerup and D. Matula, Eds., Proc. 10th Symposium on Computer Arithmetic, 
    /// IEEE Computer Society Press, Los Alamitos, Calif., 1991.</item>
    /// <item>Yozo Hida, Xiaoye S. Li and David H. Bailey, 
    /// <i>Quad-Double Arithmetic: Algorithms, Implementation, and Application</i>, 
    /// manuscript, Oct 2000; Lawrence Berkeley National Laboratory Report BNL-46996.</item>
    /// <item>David Bailey, <i>High Precision Software Directory</i>; 
    /// <tt>http://crd.lbl.gov/~dhbailey/mpdist/index.html</tt></item>
    /// </list>
    /// </summary>
    /// <author>Martin Davis</author>
#if !PCL
    [Serializable]
#endif
    public struct DoubleDouble : IComparable, IComparable<DoubleDouble> /*, IFormattable*/
    {
        #region Constant Fields

        /*------------------------------------------------------------
         *   Output
         *------------------------------------------------------------
         */

        private const int MaxPrintDigits = 32;

        /// <summary>
        /// The value to split a double-precision value on during multiplication
        /// </summary>
        private const double Split = 134217729.0D; // 2^27+1, for IEEE double

        #endregion

        #region Fields

        /// <summary>
        /// The value nearest to the constant e (the natural logarithm base). 
        /// </summary>
        public static readonly DoubleDouble E = new DoubleDouble(2.718281828459045091e+00, 1.445646891729250158e-16);

        /// <summary>
        /// The smallest representable relative difference between two <see cref="DoubleDouble"/> values
        /// </summary>
        public static readonly double Epsilon = 1.23259516440783e-32; /* = 2^-106 */

        /// <summary>
        /// A value representing the result of an operation which does not return a valid number.
        /// </summary>
        public static readonly DoubleDouble NaN = new DoubleDouble(Double.NaN, Double.NaN);

        /// <summary>The value nearest to the constant Pi.</summary>
        public static readonly DoubleDouble Pi = new DoubleDouble(3.141592653589793116e+00, 1.224646799147353207e-16);

        /// <summary>The value nearest to the constant Pi / 2.</summary>
        public static readonly DoubleDouble PiHalf = new DoubleDouble(1.570796326794896558e+00, 6.123233995736766036e-17);

        /// <summary>The value nearest to the constant 2 * Pi.</summary>
        public static readonly DoubleDouble TwoPi = new DoubleDouble(6.283185307179586232e+00, 2.449293598294706414e-16);

        private static readonly DoubleDouble Ten = ValueOf(10.0);
        private static readonly DoubleDouble One = ValueOf(1.0);
        private static readonly String SCI_NOT_EXPONENT_CHAR = "E";
        private static readonly String SCI_NOT_ZERO = "0.0E0";
        /**
         * The high-order component of the double-double precision value.
         */
        private readonly double _hi;
        /**
         * The low-order component of the double-double precision value.
         */
        private readonly double _lo;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="DoubleDouble"/> with value x.
        /// </summary>
        /// <param name="x">The initial value</param>
        public DoubleDouble(double x) : this(x, 0d) { }

        /// <summary>
        /// Creates a new <see cref="DoubleDouble"/> with value (hi, lo).
        /// </summary>
        /// <param name="hi">The high order component</param>
        /// <param name="lo">The low order component</param>
        public DoubleDouble(double hi, double lo)
        {
            _hi = hi;
            _lo = lo;
        }

        /// <summary>
        /// Creates a <see cref="DoubleDouble"/> with a value equal to the argument
        /// </summary>
        /// <param name="dd">The initial value</param>
        public DoubleDouble(DoubleDouble dd)
        {
            _hi = dd._hi;
            _lo = dd._lo;
        }

        /// <summary>
        /// Creates a new <see cref="DoubleDouble"/> equal to the argument.
        /// </summary>
        /// <param name="str">the value to initialize by</param>
        /// <remarks> NumberFormatException if <tt>str</tt> is not a valid representation of a number.</remarks>
        public DoubleDouble(String str) : this(Parse(str)) { }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this object is negative or not
        /// </summary>
        public bool IsNegative
        {
            get { return _hi < 0.0 || (_hi == 0.0 && _lo < 0.0); }
        }

        /*------------------------------------------------------------
         *   Predicates
         *------------------------------------------------------------
         */

        /// <summary>
        /// Gets a value indicating whether this object is zero (0) or not
        /// </summary>
        public bool IsZero
        {
            get { return _hi == 0.0 && _lo == 0.0; }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Returns the sum of <paramref name="lhs"/> and <paramref name="rhs"/>.
        /// </summary>
        /// <param name="lhs">The left hand side</param>
        /// <param name="rhs">The right hand side</param>
        /// <returns>The sum of <paramref name="lhs"/> and <paramref name="rhs"/></returns>
        public static DoubleDouble operator +(DoubleDouble lhs, DoubleDouble rhs)
        {
            double S = lhs._hi + rhs._hi;
            double T = lhs._lo + rhs._lo;
            double e = S - lhs._hi;
            double f = T - lhs._lo;
            double s = S - e;
            double t = T - f;
            s = (rhs._hi - e) + (lhs._hi - s);
            t = (rhs._lo - f) + (lhs._lo - t);
            e = s + T;
            double H = S + e;
            double h = e + (S - H);
            e = t + h;

            double zhi = H + e;
            return new DoubleDouble(zhi, e + (H - zhi));
        }

        /// <summary>
        /// Returns the sum of <paramref name="lhs"/> and <paramref name="rhs"/>.
        /// </summary>
        /// <param name="lhs">The left hand side</param>
        /// <param name="rhs">The right hand side</param>
        /// <returns>The sum of <paramref name="lhs"/> and <paramref name="rhs"/></returns>
        public static DoubleDouble operator +(DoubleDouble lhs, Double rhs)
        {
            return lhs + new DoubleDouble(rhs, 0);
        }

        public static DoubleDouble operator /(DoubleDouble lhs, Double rhs)
        {
            return lhs / new DoubleDouble(rhs, 0d);
        }

        public static DoubleDouble operator /(DoubleDouble lhs, DoubleDouble rhs)
        {
            if (IsNaN(rhs)) return CreateNaN();

            double C = lhs._hi / rhs._hi;
            double c = Split * C;
            double hc = c - C;
            double u = Split * rhs._hi;
            hc = c - hc;
            double tc = C - hc;
            double hy = u - rhs._hi;
            double U = C * rhs._hi;
            hy = u - hy;
            double ty = rhs._hi - hy;
            u = (((hc * hy - U) + hc * ty) + tc * hy) + tc * ty;
            c = ((((lhs._hi - U) - u) + lhs._lo) - C * rhs._lo) / rhs._hi;
            u = C + c;

            return new DoubleDouble(u, (C - u) + c);
        }

        public static bool operator ==(DoubleDouble lhs, DoubleDouble rhs)
        {
            return lhs._hi == rhs._hi && lhs._lo == rhs._lo;
        }

        public static explicit operator DoubleDouble(String val)
        {
            return Parse(val);
        }

        public static implicit operator DoubleDouble(Double val)
        {
            return new DoubleDouble(val);
        }

        public static bool operator !=(DoubleDouble rhs, DoubleDouble lhs)
        {
            return !(rhs == lhs);
        }

        public static DoubleDouble operator *(DoubleDouble lhs, Double rhs)
        {
            return lhs * new DoubleDouble(rhs, 0d);
        }

        public static DoubleDouble operator *(DoubleDouble lhs, DoubleDouble rhs)
        {
            if (IsNaN(rhs)) return CreateNaN();

            double C = Split * lhs._hi;
            double hx = C - lhs._hi;
            double c = Split * rhs._hi;
            hx = C - hx;
            double tx = lhs._hi - hx;
            double hy = c - rhs._hi;
            C = lhs._hi * rhs._hi;
            hy = c - hy;
            double ty = rhs._hi - hy;
            c = ((((hx * hy - C) + hx * ty) + tx * hy) + tx * ty) + (lhs._hi * rhs._lo + lhs._lo * rhs._hi);
            double zhi = C + c;
            hx = C - zhi;
            double zlo = c + hx;

            return new DoubleDouble(zhi, zlo);
        }

        /// <summary>
        /// Returns the difference of <paramref name="lhs"/> and <paramref name="rhs"/>.
        /// </summary>
        /// <param name="lhs">The left hand side</param>
        /// <param name="rhs">The right hand side</param>
        /// <returns>The difference of <paramref name="lhs"/> and <paramref name="rhs"/></returns>
        public static DoubleDouble operator -(DoubleDouble lhs, DoubleDouble rhs)
        {
            return lhs + rhs.Negate();
        }

        /// <summary>
        /// Returns the difference of <paramref name="lhs"/> and <paramref name="rhs"/>.
        /// </summary>
        /// <param name="lhs">The left hand side</param>
        /// <param name="rhs">The right hand side</param>
        /// <returns>The difference of <paramref name="lhs"/> and <paramref name="rhs"/></returns>
        public static DoubleDouble operator -(DoubleDouble lhs, Double rhs)
        {
            return lhs + new DoubleDouble(-rhs, 0);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the absolute value of this value.
        /// <para/>
        /// Special cases:
        /// <list type="Bullet">
        /// <item>if this value is NaN, it is returned.</item>
        /// </list>
        /// </summary>
        /// <returns>The absolute value of this value</returns>
        public DoubleDouble Abs()
        {
            if (IsNaN(this)) return NaN;
            return IsNegative ? Negate() : new DoubleDouble(this);
        }

        /// <summary>
        /// Returns a <see cref="DoubleDouble"/> whose value is <c>(this + <paramref name="y"/>)</c>
        /// </summary>
        /// <param name="y">The addende</param>"/>
        /// <returns><c>(this + <paramref name="y"/>)</c></returns>
        [Obsolete("Use operator +")]
        public DoubleDouble Add(DoubleDouble y)
        {
            return this + y;
        }

        /// <summary>
        /// Returns a <see cref="DoubleDouble"/> whose value is <tt>(this + y)</tt>.
        /// </summary>
        /// <param name="y">The addend</param>
        /// <returns><tt>(this + y)</tt></returns>
        [Obsolete("Use Operator +")]
        public DoubleDouble Add(double y)
        {
            return this + y;
        }

        /// <summary>
        ///  /// Returns the smallest (closest to negative infinity) value 
        /// that is not less than the argument and is equal to a mathematical integer. 
        /// Special cases:
        /// If this value is NaN, returns NaN.
        /// </summary>
        /// <returns>the smallest (closest to negative infinity) value 
        /// that is not less than the argument and is equal to a mathematical integer. </returns>
        public DoubleDouble Ceiling()
        {
            if (IsNaN(this)) return NaN;
            double fhi = Math.Ceiling(_hi);
            double flo = 0.0;
            // Hi is already integral.  Ceil the low word
            if (fhi == _hi)
            {
                flo = Math.Ceiling(_lo);
                // do we need to renormalize here?
            }
            return new DoubleDouble(fhi, flo);
        }

        /// <summary>
        /// Creates and returns a copy of this value.
        /// </summary>
        /// <returns>Acopy of this value</returns>
        public Object Clone()
        {
            return new DoubleDouble(_hi, _lo);
        }

        /// <summary>
        /// Compares two DoubleDouble objects numerically.
        /// </summary>
        /// <param name="other"></param>
        /// <returns>-1,0 or 1 depending on whether this value is less than, equal to or greater than the value of <tt>other</tt>.</returns>
        public int CompareTo(DoubleDouble other)
        {
            if (_hi < other._hi) return -1;
            if (_hi > other._hi) return 1;
            if (_lo < other._lo) return -1;
            if (_lo > other._lo) return 1;
            return 0;
        }

        /// <summary>
        /// Compares two DoubleDouble objects numerically.
        /// </summary>
        /// <param name="other">object this DoubleDouble gets compared to. Must be a DoubleDouble.</param>
        /// <returns>-1,0 or 1 depending on whether this value is less than, equal to or greater than the value of <tt>other</tt>.</returns>
        public int CompareTo(Object other)
        {
            return CompareTo((DoubleDouble)other);
        }

        /// <summary>
        /// Creates a new <see cref="DoubleDouble"/> with the value of the argument.
        /// </summary>
        /// <param name="dd">The value to copy</param>
        /// <returns>A copy of <paramref name="dd"/></returns>
        public static DoubleDouble Copy(DoubleDouble dd)
        {
            return new DoubleDouble(dd);
        }

        private static DoubleDouble CreateNaN()
        {
            return new DoubleDouble(Double.NaN, Double.NaN);
        }

        /// <summary>
        /// Computes a new <see cref="DoubleDouble"/> whose value is <tt>(this / y)</tt>.
        /// </summary>
        /// <param name="y">The divisor</param>
        /// <returns>A new <see cref="DoubleDouble"/> with the value <c>(this / y)</c></returns>
        public DoubleDouble Divide(DoubleDouble y)
        {
            double C = _hi / y._hi;
            double c = Split * C;
            double hc = c - C;
            double u = Split * y._hi;
            hc = c - hc;
            double tc = C - hc;
            double hy = u - y._hi;
            double U = C * y._hi;
            hy = u - hy;
            double ty = y._hi - hy;
            u = (((hc * hy - U) + hc * ty) + tc * hy) + tc * ty;
            c = ((((_hi - U) - u) + _lo) - C * y._lo) / y._hi;
            u = C + c;

            double zhi = u;
            double zlo = (C - u) + c;
            return new DoubleDouble(zhi, zlo);
        }

        /// <summary>
        /// Computes a new <see cref="DoubleDouble"/> whose value is <tt>(this / y)</tt>.
        /// </summary>
        /// <param name="y">The divisor</param>
        /// <returns>A new <see cref="DoubleDouble"/> with the value <c>(this / y)</c></returns>
        [Obsolete("Use /-operator instead")]
        public DoubleDouble Divide(double y)
        {
            return this / y;
        }

        /// <summary>
        /// Dumps the components of this number to a string.
        /// </summary>
        /// <returns>A string showing the components of the number</returns>
        public String Dump()
        {
            return string.Format(NumberFormatInfo.InvariantInfo, "DD<{0}, {1}>", _hi, _lo);
        }

        /// <summary>
        /// Tests whether this value is equal to another <tt>DoubleDouble</tt> value.
        /// </summary>
        /// <param name="y">a DoubleDouble value</param>
        /// <returns>true if this value = y</returns>
        public bool Equals(DoubleDouble y)
        {
            return y._hi.Equals(_hi) && y._lo.Equals(_lo);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(DoubleDouble)) return false;
            return Equals((DoubleDouble)obj);
        }

        /// <summary>
        /// Extracts the significant digits in the decimal representation of the argument.
        /// A decimal point may be optionally inserted in the string of digits
        /// (as long as its position lies within the extracted digits
        /// - if not, the caller must prepend or append the appropriate zeroes and decimal point).
        /// </summary>
        /// <param name="insertDecimalPoint"></param>
        /// <param name="magnitudes"></param>
        /// <returns>the string containing the significant digits and possibly a decimal point</returns>
        private String ExtractSignificantDigits(bool insertDecimalPoint, int[] magnitudes)
        {
            var y = this.Abs();
            // compute *correct* magnitude of y
            var mag = Magnitude(y._hi);
            var scale = Ten.Pow(mag);
            y /= scale;

            // fix magnitude if off by one
            if (y.GreaterThan(Ten))
            {
                y /= Ten;
                mag += 1;
            }
            else if (y.LessThan(One))
            {
                y *= Ten;
                mag -= 1;
            }

            int decimalPointPos = mag + 1;
            var buf = new StringBuilder();
            int numDigits = MaxPrintDigits - 1;
            for (int i = 0; i <= numDigits; i++)
            {
                if (insertDecimalPoint && i == decimalPointPos)
                {
                    buf.Append('.');
                }
                int digit = (int)y._hi;

                /**
                 * This should never happen, due to heuristic checks on remainder below
                 */
                if (digit < 0 || digit > 9)
                {
                    //        System.out.println("digit > 10 : " + digit);
                    //        throw new IllegalStateException("Internal errror: found digit = " + digit);
                }
                /**
                 * If a negative remainder is encountered, simply terminate the extraction.  
                 * This is robust, but maybe slightly inaccurate.
                 * My current hypothesis is that negative remainders only occur for very small lo components, 
                 * so the inaccuracy is tolerable
                 */
                if (digit < 0)
                {
                    break;
                    // throw new IllegalStateException("Internal errror: found digit = " + digit);
                }
                bool rebiasBy10 = false;
                char digitChar;
                if (digit > 9)
                {
                    // set flag to re-bias after next 10-shift
                    rebiasBy10 = true;
                    // output digit will end up being '9'
                    digitChar = '9';
                }
                else
                {
                    digitChar = (char)('0' + digit);
                }
                buf.Append(digitChar);
                y = (y - ValueOf(digit)) * Ten;
                if (rebiasBy10)
                    y += Ten;

                bool continueExtractingDigits = true;
                /**
                 * Heuristic check: if the remaining portion of 
                 * y is non-positive, assume that output is complete
                 */
                //      if (y.hi <= 0.0)
                //        if (y.hi < 0.0)
                //        continueExtractingDigits = false;
                /**
                 * Check if remaining digits will be 0, and if so don't output them.
                 * Do this by comparing the magnitude of the remainder with the expected precision.
                 */
                int remMag = Magnitude(y._hi);
                if (remMag < 0 && Math.Abs(remMag) >= (numDigits - i))
                    continueExtractingDigits = false;
                if (!continueExtractingDigits)
                    break;
            }
            magnitudes[0] = mag;
            return buf.ToString();
        }

        /// <summary>
        /// Returns the largest (closest to positive infinity) value that is not greater than the argument 
        /// and is equal to a mathematical integer.
        /// Special cases: If this value is NaN, returns NaN.
        /// </summary>
        /// <returns>the largest (closest to positive infinity) value that is not greater than the argument 
        /// and is equal to a mathematical integer.</returns>
        public DoubleDouble Floor()
        {
            if (IsNaN(this)) return NaN;
            double fhi = Math.Floor(_hi);
            double flo = 0.0;
            // Hi is already integral.  Floor the low word
            if (fhi == _hi)
            {
                flo = Math.Floor(_lo);
            }
            // do we need to renormalize here?    
            return new DoubleDouble(fhi, flo);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_hi.GetHashCode() * 397) ^ _lo.GetHashCode();
            }
        }

        /// <summary>
        /// Returns the string for this value if it has a known representation (e.g. NaN or 0.0).
        /// </summary>
        /// <returns>The string for this special number <br/>
        /// or <c>null</c> if the number is not a special number</returns>
        private String GetSpecialNumberString()
        {
            if (IsZero) return "0.0";
            if (IsNaN(this)) return "NaN ";
            return null;
        }

        /// <summary>
        /// Tests whether this value is greater than or equals to another <tt>DoubleDouble</tt> value.
        /// </summary>
        /// <param name="y">a DoubleDouble value</param>
        /// <returns>true if this value >= y</returns>
        public bool GreaterOrEqualThan(DoubleDouble y)
        {
            return (_hi > y._hi) || (_hi == y._hi && _lo >= y._lo);
        }

        /// <summary>
        /// Tests whether this value is greater than another <tt>DoubleDouble</tt> value.
        /// </summary>
        /// <param name="y">a DoubleDouble value</param>
        /// <returns>true if this value > y</returns>
        public bool GreaterThan(DoubleDouble y)
        {
            return (_hi > y._hi) || (_hi == y._hi && _lo > y._lo);
        }

        /// <summary>
        /// Gets a value indicating whether this object is positive or not
        /// </summary>
        public static bool IsNaN(DoubleDouble value)
        {
            return Double.IsNaN(value._hi);
        }

        /// <summary>
        /// Gets a value indicating whether this object is positive or not
        /// </summary>
        public bool IsPositive()
        {
            return _hi > 0.0 || (_hi == 0.0 && _lo > 0.0);
        }

        /// <summary>
        /// Tests whether this value is less than or equal to another <tt>DoubleDouble</tt> value.
        /// </summary>
        /// <param name="y">a DoubleDouble value</param>
        /// <returns>true if this value <= y</returns>
        public bool LessOrEqualThan(DoubleDouble y)
        {
            return (_hi < y._hi) || (_hi == y._hi && _lo <= y._lo);
        }

        /// <summary>
        /// Tests whether this value is less than another <tt>DoubleDouble</tt> value.
        /// </summary>
        /// <param name="y">A DoubleDouble value</param>
        /// <returns><c>true</c> if this value is &lt; <paramref name="y"/> </returns>
        public bool LessThan(DoubleDouble y)
        {
            return (_hi < y._hi) || (_hi == y._hi && _lo < y._lo);
        }

        /// <summary>
        /// Determines the decimal magnitude of a number.<para/>
        /// The magnitude is the exponent of the greatest power of 10 which is less than
        /// or equal to the number.
        /// </summary>
        /// <param name="x">The number to find the magnitude of</param>
        /// <returns>The decimal magnitude of <paramref name="x"/></returns>
        private static int Magnitude(double x)
        {
            var xAbs = Math.Abs(x);
            var xLog10 = Math.Log(xAbs) / Math.Log(10);
            var xMag = (int)Math.Floor(xLog10);
            /**
             * Since log computation is inexact, there may be an off-by-one error
             * in the computed magnitude. 
             * Following tests that magnitude is correct, and adjusts it if not
             */
            var xApprox = Math.Pow(10, xMag);
            if (xApprox * 10 <= xAbs)
                xMag += 1;

            return xMag;
        }

        /// <summary>
        /// Computes the maximum of this and another DD number.
        /// </summary>
        /// <param name="x">A DD number</param>
        /// <returns>The maximum of the two numbers</returns>
        public DoubleDouble Max(DoubleDouble x)
        {
            return GreaterOrEqualThan(x) ? this : x;
        }

        /// <summary>
        /// Computes the minimum of this and another DD number.
        /// </summary>
        /// <param name="x">A DD number</param>
        /// <returns>The minimum of the two numbers</returns>
        public DoubleDouble Min(DoubleDouble x)
        {
            return LessThan(x) ? this : x;
        }

        /**
         * Returns a new DoubleDouble whose value is <tt>(this * y)</tt>.
         * 
         * @param y the multiplicand
         * @return <tt>(this * y)</tt>
         */

        [Obsolete("Use *-operator instead")]
        public DoubleDouble Multiply(DoubleDouble y)
        {
            return this * y;
            //return Copy(this).SelfMultiply(y);
        }

        /**
         * Returns a new DoubleDouble whose value is <tt>(this * y)</tt>.
         * 
         * @param y the multiplicand
         * @return <tt>(this * y)</tt>
         */

        [Obsolete("Use *-operator instead")]
        public DoubleDouble Multiply(double y)
        {
            return this * y;
        }

        /// <summary>
        /// Returns a <see cref="DoubleDouble"/> whose value is <c>-this</c>.
        /// </summary>
        /// <returns><c>-this</c></returns>
        public DoubleDouble Negate()
        {
            if (IsNaN(this)) return this;
            return new DoubleDouble(-_hi, -_lo);
        }

        /*------------------------------------------------------------
         *   Input
         *------------------------------------------------------------
         */

        /// <summary>
        /// Converts a string representation of a real number into a DoubleDouble value.
        /// The format accepted is similar to the standard Java real number syntax.  
        /// It is defined by the following regular expression:
        /// <pre>
        /// [<tt>+</tt>|<tt>-</tt>] {<i>digit</i>} [ <tt>.</tt> {<i>digit</i>} ] [ ( <tt>e</tt> | <tt>E</tt> ) [<tt>+</tt>|<tt>-</tt>] {<i>digit</i>}+
        /// </pre>
        ///  </summary>
        /// <param name="str">The string to parse</param>
        /// <returns>The value of the parsed number</returns>
        /// <exception cref="FormatException">Thrown if <tt>str</tt> is not a valid representation of a number</exception>
        public static DoubleDouble Parse(String str)
        {
            int i = 0;
            int strlen = str.Length;

            // skip leading whitespace
            while (Char.IsWhiteSpace(str[i]))
                i++;

            // check for sign
            bool isNegative = false;
            if (i < strlen)
            {
                char signCh = str[i];
                if (signCh == '-' || signCh == '+')
                {
                    i++;
                    if (signCh == '-') isNegative = true;
                }
            }

            // scan all digits and accumulate into an integral value
            // Keep track of the location of the decimal point (if any) to allow scaling later
            DoubleDouble val = new DoubleDouble();

            int numDigits = 0;
            int numBeforeDec = 0;
            int exp = 0;
            while (true)
            {
                if (i >= strlen)
                    break;
                char ch = str[i];
                i++;
                if (Char.IsDigit(ch))
                {
                    double d = ch - '0';
                    val *= Ten;
                    // MD: need to optimize this
                    val += d;
                    numDigits++;
                    continue;
                }
                if (ch == '.')
                {
                    numBeforeDec = numDigits;
                    continue;
                }
                if (ch == 'e' || ch == 'E')
                {
                    String expStr = str.Substring(i);
                    // this should catch any format problems with the exponent
                    try
                    {
                        exp = int.Parse(expStr);
                    }
                    catch (FormatException ex)
                    {
                        throw new FormatException("Invalid exponent " + expStr + " in string " + str, ex);
                    }
                    break;
                }
                throw new FormatException("Unexpected character '" + ch
                                          + "' at position " + i
                                          + " in string " + str);
            }
            DoubleDouble val2 = val;

            // scale the number correctly
            int numDecPlaces = numDigits - numBeforeDec - exp;
            if (numDecPlaces == 0)
            {
                val2 = val;
            }
            else if (numDecPlaces > 0)
            {
                var scale = Ten.Pow(numDecPlaces);
                val2 = val.Divide(scale);
            }
            else if (numDecPlaces < 0)
            {
                DoubleDouble scale = Ten.Pow(-numDecPlaces);
                val2 = val * scale;
            }
            // apply leading sign, if any
            if (isNegative)
            {
                return val2.Negate();
            }
            return val2;

        }

        /// <summary>
        /// Computes the value of this number raised to an integral power.
        /// Follows semantics of .Net Math.Pow as closely as possible.
        /// </summary>
        /// <param name="exp">The integer exponent</param>
        /// <returns>x raised to the integral power exp</returns>
        public DoubleDouble Pow(int exp)
        {
            if (exp == 0.0)
                return ValueOf(1.0);

            DoubleDouble r = new DoubleDouble(this);
            DoubleDouble s = ValueOf(1.0);
            int n = Math.Abs(exp);

            if (n > 1)
            {
                /* Use binary exponentiation */
                while (n > 0)
                {
                    if (n % 2 == 1)
                    {
                        s *= r;
                    }
                    n /= 2;
                    if (n > 0)
                        r = r.Sqr();
                }
            }
            else
            {
                s = r;
            }

            /* Compute the reciprocal if n is negative. */
            if (exp < 0)
                return s.Reciprocal();
            return s;
        }

        /// <summary>
        /// Returns a DoubleDouble whose value is  <tt>1 / this</tt>.
        /// </summary>
        /// <returns>the reciprocal of this value</returns>
        public DoubleDouble Reciprocal()
        {
            double C = 1.0 / _hi;
            double c = Split * C;
            double hc = c - C;
            double u = Split * _hi;
            hc = c - hc;
            double tc = C - hc;
            double hy = u - _hi;
            double U = C * _hi;
            hy = u - hy;
            double ty = _hi - hy;
            u = (((hc * hy - U) + hc * ty) + tc * hy) + tc * ty;
            c = ((((1.0 - U) - u)) - C * _lo) / _hi;

            double zhi = C + c;
            double zlo = (C - zhi) + c;
            return new DoubleDouble(zhi, zlo);
        }

        /// <summary>
        /// Rounds this value to the nearest integer.
        /// The value is rounded to an integer by adding 1/2 and taking the floor of the result.
        /// Special cases:
        /// <list type="Bullet">
        /// <item>If this value is NaN, returns NaN.</item>
        /// </list>
        /// </summary>
        /// <returns>This value rounded to the nearest integer</returns>
        public DoubleDouble Rint()
        {
            if (IsNaN(this)) return this;
            // may not be 100% correct
            var plus5 = this + 0.5d;
            return plus5.Floor();
        }

        /// <summary>
        /// Returns an integer indicating the sign of this value.
        /// <para>
        /// <list type="Bullet">
        /// <item>if this value is &gt; 0, returns 1</item>
        /// <item>if this value is &lt; 0, returns -1</item>
        /// <item>if this value is = 0, returns 0</item>
        /// <item>if this value is NaN, returns 0</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <returns>An integer indicating the sign of this value</returns>
        public int Signum()
        {
            if (_hi > 0) return 1;
            if (_hi < 0) return -1;
            if (_lo > 0) return 1;
            if (_lo < 0) return -1;
            return 0;
        }

        /// <summary>
        /// Computes the square of this value.
        /// </summary>
        /// <returns>The square of this value.</returns>
        public static DoubleDouble Sqr(double x)
        {
            return ValueOf(x) * x;
        }

        /// <summary>
        /// Computes the square of this value.
        /// </summary>
        /// <returns>The square of this value</returns>
        public DoubleDouble Sqr()
        {
            return this * this;
        }

        public static DoubleDouble Sqrt(double x)
        {
            return ValueOf(x).Sqrt();
        }

        /// <summary>
        /// Computes the positive square root of this value.
        /// If the number is NaN or negative, NaN is returned.
        /// </summary>
        /// <returns>the positive square root of this number. 
        /// If the argument is NaN or less than zero, the result is NaN.</returns>
        public DoubleDouble Sqrt()
        {
            /* Strategy:  Use Karp's trick:  if x is an approximation
            to sqrt(a), then

               sqrt(a) = a*x + [a - (a*x)^2] * x / 2   (approx)

            The approximation is accurate to twice the accuracy of x.
            Also, the multiplication (a*x) and [-]*x can be done with
            only half the precision.
         */

            if (IsZero)
                return ValueOf(0.0);

            if (IsNegative)
            {
                return NaN;
            }

            double x = 1.0 / Math.Sqrt(_hi);
            double ax = _hi * x;

            var axdd = ValueOf(ax);
            var diffSq = this - axdd.Sqr();
            var d2 = diffSq._hi * (x * 0.5);

            return axdd + d2;
        }

        /// <summary>
        /// Computes a new <see cref="DoubleDouble"/> object whose value is <tt>(this - y)</tt>.
        /// </summary>
        /// <param name="y">The subtrahend</param>
        /// <returns><tt>(this - y)</tt></returns>
        [Obsolete("Use operator -")]
        public DoubleDouble Subtract(DoubleDouble y)
        {
            return Add(y.Negate());
        }

        /// <summary>
        /// Computes a new <see cref="DoubleDouble"/> object whose value is <tt>(this - y)</tt>.
        /// </summary>
        /// <param name="y">The subtrahend</param>
        /// <returns><tt>(this - y)</tt></returns>
        [Obsolete("Use operator -")]
        public DoubleDouble Subtract(double y)
        {
            return Add(-y);
        }

        /*------------------------------------------------------------
         *   Conversion Functions
         *------------------------------------------------------------
         */

        /// <summary>
        /// Converts this value to the nearest double-precision number.
        /// </summary>
        /// <returns>the nearest double-precision number to this value</returns>
        public double ToDoubleValue()
        {
            return _hi + _lo;
        }

        /// <summary>
        /// Converts this value to the nearest integer.
        /// </summary>
        /// <returns>the nearest integer to this value</returns>
        public int ToIntValue()
        {
            return (int)_hi;
        }

        /// <summary>
        /// Returns the string representation of this value in scientific notation.
        /// </summary>
        /// <returns>The string representation in scientific notation</returns>
        public String ToSciNotation()
        {
            // special case zero, to allow as
            if (IsZero)
                return SCI_NOT_ZERO;

            String specialStr = GetSpecialNumberString();
            if (specialStr != null)
                return specialStr;

            int[] magnitude = new int[1];
            String digits = ExtractSignificantDigits(false, magnitude);
            String expStr = SCI_NOT_EXPONENT_CHAR + magnitude[0];

            // should never have leading zeroes
            // MD - is this correct?  Or should we simply strip them if they are present?
            if (digits[0] == '0')
            {
                throw new InvalidOperationException("Found leading zero: " + digits);
            }

            // add decimal point
            String trailingDigits = "";
            if (digits.Length > 1)
                trailingDigits = digits.Substring(1);
            String digitsWithDecimal = digits[0] + "." + trailingDigits;

            if (IsNegative)
                return "-" + digitsWithDecimal + expStr;
            return digitsWithDecimal + expStr;
        }

        /// <summary>
        /// Returns the string representation of this value in standard notation.
        /// </summary>
        /// <returns>The string representation in standard notation</returns>
        public String ToStandardNotation()
        {
            var specialStr = GetSpecialNumberString();
            if (specialStr != null)
                return specialStr;

            var magnitude = new int[1];
            var sigDigits = ExtractSignificantDigits(true, magnitude);
            int decimalPointPos = magnitude[0] + 1;

            String num = sigDigits;
            // add a leading 0 if the decimal point is the first char
            if (sigDigits[0] == '.')
            {
                num = "0" + sigDigits;
            }
            else if (decimalPointPos < 0)
            {
                num = "0." + new string('0', -decimalPointPos) + sigDigits;
            }
            else if (sigDigits.IndexOf('.') == -1)
            {
                // no point inserted - sig digits must be smaller than magnitude of number
                // add zeroes to end to make number the correct size
                int numZeroes = decimalPointPos - sigDigits.Length;
                var zeroes = new string('0', numZeroes);
                num = sigDigits + zeroes + ".0";
            }

            if (IsNegative)
                return "-" + num;
            return num;
        }

        /// <summary>
        /// Returns a string representation of this number, in either standard or scientific notation.
        /// If the magnitude of the number is in the range [ 10<sup>-3</sup>, 10<sup>8</sup> ]
        /// standard notation will be used.  Otherwise, scientific notation will be used.
        /// </summary>
        /// <returns>A string representation of this number</returns>
        public override String ToString()
        {
            var mag = Magnitude(_hi);
            if (mag >= -3 && mag <= 20)
                return ToStandardNotation();
            return ToSciNotation();
        }

        /// <summary>
        /// Returns the integer which is largest in absolute value and not further
        /// from zero than this value.  
        /// <para/>
        /// Special cases:
        /// <list type="Bullet">
        /// <item>If this value is NaN, returns NaN.</item>
        /// </list>
        /// </summary>
        /// <returns>
        /// The integer which is largest in absolute value and not further from zero than this value
        /// </returns>
        public DoubleDouble Truncate()
        {
            if (IsNaN(this)) return NaN;
            return IsPositive() ? Floor() : Ceiling();
        }

        /// <summary>
        /// Converts the string argument to a DoubleDouble number.
        /// </summary>
        /// <param name="str">A string containing a representation of a numeric value</param>
        /// <returns>The extended precision version of the value</returns>
        /// <exception cref="FormatException">Thrown if <paramref name="str"/> is not a valid representation of a number</exception>
        public static DoubleDouble ValueOf(String str)
        {
            return Parse(str);
        }

        /// <summary>
        /// Converts the <tt>double</tt> argument to a DoubleDouble number.
        /// </summary>
        /// <param name="x">a numeric value</param>
        /// <returns>the extended precision version of the value</returns>
        public static DoubleDouble ValueOf(double x)
        {
            return new DoubleDouble(x);
        }

        #endregion
    }
}