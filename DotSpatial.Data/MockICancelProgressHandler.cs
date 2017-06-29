// -----------------------------------------------------------------------
// <copyright file="WrapIProgressHandler.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DotSpatial.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// This class converts an IProgressHandler to an ICancelProgressHandler. It wraps the IProgressHandler so that
    /// it can be used by a class that requires an ICancelProgressHandler. Cancel will always return false.
    /// </summary>
    public class MockICancelProgressHandler : ICancelProgressHandler
    {
        private IProgressHandler _ProgressHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockICancelProgressHandler"/> class.
        /// </summary>
        /// <param name="progressHandler">The progress handler.</param>
        public MockICancelProgressHandler(IProgressHandler progressHandler)
        {
            _ProgressHandler = progressHandler;
        }

        /// <summary>
        /// Returns true if the running process should be cancelled
        /// </summary>
        public bool Cancel
        {
            get { return false; }
        }

        /// <summary>
        /// Progress is the method that should receive a progress message.
        /// </summary>
        /// <param name="key">The message string without any information about the status of completion.</param>
        /// <param name="percent">An integer from 0 to 100 that indicates the condition for a status bar etc.</param>
        /// <param name="message">A string containing both information on what the process is, as well as its completion status.</param>
        public void Progress(string key, int percent, string message)
        {
            _ProgressHandler.Progress(key, percent, message);
        }
    }
}
