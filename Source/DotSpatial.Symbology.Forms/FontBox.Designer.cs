using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class FontBox
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(FontBox));
            this.lblFont = new Label();
            this.txtFont = new TextBox();
            this.cmdShowDialog = new Button();
            this.SuspendLayout();
            //
            // lblFont
            //
            this.lblFont.AccessibleDescription = null;
            this.lblFont.AccessibleName = null;
            resources.ApplyResources(this.lblFont, "lblFont");
            this.lblFont.Font = null;
            this.lblFont.Name = "lblFont";
            //
            // txtFont
            //
            this.txtFont.AccessibleDescription = null;
            this.txtFont.AccessibleName = null;
            resources.ApplyResources(this.txtFont, "txtFont");
            this.txtFont.BackgroundImage = null;
            this.txtFont.Font = null;
            this.txtFont.Name = "txtFont";
            //
            // cmdShowDialog
            //
            this.cmdShowDialog.AccessibleDescription = null;
            this.cmdShowDialog.AccessibleName = null;
            resources.ApplyResources(this.cmdShowDialog, "cmdShowDialog");
            this.cmdShowDialog.BackgroundImage = null;
            this.cmdShowDialog.Font = null;
            this.cmdShowDialog.Name = "cmdShowDialog";
            this.cmdShowDialog.UseVisualStyleBackColor = true;
            this.cmdShowDialog.Click += new EventHandler(this.CmdShowDialogClick);
            //
            // FontBox
            //
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");

            this.BackgroundImage = null;
            this.Controls.Add(this.cmdShowDialog);
            this.Controls.Add(this.txtFont);
            this.Controls.Add(this.lblFont);
            this.Font = null;
            this.Name = "FontBox";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Button cmdShowDialog;
        private Label lblFont;
        private TextBox txtFont;
    }
}