// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Plugins.CoordinateConverter.Properties;

namespace DotSpatial.Plugins.CoordinateConverter
{
    /// <summary>
    /// This plugin adds a simple coordinate converter from one projection to another.
    /// </summary>
    public class CoordinateConverterPlugin : Extension
    {
        /// <inheritdoc />
        public override void Activate()
        {
            App.HeaderControl.Add(new SimpleActionItem(HeaderControl.HomeRootItemKey, "Coordinate Converter", ButtonClick)
            {
                GroupCaption = "Map Tool",
                SmallImage = Resources.cc_16x16,
                LargeImage = Resources.cc_32x32
            });
            base.Activate();
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        /// <summary>
        /// Opens the converter window.
        /// </summary>
        public void ButtonClick(object sender, EventArgs e)
        {
            FormCoordConverter frm = new();
            frm.Show();
        }
    }
}