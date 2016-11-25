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
    public class CollectiveCancel<T> : CancelEventArgs
    {
        private IEnumerable<T> _collection;

        #region Methods

        /// <summary>
        /// Creates a new instance of a ListEventArgs class
        /// </summary>
        /// <param name="inCollection">the IEnumerable&lt;T&gt; responsible for the event</param>
        public CollectiveCancel(IEnumerable<T> inCollection)
        {
            _collection = inCollection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list item being referenced by this event
        /// </summary>
        public IEnumerable<T> Collection
        {
            get { return _collection; }
            protected set { _collection = value; }
        }

        #endregion
    }
}