using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    partial class RasterCategoryControl
    {
        private IContainer components = null;

        /// <summary>
        /// Handles disposing unmanaged memory
        /// </summary>
        /// <param name="disposing">The disposed item</param>
        protected override void Dispose(bool disposing)
        {
            breakSliderGraph1.Dispose();
            if (components != null && disposing)
            {
                foreach (var control in components.Components)
                {
                    var id = control as IDisposable;
                    if (id != null)
                        id.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RasterCategoryControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.btnQuick = new System.Windows.Forms.Button();
            this.btnRamp = new System.Windows.Forms.Button();
            this.cmdRefresh = new System.Windows.Forms.Button();
            this.tabScheme = new System.Windows.Forms.TabControl();
            this.tabStatistics = new System.Windows.Forms.TabPage();
            this.nudSigFig = new System.Windows.Forms.NumericUpDown();
            this.lblSigFig = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbIntervalSnapping = new System.Windows.Forms.ComboBox();
            this.dgvStatistics = new System.Windows.Forms.DataGridView();
            this.Stat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbInterval = new System.Windows.Forms.ComboBox();
            this.nudCategoryCount = new System.Windows.Forms.NumericUpDown();
            this.lblBreaks = new System.Windows.Forms.Label();
            this.tabGraph = new System.Windows.Forms.TabPage();
            this.lblColumns = new System.Windows.Forms.Label();
            this.nudColumns = new System.Windows.Forms.NumericUpDown();
            this.chkLog = new System.Windows.Forms.CheckBox();
            this.chkShowStd = new System.Windows.Forms.CheckBox();
            this.chkShowMean = new System.Windows.Forms.CheckBox();
            this.dgvCategories = new System.Windows.Forms.DataGridView();
            this.colSymbol = new System.Windows.Forms.DataGridViewImageColumn();
            this.colValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLegendText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkHillshade = new System.Windows.Forms.CheckBox();
            this.btnShadedRelief = new System.Windows.Forms.Button();
            this.grpHillshade = new System.Windows.Forms.GroupBox();
            this.btnElevation = new System.Windows.Forms.Button();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.opacityNoData = new DotSpatial.Symbology.Forms.RampSlider();
            this.colorNoData = new DotSpatial.Symbology.Forms.ColorButton();
            this.sldSchemeOpacity = new DotSpatial.Symbology.Forms.RampSlider();
            this.angLightDirection = new DotSpatial.Symbology.Forms.AngleControl();
            this.dbxElevationFactor = new DotSpatial.Symbology.Forms.DoubleBox();
            this.mwProgressBar1 = new DotSpatial.Symbology.Forms.SymbologyProgressBar();
            this.tccColorRange = new DotSpatial.Symbology.Forms.TabColorControl();
            this.dbxMax = new DotSpatial.Symbology.Forms.DoubleBox();
            this.dbxMin = new DotSpatial.Symbology.Forms.DoubleBox();
            this.breakSliderGraph1 = new DotSpatial.Symbology.Forms.BreakSliderGraph();
            this.tabScheme.SuspendLayout();
            this.tabStatistics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSigFig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistics)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCategoryCount)).BeginInit();
            this.tabGraph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategories)).BeginInit();
            this.grpHillshade.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnQuick
            // 
            resources.ApplyResources(this.btnQuick, "btnQuick");
            this.btnQuick.Name = "btnQuick";
            this.ttHelp.SetToolTip(this.btnQuick, resources.GetString("btnQuick.ToolTip"));
            this.btnQuick.UseVisualStyleBackColor = true;
            this.btnQuick.Click += new System.EventHandler(this.btnQuick_Click);
            // 
            // btnRamp
            // 
            resources.ApplyResources(this.btnRamp, "btnRamp");
            this.btnRamp.Name = "btnRamp";
            this.ttHelp.SetToolTip(this.btnRamp, resources.GetString("btnRamp.ToolTip"));
            this.btnRamp.UseVisualStyleBackColor = true;
            this.btnRamp.Click += new System.EventHandler(this.btnRamp_Click);
            // 
            // cmdRefresh
            // 
            resources.ApplyResources(this.cmdRefresh, "cmdRefresh");
            this.cmdRefresh.Name = "cmdRefresh";
            this.ttHelp.SetToolTip(this.cmdRefresh, resources.GetString("cmdRefresh.ToolTip"));
            this.cmdRefresh.UseVisualStyleBackColor = true;
            this.cmdRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
            // 
            // tabScheme
            // 
            this.tabScheme.Controls.Add(this.tabStatistics);
            this.tabScheme.Controls.Add(this.tabGraph);
            resources.ApplyResources(this.tabScheme, "tabScheme");
            this.tabScheme.Name = "tabScheme";
            this.tabScheme.SelectedIndex = 0;
            // 
            // tabStatistics
            // 
            this.tabStatistics.Controls.Add(this.dbxMax);
            this.tabStatistics.Controls.Add(this.dbxMin);
            this.tabStatistics.Controls.Add(this.nudSigFig);
            this.tabStatistics.Controls.Add(this.lblSigFig);
            this.tabStatistics.Controls.Add(this.label1);
            this.tabStatistics.Controls.Add(this.cmbIntervalSnapping);
            this.tabStatistics.Controls.Add(this.dgvStatistics);
            this.tabStatistics.Controls.Add(this.label2);
            this.tabStatistics.Controls.Add(this.cmbInterval);
            this.tabStatistics.Controls.Add(this.nudCategoryCount);
            this.tabStatistics.Controls.Add(this.lblBreaks);
            resources.ApplyResources(this.tabStatistics, "tabStatistics");
            this.tabStatistics.Name = "tabStatistics";
            this.tabStatistics.UseVisualStyleBackColor = true;
            // 
            // nudSigFig
            // 
            resources.ApplyResources(this.nudSigFig, "nudSigFig");
            this.nudSigFig.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.nudSigFig.Name = "nudSigFig";
            this.nudSigFig.ValueChanged += new System.EventHandler(this.nudSigFig_ValueChanged);
            // 
            // lblSigFig
            // 
            resources.ApplyResources(this.lblSigFig, "lblSigFig");
            this.lblSigFig.Name = "lblSigFig";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // cmbIntervalSnapping
            // 
            this.cmbIntervalSnapping.FormattingEnabled = true;
            resources.ApplyResources(this.cmbIntervalSnapping, "cmbIntervalSnapping");
            this.cmbIntervalSnapping.Name = "cmbIntervalSnapping";
            this.cmbIntervalSnapping.SelectedIndexChanged += new System.EventHandler(this.cmbIntervalSnapping_SelectedIndexChanged);
            // 
            // dgvStatistics
            // 
            this.dgvStatistics.AllowUserToAddRows = false;
            this.dgvStatistics.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AntiqueWhite;
            this.dgvStatistics.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvStatistics.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvStatistics.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvStatistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStatistics.ColumnHeadersVisible = false;
            this.dgvStatistics.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Stat,
            this.Value});
            resources.ApplyResources(this.dgvStatistics, "dgvStatistics");
            this.dgvStatistics.Name = "dgvStatistics";
            this.dgvStatistics.RowHeadersVisible = false;
            this.dgvStatistics.ShowCellErrors = false;
            // 
            // Stat
            // 
            resources.ApplyResources(this.Stat, "Stat");
            this.Stat.Name = "Stat";
            // 
            // Value
            // 
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.Value, "Value");
            this.Value.Name = "Value";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // cmbInterval
            // 
            this.cmbInterval.FormattingEnabled = true;
            this.cmbInterval.Items.AddRange(new object[] {
            resources.GetString("cmbInterval.Items"),
            resources.GetString("cmbInterval.Items1"),
            resources.GetString("cmbInterval.Items2")});
            resources.ApplyResources(this.cmbInterval, "cmbInterval");
            this.cmbInterval.Name = "cmbInterval";
            this.cmbInterval.SelectedIndexChanged += new System.EventHandler(this.cmbInterval_SelectedIndexChanged);
            // 
            // nudCategoryCount
            // 
            resources.ApplyResources(this.nudCategoryCount, "nudCategoryCount");
            this.nudCategoryCount.Name = "nudCategoryCount";
            this.nudCategoryCount.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudCategoryCount.ValueChanged += new System.EventHandler(this.nudCategoryCount_ValueChanged);
            // 
            // lblBreaks
            // 
            resources.ApplyResources(this.lblBreaks, "lblBreaks");
            this.lblBreaks.Name = "lblBreaks";
            // 
            // tabGraph
            // 
            this.tabGraph.Controls.Add(this.lblColumns);
            this.tabGraph.Controls.Add(this.nudColumns);
            this.tabGraph.Controls.Add(this.chkLog);
            this.tabGraph.Controls.Add(this.chkShowStd);
            this.tabGraph.Controls.Add(this.chkShowMean);
            this.tabGraph.Controls.Add(this.breakSliderGraph1);
            resources.ApplyResources(this.tabGraph, "tabGraph");
            this.tabGraph.Name = "tabGraph";
            this.tabGraph.UseVisualStyleBackColor = true;
            // 
            // lblColumns
            // 
            resources.ApplyResources(this.lblColumns, "lblColumns");
            this.lblColumns.Name = "lblColumns";
            // 
            // nudColumns
            // 
            this.nudColumns.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            resources.ApplyResources(this.nudColumns, "nudColumns");
            this.nudColumns.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudColumns.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudColumns.Name = "nudColumns";
            this.nudColumns.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            this.nudColumns.ValueChanged += new System.EventHandler(this.nudColumns_ValueChanged);
            // 
            // chkLog
            // 
            resources.ApplyResources(this.chkLog, "chkLog");
            this.chkLog.Name = "chkLog";
            this.chkLog.UseVisualStyleBackColor = true;
            this.chkLog.CheckedChanged += new System.EventHandler(this.chkLog_CheckedChanged);
            // 
            // chkShowStd
            // 
            resources.ApplyResources(this.chkShowStd, "chkShowStd");
            this.chkShowStd.Name = "chkShowStd";
            this.chkShowStd.UseVisualStyleBackColor = true;
            this.chkShowStd.CheckedChanged += new System.EventHandler(this.chkShowStd_CheckedChanged);
            // 
            // chkShowMean
            // 
            resources.ApplyResources(this.chkShowMean, "chkShowMean");
            this.chkShowMean.Name = "chkShowMean";
            this.chkShowMean.UseVisualStyleBackColor = true;
            this.chkShowMean.CheckedChanged += new System.EventHandler(this.chkShowMean_CheckedChanged);
            // 
            // dgvCategories
            // 
            this.dgvCategories.AllowUserToAddRows = false;
            this.dgvCategories.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCategories.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvCategories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCategories.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSymbol,
            this.colValues,
            this.colLegendText,
            this.colCount});
            resources.ApplyResources(this.dgvCategories, "dgvCategories");
            this.dgvCategories.MultiSelect = false;
            this.dgvCategories.Name = "dgvCategories";
            this.dgvCategories.RowHeadersVisible = false;
            this.dgvCategories.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // colSymbol
            // 
            this.colSymbol.FillWeight = 49.97129F;
            resources.ApplyResources(this.colSymbol, "colSymbol");
            this.colSymbol.Name = "colSymbol";
            this.colSymbol.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // colValues
            // 
            this.colValues.FillWeight = 142.132F;
            resources.ApplyResources(this.colValues, "colValues");
            this.colValues.Name = "colValues";
            this.colValues.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colLegendText
            // 
            this.colLegendText.FillWeight = 157.008F;
            resources.ApplyResources(this.colLegendText, "colLegendText");
            this.colLegendText.Name = "colLegendText";
            // 
            // colCount
            // 
            this.colCount.FillWeight = 50.88878F;
            resources.ApplyResources(this.colCount, "colCount");
            this.colCount.Name = "colCount";
            this.colCount.ReadOnly = true;
            this.colCount.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // chkHillshade
            // 
            resources.ApplyResources(this.chkHillshade, "chkHillshade");
            this.chkHillshade.Name = "chkHillshade";
            this.chkHillshade.UseVisualStyleBackColor = true;
            this.chkHillshade.CheckedChanged += new System.EventHandler(this.chkHillshade_CheckedChanged);
            // 
            // btnShadedRelief
            // 
            resources.ApplyResources(this.btnShadedRelief, "btnShadedRelief");
            this.btnShadedRelief.Name = "btnShadedRelief";
            this.btnShadedRelief.UseVisualStyleBackColor = true;
            this.btnShadedRelief.Click += new System.EventHandler(this.btnShadedRelief_Click);
            // 
            // grpHillshade
            // 
            this.grpHillshade.Controls.Add(this.angLightDirection);
            this.grpHillshade.Controls.Add(this.btnShadedRelief);
            this.grpHillshade.Controls.Add(this.btnElevation);
            this.grpHillshade.Controls.Add(this.dbxElevationFactor);
            this.grpHillshade.Controls.Add(this.chkHillshade);
            resources.ApplyResources(this.grpHillshade, "grpHillshade");
            this.grpHillshade.Name = "grpHillshade";
            this.grpHillshade.TabStop = false;
            // 
            // btnElevation
            // 
            this.btnElevation.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.ArrowDownGreen;
            resources.ApplyResources(this.btnElevation, "btnElevation");
            this.btnElevation.Name = "btnElevation";
            this.btnElevation.UseVisualStyleBackColor = true;
            this.btnElevation.Click += new System.EventHandler(this.btnElevation_Click);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.FillWeight = 76.14214F;
            resources.ApplyResources(this.dataGridViewImageColumn1, "dataGridViewImageColumn1");
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.add;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.delete;
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.opacityNoData);
            this.groupBox1.Controls.Add(this.colorNoData);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // opacityNoData
            // 
            this.opacityNoData.ColorButton = this.colorNoData;
            this.opacityNoData.FlipRamp = false;
            this.opacityNoData.FlipText = false;
            this.opacityNoData.InvertRamp = false;
            resources.ApplyResources(this.opacityNoData, "opacityNoData");
            this.opacityNoData.Maximum = 1D;
            this.opacityNoData.MaximumColor = System.Drawing.Color.Green;
            this.opacityNoData.Minimum = 0D;
            this.opacityNoData.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.opacityNoData.Name = "opacityNoData";
            this.opacityNoData.NumberFormat = "#.00";
            this.opacityNoData.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.opacityNoData.RampRadius = 10F;
            this.opacityNoData.RampText = "Opacity";
            this.opacityNoData.RampTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.opacityNoData.RampTextBehindRamp = true;
            this.opacityNoData.RampTextColor = System.Drawing.Color.Black;
            this.opacityNoData.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opacityNoData.ShowMaximum = false;
            this.opacityNoData.ShowMinimum = false;
            this.opacityNoData.ShowTicks = false;
            this.opacityNoData.ShowValue = false;
            this.opacityNoData.SliderColor = System.Drawing.Color.Blue;
            this.opacityNoData.SliderRadius = 4F;
            this.opacityNoData.TickColor = System.Drawing.Color.DarkGray;
            this.opacityNoData.TickSpacing = 5F;
            this.opacityNoData.Value = 0D;
            // 
            // colorNoData
            // 
            this.colorNoData.BevelRadius = 0;
            this.colorNoData.Color = System.Drawing.Color.Transparent;
            this.colorNoData.LaunchDialogOnClick = true;
            resources.ApplyResources(this.colorNoData, "colorNoData");
            this.colorNoData.Name = "colorNoData";
            this.colorNoData.RoundingRadius = 3;
            this.colorNoData.ColorChanged += new System.EventHandler(this.colorNoData_ColorChanged);
            // 
            // sldSchemeOpacity
            // 
            this.sldSchemeOpacity.ColorButton = null;
            this.sldSchemeOpacity.FlipRamp = false;
            this.sldSchemeOpacity.FlipText = false;
            resources.ApplyResources(this.sldSchemeOpacity, "sldSchemeOpacity");
            this.sldSchemeOpacity.InvertRamp = false;
            this.sldSchemeOpacity.Maximum = 1D;
            this.sldSchemeOpacity.MaximumColor = System.Drawing.Color.Green;
            this.sldSchemeOpacity.Minimum = 0D;
            this.sldSchemeOpacity.MinimumColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.sldSchemeOpacity.Name = "sldSchemeOpacity";
            this.sldSchemeOpacity.NumberFormat = "#.00";
            this.sldSchemeOpacity.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.sldSchemeOpacity.RampRadius = 9F;
            this.sldSchemeOpacity.RampText = "Opacity";
            this.sldSchemeOpacity.RampTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.sldSchemeOpacity.RampTextBehindRamp = true;
            this.sldSchemeOpacity.RampTextColor = System.Drawing.Color.Black;
            this.sldSchemeOpacity.RampTextFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sldSchemeOpacity.ShowMaximum = false;
            this.sldSchemeOpacity.ShowMinimum = false;
            this.sldSchemeOpacity.ShowTicks = false;
            this.sldSchemeOpacity.ShowValue = false;
            this.sldSchemeOpacity.SliderColor = System.Drawing.Color.Blue;
            this.sldSchemeOpacity.SliderRadius = 4F;
            this.sldSchemeOpacity.TickColor = System.Drawing.Color.DarkGray;
            this.sldSchemeOpacity.TickSpacing = 5F;
            this.sldSchemeOpacity.Value = 1D;
            this.sldSchemeOpacity.ValueChanged += new System.EventHandler(this.sldSchemeOpacity_ValueChanged);
            // 
            // angLightDirection
            // 
            this.angLightDirection.Angle = 45;
            this.angLightDirection.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.angLightDirection, "angLightDirection");
            this.angLightDirection.Clockwise = true;
            this.angLightDirection.KnobColor = System.Drawing.Color.Green;
            this.angLightDirection.Name = "angLightDirection";
            this.angLightDirection.StartAngle = 90;
            this.ttHelp.SetToolTip(this.angLightDirection, resources.GetString("angLightDirection.ToolTip"));
            // 
            // dbxElevationFactor
            // 
            this.dbxElevationFactor.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxElevationFactor.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.dbxElevationFactor, "dbxElevationFactor");
            this.dbxElevationFactor.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.dbxElevationFactor.IsValid = true;
            this.dbxElevationFactor.Name = "dbxElevationFactor";
            this.dbxElevationFactor.NumberFormat = "E7";
            this.dbxElevationFactor.RegularHelp = "The Elevation Factor is a constant multiplier that converts the elevation unit (e" +
    "g. feet) into the projection units (eg. decimal degrees).";
            this.ttHelp.SetToolTip(this.dbxElevationFactor, resources.GetString("dbxElevationFactor.ToolTip"));
            this.dbxElevationFactor.Value = 0D;
            this.dbxElevationFactor.TextChanged += new System.EventHandler(this.dbxElevationFactor_TextChanged);
            // 
            // mwProgressBar1
            // 
            this.mwProgressBar1.FontColor = System.Drawing.Color.Black;
            resources.ApplyResources(this.mwProgressBar1, "mwProgressBar1");
            this.mwProgressBar1.Name = "mwProgressBar1";
            this.mwProgressBar1.ShowMessage = true;
            // 
            // tccColorRange
            // 
            this.tccColorRange.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tccColorRange.HueShift = 0;
            resources.ApplyResources(this.tccColorRange, "tccColorRange");
            this.tccColorRange.Name = "tccColorRange";
            this.tccColorRange.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tccColorRange.UseRangeChecked = true;
            this.tccColorRange.ColorChanged += new System.EventHandler<DotSpatial.Symbology.Forms.ColorRangeEventArgs>(this.tccColorRange_ColorChanged);
            // 
            // dbxMax
            // 
            this.dbxMax.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMax.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.dbxMax, "dbxMax");
            this.dbxMax.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.dbxMax.IsValid = true;
            this.dbxMax.Name = "dbxMax";
            this.dbxMax.NumberFormat = null;
            this.dbxMax.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMax.Value = 1000000D;
            this.dbxMax.TextChanged += new System.EventHandler(this.dbxMax_TextChanged);
            // 
            // dbxMin
            // 
            this.dbxMin.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMin.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.dbxMin, "dbxMin");
            this.dbxMin.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.dbxMin.IsValid = true;
            this.dbxMin.Name = "dbxMin";
            this.dbxMin.NumberFormat = null;
            this.dbxMin.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMin.Value = -100000D;
            this.dbxMin.TextChanged += new System.EventHandler(this.dbxMin_TextChanged);
            // 
            // breakSliderGraph1
            // 
            this.breakSliderGraph1.AttributeSource = null;
            this.breakSliderGraph1.BackColor = System.Drawing.Color.White;
            this.breakSliderGraph1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.breakSliderGraph1.BreakColor = System.Drawing.Color.Blue;
            this.breakSliderGraph1.BreakSelectedColor = System.Drawing.Color.Red;
            this.breakSliderGraph1.FontColor = System.Drawing.Color.Black;
            this.breakSliderGraph1.IntervalMethod = DotSpatial.Symbology.IntervalMethod.EqualInterval;
            resources.ApplyResources(this.breakSliderGraph1, "breakSliderGraph1");
            this.breakSliderGraph1.LogY = false;
            this.breakSliderGraph1.MaximumSampleSize = 10000;
            this.breakSliderGraph1.MinHeight = 20;
            this.breakSliderGraph1.Name = "breakSliderGraph1";
            this.breakSliderGraph1.NumColumns = 40;
            this.breakSliderGraph1.RasterLayer = null;
            this.breakSliderGraph1.Scheme = null;
            this.breakSliderGraph1.ShowMean = false;
            this.breakSliderGraph1.ShowStandardDeviation = false;
            this.breakSliderGraph1.Title = "Statistical Breaks:";
            this.breakSliderGraph1.TitleColor = System.Drawing.Color.Black;
            this.breakSliderGraph1.TitleFont = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold);
            this.breakSliderGraph1.SliderMoved += new System.EventHandler<DotSpatial.Symbology.Forms.BreakSliderEventArgs>(this.breakSliderGraph1_SliderMoved);
            // 
            // RasterCategoryControl
            // 
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.sldSchemeOpacity);
            this.Controls.Add(this.grpHillshade);
            this.Controls.Add(this.mwProgressBar1);
            this.Controls.Add(this.btnQuick);
            this.Controls.Add(this.tccColorRange);
            this.Controls.Add(this.tabScheme);
            this.Controls.Add(this.dgvCategories);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRamp);
            this.Controls.Add(this.cmdRefresh);
            this.Name = "RasterCategoryControl";
            resources.ApplyResources(this, "$this");
            this.tabScheme.ResumeLayout(false);
            this.tabStatistics.ResumeLayout(false);
            this.tabStatistics.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSigFig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistics)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCategoryCount)).EndInit();
            this.tabGraph.ResumeLayout(false);
            this.tabGraph.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategories)).EndInit();
            this.grpHillshade.ResumeLayout(false);
            this.grpHillshade.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ContextMenu _elevationQuickPick;
        private DataGridViewImageColumn dataGridViewImageColumn1;
        private DoubleBox dbxElevationFactor;
        private DoubleBox dbxMax;
        private DoubleBox dbxMin;
        private DataGridView dgvCategories;
        private DataGridView dgvStatistics;
        private GroupBox groupBox1;
        private GroupBox grpHillshade;
        private Label label1;
        private Label label2;
        private Label lblBreaks;
        private Label lblColumns;
        private Label lblSigFig;
        private SymbologyProgressBar mwProgressBar1;
        private NumericUpDown nudCategoryCount;
        private NumericUpDown nudColumns;
        private NumericUpDown nudSigFig;
        private RampSlider opacityNoData;
        private RampSlider sldSchemeOpacity;
        private DataGridViewTextBoxColumn stat;
        private TabPage tabGraph;
        private TabControl tabScheme;
        private TabPage tabStatistics;
        private TabColorControl tccColorRange;
        private ToolTip ttHelp;
        private DataGridViewTextBoxColumn value;
        private TabColorDialog _tabColorDialog;
        private BreakSliderGraph breakSliderGraph1;
        private Button btnAdd;
        private Button btnDelete;
        private Button btnElevation;
        private Button btnQuick;
        private Button btnRamp;
        private Button btnShadedRelief;
        private CheckBox chkHillshade;
        private CheckBox chkLog;
        private CheckBox chkShowMean;
        private CheckBox chkShowStd;
        private ComboBox cmbInterval;
        private ComboBox cmbIntervalSnapping;
        private Button cmdRefresh;
        private DataGridViewTextBoxColumn colCount;
        private DataGridViewTextBoxColumn colLegendText;
        private DataGridViewImageColumn colSymbol;
        private DataGridViewTextBoxColumn colValues;
        private ColorButton colorNoData;
        private PropertyDialog _shadedReliefDialog;
        private DataGridViewTextBoxColumn Stat;
        private DataGridViewTextBoxColumn Value;
        private AngleControl angLightDirection;
    }
}
