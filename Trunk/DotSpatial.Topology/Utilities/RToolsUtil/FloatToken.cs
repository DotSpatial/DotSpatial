using System;
using System.Globalization;

namespace DotSpatial.Topology.Utilities.RToolsUtil
{
    /// <summary>
    /// Token type for floating point numbers, stored internally as a Double.
    /// </summary>
    public class FloatToken : Token
    {
        #region Fields

        // NOTE: modified for "safe" assembly in Sql 2005
        // Static field now is an instance field!
        private NumberFormatInfo _numberFormatInfo;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with the specified value.
        /// </summary>
        public FloatToken(string s)
            : base(0)
        {
            try { Object = double.Parse(s, GetNumberFormatInfo()); }
            catch (Exception) { Object = null; }
        }

        /// <summary>
        /// Constructor with the specified value.
        /// </summary>
        public FloatToken(float f)
            : base(0)
        {
            try { Object = (double)f; }
            catch (Exception) { Object = null; }
        }

        /// <summary>
        /// Constructor with the specified value.
        /// </summary>
        public FloatToken(double d)
            : base(0)
        {
            try { Object = d; }
            catch (Exception) { Object = null; }
        }

        /// <summary>
        /// Constructor with the specified value and line number.
        /// </summary>
        public FloatToken(string s, int line)
            : base(line)
        {
            try { Object = double.Parse(s, GetNumberFormatInfo()); }
            catch (Exception) { Object = null; }
        }

        /// <summary>
        /// Constructor with the specified value and line number.
        /// </summary>
        public FloatToken(double f, int line)
            : base(line)
        {
            try { Object = f; }
            catch (Exception) { Object = null; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Override, see base <see cref="Token"/>
        /// </summary>
        public override string StringValue
        {
            get
            {
                return Object != null ? string.Format(GetNumberFormatInfo(), "{0:R}", (double)Object) : "null";
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Override, see base <see cref="Token"/>
        /// </summary>
        public override bool Equals(object other)
        {
            if (other == null) return false;
            if (!GetType().Equals(other.GetType())) return false;
            if (Object == null || ((FloatToken)other).Object == null) return false;
            return ((double)Object).Equals((double)((FloatToken)other).Object);
        }

        /// <summary>
        /// Override, see base <see cref="Token"/>
        /// </summary>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Override, see base <see cref="Token"/>
        /// </summary>
        public override string ToDebugString()
        {
            return Object != null ? string.Format("FloatToken: {0:R}", (double)Object) : "FloatToken: null";
        }

        /// <summary>
        /// Override, see base <see cref="Token"/>
        /// </summary>
        public override string ToString()
        {
            return Object != null ? string.Format("{0:R}", (double)Object) : "null";
        }

        // Static method now is an instance method!
        private NumberFormatInfo GetNumberFormatInfo()
        {
            if (_numberFormatInfo == null)
                _numberFormatInfo = new NumberFormatInfo {NumberDecimalSeparator = "."};
            return _numberFormatInfo;
        }

        #endregion
    }
}
