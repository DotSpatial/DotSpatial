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
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/21/2010 11:25:19 AM
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Data
{
    /// <summary>
    /// Sets the InRamImage for this class.  This is just a georeferenced .Net bitmap that also implements
    /// IGetBitmap for cross purpose use with IImageData implementations.
    /// </summary>
    public class InRamImage : DisposeBase, IGetBitmap
    {
        private Bitmap _myImage;

        /// <summary>
        /// Initializes a new instance of an InRamImage class.  This class supports a basic .Net Image
        /// plus a geographic extent as a RasterBounds.  This class does not feature any byte level
        /// data access or built in file access.  The expectation is that this will only be used in memory.
        /// </summary>
        /// <param name="image">THe Bitmap image to use to create this image.</param>
        public InRamImage(Bitmap image)
        {
            _myImage = image;
            Bounds = new RasterBounds(image.Height, image.Width, new double[] { 0, 1, 0, image.Height, 0, -1 });
        }

        #region IGetBitmap Members

        /// <summary>
        /// Gets or sets the raster bounds
        /// </summary>
        public IRasterBounds Bounds { get; set; }

        /// <summary>
        /// Returns the internal bitmap in this case.  In other cases, this may have to be constructed
        /// from the unmanaged memory content.
        /// </summary>
        /// <returns>
        /// A Bitmap that represents the entire image.
        /// </returns>
        public Bitmap GetBitmap()
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
        public Bitmap GetBitmap(Extent envelope, Rectangle window)
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
        /// The geographic envelope gives the region that the image should be created for.
        /// The window gives the corresponding pixel dimensions for the image, so that
        /// images matching the resolution of the screen can be used.
        /// </summary>
        /// <param name="envelope">The geographic extents to retrieve data for</param>
        /// <param name="size">The rectangle that defines the size of the drawing area in pixels</param>
        /// <returns>A bitmap captured from the main image </returns>
        public Bitmap GetBitmap(Extent envelope, Size size)
        {
            return GetBitmap(envelope, new Rectangle(new Point(0, 0), size));
        }

        #endregion

        /// <summary>
        /// Sets the bitmap for this InRamImage to use.
        /// </summary>
        /// <param name="value">the value to set.</param>
        public void SetBitmap(Bitmap value)
        {
            _myImage = value;
        }

        /// <summary>
        /// If byte access is required, a new class can be created that is a copy of this class for
        /// modifying values byte by byte.
        /// </summary>
        /// <returns></returns>
        public InRamImageData ToImageData()
        {
            return new InRamImageData(_myImage, Bounds.Extent);
        }
    }
}