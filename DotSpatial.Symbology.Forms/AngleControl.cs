// ********************************************************************************************************
// Product Name: DotSpatial.Projections.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is MapWindow.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/19/2009 1:06:14 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A user control for specifying angles
    /// </summary>
    [DefaultEvent("AngleChanged")]
    [ToolboxBitmap(typeof(AngleControl), "Angles.AngleControl.ico")]
    public class AngleControl : UserControl
    {
        #region Events

        /// <summary>
        /// Occurs when the angle changes either by the picker or the textbox.
        /// </summary>
        public event EventHandler AngleChanged;

        /// <summary>
        /// Occurs when the mouse up event occurs on the picker
        /// </summary>
        public event EventHandler AngleChosen;

        #endregion

        #region Private Variables

        private AnglePicker _anglePicker1;
        private Label _lblText;
        private NumericUpDown _nudAngle;

        #endregion

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(AngleControl));
            this._lblText = new Label();
            this._nudAngle = new NumericUpDown();
            this._anglePicker1 = new AnglePicker();
            ((ISupportInitialize)(this._nudAngle)).BeginInit();
            this.SuspendLayout();
            //
            // lblText
            //
            this._lblText.AccessibleDescription = null;
            this._lblText.AccessibleName = null;
            resources.ApplyResources(this._lblText, "_lblText");
            this._lblText.Font = null;
            this._lblText.Name = "_lblText";
            //
            // nudAngle
            //
            this._nudAngle.AccessibleDescription = null;
            this._nudAngle.AccessibleName = null;
            resources.ApplyResources(this._nudAngle, "_nudAngle");
            this._nudAngle.Maximum = new decimal(new int[] {
                                                               360,
                                                               0,
                                                               0,
                                                               0});
            this._nudAngle.Minimum = new decimal(new int[] {
                                                               -360,
                                                               0,
                                                               0,
                                                               -2147483648});
            this._nudAngle.Name = "_nudAngle";
            this._nudAngle.ValueChanged += new EventHandler(this.nudAngle_ValueChanged);
            //
            // anglePicker1
            //
            this._anglePicker1.AccessibleDescription = null;
            this._anglePicker1.AccessibleName = null;
            resources.ApplyResources(this._anglePicker1, "_anglePicker1");
            this._anglePicker1.Angle = 0;
            this._anglePicker1.BackgroundImage = null;
            //this._anglePicker1.BorderStyle = BorderStyle.None;
            this._anglePicker1.CircleBorderColor = Color.LightGray;
            //this._anglePicker1.CircleBorderStyle = BorderStyle.Fixed3D;
            this._anglePicker1.CircleFillColor = Color.LightGray;
            this._anglePicker1.Clockwise = false;
            this._anglePicker1.Font = null;
            this._anglePicker1.KnobColor = Color.Green;
            this._anglePicker1.KnobVisible = true;
            this._anglePicker1.Name = "_anglePicker1";
            this._anglePicker1.PieFillColor = Color.SteelBlue;
            this._anglePicker1.Snap = 3;
            this._anglePicker1.StartAngle = 0;
            this._anglePicker1.TextAlignment = ContentAlignment.BottomCenter;
            this._anglePicker1.AngleChanged += new EventHandler(this.anglePicker1_AngleChanged);
            //
            // AngleControl
            //
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");

            this.BackColor = SystemColors.Control;
            this.BackgroundImage = null;
            this.Controls.Add(this._nudAngle);
            this.Controls.Add(this._anglePicker1);
            this.Controls.Add(this._lblText);
            this.Font = null;
            this.Name = "AngleControl";
            ((ISupportInitialize)(this._nudAngle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #region Constructors

        /// <summary>
        /// A user control designed to allow an angle to be chosen
        /// </summary>
        public AngleControl()
        {
            InitializeComponent();
            Configure();
        }

        private void Configure()
        {
            _anglePicker1.AngleChosen += anglePicker1_AngleChosen;
        }

        private void anglePicker1_AngleChosen(object sender, EventArgs e)
        {
            OnAngleChosen();
        }

        #endregion

        /// <summary>
        /// Gets or sets the integer angle in degrees.
        /// </summary>
        [Description("Gets or sets the integer angle in degrees")]
        public int Angle
        {
            get { return _anglePicker1.Angle; }
            set
            {
                _anglePicker1.Angle = value;
                _nudAngle.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean indicating if the values should increase in the
        /// clockwise direction instead of the counter clockwise direction
        /// </summary>
        [Description("Gets or sets a boolean indicating if the values should increase in the clockwise direction instead of the counter clockwise direction")]
        public bool Clockwise
        {
            get { return _anglePicker1.Clockwise; }
            set
            {
                _anglePicker1.Clockwise = value;
            }
        }

        /// <summary>
        /// Gets or sets the base Knob Color
        /// </summary>
        [Description("Gets or sets the color to use as the base color for drawing the knob.")]
        public Color KnobColor
        {
            get { return _anglePicker1.KnobColor; }
            set { _anglePicker1.KnobColor = value; }
        }

        /// <summary>
        /// Gets or sets the start angle in degrees measured counter clockwise from the X axis.
        /// For instance, for an azimuth angle that starts at the top, this should be set to 90.
        /// </summary>
        [Description("Gets or sets the start angle in degrees measured counter clockwise from the X axis.  For instance, for an azimuth angle that starts at the top, this should be set to 90.")]
        public int StartAngle
        {
            get { return _anglePicker1.StartAngle; }
            set { _anglePicker1.StartAngle = value; }
        }

        /// <summary>
        /// Gets or sets the string text for this control.
        /// </summary>
        [Localizable(true), Description("Gets or sets the string text for this control.")]
        public string Caption
        {
            get { return _lblText.Text; }
            set { _lblText.Text = value; }
        }

        #region Protected Methods

        /// <summary>
        /// Fires the angle changed event
        /// </summary>
        protected virtual void OnAngleChanged()
        {
            if (AngleChanged != null) AngleChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// fires an event once the mouse has been released.
        /// </summary>
        protected virtual void OnAngleChosen()
        {
            if (AngleChosen != null) AngleChosen(this, EventArgs.Empty);
        }

        #endregion

        private void nudAngle_ValueChanged(object sender, EventArgs e)
        {
            if (_anglePicker1.Angle != _nudAngle.Value)
            {
                _anglePicker1.Angle = (int)_nudAngle.Value;
                OnAngleChanged();
            }
        }

        private void anglePicker1_AngleChanged(object sender, EventArgs e)
        {
            if (_nudAngle.Value != _anglePicker1.Angle)
            {
                _nudAngle.Value = _anglePicker1.Angle;
                OnAngleChanged();
            }
        }
    }
}