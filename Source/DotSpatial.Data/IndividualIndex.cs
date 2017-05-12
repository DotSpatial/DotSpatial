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

namespace DotSpatial.Data
{
    /// <summary>
    /// Carries event arguments for the generic IEventList
    /// </summary>
    /// <typeparam name="T">Type of the contained items.</typeparam>
    public class IndividualIndex<T> : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IndividualIndex{T}"/> class.
        /// </summary>
        /// <param name="inListItem">an object that is being interacted with in the list</param>
        public IndividualIndex(T inListItem)
        {
            ListItem = inListItem;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IndividualIndex{T}"/> class.
        /// </summary>
        /// <param name="inListItem">The list item that the event belongs to</param>
        /// <param name="inIndex">The list index, if any, that is specified.</param>
        public IndividualIndex(T inListItem, int inIndex)
        {
            ListItem = inListItem;
            Index = inIndex;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the index for the ListItem.
        /// </summary>
        public int Index { get; protected set; } = -1;

        /// <summary>
        /// Gets or sets the list item being referenced by this event.
        /// </summary>
        public T ListItem { get; protected set; }

        #endregion
    }
}