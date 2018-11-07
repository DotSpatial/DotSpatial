// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Globalization;
using System.Threading;
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
        private SimpleActionItem _measureButton;

        /// <inheritdoc />
        public override void Activate()
        {
            if (App == null) return;
            App.AppCultureChanged += OnAppCultureChanged;
            _measureButton = new SimpleActionItem(HeaderControl.HomeRootItemKey, Resources.MeasureButton, MeasureToolClick)
                            { GroupCaption = "Map Tool", SmallImage = Resources.measure_16x16, LargeImage = Resources.measure_32x32 };
            App.HeaderControl?.Add(_measureButton);
            base.Activate();
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            if (App == null) return;
            App.AppCultureChanged -= OnAppCultureChanged;
            App.HeaderControl?.RemoveAll();
            base.Deactivate();
        }

        /// <summary>
        /// For AppCulture retrieving.
        /// </summary>
        /// <param name="sender"> sender</param>
        /// <param name="appCulture"> AppCulture</param>
        private void OnAppCultureChanged(object sender, CultureInfo appCulture)
        {
            ExtensionCulture = appCulture;

            if (_painter != null) _painter.MeasureFuncCulture = ExtensionCulture;
            if (_measureButton != null)
            {
                _measureButton.Caption = Resources.MeasureButton;
                _measureButton.ToolTipText = Resources.MeasureButton;
            }
        }

        private void MeasureToolClick(object sender, EventArgs e)
        {
            if (_painter == null)
                _painter = new MapFunctionMeasure(App.Map);
            _painter.MeasureFuncCulture = ExtensionCulture;

            if (!App.Map.MapFunctions.Contains(_painter))
                App.Map.MapFunctions.Add(_painter);

            App.Map.FunctionMode = FunctionMode.None;
            App.Map.Cursor = Cursors.Cross;
            _painter.Activate();
        }
    }
}