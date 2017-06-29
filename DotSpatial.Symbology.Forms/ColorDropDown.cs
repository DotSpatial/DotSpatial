// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Projections library.
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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/24/2008 7:00:40 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// A drop down coded by
    /// </summary>
    internal class ColorDropDown : ComboBox
    {
        readonly Pen _boxPen;
        Brush _backBrush;
        Array _colors;
        Brush _foreBrush;

        /// <summary>
        /// Creates a dropdown with known colors populated in it
        /// </summary>
        public ColorDropDown()
        {
            // Fill the color array with known colors
            _colors = Enum.GetValues(typeof(KnownColor));
            _boxPen = Pens.Black;

            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;

            if (DesignMode == false)
            {
                foreach (KnownColor kc in _colors)
                {
                    Items.Add(kc);
                }
            }
        }

        /// <summary>
        /// Basically still allow this to be affected in code, but remove it from the designer stuff
        /// </summary>
        [Browsable(false)]
        public new DrawMode DrawMode
        {
            get { return base.DrawMode; }
            set { base.DrawMode = value; }
        }

        /// <summary>
        /// Gets or sets the currently selected color from this dropdown control
        /// </summary>
        public Color Value
        {
            get
            {
                Color col = Color.Empty;

                if (SelectedItem is KnownColor)
                {
                    // the color to draw
                    col = Color.FromKnownColor((KnownColor)SelectedItem);
                }
                if (SelectedItem is Color)
                {
                    col = (Color)SelectedItem;
                }
                return col;
            }
            set
            {
                foreach (object item in Items)
                {
                    Color col;
                    if (item is KnownColor)
                    {
                        col = Color.FromKnownColor(((KnownColor)item));
                        if (col == value)
                        {
                            SelectedItem = item;
                            return;
                        }
                    }
                    if (item is Color)
                    {
                        col = (Color)item;
                        if (col == value)
                        {
                            SelectedItem = item;
                            return;
                        }
                    }
                }
                Items.Add(value);
                SelectedIndex = Items.Count - 1;
            }
        }

        /// <summary>
        /// Basically still allow this to be affected in code, but remove it from the designer stuff
        /// </summary>
        [Browsable(false)]
        public new ComboBoxStyle DropDownStyle
        {
            get { return base.DropDownStyle; }
            set { base.DropDownStyle = value; }
        }

        /// <summary>
        /// Disposes stuff
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            _colors = null;
            if (_backBrush != null) _backBrush.Dispose();
            if (_foreBrush != null) _foreBrush.Dispose();
            // Do not dispose these because they are system members
            // if (_boxPen != null) _boxPen.Dispose();
            // if (_fillBrush != null) _fillBrush.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Prevents flicker .. or possibly does nothing.. I'm not sure.
        /// </summary>
        /// <param name="pevent">PaintEventArgs</param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //base.OnPaintBackground(pevent);
        }

        /// <summary>
        /// Actually handles the drawing of a single item
        /// </summary>
        /// <param name="e">DrawItemEventArgs</param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (IsDisposed) return;
            if (Visible == false) return;
            if (e.Index == -1) return; // no items to draw
            Color col = Color.Empty;
            string name = "Empty";
            if (Items[e.Index] is KnownColor)
            {
                // the color to draw
                col = Color.FromKnownColor((KnownColor)Items[e.Index]);
                name = col.Name;
            }
            if (Items[e.Index] is Color)
            {
                col = (Color)Items[e.Index];
                name = "R: " + col.R + " G: " + col.G + " B: " + col.B;
            }
            // by erasing and drawing off-camera, we avoid flicker
            Bitmap bmp = new Bitmap(e.Bounds.Width, e.Bounds.Height);
            Graphics backBuffer = Graphics.FromImage(bmp);

            // erase by drawing the background
            if (_backBrush == null) _backBrush = new SolidBrush(e.BackColor);
            if (_foreBrush == null) _foreBrush = new SolidBrush(e.ForeColor);
            if (((SolidBrush)_backBrush).Color != e.BackColor)
            {
                _backBrush = new SolidBrush(e.BackColor);
            }
            if (((SolidBrush)_foreBrush).Color != e.ForeColor)
            {
                _foreBrush = new SolidBrush(e.ForeColor);
            }

            backBuffer.FillRectangle(_backBrush, new Rectangle(0, 0, e.Bounds.Width, e.Bounds.Height));

            // draw the color box
            Rectangle colorBox = new Rectangle(1, 1, 25, e.Bounds.Height - 2);
            Brush colBrush = new SolidBrush(col);
            backBuffer.FillRectangle(colBrush, colorBox);
            colBrush.Dispose();
            backBuffer.DrawRectangle(_boxPen, colorBox);

            // label the color
            backBuffer.DrawString(name, Font, _foreBrush, 32F, 1);

            // now that we have drawn the item, add it to the front
            e.Graphics.DrawImage(bmp, e.Bounds.X, e.Bounds.Y);
        }
    }
}