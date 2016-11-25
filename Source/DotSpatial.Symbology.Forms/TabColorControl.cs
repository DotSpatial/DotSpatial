// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 10/6/2009 8:53:01 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// TabColorControl
    /// </summary>
    [DefaultEvent("ColorChanged"),
    ToolboxItem(false)]
    public class TabColorControl : UserControl
    {
        #region Events

        /// <summary>
        /// Occurs when the color is changed.
        /// </summary>
        public event EventHandler<ColorRangeEventArgs> ColorChanged;

        #endregion

        #region Private Variables

        private Color _endColor;
        private int _endHue;
        private float _endLight;
        private float _endSat;
        private bool _hsl;
        private int _hueShift;
        private bool _ignoreUpdates;
        private Color _startColor;
        private int _startHue;
        private float _startLight;
        private float _startSat;
        private Button btnHueShift;
        private Button btnReverseHue;
        private Button btnReverseLight;
        private Button btnReverseSat;
        private ColorButton cbEndColor;
        private ColorButton cbStartColor;
        private CheckBox chkUseColorRange;
        private GroupBox groupBox1;
        private Label lblEndColor;
        private Label lblHueRange;
        private Label lblLightnessRange;
        private Label lblSaturationRange;
        private Label lblStartColor;
        private RampSlider rampSlider1;
        private RampSlider rampSlider2;
        private HueSlider sldHue;
        private TwoColorSlider sldLightness;
        private TwoColorSlider sldSaturation;
        private TabControl tabColorRange;
        private TabPage tabHSL;
        private TabPage tabRGB;
        private Timer tmrHueShift;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of TabColorControl
        /// </summary>
        public TabColorControl()
        {
            InitializeComponent();
            Configure();
        }

        private void Configure()
        {
            tmrHueShift = new Timer { Interval = 100 };
            tmrHueShift.Tick += tmrHueShift_Tick;
            btnHueShift.MouseDown += btnHueShift_MouseDown;
            btnHueShift.MouseUp += btnHueShift_MouseUp;
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
            _hsl = args.HSL;
            _hueShift = args.HueShift;
            _startColor = args.StartColor;
            chkUseColorRange.Checked = args.UseColorRange;
            SetStartHsl();
            SetEndHsl();
            UpdateControls();
        }

        private void SetStartHsl()
        {
            _startHue = (int)_startColor.GetHue();
            _startSat = _startColor.GetSaturation();
            _startLight = _startColor.GetBrightness();
        }

        private void SetEndHsl()
        {
            _endHue = (int)_endColor.GetHue();
            _endSat = _endColor.GetSaturation();
            _endLight = _endColor.GetBrightness();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the start color, which controls the RGB start colors and the HSL left ranges
        /// </summary>
        [Category("Colors"), Description("Gets or sets the start color, which controls the RGB colors and the HSL range")]
        public Color StartColor
        {
            get { return _startColor; }
            set
            {
                _startColor = value;
                SetStartHsl();
                cbStartColor.Color = value;
            }
        }

        /// <summary>
        /// Gets or sets the end color, which controls the RGB end color and the right HSL ranges
        /// </summary>
        [Category("Colors"), Description("Gets or sets the end color, which controls the RGB end color and the right HSL ranges")]
        public Color EndColor
        {
            get { return _endColor; }
            set
            {
                _endColor = value;
                SetEndHsl();
                cbEndColor.Color = value;
            }
        }

        /// <summary>
        /// Gets or sets the integer hue shift marking how much the hue slider should be shifted
        /// </summary>
        [Category("Behavior"), Description("Gets or sets the integer hue shift marking how much the hue slider should be shifted")]
        public int HueShift
        {
            get { return _hueShift; }
            set { _hueShift = value; }
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether or not the hue range is to be used.
        /// </summary>
        [Category("Behavior"), Description("Gets or sets a boolean indicating whether or not the hue range is to be used.")]
        public bool UseRangeChecked
        {
            get { return chkUseColorRange.Checked; }
            set { chkUseColorRange.Checked = value; }
        }

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Disposes the unmanaged memory or controls
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabColorRange = new System.Windows.Forms.TabControl();
            this.tabHSL = new System.Windows.Forms.TabPage();
            this.btnReverseLight = new System.Windows.Forms.Button();
            this.btnReverseSat = new System.Windows.Forms.Button();
            this.btnReverseHue = new System.Windows.Forms.Button();
            this.sldLightness = new DotSpatial.Symbology.Forms.TwoColorSlider();
            this.sldSaturation = new DotSpatial.Symbology.Forms.TwoColorSlider();
            this.btnHueShift = new System.Windows.Forms.Button();
            this.sldHue = new DotSpatial.Symbology.Forms.HueSlider();
            this.lblHueRange = new System.Windows.Forms.Label();
            this.lblSaturationRange = new System.Windows.Forms.Label();
            this.lblLightnessRange = new System.Windows.Forms.Label();
            this.tabRGB = new System.Windows.Forms.TabPage();
            this.rampSlider2 = new DotSpatial.Symbology.Forms.RampSlider();
            this.cbEndColor = new DotSpatial.Symbology.Forms.ColorButton();
            this.rampSlider1 = new DotSpatial.Symbology.Forms.RampSlider();
            this.cbStartColor = new DotSpatial.Symbology.Forms.ColorButton();
            this.lblEndColor = new System.Windows.Forms.Label();
            this.lblStartColor = new System.Windows.Forms.Label();
            this.chkUseColorRange = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.tabColorRange.SuspendLayout();
            this.tabHSL.SuspendLayout();
            this.tabRGB.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabColorRange);
            this.groupBox1.Controls.Add(this.chkUseColorRange);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(227, 219);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // tabColorRange
            // 
            this.tabColorRange.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabColorRange.Controls.Add(this.tabHSL);
            this.tabColorRange.Controls.Add(this.tabRGB);
            this.tabColorRange.Location = new System.Drawing.Point(6, 23);
            this.tabColorRange.Name = "tabColorRange";
            this.tabColorRange.SelectedIndex = 0;
            this.tabColorRange.Size = new System.Drawing.Size(217, 189);
            this.tabColorRange.TabIndex = 12;
            // 
            // tabHSL
            // 
            this.tabHSL.Controls.Add(this.btnReverseLight);
            this.tabHSL.Controls.Add(this.btnReverseSat);
            this.tabHSL.Controls.Add(this.btnReverseHue);
            this.tabHSL.Controls.Add(this.sldLightness);
            this.tabHSL.Controls.Add(this.sldSaturation);
            this.tabHSL.Controls.Add(this.btnHueShift);
            this.tabHSL.Controls.Add(this.sldHue);
            this.tabHSL.Controls.Add(this.lblHueRange);
            this.tabHSL.Controls.Add(this.lblSaturationRange);
            this.tabHSL.Controls.Add(this.lblLightnessRange);
            this.tabHSL.Location = new System.Drawing.Point(4, 25);
            this.tabHSL.Name = "tabHSL";
            this.tabHSL.Padding = new System.Windows.Forms.Padding(3);
            this.tabHSL.Size = new System.Drawing.Size(209, 160);
            this.tabHSL.TabIndex = 0;
            this.tabHSL.Text = "HSL";
            this.tabHSL.UseVisualStyleBackColor = true;
            // 
            // btnReverseLight
            // 
            this.btnReverseLight.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.redbluearrows;
            this.btnReverseLight.Location = new System.Drawing.Point(144, 127);
            this.btnReverseLight.Name = "btnReverseLight";
            this.btnReverseLight.Size = new System.Drawing.Size(28, 23);
            this.btnReverseLight.TabIndex = 9;
            this.btnReverseLight.UseVisualStyleBackColor = true;
            this.btnReverseLight.Click += new System.EventHandler(this.btnReverseLight_Click);
            // 
            // btnReverseSat
            // 
            this.btnReverseSat.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.redbluearrows;
            this.btnReverseSat.Location = new System.Drawing.Point(144, 78);
            this.btnReverseSat.Name = "btnReverseSat";
            this.btnReverseSat.Size = new System.Drawing.Size(28, 23);
            this.btnReverseSat.TabIndex = 6;
            this.btnReverseSat.UseVisualStyleBackColor = true;
            this.btnReverseSat.Click += new System.EventHandler(this.btnReverseSat_Click);
            // 
            // btnReverseHue
            // 
            this.btnReverseHue.BackColor = System.Drawing.Color.Transparent;
            this.btnReverseHue.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.redbluearrows;
            this.btnReverseHue.Location = new System.Drawing.Point(144, 29);
            this.btnReverseHue.Name = "btnReverseHue";
            this.btnReverseHue.Size = new System.Drawing.Size(28, 23);
            this.btnReverseHue.TabIndex = 2;
            this.btnReverseHue.UseVisualStyleBackColor = false;
            this.btnReverseHue.Click += new System.EventHandler(this.btnReverseHue_Click);
            // 
            // sldLightness
            // 
            this.sldLightness.Inverted = false;
            this.sldLightness.LeftHandle.Color = System.Drawing.Color.SteelBlue;
            this.sldLightness.LeftHandle.IsLeft = true;
            this.sldLightness.LeftHandle.Position = 0.0406504F;
            this.sldLightness.LeftHandle.RoundingRadius = 2;
            this.sldLightness.LeftHandle.Visible = true;
            this.sldLightness.LeftHandle.Width = 5;
            this.sldLightness.LeftValue = 0.0406504F;
            this.sldLightness.Location = new System.Drawing.Point(15, 127);
            this.sldLightness.Maximum = 1F;
            this.sldLightness.MaximumColor = System.Drawing.Color.White;
            this.sldLightness.Minimum = 0F;
            this.sldLightness.MinimumColor = System.Drawing.Color.Black;
            this.sldLightness.Name = "sldLightness";
            this.sldLightness.RightHandle.Color = System.Drawing.Color.SteelBlue;
            this.sldLightness.RightHandle.IsLeft = false;
            this.sldLightness.RightHandle.Position = 0.8F;
            this.sldLightness.RightHandle.RoundingRadius = 2;
            this.sldLightness.RightHandle.Visible = true;
            this.sldLightness.RightHandle.Width = 5;
            this.sldLightness.RightValue = 0.8F;
            this.sldLightness.Size = new System.Drawing.Size(123, 23);
            this.sldLightness.TabIndex = 8;
            this.sldLightness.Text = "twoColorSlider2";
            this.sldLightness.PositionChanging += new System.EventHandler(this.sldLightness_PositionChanging);
            // 
            // sldSaturation
            // 
            this.sldSaturation.Inverted = false;
            this.sldSaturation.LeftHandle.Color = System.Drawing.Color.SteelBlue;
            this.sldSaturation.LeftHandle.IsLeft = true;
            this.sldSaturation.LeftHandle.Position = 0.04098361F;
            this.sldSaturation.LeftHandle.RoundingRadius = 2;
            this.sldSaturation.LeftHandle.Visible = true;
            this.sldSaturation.LeftHandle.Width = 5;
            this.sldSaturation.LeftValue = 0.04098361F;
            this.sldSaturation.Location = new System.Drawing.Point(15, 78);
            this.sldSaturation.Maximum = 1F;
            this.sldSaturation.MaximumColor = System.Drawing.Color.Blue;
            this.sldSaturation.Minimum = 0F;
            this.sldSaturation.MinimumColor = System.Drawing.Color.White;
            this.sldSaturation.Name = "sldSaturation";
            this.sldSaturation.RightHandle.Color = System.Drawing.Color.SteelBlue;
            this.sldSaturation.RightHandle.IsLeft = false;
            this.sldSaturation.RightHandle.Position = 0.8F;
            this.sldSaturation.RightHandle.RoundingRadius = 2;
            this.sldSaturation.RightHandle.Visible = true;
            this.sldSaturation.RightHandle.Width = 5;
            this.sldSaturation.RightValue = 0.8F;
            this.sldSaturation.Size = new System.Drawing.Size(122, 23);
            this.sldSaturation.TabIndex = 5;
            this.sldSaturation.Text = "twoColorSlider1";
            this.sldSaturation.PositionChanging += new System.EventHandler(this.sldSaturation_PositionChanging);
            // 
            // btnHueShift
            // 
            this.btnHueShift.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.RunModel;
            this.btnHueShift.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnHueShift.Location = new System.Drawing.Point(178, 29);
            this.btnHueShift.Name = "btnHueShift";
            this.btnHueShift.Size = new System.Drawing.Size(21, 23);
            this.btnHueShift.TabIndex = 3;
            this.btnHueShift.UseVisualStyleBackColor = true;
            // 
            // sldHue
            // 
            this.sldHue.HueShift = 0;
            this.sldHue.Inverted = false;
            this.sldHue.LeftHandle.Color = System.Drawing.Color.SteelBlue;
            this.sldHue.LeftHandle.Left = true;
            this.sldHue.LeftHandle.Position = 14.7541F;
            this.sldHue.LeftHandle.RoundingRadius = 2;
            this.sldHue.LeftHandle.Visible = true;
            this.sldHue.LeftHandle.Width = 5;
            this.sldHue.LeftValue = 14.7541F;
            this.sldHue.Location = new System.Drawing.Point(16, 29);
            this.sldHue.Maximum = 360;
            this.sldHue.Minimum = 0;
            this.sldHue.Name = "sldHue";
            this.sldHue.RightHandle.Color = System.Drawing.Color.SteelBlue;
            this.sldHue.RightHandle.Left = false;
            this.sldHue.RightHandle.Position = 288F;
            this.sldHue.RightHandle.RoundingRadius = 2;
            this.sldHue.RightHandle.Visible = true;
            this.sldHue.RightHandle.Width = 5;
            this.sldHue.RightValue = 288F;
            this.sldHue.Size = new System.Drawing.Size(122, 23);
            this.sldHue.TabIndex = 1;
            this.sldHue.Text = "hueSlider1";
            this.sldHue.PositionChanging += new System.EventHandler(this.sldHue_PositionChanging);
            // 
            // lblHueRange
            // 
            this.lblHueRange.AutoSize = true;
            this.lblHueRange.Location = new System.Drawing.Point(9, 13);
            this.lblHueRange.Name = "lblHueRange";
            this.lblHueRange.Size = new System.Drawing.Size(84, 17);
            this.lblHueRange.TabIndex = 0;
            this.lblHueRange.Text = "Hue Range:";
            // 
            // lblSaturationRange
            // 
            this.lblSaturationRange.AutoSize = true;
            this.lblSaturationRange.Location = new System.Drawing.Point(9, 62);
            this.lblSaturationRange.Name = "lblSaturationRange";
            this.lblSaturationRange.Size = new System.Drawing.Size(123, 17);
            this.lblSaturationRange.TabIndex = 4;
            this.lblSaturationRange.Text = "Saturation Range:";
            // 
            // lblLightnessRange
            // 
            this.lblLightnessRange.AutoSize = true;
            this.lblLightnessRange.Location = new System.Drawing.Point(9, 111);
            this.lblLightnessRange.Name = "lblLightnessRange";
            this.lblLightnessRange.Size = new System.Drawing.Size(119, 17);
            this.lblLightnessRange.TabIndex = 7;
            this.lblLightnessRange.Text = "Lightness Range:";
            // 
            // tabRGB
            // 
            this.tabRGB.Controls.Add(this.rampSlider2);
            this.tabRGB.Controls.Add(this.rampSlider1);
            this.tabRGB.Controls.Add(this.lblEndColor);
            this.tabRGB.Controls.Add(this.lblStartColor);
            this.tabRGB.Controls.Add(this.cbEndColor);
            this.tabRGB.Controls.Add(this.cbStartColor);
            this.tabRGB.Location = new System.Drawing.Point(4, 25);
            this.tabRGB.Name = "tabRGB";
            this.tabRGB.Padding = new System.Windows.Forms.Padding(3);
            this.tabRGB.Size = new System.Drawing.Size(209, 160);
            this.tabRGB.TabIndex = 1;
            this.tabRGB.Text = "RGB";
            this.tabRGB.UseVisualStyleBackColor = true;
            // 
            // rampSlider2
            // 
            this.rampSlider2.ColorButton = this.cbEndColor;
            this.rampSlider2.FlipRamp = false;
            this.rampSlider2.FlipText = false;
            this.rampSlider2.InvertRamp = false;
            this.rampSlider2.Location = new System.Drawing.Point(93, 106);
            this.rampSlider2.Maximum = 1D;
            this.rampSlider2.MaximumColor = System.Drawing.Color.Blue;
            this.rampSlider2.Minimum = 0D;
            this.rampSlider2.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.rampSlider2.Name = "rampSlider2";
            this.rampSlider2.NumberFormat = "#.00";
            this.rampSlider2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.rampSlider2.RampRadius = 10F;
            this.rampSlider2.RampText = "Opacity";
            this.rampSlider2.RampTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rampSlider2.RampTextBehindRamp = true;
            this.rampSlider2.RampTextColor = System.Drawing.Color.Black;
            this.rampSlider2.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rampSlider2.ShowMaximum = false;
            this.rampSlider2.ShowMinimum = false;
            this.rampSlider2.ShowTicks = false;
            this.rampSlider2.ShowValue = false;
            this.rampSlider2.Size = new System.Drawing.Size(97, 25);
            this.rampSlider2.SliderColor = System.Drawing.Color.Blue;
            this.rampSlider2.SliderRadius = 4F;
            this.rampSlider2.TabIndex = 5;
            this.rampSlider2.Text = "rampSlider2";
            this.rampSlider2.TickColor = System.Drawing.Color.DarkGray;
            this.rampSlider2.TickSpacing = 5F;
            this.rampSlider2.Value = 1D;
            // 
            // cbEndColor
            // 
            this.cbEndColor.BevelRadius = 2;
            this.cbEndColor.Color = System.Drawing.Color.Navy;
            this.cbEndColor.LaunchDialogOnClick = true;
            this.cbEndColor.Location = new System.Drawing.Point(33, 106);
            this.cbEndColor.Name = "cbEndColor";
            this.cbEndColor.RoundingRadius = 4;
            this.cbEndColor.Size = new System.Drawing.Size(40, 25);
            this.cbEndColor.TabIndex = 2;
            this.cbEndColor.Text = "colorButton2";
            this.cbEndColor.ColorChanged += new System.EventHandler(this.cbEndColor_ColorChanged);
            // 
            // rampSlider1
            // 
            this.rampSlider1.ColorButton = this.cbStartColor;
            this.rampSlider1.FlipRamp = false;
            this.rampSlider1.FlipText = false;
            this.rampSlider1.InvertRamp = false;
            this.rampSlider1.Location = new System.Drawing.Point(93, 38);
            this.rampSlider1.Maximum = 1D;
            this.rampSlider1.MaximumColor = System.Drawing.Color.Blue;
            this.rampSlider1.Minimum = 0D;
            this.rampSlider1.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.rampSlider1.Name = "rampSlider1";
            this.rampSlider1.NumberFormat = "#.00";
            this.rampSlider1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.rampSlider1.RampRadius = 10F;
            this.rampSlider1.RampText = "Opacity";
            this.rampSlider1.RampTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.rampSlider1.RampTextBehindRamp = true;
            this.rampSlider1.RampTextColor = System.Drawing.Color.Black;
            this.rampSlider1.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rampSlider1.ShowMaximum = false;
            this.rampSlider1.ShowMinimum = false;
            this.rampSlider1.ShowTicks = false;
            this.rampSlider1.ShowValue = false;
            this.rampSlider1.Size = new System.Drawing.Size(97, 25);
            this.rampSlider1.SliderColor = System.Drawing.Color.Blue;
            this.rampSlider1.SliderRadius = 4F;
            this.rampSlider1.TabIndex = 4;
            this.rampSlider1.Text = "rampSlider1";
            this.rampSlider1.TickColor = System.Drawing.Color.DarkGray;
            this.rampSlider1.TickSpacing = 5F;
            this.rampSlider1.Value = 1D;
            // 
            // cbStartColor
            // 
            this.cbStartColor.BevelRadius = 2;
            this.cbStartColor.Color = System.Drawing.Color.LightBlue;
            this.cbStartColor.LaunchDialogOnClick = true;
            this.cbStartColor.Location = new System.Drawing.Point(33, 38);
            this.cbStartColor.Name = "cbStartColor";
            this.cbStartColor.RoundingRadius = 4;
            this.cbStartColor.Size = new System.Drawing.Size(40, 25);
            this.cbStartColor.TabIndex = 0;
            this.cbStartColor.Text = "colorButton1";
            this.cbStartColor.ColorChanged += new System.EventHandler(this.cbStartColor_ColorChanged);
            // 
            // lblEndColor
            // 
            this.lblEndColor.AutoSize = true;
            this.lblEndColor.Location = new System.Drawing.Point(8, 80);
            this.lblEndColor.Name = "lblEndColor";
            this.lblEndColor.Size = new System.Drawing.Size(70, 17);
            this.lblEndColor.TabIndex = 3;
            this.lblEndColor.Text = "&End Color";
            // 
            // lblStartColor
            // 
            this.lblStartColor.AutoSize = true;
            this.lblStartColor.Location = new System.Drawing.Point(8, 12);
            this.lblStartColor.Name = "lblStartColor";
            this.lblStartColor.Size = new System.Drawing.Size(75, 17);
            this.lblStartColor.TabIndex = 1;
            this.lblStartColor.Text = "&Start Color";
            // 
            // chkUseColorRange
            // 
            this.chkUseColorRange.AutoSize = true;
            this.chkUseColorRange.Checked = true;
            this.chkUseColorRange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseColorRange.Location = new System.Drawing.Point(6, 0);
            this.chkUseColorRange.Name = "chkUseColorRange";
            this.chkUseColorRange.Size = new System.Drawing.Size(138, 21);
            this.chkUseColorRange.TabIndex = 11;
            this.chkUseColorRange.Text = "Use Color &Range";
            this.chkUseColorRange.UseVisualStyleBackColor = true;
            this.chkUseColorRange.CheckedChanged += new System.EventHandler(this.chkUseColorRange_CheckedChanged);
            // 
            // TabColorControl
            // 
            this.Controls.Add(this.groupBox1);
            this.Name = "TabColorControl";
            this.Size = new System.Drawing.Size(227, 219);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabColorRange.ResumeLayout(false);
            this.tabHSL.ResumeLayout(false);
            this.tabHSL.PerformLayout();
            this.tabRGB.ResumeLayout(false);
            this.tabRGB.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private void btnHueShift_MouseUp(object sender, MouseEventArgs e)
        {
            tmrHueShift.Stop();
        }

        private void btnHueShift_MouseDown(object sender, MouseEventArgs e)
        {
            tmrHueShift.Start();
        }

        private void sldHue_PositionChanging(object sender, EventArgs e)
        {
            SetHsl();
        }

        private void tmrHueShift_Tick(object sender, EventArgs e)
        {
            int shift = sldHue.Inverted ? 36 : -36;
            _ignoreUpdates = true;

            sldHue.HueShift = (sldHue.HueShift + shift) % 360;
            //sldHue.LeftValue = sldHue.LeftValue;
            //sldHue.RightValue = sldHue.RightValue;

            SetHsl();
        }

        private void btnReverseHue_Click(object sender, EventArgs e)
        {
            sldHue.Inverted = !sldHue.Inverted;
            SetHsl();
        }

        private void btnReverseSat_Click(object sender, EventArgs e)
        {
            sldSaturation.Inverted = !sldSaturation.Inverted;
            SetHsl();
        }

        private void btnReverseLight_Click(object sender, EventArgs e)
        {
            sldLightness.Inverted = !sldLightness.Inverted;
            SetHsl();
        }

        private void sldSaturation_PositionChanging(object sender, EventArgs e)
        {
            SetHsl();
        }

        private void sldLightness_PositionChanging(object sender, EventArgs e)
        {
            SetHsl();
        }

        private void cbStartColor_ColorChanged(object sender, EventArgs e)
        {
            SetRgb();
        }

        private void cbEndColor_ColorChanged(object sender, EventArgs e)
        {
            SetRgb();
        }

        // Reads the current control settings to the cached values.
        private void SetHsl()
        {
            if (_ignoreUpdates) return;
            _hueShift = sldHue.HueShift;
            int h = (int)(sldHue.LeftValue + (_hueShift) % 360);
            float s = sldSaturation.LeftValue;
            float l = sldLightness.LeftValue;
            float a = _startColor.A / 255f;
            _startColor = SymbologyGlobal.ColorFromHsl(h, s, l).ToTransparent(a);
            _startHue = h;
            _startSat = s;
            _startLight = l;
            h = (int)(sldHue.RightValue + _hueShift) % 360;
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

                sldHue.HueShift = _hueShift;
                cbStartColor.Color = _startColor;
                cbEndColor.Color = _endColor;
            }

            tabColorRange.SelectedTab = _hsl ? tabHSL : tabRGB;
            _ignoreUpdates = false;
            OnColorChanged();
        }

        /// <summary>
        /// Fires the ColorChanged event
        /// </summary>
        protected virtual void OnColorChanged()
        {
            if (ColorChanged != null) ColorChanged(this, new ColorRangeEventArgs(_startColor, _endColor, _hueShift, _hsl, chkUseColorRange.Checked));
        }

        private void chkUseColorRange_CheckedChanged(object sender, EventArgs e)
        {
            OnColorChanged();
        }
    }
}