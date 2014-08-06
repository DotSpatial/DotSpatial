// -----------------------------------------------------------------------
// <copyright file="StatusBarCoordinates.cs" company="">
// </copyright>
// -----------------------------------------------------------------------
using System;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Symbology;

namespace DemoMap
{
    /// <summary>
    /// Displays how much layers selected
    /// </summary>
    public class StatusShowSelectionCount : Extension
    {
        private StatusPanel panel;

        public override void Activate()
        {
            App.Map.SelectionChanged += Map_SelectionChanged;
            App.Map.MapFrame.LayerSelected += MapFrame_LayerSelected;

            panel = new StatusPanel {Width = 180};
            App.ProgressHandler.Add(panel);

            base.Activate();
        }

        void MapFrame_LayerSelected(object sender, LayerSelectedEventArgs e)
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (App.Map.Layers.SelectedLayer == null)
            {
                panel.Caption = "No selected layer";
            }
            else
            {
                var layer = App.Map.Layers.SelectedLayer as IMapFeatureLayer;
                if (layer != null)
                {
                    panel.Caption = String.Format("{0}: {1} feature{2} selected", layer.LegendText, layer.Selection.Count, layer.Selection.Count == 1 ? null : "s");
                }
            }
        }

        void Map_SelectionChanged(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        public override void Deactivate()
        {
            App.Map.SelectionChanged -= Map_SelectionChanged;
            App.Map.MapFrame.LayerSelected -= MapFrame_LayerSelected;

            App.ProgressHandler.Remove(panel);

            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }
    }
}
