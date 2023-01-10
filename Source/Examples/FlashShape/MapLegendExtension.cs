// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;

namespace SimpleApp
{
    /// <summary>
    /// Adds a map and a legend to the application.
    /// </summary>
    public class MapLegendExtension : Extension
    {
        /// <summary>
        /// Initialzes a new MapLegendExtension.
        /// </summary>
        public MapLegendExtension()
        {
            DeactivationAllowed = false;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override int Priority => -100;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Activate()
        {
            base.Activate();

            // Add legend
            Legend legend = new() { Text = "Legend" };
            App.Legend = legend;
            App.DockManager.Add(new DockablePanel("kLegend", "Legend", legend, DockStyle.Left));

            // Add map
            Map map = new() { Text = "Map", Legend = App.Legend };
            App.Map = map;
            App.DockManager.Add(new DockablePanel("kMap", "Map", map, DockStyle.Fill));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            App.DockManager.Remove("kLegend");
            App.DockManager.Remove("kMap");
            base.Deactivate();
        }
    }
}