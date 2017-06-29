namespace DotSpatial.Controls
{
    partial class ZoomToCoordinatesDialog
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
            this.BT_Accept.Location = new System.Drawing.Point(58, 130);
            this.BT_Accept.Name = "BT_Accept";
            this.BT_Accept.Size = new System.Drawing.Size(72, 30);
            this.BT_Accept.TabIndex = 3;
            this.BT_Accept.Text = "OK";
            this.BT_Accept.UseVisualStyleBackColor = true;
            this.BT_Accept.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // BT_Cancel
            // 
            this.BT_Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BT_Cancel.Location = new System.Drawing.Point(136, 130);
            this.BT_Cancel.Name = "BT_Cancel";
            this.BT_Cancel.Size = new System.Drawing.Size(72, 30);
            this.BT_Cancel.TabIndex = 4;
            this.BT_Cancel.Text = "Cancel";
            this.BT_Cancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Latitude";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Longitude";
            // 
            // d1
            // 
            this.d1.Location = new System.Drawing.Point(118, 41);
            this.d1.Name = "d1";
            this.d1.Size = new System.Drawing.Size(91, 20);
            this.d1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(210, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "°";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(210, 85);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(11, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "°";
            // 
            // d2
            // 
            this.d2.Location = new System.Drawing.Point(118, 85);
            this.d2.Name = "d2";
            this.d2.Size = new System.Drawing.Size(91, 20);
            this.d2.TabIndex = 2;
            // 
            // latStatus
            // 
            this.latStatus.AutoSize = true;
            this.latStatus.Location = new System.Drawing.Point(28, 64);
            this.latStatus.Name = "latStatus";
            this.latStatus.Size = new System.Drawing.Size(58, 13);
            this.latStatus.TabIndex = 24;
            this.latStatus.Text = "latWarning";
            // 
            // lonStatus
            // 
            this.lonStatus.AutoSize = true;
            this.lonStatus.Location = new System.Drawing.Point(28, 109);
            this.lonStatus.Name = "lonStatus";
            this.lonStatus.Size = new System.Drawing.Size(67, 13);
            this.lonStatus.TabIndex = 25;
            this.lonStatus.Text = "longWarning";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(182, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Please enter the desired coordinates:";
            // 
            // ZoomToCoordinatesDialog
            // 
            this.AcceptButton = this.BT_Accept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(277, 172);
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
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Zoom to Coordinates";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BT_Accept;
        private System.Windows.Forms.Button BT_Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox d1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox d2;
        private System.Windows.Forms.Label latStatus;
        private System.Windows.Forms.Label lonStatus;
        private System.Windows.Forms.Label label4;
    }
}