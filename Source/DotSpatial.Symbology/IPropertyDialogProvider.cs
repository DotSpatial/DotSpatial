// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/15/2009 3:35:43 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IPropertyDialogProvider
    /// </summary>
    public interface IPropertyDialogProvider
    {
        #region Events

        /// <summary>
        /// Fires an event signifying that the item has been updated
        /// </summary>
        event EventHandler<ChangedObjectEventArgs> ChangesApplied;

        #endregion

        #region Methods

        /// <summary>
        /// Shows a PropertyGrid Dialog and uses the specified object as the edit copy.
        /// </summary>
        /// <param name="editCopy">A clone of the actual object to be edited.  This should not be the original object.</param>
        void ShowDialog(object editCopy);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the item that was changed by this operation.
        /// </summary>
        object ChangeItem
        {
            get;
        }

        #endregion
    }
}