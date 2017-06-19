// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using GeoAPI.Geometries;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This supports the FeatureLayer event SelectionChanged, for both groups and feature layers, or
    /// any other layer that supports both an ISelection and inherits from IFeatureLayer.
    /// </summary>
    public interface ISelectable
    {
        #region Events

        /// <summary>
        /// Occurs after all of the layers have been updated with new selection content.
        /// </summary>
        event EventHandler SelectionChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this item is actively supporting selection.
        /// </summary>
        bool SelectionEnabled { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Removes any members from existing in the selected state.
        /// </summary>
        /// <param name="affectedArea">The affected area.</param>
        /// <param name="force">Indicates whether the selection should be cleared although SelectionEnabled is false.</param>
        /// <returns>Boolean, true if members were removed from the selection</returns>
        bool ClearSelection(out Envelope affectedArea, bool force);

        /// <summary>
        /// Inverts the selected state of any members in the specified region.
        /// </summary>
        /// <param name="tolerant">The geographic region to invert the selected state of members</param>
        /// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        /// <param name="mode">The selection mode determining how to test for intersection</param>
        /// <param name="affectedArea">The geographic region encapsulating the changed members</param>
        /// <returns>Boolean, true if members were changed by the selection process.</returns>
        bool InvertSelection(Envelope tolerant, Envelope strict, SelectionMode mode, out Envelope affectedArea);

        /// <summary>
        /// Adds any members found in the specified region to the selected state as long as
        /// SelectionEnabled is set to true.
        /// </summary>
        /// <param name="tolerant">The geographic region where selection occurs</param>
        /// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        /// <param name="mode">The selection mode</param>
        /// <param name="affectedArea">The envelope affected area</param>
        /// <returns>Boolean, true if any members were added to the selection</returns>
        bool Select(Envelope tolerant, Envelope strict, SelectionMode mode, out Envelope affectedArea);

        /// <summary>
        /// Removes any members found in the specified region from the selection
        /// </summary>
        /// <param name="tolerant">The geographic region to investigate</param>
        /// <param name="strict">The geographic region when working with absolutes, without a tolerance</param>
        /// <param name="mode">The selection mode to use for selecting items</param>
        /// <param name="affectedArea">The geographic region containing all the shapes that were altered</param>
        /// <returns>Boolean, true if any members were removed from the selection</returns>
        bool UnSelect(Envelope tolerant, Envelope strict, SelectionMode mode, out Envelope affectedArea);

        #endregion
    }
}