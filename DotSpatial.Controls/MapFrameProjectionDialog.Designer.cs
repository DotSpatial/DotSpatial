// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/29/2010 11:03:10 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// ------------------|------------|---------------------------------------------------------------
//        Name       |    Date    |                       Comments
// ------------------|------------|---------------------------------------------------------------
// ********************************************************************************************************
namespace DotSpatial.Controls
{
    /// <summary>
    /// The Designer for the AppDialog Class
    /// </summary>
    partial class MapFrameProjectionDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapFrameProjectionDialog));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.txtEsriString = new System.Windows.Forms.TextBox();
            this.lblEsriString = new System.Windows.Forms.Label();
            this.lblProj4String = new System.Windows.Forms.Label();
            this.txtProj4String = new System.Windows.Forms.TextBox();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tbMain = new System.Windows.Forms.TabPage();
            this.lnkSpatialReference = new System.Windows.Forms.LinkLabel();
            this.txtAuthority = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtProjectionType = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDetails = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnChange = new System.Windows.Forms.Button();
            this.txtAuthorityCode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tcMain.SuspendLayout();
            this.tbMain.SuspendLayout();
            this.tbDetails.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnApply
            // 
            resources.ApplyResources(this.btnApply, "btnApply");
            this.btnApply.Name = "btnApply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // txtEsriString
            // 
            resources.ApplyResources(this.txtEsriString, "txtEsriString");
            this.txtEsriString.Name = "txtEsriString";
            this.txtEsriString.ReadOnly = true;
            // 
            // lblEsriString
            // 
            resources.ApplyResources(this.lblEsriString, "lblEsriString");
            this.lblEsriString.Name = "lblEsriString";
            // 
            // lblProj4String
            // 
            resources.ApplyResources(this.lblProj4String, "lblProj4String");
            this.lblProj4String.Name = "lblProj4String";
            // 
            // txtProj4String
            // 
            resources.ApplyResources(this.txtProj4String, "txtProj4String");
            this.txtProj4String.Name = "txtProj4String";
            this.txtProj4String.ReadOnly = true;
            // 
            // tcMain
            // 
            resources.ApplyResources(this.tcMain, "tcMain");
            this.tcMain.Controls.Add(this.tbMain);
            this.tcMain.Controls.Add(this.tbDetails);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            // 
            // tbMain
            // 
            this.tbMain.Controls.Add(this.txtAuthorityCode);
            this.tbMain.Controls.Add(this.label4);
            this.tbMain.Controls.Add(this.lnkSpatialReference);
            this.tbMain.Controls.Add(this.txtAuthority);
            this.tbMain.Controls.Add(this.label3);
            this.tbMain.Controls.Add(this.txtProjectionType);
            this.tbMain.Controls.Add(this.label2);
            this.tbMain.Controls.Add(this.txtName);
            this.tbMain.Controls.Add(this.label1);
            resources.ApplyResources(this.tbMain, "tbMain");
            this.tbMain.Name = "tbMain";
            this.tbMain.UseVisualStyleBackColor = true;
            // 
            // lnkSpatialReference
            // 
            resources.ApplyResources(this.lnkSpatialReference, "lnkSpatialReference");
            this.lnkSpatialReference.Name = "lnkSpatialReference";
            this.lnkSpatialReference.TabStop = true;
            this.lnkSpatialReference.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSpatialReference_LinkClicked);
            // 
            // txtAuthority
            // 
            resources.ApplyResources(this.txtAuthority, "txtAuthority");
            this.txtAuthority.Name = "txtAuthority";
            this.txtAuthority.ReadOnly = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // txtProjectionType
            // 
            resources.ApplyResources(this.txtProjectionType, "txtProjectionType");
            this.txtProjectionType.Name = "txtProjectionType";
            this.txtProjectionType.ReadOnly = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtName
            // 
            resources.ApplyResources(this.txtName, "txtName");
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tbDetails
            // 
            this.tbDetails.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.tbDetails, "tbDetails");
            this.tbDetails.Name = "tbDetails";
            this.tbDetails.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblEsriString);
            this.panel1.Controls.Add(this.txtEsriString);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblProj4String);
            this.panel2.Controls.Add(this.txtProj4String);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // btnChange
            // 
            resources.ApplyResources(this.btnChange, "btnChange");
            this.btnChange.Name = "btnChange";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChangeToSelected_Click);
            // 
            // txtAuthorityCode
            // 
            resources.ApplyResources(this.txtAuthorityCode, "txtAuthorityCode");
            this.txtAuthorityCode.Name = "txtAuthorityCode";
            this.txtAuthorityCode.ReadOnly = true;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // MapFrameProjectionDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnChange);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.tcMain);
            this.Name = "MapFrameProjectionDialog";
            this.tcMain.ResumeLayout(false);
            this.tbMain.ResumeLayout(false);
            this.tbMain.PerformLayout();
            this.tbDetails.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.TextBox txtEsriString;
        private System.Windows.Forms.Label lblEsriString;
        private System.Windows.Forms.Label lblProj4String;
        private System.Windows.Forms.TextBox txtProj4String;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tbMain;
        private System.Windows.Forms.TabPage tbDetails;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProjectionType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtAuthority;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel lnkSpatialReference;
        private System.Windows.Forms.Button btnChange;
        private System.Windows.Forms.TextBox txtAuthorityCode;
        private System.Windows.Forms.Label label4;
    }
}