using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class ColorPicker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ColorPicker));
            this.grpPreview = new System.Windows.Forms.GroupBox();
            this.lblPreview = new System.Windows.Forms.Label();
            this.dialogButtons1 = new DotSpatial.Symbology.Forms.DialogButtons();
            this.grdSlider = new DotSpatial.Symbology.Forms.GradientControl();
            this._spatialStatusStrip1 = new DotSpatial.Symbology.Forms.SymbologyStatusStrip();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.grpPreview.SuspendLayout();
            this._spatialStatusStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            //
            // grpPreview
            //
            resources.ApplyResources(this.grpPreview, "grpPreview");
            this.grpPreview.Controls.Add(this.lblPreview);
            this.helpProvider1.SetHelpString(this.grpPreview, resources.GetString("grpPreview.HelpString"));
            this.grpPreview.Name = "grpPreview";
            this.helpProvider1.SetShowHelp(this.grpPreview, ((bool)(resources.GetObject("grpPreview.ShowHelp"))));
            this.grpPreview.TabStop = false;
            //
            // lblPreview
            //
            this.lblPreview.BackColor = System.Drawing.Color.White;
            this.lblPreview.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lblPreview, "lblPreview");
            this.lblPreview.Name = "lblPreview";
            this.helpProvider1.SetShowHelp(this.lblPreview, ((bool)(resources.GetObject("lblPreview.ShowHelp"))));
            //
            // dialogButtons1
            //
            resources.ApplyResources(this.dialogButtons1, "dialogButtons1");
            this.dialogButtons1.Name = "dialogButtons1";
            this.helpProvider1.SetShowHelp(this.dialogButtons1, ((bool)(resources.GetObject("dialogButtons1.ShowHelp"))));
            //
            // grdSlider
            //
            resources.ApplyResources(this.grdSlider, "grdSlider");
            this.grdSlider.EndValue = 0.8F;
            this.helpProvider1.SetHelpString(this.grdSlider, resources.GetString("grdSlider.HelpString"));
            this.grdSlider.MaximumColor = System.Drawing.Color.Blue;
            this.grdSlider.MinimumColor = System.Drawing.Color.Lime;
            this.grdSlider.Name = "grdSlider";
            this.helpProvider1.SetShowHelp(this.grdSlider, ((bool)(resources.GetObject("grdSlider.ShowHelp"))));
            this.grdSlider.SlidersEnabled = true;
            this.grdSlider.StartValue = 0.2F;
            //
            // _spatialStatusStrip1
            //
            this._spatialStatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                                                                                                  this.statusText,
                                                                                                  this.statusProgress});
            resources.ApplyResources(this._spatialStatusStrip1, "_spatialStatusStrip1");
            this._spatialStatusStrip1.Name = "_spatialStatusStrip1";
            this.helpProvider1.SetShowHelp(this._spatialStatusStrip1, ((bool)(resources.GetObject("_spatialStatusStrip1.ShowHelp"))));
            //
            // statusText
            //
            this.statusText.Name = "statusText";
            resources.ApplyResources(this.statusText, "statusText");
            this.statusText.Spring = true;
            //
            // statusProgress
            //
            this.statusProgress.Name = "statusProgress";
            resources.ApplyResources(this.statusProgress, "statusProgress");
            //
            // tableLayoutPanel1
            //
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.dialogButtons1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.grdSlider, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.grpPreview, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.helpProvider1.SetShowHelp(this.tableLayoutPanel1, ((bool)(resources.GetObject("tableLayoutPanel1.ShowHelp"))));
            //
            // ColorPicker
            //
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this._spatialStatusStrip1);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ColorPicker";
            this.helpProvider1.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.ShowInTaskbar = false;
            this.grpPreview.ResumeLayout(false);
            this._spatialStatusStrip1.ResumeLayout(false);
            this._spatialStatusStrip1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private DialogButtons dialogButtons1;
        private GradientControl grdSlider;
        private GroupBox grpPreview;
        private HelpProvider helpProvider1;
        private Label lblPreview;
        private ToolStripProgressBar statusProgress;
        private ToolStripStatusLabel statusText;
        private TableLayoutPanel tableLayoutPanel1;
    }
}