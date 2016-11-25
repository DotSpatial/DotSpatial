// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
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