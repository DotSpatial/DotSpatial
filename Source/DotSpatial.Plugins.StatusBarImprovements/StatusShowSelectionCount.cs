// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Symbology;

namespace DotSpatial.Plugins.StatusBarImprovements
{
    /// <summary>
    /// Displays how many features are selected in which layer.
    /// </summary>
    public class StatusShowSelectionCount : Extension
    {
        #region Fields

        private StatusPanel _panel;

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void Activate()
        {
            App.Map.SelectionChanged += MapSelectionChanged;
            App.Map.MapFrame.LayerSelected += MapFrameLayerSelected;

            _panel = new StatusPanel
            {
                Width = 180
            };
            App.ProgressHandler.Add(_panel);

            base.Activate();
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            App.Map.SelectionChanged -= MapSelectionChanged;
            App.Map.MapFrame.LayerSelected -= MapFrameLayerSelected;

            App.ProgressHandler.Remove(_panel);

            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        private void MapSelectionChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void MapFrameLayerSelected(object sender, LayerSelectedEventArgs e)
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (App.Map.Layers.SelectedLayer == null)
            {
                _panel.Caption = "No selected layer";
            }
            else
            {
                var layer = App.Map.Layers.SelectedLayer as IMapFeatureLayer;
                if (layer != null)
                {
                    _panel.Caption = string.Format("{0}: {1} feature{2} selected", layer.LegendText, layer.Selection.Count, layer.Selection.Count == 1 ? null : "s");
                }
            }
        }

        #endregion
    }
}