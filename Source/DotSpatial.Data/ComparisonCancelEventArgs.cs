// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created Before 2010.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |      Name            |    Date     |                                Comments
// |----------------------|-------------|-----------------------------------------------------------------
// |   Ted Dunsford       |  6/30/2010  |  Moved to DotSpatial
// ********************************************************************************************************

using System;
using System.ComponentModel;

namespace DotSpatial.Data
{
    /// <summary>
    /// The same as a ListEventArgs, but provides an option to cancel the event
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
        /// <param name="inComparison">The System.Collections.Generic.IComparer&lt;T&gt; being used by this action </param>
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