// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// This interface stores a single extent window describing a view, and also contains
    /// the list of all the layers associated with that view. The layers are ordered.
    /// </summary>
    public interface IFrame : IGroup
    {
        #region Events

        /// <summary>
        /// Occurs when some items should no longer render, and the map needs a refresh.
        /// </summary>
        event EventHandler UpdateMap;

        /// <summary>
        /// Occurs after zooming to a specific location on the map and causes a camera recent.
        /// </summary>
        event EventHandler<ExtentArgs> ViewExtentsChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not the visibility of the children of a group are
        /// triggered by the checked status of the group. If this is true then this will force all the layers
        /// in this group to become visible. In other words, checking a group to ON will programatically check
        /// all the children layers to ON as well. if this set to false then the visibility of each layer is
        /// dependent on the status of every one of its parent group. In other words if a child layer is checked
        /// to ON it will be displayed only if every parent group it is a part of is also checked to ON.
        /// </summary>
        bool AutoDisplayGroupChildren { get; set; }

        /// <summary>
        /// Gets or sets the drawing layers. Drawing layers are tracked separately, and. do not appear in the legend.
        /// </summary>
        List<ILayer> DrawingLayers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not a newly added layer
        /// will also force a zoom to that layer. If this is true, then nothing
        /// will happen. Otherwise, adding layers to this frame or a group in this
        /// frame will set the extent.
        /// </summary>
        bool ExtentsInitialized { get; set; }

        /// <summary>
        /// Gets the currently active layer.
        /// </summary>
        ILayer SelectedLayer { get; }

        /// <summary>
        /// Gets or sets the smoothing mode. Default or None will have faster performance
        /// at the cost of quality.
        /// </summary>
        SmoothingMode SmoothingMode { get; set; }

        /// <summary>
        /// Gets or sets the geographic envelope in view.
        /// </summary>
        Extent ViewExtents { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// This will create a new layer from the featureset and add it.
        /// </summary>
        /// <param name="featureSet">Any valid IFeatureSet that does not yet have drawing characteristics.</param>
        void Add(IFeatureSet featureSet);

        #endregion
    }
}