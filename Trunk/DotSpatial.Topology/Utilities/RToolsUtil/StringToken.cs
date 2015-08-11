namespace DotSpatial.Topology.Utilities.RToolsUtil
{
    /// <summary>
    /// Abstract base class for string tokens.
    /// </summary>
    public abstract class StringToken : Token
    {
        #region Constructors

        /// <summary>Default constructor.</summary>
        protected StringToken(string s) : base(0) { Object = s; }

        /// <summary>Constructor with the specified value
        /// and line number.</summary>
        protected StringToken(string s, int line) : base(line) { Object = s; }

        #endregion

        #region Properties

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override string StringValue { get { return (string)Object; } }

        #endregion

        #region Methods

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override string ToDebugString()
        {
            return GetType().Name + ":'" + (string)Object + "'";
        }

        /// <summary>Override, see base <see cref="Token"/></summary>
        public override string ToString() { return (string)Object; }

        #endregion
    }

    #region CommentToken

    /// <summary>
    /// Token type for comments, including line and block
    /// comments.
    /// </summary>
    public class CommentToken : StringToken
    {
        #region Constructors

        /// <summary>Constructor with the specified value.</summary>
        public CommentToken(string s) : base(s) { }

        /// <summary>Constructor with the specified value and line number.</summary>
        public CommentToken(string s, int line) : base(s, line) { }

        #endregion
    }

    #endregion

    #region QuoteToken

    /// <summary>
    /// Token type for Quotes such as "this is a quote".
    /// </summary>
    public class QuoteToken : StringToken
    {
        #region Constructors

        /// <summary>Constructor with the specified value.</summary>
        public QuoteToken(string s) : base(s) { }

        /// <summary>Constructor with the specified value and line number.</summary>
        public QuoteToken(string s, int line) : base(s, line) { }

        #endregion
    }

    #endregion

    #region WhitespaceToken

    /// <summary>
    /// Token type for whitespace such as spaces and tabs.
    /// </summary>
    public class WhitespaceToken : StringToken
    {
        #region Constructors

        /// <summary>Constructor with the specified value.</summary>
        public WhitespaceToken(string s) : base(s) { }

        /// <summary>Constructor with the specified value and line number.</summary>
        public WhitespaceToken(string s, int line) : base(s, line) { }

        #endregion
    }

    #endregion

    #region WordToken

    /// <summary>
    /// Token type for words, meaning sequences of word
    /// characters.
    /// </summary>
    public class WordToken : StringToken
    {
        #region Constructors

        /// <summary>Constructor with the specified value.</summary>
        public WordToken(string s) : base(s) { }

        /// <summary>Constructor with the specified value and line number.</summary>
        public WordToken(string s, int line) : base(s, line) { }

        #endregion
    }

    #endregion
}
