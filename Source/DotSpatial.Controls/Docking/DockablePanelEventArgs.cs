// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Controls.Docking
{
    /// <summary>
    /// The active panel changed event args.
    /// </summary>
    public class DockablePanelEventArgs : EventArgs
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DockablePanelEventArgs"/> class.
        /// </summary>
        /// <param name="activePanelKey">The active Panel Key.</param>
        public DockablePanelEventArgs(string activePanelKey)
        {
            ActivePanelKey = activePanelKey;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the active panel key.
        /// </summary>
        /// <value>The active panel key.</value>
        public string ActivePanelKey { get; set; }

        #endregion
    }
}