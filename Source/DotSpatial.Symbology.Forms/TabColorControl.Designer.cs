using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class TabColorControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
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
            this.btnReverseLight.Click += new System.EventHandler(this.BtnReverseLightClick);
            // 
            // btnReverseSat
            // 
            this.btnReverseSat.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.redbluearrows;
            this.btnReverseSat.Location = new System.Drawing.Point(144, 78);
            this.btnReverseSat.Name = "btnReverseSat";
            this.btnReverseSat.Size = new System.Drawing.Size(28, 23);
            this.btnReverseSat.TabIndex = 6;
            this.btnReverseSat.UseVisualStyleBackColor = true;
            this.btnReverseSat.Click += new System.EventHandler(this.BtnReverseSatClick);
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
            this.btnReverseHue.Click += new System.EventHandler(this.BtnReverseHueClick);
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
            this.sldLightness.PositionChanging += new System.EventHandler(this.SldLightnessPositionChanging);
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
            this.sldSaturation.PositionChanging += new System.EventHandler(this.SldSaturationPositionChanging);
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
            this.sldHue.PositionChanging += new System.EventHandler(this.SldHuePositionChanging);
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
            this.cbEndColor.ColorChanged += new System.EventHandler(this.CbEndColorColorChanged);
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
            this.cbStartColor.ColorChanged += new System.EventHandler(this.CbStartColorColorChanged);
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
            this.chkUseColorRange.CheckedChanged += new System.EventHandler(this.ChkUseColorRangeCheckedChanged);
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
    }
}