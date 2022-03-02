// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;

namespace DotSpatial.Plugins.LiDAR
{
    /// <summary>
    /// This allows to add LiDAR layers to the project.
    /// </summary>
    public class LiDarProvider : Extension
    {
        #region Methods

        /// <inheritdoc />
        public override void Activate()
        {
            App.HeaderControl.Add(new SimpleActionItem("LiDARProvider", ButtonClick));
            base.Activate();
        }

        /// <summary>
        /// Adds a new LiDAR Layer.
        /// </summary>
        /// <param name="sender">Sender that raised the event.</param>
        /// <param name="e">The event args.</param>
        public void ButtonClick(object sender, EventArgs e)
        {
            LiDarLayer lay = new()
            {
                                 LegendText = "LiDARLayer"
                             };
            App.Map.Layers.Add(lay);
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        #endregion
    }
}