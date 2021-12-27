// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for Group.
    /// </summary>
    public interface IGroup : ILayer, IList<ILayer>
    {
        #region Events

        /// <summary>
        /// This occurs when a new layer is added either to this group, or one of the child groups within this group.
        /// </summary>
        event EventHandler<LayerEventArgs> LayerAdded;

        /// <summary>
        /// This occurs when a layer is removed from this group.
        /// </summary>
        event EventHandler<LayerEventArgs> LayerRemoved;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether or not the events are suspended on the layer collection.
        /// </summary>
        bool EventsSuspended { get; }

        /// <summary>
        /// Gets the integer handle for this group.
        /// </summary>
        int Handle { get; }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        Image Icon { get; set; }

        /// <summary>
        /// Gets the integer count of layers. This can also be accessed through Layers.Count.
        /// </summary>
        int LayerCount { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the layers within this group are visible.
        /// Note: When reading this property, it returns true if any layer is visible within
        /// this group.
        /// </summary>
        bool LayersVisible { get; set; }

        /// <summary>
        /// Gets the parent group of this group.
        /// </summary>
        IGroup ParentGroup { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this is locked. This property prevents the user from changing the visual state
        /// except layer by layer.
        /// </summary>
        bool StateLocked { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the number of data layers, not counting groups. If recursive is true, then layers that are within
        /// groups will be counted, even though the groups themselves are not.
        /// </summary>
        /// <param name="recursive">Boolean, if true forces checking even the number of child members.</param>
        /// <returns>An integer representing the total number of layers in this collection and its children.</returns>
        int GetLayerCount(bool recursive);

        /// <summary>
        /// Gets all feature layers of the map frame including feature layers which are nested
        /// within groups. The group objects themselves are not included in this list.
        /// </summary>
        /// <returns>The list of the feature layers.</returns>
        List<IFeatureLayer> GetAllFeatureLayers();

        /// <summary>
        /// Gets all map groups in the map including the nested groups.
        /// </summary>
        /// <returns>the list of the groups.</returns>
        List<IGroup> GetAllGroups();

        /// <summary>
        /// Gets all image layers of the map frame including image layers which are nested
        /// within groups. The group objects themselves are not included in this list.
        /// </summary>
        /// <returns>The list of the image layers.</returns>
        List<IImageLayer> GetAllImageLayers();

        /// <summary>
        /// Gets all layers of the map frame including layers which are nested
        /// within groups. The group objects themselves are not included in this list,
        /// but all FeatureLayers, RasterLayers, ImageLayers and other layers are included.
        /// </summary>
        /// <returns>The list of the layers.</returns>
        List<ILayer> GetAllLayers();

        /// <summary>
        /// Gets all line layers of the map frame including line layers which are nested
        /// within groups. The group objects themselves are not included in this list.
        /// </summary>
        /// <returns>The list of the line layers.</returns>
        List<ILineLayer> GetAllLineLayers();

        /// <summary>
        /// Gets all point layers of the map frame including point layers which are nested
        /// within groups. The group objects themselves are not included in this list.
        /// </summary>
        /// <returns>The list of the point layers.</returns>
        List<IPointLayer> GetAllPointLayers();

        /// <summary>
        /// Gets all polygon layers of the map frame including polygon layers which are nested
        /// within groups. The group objects themselves are not included in this list.
        /// </summary>
        /// <returns>The list of the polygon layers.</returns>
        List<IPolygonLayer> GetAllPolygonLayers();

        /// <summary>
        /// Gets all raster layers of the map frame including raster layers which are nested
        /// within groups. The group objects themselves are not included in this list.
        /// </summary>
        /// <returns>The list of the raster layers.</returns>
        List<IRasterLayer> GetAllRasterLayers();

        /// <summary>
        /// Gets the layers cast as ILayer without any information about the actual drawing methods.
        /// This is useful for handling methods that my come from various types of maps.
        /// </summary>
        /// <returns>An enumerable collection of ILayer.</returns>
        IList<ILayer> GetLayers();

        /// <summary>
        /// Gets the layer handle of the specified layer.
        /// </summary>
        /// <param name="positionInGroup">0 based index into list of layers.</param>
        /// <returns>Layer's handle on success, -1 on failure.</returns>
        int LayerHandle(int positionInGroup);

        /// <summary>
        /// Returns a snapshot image of this group.
        /// </summary>
        /// <param name="imgWidth">Width in pixels of the returned image (height is determined by the number of layers in the group).</param>
        /// <returns>Bitmap of the group and sublayers (expanded).</returns>
        Bitmap LegendSnapShot(int imgWidth);

        /// <summary>
        /// Resume events will resume events on the layers if all the suspensions are
        /// canceled out.
        /// </summary>
        void ResumeEvents();

        /// <summary>
        /// Adds one more increment of suspension which will prevent events from firing
        /// for the layers.
        /// </summary>
        void SuspendEvents();

        #endregion
    }
}