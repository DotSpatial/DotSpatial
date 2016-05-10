// -----------------------------------------------------------------------
// <copyright file="StatusBarCoordinates.cs" company="">
// </copyright>
// -----------------------------------------------------------------------

using System;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;

namespace DotSpatial.Plugins.StatusBarImprovements
{
    /// <summary>
    /// Displays latitude and longitude coordinates at the current cursor position.
    /// </summary>
    public class StatusShowCoordinates : Extension
    {
        private Map _Map;
        private StatusPanel xPanel;
        private StatusPanel yPanel;

        public override void Activate()
        {
            _Map = (Map)App.Map;
            _Map.GeoMouseMove += Map_GeoMouseMove;

            xPanel = new StatusPanel { Width = 160 };
            yPanel = new StatusPanel { Width = 160 };
            App.ProgressHandler.Add(xPanel);
            App.ProgressHandler.Add(yPanel);

            base.Activate();
        }

        public override void Deactivate()
        {
            _Map.GeoMouseMove -= Map_GeoMouseMove;

            App.ProgressHandler.Remove(xPanel);
            App.ProgressHandler.Remove(yPanel);

            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        private void Map_GeoMouseMove(object sender, GeoMouseArgs e)
        {
            xPanel.Caption = String.Format("X: {0:.#####}", e.GeographicLocation.X);
            yPanel.Caption = String.Format("Y: {0:.#####}", e.GeographicLocation.Y);
        }
    }
}
