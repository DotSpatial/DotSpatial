// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for PictureSymbol.
    /// </summary>
    public interface IPictureSymbol : IOutlinedSymbol, IDisposable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the image to use when the PictureMode is set to Image.
        /// </summary>
        Image Image { get; set; }

        /// <summary>
        /// Gets or sets the string fileName, if any, associated with the image file.
        /// Setting this will automatically also load the Image.
        /// </summary>
        string ImageFilename { get; set; }

        /// <summary>
        /// Gets or sets the opacity for this image. Setting this will automatically change the image in memory.
        /// </summary>
        float Opacity { get; set; }

        #endregion
    }
}