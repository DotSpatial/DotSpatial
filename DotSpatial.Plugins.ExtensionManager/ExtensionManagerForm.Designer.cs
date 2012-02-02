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
            this.extensionDescription = new System.Windows.Forms.Label();
            this.installButton = new System.Windows.Forms.Button();
            this.uxPackages = new System.Windows.Forms.ListBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
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
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabOnline);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(584, 362);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(576, 336);
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
            this.splitContainer1.Size = new System.Drawing.Size(570, 294);
            this.splitContainer1.SplitterDistance = 264;
            this.splitContainer1.TabIndex = 4;
            // 
            // grpApps
            // 
            this.grpApps.Controls.Add(this.clbApps);
            this.grpApps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpApps.Location = new System.Drawing.Point(0, 0);
            this.grpApps.Name = "grpApps";
            this.grpApps.Size = new System.Drawing.Size(264, 294);
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
            this.clbApps.Size = new System.Drawing.Size(258, 275);
            this.clbApps.TabIndex = 0;
            this.clbApps.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbApps_ItemCheck);
            // 
            // grpData
            // 
            this.grpData.Controls.Add(this.clbData);
            this.grpData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpData.Location = new System.Drawing.Point(0, 0);
            this.grpData.Name = "grpData";
            this.grpData.Size = new System.Drawing.Size(302, 294);
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
            this.clbData.Size = new System.Drawing.Size(296, 275);
            this.clbData.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.uxShowExtensionsFolder);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 297);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(570, 36);
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
            this.btnCancel.Location = new System.Drawing.Point(507, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Close";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tabOnline
            // 
            this.tabOnline.Controls.Add(this.extensionDescription);
            this.tabOnline.Controls.Add(this.installButton);
            this.tabOnline.Controls.Add(this.uxPackages);
            this.tabOnline.Location = new System.Drawing.Point(4, 22);
            this.tabOnline.Name = "tabOnline";
            this.tabOnline.Padding = new System.Windows.Forms.Padding(3);
            this.tabOnline.Size = new System.Drawing.Size(576, 336);
            this.tabOnline.TabIndex = 1;
            this.tabOnline.Text = "Online";
            this.tabOnline.UseVisualStyleBackColor = true;
            // 
            // extensionDescription
            // 
            this.extensionDescription.Location = new System.Drawing.Point(328, 42);
            this.extensionDescription.Name = "extensionDescription";
            this.extensionDescription.Size = new System.Drawing.Size(245, 289);
            this.extensionDescription.TabIndex = 5;
            // 
            // installButton
            // 
            this.installButton.Enabled = false;
            this.installButton.Location = new System.Drawing.Point(331, 6);
            this.installButton.Name = "installButton";
            this.installButton.Size = new System.Drawing.Size(75, 23);
            this.installButton.TabIndex = 3;
            this.installButton.Text = "Install";
            this.installButton.UseVisualStyleBackColor = true;
            this.installButton.Click += new System.EventHandler(this.InstallButton_Click);
            // 
            // uxPackages
            // 
            this.uxPackages.Dock = System.Windows.Forms.DockStyle.Left;
            this.uxPackages.FormattingEnabled = true;
            this.uxPackages.Location = new System.Drawing.Point(3, 3);
            this.uxPackages.Name = "uxPackages";
            this.uxPackages.Size = new System.Drawing.Size(319, 330);
            this.uxPackages.TabIndex = 2;
            this.uxPackages.SelectedValueChanged += new System.EventHandler(this.uxPackages_SelectedValueChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(576, 336);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Updates";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(301, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "This planned feature will help keep your extensions up to date.";
            // 
            // ExtensionManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 362);
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
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
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
        private System.Windows.Forms.Button installButton;
        private System.Windows.Forms.ListBox uxPackages;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label extensionDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button uxShowExtensionsFolder;

    }
}