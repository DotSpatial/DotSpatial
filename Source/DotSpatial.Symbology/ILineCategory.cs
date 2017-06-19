// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for LineCategory.
    /// </summary>
    public interface ILineCategory : IFeatureCategory
    {
        #region Properties

        /// <summary>
        /// Gets or sets the symbolizer for this category
        /// </summary>
        new ILineSymbolizer Symbolizer { get; set; }

        /// <summary>
        /// Gets or sets the symbolizer to use to draw selected features from this category.
        /// </summary>
        new ILineSymbolizer SelectionSymbolizer { get; set; }

        #endregion
    }
}