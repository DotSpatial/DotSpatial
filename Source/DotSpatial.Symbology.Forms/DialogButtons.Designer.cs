using System.ComponentModel;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class DialogButtons
    {
        private System.ComponentModel.ComponentResourceManager resources;


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
            resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogButtons));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.helpProvider1.SetHelpKeyword(this.btnOK, resources.GetString("btnOK.HelpKeyword"));
            this.helpProvider1.SetHelpNavigator(this.btnOK, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("btnOK.HelpNavigator"))));
            this.helpProvider1.SetHelpString(this.btnOK, resources.GetString("btnOK.HelpString"));
            this.btnOK.Name = "btnOK";
            this.helpProvider1.SetShowHelp(this.btnOK, ((bool)(resources.GetObject("btnOK.ShowHelp"))));
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOkClick);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.helpProvider1.SetHelpKeyword(this.btnCancel, resources.GetString("btnCancel.HelpKeyword"));
            this.helpProvider1.SetHelpNavigator(this.btnCancel, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("btnCancel.HelpNavigator"))));
            this.helpProvider1.SetHelpString(this.btnCancel, resources.GetString("btnCancel.HelpString"));
            this.btnCancel.Name = "btnCancel";
            this.helpProvider1.SetShowHelp(this.btnCancel, ((bool)(resources.GetObject("btnCancel.ShowHelp"))));
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancelClick);
            // 
            // btnApply
            // 
            resources.ApplyResources(this.btnApply, "btnApply");
            this.helpProvider1.SetHelpKeyword(this.btnApply, resources.GetString("btnApply.HelpKeyword"));
            this.helpProvider1.SetHelpNavigator(this.btnApply, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("btnApply.HelpNavigator"))));
            this.helpProvider1.SetHelpString(this.btnApply, resources.GetString("btnApply.HelpString"));
            this.btnApply.Name = "btnApply";
            this.helpProvider1.SetShowHelp(this.btnApply, ((bool)(resources.GetObject("btnApply.ShowHelp"))));
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.BtnApplyClick);
            // 
            // helpProvider1
            // 
            resources.ApplyResources(this.helpProvider1, "helpProvider1");
            // 
            // DialogButtons
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.helpProvider1.SetHelpKeyword(this, resources.GetString("$this.HelpKeyword"));
            this.helpProvider1.SetHelpNavigator(this, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("$this.HelpNavigator"))));
            this.helpProvider1.SetHelpString(this, resources.GetString("$this.HelpString"));
            this.Name = "DialogButtons";
            this.helpProvider1.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnApply;
        private Button btnCancel;
        private Button btnOK;
        private HelpProvider helpProvider1;
    }
}