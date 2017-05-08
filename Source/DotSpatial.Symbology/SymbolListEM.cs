// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/11/2009 5:25:52 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

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
        /// <param name="self">this</param>
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