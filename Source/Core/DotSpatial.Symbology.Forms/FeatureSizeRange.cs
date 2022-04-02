// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Drawing;
using DotSpatial.Serialization;

namespace DotSpatial.Symbology.Forms
{
    /// <summary>
    /// FeatureSizeRange.
    /// </summary>
    public class FeatureSizeRange
    {
        #region Fields

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSizeRange"/> class.
        /// </summary>
        public FeatureSizeRange()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureSizeRange"/> class.
        /// </summary>
        /// <param name="symbolizer">The symbolizer.</param>
        /// <param name="start">The minimum size.</param>
        /// <param name="end">The maximum size.</param>
        public FeatureSizeRange(IFeatureSymbolizer symbolizer, double start, double end)
        {
            Symbolizer = symbolizer;
            Start = start;
            End = end;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the maximum size.
        /// </summary>
        public double End { get; set; }

        /// <summary>
        /// Gets or sets the minimum size.
        /// </summary>
        public double Start { get; set; }

        /// <summary>
        /// Gets or sets the symbolizer that controls everything except for size.
        /// </summary>
        public IFeatureSymbolizer Symbolizer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the size range should be used.
        /// </summary>
        public bool UseSizeRange { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Given a size, this will return the native symbolizer that has been adjusted to the specified size.
        /// </summary>
        /// <param name="size">The size of the symbol.</param>
        /// <param name="color">The color of the symbol.</param>
        /// <returns>The adjusted symbolizer.</returns>
        public IFeatureSymbolizer GetSymbolizer(double size, Color color)
        {
            IFeatureSymbolizer copy = Symbolizer.Copy();

            // preserve aspect ratio, larger dimension specified
            if (copy is IPointSymbolizer ps)
            {
                Size2D s = ps.GetSize();
                double ratio = size / Math.Max(s.Width, s.Height);
                s.Width *= ratio;
                s.Height *= ratio;
                ps.SetSize(s);
                ps.SetFillColor(color);
            }

            if (copy is ILineSymbolizer ls)
            {
                ls.SetWidth(size);
                ls.SetFillColor(color);
            }

            return copy;
        }

        #endregion
    }
}