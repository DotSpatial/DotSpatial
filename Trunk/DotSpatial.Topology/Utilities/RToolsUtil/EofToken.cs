namespace DotSpatial.Topology.Utilities.RToolsUtil
{
    /// <summary>
    /// Represents end of file/stream.
    /// </summary>
    public class EofToken : Token
    {
        #region Constructors

        /// <summary>Default constructor.</summary>
        public EofToken() : base(0) { }

        /// <summary>Constructor that takes line number.</summary>
        public EofToken(int line) : base(line) { }

        #endregion

        #region Properties

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override string StringValue { get { return ToString(); } }

        #endregion

        #region Methods

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override bool Equals(object other)
        {
            return other is EofToken;
        }

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override string ToDebugString() { return "Eof"; }

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override string ToString() { return string.Empty; }

        #endregion
    }
}
