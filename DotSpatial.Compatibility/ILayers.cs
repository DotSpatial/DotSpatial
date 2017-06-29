// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 11:25:15 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

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
        #region Methods

        /// <summary>
        /// Default add layer overload.  Displays an open file dialog
        /// </summary>
        /// <returns>An array of <c>DotSpatial.Interfaces.Layer</c> objects.</returns>
        ILayerOld[] Add();

        /// <summary>
        /// Adds a layer by fileName.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(string fileName);

        /// <summary>
        /// Adds a layer by fileName.  The layer name is also set.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(string fileName, string layerName);

        /// <summary>
        /// Adds an <c>Image</c> layer to the DotSpatial.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IImageData imageObject);

        /// <summary>
        /// Adds an <c>Image</c> layer to the DotSpatial with the specified layer name.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IImageData imageObject, string layerName);

        /// <summary>
        /// Adds a <c>FeatureSet</c> layer to the DotSpatial.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IFeatureSet shapefileObject);

        /// <summary>
        /// Adds a <c>FeatureSet</c> layer to the DotSpatial with the specified layer name.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IFeatureSet shapefileObject, string layerName);

        /// <summary>
        /// Adds a <c>FeatureSet</c> layer to the DotSpatial with the specified properties.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IFeatureSet shapefileObject, string layerName, int color, int outlineColor);

        /// <summary>
        /// Adds a <c>FeatureSet</c> layer to the DotSpatial with the specified properties.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IFeatureSet shapefileObject, string layerName, int color, int outlineColor, int lineOrPointSize);

        /// <summary>
        /// Adds a <c>Raster</c> layer to the DotSpatial.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IRaster gridObject);

        /// <summary>
        /// Adds a <c>Raster</c> layer to the DotSpatial, with the specified coloring scheme.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IRaster gridObject, IRasterSymbolizer colorScheme);

        /// <summary>
        /// Adds a <c>Raster</c> layer to the DotSpatial with the specified layer name.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IRaster gridObject, string layerName);

        /// <summary>
        /// Adds a <c>Rster</c> object to the DotSpatial with the specified properties.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(IRaster gridObject, IRasterSymbolizer colorScheme, string layerName);

        /// <summary>
        /// Adds a layer by fileName.  The layer name is also set, as well as the legend visibility.
        /// </summary>
        /// <returns>Returns a <c>Layer</c> object.</returns>
        ILayerOld Add(string fileName, string layerName, bool visibleInLegend);

        /// <summary>
        /// Adds a layer to the map, optionally placing it above the currently selected layer (otherwise at top of layer list).
        /// </summary>
        /// <param name="fileName">The name of the file to add.</param>
        /// <param name="layerName">The name of the layer, as displayed in the legend.</param>
        /// <param name="visible">Whether or not the layer is visible upon adding it.</param>
        /// <param name="placeAboveCurrentlySelected">Whether the layer should be placed above currently selected layer, or at top of layer list.</param>
        ILayerOld Add(string fileName, string layerName, bool visible, bool placeAboveCurrentlySelected);

        /// <summary>
        /// Removes all layers from the map.
        /// </summary>
        void Clear();

        /// <summary>
        /// Returns true if the layer handle specified belongs to a valid layer.  Drawing layers are not
        /// considered normal layers so this function will not return consistent results if a drawing
        /// layer is passed in.
        /// </summary>
        /// <param name="layerHandle">Handle of the layer to check.</param>
        /// <returns>True if the layer handle is valid.  False otherwise.</returns>
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
        /// <returns></returns>
        bool RebuildGridLayer(int layerHandle, IRaster gridObject, IRasterSymbolizer colorScheme);

        /// <summary>
        /// Removes the layer from the DotSpatial.
        /// </summary>
        /// <param name="layerHandle"></param>
        void Remove(int layerHandle);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current layer handle.
        /// </summary>
        int CurrentLayer { get; set; }

        /// <summary>
        /// Allows access to group properties in the legend.
        /// </summary>
        List<IGroup> Groups { get; }

        /// <summary>
        /// Returns the number of layers loaded in the DotSpatial.  Drawing layers are not counted.
        /// </summary>
        int NumLayers { get; }

        /// <summary>
        /// Returns the layer object corresponding the the specified <c>LayerHandle</c>
        /// </summary>
        ILayerOld this[int layerHandle] { get; }

        /// <summary>
        /// Finds a layer handle when given the global position of a layer.
        /// </summary>
        /// <param name="globalPosition">Position in the layers list, disregarding groups.</param>
        /// <returns>The handle of the layer at the specified position, or -1 if no layer is found there.</returns>
        int GetHandle(int globalPosition);

        #endregion
    }
}