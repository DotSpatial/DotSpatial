namespace BasicMapAndLegend
{
    partial class frmMapWithLegend
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
            this.legend1 = new DotSpatial.Controls.Legend();
            this.map1 = new DotSpatial.Controls.Map();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // legend1
            // 
            this.legend1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.legend1.BackColor = System.Drawing.Color.White;
            this.legend1.ControlRectangle = new System.Drawing.Rectangle(0, 0, 266, 366);
            this.legend1.DocumentRectangle = new System.Drawing.Rectangle(0, 0, 266, 392);
            this.legend1.HorizontalScrollEnabled = true;
            this.legend1.Indentation = 30;
            this.legend1.IsInitialized = false;
            this.legend1.Location = new System.Drawing.Point(12, 38);
            this.legend1.MinimumSize = new System.Drawing.Size(5, 5);
            this.legend1.Name = "legend1";
            this.legend1.ProgressHandler = null;
            this.legend1.ResetOnResize = false;
            this.legend1.SelectionFontColor = System.Drawing.Color.Black;
            this.legend1.SelectionHighlight = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(238)))), ((int)(((byte)(252)))));
            this.legend1.Size = new System.Drawing.Size(266, 366);
            this.legend1.TabIndex = 0;
            this.legend1.Text = "legend1";
            this.legend1.UseLegendForSelection = true;
            this.legend1.VerticalScrollEnabled = true;
            // 
            // map1
            // 
            this.map1.AllowDrop = true;
            this.map1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.map1.BackColor = System.Drawing.Color.White;
            this.map1.CollisionDetection = false;
            this.map1.ExtendBuffer = false;
            this.map1.FunctionMode = DotSpatial.Controls.FunctionMode.None;
            this.map1.IsBusy = false;
            this.map1.IsZoomedToMaxExtent = false;
            this.map1.Legend = this.legend1;
            this.map1.Location = new System.Drawing.Point(299, 38);
            this.map1.Name = "map1";
            this.map1.ProgressHandler = null;
            this.map1.ProjectionModeDefine = DotSpatial.Controls.ActionMode.Prompt;
            this.map1.ProjectionModeReproject = DotSpatial.Controls.ActionMode.Prompt;
            this.map1.RedrawLayersWhileResizing = false;
            this.map1.SelectionEnabled = true;
            this.map1.Size = new System.Drawing.Size(405, 366);
            this.map1.TabIndex = 1;
            this.map1.ZoomOutFartherThanMaxExtent = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(560, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "DotSpatial Example Project: Basic Map with Legend. Try putting data in the map by" +
    " dragging a shapefile into the map.";
            // 
            // frmMapWithLegend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(722, 425);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.map1);
            this.Controls.Add(this.legend1);
            this.Name = "frmMapWithLegend";
            this.Text = "Basic Map With Legend";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DotSpatial.Controls.Legend legend1;
        private DotSpatial.Controls.Map map1;
        private System.Windows.Forms.Label label1;
    }
}

