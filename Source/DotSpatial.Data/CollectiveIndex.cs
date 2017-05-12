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
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// Carries event arguments for the generic IEventList.
    /// </summary>
    /// <typeparam name="T">Type of the contained items.</typeparam>
    public class CollectiveIndex<T> : EventArgs
    {
        #region Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectiveIndex{T}"/> class.
        /// </summary>
        /// <param name="inCollection">The IEnumerable&lt;T&gt; specified during the event"/></param>
        /// <param name="inIndex">The integer index associated with this event</param>
        public CollectiveIndex(IEnumerable<T> inCollection, int inIndex)
        {
            Index = inIndex;
            Collection = inCollection;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the IEnumerable&lt;T&gt; collection of items involved in this event.
        /// </summary>
        public IEnumerable<T> Collection { get; protected set; }

        /// <summary>
        /// Gets or sets the index in the list where the event is associated.
        /// </summary>
        public int Index { get; protected set; }

        #endregion
    }
}