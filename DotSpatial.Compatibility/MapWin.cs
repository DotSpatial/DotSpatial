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
        #region Private Variables

        // New Stuff

        // Legacy Stuff
        private readonly IUserInteraction _userInteraction;
        private IAppInfo _appInfo;
        private bool _displayFullProjectPath;
        private string _lastError;
        private ILegend _legend;
        private Form _mainForm;
        private IBasicMap _map;
        private MenuStrip _menuStrip;

        #endregion

        /// <summary>
        /// Constructs a new empty instance of this interface wrapper that is mostly used
        /// for backwards compatibility.
        /// </summary>
        public MapWin()
        {
            _appInfo = new AppInfo();
        }

        /// <summary>
        /// Constructs a new instance of a MapWin interface where the Map, Legend, Form and MenuStrip
        /// are all specified.
        /// </summary>
        /// <param name="inMap">Any valid implementation of IBasicMap</param>
        /// <param name="inLegend">Any valid implementation of ILegend</param>
        /// <param name="inMainForm">Any valid windows Form</param>
        /// <param name="inMenuStrip">Any valid windows MenuStrip</param>
        public MapWin(IBasicMap inMap, ILegend inLegend, Form inMainForm, MenuStrip inMenuStrip)
        {
            _map = inMap;
            _legend = inLegend;
            _mainForm = inMainForm;
            _menuStrip = inMenuStrip;
            _userInteraction = new UserInteraction();
        }

        #region Methods

        /// <summary>
        /// Returns dialog title for the main window to the default "project name" title.
        /// </summary>
        public void ClearCustomWindowTitle()
        {
            _mainForm.Text = "DotSpatial 6.0";
        }

        /// <summary>
        /// In The new context, this return whatever the BasicMap is.
        /// </summary>
        public object GetOCX
        {
            get { return _map; }
        }

        /// <summary>
        ///  Prompt the user to select a projection, and return the PROJ4 representation of this
        ///  projection. Specify the dialog caption and an optional default projection ("" for none).
        /// </summary>
        /// <param name="dialogCaption">The text to be displayed on the dialog, e.g. "Please select a projection."</param>
        /// <param name="defaultProjection">The PROJ4 projection string of the projection to default to, "" for none.</param>
        /// <returns></returns>
        public string GetProjectionFromUser(string dialogCaption, string defaultProjection)
        {
            //bool TO_DO_PROJECTION_RETRIEVAL_FORMS = true;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Refreshes the DotSpatial display.
        /// </summary>
        public void Refresh()
        {
            _map.RefreshMap(_map.ClientRectangle);
        }

        /// <summary>
        /// Refreshes Dynamic Visibility
        /// </summary>
        public void RefreshDynamicVisibility()
        {
            //bool To_DO_DYNAMIC_VISIBILITY = true;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the dialog title to be displayed after the "AppInfo" name for the main window.
        /// Overrides the default "project name" title.
        /// </summary>
        public void SetCustomWindowTitle(string newTitleText)
        {
            _mainForm.Text = newTitleText;
        }

        /// <summary>
        /// Displays the DotSpatial error dialog.
        /// </summary>
        /// <param name="ex"></param>
        public void ShowErrorDialog(Exception ex)
        {
            //bool TO_DO_ERROR_DIALOG = true;
            throw new NotImplementedException();
        }

        /// <summary>
        /// Displays the DotSpatial error dialog, sending to a specific address.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="sendEmailTo"></param>
        public void ShowErrorDialog(Exception ex, string sendEmailTo)
        {
            //bool TO_DO_ERROR_DIALOG = true;
            throw new NotImplementedException();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Provides control over application-level settings like the app name.
        /// </summary>
        public IAppInfo ApplicationInfo
        {
            get { return _appInfo; }
            set { _appInfo = value; }
        }

        /// <summary>
        /// Specify whether the full project path should be specified rather than just fileName, in title bar for main window.
        /// </summary>
        public bool DisplayFullProjectPath
        {
            get { return _displayFullProjectPath; }
            set { _displayFullProjectPath = value; }
        }

        /// <summary>
        /// Gets the last error message set.  Note:  This error message could have been set at any time.
        /// </summary>
        public string LastError
        {
            get { return _lastError; }
            set { _lastError = value; }
        }

        /// <summary>
        /// Returns the <c>Layers</c> object that handles layers.
        /// </summary>
        public ILayers Layers
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Returns the <c>Menus</c> object that manages the menus.
        /// </summary>
        public IMenus Menus
        {
            get
            {
                return new Menus(_menuStrip);
            }
        }

        /// <summary>
        /// Returns the <c>Plugins</c> object that manages plugins.
        /// </summary>
        public IPlugins Plugins
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Returns the <c>PreviewMap</c> object that manages the preview map.
        /// </summary>
        public IPreviewMap PreviewMap
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Provides control over project and configuration files.
        /// </summary>
        public IProject Project
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Provides access to report generation methods and properties.
        /// </summary>
        public IReports Reports
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Returns the <c>StausBar</c> object that manages the status bar.
        /// </summary>
        public IStatusBar StatusBar
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Returns the <c>Toolbar</c> object that manages toolbars.
        /// </summary>
        public IToolbar Toolbar
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Provides access to the user panel in the lower right of the DotSpatial form.
        /// </summary>
        public IUIPanel UiPanel
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// User-interactive functions. Used to prompt users to enter things, or otherwise prompt users.
        /// </summary>
        public IUserInteraction UserInteraction
        {
            get { return _userInteraction; }
        }

        /// <summary>
        /// Returns the <c>View</c> object that handles the map view.
        /// </summary>
        public IViewOld View
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region Link Properties

        /// <summary>
        /// Gets or sets the basic map for this MapWin
        /// </summary>
        public IBasicMap Map
        {
            get { return _map; }
            set { _map = value; }
        }

        /// <summary>
        /// Gets or sets the legend to use for this MapWin
        /// </summary>
        public ILegend Legend
        {
            get { return _legend; }
            set { _legend = value; }
        }

        /// <summary>
        /// Gets or sets the main form to use for this MapWin
        /// </summary>
        public Form MainForm
        {
            get { return _mainForm; }
            set { _mainForm = value; }
        }

        /// <summary>
        /// Gets or sets the menu strip to use for this MapWin
        /// </summary>
        public MenuStrip MenuStrip
        {
            get { return _menuStrip; }
            set { _menuStrip = value; }
        }

        #endregion
    }
}