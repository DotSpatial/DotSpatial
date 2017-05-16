// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for SimplePattern.
    /// </summary>
    public interface ISimplePattern : IPattern
    {
        #region Properties

        /// <summary>
        /// Gets or sets solid Color used for filling this pattern.
        /// </summary>
        Color FillColor { get; set; }

        /// <summary>
        /// Gets or sets the opacity of this simple pattern by modifying the alpha channel of the fill color.
        /// </summary>
        float Opacity { get; set; }

        #endregion
    }
}