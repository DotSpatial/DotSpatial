// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel;

namespace DotSpatial.Data
{
    /// <summary>
    /// Contains properties for both a specified item and an integer index
    /// as well as the option to cancel.
    /// </summary>
    /// <typeparam name="T">Type of the contained items.</typeparam>
    public class IndividualIndexCancel<T> : CancelEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IndividualIndexCancel{T}"/> class.
        /// </summary>
        /// <param name="inListItem">an object that is being interacted with in the list.</param>
        public IndividualIndexCancel(T inListItem)
        {
            ListItem = inListItem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndividualIndexCancel{T}"/> class.
        /// </summary>
        /// <param name="inListItem">The list item that the event belongs to.</param>
        /// <param name="inIndex">The list index, if any, that is specified.</param>
        public IndividualIndexCancel(T inListItem, int inIndex)
        {
            ListItem = inListItem;
            Index = inIndex;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the index for the ListItem.
        /// </summary>
        public int Index { get; protected set; } = -1;

        /// <summary>
        /// Gets or sets the list item being referenced by this event.
        /// </summary>
        public T ListItem { get; protected set; }

        #endregion
    }
}