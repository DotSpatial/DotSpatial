// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This is a glyph on the map that if you click on it, it will launch the PluginDialog.
    /// </summary>
    public class MapFunctionGlyph : MapFunction
    {
        #region Fields

        /// <summary>
        /// This is the cursor that was the cursor before we did anything.
        /// </summary>
        private Cursor _previousCursor;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MapFunctionGlyph"/> class.
        /// </summary>
        public MapFunctionGlyph()
        {
            YieldStyle = YieldStyles.AlwaysOn;
            Cursor = Cursors.Hand;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapFunctionGlyph"/> class with the specified Map.
        /// </summary>
        /// <param name="map">The map the tool should work on.</param>
        public MapFunctionGlyph(IMap map)
            : base(map)
        {
            YieldStyle = YieldStyles.AlwaysOn;
            Cursor = Cursors.Hand;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the cursor to use when the mouse is over this glyph.
        /// </summary>
        public Cursor Cursor { get; set; }

        /// <summary>
        /// Gets the GlyphBounds. This should be overridden by the specific glyph so they don't overlap and appear where they should.
        /// </summary>
        public virtual Rectangle GlyphBounds => new Rectangle(Map.Bounds.Right - 25, Map.Bounds.Bottom - 25, 25, 25);

        /// <summary>
        /// Gets or sets a value indicating whether or not this glyph currently holds the mouse.
        /// </summary>
        public bool HasMouse { get; set; }

        /// <summary>
        /// Gets or sets the LitImage. Unless the OnDrawGlyphLit method is overridden, this is drawn when the mouse is over the image.
        /// </summary>
        public Image LitImage { get; set; }

        /// <summary>
        /// Gets or sets the normal image. Unless the OnDrawGlyph method is overridden, this is drawn. If no LitImage is specified,
        /// then this image will be drawn also for the LitImage case.
        /// </summary>
        public Image NormalImage { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the glyph on the map.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnDraw(MapDrawArgs e)
        {
            Rectangle glyph = GlyphBounds;
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
        /// <param name="e">The event args.</param>
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
        /// <param name="e">The event args.</param>
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
        /// Occurs when the mouse clicks on the glyph.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnGlpyhClick(GeoMouseArgs e)
        {
        }

        /// <summary>
        /// Occurs when the mouse leaves the glyph.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnGlyphMouseEnter(GeoMouseArgs e)
        {
            HasMouse = true;
            _previousCursor = Map.Cursor;
            Map.Invalidate(GlyphBounds);
            Map.Cursor = Cursor;
        }

        /// <summary>
        /// Occurs when the mouse enters the glyph.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected virtual void OnGlyphMouseLeave(GeoMouseArgs e)
        {
            HasMouse = false;
            Map.Invalidate(GlyphBounds);
            if (_previousCursor != null)
            {
                Map.Cursor = _previousCursor;
            }
        }

        /// <summary>
        /// Even though we don't take action here, we need to indicate that we handled the event so that the other
        /// functions that my be active don't do anything for this part.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void OnMouseDown(GeoMouseArgs e)
        {
            Rectangle glyph = GlyphBounds;
            if (glyph.Contains(e.Location))
            {
                // whether they do anything or not, the glyph is the only thing that should happen in this window.
                e.Handled = true;
            }
        }

        /// <inheritdoc/>
        protected override void OnMouseMove(GeoMouseArgs e)
        {
            Rectangle glyph = GlyphBounds;
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
            Rectangle glyph = GlyphBounds;
            if (glyph.Contains(e.Location))
            {
                OnGlpyhClick(e);

                // whether they do anything or not, the glyph is the only thing that should happen in this window.
                e.Handled = true;
            }
        }

        #endregion
    }
}