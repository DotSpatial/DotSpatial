// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/8/2010 10:24:50 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.Drawing;

namespace DotSpatial.Data
{
    /// <summary>
    /// IImageSet is simple interface that gives some basic information that is common between tiled images and
    /// the more general image coverages
    /// </summary>
    public interface IImageSet : IDataSet
    {
        /// <summary>
        /// Gets the count of the images in the image set
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Uses the geographic envelope and the specified pixelSize in order to calculate an
        /// appropriate bitmap for display based on the various images in this set.
        /// </summary>
        /// <param name="envelope">The geographic bounds to display</param>
        /// <param name="pixelSize">The pixelSize of the bitmap to display</param>
        /// <returns>A Bitmap showing the appropriate size and dimensions of the image</returns>
        Bitmap GetBitmap(Extent envelope, Size pixelSize);

        /// <summary>
        /// This is very generic, but allows the user to cycle through the images currently in the image set,
        /// regardless of whether they are in a list or an array or other data structure.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IImageData> GetImages();
    }
}