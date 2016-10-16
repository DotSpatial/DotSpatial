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
    public class CollectiveIndexCompareCancel<T> : CancelEventArgs
    {
        private IEnumerable<T> _collection;
        private IComparer<T> _comparer;
        private int _index = -1;

        #region Methods

        /// <summary>
        /// Creates a new instance of a ListEventArgs class.
        /// </summary>
        /// <param name="inCollection">the IEnumerable&lt;T&gt; responsible for the event.</param>
        /// <param name="inComparer">The System.Collections.Generic.IComparer&lt;T&gt; being used by this action.</param>
        /// <param name="inIndex">the Integer index associated with this event.</param>
        public CollectiveIndexCompareCancel(IEnumerable<T> inCollection, IComparer<T> inComparer, int inIndex)
        {
            _collection = inCollection;
            _index = inIndex;
            _comparer = inComparer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the IEnumerable&lt;T&gt; collection involved in this event
        /// </summary>
        public virtual IEnumerable<T> Collection
        {
            get { return _collection; }
            protected set { _collection = value; }
        }

        /// <summary>
        /// Gets the integer index in the IEventList where this event occured
        /// </summary>
        public virtual int Index
        {
            get { return _index; }
            protected set { _index = value; }
        }

        /// <summary>
        /// The System.Collections.Generic.IComparer&lt;T&gt; being used by this action
        /// </summary>
        public virtual IComparer<T> Comparer
        {
            get { return _comparer; }
            protected set { _comparer = value; }
        }

        #endregion
    }
}