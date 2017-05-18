using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class DynamicVisibilityControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DynamicVisibilityControl));
            this.chkUseDynamicVisibility = new System.Windows.Forms.CheckBox();
            this.btnGrabExtents = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkUseDynamicVisibility
            // 
            resources.ApplyResources(this.chkUseDynamicVisibility, "chkUseDynamicVisibility");
            this.chkUseDynamicVisibility.Name = "chkUseDynamicVisibility";
            this.chkUseDynamicVisibility.UseVisualStyleBackColor = true;
            this.chkUseDynamicVisibility.CheckedChanged += new System.EventHandler(this.ChkUseDynamicVisibilityCheckedChanged);
            // 
            // btnGrabExtents
            // 
            resources.ApplyResources(this.btnGrabExtents, "btnGrabExtents");
            this.btnGrabExtents.Name = "btnGrabExtents";
            this.btnGrabExtents.UseVisualStyleBackColor = true;
            this.btnGrabExtents.Click += new System.EventHandler(this.BtnGrabExtentsClick);
            // 
            // DynamicVisibilityControl
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.btnGrabExtents);
            this.Controls.Add(this.chkUseDynamicVisibility);
            this.Name = "DynamicVisibilityControl";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
        }

        #endregion

        private Button btnGrabExtents;
        private CheckBox chkUseDynamicVisibility;

    }
}