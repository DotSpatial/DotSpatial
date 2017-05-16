// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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

        #region Properties

        /// <summary>
        /// Gets the item that was changed by this operation.
        /// </summary>
        object ChangeItem { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Shows a PropertyGrid Dialog and uses the specified object as the edit copy.
        /// </summary>
        /// <param name="editCopy">A clone of the actual object to be edited. This should not be the original object.</param>
        void ShowDialog(object editCopy);

        #endregion
    }
}