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
// The Initial Developer of this Original Code is Kandasamy Prasanna. Created in 09/11/09.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// Presumably a class for adding a new column to the attribute Table editor
    /// </summary>
    partial class AddNewColum
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(AddNewColum));
            this.lblName = new Label();
            this.lblType = new Label();
            this.lblSize = new Label();
            this.txtName = new TextBox();
            this.cmbType = new ComboBox();
            this.nudSize = new NumericUpDown();
            this.dialogButtons1 = new DialogButtons();
            ((ISupportInitialize)(this.nudSize)).BeginInit();
            this.SuspendLayout();
            //
            // lblName
            //
            this.lblName.AccessibleDescription = null;
            this.lblName.AccessibleName = null;
            resources.ApplyResources(this.lblName, "lblName");
            this.lblName.Font = null;
            this.lblName.Name = "lblName";
            //
            // lblType
            //
            this.lblType.AccessibleDescription = null;
            this.lblType.AccessibleName = null;
            resources.ApplyResources(this.lblType, "lblType");
            this.lblType.Font = null;
            this.lblType.Name = "lblType";
            //
            // lblSize
            //
            this.lblSize.AccessibleDescription = null;
            this.lblSize.AccessibleName = null;
            resources.ApplyResources(this.lblSize, "lblSize");
            this.lblSize.Font = null;
            this.lblSize.Name = "lblSize";
            //
            // txtName
            //
            this.txtName.AccessibleDescription = null;
            this.txtName.AccessibleName = null;
            resources.ApplyResources(this.txtName, "txtName");
            this.txtName.BackgroundImage = null;
            this.txtName.Font = null;
            this.txtName.Name = "txtName";
            //
            // cmbType
            //
            this.cmbType.AccessibleDescription = null;
            this.cmbType.AccessibleName = null;
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.BackgroundImage = null;
            this.cmbType.Font = null;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            resources.GetString("cmbType.Items"),
            resources.GetString("cmbType.Items1"),
            resources.GetString("cmbType.Items2")});
            this.cmbType.Name = "cmbType";
            //
            // nudSize
            //
            this.nudSize.AccessibleDescription = null;
            this.nudSize.AccessibleName = null;
            resources.ApplyResources(this.nudSize, "nudSize");
            this.nudSize.Font = null;
            this.nudSize.Name = "nudSize";
            this.nudSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            //
            // dialogButtons1
            //
            this.dialogButtons1.AccessibleDescription = null;
            this.dialogButtons1.AccessibleName = null;
            resources.ApplyResources(this.dialogButtons1, "dialogButtons1");
            this.dialogButtons1.BackgroundImage = null;
            this.dialogButtons1.Font = null;
            this.dialogButtons1.Name = "dialogButtons1";
            //
            // AddNewColum
            //
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = ((ContainerControl)this).AutoScaleMode;
            this.BackgroundImage = null;
            this.Controls.Add(this.dialogButtons1);
            this.Controls.Add(this.nudSize);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblSize);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.lblName);
            this.Font = null;
            this.Name = "AddNewColum";
            ((ISupportInitialize)(this.nudSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Label lblName;
        private Label lblType;
        private Label lblSize;
        private TextBox txtName;
        private ComboBox cmbType;
        private NumericUpDown nudSize;
        private DialogButtons dialogButtons1;
    }
}