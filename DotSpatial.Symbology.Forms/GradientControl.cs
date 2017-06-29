// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  An open source drawing pad that is super simple, but extendable
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created before 2010.
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A Control that can be used to create custom gradients.
    /// </summary>
    [DefaultEvent("GradientChanging")]
    public class GradientControl : UserControl
    {
        #region Events

        /// <summary>
        /// Occurs when the gradient is updated to its final value when the sliders or levers are released.
        /// </summary>
        public event EventHandler GradientChanged;

        /// <summary>
        /// Occurs when the gradient changes as the result of a sliding action, either from the dragging of a slider or else
        /// the dragging of a lever.
        /// </summary>
        public event EventHandler GradientChanging;

        #endregion

        #region Private Variables

        private GradientSlider _gradientSlider1;
        private ColorLever _leverMaximum;
        private ColorLever _leverMinimum;

        #endregion

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GradientControl));
            this._gradientSlider1 = new DotSpatial.Symbology.Forms.GradientSlider();
            this._leverMinimum = new DotSpatial.Symbology.Forms.ColorLever();
            this._leverMaximum = new DotSpatial.Symbology.Forms.ColorLever();
            this.SuspendLayout();
            //
            // _gradientSlider1
            //
            this._gradientSlider1.LeftHandle.Color = System.Drawing.Color.SteelBlue;
            this._gradientSlider1.LeftHandle.Position = 0.2F;
            this._gradientSlider1.LeftHandle.RoundingRadius = 4;
            this._gradientSlider1.LeftHandle.Visible = true;
            this._gradientSlider1.LeftHandle.Width = 10;
            resources.ApplyResources(this._gradientSlider1, "_gradientSlider1");
            this._gradientSlider1.Maximum = 1F;
            this._gradientSlider1.MaximumColor = System.Drawing.Color.Blue;
            this._gradientSlider1.Minimum = 0F;
            this._gradientSlider1.MinimumColor = System.Drawing.Color.Lime;
            this._gradientSlider1.Name = "_gradientSlider1";
            this._gradientSlider1.RightHandle.Color = System.Drawing.Color.SteelBlue;
            this._gradientSlider1.RightHandle.Position = 0.8F;
            this._gradientSlider1.RightHandle.RoundingRadius = 4;
            this._gradientSlider1.RightHandle.Visible = true;
            this._gradientSlider1.RightHandle.Width = 10;
            this._gradientSlider1.PositionChanging += new System.EventHandler(this.gradientSlider1_PositionChanging);
            this._gradientSlider1.PositionChanged += new System.EventHandler(this.gradientSlider1_PositionChanged);
            //
            // _leverMinimum
            //
            this._leverMinimum.Angle = 0D;
            this._leverMinimum.BackColor = System.Drawing.SystemColors.Control;
            this._leverMinimum.BarLength = 5;
            this._leverMinimum.BarWidth = 5;
            this._leverMinimum.BorderWidth = 5;
            this._leverMinimum.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))));
            this._leverMinimum.Flip = true;
            this._leverMinimum.KnobColor = System.Drawing.Color.SteelBlue;
            this._leverMinimum.KnobRadius = 7;
            resources.ApplyResources(this._leverMinimum, "_leverMinimum");
            this._leverMinimum.Name = "_leverMinimum";
            this._leverMinimum.Opacity = 0F;
            this._leverMinimum.ColorChanged += new System.EventHandler(this.leverMinimum_ColorChanged);
            this._leverMinimum.ColorChanging += new System.EventHandler(this.leverMinimum_ColorChanging);
            //
            // _leverMaximum
            //
            this._leverMaximum.Angle = 0D;
            this._leverMaximum.BackColor = System.Drawing.SystemColors.Control;
            this._leverMaximum.BarLength = 5;
            this._leverMaximum.BarWidth = 5;
            this._leverMaximum.BorderWidth = 5;
            this._leverMaximum.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this._leverMaximum.Flip = false;
            this._leverMaximum.KnobColor = System.Drawing.Color.SteelBlue;
            this._leverMaximum.KnobRadius = 7;
            resources.ApplyResources(this._leverMaximum, "_leverMaximum");
            this._leverMaximum.Name = "_leverMaximum";
            this._leverMaximum.Opacity = 1F;
            this._leverMaximum.ColorChanged += new System.EventHandler(this.leverMaximum_ColorChanged);
            this._leverMaximum.ColorChanging += new System.EventHandler(this.leverMaximum_ColorChanging);
            //
            // GradientControl
            //
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this._gradientSlider1);
            this.Controls.Add(this._leverMinimum);
            this.Controls.Add(this._leverMaximum);
            this.Name = "GradientControl";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
        }

        #endregion

        /// <summary>
        /// Creates a new instance of a Gradient Control
        /// </summary>
        public GradientControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the position of the minimum handle
        /// </summary>
        public float StartValue
        {
            get { return _gradientSlider1.LeftHandle.Position; }
            set { _gradientSlider1.LeftHandle.Position = value; }
        }

        /// <summary>
        /// Gets or sets the position of the maximum handle
        /// </summary>
        public float EndValue
        {
            get { return _gradientSlider1.RightHandle.Position; }
            set { _gradientSlider1.RightHandle.Position = value; }
        }

        /// <summary>
        /// Gets or sets the minimum color
        /// </summary>
        [Description("Gets or sets the color associated with the minimum side of the gradient slider.")]
        public Color MinimumColor
        {
            get { return _gradientSlider1.MinimumColor; }
            set
            {
                _gradientSlider1.MinimumColor = value;
                _leverMinimum.Color = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum color
        /// </summary>
        [Description("Gets or sets the color associated with the maximum side of the gradient slider.")]
        public Color MaximumColor
        {
            get { return _gradientSlider1.MaximumColor; }
            set
            {
                _gradientSlider1.MaximumColor = value;
                _leverMaximum.Color = value;
            }
        }

        /// <summary>
        /// Gets or sets a boolean that determine whether the sliders are enabled on the gradient control.
        /// Disabling the sliders will prevent them from being drawn or changed, and will automatically
        /// set the values to minimum and maximumum.
        /// </summary>
        [Description("Gets or sets a boolean that determine whether the sliders are enabled on the gradient control. Disabling the sliders will prevent them from being drawn or changed, and will automatically set the values to minimum and maximumum.")]
        public bool SlidersEnabled
        {
            get { return _gradientSlider1.Enabled; }
            set
            {
                if (value == false)
                {
                    _gradientSlider1.LeftHandle.Position = _gradientSlider1.Minimum;
                    _gradientSlider1.RightHandle.Position = _gradientSlider1.Maximum;
                }
                _gradientSlider1.Enabled = value;
                Invalidate();
            }
        }

        #region Protected Methods

        /// <summary>
        /// Fires the Gradient Changed event
        /// </summary>
        protected virtual void OnGradientChanged()
        {
            if (GradientChanged != null) GradientChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the Gradient Changing event
        /// </summary>
        protected virtual void OnGradientChanging()
        {
            if (GradientChanging != null) GradientChanging(this, EventArgs.Empty);
        }

        #endregion

        private void leverMinimum_ColorChanging(object sender, EventArgs e)
        {
            _gradientSlider1.MinimumColor = _leverMinimum.Color;
            OnGradientChanging();
        }

        private void leverMaximum_ColorChanging(object sender, EventArgs e)
        {
            _gradientSlider1.MaximumColor = _leverMaximum.Color;
            OnGradientChanging();
        }

        private void leverMaximum_ColorChanged(object sender, EventArgs e)
        {
            _gradientSlider1.MaximumColor = _leverMaximum.Color;
            OnGradientChanged();
        }

        private void leverMinimum_ColorChanged(object sender, EventArgs e)
        {
            _gradientSlider1.MinimumColor = _leverMinimum.Color;
            OnGradientChanged();
        }

        private void gradientSlider1_PositionChanging(object sender, EventArgs e)
        {
            OnGradientChanging();
        }

        private void gradientSlider1_PositionChanged(object sender, EventArgs e)
        {
            OnGradientChanged();
        }
    }
}