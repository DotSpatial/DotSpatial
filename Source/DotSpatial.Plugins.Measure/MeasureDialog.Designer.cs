using System;
using System.ComponentModel;
using System.Globalization;
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
        private static ComponentResourceManager resources = new ComponentResourceManager(typeof(MeasureDialog));

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
            this.toolStrip1.Name = "toolStrip1";

            // tsbDistance
            this.tsbDistance.Checked = true;
            this.tsbDistance.CheckOnClick = true;
            this.tsbDistance.CheckState = CheckState.Checked;
            this.tsbDistance.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbDistance.Image = Properties.Resources.Line;
            this.tsbDistance.Name = "tsbDistance";
            this.tsbDistance.Click += new EventHandler(this.DistanceButtonClick);

            // tsbArea
            this.tsbArea.CheckOnClick = true;
            this.tsbArea.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbArea.Image = Properties.Resources.Area;
            this.tsbArea.Name = "tsbArea";
            this.tsbArea.Click += new EventHandler(this.AreaButtonClick);

            // tsbClear
            this.tsbClear.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Click += new EventHandler(this.TsbClearClick);

            // lblPartialValue
            this.lblPartialValue.Name = "lblPartialValue";
            this.ttHelp.SetToolTip(this.lblPartialValue, resources.GetString("lblPartialValue.ToolTip"));

            // lblTotalValue
            this.lblTotalValue.Name = "lblTotalValue";
            this.ttHelp.SetToolTip(this.lblTotalValue, resources.GetString("lblTotalValue.ToolTip"));

            // cmbUnits
            this.cmbUnits.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbUnits.FormattingEnabled = true;
            this.cmbUnits.Name = "cmbUnits";
            this.ttHelp.SetToolTip(this.cmbUnits, resources.GetString("cmbUnits.ToolTip"));
            this.cmbUnits.SelectedIndexChanged += new EventHandler(this.CmbUnitsSelectedIndexChanged);

            // label1
            this.label1.Name = "label1";

            // lblMeasure
            this.lblMeasure.Name = "lblMeasure";

            // label2
            this.label2.Name = "label2";

            // label3
            this.label3.Name = "label3";

            // lblTotalUnits
            this.lblTotalUnits.BackColor = System.Drawing.SystemColors.Control;
            this.lblTotalUnits.Name = "lblTotalUnits";

            // label4
            this.label4.Name = "label4";

            // MeasureDialog
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

            resources.ApplyResources(this.lblTotalUnits, "lblTotalUnits");
            resources.ApplyResources(this.lblTotalValue, "lblTotalValue");
            resources.ApplyResources(this.lblPartialValue, "lblPartialValue");
        }

        private void UpdateResources()
        {
            resources.ApplyResources(this, "$this");
            resources.ApplyResources(this.label4, "label4");
            resources.ApplyResources(this.label3, "label3");
            resources.ApplyResources(this.label2, "label2");
            resources.ApplyResources(this.lblMeasure, "lblMeasure");
            resources.ApplyResources(this.label1, "label1");
            resources.ApplyResources(this.cmbUnits, "cmbUnits");
            resources.ApplyResources(this.tsbClear, "tsbClear");
            resources.ApplyResources(this.tsbArea, "tsbArea");
            resources.ApplyResources(this.tsbDistance, "tsbDistance");
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            /*_distanceUnitNames = new string[] { resources.GetString("cmbUnitsDist.Items"), resources.GetString("cmbUnitsDist.Items1"), resources.GetString("cmbUnitsDist.Items2"), resources.GetString("cmbUnitsDist.Items3"), resources.GetString("cmbUnitsDist.Items4"), resources.GetString("cmbUnitsDist.Items5"), resources.GetString("cmbUnitsDist.Items6"), resources.GetString("cmbUnitsDist.Items7"), resources.GetString("cmbUnitsDist.Items8"), resources.GetString("cmbUnitsDist.Items9"), resources.GetString("cmbUnitsDist.Items10") };*/
            for (int distUnitID = 0; distUnitID < _distanceUnitNames.Length; distUnitID++)
            {
                if (distUnitID == 0) _distanceUnitNames[distUnitID] = resources.GetString("cmbUnitsDist.Items");
                else _distanceUnitNames[distUnitID] = resources.GetString("cmbUnitsDist.Items" + distUnitID);
            }

            for (int areaUnitID = 0; areaUnitID < _areaUnitNames.Length; areaUnitID++)
            {
                if (areaUnitID == 0) _areaUnitNames[areaUnitID] = resources.GetString("cmbUnitsArea.Items");
                else _areaUnitNames[areaUnitID] = resources.GetString("cmbUnitsArea.Items" + areaUnitID);
            }

            switch (MeasureMode)
            {
                case MeasureMode.Distance:
                    _distanceUnitIndex = cmbUnits.SelectedIndex;
                    cmbUnits.Items.Clear();
                    cmbUnits.Items.AddRange(_distanceUnitNames);
                    cmbUnits.SelectedIndex = _distanceUnitIndex;
                    break;

                case MeasureMode.Area:
                    _areaUnitIndex = cmbUnits.SelectedIndex;
                    cmbUnits.Items.Clear();
                    cmbUnits.Items.AddRange(_areaUnitNames);
                    cmbUnits.SelectedIndex = _areaUnitIndex;
                    break;
            }

            CmbUnitsSelectedIndexChanged(this.cmbUnits, new EventArgs());

            Refresh();
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