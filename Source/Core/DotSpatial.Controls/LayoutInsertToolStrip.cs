// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This ToolStrip contains buttons that allow the insertion of layout elements.
    /// </summary>
    [ToolboxItem(false)]
    public partial class LayoutInsertToolStrip : LayoutToolStrip
    {
        #region Constructors

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
        public override LayoutControl LayoutControl
        {
            get
            {
                return base.LayoutControl;
            }

            set
            {
                if (base.LayoutControl != null)
                {
                    base.LayoutControl.MouseModeChanged -= LayoutControlMouseModeChanged;
                }

                base.LayoutControl = value;

                if (base.LayoutControl != null)
                {
                    base.LayoutControl.MouseModeChanged += LayoutControlMouseModeChanged;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Activates a function that allows the user to add a bitmap to the layout.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void BtnBitmapClick(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Filter = @"Images (*.png, *.jpg, *.bmp, *.gif, *.tif)|*.png;*.jpg;*.bmp;*.gif;*.tif",
                FilterIndex = 1,
                CheckFileExists = true
            };
            if (ofd.ShowDialog(Parent) == DialogResult.OK)
            {
                var newBitmap = new LayoutBitmap
                {
                    Size = new SizeF(100, 100),
                    Filename = ofd.FileName
                };
                LayoutControl.AddElementWithMouse(newBitmap);
                SetChecked(_btnBitmap);
            }
        }

        /// <summary>
        /// Activates the default cursor.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void BtnDefaultClick(object sender, EventArgs e)
        {
            LayoutControl.MouseMode = MouseMode.Default;
        }

        /// <summary>
        /// Activates a function that allows the user to add a legend to the layout.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void BtnLegendClick(object sender, EventArgs e)
        {
            LayoutControl.AddElementWithMouse(LayoutControl.CreateLegendElement());
            SetChecked(_btnLegend);
        }

        /// <summary>
        /// Activates a function that allows the user to add a map to the layout.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void BtnMapClick(object sender, EventArgs e)
        {
            if (LayoutControl.MapControl != null)
            {
                LayoutControl.AddElementWithMouse(LayoutControl.CreateMapElement());
                SetChecked(_btnMap);
            }
            else
            {
                MessageBox.Show(Parent, MessageStrings.LayoutInsertToolStrip_UnableToAddMapWithoutAssociatedMapControl, MessageStrings.LayoutInsertToolStrip_MissingMapControl, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Activates a function that allows the user to add a north arrow to the layout.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void BtnNorthArrowClick(object sender, EventArgs e)
        {
            LayoutControl.AddElementWithMouse(new LayoutNorthArrow());
            SetChecked(_btnNorthArrow);
        }

        /// <summary>
        /// Activates a function that allows the user to add a rectangle to the layout.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void BtnRectangleClick(object sender, EventArgs e)
        {
            LayoutControl.AddElementWithMouse(new LayoutRectangle());
            SetChecked(_btnRectangle);
        }

        /// <summary>
        /// Activates a function that allows the user to add a scale bar to the layout.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void BtnScaleBarClick(object sender, EventArgs e)
        {
            LayoutControl.AddElementWithMouse(LayoutControl.CreateScaleBarElement());
            SetChecked(_btnScaleBar);
        }

        /// <summary>
        /// Activates a function that allows the user to add some text to the layout.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void BtnTextClick(object sender, EventArgs e)
        {
            LayoutControl.AddElementWithMouse(new LayoutText());
            SetChecked(_btnText);
        }

        /// <summary>
        /// Check the default button on default mousemode and uncheck all buttons if were not in insert mode.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event args.</param>
        private void LayoutControlMouseModeChanged(object sender, EventArgs e)
        {
            switch (LayoutControl.MouseMode)
            {
                case MouseMode.Default:
                    SetChecked(_btnDefault);
                    break;
                case MouseMode.InsertNewElement:
                case MouseMode.StartInsertNewElement:
                    break;
                default:
                    UncheckAll();
                    break;
            }
        }

        #endregion
    }
}