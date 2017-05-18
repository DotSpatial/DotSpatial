// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// By using this interface, the developers can create a button, menu, ribbon tab. etc
    /// without considering whether the user interface is ribbon style or standard toolbar
    /// style
    /// </summary>
    public interface IHeaderControl
    {
        #region Events

        /// <summary>
        /// This event occurs when a root item is selected
        /// </summary>
        event EventHandler<RootItemEventArgs> RootItemSelected;

        #endregion

        #region Methods

        /// <summary>
        /// This will add a new item that will appear on the standard toolbar or ribbon control.
        /// </summary>
        /// <param name="item">The item that gets added.</param>
        /// <returns>Added GUI item</returns>
        object Add(HeaderItem item);

        /// <summary>
        /// Remove item from the standard toolbar or ribbon control
        /// </summary>
        /// <param name="key">The string itemName to remove from the standard toolbar or ribbon control</param>
        void Remove(string key);

        /// <summary>
        /// Removes all items the plugin created.
        /// </summary>
        void RemoveAll();

        /// <summary>
        /// Selects the root, making it the active root.
        /// </summary>
        /// <param name="key">The key.</param>
        void SelectRoot(string key);

        #endregion
    }
}