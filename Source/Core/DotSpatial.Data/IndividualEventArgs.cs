// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// Carries event arguments for the generic IEventList.
    /// </summary>
    /// <typeparam name="T">Type of the contained items.</typeparam>
    public class IndividualEventArgs<T> : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IndividualEventArgs{T}"/> class.
        /// </summary>
        /// <param name="inListItem">an object that is being interacted with in the list.</param>
        public IndividualEventArgs(T inListItem)
        {
            ListItem = inListItem;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list item being referenced by this event.
        /// </summary>
        public T ListItem { get; protected set; }

        #endregion
    }
}