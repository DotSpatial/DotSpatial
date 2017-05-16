// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A user control for specifying angles.
    /// </summary>
    [DefaultEvent("AngleChanged")]
    [ToolboxBitmap(typeof(AngleControl), "Angles.AngleControl.ico")]
    public partial class AngleControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AngleControl"/> class.
        /// </summary>
        public AngleControl()
        {
            InitializeComponent();
            Configure();
        }

        #endregion

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

        #region Properties

        /// <summary>
        /// Gets or sets the integer angle in degrees.
        /// </summary>
        [Description("Gets or sets the integer angle in degrees")]
        public int Angle
        {
            get
            {
                return _anglePicker1.Angle;
            }

            set
            {
                _anglePicker1.Angle = value;
                _nudAngle.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the string text for this control.
        /// </summary>
        [Localizable(true)]
        [Description("Gets or sets the string text for this control.")]
        public string Caption
        {
            get
            {
                return _lblText.Text;
            }

            set
            {
                _lblText.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the values should increase in the
        /// clockwise direction instead of the counter clockwise direction.
        /// </summary>
        [Description("Gets or sets a boolean indicating if the values should increase in the clockwise direction instead of the counter clockwise direction")]
        public bool Clockwise
        {
            get
            {
                return _anglePicker1.Clockwise;
            }

            set
            {
                _anglePicker1.Clockwise = value;
            }
        }

        /// <summary>
        /// Gets or sets the color to use as the base color for drawing the knob.
        /// </summary>
        [Description("Gets or sets the color to use as the base color for drawing the knob.")]
        public Color KnobColor
        {
            get
            {
                return _anglePicker1.KnobColor;
            }

            set
            {
                _anglePicker1.KnobColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the start angle in degrees measured counter clockwise from the X axis.
        /// For instance, for an azimuth angle that starts at the top, this should be set to 90.
        /// </summary>
        [Description("Gets or sets the start angle in degrees measured counter clockwise from the X axis. For instance, for an azimuth angle that starts at the top, this should be set to 90.")]
        public int StartAngle
        {
            get
            {
                return _anglePicker1.StartAngle;
            }

            set
            {
                _anglePicker1.StartAngle = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fires the angle changed event.
        /// </summary>
        protected virtual void OnAngleChanged()
        {
            AngleChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// fires an event once the mouse has been released.
        /// </summary>
        protected virtual void OnAngleChosen()
        {
            AngleChosen?.Invoke(this, EventArgs.Empty);
        }

        private void AnglePicker1AngleChanged(object sender, EventArgs e)
        {
            if (_nudAngle.Value != _anglePicker1.Angle)
            {
                _nudAngle.Value = _anglePicker1.Angle;
                OnAngleChanged();
            }
        }

        private void AnglePicker1AngleChosen(object sender, EventArgs e)
        {
            OnAngleChosen();
        }

        private void Configure()
        {
            _anglePicker1.AngleChosen += AnglePicker1AngleChosen;
        }

        private void NudAngleValueChanged(object sender, EventArgs e)
        {
            if (_anglePicker1.Angle != _nudAngle.Value)
            {
                _anglePicker1.Angle = (int)_nudAngle.Value;
                OnAngleChanged();
            }
        }

        #endregion
    }
}