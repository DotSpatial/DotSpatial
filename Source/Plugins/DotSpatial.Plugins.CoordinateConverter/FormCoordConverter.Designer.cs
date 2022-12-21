// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System.Windows.Forms;

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
            this.lblSourceProj = new System.Windows.Forms.Label();
            this.btnChangeSourceProj = new System.Windows.Forms.Button();
            this.btnChangeTargetProj = new System.Windows.Forms.Button();
            this.lblTargetProj = new System.Windows.Forms.Label();
            this.btnCopy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtSourceX
            // 
            this.txtSourceX.Location = new System.Drawing.Point(12, 46);
            this.txtSourceX.Name = "txtSourceX";
            this.txtSourceX.Size = new System.Drawing.Size(140, 23);
            this.txtSourceX.TabIndex = 4;
            this.txtSourceX.Text = "0";
            this.txtSourceX.TextChanged += new System.EventHandler(this.TxtSourceX_TextChanged);
            // 
            // txtSourceY
            // 
            this.txtSourceY.Location = new System.Drawing.Point(158, 46);
            this.txtSourceY.Name = "txtSourceY";
            this.txtSourceY.Size = new System.Drawing.Size(140, 23);
            this.txtSourceY.TabIndex = 6;
            this.txtSourceY.Text = "0";
            this.txtSourceY.TextChanged += new System.EventHandler(this.TxtSourceY_TextChanged);
            // 
            // txtSourceZ
            // 
            this.txtSourceZ.Location = new System.Drawing.Point(304, 46);
            this.txtSourceZ.Name = "txtSourceZ";
            this.txtSourceZ.Size = new System.Drawing.Size(140, 23);
            this.txtSourceZ.TabIndex = 8;
            this.txtSourceZ.Text = "0";
            this.txtSourceZ.TextChanged += new System.EventHandler(this.TxtSourceZ_TextChanged);
            // 
            // txtTargetX
            // 
            this.txtTargetX.Location = new System.Drawing.Point(12, 139);
            this.txtTargetX.Name = "txtTargetX";
            this.txtTargetX.ReadOnly = true;
            this.txtTargetX.Size = new System.Drawing.Size(140, 23);
            this.txtTargetX.TabIndex = 13;
            // 
            // txtTargetY
            // 
            this.txtTargetY.Location = new System.Drawing.Point(158, 139);
            this.txtTargetY.Name = "txtTargetY";
            this.txtTargetY.ReadOnly = true;
            this.txtTargetY.Size = new System.Drawing.Size(140, 23);
            this.txtTargetY.TabIndex = 15;
            // 
            // txtTargetZ
            // 
            this.txtTargetZ.Location = new System.Drawing.Point(304, 139);
            this.txtTargetZ.Name = "txtTargetZ";
            this.txtTargetZ.ReadOnly = true;
            this.txtTargetZ.Size = new System.Drawing.Size(140, 23);
            this.txtTargetZ.TabIndex = 17;
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(288, 189);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(75, 23);
            this.btnConvert.TabIndex = 19;
            this.btnConvert.Text = "&Convert";
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.BtnConvert_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(369, 189);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "Clos&e";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
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
            this.label2.TabIndex = 3;
            this.label2.Text = "X";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(158, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Y";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(304, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Z";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(304, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 15);
            this.label5.TabIndex = 16;
            this.label5.Text = "Z";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(158, 121);
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
            this.label7.TabIndex = 12;
            this.label7.Text = "X";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 102);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 15);
            this.label8.TabIndex = 9;
            this.label8.Text = "Target:";
            // 
            // lblSourceProj
            // 
            this.lblSourceProj.AutoSize = true;
            this.lblSourceProj.Location = new System.Drawing.Point(60, 9);
            this.lblSourceProj.Name = "lblSourceProj";
            this.lblSourceProj.Size = new System.Drawing.Size(22, 15);
            this.lblSourceProj.TabIndex = 1;
            this.lblSourceProj.Text = "---";
            this.lblSourceProj.TextChanged += new System.EventHandler(this.LblSourceProj_TextChanged);
            // 
            // btnChangeSourceProj
            // 
            this.btnChangeSourceProj.Location = new System.Drawing.Point(409, 9);
            this.btnChangeSourceProj.Name = "btnChangeSourceProj";
            this.btnChangeSourceProj.Size = new System.Drawing.Size(35, 23);
            this.btnChangeSourceProj.TabIndex = 2;
            this.btnChangeSourceProj.Text = "...";
            this.btnChangeSourceProj.UseVisualStyleBackColor = true;
            this.btnChangeSourceProj.Click += new System.EventHandler(this.BtnChangeSourceProj_Click);
            // 
            // btnChangeTargetProj
            // 
            this.btnChangeTargetProj.Location = new System.Drawing.Point(409, 102);
            this.btnChangeTargetProj.Name = "btnChangeTargetProj";
            this.btnChangeTargetProj.Size = new System.Drawing.Size(35, 23);
            this.btnChangeTargetProj.TabIndex = 11;
            this.btnChangeTargetProj.Text = "...";
            this.btnChangeTargetProj.UseVisualStyleBackColor = true;
            this.btnChangeTargetProj.Click += new System.EventHandler(this.BtnChangeTargetProj_Click);
            // 
            // lblTargetProj
            // 
            this.lblTargetProj.AutoSize = true;
            this.lblTargetProj.Location = new System.Drawing.Point(60, 102);
            this.lblTargetProj.Name = "lblTargetProj";
            this.lblTargetProj.Size = new System.Drawing.Size(22, 15);
            this.lblTargetProj.TabIndex = 10;
            this.lblTargetProj.Text = "---";
            this.lblTargetProj.TextChanged += new System.EventHandler(this.LblTargetProj_TextChanged);
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(12, 189);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(127, 23);
            this.btnCopy.TabIndex = 18;
            this.btnCopy.Text = "Copy to Clip&board";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // FormCoordConverter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 225);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnChangeTargetProj);
            this.Controls.Add(this.lblTargetProj);
            this.Controls.Add(this.btnChangeSourceProj);
            this.Controls.Add(this.lblSourceProj);
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
        private Label lblSourceProj;
        private Button btnChangeSourceProj;
        private Button btnChangeTargetProj;
        private Label lblTargetProj;
        private Button btnCopy;
    }
}