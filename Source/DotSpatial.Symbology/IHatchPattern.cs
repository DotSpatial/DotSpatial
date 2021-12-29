// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for HatchPattern.
    /// </summary>
    public interface IHatchPattern : IPattern
    {
        #region Properties

        /// <summary>
        /// Gets or sets the hatch style.
        /// </summary>
        HatchStyle HatchStyle { get; set; }

        /// <summary>
        /// Gets or sets the fore color of the hatch pattern.
        /// </summary>
        Color ForeColor { get; set; }

        /// <summary>
        /// Gets or sets the opacity of this simple pattern by modifying the alpha channel of the fill color.
        /// </summary>
        float ForeColorOpacity { get; set; }

        /// <summary>
        /// Gets or sets the background color of the hatch pattern.
        /// </summary>
        Color BackColor { get; set; }

        /// <summary>
        /// Gets or sets the opacity of this simple pattern by modifying the alpha channel of the fill color.
        /// </summary>
        float BackColorOpacity { get; set; }

        #endregion
    }
}