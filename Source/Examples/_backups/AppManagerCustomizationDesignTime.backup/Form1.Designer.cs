using System.ComponentModel;
using System.Windows.Forms;
using DotSpatial.Controls;

namespace DotSpatial.Examples.AppManagerCustomizationDesignTime
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.item2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.spatialToolStripPanel2 = new DotSpatial.Controls.SpatialToolStripPanel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.spatialToolStripPanel1 = new DotSpatial.Controls.SpatialToolStripPanel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbLegend = new System.Windows.Forms.TabPage();
            this.legend1 = new DotSpatial.Controls.Legend();
            this.spatialStatusStrip1 = new DotSpatial.Controls.SpatialStatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tbMap = new System.Windows.Forms.TabPage();
            this.map1 = new DotSpatial.Controls.Map();
            this.appManager = new DotSpatial.Controls.AppManager();
            this.spatialDockManager1 = new DotSpatial.Controls.SpatialDockManager();
            this.spatialHeaderControl1 = new DotSpatial.Controls.SpatialHeaderControl();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.spatialToolStripPanel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbLegend.SuspendLayout();
            this.spatialStatusStrip1.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tbMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spatialDockManager1)).BeginInit();
            this.spatialDockManager1.Panel1.SuspendLayout();
            this.spatialDockManager1.Panel2.SuspendLayout();
            this.spatialDockManager1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spatialHeaderControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(901, 26);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.item2ToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(95, 22);
            this.toolStripMenuItem1.Text = "Custom menu";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(107, 22);
            this.toolStripMenuItem3.Text = "Item 1";
            // 
            // item2ToolStripMenuItem
            // 
            this.item2ToolStripMenuItem.Name = "item2ToolStripMenuItem";
            this.item2ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.item2ToolStripMenuItem.Text = "Item 2";
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Controls.Add(this.spatialToolStripPanel2);
            this.panel1.Controls.Add(this.spatialToolStripPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 26);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(901, 50);
            this.panel1.TabIndex = 4;
            // 
            // spatialToolStripPanel2
            // 
            this.spatialToolStripPanel2.Controls.Add(this.toolStrip1);
            this.spatialToolStripPanel2.Controls.Add(this.toolStrip2);
            this.spatialToolStripPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.spatialToolStripPanel2.Location = new System.Drawing.Point(0, 25);
            this.spatialToolStripPanel2.Name = "spatialToolStripPanel2";
            this.spatialToolStripPanel2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.spatialToolStripPanel2.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.spatialToolStripPanel2.Size = new System.Drawing.Size(901, 25);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(133, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(98, 22);
            this.toolStripLabel1.Text = "Design time label";
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripTextBox1,
            this.toolStripButton2});
            this.toolStrip2.Location = new System.Drawing.Point(144, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(137, 25);
            this.toolStrip2.TabIndex = 1;
            // 
            // toolStripTextBox1
            // 
            this.toolStripTextBox1.Name = "toolStripTextBox1";
            this.toolStripTextBox1.Size = new System.Drawing.Size(100, 25);
            this.toolStripTextBox1.Text = "Design time input";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "toolStripButton2";
            // 
            // spatialToolStripPanel1
            // 
            this.spatialToolStripPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.spatialToolStripPanel1.Location = new System.Drawing.Point(0, 0);
            this.spatialToolStripPanel1.Name = "spatialToolStripPanel1";
            this.spatialToolStripPanel1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.spatialToolStripPanel1.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.spatialToolStripPanel1.Size = new System.Drawing.Size(901, 25);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbLegend);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(300, 413);
            this.tabControl1.TabIndex = 0;
            // 
            // tbLegend
            // 
            this.tbLegend.Controls.Add(this.legend1);
            this.tbLegend.Location = new System.Drawing.Point(4, 22);
            this.tbLegend.Name = "tbLegend";
            this.tbLegend.Padding = new System.Windows.Forms.Padding(3);
            this.tbLegend.Size = new System.Drawing.Size(292, 387);
            this.tbLegend.TabIndex = 0;
            this.tbLegend.Text = "Legend";
            this.tbLegend.UseVisualStyleBackColor = true;
            // 
            // legend1
            // 
            this.legend1.BackColor = System.Drawing.Color.White;
            this.legend1.ControlRectangle = new System.Drawing.Rectangle(0, 0, 286, 381);
            this.legend1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.legend1.DocumentRectangle = new System.Drawing.Rectangle(0, 0, 187, 428);
            this.legend1.HorizontalScrollEnabled = true;
            this.legend1.Indentation = 30;
            this.legend1.IsInitialized = false;
            this.legend1.Location = new System.Drawing.Point(3, 3);
            this.legend1.MinimumSize = new System.Drawing.Size(5, 5);
            this.legend1.Name = "legend1";
            this.legend1.ProgressHandler = this.spatialStatusStrip1;
            this.legend1.ResetOnResize = false;
            this.legend1.SelectionFontColor = System.Drawing.Color.Black;
            this.legend1.SelectionHighlight = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.legend1.Size = new System.Drawing.Size(286, 381);
            this.legend1.TabIndex = 0;
            this.legend1.Text = "legend1";
            this.legend1.VerticalScrollEnabled = true;
            // 
            // spatialStatusStrip1
            // 
            this.spatialStatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel3});
            this.spatialStatusStrip1.Location = new System.Drawing.Point(0, 489);
            this.spatialStatusStrip1.Name = "spatialStatusStrip1";
            this.spatialStatusStrip1.ProgressBar = this.toolStripProgressBar1;
            this.spatialStatusStrip1.ProgressLabel = this.toolStripStatusLabel1;
            this.spatialStatusStrip1.Size = new System.Drawing.Size(901, 22);
            this.spatialStatusStrip1.TabIndex = 1;
            this.spatialStatusStrip1.Text = "spatialStatusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(108, 17);
            this.toolStripStatusLabel2.Text = "Design Time Status";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel1.Text = "Ready";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(637, 17);
            this.toolStripStatusLabel3.Spring = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tbMap);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(597, 413);
            this.tabControl2.TabIndex = 0;
            // 
            // tbMap
            // 
            this.tbMap.Controls.Add(this.map1);
            this.tbMap.Location = new System.Drawing.Point(4, 22);
            this.tbMap.Name = "tbMap";
            this.tbMap.Padding = new System.Windows.Forms.Padding(3);
            this.tbMap.Size = new System.Drawing.Size(589, 387);
            this.tbMap.TabIndex = 0;
            this.tbMap.Text = "Map";
            this.tbMap.UseVisualStyleBackColor = true;
            // 
            // map1
            // 
            this.map1.AllowDrop = true;
            this.map1.BackColor = System.Drawing.Color.White;
            this.map1.CollectAfterDraw = false;
            this.map1.CollisionDetection = false;
            this.map1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map1.ExtendBuffer = false;
            this.map1.FunctionMode = DotSpatial.Controls.FunctionMode.None;
            this.map1.IsBusy = false;
            this.map1.IsZoomedToMaxExtent = false;
            this.map1.Legend = this.legend1;
            this.map1.Location = new System.Drawing.Point(3, 3);
            this.map1.Name = "map1";
            this.map1.ProgressHandler = null;
            this.map1.ProjectionModeDefine = DotSpatial.Controls.ActionMode.Prompt;
            this.map1.ProjectionModeReproject = DotSpatial.Controls.ActionMode.Prompt;
            this.map1.RedrawLayersWhileResizing = false;
            this.map1.SelectionEnabled = true;
            this.map1.Size = new System.Drawing.Size(583, 381);
            this.map1.TabIndex = 0;
            // 
            // appManager
            // 
            this.appManager.Directories = ((System.Collections.Generic.List<string>)(resources.GetObject("appManager.Directories")));
            this.appManager.DockManager = this.spatialDockManager1;
            this.appManager.HeaderControl = this.spatialHeaderControl1;
            this.appManager.Legend = this.legend1;
            this.appManager.Map = this.map1;
            this.appManager.ProgressHandler = this.spatialStatusStrip1;
            // 
            // spatialDockManager1
            // 
            this.spatialDockManager1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spatialDockManager1.Location = new System.Drawing.Point(0, 76);
            this.spatialDockManager1.Name = "spatialDockManager1";
            // 
            // spatialDockManager1.Panel1
            // 
            this.spatialDockManager1.Panel1.Controls.Add(this.tabControl1);
            // 
            // spatialDockManager1.Panel2
            // 
            this.spatialDockManager1.Panel2.Controls.Add(this.tabControl2);
            this.spatialDockManager1.Size = new System.Drawing.Size(901, 413);
            this.spatialDockManager1.SplitterDistance = 300;
            this.spatialDockManager1.TabControl1 = this.tabControl1;
            this.spatialDockManager1.TabControl2 = this.tabControl2;
            this.spatialDockManager1.TabIndex = 5;
            // 
            // spatialHeaderControl1
            // 
            this.spatialHeaderControl1.ApplicationManager = this.appManager;
            this.spatialHeaderControl1.MenuStrip = this.menuStrip1;
            this.spatialHeaderControl1.ToolbarsContainer = this.spatialToolStripPanel1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(901, 511);
            this.Controls.Add(this.spatialDockManager1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.spatialStatusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.spatialToolStripPanel2.ResumeLayout(false);
            this.spatialToolStripPanel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tbLegend.ResumeLayout(false);
            this.spatialStatusStrip1.ResumeLayout(false);
            this.spatialStatusStrip1.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tbMap.ResumeLayout(false);
            this.spatialDockManager1.Panel1.ResumeLayout(false);
            this.spatialDockManager1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spatialDockManager1)).EndInit();
            this.spatialDockManager1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spatialHeaderControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AppManager appManager;
        private Map map1;
        private SpatialStatusStrip spatialStatusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel2;
        private ToolStripProgressBar toolStripProgressBar1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripStatusLabel toolStripStatusLabel3;
        private TabControl tabControl1;
        private TabPage tbLegend;
        private TabControl tabControl2;
        private TabPage tbMap;
        private Legend legend1;
        private SpatialHeaderControl spatialHeaderControl1;
        private MenuStrip menuStrip1;
        private SpatialToolStripPanel spatialToolStripPanel1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem3;
        private Panel panel1;
        private SpatialToolStripPanel spatialToolStripPanel2;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripLabel toolStripLabel1;
        private ToolStrip toolStrip2;
        private ToolStripButton toolStripButton2;
        private ToolStripTextBox toolStripTextBox1;
        private ToolStripMenuItem item2ToolStripMenuItem;
        private SpatialDockManager spatialDockManager1;
    }
}

