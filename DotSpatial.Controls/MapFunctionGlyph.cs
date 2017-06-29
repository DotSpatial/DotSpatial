// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/29/2010 12:39:30 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
//       Name     |    Date    |                          Comments
// ---------------|------------|--------------------------------------------------------------------------
// ********************************************************************************************************

using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is a glyph on the map that if you click on it, it will launch the PluginDialog.
    /// </summary>
    public class MapFunctionGlyph : MapFunction
    {
        /// <summary>
        /// This is the cursor that was the cursor before we did anything.
        /// </summary>
        private Cursor _previousCursor;

        /// <summary>
        /// Creates a new instance of the GlyphFunction
        /// </summary>
        public MapFunctionGlyph()
        {
            YieldStyle = YieldStyles.AlwaysOn;
            Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Creates a new instance of a GLyphFunction with the specified Map
        /// </summary>
        /// <param name="map"></param>
        public MapFunctionGlyph(Map map)
            : base(map)
        {
            YieldStyle = YieldStyles.AlwaysOn;
            Cursor = Cursors.Hand;
        }

        /// <summary>
        /// The cursor to use when the mouse is over this glyph.
        /// </summary>
        public Cursor Cursor { get; set; }

        /// <summary>
        /// This should be overridden by the specific glyph so they don't overlap and appear where they should.
        /// </summary>
        public virtual Rectangle GlpyhBounds
        {
            get { return new Rectangle(Map.Bounds.Right - 25, Map.Bounds.Bottom - 25, 25, 25); }
        }

        /// <summary>
        /// Gets or sets a Boolean indicating whether or not this glyph currently holds the mouse
        /// </summary>
        public bool HasMouse { get; set; }

        /// <summary>
        /// Unless the OnDrawGlyphLit method is overridden, this is drawn when the mouse is over the image.
        /// </summary>
        public Image LitImage { get; set; }

        /// <summary>
        /// Unless the OnDrawGlyph method is overridden, this is drawn.  If no LitImage is specified,
        /// then this image will be drawn also for the LitImage case.
        /// </summary>
        public Image NormalImage { get; set; }

        /// <summary>
        /// Draws the glyph on the map.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDraw(MapDrawArgs e)
        {
            Rectangle glyph = GlpyhBounds;
            Bitmap bmp = new Bitmap(glyph.Width, glyph.Height);
            Graphics g = Graphics.FromImage(bmp);
            if (e.ClipRectangle.IntersectsWith(glyph))
            {
                Rectangle r = Rectangle.Intersect(e.ClipRectangle, glyph);
                r.X -= glyph.X;
                r.Y -= glyph.Y;
                if (HasMouse)
                {
                    OnDrawGlyphLit(new PaintEventArgs(g, r));
                }
                else
                {
                    OnDrawGlyph(new PaintEventArgs(g, r));
                }
            }
            e.Graphics.DrawImageUnscaled(bmp, glyph.X, glyph.Y);
            base.OnDraw(e);
        }

        /// <summary>
        /// The drawing space is only the size of the GlyphBounds.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDrawGlyph(PaintEventArgs e)
        {
            if (NormalImage == null)
            {
                return;
            }
            e.Graphics.DrawImageUnscaled(NormalImage, 0, 0);
        }

        /// <summary>
        /// When the mouse moves over a glyph, it should light up or change to show it is clickable.
        /// </summary>
        protected virtual void OnDrawGlyphLit(PaintEventArgs e)
        {
            if (LitImage == null)
            {
                if (NormalImage == null)
                {
                    return;
                }
                e.Graphics.DrawImageUnscaled(NormalImage, 0, 0);
                return;
            }
            e.Graphics.DrawImageUnscaled(LitImage, 0, 0);
        }

        /// <summary>
        /// Occurs when the mouse clicks on the glyph
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnGlpyhClick(GeoMouseArgs e)
        {
        }

        /// <summary>
        /// Occurs when the mouse leaves the glyph
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnGlyphMouseEnter(GeoMouseArgs e)
        {
            HasMouse = true;
            _previousCursor = Map.Cursor;
            Map.Invalidate(GlpyhBounds);
            Map.Cursor = Cursor;
        }

        /// <summary>
        /// Occurs when the mouse enters the glyph
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnGlyphMouseLeave(GeoMouseArgs e)
        {
            HasMouse = false;
            Map.Invalidate(GlpyhBounds);
            if (_previousCursor != null)
            {
                Map.Cursor = _previousCursor;
            }
        }

        /// <summary>
        /// Even though we don't take action here, we need to indicate that we handled the event so that the other
        /// functions that my be active don't do anything for this part.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(GeoMouseArgs e)
        {
            Rectangle glyph = GlpyhBounds;
            if (glyph.Contains(e.Location))
            {
                // whether they do anything or not, the glyph is the only thing that should happen in this window.
                e.Handled = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(GeoMouseArgs e)
        {
            Rectangle glyph = GlpyhBounds;
            if (glyph.Contains(e.Location))
            {
                if (!HasMouse)
                {
                    HasMouse = true;
                    OnGlyphMouseEnter(e);
                }
                e.Handled = true;
            }
            else
            {
                if (HasMouse)
                {
                    HasMouse = false;
                    OnGlyphMouseLeave(e);
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseUp(GeoMouseArgs e)
        {
            Rectangle glyph = GlpyhBounds;
            if (glyph.Contains(e.Location))
            {
                OnGlpyhClick(e);
                // whether they do anything or not, the glyph is the only thing that should happen in this window.
                e.Handled = true;
            }
        }
    }
}