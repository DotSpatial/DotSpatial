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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtensionManagerForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.uxShowExtensionsFolder = new System.Windows.Forms.Button();
            this.Installed = new System.Windows.Forms.CheckedListBox();
            this.uxCategoryList = new System.Windows.Forms.ListBox();
            this.uxUninstall = new System.Windows.Forms.Button();
            this.tabOnline = new System.Windows.Forms.TabPage();
            this.uxPackages = new System.Windows.Forms.ListView();
            this.Pack = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.uxFeedSelection = new System.Windows.Forms.ComboBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.uxSearchText = new System.Windows.Forms.TextBox();
            this.uxClear = new System.Windows.Forms.Button();
            this.uxSearch = new System.Windows.Forms.Button();
            this.uxUpdate = new System.Windows.Forms.Button();
            this.uxInstall = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
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
            this.tabControl1.Size = new System.Drawing.Size(659, 562);
            this.tabControl1.TabIndex = 2;
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
            this.tabPage1.Size = new System.Drawing.Size(651, 536);
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
            this.tabOnline.Controls.Add(this.uxPackages);
            this.tabOnline.Controls.Add(this.uxFeedSelection);
            this.tabOnline.Controls.Add(this.richTextBox1);
            this.tabOnline.Controls.Add(this.uxSearchText);
            this.tabOnline.Controls.Add(this.uxClear);
            this.tabOnline.Controls.Add(this.uxSearch);
            this.tabOnline.Controls.Add(this.uxUpdate);
            this.tabOnline.Controls.Add(this.uxInstall);
            this.tabOnline.Location = new System.Drawing.Point(4, 22);
            this.tabOnline.Name = "tabOnline";
            this.tabOnline.Padding = new System.Windows.Forms.Padding(3);
            this.tabOnline.Size = new System.Drawing.Size(651, 536);
            this.tabOnline.TabIndex = 1;
            this.tabOnline.Text = "Online";
            this.tabOnline.UseVisualStyleBackColor = true;
            // 
            // uxPackages
            // 
            this.uxPackages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.uxPackages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Pack,
            this.Description});
            this.uxPackages.FullRowSelect = true;
            listViewGroup1.Header = "ListViewGroup";
            listViewGroup1.Name = "listViewGroup1";
            this.uxPackages.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
            this.uxPackages.Location = new System.Drawing.Point(3, 68);
            this.uxPackages.Name = "uxPackages";
            this.uxPackages.ShowGroups = false;
            this.uxPackages.Size = new System.Drawing.Size(326, 459);
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
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(349, 69);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(293, 455);
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
            // uxUpdate
            // 
            this.uxUpdate.Enabled = false;
            this.uxUpdate.Location = new System.Drawing.Point(567, 10);
            this.uxUpdate.Name = "uxUpdate";
            this.uxUpdate.Size = new System.Drawing.Size(75, 23);
            this.uxUpdate.TabIndex = 3;
            this.uxUpdate.Text = "Update";
            this.uxUpdate.UseVisualStyleBackColor = true;
            this.uxUpdate.Click += new System.EventHandler(this.uxUpdate_Click);
            // 
            // uxInstall
            // 
            this.uxInstall.Enabled = false;
            this.uxInstall.Location = new System.Drawing.Point(377, 10);
            this.uxInstall.Name = "uxInstall";
            this.uxInstall.Size = new System.Drawing.Size(75, 23);
            this.uxInstall.TabIndex = 3;
            this.uxInstall.Text = "Install";
            this.uxInstall.UseVisualStyleBackColor = true;
            this.uxInstall.Click += new System.EventHandler(this.InstallButton_Click);
            // 
            // ExtensionManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 562);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExtensionManagerForm";
            this.Text = "Extension Manager";
            this.Load += new System.EventHandler(this.PackageManagerForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabOnline.ResumeLayout(false);
            this.tabOnline.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabOnline;
        private System.Windows.Forms.Button uxInstall;
        private System.Windows.Forms.Button uxUpdate;
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
    }
}