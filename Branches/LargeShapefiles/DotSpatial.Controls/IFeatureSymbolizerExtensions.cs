using System;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    internal static class IFeatureSymbolizerExtensions
    {
        public static double GetScale(this IFeatureSymbolizer symbolizer, IProj e)
        {
            if (symbolizer == null) throw new ArgumentNullException("symbolizer");

            double scaleSize = 1;
            if (symbolizer.ScaleMode == ScaleMode.Geographic)
            {
                scaleSize = e.ImageRectangle.Width / e.GeographicExtents.Width;
            }

            return scaleSize;
        }
    }
}