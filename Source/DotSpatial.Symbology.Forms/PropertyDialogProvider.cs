// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// PropertyDialogProvider
    /// </summary>
    public class PropertyDialogProvider : IPropertyDialogProvider
    {
        #region Fields

        private PropertyDialog _frmDialog;

        #endregion

        #region Events

        /// <summary>
        /// Fires an event signifying that the item has been updated
        /// </summary>
        public event EventHandler<ChangedObjectEventArgs> ChangesApplied;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the item that was changed by this operation.
        /// </summary>
        public object ChangeItem => _frmDialog.PropertyGrid.SelectedObject;

        #endregion

        #region Methods

        /// <summary>
        /// Shows a PropertyGrid Dialog and uses the specified object as the edit copy.
        /// </summary>
        /// <param name="editCopy">Object that should be used as edit copy.</param>
        public void ShowDialog(object editCopy)
        {
            _frmDialog = new PropertyDialog
            {
                PropertyGrid = { SelectedObject = editCopy }
            };
            _frmDialog.ChangesApplied += FrmDialogChangesApplied;
            _frmDialog.ShowDialog();
        }

        /// <summary>
        /// Fires a the ChangesApplied event.
        /// </summary>
        /// <param name="changedItem">Item that was changed.</param>
        protected virtual void OnChangesApplied(object changedItem)
        {
            ChangesApplied?.Invoke(this, new ChangedObjectEventArgs(changedItem));
        }

        private void FrmDialogChangesApplied(object sender, EventArgs e)
        {
            OnChangesApplied(_frmDialog.PropertyGrid.SelectedObject);
        }

        #endregion
    }
}