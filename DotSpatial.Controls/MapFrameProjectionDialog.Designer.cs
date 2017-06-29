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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.txtEsriString = new System.Windows.Forms.TextBox();
            this.lblEsriString = new System.Windows.Forms.Label();
            this.lblProj4String = new System.Windows.Forms.Label();
            this.txtProj4String = new System.Windows.Forms.TextBox();
            this.btnChangeProjection = new System.Windows.Forms.Button();
            this.groupBoxCurrentProjection = new System.Windows.Forms.GroupBox();
            this.panel1.SuspendLayout();
            this.groupBoxCurrentProjection.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.btnApply);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
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
            // 
            // btnChangeProjection
            // 
            resources.ApplyResources(this.btnChangeProjection, "btnChangeProjection");
            this.btnChangeProjection.Name = "btnChangeProjection";
            this.btnChangeProjection.UseVisualStyleBackColor = true;
            this.btnChangeProjection.Click += new System.EventHandler(this.btnChangeProjection_Click);
            // 
            // groupBoxCurrentProjection
            // 
            resources.ApplyResources(this.groupBoxCurrentProjection, "groupBoxCurrentProjection");
            this.groupBoxCurrentProjection.Controls.Add(this.btnChangeProjection);
            this.groupBoxCurrentProjection.Controls.Add(this.lblEsriString);
            this.groupBoxCurrentProjection.Controls.Add(this.txtEsriString);
            this.groupBoxCurrentProjection.Controls.Add(this.lblProj4String);
            this.groupBoxCurrentProjection.Controls.Add(this.txtProj4String);
            this.groupBoxCurrentProjection.Name = "groupBoxCurrentProjection";
            this.groupBoxCurrentProjection.TabStop = false;
            // 
            // MapFrameProjectionDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxCurrentProjection);
            this.Controls.Add(this.panel1);
            this.Name = "MapFrameProjectionDialog";
            this.panel1.ResumeLayout(false);
            this.groupBoxCurrentProjection.ResumeLayout(false);
            this.groupBoxCurrentProjection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.TextBox txtEsriString;
        private System.Windows.Forms.Label lblEsriString;
        private System.Windows.Forms.Label lblProj4String;
        private System.Windows.Forms.TextBox txtProj4String;
        private System.Windows.Forms.Button btnChangeProjection;
        private System.Windows.Forms.GroupBox groupBoxCurrentProjection;
    }
}