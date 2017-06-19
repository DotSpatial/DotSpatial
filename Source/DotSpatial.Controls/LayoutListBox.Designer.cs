using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    public partial class LayoutListBox
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
            ComponentResourceManager resources = new ComponentResourceManager(typeof(LayoutListBox));
            this._lbxItems = new ListBox();
            this._btnPanel = new Panel();
            this._btnDown = new Button();
            this._btnUp = new Button();
            this._btnRemove = new Button();
            this._listPanel = new Panel();
            this._btnPanel.SuspendLayout();
            this._listPanel.SuspendLayout();
            this.SuspendLayout();

            // _lbxItems
            resources.ApplyResources(this._lbxItems, "_lbxItems");
            this._lbxItems.DrawMode = DrawMode.OwnerDrawFixed;
            this._lbxItems.FormattingEnabled = true;
            this._lbxItems.Name = "_lbxItems";
            this._lbxItems.SelectionMode = SelectionMode.MultiExtended;
            this._lbxItems.SelectedIndexChanged += LbxItemsSelectedIndexChanged;

            // _btnPanel
            resources.ApplyResources(this._btnPanel, "_btnPanel");
            this._btnPanel.Controls.Add(this._btnDown);
            this._btnPanel.Controls.Add(this._btnUp);
            this._btnPanel.Controls.Add(this._btnRemove);
            this._btnPanel.Name = "_btnPanel";

            // _btnDown
            resources.ApplyResources(this._btnDown, "_btnDown");
            this._btnDown.Image = Images.down;
            this._btnDown.Name = "_btnDown";
            this._btnDown.UseVisualStyleBackColor = true;
            this._btnDown.Click += BtnDownClick;

            // _btnUp
            resources.ApplyResources(this._btnUp, "_btnUp");
            this._btnUp.Image = Images.up;
            this._btnUp.Name = "_btnUp";
            this._btnUp.UseVisualStyleBackColor = true;
            this._btnUp.Click += BtnUpClick;

            // _btnRemove
            resources.ApplyResources(this._btnRemove, "_btnRemove");
            this._btnRemove.Image = Images.mnuLayerClear;
            this._btnRemove.Name = "_btnRemove";
            this._btnRemove.UseVisualStyleBackColor = true;
            this._btnRemove.Click += BtnRemoveClick;

            // _listPanel
            resources.ApplyResources(this._listPanel, "_listPanel");
            this._listPanel.BackColor = Color.White;
            this._listPanel.Controls.Add(this._lbxItems);
            this._listPanel.Name = "_listPanel";

            // LayoutListBox
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this._listPanel);
            this.Controls.Add(this._btnPanel);
            this.Name = "LayoutListBox";
            this._btnPanel.ResumeLayout(false);
            this._listPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private Button _btnDown;
        private Panel _btnPanel;
        private Button _btnRemove;
        private Button _btnUp;
        private LayoutControl _layoutControl;
        private ListBox _lbxItems;
        private Panel _listPanel;
    }
}