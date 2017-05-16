﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel.Composition;

namespace DotSpatial.Controls.Docking
{
    /// <summary>
    /// An interface that allows plugins to add controls which are managed by the application as forms or docking panels.
    /// </summary>
    [InheritedExport]
    public interface IDockManager
    {
        #region Events

        /// <summary>
        /// Occurs when the active panel is changed, meaning a difference panel is activated.
        /// </summary>
        event EventHandler<DockablePanelEventArgs> ActivePanelChanged;

        /// <summary>
        /// Occurs after a panel is added.
        /// </summary>
        event EventHandler<DockablePanelEventArgs> PanelAdded;

        /// <summary>
        /// Occurs when a panel is closed, which means the panel can still be activated or removed.
        /// </summary>
        event EventHandler<DockablePanelEventArgs> PanelClosed;

        /// <summary>
        /// Occurs when a panel is hidden.
        /// </summary>
        event EventHandler<DockablePanelEventArgs> PanelHidden;

        /// <summary>
        /// Occurs after a panel is removed.
        /// </summary>
        event EventHandler<DockablePanelEventArgs> PanelRemoved;

        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified panel.
        /// </summary>
        /// <param name="panel">
        /// The panel.
        /// </param>
        void Add(DockablePanel panel);

        /// <summary>
        /// Hides the panel. A subsequent call to SelectPanel will show this panel in the same place it was when hidden.
        /// </summary>
        /// <param name="key">The key.</param>
        void HidePanel(string key);

        /// <summary>
        /// Removes the specified panel.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        void Remove(string key);

        /// <summary>
        /// Resets the layout of the dock panels to a developer specified location.
        /// </summary>
        void ResetLayout();

        /// <summary>
        /// Activates and selects the panel.
        /// </summary>
        /// <param name="key">
        /// The key.
        /// </param>
        void SelectPanel(string key);

        /// <summary>
        /// Shows the panel but does not select it.
        /// </summary>
        /// <param name="key">The key.</param>
        void ShowPanel(string key);

        #endregion
    }
}