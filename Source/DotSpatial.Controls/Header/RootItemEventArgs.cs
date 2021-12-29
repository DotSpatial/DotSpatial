// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// Information about the selected root item.
    /// </summary>
    public class RootItemEventArgs : EventArgs
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RootItemEventArgs"/> class.
        /// </summary>
        public RootItemEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RootItemEventArgs"/> class.
        /// </summary>
        /// <param name="selectedKey">The key of the selected root item.</param>
        public RootItemEventArgs(string selectedKey)
        {
            SelectedRootKey = selectedKey;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the selected root item key.
        /// </summary>
        /// <value>
        /// The selected root item key.
        /// </value>
        public string SelectedRootKey { get; set; }

        #endregion
    }
}