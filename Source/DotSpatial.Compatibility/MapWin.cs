// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/21/2009 1:00:46 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Windows.Forms;

using DotSpatial.Controls;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// MapWin
    /// </summary>
    public class MapWin : IMapWin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapWin"/> class that is empty and mostly used
        /// for backwards compatibility.
        /// </summary>
        public MapWin()
        {
            ApplicationInfo = new AppInfo();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapWin"/> class where the Map, Legend, Form and MenuStrip
        /// are all specified.
        /// </summary>
        /// <param name="inMap">Any valid implementation of IBasicMap</param>
        /// <param name="inLegend">Any valid implementation of ILegend</param>
        /// <param name="inMainForm">Any valid windows Form</param>
        /// <param name="inMenuStrip">Any valid windows MenuStrip</param>
        public MapWin(IBasicMap inMap, ILegend inLegend, Form inMainForm, MenuStrip inMenuStrip)
        {
            Map = inMap;
            Legend = inLegend;
            MainForm = inMainForm;
            MenuStrip = inMenuStrip;
            UserInteraction = new UserInteraction();
        }

        #region Properties

        /// <summary>
        /// Gets or sets access to application-level settings like the app name.
        /// </summary>
        public IAppInfo ApplicationInfo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the full project path should be specified rather than just fileName, in title bar for main window.
        /// </summary>
        public bool DisplayFullProjectPath { get; set; }

        /// <summary>
        /// Gets whatever the BasicMap is.
        /// </summary>
        public object GetOCX => Map;

        /// <summary>
        /// Gets or sets the last error message set. Note: This error message could have been set at any time.
        /// </summary>
        public string LastError { get; set; }

        /// <summary>
        /// Gets the <c>Layers</c> object that handles layers.
        /// </summary>
        public ILayers Layers
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets or sets the legend to use for this MapWin
        /// </summary>
        public ILegend Legend { get; set; }

        /// <summary>
        /// Gets or sets the main form to use for this MapWin
        /// </summary>
        public Form MainForm { get; set; }

        /// <summary>
        /// Gets or sets the basic map for this MapWin
        /// </summary>
        public IBasicMap Map { get; set; }

        /// <summary>
        /// Gets the <c>Menus</c> object that manages the menus.
        /// </summary>
        public IMenus Menus => new Menus(MenuStrip);

        /// <summary>
        /// Gets or sets the menu strip to use for this MapWin
        /// </summary>
        public MenuStrip MenuStrip { get; set; }

        /// <summary>
        /// Gets the <c>Plugins</c> object that manages plugins.
        /// </summary>
        public IPlugins Plugins
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the <c>PreviewMap</c> object that manages the preview map.
        /// </summary>
        public IPreviewMap PreviewMap
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets control over project and configuration files.
        /// </summary>
        public IProject Project
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets access to report generation methods and properties.
        /// </summary>
        public IReports Reports
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the <c>StausBar</c> object that manages the status bar.
        /// </summary>
        public IStatusBar StatusBar
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the <c>Toolbar</c> object that manages toolbars.
        /// </summary>
        public IToolbar Toolbar
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets access to the user panel in the lower right of the DotSpatial form.
        /// </summary>
        public IUIPanel UiPanel
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets User-interactive functions. Used to prompt users to enter things, or otherwise prompt users.
        /// </summary>
        public IUserInteraction UserInteraction { get; }

        /// <summary>
        /// Gets the <c>View</c> object that handles the map view.
        /// </summary>
        public IViewOld View
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns dialog title for the main window to the default "project name" title.
        /// </summary>
        public void ClearCustomWindowTitle()
        {
            MainForm.Text = @"DotSpatial 6.0";
        }

        /// <summary>
        ///  Prompt the user to select a projection, and return the PROJ4 representation of this
        ///  projection. Specify the dialog caption and an optional default projection ("" for none).
        /// </summary>
        /// <param name="dialogCaption">The text to be displayed on the dialog, e.g. "Please select a projection."</param>
        /// <param name="defaultProjection">The PROJ4 projection string of the projection to default to, "" for none.</param>
        /// <returns>The PROJ4 string of the selected projection.</returns>
        public string GetProjectionFromUser(string dialogCaption, string defaultProjection)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Refreshes the DotSpatial display.
        /// </summary>
        public void Refresh()
        {
            Map.RefreshMap(Map.ClientRectangle);
        }

        /// <summary>
        /// Refreshes Dynamic Visibility
        /// </summary>
        public void RefreshDynamicVisibility()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the dialog title to be displayed after the "AppInfo" name for the main window.
        /// Overrides the default "project name" title.
        /// </summary>
        /// <param name="newTitleText">The new title.</param>
        public void SetCustomWindowTitle(string newTitleText)
        {
            MainForm.Text = newTitleText;
        }

        /// <summary>
        /// Displays the DotSpatial error dialog.
        /// </summary>
        /// <param name="ex">The error message to show.</param>
        public void ShowErrorDialog(Exception ex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Displays the DotSpatial error dialog, sending to a specific address.
        /// </summary>
        /// <param name="ex">The error message to show.</param>
        /// <param name="sendEmailTo">An email adress.</param>
        public void ShowErrorDialog(Exception ex, string sendEmailTo)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}