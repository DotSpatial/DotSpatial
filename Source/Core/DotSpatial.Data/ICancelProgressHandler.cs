// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// IProgressHandler that carries a boolean property allowing the process using the handler to know if you should cancelled.
    /// </summary>
    public interface ICancelProgressHandler : IProgressHandler
    {
        /// <summary>
        /// Gets a value indicating whether the progress handler has been notified that the running process should be cancelled.
        /// </summary>
        bool Cancel { get; }
    }
}