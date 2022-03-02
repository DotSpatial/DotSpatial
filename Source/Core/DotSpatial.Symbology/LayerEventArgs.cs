// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Event args for events that need a layer.
    /// </summary>
    public class LayerEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerEventArgs"/> class.
        /// </summary>
        /// <param name="layer">The layer of the event.</param>
        public LayerEventArgs(ILayer layer)
        {
            Layer = layer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a layer.
        /// </summary>
        public ILayer Layer { get; protected set; }

        #endregion
    }
}