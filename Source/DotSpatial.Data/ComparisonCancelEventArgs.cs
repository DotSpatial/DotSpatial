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
    public class ComparisonCancelEventArgs<T> : CancelEventArgs
    {
        /// <summary>
        /// The protected System.Collections.Generic.IComparer&lt;T&gt; being used by this action
        /// </summary>
        private Comparison<T> _comparison;

        #region Methods

        /// <summary>
        /// Creates a new instance of a ListEventArgs class
        /// </summary>
        /// <param name="inComparison">The System.Collections.Generic.IComparer&lt;T&gt; being used by this action </param>
        public ComparisonCancelEventArgs(Comparison<T> inComparison)
        {
            _comparison = inComparison;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the comparer being used in this action
        /// </summary>
        public Comparison<T> Comparison
        {
            get { return _comparison; }
            set { _comparison = value; }
        }

        #endregion
    }
}