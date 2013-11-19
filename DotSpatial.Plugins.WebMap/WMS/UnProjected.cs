using System.Globalization;
using BruTile;

namespace DotSpatial.Plugins.WebMap.WMS
{
    public class UnProjected : TileSchema
    {
        public UnProjected()
        {
            var resolutions = new[] { 
                                        0.703125, 0.3515625, 0.17578125, 0.087890625,
                                        0.0439453125, 0.02197265625, 0.010986328125, 0.0054931640625,
                                        0.00274658203125, 0.001373291015625, 0.0006866455078125, 3.4332275390625e-4,
                                        1.71661376953125e-4, 8.58306884765625e-5, 4.291534423828125e-5
                                    };

            var count = 0;
            foreach (var resolution in resolutions)
            {
                Resolutions[count.ToString(CultureInfo.InvariantCulture)] = new Resolution
                {
                    Id = count.ToString(CultureInfo.InvariantCulture),
                    UnitsPerPixel = resolution
                };
                count++;
            }
            Height = 256;
            Width = 256;
            Extent = new Extent(-180, -90, 180, 90);
            OriginX = -180;
            OriginY = -90;
            Name = "UnProjected";
            Format = "image/png";
            Axis = AxisDirection.Normal;
            Srs = "EPSG:4326";
        }
    }
}