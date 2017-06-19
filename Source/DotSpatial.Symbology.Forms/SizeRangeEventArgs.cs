// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// SizeRangeEventArgs
    /// </summary>
    public class SizeRangeEventArgs : EventArgs
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeRangeEventArgs"/> class.
        /// </summary>
        /// <param name="startSize">The start size of the size range.</param>
        /// <param name="endSize">The end size of the size range.</param>
        /// <param name="template">The feature symbolizer.</param>
        /// <param name="useSizeRange">Indicates whether the size range should be used.</param>
        public SizeRangeEventArgs(double startSize, double endSize, IFeatureSymbolizer template, bool useSizeRange)
        {
            StartSize = startSize;
            EndSize = endSize;
            Template = template;
            UseSizeRange = useSizeRange;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SizeRangeEventArgs"/> class.
        /// </summary>
        /// <param name="range">The feature size range.</param>
        public SizeRangeEventArgs(FeatureSizeRange range)
        {
            StartSize = range.Start;
            EndSize = range.End;
            Template = range.Symbolizer;
            UseSizeRange = range.UseSizeRange;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the start size of the size range.
        /// </summary>
        public double StartSize { get; protected set; }

        /// <summary>
        /// Gets or sets the end size of the range.
        /// </summary>
        public double EndSize { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the size range should be used.
        /// </summary>
        public bool UseSizeRange { get; protected set; }

        /// <summary>
        /// Gets or sets the symbolizer template that describes everything not covered by a range parameter.
        /// </summary>
        public IFeatureSymbolizer Template { get; protected set; }

        #endregion
    }
}