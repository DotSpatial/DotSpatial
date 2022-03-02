// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for OutlinedSymbol.
    /// </summary>
    public interface IOutlinedSymbol : ISymbol
    {
        /// <summary>
        /// Gets or sets the outline color that surrounds this specific symbol.
        /// (this will have the same shape as the symbol, but be larger.
        /// </summary>
        Color OutlineColor { get; set; }

        /// <summary>
        /// Gets or sets the Alpha channel of the color to a floating point opacity
        /// that ranges from 0 to 1.
        /// </summary>
        float OutlineOpacity { get; set; }

        /// <summary>
        /// Gets or sets the size of the outline beyond the size of this symbol.
        /// </summary>
        double OutlineWidth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the outline should be used.
        /// </summary>
        bool UseOutline { get; set; }

        #region Methods

        /// <summary>
        /// Copies only the use outline, outline width and outline color properties from the specified symbol.
        /// </summary>
        /// <param name="symbol">The symbol to copy from.</param>
        void CopyOutline(IOutlinedSymbol symbol);

        #endregion
    }
}