using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    internal partial class PatternCollectionControl
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(PatternCollectionControl));
            this.lbxItems = new ListBox();
            this.panel1 = new Panel();
            this.btnDown = new Button();
            this.btnUp = new Button();
            this.btnRemove = new Button();
            this.btnAdd = new Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // lbxItems
            //
            this.lbxItems.AccessibleDescription = null;
            this.lbxItems.AccessibleName = null;
            resources.ApplyResources(this.lbxItems, "lbxItems");
            this.lbxItems.BackgroundImage = null;
            this.lbxItems.DrawMode = DrawMode.OwnerDrawFixed;
            this.lbxItems.Font = null;
            this.lbxItems.FormattingEnabled = true;
            this.lbxItems.Name = "lbxItems";
            this.lbxItems.SelectedIndexChanged += new EventHandler(this.LbxItemsSelectedIndexChanged);
            //
            // panel1
            //
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.btnDown);
            this.panel1.Controls.Add(this.btnUp);
            this.panel1.Controls.Add(this.btnRemove);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            //
            // btnDown
            //
            this.btnDown.AccessibleDescription = null;
            this.btnDown.AccessibleName = null;
            resources.ApplyResources(this.btnDown, "btnDown");
            this.btnDown.BackgroundImage = null;
            this.btnDown.Font = null;
            this.btnDown.Image = SymbologyFormsImages.down as Image;
            this.btnDown.Name = "btnDown";
            this.btnDown.UseVisualStyleBackColor = true;
            this.btnDown.Click += new EventHandler(this.BtnDownClick);
            //
            // btnUp
            //
            this.btnUp.AccessibleDescription = null;
            this.btnUp.AccessibleName = null;
            resources.ApplyResources(this.btnUp, "btnUp");
            this.btnUp.BackgroundImage = null;
            this.btnUp.Font = null;
            this.btnUp.Image = SymbologyFormsImages.up as Image;
            this.btnUp.Name = "btnUp";
            this.btnUp.UseVisualStyleBackColor = true;
            this.btnUp.Click += new EventHandler(this.BtnUpClick);
            //
            // btnRemove
            //
            this.btnRemove.AccessibleDescription = null;
            this.btnRemove.AccessibleName = null;
            resources.ApplyResources(this.btnRemove, "btnRemove");
            this.btnRemove.BackgroundImage = null;
            this.btnRemove.Font = null;
            this.btnRemove.Image = SymbologyFormsImages.mnuLayerClear as Image;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new EventHandler(this.BtnRemoveClick);
            //
            // btnAdd
            //
            this.btnAdd.AccessibleDescription = null;
            this.btnAdd.AccessibleName = null;
            resources.ApplyResources(this.btnAdd, "btnAdd");
            this.btnAdd.BackgroundImage = null;
            this.btnAdd.Font = null;
            this.btnAdd.Image = SymbologyFormsImages.mnuLayerAdd as Image;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new EventHandler(this.BtnAddClick);
            //
            // PatternCollectionControl
            //
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");

            this.BackgroundImage = null;
            this.Controls.Add(this.lbxItems);
            this.Controls.Add(this.panel1);
            this.Font = null;
            this.Name = "PatternCollectionControl";
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