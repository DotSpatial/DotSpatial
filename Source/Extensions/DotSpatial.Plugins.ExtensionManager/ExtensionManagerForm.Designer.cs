using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Plugins.ExtensionManager
{
    partial class ExtensionManagerForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtensionManagerForm));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabInstalled = new System.Windows.Forms.TabPage();
            this.Installed = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.uxShowExtensionsFolder = new System.Windows.Forms.Button();
            this.uxUninstall = new System.Windows.Forms.Button();
            this.tabOnline = new System.Windows.Forms.TabPage();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.uxClear = new System.Windows.Forms.PictureBox();
            this.uxSearch = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.uxPackages = new System.Windows.Forms.ListView();
            this.Pack = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Description = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.uxSearchText = new System.Windows.Forms.TextBox();
            this.uxInstall = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl.SuspendLayout();
            this.tabInstalled.SuspendLayout();
            this.tabOnline.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uxClear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabInstalled);
            this.tabControl.Controls.Add(this.tabOnline);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(722, 528);
            this.tabControl.TabIndex = 2;
            // 
            // tabInstalled
            // 
            this.tabInstalled.Controls.Add(this.Installed);
            this.tabInstalled.Controls.Add(this.richTextBox2);
            this.tabInstalled.Controls.Add(this.uxShowExtensionsFolder);
            this.tabInstalled.Controls.Add(this.uxUninstall);
            this.tabInstalled.Location = new System.Drawing.Point(4, 22);
            this.tabInstalled.Name = "tabInstalled";
            this.tabInstalled.Padding = new System.Windows.Forms.Padding(3);
            this.tabInstalled.Size = new System.Drawing.Size(714, 502);
            this.tabInstalled.TabIndex = 0;
            this.tabInstalled.Text = "Installed Extensions";
            this.tabInstalled.UseVisualStyleBackColor = true;
            // 
            // Installed
            // 
            this.Installed.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.Installed.FullRowSelect = true;
            this.Installed.Location = new System.Drawing.Point(6, 39);
            this.Installed.MultiSelect = false;
            this.Installed.Name = "Installed";
            this.Installed.ShowGroups = false;
            this.Installed.Size = new System.Drawing.Size(702, 270);
            this.Installed.TabIndex = 18;
            this.Installed.UseCompatibleStateImageBehavior = false;
            this.Installed.View = System.Windows.Forms.View.Tile;
            this.Installed.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.InstalledSelectedItemChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Pack";
            this.columnHeader1.Width = 250;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Description";
            this.columnHeader2.Width = 1500;
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(6, 315);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(702, 155);
            this.richTextBox2.TabIndex = 10;
            this.richTextBox2.Text = "";
            // 
            // uxShowExtensionsFolder
            // 
            this.uxShowExtensionsFolder.Location = new System.Drawing.Point(6, 6);
            this.uxShowExtensionsFolder.Name = "uxShowExtensionsFolder";
            this.uxShowExtensionsFolder.Size = new System.Drawing.Size(136, 23);
            this.uxShowExtensionsFolder.TabIndex = 9;
            this.uxShowExtensionsFolder.Text = "Show Extensions Folder";
            this.uxShowExtensionsFolder.UseVisualStyleBackColor = true;
            this.uxShowExtensionsFolder.Click += new System.EventHandler(this.UxShowExtensionsFolderClick);
            // 
            // uxUninstall
            // 
            this.uxUninstall.Enabled = false;
            this.uxUninstall.Location = new System.Drawing.Point(402, 476);
            this.uxUninstall.Name = "uxUninstall";
            this.uxUninstall.Size = new System.Drawing.Size(75, 23);
            this.uxUninstall.TabIndex = 7;
            this.uxUninstall.Text = "Uninstall";
            this.uxUninstall.UseVisualStyleBackColor = true;
            this.uxUninstall.Click += new System.EventHandler(this.UxUninstallClick);
            // 
            // tabOnline
            // 
            this.tabOnline.Controls.Add(this.checkBox1);
            this.tabOnline.Controls.Add(this.uxClear);
            this.tabOnline.Controls.Add(this.uxSearch);
            this.tabOnline.Controls.Add(this.label1);
            this.tabOnline.Controls.Add(this.uxPackages);
            this.tabOnline.Controls.Add(this.richTextBox1);
            this.tabOnline.Controls.Add(this.uxSearchText);
            this.tabOnline.Controls.Add(this.uxInstall);
            this.tabOnline.Location = new System.Drawing.Point(4, 22);
            this.tabOnline.Name = "tabOnline";
            this.tabOnline.Padding = new System.Windows.Forms.Padding(3);
            this.tabOnline.Size = new System.Drawing.Size(714, 502);
            this.tabOnline.TabIndex = 1;
            this.tabOnline.Text = "Online";
            this.tabOnline.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(620, 16);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(86, 17);
            this.checkBox1.TabIndex = 28;
            this.checkBox1.Text = "Auto Update";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1CheckedChanged);
            // 
            // uxClear
            // 
            this.uxClear.Location = new System.Drawing.Point(288, 13);
            this.uxClear.Name = "uxClear";
            this.uxClear.Size = new System.Drawing.Size(25, 25);
            this.uxClear.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.uxClear.TabIndex = 27;
            this.uxClear.TabStop = false;
            this.toolTip1.SetToolTip(this.uxClear, "Clear Search");
            // 
            // uxSearch
            // 
            this.uxSearch.Location = new System.Drawing.Point(288, 13);
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
            this.label1.Location = new System.Drawing.Point(8, 481);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Page";
            this.label1.Visible = false;
            // 
            // uxPackages
            // 
            this.uxPackages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Pack,
            this.Description});
            this.uxPackages.FullRowSelect = true;
            this.uxPackages.Location = new System.Drawing.Point(6, 39);
            this.uxPackages.MultiSelect = false;
            this.uxPackages.Name = "uxPackages";
            this.uxPackages.ShowGroups = false;
            this.uxPackages.Size = new System.Drawing.Size(702, 270);
            this.uxPackages.TabIndex = 17;
            this.uxPackages.UseCompatibleStateImageBehavior = false;
            this.uxPackages.View = System.Windows.Forms.View.Tile;
            this.uxPackages.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.OnlineSelectedItemChanged);
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
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(6, 315);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(702, 155);
            this.richTextBox1.TabIndex = 25;
            this.richTextBox1.Text = "";
            // 
            // uxSearchText
            // 
            this.uxSearchText.Location = new System.Drawing.Point(8, 13);
            this.uxSearchText.Name = "uxSearchText";
            this.uxSearchText.Size = new System.Drawing.Size(274, 20);
            this.uxSearchText.TabIndex = 12;
            this.uxSearchText.Text = "Search";
            this.uxSearchText.Click += new System.EventHandler(this.UxSearchTextClick);
            this.uxSearchText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UxSearchTextKeyDown);
            // 
            // uxInstall
            // 
            this.uxInstall.Enabled = false;
            this.uxInstall.Location = new System.Drawing.Point(402, 476);
            this.uxInstall.Name = "uxInstall";
            this.uxInstall.Size = new System.Drawing.Size(75, 23);
            this.uxInstall.TabIndex = 3;
            this.uxInstall.Text = "Install";
            this.uxInstall.UseVisualStyleBackColor = true;
            this.uxInstall.Click += new System.EventHandler(this.InstallButtonClick);
            // 
            // ExtensionManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(722, 528);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExtensionManagerForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Extension Manager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExtensionManagerFormFormClosed);
            this.Load += new System.EventHandler(this.ExtensionManagerFormLoad);
            this.tabControl.ResumeLayout(false);
            this.tabInstalled.ResumeLayout(false);
            this.tabOnline.ResumeLayout(false);
            this.tabOnline.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uxClear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uxSearch)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl;
        private TabPage tabOnline;
        private Button uxInstall;
        private TextBox uxSearchText;
        private RichTextBox richTextBox1;
        private ListView uxPackages;
        private ColumnHeader Pack;
        private ColumnHeader Description;
        private Label label1;
        private PictureBox uxSearch;
        private PictureBox uxClear;
        private ToolTip toolTip1;
        private TabPage tabInstalled;
        private ListView Installed;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private RichTextBox richTextBox2;
        private Button uxShowExtensionsFolder;
        private Button uxUninstall;
        private CheckBox checkBox1;
    }
}