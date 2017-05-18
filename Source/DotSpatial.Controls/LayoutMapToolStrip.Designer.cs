using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    public partial class LayoutMapToolStrip
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
            this._btnZoomViewExtent = new ToolStripButton();
            this._btnPan = new ToolStripButton();
            this.SuspendLayout();

            // _btnZoomIn
            this._btnZoomIn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnZoomIn.Image = Images.ZoomInMap;
            this._btnZoomIn.ImageTransparentColor = Color.Magenta;
            this._btnZoomIn.Name = "_btnZoomIn";
            this._btnZoomIn.Size = new Size(23, 22);
            this._btnZoomIn.Text = MessageStrings.LayoutMapToolStripZoomIn;
            this._btnZoomIn.Click += this.BtnZoomInClick;

            // _btnZoomOut
            this._btnZoomOut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnZoomOut.Image = Images.ZoomOutMap;
            this._btnZoomOut.ImageTransparentColor = Color.Magenta;
            this._btnZoomOut.Name = "_btnZoomOut";
            this._btnZoomOut.Size = new Size(23, 22);
            this._btnZoomOut.Text = MessageStrings.LayoutMapToolStripZoomOut;
            this._btnZoomOut.Click += this.BtnZoomOutClick;

            // _btnZoomFullExtent
            this._btnZoomFullExtent.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnZoomFullExtent.Image = Images.ZoomFullMap;
            this._btnZoomFullExtent.ImageTransparentColor = Color.Magenta;
            this._btnZoomFullExtent.Name = "_btnZoomFullExtent";
            this._btnZoomFullExtent.Size = new Size(23, 22);
            this._btnZoomFullExtent.Text = MessageStrings.LayoutMapToolStripMaxExtent;
            this._btnZoomFullExtent.Click += this.BtnZoomFullExtentClick;

            // _btnZoomFullExtent
            this._btnZoomViewExtent.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnZoomViewExtent.Image = Images.ZoomFullView;
            this._btnZoomViewExtent.ImageTransparentColor = Color.Magenta;
            this._btnZoomViewExtent.Name = "_btnZoomViewExtent";
            this._btnZoomViewExtent.Size = new Size(23, 22);
            this._btnZoomViewExtent.Text = MessageStrings.LayoutMapToolStripViewExtent;
            this._btnZoomViewExtent.Click += BtnZoomViewExtentClick;

            // _btnPan
            this._btnPan.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnPan.Image = Images.PanMap;
            this._btnPan.CheckOnClick = true;
            this._btnPan.Checked = false;
            this._btnPan.ImageTransparentColor = Color.Magenta;
            this._btnPan.Name = "_btnZoomFullExtent";
            this._btnPan.Size = new Size(23, 22);
            this._btnPan.Text = MessageStrings.LayoutMapToolStripPan;
            this._btnPan.Click += BtnPanClick;

            // LayoutToolStrip
            this.Items.AddRange(new ToolStripItem[] { this._btnZoomIn, this._btnZoomOut, this._btnZoomFullExtent, this._btnZoomViewExtent, this._btnPan });
            this.EnabledChanged += LayoutMapToolStripEnabledChanged;
            this.Enabled = false;
            this.ResumeLayout(false);
        }

        #endregion

        private ToolStripButton _btnPan;
        private ToolStripButton _btnZoomFullExtent;
        private ToolStripButton _btnZoomIn;
        private ToolStripButton _btnZoomOut;
        private ToolStripButton _btnZoomViewExtent;
    }
}