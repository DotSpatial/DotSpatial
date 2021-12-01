// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// This class converts an IProgressHandler to an ICancelProgressHandler. It wraps the IProgressHandler so that
    /// it can be used by a class that requires an ICancelProgressHandler. Cancel will always return false.
    /// </summary>
    public class MockICancelProgressHandler : ICancelProgressHandler
    {
        private readonly IProgressHandler _progressHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockICancelProgressHandler"/> class.
        /// </summary>
        /// <param name="progressHandler">The progress handler.</param>
        public MockICancelProgressHandler(IProgressHandler progressHandler)
        {
            _progressHandler = progressHandler;
        }

        /// <summary>
        /// Gets a value indicating whether the running process should be canceled.
        /// </summary>
        public bool Cancel => false;

        /// <summary>
        /// Progress is the method that should receive a progress message.
        /// </summary>
        /// <param name="percent">An integer from 0 to 100 that indicates the condition for a status bar etc.</param>
        /// <param name="message">A string containing both information on what the process is, as well as its completion status if wanted.</param>
        public void Progress(int percent, string message)
        {
            _progressHandler.Progress(percent, message);
        }

        /// <summary>
        /// Resets the progress.
        /// </summary>
        public void Reset()
        {
            _progressHandler.Reset();
        }
    }
}
