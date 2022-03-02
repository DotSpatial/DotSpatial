// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// OutlinedSymbol.
    /// </summary>
    public class OutlinedSymbol : Symbol, IOutlinedSymbol
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OutlinedSymbol"/> class.
        /// </summary>
        public OutlinedSymbol()
        {
            UseOutline = false;
            OutlineWidth = 1;
            OutlineColor = Color.Black;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the outline color that surrounds this specific symbol.
        /// (this will have the same shape as the symbol, but be larger.
        /// </summary>
        [XmlIgnore]
        public Color OutlineColor { get; set; }

        /// <summary>
        /// Gets or sets the outline opacity. This redefines the Alpha channel of the color to a floating point opacity
        /// that ranges from 0 to 1.
        /// </summary>
        [Serialize("OutlineOpacity")]
        public float OutlineOpacity
        {
            get
            {
                return OutlineColor.A / 255F;
            }

            set
            {
                int alpha = (int)(value * 255);
                if (alpha > 255) alpha = 255;
                if (alpha < 0) alpha = 0;
                if (alpha != OutlineColor.A)
                {
                    OutlineColor = Color.FromArgb(alpha, OutlineColor);
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of the outline beyond the size of this symbol.
        /// </summary>
        [Serialize("OutlineWidth")]
        public double OutlineWidth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the outline should be used.
        /// </summary>
        [Serialize("UseOutline")]
        public bool UseOutline { get; set; }

        /// <summary>
        /// Gets or sets the outline color for XML serialization.
        /// </summary>
        [XmlElement("OutlineColor")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Serialize("XmlOutlineColor")]
        public string XmlOutlineColor
        {
            get
            {
                return ColorTranslator.ToHtml(OutlineColor);
            }

            set
            {
                OutlineColor = ColorTranslator.FromHtml(value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Copies only the use outline, outline width and outline color properties from the specified symbol.
        /// </summary>
        /// <param name="symbol">The symbol to copy from.</param>
        public void CopyOutline(IOutlinedSymbol symbol)
        {
            OutlineColor = symbol.OutlineColor;
            OutlineWidth = symbol.OutlineWidth;
            UseOutline = symbol.UseOutline;
        }

        /// <summary>
        /// Handles the drawing code, extending it to include some outline content.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="scaleSize">If this should draw in pixels, this should be 1. Otherwise, this should be
        /// the constant that you multiply against so that drawing using geographic units will draw in pixel units.</param>
        protected override void OnDraw(Graphics g, double scaleSize)
        {
            base.OnDraw(g, scaleSize);
            float dx = (float)(Size.Width * scaleSize / 2);
            float dy = (float)(Size.Height * scaleSize / 2);
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddRectangle(new RectangleF(-dx, -dy, 2F * dx, 2f * dy));
                OnDrawOutline(g, scaleSize, gp);
            }
        }

        /// <summary>
        /// Actually handles the rendering of the outline itself.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="scaleSize">If this should draw in pixels, this should be 1. Otherwise, this should be
        /// the constant that you multiply against so that drawing using geographic units will draw in pixel units.</param>
        /// <param name="gp">The graphics path of the outline.</param>
        protected virtual void OnDrawOutline(Graphics g, double scaleSize, GraphicsPath gp)
        {
            if (!UseOutline || OutlineWidth == 0) return;

            using (Pen p = new Pen(OutlineColor)
            {
                Width = (float)(scaleSize * OutlineWidth),
                Alignment = PenAlignment.Outset
            })
            {
                g.DrawPath(p, gp);
            }
        }

        /// <summary>
        /// Occurs during the randomize process and allows future overriding of the process for sub-classes.
        /// </summary>
        /// <param name="generator">The random generator.</param>
        protected override void OnRandomize(Random generator)
        {
            // randomize properties of the base class & any properties that are types that implement IRandomizable
            base.OnRandomize(generator);

            // randomize whatever is left
            UseOutline = generator.Next(0, 1) == 1;
            OutlineColor = SymbologyGlobal.RandomColor();
            OutlineWidth = generator.Next();
        }

        #endregion
    }
}