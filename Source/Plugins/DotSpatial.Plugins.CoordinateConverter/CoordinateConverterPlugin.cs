// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Plugins.CoordinateConverter.Properties;
using DotSpatial.Symbology;
using static System.Windows.Forms.DataFormats;

namespace DotSpatial.Plugins.CoordinateConverter
{
    /// <summary>
    /// This plugin adds a simple coordinate converter from one projection to another.
    /// </summary>
    public class CoordinateConverterPlugin : Extension
    {
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

        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        public void ButtonClick(object sender, EventArgs e)
        {
            FormCoordConverter frm = new FormCoordConverter(App);
            frm.Show();
        }
    }
}