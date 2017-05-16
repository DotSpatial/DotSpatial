// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
        /// Gets or sets the name of the main application.
        /// </summary>
        string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the default directory for file dialogs
        /// </summary>
        string DefaultDir { get; set; }

        /// <summary>
        /// Gets or sets the icon to be displayed as the default form icon
        /// </summary>
        Icon FormIcon { get; set; }

        /// <summary>
        /// Gets or sets the path to the help file to be displayed from the Help menu.
        /// </summary>
        string HelpFilePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to show a welcome screen (overriding the Splash Screen)
        /// </summary>
        bool ShowWelcomeScreen { get; set; }

        /// <summary>
        /// Gets or sets the image to be displayed on the splash screen.
        /// </summary>
        Image SplashPicture { get; set; }

        /// <summary>
        /// Gets or sets how long the splash screen should be displayed
        /// </summary>
        double SplashTime { get; set; }

        /// <summary>
        /// Gets or sets the URL to be displayed on the Help->About dialog.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        string URL { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display a splash screen on starting the application
        /// </summary>
        bool UseSplashScreen { get; set; }

        /// <summary>
        /// Gets or sets the name of the plugin responsible for displaying a custom welcome screen
        /// in response to the WELCOME_SCREEN message.
        /// </summary>
        string WelcomePlugin { get; set; }

        #endregion
    }
}