using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    internal partial class StrokeCollectionControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StrokeCollectionControl));
            this._lbxItems = new System.Windows.Forms.ListBox();
            this._panel1 = new System.Windows.Forms.Panel();
            this._btnDown = new System.Windows.Forms.Button();
            this._btnUp = new System.Windows.Forms.Button();
            this._btnRemove = new System.Windows.Forms.Button();
            this._btnAdd = new System.Windows.Forms.Button();
            this._panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // _lbxItems
            //
            resources.ApplyResources(this._lbxItems, "_lbxItems");
            this._lbxItems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._lbxItems.FormattingEnabled = true;
            this._lbxItems.Name = "_lbxItems";
            this._lbxItems.SelectedIndexChanged += new System.EventHandler(this.LbxItemsSelectedIndexChanged);
            //
            // _panel1
            //
            this._panel1.Controls.Add(this._btnDown);
            this._panel1.Controls.Add(this._btnUp);
            this._panel1.Controls.Add(this._btnRemove);
            this._panel1.Controls.Add(this._btnAdd);
            resources.ApplyResources(this._panel1, "_panel1");
            this._panel1.Name = "_panel1";
            //
            // _btnDown
            //
            this._btnDown.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.down;
            resources.ApplyResources(this._btnDown, "_btnDown");
            this._btnDown.Name = "_btnDown";
            this._btnDown.UseVisualStyleBackColor = true;
            this._btnDown.Click += new System.EventHandler(this.BtnDownClick);
            //
            // _btnUp
            //
            this._btnUp.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.up;
            resources.ApplyResources(this._btnUp, "_btnUp");
            this._btnUp.Name = "_btnUp";
            this._btnUp.UseVisualStyleBackColor = true;
            this._btnUp.Click += new System.EventHandler(this.BtnUpClick);
            //
            // _btnRemove
            //
            this._btnRemove.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.mnuLayerClear;
            resources.ApplyResources(this._btnRemove, "_btnRemove");
            this._btnRemove.Name = "_btnRemove";
            this._btnRemove.UseVisualStyleBackColor = true;
            this._btnRemove.Click += new System.EventHandler(this.BtnRemoveClick);
            //
            // _btnAdd
            //
            this._btnAdd.Image = global::DotSpatial.Symbology.Forms.SymbologyFormsImages.mnuLayerAdd;
            resources.ApplyResources(this._btnAdd, "_btnAdd");
            this._btnAdd.Name = "_btnAdd";
            this._btnAdd.UseVisualStyleBackColor = true;
            this._btnAdd.Click += new System.EventHandler(this.BtnAddClick);
            //
            // StrokeCollectionControl
            //
            this.Controls.Add(this._lbxItems);
            this.Controls.Add(this._panel1);
            this.Name = "StrokeCollectionControl";
            resources.ApplyResources(this, "$this");
            this._panel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private Button _btnAdd;
        private Button _btnDown;
        private Button _btnRemove;
        private Button _btnUp;
        private ListBox _lbxItems;
        private Panel _panel1;
    }
}