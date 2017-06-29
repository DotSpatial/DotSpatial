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
// The Initial Developer of this Original Code is Ted Dunsford. Created 4/10/2009 9:24:20 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// ModifySelectionModes
    /// </summary>
    public enum ModifySelectionMode
    {
        /// <summary>
        /// Appends the newly selected features to the existing selection
        /// </summary>
        Append,
        /// <summary>
        /// Subtracts the newly selected features from the existing features.
        /// </summary>
        Subtract,
        /// <summary>
        /// Clears the current selection and selects the new features
        /// </summary>
        Replace,
        /// <summary>
        /// Selects the new features only from the existing selection
        /// </summary>
        SelectFrom,
    }
}