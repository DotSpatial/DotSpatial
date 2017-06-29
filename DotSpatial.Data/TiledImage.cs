// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
        #region Private Variables

        private string _fileName;
        private int _numBands;
        private int _stride;
        private TileCollection _tiles;
        private WorldFile _worldFile;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the TiledImage where the fileName is specified.
        /// This doesn't actually open the file until the Open method is called.
        /// </summary>
        /// <param name="fileName"></param>
        public TiledImage(string fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// Creates a new instance of TiledImage
        /// </summary>
        public TiledImage(int width, int height)
        {
            Init(width, height);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets or sets the array of tiles used for this
        /// </summary>
        public virtual IImageData[,] Tiles
        {
            get { return _tiles.Tiles; }
            set { _tiles.Tiles = value; }
        }

        /// <summary>
        ///
        /// </summary>
        protected TileCollection TileCollection
        {
            get { return _tiles; }
            set { _tiles = value; }
        }

        #region IImageData Members

        /// <summary>
        /// This should be overridden with custom file handling
        /// </summary>
        public virtual void Open()
        {
        }

        #endregion

        #region ITiledImage Members

        /// <inheritdoc />
        public virtual Bitmap GetBitmap(Extent envelope, Size pixelSize)
        {
            Bitmap result = new Bitmap(pixelSize.Width, pixelSize.Height);
            Graphics g = Graphics.FromImage(result);
            foreach (var image in GetImages())
            {
                Extent bounds = envelope.Intersection(image.Extent);

                Size ps = new Size((int)(pixelSize.Width * bounds.Width / envelope.Width),
                                   (int)(pixelSize.Height * bounds.Height / envelope.Height));
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
        public override void Close()
        {
            if (Tiles == null) return;
            foreach (IImageData image in Tiles)
            {
                if (image != null) image.Close();
            }
            base.Close();
        }

        #endregion

        /// <summary>
        /// Even if this TiledImage has already been constructed, we can initialize the tile collection later.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void Init(int width, int height)
        {
            _tiles = new TileCollection(width, height);
            TypeName = "TileImage";
            SpaceTimeSupport = SpaceTimeSupport.Spatial;
        }

        /// <summary>
        /// Calls a method that calculates the proper image bounds for each of the extents of the tiles,
        /// given the affine coefficients for the whole image.
        /// </summary>
        /// <param name="affine"> x' = A + Bx + Cy; y' = D + Ex + Fy</param>
        public void SetTileBounds(double[] affine)
        {
            _tiles.SetTileBounds(affine);
        }

        #endregion

        #region Properties

        /// <inheritdoc />
        public string Filename
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <inheritdoc />
        public int Height
        {
            get
            {
                return _tiles.Height;
            }
            set
            {
            }
        }

        /// <inheritdoc />
        public int Stride
        {
            get { return _stride; }
            set { _stride = value; }
        }

        /// <inheritdoc />
        public int Width
        {
            get
            {
                return _tiles.Width;
            }
            set
            {
            }
        }

        #endregion

        #region IImageData Members

        /// <summary>
        /// Gets or sets the number of bands
        /// </summary>
        public int NumBands
        {
            get { return _numBands; }
            set { _numBands = value; }
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
        public void CopyValues(IImageData source)
        {
            throw new NotImplementedException();
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

        /// <inheritdoc />
        public void CopyBitmapToValues()
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

        /// <inheritdoc />
        public void CopyValuesToBitmap()
        {
            throw new NotImplementedException();
        }

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

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="image"></param>
        public void SetBitmap(Bitmap image)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not Implemented.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Color> GetColorPalette()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="value"></param>
        public void SetColorPalette(IEnumerable<Color> value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <param name="xSize"></param>
        /// <param name="ySize"></param>
        /// <returns></returns>
        public Bitmap ReadBlock(int xOffset, int yOffset, int xSize, int ySize)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="value"></param>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        public void WriteBlock(Bitmap value, int xOffset, int yOffset)
        {
            throw new NotImplementedException();
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

        #endregion

        #region ITiledImage Members

        /// <inheritdoc />
        public int TileWidth
        {
            get { return _tiles.TileWidth; }
        }

        /// <inheritdoc />
        public int TileHeight
        {
            get { return _tiles.TileHeight; }
        }

        /// <inheritdoc />
        public int Count
        {
            get { return _tiles.NumTiles; }
        }

        /// <inheritdoc />
        public override Extent Extent
        {
            get
            {
                Extent ext = new Extent();
                foreach (IImageData image in _tiles)
                {
                    ext.ExpandToInclude(image.Extent);
                }
                return ext;
            }
        }

        /// <inheritdoc />
        public IEnumerable<IImageData> GetImages()
        {
            return _tiles;
        }

        /// <summary>
        /// Gets or sets the WorldFile for this set of tiles.
        /// </summary>
        public WorldFile WorldFile
        {
            get { return _worldFile; }
            set { _worldFile = value; }
        }

        #endregion

        /// <inheritdoc />
        protected override void Dispose(bool disposeManagedResources)
        {
            foreach (IImageData tile in _tiles)
            {
                tile.Dispose();
            }
            base.Dispose(disposeManagedResources);
        }
    }
}