// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A Brian Marchioni original toolstrip... preloaded with content.
    /// </summary>
    // This control will no longer be visible
    [ToolboxItem(false)]
    public partial class LayoutInsertToolStrip : ToolStrip
    {
        #region Fields

        private LayoutControl _layoutControl;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayoutInsertToolStrip"/> class.
        /// </summary>
        public LayoutInsertToolStrip()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the layout control associated with this toolstrip.
        /// </summary>
        [Browsable(false)]
        public LayoutControl LayoutControl
        {
            get
            {
                return _layoutControl;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _layoutControl = value;
            }
        }

        #endregion

        #region Methods

        // Fires the open method on the layoutcontrol
        private void BtnBitmapClick(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog
            {
                Filter = @"Images (*.png, *.jpg, *.bmp, *.gif, *.tif)|*.png;*.jpg;*.bmp;*.gif;*.tif",
                FilterIndex = 1,
                CheckFileExists = true
            })
            {
                if (ofd.ShowDialog(Parent) == DialogResult.OK)
                {
                    var newBitmap = new LayoutBitmap
                    {
                        Size = new SizeF(100, 100),
                        Filename = ofd.FileName
                    };
                    _layoutControl.AddElementWithMouse(newBitmap);
                }
            }
        }

        private void BtnLegendClick(object sender, EventArgs e)
        {
            _layoutControl.AddElementWithMouse(_layoutControl.CreateLegendElement());
        }

        // Fires the print method on the layoutcontrol
        private void BtnMapClick(object sender, EventArgs e)
        {
            if (_layoutControl.MapControl != null)
            {
                _layoutControl.AddElementWithMouse(_layoutControl.CreateMapElement());
            }
            else
            {
                MessageBox.Show(Parent, MessageStrings.LayoutInsertToolStrip_UnableToAddMapWithoutAssociatedMapControl, MessageStrings.LayoutInsertToolStrip_MissingMapControl, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Fires the new method on the layoutcontrol
        private void BtnNorthArrowClick(object sender, EventArgs e)
        {
            _layoutControl.AddElementWithMouse(new LayoutNorthArrow());
        }

        // Fires the save method on the layoutcontrol
        private void BtnRectangleClick(object sender, EventArgs e)
        {
            _layoutControl.AddElementWithMouse(new LayoutRectangle());
        }

        // Adds a scale bar element to the layout and if there is already a map on the form we link it to the first one
        private void BtnScaleBarClick(object sender, EventArgs e)
        {
            _layoutControl.AddElementWithMouse(_layoutControl.CreateScaleBarElement());
        }

        // Fires the saveas method on the layoutcontrol
        private void BtnTextClick(object sender, EventArgs e)
        {
            _layoutControl.AddElementWithMouse(new LayoutText());
        }

        #endregion
    }
}