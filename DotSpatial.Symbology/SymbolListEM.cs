// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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
    /// SymbolListEM
    /// </summary>
    public static class SymbolListEM
    {
        /// <summary>
        /// Calculates the bounding size for this entire symbol.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Size2D GetBoundingSize(this IList<ISymbol> self)
        {
            Size2D size = new Size2D();

            foreach (ISymbol symbol in self)
            {
                Size2D bsize = symbol.GetBoundingSize();
                size.Width = Math.Max(size.Width, bsize.Width);
                size.Height = Math.Max(size.Width, bsize.Height);
            }
            return size;
        }
    }
}