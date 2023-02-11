// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;

namespace DemoCustomLayer.DemoCustomLayerExtension
{
    /// <summary>
    /// Shows how to create custom layers.
    /// </summary>
    public class DemoCustomLayerPlugin : Extension
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Activate()
        {
            App.HeaderControl.Add(new SimpleActionItem("Add Custom Layer", ButtonClick));
            base.Activate();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ButtonClick(object sender, EventArgs e)
        {
            MyCustomLayer2 lay = new()
            {
                LegendText = "My Custom Layer"
            };
            App.Map.Layers.Add(lay);
        }
    }
}
