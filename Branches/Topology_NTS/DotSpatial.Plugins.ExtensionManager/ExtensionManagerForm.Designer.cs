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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtensionManagerForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.uxShowExtensionsFolder = new System.Windows.Forms.Button();
            this.Installed = new System.Windows.Forms.CheckedListBox();
            this.uxCategoryList = new System.Windows.Forms.ListBox();
            this.uxUninstall = new System.Windows.Forms.Button();
            this.tabOnline = new System.Windows.Forms.TabPage();
            this.uxClear = new System.Windows.Forms.PictureBox();
            this.uxSearch = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.uxPackages = new System.Windows.Forms.ListView();
            this.Pack = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.uxFeedSelection = new System.Windows.Forms.ComboBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.uxSearchText = new System.Windows.Forms.TextBox();
            this.uxInstall = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.uxFeedSelection2 = new System.Windows.Forms.ComboBox();
            this.uxUpdateAll = new System.Windows.Forms.Button();
            this.uxUpdate = new System.Windows.Forms.Button();
            this.uxUpdatePackages = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.uxApply = new System.Windows.Forms.Button();
            this.autoUpdateExplanation = new System.Windows.Forms.Label();
            this.uxFeedGroupBox = new System.Windows.Forms.GroupBox();
            this.uxAdd = new System.Windows.Forms.Button();
            this.uxSourceName = new System.Windows.Forms.TextBox();
            this.uxSource = new System.Windows.Forms.Label();
            this.uxSourceUrl = new System.Windows.Forms.TextBox();
            this.uxName = new System.Windows.Forms.Label();
            this.uxFeedSources = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.uxRemove = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabOnline.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uxClear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxSearch)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.uxFeedGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabOnline);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage4);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(722, 562);
            this.tabControl.TabIndex = 2;
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
            this.tabPage1.Size = new System.Drawing.Size(714, 536);
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
            this.uxShowExtensionsFolder.Location = new System.Drawing.Point(151, 11);
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
            this.uxCategoryList.Location = new System.Drawing.Point(8, 18);
            this.uxCategoryList.Name = "uxCategoryList";
            this.uxCategoryList.Size = new System.Drawing.Size(136, 511);
            this.uxCategoryList.TabIndex = 8;
            this.uxCategoryList.SelectedIndexChanged += new System.EventHandler(this.uxCategoryList_SelectedIndexChanged);
            // 
            // uxUninstall
            // 
            this.uxUninstall.Enabled = false;
            this.uxUninstall.Location = new System.Drawing.Point(583, 11);
            this.uxUninstall.Name = "uxUninstall";
            this.uxUninstall.Size = new System.Drawing.Size(74, 23);
            this.uxUninstall.TabIndex = 7;
            this.uxUninstall.Text = "Uninstall";
            this.uxUninstall.UseVisualStyleBackColor = true;
            this.uxUninstall.Click += new System.EventHandler(this.uxUninstall_Click);
            // 
            // tabOnline
            // 
            this.tabOnline.Controls.Add(this.uxClear);
            this.tabOnline.Controls.Add(this.uxSearch);
            this.tabOnline.Controls.Add(this.label1);
            this.tabOnline.Controls.Add(this.uxPackages);
            this.tabOnline.Controls.Add(this.uxFeedSelection);
            this.tabOnline.Controls.Add(this.richTextBox1);
            this.tabOnline.Controls.Add(this.uxSearchText);
            this.tabOnline.Controls.Add(this.uxInstall);
            this.tabOnline.Location = new System.Drawing.Point(4, 22);
            this.tabOnline.Name = "tabOnline";
            this.tabOnline.Padding = new System.Windows.Forms.Padding(3);
            this.tabOnline.Size = new System.Drawing.Size(714, 536);
            this.tabOnline.TabIndex = 1;
            this.tabOnline.Text = "Online";
            this.tabOnline.UseVisualStyleBackColor = true;
            // 
            // uxClear
            // 
            this.uxClear.Location = new System.Drawing.Point(366, 42);
            this.uxClear.Name = "uxClear";
            this.uxClear.Size = new System.Drawing.Size(25, 25);
            this.uxClear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.uxClear.TabIndex = 27;
            this.uxClear.TabStop = false;
            this.toolTip1.SetToolTip(this.uxClear, "Clear Search");
            // 
            // uxSearch
            // 
            this.uxSearch.Location = new System.Drawing.Point(288, 42);
            this.uxSearch.Name = "uxSearch";
            this.uxSearch.Size = new System.Drawing.Size(25, 25);
            this.uxSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.uxSearch.TabIndex = 26;
            this.uxSearch.TabStop = false;
            this.toolTip1.SetToolTip(this.uxSearch, "Start Search");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 510);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Page";
            this.label1.Visible = false;
            // 
            // uxPackages
            // 
            this.uxPackages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.uxPackages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Pack,
            this.Description});
            this.uxPackages.FullRowSelect = true;
            this.uxPackages.Location = new System.Drawing.Point(6, 68);
            this.uxPackages.Name = "uxPackages";
            this.uxPackages.ShowGroups = false;
            this.uxPackages.Size = new System.Drawing.Size(471, 431);
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
            this.richTextBox1.Location = new System.Drawing.Point(484, 10);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(230, 489);
            this.richTextBox1.TabIndex = 25;
            this.richTextBox1.Text = "";
            // 
            // uxSearchText
            // 
            this.uxSearchText.Location = new System.Drawing.Point(8, 42);
            this.uxSearchText.Name = "uxSearchText";
            this.uxSearchText.Size = new System.Drawing.Size(274, 20);
            this.uxSearchText.TabIndex = 12;
            this.uxSearchText.Text = "Search";
            this.uxSearchText.Click += new System.EventHandler(this.uxSearchText_Click);
            this.uxSearchText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uxSearchText_KeyDown);
            // 
            // uxInstall
            // 
            this.uxInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.uxInstall.Enabled = false;
            this.uxInstall.Location = new System.Drawing.Point(402, 505);
            this.uxInstall.Name = "uxInstall";
            this.uxInstall.Size = new System.Drawing.Size(75, 23);
            this.uxInstall.TabIndex = 3;
            this.uxInstall.Text = "Install";
            this.uxInstall.UseVisualStyleBackColor = true;
            this.uxInstall.Click += new System.EventHandler(this.InstallButton_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.richTextBox3);
            this.tabPage2.Controls.Add(this.uxFeedSelection2);
            this.tabPage2.Controls.Add(this.uxUpdateAll);
            this.tabPage2.Controls.Add(this.uxUpdate);
            this.tabPage2.Controls.Add(this.uxUpdatePackages);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(714, 536);
            this.tabPage2.TabIndex = 2;
            this.tabPage2.Text = "Update";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBox3
            // 
            this.richTextBox3.Location = new System.Drawing.Point(484, 10);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Size = new System.Drawing.Size(230, 489);
            this.richTextBox3.TabIndex = 26;
            this.richTextBox3.Text = "";
            // 
            // uxFeedSelection2
            // 
            this.uxFeedSelection2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uxFeedSelection2.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uxFeedSelection2.FormattingEnabled = true;
            this.uxFeedSelection2.Location = new System.Drawing.Point(8, 10);
            this.uxFeedSelection2.Name = "uxFeedSelection2";
            this.uxFeedSelection2.Size = new System.Drawing.Size(314, 23);
            this.uxFeedSelection2.TabIndex = 15;
            this.uxFeedSelection2.SelectedIndexChanged += new System.EventHandler(this.uxFeedSelection2_SelectedIndexChanged);
            // 
            // uxUpdateAll
            // 
            this.uxUpdateAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.uxUpdateAll.Location = new System.Drawing.Point(109, 505);
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
            this.uxUpdate.Location = new System.Drawing.Point(28, 505);
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
            this.uxUpdatePackages.Size = new System.Drawing.Size(471, 456);
            this.uxUpdatePackages.TabIndex = 2;
            this.uxUpdatePackages.UseCompatibleStateImageBehavior = false;
            this.uxUpdatePackages.View = System.Windows.Forms.View.Tile;
            this.uxUpdatePackages.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.uxUpdate_SelectionChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.uxApply);
            this.tabPage4.Controls.Add(this.autoUpdateExplanation);
            this.tabPage4.Controls.Add(this.uxFeedGroupBox);
            this.tabPage4.Controls.Add(this.uxFeedSources);
            this.tabPage4.Controls.Add(this.uxRemove);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(714, 556);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Feeds";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // uxApply
            // 
            this.uxApply.Enabled = false;
            this.uxApply.Location = new System.Drawing.Point(478, 402);
            this.uxApply.Name = "uxApply";
            this.uxApply.Size = new System.Drawing.Size(75, 23);
            this.uxApply.TabIndex = 15;
            this.uxApply.Text = "Apply";
            this.uxApply.UseVisualStyleBackColor = true;
            this.uxApply.Click += new System.EventHandler(this.uxApply_Click);
            // 
            // autoUpdateExplanation
            // 
            this.autoUpdateExplanation.AutoSize = true;
            this.autoUpdateExplanation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.autoUpdateExplanation.Location = new System.Drawing.Point(8, 402);
            this.autoUpdateExplanation.Name = "autoUpdateExplanation";
            this.autoUpdateExplanation.Size = new System.Drawing.Size(277, 16);
            this.autoUpdateExplanation.TabIndex = 14;
            this.autoUpdateExplanation.Text = "Checked feeds will be automatically updated.";
            // 
            // uxFeedGroupBox
            // 
            this.uxFeedGroupBox.Controls.Add(this.uxAdd);
            this.uxFeedGroupBox.Controls.Add(this.uxSourceName);
            this.uxFeedGroupBox.Controls.Add(this.uxSource);
            this.uxFeedGroupBox.Controls.Add(this.uxSourceUrl);
            this.uxFeedGroupBox.Controls.Add(this.uxName);
            this.uxFeedGroupBox.Location = new System.Drawing.Point(7, 440);
            this.uxFeedGroupBox.Name = "uxFeedGroupBox";
            this.uxFeedGroupBox.Size = new System.Drawing.Size(700, 82);
            this.uxFeedGroupBox.TabIndex = 13;
            this.uxFeedGroupBox.TabStop = false;
            // 
            // uxAdd
            // 
            this.uxAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxAdd.Location = new System.Drawing.Point(608, 41);
            this.uxAdd.Name = "uxAdd";
            this.uxAdd.Size = new System.Drawing.Size(70, 23);
            this.uxAdd.TabIndex = 2;
            this.uxAdd.Text = "Add";
            this.uxAdd.UseVisualStyleBackColor = true;
            this.uxAdd.Click += new System.EventHandler(this.uxAdd_Click);
            // 
            // uxSourceName
            // 
            this.uxSourceName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxSourceName.Location = new System.Drawing.Point(79, 12);
            this.uxSourceName.Name = "uxSourceName";
            this.uxSourceName.Size = new System.Drawing.Size(500, 20);
            this.uxSourceName.TabIndex = 10;
            // 
            // uxSource
            // 
            this.uxSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.uxSource.AutoSize = true;
            this.uxSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uxSource.Location = new System.Drawing.Point(22, 45);
            this.uxSource.Name = "uxSource";
            this.uxSource.Size = new System.Drawing.Size(51, 16);
            this.uxSource.TabIndex = 9;
            this.uxSource.Text = "Source";
            // 
            // uxSourceUrl
            // 
            this.uxSourceUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxSourceUrl.Location = new System.Drawing.Point(79, 44);
            this.uxSourceUrl.Name = "uxSourceUrl";
            this.uxSourceUrl.Size = new System.Drawing.Size(500, 20);
            this.uxSourceUrl.TabIndex = 11;
            // 
            // uxName
            // 
            this.uxName.AutoSize = true;
            this.uxName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uxName.Location = new System.Drawing.Point(22, 16);
            this.uxName.Name = "uxName";
            this.uxName.Size = new System.Drawing.Size(45, 16);
            this.uxName.TabIndex = 8;
            this.uxName.Text = "Name";
            // 
            // uxFeedSources
            // 
            this.uxFeedSources.CheckBoxes = true;
            this.uxFeedSources.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5});
            this.uxFeedSources.FullRowSelect = true;
            this.uxFeedSources.Location = new System.Drawing.Point(8, 6);
            this.uxFeedSources.Name = "uxFeedSources";
            this.uxFeedSources.Size = new System.Drawing.Size(545, 393);
            this.uxFeedSources.TabIndex = 3;
            this.uxFeedSources.UseCompatibleStateImageBehavior = false;
            this.uxFeedSources.View = System.Windows.Forms.View.Details;
            this.uxFeedSources.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.uxSourceFeed_Checked);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Name";
            this.columnHeader4.Width = 223;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "URL";
            this.columnHeader5.Width = 318;
            // 
            // uxRemove
            // 
            this.uxRemove.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxRemove.Location = new System.Drawing.Point(559, 6);
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
            this.ClientSize = new System.Drawing.Size(722, 562);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExtensionManagerForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Extension Manager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExtensionManagerForm_FormClosed);
            this.Load += new System.EventHandler(this.ExtensionManagerForm_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabOnline.ResumeLayout(false);
            this.tabOnline.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uxClear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxSearch)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.uxFeedGroupBox.ResumeLayout(false);
            this.uxFeedGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabOnline;
        private System.Windows.Forms.Button uxInstall;
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
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button uxUpdateAll;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ListView uxFeedSources;
        private System.Windows.Forms.Button uxAdd;
        private System.Windows.Forms.Button uxRemove;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TextBox uxSourceUrl;
        private System.Windows.Forms.TextBox uxSourceName;
        private System.Windows.Forms.Label uxSource;
        private System.Windows.Forms.Label uxName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.GroupBox uxFeedGroupBox;
        private System.Windows.Forms.PictureBox uxSearch;
        private System.Windows.Forms.PictureBox uxClear;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Label autoUpdateExplanation;
        private System.Windows.Forms.Button uxApply;
        private System.Windows.Forms.ComboBox uxFeedSelection2;
        private System.Windows.Forms.RichTextBox richTextBox3;
    }
}