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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A Brian Marchioni original toolstrip... preloaded with content.
    /// </summary>
    //This control will no longer be visible
    [ToolboxItem(false)]
    public partial class LayoutInsertToolStrip : ToolStrip
    {
        #region Fields
        
        private LayoutControl _layoutControl;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates an instance of the toolstrip
        /// </summary>
        public LayoutInsertToolStrip()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The layout control associated with this toolstrip
        /// </summary>
        [Browsable(false)]
        public LayoutControl LayoutControl
        {
            get { return _layoutControl; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                _layoutControl = value;
            }
        }

        #endregion

        #region Methods

        private void _btnLegend_Click(object sender, EventArgs e)
        {
            _layoutControl.AddElementWithMouse(_layoutControl.CreateLegendElement());
        }

        //Adds a scale bar element to the layout and if there is already a map on the form we link it to the first one
        private void _btnScaleBar_Click(object sender, EventArgs e)
        {
            _layoutControl.AddElementWithMouse(_layoutControl.CreateScaleBarElement());
        }

        //Fires the print method on the layoutcontrol
        private void _btnMap_Click(object sender, EventArgs e)
        {
            if (_layoutControl.MapControl != null)
            {
                _layoutControl.AddElementWithMouse(_layoutControl.CreateMapElement());
            }
            else
            {
                MessageBox.Show(Parent, "Unable to add map without associated MapControl.", "Missing MapControl",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
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
            var ofd = new OpenFileDialog
            {
                Filter = "Images (*.png, *.jpg, *.bmp, *.gif, *.tif)|*.png;*.jpg;*.bmp;*.gif;*.tif",
                FilterIndex = 1,
                CheckFileExists = true
            };
            if (ofd.ShowDialog(Parent) == DialogResult.OK)
            {
                var newBitmap = new LayoutBitmap {Size = new SizeF(100, 100), Filename = ofd.FileName};
                _layoutControl.AddElementWithMouse(newBitmap);
            }
        }

        #endregion
    }
}