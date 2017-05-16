// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// Interface for ChangeItem.
    /// </summary>
    public interface IChangeItem
    {
        /// <summary>
        /// Occurs when internal properties or characteristics of this member change.
        /// The member should send itself as the sender of the event.
        /// </summary>
        event EventHandler ItemChanged;

        /// <summary>
        /// An instruction has been sent to remove the specified item from its container.
        /// </summary>
        event EventHandler RemoveItem;
    }
}