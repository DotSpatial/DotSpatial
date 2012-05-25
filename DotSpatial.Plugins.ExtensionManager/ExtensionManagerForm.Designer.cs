namespace DotSpatial.Plugins.ExtensionManager
{
    partial class ExtensionManagerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtensionManagerForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grpApps = new System.Windows.Forms.GroupBox();
            this.clbApps = new System.Windows.Forms.CheckedListBox();
            this.grpData = new System.Windows.Forms.GroupBox();
            this.clbData = new System.Windows.Forms.CheckedListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.uxShowExtensionsFolder = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabOnline = new System.Windows.Forms.TabPage();
            this.uxSearchText = new System.Windows.Forms.TextBox();
            this.uxClear = new System.Windows.Forms.Button();
            this.uxSearch = new System.Windows.Forms.Button();
            this.extensionDescription = new System.Windows.Forms.Label();
            this.uxUpdate = new System.Windows.Forms.Button();
            this.uxUninstall = new System.Windows.Forms.Button();
            this.uxInstall = new System.Windows.Forms.Button();
            this.uxPackages = new System.Windows.Forms.ListBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.grpApps.SuspendLayout();
            this.grpData.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabOnline.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabOnline);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(658, 362);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(650, 336);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Installed Extensions";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grpApps);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grpData);
            this.splitContainer1.Size = new System.Drawing.Size(644, 294);
            this.splitContainer1.SplitterDistance = 297;
            this.splitContainer1.TabIndex = 4;
            // 
            // grpApps
            // 
            this.grpApps.Controls.Add(this.clbApps);
            this.grpApps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpApps.Location = new System.Drawing.Point(0, 0);
            this.grpApps.Name = "grpApps";
            this.grpApps.Size = new System.Drawing.Size(297, 294);
            this.grpApps.TabIndex = 0;
            this.grpApps.TabStop = false;
            this.grpApps.Text = "Apps";
            // 
            // clbApps
            // 
            this.clbApps.CheckOnClick = true;
            this.clbApps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbApps.FormattingEnabled = true;
            this.clbApps.Location = new System.Drawing.Point(3, 16);
            this.clbApps.Name = "clbApps";
            this.clbApps.Size = new System.Drawing.Size(291, 275);
            this.clbApps.TabIndex = 0;
            this.clbApps.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbApps_ItemCheck);
            // 
            // grpData
            // 
            this.grpData.Controls.Add(this.clbData);
            this.grpData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpData.Location = new System.Drawing.Point(0, 0);
            this.grpData.Name = "grpData";
            this.grpData.Size = new System.Drawing.Size(343, 294);
            this.grpData.TabIndex = 1;
            this.grpData.TabStop = false;
            this.grpData.Text = "Data Extensions";
            // 
            // clbData
            // 
            this.clbData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbData.Enabled = false;
            this.clbData.FormattingEnabled = true;
            this.clbData.Location = new System.Drawing.Point(3, 16);
            this.clbData.Name = "clbData";
            this.clbData.Size = new System.Drawing.Size(337, 275);
            this.clbData.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.uxShowExtensionsFolder);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 297);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(644, 36);
            this.panel1.TabIndex = 5;
            // 
            // uxShowExtensionsFolder
            // 
            this.uxShowExtensionsFolder.Location = new System.Drawing.Point(5, 6);
            this.uxShowExtensionsFolder.Name = "uxShowExtensionsFolder";
            this.uxShowExtensionsFolder.Size = new System.Drawing.Size(259, 23);
            this.uxShowExtensionsFolder.TabIndex = 3;
            this.uxShowExtensionsFolder.Text = "Show Extensions Folder";
            this.uxShowExtensionsFolder.UseVisualStyleBackColor = true;
            this.uxShowExtensionsFolder.Click += new System.EventHandler(this.uxShowExtensionsFolder_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(581, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tabOnline
            // 
            this.tabOnline.Controls.Add(this.uxSearchText);
            this.tabOnline.Controls.Add(this.uxClear);
            this.tabOnline.Controls.Add(this.uxSearch);
            this.tabOnline.Controls.Add(this.extensionDescription);
            this.tabOnline.Controls.Add(this.uxUpdate);
            this.tabOnline.Controls.Add(this.uxUninstall);
            this.tabOnline.Controls.Add(this.uxInstall);
            this.tabOnline.Controls.Add(this.uxPackages);
            this.tabOnline.Location = new System.Drawing.Point(4, 22);
            this.tabOnline.Name = "tabOnline";
            this.tabOnline.Padding = new System.Windows.Forms.Padding(3);
            this.tabOnline.Size = new System.Drawing.Size(650, 336);
            this.tabOnline.TabIndex = 1;
            this.tabOnline.Text = "Online";
            this.tabOnline.UseVisualStyleBackColor = true;
            // 
            // uxSearchText
            // 
            this.uxSearchText.Location = new System.Drawing.Point(87, 9);
            this.uxSearchText.Name = "uxSearchText";
            this.uxSearchText.Size = new System.Drawing.Size(156, 20);
            this.uxSearchText.TabIndex = 12;
            this.uxSearchText.TextChanged += new System.EventHandler(this.uxSearchText_TextChanged);
            this.uxSearchText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uxSearchText_KeyDown);
            // 
            // uxClear
            // 
            this.uxClear.Location = new System.Drawing.Point(250, 6);
            this.uxClear.Name = "uxClear";
            this.uxClear.Size = new System.Drawing.Size(75, 23);
            this.uxClear.TabIndex = 11;
            this.uxClear.Text = "Clear";
            this.uxClear.UseVisualStyleBackColor = true;
            this.uxClear.Click += new System.EventHandler(this.clear_Click);
            // 
            // uxSearch
            // 
            this.uxSearch.Location = new System.Drawing.Point(3, 7);
            this.uxSearch.Name = "uxSearch";
            this.uxSearch.Size = new System.Drawing.Size(75, 23);
            this.uxSearch.TabIndex = 7;
            this.uxSearch.Text = "Search";
            this.uxSearch.UseVisualStyleBackColor = true;
            this.uxSearch.Click += new System.EventHandler(this.search_Click);
            // 
            // extensionDescription
            // 
            this.extensionDescription.Location = new System.Drawing.Point(398, 42);
            this.extensionDescription.Name = "extensionDescription";
            this.extensionDescription.Size = new System.Drawing.Size(245, 289);
            this.extensionDescription.TabIndex = 5;
            // 
            // uxUpdate
            // 
            this.uxUpdate.Enabled = false;
            this.uxUpdate.Location = new System.Drawing.Point(568, 6);
            this.uxUpdate.Name = "uxUpdate";
            this.uxUpdate.Size = new System.Drawing.Size(75, 23);
            this.uxUpdate.TabIndex = 3;
            this.uxUpdate.Text = "Update";
            this.uxUpdate.UseVisualStyleBackColor = true;
            this.uxUpdate.Click += new System.EventHandler(this.uxUpdate_Click);
            // 
            // uxUninstall
            // 
            this.uxUninstall.Enabled = false;
            this.uxUninstall.Location = new System.Drawing.Point(487, 6);
            this.uxUninstall.Name = "uxUninstall";
            this.uxUninstall.Size = new System.Drawing.Size(75, 23);
            this.uxUninstall.TabIndex = 3;
            this.uxUninstall.Text = "Uninstall";
            this.uxUninstall.UseVisualStyleBackColor = true;
            this.uxUninstall.Click += new System.EventHandler(this.uxUninstall_Click);
            // 
            // uxInstall
            // 
            this.uxInstall.Enabled = false;
            this.uxInstall.Location = new System.Drawing.Point(406, 6);
            this.uxInstall.Name = "uxInstall";
            this.uxInstall.Size = new System.Drawing.Size(75, 23);
            this.uxInstall.TabIndex = 3;
            this.uxInstall.Text = "Install";
            this.uxInstall.UseVisualStyleBackColor = true;
            this.uxInstall.Click += new System.EventHandler(this.InstallButton_Click);
            // 
            // uxPackages
            // 
            this.uxPackages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.uxPackages.FormattingEnabled = true;
            this.uxPackages.Location = new System.Drawing.Point(3, 42);
            this.uxPackages.Name = "uxPackages";
            this.uxPackages.Size = new System.Drawing.Size(319, 290);
            this.uxPackages.TabIndex = 2;
            this.uxPackages.SelectedValueChanged += new System.EventHandler(this.uxPackages_SelectedValueChanged);
            // 
            // ExtensionManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 362);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExtensionManagerForm";
            this.Text = "Extension Manager";
            this.Load += new System.EventHandler(this.PackageManagerForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.grpApps.ResumeLayout(false);
            this.grpData.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabOnline.ResumeLayout(false);
            this.tabOnline.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox grpApps;
        private System.Windows.Forms.CheckedListBox clbApps;
        private System.Windows.Forms.GroupBox grpData;
        private System.Windows.Forms.CheckedListBox clbData;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabPage tabOnline;
        private System.Windows.Forms.Button uxInstall;
        private System.Windows.Forms.ListBox uxPackages;
        private System.Windows.Forms.Label extensionDescription;
        private System.Windows.Forms.Button uxShowExtensionsFolder;
        private System.Windows.Forms.Button uxUpdate;
        private System.Windows.Forms.Button uxUninstall;
        private System.Windows.Forms.Button uxSearch;
        private System.Windows.Forms.Button uxClear;
        private System.Windows.Forms.TextBox uxSearchText;

    }
}