﻿// -----------------------------------------------------------------------
// <copyright file="SatisfyImportsOnStarupExtension.cs" company="DotSpatial Team">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;
using DotSpatial.Controls.Header;
using DotSpatial.Extensions;

namespace DotSpatial.Plugins.ExtensionManager
{
    /// <summary>
    /// If a user doesn't have a DockManager or other required extensions, help them locate one.
    /// </summary>
    public class SatisfyImportsOnStartupExtension : ISatisfyImportsExtension
    {
        private readonly Packages packages = new Packages();

        /// <summary>
        /// Gets the AppManager that is responsible for activating and deactivating plugins as well as coordinating
        /// all of the other properties.
        /// </summary>
        [Import]
        private AppManager App { get; set; }

        #region ISatisfyImportsExtension Members

        /// <inheritdoc />
        public void Activate()
        {
            bool isDockManagerNeeded = !App.CompositionContainer.GetExportedValues<IDockManager>().Any();
            bool isHeaderControlNeeded = !App.CompositionContainer.GetExportedValues<IHeaderControl>().Any();
            bool isStatusControlNeeded = !App.CompositionContainer.GetExportedValues<IStatusControl>().Any();

            if (isDockManagerNeeded && isHeaderControlNeeded && isStatusControlNeeded)
            {
                App.UpdateProgress("Downloading a Ribbon extension...");
                var package = packages.Install("DotSpatial.Plugins.Ribbon");
                if (package == null)
                {
                    MessageBox.Show("No HeaderControl was available, but we couldn't download the ribbon. Please make sure you are connected to the Internet.");
                }
                else
                {
                    // Download the other extensions.
                    App.UpdateProgress("Downloading a DockManager extension...");
                    packages.Install("DotSpatial.Plugins.DockManager");

                    App.UpdateProgress("Downloading a MenuBar extension...");
                    packages.Install("DotSpatial.Plugins.MenuBar");
                    packages.Install("DotSpatial.Plugins.Measure");
                    packages.Install("DotSpatial.Plugins.TableEditor");

                    App.RefreshExtensions();
                }
            }
        }

        /// <inheritdoc />
        public int Priority
        {
            get { return 0; }
        }

        #endregion
    }
}