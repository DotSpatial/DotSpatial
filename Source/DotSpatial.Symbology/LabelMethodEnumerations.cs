// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Methods used in calculating the placement of a label.
    /// </summary>
    public enum LabelPlacementMethod
    {
        /// <summary>
        /// Use the centroid of the feature.
        /// </summary>
        Centroid,

        /// <summary>
        /// Use the center of the extents of the feature.
        /// </summary>
        Center,

        /// <summary>
        /// Use the closest point to the centroid that is in the feature.
        /// </summary>
        InteriorPoint
    }

    /// <summary>
    /// Used to indicate the label orientation.
    /// </summary>
    public enum LineOrientation
    {
        /// <summary>
        /// Orientation of the line label is parallel to the line at the PlacementPosition.
        /// </summary>
        Parallel,

        /// <summary>
        /// Orientation of the line label is perpendicular to the line at the PlacementPosition.
        /// </summary>
        Perpendicular
    }

    /// <summary>
    /// Methods used to calculate the placement of line labels.
    /// </summary>
    public enum LineLabelPlacementMethod
    {
        /// <summary>
        /// Uses the longest segment of the LineString.
        /// </summary>
        LongestSegment,

        /// <summary>
        /// Uses the first segment of the LineString.
        /// </summary>
        FirstSegment,

        /// <summary>
        /// Uses the middle segment of the LineString.
        /// </summary>
        MiddleSegment,

        /// <summary>
        /// Uses the last segment of the LineString.
        /// </summary>
        LastSegment
    }

    /// <summary>
    /// Determines if all parts should be labeled or just the largest.
    /// </summary>
    public enum PartLabelingMethod
    {
        /// <summary>
        /// Label all parts.
        /// </summary>
        LabelAllParts,

        /// <summary>
        /// Only label the largest part.
        /// </summary>
        LabelLargestPart
    }
}