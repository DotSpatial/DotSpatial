// ********************************************************************************************************
// Product Name: DotSpatial.Layout.LayoutInsertToolStrip
// Description:  A tool strip designed to work along with the layout engine
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll Version 6.0
//
// The Initial Developer of this Original Code is Brian Marchionni. Created in Aug, 2009.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ************************************************+********************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A Brian Marchioni original toolstrip... preloaded with content.
    /// </summary>
    [ToolboxItem(true)]
    public class LayoutInsertToolStrip : ToolStrip
    {
        #region "Private Variables"

        private ToolStripButton _btnBitmap;
        private ToolStripButton _btnLegend;
        private ToolStripButton _btnMap;
        private ToolStripButton _btnNorthArrow;
        private ToolStripButton _btnRectangle;
        private ToolStripButton _btnScaleBar;
        private ToolStripButton _btnText;
        private LayoutControl _layoutControl;

        #endregion

        #region "Constructor"

        /// <summary>
        /// Creates an instance of the toolstrip
        /// </summary>
        public LayoutInsertToolStrip()
        {
            InitializeComponent();
        }

        #endregion

        #region "properties"

        /// <summary>
        /// The layout control associated with this toolstrip
        /// </summary>
        [Browsable(false)]
        public LayoutControl LayoutControl
        {
            get { return _layoutControl; }
            set { _layoutControl = value; if (_layoutControl == null) return; }
        }

        #endregion

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

        #region "Envent Handlers"

        private void _btnLegend_Click(object sender, EventArgs e)
        {
            LayoutLegend lsb = _layoutControl.CreateLegendElement() as LayoutLegend;
            List<LayoutElement> mapElements = _layoutControl.LayoutElements.FindAll(delegate(LayoutElement o) { return (o is LayoutMap); });
            if (mapElements.Count > 0)
                lsb.Map = mapElements[0] as LayoutMap;
            lsb.LayoutControl = _layoutControl;
            _layoutControl.AddElementWithMouse(lsb);
        }

        //Adds a scale bar element to the layout and if there is already a map on the form we link it to the first one
        private void _btnScaleBar_Click(object sender, EventArgs e)
        {
            LayoutScaleBar lsb = _layoutControl.CreateScaleBarElement() as LayoutScaleBar;
            List<LayoutElement> mapElements = _layoutControl.LayoutElements.FindAll(delegate(LayoutElement o) { return (o is LayoutMap); });
            if (mapElements.Count > 0)
                lsb.Map = mapElements[0] as LayoutMap;
            lsb.LayoutControl = _layoutControl;
            _layoutControl.AddElementWithMouse(lsb);
        }

        //Fires the print method on the layoutcontrol
        private void _btnMap_Click(object sender, EventArgs e)
        {
            _layoutControl.AddElementWithMouse(_layoutControl.CreateMapElement());
        }

        //Fires the saveas method on the layoutcontrol
        private void _btnText_Click(object sender, EventArgs e)
        {
            _layoutControl.AddElementWithMouse(new LayoutText());
        }

        //Fires the save method on the layoutcontrol
        private void _btnRectangle_Click(object sender, EventArgs e)
        {
            _layoutControl.AddElementWithMouse(new LayoutRectangle());
        }

        //Fires the new method on the layoutcontrol
        private void _btnNorthArrow_Click(object sender, EventArgs e)
        {
            _layoutControl.AddElementWithMouse(new LayoutNorthArrow());
        }

        //Fires the open method on the layoutcontrol
        private void _btnBitmap_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images (*.png, *.jpg, *.bmp, *.gif, *.tif)|*.png;*.jpg;*.bmp;*.gif;*.tif";
            ofd.FilterIndex = 1;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                LayoutBitmap newBitmap = new LayoutBitmap();
                newBitmap.Size = new SizeF(100, 100);
                newBitmap.Filename = ofd.FileName;
                _layoutControl.AddElementWithMouse(newBitmap);
            }
        }

        #endregion
    }
}