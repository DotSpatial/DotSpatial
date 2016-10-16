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

using System.Collections.Generic;
using System.ComponentModel;

namespace DotSpatial.Data
{
    /// <summary>
    /// The same as a ListEventArgs, but provides an option to cancel the event
    /// </summary>
    public class CompareCancel<T> : CancelEventArgs
    {
        private IComparer<T> _comparer;

        #region Methods

        /// <summary>
        /// Creates a new instance of a ListEventArgs class
        /// </summary>
        /// <param name="inComparer">The System.Collections.Generic.IComparer&lt;T&gt; being used by this action </param>
        public CompareCancel(IComparer<T> inComparer)
        {
            _comparer = inComparer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the comparer being used in this action
        /// </summary>
        public IComparer<T> Comparer
        {
            get { return _comparer; }
            protected set { _comparer = value; }
        }

        #endregion
    }
}