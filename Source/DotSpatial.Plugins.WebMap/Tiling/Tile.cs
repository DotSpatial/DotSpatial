using System.Drawing;
using DotSpatial.Topology;
using Point = DotSpatial.Topology.Point;

namespace DotSpatial.Plugins.WebMap.Tiling
{
    public class Tile
    {
        public Tile(int x, int y, int zoomLevel, Envelope envelope, Bitmap bitmap)
        {
            X = x;
            Y = y;
            ZoomLevel = zoomLevel;
            Envelope = envelope;
            Bitmap = bitmap;
        }

        #region Properties

        public int X { get; set; }

        public int Y { get; set; }

        public int ZoomLevel { get; set; }

        public Envelope Envelope { get; set; }

        public Bitmap Bitmap { get; set; }

        public Point TileXY
        {
            get { return new Point(X, Y); }
        }

        #endregion
    }
}