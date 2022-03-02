using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class DynamicVisibilityModeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DynamicVisibilityModeDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.btnZoomedIn = new System.Windows.Forms.Button();
            this.btnZoomedOut = new System.Windows.Forms.Button();
            this.imgZoomedOut = new System.Windows.Forms.Label();
            this.imgZoomedIn = new System.Windows.Forms.Label();
            this.btnAlways = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btnZoomedIn
            // 
            resources.ApplyResources(this.btnZoomedIn, "btnZoomedIn");
            this.btnZoomedIn.Name = "btnZoomedIn";
            this.btnZoomedIn.UseVisualStyleBackColor = true;
            this.btnZoomedIn.Click += new System.EventHandler(this.BtnZoomedInClick);
            // 
            // btnZoomedOut
            // 
            resources.ApplyResources(this.btnZoomedOut, "btnZoomedOut");
            this.btnZoomedOut.Name = "btnZoomedOut";
            this.btnZoomedOut.UseVisualStyleBackColor = true;
            this.btnZoomedOut.Click += new System.EventHandler(this.BtnZoomedOutClick);
            // 
            // imgZoomedOut
            // 
            resources.ApplyResources(this.imgZoomedOut, "imgZoomedOut");
            this.imgZoomedOut.Name = "imgZoomedOut";
            // 
            // imgZoomedIn
            // 
            resources.ApplyResources(this.imgZoomedIn, "imgZoomedIn");
            this.imgZoomedIn.Name = "imgZoomedIn";
            // 
            // btnAlways
            // 
            resources.ApplyResources(this.btnAlways, "btnAlways");
            this.btnAlways.Name = "btnAlways";
            this.btnAlways.UseVisualStyleBackColor = true;
            this.btnAlways.Click += new System.EventHandler(this.BtnAlwaysClick);
            // 
            // DynamicVisibilityModeDialog
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnAlways);
            this.Controls.Add(this.imgZoomedIn);
            this.Controls.Add(this.imgZoomedOut);
            this.Controls.Add(this.btnZoomedOut);
            this.Controls.Add(this.btnZoomedIn);
            this.Controls.Add(this.label1);
            this.Name = "DynamicVisibilityModeDialog";
            this.ShowIcon = false;
            this.ResumeLayout(false);
        }

        #endregion

        private Button btnZoomedIn;
        private Button btnZoomedOut;
        private Label imgZoomedIn;
        private Label imgZoomedOut;
        private Label label1;
        private Button btnAlways;
    }
}