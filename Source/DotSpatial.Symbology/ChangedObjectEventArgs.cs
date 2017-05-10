// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/15/2009 3:24:43 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Event args for the ChangedObjectEvent.
    /// </summary>
    public class ChangedObjectEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangedObjectEventArgs"/> class.
        /// </summary>
        /// <param name="changedItem">The item that has been changed.</param>
        public ChangedObjectEventArgs(object changedItem)
        {
            Item = changedItem;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the item that has been changed.
        /// </summary>
        public object Item { get; }

        #endregion
    }
}