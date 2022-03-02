// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LegendType.
    /// </summary>
    public enum LegendType
    {
        /// <summary>
        /// Schemes can contain symbols and be contained by layers
        /// </summary>
        Scheme,

        /// <summary>
        /// The ability to contain another layer type is controlled by CanReceiveItem instead
        /// of being specified by these pre-defined criteria.
        /// </summary>
        Custom,

        /// <summary>
        /// Groups can be contained by groups, and contain groups or layers, but not categories or symbols
        /// </summary>
        Group,

        /// <summary>
        /// Layers can contain symbols or categories, but not other layers or groups
        /// </summary>
        Layer,

        /// <summary>
        /// Symbols can't contain anything, but can be contained by layers and categories
        /// </summary>
        Symbol
    }
}