// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Opp.
    /// </summary>
    public static class Opp
    {
        /// <summary>
        /// Calculates a new rectangle by using the input points to define the far corners.
        /// The position of the points doesn't matter.
        /// </summary>
        /// <param name="pt1">One of the points to form a rectangle with.</param>
        /// <param name="pt2">The second point to use when drawing a rectangle.</param>
        /// <returns>A Rectangle created from the points.</returns>
        public static Rectangle RectangleFromPoints(Point pt1, Point pt2)
        {
            int x = Math.Min(pt1.X, pt2.X);
            int y = Math.Min(pt1.Y, pt2.Y);
            int w = Math.Max(pt1.X, pt2.X) - x;
            int h = Math.Max(pt1.Y, pt2.Y) - y;
            return new Rectangle(x, y, w, h);
        }
    }
}