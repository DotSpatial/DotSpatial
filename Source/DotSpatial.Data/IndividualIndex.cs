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
    public class IndividualIndex<T> : EventArgs
    {
        private int _index = -1;

        /// <summary>
        /// The protected object internal to this list event args class
        /// </summary>
        private T _listItem;

        #region Methods

        /// <summary>
        /// Creates a new instance of a ListEventArgs class
        /// </summary>
        /// <param name="inListItem">an object that is being interacted with in the list</param>
        public IndividualIndex(T inListItem)
        {
            _listItem = inListItem;
        }

        /// <summary>
        /// Creates a new instance of a ListEventArgs class
        /// </summary>
        /// <param name="inListItem">The list item that the event belongs to</param>
        /// <param name="inIndex">The list index, if any, that is specified.</param>
        public IndividualIndex(T inListItem, int inIndex)
        {
            _listItem = inListItem;
            _index = inIndex;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list item being referenced by this event
        /// </summary>
        public T ListItem
        {
            get
            {
                return _listItem;
            }
            protected set
            {
                _listItem = value;
            }
        }

        /// <summary>
        /// Gets the index for the ListItem
        /// </summary>
        public int Index
        {
            get
            {
                return _index;
            }
            protected set
            {
                _index = value;
            }
        }

        #endregion
    }
}