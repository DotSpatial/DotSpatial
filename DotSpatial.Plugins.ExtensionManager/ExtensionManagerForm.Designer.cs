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
            this.TabPage3 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.uxShowExtensionsFolder = new System.Windows.Forms.Button();
            this.Installed = new System.Windows.Forms.CheckedListBox();
            this.uxCategoryList = new System.Windows.Forms.ListBox();
            this.uxUninstall = new System.Windows.Forms.Button();
            this.tabOnline = new System.Windows.Forms.TabPage();
            this.uxDownloadStatus = new System.Windows.Forms.RichTextBox();
            this.uxInstallProgress = new System.Windows.Forms.ProgressBar();
            this.uxPackages = new System.Windows.Forms.ListView();
            this.Pack = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.uxFeedSelection = new System.Windows.Forms.ComboBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.uxSearchText = new System.Windows.Forms.TextBox();
            this.uxClear = new System.Windows.Forms.Button();
            this.uxSearch = new System.Windows.Forms.Button();
            this.uxInstall = new System.Windows.Forms.Button();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.uxUpdateAll = new System.Windows.Forms.Button();
            this.uxUpdate = new System.Windows.Forms.Button();
            this.uxUpdatePackages = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TabPage3.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabOnline.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabPage3
            // 
            this.TabPage3.Controls.Add(this.tabPage1);
            this.TabPage3.Controls.Add(this.tabOnline);
            this.TabPage3.Controls.Add(this.tabPage2);
            this.TabPage3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabPage3.Location = new System.Drawing.Point(0, 0);
            this.TabPage3.Name = "TabPage3";
            this.TabPage3.SelectedIndex = 0;
            this.TabPage3.Size = new System.Drawing.Size(737, 612);
            this.TabPage3.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.uxShowExtensionsFolder);
            this.tabPage1.Controls.Add(this.Installed);
            this.tabPage1.Controls.Add(this.uxCategoryList);
            this.tabPage1.Controls.Add(this.uxUninstall);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(651, 586);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Installed Extensions";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // uxShowExtensionsFolder
            // 
            this.uxShowExtensionsFolder.Location = new System.Drawing.Point(150, 6);
            this.uxShowExtensionsFolder.Name = "uxShowExtensionsFolder";
            this.uxShowExtensionsFolder.Size = new System.Drawing.Size(136, 23);
            this.uxShowExtensionsFolder.TabIndex = 9;
            this.uxShowExtensionsFolder.Text = "Show Extensions Folder";
            this.uxShowExtensionsFolder.UseVisualStyleBackColor = true;
            this.uxShowExtensionsFolder.Click += new System.EventHandler(this.uxShowExtensionsFolder_Click);
            // 
            // Installed
            // 
            this.Installed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Installed.CheckOnClick = true;
            this.Installed.FormattingEnabled = true;
            this.Installed.Location = new System.Drawing.Point(150, 45);
            this.Installed.Name = "Installed";
            this.Installed.Size = new System.Drawing.Size(492, 484);
            this.Installed.TabIndex = 6;
            this.Installed.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.Installed_ItemCheck);
            this.Installed.SelectedValueChanged += new System.EventHandler(this.Installed_SelectedValueChanged);
            // 
            // uxCategoryList
            // 
            this.uxCategoryList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.uxCategoryList.FormattingEnabled = true;
            this.uxCategoryList.Items.AddRange(new object[] {
            "All"});
            this.uxCategoryList.Location = new System.Drawing.Point(8, 12);
            this.uxCategoryList.Name = "uxCategoryList";
            this.uxCategoryList.Size = new System.Drawing.Size(136, 511);
            this.uxCategoryList.TabIndex = 8;
            this.uxCategoryList.SelectedIndexChanged += new System.EventHandler(this.uxCategoryList_SelectedIndexChanged);
            // 
            // uxUninstall
            // 
            this.uxUninstall.Enabled = false;
            this.uxUninstall.Location = new System.Drawing.Point(567, 6);
            this.uxUninstall.Name = "uxUninstall";
            this.uxUninstall.Size = new System.Drawing.Size(75, 23);
            this.uxUninstall.TabIndex = 7;
            this.uxUninstall.Text = "Uninstall";
            this.uxUninstall.UseVisualStyleBackColor = true;
            this.uxUninstall.Click += new System.EventHandler(this.uxUninstall_Click);
            // 
            // tabOnline
            // 
            this.tabOnline.Controls.Add(this.uxDownloadStatus);
            this.tabOnline.Controls.Add(this.uxInstallProgress);
            this.tabOnline.Controls.Add(this.uxPackages);
            this.tabOnline.Controls.Add(this.uxFeedSelection);
            this.tabOnline.Controls.Add(this.richTextBox1);
            this.tabOnline.Controls.Add(this.uxSearchText);
            this.tabOnline.Controls.Add(this.uxClear);
            this.tabOnline.Controls.Add(this.uxSearch);
            this.tabOnline.Controls.Add(this.uxInstall);
            this.tabOnline.Controls.Add(this.shapeContainer1);
            this.tabOnline.Location = new System.Drawing.Point(4, 22);
            this.tabOnline.Name = "tabOnline";
            this.tabOnline.Padding = new System.Windows.Forms.Padding(3);
            this.tabOnline.Size = new System.Drawing.Size(729, 586);
            this.tabOnline.TabIndex = 1;
            this.tabOnline.Text = "Online";
            this.tabOnline.UseVisualStyleBackColor = true;
            // 
            // uxDownloadStatus
            // 
            this.uxDownloadStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxDownloadStatus.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.uxDownloadStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uxDownloadStatus.Location = new System.Drawing.Point(349, 494);
            this.uxDownloadStatus.Name = "uxDownloadStatus";
            this.uxDownloadStatus.Size = new System.Drawing.Size(371, 66);
            this.uxDownloadStatus.TabIndex = 20;
            this.uxDownloadStatus.Text = "";
            // 
            // uxInstallProgress
            // 
            this.uxInstallProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxInstallProgress.Location = new System.Drawing.Point(349, 557);
            this.uxInstallProgress.Name = "uxInstallProgress";
            this.uxInstallProgress.Size = new System.Drawing.Size(371, 23);
            this.uxInstallProgress.TabIndex = 18;
            this.uxInstallProgress.Value = 50;
            // 
            // uxPackages
            // 
            this.uxPackages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.uxPackages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Pack,
            this.Description});
            this.uxPackages.FullRowSelect = true;
            this.uxPackages.Location = new System.Drawing.Point(3, 68);
            this.uxPackages.Name = "uxPackages";
            this.uxPackages.ShowGroups = false;
            this.uxPackages.Size = new System.Drawing.Size(326, 473);
            this.uxPackages.TabIndex = 17;
            this.uxPackages.UseCompatibleStateImageBehavior = false;
            this.uxPackages.View = System.Windows.Forms.View.Tile;
            this.uxPackages.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.SelectedItemChanged);
            // 
            // Pack
            // 
            this.Pack.Text = "Pack";
            this.Pack.Width = 250;
            // 
            // Description
            // 
            this.Description.Text = "Description";
            this.Description.Width = 1500;
            // 
            // uxFeedSelection
            // 
            this.uxFeedSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uxFeedSelection.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uxFeedSelection.FormattingEnabled = true;
            this.uxFeedSelection.Items.AddRange(new object[] {
            "http://www.myget.org/F/dotspatial/",
            "http://www.myget.org/F/dotspatialstaging/",
            "http://www.myget.org/F/hydrodesktop/"});
            this.uxFeedSelection.Location = new System.Drawing.Point(8, 10);
            this.uxFeedSelection.Name = "uxFeedSelection";
            this.uxFeedSelection.Size = new System.Drawing.Size(314, 23);
            this.uxFeedSelection.TabIndex = 15;
            this.uxFeedSelection.SelectedIndexChanged += new System.EventHandler(this.uxFeedSelection_SelectedIndexChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(349, 10);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(371, 568);
            this.richTextBox1.TabIndex = 14;
            this.richTextBox1.Text = "";
            // 
            // uxSearchText
            // 
            this.uxSearchText.Location = new System.Drawing.Point(89, 42);
            this.uxSearchText.Name = "uxSearchText";
            this.uxSearchText.Size = new System.Drawing.Size(156, 20);
            this.uxSearchText.TabIndex = 12;
            this.uxSearchText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uxSearchText_KeyDown);
            // 
            // uxClear
            // 
            this.uxClear.Location = new System.Drawing.Point(247, 40);
            this.uxClear.Name = "uxClear";
            this.uxClear.Size = new System.Drawing.Size(75, 23);
            this.uxClear.TabIndex = 11;
            this.uxClear.Text = "Clear";
            this.uxClear.UseVisualStyleBackColor = true;
            this.uxClear.Click += new System.EventHandler(this.uxClear_Click);
            // 
            // uxSearch
            // 
            this.uxSearch.Location = new System.Drawing.Point(8, 39);
            this.uxSearch.Name = "uxSearch";
            this.uxSearch.Size = new System.Drawing.Size(75, 23);
            this.uxSearch.TabIndex = 7;
            this.uxSearch.Text = "Search";
            this.uxSearch.UseVisualStyleBackColor = true;
            this.uxSearch.Click += new System.EventHandler(this.uxSearch_Click);
            // 
            // uxInstall
            // 
            this.uxInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.uxInstall.Enabled = false;
            this.uxInstall.Location = new System.Drawing.Point(8, 555);
            this.uxInstall.Name = "uxInstall";
            this.uxInstall.Size = new System.Drawing.Size(75, 23);
            this.uxInstall.TabIndex = 3;
            this.uxInstall.Text = "Install";
            this.uxInstall.UseVisualStyleBackColor = true;
            this.uxInstall.Click += new System.EventHandler(this.InstallButton_Click);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(3, 3);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(723, 580);
            this.shapeContainer1.TabIndex = 19;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lineShape1.BorderWidth = 2;
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 335;
            this.lineShape1.X2 = 335;
            this.lineShape1.Y1 = 0;
            this.lineShape1.Y2 = 583;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.uxUpdateAll);
            this.tabPage2.Controls.Add(this.uxUpdate);
            this.tabPage2.Controls.Add(this.uxUpdatePackages);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(651, 586);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "Update";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // uxUpdateAll
            // 
            this.uxUpdateAll.Location = new System.Drawing.Point(227, 555);
            this.uxUpdateAll.Name = "uxUpdateAll";
            this.uxUpdateAll.Size = new System.Drawing.Size(75, 23);
            this.uxUpdateAll.TabIndex = 3;
            this.uxUpdateAll.Text = "Update All";
            this.uxUpdateAll.UseVisualStyleBackColor = true;
            this.uxUpdateAll.Click += new System.EventHandler(this.uxUpdateAll_Click);
            // 
            // uxUpdate
            // 
            this.uxUpdate.Location = new System.Drawing.Point(8, 555);
            this.uxUpdate.Name = "uxUpdate";
            this.uxUpdate.Size = new System.Drawing.Size(75, 23);
            this.uxUpdate.TabIndex = 1;
            this.uxUpdate.Text = "Update";
            this.uxUpdate.UseVisualStyleBackColor = true;
            this.uxUpdate.Click += new System.EventHandler(this.uxUpdate_Click);
            // 
            // uxUpdatePackages
            // 
            this.uxUpdatePackages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.uxUpdatePackages.Location = new System.Drawing.Point(6, 43);
            this.uxUpdatePackages.Name = "uxUpdatePackages";
            this.uxUpdatePackages.ShowGroups = false;
            this.uxUpdatePackages.Size = new System.Drawing.Size(326, 473);
            this.uxUpdatePackages.TabIndex = 2;
            this.uxUpdatePackages.UseCompatibleStateImageBehavior = false;
            this.uxUpdatePackages.View = System.Windows.Forms.View.Tile;
            // 
            // ExtensionManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 612);
            this.Controls.Add(this.TabPage3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExtensionManagerForm";
            this.Text = "Extension Manager";
            this.Load += new System.EventHandler(this.PackageManagerForm_Load);
            this.TabPage3.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabOnline.ResumeLayout(false);
            this.tabOnline.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl TabPage3;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabOnline;
        private System.Windows.Forms.Button uxInstall;
        private System.Windows.Forms.Button uxSearch;
        private System.Windows.Forms.Button uxClear;
        private System.Windows.Forms.TextBox uxSearchText;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button uxShowExtensionsFolder;
        private System.Windows.Forms.CheckedListBox Installed;
        private System.Windows.Forms.ListBox uxCategoryList;
        private System.Windows.Forms.Button uxUninstall;
        private System.Windows.Forms.ComboBox uxFeedSelection;
        private System.Windows.Forms.ListView uxPackages;
        private System.Windows.Forms.ColumnHeader Pack;
        private System.Windows.Forms.ColumnHeader Description;
        private System.Windows.Forms.ProgressBar uxInstallProgress;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView uxUpdatePackages;
        private System.Windows.Forms.Button uxUpdate;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.RichTextBox uxDownloadStatus;
        private System.Windows.Forms.Button uxUpdateAll;
    }
}