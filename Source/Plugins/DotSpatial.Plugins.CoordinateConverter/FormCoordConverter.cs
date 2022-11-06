// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Projections;

namespace DotSpatial.Plugins.CoordinateConverter
{
    public partial class FormCoordConverter : Form
    {
        AppManager appManager;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FormCoordConverter"/> class.
        /// </summary>
        public FormCoordConverter()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FormCoordConverter"/> class.
        /// </summary>
        public FormCoordConverter(AppManager app)
        {
            InitializeComponent();
            appManager = app;
        }

        #endregion

        #region Properties

        public double Xsource { get; set; }
        public double Ysource { get; set; }
        public double Zsource { get; set; }
        public ProjectionInfo ProjSource { get; set; } = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneAlaska1FIPS5001;
        public double Xtarget { get; set; }
        public double Ytarget { get; set; }
        public double Ztarget { get; set; }
        public ProjectionInfo ProjTarget { get; set; } = KnownCoordinateSystems.Geographic.NorthAmerica.NorthAmericanDatum1927;

        #endregion

        #region Methods

        private void txtSourceX_TextChanged(object sender, EventArgs e)
        {
            ClearTarget();
        }

        private void txtSourceY_TextChanged(object sender, EventArgs e)
        {
            ClearTarget();
        }

        private void txtSourceZ_TextChanged(object sender, EventArgs e)
        {
            ClearTarget();
        }

        private void ClearTarget()
        {
            Xtarget = double.NaN;
            Ytarget = double.NaN;
            Ztarget = double.NaN;

            txtTargetX.Text = String.Empty;
            txtTargetY.Text = String.Empty;
            txtTargetZ.Text = String.Empty;
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                //check and assign vars
                bool isok = Double.TryParse(txtSourceX.Text, out double resX);
                if (isok)
                    Xsource = resX;
                else
                    MessageBox.Show("Invalid x input", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);

                isok = Double.TryParse(txtSourceY.Text, out double resY);
                if (isok)
                    Ysource = resY;
                else
                    MessageBox.Show("Invalid y input", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);

                isok = Double.TryParse(txtSourceZ.Text, out double resZ);
                if (isok)
                    Zsource = resZ;
                else
                    MessageBox.Show("Invalid z input", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //create array of the source coordinates
                double[] xy = { Xsource, Ysource };
                double[] z = { Zsource };

                //reproject the coordinates from the source projection to target projection
                Reproject.ReprojectPoints(xy, z, ProjSource, ProjTarget, 0, 1);

                //assign reprojected coordinates from the result to the target
                Xtarget = xy[0];
                Ytarget = xy[1];
                Ztarget = z[0];

                txtTargetX.Text = Xtarget.ToString();
                txtTargetY.Text = Ytarget.ToString();
                txtTargetZ.Text = Ztarget.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormCoordConverter_Load(object sender, EventArgs e)
        {
            txtSourceX.Text = "0";
            txtSourceY.Text = "0";
            txtSourceZ.Text = "0";
        }

        #endregion
    }
}
