// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;
using DotSpatial.Plugins.SimpleLegend.Properties;

namespace DotSpatial.Plugins.SimpleLegend
{
    /// <summary>
    /// Adds a simple legend to the dock manager.
    /// </summary>
    public class SimpleLegendPlugin : Extension
    {
        /// <inheritdoc />
        public override void Activate()
        {
            ShowLegend();
            base.Activate();
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            App.DockManager.Remove("kLegend");
            base.Deactivate();
        }

        private void ShowLegend()
        {
            var legend1 = new Legend { Text = Resources.Legend };
            if (App.Map != null)
            {
                App.Map.Legend = legend1;
            }

            App.Legend = legend1;
            App.DockManager.Add(new DockablePanel("kLegend", Resources.Legend, legend1, DockStyle.Left) { SmallImage = Resources.legend_16x16 });
        }
    }
}