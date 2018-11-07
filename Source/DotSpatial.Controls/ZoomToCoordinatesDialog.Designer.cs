using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    partial class ZoomToCoordinatesDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZoomToCoordinatesDialog));
            this.BT_Accept = new System.Windows.Forms.Button();
            this.BT_Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.d1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.d2 = new System.Windows.Forms.TextBox();
            this.latStatus = new System.Windows.Forms.Label();
            this.lonStatus = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BT_Accept
            // 
            resources.ApplyResources(this.BT_Accept, "BT_Accept");
            this.BT_Accept.Name = "BT_Accept";
            this.BT_Accept.UseVisualStyleBackColor = true;
            this.BT_Accept.Click += new System.EventHandler(this.AcceptButtonClick);
            // 
            // BT_Cancel
            // 
            this.BT_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.BT_Cancel, "BT_Cancel");
            this.BT_Cancel.Name = "BT_Cancel";
            this.BT_Cancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // d1
            // 
            resources.ApplyResources(this.d1, "d1");
            this.d1.Name = "d1";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // d2
            // 
            resources.ApplyResources(this.d2, "d2");
            this.d2.Name = "d2";
            // 
            // latStatus
            // 
            resources.ApplyResources(this.latStatus, "latStatus");
            this.latStatus.Name = "latStatus";
            // 
            // lonStatus
            // 
            resources.ApplyResources(this.lonStatus, "lonStatus");
            this.lonStatus.Name = "lonStatus";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // ZoomToCoordinatesDialog
            // 
            this.AcceptButton = this.BT_Accept;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lonStatus);
            this.Controls.Add(this.latStatus);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.d2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.d1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BT_Cancel);
            this.Controls.Add(this.BT_Accept);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ZoomToCoordinatesDialog";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button BT_Accept;
        private Button BT_Cancel;
        private Label label1;
        private Label label2;
        private TextBox d1;
        private Label label3;
        private Label label10;
        private TextBox d2;
        private Label latStatus;
        private Label lonStatus;
        private Label label4;
    }
}