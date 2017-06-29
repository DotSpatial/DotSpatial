using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    partial class LayoutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutForm));
            this._toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this._splitContainer1 = new System.Windows.Forms.SplitContainer();
            this._splitContainer2 = new System.Windows.Forms.SplitContainer();
            this._layoutControl1 = new DotSpatial.Controls.LayoutControl();
            this._layoutDocToolStrip1 = new DotSpatial.Controls.LayoutDocToolStrip();
            this._layoutInsertToolStrip1 = new DotSpatial.Controls.LayoutInsertToolStrip();
            this._layoutListBox1 = new DotSpatial.Controls.LayoutListBox();
            this._layoutMapToolStrip1 = new DotSpatial.Controls.LayoutMapToolStrip();
            this._layoutMenuStrip1 = new DotSpatial.Controls.LayoutMenuStrip();
            this._layoutPropertyGrid1 = new DotSpatial.Controls.LayoutPropertyGrid();
            this._layoutZoomToolStrip1 = new DotSpatial.Controls.LayoutZoomToolStrip();
            this._toolStripContainer1.ContentPanel.SuspendLayout();
            this._toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this._toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer1)).BeginInit();
            this._splitContainer1.Panel1.SuspendLayout();
            this._splitContainer1.Panel2.SuspendLayout();
            this._splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer2)).BeginInit();
            this._splitContainer2.Panel1.SuspendLayout();
            this._splitContainer2.Panel2.SuspendLayout();
            this._splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // _toolStripContainer1
            // 
            // 
            // _toolStripContainer1.ContentPanel
            // 
            this._toolStripContainer1.ContentPanel.Controls.Add(this._splitContainer1);
            resources.ApplyResources(this._toolStripContainer1.ContentPanel, "_toolStripContainer1.ContentPanel");
            resources.ApplyResources(this._toolStripContainer1, "_toolStripContainer1");
            this._toolStripContainer1.Name = "_toolStripContainer1";
            // 
            // _toolStripContainer1.TopToolStripPanel
            // 
            this._toolStripContainer1.TopToolStripPanel.Controls.Add(this._layoutDocToolStrip1);
            this._toolStripContainer1.TopToolStripPanel.Controls.Add(this._layoutZoomToolStrip1);
            this._toolStripContainer1.TopToolStripPanel.Controls.Add(this._layoutInsertToolStrip1);
            this._toolStripContainer1.TopToolStripPanel.Controls.Add(this._layoutMapToolStrip1);
            // 
            // _splitContainer1
            // 
            resources.ApplyResources(this._splitContainer1, "_splitContainer1");
            this._splitContainer1.Name = "_splitContainer1";
            // 
            // _splitContainer1.Panel1
            // 
            this._splitContainer1.Panel1.Controls.Add(this._layoutControl1);
            // 
            // _splitContainer1.Panel2
            // 
            this._splitContainer1.Panel2.Controls.Add(this._splitContainer2);
            // 
            // _splitContainer2
            // 
            resources.ApplyResources(this._splitContainer2, "_splitContainer2");
            this._splitContainer2.Name = "_splitContainer2";
            // 
            // _splitContainer2.Panel1
            // 
            this._splitContainer2.Panel1.Controls.Add(this._layoutListBox1);
            // 
            // _splitContainer2.Panel2
            // 
            this._splitContainer2.Panel2.Controls.Add(this._layoutPropertyGrid1);
            // 
            // _layoutControl1
            // 
            this._layoutControl1.Cursor = System.Windows.Forms.Cursors.Default;
            resources.ApplyResources(this._layoutControl1, "_layoutControl1");
            this._layoutControl1.DrawingQuality = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this._layoutControl1.Filename = "";
            this._layoutControl1.LayoutDocToolStrip = this._layoutDocToolStrip1;
            this._layoutControl1.LayoutInsertToolStrip = this._layoutInsertToolStrip1;
            this._layoutControl1.LayoutListBox = this._layoutListBox1;
            this._layoutControl1.LayoutMapToolStrip = this._layoutMapToolStrip1;
            this._layoutControl1.LayoutMenuStrip = this._layoutMenuStrip1;
            this._layoutControl1.LayoutPropertyGrip = this._layoutPropertyGrid1;
            this._layoutControl1.LayoutZoomToolStrip = this._layoutZoomToolStrip1;
            this._layoutControl1.MapPanMode = false;
            this._layoutControl1.Name = "_layoutControl1";
            this._layoutControl1.ShowMargin = false;
            this._layoutControl1.Zoom = 0.3541667F;
            this._layoutControl1.FilenameChanged += new System.EventHandler(this.layoutControl1_FilenameChanged);
            // 
            // _layoutDocToolStrip1
            // 
            resources.ApplyResources(this._layoutDocToolStrip1, "_layoutDocToolStrip1");
            this._layoutDocToolStrip1.LayoutControl = this._layoutControl1;
            this._layoutDocToolStrip1.Name = "_layoutDocToolStrip1";
            // 
            // _layoutInsertToolStrip1
            // 
            resources.ApplyResources(this._layoutInsertToolStrip1, "_layoutInsertToolStrip1");
            this._layoutInsertToolStrip1.LayoutControl = this._layoutControl1;
            this._layoutInsertToolStrip1.Name = "_layoutInsertToolStrip1";
            // 
            // _layoutListBox1
            // 
            this._layoutListBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this._layoutListBox1, "_layoutListBox1");
            this._layoutListBox1.LayoutControl = this._layoutControl1;
            this._layoutListBox1.Name = "_layoutListBox1";
            // 
            // _layoutMapToolStrip1
            // 
            resources.ApplyResources(this._layoutMapToolStrip1, "_layoutMapToolStrip1");
            this._layoutMapToolStrip1.LayoutControl = this._layoutControl1;
            this._layoutMapToolStrip1.Name = "_layoutMapToolStrip1";
            // 
            // _layoutMenuStrip1
            // 
            this._layoutMenuStrip1.LayoutControl = this._layoutControl1;
            resources.ApplyResources(this._layoutMenuStrip1, "_layoutMenuStrip1");
            this._layoutMenuStrip1.Name = "_layoutMenuStrip1";
            this._layoutMenuStrip1.CloseClicked += new System.EventHandler(this.layoutMenuStrip1_CloseClicked);
            // 
            // _layoutPropertyGrid1
            // 
            this._layoutPropertyGrid1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this._layoutPropertyGrid1, "_layoutPropertyGrid1");
            this._layoutPropertyGrid1.LayoutControl = this._layoutControl1;
            this._layoutPropertyGrid1.Name = "_layoutPropertyGrid1";
            // 
            // _layoutZoomToolStrip1
            // 
            resources.ApplyResources(this._layoutZoomToolStrip1, "_layoutZoomToolStrip1");
            this._layoutZoomToolStrip1.LayoutControl = this._layoutControl1;
            this._layoutZoomToolStrip1.Name = "_layoutZoomToolStrip1";
            // 
            // LayoutForm
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this._toolStripContainer1);
            this.Controls.Add(this._layoutMenuStrip1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "LayoutForm";
            this.Load += new System.EventHandler(this.LayoutForm_Load);
            this._toolStripContainer1.ContentPanel.ResumeLayout(false);
            this._toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this._toolStripContainer1.TopToolStripPanel.PerformLayout();
            this._toolStripContainer1.ResumeLayout(false);
            this._toolStripContainer1.PerformLayout();
            this._splitContainer1.Panel1.ResumeLayout(false);
            this._splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer1)).EndInit();
            this._splitContainer1.ResumeLayout(false);
            this._splitContainer2.Panel1.ResumeLayout(false);
            this._splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer2)).EndInit();
            this._splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LayoutControl _layoutControl1;
        private LayoutDocToolStrip _layoutDocToolStrip1;
        private LayoutInsertToolStrip _layoutInsertToolStrip1;
        private LayoutListBox _layoutListBox1;
        private LayoutMapToolStrip _layoutMapToolStrip1;
        private LayoutMenuStrip _layoutMenuStrip1;
        private LayoutPropertyGrid _layoutPropertyGrid1;
        private LayoutZoomToolStrip _layoutZoomToolStrip1;
        private SplitContainer _splitContainer1;
        private SplitContainer _splitContainer2;
        private ToolStripContainer _toolStripContainer1;
    }
}
