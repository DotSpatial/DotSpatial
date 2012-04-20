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
        public MockICancelProgressHandler(IProgressHandler progressHandler)
        {
            _ProgressHandler = progressHandler;
        }

        public bool Cancel
        {
            get { return false; }
        }

        public void Progress(string key, int percent, string message)
        {
            _ProgressHandler.Progress(key, percent, message);
        }
    }
}
