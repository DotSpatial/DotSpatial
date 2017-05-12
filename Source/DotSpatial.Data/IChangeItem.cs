// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/8/2008 8:09:59 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Data
{
    /// <summary>
    /// Interface for ChangeItem.
    /// </summary>
    public interface IChangeItem
    {
        /// <summary>
        /// Occurs when internal properties or characteristics of this member change.
        /// The member should send itself as the sender of the event.
        /// </summary>
        event EventHandler ItemChanged;

        /// <summary>
        /// An instruction has been sent to remove the specified item from its container.
        /// </summary>
        event EventHandler RemoveItem;
    }
}