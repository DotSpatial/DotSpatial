// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using NetTopologySuite.Geometries;

namespace DotSpatial.Plugins.ShapeEditor
{
    /// <summary>
    /// A dialog that displays the coordinates while drawing shapes.
    /// </summary>
    public partial class CoordinateDialog : Form
    {
        #region Fields
        private bool _showM;
        private bool _showZ;
        private ToolTip _ttHelp;
        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CoordinateDialog"/> class.
        /// </summary>
        public CoordinateDialog()
        {
            InitializeComponent();
            _showM = true;
            _showZ = true;
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the ok button is clicked.
        /// </summary>
        public event EventHandler CoordinateAdded;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a coordinate based on the current values.
        /// </summary>
        public Coordinate Coordinate
        {
            get
            {
                Coordinate c = _showZ ? new CoordinateZ(_dbxX.Value, _dbxY.Value, _dbxZ.Value) : new Coordinate(_dbxX.Value, _dbxY.Value);
                if (_showM)
                {
                    c.M = _dbxM.Value;
                }

                return c;
            }
        }

        /// <summary>
        /// Gets or sets the M vlaue.
        /// </summary>
        public double M
        {
            get
            {
                return _dbxM.Value;
            }

            set
            {
                _dbxM.Text = value.ToString(CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not to show M values.
        /// </summary>
        public bool ShowMValues
        {
            get
            {
                return _showM;
            }

            set
            {
                if (_showM != value)
                {
                    if (value == false)
                    {
                        _dbxM.Visible = false;
                        Height -= 20;
                    }
                    else
                    {
                        _dbxM.Visible = true;
                        Height += 20;
                    }
                }

                _showM = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not to show Z values.
        /// </summary>
        public bool ShowZValues
        {
            get
            {
                return _showZ;
            }

            set
            {
                if (_showZ != value)
                {
                    if (value == false)
                    {
                        _dbxZ.Visible = false;
                        _dbxM.Top -= 20;
                        Height -= 20;
                    }
                    else
                    {
                        _dbxZ.Visible = true;
                        _dbxM.Top += 20;
                        Height += 20;
                    }
                }

                _showZ = value;
            }
        }

        /// <summary>
        /// Gets or sets the X value.
        /// </summary>
        public double X
        {
            get
            {
                return _dbxX.Value;
            }

            set
            {
                _dbxX.Text = value.ToString(CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Gets or sets the Y value.
        /// </summary>
        public double Y
        {
            get
            {
                return _dbxY.Value;
            }

            set
            {
                _dbxY.Text = value.ToString(CultureInfo.CurrentCulture);
            }
        }

        /// <summary>
        /// Gets or sets the Z value.
        /// </summary>
        public double Z
        {
            get
            {
                return _dbxZ.Value;
            }

            set
            {
                _dbxZ.Text = value.ToString(CultureInfo.CurrentCulture);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prevents disposing this form when the user closes it.
        /// </summary>
        /// <param name="e">The CancelEventArgs parameter allows canceling the complete closure of this dialog.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
            base.OnClosing(e);
        }

        private void CloseButtonClick(object sender, EventArgs e)
        {
            Hide();
        }

        private void CoordinateValidChanged(object sender, EventArgs e)
        {
            UpdateOk();
        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            CoordinateAdded?.Invoke(this, EventArgs.Empty);

            Hide();
        }

        private void UpdateOk()
        {
            bool isValid = _dbxX.IsValid && _dbxY.IsValid;

            if (_showZ && !_dbxZ.IsValid)
            {
                isValid = false;
            }

            if (_showM && !_dbxM.IsValid)
            {
                isValid = false;
            }

            _btnOk.Enabled = isValid;
        }

        #endregion
    }
}