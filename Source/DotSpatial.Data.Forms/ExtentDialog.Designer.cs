using System.ComponentModel;
using System.Windows.Forms;
using DotSpatial.Projections.Forms;

namespace DotSpatial.Data.Forms
{
    partial class ExtentDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtentDialog));
            this.grpXY = new System.Windows.Forms.GroupBox();
            this.dbxMaximumX = new DotSpatial.Projections.Forms.DoubleBox();
            this.dbxMinimumY = new DotSpatial.Projections.Forms.DoubleBox();
            this.dbxMaximumY = new DotSpatial.Projections.Forms.DoubleBox();
            this.dbxMinimumX = new DotSpatial.Projections.Forms.DoubleBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkM = new System.Windows.Forms.CheckBox();
            this.dbxMaximumM = new DotSpatial.Projections.Forms.DoubleBox();
            this.dbxMinimumM = new DotSpatial.Projections.Forms.DoubleBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkZ = new System.Windows.Forms.CheckBox();
            this.dbxMaximumZ = new DotSpatial.Projections.Forms.DoubleBox();
            this.dbxMinimumZ = new DotSpatial.Projections.Forms.DoubleBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
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
            resources.ApplyResources(this.grpXY, "grpXY");
            this.grpXY.Name = "grpXY";
            this.grpXY.TabStop = false;
            // 
            // dbxMaximumX
            // 
            this.dbxMaximumX.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMaximumX.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.dbxMaximumX, "dbxMaximumX");
            this.dbxMaximumX.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.dbxMaximumX.IsValid = true;
            this.dbxMaximumX.Name = "dbxMaximumX";
            this.dbxMaximumX.NumberFormat = null;
            this.dbxMaximumX.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMaximumX.Value = 180D;
            // 
            // dbxMinimumY
            // 
            this.dbxMinimumY.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMinimumY.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.dbxMinimumY, "dbxMinimumY");
            this.dbxMinimumY.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.dbxMinimumY.IsValid = true;
            this.dbxMinimumY.Name = "dbxMinimumY";
            this.dbxMinimumY.NumberFormat = null;
            this.dbxMinimumY.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMinimumY.Value = -90D;
            // 
            // dbxMaximumY
            // 
            this.dbxMaximumY.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMaximumY.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.dbxMaximumY, "dbxMaximumY");
            this.dbxMaximumY.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.dbxMaximumY.IsValid = true;
            this.dbxMaximumY.Name = "dbxMaximumY";
            this.dbxMaximumY.NumberFormat = null;
            this.dbxMaximumY.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMaximumY.Value = 90D;
            // 
            // dbxMinimumX
            // 
            this.dbxMinimumX.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMinimumX.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.dbxMinimumX, "dbxMinimumX");
            this.dbxMinimumX.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.dbxMinimumX.IsValid = true;
            this.dbxMinimumX.Name = "dbxMinimumX";
            this.dbxMinimumX.NumberFormat = null;
            this.dbxMinimumX.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMinimumX.Value = -180D;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkM);
            this.groupBox2.Controls.Add(this.dbxMaximumM);
            this.groupBox2.Controls.Add(this.dbxMinimumM);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // chkM
            // 
            resources.ApplyResources(this.chkM, "chkM");
            this.chkM.Name = "chkM";
            this.chkM.UseVisualStyleBackColor = true;
            this.chkM.CheckedChanged += new System.EventHandler(this.ChkMCheckedChanged);
            // 
            // dbxMaximumM
            // 
            this.dbxMaximumM.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMaximumM.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.dbxMaximumM, "dbxMaximumM");
            this.dbxMaximumM.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.dbxMaximumM.IsValid = true;
            this.dbxMaximumM.Name = "dbxMaximumM";
            this.dbxMaximumM.NumberFormat = null;
            this.dbxMaximumM.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMaximumM.Value = 0D;
            // 
            // dbxMinimumM
            // 
            this.dbxMinimumM.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMinimumM.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.dbxMinimumM, "dbxMinimumM");
            this.dbxMinimumM.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.dbxMinimumM.IsValid = true;
            this.dbxMinimumM.Name = "dbxMinimumM";
            this.dbxMinimumM.NumberFormat = null;
            this.dbxMinimumM.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMinimumM.Value = 0D;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkZ);
            this.groupBox3.Controls.Add(this.dbxMaximumZ);
            this.groupBox3.Controls.Add(this.dbxMinimumZ);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // chkZ
            // 
            resources.ApplyResources(this.chkZ, "chkZ");
            this.chkZ.Name = "chkZ";
            this.chkZ.UseVisualStyleBackColor = true;
            this.chkZ.CheckedChanged += new System.EventHandler(this.ChkZCheckedChanged);
            // 
            // dbxMaximumZ
            // 
            this.dbxMaximumZ.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMaximumZ.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.dbxMaximumZ, "dbxMaximumZ");
            this.dbxMaximumZ.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.dbxMaximumZ.IsValid = true;
            this.dbxMaximumZ.Name = "dbxMaximumZ";
            this.dbxMaximumZ.NumberFormat = null;
            this.dbxMaximumZ.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMaximumZ.Value = 0D;
            // 
            // dbxMinimumZ
            // 
            this.dbxMinimumZ.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbxMinimumZ.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.dbxMinimumZ, "dbxMinimumZ");
            this.dbxMinimumZ.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
    "ating point value.";
            this.dbxMinimumZ.IsValid = true;
            this.dbxMinimumZ.Name = "dbxMinimumZ";
            this.dbxMinimumZ.NumberFormat = null;
            this.dbxMinimumZ.RegularHelp = "Enter a double precision floating point value.";
            this.dbxMinimumZ.Value = 0D;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // ExtentDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpXY);
            this.Name = "ExtentDialog";
            this.grpXY.ResumeLayout(false);
            this.grpXY.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox grpXY;
        private DoubleBox dbxMaximumX;
        private DoubleBox dbxMinimumY;
        private DoubleBox dbxMaximumY;
        private DoubleBox dbxMinimumX;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private GroupBox groupBox2;
        private CheckBox chkM;
        private DoubleBox dbxMaximumM;
        private DoubleBox dbxMinimumM;
        private GroupBox groupBox3;
        private CheckBox chkZ;
        private DoubleBox dbxMaximumZ;
        private DoubleBox dbxMinimumZ;
        private Button btnOK;
        private Button btnCancel;
    }
}