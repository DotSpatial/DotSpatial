// ********************************************************************************************************
// Product Name: DotSpatial.Plugins.ShapeEditor.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/11/2009 9:24:49 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using DotSpatial.Symbology.Forms;
using GeoAPI.Geometries;

namespace DotSpatial.Plugins.ShapeEditor
{
    /// <summary>
    /// A dialog that displays the coordinates while drawing shapes.
    /// </summary>
    public class CoordinateDialog : Form
    {
        private Button _btnClose;
        private Button _btnOk;
        private DoubleBox _dbxM;
        private DoubleBox _dbxX;
        private DoubleBox _dbxY;
        private DoubleBox _dbxZ;
        private bool _showM;
        private bool _showZ;
        private ToolTip _ttHelp;

        #region Private Variables

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CoordinateDialog));
            this._btnOk = new System.Windows.Forms.Button();
            this._btnClose = new System.Windows.Forms.Button();
            this._ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this._dbxM = new DotSpatial.Symbology.Forms.DoubleBox();
            this._dbxZ = new DotSpatial.Symbology.Forms.DoubleBox();
            this._dbxY = new DotSpatial.Symbology.Forms.DoubleBox();
            this._dbxX = new DotSpatial.Symbology.Forms.DoubleBox();
            this.SuspendLayout();
            // 
            // _btnOk
            // 
            resources.ApplyResources(this._btnOk, "_btnOk");
            this._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOk.Name = "_btnOk";
            this._ttHelp.SetToolTip(this._btnOk, resources.GetString("_btnOk.ToolTip"));
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // _btnClose
            // 
            resources.ApplyResources(this._btnClose, "_btnClose");
            this._btnClose.Name = "_btnClose";
            this._ttHelp.SetToolTip(this._btnClose, resources.GetString("_btnClose.ToolTip"));
            this._btnClose.UseVisualStyleBackColor = true;
            this._btnClose.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // _dbxM
            // 
            resources.ApplyResources(this._dbxM, "_dbxM");
            this._dbxM.BackColorInvalid = System.Drawing.Color.Salmon;
            this._dbxM.BackColorRegular = System.Drawing.Color.Empty;
            this._dbxM.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this._dbxM.IsValid = true;
            this._dbxM.Name = "_dbxM";
            this._dbxM.NumberFormat = null;
            this._dbxM.RegularHelp = "Enter a double precision floating point value.";
            this._dbxM.Value = 0D;
            this._dbxM.ValidChanged += new System.EventHandler(this.Coordinate_ValidChanged);
            // 
            // _dbxZ
            // 
            resources.ApplyResources(this._dbxZ, "_dbxZ");
            this._dbxZ.BackColorInvalid = System.Drawing.Color.Salmon;
            this._dbxZ.BackColorRegular = System.Drawing.Color.Empty;
            this._dbxZ.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this._dbxZ.IsValid = true;
            this._dbxZ.Name = "_dbxZ";
            this._dbxZ.NumberFormat = null;
            this._dbxZ.RegularHelp = "Enter a double precision floating point value.";
            this._dbxZ.Value = 0D;
            // 
            // _dbxY
            // 
            resources.ApplyResources(this._dbxY, "_dbxY");
            this._dbxY.BackColorInvalid = System.Drawing.Color.Salmon;
            this._dbxY.BackColorRegular = System.Drawing.Color.Empty;
            this._dbxY.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this._dbxY.IsValid = true;
            this._dbxY.Name = "_dbxY";
            this._dbxY.NumberFormat = null;
            this._dbxY.RegularHelp = "Enter a double precision floating point value.";
            this._dbxY.Value = 0D;
            this._dbxY.ValidChanged += new System.EventHandler(this.Coordinate_ValidChanged);
            // 
            // _dbxX
            // 
            resources.ApplyResources(this._dbxX, "_dbxX");
            this._dbxX.BackColorInvalid = System.Drawing.Color.Salmon;
            this._dbxX.BackColorRegular = System.Drawing.Color.Empty;
            this._dbxX.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this._dbxX.IsValid = true;
            this._dbxX.Name = "_dbxX";
            this._dbxX.NumberFormat = null;
            this._dbxX.RegularHelp = "Enter a double precision floating point value.";
            this._dbxX.Value = 0D;
            this._dbxX.ValidChanged += new System.EventHandler(this.Coordinate_ValidChanged);
            // 
            // CoordinateDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._dbxM);
            this.Controls.Add(this._dbxZ);
            this.Controls.Add(this._dbxY);
            this.Controls.Add(this._dbxX);
            this.Controls.Add(this._btnClose);
            this.Controls.Add(this._btnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CoordinateDialog";
            this.ShowIcon = false;
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the CoordinateDialog class.
        /// </summary>
        public CoordinateDialog()
        {
            InitializeComponent();
            _showM = true;
            _showZ = true;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets a coordinate based on the current values.
        /// </summary>
        public Coordinate Coordinate
        {
            get
            {
                Coordinate c = _showZ ? new Coordinate(_dbxX.Value, _dbxY.Value, _dbxZ.Value) : new Coordinate(_dbxX.Value, _dbxY.Value);
                if (_showM)
                {
                    c.M = _dbxM.Value;
                }

                return c;
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

        #endregion

        #region Events

        #endregion

        #region Event Handlers

        private void Coordinate_ValidChanged(object sender, EventArgs e)
        {
            UpdateOk();
        }

        #endregion

        #region Protected Methods

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

        #endregion

        #region Private Functions

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

        #endregion

        /// <summary>
        /// Gets or sets the X value.
        /// </summary>
        public double X
        {
            get { return _dbxX.Value; }
            set { _dbxX.Text = value.ToString(CultureInfo.CurrentCulture); }
        }

        /// <summary>
        /// Gets or sets the Y value.
        /// </summary>
        public double Y
        {
            get { return _dbxY.Value; }
            set { _dbxY.Text = value.ToString(CultureInfo.CurrentCulture); }
        }

        /// <summary>
        /// Gets or sets the Z value.
        /// </summary>
        public double Z
        {
            get { return _dbxZ.Value; }
            set { _dbxZ.Text = value.ToString(CultureInfo.CurrentCulture); }
        }

        /// <summary>
        /// Gets or sets the M vlaue.
        /// </summary>
        public double M
        {
            get { return _dbxM.Value; }
            set { _dbxM.Text = value.ToString(CultureInfo.CurrentCulture); }
        }

        /// <summary>
        /// Occurs when the ok button is clicked.
        /// </summary>
        public event EventHandler CoordinateAdded;

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (CoordinateAdded != null) { CoordinateAdded(this, EventArgs.Empty); }
            Hide();
        }

        private void UpdateOk()
        {
            bool isValid = true;
            if (_dbxX.IsValid == false) { isValid = false; }
            if (_dbxY.IsValid == false) { isValid = false; }
            if (_showZ)
            {
                if (_dbxZ.IsValid == false) { isValid = false; }
            }
            if (_showM)
            {
                if (_dbxM.IsValid == false) { isValid = false; }
            }
            _btnOk.Enabled = isValid;
        }
    }
}