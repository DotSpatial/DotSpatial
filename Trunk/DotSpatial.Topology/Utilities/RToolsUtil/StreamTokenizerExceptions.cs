using System;

namespace DotSpatial.Topology.Utilities.RToolsUtil
{
    /// <summary>
    /// Exception class for unterminated tokens.
    /// </summary>
    public class StreamTokenizerUntermException : Exception
    {
        #region Constructors

        /// <summary>
        /// Construct with a particular message.
        /// </summary>
        /// <param name="msg">The message to store in this object.</param>
        public StreamTokenizerUntermException(string msg) : base(msg) { }

        #endregion
    }

    /// <summary>
    /// Exception class for unterminated quotes.
    /// </summary>
    public class StreamTokenizerUntermQuoteException : StreamTokenizerUntermException
    {
        #region Constructors

        /// <summary>
        /// Construct with a particular message.
        /// </summary>
        /// <param name="msg">The message to store in this object.</param>
        public StreamTokenizerUntermQuoteException(string msg) : base(msg) { }

        #endregion
    }

    /// <summary>
    /// Exception class for unterminated block comments.
    /// </summary>
    public class StreamTokenizerUntermCommentException : StreamTokenizerUntermException
    {
        #region Constructors

        /// <summary>
        /// Construct with a particular message.
        /// </summary>
        /// <param name="msg">The message to store in this object.</param>
        public StreamTokenizerUntermCommentException(string msg) : base(msg) { }

        #endregion
    }
}
