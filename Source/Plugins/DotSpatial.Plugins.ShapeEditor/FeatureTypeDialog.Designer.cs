using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Plugins.ShapeEditor
{
    public partial class FeatureTypeDialog
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
            ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeatureTypeDialog));
            this._btnOk = new Button();
            this._btnCancel = new Button();
            this._cmbFeatureType = new ComboBox();
            this._chkM = new CheckBox();
            this._chkZ = new CheckBox();
            this.label1 = new Label();
            this._tbFilename = new TextBox();
            this.label2 = new Label();
            this._btnSelectFilename = new Button();
            this._sfdFilename = new SaveFileDialog();
            this.SuspendLayout();

            // _btnOk
            resources.ApplyResources(this._btnOk, "_btnOk");
            this._btnOk.DialogResult = DialogResult.OK;
            this._btnOk.Name = "_btnOk";
            this._btnOk.UseVisualStyleBackColor = true;
            this._btnOk.Click += new EventHandler(this.OkButtonClick);

            // _btnCancel
            resources.ApplyResources(this._btnCancel, "_btnCancel");
            this._btnCancel.DialogResult = DialogResult.Cancel;
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new EventHandler(this.CancelButtonClick);

            // _cmbFeatureType
            resources.ApplyResources(this._cmbFeatureType, "_cmbFeatureType");
            this._cmbFeatureType.DropDownStyle = ComboBoxStyle.DropDownList;
            this._cmbFeatureType.FormattingEnabled = true;
            this._cmbFeatureType.Items.AddRange(new object[] { resources.GetString("_cmbFeatureType.Items"), resources.GetString("_cmbFeatureType.Items1"), resources.GetString("_cmbFeatureType.Items2"), resources.GetString("_cmbFeatureType.Items3") });
            this._cmbFeatureType.Name = "_cmbFeatureType";

            // _chkM
            resources.ApplyResources(this._chkM, "_chkM");
            this._chkM.Name = "_chkM";
            this._chkM.UseVisualStyleBackColor = true;

            // _chkZ
            resources.ApplyResources(this._chkZ, "_chkZ");
            this._chkZ.Name = "_chkZ";
            this._chkZ.UseVisualStyleBackColor = true;

            // label1
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";

            // _tbFilename
            resources.ApplyResources(this._tbFilename, "_tbFilename");
            this._tbFilename.Name = "_tbFilename";

            // label2
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";

            // _btnSelectFilename
            resources.ApplyResources(this._btnSelectFilename, "_btnSelectFilename");
            this._btnSelectFilename.Name = "_btnSelectFilename";
            this._btnSelectFilename.UseVisualStyleBackColor = true;
            this._btnSelectFilename.Click += new EventHandler(this.BtnSelectFilenameClick);

            // _sfdFilename
            this._sfdFilename.DefaultExt = "*.shp";
            resources.ApplyResources(this._sfdFilename, "_sfdFilename");

            // FeatureTypeDialog
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this._btnSelectFilename);
            this.Controls.Add(this._tbFilename);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._chkZ);
            this.Controls.Add(this._chkM);
            this.Controls.Add(this._cmbFeatureType);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._btnOk);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FeatureTypeDialog";
            this.ShowIcon = false;
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Button _btnCancel;
        private Button _btnOk;
        private Button _btnSelectFilename;
        private CheckBox _chkM;
        private CheckBox _chkZ;
        private ComboBox _cmbFeatureType;
        private SaveFileDialog _sfdFilename;
        private TextBox _tbFilename;
        private Label label1;
        private Label label2;

    }
}