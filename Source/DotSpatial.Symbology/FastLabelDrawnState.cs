// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Fast drawn state for labels.
    /// </summary>
    public class FastLabelDrawnState
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FastLabelDrawnState"/> class.
        /// </summary>
        /// <param name="category">The category.</param>
        public FastLabelDrawnState(ILabelCategory category)
        {
            Category = category;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        public ILabelCategory Category { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the label is selected.
        /// </summary>
        public bool Selected { get; set; }

        #endregion
    }
}