// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The core assembly for the DotSpatial 6.0 distribution.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 9/27/2009 10:07:34 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Data;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// StatisticalBreaksDialog
    /// </summary>
    public class StatisticalBreaksDialog : Form
    {
        private BreakSliderGraph breakSliderGraph1;
        private Button btnCancel;
        private Button btnOK;
        private CheckBox chkLog;
        private CheckBox chkShowMean;
        private CheckBox chkShowStd;
        private ComboBox cmbInterval;
        private Label label1;
        private Label label2;
        private NumericUpDown nudColumns;
        private Panel panel1;

        #region Private Variables

        #endregion

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatisticalBreaksDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkShowMean = new System.Windows.Forms.CheckBox();
            this.chkShowStd = new System.Windows.Forms.CheckBox();
            this.nudColumns = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.chkLog = new System.Windows.Forms.CheckBox();
            this.breakSliderGraph1 = new DotSpatial.Symbology.Forms.BreakSliderGraph();
            this.cmbInterval = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns)).BeginInit();
            this.SuspendLayout();
            //
            // panel1
            //
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.btnCancel);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            //
            // btnOK
            //
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            //
            // btnCancel
            //
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // chkShowMean
            //
            resources.ApplyResources(this.chkShowMean, "chkShowMean");
            this.chkShowMean.Name = "chkShowMean";
            this.chkShowMean.UseVisualStyleBackColor = true;
            this.chkShowMean.CheckedChanged += new System.EventHandler(this.chkShowMean_CheckedChanged);
            //
            // chkShowStd
            //
            resources.ApplyResources(this.chkShowStd, "chkShowStd");
            this.chkShowStd.Name = "chkShowStd";
            this.chkShowStd.UseVisualStyleBackColor = true;
            this.chkShowStd.CheckedChanged += new System.EventHandler(this.chkShowStd_CheckedChanged);
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
                                                              100,
                                                              0,
                                                              0,
                                                              0});
            this.nudColumns.ValueChanged += new System.EventHandler(this.nudColumns_ValueChanged);
            //
            // label1
            //
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            //
            // chkLog
            //
            resources.ApplyResources(this.chkLog, "chkLog");
            this.chkLog.Name = "chkLog";
            this.chkLog.UseVisualStyleBackColor = true;
            this.chkLog.CheckedChanged += new System.EventHandler(this.chkLog_CheckedChanged);
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
            this.breakSliderGraph1.NumColumns = 100;
            this.breakSliderGraph1.RasterLayer = null;
            this.breakSliderGraph1.Scheme = null;
            this.breakSliderGraph1.ShowMean = false;
            this.breakSliderGraph1.ShowStandardDeviation = false;
            this.breakSliderGraph1.Title = null;
            this.breakSliderGraph1.TitleColor = System.Drawing.Color.Black;
            this.breakSliderGraph1.TitleFont = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold);
            //
            // cmbInterval
            //
            this.cmbInterval.FormattingEnabled = true;
            resources.ApplyResources(this.cmbInterval, "cmbInterval");
            this.cmbInterval.Name = "cmbInterval";
            this.cmbInterval.SelectedIndexChanged += new System.EventHandler(this.cmbInterval_SelectedIndexChanged);
            //
            // label2
            //
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            //
            // StatisticalBreaksDialog
            //
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbInterval);
            this.Controls.Add(this.chkLog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudColumns);
            this.Controls.Add(this.chkShowStd);
            this.Controls.Add(this.chkShowMean);
            this.Controls.Add(this.breakSliderGraph1);
            this.Controls.Add(this.panel1);
            this.Name = "StatisticalBreaksDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.StatisticalBreaksDialog_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of CollectionPropertyGrid
        /// </summary>
        public StatisticalBreaksDialog()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the string field name to apply statistics to
        /// </summary>
        public string FieldName
        {
            get { return breakSliderGraph1.Fieldname; }
            set { breakSliderGraph1.Fieldname = value; }
        }

        /// <summary>
        /// Gets or sets the normalization field to apply statistics to.
        /// </summary>
        public string NormalizationField
        {
            get { return breakSliderGraph1.NormalizationField; }
            set { breakSliderGraph1.NormalizationField = value; }
        }

        /// <summary>
        /// Gets or sets the Table that has the data row values to use for statistics.
        /// </summary>
        public DataTable Table
        {
            get { return breakSliderGraph1.Table; }
            set { breakSliderGraph1.Table = value; }
        }

        /// <summary>
        /// Gets or sets the feature scheme to use for coloring and existing break positions
        /// </summary>
        public IFeatureScheme Scheme
        {
            get { return breakSliderGraph1.Scheme; }
            set { breakSliderGraph1.Scheme = value; }
        }

        /// <summary>
        /// Gets or sets the title for the graph
        /// </summary>
        public string Title
        {
            get { return breakSliderGraph1.Title; }
            set { breakSliderGraph1.Title = value; }
        }

        #endregion

        #region Events

        #endregion

        #region Event Handlers

        #endregion

        private void chkShowStd_CheckedChanged(object sender, EventArgs e)
        {
            breakSliderGraph1.ShowStandardDeviation = chkShowStd.Checked;
        }

        private void chkShowMean_CheckedChanged(object sender, EventArgs e)
        {
            breakSliderGraph1.ShowMean = chkShowMean.Checked;
        }

        private void nudColumns_ValueChanged(object sender, EventArgs e)
        {
            breakSliderGraph1.NumColumns = (int)nudColumns.Value;
        }

        private void chkLog_CheckedChanged(object sender, EventArgs e)
        {
            breakSliderGraph1.LogY = chkLog.Checked;
        }

        private void StatisticalBreaksDialog_Load(object sender, EventArgs e)
        {
            Array methods = Enum.GetValues(typeof(IntervalMethod));
            cmbInterval.Items.Clear();
            foreach (object method in methods)
            {
                cmbInterval.Items.Add(method);
            }
            cmbInterval.SelectedItem = IntervalMethod.EqualInterval;
        }

        private void cmbInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            breakSliderGraph1.IntervalMethod = ((IntervalMethod)cmbInterval.SelectedItem);
            breakSliderGraph1.ResetBreaks(null);
            breakSliderGraph1.Invalidate();
        }
    }
}