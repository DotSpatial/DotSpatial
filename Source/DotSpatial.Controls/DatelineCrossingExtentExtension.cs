using System;
using DotSpatial.Data;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Extension of Extent class to deal with dateline crossing.
    /// </summary>
    public static class DatelineCrossingExtentExtension
    {
        #region Methods

        /// <summary>
        /// Indicates whether the extent crosses the date line.
        /// </summary>
        /// <param name="extent">Extent that is checked.</param>
        /// <returns>True, if the extent crosses the date line.</returns>
        public static bool IsCrossDateline(this Extent extent)
        {
            return extent.MaxX > 180.0;
        }

        /// <summary>Modifies the extent such that its left edge is normalised into the range [-180..180] degrees.
        /// Its width remains constant unless it was originally greater than 360 degrees,
        /// in which case it is scaled to 360 degrees width and retains its aspect ratio.</summary>
        /// <param name="extent">Extent that gets normalised.</param>
        /// <returns>The normalised extent.</returns>
        public static Extent Normalised(this Extent extent)
        {
            var newExtent = (Extent)extent.Clone();
            if (newExtent.Width > 360.0)
            {
                const double NewWidth = 360.0;
                double newHeight = newExtent.Height * NewWidth / newExtent.Width;
                double xOffset = (newExtent.Width - NewWidth) / 2.0;
                double yOffset = (newExtent.Height - newHeight) / 2.0;
                newExtent.Width = NewWidth;
                newExtent.Height = newHeight;
                newExtent.X += xOffset;
                newExtent.Y -= yOffset; // pinned to top left.
            }

            while (newExtent.X < -180)
            {
                double x = newExtent.X + 360.0;
                if (x == newExtent.X)
                    throw new ArgumentException(MessageStrings.ExtentXIsTooLargeForDegreesToBeSignificant);
                newExtent.X = x;
            }

            while (newExtent.X > 180)
            {
                double x = newExtent.X - 360.0;
                if (x == newExtent.X)
                    throw new ArgumentException(MessageStrings.ExtentXIsTooLargeForDegreesToBeSignificant);
                newExtent.X = x;
            }

            return newExtent;
        }

        #endregion
    }
}