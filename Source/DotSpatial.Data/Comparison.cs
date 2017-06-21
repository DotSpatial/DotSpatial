// ********************************************************************************************************
// Product Name: DotSpatial.Interfaces Alpha
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// Carries event arguments for the generic IEventList
    /// </summary>
    public class ComparisonArgs<T> : EventArgs
    {
        private Comparison<T> _comparison;

        #region Methods

        /// <summary>
        /// Creates a new instance of a ListEventArgs class
        /// </summary>
        /// <param name="inComparison">The System.Comparison&lt;T&gt; being used by this action </param>
        public ComparisonArgs(Comparison<T> inComparison)
        {
            _comparison = inComparison;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets System.Comparison being referenced by this event
        /// </summary>
        public Comparison<T> Comparison
        {
            get { return _comparison; }
            set { _comparison = value; }
        }

        #endregion
    }
}