// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using DotSpatial.NTSExtension;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// PointCategory
    /// </summary>
    [Serializable]
    public class PointCategory : FeatureCategory, IPointCategory
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PointCategory"/> class.
        /// </summary>
        public PointCategory()
        {
            Symbolizer = new PointSymbolizer();
            SelectionSymbolizer = new PointSymbolizer(true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointCategory"/> class where the geographic symbol size has been
        /// scaled to the specified extent.
        /// </summary>
        /// <param name="extent">The geographic extent that is 100 times wider than the geographic size of the points.</param>
        public PointCategory(IRectangle extent)
        {
            Symbolizer = new PointSymbolizer(false, extent);
            SelectionSymbolizer = new PointSymbolizer(true, extent);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointCategory"/> class based on a symbolizer.
        /// This uses the same symbolizer, but with a fill and border color of light cyan
        /// for the selection symbolizer.
        /// </summary>
        /// <param name="pointSymbolizer">The symbolizer to use in order to create a category</param>
        public PointCategory(IPointSymbolizer pointSymbolizer)
        {
            Symbolizer = pointSymbolizer;
            SelectionSymbolizer = pointSymbolizer.Copy();
            SelectionSymbolizer.SetFillColor(Color.Cyan);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointCategory"/> class where the symbolizer is based on the simple characteristics.
        /// The selection symbolizer has the same shape and size, but will be colored cyan.
        /// </summary>
        /// <param name="color">The color of the regular symbolizer</param>
        /// <param name="shape">The shape of the regular symbolizer</param>
        /// <param name="size">the size of the regular symbolizer</param>
        public PointCategory(Color color, PointShape shape, double size)
        {
            Symbolizer = new PointSymbolizer(color, shape, size);
            SelectionSymbolizer = new PointSymbolizer(Color.Cyan, shape, size);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointCategory"/> class based on the specified character.
        /// </summary>
        /// <param name="character">The character to use for the symbol</param>
        /// <param name="fontFamilyName">The font family name to use as the font</param>
        /// <param name="color">The color of the character</param>
        /// <param name="size">The size of the symbol</param>
        public PointCategory(char character, string fontFamilyName, Color color, double size)
        {
            Symbolizer = new PointSymbolizer(character, fontFamilyName, color, size);
            SelectionSymbolizer = new PointSymbolizer(character, fontFamilyName, Color.Cyan, size);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointCategory"/> class where the picture is used for the symbol,
        /// and a selected symbol is created as the same symbol but with a cyan border.
        /// </summary>
        /// <param name="picture">The image to use</param>
        /// <param name="size">The size of the symbol</param>
        public PointCategory(Image picture, double size)
        {
            Symbolizer = new PointSymbolizer(picture, size);
            PictureSymbol ps = new PictureSymbol(picture, size)
            {
                OutlineColor = Color.Cyan,
                OutlineWidth = 2,
                OutlineOpacity = 1f
            };
            SelectionSymbolizer = new PointSymbolizer(ps);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointCategory"/> class from the single symbol specified.
        /// If the symbol is colorable, the color of the selection symbol will be duplicated, but set to cyan.
        /// </summary>
        /// <param name="symbol">Symbol used for the category.</param>
        public PointCategory(ISymbol symbol)
        {
            Symbolizer = new PointSymbolizer(symbol);
            ISymbol copy = symbol.Copy();
            IColorable c = copy as IColorable;
            if (c != null)
            {
                c.Color = Color.Cyan;
            }

            SelectionSymbolizer = new PointSymbolizer(copy);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PointCategory"/> class from the list of symbols.
        /// </summary>
        /// <param name="symbols">Symbols used for this category.</param>
        public PointCategory(IEnumerable<ISymbol> symbols)
        {
            var symb = symbols as IList<ISymbol> ?? symbols.ToList();
            Symbolizer = new PointSymbolizer(symb);
            List<ISymbol> copy = symb.CloneList();

            if (copy.Any())
            {
                IColorable c = symb.Last() as IColorable;
                if (c != null)
                {
                    c.Color = Color.Cyan;
                }
            }

            SelectionSymbolizer = new PointSymbolizer(copy);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolizer to use to draw selected features from this category.
        /// </summary>
        [Description("Gets or sets the symbolizer to use to draw selected features from this category.")]
        public new IPointSymbolizer SelectionSymbolizer
        {
            get
            {
                return base.SelectionSymbolizer as IPointSymbolizer;
            }

            set
            {
                base.SelectionSymbolizer = value;
            }
        }

        /// <summary>
        /// Gets or sets the symbolizer for this category.
        /// </summary>
        [Description("Gets or sets the symbolizer for this category")]
        public new IPointSymbolizer Symbolizer
        {
            get
            {
                return base.Symbolizer as IPointSymbolizer;
            }

            set
            {
                base.Symbolizer = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This gets a single color that attempts to represent the specified
        /// category. For polygons, for example, this is the fill color (or central fill color)
        /// of the top pattern. If an image is being used, the color will be gray.
        /// </summary>
        /// <returns>The System.Color that can be used as an approximation to represent this category.</returns>
        public override Color GetColor()
        {
            if (Symbolizer?.Symbols == null || Symbolizer.Symbols.Count == 0) return Color.Gray;

            ISymbol p = Symbolizer.Symbols[0];
            return p.GetColor();
        }

        /// <summary>
        /// Gets the legend symbol size of the symbolizer for this category.
        /// </summary>
        /// <returns>The legend symbol size.</returns>
        public override Size GetLegendSymbolSize()
        {
            return Symbolizer.GetLegendSymbolSize();
        }

        /// <summary>
        /// Sets the Color of the top symbol in the symbols.
        /// </summary>
        /// <param name="color">The color to set the point.</param>
        public override void SetColor(Color color)
        {
            Symbolizer?.SetFillColor(color);
        }

        #endregion
    }
}