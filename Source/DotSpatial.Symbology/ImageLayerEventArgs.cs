// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// An EventArgs specifically tailored to ImageLayerEventArgs.
    /// </summary>
    public class ImageLayerEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageLayerEventArgs"/> class.
        /// </summary>
        /// <param name="imageLayer">The IImageLayer that is involved in this event.</param>
        public ImageLayerEventArgs(IImageLayer imageLayer)
        {
            ImageLayer = imageLayer;
        }

        /// <summary>
        /// Gets or sets the ImageLayer associated with this event.
        /// </summary>
        public IImageLayer ImageLayer { get; protected set; }
    }
}