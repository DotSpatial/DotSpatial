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
    /// <typeparam name="T">Type of the collection items.</typeparam>
    public class CollectiveIndexCancelEventArgs<T> : CancelEventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectiveIndexCancelEventArgs{T}"/> class.
        /// </summary>
        /// <param name="inCollection">the IEnumerable&lt;T&gt; responsible for the event</param>
        /// <param name="inIndex">the Integer index associated with this event</param>
        public CollectiveIndexCancelEventArgs(IEnumerable<T> inCollection, int inIndex)
        {
            Collection = inCollection;
            Index = inIndex;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the IEnumerable&lt;T&gt; collection involved in this event.
        /// </summary>
        public IEnumerable<T> Collection { get; protected set; }

        /// <summary>
        /// Gets or sets the integer index in the IEventList where this event occurred.
        /// </summary>
        public int Index { get; protected set; }

        #endregion
    }
}