// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Data
{
    /// <summary>
    /// DotNetImageProvider uses the standard image object to support basic image types through standard in-ram treatments.
    /// Images are not responsible for producing grid values that can be represented symbolically.
    /// </summary>
    public class DotNetImageProvider : IImageDataProvider
    {
        #region Properties

        /// <summary>
        /// Gets a basic description of what your provider does.
        /// </summary>
        public string Description => "A Dot Net Image object driven image handling system.";

        /// <summary>
        /// Gets a dialog read filter that lists each of the file type descriptions and file extensions, delimeted
        /// by the | symbol. Each will appear in DotSpatial's open file dialog filter, preceeded by the name provided
        /// on this object.
        /// </summary>
        public string DialogReadFilter => "Images|*.bmp;*.emf;*.exf;*.gif;*.ico;*.jpg;*.mbp;*.png;*.tif;*.wmf|Bitmap|*.bmp";

        /// <summary>
        /// Gets a dialog filter that lists each of the file type descriptions and extensions for a Save File Dialog.
        /// Each will appear in DotSpatial's open file dialog filter, preceeded by the name provided on this object.
        /// </summary>
        public string DialogWriteFilter => "Images|*.bmp;*.emf;*.exf;*.gif;*.ico;*.jpg;*.mbp;*.png;*.tif;*.wmf|Bitmap|*.bmp";

        /// <summary>
        /// Gets a prefereably short name that identifies this data provider. Example might be GDAL.
        /// This will be prepended to each of the DialogReadFilter members from this plugin.
        /// </summary>
        public string Name => "DotNet Image";

        /// <summary>
        /// Gets or sets the progress handler.
        /// </summary>
        public IProgressHandler ProgressHandler { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new instance of an Image.
        /// </summary>
        /// <param name="fileName">The string fileName to use.</param>
        /// <param name="width">The integer width in pixels.</param>
        /// <param name="height">The integer height in pixels.</param>
        /// <param name="inRam">Boolean, true if the entire contents should be stored in memory.</param>
        /// <param name="progHandler">A Progress handler to use.</param>
        /// <param name="band">.Net type ignores this for now.</param>
        /// <returns>
        /// A New IImageData object allowing access to the content of the image.
        /// </returns>
        public IImageData Create(string fileName, int width, int height, bool inRam, IProgressHandler progHandler, ImageBandType band)
        {
            InRamImageData img = new InRamImageData();
            img.Create(fileName, width, height, band);
            return img;
        }

        /// <summary>
        /// Opens a new Image with the specified fileName.
        /// </summary>
        /// <param name="fileName">The string file to open.</param>
        /// <returns>An IImageData object.</returns>
        public IImageData Open(string fileName)
        {
            InRamImageData data = new InRamImageData(fileName);
            return data;
        }

        /// <summary>
        /// Opens a new Image with the specified fileName.
        /// </summary>
        /// <param name="fileName">The string file to open.</param>
        /// <returns>An IDataSet object.</returns>
        IDataSet IDataProvider.Open(string fileName)
        {
            InRamImageData data = new InRamImageData(fileName);
            return data;
        }

        #endregion
    }
}