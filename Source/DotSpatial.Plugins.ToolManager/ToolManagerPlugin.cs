// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;
using DotSpatial.Controls.Header;
using DotSpatial.Modeling.Forms;

namespace DotSpatial.Plugins.ToolManager
{
    /// <summary>
    /// The Toolmanager plugin is used to load the ToolManager from the Controls project into the Tools tab.
    /// </summary>
    public class ToolManagerPlugin : Extension, IPartImportsSatisfiedNotification
    {
        private Controls.ToolManager _toolManager;
        private SimpleActionItem _toolsButton;
        private DockablePanel _toolsPanel;

        [Import("Shell", typeof(ContainerControl))]
        private ContainerControl Shell { get; set; }

        /// <summary>
        /// Gets or sets the list tools available.
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        private IEnumerable<ITool> Tools { get; set; }

        #region IPartImportsSatisfiedNotification Members

        /// <summary>
        /// Shows the tool panel after the imports are satisfied.
        /// </summary>
        public void OnImportsSatisfied()
        {
            if (IsActive)
            {
                // This method may be called on another thread after recomposition.
                if (Shell.InvokeRequired)
                {
                    Shell.Invoke((MethodInvoker)ShowToolsPanel);
                }
                else
                {
                    ShowToolsPanel();
                }
            }
        }

        #endregion

        /// <inheritdoc />
        public override void Activate()
        {
            _toolsButton = new SimpleActionItem(Resources.ToolsButtonText, ButtonClick);
            App.HeaderControl.Add(_toolsButton);
            ShowToolsPanel();
            base.Activate();
            OnAppCultureChanged(App, App.AppCulture);
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            App.AppCultureChanged -= OnAppCultureChanged;
            App.HeaderControl.RemoveAll();
            App.DockManager.Remove("kTools");
            _toolManager = null;
            base.Deactivate();
        }

        /// <summary>
        /// Shows the tool panel.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        public void ButtonClick(object sender, EventArgs e)
        {
            ShowToolsPanel();
        }

        private void ShowToolsPanel()
        {
            if (Tools != null && Tools.Any())
            {
                if (_toolManager != null) return;
                _toolManager = new Controls.ToolManager
                {
                    App = App,
                    Legend = App.Legend,
                    Location = new Point(208, 12),
                    Name = "toolManager",
                    Size = new Size(192, 308),
                    TabIndex = 1
                };

                App.CompositionContainer.ComposeParts(_toolManager);
                Shell.Controls.Add(_toolManager);

                _toolsPanel = new DockablePanel("kTools", Resources.ToolsTabText, _toolManager, DockStyle.Left)
                { SmallImage = _toolManager.ImageList.Images["Hammer"] };
                App.DockManager.Add(_toolsPanel);
                App.AppCultureChanged += OnAppCultureChanged;
            }
            else
            {
                _toolManager = null;
                App.DockManager.Remove("kTools");
            }
        }

        private void OnAppCultureChanged(object sender, CultureInfo appCulture)
        {
            ExtensionCulture = appCulture;
            _toolManager.ToolManagerCulture = ExtensionCulture;
            UpdatePlugInItems();
        }

        private void UpdatePlugInItems()
        {
            _toolsPanel.Caption = Resources.ToolsTabText;
            _toolsButton.Caption = Resources.ToolsButtonText;

            // _toolManager.RefreshTree();
        }
    }
}