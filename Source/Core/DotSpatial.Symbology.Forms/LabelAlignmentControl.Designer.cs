using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class LabelAlignmentControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LabelAlignmentControl));
            this.btnDrop = new System.Windows.Forms.Button();
            this.lblAlignmentText = new System.Windows.Forms.Label();
            this.labelAlignmentPicker1 = new DotSpatial.Symbology.Forms.LabelAlignmentPicker();
            this.SuspendLayout();
            //
            // btnDrop
            //
            resources.ApplyResources(this.btnDrop, "btnDrop");
            this.btnDrop.Name = "btnDrop";
            this.btnDrop.UseVisualStyleBackColor = true;
            this.btnDrop.Click += new System.EventHandler(this.BtnDropClick);
            //
            // lblAlignmentText
            //
            this.lblAlignmentText.BackColor = System.Drawing.Color.White;
            this.lblAlignmentText.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.lblAlignmentText, "lblAlignmentText");
            this.lblAlignmentText.Name = "lblAlignmentText";
            //
            // labelAlignmentPicker1
            //
            resources.ApplyResources(this.labelAlignmentPicker1, "labelAlignmentPicker1");
            this.labelAlignmentPicker1.Name = "labelAlignmentPicker1";
            this.labelAlignmentPicker1.Value = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // LabelAlignmentControl
            //
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblAlignmentText);
            this.Controls.Add(this.labelAlignmentPicker1);
            this.Controls.Add(this.btnDrop);
            this.Name = "LabelAlignmentControl";
            resources.ApplyResources(this, "$this");
            this.ResumeLayout(false);
        }

        #endregion

        private Button btnDrop;
        private LabelAlignmentPicker labelAlignmentPicker1;
        private Label lblAlignmentText;

    }
}