using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class LayerDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.ComponentModel.ComponentResourceManager resources;

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
            resources = new System.ComponentModel.ComponentResourceManager(typeof(LayerDialog));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSymbology = new System.Windows.Forms.TabPage();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.tabDetails = new System.Windows.Forms.TabPage();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dialogButtons1 = new DotSpatial.Symbology.Forms.DialogButtons();
            this.tabControl1.SuspendLayout();
            this.tabSymbology.SuspendLayout();
            this.tabDetails.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabSymbology);
            this.tabControl1.Controls.Add(this.tabDetails);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabSymbology
            // 
            this.tabSymbology.BackColor = System.Drawing.SystemColors.Control;
            this.tabSymbology.Controls.Add(this.pnlContent);
            resources.ApplyResources(this.tabSymbology, "tabSymbology");
            this.tabSymbology.Name = "tabSymbology";
            // 
            // pnlContent
            // 
            resources.ApplyResources(this.pnlContent, "pnlContent");
            this.pnlContent.BackColor = System.Drawing.SystemColors.Control;
            this.pnlContent.Name = "pnlContent";
            // 
            // tabDetails
            // 
            this.tabDetails.BackColor = System.Drawing.SystemColors.Control;
            this.tabDetails.Controls.Add(this.propertyGrid1);
            resources.ApplyResources(this.tabDetails, "tabDetails");
            this.tabDetails.Name = "tabDetails";
            // 
            // propertyGrid1
            // 
            resources.ApplyResources(this.propertyGrid1, "propertyGrid1");
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ControlDark;
            this.propertyGrid1.Name = "propertyGrid1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dialogButtons1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // dialogButtons1
            // 
            this.dialogButtons1.ButtonsCulture = new System.Globalization.CultureInfo("");
            resources.ApplyResources(this.dialogButtons1, "dialogButtons1");
            this.dialogButtons1.Name = "dialogButtons1";
            this.dialogButtons1.ApplyClicked += new System.EventHandler(this.DialogButtons1ApplyClicked);
            this.dialogButtons1.CancelClicked += new System.EventHandler(this.DialogButtons1CancelClicked);
            this.dialogButtons1.OkClicked += new System.EventHandler(this.DialogButtons1OkClicked);
            // 
            // LayerDialog
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LayerDialog";
            this.ShowInTaskbar = false;
            this.tabControl1.ResumeLayout(false);
            this.tabSymbology.ResumeLayout(false);
            this.tabDetails.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DialogButtons dialogButtons1;
        private Panel panel1;
        private Panel pnlContent;
        private PropertyGrid propertyGrid1;
        private TabControl tabControl1;
        private TabPage tabDetails;
        private TabPage tabSymbology;
    }
}