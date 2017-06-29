// -----------------------------------------------------------------------
// <copyright file="IDockManager.cs" company="DotSpatial Team">
// </copyright>
// -----------------------------------------------------------------------

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
        #region Public Events

        /// <summary>
        /// Occurs when the active panel is changed, meaning a difference panel is activated.
        /// </summary>
        event EventHandler<DockablePanelEventArgs> ActivePanelChanged;

        /// <summary>
        /// Occurs when a panel is closed, which means the panel can still be activated or removed.
        /// </summary>
        event EventHandler<DockablePanelEventArgs> PanelClosed;

        /// <summary>
        /// Occurs after a panel is added.
        /// </summary>
        event EventHandler<DockablePanelEventArgs> PanelAdded;

        /// <summary>
        /// Occurs after a panel is removed.
        /// </summary>
        event EventHandler<DockablePanelEventArgs> PanelRemoved;

        /// <summary>
        /// Occurs when a panel is hidden.
        /// </summary>
        event EventHandler<DockablePanelEventArgs> PanelHidden;

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the specified panel.
        /// </summary>
        /// <param name="panel">
        /// The panel.
        /// </param>
        void Add(DockablePanel panel);

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
        /// Hides the panel. A subsequent call to SelectPanel will show this panel in the same place it was when hidden.
        /// </summary>
        /// <param name="key">The key.</param>
        void HidePanel(string key);

        /// <summary>
        /// Shows the panel but does not select it. 
        /// </summary>
        /// <param name="key">The key.</param>
        void ShowPanel(string key);

        #endregion
    }
}