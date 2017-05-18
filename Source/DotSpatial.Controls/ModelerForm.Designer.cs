using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DotSpatial.Modeling.Forms;

namespace DotSpatial.Controls
{
    public partial class ModelerForm
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(ModelerForm));
            this._toolStripContainer = new ToolStripContainer();
            this._modeler = new Modeler();
            this._modelerToolStrip = new ModelerToolStrip();
            this._modelerMenuStrip = new ModelerMenuStrip();
            this._toolStripMenuItem = new ToolStripMenuItem();
            this._toolStripContainer.ContentPanel.SuspendLayout();
            this._toolStripContainer.TopToolStripPanel.SuspendLayout();
            this._toolStripContainer.SuspendLayout();
            this.SuspendLayout();

            // _toolStripContainer
            resources.ApplyResources(this._toolStripContainer, "_toolStripContainer");

            // _toolStripContainer.BottomToolStripPanel
            resources.ApplyResources(this._toolStripContainer.BottomToolStripPanel, "_toolStripContainer.BottomToolStripPanel");

            // _toolStripContainer.ContentPanel
            resources.ApplyResources(this._toolStripContainer.ContentPanel, "_toolStripContainer.ContentPanel");
            this._toolStripContainer.ContentPanel.Controls.Add(this._modeler);

            // _toolStripContainer.LeftToolStripPanel
            resources.ApplyResources(this._toolStripContainer.LeftToolStripPanel, "_toolStripContainer.LeftToolStripPanel");
            this._toolStripContainer.Name = "_toolStripContainer";

            // _toolStripContainer.RightToolStripPanel
            resources.ApplyResources(this._toolStripContainer.RightToolStripPanel, "_toolStripContainer.RightToolStripPanel");

            // _toolStripContainer.TopToolStripPanel
            resources.ApplyResources(this._toolStripContainer.TopToolStripPanel, "_toolStripContainer.TopToolStripPanel");
            this._toolStripContainer.TopToolStripPanel.Controls.Add(this._modelerMenuStrip);
            this._toolStripContainer.TopToolStripPanel.Controls.Add(this._modelerToolStrip);

            // _modeler
            resources.ApplyResources(this._modeler, "_modeler");
            this._modeler.AllowDrop = true;
            this._modeler.BackColor = System.Drawing.Color.FromArgb((int)(((byte)(192))), (int)(((byte)(192))), (int)(((byte)(255))));
            this._modeler.BorderStyle = BorderStyle.Fixed3D;
            this._modeler.Cursor = Cursors.Default;
            this._modeler.DataColor = System.Drawing.Color.LightGreen;
            this._modeler.DataFont = new System.Drawing.Font("Tahoma", 8F);
            this._modeler.DataShape = ModelShape.Ellipse;
            this._modeler.DefaultFileExtension = "mwm";
            this._modeler.DrawingQuality = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this._modeler.EnableLinking = false;
            this._modeler.IsInitialized = true;
            this._modeler.MaxExecutionThreads = 2;
            this._modeler.ModelFilename = null;
            this._modeler.Name = "_modeler";
            this._modeler.ShowWaterMark = true;
            this._modeler.ToolColor = System.Drawing.Color.Khaki;
            this._modeler.ToolFont = new System.Drawing.Font("Tahoma", 8F);
            this._modeler.ToolManager = null;
            this._modeler.ToolShape = ModelShape.Rectangle;
            this._modeler.WorkingPath = null;
            this._modeler.ZoomFactor = 1F;

            // _modelerToolStrip
            resources.ApplyResources(this._modelerToolStrip, "_modelerToolStrip");
            this._modelerToolStrip.Modeler = this._modeler;
            this._modelerToolStrip.Name = "_modelerToolStrip";

            // _modelerMenuStrip
            resources.ApplyResources(this._modelerMenuStrip, "_modelerMenuStrip");
            this._modelerMenuStrip.Name = "_modelerMenuStrip";

            // _toolStripMenuItem
            resources.ApplyResources(this._toolStripMenuItem, "_toolStripMenuItem");
            this._toolStripMenuItem.Name = "_toolStripMenuItem";

            // ModelerForm
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this._toolStripContainer);
            this.Icon = DotSpatial.Controls.Images.NewModel;
            this.Name = "ModelerForm";
            this._toolStripContainer.ContentPanel.ResumeLayout(false);
            this._toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this._toolStripContainer.TopToolStripPanel.PerformLayout();
            this._toolStripContainer.ResumeLayout(false);
            this._toolStripContainer.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private Modeler _modeler;
        private ModelerMenuStrip _modelerMenuStrip;
        private ModelerToolStrip _modelerToolStrip;
        private ToolStripContainer _toolStripContainer;
        private ToolStripMenuItem _toolStripMenuItem;
    }
}