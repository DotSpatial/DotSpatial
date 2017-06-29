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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/23/2009 10:11:33 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// DrawFeatures simply group the feature with characteristics like selected and category for easier tracking.
    /// </summary>
    public class DrawnState : IDrawnState
    {
        #region Private Variables

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of DrawnState class that helps to narrow down the features to be drawn.
        /// </summary>
        public DrawnState()
        {
        }

        /// <summary>
        /// Creates a new instance of a DrawnState class for subdividing features.
        /// </summary>
        /// <param name="category">A category that the feature belongs to</param>
        /// <param name="selected">Boolean, true if the feature is currently selected</param>
        /// <param name="chunk">An integer chunk that this feature should belong to</param>
        /// <param name="visible">A boolean indicating whether this feature is visible or not</param>
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
        /// Gets or sets the scheme category
        /// </summary>
        public IFeatureCategory SchemeCategory { get; set; }

        /// <summary>
        /// Gets or sets a boolean, true if this feature is currently selected
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets whether this feature is currently being drawn.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the integer chunk that this item belongs to.
        /// </summary>
        public int Chunk { get; set; }

        #endregion

        /// <summary>
        /// Not entirely sure about using these features this way.  It might not work well with interfaces.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Takes any object, but attempts to compare it with values as an IDrawnState.  If it can satisfy
        /// the IDrawnState interface and all the values are the same then this returns true.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            IDrawnState ds = obj as IDrawnState;
            if (ds == null) return false;

            return (this == ds);
        }

        /// <summary>
        /// Overrides the standard equal operator
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool operator ==(DrawnState u, IDrawnState v)
        {
            if (u.Chunk != v.Chunk) return false;
            if (u.IsSelected != v.IsSelected) return false;
            if (u.IsVisible != v.IsVisible) return false;
            if (u.SchemeCategory != v.SchemeCategory) return false;
            return true;
        }

        /// <summary>
        /// Overrides the not-equal to operator
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static bool operator !=(DrawnState u, IDrawnState v)
        {
            return !(u == v);
        }
    }
}