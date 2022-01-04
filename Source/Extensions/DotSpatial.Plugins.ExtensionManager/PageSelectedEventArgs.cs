// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Plugins.ExtensionManager
{
    /// <summary>
    /// Event args for page selected event.
    /// </summary>
    internal class PageSelectedEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets or sets the selected page.
        /// </summary>
        public int SelectedPage { get; set; }

        #endregion
    }
}