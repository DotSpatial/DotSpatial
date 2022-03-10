// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

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
        #region Methods

        /// <summary>
        /// Occurs during the drawing of an item.
        /// </summary>
        /// <param name="e">The event args.</param>
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
                Pen p = new(Color.FromArgb(215, 238, 252));
                SymbologyGlobal.DrawRoundedRectangle(e.Graphics, p, e.Bounds);
                p.Dispose();
            }

            string name = Items[e.Index].ToString();
            FontFamily ff = new(name);
            Font fnt = new("Arial", 10, FontStyle.Regular);
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
    }
}