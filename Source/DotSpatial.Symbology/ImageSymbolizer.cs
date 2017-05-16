// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ImageSymbolizer
    /// </summary>
    [Serializable]
    public class ImageSymbolizer : LegendItem, IImageSymbolizer
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSymbolizer"/> class.
        /// </summary>
        public ImageSymbolizer()
        {
            Opacity = 1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a float value from 0 to 1, where 1 is fully opaque while 0 is fully transparent.
        /// </summary>
        [Serialize("Opacity")]
        public float Opacity { get; set; }

        #endregion
    }
}