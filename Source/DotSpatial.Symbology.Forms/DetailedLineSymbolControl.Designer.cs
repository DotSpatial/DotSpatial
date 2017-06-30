using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    partial class DetailedLineSymbolControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DetailedLineSymbolControl));
            this.cmbStrokeType = new System.Windows.Forms.ComboBox();
            this.chkSmoothing = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblPreview = new System.Windows.Forms.Label();
            this.lblScaleMode = new System.Windows.Forms.Label();
            this.cmbScaleMode = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabStrokeProperties = new System.Windows.Forms.TabControl();
            this.tabSimple = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cbColorSimple = new DotSpatial.Symbology.Forms.ColorButton();
            this.sldOpacitySimple = new DotSpatial.Symbology.Forms.RampSlider();
            this.cmbStrokeStyle = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dblWidth = new DotSpatial.Symbology.Forms.DoubleBox();
            this.tabCartographic = new System.Windows.Forms.TabPage();
            this.lblColorCartographic = new System.Windows.Forms.Label();
            this.lblOpacityCartographic = new System.Windows.Forms.Label();
            this.grpCaps = new System.Windows.Forms.GroupBox();
            this.lblStartCap = new System.Windows.Forms.Label();
            this.cmbEndCap = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbStartCap = new System.Windows.Forms.ComboBox();
            this.cbColorCartographic = new DotSpatial.Symbology.Forms.ColorButton();
            this.sldOpacityCartographic = new DotSpatial.Symbology.Forms.RampSlider();
            this.dblStrokeOffset = new DotSpatial.Symbology.Forms.DoubleBox();
            this.radLineJoin = new DotSpatial.Symbology.Forms.LineJoinControl();
            this.dblWidthCartographic = new DotSpatial.Symbology.Forms.DoubleBox();
            this.tabDash = new System.Windows.Forms.TabPage();
            this.dashControl1 = new DotSpatial.Symbology.Forms.DashControl();
            this.tabDecoration = new System.Windows.Forms.TabPage();
            this.lblPercentualPosition = new System.Windows.Forms.Label();
            this.nudPercentualPosition = new System.Windows.Forms.NumericUpDown();
            this.btnEdit = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.lblDecorationPreview = new System.Windows.Forms.Label();
            this.lblNumberOfPositions = new System.Windows.Forms.Label();
            this.nudDecorationCount = new System.Windows.Forms.NumericUpDown();
            this.grpRotation = new System.Windows.Forms.GroupBox();
            this.radRotationFixed = new System.Windows.Forms.RadioButton();
            this.radRotationWithLine = new System.Windows.Forms.RadioButton();
            this.grpFlip = new System.Windows.Forms.GroupBox();
            this.chkFlipFirst = new System.Windows.Forms.CheckBox();
            this.chkFlipAll = new System.Windows.Forms.CheckBox();
            this.dblOffset = new DotSpatial.Symbology.Forms.DoubleBox();
            this.ccDecorations = new DotSpatial.Symbology.Forms.DecorationCollectionControl();
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.ccStrokes = new DotSpatial.Symbology.Forms.StrokeCollectionControl();
            this.groupBox1.SuspendLayout();
            this.tabStrokeProperties.SuspendLayout();
            this.tabSimple.SuspendLayout();
            this.tabCartographic.SuspendLayout();
            this.grpCaps.SuspendLayout();
            this.tabDash.SuspendLayout();
            this.tabDecoration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPercentualPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDecorationCount)).BeginInit();
            this.grpRotation.SuspendLayout();
            this.grpFlip.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbStrokeType
            // 
            resources.ApplyResources(this.cmbStrokeType, "cmbStrokeType");
            this.cmbStrokeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStrokeType.FormattingEnabled = true;
            this.cmbStrokeType.Items.AddRange(new object[] {
            resources.GetString("cmbStrokeType.Items"),
            resources.GetString("cmbStrokeType.Items1")});
            this.cmbStrokeType.Name = "cmbStrokeType";
            this.ttHelp.SetToolTip(this.cmbStrokeType, resources.GetString("cmbStrokeType.ToolTip"));
            this.cmbStrokeType.SelectedIndexChanged += new System.EventHandler(this.CmbStrokeTypeSelectedIndexChanged);
            // 
            // chkSmoothing
            // 
            resources.ApplyResources(this.chkSmoothing, "chkSmoothing");
            this.chkSmoothing.Name = "chkSmoothing";
            this.ttHelp.SetToolTip(this.chkSmoothing, resources.GetString("chkSmoothing.ToolTip"));
            this.chkSmoothing.UseVisualStyleBackColor = true;
            this.chkSmoothing.CheckedChanged += new System.EventHandler(this.ChkSmoothingCheckedChanged);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lblPreview);
            this.groupBox1.Controls.Add(this.lblScaleMode);
            this.groupBox1.Controls.Add(this.cmbScaleMode);
            this.groupBox1.Controls.Add(this.chkSmoothing);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.ttHelp.SetToolTip(this.groupBox1, resources.GetString("groupBox1.ToolTip"));
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            this.ttHelp.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
            // 
            // lblPreview
            // 
            resources.ApplyResources(this.lblPreview, "lblPreview");
            this.lblPreview.BackColor = System.Drawing.Color.White;
            this.lblPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblPreview.Name = "lblPreview";
            this.ttHelp.SetToolTip(this.lblPreview, resources.GetString("lblPreview.ToolTip"));
            // 
            // lblScaleMode
            // 
            resources.ApplyResources(this.lblScaleMode, "lblScaleMode");
            this.lblScaleMode.Name = "lblScaleMode";
            this.ttHelp.SetToolTip(this.lblScaleMode, resources.GetString("lblScaleMode.ToolTip"));
            // 
            // cmbScaleMode
            // 
            resources.ApplyResources(this.cmbScaleMode, "cmbScaleMode");
            this.cmbScaleMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbScaleMode.FormattingEnabled = true;
            this.cmbScaleMode.Items.AddRange(new object[] {
            resources.GetString("cmbScaleMode.Items"),
            resources.GetString("cmbScaleMode.Items1"),
            resources.GetString("cmbScaleMode.Items2")});
            this.cmbScaleMode.Name = "cmbScaleMode";
            this.ttHelp.SetToolTip(this.cmbScaleMode, resources.GetString("cmbScaleMode.ToolTip"));
            this.cmbScaleMode.SelectedIndexChanged += new System.EventHandler(this.CmbScaleModeSelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.ttHelp.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // tabStrokeProperties
            // 
            resources.ApplyResources(this.tabStrokeProperties, "tabStrokeProperties");
            this.tabStrokeProperties.Controls.Add(this.tabSimple);
            this.tabStrokeProperties.Controls.Add(this.tabCartographic);
            this.tabStrokeProperties.Controls.Add(this.tabDash);
            this.tabStrokeProperties.Controls.Add(this.tabDecoration);
            this.tabStrokeProperties.Name = "tabStrokeProperties";
            this.tabStrokeProperties.SelectedIndex = 0;
            this.ttHelp.SetToolTip(this.tabStrokeProperties, resources.GetString("tabStrokeProperties.ToolTip"));
            // 
            // tabSimple
            // 
            resources.ApplyResources(this.tabSimple, "tabSimple");
            this.tabSimple.AllowDrop = true;
            this.tabSimple.Controls.Add(this.label5);
            this.tabSimple.Controls.Add(this.label9);
            this.tabSimple.Controls.Add(this.cbColorSimple);
            this.tabSimple.Controls.Add(this.sldOpacitySimple);
            this.tabSimple.Controls.Add(this.cmbStrokeStyle);
            this.tabSimple.Controls.Add(this.label2);
            this.tabSimple.Controls.Add(this.dblWidth);
            this.tabSimple.Name = "tabSimple";
            this.ttHelp.SetToolTip(this.tabSimple, resources.GetString("tabSimple.ToolTip"));
            this.tabSimple.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            this.ttHelp.SetToolTip(this.label5, resources.GetString("label5.ToolTip"));
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            this.ttHelp.SetToolTip(this.label9, resources.GetString("label9.ToolTip"));
            // 
            // cbColorSimple
            // 
            resources.ApplyResources(this.cbColorSimple, "cbColorSimple");
            this.cbColorSimple.BevelRadius = 6;
            this.cbColorSimple.Color = System.Drawing.Color.Blue;
            this.cbColorSimple.LaunchDialogOnClick = true;
            this.cbColorSimple.Name = "cbColorSimple";
            this.cbColorSimple.RoundingRadius = 15;
            this.ttHelp.SetToolTip(this.cbColorSimple, resources.GetString("cbColorSimple.ToolTip"));
            // 
            // sldOpacitySimple
            // 
            resources.ApplyResources(this.sldOpacitySimple, "sldOpacitySimple");
            this.sldOpacitySimple.ColorButton = null;
            this.sldOpacitySimple.FlipRamp = false;
            this.sldOpacitySimple.FlipText = false;
            this.sldOpacitySimple.InvertRamp = false;
            this.sldOpacitySimple.Maximum = 1D;
            this.sldOpacitySimple.MaximumColor = System.Drawing.Color.CornflowerBlue;
            this.sldOpacitySimple.Minimum = 0D;
            this.sldOpacitySimple.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sldOpacitySimple.Name = "sldOpacitySimple";
            this.sldOpacitySimple.NumberFormat = null;
            this.sldOpacitySimple.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.sldOpacitySimple.RampRadius = 10F;
            this.sldOpacitySimple.RampText = "Opacity";
            this.sldOpacitySimple.RampTextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.sldOpacitySimple.RampTextBehindRamp = true;
            this.sldOpacitySimple.RampTextColor = System.Drawing.Color.Black;
            this.sldOpacitySimple.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sldOpacitySimple.ShowMaximum = true;
            this.sldOpacitySimple.ShowMinimum = true;
            this.sldOpacitySimple.ShowTicks = true;
            this.sldOpacitySimple.ShowValue = false;
            this.sldOpacitySimple.SliderColor = System.Drawing.Color.Blue;
            this.sldOpacitySimple.SliderRadius = 4F;
            this.sldOpacitySimple.TickColor = System.Drawing.Color.DarkGray;
            this.sldOpacitySimple.TickSpacing = 5F;
            this.ttHelp.SetToolTip(this.sldOpacitySimple, resources.GetString("sldOpacitySimple.ToolTip"));
            this.sldOpacitySimple.Value = 0D;
            // 
            // cmbStrokeStyle
            // 
            resources.ApplyResources(this.cmbStrokeStyle, "cmbStrokeStyle");
            this.cmbStrokeStyle.FormattingEnabled = true;
            this.cmbStrokeStyle.Items.AddRange(new object[] {
            resources.GetString("cmbStrokeStyle.Items"),
            resources.GetString("cmbStrokeStyle.Items1"),
            resources.GetString("cmbStrokeStyle.Items2"),
            resources.GetString("cmbStrokeStyle.Items3"),
            resources.GetString("cmbStrokeStyle.Items4"),
            resources.GetString("cmbStrokeStyle.Items5")});
            this.cmbStrokeStyle.Name = "cmbStrokeStyle";
            this.ttHelp.SetToolTip(this.cmbStrokeStyle, resources.GetString("cmbStrokeStyle.ToolTip"));
            this.cmbStrokeStyle.SelectedIndexChanged += new System.EventHandler(this.ComboBox1SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            this.ttHelp.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // dblWidth
            // 
            resources.ApplyResources(this.dblWidth, "dblWidth");
            this.dblWidth.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dblWidth.BackColorRegular = System.Drawing.Color.Empty;
            this.dblWidth.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.dblWidth.IsValid = true;
            this.dblWidth.Name = "dblWidth";
            this.dblWidth.NumberFormat = null;
            this.dblWidth.RegularHelp = "Enter a double precision floating point value.";
            this.ttHelp.SetToolTip(this.dblWidth, resources.GetString("dblWidth.ToolTip"));
            this.dblWidth.Value = 0D;
            this.dblWidth.TextChanged += new System.EventHandler(this.DblWidthTextChanged);
            // 
            // tabCartographic
            // 
            resources.ApplyResources(this.tabCartographic, "tabCartographic");
            this.tabCartographic.Controls.Add(this.lblColorCartographic);
            this.tabCartographic.Controls.Add(this.lblOpacityCartographic);
            this.tabCartographic.Controls.Add(this.grpCaps);
            this.tabCartographic.Controls.Add(this.cbColorCartographic);
            this.tabCartographic.Controls.Add(this.sldOpacityCartographic);
            this.tabCartographic.Controls.Add(this.dblStrokeOffset);
            this.tabCartographic.Controls.Add(this.radLineJoin);
            this.tabCartographic.Controls.Add(this.dblWidthCartographic);
            this.tabCartographic.Name = "tabCartographic";
            this.ttHelp.SetToolTip(this.tabCartographic, resources.GetString("tabCartographic.ToolTip"));
            this.tabCartographic.UseVisualStyleBackColor = true;
            // 
            // lblColorCartographic
            // 
            resources.ApplyResources(this.lblColorCartographic, "lblColorCartographic");
            this.lblColorCartographic.Name = "lblColorCartographic";
            this.ttHelp.SetToolTip(this.lblColorCartographic, resources.GetString("lblColorCartographic.ToolTip"));
            // 
            // lblOpacityCartographic
            // 
            resources.ApplyResources(this.lblOpacityCartographic, "lblOpacityCartographic");
            this.lblOpacityCartographic.Name = "lblOpacityCartographic";
            this.ttHelp.SetToolTip(this.lblOpacityCartographic, resources.GetString("lblOpacityCartographic.ToolTip"));
            // 
            // grpCaps
            // 
            resources.ApplyResources(this.grpCaps, "grpCaps");
            this.grpCaps.Controls.Add(this.lblStartCap);
            this.grpCaps.Controls.Add(this.cmbEndCap);
            this.grpCaps.Controls.Add(this.label4);
            this.grpCaps.Controls.Add(this.cmbStartCap);
            this.grpCaps.Name = "grpCaps";
            this.grpCaps.TabStop = false;
            this.ttHelp.SetToolTip(this.grpCaps, resources.GetString("grpCaps.ToolTip"));
            // 
            // lblStartCap
            // 
            resources.ApplyResources(this.lblStartCap, "lblStartCap");
            this.lblStartCap.Name = "lblStartCap";
            this.ttHelp.SetToolTip(this.lblStartCap, resources.GetString("lblStartCap.ToolTip"));
            // 
            // cmbEndCap
            // 
            resources.ApplyResources(this.cmbEndCap, "cmbEndCap");
            this.cmbEndCap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEndCap.FormattingEnabled = true;
            this.cmbEndCap.Name = "cmbEndCap";
            this.ttHelp.SetToolTip(this.cmbEndCap, resources.GetString("cmbEndCap.ToolTip"));
            this.cmbEndCap.SelectedIndexChanged += new System.EventHandler(this.CmbEndCapSelectedIndexChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            this.ttHelp.SetToolTip(this.label4, resources.GetString("label4.ToolTip"));
            // 
            // cmbStartCap
            // 
            resources.ApplyResources(this.cmbStartCap, "cmbStartCap");
            this.cmbStartCap.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStartCap.FormattingEnabled = true;
            this.cmbStartCap.Name = "cmbStartCap";
            this.ttHelp.SetToolTip(this.cmbStartCap, resources.GetString("cmbStartCap.ToolTip"));
            this.cmbStartCap.SelectedIndexChanged += new System.EventHandler(this.CmbStartCapSelectedIndexChanged);
            // 
            // cbColorCartographic
            // 
            resources.ApplyResources(this.cbColorCartographic, "cbColorCartographic");
            this.cbColorCartographic.BevelRadius = 6;
            this.cbColorCartographic.Color = System.Drawing.Color.Blue;
            this.cbColorCartographic.LaunchDialogOnClick = true;
            this.cbColorCartographic.Name = "cbColorCartographic";
            this.cbColorCartographic.RoundingRadius = 15;
            this.ttHelp.SetToolTip(this.cbColorCartographic, resources.GetString("cbColorCartographic.ToolTip"));
            // 
            // sldOpacityCartographic
            // 
            resources.ApplyResources(this.sldOpacityCartographic, "sldOpacityCartographic");
            this.sldOpacityCartographic.ColorButton = null;
            this.sldOpacityCartographic.FlipRamp = false;
            this.sldOpacityCartographic.FlipText = false;
            this.sldOpacityCartographic.InvertRamp = false;
            this.sldOpacityCartographic.Maximum = 1D;
            this.sldOpacityCartographic.MaximumColor = System.Drawing.Color.CornflowerBlue;
            this.sldOpacityCartographic.Minimum = 0D;
            this.sldOpacityCartographic.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sldOpacityCartographic.Name = "sldOpacityCartographic";
            this.sldOpacityCartographic.NumberFormat = null;
            this.sldOpacityCartographic.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.sldOpacityCartographic.RampRadius = 10F;
            this.sldOpacityCartographic.RampText = "Opacity";
            this.sldOpacityCartographic.RampTextAlignment = System.Drawing.ContentAlignment.BottomRight;
            this.sldOpacityCartographic.RampTextBehindRamp = true;
            this.sldOpacityCartographic.RampTextColor = System.Drawing.Color.Black;
            this.sldOpacityCartographic.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sldOpacityCartographic.ShowMaximum = true;
            this.sldOpacityCartographic.ShowMinimum = true;
            this.sldOpacityCartographic.ShowTicks = true;
            this.sldOpacityCartographic.ShowValue = true; // CGX false -> true
            this.sldOpacityCartographic.SliderColor = System.Drawing.Color.Blue;
            this.sldOpacityCartographic.SliderRadius = 4F;
            this.sldOpacityCartographic.TickColor = System.Drawing.Color.DarkGray;
            this.sldOpacityCartographic.TickSpacing = 5F;
            this.ttHelp.SetToolTip(this.sldOpacityCartographic, resources.GetString("sldOpacityCartographic.ToolTip"));
            this.sldOpacityCartographic.Value = 0D;
            // 
            // dblStrokeOffset
            // 
            resources.ApplyResources(this.dblStrokeOffset, "dblStrokeOffset");
            this.dblStrokeOffset.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dblStrokeOffset.BackColorRegular = System.Drawing.Color.Empty;
            this.dblStrokeOffset.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.dblStrokeOffset.IsValid = true;
            this.dblStrokeOffset.Name = "dblStrokeOffset";
            this.dblStrokeOffset.NumberFormat = null;
            this.dblStrokeOffset.RegularHelp = "Enter a double precision floating point value.";
            this.ttHelp.SetToolTip(this.dblStrokeOffset, resources.GetString("dblStrokeOffset.ToolTip"));
            this.dblStrokeOffset.Value = 0D;
            // 
            // radLineJoin
            // 
            resources.ApplyResources(this.radLineJoin, "radLineJoin");
            this.radLineJoin.Name = "radLineJoin";
            this.ttHelp.SetToolTip(this.radLineJoin, resources.GetString("radLineJoin.ToolTip"));
            this.radLineJoin.Value = DotSpatial.Symbology.LineJoinType.Round;
            this.radLineJoin.ValueChanged += new System.EventHandler(this.RadLineJoinValueChanged);
            // 
            // dblWidthCartographic
            // 
            resources.ApplyResources(this.dblWidthCartographic, "dblWidthCartographic");
            this.dblWidthCartographic.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dblWidthCartographic.BackColorRegular = System.Drawing.Color.Empty;
            this.dblWidthCartographic.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.dblWidthCartographic.IsValid = true;
            this.dblWidthCartographic.Name = "dblWidthCartographic";
            this.dblWidthCartographic.NumberFormat = null;
            this.dblWidthCartographic.RegularHelp = "Enter a double precision floating point value.";
            this.ttHelp.SetToolTip(this.dblWidthCartographic, resources.GetString("dblWidthCartographic.ToolTip"));
            this.dblWidthCartographic.Value = 0D;
            this.dblWidthCartographic.TextChanged += new System.EventHandler(this.DblWidthCartographicTextChanged);
            // 
            // tabDash
            // 
            resources.ApplyResources(this.tabDash, "tabDash");
            this.tabDash.Controls.Add(this.dashControl1);
            this.tabDash.Name = "tabDash";
            this.ttHelp.SetToolTip(this.tabDash, resources.GetString("tabDash.ToolTip"));
            this.tabDash.UseVisualStyleBackColor = true;
            // 
            // dashControl1
            // 
            resources.ApplyResources(this.dashControl1, "dashControl1");
            this.dashControl1.BlockSize = new System.Drawing.SizeF(10F, 10F);
            this.dashControl1.ButtonDownDarkColor = System.Drawing.SystemColors.ControlDark;
            this.dashControl1.ButtonDownLitColor = System.Drawing.SystemColors.ActiveCaption;
            this.dashControl1.ButtonUpDarkColor = System.Drawing.SystemColors.Control;
            this.dashControl1.ButtonUpLitColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.dashControl1.HorizontalSlider.Color = System.Drawing.Color.Blue;
            this.dashControl1.HorizontalSlider.Image = null;
            this.dashControl1.HorizontalSlider.Position = ((System.Drawing.PointF)(resources.GetObject("resource.Position")));
            this.dashControl1.HorizontalSlider.Size = new System.Drawing.SizeF(10F, 15F);
            this.dashControl1.HorizontalSlider.Visible = true;
            this.dashControl1.LineColor = System.Drawing.Color.Red;
            this.dashControl1.LineWidth = 0D;
            this.dashControl1.Name = "dashControl1";
            this.ttHelp.SetToolTip(this.dashControl1, resources.GetString("dashControl1.ToolTip"));
            this.dashControl1.VerticalSlider.Color = System.Drawing.Color.Lime;
            this.dashControl1.VerticalSlider.Image = null;
            this.dashControl1.VerticalSlider.Position = ((System.Drawing.PointF)(resources.GetObject("resource.Position1")));
            this.dashControl1.VerticalSlider.Size = new System.Drawing.SizeF(15F, 10F);
            this.dashControl1.VerticalSlider.Visible = true;
            // 
            // tabDecoration
            // 
            resources.ApplyResources(this.tabDecoration, "tabDecoration");
            this.tabDecoration.Controls.Add(this.lblPercentualPosition);
            this.tabDecoration.Controls.Add(this.nudPercentualPosition);
            this.tabDecoration.Controls.Add(this.btnEdit);
            this.tabDecoration.Controls.Add(this.label6);
            this.tabDecoration.Controls.Add(this.lblDecorationPreview);
            this.tabDecoration.Controls.Add(this.lblNumberOfPositions);
            this.tabDecoration.Controls.Add(this.nudDecorationCount);
            this.tabDecoration.Controls.Add(this.grpRotation);
            this.tabDecoration.Controls.Add(this.grpFlip);
            this.tabDecoration.Controls.Add(this.dblOffset);
            this.tabDecoration.Controls.Add(this.ccDecorations);
            this.tabDecoration.Name = "tabDecoration";
            this.ttHelp.SetToolTip(this.tabDecoration, resources.GetString("tabDecoration.ToolTip"));
            this.tabDecoration.UseVisualStyleBackColor = true;
            // 
            // lblPercentualPosition
            // 
            resources.ApplyResources(this.lblPercentualPosition, "lblPercentualPosition");
            this.lblPercentualPosition.Name = "lblPercentualPosition";
            this.ttHelp.SetToolTip(this.lblPercentualPosition, resources.GetString("lblPercentualPosition.ToolTip"));
            // 
            // nudPercentualPosition
            // 
            resources.ApplyResources(this.nudPercentualPosition, "nudPercentualPosition");
            this.nudPercentualPosition.Name = "nudPercentualPosition";
            this.ttHelp.SetToolTip(this.nudPercentualPosition, resources.GetString("nudPercentualPosition.ToolTip"));
            this.nudPercentualPosition.ValueChanged += new System.EventHandler(this.NudPercentualPositionValueChanged);
            // 
            // btnEdit
            // 
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.Name = "btnEdit";
            this.ttHelp.SetToolTip(this.btnEdit, resources.GetString("btnEdit.ToolTip"));
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.BtnEditClick);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            this.ttHelp.SetToolTip(this.label6, resources.GetString("label6.ToolTip"));
            // 
            // lblDecorationPreview
            // 
            resources.ApplyResources(this.lblDecorationPreview, "lblDecorationPreview");
            this.lblDecorationPreview.BackColor = System.Drawing.Color.White;
            this.lblDecorationPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDecorationPreview.Name = "lblDecorationPreview";
            this.ttHelp.SetToolTip(this.lblDecorationPreview, resources.GetString("lblDecorationPreview.ToolTip"));
            // 
            // lblNumberOfPositions
            // 
            resources.ApplyResources(this.lblNumberOfPositions, "lblNumberOfPositions");
            this.lblNumberOfPositions.Name = "lblNumberOfPositions";
            this.ttHelp.SetToolTip(this.lblNumberOfPositions, resources.GetString("lblNumberOfPositions.ToolTip"));
            // 
            // nudDecorationCount
            // 
            resources.ApplyResources(this.nudDecorationCount, "nudDecorationCount");
            this.nudDecorationCount.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudDecorationCount.Name = "nudDecorationCount";
            this.ttHelp.SetToolTip(this.nudDecorationCount, resources.GetString("nudDecorationCount.ToolTip"));
            this.nudDecorationCount.ValueChanged += new System.EventHandler(this.NudDecorationCountValueChanged);
            // 
            // grpRotation
            // 
            resources.ApplyResources(this.grpRotation, "grpRotation");
            this.grpRotation.Controls.Add(this.radRotationFixed);
            this.grpRotation.Controls.Add(this.radRotationWithLine);
            this.grpRotation.Name = "grpRotation";
            this.grpRotation.TabStop = false;
            this.ttHelp.SetToolTip(this.grpRotation, resources.GetString("grpRotation.ToolTip"));
            // 
            // radRotationFixed
            // 
            resources.ApplyResources(this.radRotationFixed, "radRotationFixed");
            this.radRotationFixed.Name = "radRotationFixed";
            this.radRotationFixed.TabStop = true;
            this.ttHelp.SetToolTip(this.radRotationFixed, resources.GetString("radRotationFixed.ToolTip"));
            this.radRotationFixed.UseVisualStyleBackColor = true;
            // 
            // radRotationWithLine
            // 
            resources.ApplyResources(this.radRotationWithLine, "radRotationWithLine");
            this.radRotationWithLine.Name = "radRotationWithLine";
            this.radRotationWithLine.TabStop = true;
            this.ttHelp.SetToolTip(this.radRotationWithLine, resources.GetString("radRotationWithLine.ToolTip"));
            this.radRotationWithLine.UseVisualStyleBackColor = true;
            this.radRotationWithLine.CheckedChanged += new System.EventHandler(this.RadRotationWithLineCheckedChanged);
            // 
            // grpFlip
            // 
            resources.ApplyResources(this.grpFlip, "grpFlip");
            this.grpFlip.Controls.Add(this.chkFlipFirst);
            this.grpFlip.Controls.Add(this.chkFlipAll);
            this.grpFlip.Name = "grpFlip";
            this.grpFlip.TabStop = false;
            this.ttHelp.SetToolTip(this.grpFlip, resources.GetString("grpFlip.ToolTip"));
            // 
            // chkFlipFirst
            // 
            resources.ApplyResources(this.chkFlipFirst, "chkFlipFirst");
            this.chkFlipFirst.Name = "chkFlipFirst";
            this.ttHelp.SetToolTip(this.chkFlipFirst, resources.GetString("chkFlipFirst.ToolTip"));
            this.chkFlipFirst.UseVisualStyleBackColor = true;
            this.chkFlipFirst.CheckedChanged += new System.EventHandler(this.ChkFlipFirstCheckedChanged);
            // 
            // chkFlipAll
            // 
            resources.ApplyResources(this.chkFlipAll, "chkFlipAll");
            this.chkFlipAll.Name = "chkFlipAll";
            this.ttHelp.SetToolTip(this.chkFlipAll, resources.GetString("chkFlipAll.ToolTip"));
            this.chkFlipAll.UseVisualStyleBackColor = true;
            this.chkFlipAll.CheckedChanged += new System.EventHandler(this.ChkFlipAllCheckedChanged);
            // 
            // dblOffset
            // 
            resources.ApplyResources(this.dblOffset, "dblOffset");
            this.dblOffset.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dblOffset.BackColorRegular = System.Drawing.Color.Empty;
            this.dblOffset.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.dblOffset.IsValid = true;
            this.dblOffset.Name = "dblOffset";
            this.dblOffset.NumberFormat = null;
            this.dblOffset.RegularHelp = "Enter a double precision floating point value.";
            this.ttHelp.SetToolTip(this.dblOffset, resources.GetString("dblOffset.ToolTip"));
            this.dblOffset.Value = 0D;
            // 
            // ccDecorations
            // 
            resources.ApplyResources(this.ccDecorations, "ccDecorations");
            this.ccDecorations.Name = "ccDecorations";
            this.ttHelp.SetToolTip(this.ccDecorations, resources.GetString("ccDecorations.ToolTip"));
            // 
            // ccStrokes
            // 
            resources.ApplyResources(this.ccStrokes, "ccStrokes");
            this.ccStrokes.Name = "ccStrokes";
            this.ttHelp.SetToolTip(this.ccStrokes, resources.GetString("ccStrokes.ToolTip"));
            // 
            // DetailedLineSymbolControl
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.ccStrokes);
            this.Controls.Add(this.tabStrokeProperties);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmbStrokeType);
            this.Name = "DetailedLineSymbolControl";
            this.ttHelp.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabStrokeProperties.ResumeLayout(false);
            this.tabSimple.ResumeLayout(false);
            this.tabSimple.PerformLayout();
            this.tabCartographic.ResumeLayout(false);
            this.tabCartographic.PerformLayout();
            this.grpCaps.ResumeLayout(false);
            this.grpCaps.PerformLayout();
            this.tabDash.ResumeLayout(false);
            this.tabDecoration.ResumeLayout(false);
            this.tabDecoration.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPercentualPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDecorationCount)).EndInit();
            this.grpRotation.ResumeLayout(false);
            this.grpRotation.PerformLayout();
            this.grpFlip.ResumeLayout(false);
            this.grpFlip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DashControl dashControl1;
        private DoubleBox dblOffset;
        private DoubleBox dblStrokeOffset;
        private DoubleBox dblWidth;
        private DoubleBox dblWidthCartographic;
        private GroupBox groupBox1;
        private GroupBox grpCaps;
        private GroupBox grpFlip;
        private GroupBox grpRotation;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label9;
        private Label lblColorCartographic;
        private Label lblDecorationPreview;
        private Label lblNumberOfPositions;
        private Label lblOpacityCartographic;
        private Label lblPreview;
        private Label lblScaleMode;
        private Label lblStartCap;
        private NumericUpDown nudDecorationCount;
        private LineJoinControl radLineJoin;
        private RadioButton radRotationFixed;
        private RadioButton radRotationWithLine;
        private RampSlider sldOpacityCartographic;
        private RampSlider sldOpacitySimple;
        private TabPage tabCartographic;
        private TabPage tabDash;
        private TabPage tabDecoration;
        private TabPage tabSimple;
        private TabControl tabStrokeProperties;
        private ToolTip ttHelp;
        private Button btnEdit;
        private ColorButton cbColorCartographic;
        private ColorButton cbColorSimple;
        private DecorationCollectionControl ccDecorations;
        private StrokeCollectionControl ccStrokes;
        private CheckBox chkFlipAll;
        private CheckBox chkFlipFirst;
        private CheckBox chkSmoothing;
        private ComboBox cmbEndCap;
        private ComboBox cmbScaleMode;
        private ComboBox cmbStartCap;
        private ComboBox cmbStrokeStyle;
        private ComboBox cmbStrokeType;
        private Label lblPercentualPosition;
        private NumericUpDown nudPercentualPosition;
    }
}
