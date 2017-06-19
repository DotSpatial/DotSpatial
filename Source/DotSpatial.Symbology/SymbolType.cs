// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// The symbol type.
    /// </summary>
    public enum SymbolType
    {
        /// <summary>
        /// A symbol based on a character, including special purpose symbolic character sets.
        /// </summary>
        Character,

        /// <summary>
        /// An extended, custom symbol that is not part of the current design.
        /// </summary>
        Custom,

        /// <summary>
        /// A symbol based on an image or icon.
        /// </summary>
        Picture,

        /// <summary>
        /// A symbol described by a simple geometry, outline and color.
        /// </summary>
        Simple
    }
}