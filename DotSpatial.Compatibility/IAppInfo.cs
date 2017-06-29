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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 11:09:38 AM
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
    public interface IAppInfo
    {
        #region Properties

        /// <summary>
        /// The name of the main application.
        /// </summary>
        string ApplicationName { get; set; }

        /// <summary>
        /// The default directory for file dialogs
        /// </summary>
        string DefaultDir { get; set; }

        /// <summary>
        /// The icon to be displayed as the default form icon
        /// </summary>
        Icon FormIcon { get; set; }

        /// <summary>
        /// The path to the help file to be displayed from the Help menu.
        /// </summary>
        string HelpFilePath { get; set; }

        /// <summary>
        /// Whether or not to show a welcome screen (overriding the Splash Screen)
        /// </summary>
        bool ShowWelcomeScreen { get; set; }

        /// <summary>
        /// The image to be displayed on the splash screen.
        /// </summary>
        Image SplashPicture { get; set; }

        /// <summary>
        /// How long the splash screen should be displayed
        /// </summary>
        double SplashTime { get; set; }

        /// <summary>
        /// The URL to be displayed on the Help->About dialog.
        /// </summary>
        string URL { get; set; }

        /// <summary>
        /// Whether to display a splash screen on starting the application
        /// </summary>
        bool UseSplashScreen { get; set; }

        /// <summary>
        /// The name of the plugin responsible for displaying a custom welcome screen
        /// in response to the WELCOME_SCREEN message.
        /// </summary>
        string WelcomePlugin { get; set; }

        #endregion
    }
}