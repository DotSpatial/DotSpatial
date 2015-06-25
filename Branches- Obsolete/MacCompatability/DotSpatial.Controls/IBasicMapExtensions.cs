using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using DotSpatial.Data;
using DotSpatial.Data.Forms;
using DotSpatial.Projections;
using DotSpatial.Serialization;
using DotSpatial.Symbology;
using DotSpatial.Topology;
using Point = System.Drawing.Point;
using SelectionMode = DotSpatial.Symbology.SelectionMode;

namespace DotSpatial.Controls
{
    public static class IBasicMapExtensions
    {
        /// <summary>
        /// Converts a single point location into an equivalent geographic coordinate
        /// </summary>
        /// <param name="position">The client coordinate relative to the map control</param>
        /// <returns>The geographic ICoordinate interface</returns>
        public static Coordinate PixelToProj(this IBasicMap map, Point position)
        {
            var mapFrame = map.MapFrame as MapFrame;
            if (mapFrame != null)
                return mapFrame.PixelToProj (position);
            else
                return Coordinate.Empty;
        }

        /// <summary>
        /// Converts a rectangle in pixel coordinates relative to the map control into
        /// a geographic envelope.
        /// </summary>
        /// <param name="rect">The rectangle to convert</param>
        /// <returns>An IEnvelope interface</returns>
        public static Extent PixelToProj(this IBasicMap map, Rectangle rect)
        {
            var mapFrame = map.MapFrame as MapFrame;
            if (mapFrame != null) 
                return mapFrame.PixelToProj(rect);
            else
                return null;
        }

        /// <summary>
        /// Converts a single geographic location into the equivalent point on the
        /// screen relative to the top left corner of the map.
        /// </summary>
        /// <param name="location">The geographic position to transform</param>
        /// <returns>A Point with the new location.</returns>
        public static Point ProjToPixel(this IBasicMap map, Coordinate location)
        {
            var mapFrame = map.MapFrame as MapFrame;
            if (mapFrame != null) 
                return mapFrame.ProjToPixel(location);
            else
                return Point.Empty;
        }

        /// <summary>
        /// Converts a single geographic envelope into an equivalent Rectangle
        /// as it would be drawn on the screen.
        /// </summary>
        /// <param name="env">The geographic IEnvelope</param>
        /// <returns>A Rectangle</returns>
        public static Rectangle ProjToPixel(this IBasicMap map, Extent env)
        {
            var mapFrame = map.MapFrame as MapFrame;
            if (mapFrame != null) 
                return mapFrame.ProjToPixel(env);
            else
                return Rectangle.Empty;
        }

        /// <summary>
        /// Instructs the map to update the specified clipRectangle by drawing it to the back buffer.
        /// </summary>
        /// <param name="clipRectangle"></param>
        public static void RefreshMap(this IBasicMap map, Rectangle clipRectangle)
        {
            var mapFrame = map.MapFrame as MapFrame;
            if (mapFrame != null) {
                Extent region = (map.MapFrame as MapFrame).BufferToProj (clipRectangle);
                map.MapFrame.Invalidate (region);
            }
        }

        /// <summary>
        /// Instructs the map to change the perspective to include the entire drawing content, and
        /// in the case of 3D maps, changes the perspective to look from directly overhead.
        /// </summary>
        public static void ZoomToMaxExtent(this IBasicMap map)
        {
            // to prevent exception when zoom to map with one layer with one point
            const double eps = 1e-7;
            if (map.Extent.Width < eps || map.Extent.Height < eps)
            {
                var newExtent = new Extent(map.Extent.MinX - eps, map.Extent.MinY - eps, map.Extent.MaxX + eps, map.Extent.MaxY + eps);
                map.ViewExtents = newExtent;
            }
            else
            {
                map.ViewExtents = map.Extent;
            }

            map.IsZoomedToMaxExtent = true;
        }

        //  Added by Eric Hullinger 12/28/2012 for use in preventing zooming out too far.
        /// <summary> 
        /// Gets the MaxExtent Window of the current Map
        /// </summary>
        public static Extent GetMaxExtent(this IBasicMap map)
        {
            // to prevent exception when zoom to map with one layer with one point
            const double eps = 1e-7;
            var maxExtent = map.Extent.Width < eps || map.Extent.Height < eps
                ? new Extent(map.Extent.MinX - eps, map.Extent.MinY - eps, map.Extent.MaxX + eps, map.Extent.MaxY + eps)
                : map.Extent;
            return maxExtent;
        }
    }
}

