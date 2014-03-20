using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DotSpatial.Plugins.MenuBar
{
    public partial class ZoomToCoordinatesDialog : Form
    {
        private const String regExpression = "(-?\\d{1,3})[\\.\\,°]{0,1}\\s*(\\d{0,2})[\\.\\,\']{0,1}\\s*(\\d*)[\\.\\,°]{0,1}\\s*([NSnsEeWw]?)";

        private readonly double[] lat;
        private readonly double[] lon;

        public double latCoor { get; set; }
        public double lonCoor { get; set; }

        public ZoomToCoordinatesDialog()
        {
            InitializeComponent();
            lonStatus.Text = "";
            latStatus.Text = "";
            lat = new double[3];
            lon = new double[3];
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (checkCoordinates())
            {
                latCoor = loadCoordinates(lat);
                lonCoor = loadCoordinates(lon);
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private bool checkCoordinates()
        {
            var latCheck = parseCoordinates(lat, d1.Text);
            var lonCheck = parseCoordinates(lon, d2.Text);

            if (!latCheck) { latStatus.Text = "Invalid Latitude (Valid example: \"41.1939 N\")"; }
            else { latStatus.Text = ""; }
            if (!lonCheck) { lonStatus.Text = "Invalid Longitude (Valid example: \"19.4908 E\")"; }
            else { latStatus.Text = "";}

            return latCheck && lonCheck;
        }

        // Parse Coordinates will understand lat-lon coordinates in a variety of formats and separate them into Degrees, Minutes, and Seconds.
        // We could just accept a simple decimal value for the coordinates, but since users might be copying and pasting from a variety of sources
        // it makes it user friendly to be able to accept a number of different formats.
        private bool parseCoordinates(IList<double> values, String text)
        {
            var match = Regex.Match(text, regExpression);
            var groups = match.Groups;
            try
            {  
                values[0] = Double.Parse(groups[1].ToString());
                if (groups[2].Length > 0)
                {
                    values[1] = Double.Parse(groups[2].ToString());
                    if (groups[2].Length == 1) values[1] *= 10;
                }
                if (groups[3].Length > 0)
                {
                    values[2] = Double.Parse(groups[3].ToString());
                    if (groups[3].Length == 1) values[2] *= 10;
                }
            }
            catch
            {
                return false;
            }

            if ((groups[4].ToString().Equals("S", StringComparison.OrdinalIgnoreCase)
                || groups[4].ToString().Equals("W", StringComparison.OrdinalIgnoreCase))
                && values[0] > 0)
            {
                values[0] *= -1;
            }

            return true;
        }

        // Take Degrees-Minutes-Seconds from ParseCoordinates and turn them into doubles.
        private static double loadCoordinates(IList<double> values)
        {
            //Convert Degrees, Minutes, Seconds to x, y coordinates for both lat and long.
            var coor = values[2] / 100;
            coor += values[1];
            coor = coor / 100;
            coor += Math.Abs(values[0]);

            //Change signs to get to the right quadrant.
            if (values[0] < 0) { coor *= -1; }

            return coor;
        }
    }
}