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

        private readonly StatusPanel _panel;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes a new Instance of the StatusShowSelectionCount class.
        /// </summary>
        public StatusShowSelectionCount()
        {
            _panel = new StatusPanel
            {
                Width = 180
            };
        }

        /// <inheritdoc />
        public override void Activate()
        {
            App.Map.SelectionChanged += MapSelectionChanged;
            App.Map.MapFrame.LayerSelected += MapFrameLayerSelected;
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
                if (App.Map.Layers.SelectedLayer is IMapFeatureLayer layer)
                {
                    _panel.Caption = string.Format("{0}: {1} feature{2} selected", layer.LegendText, layer.Selection.Count, layer.Selection.Count == 1 ? null : "s");
                }
            }
        }

        #endregion
    }
}