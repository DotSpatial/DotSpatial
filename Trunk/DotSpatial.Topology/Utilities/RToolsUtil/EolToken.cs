namespace DotSpatial.Topology.Utilities.RToolsUtil
{
    /// <summary>
    /// Represents end-of-lines (line separator characters).
    /// </summary>
    public class EolToken : Token
    {
        #region Constructors

        /// <summary>Default constructor.</summary>
        public EolToken() : base(0) { }

        /// <summary>Constructor that takes line number.</summary>
        public EolToken(int line) : base(line) { }

        #endregion

        #region Properties

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override string StringValue { get { return ToString(); } }

        #endregion

        #region Methods

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override bool Equals(object other)
        {
            return other is EolToken;
        }

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override string ToDebugString() { return ("Eol"); }

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override string ToString() { return ("\n"); }

        #endregion
    }
}
