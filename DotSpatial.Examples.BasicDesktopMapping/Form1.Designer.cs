namespace DotSpatial.Examples.BasicDesktopMapping
{
    public partial class Form1
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
            this.uxMap = new DotSpatial.Controls.Map();
            this.uxOpenFile = new System.Windows.Forms.Button();
            this.uxZoomIn = new System.Windows.Forms.Button();
            this.uxZoomWide = new System.Windows.Forms.Button();
            this.uxPan = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // uxMap
            // 
            this.uxMap.AllowDrop = true;
            this.uxMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxMap.BackColor = System.Drawing.Color.White;
            this.uxMap.CollectAfterDraw = false;
            this.uxMap.CollisionDetection = false;
            this.uxMap.ExtendBuffer = false;
            this.uxMap.FunctionMode = DotSpatial.Controls.FunctionMode.None;
            this.uxMap.IsBusy = false;
            this.uxMap.Legend = null;
            this.uxMap.Location = new System.Drawing.Point(12, 30);
            this.uxMap.Name = "uxMap";
            this.uxMap.ProgressHandler = null;
            this.uxMap.ProjectionModeDefine = DotSpatial.Controls.ActionMode.Prompt;
            this.uxMap.ProjectionModeReproject = DotSpatial.Controls.ActionMode.Prompt;
            this.uxMap.RedrawLayersWhileResizing = false;
            this.uxMap.SelectionEnabled = true;
            this.uxMap.Size = new System.Drawing.Size(560, 320);
            this.uxMap.TabIndex = 0;
            // 
            // uxOpenFile
            // 
            this.uxOpenFile.Location = new System.Drawing.Point(12, 1);
            this.uxOpenFile.Name = "uxOpenFile";
            this.uxOpenFile.Size = new System.Drawing.Size(75, 23);
            this.uxOpenFile.TabIndex = 1;
            this.uxOpenFile.Text = "Open File";
            this.uxOpenFile.UseVisualStyleBackColor = true;
            this.uxOpenFile.Click += new System.EventHandler(this.uxOpenFile_Click);
            // 
            // uxZoomIn
            // 
            this.uxZoomIn.Location = new System.Drawing.Point(93, 1);
            this.uxZoomIn.Name = "uxZoomIn";
            this.uxZoomIn.Size = new System.Drawing.Size(75, 23);
            this.uxZoomIn.TabIndex = 2;
            this.uxZoomIn.Text = "Zoom In";
            this.uxZoomIn.UseVisualStyleBackColor = true;
            this.uxZoomIn.Click += new System.EventHandler(this.uxZoomIn_Click);
            // 
            // uxZoomWide
            // 
            this.uxZoomWide.Location = new System.Drawing.Point(175, 1);
            this.uxZoomWide.Name = "uxZoomWide";
            this.uxZoomWide.Size = new System.Drawing.Size(75, 23);
            this.uxZoomWide.TabIndex = 3;
            this.uxZoomWide.Text = "Zoom Wide";
            this.uxZoomWide.UseVisualStyleBackColor = true;
            this.uxZoomWide.Click += new System.EventHandler(this.uxZoomWide_Click);
            // 
            // uxPan
            // 
            this.uxPan.Location = new System.Drawing.Point(257, 1);
            this.uxPan.Name = "uxPan";
            this.uxPan.Size = new System.Drawing.Size(75, 23);
            this.uxPan.TabIndex = 4;
            this.uxPan.Text = "Pan";
            this.uxPan.UseVisualStyleBackColor = true;
            this.uxPan.Click += new System.EventHandler(this.uxPan_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.uxPan);
            this.Controls.Add(this.uxZoomWide);
            this.Controls.Add(this.uxZoomIn);
            this.Controls.Add(this.uxOpenFile);
            this.Controls.Add(this.uxMap);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private DotSpatial.Controls.Map uxMap;
        private System.Windows.Forms.Button uxOpenFile;
        private System.Windows.Forms.Button uxZoomIn;
        private System.Windows.Forms.Button uxZoomWide;
        private System.Windows.Forms.Button uxPan;

    }
}