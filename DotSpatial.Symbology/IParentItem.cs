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
// The Initial Developer of this Original Code is Ted Dunsford. Created 2/3/2009 2:48:48 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Any item which can be contained by a parent item
    /// </summary>
    /// <typeparam name="T">The type class of the potential parent</typeparam>
    public interface IParentItem<T>
    {
        #region Properties

        /// <summary>
        /// Gets the parent item relative to this item.
        /// </summary>
        T GetParentItem();

        /// <summary>
        /// Sets teh parent legend item for this item
        /// </summary>
        /// <param name="value"></param>
        void SetParentItem(T value);

        #endregion
    }
}