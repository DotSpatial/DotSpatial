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
// The Initial Developer of this Original Code is Ted Dunsford. Created 5/21/2009 4:01:18 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// FastDrawnState uses fields, not properties.
    /// </summary>
    public class FastDrawnState
    {
        #region Fields

        /// <summary>
        /// The category that describes the symbolic drawing for this item.
        /// </summary>
        public IFeatureCategory Category;

        /// <summary>
        /// Boolean, true if this item should be drawn as being selected
        /// </summary>
        public bool Selected;

        /// <summary>
        /// A Boolean that indicates whether or not this feature should be drawn at all.
        /// This should default to true.
        /// </summary>
        public bool Visible;

        /// <summary>
        /// Creates a blank fast drawn state
        /// </summary>
        public FastDrawnState()
        {
            Visible = true;
        }

        /// <summary>
        /// Creates a new FastDrawnState with the specified parameters
        /// </summary>
        /// <param name="sel"></param>
        /// <param name="cat"></param>
        public FastDrawnState(bool sel, IFeatureCategory cat)
        {
            Selected = sel;
            Category = cat;
            Visible = true;
        }

        /// <summary>
        /// Overrides ToString to give a description of the FastDrawnState.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Selected: " + Selected + ", Category: " + Category;
        }

        /// <summary>
        /// Equality based on the Boolean and category settings
        /// </summary>
        /// <param name="obj">The fast drawn state to compare with</param>
        /// <returns></returns>
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

        #endregion
    }
}