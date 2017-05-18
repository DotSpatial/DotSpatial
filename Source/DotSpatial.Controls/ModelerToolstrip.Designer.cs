using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    public partial class ModelerToolStrip
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
            this.SuspendLayout();

            // New model button
            _btnNewModel = new ToolStripButton();
            _btnNewModel.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnNewModel.Image = Images.file_new;
            _btnNewModel.ImageTransparentColor = Color.Magenta;
            _btnNewModel.Name = "btnNewModel";
            _btnNewModel.Size = new Size(23, 22);
            _btnNewModel.Text = MessageStrings.ModelTipNew;

            // _btnNewModel.Click

            // save model button
            _btnSaveModel = new ToolStripButton();
            _btnSaveModel.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnSaveModel.Image = Images.file_saveas;
            _btnSaveModel.ImageTransparentColor = Color.Magenta;
            _btnSaveModel.Name = "btnSaveModel";
            _btnSaveModel.Size = new Size(23, 22);
            _btnSaveModel.Text = MessageStrings.ModelTipSave;
            _btnSaveModel.Click += BtnSaveModelClick;

            // Load model button
            _btnLoadModel = new ToolStripButton();
            _btnLoadModel.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnLoadModel.Image = Images.FolderOpen;
            _btnLoadModel.ImageTransparentColor = Color.Magenta;
            _btnLoadModel.Name = "btnLoadModel";
            _btnLoadModel.Size = new Size(23, 22);
            _btnLoadModel.Text = MessageStrings.ModelTipLoad;
            _btnLoadModel.Click += BtnLoadModelClick;

            // Zoom In button
            _btnZoomIn = new ToolStripButton();
            _btnZoomIn.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnZoomIn.Image = Images.zoom_in.ToBitmap();
            _btnZoomIn.ImageTransparentColor = Color.Magenta;
            _btnZoomIn.Name = "btnZoomIn";
            _btnZoomIn.Size = new Size(23, 22);
            _btnZoomIn.Text = MessageStrings.ModelTipZoonIn;
            _btnZoomIn.Click += BtnZoomInClick;

            // Zoom out button
            _btnZoomOut = new ToolStripButton();
            _btnZoomOut.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnZoomOut.Image = Images.zoom_out.ToBitmap();
            _btnZoomOut.ImageTransparentColor = Color.Magenta;
            _btnZoomOut.Name = "btnZoomOut";
            _btnZoomOut.Size = new Size(23, 22);
            _btnZoomOut.Text = MessageStrings.ModelTipZoomOut;
            _btnZoomOut.Click += BtnZoomOutClick;

            // Zoom full extent
            _btnZoomFullExtent = new ToolStripButton();
            _btnZoomFullExtent.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnZoomFullExtent.Image = Images.zoom_full_extent.ToBitmap();
            _btnZoomFullExtent.ImageTransparentColor = Color.Magenta;
            _btnZoomFullExtent.Name = "btnZoomOut";
            _btnZoomFullExtent.Size = new Size(23, 22);
            _btnZoomFullExtent.Text = MessageStrings.ModelTipFullExtent;
            _btnZoomFullExtent.Click += BtnZoomFullExtentClick;

            // Add data button
            _btnAddData = new ToolStripButton();
            _btnAddData.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnAddData.Image = Images.AddLayer;
            _btnAddData.ImageTransparentColor = Color.Magenta;
            _btnAddData.Name = "btnLink";
            _btnAddData.Size = new Size(23, 22);
            _btnAddData.Text = MessageStrings.ModelTipAddData;

            // Zoom link tools
            _btnLink = new ToolStripButton();
            _btnLink.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnLink.Image = Images.LinkData;
            _btnLink.ImageTransparentColor = Color.Magenta;
            _btnLink.Name = "btnLink";
            _btnLink.Size = new Size(23, 22);
            _btnLink.Text = MessageStrings.ModelTipLink;
            _btnLink.Click += BtnLinkClick;

            // delete stuff
            _btnDelete = new ToolStripButton();
            _btnDelete.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnDelete.Image = Images.mnuLayerClear;
            _btnDelete.ImageTransparentColor = Color.Magenta;
            _btnDelete.Name = "btnLink";
            _btnDelete.Size = new Size(23, 22);
            _btnDelete.Text = MessageStrings.ModelTipDelete;
            _btnDelete.Click += BtnDeleteClick;

            // delete stuff
            _btnRun = new ToolStripButton();
            _btnRun.DisplayStyle = ToolStripItemDisplayStyle.Image;
            _btnRun.Image = Images.RunModel;
            _btnRun.ImageTransparentColor = Color.Magenta;
            _btnRun.Name = "btnLink";
            _btnRun.Size = new Size(23, 22);
            _btnRun.Text = MessageStrings.ModelTipRunModel;
            _btnRun.Click += BtnRunClick;

            // Adds all the buttons to the toolstrip
            Items.Add(_btnNewModel);
            Items.Add(_btnLoadModel);
            Items.Add(_btnSaveModel);
            Items.Add(new ToolStripSeparator());
            Items.Add(_btnZoomIn);
            Items.Add(_btnZoomOut);
            Items.Add(_btnZoomFullExtent);
            Items.Add(new ToolStripSeparator());
            Items.Add(_btnAddData);
            Items.Add(_btnLink);
            Items.Add(_btnDelete);
            Items.Add(new ToolStripSeparator());
            Items.Add(_btnRun);

            ResumeLayout();
        }

        #endregion

        private ToolStripButton _btnAddData;
        private ToolStripButton _btnDelete;
        private ToolStripButton _btnLink;
        private ToolStripButton _btnLoadModel;
        private ToolStripButton _btnNewModel;
        private ToolStripButton _btnRun;
        private ToolStripButton _btnSaveModel;
        private ToolStripButton _btnZoomFullExtent;

        private ToolStripButton _btnZoomIn;
        private ToolStripButton _btnZoomOut;
    }
}