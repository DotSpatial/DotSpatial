// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  An open source drawing pad that is super simple, but extendable
// ********************************************************************************************************
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
    public partial class GradientControl : UserControl
    {
        #region Fields

        private GradientSlider _gradientSlider1;
        private ColorLever _leverMaximum;
        private ColorLever _leverMinimum;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GradientControl"/> class.
        /// </summary>
        public GradientControl()
        {
            InitializeComponent();
        }

        #endregion

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

        #region Properties

        /// <summary>
        /// Gets or sets the position of the maximum handle
        /// </summary>
        public float EndValue
        {
            get
            {
                return _gradientSlider1.RightHandle.Position;
            }

            set
            {
                _gradientSlider1.RightHandle.Position = value;
            }
        }

        /// <summary>
        /// Gets or sets the maximum color
        /// </summary>
        [Description("Gets or sets the color associated with the maximum side of the gradient slider.")]
        public Color MaximumColor
        {
            get
            {
                return _gradientSlider1.MaximumColor;
            }

            set
            {
                _gradientSlider1.MaximumColor = value;
                _leverMaximum.Color = value;
            }
        }

        /// <summary>
        /// Gets or sets the minimum color
        /// </summary>
        [Description("Gets or sets the color associated with the minimum side of the gradient slider.")]
        public Color MinimumColor
        {
            get
            {
                return _gradientSlider1.MinimumColor;
            }

            set
            {
                _gradientSlider1.MinimumColor = value;
                _leverMinimum.Color = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the sliders are enabled on the gradient control.
        /// Disabling the sliders will prevent them from being drawn or changed, and will automatically
        /// set the values to minimum and maximumum.
        /// </summary>
        [Description("Gets or sets a value indicating whether the sliders are enabled on the gradient control. Disabling the sliders will prevent them from being drawn or changed, and will automatically set the values to minimum and maximumum.")]
        public bool SlidersEnabled
        {
            get
            {
                return _gradientSlider1.Enabled;
            }

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

        /// <summary>
        /// Gets or sets the position of the minimum handle
        /// </summary>
        public float StartValue
        {
            get
            {
                return _gradientSlider1.LeftHandle.Position;
            }

            set
            {
                _gradientSlider1.LeftHandle.Position = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fires the Gradient Changed event.
        /// </summary>
        protected virtual void OnGradientChanged()
        {
            GradientChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Fires the Gradient Changing event.
        /// </summary>
        protected virtual void OnGradientChanging()
        {
            GradientChanging?.Invoke(this, EventArgs.Empty);
        }

        private void GradientSlider1PositionChanged(object sender, EventArgs e)
        {
            OnGradientChanged();
        }

        private void GradientSlider1PositionChanging(object sender, EventArgs e)
        {
            OnGradientChanging();
        }

        private void LeverMaximumColorChanged(object sender, EventArgs e)
        {
            _gradientSlider1.MaximumColor = _leverMaximum.Color;
            OnGradientChanged();
        }

        private void LeverMaximumColorChanging(object sender, EventArgs e)
        {
            _gradientSlider1.MaximumColor = _leverMaximum.Color;
            OnGradientChanging();
        }

        private void LeverMinimumColorChanged(object sender, EventArgs e)
        {
            _gradientSlider1.MinimumColor = _leverMinimum.Color;
            OnGradientChanged();
        }

        private void LeverMinimumColorChanging(object sender, EventArgs e)
        {
            _gradientSlider1.MinimumColor = _leverMinimum.Color;
            OnGradientChanging();
        }

        #endregion
    }
}