using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    partial class LayoutInsertToolStrip
    {
        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this._btnMap = new ToolStripButton();
            this._btnText = new ToolStripButton();
            this._btnRectangle = new ToolStripButton();
            this._btnNorthArrow = new ToolStripButton();
            this._btnBitmap = new ToolStripButton();
            this._btnScaleBar = new ToolStripButton();
            this._btnLegend = new ToolStripButton();
            this.SuspendLayout();
            //
            // _btnMap
            //
            this._btnMap.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnMap.Image = Images.map;
            this._btnMap.Size = new Size(23, 22);
            this._btnMap.Text = MessageStrings.LayoutInsertToolStripMap;
            this._btnMap.Click += this._btnMap_Click;
            //
            // _btnText
            //
            this._btnText.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnText.Image = Images.text;
            this._btnText.Size = new Size(23, 22);
            this._btnText.Text = MessageStrings.LayoutInsertToolStripText;
            this._btnText.Click += this._btnText_Click;
            //
            // _btnRectangle
            //
            this._btnRectangle.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnRectangle.Image = Images.Rectangle;
            this._btnRectangle.Size = new Size(23, 22);
            this._btnRectangle.Text = MessageStrings.LayoutInsertToolStripRectangle;
            this._btnRectangle.Click += this._btnRectangle_Click;
            //
            // _comboNorthArrow
            //
            this._btnNorthArrow.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnNorthArrow.Image = Images.NorthArrow;
            this._btnNorthArrow.Size = new Size(23, 22);
            this._btnNorthArrow.Text = MessageStrings.LayoutInsertToolStripNorthArrow;
            this._btnNorthArrow.Click += _btnNorthArrow_Click;

            //_Insert Scale bar
            this._btnScaleBar.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnScaleBar.Image = Images.ScaleBar;
            this._btnScaleBar.Size = new Size(23, 22);
            this._btnScaleBar.Text = MessageStrings.LayoutInsertMenuStripScaleBar;
            this._btnScaleBar.Click += _btnScaleBar_Click;

            //_Insert Legend
            this._btnLegend.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnLegend.Image = Images.Legend;
            this._btnLegend.Size = new Size(23, 22);
            this._btnLegend.Text = MessageStrings.LayoutInsertMenuStripLegend;
            this._btnLegend.Click += _btnLegend_Click;

            //_Insert Bitmap
            this._btnBitmap.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this._btnBitmap.Image = Images.Apple;
            this._btnBitmap.Size = new Size(23, 22);
            this._btnBitmap.Text = MessageStrings.LayoutInsertToolStripBitmap;
            this._btnBitmap.Click += _btnBitmap_Click;

            //
            // LayoutToolStrip
            //
            this.Items.AddRange(new ToolStripItem[] {
            this._btnMap,
            this._btnNorthArrow,
            this._btnLegend,
            this._btnScaleBar,
            this._btnText,
            this._btnRectangle,
            this._btnBitmap});
            this.ResumeLayout(false);
        }

        #endregion

        private ToolStripButton _btnBitmap;
        private ToolStripButton _btnLegend;
        private ToolStripButton _btnMap;
        private ToolStripButton _btnNorthArrow;
        private ToolStripButton _btnRectangle;
        private ToolStripButton _btnScaleBar;
        private ToolStripButton _btnText;
    }
}
