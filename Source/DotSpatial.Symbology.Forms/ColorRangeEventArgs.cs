// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// ColorRangeEventArgs
    /// </summary>
    public class ColorRangeEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorRangeEventArgs"/> class.
        /// </summary>
        /// <param name="startColor">The start color.</param>
        /// <param name="endColor">The end color.</param>
        /// <param name="hueShift">The hue shift.</param>
        /// <param name="hsl">Indicates whether to use hsl or rgb colors.</param>
        /// <param name="useRange">Indicates whether the color range should be used.</param>
        public ColorRangeEventArgs(Color startColor, Color endColor, int hueShift, bool hsl, bool useRange)
        {
            StartColor = startColor;
            EndColor = endColor;
            HueShift = hueShift;
            Hsl = hsl;
            UseColorRange = useRange;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the end color.
        /// </summary>
        public Color EndColor { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the ramp of colors should
        /// be built using the HSL characteristics of the start and
        /// end colors rather than the RGB characteristics.
        /// </summary>
        public bool Hsl { get; protected set; }

        /// <summary>
        /// Gets or sets the hue shift.
        /// </summary>
        public int HueShift { get; protected set; }

        /// <summary>
        /// Gets or sets the start color.
        /// </summary>
        public Color StartColor { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this color range should be used.
        /// </summary>
        public bool UseColorRange { get; set; }

        #endregion
    }
}