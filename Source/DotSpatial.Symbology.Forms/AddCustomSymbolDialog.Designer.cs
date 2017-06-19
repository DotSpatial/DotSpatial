using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class AddCustomSymbolDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddCustomSymbolDialog));
            this._btnCancel = new System.Windows.Forms.Button();
            this._btnOk = new System.Windows.Forms.Button();
            this._lblName = new System.Windows.Forms.Label();
            this._lblCategory = new System.Windows.Forms.Label();
            this._cmbSymbolCategory = new System.Windows.Forms.ComboBox();
            this._txtSymbolName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            //
            // _btnCancel
            //
            resources.ApplyResources(this._btnCancel, "_btnCancel");
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            //
            // _btnOk
            //
            resources.ApplyResources(this._btnOk, "_btnOk");
            this._btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._btnOk.Name = "_btnOk";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new System.EventHandler(this.BtnOkClick);
            //
            // _lblName
            //
            resources.ApplyResources(this._lblName, "_lblName");
            this._lblName.Name = "_lblName";
            this._lblName.Click += new System.EventHandler(this.LblNameClick);
            //
            // _lblCategory
            //
            resources.ApplyResources(this._lblCategory, "_lblCategory");
            this._lblCategory.Name = "_lblCategory";
            //
            // _cmbSymbolCategory
            //
            resources.ApplyResources(this._cmbSymbolCategory, "_cmbSymbolCategory");
            this._cmbSymbolCategory.FormattingEnabled = true;
            this._cmbSymbolCategory.Name = "_cmbSymbolCategory";
            //
            // _txtSymbolName
            //
            resources.ApplyResources(this._txtSymbolName, "_txtSymbolName");
            this._txtSymbolName.Name = "_txtSymbolName";
            //
            // AddCustomSymbolDialog
            //
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this._txtSymbolName);
            this.Controls.Add(this._cmbSymbolCategory);
            this.Controls.Add(this._lblCategory);
            this.Controls.Add(this._lblName);
            this.Controls.Add(this._btnOk);
            this.Controls.Add(this._btnCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddCustomSymbolDialog";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Button _btnCancel;
        private Button _btnOk;
        private ComboBox _cmbSymbolCategory;
        private Label _lblCategory;
        private Label _lblName;
        private TextBox _txtSymbolName;

    }
}