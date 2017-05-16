// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for FeatureCategory.
    /// </summary>
    public interface IFeatureCategory : ICategory
    {
        #region Events

        /// <summary>
        /// Occurs when the deselect features context menu is clicked.
        /// </summary>
        event EventHandler<ExpressionEventArgs> DeselectFeatures;

        /// <summary>
        /// Occurs when the select features context menu is clicked.
        /// </summary>
        event EventHandler<ExpressionEventArgs> SelectFeatures;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the filter expression that is used to add members to generate a category based on this scheme.
        /// </summary>
        string FilterExpression { get; set; }

        /// <summary>
        /// Gets or sets the symbolizer used for this category.
        /// </summary>
        IFeatureSymbolizer SelectionSymbolizer { get; set; }

        /// <summary>
        /// Gets or sets the symbolizer used for this category.
        /// </summary>
        IFeatureSymbolizer Symbolizer { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// In some cases, it is useful to simply be able to show an approximation of the actual expression.
        /// This also removes brackets from the field names to make it slightly cleaner.
        /// </summary>
        void DisplayExpression();

        /// <summary>
        /// This gets a single color that attempts to represent the specified
        /// category. For polygons, for example, this is the fill color (or central fill color)
        /// of the top pattern. If an image is being used, the color will be gray.
        /// </summary>
        /// <returns>The System.Color that can be used as an approximation to represent this category.</returns>
        Color GetColor();

        /// <summary>
        /// Queries this layer and the entire parental tree up to the map frame to determine if
        /// this layer is within the selected layers.
        /// </summary>
        /// <returns>True, if this item is within legend selection.</returns>
        bool IsWithinLegendSelection();

        /// <summary>
        /// This applies the color to the top symbol stroke or pattern.
        /// </summary>
        /// <param name="color">The Color to apply</param>
        void SetColor(Color color);

        #endregion
    }
}