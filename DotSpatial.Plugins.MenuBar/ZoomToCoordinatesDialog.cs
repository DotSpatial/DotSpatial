using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DotSpatial.Plugins.MenuBar
{
    public partial class ZoomToCoordinatesDialog : Form
    {
        String regExpression = "(-?\\d*)[\\.\\,°]\\s*(\\d\\d)[\\.\\,\']*\\s*(\\d*)\\s*([NSnsEeWw]?)";

        public double[] lat{ get; set; }

        public double[] lon { get; set; }

        public ZoomToCoordinatesDialog()
        {
            InitializeComponent();
            lonStatus.Text = "";
            latStatus.Text = "";
            lat = new double[3];
            lon = new double[3];
        }

        private void ZoomToCoordinatesDialog_Load(object sender, EventArgs e)
        {

        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (checkCoordinates())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool checkCoordinates()
        {
            bool latCheck = parseCoordinates(lat, d1.Text.ToString());
            bool lonCheck = parseCoordinates(lon, d2.Text.ToString());

            if (!latCheck) { latStatus.Text = "Invalid Latitude (Valid example: \"41.1939 N\")"; }
            if (!lonCheck) { lonStatus.Text = "Invalid Longitude (Valid example: \"19.4908 E\")"; }

            return latCheck && lonCheck;
        }

        private bool parseCoordinates(double[] values, String text)
        {
            Match match;
            GroupCollection groups;

            match = Regex.Match(text, regExpression);
            groups = match.Groups;
            try
            {
                values[0] = Double.Parse(groups[1].ToString());
                values[1] = Double.Parse(groups[2].ToString());
                values[2] = Double.Parse(groups[3].ToString());
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
    }
}
