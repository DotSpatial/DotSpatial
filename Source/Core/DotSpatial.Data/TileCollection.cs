// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace DotSpatial.Data
{
    /// <summary>
    /// TileCollection.
    /// </summary>
    public class TileCollection : IEnumerable<IImageData>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TileCollection"/> class.
        /// </summary>
        /// <param name="width">The integer pixel width for the combined image at its maximum resolution.</param>
        /// <param name="height">The integer height in pixels for the combined image at its maximum resolution.</param>
        public TileCollection(int width, int height)
        {
            TileWidth = 5000;
            TileHeight = 5000;
            Width = width;
            Height = height;
            if (Width < TileWidth) TileWidth = width;
            if (Height < TileHeight) TileHeight = height;
            Tiles = new IImageData[NumTilesTall(), NumTilesWide()];
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the integer height in pixels for the combined image at its maximum resolution.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets the total number of tiles.
        /// </summary>
        public int NumTiles => NumTilesWide() * NumTilesTall();

        /// <summary>
        /// Gets the height of the standard sized tile, not counting the remainder.
        /// </summary>
        public int TileHeight { get; }

        /// <summary>
        /// Gets or sets the 2D array of tiles.
        /// </summary>
        public IImageData[,] Tiles { get; set; }

        /// <summary>
        /// Gets the width of the standard sized tile (not counting the remainder).
        /// </summary>
        public int TileWidth { get; }

        /// <summary>
        /// Gets or sets the integer pixel width for the combined image at its maximum resolution.
        /// </summary>
        public int Width { get; set; }

        #endregion

        #region Methods

        /// <inheritdoc />
        public IEnumerator<IImageData> GetEnumerator()
        {
            return new TileCollectionEnumerator(this);
        }

        /// <summary>
        /// Gets the height of the tile.
        /// </summary>
        /// <param name="row">The row number of the tile.</param>
        /// <returns>The height of the tile.</returns>
        public int GetTileHeight(int row)
        {
            if (row < NumTilesTall() - 1) return TileHeight;

            return Height - ((NumTilesTall() - 1) * TileHeight);
        }

        /// <summary>
        /// Gets the width of the tile.
        /// </summary>
        /// <param name="col">The column number of the tile.</param>
        /// <returns>The width of the tile.</returns>
        public int GetTileWidth(int col)
        {
            if (col < NumTilesWide() - 1) return TileWidth;

            return Width - ((NumTilesWide() - 1) * TileWidth);
        }

        /// <summary>
        /// Gets the integer number of tiles in the Y direction.
        /// </summary>
        /// <returns>Number of tiles in the Y direction.</returns>
        public int NumTilesTall()
        {
            return (int)Math.Ceiling(Height / (double)TileHeight);
        }

        /// <summary>
        /// Gets the number of tiles in the X direction.
        /// </summary>
        /// <returns>Number of tiles in the X direction.</returns>
        public int NumTilesWide()
        {
            return (int)Math.Ceiling(Width / (double)TileWidth);
        }

        /// <summary>
        /// Calls a method that calculates the propper image bounds for each of the extents of the tiles,
        /// given the affine coefficients for the whole image.
        /// </summary>
        /// <param name="affine"> x' = A + Bx + Cy; y' = D + Ex + Fy.</param>
        public void SetTileBounds(double[] affine)
        {
            double[] tileAffine = new double[6];
            for (int i = 0; i < 6; i++)
            {
                tileAffine[i] = affine[i];
            }

            if (Tiles == null) return;

            for (int row = 0; row < NumTilesTall(); row++)
            {
                for (int col = 0; col < NumTilesWide(); col++)
                {
                    int h = GetTileHeight(row);
                    int w = GetTileWidth(col);

                    // The rotation terms are the same, but the top-left values need to be shifted.
                    tileAffine[0] = affine[0] + (affine[1] * col * TileWidth) + (affine[2] * row * TileHeight);
                    tileAffine[3] = affine[3] + (affine[4] * col * TileWidth) + (affine[5] * row * TileHeight);
                    Tiles[row, col].Bounds = new RasterBounds(h, w, tileAffine);
                }
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Classes

        /// <summary>
        /// Enumerates the collection of tiles.
        /// </summary>
        private class TileCollectionEnumerator : IEnumerator<IImageData>
        {
            #region Fields

            private readonly IImageData[,] _tiles;

            private int _col;

            private int _row;

            #endregion

            #region Constructors

            /// <summary>
            /// Initializes a new instance of the <see cref="TileCollectionEnumerator"/> class.
            /// </summary>
            /// <param name="parent">The parent tileCollection.</param>
            public TileCollectionEnumerator(TileCollection parent)
            {
                _tiles = parent.Tiles;
                _row = 0;
                _col = -1;
            }

            #endregion

            #region Properties

            /// <inheritdoc />
            public IImageData Current => _tiles[_row, _col];

            object IEnumerator.Current => Current;

            #endregion

            #region Methods

            /// <inheritdoc />
            public void Dispose()
            {
                // Does nothing
            }

            /// <inheritdoc />
            public bool MoveNext()
            {
                do
                {
                    _col++;
                    if (_col > _tiles.GetUpperBound(1))
                    {
                        _row++;
                        _col = 0;
                    }

                    if (_row > _tiles.GetUpperBound(0)) return false;
                }
                while (_tiles[_row, _col] == null);

                return true;
            }

            /// <inheritdoc />
            public void Reset()
            {
                _row = 0;
                _col = 0;
            }

            #endregion
        }

        #endregion
    }
}