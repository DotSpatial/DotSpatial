namespace DotSpatial.Topology.Utilities.RToolsUtil
{
    /// <summary>
    /// Token type for characters, meaning non-word characters.
    /// </summary>
    public class CharToken : Token
    {
        #region Constructors

        /// <summary>Constructor with the specified value and line number.</summary>
        public CharToken(string s, int line)
            : base(line)
        {
            if (s.Length > 0) Object = s[0];
        }

        /// <summary>Constructor with the specified value.</summary>
        public CharToken(char c) : base(0) { Object = c; }

        /// <summary>Constructor with the specified value.</summary>
        public CharToken(char c, int line) : base(line) { Object = c; }

        #endregion

        #region Properties

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override string StringValue { get { return (string.Format("{0}", (char)Object)); } }

        #endregion

        #region Methods

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override bool Equals(object other)
        {
            if (other == null) return false;
            if (!GetType().Equals(other.GetType())) return false;
            if (Object == null || ((CharToken)other).Object == null) return false;
            return ((char)Object).Equals((char)((CharToken)other).Object);
        }

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override string ToDebugString() { return string.Format("CharToken: {0}", (char)Object); }

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override string ToString() { return string.Format("{0}", (char)Object); }

        #endregion
    }
}
