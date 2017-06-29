namespace DotSpatial.Plugins.About
{
	partial class AboutBox
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
            this.DetailsButton = new System.Windows.Forms.Button();
            this.ImagePictureBox = new System.Windows.Forms.PictureBox();
            this.AppDateLabel = new System.Windows.Forms.Label();
            this.SysInfoButton = new System.Windows.Forms.Button();
            this.AppCopyrightLabel = new System.Windows.Forms.Label();
            this.AppVersionLabel = new System.Windows.Forms.Label();
            this.AppDescriptionLabel = new System.Windows.Forms.Label();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.AppTitleLabel = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.MoreRichTextBox = new System.Windows.Forms.RichTextBox();
            this.TabPanelDetails = new System.Windows.Forms.TabControl();
            this.TabPageApplication = new System.Windows.Forms.TabPage();
            this.AppInfoListView = new System.Windows.Forms.ListView();
            this.colKey = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TabPageAssemblies = new System.Windows.Forms.TabPage();
            this.AssemblyInfoListView = new System.Windows.Forms.ListView();
            this.colAssemblyName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAssemblyVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAssemblyBuilt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAssemblyCodeBase = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.TabPageAssemblyDetails = new System.Windows.Forms.TabPage();
            this.AssemblyDetailsListView = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.AssemblyNamesComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.ImagePictureBox)).BeginInit();
            this.TabPanelDetails.SuspendLayout();
            this.TabPageApplication.SuspendLayout();
            this.TabPageAssemblies.SuspendLayout();
            this.TabPageAssemblyDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // DetailsButton
            // 
            this.DetailsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DetailsButton.Location = new System.Drawing.Point(228, 245);
            this.DetailsButton.Name = "DetailsButton";
            this.DetailsButton.Size = new System.Drawing.Size(76, 23);
            this.DetailsButton.TabIndex = 25;
            this.DetailsButton.Text = "&Details >>";
            this.DetailsButton.Click += new System.EventHandler(this.DetailsButton_Click);
            // 
            // ImagePictureBox
            // 
            this.ImagePictureBox.Location = new System.Drawing.Point(14, 7);
            this.ImagePictureBox.Name = "ImagePictureBox";
            this.ImagePictureBox.Size = new System.Drawing.Size(32, 32);
            this.ImagePictureBox.TabIndex = 24;
            this.ImagePictureBox.TabStop = false;
            // 
            // AppDateLabel
            // 
            this.AppDateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AppDateLabel.Location = new System.Drawing.Point(6, 79);
            this.AppDateLabel.Name = "AppDateLabel";
            this.AppDateLabel.Size = new System.Drawing.Size(380, 16);
            this.AppDateLabel.TabIndex = 23;
            this.AppDateLabel.Text = "Built on %builddate%";
            // 
            // SysInfoButton
            // 
            this.SysInfoButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SysInfoButton.Location = new System.Drawing.Point(212, 245);
            this.SysInfoButton.Name = "SysInfoButton";
            this.SysInfoButton.Size = new System.Drawing.Size(92, 23);
            this.SysInfoButton.TabIndex = 22;
            this.SysInfoButton.Text = "&System Info...";
            this.SysInfoButton.Visible = false;
            this.SysInfoButton.Click += new System.EventHandler(this.SysInfoButton_Click);
            // 
            // AppCopyrightLabel
            // 
            this.AppCopyrightLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AppCopyrightLabel.Location = new System.Drawing.Point(6, 99);
            this.AppCopyrightLabel.Name = "AppCopyrightLabel";
            this.AppCopyrightLabel.Size = new System.Drawing.Size(380, 16);
            this.AppCopyrightLabel.TabIndex = 21;
            this.AppCopyrightLabel.Text = "Copyright © %year%, %company%";
            // 
            // AppVersionLabel
            // 
            this.AppVersionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AppVersionLabel.Location = new System.Drawing.Point(6, 59);
            this.AppVersionLabel.Name = "AppVersionLabel";
            this.AppVersionLabel.Size = new System.Drawing.Size(380, 16);
            this.AppVersionLabel.TabIndex = 20;
            this.AppVersionLabel.Text = "Version %version%";
            // 
            // AppDescriptionLabel
            // 
            this.AppDescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AppDescriptionLabel.Location = new System.Drawing.Point(58, 27);
            this.AppDescriptionLabel.Name = "AppDescriptionLabel";
            this.AppDescriptionLabel.Size = new System.Drawing.Size(328, 16);
            this.AppDescriptionLabel.TabIndex = 19;
            this.AppDescriptionLabel.Text = "%description%";
            // 
            // GroupBox1
            // 
            this.GroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox1.Location = new System.Drawing.Point(6, 47);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(380, 2);
            this.GroupBox1.TabIndex = 18;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "GroupBox1";
            // 
            // AppTitleLabel
            // 
            this.AppTitleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AppTitleLabel.Location = new System.Drawing.Point(58, 7);
            this.AppTitleLabel.Name = "AppTitleLabel";
            this.AppTitleLabel.Size = new System.Drawing.Size(328, 16);
            this.AppTitleLabel.TabIndex = 17;
            this.AppTitleLabel.Text = "%title%";
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OKButton.Location = new System.Drawing.Point(312, 245);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(76, 23);
            this.OKButton.TabIndex = 16;
            this.OKButton.Text = "OK";
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // MoreRichTextBox
            // 
            this.MoreRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MoreRichTextBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.MoreRichTextBox.Location = new System.Drawing.Point(6, 123);
            this.MoreRichTextBox.Name = "MoreRichTextBox";
            this.MoreRichTextBox.ReadOnly = true;
            this.MoreRichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.MoreRichTextBox.Size = new System.Drawing.Size(380, 114);
            this.MoreRichTextBox.TabIndex = 26;
            this.MoreRichTextBox.Text = "%product% is %copyright%, %trademark%";
            this.MoreRichTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.MoreRichTextBox_LinkClicked);
            // 
            // TabPanelDetails
            // 
            this.TabPanelDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabPanelDetails.Controls.Add(this.TabPageApplication);
            this.TabPanelDetails.Controls.Add(this.TabPageAssemblies);
            this.TabPanelDetails.Controls.Add(this.TabPageAssemblyDetails);
            this.TabPanelDetails.Location = new System.Drawing.Point(6, 123);
            this.TabPanelDetails.Name = "TabPanelDetails";
            this.TabPanelDetails.SelectedIndex = 0;
            this.TabPanelDetails.Size = new System.Drawing.Size(378, 114);
            this.TabPanelDetails.TabIndex = 27;
            this.TabPanelDetails.Visible = false;
            this.TabPanelDetails.SelectedIndexChanged += new System.EventHandler(this.TabPanelDetails_SelectedIndexChanged);
            // 
            // TabPageApplication
            // 
            this.TabPageApplication.Controls.Add(this.AppInfoListView);
            this.TabPageApplication.Location = new System.Drawing.Point(4, 22);
            this.TabPageApplication.Name = "TabPageApplication";
            this.TabPageApplication.Size = new System.Drawing.Size(370, 88);
            this.TabPageApplication.TabIndex = 0;
            this.TabPageApplication.Text = "Application";
            // 
            // AppInfoListView
            // 
            this.AppInfoListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colKey,
            this.colValue});
            this.AppInfoListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AppInfoListView.FullRowSelect = true;
            this.AppInfoListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.AppInfoListView.Location = new System.Drawing.Point(0, 0);
            this.AppInfoListView.Name = "AppInfoListView";
            this.AppInfoListView.Size = new System.Drawing.Size(370, 88);
            this.AppInfoListView.TabIndex = 16;
            this.AppInfoListView.UseCompatibleStateImageBehavior = false;
            this.AppInfoListView.View = System.Windows.Forms.View.Details;
            // 
            // colKey
            // 
            this.colKey.Text = "Application Key";
            this.colKey.Width = 120;
            // 
            // colValue
            // 
            this.colValue.Text = "Value";
            this.colValue.Width = 700;
            // 
            // TabPageAssemblies
            // 
            this.TabPageAssemblies.Controls.Add(this.AssemblyInfoListView);
            this.TabPageAssemblies.Location = new System.Drawing.Point(4, 22);
            this.TabPageAssemblies.Name = "TabPageAssemblies";
            this.TabPageAssemblies.Size = new System.Drawing.Size(370, 88);
            this.TabPageAssemblies.TabIndex = 1;
            this.TabPageAssemblies.Text = "Assemblies";
            // 
            // AssemblyInfoListView
            // 
            this.AssemblyInfoListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAssemblyName,
            this.colAssemblyVersion,
            this.colAssemblyBuilt,
            this.colAssemblyCodeBase});
            this.AssemblyInfoListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AssemblyInfoListView.FullRowSelect = true;
            this.AssemblyInfoListView.Location = new System.Drawing.Point(0, 0);
            this.AssemblyInfoListView.MultiSelect = false;
            this.AssemblyInfoListView.Name = "AssemblyInfoListView";
            this.AssemblyInfoListView.Size = new System.Drawing.Size(370, 88);
            this.AssemblyInfoListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.AssemblyInfoListView.TabIndex = 13;
            this.AssemblyInfoListView.UseCompatibleStateImageBehavior = false;
            this.AssemblyInfoListView.View = System.Windows.Forms.View.Details;
            this.AssemblyInfoListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.AssemblyInfoListView_ColumnClick);
            this.AssemblyInfoListView.DoubleClick += new System.EventHandler(this.AssemblyInfoListView_DoubleClick);
            // 
            // colAssemblyName
            // 
            this.colAssemblyName.Text = "Assembly";
            this.colAssemblyName.Width = 123;
            // 
            // colAssemblyVersion
            // 
            this.colAssemblyVersion.Text = "Version";
            this.colAssemblyVersion.Width = 100;
            // 
            // colAssemblyBuilt
            // 
            this.colAssemblyBuilt.Text = "Built";
            this.colAssemblyBuilt.Width = 130;
            // 
            // colAssemblyCodeBase
            // 
            this.colAssemblyCodeBase.Text = "CodeBase";
            this.colAssemblyCodeBase.Width = 750;
            // 
            // TabPageAssemblyDetails
            // 
            this.TabPageAssemblyDetails.Controls.Add(this.AssemblyDetailsListView);
            this.TabPageAssemblyDetails.Controls.Add(this.AssemblyNamesComboBox);
            this.TabPageAssemblyDetails.Location = new System.Drawing.Point(4, 22);
            this.TabPageAssemblyDetails.Name = "TabPageAssemblyDetails";
            this.TabPageAssemblyDetails.Size = new System.Drawing.Size(370, 88);
            this.TabPageAssemblyDetails.TabIndex = 2;
            this.TabPageAssemblyDetails.Text = "Assembly Details";
            // 
            // AssemblyDetailsListView
            // 
            this.AssemblyDetailsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1,
            this.ColumnHeader2});
            this.AssemblyDetailsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AssemblyDetailsListView.FullRowSelect = true;
            this.AssemblyDetailsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.AssemblyDetailsListView.Location = new System.Drawing.Point(0, 21);
            this.AssemblyDetailsListView.Name = "AssemblyDetailsListView";
            this.AssemblyDetailsListView.Size = new System.Drawing.Size(370, 67);
            this.AssemblyDetailsListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.AssemblyDetailsListView.TabIndex = 19;
            this.AssemblyDetailsListView.UseCompatibleStateImageBehavior = false;
            this.AssemblyDetailsListView.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "Assembly Key";
            this.ColumnHeader1.Width = 120;
            // 
            // ColumnHeader2
            // 
            this.ColumnHeader2.Text = "Value";
            this.ColumnHeader2.Width = 700;
            // 
            // AssemblyNamesComboBox
            // 
            this.AssemblyNamesComboBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.AssemblyNamesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AssemblyNamesComboBox.Location = new System.Drawing.Point(0, 0);
            this.AssemblyNamesComboBox.Name = "AssemblyNamesComboBox";
            this.AssemblyNamesComboBox.Size = new System.Drawing.Size(370, 21);
            this.AssemblyNamesComboBox.Sorted = true;
            this.AssemblyNamesComboBox.TabIndex = 18;
            this.AssemblyNamesComboBox.SelectedIndexChanged += new System.EventHandler(this.AssemblyNamesComboBox_SelectedIndexChanged);
            // 
            // AboutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.OKButton;
            this.ClientSize = new System.Drawing.Size(394, 275);
            this.Controls.Add(this.DetailsButton);
            this.Controls.Add(this.ImagePictureBox);
            this.Controls.Add(this.AppDateLabel);
            this.Controls.Add(this.SysInfoButton);
            this.Controls.Add(this.AppCopyrightLabel);
            this.Controls.Add(this.AppVersionLabel);
            this.Controls.Add(this.AppDescriptionLabel);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.AppTitleLabel);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.MoreRichTextBox);
            this.Controls.Add(this.TabPanelDetails);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About %title%";
            this.Load += new System.EventHandler(this.AboutBox_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.AboutBox_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.ImagePictureBox)).EndInit();
            this.TabPanelDetails.ResumeLayout(false);
            this.TabPageApplication.ResumeLayout(false);
            this.TabPageAssemblies.ResumeLayout(false);
            this.TabPageAssemblyDetails.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button DetailsButton;
		private System.Windows.Forms.PictureBox ImagePictureBox;
		private System.Windows.Forms.Label AppDateLabel;
		private System.Windows.Forms.Button SysInfoButton;
		private System.Windows.Forms.Label AppCopyrightLabel;
		private System.Windows.Forms.Label AppVersionLabel;
		private System.Windows.Forms.Label AppDescriptionLabel;
		private System.Windows.Forms.GroupBox GroupBox1;
		private System.Windows.Forms.Label AppTitleLabel;
		private System.Windows.Forms.Button OKButton;
		internal System.Windows.Forms.RichTextBox MoreRichTextBox;
		internal System.Windows.Forms.TabControl TabPanelDetails;
		internal System.Windows.Forms.TabPage TabPageApplication;
		internal System.Windows.Forms.ListView AppInfoListView;
		internal System.Windows.Forms.ColumnHeader colKey;
		internal System.Windows.Forms.ColumnHeader colValue;
		internal System.Windows.Forms.TabPage TabPageAssemblies;
		internal System.Windows.Forms.ListView AssemblyInfoListView;
		internal System.Windows.Forms.ColumnHeader colAssemblyName;
		internal System.Windows.Forms.ColumnHeader colAssemblyVersion;
		internal System.Windows.Forms.ColumnHeader colAssemblyBuilt;
		internal System.Windows.Forms.ColumnHeader colAssemblyCodeBase;
		internal System.Windows.Forms.TabPage TabPageAssemblyDetails;
		internal System.Windows.Forms.ListView AssemblyDetailsListView;
		internal System.Windows.Forms.ColumnHeader ColumnHeader1;
		internal System.Windows.Forms.ColumnHeader ColumnHeader2;
		internal System.Windows.Forms.ComboBox AssemblyNamesComboBox;
	}
}