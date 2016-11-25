// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
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