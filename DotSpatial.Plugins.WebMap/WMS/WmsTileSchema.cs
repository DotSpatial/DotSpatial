using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using BruTile;

namespace DotSpatial.Plugins.WebMap.WMS
{
    class WmsTileSchema : TileSchema
    {
        public WmsTileSchema(string name, BruTile.Extent extent, string srs, string format, int tileSize, double highestResUnitsPerPixel, AxisDirection axis)
        {
            // Make us a new extent that is square
            double minx, miny, maxx, maxy, size;
            if (extent.Width > extent.Height)
            {
                minx = extent.MinX;
                maxx = extent.MaxX;
                size = maxx - minx;
                miny = extent.MinY;
                maxy = miny + size;

            }
            else
            {
                miny = extent.MinY;
                maxy = extent.MaxY;
                size = maxy - miny;
                minx = extent.MinX;
                maxx = minx + size;
            }
            int count = 0;
            double unitsPerPixel = size / tileSize;
            do
            {
                Resolutions[count.ToString(CultureInfo.InvariantCulture)] = new Resolution {Id = count.ToString(CultureInfo.InvariantCulture), UnitsPerPixel = unitsPerPixel};
                count++;
                //Resolutions.Add(unitsPerPixel);
                unitsPerPixel /= 2;
            } while (unitsPerPixel > highestResUnitsPerPixel);
            BruTile.Extent myExtent = new BruTile.Extent(minx, miny, maxx, maxy);
            //Extent myExtent = extent;
            Width = tileSize;
            Height = tileSize;
            Extent = myExtent;
            OriginX = myExtent.MinX;
            OriginY = myExtent.MinY;
            Name = name;
            Format = format;
            Axis = axis;
            Srs = srs;
        }
    }
}
