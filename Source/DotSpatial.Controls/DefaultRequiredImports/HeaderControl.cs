// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls.Header;
using DotSpatial.Extensions;

namespace DotSpatial.Controls.DefaultRequiredImports
{
    /// <summary>
    /// Default Header control. It will be used when no custom implementation of IHeaderControl can be found.
    /// </summary>
    [Export(typeof(IHeaderControl))]
    [DefaultRequiredImport]
    internal class HeaderControl : MenuBarHeaderControl, ISatisfyImportsExtension
    {
        #region Fields

        private bool _isActivated;

        #endregion

        #region Properties

        /// <inheritdoc />
        public int Priority => 1;

        [Import]
        private AppManager App { get; set; }

        [Import("Shell", typeof(ContainerControl))]
        private ContainerControl Shell { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public void Activate()
        {
            if (_isActivated) return;

            var headerControls = App.CompositionContainer.GetExportedValues<IHeaderControl>().ToList();

            // Activate only if there are no other IHeaderControl implementations and
            // custom HeaderControl not yet set
            if (App.HeaderControl == null && headerControls.Count == 1 && headerControls[0].GetType() == GetType())
            {
                _isActivated = true;

                var container = new ToolStripPanel
                {
                    Dock = DockStyle.Top
                };
                Shell.Controls.Add(container);

                var menuStrip = new MenuStrip
                {
                    Name = DefaultGroupName,
                    Dock = DockStyle.Top
                };
                Shell.Controls.Add(menuStrip);

                Initialize(container, menuStrip);
                App.ExtensionsActivated += (sender, args) => LoadToolstrips();

                // Add default buttons
                new DefaultMenuBars(App).Initialize(this);
            }
        }

        #endregion
    }
}