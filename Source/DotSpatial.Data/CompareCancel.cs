// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel;

namespace DotSpatial.Data
{
    /// <summary>
    /// The same as a ListEventArgs, but provides an option to cancel the event
    /// </summary>
    /// <typeparam name="T">Type of the comparer.</typeparam>
    public class CompareCancel<T> : CancelEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CompareCancel{T}"/> class.
        /// </summary>
        /// <param name="inComparer">The System.Collections.Generic.IComparer&lt;T&gt; being used by this action </param>
        public CompareCancel(IComparer<T> inComparer)
        {
            Comparer = inComparer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the comparer being used in this action.
        /// </summary>
        public IComparer<T> Comparer { get; protected set; }

        #endregion
    }
}