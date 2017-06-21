using System;
using System.Drawing;
using System.IO;

namespace DotSpatial.Plugins.WebMap.Tiling
{
    internal class TileCache
    {
        private readonly string _fullPath;

        /// <summary>
        ///
        /// </summary>
        /// <param name="serviceName"></param>
        public TileCache(string serviceName)
        {
            ServiceName = serviceName;

            _fullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TileCache", serviceName);
            Directory.CreateDirectory(_fullPath);
        }

        public string ServiceName { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="zoom"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public Bitmap Get(int zoom, int x, int y)
        {
            try
            {
                var pathToTile = Path.Combine(_fullPath, zoom.ToString(), x.ToString());

                var monthPrior = DateTime.Today.Subtract(new TimeSpan(30, 0, 0, 0));
                var fi = new FileInfo(pathToTile);
                if (fi.CreationTime.CompareTo(monthPrior) < 0) //If the tile file is over 1 month old
                    return null;                                //then we want to return null so a new tile is downloaded.

                string filepath = Path.Combine(pathToTile, y.ToString());
                if (!File.Exists(filepath))
                    return null;

                var tile = new Bitmap(filepath);

                return tile;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public bool Put(Tile tile)
        {
            try
            {
                string pathToTile = Path.Combine(_fullPath, tile.ZoomLevel.ToString());
                pathToTile = Path.Combine(pathToTile, tile.X.ToString());

                Directory.CreateDirectory(pathToTile);

                tile.Bitmap.Save(Path.Combine(pathToTile, tile.Y.ToString()));

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}