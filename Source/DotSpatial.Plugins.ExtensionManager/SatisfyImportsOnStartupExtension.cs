using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;
using DotSpatial.Controls.Header;
using DotSpatial.Extensions;
using DotSpatial.Plugins.ExtensionManager.Properties;

namespace DotSpatial.Plugins.ExtensionManager
{
    /// <summary>
    /// If a user doesn't have a DockManager or other required extensions, help them locate one.
    /// </summary>
    public class SatisfyImportsOnStartupExtension : ISatisfyImportsExtension
    {
        #region Fields

        private readonly Packages _packages = new Packages();

        #endregion

        #region Properties

        /// <inheritdoc />
        public int Priority => 0;

        /// <summary>
        /// Gets or sets the AppManager that is responsible for activating and deactivating plugins as well as coordinating
        /// all of the other properties.
        /// </summary>
        [Import]
        private AppManager App { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Activate()
        {
            bool isDockManagerNeeded = !App.CompositionContainer.GetExportedValues<IDockManager>().Any();
            bool isHeaderControlNeeded = !App.CompositionContainer.GetExportedValues<IHeaderControl>().Any();
            bool isStatusControlNeeded = !App.CompositionContainer.GetExportedValues<IStatusControl>().Any();

            if (isDockManagerNeeded && isHeaderControlNeeded && isStatusControlNeeded)
            {
                App.UpdateProgress("Downloading a Ribbon extension...");
                var package = _packages.Install("DotSpatial.Plugins.Ribbon");
                if (package == null)
                {
                    MessageBox.Show(Resources.NoHeaderControlErrorDownloadingRibbon);
                }
                else
                {
                    // Download the other extensions.
                    App.UpdateProgress("Downloading a DockManager extension...");
                    _packages.Install("DotSpatial.Plugins.DockManager");

                    App.UpdateProgress("Downloading a MenuBar extension...");
                    _packages.Install("DotSpatial.Plugins.MenuBar");
                    _packages.Install("DotSpatial.Plugins.Measure");
                    _packages.Install("DotSpatial.Plugins.TableEditor");

                    App.RefreshExtensions();
                }
            }
        }

        #endregion
    }
}