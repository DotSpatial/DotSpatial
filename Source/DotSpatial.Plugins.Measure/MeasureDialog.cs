// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using DotSpatial.Plugins.Measure.Properties;

namespace DotSpatial.Plugins.Measure
{
    /// <summary>
    /// MeasureDialog
    /// </summary>
    public partial class MeasureDialog : Form
    {
        #region Fields

        private readonly double[] _areaUnitFactors = { 1E-6, 0.0001, 1, .01, 3.86102159E-7, 0.000247105381, 10.7639104, 1.19599005 };
        /*private readonly string[] _areaUnitNames = { "Square Kilometers", "Hectares", "Square Meters", "Ares", "Square Miles", "Acres", "Square Feet", "Square Yards" };*/
        private readonly string[] _areaUnitNames = { resources.GetString("cmbUnitsArea.Items"), resources.GetString("cmbUnitsArea.Items1"), resources.GetString("cmbUnitsArea.Items2"), resources.GetString("cmbUnitsArea.Items3"), resources.GetString("cmbUnitsArea.Items4"), resources.GetString("cmbUnitsArea.Items5"), resources.GetString("cmbUnitsArea.Items6"), resources.GetString("cmbUnitsArea.Items7") };
        private readonly double[] _distanceUnitFactors = { .001, 1, 10, 100, 1000, 0.000621371192, 0.000539956803, 1.0936133, 3.2808399, 39.3700787, 8.983152098E-6 };
        /*private readonly string[] _distanceUnitNames = { "Kilometers", "Meters", "Decimeters", "Centimeters", "Millimeters", "Miles", "NauticalMiles", "Yards", "Feet", "Inches", "DecimalDegrees" };*/
        private readonly string[] _distanceUnitNames = { resources.GetString("cmbUnitsDist.Items"), resources.GetString("cmbUnitsDist.Items1"), resources.GetString("cmbUnitsDist.Items2"), resources.GetString("cmbUnitsDist.Items3"), resources.GetString("cmbUnitsDist.Items4"), resources.GetString("cmbUnitsDist.Items5"), resources.GetString("cmbUnitsDist.Items6"), resources.GetString("cmbUnitsDist.Items7"), resources.GetString("cmbUnitsDist.Items8"), resources.GetString("cmbUnitsDist.Items9"), resources.GetString("cmbUnitsDist.Items10") };
        private double _areaIntoSquareMeters;
        private int _areaUnitIndex;
        private double _distance;
        private int _distanceUnitIndex;
        private double _distIntoMeters;
        private double _totalArea;
        private double _totalDistance;
        private CultureInfo _measureDialogCulture;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MeasureDialog"/> class.
        /// </summary>
        public MeasureDialog()
        {
            InitializeComponent();
            MeasureMode = MeasureMode.Distance;
            cmbUnits.Items.AddRange(_distanceUnitNames);
            /*cmbUnits.Items.AddRange(new object[]
            {
            resources.GetString("cmbUnitsDist.Items"),
            resources.GetString("cmbUnitsDist.Items1")
            });*/
            cmbUnits.SelectedIndex = 1;
            _distanceUnitIndex = 1;
            _areaUnitIndex = 2;
            _distIntoMeters = 1;
            _areaIntoSquareMeters = 1;

            MeasureDialogCulture = new CultureInfo(string.Empty);
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the clear button has been pressed.
        /// </summary>
        public event EventHandler MeasurementsCleared;

        /// <summary>
        /// Occurs when the measuring mode has been changed.
        /// </summary>
        public event EventHandler MeasureModeChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the distance in meters of just one segment.
        /// </summary>
        public double Distance
        {
            get
            {
                return _distance;
            }

            set
            {
                _distance = value;
                if (MeasureMode == MeasureMode.Distance)
                {
                    lblPartialValue.Text = (_distance * _distIntoMeters).ToString("#, ###");
                }
            }
        }

        /// <summary>
        /// Gets or sets whether to display the distances or areas.
        /// </summary>
        public MeasureMode MeasureMode { get; set; }

        /// <summary>
        /// Gets or sets the total area in square meters.
        /// </summary>
        public double TotalArea
        {
            get
            {
                return _totalArea;
            }

            set
            {
                _totalArea = value;
                lblTotalValue.Text = (_totalArea * _areaIntoSquareMeters).ToString("#, ###");
            }
        }

        /// <summary>
        /// Gets or sets the total distance across all segments in meters.
        /// </summary>
        public double TotalDistance
        {
            get
            {
                return _totalDistance;
            }

            set
            {
                _totalDistance = value;
                if (MeasureMode == MeasureMode.Distance)
                {
                    lblTotalValue.Text = (_totalDistance * _distIntoMeters).ToString("#, ###");
                }
            }
        }

        /// <summary>
        /// sets a value indicating the culture to use for resources.
        /// </summary>
        public CultureInfo MeasureDialogCulture
        {
            set
            {
                if (_measureDialogCulture == value) return;

                _measureDialogCulture = value;

                if (_measureDialogCulture == null) _measureDialogCulture = new CultureInfo(string.Empty);

                Thread.CurrentThread.CurrentCulture = _measureDialogCulture;
                Thread.CurrentThread.CurrentUICulture = _measureDialogCulture;

                UpdateResources();
            }
        }

        #endregion

        #region Methods

        private void AreaButtonClick(object sender, EventArgs e)
        {
            if (MeasureMode != MeasureMode.Area)
            {
                MeasureMode = MeasureMode.Area;
                _distanceUnitIndex = cmbUnits.SelectedIndex;
                cmbUnits.SuspendLayout();
                cmbUnits.Items.Clear();
                cmbUnits.Items.AddRange(_areaUnitNames);
                cmbUnits.SelectedIndex = _areaUnitIndex;
                OnMeasureModeChanged();
                cmbUnits.ResumeLayout();
                Text = Resources.MeasureArea;
                lblMeasure.Text = Resources.StrArea;
                tsbDistance.Checked = false;
            }
        }

        private void CmbUnitsSelectedIndexChanged(object sender, EventArgs e)
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
            lblPartialValue.Text = string.Empty;
            lblTotalValue.Text = string.Empty;
        }

        private void DistanceButtonClick(object sender, EventArgs e)
        {
            if (MeasureMode != MeasureMode.Distance)
            {
                MeasureMode = MeasureMode.Distance;
                _areaUnitIndex = cmbUnits.SelectedIndex;
                cmbUnits.SuspendLayout();
                cmbUnits.Items.Clear();
                cmbUnits.Items.AddRange(_distanceUnitNames);
                cmbUnits.SelectedIndex = _distanceUnitIndex;
                cmbUnits.ResumeLayout();
                OnMeasureModeChanged();
                Text = Resources.MeasureDistance;
                lblMeasure.Text = Resources.Distance;
                tsbArea.Checked = false;
                resources.ApplyResources(cmbUnits, "cmbUnits");
            }
        }

        private void OnMeasureModeChanged()
        {
            MeasureModeChanged?.Invoke(this, EventArgs.Empty);
        }

        private void TsbClearClick(object sender, EventArgs e)
        {
            MeasurementsCleared?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}