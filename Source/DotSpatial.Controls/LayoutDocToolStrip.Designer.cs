using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    public partial class LayoutDocToolStrip
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
            this._btnNew = new ToolStripButton();
            this._btnOpen = new ToolStripButton();
            this._btnSave = new ToolStripButton();
            this._btnSaveAs = new ToolStripButton();
            this._btnPrint = new ToolStripButton();
            this.SuspendLayout();

            // _btnZoomIn
            this._btnNew.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnNew.Image = Images.file_new;
            this._btnNew.Size = new Size(23, 22);
            this._btnNew.Text = MessageStrings.LayoutMenuStripNew;
            this._btnNew.Click += this.BtnNewClick;

            // _btnZoomOut
            this._btnOpen.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnOpen.Image = Images.FolderOpen;
            this._btnOpen.Size = new Size(23, 22);
            this._btnOpen.Text = MessageStrings.LayoutMenuStripOpen;
            this._btnOpen.Click += this.BtnOpenClick;

            // _btnZoomFullExtent
            this._btnSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnSave.Image = Images.save.ToBitmap();
            this._btnSave.Size = new Size(23, 22);
            this._btnSave.Text = MessageStrings.LayoutMenuStripSave;
            this._btnSave.Click += this.BtnSaveClick;

            // _comboZoom
            this._btnSaveAs.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnSaveAs.Image = Images.file_saveas;
            this._btnSaveAs.Size = new Size(23, 22);
            this._btnSaveAs.Text = MessageStrings.LayoutMenuStripSaveAs;
            this._btnSaveAs.Click += BtnSaveAsClick;

            this._btnPrint.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnPrint.Image = Images.printer;
            this._btnPrint.Size = new Size(23, 22);
            this._btnPrint.Text = MessageStrings.LayoutMenuStripPrint;
            this._btnPrint.Click += BtnPrintClick;

            // LayoutToolStrip
            this.Items.AddRange(new ToolStripItem[] { this._btnNew, this._btnOpen, this._btnSave, this._btnSaveAs, new ToolStripSeparator(), this._btnPrint });
            this.ResumeLayout(false);
        }

        #endregion

        private ToolStripButton _btnNew;
        private ToolStripButton _btnOpen;
        private ToolStripButton _btnPrint;
        private ToolStripButton _btnSave;
        private ToolStripButton _btnSaveAs;
    }
}