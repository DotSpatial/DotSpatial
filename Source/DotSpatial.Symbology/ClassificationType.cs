// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ClassificationTypes
    /// </summary>
    public enum ClassificationType
    {
        /// <summary>
        /// Each category is designed with a custom expression
        /// </summary>
        Custom,

        /// <summary>
        /// Unique values are added
        /// </summary>
        UniqueValues,

        /// <summary>
        /// A Quantile scheme is applied, which forces the behavior of continuous categories.
        /// </summary>
        Quantities,
    }
}