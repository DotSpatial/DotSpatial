// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;

using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Layers
    /// </summary>
    public interface ILayers
    {
        #region Properties

        /// <summary>
        /// Gets or sets the current layer handle.
        /// </summary>
        int CurrentLayer { get; set; }

        /// <summary>
        /// Gets access to group properties in the legend.
        /// </summary>
        List<IGroup> Groups { get; }

        /// <summary>
        /// Gets the number of layers loaded in the DotSpatial. Drawing layers are not counted.
        /// </summary>
        int NumLayers { get; }

        #endregion

        /// <summary>
        /// Returns the layer object corresponding the the specified <c>LayerHandle</c>
        /// </summary>
        /// <param name="layerHandle">Handle of the layer.</param>
        ILayerOld this[int layerHandle] { get; }

        #region Methods

        /// <summary>
        /// Default add layer overload. Displays an open file dialog
        /// </summary>
        /// <returns>An array of <c>DotSpatial.Interfaces.Layer</c> objects.</returns>
        ILayerOld[] Add();

        /// <summary>
        /// Adds a layer by fileName.
        /// </summary>
        /// <param name="fileName">Name of the file that should be added.</param>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(string fileName);

        /// <summary>
        /// Adds a layer by fileName. The layer name is also set.
        /// </summary>
        /// <param name="fileName">Name of the file that should be added.</param>
        /// <param name="layerName">Layername that gets assigned</param>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(string fileName, string layerName);

        /// <summary>
        /// Adds an <c>Image</c> layer to the DotSpatial.
        /// </summary>
        /// <param name="imageObject">The IImageData object that gets added.</param>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IImageData imageObject);

        /// <summary>
        /// Adds an <c>Image</c> layer to the DotSpatial with the specified layer name.
        /// </summary>
        /// <param name="imageObject">The IImageData object that gets added.</param>
        /// <param name="layerName">Layername that gets assigned</param>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IImageData imageObject, string layerName);

        /// <summary>
        /// Adds a <c>FeatureSet</c> layer to the DotSpatial.
        /// </summary>
        /// <param name="featuresetObject">The IFeatureSet object that gets added.</param>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IFeatureSet featuresetObject);

        /// <summary>
        /// Adds a <c>FeatureSet</c> layer to the DotSpatial with the specified layer name.
        /// </summary>
        /// <param name="featuresetObject">The IFeatureSet object that gets added.</param>
        /// <param name="layerName">Layername that gets assigned</param>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IFeatureSet featuresetObject, string layerName);

        /// <summary>
        /// Adds a <c>FeatureSet</c> layer to the DotSpatial with the specified properties.
        /// </summary>
        /// <param name="featuresetObject">The IFeatureSet object that gets added.</param>
        /// <param name="layerName">Layername that gets assigned</param>
        /// <param name="color">The fill color that gets assigned.</param>
        /// <param name="outlineColor">The outline color that gets assigned.</param>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IFeatureSet featuresetObject, string layerName, int color, int outlineColor);

        /// <summary>
        /// Adds a <c>FeatureSet</c> layer to the DotSpatial with the specified properties.
        /// </summary>
        /// <param name="featuresetObject">The IFeatureSet object that gets added.</param>
        /// <param name="layerName">Layername that gets assigned</param>
        /// <param name="color">The fill color that gets assigned.</param>
        /// <param name="outlineColor">The outline color that gets assigned.</param>
        /// <param name="lineOrPointSize">The size of the line or point.</param>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IFeatureSet featuresetObject, string layerName, int color, int outlineColor, int lineOrPointSize);

        /// <summary>
        /// Adds a <c>Raster</c> layer to the DotSpatial.
        /// </summary>
        /// <param name="gridObject">The IRaster that gets added.</param>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IRaster gridObject);

        /// <summary>
        /// Adds a <c>Raster</c> layer to the DotSpatial, with the specified coloring scheme.
        /// </summary>
        /// <param name="gridObject">The IRaster that gets added.</param>
        /// <param name="colorScheme">The color scheme used for drawing.</param>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IRaster gridObject, IRasterSymbolizer colorScheme);

        /// <summary>
        /// Adds a <c>Raster</c> layer to the DotSpatial with the specified layer name.
        /// </summary>
        /// <param name="gridObject">The IRaster that gets added.</param>
        /// <param name="layerName">Layername that gets assigned</param>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IRaster gridObject, string layerName);

        /// <summary>
        /// Adds a <c>Rster</c> object to the DotSpatial with the specified properties.
        /// </summary>
        /// <param name="gridObject">The IRaster that gets added.</param>
        /// <param name="colorScheme">The color scheme used for drawing.</param>
        /// <param name="layerName">Layername that gets assigned</param>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IRaster gridObject, IRasterSymbolizer colorScheme, string layerName);

        /// <summary>
        /// Adds a layer by fileName. The layer name is also set, as well as the legend visibility.
        /// </summary>
        /// <param name="fileName">Name of the file that should be added.</param>
        /// <param name="layerName">Layername that gets assigned</param>
        /// <param name="visibleInLegend">Indicates whether or not the layer is visible upon adding it.</param>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(string fileName, string layerName, bool visibleInLegend);

        /// <summary>
        /// Adds a layer to the map, optionally placing it above the currently selected layer (otherwise at top of layer list).
        /// </summary>
        /// <param name="fileName">The name of the file to add.</param>
        /// <param name="layerName">The name of the layer, as displayed in the legend.</param>
        /// <param name="visible">Whether or not the layer is visible upon adding it.</param>
        /// <param name="placeAboveCurrentlySelected">Whether the layer should be placed above currently selected layer, or at top of layer list.</param>
        /// <returns>The added layer.</returns>
        ILayerOld Add(string fileName, string layerName, bool visible, bool placeAboveCurrentlySelected);

        /// <summary>
        /// Removes all layers from the map.
        /// </summary>
        void Clear();

        /// <summary>
        /// Finds a layer handle when given the global position of a layer.
        /// </summary>
        /// <param name="globalPosition">Position in the layers list, disregarding groups.</param>
        /// <returns>The handle of the layer at the specified position, or -1 if no layer is found there.</returns>
        int GetHandle(int globalPosition);

        /// <summary>
        /// Returns true if the layer handle specified belongs to a valid layer. Drawing layers are not
        /// considered normal layers so this function will not return consistent results if a drawing
        /// layer is passed in.
        /// </summary>
        /// <param name="layerHandle">Handle of the layer to check.</param>
        /// <returns>True if the layer handle is valid. False otherwise.</returns>
        bool IsValidHandle(int layerHandle);

        /// <summary>
        /// Moves a layer to another position and/or group.
        /// </summary>
        /// <param name="handle">Handle of the layer to move.</param>
        /// <param name="newPosition">New position in the target group.</param>
        /// <param name="targetGroup">Group to move the layer to.</param>
        void MoveLayer(int handle, int newPosition, int targetGroup);

        /// <summary>
        /// Rebuilds a grid layer using the specified <c>GridColorScheme</c>
        /// </summary>
        /// <param name="layerHandle">Handle of the grid layer.</param>
        /// <param name="gridObject">Grid object corresponding to that layer handle.</param>
        /// <param name="colorScheme">Coloring scheme to use when rebuilding.</param>
        /// <returns>Bool if the function was successful.</returns>
        bool RebuildGridLayer(int layerHandle, IRaster gridObject, IRasterSymbolizer colorScheme);

        /// <summary>
        /// Removes the layer from the DotSpatial.
        /// </summary>
        /// <param name="layerHandle">Handle of the layer that should be removed.</param>
        void Remove(int layerHandle);

        #endregion
    }
}