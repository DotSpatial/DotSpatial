// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel;

namespace DotSpatial.Data
{
    /// <summary>
    /// The same as a ListEventArgs, but provides an option to cancel the event
    /// </summary>
    /// <typeparam name="T">Type of the contained items.</typeparam>
    public class CollectiveIndexCompareCancel<T> : CancelEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectiveIndexCompareCancel{T}"/> class.
        /// </summary>
        /// <param name="inCollection">the IEnumerable&lt;T&gt; responsible for the event.</param>
        /// <param name="inComparer">The System.Collections.Generic.IComparer&lt;T&gt; being used by this action.</param>
        /// <param name="inIndex">the Integer index associated with this event.</param>
        public CollectiveIndexCompareCancel(IEnumerable<T> inCollection, IComparer<T> inComparer, int inIndex)
        {
            Collection = inCollection;
            Index = inIndex;
            Comparer = inComparer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the IEnumerable&lt;T&gt; collection involved in this event.
        /// </summary>
        public IEnumerable<T> Collection { get; protected set; }

        /// <summary>
        /// Gets or sets the System.Collections.Generic.IComparer&lt;T&gt; being used by this action.
        /// </summary>
        public IComparer<T> Comparer { get; protected set; }

        /// <summary>
        /// Gets or sets the integer index in the IEventList where this event occurred.
        /// </summary>
        public int Index { get; protected set; }

        #endregion
    }
}