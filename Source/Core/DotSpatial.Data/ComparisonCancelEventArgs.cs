// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;

namespace DotSpatial.Data
{
    /// <summary>
    /// The same as a ListEventArgs, but provides an option to cancel the event.
    /// </summary>
    /// <typeparam name="T">Type of the comparison.</typeparam>
    public class ComparisonCancelEventArgs<T> : CancelEventArgs
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComparisonCancelEventArgs{T}"/> class.
        /// </summary>
        /// <param name="inComparison">The System.Collections.Generic.IComparer&lt;T&gt; being used by this action. </param>
        public ComparisonCancelEventArgs(Comparison<T> inComparison)
        {
            Comparison = inComparison;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the comparer being used in this action.
        /// </summary>
        public Comparison<T> Comparison { get; set; }

        #endregion
    }
}