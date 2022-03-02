// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Extension methods for Symbol lists.
    /// </summary>
    public static class SymbolListEm
    {
        #region Methods

        /// <summary>
        /// Calculates the bounding size for this entire symbol.
        /// </summary>
        /// <param name="self">this.</param>
        /// <returns>The calculated size.</returns>
        public static Size2D GetBoundingSize(this IList<ISymbol> self)
        {
            Size2D size = new Size2D();

            foreach (ISymbol symbol in self)
            {
                Size2D bsize = symbol.GetBoundingSize();
                size.Width = Math.Max(size.Width, bsize.Width);
                size.Height = Math.Max(size.Height, bsize.Height);
            }

            return size;
        }

        #endregion
    }
}