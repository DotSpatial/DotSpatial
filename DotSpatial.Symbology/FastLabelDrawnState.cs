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
// The Initial Developer of this Original Code is Ted Dunsford. Created 12/3/2009 12:26:14 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// FastLabelDrawnState
    /// </summary>
    public class FastLabelDrawnState
    {
        /// <summary>
        /// Gets or sets the category
        /// </summary>
        public ILabelCategory Category;

        /// <summary>
        /// Gets or sets whether the label is selected
        /// </summary>
        public bool Selected;

        /// <summary>
        /// Creates a new drawn state with the specified category
        /// </summary>
        /// <param name="category">The category</param>
        public FastLabelDrawnState(ILabelCategory category)
        {
            Category = category;
        }
    }
}