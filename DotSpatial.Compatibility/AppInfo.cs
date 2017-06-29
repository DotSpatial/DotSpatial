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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/21/2009 1:21:59 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// AppInfo
    /// </summary>
    public class AppInfo : IAppInfo
    {
        #region Private Variables

        private string _applicationName;
        private string _defaultDir;
        private Icon _formIcon;
        private string _helpFilePath;
        private bool _showWelcomeScreen;
        private Image _splashPicture;
        private double _splashTime;
        private string _url;
        private bool _useSplashScreen;
        private string _welcomePlugin;

        #endregion

        #region Properties

        /// <summary>
        /// The name of the main application.
        /// </summary>
        public string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }

        /// <summary>
        /// The default directory for file dialogs
        /// </summary>
        public string DefaultDir
        {
            get { return _defaultDir; }
            set { _defaultDir = value; }
        }

        /// <summary>
        /// The icon to be displayed as the default form icon
        /// </summary>
        public Icon FormIcon
        {
            get { return _formIcon; }
            set { _formIcon = value; }
        }

        /// <summary>
        /// The path to the help file to be displayed from the Help menu.
        /// </summary>
        public string HelpFilePath
        {
            get { return _helpFilePath; }
            set { _helpFilePath = value; }
        }

        /// <summary>
        /// Whether or not to show a welcome screen (overriding the Splash Screen)
        /// </summary>
        public bool ShowWelcomeScreen
        {
            get { return _showWelcomeScreen; }
            set { _showWelcomeScreen = value; }
        }

        /// <summary>
        /// The image to be displayed on the splash screen.
        /// </summary>
        public Image SplashPicture
        {
            get { return _splashPicture; }
            set { _splashPicture = value; }
        }

        /// <summary>
        /// How long the splash screen should be displayed
        /// </summary>
        public double SplashTime
        {
            get { return _splashTime; }
            set { _splashTime = value; }
        }

        /// <summary>
        /// The URL to be displayed on the Help->About dialog.
        /// </summary>
        public string URL
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// Whether to display a splash screen on starting the application
        /// </summary>
        public bool UseSplashScreen
        {
            get { return _useSplashScreen; }
            set { _useSplashScreen = value; }
        }

        /// <summary>
        /// The name of the plugin responsible for displaying a custom welcome screen
        /// in response to the WELCOME_SCREEN message.
        /// </summary>
        public string WelcomePlugin
        {
            get { return _welcomePlugin; }
            set { _welcomePlugin = value; }
        }

        #endregion
    }
}