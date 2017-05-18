using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Plugins.Measure
{
    public partial class MeasureDialog
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MeasureDialog));
            this.toolStrip1 = new ToolStrip();
            this.tsbDistance = new ToolStripButton();
            this.tsbArea = new ToolStripButton();
            this.tsbClear = new ToolStripButton();
            this.ttHelp = new ToolTip(this.components);
            this.lblPartialValue = new Label();
            this.lblTotalValue = new Label();
            this.cmbUnits = new ComboBox();
            this.label1 = new Label();
            this.lblMeasure = new Label();
            this.label2 = new Label();
            this.label3 = new Label();
            this.lblTotalUnits = new Label();
            this.label4 = new Label();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();

            // toolStrip1
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.tsbDistance, this.tsbArea, this.tsbClear });
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";

            // tsbDistance
            this.tsbDistance.Checked = true;
            this.tsbDistance.CheckOnClick = true;
            this.tsbDistance.CheckState = CheckState.Checked;
            this.tsbDistance.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbDistance.Image = Properties.Resources.Line;
            resources.ApplyResources(this.tsbDistance, "tsbDistance");
            this.tsbDistance.Name = "tsbDistance";
            this.tsbDistance.Click += new EventHandler(this.DistanceButtonClick);

            // tsbArea
            this.tsbArea.CheckOnClick = true;
            this.tsbArea.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbArea.Image = Properties.Resources.Area;
            resources.ApplyResources(this.tsbArea, "tsbArea");
            this.tsbArea.Name = "tsbArea";
            this.tsbArea.Click += new EventHandler(this.AreaButtonClick);

            // tsbClear
            this.tsbClear.DisplayStyle = ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.tsbClear, "tsbClear");
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Click += new EventHandler(this.TsbClearClick);

            // lblPartialValue
            resources.ApplyResources(this.lblPartialValue, "lblPartialValue");
            this.lblPartialValue.Name = "lblPartialValue";
            this.ttHelp.SetToolTip(this.lblPartialValue, resources.GetString("lblPartialValue.ToolTip"));

            // lblTotalValue
            resources.ApplyResources(this.lblTotalValue, "lblTotalValue");
            this.lblTotalValue.Name = "lblTotalValue";
            this.ttHelp.SetToolTip(this.lblTotalValue, resources.GetString("lblTotalValue.ToolTip"));

            // cmbUnits
            this.cmbUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbUnits.FormattingEnabled = true;
            resources.ApplyResources(this.cmbUnits, "cmbUnits");
            this.cmbUnits.Name = "cmbUnits";
            this.ttHelp.SetToolTip(this.cmbUnits, resources.GetString("cmbUnits.ToolTip"));
            this.cmbUnits.SelectedIndexChanged += new EventHandler(this.CmbUnitsSelectedIndexChanged);

            // label1
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";

            // lblMeasure
            resources.ApplyResources(this.lblMeasure, "lblMeasure");
            this.lblMeasure.Name = "lblMeasure";

            // label2
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";

            // label3
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";

            // lblTotalUnits
            this.lblTotalUnits.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.lblTotalUnits, "lblTotalUnits");
            this.lblTotalUnits.Name = "lblTotalUnits";

            // label4
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";

            // MeasureDialog
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblTotalUnits);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbUnits);
            this.Controls.Add(this.lblTotalValue);
            this.Controls.Add(this.lblPartialValue);
            this.Controls.Add(this.lblMeasure);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MeasureDialog";
            this.ShowIcon = false;
            this.TopMost = true;
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private ComboBox cmbUnits;
        private Label label1;
        private Label label2;
        private Label label3;

        private Label label4;
        private Label lblMeasure;
        private Label lblPartialValue;
        private Label lblTotalUnits;
        private Label lblTotalValue;
        private ToolStrip toolStrip1;
        private ToolStripButton tsbArea;
        private ToolStripButton tsbClear;
        private ToolStripButton tsbDistance;
        private ToolTip ttHelp;
    }
}