// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for LayerEvents.
    /// </summary>
    public interface ILayerEvents : IChangeItem
    {
        /// <summary>
        /// Occurs when a layer is added to this item.
        /// </summary>
        event EventHandler<LayerEventArgs> LayerAdded;

        /// <summary>
        /// Occurs when a layer is moved.
        /// </summary>
        event EventHandler<LayerMovedEventArgs> LayerMoved;

        /// <summary>
        /// Occurs when a layer is removed from this item.
        /// </summary>
        event EventHandler<LayerEventArgs> LayerRemoved;

        /// <summary>
        /// Occurs when one of the layers in this collection changes visibility.
        /// </summary>
        event EventHandler LayerVisibleChanged;

        /// <summary>
        /// Zooms to a layer
        /// </summary>
        event EventHandler<EnvelopeArgs> ZoomToLayer;

        /// <summary>
        /// Occurs immediately after the layer is selected.
        /// </summary>
        event EventHandler<LayerSelectedEventArgs> LayerSelected;

        /// <summary>
        /// Occurs when the selection on a feature layer changes.
        /// </summary>
        event EventHandler<FeatureLayerSelectionEventArgs> SelectionChanging;

        /// <summary>
        /// Occurs after the selection has been updated for all the layers.
        /// </summary>
        event EventHandler SelectionChanged;

        /// <summary>
        /// Selects the layer to use for tools or operations that act on a single layer.
        /// </summary>
        /// <param name="index">The integer index indicating the layer to select in this collection.</param>
        void SelectLayer(int index);
    }
}