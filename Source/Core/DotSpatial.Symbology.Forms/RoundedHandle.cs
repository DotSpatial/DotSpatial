// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// RoundedHandle.
    /// </summary>
    public class RoundedHandle
    {
        #region Fields

        private float _position;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundedHandle"/> class.
        /// </summary>
        public RoundedHandle()
        {
            Width = 10;
            RoundingRadius = 4;
            Color = Color.SteelBlue;
            Visible = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoundedHandle"/> class specifying the parent gradient slider.
        /// </summary>
        /// <param name="parent">The parent gradient slider.</param>
        public RoundedHandle(GradientSlider parent)
        {
            Parent = parent;
            Width = 10;
            RoundingRadius = 4;
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
        /// Gets or sets the parent.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GradientSlider Parent { get; set; }

        /// <summary>
        /// Gets or sets the Position.
        /// </summary>
        [Description("Gets or sets the Position")]
        public float Position
        {
            get
            {
                return _position;
            }

            set
            {
                _position = value;
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

        #endregion
    }
}