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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2009 3:48:20 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    public class Selection : FeatureSelection
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of Selection
        /// </summary>
        public Selection(IFeatureSet fs, IDrawingFilter inFilter)
            : base(fs, inFilter, FilterType.Selection)
        {
            Selected = true;
            UseSelection = true;
            UseCategory = false;
            UseVisibility = false;
            UseChunks = false;
            SelectionMode = SelectionMode.IntersectsExtent;
        }

        #endregion
    }
}