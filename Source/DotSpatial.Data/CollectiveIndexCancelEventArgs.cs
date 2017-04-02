// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created before 2010 refactoring.
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
    public class CollectiveIndexCancelEventArgs<T> : CancelEventArgs
    {
        private IEnumerable<T> _collection;
        private int _index = -1;

        #region Methods

        /// <summary>
        /// Creates a new instance of a ListEventArgs class
        /// </summary>
        /// <param name="inCollection">the IEnumerable&lt;T&gt; responsible for the event</param>
        /// <param name="inIndex">the Integer index associated with this event</param>
        public CollectiveIndexCancelEventArgs(IEnumerable<T> inCollection, int inIndex)
        {
            _collection = inCollection;
            _index = inIndex;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the IEnumerable&lt;T&gt; collection involved in this event
        /// </summary>
        public IEnumerable<T> Collection
        {
            get { return _collection; }
            protected set { _collection = value; }
        }

        /// <summary>
        /// Gets the integer index in the IEventList where this event occurred
        /// </summary>
        public int Index
        {
            get { return _index; }
            protected set { _index = value; }
        }

        #endregion
    }
}