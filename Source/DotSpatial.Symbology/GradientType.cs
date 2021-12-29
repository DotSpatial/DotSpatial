// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// GradientType.
    /// </summary>
    public enum GradientType
    {
        /// <summary>
        /// Draws the gradient with the start color for the center of the circle
        /// and the end color for the surround color
        /// </summary>
        Circular,

        /// <summary>
        /// Draws the gradient with the start color for the center of the circle
        /// and the end color for the color at the contour.
        /// </summary>
        Contour,

        /// <summary>
        /// Draws the gradient in a line with a specified direction
        /// </summary>
        Linear,

        /// <summary>
        /// Draws the gradient in a rectangular path with the start color
        /// at the center and the end color for the surround color
        /// </summary>
        Rectangular
    }
}