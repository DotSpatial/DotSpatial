using DotSpatial.Controls;
using DotSpatial.Controls.Header;

namespace DotSpatial.Plugins.StatusBarImprovements
{
    /// <summary>
    /// Displays latitude and longitude coordinates at the current cursor position.
    /// </summary>
    public class StatusShowCoordinates : Extension
    {
        #region Fields

        private Map _map;
        private StatusPanel _xPanel;
        private StatusPanel _yPanel;

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void Activate()
        {
            _map = (Map)App.Map;
            _map.GeoMouseMove += MapGeoMouseMove;

            _xPanel = new StatusPanel
                     {
                         Width = 160
                     };
            _yPanel = new StatusPanel
                     {
                         Width = 160
                     };
            App.ProgressHandler.Add(_xPanel);
            App.ProgressHandler.Add(_yPanel);

            base.Activate();
        }

        /// <inheritdoc />
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
            _xPanel.Caption = string.Format("X: {0:.#####}", e.GeographicLocation.X);
            _yPanel.Caption = string.Format("Y: {0:.#####}", e.GeographicLocation.Y);
        }

        #endregion
    }
}