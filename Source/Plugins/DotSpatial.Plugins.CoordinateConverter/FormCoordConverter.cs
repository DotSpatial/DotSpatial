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
using DotSpatial.Projections.Forms;

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

        private void lblSourceProj_TextChanged(object sender, EventArgs e)
        {
            ClearTarget();
        }

        private void lblTargetProj_TextChanged(object sender, EventArgs e)
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
            lblSourceProj.Text = ProjSource.Name;
            txtSourceX.Text = "0";
            txtSourceY.Text = "0";
            txtSourceZ.Text = "0";

            ToolTip tp0 = new();
            tp0.IsBalloon = true;
            tp0.Active = true;
            tp0.SetToolTip(btnChangeSourceProj, "Change the source projection");

            lblTargetProj.Text = ProjTarget.Name;

            ToolTip tp1 = new();
            tp1.IsBalloon = true;
            tp1.Active = true;
            tp1.SetToolTip(btnChangeTargetProj, "Change the target projection");
        }

        #endregion

        private void btnChangeSourceProj_Click(object sender, EventArgs e)
        {
            using var pf = new ProjectionSelectDialog();
            pf.SelectedCoordinateSystem = ProjSource;
            pf.ChangesApplied += PfSourceOnChangesApplied;
            pf.ShowDialog(this);
            pf.ChangesApplied -= PfSourceOnChangesApplied;
        }

        private void PfSourceOnChangesApplied(object sender, EventArgs eventArgs)
        {
            var pf = (ProjectionSelectDialog)sender;
            ProjSource = pf.SelectedCoordinateSystem;
            lblSourceProj.Text = ProjSource.Name;
        }

        private void btnChangeTargetProj_Click(object sender, EventArgs e)
        {
            using var pf = new ProjectionSelectDialog();
            pf.SelectedCoordinateSystem = ProjTarget;
            pf.ChangesApplied += PfTargetOnChangesApplied;
            pf.ShowDialog(this);
            pf.ChangesApplied -= PfTargetOnChangesApplied;
        }

        private void PfTargetOnChangesApplied(object sender, EventArgs eventArgs)
        {
            var pf = (ProjectionSelectDialog)sender;
            ProjTarget = pf.SelectedCoordinateSystem;
            lblTargetProj.Text = ProjTarget.Name;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            //gather all the data
            StringBuilder sb = new();
            sb.AppendFormat("Source: {0}\r\nProj4:{1}", ProjSource.Name, ProjSource.ToProj4String());
            sb.AppendLine();
            sb.AppendFormat("X={0} Y={1} Z={2}", txtSourceX.Text, txtSourceY.Text, txtSourceZ.Text);
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendFormat("Target: {0}\r\nProj4:{1}", ProjTarget.Name, ProjTarget.ToProj4String());
            sb.AppendLine();
            sb.AppendFormat("X={0} Y={1} Z={2}", txtTargetX.Text, txtTargetY.Text, txtTargetZ.Text);

            //add it to the clipboard
            Clipboard.SetText(sb.ToString(), TextDataFormat.Text);

            //confirmation message
            MessageBox.Show("Data has been copied to the clipboard.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
