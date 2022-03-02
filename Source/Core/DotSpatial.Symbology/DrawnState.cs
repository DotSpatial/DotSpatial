// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Symbology
{
    /// <summary>
    /// DrawnState simply groups the feature with characteristics like selected and category for easier tracking.
    /// </summary>
    public class DrawnState : IDrawnState
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawnState"/> class.
        /// Creates a new instance of DrawnState class that helps to narrow down the features to be drawn.
        /// </summary>
        public DrawnState()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawnState"/> class for subdividing features.
        /// </summary>
        /// <param name="category">A category that the feature belongs to.</param>
        /// <param name="selected">Boolean, true if the feature is currently selected.</param>
        /// <param name="chunk">An integer chunk that this feature should belong to.</param>
        /// <param name="visible">A boolean indicating whether this feature is visible or not.</param>
        public DrawnState(IFeatureCategory category, bool selected, int chunk, bool visible)
        {
            SchemeCategory = category;
            IsSelected = selected;
            IsVisible = visible;
            Chunk = chunk;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the integer chunk that this item belongs to.
        /// </summary>
        public int Chunk { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this feature is currently selected.
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this feature is currently being drawn.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the scheme category.
        /// </summary>
        public IFeatureCategory SchemeCategory { get; set; }

        #endregion

        #region Operators

        /// <summary>
        /// Overrides the standard equal operator.
        /// </summary>
        /// <param name="u">First drawn state to check.</param>
        /// <param name="v">Second drawn state to check.</param>
        /// <returns>True, if both drawn states are equal in Chunk, IsSelected, IsVisible and SchemeCategory.</returns>
        public static bool operator ==(DrawnState u, IDrawnState v)
        {
            if (u == null || v == null) return u == v;
            if (u.Chunk != v.Chunk) return false;
            if (u.IsSelected != v.IsSelected) return false;
            if (u.IsVisible != v.IsVisible) return false;
            if (u.SchemeCategory != v.SchemeCategory) return false;

            return true;
        }

        /// <summary>
        /// Overrides the not-equal to operator.
        /// </summary>
        /// <param name="u">First drawn state to check.</param>
        /// <param name="v">Second drawn state to check.</param>
        /// <returns>True, if both drawn states are not equal.</returns>
        public static bool operator !=(DrawnState u, IDrawnState v)
        {
            return !(u == v);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Takes any object, but attempts to compare it with values as an IDrawnState. If it can satisfy
        /// the IDrawnState interface and all the values are the same then this returns true.
        /// </summary>
        /// <param name="obj">IDrawnState to compare to this.</param>
        /// <returns>True, if this and obj are equal.</returns>
        public override bool Equals(object obj)
        {
            IDrawnState ds = obj as IDrawnState;
            if (ds == null) return false;

            return this == ds;
        }

        /// <summary>
        /// Not entirely sure about using these features this way. It might not work well with interfaces.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}