
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    public partial class SymbolizerCollectionControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SymbolizerCollectionControl));
            this.lbxItems = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // lbxItems
            //
            resources.ApplyResources(this.lbxItems, "lbxItems");
            this.lbxItems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lbxItems.FormattingEnabled = true;
            this.lbxItems.Name = "lbxItems";
            this.lbxItems.SelectedIndexChanged += new System.EventHandler(this.LbxItemsSelectedIndexChanged);
            //
            // panel1
            //
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.btnDown);
            this.panel1.Controls.Add(this.btnUp);
            this.panel1.Controls.Add(this.btnRemove);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Name = "panel1";
            //
            // btnDown
            //
            resources.ApplyResources(this.btnDown, "btnDown");
            this.btnDown.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.down;
            this.btnDown.Name = "btnDown";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new System.EventHandler(this.BtnDownClick);
            //
            // btnUp
            //
            resources.ApplyResources(this.btnUp, "btnUp");
            this.btnUp.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.up;
            this.btnUp.Name = "btnUp";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new System.EventHandler(this.BtnUpClick);
            //
            // btnRemove
            //
            resources.ApplyResources(this.btnRemove, "btnRemove");
            this.btnRemove.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.mnuLayerClear;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.BtnRemoveClick);
            //
            // btnAdd
            //
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.mnuLayerAdd;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.BtnAddClick);
            //
            // SymbolizerCollectionControl
            //
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lbxItems);
            this.Controls.Add(this.panel1);
            this.Name = "SymbolizerCollectionControl";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private Button btnAdd;
        private Button btnDown;
        private Button btnRemove;
        private Button btnUp;
        private ListBox lbxItems;
        private Panel panel1;
    }
}