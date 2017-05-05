using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class SelectByAttributes
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectByAttributes));
            this.lblLayer = new System.Windows.Forms.Label();
            this.cmbLayers = new System.Windows.Forms.ComboBox();
            this.lblMethod = new System.Windows.Forms.Label();
            this.cmbMethod = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.ttHelp = new System.Windows.Forms.ToolTip(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.sqlQueryControl1 = new DotSpatial.Symbology.Forms.SqlQueryControl();
            this.SuspendLayout();
            // 
            // lblLayer
            // 
            resources.ApplyResources(this.lblLayer, "lblLayer");
            this.lblLayer.Name = "lblLayer";
            // 
            // cmbLayers
            // 
            resources.ApplyResources(this.cmbLayers, "cmbLayers");
            this.cmbLayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLayers.FormattingEnabled = true;
            this.cmbLayers.Name = "cmbLayers";
            this.ttHelp.SetToolTip(this.cmbLayers, resources.GetString("cmbLayers.ToolTip"));
            this.cmbLayers.SelectedIndexChanged += new System.EventHandler(this.CmbLayersSelectedIndexChanged);
            // 
            // lblMethod
            // 
            resources.ApplyResources(this.lblMethod, "lblMethod");
            this.lblMethod.Name = "lblMethod";
            // 
            // cmbMethod
            // 
            resources.ApplyResources(this.cmbMethod, "cmbMethod");
            this.cmbMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMethod.FormattingEnabled = true;
            this.cmbMethod.Items.AddRange(new object[] {
            resources.GetString("cmbMethod.Items"),
            resources.GetString("cmbMethod.Items1"),
            resources.GetString("cmbMethod.Items2"),
            resources.GetString("cmbMethod.Items3")});
            this.cmbMethod.Name = "cmbMethod";
            this.ttHelp.SetToolTip(this.cmbMethod, resources.GetString("cmbMethod.ToolTip"));
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // btnApply
            // 
            resources.ApplyResources(this.btnApply, "btnApply");
            this.btnApply.Name = "btnApply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.BtnApplyClick);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnCloseClick);
            // 
            // sqlQueryControl1
            // 
            this.sqlQueryControl1.AttributeSource = null;
            this.sqlQueryControl1.ExpressionText = "";
            resources.ApplyResources(this.sqlQueryControl1, "sqlQueryControl1");
            this.sqlQueryControl1.Name = "sqlQueryControl1";
            this.sqlQueryControl1.Table = null;
            // 
            // SelectByAttributes
            // 
            this.AcceptButton = this.btnOk;
            this.CancelButton = this.btnClose;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.sqlQueryControl1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cmbMethod);
            this.Controls.Add(this.lblMethod);
            this.Controls.Add(this.cmbLayers);
            this.Controls.Add(this.lblLayer);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectByAttributes";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Button btnApply;
        private Button btnOk;
        private ComboBox cmbLayers;
        private ComboBox cmbMethod;
        private Label lblLayer;
        private Label lblMethod;
        private Button btnClose;
        private SqlQueryControl sqlQueryControl1;
        private ToolTip ttHelp;

    }
}