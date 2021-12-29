// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Drawing;

namespace DotSpatial.Data
{
    /// <summary>
    /// IImageSet is simple interface that gives some basic information that is common between tiled images and
    /// the more general image coverages.
    /// </summary>
    public interface IImageSet : IDataSet
    {
        /// <summary>
        /// Gets the count of the images in the image set.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Uses the geographic envelope and the specified pixelSize in order to calculate an
        /// appropriate bitmap for display based on the various images in this set.
        /// </summary>
        /// <param name="envelope">The geographic bounds to display.</param>
        /// <param name="pixelSize">The pixelSize of the bitmap to display.</param>
        /// <returns>A Bitmap showing the appropriate size and dimensions of the image.</returns>
        Bitmap GetBitmap(Extent envelope, Size pixelSize);

        /// <summary>
        /// This is very generic, but allows the user to cycle through the images currently in the image set,
        /// regardless of whether they are in a list or an array or other data structure.
        /// </summary>
        /// <returns>The images of the image set.</returns>
        IEnumerable<IImageData> GetImages();
    }
}