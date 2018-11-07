using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Plugins.About
{
	partial class AboutBox
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
            this.DetailsButton = new System.Windows.Forms.Button();
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
            this.ImagePictureBox = new System.Windows.Forms.PictureBox();
            this.TabPanelDetails.SuspendLayout();
            this.TabPageApplication.SuspendLayout();
            this.TabPageAssemblies.SuspendLayout();
            this.TabPageAssemblyDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImagePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // DetailsButton
            // 
            resources.ApplyResources(this.DetailsButton, "DetailsButton");
            this.DetailsButton.Name = "DetailsButton";
            this.DetailsButton.Click += new System.EventHandler(this.DetailsButtonClick);
            // 
            // AppDateLabel
            // 
            resources.ApplyResources(this.AppDateLabel, "AppDateLabel");
            this.AppDateLabel.Name = "AppDateLabel";
            // 
            // SysInfoButton
            // 
            resources.ApplyResources(this.SysInfoButton, "SysInfoButton");
            this.SysInfoButton.Name = "SysInfoButton";
            this.SysInfoButton.Click += new System.EventHandler(this.SysInfoButtonClick);
            // 
            // AppCopyrightLabel
            // 
            resources.ApplyResources(this.AppCopyrightLabel, "AppCopyrightLabel");
            this.AppCopyrightLabel.Name = "AppCopyrightLabel";
            // 
            // AppVersionLabel
            // 
            resources.ApplyResources(this.AppVersionLabel, "AppVersionLabel");
            this.AppVersionLabel.Name = "AppVersionLabel";
            // 
            // AppDescriptionLabel
            // 
            resources.ApplyResources(this.AppDescriptionLabel, "AppDescriptionLabel");
            this.AppDescriptionLabel.Name = "AppDescriptionLabel";
            // 
            // GroupBox1
            // 
            resources.ApplyResources(this.GroupBox1, "GroupBox1");
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.TabStop = false;
            // 
            // AppTitleLabel
            // 
            resources.ApplyResources(this.AppTitleLabel, "AppTitleLabel");
            this.AppTitleLabel.Name = "AppTitleLabel";
            // 
            // OKButton
            // 
            resources.ApplyResources(this.OKButton, "OKButton");
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.OKButton.Name = "OKButton";
            this.OKButton.Click += new System.EventHandler(this.OkButtonClick);
            // 
            // MoreRichTextBox
            // 
            resources.ApplyResources(this.MoreRichTextBox, "MoreRichTextBox");
            this.MoreRichTextBox.BackColor = System.Drawing.SystemColors.ControlLight;
            this.MoreRichTextBox.Name = "MoreRichTextBox";
            this.MoreRichTextBox.ReadOnly = true;
            this.MoreRichTextBox.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.MoreRichTextBoxLinkClicked);
            // 
            // TabPanelDetails
            // 
            resources.ApplyResources(this.TabPanelDetails, "TabPanelDetails");
            this.TabPanelDetails.Controls.Add(this.TabPageApplication);
            this.TabPanelDetails.Controls.Add(this.TabPageAssemblies);
            this.TabPanelDetails.Controls.Add(this.TabPageAssemblyDetails);
            this.TabPanelDetails.Name = "TabPanelDetails";
            this.TabPanelDetails.SelectedIndex = 0;
            this.TabPanelDetails.SelectedIndexChanged += new System.EventHandler(this.TabPanelDetailsSelectedIndexChanged);
            // 
            // TabPageApplication
            // 
            resources.ApplyResources(this.TabPageApplication, "TabPageApplication");
            this.TabPageApplication.Controls.Add(this.AppInfoListView);
            this.TabPageApplication.Name = "TabPageApplication";
            // 
            // AppInfoListView
            // 
            resources.ApplyResources(this.AppInfoListView, "AppInfoListView");
            this.AppInfoListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colKey,
            this.colValue});
            this.AppInfoListView.FullRowSelect = true;
            this.AppInfoListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.AppInfoListView.Name = "AppInfoListView";
            this.AppInfoListView.UseCompatibleStateImageBehavior = false;
            this.AppInfoListView.View = System.Windows.Forms.View.Details;
            // 
            // colKey
            // 
            resources.ApplyResources(this.colKey, "colKey");
            // 
            // colValue
            // 
            resources.ApplyResources(this.colValue, "colValue");
            // 
            // TabPageAssemblies
            // 
            resources.ApplyResources(this.TabPageAssemblies, "TabPageAssemblies");
            this.TabPageAssemblies.Controls.Add(this.AssemblyInfoListView);
            this.TabPageAssemblies.Name = "TabPageAssemblies";
            // 
            // AssemblyInfoListView
            // 
            resources.ApplyResources(this.AssemblyInfoListView, "AssemblyInfoListView");
            this.AssemblyInfoListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAssemblyName,
            this.colAssemblyVersion,
            this.colAssemblyBuilt,
            this.colAssemblyCodeBase});
            this.AssemblyInfoListView.FullRowSelect = true;
            this.AssemblyInfoListView.MultiSelect = false;
            this.AssemblyInfoListView.Name = "AssemblyInfoListView";
            this.AssemblyInfoListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.AssemblyInfoListView.UseCompatibleStateImageBehavior = false;
            this.AssemblyInfoListView.View = System.Windows.Forms.View.Details;
            this.AssemblyInfoListView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.AssemblyInfoListViewColumnClick);
            this.AssemblyInfoListView.DoubleClick += new System.EventHandler(this.AssemblyInfoListViewDoubleClick);
            // 
            // colAssemblyName
            // 
            resources.ApplyResources(this.colAssemblyName, "colAssemblyName");
            // 
            // colAssemblyVersion
            // 
            resources.ApplyResources(this.colAssemblyVersion, "colAssemblyVersion");
            // 
            // colAssemblyBuilt
            // 
            resources.ApplyResources(this.colAssemblyBuilt, "colAssemblyBuilt");
            // 
            // colAssemblyCodeBase
            // 
            resources.ApplyResources(this.colAssemblyCodeBase, "colAssemblyCodeBase");
            // 
            // TabPageAssemblyDetails
            // 
            resources.ApplyResources(this.TabPageAssemblyDetails, "TabPageAssemblyDetails");
            this.TabPageAssemblyDetails.Controls.Add(this.AssemblyDetailsListView);
            this.TabPageAssemblyDetails.Controls.Add(this.AssemblyNamesComboBox);
            this.TabPageAssemblyDetails.Name = "TabPageAssemblyDetails";
            // 
            // AssemblyDetailsListView
            // 
            resources.ApplyResources(this.AssemblyDetailsListView, "AssemblyDetailsListView");
            this.AssemblyDetailsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1,
            this.ColumnHeader2});
            this.AssemblyDetailsListView.FullRowSelect = true;
            this.AssemblyDetailsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.AssemblyDetailsListView.Name = "AssemblyDetailsListView";
            this.AssemblyDetailsListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.AssemblyDetailsListView.UseCompatibleStateImageBehavior = false;
            this.AssemblyDetailsListView.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHeader1
            // 
            resources.ApplyResources(this.ColumnHeader1, "ColumnHeader1");
            // 
            // ColumnHeader2
            // 
            resources.ApplyResources(this.ColumnHeader2, "ColumnHeader2");
            // 
            // AssemblyNamesComboBox
            // 
            resources.ApplyResources(this.AssemblyNamesComboBox, "AssemblyNamesComboBox");
            this.AssemblyNamesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AssemblyNamesComboBox.Name = "AssemblyNamesComboBox";
            this.AssemblyNamesComboBox.Sorted = true;
            this.AssemblyNamesComboBox.SelectedIndexChanged += new System.EventHandler(this.AssemblyNamesComboBoxSelectedIndexChanged);
            // 
            // ImagePictureBox
            // 
            resources.ApplyResources(this.ImagePictureBox, "ImagePictureBox");
            this.ImagePictureBox.Name = "ImagePictureBox";
            this.ImagePictureBox.TabStop = false;
            // 
            // AboutBox
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.OKButton;
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
            this.Load += new System.EventHandler(this.AboutBoxLoad);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.AboutBoxPaint);
            this.TabPanelDetails.ResumeLayout(false);
            this.TabPageApplication.ResumeLayout(false);
            this.TabPageAssemblies.ResumeLayout(false);
            this.TabPageAssemblyDetails.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImagePictureBox)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private Button DetailsButton;
		private PictureBox ImagePictureBox;
		private Label AppDateLabel;
		private Button SysInfoButton;
		private Label AppCopyrightLabel;
		private Label AppVersionLabel;
		private Label AppDescriptionLabel;
		private GroupBox GroupBox1;
		private Label AppTitleLabel;
		private Button OKButton;
		internal RichTextBox MoreRichTextBox;
		internal TabControl TabPanelDetails;
		internal TabPage TabPageApplication;
		internal ListView AppInfoListView;
		internal ColumnHeader colKey;
		internal ColumnHeader colValue;
		internal TabPage TabPageAssemblies;
		internal ListView AssemblyInfoListView;
		internal ColumnHeader colAssemblyName;
		internal ColumnHeader colAssemblyVersion;
		internal ColumnHeader colAssemblyBuilt;
		internal ColumnHeader colAssemblyCodeBase;
		internal TabPage TabPageAssemblyDetails;
		internal ListView AssemblyDetailsListView;
		internal ColumnHeader ColumnHeader1;
		internal ColumnHeader ColumnHeader2;
		internal ComboBox AssemblyNamesComboBox;
	}
}