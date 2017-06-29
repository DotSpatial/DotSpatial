using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    public partial class SpatialToolStrip
    {
        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.cmdLabel = new System.Windows.Forms.ToolStripButton();
            this.cmdNew = new System.Windows.Forms.ToolStripButton();
            this.cmdOpen = new System.Windows.Forms.ToolStripButton();
            this.cmdSave = new System.Windows.Forms.ToolStripButton();
            this.cmdPrint = new System.Windows.Forms.ToolStripButton();
            this.cmdAddData = new System.Windows.Forms.ToolStripButton();
            this.cmdPan = new System.Windows.Forms.ToolStripButton();
            this.cmdSelect = new System.Windows.Forms.ToolStripButton();
            this.cmdZoom = new System.Windows.Forms.ToolStripButton();
            this.cmdZoomOut = new System.Windows.Forms.ToolStripButton();
            this.cmdZoomPrevious = new System.Windows.Forms.ToolStripButton();
            this.cmdZoomNext = new System.Windows.Forms.ToolStripButton();
            this.cmdInfo = new System.Windows.Forms.ToolStripButton();
            this.cmdTable = new System.Windows.Forms.ToolStripButton();
            this.cmdMaxExtents = new System.Windows.Forms.ToolStripButton();
            this.cmdZoomToCoordinates = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.SuspendLayout();
            // 
            // cmdLabel
            // 
            this.cmdLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdLabel.Image = global::DotSpatial.Controls.Images.Label;
            this.cmdLabel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdLabel.Name = "cmdLabel";
            this.cmdLabel.Size = new System.Drawing.Size(23, 20);
            this.cmdLabel.ToolTipText = "Select Label";
            this.cmdLabel.Click += new System.EventHandler(this.cmdLabel_Click);
            // 
            // cmdNew
            // 
            this.cmdNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdNew.Image = global::DotSpatial.Controls.Images.file_new;
            this.cmdNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdNew.Name = "cmdNew";
            this.cmdNew.Size = new System.Drawing.Size(23, 69);
            this.cmdNew.ToolTipText = "New";
            this.cmdNew.Click += new System.EventHandler(this.cmdNew_Click);
            // 
            // cmdOpen
            // 
            this.cmdOpen.Image = global::DotSpatial.Controls.Images.FolderOpen;
            this.cmdOpen.Name = "cmdOpen";
            this.cmdOpen.Size = new System.Drawing.Size(23, 69);
            this.cmdOpen.ToolTipText = "Open Project";
            this.cmdOpen.Click += new System.EventHandler(this.cmdOpen_Click);
            // 
            // cmdSave
            // 
            this.cmdSave.Image = global::DotSpatial.Controls.Images.file_saveas;
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(23, 69);
            this.cmdSave.ToolTipText = "Save Project";
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdPrint
            // 
            this.cmdPrint.Image = global::DotSpatial.Controls.Images.printer;
            this.cmdPrint.Name = "cmdPrint";
            this.cmdPrint.Size = new System.Drawing.Size(23, 20);
            this.cmdPrint.ToolTipText = "Print";
            this.cmdPrint.Click += new System.EventHandler(this.cmdPrint_Click);
            // 
            // cmdAddData
            // 
            this.cmdAddData.Image = global::DotSpatial.Controls.Images.AddLayer;
            this.cmdAddData.Name = "cmdAddData";
            this.cmdAddData.Size = new System.Drawing.Size(23, 20);
            this.cmdAddData.ToolTipText = "Add Data";
            this.cmdAddData.Click += new System.EventHandler(this.cmdAddData_Click);
            // 
            // cmdPan
            // 
            this.cmdPan.Image = global::DotSpatial.Controls.Images.PanSimple;
            this.cmdPan.Name = "cmdPan";
            this.cmdPan.Size = new System.Drawing.Size(23, 20);
            this.cmdPan.ToolTipText = "Pan";
            this.cmdPan.Click += new System.EventHandler(this.cmdPan_Click);
            // 
            // cmdSelect
            // 
            this.cmdSelect.Image = global::DotSpatial.Controls.Images.SelectSimple;
            this.cmdSelect.Name = "cmdSelect";
            this.cmdSelect.Size = new System.Drawing.Size(23, 20);
            this.cmdSelect.ToolTipText = "Select";
            this.cmdSelect.Click += new System.EventHandler(this.cmdSelect_Click);
            // 
            // cmdZoom
            // 
            this.cmdZoom.Image = global::DotSpatial.Controls.Images.ZoomInMap;
            this.cmdZoom.Name = "cmdZoom";
            this.cmdZoom.Size = new System.Drawing.Size(23, 20);
            this.cmdZoom.ToolTipText = "Zoom In";
            this.cmdZoom.Click += new System.EventHandler(this.cmdZoom_Click);
            // 
            // cmdZoomOut
            // 
            this.cmdZoomOut.Image = global::DotSpatial.Controls.Images.ZoomOutMap;
            this.cmdZoomOut.Name = "cmdZoomOut";
            this.cmdZoomOut.Size = new System.Drawing.Size(23, 20);
            this.cmdZoomOut.ToolTipText = "Zoom Out";
            this.cmdZoomOut.Click += new System.EventHandler(this.cmdZoomOut_Click);
            // 
            // cmdZoomPrevious
            // 
            this.cmdZoomPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdZoomPrevious.Image = global::DotSpatial.Controls.Images.ZoomPrevious;
            this.cmdZoomPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdZoomPrevious.Name = "cmdZoomPrevious";
            this.cmdZoomPrevious.Size = new System.Drawing.Size(23, 20);
            this.cmdZoomPrevious.ToolTipText = "Zoom to Previous Extents";
            this.cmdZoomPrevious.Click += new System.EventHandler(this.cmdZoomPrevious_Click);
            // 
            // cmdZoomNext
            // 
            this.cmdZoomNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdZoomNext.Image = global::DotSpatial.Controls.Images.ZoomNext;
            this.cmdZoomNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdZoomNext.Name = "cmdZoomNext";
            this.cmdZoomNext.Size = new System.Drawing.Size(23, 20);
            this.cmdZoomNext.ToolTipText = "Zoom to Next Extent";
            this.cmdZoomNext.Click += new System.EventHandler(this.cmdZoomNext_Click);
            // 
            // cmdInfo
            // 
            this.cmdInfo.BackColor = System.Drawing.Color.Transparent;
            this.cmdInfo.Image = global::DotSpatial.Controls.Images.info_rhombus_16x16;
            this.cmdInfo.Name = "cmdInfo";
            this.cmdInfo.Size = new System.Drawing.Size(23, 20);
            this.cmdInfo.ToolTipText = "Identifier";
            this.cmdInfo.Click += new System.EventHandler(this.cmdInfo_Click);
            // 
            // cmdTable
            // 
            this.cmdTable.Image = global::DotSpatial.Controls.Images.table_16x16;
            this.cmdTable.Name = "cmdTable";
            this.cmdTable.Size = new System.Drawing.Size(23, 20);
            this.cmdTable.ToolTipText = "Attribute Table";
            this.cmdTable.Click += new System.EventHandler(this.cmdTable_Click);
            // 
            // cmdMaxExtents
            // 
            this.cmdMaxExtents.Image = global::DotSpatial.Controls.Images.MaxExtents;
            this.cmdMaxExtents.Name = "cmdMaxExtents";
            this.cmdMaxExtents.Size = new System.Drawing.Size(23, 20);
            this.cmdMaxExtents.ToolTipText = "Zoom to Maximum Extents";
            this.cmdMaxExtents.Click += new System.EventHandler(this.cmdMaxExtents_Click);
            // 
            // cmdZoomToCoordinates
            // 
            this.cmdZoomToCoordinates.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdZoomToCoordinates.Image = global::DotSpatial.Controls.Images.zoom_coordinate_16x16;
            this.cmdZoomToCoordinates.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmdZoomToCoordinates.Name = "cmdZoomToCoordinates";
            this.cmdZoomToCoordinates.Size = new System.Drawing.Size(23, 20);
            this.cmdZoomToCoordinates.ToolTipText = "Zoom To Coordinates";
            this.cmdZoomToCoordinates.Click += new System.EventHandler(this.cmdZoomToCoordinates_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 6);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 6);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 6);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 6);
            // 
            // SpatialToolStrip
            // 
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdNew,
            this.cmdOpen,
            this.cmdSave,
            this.cmdPrint,
            this.toolStripSeparator1,
            this.cmdAddData,
            this.toolStripSeparator2,
            this.cmdPan,
            this.cmdSelect,
            this.cmdZoom,
            this.cmdZoomOut,
            this.cmdZoomPrevious,
            this.cmdZoomNext,
            this.cmdMaxExtents,
            this.cmdZoomToCoordinates,
            this.toolStripSeparator3,
            this.cmdInfo,
            this.cmdTable,
            this.cmdLabel,
            this.toolStripSeparator4});
            this.Size = new System.Drawing.Size(100, 72);
            this.ResumeLayout(false);

        }

        #endregion

        private ToolStripButton cmdAddData;
        private ToolStripButton cmdInfo;
        private ToolStripButton cmdLabel;
        private ToolStripButton cmdMaxExtents;
        private ToolStripButton cmdNew;
        private ToolStripButton cmdOpen;
        private ToolStripButton cmdPan;
        private ToolStripButton cmdPrint;
        private ToolStripButton cmdSave;
        private ToolStripButton cmdSelect;
        private ToolStripButton cmdTable;
        private ToolStripButton cmdZoom;
        private ToolStripButton cmdZoomNext;
        private ToolStripButton cmdZoomOut;
        private ToolStripButton cmdZoomPrevious;
        private ToolStripButton cmdZoomToCoordinates;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
    }
}
