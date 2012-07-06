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
            this.uxUpdates = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.uxShowExtensionsFolder = new System.Windows.Forms.Button();
            this.Installed = new System.Windows.Forms.CheckedListBox();
            this.uxCategoryList = new System.Windows.Forms.ListBox();
            this.uxUninstall = new System.Windows.Forms.Button();
            this.tabOnline = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
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
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.uxSourceUrl = new System.Windows.Forms.TextBox();
            this.uxSourceName = new System.Windows.Forms.TextBox();
            this.uxSource = new System.Windows.Forms.Label();
            this.uxName = new System.Windows.Forms.Label();
            this.uxFeedSources = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.uxAdd = new System.Windows.Forms.Button();
            this.uxRemove = new System.Windows.Forms.Button();
            this.uxUpdates.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabOnline.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // uxUpdates
            // 
            this.uxUpdates.Controls.Add(this.tabPage1);
            this.uxUpdates.Controls.Add(this.tabOnline);
            this.uxUpdates.Controls.Add(this.tabPage2);
            this.uxUpdates.Controls.Add(this.tabPage4);
            this.uxUpdates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uxUpdates.Location = new System.Drawing.Point(0, 0);
            this.uxUpdates.Name = "uxUpdates";
            this.uxUpdates.SelectedIndex = 0;
            this.uxUpdates.Size = new System.Drawing.Size(754, 582);
            this.uxUpdates.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBox2);
            this.tabPage1.Controls.Add(this.uxShowExtensionsFolder);
            this.tabPage1.Controls.Add(this.Installed);
            this.tabPage1.Controls.Add(this.uxCategoryList);
            this.tabPage1.Controls.Add(this.uxUninstall);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(746, 556);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Installed Extensions";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(532, 45);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(176, 484);
            this.richTextBox2.TabIndex = 10;
            this.richTextBox2.Text = "";
            // 
            // uxShowExtensionsFolder
            // 
            this.uxShowExtensionsFolder.Location = new System.Drawing.Point(150, 6);
            this.uxShowExtensionsFolder.Name = "uxShowExtensionsFolder";
            this.uxShowExtensionsFolder.Size = new System.Drawing.Size(136, 23);
            this.uxShowExtensionsFolder.TabIndex = 9;
            this.uxShowExtensionsFolder.Text = "Open Extensions Folder";
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
            this.Installed.Size = new System.Drawing.Size(376, 484);
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
            this.tabOnline.Controls.Add(this.label1);
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
            this.tabOnline.Size = new System.Drawing.Size(746, 556);
            this.tabOnline.TabIndex = 1;
            this.tabOnline.Text = "Online";
            this.tabOnline.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 515);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Page";
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
            this.uxPackages.Size = new System.Drawing.Size(503, 401);
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
            this.uxFeedSelection.Location = new System.Drawing.Point(8, 10);
            this.uxFeedSelection.Name = "uxFeedSelection";
            this.uxFeedSelection.Size = new System.Drawing.Size(314, 23);
            this.uxFeedSelection.TabIndex = 15;
            this.uxFeedSelection.SelectedIndexChanged += new System.EventHandler(this.uxFeedSelection_SelectedIndexChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(516, 10);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(230, 489);
            this.richTextBox1.TabIndex = 25;
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
            this.uxInstall.Location = new System.Drawing.Point(411, 510);
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
            this.shapeContainer1.Size = new System.Drawing.Size(740, 550);
            this.shapeContainer1.TabIndex = 19;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lineShape1.BorderWidth = 2;
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 507;
            this.lineShape1.X2 = 507;
            this.lineShape1.Y1 = -1;
            this.lineShape1.Y2 = 552;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.uxUpdateAll);
            this.tabPage2.Controls.Add(this.uxUpdate);
            this.tabPage2.Controls.Add(this.uxUpdatePackages);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(746, 556);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "Update";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // uxUpdateAll
            // 
            this.uxUpdateAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.uxUpdateAll.Location = new System.Drawing.Point(227, 525);
            this.uxUpdateAll.Name = "uxUpdateAll";
            this.uxUpdateAll.Size = new System.Drawing.Size(75, 23);
            this.uxUpdateAll.TabIndex = 3;
            this.uxUpdateAll.Text = "Update All";
            this.uxUpdateAll.UseVisualStyleBackColor = true;
            this.uxUpdateAll.Click += new System.EventHandler(this.uxUpdateAll_Click);
            // 
            // uxUpdate
            // 
            this.uxUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.uxUpdate.Location = new System.Drawing.Point(24, 525);
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
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.uxSourceUrl);
            this.tabPage4.Controls.Add(this.uxSourceName);
            this.tabPage4.Controls.Add(this.uxSource);
            this.tabPage4.Controls.Add(this.uxName);
            this.tabPage4.Controls.Add(this.uxFeedSources);
            this.tabPage4.Controls.Add(this.uxAdd);
            this.tabPage4.Controls.Add(this.uxRemove);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(746, 556);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Feeds";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // uxSourceUrl
            // 
            this.uxSourceUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxSourceUrl.Location = new System.Drawing.Point(107, 526);
            this.uxSourceUrl.Name = "uxSourceUrl";
            this.uxSourceUrl.Size = new System.Drawing.Size(457, 20);
            this.uxSourceUrl.TabIndex = 11;
            // 
            // uxSourceName
            // 
            this.uxSourceName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxSourceName.Location = new System.Drawing.Point(107, 487);
            this.uxSourceName.Name = "uxSourceName";
            this.uxSourceName.Size = new System.Drawing.Size(457, 20);
            this.uxSourceName.TabIndex = 10;
            // 
            // uxSource
            // 
            this.uxSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.uxSource.AutoSize = true;
            this.uxSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uxSource.Location = new System.Drawing.Point(50, 530);
            this.uxSource.Name = "uxSource";
            this.uxSource.Size = new System.Drawing.Size(51, 16);
            this.uxSource.TabIndex = 9;
            this.uxSource.Text = "Source";
            // 
            // uxName
            // 
            this.uxName.AutoSize = true;
            this.uxName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uxName.Location = new System.Drawing.Point(56, 491);
            this.uxName.Name = "uxName";
            this.uxName.Size = new System.Drawing.Size(45, 16);
            this.uxName.TabIndex = 8;
            this.uxName.Text = "Name";
            // 
            // uxFeedSources
            // 
            this.uxFeedSources.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.uxFeedSources.Location = new System.Drawing.Point(8, 6);
            this.uxFeedSources.Name = "uxFeedSources";
            this.uxFeedSources.Size = new System.Drawing.Size(388, 454);
            this.uxFeedSources.TabIndex = 3;
            this.uxFeedSources.UseCompatibleStateImageBehavior = false;
            this.uxFeedSources.View = System.Windows.Forms.View.Tile;
            // 
            // uxAdd
            // 
            this.uxAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxAdd.Location = new System.Drawing.Point(594, 524);
            this.uxAdd.Name = "uxAdd";
            this.uxAdd.Size = new System.Drawing.Size(75, 23);
            this.uxAdd.TabIndex = 2;
            this.uxAdd.Text = "Add";
            this.uxAdd.UseVisualStyleBackColor = true;
            this.uxAdd.Click += new System.EventHandler(this.uxAdd_Click);
            // 
            // uxRemove
            // 
            this.uxRemove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxRemove.Location = new System.Drawing.Point(428, 6);
            this.uxRemove.Name = "uxRemove";
            this.uxRemove.Size = new System.Drawing.Size(75, 23);
            this.uxRemove.TabIndex = 1;
            this.uxRemove.Text = "Remove";
            this.uxRemove.UseVisualStyleBackColor = true;
            this.uxRemove.Click += new System.EventHandler(this.uxRemove_Click);
            // 
            // ExtensionManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(754, 582);
            this.Controls.Add(this.uxUpdates);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExtensionManagerForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Extension Manager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExtensionManagerForm_FormClosed);
            this.Load += new System.EventHandler(this.PackageManagerForm_Load);
            this.uxUpdates.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabOnline.ResumeLayout(false);
            this.tabOnline.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl uxUpdates;
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
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView uxUpdatePackages;
        private System.Windows.Forms.Button uxUpdate;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button uxUpdateAll;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ListView uxFeedSources;
        private System.Windows.Forms.Button uxAdd;
        private System.Windows.Forms.Button uxRemove;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TextBox uxSourceUrl;
        private System.Windows.Forms.TextBox uxSourceName;
        private System.Windows.Forms.Label uxSource;
        private System.Windows.Forms.Label uxName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox2;
    }
}