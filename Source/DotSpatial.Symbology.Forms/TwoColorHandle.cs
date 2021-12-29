// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// TwoColorHandle.
    /// </summary>
    public class TwoColorHandle
    {
        #region Fields

        private float _position;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoColorHandle"/> class.
        /// </summary>
        public TwoColorHandle()
        {
            Width = 5;
            RoundingRadius = 2;
            Color = Color.SteelBlue;
            Visible = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TwoColorHandle"/> class, specifying the parent two color slider.
        /// </summary>
        /// <param name="parent">The parent two color slider.</param>
        public TwoColorHandle(TwoColorSlider parent)
        {
            Parent = parent;
            Width = 5;
            RoundingRadius = 2;
            Color = Color.SteelBlue;
            Visible = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the basic color of the slider.
        /// </summary>
        [Description("Gets or sets the basic color of the slider")]
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is visible or not.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsDragging { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is the left handle or not.
        /// </summary>
        public bool IsLeft { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TwoColorSlider Parent { get; set; }

        /// <summary>
        /// Gets or sets the Position.
        /// </summary>
        [Description("Gets or sets the Position")]
        public float Position
        {
            get
            {
                float w = Width * (Parent.Maximum - Parent.Minimum) / Parent.Width;
                return IsLeft ? _position + w : _position;
            }

            set
            {
                float w = Width * (Parent.Maximum - Parent.Minimum) / Parent.Width;
                _position = IsLeft ? value - w : value;
                if (Parent != null)
                {
                    if (_position > Parent.Maximum) _position = Parent.Maximum;
                    if (_position < Parent.Minimum) _position = Parent.Minimum;
                }
            }
        }

        /// <summary>
        /// Gets or sets the integer describing the radius of the curves in the corners of the slider.
        /// </summary>
        [Description("Gets or sets the integer describing the radius of the curves in the corners of the slider")]
        public int RoundingRadius { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this handle is drawn and visible.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the integer width of this slider in pixels.
        /// </summary>
        [Description("Gets or sets the integer width of this slider in pixels.")]
        public int Width { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Draws this slider on the specified graphics object.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        public void Draw(Graphics g)
        {
            if (!Visible || Width == 0) return;
            Rectangle bounds = GetBounds();
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddRoundedRectangle(bounds, RoundingRadius);
                using (LinearGradientBrush lgb = new LinearGradientBrush(bounds, Color.Lighter(.3F), Color.Darker(.3F), LinearGradientMode.ForwardDiagonal))
                {
                    g.FillPath(lgb, gp);
                    using (Pen l = new Pen(Color.Darker(.2f), 2))
                    using (Pen r = new Pen(Color.Lighter(.2f), 2))
                    {
                        if (IsLeft)
                        {
                            g.DrawLine(l, bounds.Right - 1, bounds.Top, bounds.Right - 1, bounds.Height);
                        }
                        else
                        {
                            g.DrawLine(r, bounds.Left + 1, bounds.Top, bounds.Left + 1, bounds.Right);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the bounds of this handle in the coordinates of the parent slider.
        /// </summary>
        /// <returns>The bounds of this handle.</returns>
        public Rectangle GetBounds()
        {
            float sx = (_position - Parent.Minimum) / (Parent.Maximum - Parent.Minimum);
            int x = Convert.ToInt32(sx * (Parent.Width - Width));
            Rectangle bounds = new Rectangle(x, 0, Width, Parent.Height);
            return bounds;
        }

        /// <summary>
        /// Gets the color at the handles current position.
        /// </summary>
        /// <returns>The color at the handles current position.</returns>
        public Color GetColor()
        {
            Color min = Parent.MinimumColor;
            Color max = Parent.MaximumColor;
            float p = _position;
            int r = min.R + (int)((max.R - min.R) * p);
            int g = min.G + (int)((max.G - min.G) * p);
            int b = min.B + (int)((max.B - min.B) * p);
            return Color.FromArgb(r, g, b);
        }

        #endregion
    }
}