// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using System.Drawing.Drawing2D;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for SimpleStroke.
    /// </summary>
    public interface ISimpleStroke : IStroke
    {
        #region Properties

        /// <summary>
        /// Gets or sets the color for this drawing layer.
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// Gets or sets the DashStyle for this stroke. (Custom is just solid for simple strokes).
        /// </summary>
        DashStyle DashStyle { get; set; }

        /// <summary>
        /// Gets or sets the opacity of the color. 1 is fully opaque while 0 is fully transparent.
        /// </summary>
        float Opacity { get; set; }

        /// <summary>
        /// Gets or sets the width of this line. In geographic ScaleMode,
        /// this width is the actual geographic width of the line. In Symbolic scale mode
        /// this is the width of the line in pixels.
        /// </summary>
        double Width { get; set; }

        #endregion
    }
}