// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Drawing;

namespace DotSpatial.Data
{
    /// <summary>
    /// ImageData (not named Image because of conflicting with the Dot Net Image object).
    /// </summary>
    public class ImageData : RasterBoundDataSet, IImageData
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageData"/> class.
        /// </summary>
        public ImageData()
        {
            WorldFile = new WorldFile();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the interpretation for the image bands. This currently is only for GDAL images.
        /// </summary>
        public ImageBandType BandType { get; set; }

        /// <summary>
        /// Gets or sets an integer indicating how many bytes exist for each pixel.
        /// Eg. 32 ARGB = 4, 24 RGB = 3, 16 bit GrayScale = 2.
        /// </summary>
        public int BytesPerPixel { get; set; }

        /// <summary>
        /// Gets or sets the image height in pixels.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the number of bands that are in the image. One band is a gray valued image, 3 bands for color RGB and 4 bands
        /// for ARGB.
        /// </summary>
        public int NumBands { get; set; }

        /// <summary>
        /// Gets or sets the stride in bytes.
        /// </summary>
        public int Stride { get; set; }

        /// <summary>
        /// Gets or sets a one dimensional array of byte values.
        /// </summary>
        public virtual byte[] Values { get; set; }

        /// <summary>
        /// Gets or sets the image width in pixels.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the world file that stores the georeferencing information for this image.
        /// </summary>
        public WorldFile WorldFile { get; set; }

        /// <summary>
        /// Gets or sets the color palette.
        /// </summary>
        protected IEnumerable<Color> ColorPalette { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Opens the file with the specified fileName.
        /// </summary>
        /// <param name="fileName">The string fileName to open.</param>
        /// <returns>ImageData of the opened file.</returns>
        public static IImageData Open(string fileName)
        {
            return DataManager.DefaultDataManager.OpenImage(fileName);
        }

        /// <summary>
        /// Forces the image to read values from the graphic image format to the byte array format.
        /// </summary>
        public virtual void CopyBitmapToValues()
        {
        }

        /// <summary>
        /// Copies the values from the specified source image.
        /// </summary>
        /// <param name="source">The source image to copy values from.</param>
        public virtual void CopyValues(IImageData source)
        {
        }

        /// <summary>
        /// Forces the image to copy values from the byte array format to the image format.
        /// </summary>
        public virtual void CopyValuesToBitmap()
        {
        }

        /// <summary>
        /// Creates a new image and world file, placing the default bounds at the origin, with one pixel per unit.
        /// </summary>
        /// <param name="fileName">The string fileName.</param>
        /// <param name="width">The integer width.</param>
        /// <param name="height">The integer height.</param>
        /// <param name="bandType">The color band type.</param>
        /// <returns>The create image data.</returns>
        public virtual IImageData Create(string fileName, int width, int height, ImageBandType bandType)
        {
            return DataManager.DefaultDataManager.CreateImage(fileName, height, width, bandType);
        }

        /// <summary>
        /// Attempts to create a bitmap for the entire image. This may cause memory exceptions.
        /// </summary>
        /// <returns>A Bitmap of the image.</returns>
        public virtual Bitmap GetBitmap()
        {
            return null;
        }

        /// <summary>
        /// The geographic envelope gives the region that the image should be created for.
        /// The window gives the corresponding pixel dimensions for the image, so that
        /// images matching the resolution of the screen can be used.
        /// </summary>
        /// <param name="envelope">The geographic extents to retrieve data for.</param>
        /// <param name="size">The rectangle that defines the size of the drawing area in pixels.</param>
        /// <returns>A bitmap captured from the main image. </returns>
        public Bitmap GetBitmap(Extent envelope, Size size)
        {
            return GetBitmap(envelope, new Rectangle(new Point(0, 0), size));
        }

        /// <summary>
        /// The geographic envelope gives the region that the image should be created for.
        /// The window gives the corresponding pixel dimensions for the image, so that
        /// images matching the resolution of the screen can be used.
        /// </summary>
        /// <param name="envelope">The geographic extents to retrieve data for.</param>
        /// <param name="window">The rectangle that defines the size of the drawing area in pixels.</param>
        /// <returns>A bitmap captured from the main image. </returns>
        public virtual Bitmap GetBitmap(Extent envelope, Rectangle window)
        {
            return null;
        }

        /// <summary>
        /// Creates a color structure from the byte values in the values array that correspond to the
        /// specified position.
        /// </summary>
        /// <param name="row">The integer row index for the pixel.</param>
        /// <param name="column">The integer column index for the pixel.</param>
        /// <returns>A Color.</returns>
        public virtual Color GetColor(int row, int column)
        {
            int bpp = BytesPerPixel;
            int strd = Stride;
            int b = Values[(row * strd) + (column * bpp)];
            int g = Values[(row * strd) + (column * bpp) + 1];
            int r = Values[(row * strd) + (column * bpp) + 2];
            int a = 255;
            if (BytesPerPixel == 4)
            {
                a = Values[(row * strd) + (column * bpp) + 3];
            }

            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// This is only used in the palette indexed band type.
        /// </summary>
        /// <returns>The color palette.</returns>
        public virtual IEnumerable<Color> GetColorPalette()
        {
            return ColorPalette;
        }

        /// <summary>
        /// Opens the file, assuming that the fileName has already been specified.
        /// </summary>
        public virtual void Open()
        {
            // todo: determine how this ImageData should wire itself to the returned value of OpenImage()
            DataManager.DefaultDataManager.OpenImage(Filename);
        }

        /// <summary>
        /// Gets a block of data directly, converted into a bitmap.
        /// </summary>
        /// <param name="xOffset">The zero based integer column offset from the left.</param>
        /// <param name="yOffset">The zero based integer row offset from the top.</param>
        /// <param name="xSize">The integer number of pixel columns in the block. </param>
        /// <param name="ySize">The integer number of pixel rows in the block.</param>
        /// <returns>A Bitmap that is xSize, ySize.</returns>
        public virtual Bitmap ReadBlock(int xOffset, int yOffset, int xSize, int ySize)
        {
            // Implemented in sub-classes.
            return null;
        }

        /// <summary>
        /// Saves the image and associated world file to the current fileName.
        /// </summary>
        public virtual void Save()
        {
        }

        /// <summary>
        /// Saves the image to a new fileName.
        /// </summary>
        /// <param name="fileName">The string fileName to save the image to.</param>
        public virtual void SaveAs(string fileName)
        {
            DataManager.DefaultDataManager.CreateImage(fileName, Height, Width, BandType);
        }

        /// <summary>
        /// Sets the color value into the byte array based on the row and column position of the pixel.
        /// </summary>
        /// <param name="row">The integer row index of the pixel to set the color of.</param>
        /// <param name="column">The integer column index of the pixel to set the color of. </param>
        /// <param name="col">The color to copy values from.</param>
        public virtual void SetColor(int row, int column, Color col)
        {
            int bpp = BytesPerPixel;
            int strd = Stride;
            Values[(row * strd) + (column * bpp)] = col.B;
            Values[(row * strd) + (column * bpp) + 1] = col.G;
            Values[(row * strd) + (column * bpp) + 2] = col.R;
            if (BytesPerPixel == 4)
            {
                Values[(row * strd) + (column * bpp) + 3] = col.A;
            }
        }

        /// <summary>
        /// This should update the palette cached and in the file.
        /// </summary>
        /// <param name="value">The color palette.</param>
        public virtual void SetColorPalette(IEnumerable<Color> value)
        {
            ColorPalette = value;
        }

        /// <summary>
        /// Finalizes the blocks. In the case of a pyramid image, this forces recalculation of the
        /// various overlays. For GDAL images, this may do nothing, since the overlay recalculation
        /// may be on the fly. For InRam images this does nothing.
        /// </summary>
        public virtual void UpdateOverviews()
        {
        }

        /// <summary>
        /// Saves a bitmap of data as a continuous block into the specified location.
        /// Be sure to call UpdateOverviews after writing all blocks in pyramid images.
        /// </summary>
        /// <param name="value">The bitmap value to save.</param>
        /// <param name="xOffset">The zero based integer column offset from the left.</param>
        /// <param name="yOffset">The zero based integer row offset from the top.</param>
        public virtual void WriteBlock(Bitmap value, int xOffset, int yOffset)
        {
            // Implemented in subclasses.
        }

        /// <summary>
        /// Disposes the managed memory objects in the ImageData class, and then forwards
        /// the Dispose operation to the internal dataset in the base class, if any.
        /// </summary>
        /// <param name="disposeManagedResources">Boolean, true if both managed and unmanaged resources should be finalized.</param>
        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
                WorldFile = null;
                Filename = null;
                Bounds = null;
                Values = null;
            }

            base.Dispose(disposeManagedResources);
        }

        /// <summary>
        /// Occurs when the bounds have been set.
        /// </summary>
        /// <param name="bounds">The new bounds.</param>
        protected override void OnBoundsChanged(IRasterBounds bounds)
        {
            // Bounds may be null when the image layer is being disposed.
            // We could probably skip calling this event in that case, but at any rate, we don't want to crash.
            if (WorldFile != null && bounds != null) WorldFile.Affine = bounds.AffineCoefficients;
        }

        #endregion
    }
}