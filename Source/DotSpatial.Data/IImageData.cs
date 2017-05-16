// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Drawing;

namespace DotSpatial.Data
{
    /// <summary>
    /// Interface for ImageData.
    /// </summary>
    public interface IImageData : IRasterBoundDataSet, IGetBitmap
    {
        #region Properties

        /// <summary>
        /// Gets or sets the interpretation for the image bands. This currently is only for GDAL images.
        /// </summary>
        ImageBandType BandType { get; set; }

        /// <summary>
        /// Gets or sets an integer indicating how many bytes exist for each pixel.
        /// Eg. 32 ARGB = 4, 24 RGB = 3, 16 bit GrayScale = 2
        /// </summary>
        int BytesPerPixel { get; set; }

        /// <summary>
        /// Gets or sets the image height in pixels.
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// Gets or sets the number of bands that are in the image.
        /// One band is a gray valued image, 3 bands for color RGB and 4 bands for ARGB.
        /// </summary>
        int NumBands { get; set; }

        /// <summary>
        /// Gets or sets the stride in bytes.
        /// </summary>
        int Stride { get; set; }

        /// <summary>
        /// Gets or sets a one dimensional array of byte values.
        /// </summary>
        byte[] Values { get; set; }

        /// <summary>
        /// Gets or sets the image width in pixels.
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// Gets or sets the world file that stores the georeferencing information for this image.
        /// </summary>
        WorldFile WorldFile { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Forces the image to read values from the graphic image format to the byte array format
        /// </summary>
        void CopyBitmapToValues();

        /// <summary>
        /// Copies the values from the specified source image.
        /// </summary>
        /// <param name="source">The source image to copy values from.</param>
        void CopyValues(IImageData source);

        /// <summary>
        /// Forces the image to copy values from the byte array format to the image format.
        /// </summary>
        void CopyValuesToBitmap();

        /// <summary>
        /// Creates a color structure from the byte values in the values array that correspond to the
        /// specified position.
        /// </summary>
        /// <param name="row">The integer row index for the pixel.</param>
        /// <param name="column">The integer column index for the pixel.</param>
        /// <returns>A Color.</returns>
        Color GetColor(int row, int column);

        /// <summary>
        /// This is only used in the palette indexed band type.
        /// </summary>
        /// <returns>The color palette.</returns>
        IEnumerable<Color> GetColorPalette();

        /// <summary>
        /// Opens the file, assuming that the fileName has already been specified
        /// </summary>
        void Open();

        /// <summary>
        /// Gets a block of data directly, converted into a bitmap.
        /// </summary>
        /// <param name="xOffset">The zero based integer column offset from the left</param>
        /// <param name="yOffset">The zero based integer row offset from the top</param>
        /// <param name="xSize">The integer number of pixel columns in the block. </param>
        /// <param name="ySize">The integer number of pixel rows in the block.</param>
        /// <returns>A Bitmap that is xSize, ySize.</returns>
        Bitmap ReadBlock(int xOffset, int yOffset, int xSize, int ySize);

        /// <summary>
        /// Saves the image and associated world file to the current fileName.
        /// </summary>
        void Save();

        /// <summary>
        /// Saves the image to a new fileName.
        /// </summary>
        /// <param name="fileName">The string fileName to save the image to.</param>
        void SaveAs(string fileName);

        /// <summary>
        /// Sets the color value into the byte array based on the row and column position of the pixel.
        /// </summary>
        /// <param name="row">The integer row index of the pixel to set the color of.</param>
        /// <param name="column">The integer column index of the pixel to set the color of</param>
        /// <param name="color">The color to copy values from</param>
        void SetColor(int row, int column, Color color);

        /// <summary>
        /// This should update the palette cached and in the file.
        /// </summary>
        /// <param name="value">The color palette.</param>
        void SetColorPalette(IEnumerable<Color> value);

        /// <summary>
        /// Finalizes the blocks. In the case of a pyramid image, this forces recalculation of the
        /// various overlays. For GDAL images, this may do nothing, since the overlay recalculation
        /// may be on the fly. For InRam images this does nothing.
        /// </summary>
        void UpdateOverviews();

        /// <summary>
        /// Saves a bitmap of data as a continuous block into the specified location.
        /// </summary>
        /// <param name="value">The bitmap value to save.</param>
        /// <param name="xOffset">The zero based integer column offset from the left</param>
        /// <param name="yOffset">The zero based integer row offset from the top</param>
        void WriteBlock(Bitmap value, int xOffset, int yOffset);

        #endregion
    }
}