// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/19/2009 11:03:57 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Plugins.Measure
{
    /// <summary>
    /// MeasureDialog
    /// </summary>
    public class MeasureDialog : Form
    {
        private double _areaIntoSquareMeters;
        private int _areaUnitIndex;
        private double _distIntoMeters;
        private double _distance;
        private int _distanceUnitIndex;
        private MeasureMode _measureMode;
        private double _totalArea;
        private double _totalDistance;
        private ComboBox cmbUnits;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label lblMeasure;
        private Label lblPartialValue;
        private Label lblTotalUnits;
        private Label lblTotalValue;
        private ToolStrip toolStrip1;
        private ToolStripButton tsbArea;
        private ToolStripButton tsbDistance;
        private ToolTip ttHelp;

        #region Private Variables

        private double[] _areaUnitFactors = new[]
                                                {
                                                    1E-6, 0.0001, 1, .01, 3.86102159E-7, 0.000247105381, 10.7639104,
                                                    1.19599005
                                                };

        private string[] _areaUnitNames = new[]
                                              {
                                                  "Square Kilometers", "Hectares", "Square Meters", "Ares", "Square Miles",
                                                  "Acres", "Square Feet", "Square Yards" };

        private double[] _distanceUnitFactors = new[]
                                                    {
                                                        .001, 1, 10, 100, 1000,
                                                        0.000621371192, 0.000539956803, 1.0936133, 3.2808399, 39.3700787, 8.983152098E-6
                                                    };

        private string[] _distanceUnitNames = new[]
                                                  {
                                                      "Kilometers", "Meters", "Decimeters", "Centimeters", "Millimeters",
                                                      "Miles", "NauticalMiles", "Yards", "Feet", "Inches", "DecimalDegrees"
                                                  };

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        private Label label4;
        private ToolStripButton tsbClear;

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeasureDialog));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbDistance = new System.Windows.Forms.ToolStripButton();
            this.tsbArea = new System.Windows.Forms.ToolStripButton();
            this.tsbClear = new System.Windows.Forms.ToolStripButton();
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.lblPartialValue = new System.Windows.Forms.Label();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.cmbUnits = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMeasure = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTotalUnits = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            //
            // toolStrip1
            //
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbDistance,
            this.tsbArea,
            this.tsbClear});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            //
            // tsbDistance
            //
            this.tsbDistance.Checked = true;
            this.tsbDistance.CheckOnClick = true;
            this.tsbDistance.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbDistance.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDistance.Image = global::DotSpatial.Plugins.Measure.Properties.Resources.Line;
            resources.ApplyResources(this.tsbDistance, "tsbDistance");
            this.tsbDistance.Name = "tsbDistance";
            this.tsbDistance.Click += new System.EventHandler(this.DistanceButton_Click);
            //
            // tsbArea
            //
            this.tsbArea.CheckOnClick = true;
            this.tsbArea.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbArea.Image = global::DotSpatial.Plugins.Measure.Properties.Resources.Area;
            resources.ApplyResources(this.tsbArea, "tsbArea");
            this.tsbArea.Name = "tsbArea";
            this.tsbArea.Click += new System.EventHandler(this.AreaButton_Click);
            //
            // tsbClear
            //
            this.tsbClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsbClear, "tsbClear");
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Click += new System.EventHandler(this.tsbClear_Click);
            //
            // lblPartialValue
            //
            resources.ApplyResources(this.lblPartialValue, "lblPartialValue");
            this.lblPartialValue.Name = "lblPartialValue";
            this.ttHelp.SetToolTip(this.lblPartialValue, resources.GetString("lblPartialValue.ToolTip"));
            //
            // lblTotalValue
            //
            resources.ApplyResources(this.lblTotalValue, "lblTotalValue");
            this.lblTotalValue.Name = "lblTotalValue";
            this.ttHelp.SetToolTip(this.lblTotalValue, resources.GetString("lblTotalValue.ToolTip"));
            //
            // cmbUnits
            //
            this.cmbUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnits.FormattingEnabled = true;
            resources.ApplyResources(this.cmbUnits, "cmbUnits");
            this.cmbUnits.Name = "cmbUnits";
            this.ttHelp.SetToolTip(this.cmbUnits, resources.GetString("cmbUnits.ToolTip"));
            this.cmbUnits.SelectedIndexChanged += new System.EventHandler(this.cmbUnits_SelectedIndexChanged);
            //
            // label1
            //
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            //
            // lblMeasure
            //
            resources.ApplyResources(this.lblMeasure, "lblMeasure");
            this.lblMeasure.Name = "lblMeasure";
            //
            // label2
            //
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            //
            // label3
            //
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            //
            // lblTotalUnits
            //
            this.lblTotalUnits.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.lblTotalUnits, "lblTotalUnits");
            this.lblTotalUnits.Name = "lblTotalUnits";
            //
            // label4
            //
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            //
            // MeasureDialog
            //
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblTotalUnits);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbUnits);
            this.Controls.Add(this.lblTotalValue);
            this.Controls.Add(this.lblPartialValue);
            this.Controls.Add(this.lblMeasure);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MeasureDialog";
            this.ShowIcon = false;
            this.TopMost = true;
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of MeasureDialog
        /// </summary>
        public MeasureDialog()
        {
            InitializeComponent();
            _measureMode = MeasureMode.Distance;
            cmbUnits.Items.AddRange(_distanceUnitNames);
            cmbUnits.SelectedIndex = 1;
            _distanceUnitIndex = 1;
            _areaUnitIndex = 2;
            _distIntoMeters = 1;
            _areaIntoSquareMeters = 1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the distance in meters of just one segment
        /// </summary>
        public double Distance
        {
            get { return _distance; }
            set
            {
                _distance = value;
                if (_measureMode == MeasureMode.Distance)
                {
                    lblPartialValue.Text = (_distance * _distIntoMeters).ToString("#, ###");
                }
            }
        }

        /// <summary>
        /// The total distance across all segments in meters
        /// </summary>
        public double TotalDistance
        {
            get { return _totalDistance; }
            set
            {
                _totalDistance = value;
                if (_measureMode == MeasureMode.Distance)
                {
                    lblTotalValue.Text = (_totalDistance * _distIntoMeters).ToString("#, ###");
                }
            }
        }

        /// <summary>
        /// Gets or sets the total area in square meters
        /// </summary>
        public double TotalArea
        {
            get { return _totalArea; }
            set
            {
                _totalArea = value;
                lblTotalValue.Text = (_totalArea * _areaIntoSquareMeters).ToString("#, ###");
            }
        }

        #endregion

        /// <summary>
        /// Gets or sets whether to display the distances or areas.
        /// </summary>
        public MeasureMode MeasureMode
        {
            get { return _measureMode; }
            set
            {
                _measureMode = value;
            }
        }

        /// <summary>
        /// Occurs when the measuring mode has been changed.
        /// </summary>
        public event EventHandler MeasureModeChanged;

        /// <summary>
        /// Occurs when the clear button has been pressed.
        /// </summary>
        public event EventHandler MeasurementsCleared;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void AreaButton_Click(object sender, EventArgs e)
        {
            if (_measureMode != MeasureMode.Area)
            {
                MeasureMode = MeasureMode.Area;
                _distanceUnitIndex = cmbUnits.SelectedIndex;
                cmbUnits.SuspendLayout();
                cmbUnits.Items.Clear();
                cmbUnits.Items.AddRange(_areaUnitNames);
                cmbUnits.SelectedIndex = _areaUnitIndex;
                OnMeasureModeChanged();
                cmbUnits.ResumeLayout();
                Text = "Measure Area";
                lblMeasure.Text = "Area";
                tsbDistance.Checked = false;
            }
        }

        private void OnMeasureModeChanged()
        {
            if (MeasureModeChanged != null) MeasureModeChanged(this, new EventArgs());
        }

        private void DistanceButton_Click(object sender, EventArgs e)
        {
            if (_measureMode != MeasureMode.Distance)
            {
                MeasureMode = MeasureMode.Distance;
                _areaUnitIndex = cmbUnits.SelectedIndex;
                cmbUnits.SuspendLayout();
                cmbUnits.Items.Clear();
                cmbUnits.Items.AddRange(_distanceUnitNames);
                cmbUnits.SelectedIndex = _distanceUnitIndex;
                cmbUnits.ResumeLayout();
                OnMeasureModeChanged();
                Text = "Measure Distance";
                lblMeasure.Text = "Distance";
                tsbArea.Checked = false;
            }
        }

        private void cmbUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MeasureMode == MeasureMode.Distance)
            {
                _distIntoMeters = _distanceUnitFactors[cmbUnits.SelectedIndex];
            }
            else
            {
                _areaIntoSquareMeters = _areaUnitFactors[cmbUnits.SelectedIndex];
            }
            lblTotalUnits.Text = cmbUnits.Text;
            lblPartialValue.Text = String.Empty;
            lblTotalValue.Text = String.Empty;
        }

        private void tsbClear_Click(object sender, EventArgs e)
        {
            if (MeasurementsCleared != null) MeasurementsCleared(this, EventArgs.Empty);
        }
    }
}