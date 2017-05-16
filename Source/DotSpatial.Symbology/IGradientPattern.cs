// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for GradientPattern.
    /// </summary>
    public interface IGradientPattern : IPattern
    {
        #region Properties

        /// <summary>
        /// Gets or sets the angle for the gradient pattern.
        /// </summary>
        double Angle { get; set; }

        /// <summary>
        /// Gets or sets an array of colors that match the corresponding positions. The length of
        /// colors and positions should be the same length.
        /// </summary>
        Color[] Colors { get; set; }

        /// <summary>
        /// Gets or sets the gradient type
        /// </summary>
        GradientType GradientType { get; set; }

        /// <summary>
        /// Gets or sets the positions as floating point values from 0 to 1 that represent the corresponding location
        /// in the gradient brush pattern.
        /// </summary>
        float[] Positions { get; set; }

        #endregion
    }
}