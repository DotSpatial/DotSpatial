// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

namespace DotSpatial.Plugins.CoordinateConverter
{
    partial class FormCoordConverter
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
            this.txtSourceX = new System.Windows.Forms.TextBox();
            this.txtSourceY = new System.Windows.Forms.TextBox();
            this.txtSourceZ = new System.Windows.Forms.TextBox();
            this.txtTargetX = new System.Windows.Forms.TextBox();
            this.txtTargetY = new System.Windows.Forms.TextBox();
            this.txtTargetZ = new System.Windows.Forms.TextBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtSourceX
            // 
            this.txtSourceX.Location = new System.Drawing.Point(12, 46);
            this.txtSourceX.Name = "txtSourceX";
            this.txtSourceX.Size = new System.Drawing.Size(100, 23);
            this.txtSourceX.TabIndex = 2;
            this.txtSourceX.Text = "0";
            this.txtSourceX.TextChanged += new System.EventHandler(this.txtSourceX_TextChanged);
            // 
            // txtSourceY
            // 
            this.txtSourceY.Location = new System.Drawing.Point(118, 46);
            this.txtSourceY.Name = "txtSourceY";
            this.txtSourceY.Size = new System.Drawing.Size(100, 23);
            this.txtSourceY.TabIndex = 4;
            this.txtSourceY.Text = "0";
            this.txtSourceY.TextChanged += new System.EventHandler(this.txtSourceY_TextChanged);
            // 
            // txtSourceZ
            // 
            this.txtSourceZ.Location = new System.Drawing.Point(224, 46);
            this.txtSourceZ.Name = "txtSourceZ";
            this.txtSourceZ.Size = new System.Drawing.Size(100, 23);
            this.txtSourceZ.TabIndex = 6;
            this.txtSourceZ.Text = "0";
            this.txtSourceZ.TextChanged += new System.EventHandler(this.txtSourceZ_TextChanged);
            // 
            // txtTargetX
            // 
            this.txtTargetX.Location = new System.Drawing.Point(12, 139);
            this.txtTargetX.Name = "txtTargetX";
            this.txtTargetX.ReadOnly = true;
            this.txtTargetX.Size = new System.Drawing.Size(100, 23);
            this.txtTargetX.TabIndex = 3;
            // 
            // txtTargetY
            // 
            this.txtTargetY.Location = new System.Drawing.Point(118, 139);
            this.txtTargetY.Name = "txtTargetY";
            this.txtTargetY.ReadOnly = true;
            this.txtTargetY.Size = new System.Drawing.Size(100, 23);
            this.txtTargetY.TabIndex = 4;
            // 
            // txtTargetZ
            // 
            this.txtTargetZ.Location = new System.Drawing.Point(224, 139);
            this.txtTargetZ.Name = "txtTargetZ";
            this.txtTargetZ.ReadOnly = true;
            this.txtTargetZ.Size = new System.Drawing.Size(100, 23);
            this.txtTargetZ.TabIndex = 5;
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(168, 186);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 6;
            this.btnConvert.Text = "&Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(249, 186);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Clos&e";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "X";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(118, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Y";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(224, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "Z";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(224, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 15);
            this.label5.TabIndex = 15;
            this.label5.Text = "Z";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(118, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 15);
            this.label6.TabIndex = 14;
            this.label6.Text = "Y";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 121);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 15);
            this.label7.TabIndex = 13;
            this.label7.Text = "X";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 102);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 15);
            this.label8.TabIndex = 12;
            this.label8.Text = "Target:";
            // 
            // FormCoordConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(336, 224);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.txtTargetZ);
            this.Controls.Add(this.txtTargetY);
            this.Controls.Add(this.txtTargetX);
            this.Controls.Add(this.txtSourceZ);
            this.Controls.Add(this.txtSourceY);
            this.Controls.Add(this.txtSourceX);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCoordConverter";
            this.Text = "Coordinate Converter";
            this.Load += new System.EventHandler(this.FormCoordConverter_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtSourceX;
        private TextBox txtSourceY;
        private TextBox txtSourceZ;
        private TextBox txtTargetX;
        private TextBox txtTargetY;
        private TextBox txtTargetZ;
        private Button btnConvert;
        private Button btnClose;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
    }
}