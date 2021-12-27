// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A layer with drawing characteristics for LineStrings.
    /// </summary>
    public interface IPolygonLayer : IFeatureLayer
    {
        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the symbolizer describing the selection on the default category.
        /// </summary>
        new IPolygonSymbolizer SelectionSymbolizer { get; set; }

        /// <summary>
        /// Gets or sets the symbolizer describing the symbolizer on the default category.
        /// </summary>
        new IPolygonSymbolizer Symbolizer { get; set; }

        /// <summary>
        /// Gets or sets the polygon scheme that symbolically breaks down the drawing into symbol categories.
        /// </summary>
        new IPolygonScheme Symbology { get; set; }

        #endregion
    }
}