// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Event args for the LayerMoved event.
    /// </summary>
    public class LayerMovedEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerMovedEventArgs"/> class.
        /// </summary>
        /// <param name="layer">Layer that was moved.</param>
        /// <param name="newPosition">Position the layer was moved to.</param>
        public LayerMovedEventArgs(ILayer layer, int newPosition)
        {
            Layer = layer;
            NewPosition = newPosition;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the layer that was moved.
        /// </summary>
        public ILayer Layer { get; protected set; }

        /// <summary>
        /// Gets or sets the position the layer was moved to.
        /// </summary>
        public int NewPosition { get; protected set; }

        #endregion
    }
}