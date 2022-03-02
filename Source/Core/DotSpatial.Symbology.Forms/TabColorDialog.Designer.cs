using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class TabColorDialog
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(TabColorDialog));
            this.panel1 = new Panel();
            this.btnApply = new Button();
            this.btnCancel = new Button();
            this.cmdOk = new Button();
            this.tabColorControl1 = new TabColorControl();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // panel1
            //
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.btnApply);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.cmdOk);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            //
            // btnApply
            //
            this.btnApply.AccessibleDescription = null;
            this.btnApply.AccessibleName = null;
            resources.ApplyResources(this.btnApply, "btnApply");
            this.btnApply.BackgroundImage = null;
            this.btnApply.Font = null;
            this.btnApply.Name = "btnApply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new EventHandler(this.BtnApplyClick);
            //
            // btnCancel
            //
            this.btnCancel.AccessibleDescription = null;
            this.btnCancel.AccessibleName = null;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackgroundImage = null;
            this.btnCancel.Font = null;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.BtnCancelClick);
            //
            // cmdOk
            //
            this.cmdOk.AccessibleDescription = null;
            this.cmdOk.AccessibleName = null;
            resources.ApplyResources(this.cmdOk, "cmdOk");
            this.cmdOk.BackgroundImage = null;
            this.cmdOk.Font = null;
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new EventHandler(this.CmdOkClick);
            //
            // tabColorControl1
            //
            this.tabColorControl1.AccessibleDescription = null;
            this.tabColorControl1.AccessibleName = null;
            resources.ApplyResources(this.tabColorControl1, "tabColorControl1");
            this.tabColorControl1.BackgroundImage = null;
            this.tabColorControl1.EndColor = Color.FromArgb(0, 0, 0);
            this.tabColorControl1.Font = null;
            this.tabColorControl1.HueShift = 0;
            this.tabColorControl1.Name = "tabColorControl1";
            this.tabColorControl1.StartColor = Color.FromArgb(0, 0, 0);
            this.tabColorControl1.UseRangeChecked = true;
            //
            // TabColorDialog
            //
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");

            this.BackgroundImage = null;
            this.Controls.Add(this.tabColorControl1);
            this.Controls.Add(this.panel1);
            this.Font = null;
            this.Icon = null;
            this.Name = "TabColorDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private Button btnApply;
        private Button btnCancel;
        private Button cmdOk;
        private Panel panel1;
        private TabColorControl tabColorControl1;
    }
}