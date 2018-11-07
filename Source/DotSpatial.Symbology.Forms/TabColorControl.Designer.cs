using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class TabColorControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        System.ComponentModel.ComponentResourceManager resources;

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
            resources = new System.ComponentModel.ComponentResourceManager(typeof(TabColorControl));
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
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tabColorRange
            // 
            resources.ApplyResources(this.tabColorRange, "tabColorRange");
            this.tabColorRange.Controls.Add(this.tabHSL);
            this.tabColorRange.Controls.Add(this.tabRGB);
            this.tabColorRange.Name = "tabColorRange";
            this.tabColorRange.SelectedIndex = 0;
            // 
            // tabHSL
            // 
            this.tabHSL.BackColor = System.Drawing.SystemColors.Control;
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
            resources.ApplyResources(this.tabHSL, "tabHSL");
            this.tabHSL.Name = "tabHSL";
            // 
            // btnReverseLight
            // 
            this.btnReverseLight.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.redbluearrows;
            resources.ApplyResources(this.btnReverseLight, "btnReverseLight");
            this.btnReverseLight.Name = "btnReverseLight";
            this.btnReverseLight.UseVisualStyleBackColor = true;
            this.btnReverseLight.Click += new System.EventHandler(this.BtnReverseLightClick);
            // 
            // btnReverseSat
            // 
            this.btnReverseSat.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.redbluearrows;
            resources.ApplyResources(this.btnReverseSat, "btnReverseSat");
            this.btnReverseSat.Name = "btnReverseSat";
            this.btnReverseSat.UseVisualStyleBackColor = true;
            this.btnReverseSat.Click += new System.EventHandler(this.BtnReverseSatClick);
            // 
            // btnReverseHue
            // 
            this.btnReverseHue.BackColor = System.Drawing.Color.Transparent;
            this.btnReverseHue.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.redbluearrows;
            resources.ApplyResources(this.btnReverseHue, "btnReverseHue");
            this.btnReverseHue.Name = "btnReverseHue";
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
            resources.ApplyResources(this.sldLightness, "sldLightness");
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
            resources.ApplyResources(this.sldSaturation, "sldSaturation");
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
            this.sldSaturation.PositionChanging += new System.EventHandler(this.SldSaturationPositionChanging);
            // 
            // btnHueShift
            // 
            this.btnHueShift.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.RunModel;
            resources.ApplyResources(this.btnHueShift, "btnHueShift");
            this.btnHueShift.Name = "btnHueShift";
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
            resources.ApplyResources(this.sldHue, "sldHue");
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
            this.sldHue.PositionChanging += new System.EventHandler(this.SldHuePositionChanging);
            // 
            // lblHueRange
            // 
            resources.ApplyResources(this.lblHueRange, "lblHueRange");
            this.lblHueRange.Name = "lblHueRange";
            // 
            // lblSaturationRange
            // 
            resources.ApplyResources(this.lblSaturationRange, "lblSaturationRange");
            this.lblSaturationRange.Name = "lblSaturationRange";
            // 
            // lblLightnessRange
            // 
            resources.ApplyResources(this.lblLightnessRange, "lblLightnessRange");
            this.lblLightnessRange.Name = "lblLightnessRange";
            // 
            // tabRGB
            // 
            this.tabRGB.BackColor = System.Drawing.SystemColors.Control;
            this.tabRGB.Controls.Add(this.rampSlider2);
            this.tabRGB.Controls.Add(this.rampSlider1);
            this.tabRGB.Controls.Add(this.lblEndColor);
            this.tabRGB.Controls.Add(this.lblStartColor);
            this.tabRGB.Controls.Add(this.cbEndColor);
            this.tabRGB.Controls.Add(this.cbStartColor);
            resources.ApplyResources(this.tabRGB, "tabRGB");
            this.tabRGB.Name = "tabRGB";
            // 
            // rampSlider2
            // 
            this.rampSlider2.ColorButton = this.cbEndColor;
            this.rampSlider2.FlipRamp = false;
            this.rampSlider2.FlipText = false;
            this.rampSlider2.InvertRamp = false;
            resources.ApplyResources(this.rampSlider2, "rampSlider2");
            this.rampSlider2.Maximum = 1D;
            this.rampSlider2.MaximumColor = System.Drawing.Color.Blue;
            this.rampSlider2.Minimum = 0D;
            this.rampSlider2.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.rampSlider2.Name = "rampSlider2";
            this.rampSlider2.NumberFormat = "#.00";
            this.rampSlider2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.rampSlider2.RampRadius = 10F;
            this.rampSlider2.RampText = "Opacity";
            this.rampSlider2.RampTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.rampSlider2.RampTextBehindRamp = true;
            this.rampSlider2.RampTextColor = System.Drawing.Color.Black;
            this.rampSlider2.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rampSlider2.ShowMaximum = false;
            this.rampSlider2.ShowMinimum = false;
            this.rampSlider2.ShowTicks = false;
            this.rampSlider2.ShowValue = false;
            this.rampSlider2.SliderColor = System.Drawing.Color.Blue;
            this.rampSlider2.SliderRadius = 4F;
            this.rampSlider2.TickColor = System.Drawing.Color.DarkGray;
            this.rampSlider2.TickSpacing = 5F;
            this.rampSlider2.Value = 1D;
            // 
            // cbEndColor
            // 
            this.cbEndColor.BevelRadius = 2;
            this.cbEndColor.Color = System.Drawing.Color.Navy;
            this.cbEndColor.LaunchDialogOnClick = true;
            resources.ApplyResources(this.cbEndColor, "cbEndColor");
            this.cbEndColor.Name = "cbEndColor";
            this.cbEndColor.RoundingRadius = 4;
            this.cbEndColor.ColorChanged += new System.EventHandler(this.CbEndColorColorChanged);
            // 
            // rampSlider1
            // 
            this.rampSlider1.ColorButton = this.cbStartColor;
            this.rampSlider1.FlipRamp = false;
            this.rampSlider1.FlipText = false;
            this.rampSlider1.InvertRamp = false;
            resources.ApplyResources(this.rampSlider1, "rampSlider1");
            this.rampSlider1.Maximum = 1D;
            this.rampSlider1.MaximumColor = System.Drawing.Color.Blue;
            this.rampSlider1.Minimum = 0D;
            this.rampSlider1.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.rampSlider1.Name = "rampSlider1";
            this.rampSlider1.NumberFormat = "#.00";
            this.rampSlider1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.rampSlider1.RampRadius = 10F;
            this.rampSlider1.RampText = "Opacity";
            this.rampSlider1.RampTextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.rampSlider1.RampTextBehindRamp = true;
            this.rampSlider1.RampTextColor = System.Drawing.Color.Black;
            this.rampSlider1.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rampSlider1.ShowMaximum = false;
            this.rampSlider1.ShowMinimum = false;
            this.rampSlider1.ShowTicks = false;
            this.rampSlider1.ShowValue = false;
            this.rampSlider1.SliderColor = System.Drawing.Color.Blue;
            this.rampSlider1.SliderRadius = 4F;
            this.rampSlider1.TickColor = System.Drawing.Color.DarkGray;
            this.rampSlider1.TickSpacing = 5F;
            this.rampSlider1.Value = 1D;
            // 
            // cbStartColor
            // 
            this.cbStartColor.BevelRadius = 2;
            this.cbStartColor.Color = System.Drawing.Color.LightBlue;
            this.cbStartColor.LaunchDialogOnClick = true;
            resources.ApplyResources(this.cbStartColor, "cbStartColor");
            this.cbStartColor.Name = "cbStartColor";
            this.cbStartColor.RoundingRadius = 4;
            this.cbStartColor.ColorChanged += new System.EventHandler(this.CbStartColorColorChanged);
            // 
            // lblEndColor
            // 
            resources.ApplyResources(this.lblEndColor, "lblEndColor");
            this.lblEndColor.Name = "lblEndColor";
            // 
            // lblStartColor
            // 
            resources.ApplyResources(this.lblStartColor, "lblStartColor");
            this.lblStartColor.Name = "lblStartColor";
            // 
            // chkUseColorRange
            // 
            resources.ApplyResources(this.chkUseColorRange, "chkUseColorRange");
            this.chkUseColorRange.Checked = true;
            this.chkUseColorRange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseColorRange.Name = "chkUseColorRange";
            this.chkUseColorRange.UseVisualStyleBackColor = true;
            this.chkUseColorRange.CheckedChanged += new System.EventHandler(this.ChkUseColorRangeCheckedChanged);
            // 
            // TabColorControl
            // 
            this.Controls.Add(this.groupBox1);
            this.Name = "TabColorControl";
            resources.ApplyResources(this, "$this");
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