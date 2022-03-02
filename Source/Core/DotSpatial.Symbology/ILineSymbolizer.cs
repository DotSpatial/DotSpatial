// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for LineSymbolizer.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public interface ILineSymbolizer : IFeatureSymbolizer
    {
        #region Properties

        /// <summary>
        /// Gets or sets the list of strokes that will be combined to make up a single drawing pass for this line.
        /// </summary>
        IList<IStroke> Strokes { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the line that is needed to show this lines legend symbol.
        /// </summary>
        /// <param name="g">The graphics device to draw to.</param>
        /// <param name="target">The rectangle that is used to calculate the lines position and size.</param>
        void DrawLegendSymbol(Graphics g, Rectangle target);

        /// <summary>
        /// Sequentially draws all of the strokes using the specified graphics path.
        /// </summary>
        /// <param name="g">The graphics device to draw to.</param>
        /// <param name="gp">The graphics path that describes the pathway to draw.</param>
        /// <param name="scaleWidth">The double scale width that when multiplied by the width gives a measure in pixels.</param>
        void DrawPath(Graphics g, GraphicsPath gp, double scaleWidth);

        /// <summary>
        /// Gets the color of the top-most stroke.
        /// </summary>
        /// <returns>The color of the top-most stroke.</returns>
        Color GetFillColor();

        /// <summary>
        /// This gets the largest width of all the strokes.
        /// Setting this will change the width of all the strokes to the specified width, and is not recommended
        /// if you are using thin lines drawn over thicker lines.
        /// </summary>
        /// <returns>The largest width of all the strokes.</returns>
        double GetWidth();

        /// <summary>
        /// Sets the fill color fo the top-most stroke, and forces the top-most stroke
        /// to be a type of stroke that can accept a fill color if necessary.
        /// </summary>
        /// <param name="fillColor">The new fill color.</param>
        void SetFillColor(Color fillColor);

        /// <summary>
        /// This forces the width to exist across all the strokes in this symbolizer.
        /// </summary>
        /// <param name="width">Width that gets set.</param>
        void SetWidth(double width);

        #endregion
    }
}