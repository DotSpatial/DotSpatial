using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Projections.Forms
{
    public partial class ProjectionCustomDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProjectionCustomDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmdOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbTransform = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkSouth = new System.Windows.Forms.CheckBox();
            this.grpGeographic = new System.Windows.Forms.GroupBox();
            this.dbB = new DotSpatial.Projections.Forms.DoubleBox();
            this.dbA = new DotSpatial.Projections.Forms.DoubleBox();
            this.radWGS84 = new System.Windows.Forms.RadioButton();
            this.radGridShift = new System.Windows.Forms.RadioButton();
            this.rad7 = new System.Windows.Forms.RadioButton();
            this.rad3 = new System.Windows.Forms.RadioButton();
            this.cmbEllipsoid = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbDatums = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dbMeridian = new DotSpatial.Projections.Forms.DoubleBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbMeridian = new System.Windows.Forms.ComboBox();
            this.dbZone = new DotSpatial.Projections.Forms.DoubleBox();
            this.dbLat2 = new DotSpatial.Projections.Forms.DoubleBox();
            this.dbLat1 = new DotSpatial.Projections.Forms.DoubleBox();
            this.dbScaleFactor = new DotSpatial.Projections.Forms.DoubleBox();
            this.dbLatitudeOfOrigin = new DotSpatial.Projections.Forms.DoubleBox();
            this.dbCentralMeridian = new DotSpatial.Projections.Forms.DoubleBox();
            this.dbFalseNorthing = new DotSpatial.Projections.Forms.DoubleBox();
            this.dbFalseEasting = new DotSpatial.Projections.Forms.DoubleBox();
            this.panel1.SuspendLayout();
            this.grpGeographic.SuspendLayout();
            this.SuspendLayout();
            //
            // panel1
            //
            this.panel1.Controls.Add(this.btnApply);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.cmdOk);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            //
            // btnApply
            //
            resources.ApplyResources(this.btnApply, "btnApply");
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnApply.Name = "btnApply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.BtnApplyClick);
            //
            // btnCancel
            //
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            //
            // cmdOk
            //
            resources.ApplyResources(this.cmdOk, "cmdOk");
            this.cmdOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOk.Name = "cmdOk";
            this.cmdOk.UseVisualStyleBackColor = true;
            this.cmdOk.Click += new System.EventHandler(this.CmdOkClick);
            //
            // label1
            //
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            //
            // cmbTransform
            //
            this.cmbTransform.FormattingEnabled = true;
            resources.ApplyResources(this.cmbTransform, "cmbTransform");
            this.cmbTransform.Name = "cmbTransform";
            //
            // comboBox1
            //
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
                                                           resources.GetString("comboBox1.Items"),
                                                           resources.GetString("comboBox1.Items1"),
                                                           resources.GetString("comboBox1.Items2")});
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.Name = "comboBox1";
            //
            // label2
            //
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            //
            // chkSouth
            //
            resources.ApplyResources(this.chkSouth, "chkSouth");
            this.chkSouth.Name = "chkSouth";
            this.chkSouth.UseVisualStyleBackColor = true;
            //
            // grpGeographic
            //
            this.grpGeographic.Controls.Add(this.dbB);
            this.grpGeographic.Controls.Add(this.dbA);
            this.grpGeographic.Controls.Add(this.radWGS84);
            this.grpGeographic.Controls.Add(this.radGridShift);
            this.grpGeographic.Controls.Add(this.rad7);
            this.grpGeographic.Controls.Add(this.rad3);
            this.grpGeographic.Controls.Add(this.cmbEllipsoid);
            this.grpGeographic.Controls.Add(this.label5);
            this.grpGeographic.Controls.Add(this.cmbDatums);
            this.grpGeographic.Controls.Add(this.label4);
            this.grpGeographic.Controls.Add(this.dbMeridian);
            this.grpGeographic.Controls.Add(this.label3);
            this.grpGeographic.Controls.Add(this.cmbMeridian);
            resources.ApplyResources(this.grpGeographic, "grpGeographic");
            this.grpGeographic.Name = "grpGeographic";
            this.grpGeographic.TabStop = false;
            //
            // dbB
            //
            this.dbB.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbB.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.dbB, "dbB");
            this.dbB.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                   "ating point value.";
            this.dbB.IsValid = true;
            this.dbB.Name = "dbB";
            this.dbB.NumberFormat = null;
            this.dbB.RegularHelp = "Enter a double precision floating point value.";
            this.dbB.Value = 0D;
            //
            // dbA
            //
            this.dbA.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbA.BackColorRegular = System.Drawing.Color.Empty;
            resources.ApplyResources(this.dbA, "dbA");
            this.dbA.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                   "ating point value.";
            this.dbA.IsValid = true;
            this.dbA.Name = "dbA";
            this.dbA.NumberFormat = null;
            this.dbA.RegularHelp = "Enter a double precision floating point value.";
            this.dbA.Value = 0D;
            //
            // radWGS84
            //
            resources.ApplyResources(this.radWGS84, "radWGS84");
            this.radWGS84.Name = "radWGS84";
            this.radWGS84.TabStop = true;
            this.radWGS84.UseVisualStyleBackColor = true;
            //
            // radGridShift
            //
            resources.ApplyResources(this.radGridShift, "radGridShift");
            this.radGridShift.Name = "radGridShift";
            this.radGridShift.TabStop = true;
            this.radGridShift.UseVisualStyleBackColor = true;
            //
            // rad7
            //
            resources.ApplyResources(this.rad7, "rad7");
            this.rad7.Name = "rad7";
            this.rad7.TabStop = true;
            this.rad7.UseVisualStyleBackColor = true;
            //
            // rad3
            //
            resources.ApplyResources(this.rad3, "rad3");
            this.rad3.Name = "rad3";
            this.rad3.TabStop = true;
            this.rad3.UseVisualStyleBackColor = true;
            //
            // cmbEllipsoid
            //
            this.cmbEllipsoid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEllipsoid.FormattingEnabled = true;
            resources.ApplyResources(this.cmbEllipsoid, "cmbEllipsoid");
            this.cmbEllipsoid.Name = "cmbEllipsoid";
            this.cmbEllipsoid.SelectedIndexChanged += new System.EventHandler(this.CmbEllipsoidSelectedIndexChanged);
            //
            // label5
            //
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            //
            // cmbDatums
            //
            this.cmbDatums.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatums.FormattingEnabled = true;
            resources.ApplyResources(this.cmbDatums, "cmbDatums");
            this.cmbDatums.Name = "cmbDatums";
            this.cmbDatums.SelectedIndexChanged += new System.EventHandler(this.CmbDatumsSelectedIndexChanged);
            //
            // label4
            //
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            //
            // dbMeridian
            //
            resources.ApplyResources(this.dbMeridian, "dbMeridian");
            this.dbMeridian.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbMeridian.BackColorRegular = System.Drawing.Color.Empty;
            this.dbMeridian.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                          "ating point value.";
            this.dbMeridian.IsValid = true;
            this.dbMeridian.Name = "dbMeridian";
            this.dbMeridian.NumberFormat = null;
            this.dbMeridian.RegularHelp = "Enter a double precision floating point value.";
            this.dbMeridian.Value = 0D;
            //
            // label3
            //
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            //
            // cmbMeridian
            //
            this.cmbMeridian.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMeridian.FormattingEnabled = true;
            resources.ApplyResources(this.cmbMeridian, "cmbMeridian");
            this.cmbMeridian.Name = "cmbMeridian";
            this.cmbMeridian.SelectedIndexChanged += new System.EventHandler(this.CmbMeridianSelectedIndexChanged);
            //
            // dbZone
            //
            resources.ApplyResources(this.dbZone, "dbZone");
            this.dbZone.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbZone.BackColorRegular = System.Drawing.Color.Empty;
            this.dbZone.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                      "ating point value.";
            this.dbZone.IsValid = true;
            this.dbZone.Name = "dbZone";
            this.dbZone.NumberFormat = null;
            this.dbZone.RegularHelp = "Enter a double precision floating point value.";
            this.dbZone.Value = 0D;
            //
            // dbLat2
            //
            resources.ApplyResources(this.dbLat2, "dbLat2");
            this.dbLat2.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbLat2.BackColorRegular = System.Drawing.Color.Empty;
            this.dbLat2.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                      "ating point value.";
            this.dbLat2.IsValid = true;
            this.dbLat2.Name = "dbLat2";
            this.dbLat2.NumberFormat = null;
            this.dbLat2.RegularHelp = "Enter a double precision floating point value.";
            this.dbLat2.Value = 0D;
            //
            // dbLat1
            //
            resources.ApplyResources(this.dbLat1, "dbLat1");
            this.dbLat1.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbLat1.BackColorRegular = System.Drawing.Color.Empty;
            this.dbLat1.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                      "ating point value.";
            this.dbLat1.IsValid = true;
            this.dbLat1.Name = "dbLat1";
            this.dbLat1.NumberFormat = null;
            this.dbLat1.RegularHelp = "Enter a double precision floating point value.";
            this.dbLat1.Value = 0D;
            //
            // dbScaleFactor
            //
            resources.ApplyResources(this.dbScaleFactor, "dbScaleFactor");
            this.dbScaleFactor.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbScaleFactor.BackColorRegular = System.Drawing.Color.Empty;
            this.dbScaleFactor.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                             "ating point value.";
            this.dbScaleFactor.IsValid = true;
            this.dbScaleFactor.Name = "dbScaleFactor";
            this.dbScaleFactor.NumberFormat = null;
            this.dbScaleFactor.RegularHelp = "Enter a double precision floating point value.";
            this.dbScaleFactor.Value = 0D;
            //
            // dbLatitudeOfOrigin
            //
            resources.ApplyResources(this.dbLatitudeOfOrigin, "dbLatitudeOfOrigin");
            this.dbLatitudeOfOrigin.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbLatitudeOfOrigin.BackColorRegular = System.Drawing.Color.Empty;
            this.dbLatitudeOfOrigin.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                                  "ating point value.";
            this.dbLatitudeOfOrigin.IsValid = true;
            this.dbLatitudeOfOrigin.Name = "dbLatitudeOfOrigin";
            this.dbLatitudeOfOrigin.NumberFormat = null;
            this.dbLatitudeOfOrigin.RegularHelp = "Enter a double precision floating point value.";
            this.dbLatitudeOfOrigin.Value = 0D;
            //
            // dbCentralMeridian
            //
            resources.ApplyResources(this.dbCentralMeridian, "dbCentralMeridian");
            this.dbCentralMeridian.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbCentralMeridian.BackColorRegular = System.Drawing.Color.Empty;
            this.dbCentralMeridian.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                                 "ating point value.";
            this.dbCentralMeridian.IsValid = true;
            this.dbCentralMeridian.Name = "dbCentralMeridian";
            this.dbCentralMeridian.NumberFormat = null;
            this.dbCentralMeridian.RegularHelp = "Enter a double precision floating point value.";
            this.dbCentralMeridian.Value = 0D;
            //
            // dbFalseNorthing
            //
            resources.ApplyResources(this.dbFalseNorthing, "dbFalseNorthing");
            this.dbFalseNorthing.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbFalseNorthing.BackColorRegular = System.Drawing.Color.Empty;
            this.dbFalseNorthing.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                               "ating point value.";
            this.dbFalseNorthing.IsValid = true;
            this.dbFalseNorthing.Name = "dbFalseNorthing";
            this.dbFalseNorthing.NumberFormat = null;
            this.dbFalseNorthing.RegularHelp = "Enter a double precision floating point value.";
            this.dbFalseNorthing.Value = 0D;
            //
            // dbFalseEasting
            //
            resources.ApplyResources(this.dbFalseEasting, "dbFalseEasting");
            this.dbFalseEasting.BackColorInvalid = System.Drawing.Color.Salmon;
            this.dbFalseEasting.BackColorRegular = System.Drawing.Color.Empty;
            this.dbFalseEasting.InvalidHelp = "The value entered could not be correctly parsed into a valid double precision flo" +
                                              "ating point value.";
            this.dbFalseEasting.IsValid = true;
            this.dbFalseEasting.Name = "dbFalseEasting";
            this.dbFalseEasting.NumberFormat = null;
            this.dbFalseEasting.RegularHelp = "Enter a double precision floating point value.";
            this.dbFalseEasting.Value = 0D;
            //
            // ProjectionCustomDialog
            //
            this.AcceptButton = this.cmdOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.grpGeographic);
            this.Controls.Add(this.chkSouth);
            this.Controls.Add(this.dbZone);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dbLat2);
            this.Controls.Add(this.dbLat1);
            this.Controls.Add(this.dbScaleFactor);
            this.Controls.Add(this.dbLatitudeOfOrigin);
            this.Controls.Add(this.dbCentralMeridian);
            this.Controls.Add(this.dbFalseNorthing);
            this.Controls.Add(this.dbFalseEasting);
            this.Controls.Add(this.cmbTransform);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectionCustomDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.ProjectionCustomDialogLoad);
            this.panel1.ResumeLayout(false);
            this.grpGeographic.ResumeLayout(false);
            this.grpGeographic.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Button btnApply;
        private Button btnCancel;
        private CheckBox chkSouth;
        private ComboBox cmbDatums;
        private ComboBox cmbEllipsoid;
        private ComboBox cmbMeridian;
        private ComboBox cmbTransform;
        private Button cmdOk;
        private ComboBox comboBox1;
        private DoubleBox dbA;
        private DoubleBox dbB;
        private DoubleBox dbCentralMeridian;
        private DoubleBox dbFalseEasting;
        private DoubleBox dbFalseNorthing;
        private DoubleBox dbLat1;
        private DoubleBox dbLat2;
        private DoubleBox dbLatitudeOfOrigin;
        private DoubleBox dbMeridian;
        private DoubleBox dbScaleFactor;
        private DoubleBox dbZone;
        private GroupBox grpGeographic;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Panel panel1;
        private RadioButton rad3;
        private RadioButton rad7;
        private RadioButton radGridShift;
        private RadioButton radWGS84;
    }
}