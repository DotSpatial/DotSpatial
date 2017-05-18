// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Plugins.Measure.Properties;

namespace DotSpatial.Plugins.Measure
{
    /// <summary>
    /// This plugin adds the possibility to measure things.
    /// </summary>
    public class MeasurePlugin : Extension
    {
        private MapFunctionMeasure _painter;

        /// <inheritdoc />
        public override void Activate()
        {
            App.HeaderControl.Add(new SimpleActionItem(HeaderControl.HomeRootItemKey, "Measure", MeasureToolClick) { GroupCaption = "Map Tool", SmallImage = Resources.measure_16x16, LargeImage = Resources.measure_32x32 });
            base.Activate();
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        private void MeasureToolClick(object sender, EventArgs e)
        {
            if (_painter == null)
                _painter = new MapFunctionMeasure(App.Map);

            if (!App.Map.MapFunctions.Contains(_painter))
                App.Map.MapFunctions.Add(_painter);

            App.Map.FunctionMode = FunctionMode.None;
            App.Map.Cursor = Cursors.Cross;
            _painter.Activate();
        }
    }
}