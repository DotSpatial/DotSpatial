// ********************************************************************************************************************
// Product Name: DotSpatial.Gdal
// Description:  This is a data extension for the System.Spatial framework.
// ********************************************************************************************************************
// The contents of this file are subject to the Gnu Lesser General Public License (LGPL) you may not use this file
// except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either
// express or implied. See the License for the specific language governing rights and limitations under the
// License.
//
// The Original Code is from a plug-in for MapWindow version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 12/10/2008 11:32:21 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |     Name          |    Date     |              Comments
// |-------------------|-------------|-----------------------------------------
// |Ben tidyup Tombs   |18/11/2010   | Modified to add GDAL Helper class GdalHelper.Configure use for initialization of the GDAL Environment
// ********************************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using OSGeo.GDAL;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// gdalImage
    /// </summary>
    public class GdalImage : ImageData
    {
        private Band _alpha;
        private Band _blue;
        private Dataset _dataset;
        private Band _green;
        private Bitmap _image;
        private Band _red;

        #region Constructors

        /// <summary>
        ///
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="ds"></param>
        /// <param name="band"></param>
        internal GdalImage(string filename, Dataset ds, ImageBandType band)
        {
            _dataset = ds;
            if (band == ImageBandType.ARGB)
            {
                using (Band bnd = ds.GetRasterBand(1))
                {
                    bnd.SetRasterColorInterpretation(ColorInterp.GCI_AlphaBand);
                }
                using (Band bnd = ds.GetRasterBand(2))
                {
                    bnd.SetRasterColorInterpretation(ColorInterp.GCI_RedBand);
                }
                using (Band bnd = ds.GetRasterBand(3))
                {
                    bnd.SetRasterColorInterpretation(ColorInterp.GCI_GreenBand);
                }
                using (Band bnd = ds.GetRasterBand(4))
                {
                    bnd.SetRasterColorInterpretation(ColorInterp.GCI_BlueBand);
                }
            }
            else if (band == ImageBandType.RGB)
            {
                using (Band bnd = ds.GetRasterBand(1))
                {
                    bnd.SetRasterColorInterpretation(ColorInterp.GCI_RedBand);
                }
                using (Band bnd = ds.GetRasterBand(2))
                {
                    bnd.SetRasterColorInterpretation(ColorInterp.GCI_GreenBand);
                }
                using (Band bnd = ds.GetRasterBand(3))
                {
                    bnd.SetRasterColorInterpretation(ColorInterp.GCI_BlueBand);
                }
            }
            else if (band == ImageBandType.PalletCoded)
            {
                using (Band bnd = ds.GetRasterBand(3))
                {
                    bnd.SetRasterColorInterpretation(ColorInterp.GCI_PaletteIndex);
                }
            }
            else
            {
                using (Band bnd = ds.GetRasterBand(3))
                {
                    bnd.SetRasterColorInterpretation(ColorInterp.GCI_GrayIndex);
                }
            }

            Filename = filename;
            WorldFile = new WorldFile { Affine = new double[6] };
            Gdal.AllRegister();
        }

        /// <summary>
        /// Creates a new instance of gdalImage, and gets much of the header
        /// information without actually reading any values from the file.
        /// </summary>
        public GdalImage(string fileName)
        {
            Filename = fileName;
            WorldFile = new WorldFile { Affine = new double[6] };
            GdalHelper.Configure();
            ReadHeader();
        }

        /// <summary>
        /// Creates a new instance of gdalImage
        /// </summary>
        public GdalImage()
        {
            GdalHelper.Configure();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the size of the whole image, but doesn't keep the image open
        /// unless it was already open.
        /// </summary>
        private void ReadHeader()
        {
            try
            {
                _dataset = Gdal.Open(Filename, Access.GA_Update);
            }
            catch
            {
                try
                {
                    _dataset = Gdal.Open(Filename, Access.GA_ReadOnly);
                }
                catch (Exception ex)
                {
                    throw new GdalException(ex.ToString());
                }
            }
            Width = _dataset.RasterXSize;
            Height = _dataset.RasterYSize;
            NumBands = _dataset.RasterCount;
            double[] test = new double[6];
            _dataset.GetGeoTransform(test);
            ProjectionString = _dataset.GetProjection();
            Bounds = new RasterBounds(Height, Width, test);
            WorldFile.Affine = test;
            Close();
        }

        /// <inheritdoc />
        public override void Close()
        {
            _dataset.Dispose();
            _dataset = null;
        }

        /// <summary>
        /// Attempts to open the specified file into memory.
        /// </summary>
        public override void Open()
        {
            try
            {
                _dataset = Gdal.Open(Filename, Access.GA_Update);
            }
            catch
            {
                try
                {
                    _dataset = Gdal.Open(Filename, Access.GA_ReadOnly);
                }
                catch (Exception ex)
                {
                    throw new GdalException(ex.ToString());
                }
            }
            int numBands = _dataset.RasterCount;
            _red = _dataset.GetRasterBand(1);
            this.BandType = ImageBandType.RGB;
            for (int i = 1; i <= numBands; i++)
            {
                Band tempBand = _dataset.GetRasterBand(i);
                if (tempBand.GetRasterColorInterpretation() == ColorInterp.GCI_GrayIndex)
                {
                    this.BandType = ImageBandType.Gray;
                    _red = tempBand;
                }
                if (tempBand.GetRasterColorInterpretation() == ColorInterp.GCI_PaletteIndex)
                {
                    this.BandType = ImageBandType.PalletCoded;
                    _red = tempBand;
                }

                if (tempBand.GetRasterColorInterpretation() == ColorInterp.GCI_RedBand)
                {
                    _red = tempBand;
                }
                if (tempBand.GetRasterColorInterpretation() == ColorInterp.GCI_AlphaBand)
                {
                    _alpha = tempBand;
                    this.BandType = ImageBandType.ARGB;
                }
                else if (tempBand.GetRasterColorInterpretation() == ColorInterp.GCI_BlueBand)
                {
                    _blue = tempBand;
                }
                else if (tempBand.GetRasterColorInterpretation() == ColorInterp.GCI_GreenBand)
                {
                    _green = tempBand;
                }
            }

            if (this.BandType == ImageBandType.PalletCoded)
            {
                ReadPaletteBuffered();
            }
            else if (this.BandType == ImageBandType.Gray)
            {
                ReadGrayIndex();
            }
            else if (this.BandType == ImageBandType.ARGB)
            {
                ReadArgb();
            }
            else if (this.BandType == ImageBandType.RGB)
            {
                ReadRgb();
            }
        }

        /// <summary>
        /// This needs to return the actual image and override the base
        /// behavior that handles the internal variables only.
        /// </summary>
        /// <param name="envelope">The envelope to grab image data for.</param>
        /// <param name="window">A Rectangle</param>
        /// <returns></returns>
        public override Bitmap GetBitmap(Extent envelope, Rectangle window)
        {
            if (window.Width == 0 || window.Height == 0)
            {
                return null;
            }

            Bitmap result = new Bitmap(window.Width, window.Height);
            Graphics g = Graphics.FromImage(result);

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

            g.DrawImage(_image, new PointF(0, 0));
            g.Dispose();
            return result;
        }

        #endregion

        #region PaletteBuffered

        /// <summary>
        /// This is only used in the palette indexed band type.
        /// </summary>
        public override IEnumerable<Color> GetColorPalette()
        {
            if (ColorPalette == null)
            {
                try
                {
                    _dataset = Gdal.Open(Filename, Access.GA_Update);
                }
                catch
                {
                    try
                    {
                        _dataset = Gdal.Open(Filename, Access.GA_ReadOnly);
                    }
                    catch (Exception ex)
                    {
                        throw new GdalException(ex.ToString());
                    }
                }
                Band first = _dataset.GetRasterBand(1);
                ColorTable ct = first.GetRasterColorTable();
                int count = ct.GetCount();
                List<Color> result = new List<Color>();
                for (int i = 0; i < count; i++)
                {
                    ColorEntry ce = ct.GetColorEntry(i);
                    result.Add(Color.FromArgb(ce.c4, ce.c1, ce.c2, ce.c3));
                }
                ColorPalette = result;
            }

            return ColorPalette;
        }

        /// <summary>
        /// This should update the palette cached and in the file.
        /// </summary>
        /// <param name="value"></param>
        public override void SetColorPalette(IEnumerable<Color> value)
        {
            ColorPalette = value;
            _dataset = Gdal.Open(Filename, Access.GA_Update);

            ColorTable ct = new ColorTable(PaletteInterp.GPI_RGB);
            int index = 0;
            foreach (Color c in value)
            {
                ColorEntry ce = new ColorEntry();
                ce.c4 = c.A;
                ce.c3 = c.B;
                ce.c2 = c.G;
                ce.c1 = c.R;
                ct.SetColorEntry(index, ce);
                index++;
            }
            using (Band first = _dataset.GetRasterBand(1))
            {
                first.SetRasterColorTable(ct);
            }
        }

        private void ReadPaletteBuffered()
        {
            ColorTable ct = _red.GetRasterColorTable();
            if (ct == null)
            {
                throw new GdalException("Image was stored with a palette interpretation but has no color table.");
            }
            if (ct.GetPaletteInterpretation() != PaletteInterp.GPI_RGB)
            {
                throw new GdalException(String.Format("Only RGB palette interpretation is currently supported by this plug-in, {0} is not supported.", ct.GetPaletteInterpretation()));
            }
            int width = Width;
            int height = Height;
            byte[] r = new byte[width * height];
            _red.ReadRaster(0, 0, width, height, r, width, height, 0, 0);

            _image = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            BitmapData bData = _image.LockBits(new Rectangle(0, 0, width, height),
                                               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Stride = bData.Stride;
            _image.UnlockBits(bData);

            const int bpp = 4;
            int stride = Stride;
            byte[] vals = new byte[width * height * 4];
            byte[][] colorTable = new byte[ct.GetCount()][];
            for (int i = 0; i < ct.GetCount(); i++)
            {
                ColorEntry ce = ct.GetColorEntry(i);
                colorTable[i] = new[]
                                    {
                                        (byte) ce.c3, (byte) ce.c2, (byte) ce.c1,
                                        (byte) ce.c4
                                    };
            }
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    Array.Copy(colorTable[r[col + row * width]], 0, vals,
                               row * stride + col * bpp, 4);
                }
            }
            Values = vals;
            CopyValuesToBitmap();
        }

        private static Bitmap ReadPaletteBuffered(int xOffset, int yOffset, int xSize, int ySize, Band first)
        {
            ColorTable ct = first.GetRasterColorTable();
            if (ct == null)
            {
                throw new GdalException("Image was stored with a palette interpretation but has no color table.");
            }
            if (ct.GetPaletteInterpretation() != PaletteInterp.GPI_RGB)
            {
                throw new GdalException("Only RGB palette interpretation is currently supported by this " +
                                        " plug-in, " + ct.GetPaletteInterpretation() + " is not supported.");
            }
            int width = xSize;
            int height = ySize;
            byte[] r = new byte[width * height];
            first.ReadRaster(xOffset, yOffset, xSize, ySize, r, xSize, ySize, 0, 0);

            Bitmap result = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            BitmapData bData = result.LockBits(new Rectangle(0, 0, width, height),
                                               ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int stride = Math.Abs(bData.Stride);

            const int bpp = 4;
            byte[] vals = new byte[stride * height];
            byte[][] colorTable = new byte[ct.GetCount()][];
            for (int i = 0; i < ct.GetCount(); i++)
            {
                ColorEntry ce = ct.GetColorEntry(i);
                colorTable[i] = new[]
                                    {
                                        (byte) ce.c3, (byte) ce.c2, (byte) ce.c1,
                                        (byte) ce.c4
                                    };
            }
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    Array.Copy(colorTable[r[col + row * stride]], 0, vals,
                               row * stride + col * bpp, 4);
                }
            }
            Marshal.Copy(vals, 0, bData.Scan0, vals.Length);
            result.UnlockBits(bData);
            return result;
        }

        private static void WritePaletteBuffered(Bitmap value, int xOffset, int yOffset, Band first)
        {
            ColorTable ct = first.GetRasterColorTable();
            if (ct == null)
            {
                throw new GdalException("Image was stored with a palette interpretation but has no color table.");
            }
            if (ct.GetPaletteInterpretation() != PaletteInterp.GPI_RGB)
            {
                throw new GdalException("Only RGB palette interpretation is currently supported by this " +
                                        " plug-in, " + ct.GetPaletteInterpretation() + " is not supported.");
            }
            int width = value.Width;
            int height = value.Height;
            BitmapData bData = value.LockBits(new Rectangle(0, 0, width, height),
                                              ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            int stride = Math.Abs(bData.Stride);
            byte[] r = new byte[stride * height];
            Marshal.Copy(bData.Scan0, r, 0, r.Length);
            value.UnlockBits(bData);
            byte[] vals = new byte[width * height];
            byte[][] colorTable = new byte[ct.GetCount()][];
            for (int i = 0; i < ct.GetCount(); i++)
            {
                ColorEntry ce = ct.GetColorEntry(i);
                colorTable[i] = new[]
                                    {
                                        (byte) ce.c3, (byte) ce.c2, (byte) ce.c1,
                                        (byte) ce.c4
                                    };
            }
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    vals[row * width + col] = MatchColor(r, row * stride + col * 4, colorTable);
                }
            }
            first.WriteRaster(xOffset, yOffset, width, height, vals, width, height, 0, 0);
        }

        #endregion

        /// <summary>
        /// Finds the closest color in the table based on the hamming distance.
        /// </summary>
        /// <param name="vals"></param>
        /// <param name="offset"></param>
        /// <param name="colorTable"></param>
        /// <returns></returns>
        private static byte MatchColor(byte[] vals, int offset, byte[][] colorTable)
        {
            int shortestDistance = int.MaxValue;
            int index = 0;
            int result = 0;
            foreach (byte[] color in colorTable)
            {
                int dist = Math.Abs(vals[offset] - color[0]);
                dist += Math.Abs(vals[offset + 1] - color[1]);
                dist += Math.Abs(vals[offset + 2] - color[2]);
                dist += Math.Abs(vals[offset + 3] - color[0]);
                if (dist < shortestDistance)
                {
                    shortestDistance = dist;
                    result = index;
                }
                index++;
            }
            return Convert.ToByte(result);
        }

        private void ReadGrayIndex()
        {
            Width = _red.XSize;
            Height = _red.YSize;
            _image = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            byte[] r = new byte[Width * Height];
            _red.ReadRaster(0, 0, Width, Height, r, Width, Height, 0, 0);
            BitmapData bData =
                _image.LockBits(new Rectangle(0, 0, Width, Height),
                                ImageLockMode.ReadWrite,
                                PixelFormat.Format32bppArgb);
            Stride = bData.Stride;
            _image.UnlockBits(bData);
            byte[] vals = new byte[Width * Height * 4];

            BytesPerPixel = 4;
            int height = Height;
            int width = Width;
            int stride = Stride;
            int bpp = BytesPerPixel;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    vals[row * stride + col * bpp] = r[row * width + col];
                    vals[row * stride + col * bpp + 1] = r[row * width + col];
                    vals[row * stride + col * bpp + 2] = r[row * width + col];
                    vals[row * stride + col * bpp + 3] = 255;
                }
            }
            Values = vals;
            CopyValuesToBitmap();
        }

        private static Bitmap ReadGrayIndex(int xOffset, int yOffset, int xSize, int ySize, Band first)
        {
            int width = first.XSize;
            int height = first.YSize;
            Bitmap result = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            byte[] r = new byte[width * height];
            first.ReadRaster(xOffset, yOffset, xSize, ySize, r, xSize, ySize, 0, 0);
            BitmapData bData =
                result.LockBits(new Rectangle(0, 0, xSize, ySize),
                                ImageLockMode.ReadWrite,
                                PixelFormat.Format32bppArgb);
            int stride = Math.Abs(bData.Stride);

            byte[] vals = new byte[height * stride];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    vals[row * stride + col * 4] = r[row * width + col];
                    vals[row * stride + col * 4 + 1] = r[row * width + col];
                    vals[row * stride + col * 4 + 2] = r[row * width + col];
                    vals[row * stride + col * 4 + 3] = 255;
                }
            }
            Marshal.Copy(vals, 0, bData.Scan0, vals.Length);
            result.UnlockBits(bData);
            return result;
        }

        private static void WriteGrayIndex(Bitmap value, int xOffset, int yOffset, Band first)
        {
            int width = value.Width;
            int height = value.Height;
            BitmapData bData = value.LockBits(new Rectangle(0, 0, width, height),
                                ImageLockMode.ReadWrite,
                                PixelFormat.Format32bppArgb);
            int stride = Math.Abs(bData.Stride);

            byte[] r = new byte[stride * height * 4];

            Marshal.Copy(bData.Scan0, r, 0, r.Length);

            byte[] vals = new byte[height * stride];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    byte blue = r[row * stride + col * 4];
                    byte green = r[row * stride + col * 4 + 1];
                    byte red = r[row * stride + col * 4 + 2];
                    int gray = Convert.ToInt32(.3 * red + .59 * green + .11 * blue);
                    if (gray > 255) gray = 255;
                    if (gray < 0) gray = 0;
                    vals[row * width + col] = Convert.ToByte(gray);
                }
            }
            first.WriteRaster(xOffset, yOffset, width, height, vals, width, height, 0, 0);
        }

        private void ReadRgb()
        {
            if (_dataset.RasterCount < 3)
            {
                throw new GdalException(
                    "RGB Format was indicated but there are only " +
                    _dataset.RasterCount +
                    " bands!");
            }

            Width = _red.XSize;
            Height = _red.YSize;

            _image = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

            byte[] r = new byte[Width * Height];
            byte[] g = new byte[Width * Height];
            byte[] b = new byte[Width * Height];

            _red.ReadRaster(0, 0, Width, Height, r, Width, Height, 0, 0);
            _green.ReadRaster(0, 0, Width, Height, g, Width, Height, 0, 0);
            _blue.ReadRaster(0, 0, Width, Height, b, Width, Height, 0, 0);

            BitmapData bData =
                _image.LockBits(new Rectangle(0, 0, Width, Height),
                                ImageLockMode.ReadWrite,
                                PixelFormat.Format32bppArgb);
            Stride = bData.Stride;
            _image.UnlockBits(bData);
            byte[] vals = new byte[Width * Height * 4];
            BytesPerPixel = 4;
            int stride = Stride;
            int bpp = BytesPerPixel;
            int width = Width;
            int height = Height;
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    vals[row * stride + col * bpp] = b[row * width + col];
                    vals[row * stride + col * bpp + 1] = g[row * width + col];
                    vals[row * stride + col * bpp + 2] = r[row * width + col];
                    vals[row * stride + col * bpp + 3] = 255;
                }
            }
            Values = vals;
            CopyValuesToBitmap();
        }

        private static Bitmap ReadRgb(int xOffset, int yOffset, int xSize, int ySize, Band first, Dataset set)
        {
            if (set.RasterCount < 3)
            {
                throw new GdalException(
                    "RGB Format was indicated but there are only " +
                    set.RasterCount +
                    " bands!");
            }
            Band green = set.GetRasterBand(2);
            Band blue = set.GetRasterBand(3);

            int width = first.XSize;
            int height = first.YSize;

            Bitmap result = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            byte[] r = new byte[width * height];
            byte[] g = new byte[width * height];
            byte[] b = new byte[width * height];

            first.ReadRaster(xOffset, yOffset, width, height, r, width, height, 0, 0);
            green.ReadRaster(xOffset, yOffset, width, height, g, width, height, 0, 0);
            blue.ReadRaster(xOffset, yOffset, width, height, b, width, height, 0, 0);
            // first disposed in caller
            green.Dispose();
            blue.Dispose();

            BitmapData bData =
                result.LockBits(new Rectangle(0, 0, width, height),
                                ImageLockMode.ReadWrite,
                                PixelFormat.Format32bppArgb);
            int stride = Math.Abs(bData.Stride);
            byte[] vals = new byte[width * height * 4];
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    vals[row * stride + col * 4] = b[row * width + col];
                    vals[row * stride + col * 4 + 1] = g[row * width + col];
                    vals[row * stride + col * 4 + 2] = r[row * width + col];
                    vals[row * stride + col * 4 + 3] = 255;
                }
            }
            Marshal.Copy(vals, 0, bData.Scan0, vals.Length);
            result.UnlockBits(bData);
            return result;
        }

        private static void WriteRgb(Bitmap value, int xOffset, int yOffset, Band first, Dataset set)
        {
            if (set.RasterCount < 3)
            {
                throw new GdalException(
                    "RGB Format was indicated but there are only " +
                    set.RasterCount +
                    " bands!");
            }

            int width = value.Width;
            int height = value.Height;

            BitmapData bData = value.LockBits(new Rectangle(0, 0, width, height),
                                 ImageLockMode.ReadWrite,
                                 PixelFormat.Format24bppRgb);
            int stride = Math.Abs(bData.Stride);
            byte[] vals = new byte[stride * height];
            Marshal.Copy(bData.Scan0, vals, 0, vals.Length);
            value.UnlockBits(bData);
            byte[] r = new byte[width * height];
            byte[] g = new byte[width * height];
            byte[] b = new byte[width * height];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    b[row * width + col] = vals[row * stride + col * 4];
                    g[row * width + col] = vals[row * stride + col * 4 + 1];
                    r[row * width + col] = vals[row * stride + col * 4 + 2];
                }
            }
            Band green = set.GetRasterBand(2);
            Band blue = set.GetRasterBand(3);
            first.WriteRaster(xOffset, yOffset, width, height, r, width, height, 0, 0);
            first.FlushCache();
            green.WriteRaster(xOffset, yOffset, width, height, g, width, height, 0, 0);
            green.FlushCache();
            blue.WriteRaster(xOffset, yOffset, width, height, b, width, height, 0, 0);
            blue.FlushCache();
            // first disposed in caller
            green.Dispose();
            blue.Dispose();
        }

        private static Bitmap ReadArgb(int xOffset, int yOffset, int xSize, int ySize, Band first, Dataset set)
        {
            if (set.RasterCount < 4)
            {
                throw new GdalException(
                    "ARGB Format was indicated but there are only " +
                    set.RasterCount +
                    " bands!");
            }
            Band alpha = first;
            Band red = set.GetRasterBand(2);
            Band green = set.GetRasterBand(3);
            Band blue = set.GetRasterBand(4);

            int width = first.XSize;
            int height = first.YSize;

            Bitmap result = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            byte[] a = new byte[width * height];
            byte[] r = new byte[width * height];
            byte[] g = new byte[width * height];
            byte[] b = new byte[width * height];

            alpha.ReadRaster(0, 0, width, height, a, width, height, 0, 0);
            red.ReadRaster(0, 0, width, height, r, width, height, 0, 0);
            green.ReadRaster(0, 0, width, height, g, width, height, 0, 0);
            blue.ReadRaster(0, 0, width, height, b, width, height, 0, 0);
            // Alpha disposed in caller
            red.Dispose();
            green.Dispose();
            blue.Dispose();

            BitmapData bData =
                result.LockBits(new Rectangle(0, 0, width, height),
                                ImageLockMode.ReadWrite,
                                PixelFormat.Format32bppArgb);
            int stride = Math.Abs(bData.Stride);

            byte[] vals = new byte[height * stride];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    vals[row * stride + col * 4] =
                        b[row * width + col];
                    vals[row * stride + col * 4 + 1] =
                        g[row * width + col];
                    vals[row * stride + col * 4 + 2] =
                        r[row * width + col];
                    vals[row * stride + col * 4 + 3] =
                        a[row * width + col];
                }
            }
            Marshal.Copy(vals, 0, bData.Scan0, vals.Length);
            result.UnlockBits(bData);
            return result;
        }

        private void ReadArgb()
        {
            if (_dataset.RasterCount < 4)
            {
                throw new GdalException(
                    "ARGB Format was indicated but there are only " +
                    _dataset.RasterCount +
                    " bands!");
            }

            Width = _red.XSize;
            Height = _red.YSize;

            _image = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);

            byte[] a = new byte[Width * Height];
            byte[] r = new byte[Width * Height];
            byte[] g = new byte[Width * Height];
            byte[] b = new byte[Width * Height];

            _alpha.ReadRaster(0, 0, Width, Height, a, Width, Height, 0, 0);
            _red.ReadRaster(0, 0, Width, Height, r, Width, Height, 0, 0);
            _green.ReadRaster(0, 0, Width, Height, g, Width, Height, 0, 0);
            _blue.ReadRaster(0, 0, Width, Height, b, Width, Height, 0, 0);

            BitmapData bData =
                _image.LockBits(new Rectangle(0, 0, Width, Height),
                                ImageLockMode.ReadWrite,
                                PixelFormat.Format32bppArgb);
            Stride = bData.Stride;
            _image.UnlockBits(bData);
            Values = new byte[Width * Height * 4];
            BytesPerPixel = 4;
            for (int row = 0; row < Height; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    Values[row * Stride + col * BytesPerPixel] =
                        b[row * Width + col];
                    Values[row * Stride + col * BytesPerPixel + 1] =
                        g[row * Width + col];
                    Values[row * Stride + col * BytesPerPixel + 2] =
                        r[row * Width + col];
                    Values[row * Stride + col * BytesPerPixel + 3] =
                        a[row * Width + col];
                }
            }
            CopyValuesToBitmap();
        }

        private static void WriteArgb(Bitmap value, int xOffset, int yOffset, Band first, Dataset set)
        {
            if (set.RasterCount < 4)
            {
                throw new GdalException(
                    "ARGB Format was indicated but there are only " +
                    set.RasterCount +
                    " bands!");
            }

            int width = value.Width;
            int height = value.Height;

            BitmapData bData = value.LockBits(new Rectangle(0, 0, width, height),
                                 ImageLockMode.ReadWrite,
                                 PixelFormat.Format32bppArgb);
            int stride = Math.Abs(bData.Stride);
            byte[] vals = new byte[stride * height];
            Marshal.Copy(bData.Scan0, vals, 0, vals.Length);
            value.UnlockBits(bData);
            byte[] a = new byte[width * height];
            byte[] r = new byte[width * height];
            byte[] g = new byte[width * height];
            byte[] b = new byte[width * height];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    b[row * width + col] = vals[row * stride + col * 4];
                    g[row * width + col] = vals[row * stride + col * 4 + 1];
                    r[row * width + col] = vals[row * stride + col * 4 + 2];
                    a[row * width + col] = vals[row * stride + col * 4 + 3];
                }
            }

            Band red = set.GetRasterBand(2);
            Band green = set.GetRasterBand(3);
            Band blue = set.GetRasterBand(4);
            first.WriteRaster(xOffset, yOffset, width, height, a, width, height, 0, 0);
            red.WriteRaster(xOffset, yOffset, width, height, r, width, height, 0, 0);
            green.WriteRaster(xOffset, yOffset, width, height, g, width, height, 0, 0);
            blue.WriteRaster(xOffset, yOffset, width, height, b, width, height, 0, 0);
            // first disposed in caller
            green.Dispose();
            blue.Dispose();
        }

        /// <summary>
        /// Reads the actual image values from the image file into the array
        /// of Values, which can be accessed by the Color property.
        /// </summary>
        public override void CopyBitmapToValues()
        {
            BitmapData bData =
                _image.LockBits(new Rectangle(0, 0, Width, Height),
                                ImageLockMode.ReadOnly,
                                PixelFormat.Format32bppArgb);
            Stride = bData.Stride;
            Marshal.Copy(bData.Scan0, Values, 0, bData.Height * bData.Stride);
            _image.UnlockBits(bData);
        }

        /// <summary>
        /// Writes the byte values stored in the Bytes array into the bitmap
        /// image.
        /// </summary>
        public override void CopyValuesToBitmap()
        {
            Rectangle bnds = new Rectangle(0, 0, Width, Height);
            BitmapData bData = _image.LockBits(bnds, ImageLockMode.WriteOnly,
                                               PixelFormat.Format32bppArgb);
            Marshal.Copy(Values, 0, bData.Scan0, Values.Length);
            _image.UnlockBits(bData);
        }

        /// <summary>
        /// Gets a block of data directly, converted into a bitmap.  This always writes
        /// to the base layer, not the overviews.
        /// </summary>
        /// <param name="xOffset">The zero based integer column offset from the left</param>
        /// <param name="yOffset">The zero based integer row offset from the top</param>
        /// <param name="xSize">The integer number of pixel columns in the block. </param>
        /// <param name="ySize">The integer number of pixel rows in the block.</param>
        /// <returns>A Bitmap that is xSize, ySize.</returns>
        public override Bitmap ReadBlock(int xOffset, int yOffset, int xSize, int ySize)
        {
            try
            {
                _dataset = Gdal.Open(Filename, Access.GA_Update);
            }
            catch
            {
                try
                {
                    _dataset = Gdal.Open(Filename, Access.GA_ReadOnly);
                }
                catch (Exception ex)
                {
                    throw new GdalException(ex.ToString());
                }
            }

            Band first = _dataset.GetRasterBand(1);
            Bitmap result = null;
            if (first.GetRasterColorInterpretation() == ColorInterp.GCI_PaletteIndex)
            {
                result = ReadPaletteBuffered(xOffset, yOffset, xSize, ySize, first);
            }
            if (first.GetRasterColorInterpretation() == ColorInterp.GCI_GrayIndex)
            {
                result = ReadGrayIndex(xOffset, yOffset, xSize, ySize, first);
            }
            if (first.GetRasterColorInterpretation() == ColorInterp.GCI_RedBand)
            {
                result = ReadRgb(xOffset, yOffset, xSize, ySize, first, _dataset);
            }
            if (first.GetRasterColorInterpretation() == ColorInterp.GCI_AlphaBand)
            {
                result = ReadArgb(xOffset, yOffset, xSize, ySize, first, _dataset);
            }
            first.Dispose();
            // data set disposed on disposing this image
            return result;
        }

        /// <summary>
        /// Saves a bitmap of data as a continuous block into the specified location.
        /// This always writes to the base image, and not the overviews.
        /// </summary>
        /// <param name="value">The bitmap value to save.</param>
        /// <param name="xOffset">The zero based integer column offset from the left</param>
        /// <param name="yOffset">The zero based integer row offset from the top</param>
        public override void WriteBlock(Bitmap value, int xOffset, int yOffset)
        {
            if (_dataset == null)
            {
                // This will fail if write access is not allowed, but just pass the
                // exception back up the stack.
                _dataset = Gdal.Open(Filename, Access.GA_Update);
            }

            Band first = _dataset.GetRasterBand(1);

            if (BandType == ImageBandType.PalletCoded)
            {
                WritePaletteBuffered(value, xOffset, yOffset, first);
            }
            if (BandType == ImageBandType.Gray)
            {
                WriteGrayIndex(value, xOffset, yOffset, first);
            }
            if (BandType == ImageBandType.RGB)
            {
                WriteRgb(value, xOffset, yOffset, first, _dataset);
            }
            if (BandType == ImageBandType.ARGB)
            {
                WriteArgb(value, xOffset, yOffset, first, _dataset);
            }
            first.Dispose();
        }

        /// <inheritdocs/>
        protected override void Dispose(bool disposeManagedResources)
        {
            // All class variables are unmanaged.
            if (_dataset != null)
            {
                _dataset.FlushCache();
                _dataset.Dispose();
            }
            _dataset = null;
            if (_image != null)
            {
                _image.Dispose();
            }
            _image = null;
            if (_red != null)
            {
                _red.Dispose();
            }
            _red = null;
            if (_blue != null)
            {
                _blue.Dispose();
            }
            _blue = null;
            if (_green != null)
            {
                _green.Dispose();
            }
            _green = null;
            if (_alpha != null)
            {
                _alpha.Dispose();
            }
            _alpha = null;
            base.Dispose(disposeManagedResources);
            GC.Collect();
        }
    }
}