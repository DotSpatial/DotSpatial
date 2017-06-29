using System.ComponentModel;
using System.Windows.Forms;
using System;
using System.Linq;

namespace DotSpatial.Symbology.Forms
{
    partial class FeatureCategoryControl
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
                foreach (var id in components.Components.OfType<IDisposable>())
                {
                    id.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureCategoryControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this._cmbField = new System.Windows.Forms.ComboBox();
            this._lblFieldName = new System.Windows.Forms.Label();
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.btnRamp = new System.Windows.Forms.Button();
            this.cmdRefresh = new System.Windows.Forms.Button();
            this.btnExclude = new System.Windows.Forms.Button();
            this.radUniqueValues = new System.Windows.Forms.RadioButton();
            this.radQuantities = new System.Windows.Forms.RadioButton();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.cmbNormField = new System.Windows.Forms.ComboBox();
            this.nudSigFig = new System.Windows.Forms.NumericUpDown();
            this.cmbIntervalSnapping = new System.Windows.Forms.ComboBox();
            this.dgvStatistics = new System.Windows.Forms.DataGridView();
            this.Stat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabScheme = new System.Windows.Forms.TabControl();
            this.tabStatistics = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSigFig = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
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
            this.breakSliderGraph1 = new DotSpatial.Symbology.Forms.BreakSliderGraph();
            this.chkUseGradients = new System.Windows.Forms.CheckBox();
            this.dgvCategories = new System.Windows.Forms.DataGridView();
            this.colSymbol = new System.Windows.Forms.DataGridViewImageColumn();
            this.colValues = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLegendText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.radCustom = new System.Windows.Forms.RadioButton();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.angGradientAngle = new DotSpatial.Symbology.Forms.AngleControl();
            this.featureSizeRangeControl1 = new DotSpatial.Symbology.Forms.FeatureSizeRangeControl();
            this.tccColorRange = new DotSpatial.Symbology.Forms.TabColorControl();
            ((System.ComponentModel.ISupportInitialize)(this.nudSigFig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistics)).BeginInit();
            this.tabScheme.SuspendLayout();
            this.tabStatistics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCategoryCount)).BeginInit();
            this.tabGraph.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategories)).BeginInit();
            this.SuspendLayout();
            // 
            // _cmbField
            // 
            resources.ApplyResources(this._cmbField, "_cmbField");
            this._cmbField.FormattingEnabled = true;
            this._cmbField.Name = "_cmbField";
            this.ttHelp.SetToolTip(this._cmbField, resources.GetString("_cmbField.ToolTip"));
            this._cmbField.SelectedIndexChanged += new System.EventHandler(this.cmbField_SelectedIndexChanged);
            // 
            // _lblFieldName
            // 
            resources.ApplyResources(this._lblFieldName, "_lblFieldName");
            this._lblFieldName.Name = "_lblFieldName";
            this.ttHelp.SetToolTip(this._lblFieldName, resources.GetString("_lblFieldName.ToolTip"));
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
            // btnExclude
            // 
            resources.ApplyResources(this.btnExclude, "btnExclude");
            this.btnExclude.Name = "btnExclude";
            this.ttHelp.SetToolTip(this.btnExclude, resources.GetString("btnExclude.ToolTip"));
            this.btnExclude.UseVisualStyleBackColor = true;
            this.btnExclude.Click += new System.EventHandler(this.btnExclude_Click);
            // 
            // radUniqueValues
            // 
            resources.ApplyResources(this.radUniqueValues, "radUniqueValues");
            this.radUniqueValues.Checked = true;
            this.radUniqueValues.Name = "radUniqueValues";
            this.radUniqueValues.TabStop = true;
            this.ttHelp.SetToolTip(this.radUniqueValues, resources.GetString("radUniqueValues.ToolTip"));
            this.radUniqueValues.UseVisualStyleBackColor = true;
            this.radUniqueValues.CheckedChanged += new System.EventHandler(this.radUniqueValues_CheckedChanged);
            // 
            // radQuantities
            // 
            resources.ApplyResources(this.radQuantities, "radQuantities");
            this.radQuantities.Name = "radQuantities";
            this.ttHelp.SetToolTip(this.radQuantities, resources.GetString("radQuantities.ToolTip"));
            this.radQuantities.UseVisualStyleBackColor = true;
            this.radQuantities.CheckedChanged += new System.EventHandler(this.radQuantities_CheckedChanged);
            // 
            // btnAdd
            // 
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.add;
            this.btnAdd.Name = "btnAdd";
            this.ttHelp.SetToolTip(this.btnAdd, resources.GetString("btnAdd.ToolTip"));
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.delete;
            this.btnDelete.Name = "btnDelete";
            this.ttHelp.SetToolTip(this.btnDelete, resources.GetString("btnDelete.ToolTip"));
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUp
            // 
            resources.ApplyResources(this.btnUp, "btnUp");
            this.btnUp.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.up;
            this.btnUp.Name = "btnUp";
            this.ttHelp.SetToolTip(this.btnUp, resources.GetString("btnUp.ToolTip"));
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnDown
            // 
            resources.ApplyResources(this.btnDown, "btnDown");
            this.btnDown.BackColor = System.Drawing.SystemColors.Control;
            this.btnDown.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.down;
            this.btnDown.Name = "btnDown";
            this.ttHelp.SetToolTip(this.btnDown, resources.GetString("btnDown.ToolTip"));
            this.btnDown.UseVisualStyleBackColor = false;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // cmbNormField
            // 
            resources.ApplyResources(this.cmbNormField, "cmbNormField");
            this.cmbNormField.FormattingEnabled = true;
            this.cmbNormField.Name = "cmbNormField";
            this.ttHelp.SetToolTip(this.cmbNormField, resources.GetString("cmbNormField.ToolTip"));
            this.cmbNormField.SelectedIndexChanged += new System.EventHandler(this.cmbNormField_SelectedIndexChanged);
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
            this.ttHelp.SetToolTip(this.nudSigFig, resources.GetString("nudSigFig.ToolTip"));
            this.nudSigFig.ValueChanged += new System.EventHandler(this.nudSigFig_ValueChanged);
            // 
            // cmbIntervalSnapping
            // 
            resources.ApplyResources(this.cmbIntervalSnapping, "cmbIntervalSnapping");
            this.cmbIntervalSnapping.FormattingEnabled = true;
            this.cmbIntervalSnapping.Name = "cmbIntervalSnapping";
            this.ttHelp.SetToolTip(this.cmbIntervalSnapping, resources.GetString("cmbIntervalSnapping.ToolTip"));
            this.cmbIntervalSnapping.SelectedIndexChanged += new System.EventHandler(this.cmbIntervalSnapping_SelectedIndexChanged);
            // 
            // dgvStatistics
            // 
            resources.ApplyResources(this.dgvStatistics, "dgvStatistics");
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
            this.dgvStatistics.Name = "dgvStatistics";
            this.dgvStatistics.RowHeadersVisible = false;
            this.dgvStatistics.ShowCellErrors = false;
            this.ttHelp.SetToolTip(this.dgvStatistics, resources.GetString("dgvStatistics.ToolTip"));
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
            // tabScheme
            // 
            resources.ApplyResources(this.tabScheme, "tabScheme");
            this.tabScheme.Controls.Add(this.tabStatistics);
            this.tabScheme.Controls.Add(this.tabGraph);
            this.tabScheme.Name = "tabScheme";
            this.tabScheme.SelectedIndex = 0;
            this.ttHelp.SetToolTip(this.tabScheme, resources.GetString("tabScheme.ToolTip"));
            // 
            // tabStatistics
            // 
            resources.ApplyResources(this.tabStatistics, "tabStatistics");
            this.tabStatistics.Controls.Add(this.label3);
            this.tabStatistics.Controls.Add(this.cmbNormField);
            this.tabStatistics.Controls.Add(this.btnExclude);
            this.tabStatistics.Controls.Add(this.nudSigFig);
            this.tabStatistics.Controls.Add(this.lblSigFig);
            this.tabStatistics.Controls.Add(this.label1);
            this.tabStatistics.Controls.Add(this.cmbIntervalSnapping);
            this.tabStatistics.Controls.Add(this.dgvStatistics);
            this.tabStatistics.Controls.Add(this.label2);
            this.tabStatistics.Controls.Add(this.cmbInterval);
            this.tabStatistics.Controls.Add(this.nudCategoryCount);
            this.tabStatistics.Controls.Add(this.lblBreaks);
            this.tabStatistics.Name = "tabStatistics";
            this.ttHelp.SetToolTip(this.tabStatistics, resources.GetString("tabStatistics.ToolTip"));
            this.tabStatistics.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            this.ttHelp.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
            // 
            // lblSigFig
            // 
            resources.ApplyResources(this.lblSigFig, "lblSigFig");
            this.lblSigFig.Name = "lblSigFig";
            this.ttHelp.SetToolTip(this.lblSigFig, resources.GetString("lblSigFig.ToolTip"));
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.ttHelp.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            this.ttHelp.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // cmbInterval
            // 
            resources.ApplyResources(this.cmbInterval, "cmbInterval");
            this.cmbInterval.FormattingEnabled = true;
            this.cmbInterval.Items.AddRange(new object[] {
            resources.GetString("cmbInterval.Items"),
            resources.GetString("cmbInterval.Items1"),
            resources.GetString("cmbInterval.Items2")});
            this.cmbInterval.Name = "cmbInterval";
            this.ttHelp.SetToolTip(this.cmbInterval, resources.GetString("cmbInterval.ToolTip"));
            this.cmbInterval.SelectedIndexChanged += new System.EventHandler(this.cmbInterval_SelectedIndexChanged);
            // 
            // nudCategoryCount
            // 
            resources.ApplyResources(this.nudCategoryCount, "nudCategoryCount");
            this.nudCategoryCount.Name = "nudCategoryCount";
            this.ttHelp.SetToolTip(this.nudCategoryCount, resources.GetString("nudCategoryCount.ToolTip"));
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
            this.ttHelp.SetToolTip(this.lblBreaks, resources.GetString("lblBreaks.ToolTip"));
            // 
            // tabGraph
            // 
            resources.ApplyResources(this.tabGraph, "tabGraph");
            this.tabGraph.Controls.Add(this.lblColumns);
            this.tabGraph.Controls.Add(this.nudColumns);
            this.tabGraph.Controls.Add(this.chkLog);
            this.tabGraph.Controls.Add(this.chkShowStd);
            this.tabGraph.Controls.Add(this.chkShowMean);
            this.tabGraph.Controls.Add(this.breakSliderGraph1);
            this.tabGraph.Name = "tabGraph";
            this.ttHelp.SetToolTip(this.tabGraph, resources.GetString("tabGraph.ToolTip"));
            this.tabGraph.UseVisualStyleBackColor = true;
            // 
            // lblColumns
            // 
            resources.ApplyResources(this.lblColumns, "lblColumns");
            this.lblColumns.Name = "lblColumns";
            this.ttHelp.SetToolTip(this.lblColumns, resources.GetString("lblColumns.ToolTip"));
            // 
            // nudColumns
            // 
            resources.ApplyResources(this.nudColumns, "nudColumns");
            this.nudColumns.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
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
            this.ttHelp.SetToolTip(this.nudColumns, resources.GetString("nudColumns.ToolTip"));
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
            this.ttHelp.SetToolTip(this.chkLog, resources.GetString("chkLog.ToolTip"));
            this.chkLog.UseVisualStyleBackColor = true;
            this.chkLog.CheckedChanged += new System.EventHandler(this.chkLog_CheckedChanged);
            // 
            // chkShowStd
            // 
            resources.ApplyResources(this.chkShowStd, "chkShowStd");
            this.chkShowStd.Name = "chkShowStd";
            this.ttHelp.SetToolTip(this.chkShowStd, resources.GetString("chkShowStd.ToolTip"));
            this.chkShowStd.UseVisualStyleBackColor = true;
            this.chkShowStd.CheckedChanged += new System.EventHandler(this.chkShowStd_CheckedChanged);
            // 
            // chkShowMean
            // 
            resources.ApplyResources(this.chkShowMean, "chkShowMean");
            this.chkShowMean.Name = "chkShowMean";
            this.ttHelp.SetToolTip(this.chkShowMean, resources.GetString("chkShowMean.ToolTip"));
            this.chkShowMean.UseVisualStyleBackColor = true;
            this.chkShowMean.CheckedChanged += new System.EventHandler(this.chkShowMean_CheckedChanged);
            // 
            // breakSliderGraph1
            // 
            resources.ApplyResources(this.breakSliderGraph1, "breakSliderGraph1");
            this.breakSliderGraph1.AttributeSource = null;
            this.breakSliderGraph1.BackColor = System.Drawing.Color.White;
            this.breakSliderGraph1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.breakSliderGraph1.BreakColor = System.Drawing.Color.Blue;
            this.breakSliderGraph1.BreakSelectedColor = System.Drawing.Color.Red;
            this.breakSliderGraph1.FontColor = System.Drawing.Color.Black;
            this.breakSliderGraph1.IntervalMethod = DotSpatial.Symbology.IntervalMethod.EqualInterval;
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
            this.ttHelp.SetToolTip(this.breakSliderGraph1, resources.GetString("breakSliderGraph1.ToolTip"));
            this.breakSliderGraph1.SliderMoved += new System.EventHandler<DotSpatial.Symbology.Forms.BreakSliderEventArgs>(this.breakSliderGraph1_SliderMoved);
            // 
            // chkUseGradients
            // 
            resources.ApplyResources(this.chkUseGradients, "chkUseGradients");
            this.chkUseGradients.Name = "chkUseGradients";
            this.ttHelp.SetToolTip(this.chkUseGradients, resources.GetString("chkUseGradients.ToolTip"));
            this.chkUseGradients.UseVisualStyleBackColor = true;
            this.chkUseGradients.CheckedChanged += new System.EventHandler(this.chkUseGradients_CheckedChanged);
            // 
            // dgvCategories
            // 
            resources.ApplyResources(this.dgvCategories, "dgvCategories");
            this.dgvCategories.AllowUserToAddRows = false;
            this.dgvCategories.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCategories.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgvCategories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCategories.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSymbol,
            this.colValues,
            this.colLegendText,
            this.colCount});
            this.dgvCategories.MultiSelect = false;
            this.dgvCategories.Name = "dgvCategories";
            this.dgvCategories.RowHeadersVisible = false;
            this.dgvCategories.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ttHelp.SetToolTip(this.dgvCategories, resources.GetString("dgvCategories.ToolTip"));
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
            // radCustom
            // 
            resources.ApplyResources(this.radCustom, "radCustom");
            this.radCustom.Name = "radCustom";
            this.radCustom.TabStop = true;
            this.ttHelp.SetToolTip(this.radCustom, resources.GetString("radCustom.ToolTip"));
            this.radCustom.UseVisualStyleBackColor = true;
            this.radCustom.CheckedChanged += new System.EventHandler(this.radCustom_CheckedChanged);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.FillWeight = 76.14214F;
            resources.ApplyResources(this.dataGridViewImageColumn1, "dataGridViewImageColumn1");
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewImageColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // angGradientAngle
            // 
            resources.ApplyResources(this.angGradientAngle, "angGradientAngle");
            this.angGradientAngle.Angle = 0;
            this.angGradientAngle.BackColor = System.Drawing.Color.Transparent;
            this.angGradientAngle.Clockwise = false;
            this.angGradientAngle.KnobColor = System.Drawing.Color.Green;
            this.angGradientAngle.Name = "angGradientAngle";
            this.angGradientAngle.StartAngle = 0;
            this.ttHelp.SetToolTip(this.angGradientAngle, resources.GetString("angGradientAngle.ToolTip"));
            this.angGradientAngle.AngleChanged += new System.EventHandler(this.angGradientAngle_AngleChanged);
            // 
            // featureSizeRangeControl1
            // 
            resources.ApplyResources(this.featureSizeRangeControl1, "featureSizeRangeControl1");
            this.featureSizeRangeControl1.Name = "featureSizeRangeControl1";
            this.featureSizeRangeControl1.Scheme = null;
            this.featureSizeRangeControl1.SizeRange = null;
            this.ttHelp.SetToolTip(this.featureSizeRangeControl1, resources.GetString("featureSizeRangeControl1.ToolTip"));
            this.featureSizeRangeControl1.SizeRangeChanged += new System.EventHandler<DotSpatial.Symbology.Forms.SizeRangeEventArgs>(this.pointSizeRangeControl1_SizeRangeChanged);
            // 
            // tccColorRange
            // 
            resources.ApplyResources(this.tccColorRange, "tccColorRange");
            this.tccColorRange.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tccColorRange.HueShift = 0;
            this.tccColorRange.Name = "tccColorRange";
            this.tccColorRange.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ttHelp.SetToolTip(this.tccColorRange, resources.GetString("tccColorRange.ToolTip"));
            this.tccColorRange.UseRangeChecked = true;
            this.tccColorRange.ColorChanged += new System.EventHandler<DotSpatial.Symbology.Forms.ColorRangeEventArgs>(this.tccColorRange_ColorChanged);
            // 
            // FeatureCategoryControl
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.chkUseGradients);
            this.Controls.Add(this.angGradientAngle);
            this.Controls.Add(this.featureSizeRangeControl1);
            this.Controls.Add(this.tccColorRange);
            this.Controls.Add(this.tabScheme);
            this.Controls.Add(this.dgvCategories);
            this.Controls.Add(this.radCustom);
            this.Controls.Add(this.btnDown);
            this.Controls.Add(this.btnUp);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.radQuantities);
            this.Controls.Add(this.radUniqueValues);
            this.Controls.Add(this.btnRamp);
            this.Controls.Add(this.cmdRefresh);
            this.Controls.Add(this._lblFieldName);
            this.Controls.Add(this._cmbField);
            this.Name = "FeatureCategoryControl";
            this.ttHelp.SetToolTip(this, resources.GetString("$this.ToolTip"));
            ((System.ComponentModel.ISupportInitialize)(this.nudSigFig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistics)).EndInit();
            this.tabScheme.ResumeLayout(false);
            this.tabStatistics.ResumeLayout(false);
            this.tabStatistics.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCategoryCount)).EndInit();
            this.tabGraph.ResumeLayout(false);
            this.tabGraph.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategories)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AngleControl angGradientAngle;
        private BreakSliderGraph breakSliderGraph1;
        private Button btnAdd;
        private Button btnDelete;
        private Button btnDown;
        private Button btnExclude;
        private Button btnRamp;
        private Button btnUp;
        private CheckBox chkLog;
        private CheckBox chkShowMean;
        private CheckBox chkShowStd;
        private CheckBox chkUseGradients;
        private ComboBox cmbInterval;
        private ComboBox cmbIntervalSnapping;
        private ComboBox cmbNormField;
        private Button cmdRefresh;
        private DataGridViewTextBoxColumn colCount;
        private DataGridViewTextBoxColumn colLegendText;
        private DataGridViewImageColumn colSymbol;
        private DataGridViewTextBoxColumn colValues;
        private DataGridViewImageColumn dataGridViewImageColumn1;
        private DataGridView dgvCategories;
        private DataGridView dgvStatistics;
        private FeatureSizeRangeControl featureSizeRangeControl1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label lblBreaks;
        private Label lblColumns;
        private Label lblSigFig;
        private Label _lblFieldName;
        private NumericUpDown nudCategoryCount;
        private NumericUpDown nudColumns;
        private NumericUpDown nudSigFig;
        private RadioButton radCustom;
        private RadioButton radQuantities;
        private RadioButton radUniqueValues;
        private TabPage tabGraph;
        private TabControl tabScheme;
        private TabPage tabStatistics;
        private TabColorControl tccColorRange;
        private ToolTip ttHelp;
        private ProgressCancelDialog _cancelDialog;
        private Timer _cleanupTimer;
        private ComboBox _cmbField;
        private DataGridViewTextBoxColumn Stat;
        private DataGridViewTextBoxColumn Value;
    }
}
