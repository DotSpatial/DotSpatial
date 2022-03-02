// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// FastDrawnState uses fields, not properties.
    /// </summary>
    public class FastDrawnState
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FastDrawnState"/> class.
        /// </summary>
        public FastDrawnState()
        {
            Visible = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FastDrawnState"/> class with the specified parameters.
        /// </summary>
        /// <param name="selected">Indicates whether the feature should be drawn selected.</param>
        /// <param name="category">The category that describes the symbolic drawing for this item.</param>
        public FastDrawnState(bool selected, IFeatureCategory category)
        {
            Selected = selected;
            Category = category;
            Visible = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the category that describes the symbolic drawing for this item.
        /// </summary>
        public IFeatureCategory Category { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item should be drawn as being selected-.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this feature should be drawn at all.
        /// This should default to true.
        /// </summary>
        public bool Visible { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Equality based on the Boolean and category settings.
        /// </summary>
        /// <param name="obj">The fast drawn state to compare with.</param>
        /// <returns>True, if both items are equal.</returns>
        public override bool Equals(object obj)
        {
            FastDrawnState item = obj as FastDrawnState;
            if (item == null) return false;

            return item.Selected == Selected && item.Category == Category && Visible == item.Visible;
        }

        /// <summary>
        /// For ordering in a dictionary, if get hash code is the same, then equals is used to test equality.
        /// For performance, try to make sure that hash codes are different enough for organizing in the
        /// dictionary, but are the same in cases where Category and Selected are the same.
        /// </summary>
        /// <returns>An integer hash code for this item.</returns>
        public override int GetHashCode()
        {
            int sel;
            if (Selected)
            {
                sel = -1;
            }
            else
            {
                sel = 1;
            }

            if (Category == null) return 0;

            return Category.GetHashCode() * sel;
        }

        /// <summary>
        /// Overrides ToString to give a description of the FastDrawnState.
        /// </summary>
        /// <returns>A string that shows the selected state and category of the FastDrawnState.</returns>
        public override string ToString()
        {
            return "Selected: " + Selected + ", Category: " + Category;
        }

        #endregion
    }
}