// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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