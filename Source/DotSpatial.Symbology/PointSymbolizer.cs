// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using DotSpatial.Data;
using DotSpatial.NTSExtension;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Symbolizer for point features.
    /// </summary>
    public class PointSymbolizer : FeatureSymbolizer, IPointSymbolizer
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PointSymbolizer"/> class.
        /// </summary>
        public PointSymbolizer()
        {
            Configure();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointSymbolizer"/> class with only one symbol.
        /// </summary>
        /// <param name="symbol">The symbol to use for creating this symbolizer</param>
        public PointSymbolizer(ISymbol symbol)
        {
            Smoothing = true;
            Symbols = new CopyList<ISymbol>
                      {
                          symbol
                      };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointSymbolizer"/> class.
        /// </summary>
        /// <param name="presetSymbols">The symbols that are used.</param>
        public PointSymbolizer(IEnumerable<ISymbol> presetSymbols)
        {
            Smoothing = true;
            Symbols = new CopyList<ISymbol>();
            foreach (ISymbol symbol in presetSymbols)
            {
                Symbols.Add(symbol);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointSymbolizer"/> class with one member that constructed
        /// based on the values specified.
        /// </summary>
        /// <param name="color">The color of the symbol.</param>
        /// <param name="shape">The shape of the symbol.</param>
        /// <param name="size">The size of the symbol.</param>
        public PointSymbolizer(Color color, PointShape shape, double size)
        {
            Smoothing = true;
            Symbols = new CopyList<ISymbol>();
            ISimpleSymbol ss = new SimpleSymbol(color, shape, size);
            Symbols.Add(ss);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointSymbolizer"/> class that has a member that is constructed
        /// from the specified image.
        /// </summary>
        /// <param name="image">The image to use as a point symbol</param>
        /// <param name="size">The desired output size of the larger dimension of the image.</param>
        public PointSymbolizer(Image image, double size)
        {
            Symbols = new CopyList<ISymbol>();
            IPictureSymbol ps = new PictureSymbol(image);
            ps.Size = new Size2D(size, size);
            Symbols.Add(ps);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointSymbolizer"/> class that has a character symbol based on the specified characteristics.
        /// </summary>
        /// <param name="character">The character to draw</param>
        /// <param name="fontFamily">The font family to use for rendering the font</param>
        /// <param name="color">The font color</param>
        /// <param name="size">The size of the symbol</param>
        public PointSymbolizer(char character, string fontFamily, Color color, double size)
        {
            Symbols = new CopyList<ISymbol>();
            CharacterSymbol cs = new CharacterSymbol(character, fontFamily, color, size);
            Symbols.Add(cs);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointSymbolizer"/> class.
        /// </summary>
        /// <param name="selected">Indicates whether to use the color for selected symbols.</param>
        public PointSymbolizer(bool selected)
        {
            Configure();
            if (!selected) return;

            ISimpleSymbol ss = Symbols[0] as ISimpleSymbol;
            if (ss != null)
            {
                ss.Color = Color.Cyan;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointSymbolizer"/> class.
        /// Sets the symbol type to geographic and generates a size that is 1/100 the width of the specified extents.
        /// </summary>
        /// <param name="selected">Indicates whether to use the color for selected symbols.</param>
        /// <param name="extents">Used for calculating the size of the symbol.</param>
        public PointSymbolizer(bool selected, IRectangle extents)
        {
            Configure();

            ScaleMode = ScaleMode.Geographic;
            Smoothing = false;
            ISymbol s = Symbols[0];
            if (s == null) return;

            s.Size.Width = extents.Width / 100;
            s.Size.Height = extents.Width / 100;
            ISimpleSymbol ss = Symbols[0] as ISimpleSymbol;
            if (ss != null && selected) ss.Color = Color.Cyan;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the set of layered symbols. The symbol with the highest index is drawn on top.
        /// </summary>
        [Serialize("Symbols")]
        public IList<ISymbol> Symbols { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the specified value
        /// </summary>
        /// <param name="g">The Graphics object to draw to</param>
        /// <param name="target">The Rectangle defining the bounds to draw in</param>
        public override void Draw(Graphics g, Rectangle target)
        {
            Size2D size = GetSize();
            double scaleH = target.Width / size.Width;
            double scaleV = target.Height / size.Height;
            double scale = Math.Min(scaleH, scaleV);
            Matrix old = g.Transform;
            Matrix shift = g.Transform;
            shift.Translate(target.X + (target.Width / 2), target.Y + (target.Height / 2));
            g.Transform = shift;
            Draw(g, scale);
            g.Transform = old;
        }

        /// <summary>
        /// Draws the point symbol to the specified graphics object by cycling through each of the layers and
        /// drawing the content. This assumes that the graphics object has been translated to the specified point.
        /// </summary>
        /// <param name="g">Graphics object that is used for drawing.</param>
        /// <param name="scaleSize">Scale size represents the constant to multiply to the geographic measures in order to turn them into pixel coordinates </param>
        public void Draw(Graphics g, double scaleSize)
        {
            GraphicsState s = g.Save();
            if (Smoothing)
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            else
            {
                g.SmoothingMode = SmoothingMode.None;
                g.TextRenderingHint = TextRenderingHint.SystemDefault;
            }

            foreach (ISymbol symbol in Symbols)
            {
                symbol.Draw(g, scaleSize);
            }

            g.Restore(s); // Changed by jany_ (2015-07-06) remove smoothing because we might not want to smooth whatever is drawn with g afterwards
        }

        /// <summary>
        /// Returns the color of the top-most layer symbol.
        /// </summary>
        /// <returns>The fill color.</returns>
        public Color GetFillColor()
        {
            if (Symbols == null) return Color.Empty;
            if (Symbols.Count == 0) return Color.Empty;

            IColorable c = Symbols[Symbols.Count - 1] as IColorable;
            return c?.Color ?? Color.Empty;
        }

        /// <inheritdoc />
        public override Size GetLegendSymbolSize()
        {
            Size2D sz = GetSize();
            int w = (int)sz.Width;
            int h = (int)sz.Height;
            if (w < 1) w = 1;
            if (w > 128) w = 128;
            if (h < 1) h = 1;
            if (h > 128) h = 128;
            return new Size(w, h);
        }

        /// <summary>
        /// Returns the encapsulating size when considering all of the symbol layers that make up this symbolizer.
        /// </summary>
        /// <returns>A Size2D</returns>
        public Size2D GetSize()
        {
            return Symbols.GetBoundingSize();
        }

        /// <summary>
        /// Multiplies all the linear measurements, like width, height, and offset values by the specified value.
        /// </summary>
        /// <param name="value">The double precision value to multiply all of the values against.</param>
        public void Scale(double value)
        {
            foreach (ISymbol symbol in Symbols)
            {
                symbol.Scale(value);
            }
        }

        /// <summary>
        /// Sets the color of the top-most layer symbol.
        /// </summary>
        /// <param name="color">The color to assign to the top-most layer.</param>
        public void SetFillColor(Color color)
        {
            if (Symbols == null) return;
            if (Symbols.Count == 0) return;

            Symbols[Symbols.Count - 1].SetColor(color);
        }

        /// <summary>
        /// Sets the outline, assuming that the symbolizer either supports outlines, or
        /// else by using a second symbol layer.
        /// </summary>
        /// <param name="outlineColor">The color of the outline</param>
        /// <param name="width">The width of the outline in pixels</param>
        public override void SetOutline(Color outlineColor, double width)
        {
            if (Symbols == null) return;
            if (Symbols.Count == 0) return;

            foreach (ISymbol symbol in Symbols)
            {
                IOutlinedSymbol oSymbol = symbol as IOutlinedSymbol;
                if (oSymbol == null) continue;

                oSymbol.OutlineColor = outlineColor;
                oSymbol.OutlineWidth = width;
                oSymbol.UseOutline = true;
            }

            base.SetOutline(outlineColor, width);
        }

        /// <summary>
        /// This assumes that you wish to simply scale the various sizes.
        /// It will adjust all of the sizes so that the maximum size is the same as the specified size.
        /// </summary>
        /// <param name="value">The Size2D of the new maximum size</param>
        public void SetSize(Size2D value)
        {
            Size2D oldSize = Symbols.GetBoundingSize();
            double dX = value.Width / oldSize.Width;
            double dY = value.Height / oldSize.Height;
            foreach (ISymbol symbol in Symbols)
            {
                Size2D os = symbol.Size;
                symbol.Size = new Size2D(os.Width * dX, os.Height * dY);
            }
        }

        /// <summary>
        /// This controls randomly creating a single random symbol from the symbol types, and randomizing it.
        /// </summary>
        /// <param name="generator">The generator used to create the random symbol.</param>
        protected override void OnRandomize(Random generator)
        {
            SymbolType type = generator.NextEnum<SymbolType>();
            Symbols.Clear();
            switch (type)
            {
                case SymbolType.Custom:
                    Symbols.Add(new SimpleSymbol());
                    break;
                case SymbolType.Character:
                    Symbols.Add(new CharacterSymbol());
                    break;
                case SymbolType.Picture:
                    Symbols.Add(new CharacterSymbol());
                    break;
                case SymbolType.Simple:
                    Symbols.Add(new SimpleSymbol());
                    break;
            }

            // This part will actually randomize the sub-member
            base.OnRandomize(generator);
        }

        private void Configure()
        {
            Symbols = new CopyList<ISymbol>();
            ISimpleSymbol ss = new SimpleSymbol();
            ss.Color = SymbologyGlobal.RandomColor();
            ss.Opacity = 1F;
            ss.PointShape = PointShape.Rectangle;
            Smoothing = true;
            ScaleMode = ScaleMode.Symbolic;
            Symbols.Add(ss);
        }

        #endregion
    }
}