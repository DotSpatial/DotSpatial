// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/11/2009 1:08:21 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using DotSpatial.Data;
using DotSpatial.Serialization;
using DotSpatial.Topology;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PointSymbolizer
    /// </summary>
    public class PointSymbolizer : FeatureSymbolizer, IPointSymbolizer
    {
        #region Private Variables

        private IList<ISymbol> _symbols;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of PointSymbolizer
        /// </summary>
        public PointSymbolizer()
        {
            Configure();
        }

        /// <summary>
        /// Generates a new symbolizer with only one symbol.
        /// </summary>
        /// <param name="symbol">The symbol to use for creating this symbolizer</param>
        public PointSymbolizer(ISymbol symbol)
        {
            base.Smoothing = true;
            _symbols = new CopyList<ISymbol> { symbol };
        }

        /// <summary>
        /// Builds the new list of symbols from the symbols in the preset list or array of symbols.
        /// </summary>
        /// <param name="presetSymbols"></param>
        public PointSymbolizer(IEnumerable<ISymbol> presetSymbols)
        {
            base.Smoothing = true;
            _symbols = new CopyList<ISymbol>();
            foreach (ISymbol symbol in presetSymbols)
            {
                _symbols.Add(symbol);
            }
        }

        /// <summary>
        /// Creates a point symbolizer with one member, and that member is constructed
        /// based on the values specified.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="shape"></param>
        /// <param name="size"></param>
        public PointSymbolizer(Color color, PointShape shape, double size)
        {
            base.Smoothing = true;
            _symbols = new CopyList<ISymbol>();
            ISimpleSymbol ss = new SimpleSymbol(color, shape, size);
            _symbols.Add(ss);
        }

        /// <summary>
        /// Creates a point symbolizer with one memberw, and that member is constructed
        /// from the specified image.
        /// </summary>
        /// <param name="image">The image to use as a point symbol</param>
        /// <param name="size">The desired output size of the larger dimension of the image.</param>
        public PointSymbolizer(Image image, double size)
        {
            _symbols = new CopyList<ISymbol>();
            IPictureSymbol ps = new PictureSymbol(image);
            ps.Size = new Size2D(size, size);
            _symbols.Add(ps);
        }

        /// <summary>
        /// Creates a new point symbolizer that has a character symbol based on the specified characteristics.
        /// </summary>
        /// <param name="character">The character to draw</param>
        /// <param name="fontFamily">The font family to use for rendering the font</param>
        /// <param name="color">The font color</param>
        /// <param name="size">The size of the symbol</param>
        public PointSymbolizer(char character, string fontFamily, Color color, double size)
        {
            _symbols = new CopyList<ISymbol>();
            CharacterSymbol cs = new CharacterSymbol(character, fontFamily, color, size);
            _symbols.Add(cs);
        }

        /// <summary>
        /// Creates a new PointSymbolizer
        /// </summary>
        /// <param name="selected"></param>
        public PointSymbolizer(bool selected)
        {
            Configure();
            if (!selected) return;
            ISimpleSymbol ss = _symbols[0] as ISimpleSymbol;
            if (ss != null)
            {
                ss.Color = Color.Cyan;
            }
        }

        /// <summary>
        /// Sets the symbol type to geographic and generates a size that is 1/100 the width of the specified extents.
        /// </summary>
        /// <param name="selected"></param>
        /// <param name="extents"></param>
        public PointSymbolizer(bool selected, IRectangle extents)
        {
            Configure();

            base.ScaleMode = ScaleMode.Geographic;
            base.Smoothing = false;
            ISymbol s = _symbols[0];
            if (s == null) return;
            s.Size.Width = extents.Width / 100;
            s.Size.Height = extents.Width / 100;
            ISimpleSymbol ss = _symbols[0] as ISimpleSymbol;
            if (ss != null && selected) ss.Color = Color.Cyan;
        }

        private void Configure()
        {
            _symbols = new CopyList<ISymbol>();
            ISimpleSymbol ss = new SimpleSymbol();
            ss.Color = SymbologyGlobal.RandomColor();
            ss.Opacity = 1F;
            ss.PointShape = PointShape.Rectangle;
            Smoothing = true;
            ScaleMode = ScaleMode.Symbolic;
            _symbols.Add(ss);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the outline, assuming that the symbolizer either supports outlines, or
        /// else by using a second symbol layer.
        /// </summary>
        /// <param name="outlineColor">The color of the outline</param>
        /// <param name="width">The width of the outline in pixels</param>
        public override void SetOutline(Color outlineColor, double width)
        {
            if (_symbols == null) return;
            if (_symbols.Count == 0) return;
            foreach (ISymbol symbol in _symbols)
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
        /// Returns the encapsulating size when considering all of the symbol layers that make up this symbolizer.
        /// </summary>
        /// <returns>A Size2D</returns>
        public Size2D GetSize()
        {
            return _symbols.GetBoundingSize();
        }

        /// <summary>
        /// This assumes that you wish to simply scale the various sizes.
        /// It will adjust all of the sizes so that the maximum size is
        /// the same as the specified size.
        /// </summary>
        /// <param name="value">The Size2D of the new maximum size</param>
        public void SetSize(Size2D value)
        {
            Size2D oldSize = _symbols.GetBoundingSize();
            double dX = value.Width / oldSize.Width;
            double dY = value.Height / oldSize.Height;
            foreach (ISymbol symbol in _symbols)
            {
                Size2D os = symbol.Size;
                symbol.Size = new Size2D(os.Width * dX, os.Height * dY);
            }
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
        /// Returns the color of the top-most layer symbol
        /// </summary>
        /// <returns></returns>
        public Color GetFillColor()
        {
            if (_symbols == null) return Color.Empty;
            if (_symbols.Count == 0) return Color.Empty;
            IColorable c = _symbols[_symbols.Count - 1] as IColorable;
            return c == null ? Color.Empty : c.Color;
        }

        /// <summary>
        /// Sets the color of the top-most layer symbol
        /// </summary>
        /// <param name="color">The color to assign to the top-most layer.</param>
        public void SetFillColor(Color color)
        {
            if (_symbols == null) return;
            if (_symbols.Count == 0) return;
            _symbols[_symbols.Count - 1].SetColor(color);
        }

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
            shift.Translate(target.X + target.Width / 2, target.Y + target.Height / 2);
            g.Transform = shift;
            Draw(g, scale);
            g.Transform = old;
        }

        /// <summary>
        /// Draws the point symbol to the specified graphics object by cycling through each of the layers and
        /// drawing the content.  This assumes that the graphics object has been translated to the specified point.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="scaleSize">Scale size represents the constant to multiply to the geographic measures in order to turn them into pixel coordinates </param>
        public void Draw(Graphics g, double scaleSize)
        {
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
            foreach (ISymbol symbol in _symbols)
            {
                symbol.Draw(g, scaleSize);
            }
        }

        /// <summary>
        /// Multiplies all the linear measurements, like width, height, and offset values by the specified value.
        /// </summary>
        /// <param name="value">The double precision value to multiply all of the values against.</param>
        public void Scale(double value)
        {
            foreach (ISymbol symbol in _symbols)
            {
                symbol.Scale(value);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the set of layered symbols.  The symbol with the highest index is drawn on top.
        /// </summary>
        [Serialize("Symbols")]
        public IList<ISymbol> Symbols
        {
            get { return _symbols; }
            set { _symbols = value; }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// This controls randomly creating a single random symbol from the symbol types, and randomizing it.
        /// </summary>
        /// <param name="generator"></param>
        protected override void OnRandomize(Random generator)
        {
            SymbolType type = generator.NextEnum<SymbolType>();
            _symbols.Clear();
            switch (type)
            {
                case SymbolType.Custom: _symbols.Add(new SimpleSymbol()); break;
                case SymbolType.Character: _symbols.Add(new CharacterSymbol()); break;
                case SymbolType.Picture: _symbols.Add(new CharacterSymbol()); break;
                case SymbolType.Simple: _symbols.Add(new SimpleSymbol()); break;
            }
            // This part will actually randomize the sub-member
            base.OnRandomize(generator);
        }

        #endregion
    }
}