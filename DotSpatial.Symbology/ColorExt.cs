// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/2/2009 9:24:33 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Misc extensions for <see cref="Color"/> class
    /// </summary>
    public static class ColorExt
    {
        /// <summary>
        /// Creates a color with the same hue and saturation but that is slightly brighter than this color.
        /// </summary>
        /// <param name="self">The starting color</param>
        /// <param name="brightness">The floating point value of brightness to add to this color.
        /// If the combined result is greater than 1, the result will equal one.</param>
        /// <returns>A color lighter than this color</returns>
        public static Color Lighter(this Color self, float brightness)
        {
            float b = brightness + self.GetBrightness();
            if (b < 0F) b = 0F;
            if (b > 1F) b = 1F;
            return Color.FromArgb(self.A, SymbologyGlobal.ColorFromHsl(self.GetHue(), self.GetSaturation(), b));
        }

        /// <summary>
        /// Creates a color with the same hue and saturation but that is slightly darker than this color.
        /// </summary>
        /// <param name="self">The starting color</param>
        /// <param name="brightness">The floating point value of brightness to add to this color.</param>
        /// if the combined result is less than 0, the result will equal 0.
        /// <returns>A color darker than this color.</returns>
        public static Color Darker(this Color self, float brightness)
        {
            float b = self.GetBrightness() - brightness;
            if (b < 0F) b = 0F;
            if (b > 1F) b = 1F;
            return Color.FromArgb(self.A, SymbologyGlobal.ColorFromHsl(self.GetHue(), self.GetSaturation(), b));
        }

        /// <summary>
        /// Calculates the opacity as a function of the Alpha channel.
        /// </summary>
        /// <param name="self">The color to determine the opacity for</param>
        /// <returns></returns>
        public static float GetOpacity(this Color self)
        {
            return self.A / 255F;
        }

        /// <summary>
        /// Given a floating point opacity, where 0 is fully transparent and 1 is fully opaque,
        /// this will generate a new color that is the transparent version.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="opacity"></param>
        /// <returns></returns>
        public static Color ToTransparent(this Color self, float opacity)
        {
            int a = Convert.ToInt32(opacity * 255);
            if (a > 255) a = 255;
            if (a < 0) a = 0;
            return Color.FromArgb(a, self);
        }

        /// <summary>
        /// Returns an equivalent version of this color that is fully opaque (having an alpha value of 255)
        /// </summary>
        /// <param name="self">The transparent color</param>
        /// <returns>The new Color</returns>
        public static Color ToOpaque(this Color self)
        {
            return Color.FromArgb(255, self);
        }

        /// <summary>
        /// uses a linear ramp to extrapolate the midpoint between the specified color and the new color
        /// as defined by ARGB values independantly
        /// </summary>
        /// <param name="self">This color</param>
        /// <param name="other">The color to blend with this color</param>
        /// <returns>A color that is midway between this color and the specified color</returns>
        public static Color BlendWith(this Color self, Color other)
        {
            int a = (self.A + other.A) / 2;
            int b = (self.B + other.B) / 2;
            int g = (self.G + other.G) / 2;
            int r = (self.R + other.R) / 2;
            return Color.FromArgb(a, r, g, b);
        }
    }
}