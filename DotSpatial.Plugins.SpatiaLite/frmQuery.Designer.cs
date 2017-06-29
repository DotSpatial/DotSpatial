namespace DotSpatial.Plugins.SpatiaLite
{
    partial class frmQuery
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
            this.btnRunQuery = new System.Windows.Forms.Button();
            this.treeTables = new System.Windows.Forms.TreeView();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pnlTitle = new System.Windows.Forms.Panel();
            this.lblTreeTitle = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dgQueryResult = new System.Windows.Forms.DataGridView();
            this.btnAddToMap = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgQueryResult)).BeginInit();
            this.SuspendLayout();
            // 
            // btnRunQuery
            // 
            this.btnRunQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRunQuery.Location = new System.Drawing.Point(323, 81);
            this.btnRunQuery.Name = "btnRunQuery";
            this.btnRunQuery.Size = new System.Drawing.Size(100, 22);
            this.btnRunQuery.TabIndex = 4;
            this.btnRunQuery.Text = "Run Query";
            this.btnRunQuery.UseVisualStyleBackColor = true;
            this.btnRunQuery.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // treeTables
            // 
            this.treeTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeTables.Location = new System.Drawing.Point(0, 0);
            this.treeTables.Name = "treeTables";
            this.treeTables.Size = new System.Drawing.Size(215, 367);
            this.treeTables.TabIndex = 5;
            // 
            // txtQuery
            // 
            this.txtQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQuery.Location = new System.Drawing.Point(3, 3);
            this.txtQuery.Multiline = true;
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(420, 72);
            this.txtQuery.TabIndex = 6;
            this.txtQuery.Text = "Type the query here..";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pnlTitle);
            this.splitContainer1.Panel1.Controls.Add(this.treeTables);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(645, 367);
            this.splitContainer1.SplitterDistance = 215;
            this.splitContainer1.TabIndex = 7;
            // 
            // pnlTitle
            // 
            this.pnlTitle.Controls.Add(this.lblTreeTitle);
            this.pnlTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlTitle.Name = "pnlTitle";
            this.pnlTitle.Size = new System.Drawing.Size(215, 21);
            this.pnlTitle.TabIndex = 6;
            // 
            // lblTreeTitle
            // 
            this.lblTreeTitle.AutoSize = true;
            this.lblTreeTitle.Location = new System.Drawing.Point(4, 3);
            this.lblTreeTitle.Name = "lblTreeTitle";
            this.lblTreeTitle.Size = new System.Drawing.Size(88, 13);
            this.lblTreeTitle.TabIndex = 0;
            this.lblTreeTitle.Text = "Database Tables";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.txtQuery);
            this.splitContainer2.Panel1.Controls.Add(this.btnRunQuery);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btnAddToMap);
            this.splitContainer2.Panel2.Controls.Add(this.dgQueryResult);
            this.splitContainer2.Size = new System.Drawing.Size(426, 367);
            this.splitContainer2.SplitterDistance = 106;
            this.splitContainer2.TabIndex = 0;
            // 
            // dgQueryResult
            // 
            this.dgQueryResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgQueryResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgQueryResult.Location = new System.Drawing.Point(4, 4);
            this.dgQueryResult.Name = "dgQueryResult";
            this.dgQueryResult.Size = new System.Drawing.Size(419, 219);
            this.dgQueryResult.TabIndex = 0;
            // 
            // btnAddToMap
            // 
            this.btnAddToMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddToMap.Location = new System.Drawing.Point(323, 229);
            this.btnAddToMap.Name = "btnAddToMap";
            this.btnAddToMap.Size = new System.Drawing.Size(100, 22);
            this.btnAddToMap.TabIndex = 7;
            this.btnAddToMap.Text = "Add To Map";
            this.btnAddToMap.UseVisualStyleBackColor = true;
            this.btnAddToMap.Click += new System.EventHandler(this.btnAddToMap_Click);
            // 
            // frmQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 367);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmQuery";
            this.Text = "SpatiaLite Query";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnlTitle.ResumeLayout(false);
            this.pnlTitle.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgQueryResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnRunQuery;
        private System.Windows.Forms.TreeView treeTables;
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel pnlTitle;
        private System.Windows.Forms.Label lblTreeTitle;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridView dgQueryResult;
        private System.Windows.Forms.Button btnAddToMap;
    }
}