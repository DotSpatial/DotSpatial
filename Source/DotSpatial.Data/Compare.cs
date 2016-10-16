// ********************************************************************************************************
// Product Name: DotSpatial.Interfaces Alpha
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// Carries event arguments for the generic IEventList
    /// </summary>
    public class Compare<T> : EventArgs
    {
        private IComparer<T> _comparer;

        #region Methods

        /// <summary>
        /// Creates a new instance of a ListEventArgs class
        /// </summary>
        /// <param name="inComparer">The System.Collections.Generic.IComparer&lt;T&gt; being used by this action </param>
        public Compare(IComparer<T> inComparer)
        {
            _comparer = inComparer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the System.Collections.Generic.IComparer&lt;T&gt; being referenced by this event
        /// </summary>
        public IComparer<T> Comparer
        {
            get { return _comparer; }
            protected set { _comparer = value; }
        }

        #endregion
    }
}