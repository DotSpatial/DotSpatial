using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    partial class OptionsForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.chkZoomOutFartherThanMaxExtent = new System.Windows.Forms.CheckBox();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chkEditLegendBoxes = new System.Windows.Forms.CheckBox();
            this.chkShowLegendMenus = new System.Windows.Forms.CheckBox();
            this.cmbLanguage = new System.Windows.Forms.ComboBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // chkZoomOutFartherThanMaxExtent
            // 
            resources.ApplyResources(this.chkZoomOutFartherThanMaxExtent, "chkZoomOutFartherThanMaxExtent");
            this.chkZoomOutFartherThanMaxExtent.Name = "chkZoomOutFartherThanMaxExtent";
            this.toolTip1.SetToolTip(this.chkZoomOutFartherThanMaxExtent, resources.GetString("chkZoomOutFartherThanMaxExtent.ToolTip"));
            this.chkZoomOutFartherThanMaxExtent.UseVisualStyleBackColor = true;
            // 
            // btOk
            // 
            resources.ApplyResources(this.btOk, "btOk");
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Name = "btOk";
            this.toolTip1.SetToolTip(this.btOk, resources.GetString("btOk.ToolTip"));
            this.btOk.UseVisualStyleBackColor = true;
            this.btOk.Click += new System.EventHandler(this.BtOkClick);
            // 
            // btCancel
            // 
            resources.ApplyResources(this.btCancel, "btCancel");
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Name = "btCancel";
            this.toolTip1.SetToolTip(this.btCancel, resources.GetString("btCancel.ToolTip"));
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // chkEditLegendBoxes
            // 
            resources.ApplyResources(this.chkEditLegendBoxes, "chkEditLegendBoxes");
            this.chkEditLegendBoxes.Name = "chkEditLegendBoxes";
            this.toolTip1.SetToolTip(this.chkEditLegendBoxes, resources.GetString("chkEditLegendBoxes.ToolTip"));
            this.chkEditLegendBoxes.UseVisualStyleBackColor = true;
            // 
            // chkShowLegendMenus
            // 
            resources.ApplyResources(this.chkShowLegendMenus, "chkShowLegendMenus");
            this.chkShowLegendMenus.Name = "chkShowLegendMenus";
            this.toolTip1.SetToolTip(this.chkShowLegendMenus, resources.GetString("chkShowLegendMenus.ToolTip"));
            this.chkShowLegendMenus.UseVisualStyleBackColor = true;
            // 
            // cmbLanguage
            // 
            resources.ApplyResources(this.cmbLanguage, "cmbLanguage");
            this.cmbLanguage.FormattingEnabled = true;
            this.cmbLanguage.Items.AddRange(new object[] {
            resources.GetString("cmbLanguage.Items"),
            resources.GetString("cmbLanguage.Items1")});
            this.cmbLanguage.Name = "cmbLanguage";
            this.toolTip1.SetToolTip(this.cmbLanguage, resources.GetString("cmbLanguage.ToolTip"));
            // 
            // lblLanguage
            // 
            resources.ApplyResources(this.lblLanguage, "lblLanguage");
            this.lblLanguage.Name = "lblLanguage";
            this.toolTip1.SetToolTip(this.lblLanguage, resources.GetString("lblLanguage.ToolTip"));
            // 
            // OptionsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblLanguage);
            this.Controls.Add(this.cmbLanguage);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.chkShowLegendMenus);
            this.Controls.Add(this.chkEditLegendBoxes);
            this.Controls.Add(this.chkZoomOutFartherThanMaxExtent);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckBox chkZoomOutFartherThanMaxExtent;
        private Button btOk;
        private Button btCancel;
        private ToolTip toolTip1;
        private CheckBox chkEditLegendBoxes;
        private CheckBox chkShowLegendMenus;
        private ComboBox cmbLanguage;
        private Label lblLanguage;
    }
}