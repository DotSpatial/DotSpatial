// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;

namespace DotSpatial.Tools.Tests
{
    /// <summary>
    /// A mock progress handler.
    /// </summary>
    internal class MockProgressHandler : ICancelProgressHandler
    {
        /// <summary>
        /// Gets a value indicating whether the progress was canceled.
        /// </summary>
        public bool Cancel { get; } = false;

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="percent">The percent.</param>
        /// <param name="message">The message.</param>
        public void Progress(string key, int percent, string message)
        {
            // nothing
        }
    }
}