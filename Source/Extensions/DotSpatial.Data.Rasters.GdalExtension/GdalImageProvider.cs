// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.IO;
using System.Windows.Forms;
using OSGeo.GDAL;

namespace DotSpatial.Data.Rasters.GdalExtension
{
    /// <summary>
    /// GDalImageProvider acts as the factory to create IImageData files that use the GDAL libraries.
    /// </summary>
    public class GdalImageProvider : IImageDataProvider
    {
        #region Constructors

        static GdalImageProvider()
        {
            GdalConfiguration.ConfigureGdal();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the description.
        /// </summary>
        public string Description => "Images supported by GDAL";

        /// <summary>
        /// Gets the dialog read filter.
        /// </summary>
        public string DialogReadFilter => "Images|*.bmp;*.jpg;*.gif;*.gen;*.thf;*.blx;*.xlb;*.kap;*.bag;*.bt;*.doq;*.dt0;*.dt2;*.ers;*.n1;*.fits;*.hdr;*.grb;*.img;*.mpr;*.mpl;*.j2k;*.tif;*.sid;*.ecw;*.jp2;*.png;*.ppm;*.pgm;*.rik;*.rsw;*.mtw;*.ddf;*.ter;*.dem;*.toc";

        /// <summary>
        /// Gets the dialog write filter.
        /// </summary>
        public string DialogWriteFilter => "Images|*.bmp;*.jpg;*.gif;*.gen;*.thf;*.blx;*.xlb;*.bag;*.kap;*.bt;*.doq;*.dt0;*.dt2;*.ers;*.n1;*.fits;*.hdr;*.grb;*.img;*.mpr;*.mpl;*.j2k;*.tif;*.sid;*.ecw;*.jp2;*.png;*.ppm;*.pgm;*.rik;*.rsw;*.mtw;*.ddf;*.ter;*.dem;*.toc";

        /// <summary>
        /// Gets the string name.
        /// </summary>
        public string Name => "GDAL";

        /// <summary>
        /// Gets or sets the progress handler.
        /// </summary>
        public IProgressHandler ProgressHandler { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new image given the specified file format.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="inRam">if set to <c>true</c> should load entire file in ram.</param>
        /// <param name="progHandler">The prog handler.</param>
        /// <param name="bandType">Type of the band.</param>
        /// <returns>The created image data.</returns>
        public IImageData Create(string fileName, int width, int height, bool inRam, IProgressHandler progHandler, ImageBandType bandType)
        {
            Driver d = GetDriverByExtension(fileName);
            if (d == null) return null;
            Dataset ds = bandType switch
            {
                ImageBandType.ARGB => d.Create(fileName, width, height, 4, DataType.GDT_Byte, new string[] { }),
                ImageBandType.RGB => d.Create(fileName, width, height, 3, DataType.GDT_Byte, new string[] { }),
                ImageBandType.PalletCoded => d.Create(fileName, width, height, 1, DataType.GDT_Byte, new string[] { }),
                _ => d.Create(fileName, width, height, 1, DataType.GDT_Byte, new string[] { }),
            };
            return new GdalImage(fileName, ds, bandType);
        }

        /// <summary>
        /// Opens an existing file using the specified parameters.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The opened file as IImageData.</returns>
        public IImageData Open(string fileName)
        {
            return OpenFile(fileName);
        }

        /// <summary>
        /// Opens an existing file using the specified parameters.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The opened file as IDataSEt.</returns>
        IDataSet IDataProvider.Open(string fileName)
        {
            return OpenFile(fileName);
        }

        /// <summary>
        /// http://www.gdal.org/formats_list.html.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The driver.</returns>
        private static Driver GetDriverByExtension(string fileName)
        {
            string ext = Path.GetExtension(fileName);
            if (ext != null)
            {
                ext = ext.Replace(".", string.Empty).ToLower();
                switch (ext)
                {
                    case @"asc": return Gdal.GetDriverByName("AAIGrid");
                    case @"gen":
                    case @"thf": return Gdal.GetDriverByName("ADRG");
                    case @"adf": return Gdal.GetDriverByName("AIG");
                    case @"blx":
                    case @"bxl": return Gdal.GetDriverByName("BLX");
                    case @"bag": return Gdal.GetDriverByName("BAG");
                    case @"bmp": return Gdal.GetDriverByName("BMP");
                    case @"kap": return Gdal.GetDriverByName("BSB");
                    case @"bt": return Gdal.GetDriverByName("BT");
                    case @"dim": return Gdal.GetDriverByName("DIM");
                    case @"doq": return Gdal.GetDriverByName("DOQ2");
                    case @"dt0":
                    case @"dt1":
                    case @"dt2": return Gdal.GetDriverByName("DTED");
                    case @"ecw": return Gdal.GetDriverByName("ECW");
                    case @"htr": return Gdal.GetDriverByName("EHdr");
                    case @"ers": return Gdal.GetDriverByName("ERS");
                    case @"nl": return Gdal.GetDriverByName("ESAT");
                    case @"gif": return Gdal.GetDriverByName("GIF");
                    case @"tif": return Gdal.GetDriverByName("GTiff");
                    case @"jpg": return Gdal.GetDriverByName("JPEG");
                    case @"jp2":
                    case @"j2k": return Gdal.GetDriverByName("JPEG2000");
                    case @"ppm":
                    case @"pgm": return Gdal.GetDriverByName("PNM");
                    case @"png": return Gdal.GetDriverByName("PNG");
                    case @"rik": return Gdal.GetDriverByName("RIK");
                    case @"rsw":
                    case @"mtw": return Gdal.GetDriverByName("RMF");
                    case @"ter": return Gdal.GetDriverByName("TERRAGEN");
                    case @"dem": return Gdal.GetDriverByName("USGSDEM");
                    case @".vrt": return Gdal.GetDriverByName("VRT");
                    case @"xpm": return Gdal.GetDriverByName("XPM");
                }
            }
            else
            {
                return Gdal.GetDriverByName("AAIGrid");
            }

            return null;
        }

        private IImageData OpenFile(string fileName)
        {
            var dataset = Helpers.Open(fileName);
            bool hasOverviews;
            using (var red = dataset.GetRasterBand(1))
            {
                ColorInterp bandType = red.GetRasterColorInterpretation();
                if (bandType != ColorInterp.GCI_PaletteIndex && bandType != ColorInterp.GCI_GrayIndex && bandType != ColorInterp.GCI_RedBand && bandType != ColorInterp.GCI_AlphaBand)
                {
                    // This is an image, not a raster, so return null.
                    dataset.Dispose();
                    return null;
                }

                hasOverviews = red.GetOverviewCount() > 0;
            }

            GdalImage result = new(fileName);

            // Firstly, if there are pyramids inside of the GDAL file itself, we can just work with this directly,
            // without creating our own pyramid image.
            if ((result.Width > 8000 || result.Height > 8000) && !hasOverviews)
            {
                // For now, we can't get fast, low-res versions without some kind of pyramiding happening.
                // since that can take a while for huge images, I'd rather do this once, and create a kind of
                // standardized file-based pyramid system.  Maybe in future pyramid tiffs could be used instead?
                string pyrFile = Path.ChangeExtension(fileName, ".mwi");
                if (pyrFile != null && File.Exists(pyrFile))
                {
                    if (File.Exists(Path.ChangeExtension(pyrFile, ".mwh")))
                    {
                        return new PyramidImage(fileName);
                    }

                    File.Delete(pyrFile);
                }

                GdalImageSource gs = new(fileName);
                PyramidImage py = new(pyrFile, gs.Bounds);
                int width = gs.Bounds.NumColumns;
                int blockHeight = 64000000 / width;
                if (blockHeight > gs.Bounds.NumRows) blockHeight = gs.Bounds.NumRows;
                int numBlocks = (int)Math.Ceiling(gs.Bounds.NumRows / (double)blockHeight);
                ProgressMeter pm = new(ProgressHandler, "Copying Data To Pyramids", numBlocks * 2);

                Application.DoEvents();
                for (int j = 0; j < numBlocks; j++)
                {
                    int h = blockHeight;
                    if (j == numBlocks - 1)
                    {
                        h = gs.Bounds.NumRows - (j * blockHeight);
                    }

                    byte[] vals = gs.ReadWindow(j * blockHeight, 0, h, width, 0);

                    pm.CurrentValue = (j * 2) + 1;
                    py.WriteWindow(vals, j * blockHeight, 0, h, width, 0);
                    pm.CurrentValue = (j + 1) * 2;
                }

                gs.Dispose();
                pm.Reset();
                py.ProgressHandler = ProgressHandler;
                py.CreatePyramids();
                py.WriteHeader(pyrFile);
                result.Dispose();

                return py;
            }

            result.Open();
            return result;
        }

        #endregion
    }
}