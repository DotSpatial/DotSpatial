// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/24/2009 12:39:08 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ILayerEvents
    /// </summary>
    public interface ILayerEvents : IChangeItem
    {
        /// <summary>
        /// Occurs when a layer is added to this item.
        /// </summary>
        event EventHandler<LayerEventArgs> LayerAdded;

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