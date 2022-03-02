// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// An Image Coverage just consists of several images that can be thought of as a single image region.
    /// Queries for pixel values for a region will simply return the first value in the set that is not
    /// completely transparent.
    /// </summary>
    public interface IImageCoverage : IImageSet
    {
        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the list of tiles.
        /// </summary>
        List<IImageData> Images { get; set; }

        #endregion
    }
}