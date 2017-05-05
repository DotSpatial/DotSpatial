using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class DetailedPointSymbolDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DetailedPointSymbolDialog));
            this.panel1 = new System.Windows.Forms.Panel();
            this.dialogButtons1 = new DotSpatial.Symbology.Forms.DialogButtons();
            this.detailedPointSymbolControl1 = new DotSpatial.Symbology.Forms.DetailedPointSymbolControl();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // panel1
            //
            this.panel1.Controls.Add(this.dialogButtons1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            //
            // dialogButtons1
            //
            resources.ApplyResources(this.dialogButtons1, "dialogButtons1");
            this.dialogButtons1.Name = "dialogButtons1";
            //
            // detailedPointSymbolControl1
            //
            resources.ApplyResources(this.detailedPointSymbolControl1, "detailedPointSymbolControl1");
            this.detailedPointSymbolControl1.Name = "detailedPointSymbolControl1";
            //
            // DetailedPointSymbolDialog
            //
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.detailedPointSymbolControl1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DetailedPointSymbolDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private DetailedPointSymbolControl detailedPointSymbolControl1;
        private DialogButtons dialogButtons1;
        private Panel panel1;

    }
}