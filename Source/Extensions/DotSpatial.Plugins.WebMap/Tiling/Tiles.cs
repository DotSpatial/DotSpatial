// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using NetTopologySuite.Geometries;

namespace DotSpatial.Plugins.WebMap.Tiling
{
    /// <summary>
    /// Tiles can be used to return the tiles that the provider returned.
    /// </summary>
    internal class Tiles
    {
        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Tiles"/> class.
        /// </summary>
        /// <param name="bitmaps">An array of bitmaps.</param>
        /// <param name="topLeftTile">The top left tile.</param>
        /// <param name="bottomRightTile">The bottom right tile.</param>
        public Tiles(Bitmap[,] bitmaps, Envelope topLeftTile, Envelope bottomRightTile)
        {
            BottomRightTile = bottomRightTile;
            TopLeftTile = topLeftTile;
            Bitmaps = bitmaps;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets an array of bitmaps.
        /// </summary>
        public Bitmap[,] Bitmaps { get; private set; }

        /// <summary>
        /// Gets the bottom right tile.
        /// </summary>
        public Envelope BottomRightTile { get; private set; }

        /// <summary>
        /// Gets the top left tile.
        /// </summary>
        public Envelope TopLeftTile { get; private set; }

        #endregion
    }
}