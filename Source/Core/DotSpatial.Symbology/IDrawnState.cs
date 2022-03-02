// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Interface for DrawnState.
    /// </summary>
    public interface IDrawnState
    {
        #region Properties

        /// <summary>
        /// Gets or sets the integer chunk that this item belongs to.
        /// </summary>
        int Chunk { get; set; }

        /// <summary>
        /// Gets or sets the scheme category.
        /// </summary>
        IFeatureCategory SchemeCategory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this feature is currently selected.
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this feature is currently being drawn.
        /// </summary>
        bool IsVisible { get; set; }

        #endregion
    }
}