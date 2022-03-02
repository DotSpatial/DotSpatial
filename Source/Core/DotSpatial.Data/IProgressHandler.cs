// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// An interface for sending progress messages. Percent is an integer from 0 to 100.
    /// </summary>
    public interface IProgressHandler
    {
        /// <summary>
        /// Progress is the method that should receive a progress message.
        /// </summary>
        /// <param name="percent">An integer from 0 to 100 that indicates the condition for a status bar etc.</param>
        /// <param name="message">A string containing both information on what the process is, as well as its completion status.</param>
        void Progress(int percent, string message);

        /// <summary>
        /// Resets the progress to 0 percent and no message text.
        /// </summary>
        void Reset();
    }
}