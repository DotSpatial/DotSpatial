// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// LabelAlignmentButton.
    /// </summary>
    public class LabelAlignmentButton
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelAlignmentButton"/> class.
        /// </summary>
        public LabelAlignmentButton()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelAlignmentButton"/> class with the specified rectangle as the bounds.
        /// </summary>
        /// <param name="bounds">The bounds relative to the parent client.</param>
        /// <param name="backColor">The background color.</param>
        public LabelAlignmentButton(Rectangle bounds, Color backColor)
        {
            Bounds = bounds;
            BackColor = backColor;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the color used as the background color.
        /// </summary>
        public Color BackColor { get; set; }

        /// <summary>
        /// Gets or sets the bounds for this button.
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this button is currently highlighted (mouse over).
        /// </summary>
        public bool Highlighted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this button is currently selected.
        /// </summary>
        public bool Selected { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Instructs this button to draw itself.
        /// </summary>
        /// <param name="g">The graphics surface to draw to.</param>
        public void Draw(Graphics g)
        {
            if (Bounds.Width == 0 || Bounds.Height == 0) return;

            Pen border = null;
            Brush fill = null;
            if (!Selected && !Highlighted)
            {
                border = new Pen(Color.Gray);
                fill = new LinearGradientBrush(Bounds, BackColor.Lighter(.2f), BackColor.Darker(.2f), 45);
            }

            if (!Selected && Highlighted)
            {
                border = new Pen(Color.FromArgb(216, 240, 250));
                fill = new LinearGradientBrush(Bounds, Color.FromArgb(245, 250, 253), Color.FromArgb(232, 245, 253), LinearGradientMode.Vertical);
            }

            if (Selected && !Highlighted)
            {
                border = new Pen(Color.FromArgb(153, 222, 253));
                fill = new LinearGradientBrush(Bounds, Color.FromArgb(241, 248, 253), Color.FromArgb(213, 239, 252), LinearGradientMode.Vertical);
            }

            if (Selected && Highlighted)
            {
                border = new Pen(Color.FromArgb(182, 230, 251));
                fill = new LinearGradientBrush(Bounds, Color.FromArgb(232, 246, 253), Color.FromArgb(196, 232, 250), LinearGradientMode.Vertical);
            }

            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddRoundedRectangle(Bounds, 2);
                if (fill != null) g.FillPath(fill, gp);
                if (border != null) g.DrawPath(border, gp);
            }

            fill?.Dispose();
        }

        #endregion
    }
}