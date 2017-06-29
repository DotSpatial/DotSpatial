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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/25/2009 2:40:04 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Add, remove and clear methods don't work on all the categorical sub-filters, but rather only on the
    /// most immediate.
    /// </summary>
    public enum FilterType
    {
        /// <summary>
        /// Categories
        /// </summary>
        Category,
        /// <summary>
        /// Chunks
        /// </summary>
        Chunk,
        /// <summary>
        /// Selected or unselected
        /// </summary>
        Selection,
        /// <summary>
        /// Visible or not
        /// </summary>
        Visible,
    }
}