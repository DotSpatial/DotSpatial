// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IntervalMethods.
    /// </summary>
    public enum IntervalMethod
    {
        /// <summary>
        /// The breaks are set to being evenly spaced.
        /// </summary>
        EqualInterval,

        /// <summary>
        /// The breaks are positioned to ensure close to equal quantities
        /// in each break. (each group contains approximately same number of values)
        /// </summary>
        EqualFrequency,

        /// <summary>
        /// Jenks natural breaks looks for "clumping" in the data and
        /// attempts to group according to the clumps.
        /// </summary>
        NaturalBreaks,

        /// <summary>
        /// Breaks start equally placed, but can be positioned manually instead.
        /// </summary>
        Manual,
    }
}