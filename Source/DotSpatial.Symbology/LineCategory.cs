// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using DotSpatial.Serialization;
using NetTopologySuite.Geometries;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LineCategory.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class LineCategory : FeatureCategory, ILineCategory
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LineCategory"/> class.
        /// </summary>
        public LineCategory()
        {
            Symbolizer = new LineSymbolizer();
            SelectionSymbolizer = new LineSymbolizer(true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineCategory"/> class. Since a compound
        /// pen is used, it is possible to use this to create a transparent line with just two border parts.
        /// The selection symbolizer will be dark cyan bordering light cyan, but use the same dash and cap
        /// patterns.
        /// </summary>
        /// <param name="fillColor">The fill color for the line.</param>
        /// <param name="borderColor">The border color of the line.</param>
        /// <param name="width">The width of the entire line.</param>
        /// <param name="dash">The dash pattern to use.</param>
        /// <param name="caps">The style of the start and end caps.</param>
        public LineCategory(Color fillColor, Color borderColor, double width, DashStyle dash, LineCap caps)
        {
            Symbolizer = new LineSymbolizer(fillColor, borderColor, width, dash, caps);
            SelectionSymbolizer = new LineSymbolizer(Color.Cyan, Color.DarkCyan, width, dash, caps);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineCategory"/> class with the specified color and width.
        /// </summary>
        /// <param name="color">The color of the unselected line.</param>
        /// <param name="width">The width of both the selected and unselected lines.</param>
        public LineCategory(Color color, double width)
        {
            Symbolizer = new LineSymbolizer(color, width);
            SelectionSymbolizer = new LineSymbolizer(Color.Cyan, width);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineCategory"/> class where the geographic symbol size has been
        /// scaled to the specified extent.
        /// </summary>
        /// <param name="extent">The geographic extent that is 100 times wider than the geographic size of the points.</param>
        public LineCategory(Envelope extent)
        {
            Symbolizer = new LineSymbolizer(extent, false);
            SelectionSymbolizer = new LineSymbolizer(extent, true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LineCategory"/> class based on a symbolizer.
        /// This uses the same symbolizer, but with a fill and border color of light cyan for the selection symbolizer.
        /// </summary>
        /// <param name="lineSymbolizer">The symbolizer to use in order to create a category.</param>
        public LineCategory(ILineSymbolizer lineSymbolizer)
        {
            Symbolizer = lineSymbolizer;
            ILineSymbolizer select = lineSymbolizer.Copy();
            SelectionSymbolizer = select;
            if (select.Strokes != null && select.Strokes.Count > 0)
            {
                ISimpleStroke ss = select.Strokes[select.Strokes.Count - 1] as ISimpleStroke;
                if (ss != null) ss.Color = Color.Cyan;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolizer to use to draw selected features from this category.
        /// </summary>
        public new ILineSymbolizer SelectionSymbolizer
        {
            get
            {
                return base.SelectionSymbolizer as ILineSymbolizer;
            }

            set
            {
                base.SelectionSymbolizer = value;
            }
        }

        /// <summary>
        /// Gets or sets the symbolizer for this category.
        /// </summary>
        public new ILineSymbolizer Symbolizer
        {
            get
            {
                return base.Symbolizer as ILineSymbolizer;
            }

            set
            {
                base.Symbolizer = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// This gets a single color that attempts to represent the specified category.
        /// For polygons, for example, this is the fill color (or central fill color)
        /// of the top pattern. If an image is being used, the color will be gray.
        /// </summary>
        /// <returns>The System.Color that can be used as an approximation to represent this category.</returns>
        public override Color GetColor()
        {
            if (Symbolizer?.Strokes == null || Symbolizer.Strokes.Count == 0) return Color.Gray;
            IStroke p = Symbolizer.Strokes[0];
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
        /// Controls what happens in the legend when this item is instructed to draw a symbol.
        /// </summary>
        /// <param name="g">The graphics object used for drawing.</param>
        /// <param name="box">The rectangle used for drawing.</param>
        public override void LegendSymbolPainted(Graphics g, Rectangle box)
        {
            Symbolizer.DrawLegendSymbol(g, box);
        }

        /// <summary>
        /// Sets the specified color as the color for the top most stroke.
        /// </summary>
        /// <param name="color">The color to apply.</param>
        public override void SetColor(Color color)
        {
            if (Symbolizer?.Strokes == null || Symbolizer.Strokes.Count == 0) return;
            Symbolizer.Strokes[0].SetColor(color);
        }

        #endregion
    }
}