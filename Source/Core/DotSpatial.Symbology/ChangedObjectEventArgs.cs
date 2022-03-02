// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Event args for the ChangedObjectEvent.
    /// </summary>
    public class ChangedObjectEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangedObjectEventArgs"/> class.
        /// </summary>
        /// <param name="changedItem">The item that has been changed.</param>
        public ChangedObjectEventArgs(object changedItem)
        {
            Item = changedItem;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the item that has been changed.
        /// </summary>
        public object Item { get; }

        #endregion
    }
}