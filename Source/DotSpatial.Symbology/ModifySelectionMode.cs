// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
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