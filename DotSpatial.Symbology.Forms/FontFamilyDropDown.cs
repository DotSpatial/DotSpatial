// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.Forms.dll
// Description:  The Windows Forms user interface layer for the DotSpatial.Symbology library.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/21/2009 4:10:28 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// This control doesn't actually pre-load items, it merely overrides how the items
    /// are drawn.
    /// </summary>
    internal class FontFamilyDropDown : ComboBox
    {
        #region Private Variables

        #endregion

        #region Constructors

        #endregion

        #region Methods

        /// <summary>
        /// Occurs during the drawing of an item
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Rectangle outer = e.Bounds;
            outer.Inflate(1, 1);
            e.Graphics.FillRectangle(Brushes.White, outer);
            Brush fontBrush = Brushes.Black;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                Rectangle r = e.Bounds;
                r.Inflate(-1, -1);
                e.Graphics.FillRectangle(SymbologyGlobal.HighlightBrush(r, Color.FromArgb(215, 238, 252)), r);
                Pen p = new Pen(Color.FromArgb(215, 238, 252));
                SymbologyGlobal.DrawRoundedRectangle(e.Graphics, p, e.Bounds);
                p.Dispose();
            }

            string name = Items[e.Index].ToString();
            FontFamily ff = new FontFamily(name);
            Font fnt = new Font("Arial", 10, FontStyle.Regular);
            if (ff.IsStyleAvailable(FontStyle.Regular))
            {
                fnt = new Font(name, 10, FontStyle.Regular);
            }
            else if (ff.IsStyleAvailable(FontStyle.Italic))
            {
                fnt = new Font(name, 10, FontStyle.Italic);
            }
            SizeF box = e.Graphics.MeasureString(name, Font);
            e.Graphics.DrawString(name, Font, fontBrush, e.Bounds.X, e.Bounds.Y);
            e.Graphics.DrawString("ABC", fnt, fontBrush, e.Bounds.X + box.Width, e.Bounds.Y);
        }

        #endregion

        #region Properties

        #endregion
    }
}