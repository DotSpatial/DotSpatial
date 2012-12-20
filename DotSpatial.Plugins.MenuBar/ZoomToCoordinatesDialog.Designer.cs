namespace DotSpatial.Plugins.MenuBar
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
            this.AcceptButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.d1 = new System.Windows.Forms.TextBox();
            this.m1 = new System.Windows.Forms.TextBox();
            this.s1 = new System.Windows.Forms.TextBox();
            this.dir1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.dir2 = new System.Windows.Forms.TextBox();
            this.s2 = new System.Windows.Forms.TextBox();
            this.m2 = new System.Windows.Forms.TextBox();
            this.d2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // AcceptButton
            // 
            this.AcceptButton.Enabled = false;
            this.AcceptButton.Location = new System.Drawing.Point(74, 130);
            this.AcceptButton.Name = "AcceptButton";
            this.AcceptButton.Size = new System.Drawing.Size(72, 30);
            this.AcceptButton.TabIndex = 9;
            this.AcceptButton.Text = "OK";
            this.AcceptButton.UseVisualStyleBackColor = true;
            this.AcceptButton.Click += new System.EventHandler(this.AcceptButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Location = new System.Drawing.Point(152, 130);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(72, 30);
            this.CancelButton.TabIndex = 10;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Latitude";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Longitude";
            // 
            // d1
            // 
            this.d1.Location = new System.Drawing.Point(75, 34);
            this.d1.Name = "d1";
            this.d1.Size = new System.Drawing.Size(35, 20);
            this.d1.TabIndex = 1;
            this.d1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.d1_KeyPress);
            // 
            // m1
            // 
            this.m1.Location = new System.Drawing.Point(122, 34);
            this.m1.Name = "m1";
            this.m1.Size = new System.Drawing.Size(35, 20);
            this.m1.TabIndex = 2;
            this.m1.TextChanged += new System.EventHandler(this.m1_TextChanged);
            this.m1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.m1_KeyPress);
            // 
            // s1
            // 
            this.s1.Location = new System.Drawing.Point(170, 34);
            this.s1.Name = "s1";
            this.s1.Size = new System.Drawing.Size(35, 20);
            this.s1.TabIndex = 3;
            this.s1.TextChanged += new System.EventHandler(this.s1_TextChanged);
            this.s1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.s1_KeyPress);
            // 
            // dir1
            // 
            this.dir1.Location = new System.Drawing.Point(222, 34);
            this.dir1.Name = "dir1";
            this.dir1.Size = new System.Drawing.Size(35, 20);
            this.dir1.TabIndex = 4;
            this.dir1.TextChanged += new System.EventHandler(this.dir1_TextChanged);
            this.dir1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dir1_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(110, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "°";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(157, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(9, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "\'";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(204, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "\"";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(258, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "N/S";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(258, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(30, 13);
            this.label7.TabIndex = 23;
            this.label7.Text = "E/W";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(204, 71);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(12, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "\"";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(157, 72);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(9, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "\'";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(110, 71);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(11, 13);
            this.label10.TabIndex = 20;
            this.label10.Text = "°";
            // 
            // dir2
            // 
            this.dir2.Location = new System.Drawing.Point(222, 71);
            this.dir2.Name = "dir2";
            this.dir2.Size = new System.Drawing.Size(35, 20);
            this.dir2.TabIndex = 8;
            this.dir2.TextChanged += new System.EventHandler(this.dir2_TextChanged);
            this.dir2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dir2_KeyPress);
            // 
            // s2
            // 
            this.s2.Location = new System.Drawing.Point(170, 71);
            this.s2.Name = "s2";
            this.s2.Size = new System.Drawing.Size(35, 20);
            this.s2.TabIndex = 7;
            this.s2.TextChanged += new System.EventHandler(this.s2_TextChanged);
            this.s2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.s2_KeyPress);
            // 
            // m2
            // 
            this.m2.Location = new System.Drawing.Point(122, 71);
            this.m2.Name = "m2";
            this.m2.Size = new System.Drawing.Size(35, 20);
            this.m2.TabIndex = 6;
            this.m2.TextChanged += new System.EventHandler(this.m2_TextChanged);
            this.m2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.m2_KeyPress);
            // 
            // d2
            // 
            this.d2.Location = new System.Drawing.Point(75, 71);
            this.d2.Name = "d2";
            this.d2.Size = new System.Drawing.Size(35, 20);
            this.d2.TabIndex = 5;
            this.d2.TextChanged += new System.EventHandler(this.d2_TextChanged);
            this.d2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.d2_KeyPress);
            // 
            // ZoomToCoordinatesDialog
            // 
            this.AcceptButton = this.AcceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 172);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.dir2);
            this.Controls.Add(this.s2);
            this.Controls.Add(this.m2);
            this.Controls.Add(this.d2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dir1);
            this.Controls.Add(this.s1);
            this.Controls.Add(this.m1);
            this.Controls.Add(this.d1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.AcceptButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ZoomToCoordinatesDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Zoom to Coordinates";
            this.Load += new System.EventHandler(this.ZoomToCoordinatesDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AcceptButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox d1;
        private System.Windows.Forms.TextBox m1;
        private System.Windows.Forms.TextBox s1;
        private System.Windows.Forms.TextBox dir1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox dir2;
        private System.Windows.Forms.TextBox s2;
        private System.Windows.Forms.TextBox m2;
        private System.Windows.Forms.TextBox d2;
    }
}