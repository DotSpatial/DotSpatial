// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// DashSlider.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class DashSlider
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DashSlider"/> class.
        /// </summary>
        /// <param name="sliderOrientation">The slider orientation.</param>
        public DashSlider(Orientation sliderOrientation)
        {
            Orientation = sliderOrientation;
            Visible = true;
            Size = new SizeF(20F, 20F);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the bounds of this slider.
        /// </summary>
        [Browsable(false)]
        public virtual RectangleF Bounds => new RectangleF(Position, Size);

        /// <summary>
        /// Gets or sets the color for this control if it is not using a custom image.
        /// </summary>
        [Description("Gets or sets the color for this control if it is not using a custom image.")]
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets an image that can be used instead of the default triangular drawing.
        /// </summary>
        [Description("Gets or sets an image that can be used instead of the default drawing")]
        public Image Image { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this slider is in the process of being adjusted.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDragging { get; set; }

        /// <summary>
        /// Gets whether this control is vertical or horizontal.
        /// </summary>
        [Description("Gets or sets whether this control is vertical or horizontal")]
        public Orientation Orientation { get; }

        /// <summary>
        /// Gets or sets the position. Whether the X or Y coordinate is used depends on the orientation.
        /// </summary>
        [Description("Gets or sets the position. Whether the X or Y coordinate is used depends on the orientation.")]
        [TypeConverter(typeof(PointFConverter))]
        public PointF Position { get; set; }

        /// <summary>
        /// Gets or sets the size of this slider. This is only used when the slider is not based on an image.
        /// </summary>
        [Description("Gets or sets the size of this slider. This is only used when the slider is not based on an image.")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public SizeF Size { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this slider will draw itself.
        /// </summary>
        [Description("Gets or sets a boolean indicating whether this slider will draw itself.")]
        public bool Visible { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the current control.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="clipRectangle">The clip rectangle.</param>
        public virtual void Draw(Graphics g, Rectangle clipRectangle)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            if (Visible == false) return;

            if (Image != null)
            {
                g.DrawImage(Image, Position);
            }
            else
            {
                LinearGradientBrush br = CreateGradientBrush(Color, Bounds.Location, new PointF(Bounds.Right, Bounds.Bottom));
                g.FillRectangle(br, Bounds);
                br.Dispose();
                g.DrawRectangle(Pens.Black, Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
            }
        }

        /// <summary>
        /// Creates a Gradient Brush.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="topLeft">The top left point.</param>
        /// <param name="bottomRight">The bottom right point.</param>
        /// <returns>The resulting linear gradient brush.</returns>
        protected static LinearGradientBrush CreateGradientBrush(Color color, PointF topLeft, PointF bottomRight)
        {
            float b = color.GetBrightness();
            b += .3F;
            if (b > 1F) b = 1F;
            Color light = SymbologyGlobal.ColorFromHsl(color.GetHue(), color.GetSaturation(), b);
            float d = color.GetBrightness();
            d -= .3F;
            if (d < 0F) d = 0F;
            Color dark = SymbologyGlobal.ColorFromHsl(color.GetHue(), color.GetSaturation(), d);
            return new LinearGradientBrush(topLeft, bottomRight, light, dark);
        }

        #endregion
    }
}