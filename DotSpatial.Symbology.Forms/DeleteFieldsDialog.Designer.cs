// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
//
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
// The Initial Developer of this Original Code is Kandasamy Prasanna. Created in 09/15/09.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//  Name               Date                Comments
// ---------------------------------------------------------------------------------------------
// Ted Dunsford     |  9/19/2009    |  Added some xml comments
// ********************************************************************************************************
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A dialog for deleting a field.
    /// </summary>
    partial class DeleteFieldsDialog
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(DeleteFieldsDialog));
            this.lblHeading = new Label();
            this.clb = new CheckedListBox();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.SuspendLayout();
            //
            // lblHeading
            //
            this.lblHeading.AccessibleDescription = null;
            this.lblHeading.AccessibleName = null;
            resources.ApplyResources(this.lblHeading, "lblHeading");
            this.lblHeading.Font = null;
            this.lblHeading.Name = "lblHeading";
            //
            // clb
            //
            this.clb.AccessibleDescription = null;
            this.clb.AccessibleName = null;
            resources.ApplyResources(this.clb, "clb");
            this.clb.BackgroundImage = null;
            this.clb.CheckOnClick = true;
            this.clb.FormattingEnabled = true;
            this.clb.Name = "clb";
            this.clb.SelectedIndexChanged += new EventHandler(this.clb_SelectedIndexChanged);
            //
            // btnCancel
            //
            this.btnCancel.AccessibleDescription = null;
            this.btnCancel.AccessibleName = null;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackgroundImage = null;
            this.btnCancel.Font = null;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            //
            // btnOK
            //
            this.btnOK.AccessibleDescription = null;
            this.btnOK.AccessibleName = null;
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackgroundImage = null;
            this.btnOK.Font = null;
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new EventHandler(this.btnOK_Click);
            //
            // frmDeleteField
            //
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = ((ContainerControl)this).AutoScaleMode;
            this.BackgroundImage = null;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.clb);
            this.Controls.Add(this.lblHeading);
            this.Font = null;
            this.Icon = null;
            this.Name = "DeleteFieldsDialog";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /// <summary>
        /// This event will fire when user select the item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void clb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.clb.SelectedItems.Count > 0)
            {
                if (this.btnOK.Enabled == false)
                    this.btnOK.Enabled = true;
            }
            else
                this.btnOK.Enabled = false;
        }

        #endregion

        private Label lblHeading;
        private CheckedListBox clb;
        private Button btnCancel;
        private Button btnOK;
    }
}