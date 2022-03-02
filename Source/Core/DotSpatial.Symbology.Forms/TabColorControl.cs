// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// TabColorControl.
    /// </summary>
    [DefaultEvent("ColorChanged")]
    [ToolboxItem(false)]
    public partial class TabColorControl : UserControl
    {
        #region Fields

        private Color _endColor;
        private int _endHue;
        private float _endLight;
        private float _endSat;
        private bool _hsl;
        private bool _ignoreUpdates;
        private Color _startColor;
        private int _startHue;
        private float _startLight;
        private float _startSat;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TabColorControl"/> class.
        /// </summary>
        public TabColorControl()
        {
            InitializeComponent();
            Configure();
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the color is changed.
        /// </summary>
        public event EventHandler<ColorRangeEventArgs> ColorChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the end color, which controls the RGB end color and the right HSL ranges.
        /// </summary>
        [Category("Colors")]
        [Description("Gets or sets the end color, which controls the RGB end color and the right HSL ranges")]
        public Color EndColor
        {
            get
            {
                return _endColor;
            }

            set
            {
                _endColor = value;
                SetEndHsl();
                cbEndColor.Color = value;
            }
        }

        /// <summary>
        /// Gets or sets the integer hue shift marking how much the hue slider should be shifted.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets the integer hue shift marking how much the hue slider should be shifted")]
        public int HueShift { get; set; }

        /// <summary>
        /// Gets or sets the start color, which controls the RGB start colors and the HSL left ranges.
        /// </summary>
        [Category("Colors")]
        [Description("Gets or sets the start color, which controls the RGB colors and the HSL range")]
        public Color StartColor
        {
            get
            {
                return _startColor;
            }

            set
            {
                _startColor = value;
                SetStartHsl();
                cbStartColor.Color = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the hue range is to be used.
        /// </summary>
        [Category("Behavior")]
        [Description("Gets or sets a boolean indicating whether or not the hue range is to be used.")]
        public bool UseRangeChecked
        {
            get
            {
                return chkUseColorRange.Checked;
            }

            set
            {
                chkUseColorRange.Checked = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes a new instance of this control using the specified values.
        /// </summary>
        /// <param name="args">The ColorRangeEventArgs that stores the initial values.</param>
        public void Initialize(ColorRangeEventArgs args)
        {
            _endColor = args.EndColor;
            _hsl = args.Hsl;
            HueShift = args.HueShift;
            _startColor = args.StartColor;
            chkUseColorRange.Checked = args.UseColorRange;
            SetStartHsl();
            SetEndHsl();
            UpdateControls();
        }

        /// <summary>
        /// Fires the ColorChanged event.
        /// </summary>
        protected virtual void OnColorChanged()
        {
            ColorChanged?.Invoke(this, new ColorRangeEventArgs(_startColor, _endColor, HueShift, _hsl, chkUseColorRange.Checked));
        }

        private void BtnHueShiftMouseDown(object sender, MouseEventArgs e)
        {
            tmrHueShift.Start();
        }

        private void BtnHueShiftMouseUp(object sender, MouseEventArgs e)
        {
            tmrHueShift.Stop();
        }

        private void BtnReverseHueClick(object sender, EventArgs e)
        {
            sldHue.Inverted = !sldHue.Inverted;
            SetHsl();
        }

        private void BtnReverseLightClick(object sender, EventArgs e)
        {
            sldLightness.Inverted = !sldLightness.Inverted;
            SetHsl();
        }

        private void BtnReverseSatClick(object sender, EventArgs e)
        {
            sldSaturation.Inverted = !sldSaturation.Inverted;
            SetHsl();
        }

        private void CbEndColorColorChanged(object sender, EventArgs e)
        {
            SetRgb();
        }

        private void CbStartColorColorChanged(object sender, EventArgs e)
        {
            SetRgb();
        }

        private void ChkUseColorRangeCheckedChanged(object sender, EventArgs e)
        {
            OnColorChanged();
        }

        private void Configure()
        {
            tmrHueShift = new Timer
            {
                Interval = 100
            };
            tmrHueShift.Tick += TmrHueShiftTick;
            btnHueShift.MouseDown += BtnHueShiftMouseDown;
            btnHueShift.MouseUp += BtnHueShiftMouseUp;
        }

        private void SetEndHsl()
        {
            _endHue = (int)_endColor.GetHue();
            _endSat = _endColor.GetSaturation();
            _endLight = _endColor.GetBrightness();
        }

        // Reads the current control settings to the cached values.
        private void SetHsl()
        {
            if (_ignoreUpdates) return;
            HueShift = sldHue.HueShift;
            int h = (int)(sldHue.LeftValue + (HueShift % 360));
            float s = sldSaturation.LeftValue;
            float l = sldLightness.LeftValue;
            float a = _startColor.A / 255f;
            _startColor = SymbologyGlobal.ColorFromHsl(h, s, l).ToTransparent(a);
            _startHue = h;
            _startSat = s;
            _startLight = l;
            h = (int)(sldHue.RightValue + HueShift) % 360;
            s = sldSaturation.RightValue;
            l = sldLightness.RightValue;
            a = _endColor.A / 255f;
            _endHue = h;
            _endSat = s;
            _endLight = l;
            _endColor = SymbologyGlobal.ColorFromHsl(h, s, l).ToTransparent(a);
            _hsl = true;
            chkUseColorRange.Checked = true;
            _ignoreUpdates = true;
            cbStartColor.Color = _startColor;
            cbEndColor.Color = _endColor;
            _ignoreUpdates = false;
            UpdateControls();
        }

        // Updates
        private void SetRgb()
        {
            if (_ignoreUpdates) return;
            _startColor = cbStartColor.Color;
            _endColor = cbEndColor.Color;
            SetStartHsl();
            SetEndHsl();
            chkUseColorRange.Checked = true;
            _hsl = false;
            UpdateControls();
        }

        private void SetStartHsl()
        {
            _startHue = (int)_startColor.GetHue();
            _startSat = _startColor.GetSaturation();
            _startLight = _startColor.GetBrightness();
        }

        private void SldHuePositionChanging(object sender, EventArgs e)
        {
            SetHsl();
        }

        private void SldLightnessPositionChanging(object sender, EventArgs e)
        {
            SetHsl();
        }

        private void SldSaturationPositionChanging(object sender, EventArgs e)
        {
            SetHsl();
        }

        private void TmrHueShiftTick(object sender, EventArgs e)
        {
            int shift = sldHue.Inverted ? 36 : -36;
            _ignoreUpdates = true;

            sldHue.HueShift = (sldHue.HueShift + shift) % 360;

            // sldHue.LeftValue = sldHue.LeftValue;
            // sldHue.RightValue = sldHue.RightValue;
            SetHsl();
        }

        private void UpdateControls()
        {
            // Prevent infinite loops
            if (_ignoreUpdates) return;
            _ignoreUpdates = true;
            if (_hsl)
            {
                // Don't use the colors directly, here mainly because saturation
                // loses information when going back and forth from a color.
                if (_startHue != _endHue) sldHue.Inverted = _startHue > _endHue;

                sldHue.LeftValue = _startHue;
                sldHue.RightValue = _endHue;
                if (_startSat != _endSat) sldSaturation.Inverted = _startSat > _endSat;

                sldSaturation.LeftValue = _startSat;
                sldSaturation.RightValue = _endSat;

                if (_startLight != _endLight) sldLightness.Inverted = _startLight > _endLight;
                sldLightness.LeftValue = _startLight;
                sldLightness.RightValue = _endLight;
            }
            else
            {
                sldHue.SetRange(_startColor, _endColor);
                sldSaturation.SetSaturation(_startColor, _endColor);
                sldLightness.SetLightness(_startColor, _endColor);

                sldHue.HueShift = HueShift;
                cbStartColor.Color = _startColor;
                cbEndColor.Color = _endColor;
            }

            tabColorRange.SelectedTab = _hsl ? tabHSL : tabRGB;
            _ignoreUpdates = false;
            OnColorChanged();
        }

        #endregion
    }
}