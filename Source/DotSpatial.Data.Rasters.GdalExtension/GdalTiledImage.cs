// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using OSGeo.GDAL;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// A tiled image based on GDAL data.
    /// </summary>
    public class GdalTiledImage : TiledImage
    {
        #region Fields

        private Band _alpha;
        private Band _blue;
        private Dataset _dataset;
        private Band _green;
        private Band _red;

        #endregion

        #region Constructors

        static GdalTiledImage()
        {
            GdalConfiguration.ConfigureGdal();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GdalTiledImage"/> class.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public GdalTiledImage(string fileName)
            : base(fileName)
        {
            WorldFile = new WorldFile
            {
                Affine = new double[6]
            };
            ReadHeader();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Closes the image
        /// </summary>
        public override void Close()
        {
            _dataset?.Dispose();
            _dataset = null;
            base.Close();
        }

        /// <summary>
        /// Attempts to open the specified file.
        /// </summary>
        public override void Open()
        {
            _dataset = Helpers.Open(Filename);
            _red = _dataset.GetRasterBand(1);
            if (_red.GetRasterColorInterpretation() == ColorInterp.GCI_PaletteIndex)
            {
                ReadPaletteBuffered();
            }

            if (_red.GetRasterColorInterpretation() == ColorInterp.GCI_GrayIndex)
            {
                ReadGrayIndex();
            }

            if (_red.GetRasterColorInterpretation() == ColorInterp.GCI_RedBand)
            {
                ReadRgb();
            }

            if (_red.GetRasterColorInterpretation() == ColorInterp.GCI_AlphaBand)
            {
                ReadArgb();
            }
        }

        private void ReadArgb()
        {
            if (_dataset.RasterCount < 4)
            {
                throw new GdalException("ARGB Format was indicated but there are only " + _dataset.RasterCount + " bands!");
            }

            _alpha = _red;
            _red = _dataset.GetRasterBand(2);
            _green = _dataset.GetRasterBand(3);
            _blue = _dataset.GetRasterBand(4);

            int tw = TileCollection.TileWidth;
            int th = TileCollection.TileHeight;
            for (int row = 0; row < TileCollection.NumTilesTall(); row++)
            {
                for (int col = 0; col < TileCollection.NumTilesWide(); col++)
                {
                    int width = TileCollection.GetTileWidth(col);
                    int height = TileCollection.GetTileHeight(row);
                    InRamImageData id = new InRamImageData(width, height);

                    Bitmap image = new Bitmap(width, height, PixelFormat.Format32bppArgb);

                    byte[] red = new byte[width * height];
                    byte[] g = new byte[width * height];
                    byte[] b = new byte[width * height];
                    byte[] a = new byte[width * height];

                    _red.ReadRaster(col * tw, row * th, width, height, red, width, height, 0, 0);
                    _green.ReadRaster(col * tw, row * th, width, height, g, width, height, 0, 0);
                    _blue.ReadRaster(col * tw, row * th, width, height, b, width, height, 0, 0);
                    _alpha.ReadRaster(col * tw, row * th, width, height, a, width, height, 0, 0);

                    BitmapData bData = image.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                    Stride = bData.Stride;
                    image.UnlockBits(bData);
                    byte[] vals = new byte[Width * Height * 4];
                    int stride = Stride;
                    const int Bpp = 4;
                    for (int r = 0; r < height; r++)
                    {
                        for (int c = 0; c < width; c++)
                        {
                            vals[(r * stride) + (c * Bpp)] = b[(r * width) + c];
                            vals[(r * stride) + (c * Bpp) + 1] = g[(r * width) + c];
                            vals[(r * stride) + (c * Bpp) + 2] = red[(r * width) + c];
                            vals[(r * stride) + (c * Bpp) + 3] = a[(r * width) + c];
                        }
                    }

                    id.Values = vals;
                    id.CopyValuesToBitmap();
                    TileCollection.Tiles[row, col] = id;
                }
            }

            SetTileBounds(Bounds.AffineCoefficients);
        }

        private void ReadGrayIndex()
        {
            int tw = TileCollection.TileWidth;
            int th = TileCollection.TileHeight;
            for (int row = 0; row < TileCollection.NumTilesTall(); row++)
            {
                for (int col = 0; col < TileCollection.NumTilesWide(); col++)
                {
                    int width = TileCollection.GetTileWidth(col);
                    int height = TileCollection.GetTileHeight(row);
                    InRamImageData id = new InRamImageData(width, height);
                    byte[] red = new byte[width * height];
                    _red.ReadRaster(col * tw, row * th, width, height, red, width, height, 0, 0);
                    Bitmap image = new Bitmap(width, height);
                    BitmapData bData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                    Stride = bData.Stride;
                    image.UnlockBits(bData);
                    byte[] vals = new byte[width * height * 4];

                    int stride = Stride;
                    const int Bpp = 4;

                    for (int r = 0; r < height; r++)
                    {
                        for (int c = 0; c < width; c++)
                        {
                            vals[(r * stride) + (c * Bpp)] = red[(r * width) + c];
                            vals[(r * stride) + (c * Bpp) + 1] = red[(r * width) + c];
                            vals[(r * stride) + (c * Bpp) + 2] = red[(r * width) + c];
                            vals[(r * stride) + (c * Bpp) + 3] = 255;
                        }
                    }

                    id.Values = vals;

                    id.CopyValuesToBitmap();
                    TileCollection.Tiles[row, col] = id;
                }
            }

            SetTileBounds(Bounds.AffineCoefficients);
        }

        private void ReadHeader()
        {
            _dataset = Helpers.Open(Filename);
            Init(_dataset.RasterXSize, _dataset.RasterYSize);
            NumBands = _dataset.RasterCount;
            WorldFile = new WorldFile
            {
                Affine = new double[6]
            };
            double[] test = new double[6];
            _dataset.GetGeoTransform(test);
            test = new AffineTransform(test).TransfromToCorner(0.5, 0.5); // shift origin by half a cell
            Bounds = new RasterBounds(Height, Width, test);
            WorldFile.Affine = test;
            Close();
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
                throw new GdalException("Only RGB palette interpreation is currently supported by this plugin, " + ct.GetPaletteInterpretation() + " is not supported.");
            }

            int tw = TileCollection.TileWidth;
            int th = TileCollection.TileHeight;
            for (int row = 0; row < TileCollection.NumTilesTall(); row++)
            {
                for (int col = 0; col < TileCollection.NumTilesWide(); col++)
                {
                    // takes into account that right and bottom tiles might be smaller.
                    int width = TileCollection.GetTileWidth(col);
                    int height = TileCollection.GetTileHeight(row);
                    ImageData id = new ImageData();
                    byte[] red = new byte[width * height];
                    _red.ReadRaster(col * tw, row * th, width, height, red, width, height, 0, 0);
                    Bitmap image = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                    BitmapData bData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                    Stride = bData.Stride;
                    image.UnlockBits(bData);

                    const int Bpp = 4;
                    int stride = Stride;
                    byte[] vals = new byte[width * height * 4];
                    byte[][] colorTable = new byte[256][];
                    for (int i = 0; i < 255; i++)
                    {
                        ColorEntry ce = ct.GetColorEntry(i);
                        colorTable[i] = new[] { (byte)ce.c3, (byte)ce.c2, (byte)ce.c1, (byte)ce.c4 };
                    }

                    for (int r = 0; r < height; r++)
                    {
                        for (int c = 0; c < width; c++)
                        {
                            Array.Copy(colorTable[red[c + (r * width)]], 0, vals, (row * stride) + (col * Bpp), 4);
                        }
                    }

                    id.Values = vals;
                    id.CopyValuesToBitmap();
                    TileCollection.Tiles[row, col] = id;
                }
            }

            SetTileBounds(Bounds.AffineCoefficients);
        }

        private void ReadRgb()
        {
            if (_dataset.RasterCount < 3)
            {
                throw new GdalException("RGB Format was indicated but there are only " + _dataset.RasterCount + " bands!");
            }

            _green = _dataset.GetRasterBand(2);
            _blue = _dataset.GetRasterBand(3);

            int tw = TileCollection.TileWidth;
            int th = TileCollection.TileHeight;
            int ntt = TileCollection.NumTilesTall();
            int ntw = TileCollection.NumTilesWide();
            ProgressMeter pm = new ProgressMeter(ProgressHandler, "Reading Tiles ", ntt * ntw);
            for (int row = 0; row < ntt; row++)
            {
                for (int col = 0; col < ntw; col++)
                {
                    int width = TileCollection.GetTileWidth(col);
                    int height = TileCollection.GetTileHeight(row);
                    InRamImageData id = new InRamImageData(width, height);

                    Bitmap image = new Bitmap(width, height, PixelFormat.Format32bppArgb);

                    byte[] red = new byte[width * height];
                    byte[] g = new byte[width * height];
                    byte[] b = new byte[width * height];

                    _red.ReadRaster(col * tw, row * th, width, height, red, width, height, 0, 0);
                    _green.ReadRaster(col * tw, row * th, width, height, g, width, height, 0, 0);
                    _blue.ReadRaster(col * tw, row * th, width, height, b, width, height, 0, 0);

                    BitmapData bData = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                    Stride = bData.Stride;
                    image.UnlockBits(bData);
                    byte[] vals = new byte[width * height * 4];
                    int stride = Stride;
                    const int Bpp = 4;
                    for (int r = 0; r < height; r++)
                    {
                        for (int c = 0; c < width; c++)
                        {
                            vals[(r * stride) + (c * Bpp)] = b[(r * width) + c];
                            vals[(r * stride) + (c * Bpp) + 1] = g[(r * width) + c];
                            vals[(r * stride) + (c * Bpp) + 2] = red[(r * width) + c];
                            vals[(r * stride) + (c * Bpp) + 3] = 255;
                        }
                    }

                    id.Values = vals;
                    id.CopyValuesToBitmap();
                    TileCollection.Tiles[row, col] = id;
                    pm.CurrentValue = (row * ntw) + col;
                }
            }

            pm.Reset();
            SetTileBounds(Bounds.AffineCoefficients);
        }

        #endregion
    }
}