namespace DotSpatial.Plugins.WebMap.WMS_New
{
    partial class WMSServerParameters
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
            this.lblQuerable = new System.Windows.Forms.Label();
            this.lblNoSubsets = new System.Windows.Forms.Label();
            this.lbCRS = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbStyles = new System.Windows.Forms.ListBox();
            this.lblStyles = new System.Windows.Forms.Label();
            this.lbBoundingBox = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblOpaque = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tbAbstract = new System.Windows.Forms.TextBox();
            this.gbSelectedLayer = new System.Windows.Forms.GroupBox();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tvLayers = new System.Windows.Forms.TreeView();
            this.lblServerURL = new System.Windows.Forms.Label();
            this.btnGetCapabilities = new System.Windows.Forms.Button();
            this.tbServerUrl = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.gbSelectedLayer.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblQuerable
            // 
            this.lblQuerable.AutoSize = true;
            this.lblQuerable.Location = new System.Drawing.Point(6, 53);
            this.lblQuerable.Name = "lblQuerable";
            this.lblQuerable.Size = new System.Drawing.Size(70, 13);
            this.lblQuerable.TabIndex = 14;
            this.lblQuerable.Text = "Querable: No";
            // 
            // lblNoSubsets
            // 
            this.lblNoSubsets.AutoSize = true;
            this.lblNoSubsets.Location = new System.Drawing.Point(6, 97);
            this.lblNoSubsets.Name = "lblNoSubsets";
            this.lblNoSubsets.Size = new System.Drawing.Size(79, 13);
            this.lblNoSubsets.TabIndex = 18;
            this.lblNoSubsets.Text = "NoSubsets: No";
            // 
            // lbCRS
            // 
            this.lbCRS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCRS.FormattingEnabled = true;
            this.lbCRS.Location = new System.Drawing.Point(9, 19);
            this.lbCRS.Name = "lbCRS";
            this.lbCRS.Size = new System.Drawing.Size(365, 69);
            this.lbCRS.TabIndex = 27;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "CRS:";
            // 
            // lbStyles
            // 
            this.lbStyles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbStyles.DisplayMember = "Title";
            this.lbStyles.FormattingEnabled = true;
            this.lbStyles.Location = new System.Drawing.Point(9, 24);
            this.lbStyles.Name = "lbStyles";
            this.lbStyles.Size = new System.Drawing.Size(365, 69);
            this.lbStyles.TabIndex = 26;
            // 
            // lblStyles
            // 
            this.lblStyles.AutoSize = true;
            this.lblStyles.Location = new System.Drawing.Point(3, 6);
            this.lblStyles.Name = "lblStyles";
            this.lblStyles.Size = new System.Drawing.Size(38, 13);
            this.lblStyles.TabIndex = 21;
            this.lblStyles.Text = "Styles:";
            // 
            // lbBoundingBox
            // 
            this.lbBoundingBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbBoundingBox.FormattingEnabled = true;
            this.lbBoundingBox.Location = new System.Drawing.Point(9, 22);
            this.lbBoundingBox.Name = "lbBoundingBox";
            this.lbBoundingBox.Size = new System.Drawing.Size(365, 69);
            this.lbBoundingBox.TabIndex = 29;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "Bounding Boxes:";
            // 
            // lblOpaque
            // 
            this.lblOpaque.AutoSize = true;
            this.lblOpaque.Location = new System.Drawing.Point(6, 75);
            this.lblOpaque.Name = "lblOpaque";
            this.lblOpaque.Size = new System.Drawing.Size(65, 13);
            this.lblOpaque.TabIndex = 17;
            this.lblOpaque.Text = "Opaque: No";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbBoundingBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(387, 103);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbStyles);
            this.panel2.Controls.Add(this.lblStyles);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 112);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(387, 103);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lbCRS);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 221);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(387, 109);
            this.panel3.TabIndex = 2;
            // 
            // tbAbstract
            // 
            this.tbAbstract.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAbstract.Location = new System.Drawing.Point(91, 53);
            this.tbAbstract.Multiline = true;
            this.tbAbstract.Name = "tbAbstract";
            this.tbAbstract.ReadOnly = true;
            this.tbAbstract.Size = new System.Drawing.Size(295, 57);
            this.tbAbstract.TabIndex = 31;
            // 
            // gbSelectedLayer
            // 
            this.gbSelectedLayer.Controls.Add(this.tbTitle);
            this.gbSelectedLayer.Controls.Add(this.label3);
            this.gbSelectedLayer.Controls.Add(this.lblQuerable);
            this.gbSelectedLayer.Controls.Add(this.tbAbstract);
            this.gbSelectedLayer.Controls.Add(this.lblNoSubsets);
            this.gbSelectedLayer.Controls.Add(this.lblOpaque);
            this.gbSelectedLayer.Controls.Add(this.tableLayoutPanel1);
            this.gbSelectedLayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSelectedLayer.Location = new System.Drawing.Point(250, 3);
            this.gbSelectedLayer.Name = "gbSelectedLayer";
            this.gbSelectedLayer.Size = new System.Drawing.Size(408, 470);
            this.gbSelectedLayer.TabIndex = 25;
            this.gbSelectedLayer.TabStop = false;
            this.gbSelectedLayer.Text = "Details";
            // 
            // tbTitle
            // 
            this.tbTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTitle.Location = new System.Drawing.Point(42, 19);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.ReadOnly = true;
            this.tbTitle.Size = new System.Drawing.Size(344, 20);
            this.tbTitle.TabIndex = 33;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "Title:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 131);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(393, 333);
            this.tableLayoutPanel1.TabIndex = 30;
            // 
            // tvLayers
            // 
            this.tvLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvLayers.HideSelection = false;
            this.tvLayers.Location = new System.Drawing.Point(3, 3);
            this.tvLayers.Name = "tvLayers";
            this.tvLayers.Size = new System.Drawing.Size(241, 470);
            this.tvLayers.TabIndex = 23;
            this.tvLayers.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvLayers_AfterSelect);
            // 
            // lblServerURL
            // 
            this.lblServerURL.AutoSize = true;
            this.lblServerURL.Location = new System.Drawing.Point(15, 29);
            this.lblServerURL.Name = "lblServerURL";
            this.lblServerURL.Size = new System.Drawing.Size(38, 13);
            this.lblServerURL.TabIndex = 22;
            this.lblServerURL.Text = "Server";
            // 
            // btnGetCapabilities
            // 
            this.btnGetCapabilities.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetCapabilities.Location = new System.Drawing.Point(571, 24);
            this.btnGetCapabilities.Name = "btnGetCapabilities";
            this.btnGetCapabilities.Size = new System.Drawing.Size(110, 23);
            this.btnGetCapabilities.TabIndex = 20;
            this.btnGetCapabilities.Text = "Get data";
            this.btnGetCapabilities.UseVisualStyleBackColor = true;
            this.btnGetCapabilities.Click += new System.EventHandler(this.btnGetCapabilities_Click);
            // 
            // tbServerUrl
            // 
            this.tbServerUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServerUrl.Location = new System.Drawing.Point(58, 26);
            this.tbServerUrl.Name = "tbServerUrl";
            this.tbServerUrl.Size = new System.Drawing.Size(495, 20);
            this.tbServerUrl.TabIndex = 19;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(606, 565);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(524, 565);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.5F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.5F));
            this.tableLayoutPanel2.Controls.Add(this.tvLayers, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.gbSelectedLayer, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(18, 68);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(661, 476);
            this.tableLayoutPanel2.TabIndex = 26;
            // 
            // WMSServerParameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 606);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.lblServerURL);
            this.Controls.Add(this.btnGetCapabilities);
            this.Controls.Add(this.tbServerUrl);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 500);
            this.Name = "WMSServerParameters";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WMS Server Parameters";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.gbSelectedLayer.ResumeLayout(false);
            this.gbSelectedLayer.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblQuerable;
        private System.Windows.Forms.Label lblNoSubsets;
        private System.Windows.Forms.ListBox lbCRS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lbStyles;
        private System.Windows.Forms.Label lblStyles;
        private System.Windows.Forms.ListBox lbBoundingBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblOpaque;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox tbAbstract;
        private System.Windows.Forms.GroupBox gbSelectedLayer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TreeView tvLayers;
        private System.Windows.Forms.Label lblServerURL;
        private System.Windows.Forms.Button btnGetCapabilities;
        private System.Windows.Forms.TextBox tbServerUrl;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.Label label3;
    }
}