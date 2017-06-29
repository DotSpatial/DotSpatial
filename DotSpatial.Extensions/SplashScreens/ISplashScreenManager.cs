// -----------------------------------------------------------------------
// <copyright file="ISplashScreenManager.cs" company="DotSpatial Team">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.Composition;

namespace DotSpatial.Extensions.SplashScreens
{
    /// <summary>
    /// An interface that allows the creation of a splash screen extension, which will be loaded before other extensions.
    /// </summary>
    [InheritedExport(typeof(ISplashScreenManager))]
    public interface ISplashScreenManager
    {
        /// <summary>
        /// Show the Splash Screen.
        /// </summary>
        void Activate();

        /// <summary>
        ///
        /// </summary>
        /// <param name="cmd">A SplashScreenCommand enum value.</param>
        /// <param name="arg">The argument to pass to the command. See SplashScreenCommand for details reguarding each individual command.</param>
        void ProcessCommand(Enum cmd, object arg);

        /// <summary>
        /// Deactivates this instance.
        /// </summary>
        void Deactivate();
    }
}