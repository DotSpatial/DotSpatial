// ********************************************************************************************************
// Product Name: DotSpatial.Gdal
// Description:  This is a data extension for the System.Spatial framework.
// ********************************************************************************************************
// The contents of this file are subject to the Gnu Lesser General Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from a plugin for MapWindow version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 12/10/2008 11:32:21 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |     Name          |    Date     |              Comments
// |-------------------|-------------|-------------------------------------------------------------------
// |Ben tidyup Tombs   |18/11/2010   | Modified to add GDAL Helper class GdalHelper.Configure use for initialization of the GDAL Environment
// ********************************************************************************************************

using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using OSGeo.GDAL;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// gdalImage
    /// </summary>
    public class GdalImageSource : IImageSource
    {
        #region Private Variables

        Band _alpha;
        Band _blue;

        private RasterBounds _bounds;
        Dataset _dataset;
        private string _fileName;
        Band _green;
        private int _numOverviews;
        Band _red;

        #endregion

        #region Constructors

        static GdalImageSource()
        {
            GdalConfiguration.ConfigureGdal();
        }

        /// <summary>
        /// Creates a new instance of gdalImage, and gets much of the header information without actually
        /// reading any values from the file.
        /// </summary>
        public GdalImageSource(string fileName)
        {
            Filename = fileName;
            ReadHeader();
        }

        /// <summary>
        /// Creates a new instance of gdalImage
        /// </summary>
        public GdalImageSource()
        {
        }

        /// <summary>
        /// Gets or sets the bounds
        /// </summary>
        public RasterBounds Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        /// <summary>
        /// Gets the number of rows
        /// </summary>
        public int NumRows
        {
            get
            {
                return _bounds != null ? _bounds.NumRows : 0;
            }
        }

        /// <summary>
        /// Gets the total number of columns
        /// </summary>
        public int NumColumns
        {
            get
            {
                return _bounds != null ? _bounds.NumColumns : 0;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the number of overviews, not counting the original image.
        /// </summary>
        /// <returns>The number of overviews.</returns>
        public int NumOverviews
        {
            get
            {
                return _numOverviews;
            }
        }

        /// <summary>
        ///  Returns the data from the file in the form of ARGB bytes, regardless of how the image
        ///  data is actually stored in the file.
        /// </summary>
        /// <param name="startRow">The zero based integer index of the first row (Y)</param>
        /// <param name="startColumn">The zero based integer index of the first column (X)</param>
        /// <param name="numRows">The number of rows to read</param>
        /// <param name="numColumns">The number of columns to read</param>
        /// <param name="overview">The integer overview.  0 for the original image.  Each successive index divides the length and height in half.  </param>
        /// <returns>A Byte of values in ARGB order and in row-major raster-scan sequence</returns>
        public byte[] ReadWindow(int startRow, int startColumn, int numRows, int numColumns, int overview)
        {
            EnsureDatasetOpen();
            _red = _dataset.GetRasterBand(1);
            byte[] result = null;
            if (_red.GetRasterColorInterpretation() == ColorInterp.GCI_PaletteIndex)
            {
                result = ReadPaletteBuffered(startRow, startColumn, numRows, numColumns, overview);
            }
            if (_red.GetRasterColorInterpretation() == ColorInterp.GCI_GrayIndex)
            {
                result = ReadGrayIndex(startRow, startColumn, numRows, numColumns, overview);
            }
            if (_red.GetRasterColorInterpretation() == ColorInterp.GCI_RedBand)
            {
                result = ReadRgb(startRow, startColumn, numRows, numColumns, overview);
            }
            if (_red.GetRasterColorInterpretation() == ColorInterp.GCI_AlphaBand)
            {
                result = ReadArgb(startRow, startColumn, numRows, numColumns, overview);
            }
            return result;
        }

        /// <summary>
        /// This returns the window of data as a bitmap.
        /// </summary>
        /// <param name="startRow">The zero based integer index of the first row (Y).</param>
        /// <param name="startColumn">The zero based integer index of the first column (X).</param>
        /// <param name="numRows">The number of rows to read.</param>
        /// <param name="numColumns">The number of columns to read.</param>
        /// <param name="overview">The integer overview.  0 for the original image.  Each successive index divides the length and height in half.  </param>
        /// <returns>The bitmap representation for the specified portion of the raster.</returns>
        public Bitmap GetBitmap(int startRow, int startColumn, int numRows, int numColumns, int overview)
        {
            byte[] vals = ReadWindow(startRow, startColumn, numRows, numColumns, overview);
            Bitmap bmp = new Bitmap(numRows, numColumns);

            BitmapData bData = bmp.LockBits(new Rectangle(0, 0, numColumns, numRows), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            //int stride = bData.Stride;
            Marshal.Copy(vals, 0, bData.Scan0, vals.Length);
            bmp.UnlockBits(bData);
            return bmp;
        }

        /// <summary>
        /// Gets the size of the whole image, but doesn't keep the image open unless it was already open.
        /// </summary>
        private void ReadHeader()
        {
            EnsureDatasetOpen();

            double[] test = new double[6];
            _dataset.GetGeoTransform(test);
            Bounds = new RasterBounds(_dataset.RasterYSize, _dataset.RasterXSize, test);
            _red = _dataset.GetRasterBand(1);
            _numOverviews = _red.GetOverviewCount();
            Close();
        }

        /// <summary>
        ///
        /// </summary>
        public void Close()
        {
            _dataset.Dispose();
            _dataset = null;
        }

        /// <summary>
        /// Gets the dimensions of the original (0) plus any overlays.
        /// The overlays get smaller as the index gets larger..
        /// </summary>
        /// <returns></returns>
        public Size[] GetSizes()
        {
            EnsureDatasetOpen();
            _red = _dataset.GetRasterBand(1);
            int numOverviews = _red.GetOverviewCount();
            Debug.WriteLine("Num overviews:" + numOverviews);
            if (numOverviews == 0) return null;
            Size[] result = new Size[numOverviews + 1];
            result[0] = new Size(_red.XSize, _red.YSize);
            for (int i = 0; i < numOverviews; i++)
            {
                Band temp = _red.GetOverview(i);
                result[i + 1] = new Size(temp.XSize, temp.YSize);
            }
            return result;
        }

        private void EnsureDatasetOpen()
        {
            if (_dataset == null)
            {
                _dataset = Helpers.Open(Filename);
            }
        }

        /// <summary>
        /// Disposes the dataset
        /// </summary>
        public void Dispose()
        {
            _dataset.Dispose();
            _dataset = null;
        }

        #endregion

        #region Properties

        #endregion

        /// <summary>
        /// Gets or sets the fileName of the image source
        /// </summary>
        public string Filename
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary>
        /// Returns ARGB 32 bpp regardless of the fact that the original is palette buffered.
        /// </summary>
        private byte[] ReadPaletteBuffered(int startRow, int startColumn, int numRows, int numColumns, int overview)
        {
            ColorTable ct = _red.GetRasterColorTable();
            if (ct == null)
            {
                throw new GdalException("Image was stored with a palette interpretation but has no color table.");
            }
            if (ct.GetPaletteInterpretation() != PaletteInterp.GPI_RGB)
            {
                throw new GdalException("Only RGB palette interpreation is currently supported by this plugin, " + ct.GetPaletteInterpretation() + " is not supported.");
            }
            int width = numRows;
            int height = numColumns;
            byte[] r = new byte[width * height];
            _red.ReadRaster(startColumn, startRow, numColumns, numRows, r, width, height, 0, 0);
            if (overview > 0)
            {
                _red = _red.GetOverview(overview - 1);
            }
            const int bpp = 4;
            byte[] vals = new byte[width * height * 4];
            byte[][] colorTable = new byte[256][];
            for (int i = 0; i < 255; i++)
            {
                ColorEntry ce = ct.GetColorEntry(i);
                colorTable[i] = new[] { (byte)ce.c3, (byte)ce.c2, (byte)ce.c1, (byte)ce.c4 };
            }
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    Array.Copy(colorTable[r[col + row * width]], 0, vals, row * width + col * bpp, 4);
                }
            }
            return vals;
        }

        private byte[] ReadGrayIndex(int startRow, int startColumn, int numRows, int numColumns, int overview)
        {
            int width = numColumns;
            int height = numRows;
            byte[] r = new byte[width * height];
            if (overview > 0)
            {
                _red = _red.GetOverview(overview - 1);
            }
            _red.ReadRaster(startColumn, startRow, width, height, r, width, height, 0, 0);
            byte[] vals = new byte[width * height * 4];
            const int bpp = 4;
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    vals[row * width * bpp + col * bpp] = r[row * width + col];
                    vals[row * width * bpp + col * bpp + 1] = r[row * width + col];
                    vals[row * width * bpp + col * bpp + 2] = r[row * width + col];
                    vals[row * width * bpp + col * bpp + 3] = 255;
                }
            }
            return vals;
        }

        private byte[] ReadRgb(int startRow, int startColumn, int numRows, int numColumns, int overview)
        {
            if (_dataset.RasterCount < 3)
            {
                throw new GdalException("RGB Format was indicated but there are only " + _dataset.RasterCount + " bands!");
            }
            _green = _dataset.GetRasterBand(2);
            _blue = _dataset.GetRasterBand(3);

            if (overview > 0)
            {
                _red = _red.GetOverview(overview - 1);
                _green = _green.GetOverview(overview - 1);
                _blue = _blue.GetOverview(overview - 1);
            }

            int width = numColumns;
            int height = numRows;
            byte[] r = new byte[width * height];
            byte[] g = new byte[width * height];
            byte[] b = new byte[width * height];

            _red.ReadRaster(startColumn, startRow, width, height, r, width, height, 0, 0);
            _green.ReadRaster(startColumn, startRow, width, height, g, width, height, 0, 0);
            _blue.ReadRaster(startColumn, startRow, width, height, b, width, height, 0, 0);

            byte[] vals = new byte[width * height * 4];
            const int bpp = 4;
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    vals[row * width * bpp + col * bpp] = b[row * width + col];
                    vals[row * width * bpp + col * bpp + 1] = g[row * width + col];
                    vals[row * width * bpp + col * bpp + 2] = r[row * width + col];
                    vals[row * width * bpp + col * bpp + 3] = 255;
                }
            }
            return vals;
        }

        private byte[] ReadArgb(int startRow, int startColumn, int numRows, int numColumns, int overview)
        {
            if (_dataset.RasterCount < 4)
            {
                throw new GdalException("ARGB Format was indicated but there are only " + _dataset.RasterCount + " bands!");
            }
            _alpha = _red;
            _red = _dataset.GetRasterBand(2);
            _green = _dataset.GetRasterBand(3);
            _blue = _dataset.GetRasterBand(4);

            if (overview > 0)
            {
                _red = _red.GetOverview(overview - 1);
                _alpha = _alpha.GetOverview(overview - 1);
                _green = _green.GetOverview(overview - 1);
                _blue = _blue.GetOverview(overview - 1);
            }

            int width = numColumns;
            int height = numRows;
            byte[] a = new byte[width * height];
            byte[] r = new byte[width * height];
            byte[] g = new byte[width * height];
            byte[] b = new byte[width * height];

            _alpha.ReadRaster(startColumn, startRow, width, height, a, width, height, 0, 0);
            _red.ReadRaster(startColumn, startRow, width, height, r, width, height, 0, 0);
            _green.ReadRaster(startColumn, startRow, width, height, g, width, height, 0, 0);
            _blue.ReadRaster(startColumn, startRow, width, height, b, width, height, 0, 0);

            byte[] vals = new byte[width * height * 4];
            const int bpp = 4;
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    vals[row * width * bpp + col * bpp] = b[row * width + col];
                    vals[row * width * bpp + col * bpp + 1] = g[row * width + col];
                    vals[row * width * bpp + col * bpp + 2] = r[row * width + col];
                    vals[row * width * bpp + col * bpp + 3] = a[row * width + col];
                }
            }
            return vals;
        }
    }
}