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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/21/2009 2:13:13 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// LabelDrawState
    /// </summary>
    public class LabelDrawState
    {
        /// <summary>
        /// A LabelCategory interface representing the drawing information for this label.
        /// </summary>
        public ILabelCategory Category;

        /// <summary>
        /// A boolean indicating whether or not this is selected.
        /// </summary>
        public bool Selected;

        /// <summary>
        /// A boolean indicating whether the associated feature should be drawn.
        /// </summary>
        public bool Visible;

        /// <summary>
        /// Creates a new instance of the LabelDrawState class where selected is false
        /// but visible is true.
        /// </summary>
        /// <param name="category">The category</param>
        public LabelDrawState(ILabelCategory category)
        {
            Category = category;
            Visible = true;
        }

        /// <summary>
        /// Creates a new instance of the LabelDrawState based on the specified parameters.
        /// </summary>
        /// <param name="category">The category</param>
        /// <param name="selected">Boolean, true if the label is selected</param>
        /// <param name="visible">Boolean, true if the label should be visible</param>
        public LabelDrawState(ILabelCategory category, bool selected, bool visible)
        {
            Category = category;
            Selected = selected;
            Visible = visible;
        }
    }
}