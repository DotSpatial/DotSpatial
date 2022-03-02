// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for PolygonSymbolizer.
    /// </summary>
    public interface IPolygonSymbolizer : IFeatureSymbolizer
    {
        #region Properties

        /// <summary>
        /// Gets or sets the method for drawing the lines that make up the borders of this polygon.
        /// </summary>
        ILineSymbolizer OutlineSymbolizer { get; set; }

        /// <summary>
        /// gets or sets the list of patterns to use for filling polygons.
        /// </summary>
        IList<IPattern> Patterns { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the color of the top-most pattern, if it is a simple pattern,
        /// or return Color.Empty otherwise.
        /// </summary>
        /// <returns>The fill color.</returns>
        Color GetFillColor();

        /// <summary>
        /// This gets the largest width of all the strokes of the outlines of all the patterns. Setting this will
        /// forceably adjust the width of all the strokes of the outlines of all the patterns.
        /// </summary>
        /// <returns>The outline width.</returns>
        double GetOutlineWidth();

        /// <summary>
        /// Sets the color, forcing a simple pattern if necessary.
        /// </summary>
        /// <param name="color">Gets the color of the top-most pattern.</param>
        void SetFillColor(Color color);

        /// <summary>
        /// Forces the specified width to be the width of every stroke outlining every pattern.
        /// </summary>
        /// <param name="width">The width to force as the outline width.</param>
        void SetOutlineWidth(double width);

        #endregion
    }
}