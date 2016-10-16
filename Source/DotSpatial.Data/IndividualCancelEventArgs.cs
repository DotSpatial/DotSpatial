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

using System.ComponentModel;

namespace DotSpatial.Data
{
    /// <summary>
    /// The same as a ListEventArgs, but provides an option to cancel the event
    /// </summary>
    public class IndividualCancelEventArgs<T> : CancelEventArgs
    {
        /// <summary>
        /// The protected object internal to this list event args class
        /// </summary>
        private readonly T _listItem;

        #region Methods

        /// <summary>
        /// Creates a new instance of a ListEventArgs class
        /// </summary>
        /// <param name="inListItem">an object that is being interacted with in the list</param>
        public IndividualCancelEventArgs(T inListItem)
        {
            _listItem = inListItem;
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
        }

        #endregion
    }
}