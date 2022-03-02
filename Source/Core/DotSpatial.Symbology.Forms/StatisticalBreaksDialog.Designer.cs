using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class StatisticalBreaksDialog
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
            this.chkShowMean.CheckedChanged += new System.EventHandler(this.ChkShowMeanCheckedChanged);
            //
            // chkShowStd
            //
            resources.ApplyResources(this.chkShowStd, "chkShowStd");
            this.chkShowStd.Name = "chkShowStd";
            this.chkShowStd.UseVisualStyleBackColor = true;
            this.chkShowStd.CheckedChanged += new System.EventHandler(this.ChkShowStdCheckedChanged);
            //
            // nudColumns
            //
            this.nudColumns.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            resources.ApplyResources(this.nudColumns, "nudColumns");
            this.nudColumns.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.nudColumns.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            this.nudColumns.Name = "nudColumns";
            this.nudColumns.Value = new decimal(new int[] { 100, 0, 0, 0 });
            this.nudColumns.ValueChanged += new System.EventHandler(this.NudColumnsValueChanged);
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
            this.chkLog.CheckedChanged += new System.EventHandler(this.ChkLogCheckedChanged);
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
            this.cmbInterval.SelectedIndexChanged += new System.EventHandler(this.CmbIntervalSelectedIndexChanged);
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
            this.Load += new System.EventHandler(this.StatisticalBreaksDialogLoad);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudColumns)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

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
    }
}