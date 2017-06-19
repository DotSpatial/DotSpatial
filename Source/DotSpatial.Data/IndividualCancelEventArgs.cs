// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel;

namespace DotSpatial.Data
{
    /// <summary>
    /// The same as a ListEventArgs, but provides an option to cancel the event
    /// </summary>
    /// <typeparam name="T">Type of the contained items.</typeparam>
    public class IndividualCancelEventArgs<T> : CancelEventArgs
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IndividualCancelEventArgs{T}"/> class.
        /// </summary>
        /// <param name="inListItem">an object that is being interacted with in the list</param>
        public IndividualCancelEventArgs(T inListItem)
        {
            ListItem = inListItem;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list item being referenced by this event.
        /// </summary>
        public T ListItem { get; }

        #endregion
    }
}