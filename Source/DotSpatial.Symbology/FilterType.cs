// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
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