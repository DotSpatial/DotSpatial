using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class CollectionPropertyGrid
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CollectionPropertyGrid));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ccItems = new DotSpatial.Symbology.Forms.CollectionControl();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dialogButtons1 = new DotSpatial.Symbology.Forms.DialogButtons();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // splitContainer1
            //
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            //
            // splitContainer1.Panel1
            //
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.Controls.Add(this.ccItems);
            //
            // splitContainer1.Panel2
            //
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
            //
            // ccItems
            //
            resources.ApplyResources(this.ccItems, "ccItems");
            this.ccItems.Name = "ccItems";
            this.ccItems.SelectedName = null;
            this.ccItems.SelectedObject = null;
            //
            // propertyGrid1
            //
            resources.ApplyResources(this.propertyGrid1, "propertyGrid1");
            this.propertyGrid1.Name = "propertyGrid1";
            //
            // panel1
            //
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.dialogButtons1);
            this.panel1.Name = "panel1";
            //
            // dialogButtons1
            //
            resources.ApplyResources(this.dialogButtons1, "dialogButtons1");
            this.dialogButtons1.Name = "dialogButtons1";
            //
            // CollectionPropertyGrid
            //
            resources.ApplyResources(this, "$this");
            this.AllowDrop = true;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "CollectionPropertyGrid";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private CollectionControl ccItems;
        private DialogButtons dialogButtons1;
        private Panel panel1;
        private PropertyGrid propertyGrid1;
        private SplitContainer splitContainer1;

    }
}