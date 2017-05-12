// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/8/2010 10:01:52 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Collections.Generic;
using System.Drawing;

namespace DotSpatial.Data
{
    /// <summary>
    /// TiledImage is a special kind of image coverage where the images specifically form tiles that
    /// are adjacent and perfectly aligned to represent a larger image.
    /// </summary>
    public class TiledImage : RasterBoundDataSet, ITiledImage, IImageData
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TiledImage"/> class where the fileName is specified.
        /// This doesn't actually open the file until the Open method is called.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        public TiledImage(string fileName)
        {
            Filename = fileName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TiledImage"/> class.
        /// </summary>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        public TiledImage(int width, int height)
        {
            Init(width, height);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the band type. This is not implemented.
        /// </summary>
        public ImageBandType BandType
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc />
        public int BytesPerPixel
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc />
        public int Count => TileCollection.NumTiles;

        /// <inheritdoc />
        public override Extent Extent
        {
            get
            {
                Extent ext = new Extent();
                foreach (IImageData image in TileCollection)
                {
                    ext.ExpandToInclude(image.Extent);
                }

                return ext;
            }
        }

        /// <inheritdoc />
        public int Height
        {
            get
            {
                return TileCollection.Height;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the number of bands.
        /// </summary>
        public int NumBands { get; set; }

        /// <inheritdoc />
        public int Stride { get; set; }

        /// <inheritdoc />
        public int TileHeight => TileCollection.TileHeight;

        /// <summary>
        /// Gets or sets the array of tiles used for this.
        /// </summary>
        public virtual IImageData[,] Tiles
        {
            get
            {
                return TileCollection.Tiles;
            }

            set
            {
                TileCollection.Tiles = value;
            }
        }

        /// <inheritdoc />
        public int TileWidth => TileCollection.TileWidth;

        /// <inheritdoc />
        public byte[] Values
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <inheritdoc />
        public int Width
        {
            get
            {
                return TileCollection.Width;
            }

            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the WorldFile for this set of tiles.
        /// </summary>
        public WorldFile WorldFile { get; set; }

        /// <summary>
        /// Gets or sets the tile collection.
        /// </summary>
        protected TileCollection TileCollection { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public override void Close()
        {
            if (Tiles == null) return;

            foreach (IImageData image in Tiles)
            {
                image?.Close();
            }

            base.Close();
        }

        /// <inheritdoc />
        public void CopyBitmapToValues()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void CopyValues(IImageData source)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void CopyValuesToBitmap()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual Bitmap GetBitmap(Extent envelope, Size pixelSize)
        {
            Bitmap result = new Bitmap(pixelSize.Width, pixelSize.Height);
            Graphics g = Graphics.FromImage(result);
            foreach (var image in GetImages())
            {
                Extent bounds = envelope.Intersection(image.Extent);

                Size ps = new Size((int)(pixelSize.Width * bounds.Width / envelope.Width), (int)(pixelSize.Height * bounds.Height / envelope.Height));
                int x = pixelSize.Width * (int)((bounds.X - envelope.X) / envelope.Width);
                int y = pixelSize.Height * (int)((envelope.Y - bounds.Y) / envelope.Height);
                if (ps.Width > 0 && ps.Height > 0)
                {
                    Bitmap tile = image.GetBitmap(bounds, ps);
                    g.DrawImageUnscaled(tile, x, y);
                }
            }

            return result;
        }

        /// <inheritdoc />
        public Bitmap GetBitmap()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Bitmap GetBitmap(Extent envelope, Rectangle window)
        {
            return GetBitmap(envelope, new Size(window.Width, window.Height));
        }

        /// <inheritdoc />
        public Color GetColor(int row, int column)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <returns>The color palette.</returns>
        public IEnumerable<Color> GetColorPalette()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerable<IImageData> GetImages()
        {
            return TileCollection;
        }

        /// <summary>
        /// Even if this TiledImage has already been constructed, we can initialize the tile collection later.
        /// </summary>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        public void Init(int width, int height)
        {
            TileCollection = new TileCollection(width, height);
        }

        /// <summary>
        /// This should be overridden with custom file handling
        /// </summary>
        public virtual void Open()
        {
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="xOffset">The x offset.</param>
        /// <param name="yOffset">The y offset.</param>
        /// <param name="xSize">The x size.</param>
        /// <param name="ySize">The y size.</param>
        /// <returns>A bitmap.</returns>
        public Bitmap ReadBlock(int xOffset, int yOffset, int xSize, int ySize)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Save()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void SaveAs(string fileName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void SetColor(int row, int column, Color col)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="value">The color palette.</param>
        public void SetColorPalette(IEnumerable<Color> value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calls a method that calculates the proper image bounds for each of the extents of the tiles,
        /// given the affine coefficients for the whole image.
        /// </summary>
        /// <param name="affine"> x' = A + Bx + Cy; y' = D + Ex + Fy</param>
        public void SetTileBounds(double[] affine)
        {
            TileCollection.SetTileBounds(affine);
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        public void UpdateOverviews()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="xOffset">The x offset.</param>
        /// <param name="yOffset">The y offset.</param>
        public void WriteBlock(Bitmap value, int xOffset, int yOffset)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposeManagedResources)
        {
            foreach (IImageData tile in TileCollection)
            {
                tile.Dispose();
            }

            base.Dispose(disposeManagedResources);
        }

        #endregion
    }
}