// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for SimpleSymbol.
    /// </summary>
    public interface ISimpleSymbol : IOutlinedSymbol, IColorable
    {
        #region Properties

        /// <summary>
        /// Gets or sets the PointTypes enumeration that describes how to draw the simple symbol.
        /// </summary>
        PointShape PointShape { get; set; }

        #endregion
    }
}