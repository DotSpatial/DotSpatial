using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class LabelSetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LabelSetup));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lbCategories = new System.Windows.Forms.ListBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnCategoryDown = new System.Windows.Forms.Button();
            this.btnCategoryUp = new System.Windows.Forms.Button();
            this.btnSubtract = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblSymbolGroups = new System.Windows.Forms.Label();
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabExpression = new System.Windows.Forms.TabPage();
            this.sqlExpression = new DotSpatial.Symbology.Forms.ExpressionControl();
            this.tabBasic = new System.Windows.Forms.TabPage();
            this.tbFloatingFormat = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.grbLabelRotation = new System.Windows.Forms.GroupBox();
            this.rbLineBasedAngle = new System.Windows.Forms.RadioButton();
            this.cmbLineAngle = new System.Windows.Forms.ComboBox();
            this.cmbLabelAngleField = new System.Windows.Forms.ComboBox();
            this.rbIndividualAngle = new System.Windows.Forms.RadioButton();
            this.rbCommonAngle = new System.Windows.Forms.RadioButton();
            this.nudAngle = new System.Windows.Forms.NumericUpDown();
            this.chkPrioritizeLow = new System.Windows.Forms.CheckBox();
            this.chkPreventCollision = new System.Windows.Forms.CheckBox();
            this.lblPriorityField = new System.Windows.Forms.Label();
            this.cmbPriorityField = new System.Windows.Forms.ComboBox();
            this.gpbBorderColor = new System.Windows.Forms.GroupBox();
            this.chkBorder = new System.Windows.Forms.CheckBox();
            this.sldBorderOpacity = new DotSpatial.Symbology.Forms.RampSlider();
            this.cbBorderColor = new DotSpatial.Symbology.Forms.ColorButton();
            this.gpbFont = new System.Windows.Forms.GroupBox();
            this.lblFontColor = new System.Windows.Forms.Label();
            this.sldFontOpacity = new DotSpatial.Symbology.Forms.RampSlider();
            this.cbFontColor = new DotSpatial.Symbology.Forms.ColorButton();
            this.cmbStyle = new System.Windows.Forms.ComboBox();
            this.lblFamily = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbSize = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ffcFamilyName = new DotSpatial.Symbology.Forms.FontFamilyControl();
            this.lblPreview = new System.Windows.Forms.Label();
            this.gpbBackgroundColor = new System.Windows.Forms.GroupBox();
            this.sldBackgroundOpacity = new DotSpatial.Symbology.Forms.RampSlider();
            this.cbBackgroundColor = new DotSpatial.Symbology.Forms.ColorButton();
            this.chkBackgroundColor = new System.Windows.Forms.CheckBox();
            this.tabAdvanced = new System.Windows.Forms.TabPage();
            this.labelAlignmentControl1 = new DotSpatial.Symbology.Forms.LabelAlignmentControl();
            this.grpOffset = new System.Windows.Forms.GroupBox();
            this.nudYOffset = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.nudXOffset = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.chkHalo = new System.Windows.Forms.CheckBox();
            this.chkShadow = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbAlignment = new System.Windows.Forms.ComboBox();
            this.cmbLabelingMethod = new System.Windows.Forms.ComboBox();
            this.cmbLabelParts = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clrHalo = new DotSpatial.Symbology.Forms.ColorBox();
            this.gpbUseLabelShadow = new System.Windows.Forms.GroupBox();
            this.nudShadowOffsetY = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.nudShadowOffsetX = new System.Windows.Forms.NumericUpDown();
            this.sliderOpacityShadow = new DotSpatial.Symbology.Forms.RampSlider();
            this.label6 = new System.Windows.Forms.Label();
            this.colorButtonShadow = new DotSpatial.Symbology.Forms.ColorButton();
            this.tabMembers = new System.Windows.Forms.TabPage();
            this.sqlMembers = new DotSpatial.Symbology.Forms.SQLQueryControl();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdApply = new System.Windows.Forms.Button();
            this.lblHelp = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ttLabelSetup = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabs.SuspendLayout();
            this.tabExpression.SuspendLayout();
            this.tabBasic.SuspendLayout();
            this.grbLabelRotation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAngle)).BeginInit();
            this.gpbBorderColor.SuspendLayout();
            this.gpbFont.SuspendLayout();
            this.gpbBackgroundColor.SuspendLayout();
            this.tabAdvanced.SuspendLayout();
            this.grpOffset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudYOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXOffset)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.gpbUseLabelShadow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudShadowOffsetY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudShadowOffsetX)).BeginInit();
            this.tabMembers.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lbCategories);
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabs);
            // 
            // lbCategories
            // 
            resources.ApplyResources(this.lbCategories, "lbCategories");
            this.lbCategories.FormattingEnabled = true;
            this.lbCategories.Name = "lbCategories";
            this.lbCategories.SelectedIndexChanged += new System.EventHandler(this.lbCategories_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnCategoryDown);
            this.panel3.Controls.Add(this.btnCategoryUp);
            this.panel3.Controls.Add(this.btnSubtract);
            this.panel3.Controls.Add(this.btnAdd);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // btnCategoryDown
            // 
            this.btnCategoryDown.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.down;
            resources.ApplyResources(this.btnCategoryDown, "btnCategoryDown");
            this.btnCategoryDown.Name = "btnCategoryDown";
            this.btnCategoryDown.UseVisualStyleBackColor = true;
            this.btnCategoryDown.Click += new System.EventHandler(this.btnCategoryDown_Click);
            // 
            // btnCategoryUp
            // 
            this.btnCategoryUp.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.up;
            resources.ApplyResources(this.btnCategoryUp, "btnCategoryUp");
            this.btnCategoryUp.Name = "btnCategoryUp";
            this.btnCategoryUp.UseVisualStyleBackColor = true;
            this.btnCategoryUp.Click += new System.EventHandler(this.btnCategoryUp_Click);
            // 
            // btnSubtract
            // 
            this.btnSubtract.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.mnuLayerClear;
            resources.ApplyResources(this.btnSubtract, "btnSubtract");
            this.btnSubtract.Name = "btnSubtract";
            this.btnSubtract.UseVisualStyleBackColor = true;
            this.btnSubtract.Click += new System.EventHandler(this.btnSubtract_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.mnuLayerAdd;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblSymbolGroups);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // lblSymbolGroups
            // 
            resources.ApplyResources(this.lblSymbolGroups, "lblSymbolGroups");
            this.lblSymbolGroups.Name = "lblSymbolGroups";
            // 
            // tabs
            // 
            this.tabs.Controls.Add(this.tabExpression);
            this.tabs.Controls.Add(this.tabBasic);
            this.tabs.Controls.Add(this.tabAdvanced);
            this.tabs.Controls.Add(this.tabMembers);
            resources.ApplyResources(this.tabs, "tabs");
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            // 
            // tabExpression
            // 
            this.tabExpression.Controls.Add(this.sqlExpression);
            resources.ApplyResources(this.tabExpression, "tabExpression");
            this.tabExpression.Name = "tabExpression";
            this.tabExpression.UseVisualStyleBackColor = true;
            // 
            // sqlExpression
            // 
            this.sqlExpression.AttributeSource = null;
            resources.ApplyResources(this.sqlExpression, "sqlExpression");
            this.sqlExpression.ExpressionText = "";
            this.sqlExpression.Name = "sqlExpression";
            this.sqlExpression.Table = null;
            // 
            // tabBasic
            // 
            this.tabBasic.Controls.Add(this.tbFloatingFormat);
            this.tabBasic.Controls.Add(this.label13);
            this.tabBasic.Controls.Add(this.grbLabelRotation);
            this.tabBasic.Controls.Add(this.chkPrioritizeLow);
            this.tabBasic.Controls.Add(this.chkPreventCollision);
            this.tabBasic.Controls.Add(this.lblPriorityField);
            this.tabBasic.Controls.Add(this.cmbPriorityField);
            this.tabBasic.Controls.Add(this.gpbBorderColor);
            this.tabBasic.Controls.Add(this.gpbFont);
            this.tabBasic.Controls.Add(this.lblPreview);
            this.tabBasic.Controls.Add(this.gpbBackgroundColor);
            resources.ApplyResources(this.tabBasic, "tabBasic");
            this.tabBasic.Name = "tabBasic";
            this.tabBasic.UseVisualStyleBackColor = true;
            // 
            // tbFloatingFormat
            // 
            resources.ApplyResources(this.tbFloatingFormat, "tbFloatingFormat");
            this.tbFloatingFormat.Name = "tbFloatingFormat";
            this.tbFloatingFormat.TextChanged += new System.EventHandler(this.tbFloatingFormat_TextChanged);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // grbLabelRotation
            // 
            resources.ApplyResources(this.grbLabelRotation, "grbLabelRotation");
            this.grbLabelRotation.Controls.Add(this.rbLineBasedAngle);
            this.grbLabelRotation.Controls.Add(this.cmbLineAngle);
            this.grbLabelRotation.Controls.Add(this.cmbLabelAngleField);
            this.grbLabelRotation.Controls.Add(this.rbIndividualAngle);
            this.grbLabelRotation.Controls.Add(this.rbCommonAngle);
            this.grbLabelRotation.Controls.Add(this.nudAngle);
            this.grbLabelRotation.Name = "grbLabelRotation";
            this.grbLabelRotation.TabStop = false;
            // 
            // rbLineBasedAngle
            // 
            resources.ApplyResources(this.rbLineBasedAngle, "rbLineBasedAngle");
            this.rbLineBasedAngle.Name = "rbLineBasedAngle";
            this.rbLineBasedAngle.TabStop = true;
            this.rbLineBasedAngle.UseVisualStyleBackColor = true;
            this.rbLineBasedAngle.CheckedChanged += new System.EventHandler(this.rbLineBasedAngle_CheckedChanged);
            // 
            // cmbLineAngle
            // 
            this.cmbLineAngle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLineAngle.FormattingEnabled = true;
            resources.ApplyResources(this.cmbLineAngle, "cmbLineAngle");
            this.cmbLineAngle.Name = "cmbLineAngle";
            this.cmbLineAngle.SelectedIndexChanged += new System.EventHandler(this.cmbLineAngle_SelectedIndexChanged);
            // 
            // cmbLabelAngleField
            // 
            this.cmbLabelAngleField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLabelAngleField.FormattingEnabled = true;
            resources.ApplyResources(this.cmbLabelAngleField, "cmbLabelAngleField");
            this.cmbLabelAngleField.Name = "cmbLabelAngleField";
            this.ttLabelSetup.SetToolTip(this.cmbLabelAngleField, resources.GetString("cmbLabelAngleField.ToolTip"));
            this.cmbLabelAngleField.SelectedIndexChanged += new System.EventHandler(this.cmbLabelAngleField_SelectedIndexChanged);
            // 
            // rbIndividualAngle
            // 
            resources.ApplyResources(this.rbIndividualAngle, "rbIndividualAngle");
            this.rbIndividualAngle.Name = "rbIndividualAngle";
            this.rbIndividualAngle.TabStop = true;
            this.ttLabelSetup.SetToolTip(this.rbIndividualAngle, resources.GetString("rbIndividualAngle.ToolTip"));
            this.rbIndividualAngle.UseVisualStyleBackColor = true;
            this.rbIndividualAngle.CheckedChanged += new System.EventHandler(this.rbIndividualAngle_CheckedChanged);
            // 
            // rbCommonAngle
            // 
            resources.ApplyResources(this.rbCommonAngle, "rbCommonAngle");
            this.rbCommonAngle.Name = "rbCommonAngle";
            this.rbCommonAngle.TabStop = true;
            this.ttLabelSetup.SetToolTip(this.rbCommonAngle, resources.GetString("rbCommonAngle.ToolTip"));
            this.rbCommonAngle.UseVisualStyleBackColor = true;
            this.rbCommonAngle.CheckedChanged += new System.EventHandler(this.rbCommonAngle_CheckedChanged);
            // 
            // nudAngle
            // 
            resources.ApplyResources(this.nudAngle, "nudAngle");
            this.nudAngle.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.nudAngle.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
            this.nudAngle.Name = "nudAngle";
            this.ttLabelSetup.SetToolTip(this.nudAngle, resources.GetString("nudAngle.ToolTip"));
            this.nudAngle.ValueChanged += new System.EventHandler(this.nudAngle_ValueChanged);
            // 
            // chkPrioritizeLow
            // 
            resources.ApplyResources(this.chkPrioritizeLow, "chkPrioritizeLow");
            this.chkPrioritizeLow.Name = "chkPrioritizeLow";
            this.ttLabelSetup.SetToolTip(this.chkPrioritizeLow, resources.GetString("chkPrioritizeLow.ToolTip"));
            this.chkPrioritizeLow.UseVisualStyleBackColor = true;
            this.chkPrioritizeLow.CheckedChanged += new System.EventHandler(this.chkPrioritizeLow_CheckedChanged);
            // 
            // chkPreventCollision
            // 
            resources.ApplyResources(this.chkPreventCollision, "chkPreventCollision");
            this.chkPreventCollision.Checked = true;
            this.chkPreventCollision.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreventCollision.Name = "chkPreventCollision";
            this.ttLabelSetup.SetToolTip(this.chkPreventCollision, resources.GetString("chkPreventCollision.ToolTip"));
            this.chkPreventCollision.UseVisualStyleBackColor = true;
            this.chkPreventCollision.CheckedChanged += new System.EventHandler(this.chkPreventCollision_CheckedChanged);
            // 
            // lblPriorityField
            // 
            resources.ApplyResources(this.lblPriorityField, "lblPriorityField");
            this.lblPriorityField.Name = "lblPriorityField";
            // 
            // cmbPriorityField
            // 
            this.cmbPriorityField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPriorityField.FormattingEnabled = true;
            resources.ApplyResources(this.cmbPriorityField, "cmbPriorityField");
            this.cmbPriorityField.Name = "cmbPriorityField";
            this.ttLabelSetup.SetToolTip(this.cmbPriorityField, resources.GetString("cmbPriorityField.ToolTip"));
            this.cmbPriorityField.SelectedIndexChanged += new System.EventHandler(this.cmbPriorityField_SelectedIndexChanged);
            // 
            // gpbBorderColor
            // 
            this.gpbBorderColor.Controls.Add(this.chkBorder);
            this.gpbBorderColor.Controls.Add(this.sldBorderOpacity);
            this.gpbBorderColor.Controls.Add(this.cbBorderColor);
            resources.ApplyResources(this.gpbBorderColor, "gpbBorderColor");
            this.gpbBorderColor.Name = "gpbBorderColor";
            this.gpbBorderColor.TabStop = false;
            // 
            // chkBorder
            // 
            resources.ApplyResources(this.chkBorder, "chkBorder");
            this.chkBorder.Checked = true;
            this.chkBorder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBorder.Name = "chkBorder";
            this.chkBorder.UseVisualStyleBackColor = true;
            this.chkBorder.CheckedChanged += new System.EventHandler(this.chkBorder_CheckedChanged);
            // 
            // sldBorderOpacity
            // 
            this.sldBorderOpacity.ColorButton = this.cbBorderColor;
            this.sldBorderOpacity.FlipRamp = false;
            this.sldBorderOpacity.FlipText = false;
            this.sldBorderOpacity.InvertRamp = false;
            resources.ApplyResources(this.sldBorderOpacity, "sldBorderOpacity");
            this.sldBorderOpacity.Maximum = 1D;
            this.sldBorderOpacity.MaximumColor = System.Drawing.Color.Green;
            this.sldBorderOpacity.Minimum = 0D;
            this.sldBorderOpacity.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sldBorderOpacity.Name = "sldBorderOpacity";
            this.sldBorderOpacity.NumberFormat = null;
            this.sldBorderOpacity.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.sldBorderOpacity.RampRadius = 10F;
            this.sldBorderOpacity.RampText = "Opacity";
            this.sldBorderOpacity.RampTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.sldBorderOpacity.RampTextBehindRamp = true;
            this.sldBorderOpacity.RampTextColor = System.Drawing.Color.Black;
            this.sldBorderOpacity.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sldBorderOpacity.ShowMaximum = true;
            this.sldBorderOpacity.ShowMinimum = true;
            this.sldBorderOpacity.ShowTicks = false;
            this.sldBorderOpacity.ShowValue = false;
            this.sldBorderOpacity.SliderColor = System.Drawing.Color.Blue;
            this.sldBorderOpacity.SliderRadius = 4F;
            this.sldBorderOpacity.TickColor = System.Drawing.Color.DarkGray;
            this.sldBorderOpacity.TickSpacing = 5F;
            this.sldBorderOpacity.Value = 1D;
            // 
            // cbBorderColor
            // 
            this.cbBorderColor.BevelRadius = 2;
            this.cbBorderColor.Color = System.Drawing.Color.Blue;
            this.cbBorderColor.LaunchDialogOnClick = true;
            resources.ApplyResources(this.cbBorderColor, "cbBorderColor");
            this.cbBorderColor.Name = "cbBorderColor";
            this.cbBorderColor.RoundingRadius = 4;
            this.cbBorderColor.ColorChanged += new System.EventHandler(this.cbBorderColor_ColorChanged);
            // 
            // gpbFont
            // 
            resources.ApplyResources(this.gpbFont, "gpbFont");
            this.gpbFont.Controls.Add(this.lblFontColor);
            this.gpbFont.Controls.Add(this.sldFontOpacity);
            this.gpbFont.Controls.Add(this.cbFontColor);
            this.gpbFont.Controls.Add(this.cmbStyle);
            this.gpbFont.Controls.Add(this.lblFamily);
            this.gpbFont.Controls.Add(this.label2);
            this.gpbFont.Controls.Add(this.cmbSize);
            this.gpbFont.Controls.Add(this.label1);
            this.gpbFont.Controls.Add(this.ffcFamilyName);
            this.gpbFont.Name = "gpbFont";
            this.gpbFont.TabStop = false;
            // 
            // lblFontColor
            // 
            resources.ApplyResources(this.lblFontColor, "lblFontColor");
            this.lblFontColor.Name = "lblFontColor";
            // 
            // sldFontOpacity
            // 
            resources.ApplyResources(this.sldFontOpacity, "sldFontOpacity");
            this.sldFontOpacity.ColorButton = this.cbFontColor;
            this.sldFontOpacity.FlipRamp = false;
            this.sldFontOpacity.FlipText = false;
            this.sldFontOpacity.InvertRamp = false;
            this.sldFontOpacity.Maximum = 1D;
            this.sldFontOpacity.MaximumColor = System.Drawing.Color.Green;
            this.sldFontOpacity.Minimum = 0D;
            this.sldFontOpacity.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sldFontOpacity.Name = "sldFontOpacity";
            this.sldFontOpacity.NumberFormat = null;
            this.sldFontOpacity.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.sldFontOpacity.RampRadius = 10F;
            this.sldFontOpacity.RampText = "Opacity";
            this.sldFontOpacity.RampTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.sldFontOpacity.RampTextBehindRamp = true;
            this.sldFontOpacity.RampTextColor = System.Drawing.Color.Black;
            this.sldFontOpacity.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sldFontOpacity.ShowMaximum = true;
            this.sldFontOpacity.ShowMinimum = true;
            this.sldFontOpacity.ShowTicks = false;
            this.sldFontOpacity.ShowValue = false;
            this.sldFontOpacity.SliderColor = System.Drawing.Color.Blue;
            this.sldFontOpacity.SliderRadius = 4F;
            this.sldFontOpacity.TickColor = System.Drawing.Color.DarkGray;
            this.sldFontOpacity.TickSpacing = 5F;
            this.sldFontOpacity.Value = 1D;
            // 
            // cbFontColor
            // 
            this.cbFontColor.BevelRadius = 2;
            this.cbFontColor.Color = System.Drawing.Color.Blue;
            this.cbFontColor.LaunchDialogOnClick = true;
            resources.ApplyResources(this.cbFontColor, "cbFontColor");
            this.cbFontColor.Name = "cbFontColor";
            this.cbFontColor.RoundingRadius = 4;
            this.cbFontColor.ColorChanged += new System.EventHandler(this.cbFontColor_ColorChanged);
            // 
            // cmbStyle
            // 
            this.cmbStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStyle.FormattingEnabled = true;
            resources.ApplyResources(this.cmbStyle, "cmbStyle");
            this.cmbStyle.Name = "cmbStyle";
            this.cmbStyle.SelectedIndexChanged += new System.EventHandler(this.cmbStyle_SelectedIndexChanged);
            // 
            // lblFamily
            // 
            resources.ApplyResources(this.lblFamily, "lblFamily");
            this.lblFamily.Name = "lblFamily";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbSize
            // 
            this.cmbSize.FormattingEnabled = true;
            this.cmbSize.Items.AddRange(new object[] {
            resources.GetString("cmbSize.Items"),
            resources.GetString("cmbSize.Items1"),
            resources.GetString("cmbSize.Items2"),
            resources.GetString("cmbSize.Items3"),
            resources.GetString("cmbSize.Items4"),
            resources.GetString("cmbSize.Items5"),
            resources.GetString("cmbSize.Items6"),
            resources.GetString("cmbSize.Items7"),
            resources.GetString("cmbSize.Items8"),
            resources.GetString("cmbSize.Items9"),
            resources.GetString("cmbSize.Items10"),
            resources.GetString("cmbSize.Items11"),
            resources.GetString("cmbSize.Items12"),
            resources.GetString("cmbSize.Items13"),
            resources.GetString("cmbSize.Items14"),
            resources.GetString("cmbSize.Items15"),
            resources.GetString("cmbSize.Items16")});
            resources.ApplyResources(this.cmbSize, "cmbSize");
            this.cmbSize.Name = "cmbSize";
            this.cmbSize.SelectedIndexChanged += new System.EventHandler(this.cmbSize_SelectedIndexChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // ffcFamilyName
            // 
            resources.ApplyResources(this.ffcFamilyName, "ffcFamilyName");
            this.ffcFamilyName.Name = "ffcFamilyName";
            this.ffcFamilyName.SelectedItemChanged += new System.EventHandler(this.fontFamilyControl1_SelectedItemChanged);
            // 
            // lblPreview
            // 
            this.lblPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lblPreview, "lblPreview");
            this.lblPreview.Name = "lblPreview";
            // 
            // gpbBackgroundColor
            // 
            this.gpbBackgroundColor.Controls.Add(this.sldBackgroundOpacity);
            this.gpbBackgroundColor.Controls.Add(this.cbBackgroundColor);
            this.gpbBackgroundColor.Controls.Add(this.chkBackgroundColor);
            resources.ApplyResources(this.gpbBackgroundColor, "gpbBackgroundColor");
            this.gpbBackgroundColor.Name = "gpbBackgroundColor";
            this.gpbBackgroundColor.TabStop = false;
            // 
            // sldBackgroundOpacity
            // 
            this.sldBackgroundOpacity.ColorButton = this.cbBackgroundColor;
            this.sldBackgroundOpacity.FlipRamp = false;
            this.sldBackgroundOpacity.FlipText = false;
            this.sldBackgroundOpacity.InvertRamp = false;
            resources.ApplyResources(this.sldBackgroundOpacity, "sldBackgroundOpacity");
            this.sldBackgroundOpacity.Maximum = 1D;
            this.sldBackgroundOpacity.MaximumColor = System.Drawing.Color.Green;
            this.sldBackgroundOpacity.Minimum = 0D;
            this.sldBackgroundOpacity.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sldBackgroundOpacity.Name = "sldBackgroundOpacity";
            this.sldBackgroundOpacity.NumberFormat = null;
            this.sldBackgroundOpacity.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.sldBackgroundOpacity.RampRadius = 10F;
            this.sldBackgroundOpacity.RampText = "Opacity";
            this.sldBackgroundOpacity.RampTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.sldBackgroundOpacity.RampTextBehindRamp = true;
            this.sldBackgroundOpacity.RampTextColor = System.Drawing.Color.Black;
            this.sldBackgroundOpacity.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sldBackgroundOpacity.ShowMaximum = true;
            this.sldBackgroundOpacity.ShowMinimum = true;
            this.sldBackgroundOpacity.ShowTicks = false;
            this.sldBackgroundOpacity.ShowValue = false;
            this.sldBackgroundOpacity.SliderColor = System.Drawing.Color.Blue;
            this.sldBackgroundOpacity.SliderRadius = 4F;
            this.sldBackgroundOpacity.TickColor = System.Drawing.Color.DarkGray;
            this.sldBackgroundOpacity.TickSpacing = 5F;
            this.sldBackgroundOpacity.Value = 1D;
            // 
            // cbBackgroundColor
            // 
            this.cbBackgroundColor.BevelRadius = 2;
            this.cbBackgroundColor.Color = System.Drawing.Color.Blue;
            this.cbBackgroundColor.LaunchDialogOnClick = true;
            resources.ApplyResources(this.cbBackgroundColor, "cbBackgroundColor");
            this.cbBackgroundColor.Name = "cbBackgroundColor";
            this.cbBackgroundColor.RoundingRadius = 4;
            this.cbBackgroundColor.ColorChanged += new System.EventHandler(this.cbBackgroundColor_ColorChanged);
            // 
            // chkBackgroundColor
            // 
            resources.ApplyResources(this.chkBackgroundColor, "chkBackgroundColor");
            this.chkBackgroundColor.Checked = true;
            this.chkBackgroundColor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBackgroundColor.Name = "chkBackgroundColor";
            this.chkBackgroundColor.UseVisualStyleBackColor = true;
            this.chkBackgroundColor.CheckedChanged += new System.EventHandler(this.chkBackgroundColor_CheckedChanged);
            // 
            // tabAdvanced
            // 
            this.tabAdvanced.Controls.Add(this.labelAlignmentControl1);
            this.tabAdvanced.Controls.Add(this.grpOffset);
            this.tabAdvanced.Controls.Add(this.label10);
            this.tabAdvanced.Controls.Add(this.chkHalo);
            this.tabAdvanced.Controls.Add(this.chkShadow);
            this.tabAdvanced.Controls.Add(this.label5);
            this.tabAdvanced.Controls.Add(this.label4);
            this.tabAdvanced.Controls.Add(this.label3);
            this.tabAdvanced.Controls.Add(this.cmbAlignment);
            this.tabAdvanced.Controls.Add(this.cmbLabelingMethod);
            this.tabAdvanced.Controls.Add(this.cmbLabelParts);
            this.tabAdvanced.Controls.Add(this.groupBox1);
            this.tabAdvanced.Controls.Add(this.gpbUseLabelShadow);
            resources.ApplyResources(this.tabAdvanced, "tabAdvanced");
            this.tabAdvanced.Name = "tabAdvanced";
            this.tabAdvanced.UseVisualStyleBackColor = true;
            // 
            // labelAlignmentControl1
            // 
            this.labelAlignmentControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.labelAlignmentControl1, "labelAlignmentControl1");
            this.labelAlignmentControl1.Name = "labelAlignmentControl1";
            this.labelAlignmentControl1.Value = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelAlignmentControl1.ValueChanged += new System.EventHandler(this.labelAlignmentControl1_ValueChanged);
            // 
            // grpOffset
            // 
            resources.ApplyResources(this.grpOffset, "grpOffset");
            this.grpOffset.Controls.Add(this.nudYOffset);
            this.grpOffset.Controls.Add(this.label11);
            this.grpOffset.Controls.Add(this.label12);
            this.grpOffset.Controls.Add(this.nudXOffset);
            this.grpOffset.Name = "grpOffset";
            this.grpOffset.TabStop = false;
            // 
            // nudYOffset
            // 
            this.nudYOffset.DecimalPlaces = 2;
            resources.ApplyResources(this.nudYOffset, "nudYOffset");
            this.nudYOffset.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.nudYOffset.Minimum = new decimal(new int[] {
            256,
            0,
            0,
            -2147483648});
            this.nudYOffset.Name = "nudYOffset";
            this.nudYOffset.ValueChanged += new System.EventHandler(this.nudYOffset_ValueChanged);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // nudXOffset
            // 
            this.nudXOffset.DecimalPlaces = 2;
            resources.ApplyResources(this.nudXOffset, "nudXOffset");
            this.nudXOffset.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.nudXOffset.Minimum = new decimal(new int[] {
            256,
            0,
            0,
            -2147483648});
            this.nudXOffset.Name = "nudXOffset";
            this.nudXOffset.ValueChanged += new System.EventHandler(this.nudXOffset_ValueChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // chkHalo
            // 
            resources.ApplyResources(this.chkHalo, "chkHalo");
            this.chkHalo.Checked = true;
            this.chkHalo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHalo.Name = "chkHalo";
            this.chkHalo.UseVisualStyleBackColor = true;
            this.chkHalo.CheckedChanged += new System.EventHandler(this.chkHalo_CheckedChanged);
            // 
            // chkShadow
            // 
            resources.ApplyResources(this.chkShadow, "chkShadow");
            this.chkShadow.Checked = true;
            this.chkShadow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShadow.Name = "chkShadow";
            this.chkShadow.UseVisualStyleBackColor = true;
            this.chkShadow.CheckedChanged += new System.EventHandler(this.chkShadow_CheckedChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // cmbAlignment
            // 
            resources.ApplyResources(this.cmbAlignment, "cmbAlignment");
            this.cmbAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAlignment.FormattingEnabled = true;
            this.cmbAlignment.Items.AddRange(new object[] {
            resources.GetString("cmbAlignment.Items"),
            resources.GetString("cmbAlignment.Items1"),
            resources.GetString("cmbAlignment.Items2")});
            this.cmbAlignment.Name = "cmbAlignment";
            this.cmbAlignment.SelectedIndexChanged += new System.EventHandler(this.cmbAlignment_SelectedIndexChanged);
            // 
            // cmbLabelingMethod
            // 
            resources.ApplyResources(this.cmbLabelingMethod, "cmbLabelingMethod");
            this.cmbLabelingMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLabelingMethod.FormattingEnabled = true;
            this.cmbLabelingMethod.Name = "cmbLabelingMethod";
            this.cmbLabelingMethod.SelectedIndexChanged += new System.EventHandler(this.cmbLabelingMethod_SelectedIndexChanged);
            // 
            // cmbLabelParts
            // 
            resources.ApplyResources(this.cmbLabelParts, "cmbLabelParts");
            this.cmbLabelParts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLabelParts.FormattingEnabled = true;
            this.cmbLabelParts.Name = "cmbLabelParts";
            this.cmbLabelParts.SelectedIndexChanged += new System.EventHandler(this.cmbLabelParts_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.clrHalo);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // clrHalo
            // 
            resources.ApplyResources(this.clrHalo, "clrHalo");
            this.clrHalo.LabelText = "Halo Color:";
            this.clrHalo.Name = "clrHalo";
            this.clrHalo.Value = System.Drawing.Color.Empty;
            this.clrHalo.SelectedItemChanged += new System.EventHandler(this.clrHalo_SelectedItemChanged);
            // 
            // gpbUseLabelShadow
            // 
            resources.ApplyResources(this.gpbUseLabelShadow, "gpbUseLabelShadow");
            this.gpbUseLabelShadow.Controls.Add(this.nudShadowOffsetY);
            this.gpbUseLabelShadow.Controls.Add(this.label9);
            this.gpbUseLabelShadow.Controls.Add(this.label8);
            this.gpbUseLabelShadow.Controls.Add(this.label7);
            this.gpbUseLabelShadow.Controls.Add(this.nudShadowOffsetX);
            this.gpbUseLabelShadow.Controls.Add(this.sliderOpacityShadow);
            this.gpbUseLabelShadow.Controls.Add(this.label6);
            this.gpbUseLabelShadow.Controls.Add(this.colorButtonShadow);
            this.gpbUseLabelShadow.Name = "gpbUseLabelShadow";
            this.gpbUseLabelShadow.TabStop = false;
            // 
            // nudShadowOffsetY
            // 
            this.nudShadowOffsetY.DecimalPlaces = 2;
            this.nudShadowOffsetY.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            resources.ApplyResources(this.nudShadowOffsetY, "nudShadowOffsetY");
            this.nudShadowOffsetY.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.nudShadowOffsetY.Minimum = new decimal(new int[] {
            256,
            0,
            0,
            -2147483648});
            this.nudShadowOffsetY.Name = "nudShadowOffsetY";
            this.nudShadowOffsetY.ValueChanged += new System.EventHandler(this.UpDownShadowOffsetY_ValueChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // nudShadowOffsetX
            // 
            this.nudShadowOffsetX.DecimalPlaces = 2;
            this.nudShadowOffsetX.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            resources.ApplyResources(this.nudShadowOffsetX, "nudShadowOffsetX");
            this.nudShadowOffsetX.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.nudShadowOffsetX.Minimum = new decimal(new int[] {
            256,
            0,
            0,
            -2147483648});
            this.nudShadowOffsetX.Name = "nudShadowOffsetX";
            this.nudShadowOffsetX.ValueChanged += new System.EventHandler(this.UpDownShadowOffsetX_ValueChanged);
            // 
            // sliderOpacityShadow
            // 
            this.sliderOpacityShadow.ColorButton = null;
            this.sliderOpacityShadow.FlipRamp = false;
            this.sliderOpacityShadow.FlipText = false;
            this.sliderOpacityShadow.InvertRamp = false;
            resources.ApplyResources(this.sliderOpacityShadow, "sliderOpacityShadow");
            this.sliderOpacityShadow.Maximum = 1D;
            this.sliderOpacityShadow.MaximumColor = System.Drawing.Color.CornflowerBlue;
            this.sliderOpacityShadow.Minimum = 0D;
            this.sliderOpacityShadow.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sliderOpacityShadow.Name = "sliderOpacityShadow";
            this.sliderOpacityShadow.NumberFormat = null;
            this.sliderOpacityShadow.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.sliderOpacityShadow.RampRadius = 8F;
            this.sliderOpacityShadow.RampText = "Opacity";
            this.sliderOpacityShadow.RampTextAlignment = System.Drawing.ContentAlignment.BottomCenter;
            this.sliderOpacityShadow.RampTextBehindRamp = true;
            this.sliderOpacityShadow.RampTextColor = System.Drawing.Color.Black;
            this.sliderOpacityShadow.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sliderOpacityShadow.ShowMaximum = true;
            this.sliderOpacityShadow.ShowMinimum = true;
            this.sliderOpacityShadow.ShowTicks = true;
            this.sliderOpacityShadow.ShowValue = false;
            this.sliderOpacityShadow.SliderColor = System.Drawing.Color.Tan;
            this.sliderOpacityShadow.SliderRadius = 4F;
            this.sliderOpacityShadow.TickColor = System.Drawing.Color.DarkGray;
            this.sliderOpacityShadow.TickSpacing = 5F;
            this.sliderOpacityShadow.Value = 0D;
            this.sliderOpacityShadow.ValueChanged += new System.EventHandler(this.sliderOpacityShadow_ValueChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // colorButtonShadow
            // 
            this.colorButtonShadow.BevelRadius = 4;
            this.colorButtonShadow.Color = System.Drawing.Color.Blue;
            this.colorButtonShadow.LaunchDialogOnClick = true;
            resources.ApplyResources(this.colorButtonShadow, "colorButtonShadow");
            this.colorButtonShadow.Name = "colorButtonShadow";
            this.colorButtonShadow.RoundingRadius = 10;
            this.colorButtonShadow.ColorChanged += new System.EventHandler(this.colorButtonShadow_ColorChanged);
            // 
            // tabMembers
            // 
            this.tabMembers.Controls.Add(this.sqlMembers);
            resources.ApplyResources(this.tabMembers, "tabMembers");
            this.tabMembers.Name = "tabMembers";
            this.tabMembers.UseVisualStyleBackColor = true;
            // 
            // sqlMembers
            // 
            this.sqlMembers.AttributeSource = null;
            resources.ApplyResources(this.sqlMembers, "sqlMembers");
            this.sqlMembers.ExpressionText = "";
            this.sqlMembers.Name = "sqlMembers";
            this.sqlMembers.Table = null;
            this.sqlMembers.ExpressionTextChanged += new System.EventHandler(this.sqlMembers_ExpressionTextChanged);
            // 
            // cmdOK
            // 
            resources.ApplyResources(this.cmdOK, "cmdOK");
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            resources.ApplyResources(this.cmdCancel, "cmdCancel");
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdApply
            // 
            resources.ApplyResources(this.cmdApply, "cmdApply");
            this.cmdApply.Name = "cmdApply";
            this.cmdApply.UseVisualStyleBackColor = true;
            this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
            // 
            // lblHelp
            // 
            resources.ApplyResources(this.lblHelp, "lblHelp");
            this.lblHelp.Name = "lblHelp";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmdCancel);
            this.panel1.Controls.Add(this.cmdOK);
            this.panel1.Controls.Add(this.cmdApply);
            this.panel1.Controls.Add(this.lblHelp);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // LabelSetup
            // 
            this.AcceptButton = this.cmdOK;
            this.CancelButton = this.cmdCancel;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LabelSetup";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.tabExpression.ResumeLayout(false);
            this.tabBasic.ResumeLayout(false);
            this.tabBasic.PerformLayout();
            this.grbLabelRotation.ResumeLayout(false);
            this.grbLabelRotation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAngle)).EndInit();
            this.gpbBorderColor.ResumeLayout(false);
            this.gpbBorderColor.PerformLayout();
            this.gpbFont.ResumeLayout(false);
            this.gpbFont.PerformLayout();
            this.gpbBackgroundColor.ResumeLayout(false);
            this.gpbBackgroundColor.PerformLayout();
            this.tabAdvanced.ResumeLayout(false);
            this.tabAdvanced.PerformLayout();
            this.grpOffset.ResumeLayout(false);
            this.grpOffset.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudYOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudXOffset)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.gpbUseLabelShadow.ResumeLayout(false);
            this.gpbUseLabelShadow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudShadowOffsetY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudShadowOffsetX)).EndInit();
            this.tabMembers.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnAdd;
        private Button btnCategoryDown;
        private Button btnCategoryUp;
        private Button btnSubtract;
        private ColorButton cbBackgroundColor;
        private ColorButton cbBorderColor;
        private ColorButton cbFontColor;
        private CheckBox chkBackgroundColor;
        private CheckBox chkBorder;
        private CheckBox chkHalo;
        private CheckBox chkPreventCollision;
        private CheckBox chkPrioritizeLow;
        private CheckBox chkShadow;
        private ColorBox clrHalo;
        private ComboBox cmbAlignment;
        private ComboBox cmbLabelParts;
        private ComboBox cmbLabelingMethod;
        private ComboBox cmbPriorityField;
        private ComboBox cmbSize;
        private ComboBox cmbStyle;
        private Button cmdApply;
        private Button cmdCancel;
        private Button cmdOK;
        private ColorButton colorButtonShadow;
        private FontFamilyControl ffcFamilyName;
        private GroupBox gpbBackgroundColor;
        private GroupBox gpbBorderColor;
        private GroupBox gpbFont;

        private GroupBox gpbUseLabelShadow;
        private GroupBox groupBox1;
        private GroupBox grpOffset;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label12;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private LabelAlignmentControl labelAlignmentControl1;
        private ListBox lbCategories;
        private Label lblFamily;
        private Label lblFontColor;
        private Label lblHelp;
        private Label lblPreview;
        private Label lblPriorityField;
        private Label lblSymbolGroups;
        private NumericUpDown nudShadowOffsetX;
        private NumericUpDown nudShadowOffsetY;
        private NumericUpDown nudXOffset;
        private NumericUpDown nudYOffset;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private RampSlider sldBackgroundOpacity;
        private RampSlider sldBorderOpacity;
        private RampSlider sldFontOpacity;
        private RampSlider sliderOpacityShadow;
        private SplitContainer splitContainer1;
        private SQLQueryControl sqlMembers;
        private TabPage tabAdvanced;
        private TabPage tabBasic;
        private TabPage tabExpression;
        private TabPage tabMembers;
        private TabControl tabs;
        private NumericUpDown nudAngle;
        private GroupBox grbLabelRotation;
        private RadioButton rbIndividualAngle;
        private RadioButton rbCommonAngle;
        private ComboBox cmbLabelAngleField;
        private ToolTip ttLabelSetup;
        private TextBox tbFloatingFormat;
        private Label label13;
        private ExpressionControl sqlExpression;
        private RadioButton rbLineBasedAngle;
        private ComboBox cmbLineAngle;
    }
}
