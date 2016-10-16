// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 3/7/2010 8:55:17 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// DynamicVisibilityModes
    /// </summary>
    public enum DynamicVisibilityMode
    {
        /// <summary>
        /// The layer will only be visible when zoomed in closer than the
        /// DynamicVisibilityWidth.
        /// </summary>
        ZoomedIn,
        /// <summary>
        /// The layer will only be visible when zoomed out beyond the
        /// DynamicVisibilityWidth.
        /// </summary>
        ZoomedOut
    }
}