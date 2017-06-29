// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/23/2008 6:44:01 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Data
{
    /// <summary>
    /// IImageProvider
    /// </summary>
    public interface IImageDataProvider : IDataProvider
    {
        /// <summary>
        /// Creates a new instance of an Image.
        /// </summary>
        /// <param name="fileName">The string fileName to use</param>
        /// <param name="width">The integer width in pixels</param>
        /// <param name="height">The integer height in pixels</param>
        /// <param name="inRam">Boolean, true if the entire contents should be stored in memory</param>
        /// <param name="progHandler">A Progress handler to use</param>
        /// <param name="band">The ImageBandType clarifying how to organize the raster bands.</param>
        /// <returns>A New IImageData object allowing access to the content of the image</returns>
        IImageData Create(string fileName, int width, int height, bool inRam, IProgressHandler progHandler, ImageBandType band);

        /// <summary>
        /// Opens a new Image with the specified fileName
        /// </summary>
        /// <param name="fileName">The string file to open</param>
        /// <returns>An IImageData object</returns>
        new IImageData Open(string fileName);
    }
}