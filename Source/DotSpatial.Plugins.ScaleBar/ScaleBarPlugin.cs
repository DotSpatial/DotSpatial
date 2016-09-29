// *******************************************************************************************************
// Product: DotSpatial.Plugins.ScaleBar.ScaleBarPlugin.cs
// Description:  An extension to show and change the mapscale.
// Copyright & License: See www.DotSpatial.org.
// *******************************************************************************************************
// Contributor(s): Open source contributors may list themselves and their modifications here.
// Contribution of code constitutes transferral of copyright from authors to DotSpatial copyright holders. 
//--------------------------------------------------------------------------------------------------------
// Name               |   Date             |         Comments
//--------------------|--------------------|--------------------------------------------------------------
// jany_              |  08/2014           | Created. Unsure whether this works for lat/long based maps.
// ******************************************************************************************************* 

using System;
using System.Globalization;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;
using DotSpatial.Data;
using DotSpatial.Plugins.ScaleBar.Properties;
using DotSpatial.Symbology;
using NetTopologySuite.Geometries;

namespace DotSpatial.Plugins.ScaleBar
{
    public class ScaleBarPlugin : Extension
    {
        #region Constants and Fields

        private const string StrKeyScaleBarDropDown = "kScaleBarDropDown";

        private DropDownActionItem _scaleDropDown;
        private ToolStripComboBox _combo;
        private bool _ignore;

        #endregion

        public ScaleBarPlugin()
        {
            DeactivationAllowed = false;
        }

        #region Public Methods

        /// <summary>
        /// Initialize the DotSpatial plugin
        /// </summary>
        public override void Activate()
        {
            _scaleDropDown = new DropDownActionItem
            {
                AllowEditingText = true,
                Caption = Resources.ScaleBar_Box_Text,
                ToolTipText = Resources.ScaleBar_Box_ToolTip,
                Width = 45,
                Key = StrKeyScaleBarDropDown
            };
            _scaleDropDown.Items.Add("[" + Resources.Custom + "]");

            foreach (int k in new[] { 100, 250, 500, 1000, 1500, 2250 })
            {
                _scaleDropDown.Items.Add("1 : " + k.ToString("n0", CultureInfo.CurrentCulture));
            }

            // Paul Meems - August 17 2010, use more or less the same scales as OSM uses for their zoom levels:
            // From http://wiki.openstreetmap.org/wiki/FAQ#What_is_the_map_scale_for_a_particular_zoom_level_of_the_map.3F
            int scale = 2250;
            for (int i = 1; i <= 18; i++)
            {
                scale *= 2;
                _scaleDropDown.Items.Add("1 : " + scale.ToString("n0", CultureInfo.CurrentCulture));
            }

            _scaleDropDown.GroupCaption = Resources.Panel_Name;
            _scaleDropDown.SelectedValueChanged += ScaleToSelected;
            _scaleDropDown.RootKey = HeaderControl.HomeRootItemKey;

            //Add it to the Header
            _combo = App.HeaderControl.Add(_scaleDropDown) as ToolStripComboBox;
            if (_combo != null)
            {
                _combo.KeyPress += Combo_KeyPress;
            }

            ComputeMapScale();

            App.SerializationManager.Deserializing += SerializationManager_Deserializing;
            App.SerializationManager.NewProjectCreated += SerializationManager_Deserializing;
            AddHandler();

            base.Activate();
        }

        /// <summary>
        /// Fires when the plugin should become inactive
        /// </summary>
        public override void Deactivate()
        {
            _scaleDropDown.SelectedValueChanged -= ScaleToSelected;
            if (_combo != null)
            {
                _combo.KeyPress -= Combo_KeyPress;
            }
            _combo = null;
            _scaleDropDown = null;

            App.SerializationManager.Deserializing -= SerializationManager_Deserializing;
            App.SerializationManager.NewProjectCreated -= SerializationManager_Deserializing;
            App.HeaderControl.RemoveAll();
            RemoveHandler();

            base.Deactivate();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add EventHandlers to Map when a new project gets created or an old project gets opened.
        /// </summary>
        private void SerializationManager_Deserializing(object sender, SerializingEventArgs e)
        {
            AddHandler();
            ComputeMapScale();
        }

        /// <summary>
        /// Handles the input of user defined scales.
        /// </summary>
        private void Combo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_ignore) return;

            if (e.KeyChar == 13) //Enter starts scaling
            {
                var text = _combo.Text;
                if (string.IsNullOrWhiteSpace(text)) return;
                if (text.Contains(":")) text = text.Substring(text.IndexOf(":", StringComparison.InvariantCulture) + 1);

                double nr;
                if (double.TryParse(text, out nr)) ScaleTo(nr);
            }
            else if (!((e.KeyChar > 47 && e.KeyChar < 58) || e.KeyChar == 8)) //don't allow keys that aren't numbers or backspace
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Correct MapScale after layers got added.
        /// </summary>
        private void LayerAdded(object sender, LayerEventArgs e)
        {
            ComputeMapScale();
        }

        /// <summary>
        /// Show the new MapScale in Combobox when the MapFrameExtent was changed.
        /// </summary>
        private void MapFrameExtentsChanged(object sender, ExtentArgs e)
        {
            ComputeMapScale();
        }

        /// <summary>
        /// Resize the Map according to the selected scale.
        /// </summary>
        private void ScaleToSelected(object sender, SelectedValueChangedEventArgs e)
        {
            if (_ignore) return;
            string str = e.SelectedItem.ToString();
            if (!str.Contains("["))
            {
                double nr;
                if (double.TryParse(str.Substring(str.IndexOf(":", StringComparison.InvariantCulture) + 1), out nr))
                    ScaleTo(nr);
            }
        }

        /// <summary>
        /// Add the needed EventHandlers to the Map.
        /// </summary>
        private void AddHandler()
        {
            RemoveHandler();
            App.Map.MapFrame.ViewExtentsChanged += MapFrameExtentsChanged;
            App.Map.LayerAdded += LayerAdded;
        }

        /// <summary>
        /// Remove the used EventHandlers from the Map.
        /// </summary>
        private void RemoveHandler()
        {
            App.Map.MapFrame.ViewExtentsChanged -= MapFrameExtentsChanged;
            App.Map.LayerAdded -= LayerAdded;
        }

        /// <summary>
        /// Resize the Map to the given scale.
        /// </summary>
        /// <param name="scale"></param>
        private void ScaleTo(double scale)
        {
            if (scale == 0 || App.Map.Projection == null) return;

            var ext = App.Map.ViewExtents;

            //TODO this works for Meter-based coordinate-systems. How must this be done for lat/long?
            if (ext.Width != 0)
            {
                Point centerpoint = new Point((ext.MinX + ext.MaxX) / 2, (ext.MinY + ext.MaxY) / 2);
                const double dInchesPerMeter = 39.3700787401575;
                double dScreenWidthInMeters = (App.Map.BufferedImage.Width / App.Map.BufferedImage.HorizontalResolution) / dInchesPerMeter;
                double newwidth = ((scale * dScreenWidthInMeters) / App.Map.Projection.Unit.Meters) / 2;
                double newheight = ((App.Map.ViewExtents.Height * newwidth) / App.Map.ViewExtents.Width) / 2;
                App.Map.ViewExtents = new Extent(centerpoint.X - newwidth, centerpoint.Y - newheight, centerpoint.X + newwidth, centerpoint.Y + newheight);
            }
            if (_combo != null && _combo.Owner != null) _combo.Owner.Focus(); //Remove focus because users expect focus to leave on pressing enter.
        }

        /// <summary>
        /// Gets the scale of the Maps ViewExtents.
        /// </summary>
        private void ComputeMapScale()
        {
            try
            {
                //Code posted by kellison (https://dotspatial.codeplex.com/discussions/351173)
                const double dInchesPerMeter = 39.3700787401575;
                const double dDegreesPerRadian = 57.2957;
                double dMapWidthInMeters;

                if (App.Map.Projection == null)
                    return;

                if (App.Map.Projection.IsLatLon)
                {
                    var dMapWidthInRadians = App.Map.ViewExtents.Width * App.Map.Projection.GeographicInfo.Unit.Radians;
                    var dMapWidthInDegrees = dMapWidthInRadians * dDegreesPerRadian;
                    var dMapLatInRadians = App.Map.ViewExtents.Center.Y * App.Map.Projection.GeographicInfo.Unit.Radians;
                    var dMapLatInDegrees = dMapLatInRadians * dDegreesPerRadian;
                    dMapWidthInMeters = MetersFromDecimalDegreesPoints(0.0, dMapLatInDegrees, dMapWidthInDegrees, dMapLatInDegrees);
                }
                else
                {
                    dMapWidthInMeters = App.Map.ViewExtents.Width * App.Map.Projection.Unit.Meters;
                }

                // Get the number of pixels in one screen inch.
                // get resolution, most screens are 96 dpi, but you never know...
                double dScreenWidthInMeters = (Convert.ToDouble(App.Map.BufferedImage.Width) / App.Map.BufferedImage.HorizontalResolution) / dInchesPerMeter;
                double dMetersPerScreenMeter = dMapWidthInMeters / dScreenWidthInMeters;
                string res = "1 : " + dMetersPerScreenMeter.ToString("n0", CultureInfo.CurrentCulture);
                int index = _combo.Items.IndexOf(res);
                _ignore = true;
                if (index > -1)
                {
                    _combo.SelectedIndex = index;
                }
                else
                {
                    _combo.Items[0] = "1 : " + dMetersPerScreenMeter.ToString("n02", CultureInfo.CurrentCulture);
                    _combo.SelectedIndex = 0;
                }
                _ignore = false;
            }
            catch (Exception)
            {
                // added this catch because in debug mode _combo is accessed from a different threat that causes unwanted errors
                // can this error be removed differently?
            }
        }

        /// <summary>
        /// Uses spherical approximation method to return distance in meters between two decimal degree points.
        /// </summary>
        /// <param name="dDegX1"></param>
        /// <param name="dDegY1"></param>
        /// <param name="dDegX2"></param>
        /// <param name="dDegY2"></param>
        /// <returns></returns>
        private static double MetersFromDecimalDegreesPoints(double dDegX1, double dDegY1, double dDegX2, double dDegY2)
        {
            //Code posted by kellison (https://dotspatial.codeplex.com/discussions/351173)
            try
            {
                const double dRadius = 6378007; // radius of Earth in meters
                const double dCircumference = dRadius * 2 * Math.PI;
                const double dMetersPerLatDd = 111113.519;

                double dDeltaXdd = Math.Abs(dDegX1 - dDegX2);
                double dDeltaYdd = Math.Abs(dDegY1 - dDegY2);
                double dCenterY = (dDegY1 + dDegY2) / 2.0;
                double dMetersPerLongDd = (Math.Cos(dCenterY * (Math.PI / 180.0)) * dCircumference) / 360.0;
                double dDeltaXmeters = dMetersPerLongDd * dDeltaXdd;
                double dDeltaYmeters = dMetersPerLatDd * dDeltaYdd;

                return Math.Sqrt(Math.Pow(dDeltaXmeters, 2.0) + Math.Pow(dDeltaYmeters, 2.0));
            }
            catch { return 0.0; }
        }

        #endregion
    }
}
