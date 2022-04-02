// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using DotSpatial.Projections;

namespace DotSpatial.Controls
{
    /// <summary>
    /// The ZoomToCoordinatesDialog is used to zoom to a user given coordinate.
    /// </summary>
    public partial class ZoomToCoordinatesDialog : Form
    {
        #region Fields

        private const string RegExpression = "(-?\\d{1,3})[\\.\\,°]{0,1}\\s*(\\d{0,2})[\\.\\,\']{0,1}\\s*(\\d*)[\\.\\,°]{0,1}\\s*([NSnsEeWw]?)";
        private readonly double[] _lat;
        private readonly double[] _lon;

        private readonly IMap _map;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ZoomToCoordinatesDialog"/> class with the given map.
        /// </summary>
        /// <param name="map">Map that is used for zooming.</param>
        public ZoomToCoordinatesDialog(IMap map)
        {
            if (map == null) throw new ArgumentNullException(nameof(map));
            _map = map;
            InitializeComponent();

            lonStatus.Text = string.Empty;
            latStatus.Text = string.Empty;
            _lat = new double[3];
            _lon = new double[3];
        }

        #endregion

        #region Methods

        /// <summary>
        /// Turnes the given Degrees-Minutes-Seconds from ParseCoordinates into doubles.
        /// </summary>
        /// <param name="values">A list of values that containes the degrees, minutes and seconds that are converted to doubles.</param>
        /// <returns>The double coordinate that was calculated from the given drees, minutes, seconds.</returns>
        private static double LoadCoordinates(IList<double> values)
        {
            // Convert Degrees, Minutes, Seconds to x, y coordinates for both lat and long.
            var coor = values[2] / 100;
            coor += values[1];
            coor /= 100;
            coor += Math.Abs(values[0]);

            // Change signs to get to the right quadrant.
            if (values[0] < 0)
            {
                coor *= -1;
            }

            return coor;
        }

        /// <summary>ParseCoordinates will understand lat-lon coordinates in a variety of formats and separate them into Degrees, Minutes, and Seconds.
        /// We could just accept a simple decimal value for the coordinates, but since users might be copying and pasting from a variety of sources
        /// it makes it user friendly to be able to accept a number of different formats.</summary>
        /// <param name="values">An IList of doubles that is used to return the degrees, minutes and seconds. </param>
        /// <param name="text">The text that gets parsed to coordinates.</param>
        /// <returns>Returns a bool indicating whether the given text could be parsed.</returns>
        private static bool ParseCoordinates(IList<double> values, string text)
        {
            var match = Regex.Match(text, RegExpression);
            var groups = match.Groups;
            try
            {
                values[0] = double.Parse(groups[1].ToString());
                if (groups[2].Length > 0)
                {
                    values[1] = double.Parse(groups[2].ToString());
                    if (groups[2].Length == 1) values[1] *= 10;
                }

                if (groups[3].Length > 0)
                {
                    values[2] = double.Parse(groups[3].ToString());
                    if (groups[3].Length == 1) values[2] *= 10;
                }
            }
            catch
            {
                return false;
            }

            if ((groups[4].ToString().Equals("S", StringComparison.OrdinalIgnoreCase) || groups[4].ToString().Equals("W", StringComparison.OrdinalIgnoreCase)) && values[0] > 0)
            {
                values[0] *= -1;
            }

            return true;
        }

        private void AcceptButtonClick(object sender, EventArgs e)
        {
            if (!CheckCoordinates()) return;
            var latCoor = LoadCoordinates(_lat);
            var lonCoor = LoadCoordinates(_lon);

            // Now convert from Lat-Long to x,y coordinates that App.Map.ViewExtents can use to pan to the correct location.
            var xy = LatLonReproject(lonCoor, latCoor);

            // Get extent where center is desired X,Y coordinate.
            var width = _map.ViewExtents.Width;
            var height = _map.ViewExtents.Height;
            _map.ViewExtents.X = xy[0] - (width / 2);
            _map.ViewExtents.Y = xy[1] + (height / 2);
            var ex = _map.ViewExtents;

            // Set App.Map.ViewExtents to new extent that centers on desired LatLong.
            _map.ViewExtents = ex;

            DialogResult = DialogResult.OK;
            Close();
        }

        private bool CheckCoordinates()
        {
            var latCheck = ParseCoordinates(_lat, d1.Text);
            var lonCheck = ParseCoordinates(_lon, d2.Text);

            latStatus.Text = !latCheck ? "Invalid Latitude (Valid example: \"41.1939 N\")" : string.Empty;
            lonStatus.Text = !lonCheck ? "Invalid Longitude (Valid example: \"19.4908 E\")" : string.Empty;

            return latCheck && lonCheck;
        }

        private double[] LatLonReproject(double x, double y)
        {
            var xy = new[] { x, y };

            // Change y coordinate to be less than 90 degrees to prevent a bug.
            if (xy[1] >= 90) xy[1] = 89.9;
            if (xy[1] <= -90) xy[1] = -89.9;

            // Need to convert points to proper projection. Currently describe WGS84 points which may or may not be accurate.
            var wgs84String = "GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137,298.257223562997]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.0174532925199433]]";
            var mapProjEsriString = _map.Projection.ToEsriString();
            var isWgs84 = mapProjEsriString.Equals(wgs84String);

            // If the projection is not WGS84, then convert points to properly describe desired location.
            if (!isWgs84)
            {
                var z = new double[1];
                var wgs84Projection = ProjectionInfo.FromEsriString(wgs84String);
                var currentMapProjection = ProjectionInfo.FromEsriString(mapProjEsriString);
                Reproject.ReprojectPoints(xy, z, wgs84Projection, currentMapProjection, 0, 1);
            }

            // Return array with 1 x and 1 y value.
            return xy;
        }

        #endregion
    }
}