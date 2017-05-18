// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for ImageLayer.
    /// </summary>
    public interface IImageLayer : ILayer
    {
        /// <summary>
        /// Gets or sets a class that has some basic parameters that control how the image layer
        /// is drawn.
        /// </summary>
        IImageSymbolizer Symbolizer { get; set; }

        /// <summary>
        /// Gets or sets the dataset specifically as an IImageData object
        /// </summary>
        new IImageData DataSet { get; set; }

        /// <summary>
        /// Gets or sets the image being drawn by this layer
        /// </summary>
        IImageData Image { get; set; }
    }
}