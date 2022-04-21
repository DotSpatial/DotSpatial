// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel;

namespace DotSpatial.Data
{
    /// <summary>
    /// The same as a ListEventArgs, but provides an option to cancel the event.
    /// </summary>
    /// <typeparam name="T">Type of the contained items.</typeparam>
    public class CollectiveCancel<T> : CancelEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectiveCancel{T}"/> class.
        /// </summary>
        /// <param name="inCollection">the IEnumerable&lt;T&gt; responsible for the event.</param>
        public CollectiveCancel(IEnumerable<T> inCollection)
        {
            Collection = inCollection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list item being referenced by this event.
        /// </summary>
        public IEnumerable<T> Collection { get; protected set; }

        #endregion
    }
}