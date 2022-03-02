// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// RasterEditorSettings.
    /// </summary>
    [Serializable]
    public class RasterEditorSettings : EditorSettings
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RasterEditorSettings"/> class.
        /// </summary>
        public RasterEditorSettings()
        {
            NumBreaks = 2;
            MaxSampleCount = 10000;
            Min = -100000;
            Max = 1000000;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the maximum value that will contribute to statistics.
        /// </summary>
        [Serialize("Max")]
        public double Max { get; set; }

        /// <summary>
        /// Gets or sets the minimum value that will contribute to statistics.
        /// </summary>
        [Serialize("Min")]
        public double Min { get; set; }

        #endregion
    }
}