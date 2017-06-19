using DotSpatial.Controls;
using DotSpatial.Controls.Header;

namespace DotSpatial.Examples.AppManagerCustomizationDesignTime
{
    /// <summary>
    /// Displays latitude and longitude coordinates at the current cursor position.
    /// This also shows that extensions can be loaded into design time controls (e.g. into ProgressHandler).
    /// </summary>
    public class StatusShowCoordinates : Extension
    {
        private Map _map;
        private StatusPanel _xPanel;
        private StatusPanel _yPanel;

        public override void Activate()
        {
            _map = (Map)App.Map;
            _map.GeoMouseMove += MapGeoMouseMove;

            _xPanel = new StatusPanel { Width = 160 };
            _yPanel = new StatusPanel { Width = 160 };
            App.ProgressHandler.Add(_xPanel);
            App.ProgressHandler.Add(_yPanel);

            base.Activate();
        }

        public override void Deactivate()
        {
            _map.GeoMouseMove -= MapGeoMouseMove;

            App.ProgressHandler.Remove(_xPanel);
            App.ProgressHandler.Remove(_yPanel);

            App.HeaderControl.RemoveAll();
            base.Deactivate();
        }

        private void MapGeoMouseMove(object sender, GeoMouseArgs e)
        {
            _xPanel.Caption = $"X: {e.GeographicLocation.X:.#####}";
            _yPanel.Caption = $"Y: {e.GeographicLocation.Y:.#####}";
        }
    }
}
