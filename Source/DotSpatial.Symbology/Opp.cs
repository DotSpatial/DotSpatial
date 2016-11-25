// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/29/2008 3:30:45 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;

namespace DotSpatial.Symbology
{
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