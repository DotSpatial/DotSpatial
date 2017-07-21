// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing.Drawing2D;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Extension methods for IFeatureSymbolizer.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class IFeatureSymbolizerExt
    {
        /// <summary>
        /// Calculates the scale based on the IFeatureSymbolizers ScaleMode.
        /// </summary>
        /// <param name="self">The IFeatureSymbolizer.</param>
        /// <param name="args">The MapArgs needed for calculation.</param>
        /// <returns>Returns args.ImageRectangle.Width / args.GeographicExtents.Width if ScaleMode is Geographic, otherwise this returns 1.</returns>
        public static double GetScale(this IFeatureSymbolizer self, MapArgs args)
        {
            if (self.ScaleMode == ScaleMode.Geographic)
            {
                return args.ImageRectangle.Width / args.GeographicExtents.Width;
            }

            return 1;
        }

        /// <summary>
        /// Gets the smoothing mode based on the IFeatureSymbolizer.
        /// </summary>
        /// <param name="self">The IFeatureSymbolizer.</param>
        /// <returns>Returns AntiAlias if smothing is true, otherwise none.</returns>
        public static SmoothingMode GetSmoothingMode(this IFeatureSymbolizer self)
        {
            return self.Smoothing ? SmoothingMode.AntiAlias : SmoothingMode.None;
        }
    }
}
