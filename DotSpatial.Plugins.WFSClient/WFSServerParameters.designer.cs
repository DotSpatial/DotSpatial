namespace WFSPlugin
{
    partial class WFSServerParameters
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
            this.label1 = new System.Windows.Forms.Label();
            this.uxServer = new System.Windows.Forms.TextBox();
            this.uxGetCapabilities = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.uxListServer = new System.Windows.Forms.ComboBox();
            this.uxGroupWPS = new System.Windows.Forms.GroupBox();
            this.uxTabWfs = new System.Windows.Forms.TabControl();
            this.uxInformation = new System.Windows.Forms.TabPage();
            this.uxInfo = new System.Windows.Forms.TextBox();
            this.uxLayers = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.uxLayer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.uxLayersList = new System.Windows.Forms.DataGridView();
            this.uxAttributes = new System.Windows.Forms.TabPage();
            this.uxAttributesGrid = new System.Windows.Forms.DataGridView();
            this.uxOptions = new System.Windows.Forms.TabPage();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.uxFilter = new System.Windows.Forms.TabPage();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.uxDescription = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.uxRequest = new System.Windows.Forms.TextBox();
            this.uxOutput = new System.Windows.Forms.TextBox();
            this.uxVersion = new System.Windows.Forms.Label();
            this.uxOpen = new System.Windows.Forms.Button();
            this.uxGeographicField = new System.Windows.Forms.Label();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.groupBox1.SuspendLayout();
            this.uxGroupWPS.SuspendLayout();
            this.uxTabWfs.SuspendLayout();
            this.uxInformation.SuspendLayout();
            this.uxLayers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uxLayersList)).BeginInit();
            this.uxAttributes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uxAttributesGrid)).BeginInit();
            this.uxOptions.SuspendLayout();
            this.uxFilter.SuspendLayout();
            this.uxDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "List";
            // 
            // uxServer
            // 
            this.uxServer.Location = new System.Drawing.Point(68, 46);
            this.uxServer.Name = "uxServer";
            this.uxServer.Size = new System.Drawing.Size(401, 20);
            this.uxServer.TabIndex = 1;
            this.uxServer.Text = "http://www2.dmsolutions.ca/cgi-bin/mswfs_gmap?version=1.0.0&request=getcapabiliti" +
                "es&service=wfs";
            // 
            // uxGetCapabilities
            // 
            this.uxGetCapabilities.Location = new System.Drawing.Point(478, 43);
            this.uxGetCapabilities.Name = "uxGetCapabilities";
            this.uxGetCapabilities.Size = new System.Drawing.Size(91, 23);
            this.uxGetCapabilities.TabIndex = 2;
            this.uxGetCapabilities.Text = "Connect";
            this.uxGetCapabilities.UseVisualStyleBackColor = true;
            this.uxGetCapabilities.Click += new System.EventHandler(this.uxGetCapabilities_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.uxListServer);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.uxGetCapabilities);
            this.groupBox1.Controls.Add(this.uxServer);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(611, 110);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Server  url";
            // 
            // uxListServer
            // 
            this.uxListServer.FormattingEnabled = true;
            this.uxListServer.Items.AddRange(new object[] {
            "http://ogi.state.ok.us/geoserver/wfs",
            "http://10.141.11.221:8080/geoserver/wfs"});
            this.uxListServer.Location = new System.Drawing.Point(36, 19);
            this.uxListServer.Name = "uxListServer";
            this.uxListServer.Size = new System.Drawing.Size(533, 21);
            this.uxListServer.TabIndex = 3;
            this.uxListServer.SelectedIndexChanged += new System.EventHandler(this.uxListServer_SelectedIndexChanged);
            // 
            // uxGroupWPS
            // 
            this.uxGroupWPS.Controls.Add(this.uxTabWfs);
            this.uxGroupWPS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxGroupWPS.Enabled = false;
            this.uxGroupWPS.Location = new System.Drawing.Point(0, 0);
            this.uxGroupWPS.Name = "uxGroupWPS";
            this.uxGroupWPS.Size = new System.Drawing.Size(611, 321);
            this.uxGroupWPS.TabIndex = 4;
            this.uxGroupWPS.TabStop = false;
            this.uxGroupWPS.Text = "WFS";
            // 
            // uxTabWfs
            // 
            this.uxTabWfs.Controls.Add(this.uxInformation);
            this.uxTabWfs.Controls.Add(this.uxLayers);
            this.uxTabWfs.Controls.Add(this.uxAttributes);
            this.uxTabWfs.Controls.Add(this.uxOptions);
            this.uxTabWfs.Controls.Add(this.uxFilter);
            this.uxTabWfs.Controls.Add(this.uxDescription);
            this.uxTabWfs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxTabWfs.Location = new System.Drawing.Point(3, 16);
            this.uxTabWfs.Name = "uxTabWfs";
            this.uxTabWfs.SelectedIndex = 0;
            this.uxTabWfs.Size = new System.Drawing.Size(605, 302);
            this.uxTabWfs.TabIndex = 0;
            // 
            // uxInformation
            // 
            this.uxInformation.Controls.Add(this.uxInfo);
            this.uxInformation.Location = new System.Drawing.Point(4, 22);
            this.uxInformation.Name = "uxInformation";
            this.uxInformation.Padding = new System.Windows.Forms.Padding(3);
            this.uxInformation.Size = new System.Drawing.Size(597, 276);
            this.uxInformation.TabIndex = 0;
            this.uxInformation.Text = "Information";
            this.uxInformation.UseVisualStyleBackColor = true;
            // 
            // uxInfo
            // 
            this.uxInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxInfo.Location = new System.Drawing.Point(3, 3);
            this.uxInfo.Multiline = true;
            this.uxInfo.Name = "uxInfo";
            this.uxInfo.Size = new System.Drawing.Size(591, 270);
            this.uxInfo.TabIndex = 0;
            // 
            // uxLayers
            // 
            this.uxLayers.Controls.Add(this.splitContainer1);
            this.uxLayers.Location = new System.Drawing.Point(4, 22);
            this.uxLayers.Name = "uxLayers";
            this.uxLayers.Padding = new System.Windows.Forms.Padding(3);
            this.uxLayers.Size = new System.Drawing.Size(597, 276);
            this.uxLayers.TabIndex = 1;
            this.uxLayers.Text = "Layers";
            this.uxLayers.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.uxLayer);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.uxLayersList);
            this.splitContainer1.Size = new System.Drawing.Size(591, 270);
            this.splitContainer1.SplitterDistance = 43;
            this.splitContainer1.TabIndex = 1;
            // 
            // uxLayer
            // 
            this.uxLayer.Location = new System.Drawing.Point(53, 11);
            this.uxLayer.Name = "uxLayer";
            this.uxLayer.Size = new System.Drawing.Size(226, 20);
            this.uxLayer.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Layer";
            // 
            // uxLayersList
            // 
            this.uxLayersList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.uxLayersList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxLayersList.Location = new System.Drawing.Point(0, 0);
            this.uxLayersList.Name = "uxLayersList";
            this.uxLayersList.Size = new System.Drawing.Size(591, 223);
            this.uxLayersList.TabIndex = 0;
            this.uxLayersList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.uxLayersList_CellContentClick);
            this.uxLayersList.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.uxLayersList_RowHeaderMouseClick);
            // 
            // uxAttributes
            // 
            this.uxAttributes.Controls.Add(this.uxAttributesGrid);
            this.uxAttributes.Location = new System.Drawing.Point(4, 22);
            this.uxAttributes.Name = "uxAttributes";
            this.uxAttributes.Size = new System.Drawing.Size(597, 276);
            this.uxAttributes.TabIndex = 2;
            this.uxAttributes.Text = "Fields";
            this.uxAttributes.UseVisualStyleBackColor = true;
            // 
            // uxAttributesGrid
            // 
            this.uxAttributesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.uxAttributesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxAttributesGrid.Location = new System.Drawing.Point(0, 0);
            this.uxAttributesGrid.Name = "uxAttributesGrid";
            this.uxAttributesGrid.Size = new System.Drawing.Size(597, 276);
            this.uxAttributesGrid.TabIndex = 0;
            this.uxAttributesGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.uxAttributesGrid_CellContentClick);
            this.uxAttributesGrid.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.uxAttributesGrid_RowHeaderMouseClick);
            // 
            // uxOptions
            // 
            this.uxOptions.Controls.Add(this.textBox3);
            this.uxOptions.Location = new System.Drawing.Point(4, 22);
            this.uxOptions.Name = "uxOptions";
            this.uxOptions.Size = new System.Drawing.Size(597, 276);
            this.uxOptions.TabIndex = 3;
            this.uxOptions.Text = "Options";
            this.uxOptions.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox3.Location = new System.Drawing.Point(0, 0);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(597, 276);
            this.textBox3.TabIndex = 1;
            // 
            // uxFilter
            // 
            this.uxFilter.Controls.Add(this.textBox2);
            this.uxFilter.Location = new System.Drawing.Point(4, 22);
            this.uxFilter.Name = "uxFilter";
            this.uxFilter.Size = new System.Drawing.Size(597, 276);
            this.uxFilter.TabIndex = 4;
            this.uxFilter.Text = "Filter";
            this.uxFilter.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox2.Location = new System.Drawing.Point(0, 0);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(597, 276);
            this.textBox2.TabIndex = 1;
            // 
            // uxDescription
            // 
            this.uxDescription.Controls.Add(this.splitContainer4);
            this.uxDescription.Location = new System.Drawing.Point(4, 22);
            this.uxDescription.Name = "uxDescription";
            this.uxDescription.Size = new System.Drawing.Size(597, 276);
            this.uxDescription.TabIndex = 5;
            this.uxDescription.Text = "Output";
            this.uxDescription.UseVisualStyleBackColor = true;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.uxRequest);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.uxOutput);
            this.splitContainer4.Size = new System.Drawing.Size(597, 276);
            this.splitContainer4.SplitterDistance = 37;
            this.splitContainer4.TabIndex = 2;
            // 
            // uxRequest
            // 
            this.uxRequest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxRequest.Location = new System.Drawing.Point(0, 0);
            this.uxRequest.Multiline = true;
            this.uxRequest.Name = "uxRequest";
            this.uxRequest.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.uxRequest.Size = new System.Drawing.Size(597, 37);
            this.uxRequest.TabIndex = 1;
            // 
            // uxOutput
            // 
            this.uxOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxOutput.Location = new System.Drawing.Point(0, 0);
            this.uxOutput.Multiline = true;
            this.uxOutput.Name = "uxOutput";
            this.uxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.uxOutput.Size = new System.Drawing.Size(597, 235);
            this.uxOutput.TabIndex = 0;
            // 
            // uxVersion
            // 
            this.uxVersion.AutoSize = true;
            this.uxVersion.Location = new System.Drawing.Point(14, 13);
            this.uxVersion.Name = "uxVersion";
            this.uxVersion.Size = new System.Drawing.Size(64, 13);
            this.uxVersion.TabIndex = 5;
            this.uxVersion.Text = "Server type:";
            // 
            // uxOpen
            // 
            this.uxOpen.Enabled = false;
            this.uxOpen.Location = new System.Drawing.Point(463, 8);
            this.uxOpen.Name = "uxOpen";
            this.uxOpen.Size = new System.Drawing.Size(106, 22);
            this.uxOpen.TabIndex = 6;
            this.uxOpen.Text = "Open";
            this.uxOpen.UseVisualStyleBackColor = true;
            this.uxOpen.Click += new System.EventHandler(this.uxOpen_Click);
            // 
            // uxGeographicField
            // 
            this.uxGeographicField.AutoSize = true;
            this.uxGeographicField.Location = new System.Drawing.Point(141, 13);
            this.uxGeographicField.Name = "uxGeographicField";
            this.uxGeographicField.Size = new System.Drawing.Size(87, 13);
            this.uxGeographicField.TabIndex = 7;
            this.uxGeographicField.Text = "Geographic field:";
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
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(611, 486);
            this.splitContainer2.SplitterDistance = 110;
            this.splitContainer2.TabIndex = 8;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.uxGroupWPS);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.uxVersion);
            this.splitContainer3.Panel2.Controls.Add(this.uxGeographicField);
            this.splitContainer3.Panel2.Controls.Add(this.uxOpen);
            this.splitContainer3.Size = new System.Drawing.Size(611, 372);
            this.splitContainer3.SplitterDistance = 321;
            this.splitContainer3.TabIndex = 9;
            // 
            // WFSServerParameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 486);
            this.Controls.Add(this.splitContainer2);
            this.Name = "WFSServerParameters";
            this.Text = "WFS Client";
            this.Load += new System.EventHandler(this.WFSServerParameters_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.uxGroupWPS.ResumeLayout(false);
            this.uxTabWfs.ResumeLayout(false);
            this.uxInformation.ResumeLayout(false);
            this.uxInformation.PerformLayout();
            this.uxLayers.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uxLayersList)).EndInit();
            this.uxAttributes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uxAttributesGrid)).EndInit();
            this.uxOptions.ResumeLayout(false);
            this.uxOptions.PerformLayout();
            this.uxFilter.ResumeLayout(false);
            this.uxFilter.PerformLayout();
            this.uxDescription.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox uxServer;
        private System.Windows.Forms.Button uxGetCapabilities;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox uxGroupWPS;
        private System.Windows.Forms.TabControl uxTabWfs;
        private System.Windows.Forms.TabPage uxInformation;
        private System.Windows.Forms.TabPage uxLayers;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView uxLayersList;
        private System.Windows.Forms.TabPage uxAttributes;
        private System.Windows.Forms.TabPage uxOptions;
        private System.Windows.Forms.TabPage uxFilter;
        private System.Windows.Forms.TextBox uxLayer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label uxVersion;
        private System.Windows.Forms.Button uxOpen;
        private System.Windows.Forms.TextBox uxInfo;
        private System.Windows.Forms.DataGridView uxAttributesGrid;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox uxListServer;
        private System.Windows.Forms.Label uxGeographicField;
        private System.Windows.Forms.TabPage uxDescription;
        private System.Windows.Forms.TextBox uxOutput;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TextBox uxRequest;
        private System.Windows.Forms.SplitContainer splitContainer4;
    }
}

