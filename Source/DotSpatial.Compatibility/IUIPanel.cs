// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Windows.Forms;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// A function that is called upon close of a panel. The name (caption) of the closed panel is passed into the OnPanelClose function.
    /// </summary>
    /// <param name="caption">Caption of the closed panel.</param>
    public delegate void OnPanelClose(string caption);

    /// <summary>
    /// UiPanel.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public interface IUIPanel
    {
        #region Methods

        /// <summary>
        /// Adds a function (onCloseFunction) which is called when the panel specified by caption is closed.
        /// </summary>
        /// <param name="caption">Caption of the closed panel.</param>
        /// <param name="onCloseFunction">Close function that gets added.</param>
        void AddOnCloseHandler(string caption, OnPanelClose onCloseFunction);

        /// <summary>
        /// Returns a Panel that can be used to add dockable content to DotSpatial.
        /// </summary>
        /// <param name="caption">Caption of the closed panel.</param>
        /// <param name="dockStyle">Specifies the position and manner in which the control is docked.</param>
        /// <returns>Panel that can be used to add dockable content to DotSpatial.</returns>
        Panel CreatePanel(string caption, SpatialDockStyle dockStyle);

        /// <summary>
        /// Returns a Panel that can be used to add dockable content to DotSpatial.
        /// </summary>
        /// <param name="caption">Caption of the closed panel.</param>
        /// <param name="dockStyle">Specifies the position and manner in which the control is docked.</param>
        /// <returns>Panel that can be used to add dockable content to DotSpatial.</returns>
        Panel CreatePanel(string caption, DockStyle dockStyle);

        /// <summary>
        /// Deletes the specified panel.
        /// </summary>
        /// <param name="caption">Caption of the closed panel.</param>
        void DeletePanel(string caption);

        /// <summary>
        /// Hides or shows a panel without necessarily deleting it.
        /// </summary>
        /// <param name="caption">Caption of the closed panel.</param>
        /// <param name="visible">Indicates whether the panel should be visible.</param>
        void SetPanelVisible(string caption, bool visible);

        #endregion
    }
}