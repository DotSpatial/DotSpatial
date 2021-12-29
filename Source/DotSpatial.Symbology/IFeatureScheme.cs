// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Design;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for FeatureScheme.
    /// </summary>
    public interface IFeatureScheme : IScheme
    {
        #region Events

        /// <summary>
        /// Occurs when the deselect features context menu is clicked.
        /// </summary>
        event EventHandler<ExpressionEventArgs> DeselectFeatures;

        /// <summary>
        /// Occurs when a category indicates that its filter expression should be used
        /// to select its members.
        /// </summary>
        event EventHandler<ExpressionEventArgs> SelectFeatures;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not the legend should draw this item as a categorical
        /// tier in the legend. If so, it will allow the LegendText to be visible as a kind of group for the
        /// categories. If not, the categories will appear directly below the layer.C:\dev\Mapwindow6Dev\DotSpatial\DotSpatial\Drawing\Vectors\Points\PointCategoryCollection.cs.
        /// </summary>
        bool AppearsInLegend { get; set; }

        /// <summary>
        /// Gets or sets the dialog settings.
        /// </summary>
        new FeatureEditorSettings EditorSettings { get; set; }

        /// <summary>
        /// Gets the number of categories of this scheme.
        /// </summary>
        int NumCategories { get; }

        /// <summary>
        /// Gets the UITypeEditor to use for editing this FeatureScheme.
        /// </summary>
        /// <returns>The UITypeEditor.</returns>
        UITypeEditor PropertyEditor { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a list of all 'unique value' categories. This will use the "VectorEditorSettings"
        /// to determine how to create the values, so ensure that you update that property first.
        /// </summary>
        /// <param name="source">The attribute source that can provide the attribute information.</param>
        /// <param name="progressHandler">The progress handler for showing progress on what is likely a slow process.</param>
        void CreateCategories(IAttributeSource source, ICancelProgressHandler progressHandler);

        /// <summary>
        /// Creates the categories using the specified data table.
        /// </summary>
        /// <param name="table">Data table used for category creation.</param>
        void CreateCategories(DataTable table);

        /// <summary>
        /// Uses the settings on this scheme to create a random category.
        /// </summary>
        /// <returns>A new IFeatureCategory.</returns>
        /// <param name="filterExpression">The filter expression to use.</param>
        IFeatureCategory CreateRandomCategory(string filterExpression);

        /// <summary>
        /// When using this scheme to define the symbology, these individual categories will be referenced in order to
        /// create genuine categories (that will be cached).
        /// </summary>
        /// <returns>The categories.</returns>
        IEnumerable<IFeatureCategory> GetCategories();

        /// <summary>
        /// Queries this layer and the entire parental tree up to the map frame to determine if
        /// this layer is within the selected layers.
        /// </summary>
        /// <returns>True, if this is within legend selection.</returns>
        bool IsWithinLegendSelection();

        /// <summary>
        /// This keeps the existing categories, but uses the current scheme settings to apply
        /// a new color to each of the symbols.
        /// </summary>
        void RegenerateColors();

        #endregion
    }
}