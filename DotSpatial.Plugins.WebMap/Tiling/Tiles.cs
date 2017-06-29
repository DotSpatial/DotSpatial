using System.Drawing;
using DotSpatial.Topology;

namespace DotSpatial.Plugins.WebMap.Tiling
{
    internal class Tiles
    {
        public Tiles(Bitmap[,] bitmaps, Envelope topLeftTile, Envelope bottomRightTile)
        {
            BottomRightTile = bottomRightTile;
            TopLeftTile = topLeftTile;
            Bitmaps = bitmaps;
        }

        public Bitmap[,] Bitmaps { get; private set; }
        public Envelope TopLeftTile { get; private set; }
        public Envelope BottomRightTile { get; private set; }
    }
}