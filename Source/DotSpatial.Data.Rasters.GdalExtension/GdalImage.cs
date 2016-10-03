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
using DotSpatial.Serialization;
using OSGeo.GDAL;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    public class GdalImage : ImageData
    {
        private Band _alpha;
        private Band _blue;
        private Dataset _dataset;
        private Band _green;
        private Bitmap _image;
        private Band _red;
        private int _overview;
        private bool _isOpened;

        #region Constructors

        static GdalImage()
        {
            GdalConfiguration.ConfigureGdal();
        }
      
        internal GdalImage(string filename, Dataset ds, ImageBandType band)
        {
            var setRsColor = (Action<int, ColorInterp>)(delegate(int i, ColorInterp ci)
            {
                using (var bnd = ds.GetRasterBand(i))
                {
                    bnd.SetRasterColorInterpretation(ci);
                }
            });

            _dataset = ds;
            switch (band)
            {
                case ImageBandType.ARGB:
                    setRsColor(1, ColorInterp.GCI_AlphaBand);
                    setRsColor(2, ColorInterp.GCI_RedBand);
                    setRsColor(3, ColorInterp.GCI_GreenBand);
                    setRsColor(4, ColorInterp.GCI_BlueBand);
                    break;
                case ImageBandType.RGB:
                    setRsColor(1, ColorInterp.GCI_RedBand);
                    setRsColor(2, ColorInterp.GCI_GreenBand);
                    setRsColor(3, ColorInterp.GCI_BlueBand);
                    break;
                case ImageBandType.PalletCoded:
                    setRsColor(3, ColorInterp.GCI_PaletteIndex);
                    break;
                default:
                    setRsColor(3, ColorInterp.GCI_GrayIndex);
                    break;
            }

            Filename = filename;
            WorldFile = new WorldFile { Affine = new double[6] };
        }

        /// <summary>
        /// Creates a new instance of gdalImage, and gets much of the header
        /// information without actually reading any values from the file.
        /// </summary>
        public GdalImage(string fileName)
        {
            Filename = fileName;
            WorldFile = new WorldFile { Affine = new double[6] };
            ReadHeader();
        }

        /// <summary>
        /// Creates a new instance of gdalImage
        /// </summary>
        public GdalImage()
        {

        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the size of the whole image, but doesn't keep the image open
        /// unless it was already open.
        /// </summary>
        private void ReadHeader()
        {
            _dataset = Helpers.Open(Filename);
            Width = _dataset.RasterXSize;
            Height = _dataset.RasterYSize;
            NumBands = _dataset.RasterCount;
            var test = new double[6];
            _dataset.GetGeoTransform(test);
            test = (new AffineTransform(test)).TransfromToCorner(0.5, 0.5);//shift origin by half a cell
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
            _isOpened = false;
        }

        /// <summary>
        /// Attempts to open the specified file into memory.
        /// </summary>
        public override void Open()
        {
            _dataset = Helpers.Open(Filename);
            int numBands = _dataset.RasterCount;
            _red = _dataset.GetRasterBand(1);
            BandType = ImageBandType.RGB;
            for (int i = 1; i <= numBands; i++)
            {
                var tempBand = _dataset.GetRasterBand(i);
                switch (tempBand.GetRasterColorInterpretation())
                {
                    case ColorInterp.GCI_GrayIndex:
                        BandType = ImageBandType.Gray;
                        _red = tempBand;
                        break;
                    case ColorInterp.GCI_PaletteIndex:
                        BandType = ImageBandType.PalletCoded;
                        _red = tempBand;
                        break;
                    case ColorInterp.GCI_RedBand:
                        _red = tempBand;
                        break;
                    case ColorInterp.GCI_AlphaBand:
                        _alpha = tempBand;
                        BandType = ImageBandType.ARGB;
                        break;
                    case ColorInterp.GCI_BlueBand:
                        _blue = tempBand;
                        break;
                    case ColorInterp.GCI_GreenBand:
                        _green = tempBand;
                        break;
                }
            }
            _isOpened = true;
        }

        /// <summary>
        /// Open or close file. This is a simliar to calling Open/Close methods.
        /// </summary>
        [Serialize("IsOpened")]
        public bool IsOpened
        {
            get { return _isOpened; }
            set
            {
                if (_isOpened == value) return;
                _isOpened = value;
                if (value)
                {
                    Open();
                }
                else
                {
                    Close();
                }
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

            var result = new Bitmap(window.Width, window.Height);
            using (var g = Graphics.FromImage(result))
            {
                DrawGraphics(g, envelope, window);
            }
            return result;
        }

        private void DrawGraphics(Graphics g, Extent envelope, Rectangle window)
        {
            // Gets the scaling factor for converting from geographic to pixel coordinates
            double dx = window.Width / envelope.Width;
            double dy = window.Height / envelope.Height;
            
            double[] a = Bounds.AffineCoefficients;
            
            // calculate inverse
            double p = 1 / (a[1] * a[5] - a[2] * a[4]);
            double[] a_inv = new double[4];
            a_inv[0] = a[5] * p;
            a_inv[1] = -a[2] * p;
            a_inv[2] = -a[4] * p;
            a_inv[3] = a[1] * p;

            // estimate rectangle coordinates
            double TLX = (envelope.MinX - a[0]) * a_inv[0] + (envelope.MaxY - a[3]) * a_inv[1];
            double TLY = (envelope.MinX - a[0]) * a_inv[2] + (envelope.MaxY - a[3]) * a_inv[3];
            double TRX = (envelope.MaxX - a[0]) * a_inv[0] + (envelope.MaxY - a[3]) * a_inv[1];
            double TRY = (envelope.MaxX - a[0]) * a_inv[2] + (envelope.MaxY - a[3]) * a_inv[3];
            double BLX = (envelope.MinX - a[0]) * a_inv[0] + (envelope.MinY - a[3]) * a_inv[1];
            double BLY = (envelope.MinX - a[0]) * a_inv[2] + (envelope.MinY - a[3]) * a_inv[3];
            double BRX = (envelope.MaxX - a[0]) * a_inv[0] + (envelope.MinY - a[3]) * a_inv[1];
            double BRY = (envelope.MaxX - a[0]) * a_inv[2] + (envelope.MinY - a[3]) * a_inv[3];

            // get absolute maximum and minimum coordinates to make a rectangle on projected coordinates
            // that overlaps all the visible area.
            double _TLX = Math.Min(Math.Min(Math.Min(TLX, TRX), BLX), BRX);
            double _TLY = Math.Min(Math.Min(Math.Min(TLY, TRY), BLY), BRY);
            double _BRX = Math.Max(Math.Max(Math.Max(TLX, TRX), BLX), BRX);
            double _BRY = Math.Max(Math.Max(Math.Max(TLY, TRY), BLY), BRY);

            // limit it to the available image
            // todo: why we compare NumColumns\Rows and X,Y coordinates??
            if (_TLX > Bounds.NumColumns) _TLX = Bounds.NumColumns;
            if (_TLY > Bounds.NumRows) _TLY = Bounds.NumRows;
            if (_BRX > Bounds.NumColumns) _BRX = Bounds.NumColumns;
            if (_BRY > Bounds.NumRows) _BRY = Bounds.NumRows;

            if (_TLX < 0) _TLX = 0;
            if (_TLY < 0) _TLY = 0;
            if (_BRX < 0) _BRX = 0;
            if (_BRY < 0) _BRY = 0;

            // gets the affine scaling factors.
            float m11 = Convert.ToSingle(a[1] * dx);
            float m22 = Convert.ToSingle(a[5] * -dy);
            float m21 = Convert.ToSingle(a[2] * dx);
            float m12 = Convert.ToSingle(a[4] * -dy);
            float l = (float)(a[0] - .5 * (a[1] + a[2])); // Left of top left pixel
            float t = (float)(a[3] - .5 * (a[4] + a[5])); // top of top left pixel
            float xShift = (float)((l - envelope.MinX) * dx);
            float yShift = (float)((envelope.MaxY - t) * dy);
            g.PixelOffsetMode = PixelOffsetMode.Half;
            
            if (m11 > 1 || m22 > 1) // out of pyramids
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                _overview = -1; //don't use overviews when zooming behind the max res.
            }
            else
            {
                // estimate the pyramids that we need.
                // when using unreferenced images m11 or m22 can be negative resulting on inf logarithm.
                // so the Math.abs
                _overview = (int) Math.Min(Math.Log(Math.Abs(1/m11), 2), Math.Log(Math.Abs(1/m22), 2));

                // limit it to the available pyramids
                _overview = Math.Min(_overview, _red.GetOverviewCount() - 1);

                // additional test but probably not needed
                if (_overview < 0)
                {
                    _overview = -1;
                }
            }

            var overviewPow = Math.Pow(2, _overview + 1);

            // witdh and height of the image
            var w = (_BRX - _TLX) / overviewPow;
            var h = (_BRY - _TLY) / overviewPow;

            using (var matrix = new Matrix(
                    m11 * (float)overviewPow,
                    m12 * (float)overviewPow,
                    m21 * (float)overviewPow,
                    m22 * (float)overviewPow, xShift, yShift))
            {
                g.Transform = matrix;
            }

            int blockXsize, blockYsize;
            // get the optimal block size to request gdal. 
            // if the image is stored line by line then ask for a 100px stripe.
            if (_overview >= 0 && _red.GetOverviewCount() > 0)
            {
                using (var overview = _red.GetOverview(_overview))
                {
                    overview.GetBlockSize(out blockXsize, out blockYsize);
                    if (blockYsize == 1)
                    {
                        blockYsize = Math.Min(100, overview.YSize);
                    }
                }
            }
            else
            {
                _red.GetBlockSize(out blockXsize, out blockYsize);
                if (blockYsize == 1)
                {
                    blockYsize = Math.Min(100, _red.YSize);
                }
            }

            int nbX, nbY;
            // limit the block size to the viewable image.
            if (w < blockXsize)
            {
                blockXsize = (int)w;
                nbX = 1;
            }
            else
            {
                nbX = (int)(w / blockXsize) + 1;
            }

            if (h < blockYsize)
            {
                blockYsize = (int)h;
                nbY = 1;
            }
            else
            {
                nbY = (int)(h / blockYsize) + 1;
            }
            
            for (var i = 0; i < nbX; i++)
                for (var j = 0; j < nbY; j++)
                {
                    // The +1 is to remove the white stripes artifacts
                    using (var bitmap = ReadBlock(
                        (int)(_TLX / overviewPow) + i * blockXsize,
                        (int)(_TLY / overviewPow) + j * blockYsize, blockXsize + 1, blockYsize + 1))
                    {
                        g.DrawImage(bitmap,
                            (int)(_TLX / overviewPow) + i * blockXsize,
                            (int)(_TLY / overviewPow) + j * blockYsize);
                    }
                }
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
                _dataset = Helpers.Open(Filename);
                using (var first = _dataset.GetRasterBand(1))
                using(var ct = first.GetRasterColorTable())
                {
                    int count = ct.GetCount();
                    var result = new List<Color>(count);
                    for (int i = 0; i < count; i++)
                    {
                        using(var ce = ct.GetColorEntry(i))
                            result.Add(Color.FromArgb(ce.c4, ce.c1, ce.c2, ce.c3));
                    }
                    ColorPalette = result;
                }
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
            foreach (var c in value)
            {
                ColorEntry ce = new ColorEntry
                {
                    c4 = c.A,
                    c3 = c.B,
                    c2 = c.G,
                    c1 = c.R
                };
                ct.SetColorEntry(index, ce);
                index++;
            }
            using (Band first = _dataset.GetRasterBand(1))
            {
                first.SetRasterColorTable(ct);
            }
        }

        private Bitmap ReadPaletteBuffered(int xOffset, int yOffset, int xSize, int ySize, Band first)
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

            Band first_o;
            bool dispose_o = false;
            if (_overview >= 0 && first.GetOverviewCount() > 0)
            {
                first_o = first.GetOverview(_overview);
                dispose_o = true;
            }
            else
            {
                first_o = first;
            }
           

            int width, height;
            NormalizeSizeToBand(xOffset, yOffset, xSize, ySize, first_o, out width, out height);
            byte[] r = new byte[width * height];

            first_o.ReadRaster(xOffset, yOffset, width, height, r, width, height, 0, 0);
            if (dispose_o)
            {
                first_o.Dispose();
            }

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
                    Array.Copy(colorTable[r[col + row*width]], 0, vals, row*stride + col*bpp, 4);
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

        private static void NormalizeSizeToBand(int xOffset, int yOffset, int xSize, int ySize, Band band, out int width, out int height)
        {
            width = xSize;
            height = ySize;

            if (xOffset + width > band.XSize)
            {
                width = band.XSize - xOffset;
            }
            if (yOffset + height > band.YSize)
            {
                height = band.YSize - yOffset;
            }
        }

        private Bitmap ReadGrayIndex(int xOffset, int yOffset, int xSize, int ySize, Band first)
        {
            Band first_o;
            var dispose_o = false;

            if (_overview >= 0 && first.GetOverviewCount() > 0)
            {
                first_o = first.GetOverview(_overview);
                dispose_o = true;
            }
            else
            {
                first_o = first;
            }

            int width, height;
            NormalizeSizeToBand(xOffset, yOffset, xSize, ySize, first_o, out width, out height);

            Bitmap result = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            byte[] r = new byte[width * height];
            first_o.ReadRaster(xOffset, yOffset, width, height, r, width, height, 0, 0);

            if (dispose_o)
            {
                first_o.Dispose();
            }

            BitmapData bData =
                result.LockBits(new Rectangle(0, 0, xSize, ySize),
                                ImageLockMode.ReadWrite,
                                PixelFormat.Format32bppArgb);
            int stride = Math.Abs(bData.Stride);

            byte[] vals = new byte[height * stride];

            for (int row = 0; row < height; row++)
                for (int col = 0; col < width; col++)
                {
                    byte value = r[row*width + col];
                    vals[row*stride + col*4] = value;
                    vals[row*stride + col*4 + 1] = value;
                    vals[row*stride + col*4 + 2] = value;
                    vals[row*stride + col*4 + 3] = 255;
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

        private Bitmap ReadRgb(int xOffset, int yOffset, int xSize, int ySize, Band first, Dataset set)
        {
            if (set.RasterCount < 3)
            {
                throw new GdalException(
                    "RGB Format was indicated but there are only " +
                    set.RasterCount +
                    " bands!");
            }
            Band first_o;
            Band green_o;
            Band blue_o;
            var dispose_o = false;

            var green = set.GetRasterBand(2);
            var blue = set.GetRasterBand(3);
            if (_overview >= 0 && first.GetOverviewCount() > 0)
            {
                first_o = first.GetOverview(_overview);
                green_o = green.GetOverview(_overview);
                blue_o = blue.GetOverview(_overview);
                dispose_o = true;
            }
            else
            {
                first_o = first;
                green_o = green;
                blue_o = blue;
            }

            int width, height;
            NormalizeSizeToBand(xOffset, yOffset, xSize, ySize, first_o, out width, out height);
            Bitmap result = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            
            byte[] r = new byte[width * height];
            byte[] g = new byte[width * height];
            byte[] b = new byte[width * height];

            first_o.ReadRaster(xOffset, yOffset, width, height, r, width, height, 0, 0);
            green_o.ReadRaster(xOffset, yOffset, width, height, g, width, height, 0, 0);
            blue_o.ReadRaster(xOffset, yOffset, width, height, b, width, height, 0, 0);

            if (dispose_o) // It's local copies, dispose them
            {
                first_o.Dispose();
                green_o.Dispose();
                blue_o.Dispose();
            }
            green.Dispose();
            blue.Dispose();

            var bData =
                result.LockBits(new Rectangle(0, 0, width, height),
                                ImageLockMode.ReadWrite,
                                PixelFormat.Format32bppArgb);
            int stride = Math.Abs(bData.Stride);

            var vals = new byte[width * height * 4];
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

        private Bitmap ReadArgb(int xOffset, int yOffset, int xSize, int ySize, Band first, Dataset set)
        {
            if (set.RasterCount < 4)
            {
                throw new GdalException(
                    "ARGB Format was indicated but there are only " +
                    set.RasterCount +
                    " bands!");
            }
            Band first_o;
            Band red_o;
            Band green_o;
            Band blue_o;
            var dispose_o = false;

            Band red = set.GetRasterBand(2);
            Band green = set.GetRasterBand(3);
            Band blue = set.GetRasterBand(4);

            if (_overview >= 0 && first.GetOverviewCount() > 0)
            {
                first_o = first.GetOverview(_overview);
                red_o = red.GetOverview(_overview);
                green_o = green.GetOverview(_overview);
                blue_o = blue.GetOverview(_overview);
                dispose_o = true;
            }
            else
            {
                first_o = first;
                red_o = red;
                green_o = green;
                blue_o = blue;
            }

            int width, height;
            NormalizeSizeToBand(xOffset, yOffset, xSize, ySize, first_o, out width, out height);
            Bitmap result = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            byte[] a = new byte[width * height];
            byte[] r = new byte[width * height];
            byte[] g = new byte[width * height];
            byte[] b = new byte[width * height];

            first_o.ReadRaster(0, 0, width, height, a, width, height, 0, 0);
            red_o.ReadRaster(0, 0, width, height, r, width, height, 0, 0);
            green_o.ReadRaster(0, 0, width, height, g, width, height, 0, 0);
            blue_o.ReadRaster(0, 0, width, height, b, width, height, 0, 0);

            if (dispose_o)
            {
                first_o.Dispose();
                red_o.Dispose();
                green_o.Dispose();
                blue_o.Dispose();
            }
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
                    vals[row * stride + col * 4] = b[row * width + col];
                    vals[row * stride + col * 4 + 1] = g[row * width + col];
                    vals[row * stride + col * 4 + 2] = r[row * width + col];
                    vals[row * stride + col * 4 + 3] = a[row * width + col];
                }
            }
            Marshal.Copy(vals, 0, bData.Scan0, vals.Length);
            result.UnlockBits(bData);
            return result;
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
            if (_dataset == null)
            {
                _dataset = Helpers.Open(Filename);
            }

            Bitmap result = null;
            using (var first = _dataset.GetRasterBand(1))
            {
                switch (first.GetRasterColorInterpretation())
                {
                    case ColorInterp.GCI_PaletteIndex:
                        result = ReadPaletteBuffered(xOffset, yOffset, xSize, ySize, first);
                        break;
                    case ColorInterp.GCI_GrayIndex:
                        result = ReadGrayIndex(xOffset, yOffset, xSize, ySize, first);
                        break;
                    case ColorInterp.GCI_RedBand:
                        result = ReadRgb(xOffset, yOffset, xSize, ySize, first, _dataset);
                        break;
                    case ColorInterp.GCI_AlphaBand:
                        result = ReadArgb(xOffset, yOffset, xSize, ySize, first, _dataset);
                        break;
                }
            }
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

            using (var first = _dataset.GetRasterBand(1))
            {
                switch (BandType)
                {
                    case ImageBandType.PalletCoded:
                        WritePaletteBuffered(value, xOffset, yOffset, first);
                        break;
                    case ImageBandType.Gray:
                        WriteGrayIndex(value, xOffset, yOffset, first);
                        break;
                    case ImageBandType.RGB:
                        WriteRgb(value, xOffset, yOffset, first, _dataset);
                        break;
                    case ImageBandType.ARGB:
                        WriteArgb(value, xOffset, yOffset, first, _dataset);
                        break;
                }
            }
        }

        /// <inheritdocs/>
        protected override void Dispose(bool disposeManagedResources)
        {
            // All class variables are unmanaged.
            if (_dataset != null)
            {
                try
                {
                    _dataset.FlushCache();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
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
        }
    }
}