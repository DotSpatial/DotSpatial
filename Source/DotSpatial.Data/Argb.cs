// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;

namespace DotSpatial.Data
{
    /// <summary>
    /// Tiny structure for storing A,R,G,B components of Color.
    /// </summary>
    public struct Argb
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Argb"/> struct from the given values.
        /// If the values are bigger than 255 they are changed to 255. If the values are smaller than 0 they are changed to 0.
        /// </summary>
        /// <param name="a">The a part of a color.</param>
        /// <param name="r">The r part of a color.</param>
        /// <param name="g">The g part of a color.</param>
        /// <param name="b">The b part of a color.</param>
        public Argb(int a, int r, int g, int b)
            : this(ByteRange(a), ByteRange(r), ByteRange(g), ByteRange(b))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Argb"/> struct from the given values.
        /// </summary>
        /// <param name="a">The a part of a color.</param>
        /// <param name="r">The r part of a color.</param>
        /// <param name="g">The g part of a color.</param>
        /// <param name="b">The b part of a color.</param>
        public Argb(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        /// <summary>
        /// Gets the A part of this Argb object.
        /// </summary>
        public byte A { get; }

        /// <summary>
        /// Gets the B part of this Argb object.
        /// </summary>
        public byte B { get; }

        /// <summary>
        /// Gets the G part of this Argb object.
        /// </summary>
        public byte G { get; }

        /// <summary>
        /// Gets the R part of this Argb object.
        /// </summary>
        public byte R { get; }

        /// <summary>
        /// Converts the given Color to Argb.
        /// </summary>
        /// <param name="color">color that gets converted.</param>
        /// <returns>Argb object that contains the color values.</returns>
        public static Argb FromColor(Color color)
        {
            return new Argb(color.A, color.R, color.G, color.B);
        }

        /// <summary>
        /// Returns an byte that corresponds to the given value. If value is larger than 255, the value will be equal to 255.
        /// If the value is smaller than 0, it will be equal to 0.
        /// </summary>
        /// <param name="value">An integer value to convert.</param>
        /// <returns>A byte that corresponds to the integer ranging from 0 to 255.</returns>
        public static byte ByteRange(int value)
        {
            if (value > 255) return 255;
            if (value < 0) return 0;
            return (byte)value;
        }

        /// <summary>
        /// Converts this Argb to Color.
        /// </summary>
        /// <returns>Color build from the Argb values.</returns>
        public Color ToColor()
        {
            return Color.FromArgb(A, R, G, B);
        }
    }
}