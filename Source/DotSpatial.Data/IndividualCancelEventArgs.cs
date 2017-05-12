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
    /// <typeparam name="T">Type of the contained items.</typeparam>
    public class IndividualCancelEventArgs<T> : CancelEventArgs
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="IndividualCancelEventArgs{T}"/> class.
        /// </summary>
        /// <param name="inListItem">an object that is being interacted with in the list</param>
        public IndividualCancelEventArgs(T inListItem)
        {
            ListItem = inListItem;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the list item being referenced by this event.
        /// </summary>
        public T ListItem { get; }

        #endregion
    }
}