// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 11:03:34 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// IMapWin
    /// </summary>
    public interface IMapWin
    {
        #region Methods

        /// <summary>
        /// Returns the underlying MapWinGIS activex control for advanced operations.
        /// </summary>
        object GetOCX { get; }

        /// <summary>
        /// Returns dialog title for the main window to the default "project name" title.
        /// </summary>
        void ClearCustomWindowTitle();

        /// <summary>
        ///  Prompt the user to select a projection, and return the PROJ4 representation of this
        ///  projection. Specify the dialog caption and an optional default projection ("" for none).
        /// </summary>
        /// <param name="dialogCaption">The text to be displayed on the dialog, e.g. "Please select a projection."</param>
        /// <param name="defaultProjection">The PROJ4 projection string of the projection to default to, "" for none.</param>
        /// <returns></returns>
        string GetProjectionFromUser(string dialogCaption, string defaultProjection);

        /// <summary>
        /// Refreshes the DotSpatial display.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Refreshes Dynamic Visibility
        /// </summary>
        void RefreshDynamicVisibility();

        /// <summary>
        /// Sets the dialog title to be displayed after the "AppInfo" name for the main window.
        /// Overrides the default "project name" title.
        /// </summary>
        void SetCustomWindowTitle(string newTitleText);

        /// <summary>
        /// Displays the DotSpatial error dialog.
        /// </summary>
        /// <param name="ex"></param>
        void ShowErrorDialog(Exception ex);

        /// <summary>
        /// Displays the DotSpatial error dialog, sending to a specific address.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="sendEmailTo"></param>
        void ShowErrorDialog(Exception ex, string sendEmailTo);

        #endregion

        #region Properties

        /// <summary>
        /// Provides control over application-level settings like the app name.
        /// </summary>
        IAppInfo ApplicationInfo { get; }

        /// <summary>
        /// Specify whether the full project path should be specified rather than just fileName, in title bar for main window.
        /// </summary>
        bool DisplayFullProjectPath { set; }

        /// <summary>
        /// Gets the last error message set.  Note:  This error message could have been set at any time.
        /// </summary>
        string LastError { get; }

        /// <summary>
        /// Returns the <c>Layers</c> object that handles layers.
        /// </summary>
        ILayers Layers { get; }

        /// <summary>
        /// Returns the <c>Menus</c> object that manages the menus.
        /// </summary>
        IMenus Menus { get; }

        /// <summary>
        /// Returns the <c>Plugins</c> object that manages plugins.
        /// </summary>
        IPlugins Plugins { get; }

        /// <summary>
        /// Returns the <c>PreviewMap</c> object that manages the preview map.
        /// </summary>
        IPreviewMap PreviewMap { get; }

        /// <summary>
        /// Provides control over project and configuration files.
        /// </summary>
        IProject Project { get; }

        /// <summary>
        /// Provides access to report generation methods and properties.
        /// </summary>
        IReports Reports { get; }

        /// <summary>
        /// Returns the <c>StausBar</c> object that manages the status bar.
        /// </summary>
        IStatusBar StatusBar { get; }

        /// <summary>
        /// Returns the <c>Toolbar</c> object that manages toolbars.
        /// </summary>
        IToolbar Toolbar { get; }

        /// <summary>
        /// Provides access to the user panel in the lower right of the DotSpatial form.
        /// </summary>
        IUIPanel UiPanel { get; }

        /// <summary>
        /// User-interactive functions. Used to prompt users to enter things, or otherwise prompt users.
        /// </summary>
        IUserInteraction UserInteraction { get; }

        /// <summary>
        /// Returns the <c>View</c> object that handles the map view.
        /// </summary>
        IViewOld View { get; }

        #endregion
    }
}