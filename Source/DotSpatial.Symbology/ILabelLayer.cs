// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for LabelLayer.
    /// </summary>
    public interface ILabelLayer : ILayer
    {
        #region Properties

        /// <summary>
        /// Gets or sets an optional layer to link this layer to. If this is specified, then drawing will
        /// be associated with this layer.
        /// </summary>
        IFeatureLayer FeatureLayer { get; set; }

        /// <summary>
        /// Gets or sets the featureSet that defines the text for the labels on this layer.
        /// </summary>
        IFeatureSet FeatureSet { get; set; }

        /// <summary>
        /// Gets or sets the selection symbolizer from the first TextSymbol group.
        /// </summary>
        ILabelSymbolizer SelectionSymbolizer { get; set; }

        /// <summary>
        /// Gets or sets the regular symbolizer from the first TextSymbol group.
        /// </summary>
        ILabelSymbolizer Symbolizer { get; set; }

        /// <summary>
        /// Gets or sets the symbology
        /// </summary>
        ILabelScheme Symbology { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Clears the current selection, reverting the geometries back to their normal colors.
        /// </summary>
        void ClearSelection();

        /// <summary>
        /// Expression creates the labels based on the given string expression. Field names in
        /// square brackets will be replaced by the values for those fields in the FeatureSet.
        /// </summary>
        void CreateLabels();

        /// <summary>
        /// Invalidates any cached content for this layer.
        /// </summary>
        new void Invalidate();

        /// <summary>
        /// Highlights the values from a specified region. This will not unselect any members,
        /// so if you want to select a new region instead of an old one, first use ClearSelection.
        /// This is the default selection that only tests the anchor point, not the entire label.
        /// </summary>
        /// <param name="region">An Envelope showing a 3D selection box for intersection testing.</param>
        /// <returns>True if any members were added to the current selection.</returns>
        bool Select(Extent region);

        #endregion
    }
}