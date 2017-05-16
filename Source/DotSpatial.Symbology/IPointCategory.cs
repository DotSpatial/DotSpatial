// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for PointCategory.
    /// </summary>
    public interface IPointCategory : IFeatureCategory
    {
        #region Properties

        /// <summary>
        /// Gets or sets the symbolizer to use to draw selected features from this category.
        /// </summary>
        new IPointSymbolizer SelectionSymbolizer { get; set; }

        /// <summary>
        /// Gets or sets the symbolizer for this category.
        /// </summary>
        new IPointSymbolizer Symbolizer { get; set; }

        #endregion
    }
}