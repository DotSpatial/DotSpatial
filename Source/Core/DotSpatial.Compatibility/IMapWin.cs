// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// IMapWin.
    /// </summary>
    public interface IMapWin
    {
        #region Properties

        /// <summary>
        /// Gets control over application-level settings like the app name.
        /// </summary>
        IAppInfo ApplicationInfo { get; }

        /// <summary>
        /// Sets a value indicating whether the full project path should be specified rather than just fileName, in title bar for main window.
        /// </summary>
        bool DisplayFullProjectPath { set; }

        /// <summary>
        /// Gets the underlying MapWinGIS activex control for advanced operations.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        object GetOCX { get; }

        /// <summary>
        /// Gets the last error message set.  Note:  This error message could have been set at any time.
        /// </summary>
        string LastError { get; }

        /// <summary>
        /// Gets the <c>Layers</c> object that handles layers.
        /// </summary>
        ILayers Layers { get; }

        /// <summary>
        /// Gets the <c>Menus</c> object that manages the menus.
        /// </summary>
        IMenus Menus { get; }

        /// <summary>
        /// Gets the <c>Plugins</c> object that manages plugins.
        /// </summary>
        IPlugins Plugins { get; }

        /// <summary>
        /// Gets the <c>PreviewMap</c> object that manages the preview map.
        /// </summary>
        IPreviewMap PreviewMap { get; }

        /// <summary>
        /// Gets the project and configuration files.
        /// </summary>
        IProject Project { get; }

        /// <summary>
        /// Gets report generation methods and properties.
        /// </summary>
        IReports Reports { get; }

        /// <summary>
        /// Gets the <c>StausBar</c> object that manages the status bar.
        /// </summary>
        IStatusBar StatusBar { get; }

        /// <summary>
        /// Gets the <c>Toolbar</c> object that manages toolbars.
        /// </summary>
        IToolbar Toolbar { get; }

        /// <summary>
        /// Gets the user panel in the lower right of the DotSpatial form.
        /// </summary>
        IUIPanel UiPanel { get; }

        /// <summary>
        /// Gets the user-interactive functions. Used to prompt users to enter things, or otherwise prompt users.
        /// </summary>
        IUserInteraction UserInteraction { get; }

        /// <summary>
        /// Gets the <c>View</c> object that handles the map view.
        /// </summary>
        IViewOld View { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns dialog title for the main window to the default "project name" title.
        /// </summary>
        void ClearCustomWindowTitle();

        /// <summary>
        ///  Prompt the user to select a projection, and return the PROJ4 representation of this
        ///  projection. Specify the dialog caption and an optional default projection ("" for none).
        /// </summary>
        /// <param name="dialogCaption">The text to be displayed on the dialog, e.g. "Please select a projection.".</param>
        /// <param name="defaultProjection">The PROJ4 projection string of the projection to default to, "" for none.</param>
        /// <returns>PROJ4 of the selected projection.</returns>
        string GetProjectionFromUser(string dialogCaption, string defaultProjection);

        /// <summary>
        /// Refreshes the DotSpatial display.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Refreshes Dynamic Visibility.
        /// </summary>
        void RefreshDynamicVisibility();

        /// <summary>
        /// Sets the dialog title to be displayed after the "AppInfo" name for the main window.
        /// Overrides the default "project name" title.
        /// </summary>
        /// <param name="newTitleText">The new title.</param>
        void SetCustomWindowTitle(string newTitleText);

        /// <summary>
        /// Displays the DotSpatial error dialog.
        /// </summary>
        /// <param name="ex">The exception to show.</param>
        void ShowErrorDialog(Exception ex);

        /// <summary>
        /// Displays the DotSpatial error dialog, sending to a specific address.
        /// </summary>
        /// <param name="ex">Exception to show.</param>
        /// <param name="sendEmailTo">Email adress.</param>
        void ShowErrorDialog(Exception ex, string sendEmailTo);

        #endregion
    }
}