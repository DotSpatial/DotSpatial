// ********************************************************************************************************
// Product Name: TestViewer.exe
// Description:  A very basic demonstration of the controls.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Initial Developer of this Original Code is Ted Dunsford. Created during refactoring 2010.
// ********************************************************************************************************
using DotSpatial.Controls;

namespace DemoMap
{
    /// <summary>
    /// Form
    /// </summary>
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.appManager = new DotSpatial.Controls.AppManager();
            this.legend1 = new DotSpatial.Controls.Legend();
            this.map1 = new DotSpatial.Controls.Map();
            this.toolManager1 = new DotSpatial.Controls.ToolManager();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabLegend = new System.Windows.Forms.TabPage();
            this.tabTools = new System.Windows.Forms.TabPage();
            this.toolManagerToolStrip1 = new DotSpatial.Controls.ToolManagerToolStrip();
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabLegend.SuspendLayout();
            this.tabTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // appManager
            // 
            this.appManager.Directories = ((System.Collections.Generic.List<string>)(resources.GetObject("appManager.Directories")));
            this.appManager.Legend = this.legend1;
            this.appManager.Map = this.map1;
            this.appManager.ShowExtensionsDialog = DotSpatial.Controls.ShowExtensionsDialog.Default;
            // 
            // legend1
            // 
            this.legend1.BackColor = System.Drawing.Color.White;
            this.legend1.ControlRectangle = new System.Drawing.Rectangle(0, 0, 213, 474);
            this.legend1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.legend1.DocumentRectangle = new System.Drawing.Rectangle(0, 0, 34, 114);
            this.legend1.HorizontalScrollEnabled = true;
            this.legend1.Indentation = 30;
            this.legend1.IsInitialized = false;
            this.legend1.Location = new System.Drawing.Point(3, 3);
            this.legend1.MinimumSize = new System.Drawing.Size(5, 5);
            this.legend1.Name = "legend1";
            this.legend1.ProgressHandler = null;
            this.legend1.ResetOnResize = false;
            this.legend1.SelectionFontColor = System.Drawing.Color.Black;
            this.legend1.SelectionHighlight = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.legend1.Size = new System.Drawing.Size(213, 474);
            this.legend1.TabIndex = 0;
            this.legend1.Text = "legend1";
            this.legend1.VerticalScrollEnabled = true;
            // 
            // map1
            // 
            this.map1.AllowDrop = true;
            this.map1.BackColor = System.Drawing.Color.White;
            this.map1.CollectAfterDraw = false;
            this.map1.CollisionDetection = true;
            this.map1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.map1.ExtendBuffer = false;
            this.map1.FunctionMode = DotSpatial.Controls.FunctionMode.None;
            this.map1.IsBusy = false;
            this.map1.Legend = this.legend1;
            this.map1.Location = new System.Drawing.Point(0, 0);
            this.map1.Name = "map1";
            this.map1.ProgressHandler = null;
            this.map1.ProjectionModeDefine = DotSpatial.Controls.ActionMode.PromptOnce;
            this.map1.ProjectionModeReproject = DotSpatial.Controls.ActionMode.PromptOnce;
            this.map1.RedrawLayersWhileResizing = false;
            this.map1.SelectionEnabled = true;
            this.map1.Size = new System.Drawing.Size(626, 506);
            this.map1.TabIndex = 0;
            // 
            // toolManager1
            // 
            this.toolManager1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolManager1.ImageIndex = 0;
            this.toolManager1.Legend = this.legend1;
            this.toolManager1.Location = new System.Drawing.Point(3, 3);
            this.toolManager1.Name = "toolManager1";
            this.toolManager1.SelectedImageIndex = 0;
            this.toolManager1.Size = new System.Drawing.Size(213, 474);
            this.toolManager1.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.map1);
            this.splitContainer1.Size = new System.Drawing.Size(857, 506);
            this.splitContainer1.SplitterDistance = 227;
            this.splitContainer1.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabLegend);
            this.tabControl1.Controls.Add(this.tabTools);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(227, 506);
            this.tabControl1.TabIndex = 0;
            // 
            // tabLegend
            // 
            this.tabLegend.Controls.Add(this.legend1);
            this.tabLegend.Location = new System.Drawing.Point(4, 22);
            this.tabLegend.Name = "tabLegend";
            this.tabLegend.Padding = new System.Windows.Forms.Padding(3);
            this.tabLegend.Size = new System.Drawing.Size(219, 480);
            this.tabLegend.TabIndex = 0;
            this.tabLegend.Text = "Legend";
            this.tabLegend.UseVisualStyleBackColor = true;
            // 
            // tabTools
            // 
            this.tabTools.Controls.Add(this.toolManagerToolStrip1);
            this.tabTools.Controls.Add(this.toolManager1);
            this.tabTools.Location = new System.Drawing.Point(4, 22);
            this.tabTools.Name = "tabTools";
            this.tabTools.Padding = new System.Windows.Forms.Padding(3);
            this.tabTools.Size = new System.Drawing.Size(219, 480);
            this.tabTools.TabIndex = 1;
            this.tabTools.Text = "Toolbox";
            this.tabTools.UseVisualStyleBackColor = true;
            // 
            // toolManagerToolStrip1
            // 
            this.toolManagerToolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolManagerToolStrip1.Location = new System.Drawing.Point(3, 452);
            this.toolManagerToolStrip1.Name = "toolManagerToolStrip1";
            this.toolManagerToolStrip1.Size = new System.Drawing.Size(213, 25);
            this.toolManagerToolStrip1.TabIndex = 1;
            this.toolManagerToolStrip1.Text = "toolManagerToolStrip1";
            this.toolManagerToolStrip1.ToolManager = toolManager1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 506);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "DemoMap";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabLegend.ResumeLayout(false);
            this.tabTools.ResumeLayout(false);
            this.tabTools.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AppManager appManager;
        private Legend legend1;
        private Map map1;
        private ToolManager toolManager1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabLegend;
        private System.Windows.Forms.TabPage tabTools;
        private System.Windows.Forms.ToolTip ttHelp;
        private ToolManagerToolStrip toolManagerToolStrip1;
    }
}