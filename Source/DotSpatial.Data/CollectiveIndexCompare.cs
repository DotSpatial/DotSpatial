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
    /// The same as a ListEventArgs, but provides an option to cancel the event.
    /// </summary>
    /// <typeparam name="T">Type of the contained items.</typeparam>
    public class CollectiveIndexCompare<T> : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectiveIndexCompare{T}"/> class.
        /// </summary>
        /// <param name="inCollection">the IEnumerable&lt;T&gt; responsible for the event.</param>
        /// <param name="inComparer">The System.Collections.Generic.IComparer&lt;T&gt; being used by this action.</param>
        /// <param name="inIndex">the Integer index associated with this event.</param>
        public CollectiveIndexCompare(IEnumerable<T> inCollection, IComparer<T> inComparer, int inIndex)
        {
            Collection = inCollection;
            Index = inIndex;
            Comparer = inComparer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the IEnumerable&lt;T&gt; collection involved in this event.
        /// </summary>
        public IEnumerable<T> Collection { get; protected set; }

        /// <summary>
        /// Gets or sets the System.Collections.Generic.IComparer&lt;T&gt; being used by this action.
        /// </summary>
        public IComparer<T> Comparer { get; set; }

        /// <summary>
        /// Gets or sets the integer index in the IEventList where this event occurred.
        /// </summary>
        public int Index { get; set; }

        #endregion
    }
}