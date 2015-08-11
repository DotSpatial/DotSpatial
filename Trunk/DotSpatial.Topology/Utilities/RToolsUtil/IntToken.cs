using System;
using System.Globalization;

namespace DotSpatial.Topology.Utilities.RToolsUtil
{
    /// <summary>
    /// Token type for integer tokens. This handles both Int32 and Int64.
    /// </summary>
    public class IntToken : Token
    {
        #region Constructors

        /// <summary>Constructor with the specified value.</summary>
        public IntToken(int i)
            : base(0)
        {
            Object = i;
        }

        /// <summary>Constructor with the specified value.</summary>
        public IntToken(long i)
            : base(0)
        {
            Object = i;
        }

        /// <summary>Constructor with the specified value.</summary>
        public IntToken(string s)
            : base(0)
        {
            Parse(s);
        }

        /// <summary>Constructor with the specified value
        /// and line number.</summary>
        public IntToken(string s, int line)
            : base(line)
        {
            Parse(s);
        }

        /// <summary>Constructor with the specified value
        /// and line number.</summary>
        public IntToken(int i, int line)
            : base(line)
        {
            Object = i;
        }

        /// <summary> 
        /// Constructor for a 64 bit int 
        /// </summary> 
        public IntToken(long l, int line)
            : base(line)
        {
            Object = l;
        }

        #endregion

        #region Properties

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override string StringValue
        {
            get
            {
                return Object != null ? string.Format("{0}", Object) : "null";
            }
        }

        #endregion

        #region Methods

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override bool Equals(object other)
        {
            if (other == null) return false;
            if (!GetType().Equals(other.GetType())) return false;
            if (Object == null || ((IntToken)other).Object == null) return false;
            if (!Object.GetType().Equals(((IntToken)other).Object.GetType())) return false;
            if (Object is int)
                if (((int)Object).Equals((int)((IntToken)other).Object)) return true;
            else
                if (((long)Object).Equals((long)((IntToken)other).Object)) return true;
            return false;
        }

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override int GetHashCode()
        {
            return (ToString().GetHashCode());
        }

        /// <summary>
        /// Parse a string known to be a hex string.  This is faster
        /// than Parse which doesn't assume the number is Hex.  This will
        /// throw an exception if the input number isn't hex.
        /// </summary>
        /// <param name="s">The hex number as a string.</param>
        /// <param name="lineNumber">The line where this token was found.</param>
        /// <returns>A new IntToken set to the value in the input string.</returns>
        public static IntToken ParseHex(string s, int lineNumber)
        {
            IntToken it;
            try
            {
                it = new IntToken(Convert.ToInt32(s, 16), lineNumber);
            }
            catch
            {
                it = new IntToken(Convert.ToInt64(s, 16), lineNumber);
            }

            return it;
        }

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override string ToDebugString()
        {
            return Object != null ? (string.Format("IntToken: {0}", Object)) : "IntToken: null";
        }

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override string ToString()
        {
            return Object != null ? (string.Format("{0}", Object)) : "null";
        }

        /// <summary>
        /// Convert the input string to an integer, if possible
        /// </summary>
        /// <param name="s">The string to parse.</param>
        private void Parse(string s)
        {
            // try base 10 separately since it will be the most common case
            int val32;
            if (int.TryParse(s, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out val32))
            {
                Object = val32;
                return;
            }

            long val64;
            if (long.TryParse(s, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out val64))
            {
                Object = val64;
                return;
            }

            // not a normal int, try other bases
            int[] bases = { 16, 2, 8 };
            foreach (int b in bases)
            {
                try
                {
                    Object = Convert.ToInt32(s, b);
                    return;
                }
                catch
                {
                    // try 64 bit base 10
                    try
                    {
                        Object = Convert.ToInt64(s, b);
                        return;
                    }
                    catch { } // don't give up yet
                }
            }

            Object = null;
        }

        #endregion
    }

}
