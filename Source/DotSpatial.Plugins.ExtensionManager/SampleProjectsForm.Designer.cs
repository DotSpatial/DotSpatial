using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Plugins.ExtensionManager
{
    internal partial class SampleProjectsForm
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
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.btnUninstall = new Button();
            this.btnOK = new Button();
            this.listBoxTemplates = new ListBox();
            this.label1 = new Label();
            this.tabPage2 = new TabPage();
            this.uxFeedSelection = new ComboBox();
            this.btnInstall = new Button();
            this.uxOnlineProjects = new ListBox();
            this.label2 = new Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();

            // tabControl1
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(439, 262);
            this.tabControl1.TabIndex = 3;

            // tabPage1
            this.tabPage1.Controls.Add(this.btnUninstall);
            this.tabPage1.Controls.Add(this.btnOK);
            this.tabPage1.Controls.Add(this.listBoxTemplates);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.Size = new Size(431, 236);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Installed Sample Projects";
            this.tabPage1.UseVisualStyleBackColor = true;

            // btnUninstall
            this.btnUninstall.Anchor = (AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right));
            this.btnUninstall.Enabled = false;
            this.btnUninstall.Location = new Point(351, 6);
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new Size(72, 23);
            this.btnUninstall.TabIndex = 6;
            this.btnUninstall.Text = "Uninstall";
            this.btnUninstall.UseVisualStyleBackColor = true;
            this.btnUninstall.Click += new EventHandler(this.BtnUninstallClick);

            // btnOK
            this.btnOK.Anchor = (AnchorStyles)((AnchorStyles.Bottom | AnchorStyles.Right));
            this.btnOK.Location = new Point(356, 210);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new Size(72, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.BtnOkClick);

            // listBoxTemplates
            this.listBoxTemplates.Anchor = (AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right));
            this.listBoxTemplates.FormattingEnabled = true;
            this.listBoxTemplates.Location = new Point(3, 32);
            this.listBoxTemplates.Name = "listBoxTemplates";
            this.listBoxTemplates.Size = new Size(422, 173);
            this.listBoxTemplates.TabIndex = 3;
            this.listBoxTemplates.DoubleClick += new EventHandler(this.ListBoxTemplatesDoubleClick);

            // label1
            this.label1.AutoSize = true;
            this.label1.Location = new Point(6, 10);
            this.label1.Name = "label1";
            this.label1.Size = new Size(192, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Please select a sample project to open:";

            // tabPage2
            this.tabPage2.Controls.Add(this.uxFeedSelection);
            this.tabPage2.Controls.Add(this.btnInstall);
            this.tabPage2.Controls.Add(this.uxOnlineProjects);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new Padding(3);
            this.tabPage2.Size = new Size(431, 236);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Online";
            this.tabPage2.UseVisualStyleBackColor = true;

            // uxFeedSelection
            this.uxFeedSelection.DropDownStyle = ComboBoxStyle.DropDownList;
            this.uxFeedSelection.Font = new Font("Arial Narrow", 8.25F, FontStyle.Bold, GraphicsUnit.Point, (byte)(0));
            this.uxFeedSelection.FormattingEnabled = true;
            this.uxFeedSelection.Items.AddRange(new object[] { "Official Sample Projects", "User Uploaded Sample Projects" });
            this.uxFeedSelection.Location = new Point(246, 6);
            this.uxFeedSelection.Name = "uxFeedSelection";
            this.uxFeedSelection.Size = new Size(179, 23);
            this.uxFeedSelection.TabIndex = 16;
            this.uxFeedSelection.Visible = false;

            // btnInstall
            this.btnInstall.Anchor = (AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right));
            this.btnInstall.Enabled = false;
            this.btnInstall.Location = new Point(356, 210);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new Size(72, 23);
            this.btnInstall.TabIndex = 7;
            this.btnInstall.Text = "Install";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new EventHandler(this.BtnOkOnlineClick);

            // uxOnlineProjects
            this.uxOnlineProjects.Anchor = (AnchorStyles)((((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right));
            this.uxOnlineProjects.FormattingEnabled = true;
            this.uxOnlineProjects.Location = new Point(3, 32);
            this.uxOnlineProjects.Name = "uxOnlineProjects";
            this.uxOnlineProjects.Size = new Size(422, 173);
            this.uxOnlineProjects.TabIndex = 5;

            // label2
            this.label2.AutoSize = true;
            this.label2.Location = new Point(6, 10);
            this.label2.Name = "label2";
            this.label2.Size = new Size(234, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Please select an online sample project  to install:";

            // SampleProjectsForm
            this.AcceptButton = this.btnOK;
            this.ClientSize = new Size(439, 262);
            this.Controls.Add(this.tabControl1);
            this.Name = "SampleProjectsForm";
            this.Text = "Open Sample Project";
            this.Load += new EventHandler(this.TemplateFormLoad);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
        }

        private Button btnInstall;
        private Button btnOK;
        private Button btnUninstall;
        private Label label1;
        private Label label2;
        private ListBox listBoxTemplates;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private ComboBox uxFeedSelection;
        private ListBox uxOnlineProjects;

        #endregion
    }
}