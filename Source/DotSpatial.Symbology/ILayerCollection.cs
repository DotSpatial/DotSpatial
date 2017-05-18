// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Generic interface for collection with <see cref="ILayer"/> elements.
    /// </summary>
    public interface ILayerCollection : ILayerEventList<ILayer>, IDisposable, IDisposeLock
    {
        /// <summary>
        /// Gets or sets the ParentGroup for this layer collection, even if that parent group
        /// is not actually a map frame.
        /// </summary>
        IGroup ParentGroup { get; set; }

        /// <summary>
        /// Gets or sets the MapFrame for this layer collection.
        /// </summary>
        IFrame MapFrame { get; set; }

        /// <summary>
        /// Gets or sets the currently active layer.
        /// </summary>
        ILayer SelectedLayer { get; set; }
    }
}