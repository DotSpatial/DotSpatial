// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// A layer with drawing characteristics for LineStrings.
    /// </summary>
    public interface ILineLayer : IFeatureLayer
    {
        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the drawing characteristics to use for this layer.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Unable to assign a non-point symbolizer to a PointLayer.</exception>
        new ILineSymbolizer Symbolizer { get; set; }

        /// <summary>
        /// Gets or sets the LineSymbolizer to use for selected properties.
        /// </summary>
        new ILineSymbolizer SelectionSymbolizer { get; set; }

        /// <summary>
        /// Gets or sets the categorical scheme for categorical drawing.
        /// </summary>
        new ILineScheme Symbology { get; set; }

        #endregion
    }
}