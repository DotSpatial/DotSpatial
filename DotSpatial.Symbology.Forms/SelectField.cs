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
// The Initial Developer of this Original Code is Kandasamy Prasanna. Created in 09/21/09.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//  Name               Date                Comments
// ---------------------------------------------------------------------------------------------
// Ted Dunsford     |  9/19/2009    |  Added some xml comments
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// SelectField class
    /// </summary>
    public class SelectField : Form
    {
        private readonly List<string> _field = new List<string>();
        private string _fieldName;
        private Button btnCancel;
        private Button btnOK;
        private ComboBox cmbField;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        private Label lblField;
        private Label lblName;
        private TextBox txtName;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(SelectField));
            this.lblField = new Label();
            this.cmbField = new ComboBox();
            this.lblName = new Label();
            this.txtName = new TextBox();
            this.btnCancel = new Button();
            this.btnOK = new Button();
            this.SuspendLayout();
            //
            // lblField
            //
            this.lblField.AccessibleDescription = null;
            this.lblField.AccessibleName = null;
            resources.ApplyResources(this.lblField, "lblField");
            this.lblField.Font = null;
            this.lblField.Name = "lblField";
            //
            // cmbField
            //
            this.cmbField.AccessibleDescription = null;
            this.cmbField.AccessibleName = null;
            resources.ApplyResources(this.cmbField, "cmbField");
            this.cmbField.BackgroundImage = null;
            this.cmbField.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbField.Font = null;
            this.cmbField.FormattingEnabled = true;
            this.cmbField.Name = "cmbField";
            //
            // lblName
            //
            this.lblName.AccessibleDescription = null;
            this.lblName.AccessibleName = null;
            resources.ApplyResources(this.lblName, "lblName");
            this.lblName.Font = null;
            this.lblName.Name = "lblName";
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
            // SelectField
            //
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");

            this.BackgroundImage = null;
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.cmbField);
            this.Controls.Add(this.lblField);
            this.Font = null;
            this.Icon = null;
            this.Name = "SelectField";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        #region Properties

        /// <summary>
        /// get Field Name
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
        }

        #endregion

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

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtName.Text == string.Empty)
                _fieldName = cmbField.SelectedItem as string;
            else
                _fieldName = txtName.Text;

            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region Constructors

        /// <summary>
        /// Creates a new instance of a SelectField
        /// </summary>
        public SelectField()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates a new instance of a SelectField
        /// </summary>
        /// <param name="field"></param>
        public SelectField(List<string> field)
        {
            InitializeComponent();
            _field = field;
            foreach (string st in _field)
                cmbField.Items.Add(st);
        }

        #endregion
    }
}