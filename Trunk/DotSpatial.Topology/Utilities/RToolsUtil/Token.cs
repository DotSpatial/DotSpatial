using System;
using System.Globalization;

namespace DotSpatial.Topology.Utilities.RToolsUtil
{
    /// <summary>
    /// Token class used by StreamTokenizer. This represents a single token in the input stream.
    /// This is subclassed to provide specific token types, such as CharToken, FloatToken, etc.
    /// </summary>
    abstract public class Token
    {
        #region Fields

        /// An error message associated with unterm error.
        string _untermErrorMsg;

        #endregion

        #region Constructors

        /// <summary>
        /// Construct a Token with the specified line number.
        /// </summary>
        /// <param name="line">The line number where this token comes from.</param>
        protected Token(int line)
        {
            Object = null;
            UntermError = false;
            LineNumber = line;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Return this token's value as a string.
        /// </summary>
        /// <returns>This token's value as a string.</returns>
        public virtual string StringValue
        {
            get { return ("unset"); }
        }

        /// <summary>
        /// The line number where this token was found.  This is base-1.
        /// </summary>
        public int LineNumber { get; private set; }

        /// <summary>
        /// The Object stored by this token. This will be a primitive C# type.
        /// </summary>
        public object Object { get; protected set; }

        /// <summary>
        /// Whether or not there was an unterminated token problem when creating this token.  
        /// See UntermErrorMessage for a message associated with the problem.
        /// </summary>
        public bool UntermError { get; set; }

        /// <summary>
        /// The error message if there was an unterminated token error creating this token.
        /// </summary>
        public string UntermErrorMsg
        {
            get { return (_untermErrorMsg); }
            set
            {
                UntermError = true;
                _untermErrorMsg = value;
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Operator== overload.  Compare a token and an object.
        /// </summary>
        /// <param name="t">The token to compare.</param>
        /// <param name="o">The other object.</param>
        /// <returns>bool</returns>
        public static bool operator ==(Token t, object o)
        {
            if (t == null) return o == null;
            return o != null && t.Equals(o);
        }

        /// <summary>
        /// Operator== overload.  Compare a token and a char.
        /// </summary>
        /// <param name="t">The token to compare.</param>
        /// <param name="c">The char.</param>
        /// <returns>bool</returns>
        public static bool operator ==(Token t, char c)
        {
            return t != null && t.Equals(c);
        }

        /// <summary>
        /// Operator== overload.  Compare a token and a string.
        /// </summary>
        /// <param name="t">The token to compare.</param>
        /// <param name="s">The string.</param>
        /// <returns>bool</returns>
        public static bool operator ==(Token t, string s)
        {
            if ((object)t == null) return s == null;
            return t.Equals(s);
        }

        /// <summary>
        /// Operator!= overload.  Compare a token and an object.
        /// </summary>
        /// <param name="t">The token to compare.</param>
        /// <param name="o">The other object.</param>
        /// <returns>bool</returns>
        public static bool operator !=(Token t, object o)
        {
            if (t == null) return o != null;
            return !t.Equals(o);
        }

        /// <summary>
        /// Operator!= overload.  Compare a token and a char.
        /// </summary>
        /// <param name="t">The token to compare.</param>
        /// <param name="c">The char.</param>
        /// <returns>bool</returns>
        public static bool operator !=(Token t, char c)
        {
            if (t == null) return false;
            return !t.Equals(c);
        }

        /// <summary>
        /// Operator!= overload.  Compare a token and a string.
        /// </summary>
        /// <param name="t">The token to compare.</param>
        /// <param name="s">The string.</param>
        /// <returns>bool</returns>
        public static bool operator !=(Token t, string s)
        {
            if (t == null) return s != null;
            return !t.Equals(s);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create an object of the specified type corresponding to this token.
        /// </summary>
        /// <param name="t">The type of object to create.</param>
        /// <returns>The new object, or null for error.</returns>
        public object ConvertToType(Type t)
        {
            return Convert.ChangeType(StringValue, t, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Equals override.
        /// </summary>
        /// <param name="other">The object to compare to.</param>
        /// <returns>bool - true for equals, false otherwise.</returns>
        public override bool Equals(object other)
        {
            if (other == null) return false;
            if (!(other is Token)) return false;
            return Object.Equals(((Token)other).Object);
        }

        /// <summary>
        /// Equals overload.
        /// </summary>
        /// <param name="s">The string to compare to.</param>
        /// <returns>bool</returns>
        public bool Equals(string s)
        {
            return s != null && StringValue.Equals(s);
        }

        /// <summary>
        /// Equals overload.
        /// </summary>
        /// <param name="c">The char to compare to.</param>
        /// <returns>bool</returns>
        public bool Equals(char c)
        {
            CharToken ct = this as CharToken;
            return ct != null && ct.Object.Equals(c);
        }

        /// <summary>
        /// Override.  Returns the ToString().GetHashCode().
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Produce a string which includes the token type.
        /// </summary>
        /// <returns></returns>
        public virtual string ToDebugString()
        {
            return string.Format("{0}: line {1}", ToString(), LineNumber);
        }

        /// <summary>
        /// Produce a string which includes the line number.
        /// </summary>
        /// <returns></returns>
        public string ToLineString()
        {
            return string.Format("{0}: line {1}", ToDebugString(), LineNumber);
        }

        #endregion
    }
}
