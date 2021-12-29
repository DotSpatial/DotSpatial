// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// IntervalSnapMethods.
    /// </summary>
    public enum IntervalSnapMethod
    {
        /// <summary>
        /// Snap the chosen values to the nearest data value.
        /// </summary>
        DataValue,

        /// <summary>
        /// No snapping at all is used
        /// </summary>
        None,

        /// <summary>
        /// Snaps to the nearest integer value.
        /// </summary>
        Rounding,

        /// <summary>
        /// Disregards scale, and preserves a fixed number of figures.
        /// </summary>
        SignificantFigures
    }
}