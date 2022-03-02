// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// Event args of the SelectedValueChanged event.
    /// </summary>
    public class SelectedValueChangedEventArgs : EventArgs
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedValueChangedEventArgs"/> class.
        /// </summary>
        public SelectedValueChangedEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedValueChangedEventArgs"/> class.
        /// </summary>
        /// <param name="selectedItem">The selected item.</param>
        public SelectedValueChangedEventArgs(object selectedItem)
        {
            SelectedItem = selectedItem;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public object SelectedItem { get; set; }

        #endregion
    }
}