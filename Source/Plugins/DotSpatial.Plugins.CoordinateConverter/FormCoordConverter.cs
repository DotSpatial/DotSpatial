// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.Text;
using System.Windows.Forms;
using DotSpatial.Projections;
using DotSpatial.Projections.Forms;

namespace DotSpatial.Plugins.CoordinateConverter
{
    /// <summary>
    /// A form to convert a coordinate from one coordinate system to another.
    /// </summary>
    public partial class FormCoordConverter : Form
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FormCoordConverter"/> class.
        /// </summary>
        public FormCoordConverter()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        private double Xsource { get; set; }
        private double Ysource { get; set; }
        private double Zsource { get; set; }
        private ProjectionInfo ProjSource { get; set; } = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneAlaska1FIPS5001;
        private double Xtarget { get; set; }
        private double Ytarget { get; set; }
        private double Ztarget { get; set; }
        private ProjectionInfo ProjTarget { get; set; } = KnownCoordinateSystems.Geographic.NorthAmerica.NorthAmericanDatum1927;

        #endregion

        #region Methods

        private void TxtSourceX_TextChanged(object sender, EventArgs e)
        {
            ClearTarget();
        }

        private void TxtSourceY_TextChanged(object sender, EventArgs e)
        {
            ClearTarget();
        }

        private void TxtSourceZ_TextChanged(object sender, EventArgs e)
        {
            ClearTarget();
        }

        private void LblSourceProj_TextChanged(object sender, EventArgs e)
        {
            ClearTarget();
        }

        private void LblTargetProj_TextChanged(object sender, EventArgs e)
        {
            ClearTarget();
        }

        private void ClearTarget()
        {
            Xtarget = double.NaN;
            Ytarget = double.NaN;
            Ztarget = double.NaN;

            txtTargetX.Text = string.Empty;
            txtTargetY.Text = string.Empty;
            txtTargetZ.Text = string.Empty;
        }

        private void BtnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                //check and assign vars
                if (double.TryParse(txtSourceX.Text, out double resX))
                {
                    Xsource = resX;
                }
                else
                {
                    MessageBox.Show("Invalid x input", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (double.TryParse(txtSourceY.Text, out double resY))
                {
                    Ysource = resY;
                }
                else
                {
                    MessageBox.Show("Invalid y input", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (double.TryParse(txtSourceZ.Text, out double resZ))
                {
                    Zsource = resZ;
                }
                else
                {
                    MessageBox.Show("Invalid z input", "Invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

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

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormCoordConverter_Load(object sender, EventArgs e)
        {
            lblSourceProj.Text = ProjSource.Name;
            txtSourceX.Text = "0";
            txtSourceY.Text = "0";
            txtSourceZ.Text = "0";

            ToolTip tp0 = new()
            {
                IsBalloon = true,
                Active = true
            };
            tp0.SetToolTip(btnChangeSourceProj, "Change the source projection");

            lblTargetProj.Text = ProjTarget.Name;

            ToolTip tp1 = new()
            {
                IsBalloon = true,
                Active = true
            };
            tp1.SetToolTip(btnChangeTargetProj, "Change the target projection");
        }

        #endregion

        private void BtnChangeSourceProj_Click(object sender, EventArgs e)
        {
            using ProjectionSelectDialog pf = new();
            pf.SelectedCoordinateSystem = ProjSource;
            pf.ChangesApplied += PfSourceOnChangesApplied;
            pf.ShowDialog(this);
            pf.ChangesApplied -= PfSourceOnChangesApplied;
        }

        private void PfSourceOnChangesApplied(object sender, EventArgs eventArgs)
        {
            ProjectionSelectDialog pf = (ProjectionSelectDialog)sender;
            ProjSource = pf.SelectedCoordinateSystem;
            lblSourceProj.Text = ProjSource.Name;
        }

        private void BtnChangeTargetProj_Click(object sender, EventArgs e)
        {
            using ProjectionSelectDialog pf = new();
            pf.SelectedCoordinateSystem = ProjTarget;
            pf.ChangesApplied += PfTargetOnChangesApplied;
            pf.ShowDialog(this);
            pf.ChangesApplied -= PfTargetOnChangesApplied;
        }

        private void PfTargetOnChangesApplied(object sender, EventArgs eventArgs)
        {
            ProjectionSelectDialog pf = (ProjectionSelectDialog)sender;
            ProjTarget = pf.SelectedCoordinateSystem;
            lblTargetProj.Text = ProjTarget.Name;
        }

        private void BtnCopy_Click(object sender, EventArgs e)
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
