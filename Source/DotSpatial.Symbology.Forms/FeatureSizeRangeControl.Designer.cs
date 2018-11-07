using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class FeatureSizeRangeControl
    {
        private System.ComponentModel.ComponentResourceManager resources;

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
            resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureSizeRangeControl));
            this.nudStart = new System.Windows.Forms.NumericUpDown();
            this.nudEnd = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.grpSizeRange = new System.Windows.Forms.GroupBox();
            this.chkSizeRange = new System.Windows.Forms.CheckBox();
            this.lsvStart = new DotSpatial.Symbology.Forms.LineSymbolView();
            this.psvStart = new DotSpatial.Symbology.Forms.PointSymbolView();
            this.lsvEnd = new DotSpatial.Symbology.Forms.LineSymbolView();
            this.trkEnd = new System.Windows.Forms.TrackBar();
            this.btnEdit = new System.Windows.Forms.Button();
            this.trkStart = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.psvEnd = new DotSpatial.Symbology.Forms.PointSymbolView();
            ((System.ComponentModel.ISupportInitialize)(this.nudStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEnd)).BeginInit();
            this.grpSizeRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkEnd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkStart)).BeginInit();
            this.SuspendLayout();
            // 
            // nudStart
            // 
            resources.ApplyResources(this.nudStart, "nudStart");
            this.nudStart.Maximum = new decimal(new int[] { 128, 0, 0, 0 });
            this.nudStart.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudStart.Name = "nudStart";
            this.nudStart.Value = new decimal(new int[] { 5, 0, 0, 0 });
            this.nudStart.ValueChanged += new System.EventHandler(this.NudStartValueChanged);
            // 
            // nudEnd
            // 
            resources.ApplyResources(this.nudEnd, "nudEnd");
            this.nudEnd.Maximum = new decimal(new int[] { 128, 0, 0, 0 });
            this.nudEnd.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudEnd.Name = "nudEnd";
            this.nudEnd.Value = new decimal(new int[] { 20, 0, 0, 0 });
            this.nudEnd.ValueChanged += new System.EventHandler(this.NudEndValueChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // grpSizeRange
            // 
            this.grpSizeRange.Controls.Add(this.chkSizeRange);
            this.grpSizeRange.Controls.Add(this.lsvStart);
            this.grpSizeRange.Controls.Add(this.psvStart);
            this.grpSizeRange.Controls.Add(this.lsvEnd);
            this.grpSizeRange.Controls.Add(this.trkEnd);
            this.grpSizeRange.Controls.Add(this.btnEdit);
            this.grpSizeRange.Controls.Add(this.trkStart);
            this.grpSizeRange.Controls.Add(this.label2);
            this.grpSizeRange.Controls.Add(this.nudStart);
            this.grpSizeRange.Controls.Add(this.nudEnd);
            this.grpSizeRange.Controls.Add(this.label1);
            this.grpSizeRange.Controls.Add(this.psvEnd);
            resources.ApplyResources(this.grpSizeRange, "grpSizeRange");
            this.grpSizeRange.Name = "grpSizeRange";
            this.grpSizeRange.TabStop = false;
            // 
            // chkSizeRange
            // 
            resources.ApplyResources(this.chkSizeRange, "chkSizeRange");
            this.chkSizeRange.Name = "chkSizeRange";
            this.chkSizeRange.UseVisualStyleBackColor = true;
            this.chkSizeRange.CheckedChanged += new System.EventHandler(this.ChkSizeRangeCheckedChanged);
            // 
            // lsvStart
            // 
            this.lsvStart.BackColor = System.Drawing.Color.White;
            this.lsvStart.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lsvStart, "lsvStart");
            this.lsvStart.Name = "lsvStart";
            // 
            // psvStart
            // 
            this.psvStart.BackColor = System.Drawing.Color.White;
            this.psvStart.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.psvStart, "psvStart");
            this.psvStart.Name = "psvStart";
            // 
            // lsvEnd
            // 
            this.lsvEnd.BackColor = System.Drawing.Color.White;
            this.lsvEnd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lsvEnd, "lsvEnd");
            this.lsvEnd.Name = "lsvEnd";
            // 
            // trkEnd
            // 
            resources.ApplyResources(this.trkEnd, "trkEnd");
            this.trkEnd.Maximum = 128;
            this.trkEnd.Minimum = 1;
            this.trkEnd.Name = "trkEnd";
            this.trkEnd.TickFrequency = 16;
            this.trkEnd.Value = 20;
            this.trkEnd.Scroll += new System.EventHandler(this.TrkEndScroll);
            // 
            // btnEdit
            // 
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.BtnEditClick);
            // 
            // trkStart
            // 
            resources.ApplyResources(this.trkStart, "trkStart");
            this.trkStart.Maximum = 128;
            this.trkStart.Minimum = 1;
            this.trkStart.Name = "trkStart";
            this.trkStart.TickFrequency = 16;
            this.trkStart.Value = 5;
            this.trkStart.Scroll += new System.EventHandler(this.TrkStartScroll);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // psvEnd
            // 
            this.psvEnd.BackColor = System.Drawing.Color.White;
            this.psvEnd.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.psvEnd, "psvEnd");
            this.psvEnd.Name = "psvEnd";
            // 
            // FeatureSizeRangeControl
            // 
            this.Controls.Add(this.grpSizeRange);
            this.Name = "FeatureSizeRangeControl";
            resources.ApplyResources(this, "$this");
            ((System.ComponentModel.ISupportInitialize)(this.nudStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEnd)).EndInit();
            this.grpSizeRange.ResumeLayout(false);
            this.grpSizeRange.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkEnd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkStart)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private Button btnEdit;
        private CheckBox chkSizeRange;
        private GroupBox grpSizeRange;
        private Label label1;
        private Label label2;
        private NumericUpDown nudEnd;
        private NumericUpDown nudStart;
        private LineSymbolView lsvEnd;
        private LineSymbolView lsvStart;
        private PointSymbolView psvEnd;
        private PointSymbolView psvStart;
        private TrackBar trkEnd;
        private TrackBar trkStart;

    }
}