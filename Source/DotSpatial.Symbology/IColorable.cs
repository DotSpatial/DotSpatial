// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IColorable
    /// </summary>
    public interface IColorable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Color
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Gets or sets the opacity
        /// </summary>
        float Opacity { get; set; }

        #endregion
    }
}