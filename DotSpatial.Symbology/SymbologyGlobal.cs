// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2008 8:49:44 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Global has some basic methods that may be useful in lots of places.
    /// </summary>
    public static class SymbologyGlobal
    {
        /// <summary>
        /// An instance of Random that is created when needed and sits around so we don't keep creating new ones.
        /// </summary>
        private static readonly Random _defaultRandom = new Random();

        /// <summary>
        /// Gets a cool Highlight brush for highlighting things
        /// </summary>
        /// <param name="box">The rectangle in the box</param>
        /// <param name="selectionHighlight">The color to use for the higlight</param>
        /// <returns></returns>
        public static Brush HighlightBrush(Rectangle box, Color selectionHighlight)
        {
            float med = selectionHighlight.GetBrightness();
            float bright = med + 0.05f;
            if (bright > 1f) bright = 1f;
            float dark = med - 0.05f;
            if (dark < 0f) dark = 0f;
            Color brtCol = ColorFromHsl(selectionHighlight.GetHue(), selectionHighlight.GetSaturation(), bright);
            Color drkCol = ColorFromHsl(selectionHighlight.GetHue(), selectionHighlight.GetSaturation(), dark);
            return new LinearGradientBrush(box, brtCol, drkCol, LinearGradientMode.Vertical);
        }

        /// <summary>
        /// Draws a rectangle with ever so slightly rounded edges.  Good for selection borders.
        /// </summary>
        /// <param name="g">The Graphics object</param>
        /// <param name="pen">The pen to draw with</param>
        /// <param name="rect">The rectangle to draw to.</param>
        public static void DrawRoundedRectangle(Graphics g, Pen pen, Rectangle rect)
        {
            int l = rect.Left;
            int r = rect.Right;
            int t = rect.Top;
            int b = rect.Bottom;
            g.DrawLine(pen, l + 1, t, r - 1, t);
            g.DrawLine(pen, l + 1, b, r - 1, b);
            g.DrawLine(pen, l, t + 1, l, b - 1);
            g.DrawLine(pen, r, t + 1, r, b - 1);

            g.DrawLine(pen, l, t + 2, l + 2, t);
            g.DrawLine(pen, r - 2, t, r, t + 2);
            g.DrawLine(pen, l, b - 2, l + 2, b);
            g.DrawLine(pen, r, b - 2, r - 2, b);
        }

        /// <summary>
        /// Obtains a system.Drawing.Rectangle based on the two points, using them as
        /// opposite extremes for the rectangle.
        /// </summary>
        /// <param name="a">one corner point of the rectangle.</param>
        /// <param name="b">The opposing corner of the rectangle.</param>
        /// <returns>A System.Draing.Rectangle</returns>
        public static Rectangle GetRectangle(Point a, Point b)
        {
            int x = Math.Min(a.X, b.X);
            int y = Math.Min(a.Y, b.Y);
            int w = Math.Abs(a.X - b.X);
            int h = Math.Abs(a.Y - b.Y);
            return new Rectangle(x, y, w, h);
        }

        /// <summary>
        /// Returns a completely random opaque color.
        /// </summary>
        /// <returns>A random color.</returns>
        public static Color RandomColor()
        {
            return Color.FromArgb(_defaultRandom.Next(0, 255), _defaultRandom.Next(0, 255), _defaultRandom.Next(0, 255));
        }

        /// <summary>
        /// This allows the creation of a transparent color with the specified opacity.
        /// </summary>
        /// <param name="opacity">A float ranging from 0 for transparent to 1 for opaque</param>
        /// <returns>A Color</returns>
        public static Color RandomTranslucent(float opacity)
        {
            int alpha = Convert.ToInt32(opacity * 255);
            if (alpha > 255) alpha = 255;
            if (alpha < 0) alpha = 0;
            return Color.FromArgb(alpha, _defaultRandom.Next(0, 255), _defaultRandom.Next(0, 255), _defaultRandom.Next(0, 255));
        }

        /// <summary>
        /// This allows the creation of a transparent color with the specified opacity.
        /// </summary>
        /// <param name="opacity">A float ranging from 0 for transparent to 1 for opaque</param>
        /// <returns>A Color</returns>
        public static Color RandomLightColor(float opacity)
        {
            int alpha = Convert.ToInt32(opacity * 255);
            if (alpha > 255) alpha = 255;
            if (alpha < 0) alpha = 0;
            return Color.FromArgb(alpha, ColorFromHsl(_defaultRandom.Next(0, 360), ((double)_defaultRandom.Next(0, 255) / 256), ((double)_defaultRandom.Next(123, 255) / 256)));
        }

        /// <summary>
        /// This allows the creation of a transparent color with the specified opacity.
        /// </summary>
        /// <param name="opacity">A float ranging from 0 for transparent to 1 for opaque</param>
        /// <returns>A Color</returns>
        public static Color RandomDarkColor(float opacity)
        {
            int alpha = Convert.ToInt32(opacity * 255);
            if (alpha > 255) alpha = 255;
            if (alpha < 0) alpha = 0;
            return Color.FromArgb(alpha, ColorFromHsl(_defaultRandom.Next(0, 360), ((double)_defaultRandom.Next(0, 255) / 256), ((double)_defaultRandom.Next(0, 123) / 256)));
        }

        ///  <summary>
        /// Converts a colour from HSL to RGB
        /// </summary>
        /// <remarks>Adapted from the algoritm in Foley and Van-Dam</remarks>
        /// <param name="hue">A double representing degrees ranging from 0 to 360 and is equal to the GetHue() on a Color structure.</param>
        /// <param name="saturation">A double value ranging from 0 to 1, where 0 is gray and 1 is fully saturated with color.</param>
        /// <param name="brightness">A double value ranging from 0 to 1, where 0 is black and 1 is white.</param>
        /// <returns>A Color structure with the equivalent hue saturation and brightness</returns>
        public static Color ColorFromHsl(double hue, double saturation, double brightness)
        {
            double normalizedHue = hue / 360;

            double red, green, blue;

            if (brightness == 0)
            {
                red = green = blue = 0;
            }
            else
            {
                if (saturation == 0)
                {
                    red = green = blue = brightness;
                }
                else
                {
                    double temp2;
                    if (brightness <= 0.5)
                    {
                        temp2 = brightness * (1.0 + saturation);
                    }
                    else
                    {
                        temp2 = brightness + saturation - (brightness * saturation);
                    }

                    double temp1 = 2.0 * brightness - temp2;

                    double[] temp3 = new[] { normalizedHue + 1.0 / 3.0, normalizedHue, normalizedHue - 1.0 / 3.0 };
                    double[] color = new double[] { 0, 0, 0 };
                    for (int i = 0; i < 3; i++)
                    {
                        if (temp3[i] < 0) temp3[i] += 1.0;

                        if (temp3[i] > 1) temp3[i] -= 1.0;

                        if (6.0 * temp3[i] < 1.0)
                        {
                            color[i] = temp1 + (temp2 - temp1) * temp3[i] * 6.0;
                        }
                        else if (2.0 * temp3[i] < 1.0)
                        {
                            color[i] = temp2;
                        }
                        else if (3.0 * temp3[i] < 2.0)
                        {
                            color[i] = (temp1 + (temp2 - temp1) * ((2.0 / 3.0) - temp3[i]) * 6.0);
                        }
                        else
                        {
                            color[i] = temp1;
                        }
                    }

                    red = color[0];
                    green = color[1];
                    blue = color[2];
                }
            }
            if (red > 1) red = 1;
            if (red < 0) red = 0;
            if (green > 1) green = 1;
            if (green < 0) green = 0;
            if (blue > 1) blue = 1;
            if (blue < 0) blue = 0;
            return Color.FromArgb((int)(255 * red), (int)(255 * green), (int)(255 * blue));
        }
    }
}