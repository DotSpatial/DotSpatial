namespace DotSpatial.Modeling.Forms
{
    partial class ExtentDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtentDialog));
            this.grpXY = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dbxMinimumX = new DotSpatial.Projections.Forms.DoubleBox();
            this.dbxMaximumY = new DotSpatial.Projections.Forms.DoubleBox();
            this.dbxMinimumY = new DotSpatial.Projections.Forms.DoubleBox();
            this.dbxMaximumX = new DotSpatial.Projections.Forms.DoubleBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dbxMinimumM = new DotSpatial.Projections.Forms.DoubleBox();
            this.dbxMaximumM = new DotSpatial.Projections.Forms.DoubleBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dbxMaximumZ = new DotSpatial.Projections.Forms.DoubleBox();
            this.dbxMinimumZ = new DotSpatial.Projections.Forms.DoubleBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkM = new System.Windows.Forms.CheckBox();
            this.chkZ = new System.Windows.Forms.CheckBox();
            this.grpXY.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            //
            // grpXY
            //
            this.grpXY.Controls.Add(this.dbxMaximumX);
            this.grpXY.Controls.Add(this.dbxMinimumY);
            this.grpXY.Controls.Add(this.dbxMaximumY);
            this.grpXY.Controls.Add(this.dbxMinimumX);
            this.grpXY.Controls.Add(this.label4);
            this.grpXY.Controls.Add(this.label3);
            this.grpXY.Controls.Add(this.label2);
            this.grpXY.Controls.Add(this.label1);
            this.grpXY.Location = new System.Drawing.Point(17, 14);
            this.grpXY.Name = "grpXY";
            this.grpXY.Size = new System.Drawing.Size(351, 155);
            this.grpXY.TabIndex = 0;
            this.grpXY.TabStop = false;
            this.grpXY.Text = "X and Y";
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Minimum X";
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(259, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Maximum X";
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(144, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Maximum Y:";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(134, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Minimum Y:";
            //
            // dbxMinimumX
            //
            this.dbxMinimumX.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMinimumX.BackColorRegular = System.Drawing.Color.Empty;
            this.dbxMinimumX.Caption = "";
            this.dbxMinimumX.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                "ating point value.";
            this.dbxMinimumX.IsValid = true;
            this.dbxMinimumX.Location = new System.Drawing.Point(6, 71);
            this.dbxMinimumX.Name = "dbxMinimumX";
            this.dbxMinimumX.NumberFormat = null;
            this.dbxMinimumX.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMinimumX.Size = new System.Drawing.Size(132, 27);
            this.dbxMinimumX.TabIndex = 4;
            this.dbxMinimumX.Value = -180D;
            //
            // dbxMaximumY
            //
            this.dbxMaximumY.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMaximumY.BackColorRegular = System.Drawing.Color.Empty;
            this.dbxMaximumY.Caption = "";
            this.dbxMaximumY.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                "ating point value.";
            this.dbxMaximumY.IsValid = true;
            this.dbxMaximumY.Location = new System.Drawing.Point(101, 15);
            this.dbxMaximumY.Name = "dbxMaximumY";
            this.dbxMaximumY.NumberFormat = null;
            this.dbxMaximumY.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMaximumY.Size = new System.Drawing.Size(132, 27);
            this.dbxMaximumY.TabIndex = 5;
            this.dbxMaximumY.Value = 90D;
            //
            // dbxMinimumY
            //
            this.dbxMinimumY.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMinimumY.BackColorRegular = System.Drawing.Color.Empty;
            this.dbxMinimumY.Caption = "";
            this.dbxMinimumY.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                "ating point value.";
            this.dbxMinimumY.IsValid = true;
            this.dbxMinimumY.Location = new System.Drawing.Point(101, 122);
            this.dbxMinimumY.Name = "dbxMinimumY";
            this.dbxMinimumY.NumberFormat = null;
            this.dbxMinimumY.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMinimumY.Size = new System.Drawing.Size(132, 27);
            this.dbxMinimumY.TabIndex = 6;
            this.dbxMinimumY.Value = -90D;
            //
            // dbxMaximumX
            //
            this.dbxMaximumX.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMaximumX.BackColorRegular = System.Drawing.Color.Empty;
            this.dbxMaximumX.Caption = "";
            this.dbxMaximumX.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                "ating point value.";
            this.dbxMaximumX.IsValid = true;
            this.dbxMaximumX.Location = new System.Drawing.Point(213, 71);
            this.dbxMaximumX.Name = "dbxMaximumX";
            this.dbxMaximumX.NumberFormat = null;
            this.dbxMaximumX.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMaximumX.Size = new System.Drawing.Size(132, 27);
            this.dbxMaximumX.TabIndex = 7;
            this.dbxMaximumX.Value = 180D;
            //
            // groupBox2
            //
            this.groupBox2.Controls.Add(this.chkM);
            this.groupBox2.Controls.Add(this.dbxMaximumM);
            this.groupBox2.Controls.Add(this.dbxMinimumM);
            this.groupBox2.Location = new System.Drawing.Point(12, 198);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(356, 62);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "               ";
            //
            // dbxMinimumM
            //
            this.dbxMinimumM.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMinimumM.BackColorRegular = System.Drawing.Color.Empty;
            this.dbxMinimumM.Caption = "Minimum M";
            this.dbxMinimumM.Enabled = false;
            this.dbxMinimumM.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                "ating point value.";
            this.dbxMinimumM.IsValid = true;
            this.dbxMinimumM.Location = new System.Drawing.Point(11, 19);
            this.dbxMinimumM.Name = "dbxMinimumM";
            this.dbxMinimumM.NumberFormat = null;
            this.dbxMinimumM.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMinimumM.Size = new System.Drawing.Size(171, 27);
            this.dbxMinimumM.TabIndex = 8;
            this.dbxMinimumM.Value = 0D;
            //
            // dbxMaximumM
            //
            this.dbxMaximumM.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMaximumM.BackColorRegular = System.Drawing.Color.Empty;
            this.dbxMaximumM.Caption = "Maximum M";
            this.dbxMaximumM.Enabled = false;
            this.dbxMaximumM.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                "ating point value.";
            this.dbxMaximumM.IsValid = true;
            this.dbxMaximumM.Location = new System.Drawing.Point(188, 19);
            this.dbxMaximumM.Name = "dbxMaximumM";
            this.dbxMaximumM.NumberFormat = null;
            this.dbxMaximumM.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMaximumM.Size = new System.Drawing.Size(162, 27);
            this.dbxMaximumM.TabIndex = 9;
            this.dbxMaximumM.Value = 0D;
            //
            // groupBox3
            //
            this.groupBox3.Controls.Add(this.chkZ);
            this.groupBox3.Controls.Add(this.dbxMaximumZ);
            this.groupBox3.Controls.Add(this.dbxMinimumZ);
            this.groupBox3.Location = new System.Drawing.Point(12, 277);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(356, 62);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "               ";
            //
            // dbxMaximumZ
            //
            this.dbxMaximumZ.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMaximumZ.BackColorRegular = System.Drawing.Color.Empty;
            this.dbxMaximumZ.Caption = "Maximum Z";
            this.dbxMaximumZ.Enabled = false;
            this.dbxMaximumZ.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                "ating point value.";
            this.dbxMaximumZ.IsValid = true;
            this.dbxMaximumZ.Location = new System.Drawing.Point(188, 19);
            this.dbxMaximumZ.Name = "dbxMaximumZ";
            this.dbxMaximumZ.NumberFormat = null;
            this.dbxMaximumZ.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMaximumZ.Size = new System.Drawing.Size(162, 27);
            this.dbxMaximumZ.TabIndex = 9;
            this.dbxMaximumZ.Value = 0D;
            //
            // dbxMinimumZ
            //
            this.dbxMinimumZ.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMinimumZ.BackColorRegular = System.Drawing.Color.Empty;
            this.dbxMinimumZ.Caption = "Minimum Z";
            this.dbxMinimumZ.Enabled = false;
            this.dbxMinimumZ.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                "ating point value.";
            this.dbxMinimumZ.IsValid = true;
            this.dbxMinimumZ.Location = new System.Drawing.Point(11, 19);
            this.dbxMinimumZ.Name = "dbxMinimumZ";
            this.dbxMinimumZ.NumberFormat = null;
            this.dbxMinimumZ.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMinimumZ.Size = new System.Drawing.Size(171, 27);
            this.dbxMinimumZ.TabIndex = 8;
            this.dbxMinimumZ.Value = 0D;
            //
            // btnOK
            //
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(287, 360);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            //
            // btnCancel
            //
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(200, 360);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            //
            // chkM
            //
            this.chkM.AutoSize = true;
            this.chkM.Location = new System.Drawing.Point(13, 0);
            this.chkM.Name = "chkM";
            this.chkM.Size = new System.Drawing.Size(35, 17);
            this.chkM.TabIndex = 5;
            this.chkM.Text = "M";
            this.chkM.UseVisualStyleBackColor = true;
            this.chkM.CheckedChanged += new System.EventHandler(this.chkM_CheckedChanged);
            //
            // chkZ
            //
            this.chkZ.AutoSize = true;
            this.chkZ.Location = new System.Drawing.Point(13, 0);
            this.chkZ.Name = "chkZ";
            this.chkZ.Size = new System.Drawing.Size(33, 17);
            this.chkZ.TabIndex = 10;
            this.chkZ.Text = "Z";
            this.chkZ.UseVisualStyleBackColor = true;
            this.chkZ.CheckedChanged += new System.EventHandler(this.chkZ_CheckedChanged);
            //
            // ExtentDialog
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 395);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpXY);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExtentDialog";
            this.Text = "ExtentDialog";
            this.grpXY.ResumeLayout(false);
            this.grpXY.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox grpXY;
        private Projections.Forms.DoubleBox dbxMaximumX;
        private Projections.Forms.DoubleBox dbxMinimumY;
        private Projections.Forms.DoubleBox dbxMaximumY;
        private Projections.Forms.DoubleBox dbxMinimumX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkM;
        private Projections.Forms.DoubleBox dbxMaximumM;
        private Projections.Forms.DoubleBox dbxMinimumM;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkZ;
        private Projections.Forms.DoubleBox dbxMaximumZ;
        private Projections.Forms.DoubleBox dbxMinimumZ;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}