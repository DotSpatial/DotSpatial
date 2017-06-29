// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
// The Original Code is from MapWindow.dll version 6.0
// The Initial Developer of this Original Code is Ted Dunsford. Created 12/9/2008 2:43:38 PM
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace DotSpatial.Data
{
    /// <summary>
    /// MWImageData
    /// </summary>
    public class InRamImageData : ImageData
    {
        /// <summary>
        /// The _my image.
        /// </summary>
        private Bitmap _myImage;

        /// <summary>
        /// Initializes a new instance of the <see cref="InRamImageData"/> class.
        /// Creates an empty ImageData to be created or loaded
        /// </summary>
        public InRamImageData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InRamImageData"/> class.
        /// Creates a new MWImageData class from the specified fileName.
        /// </summary>
        /// <param name="fileName">
        /// The string filename.
        /// </param>
        public InRamImageData(string fileName)
        {
            Filename = fileName;
            Open();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InRamImageData"/> class.
        /// Creates the bitmap from the raw image specified.  The bounds should be set on this later.
        /// </summary>
        /// <param name="rawImage">
        /// The raw image.
        /// </param>
        public InRamImageData(Image rawImage)
        {
            _myImage = new Bitmap(rawImage.Width, rawImage.Height);
            Width = rawImage.Width;
            Height = rawImage.Height;
            Graphics g = Graphics.FromImage(_myImage);
            g.DrawImageUnscaled(rawImage, 0, 0);
            g.Dispose();
            WorldFile = new WorldFile();
            double[] aff = new[] { .5, 1.0, 0, _myImage.Height - .5, 0, -1.0 };
            Bounds = new RasterBounds(_myImage.Height, _myImage.Width, aff);
            MemorySetup();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InRamImageData"/> class.
        /// Uses a bitmap and a geographic envelope in order to define a new imageData object.
        /// </summary>
        /// <param name="rawImage">
        /// The raw image.
        /// </param>
        /// <param name="bounds">
        /// The envelope bounds.
        /// </param>
        public InRamImageData(Bitmap rawImage, Extent bounds)
        {
            _myImage = rawImage;
            Width = _myImage.Width;
            Height = _myImage.Height;
            Bounds = new RasterBounds(_myImage.Height, _myImage.Width, bounds);
            MemorySetup();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InRamImageData"/> class.
        /// Constructs a new ImageData of the specified width and height.
        /// </summary>
        /// <param name="width">
        /// The integer width in pixels.
        /// </param>
        /// <param name="height">
        /// The integer height in pixels.
        /// </param>
        public InRamImageData(int width, int height)
        {
            WorldFile = new WorldFile();
            double[] aff = new[] { .5, 1.0, 0, _myImage.Height - .5, 0, -1.0 };
            Bounds = new RasterBounds(height, width, aff);
            _myImage = new Bitmap(width, height);
            Width = width;
            Height = height;
            MemorySetup();
        }

        // Makes sure that the content of the image is reflected in bytes.

        /// <summary>
        /// Closes the image content.
        /// </summary>
        public override void Close()
        {
            // This doesn't need to do anything
        }

        /// <summary>
        /// Forces the image to read values from the graphic image format to the byte array format
        /// </summary>
        public override void CopyBitmapToValues()
        {
            Rectangle bnds = new Rectangle(0, 0, Width, Height);
            BitmapData bData = null;
            if (NumBands == 4)
            {
                bData = _myImage.LockBits(bnds, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            }

            if (NumBands == 3)
            {
                bData = _myImage.LockBits(bnds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            }

            if (NumBands == 1)
            {
                bData = _myImage.LockBits(bnds, ImageLockMode.ReadWrite, PixelFormat.Format16bppGrayScale);
            }

            if (bData == null)
            {
                throw new ApplicationException("The specified number of bands is not supported.");
            }

            Stride = bData.Stride;

            Values = new byte[bData.Height * bData.Stride];
            Marshal.Copy(bData.Scan0, Values, 0, bData.Height * bData.Stride);
            _myImage.UnlockBits(bData);
        }

        /// <summary>
        /// Copies the values from the specified source image.
        /// </summary>
        /// <param name="source">
        /// The source image to copy values from.
        /// </param>
        public override void CopyValues(IImageData source)
        {
            Values = (byte[])source.Values.Clone();
            NumBands = source.NumBands;
            BytesPerPixel = source.BytesPerPixel;
        }

        /// <summary>
        /// Forces the image to copy values from the byte array format to the image format.
        /// </summary>
        public override void CopyValuesToBitmap()
        {
            Rectangle bnds = new Rectangle(0, 0, Width, Height);
            BitmapData bData = null;
            if (NumBands == 4)
            {
                bData = _myImage.LockBits(bnds, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            }

            if (NumBands == 3)
            {
                bData = _myImage.LockBits(bnds, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            }

            if (NumBands == 1)
            {
                bData = _myImage.LockBits(bnds, ImageLockMode.ReadWrite, PixelFormat.Format16bppGrayScale);
            }

            if (bData == null)
            {
                throw new ApplicationException("The specified number of bands is not supported.");
            }

            Marshal.Copy(Values, 0, bData.Scan0, Values.Length);
            _myImage.UnlockBits(bData);
        }

        /// <summary>
        /// Creates a new image and world file, placing the default bounds at the origin, with one pixel per unit.
        /// </summary>
        /// <param name="fileName">
        /// The string fileName
        /// </param>
        /// <param name="width">
        /// The integer width
        /// </param>
        /// <param name="height">
        /// The integer height
        /// </param>
        /// <param name="bandType">The ImageBandType that clarifies how the separate bands are layered in the image.</param>
        public override IImageData Create(string fileName, int width, int height, ImageBandType bandType)
        {
            Filename = fileName;
            WorldFile = new WorldFile();
            double[] aff = new[] { 1.0, 0, 0, -1.0, 0, height };
            Bounds = new RasterBounds(height, width, aff);
            WorldFile.Filename = WorldFile.GenerateFilename(fileName);
            Width = width;
            Height = height;
            _myImage = new Bitmap(width, height);
            string ext = Path.GetExtension(fileName);
            switch (ext)
            {
                case ".bmp":
                    _myImage.Save(fileName, ImageFormat.Bmp);
                    break;
                case ".emf":
                    _myImage.Save(fileName, ImageFormat.Emf);
                    break;
                case ".exf":
                    _myImage.Save(fileName, ImageFormat.Exif);
                    break;
                case ".gif":
                    _myImage.Save(fileName, ImageFormat.Gif);
                    break;
                case ".ico":
                    _myImage.Save(fileName, ImageFormat.Icon);
                    break;
                case ".jpg":
                    _myImage.Save(fileName, ImageFormat.Jpeg);
                    break;
                case ".mbp":
                    _myImage.Save(fileName, ImageFormat.MemoryBmp);
                    break;
                case ".png":
                    _myImage.Save(fileName, ImageFormat.Png);
                    break;
                case ".tif":
                    _myImage.Save(fileName, ImageFormat.Tiff);
                    break;
                case ".wmf":
                    _myImage.Save(fileName, ImageFormat.Wmf);
                    break;
            }

            NumBands = 4;
            BytesPerPixel = 4;
            CopyBitmapToValues();

            return this;
        }

        /// <summary>
        /// Returns the internal bitmap in this case.  In other cases, this may have to be constructed
        /// from the unmanaged memory content.
        /// </summary>
        /// <returns>
        /// A Bitmap that represents the entire image.
        /// </returns>
        public override Bitmap GetBitmap()
        {
            return _myImage;
        }

        /// <summary>
        /// The geographic envelope gives the region that the image should be created for.
        /// The window gives the corresponding pixel dimensions for the image, so that
        /// images matching the resolution of the screen can be used.
        /// </summary>
        /// <param name="envelope">
        /// The geographic extents to retrieve data for
        /// </param>
        /// <param name="window">
        /// The rectangle that defines the size of the drawing area in pixels
        /// </param>
        /// <returns>
        /// A bitmap captured from the main image
        /// </returns>
        public override Bitmap GetBitmap(Extent envelope, Rectangle window)
        {
            if (window.Width == 0 || window.Height == 0)
            {
                return null;
            }

            Bitmap result = new Bitmap(window.Width, window.Height);
            Graphics g = Graphics.FromImage(result);

            //

            // Gets the scaling factor for converting from geographic to pixel coordinates
            double dx = (window.Width / envelope.Width);
            double dy = (window.Height / envelope.Height);

            double[] a = Bounds.AffineCoefficients;

            // gets the affine scaling factors.
            float m11 = Convert.ToSingle(a[1] * dx);
            float m22 = Convert.ToSingle(a[5] * -dy);
            float m21 = Convert.ToSingle(a[2] * dx);
            float m12 = Convert.ToSingle(a[4] * -dy);
            float l = (float)(a[0] - .5 * (a[1] + a[2])); // Left of top left pixel
            float t = (float)(a[3] - .5 * (a[4] + a[5])); // top of top left pixel
            float xShift = (float)((l - envelope.MinX) * dx);
            float yShift = (float)((envelope.MaxY - t) * dy);
            g.Transform = new Matrix(m11, m12, m21, m22, xShift, yShift);
            g.PixelOffsetMode = PixelOffsetMode.Half;
            if (m11 > 1 || m22 > 1)
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
            }

            g.DrawImage(_myImage, new PointF(0, 0));
            g.Dispose();
            return result;
        }

        /// <summary>
        /// Opens the file, assuming that the fileName has already been specified using a Dot Net Image object
        /// </summary>
        public override void Open()
        {
            if (_myImage != null)
            {
                _myImage.Dispose();
            }

            using (FileStream stream = new FileStream(Filename, FileMode.Open))
            {
                using (Image temp = Image.FromStream(stream))
                {
                    _myImage = new Bitmap(temp.Width, temp.Height, PixelFormat.Format32bppArgb);
                    Width = temp.Width;
                    Height = temp.Height;
                    using (Graphics g = Graphics.FromImage(_myImage))
                    {
                        g.DrawImage(temp, new Rectangle(0, 0, temp.Width, temp.Height));
                    }
                }
            }
            WorldFile = new WorldFile(Filename);
            if (WorldFile.Affine == null)
                WorldFile.Affine = new[] { .5, 1.0, 0, _myImage.Height - .5, 0, -1.0 };
            Bounds = new RasterBounds(_myImage.Height, _myImage.Width, WorldFile.Affine);

            NumBands = 4;
            BytesPerPixel = 4;
            CopyBitmapToValues();
        }

        /// <summary>
        /// Saves the current image and world file.
        /// </summary>
        public override void Save()
        {
            _myImage.Save(Filename);
            WorldFile.Save();
        }

        /// <summary>
        /// Saves the image to the specified fileName
        /// </summary>
        /// <param name="fileName">
        /// The string fileName to save this as
        /// </param>
        public override void SaveAs(string fileName)
        {
            Filename = fileName;
            if (WorldFile != null)
            {
                WorldFile.Filename = WorldFile.GenerateFilename(fileName);
            }

            Save();
        }

        /// <summary>
        /// Sets the bitmap
        /// </summary>
        /// <param name="image">
        /// </param>
        public override void SetBitmap(Bitmap image)
        {
            _myImage = image;
        }

        /// <summary>
        /// Release any unmanaged memory objects
        /// </summary>
        /// <param name="disposeManagedResources">
        /// The dispose Managed Resources.
        /// </param>
        protected override void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources)
            {
            }

            if (_myImage != null)
                _myImage.Dispose();
        }

        /// <summary>
        /// The memory setup.
        /// </summary>
        private void MemorySetup()
        {
            MemoryStream ms = new MemoryStream();
            _myImage.Save(ms, ImageFormat.Bmp);
            ms.Position = 0;
            NumBands = 4;
            BytesPerPixel = 4;
            Rectangle bnds = new Rectangle(0, 0, _myImage.Width, _myImage.Height);
            BitmapData bData = _myImage.LockBits(bnds, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Stride = bData.Stride;
            base.Values = new byte[bData.Height * bData.Stride];
            Marshal.Copy(bData.Scan0, base.Values, 0, bData.Height * bData.Stride);
            _myImage.UnlockBits(bData);
        }

        /// <summary>
        /// Gets a block of data directly, converted into a bitmap.
        /// </summary>
        /// <param name="xOffset">The zero based integer column offset from the left</param>
        /// <param name="yOffset">The zero based integer row offset from the top</param>
        /// <param name="xSize">The integer number of pixel columns in the block. </param>
        /// <param name="ySize">The integer number of pixel rows in the block.</param>
        /// <returns>A Bitmap that is xSize, ySize.</returns>
        public override Bitmap ReadBlock(int xOffset, int yOffset, int xSize, int ySize)
        {
            Bitmap bmp = new Bitmap(xSize, ySize);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(_myImage, 0, 0, new Rectangle(xOffset, yOffset, xSize, ySize), GraphicsUnit.Pixel);
            }
            return bmp;
        }

        /// <summary>
        /// Extends the normal bounds changing behavior to also update the world file.
        /// </summary>
        /// <param name="bounds">Updates the world file.</param>
        protected override void OnBoundsChanged(IRasterBounds bounds)
        {
            WorldFile.Affine = bounds.AffineCoefficients;
        }

        /// <summary>
        /// Saves a bitmap of data as a continuous block into the specified location.
        /// </summary>
        /// <param name="value">The bitmap value to save.</param>
        /// <param name="xOffset">The zero based integer column offset from the left</param>
        /// <param name="yOffset">The zero based integer row offset from the top</param>
        public override void WriteBlock(Bitmap value, int xOffset, int yOffset)
        {
            using (Graphics g = Graphics.FromImage(_myImage))
            {
                g.DrawImage(value, new Rectangle(xOffset, yOffset, value.Width, value.Height),
                    new Rectangle(0, 0, value.Width, value.Height), GraphicsUnit.Pixel);
            }
        }
    }
}