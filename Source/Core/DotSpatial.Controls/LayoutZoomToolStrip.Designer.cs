using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    public partial class LayoutZoomToolStrip
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
            this._btnZoomIn = new ToolStripButton();
            this._btnZoomOut = new ToolStripButton();
            this._btnZoomFullExtent = new ToolStripButton();
            this._comboZoom = new ToolStripComboBox();
            this.SuspendLayout();

            // _btnZoomIn
            this._btnZoomIn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnZoomIn.Image = Images.layout_zoom_in.ToBitmap();
            this._btnZoomIn.ImageTransparentColor = Color.Magenta;
            this._btnZoomIn.Name = "_btnZoomIn";
            this._btnZoomIn.Size = new Size(23, 22);
            this._btnZoomIn.Text = MessageStrings.LayoutToolStripZoomIn;
            this._btnZoomIn.Click += this.BtnZoomInClick;

            // _btnZoomOut
            this._btnZoomOut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnZoomOut.Image = Images.layout_zoom_out.ToBitmap();
            this._btnZoomOut.ImageTransparentColor = Color.Magenta;
            this._btnZoomOut.Name = "_btnZoomOut";
            this._btnZoomOut.Size = new Size(23, 22);
            this._btnZoomOut.Text = MessageStrings.LayoutToolStripZoomOut;
            this._btnZoomOut.Click += this.BtnZoomOutClick;

            // _btnZoomFullExtent
            this._btnZoomFullExtent.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnZoomFullExtent.Image = Images.layout_zoom_full_extent.ToBitmap();
            this._btnZoomFullExtent.ImageTransparentColor = Color.Magenta;
            this._btnZoomFullExtent.Name = "_btnZoomFullExtent";
            this._btnZoomFullExtent.Size = new Size(23, 22);
            this._btnZoomFullExtent.Text = MessageStrings.LayoutToolStripZoomFull;
            this._btnZoomFullExtent.Click += this.BtnZoomFullExtentClick;

            // _comboZoom
            this._comboZoom.Items.AddRange(new object[] { "50%", "75%", "100%", "150%", "200%", "300%" });
            this._comboZoom.Name = "_comboZoom";
            this._comboZoom.Size = new Size(75, 21);
            this._comboZoom.SelectedIndexChanged += this.ComboZoomSelectedIndexChanged;
            this._comboZoom.KeyPress += this.ComboZoomKeyPress;

            // LayoutToolStrip
            this.Items.AddRange(new ToolStripItem[] { this._btnZoomIn, this._btnZoomOut, this._btnZoomFullExtent, this._comboZoom });
            this.ResumeLayout(false);
        }

        #endregion

        private ToolStripButton _btnZoomFullExtent;
        private ToolStripButton _btnZoomIn;
        private ToolStripButton _btnZoomOut;
        private ToolStripComboBox _comboZoom;
        private LayoutControl _layoutControl;
    }
}