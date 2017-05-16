// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.NTSExtension
{
    /// <summary>
    /// An IRectangle is not as specific to being a geometry, and can apply to anything as long as it is willing
    /// to support a double valued height, width, X and Y value.
    /// </summary>
    public interface IRectangle
    {
        #region Properties

        /// <summary>
        /// Gets or sets the difference between the maximum and minimum y values.
        /// Setting this will change only the minimum Y value, leaving the Top alone
        /// </summary>
        /// <returns>max y - min y, or 0 if this is a null <c>Envelope</c>.</returns>
        double Height { get; set; }

        /// <summary>
        /// Gets or sets the difference between the maximum and minimum x values.
        /// Setting this will change only the Maximum X value, and leave the minimum X alone
        /// </summary>
        /// <returns>max x - min x, or 0 if this is a null <c>Envelope</c>.</returns>
        double Width { get; set; }

        /// <summary>
        /// Gets or sets the X value of the envelope.
        /// In the precedent set by controls, envelopes support an X value and a width.
        /// The X value represents the Minimum.X coordinate for this envelope.
        /// </summary>
        double X { get; set; }

        /// <summary>
        /// Gets or sets the Y value of the envelope.
        /// In the precedent set by controls, envelopes support a Y value and a height.
        /// the Y value represents the Top of the envelope, (Maximum.Y) and the Height
        /// represents the span between Maximum and Minimum Y.
        /// </summary>
        double Y { get; set; }

        #endregion
    }
}