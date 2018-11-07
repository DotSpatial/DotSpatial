// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Globalization;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;

namespace DotSpatial.Plugins.SimpleMap
{
    /// <summary>
    /// Adds a simple map to the program.
    /// </summary>
    public class SimpleMapPlugin : Extension
    {
        private Map _map;
        private DockablePanel _mapPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMapPlugin"/> class.
        /// </summary>
        public SimpleMapPlugin()
        {
            DeactivationAllowed = false;
        }

        /// <summary>
        /// Gets the priority. It's a very low value so it will be loaded at the beginning.
        /// </summary>
        public override int Priority => -10000;

        /// <inheritdoc />
        public override void Activate()
        {
            ShowMap();
            App.AppCultureChanged += OnAppCultureChanged;
            base.Activate();
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            App.AppCultureChanged -= OnAppCultureChanged;
            App.HeaderControl.RemoveAll();
            App.DockManager.Remove("kMap");
            if (App.Map == _map) App.Map = null;
            base.Deactivate();
        }

        private void ShowMap()
        {
            _map = new Map { Text = "Map", Name = "Map", Legend = App.Legend };
            App.Map = _map;
            _mapPanel = new DockablePanel("kMap", SimpleMap.Map_Caption, _map, DockStyle.Fill);
            App.DockManager.Add(_mapPanel);
        }

        private void OnAppCultureChanged(object sender, CultureInfo appCulture)
        {
            ExtensionCulture = appCulture;
            UpdatePlugInItems();
        }

        private void UpdatePlugInItems()
        {
            _mapPanel.Caption = SimpleMap.Map_Caption;
        }
    }
}
