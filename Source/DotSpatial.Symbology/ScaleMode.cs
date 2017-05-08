// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  Contains the business logic for symbology layers and symbol categories.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 6/27/2008 3:39:13 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// Specifies whether non-coordinate drawing properties like width or size
    /// use pixels or map coordinates. If pixels are used, a "back transform"
    /// to approximate pixel sizes.
    /// </summary>
    public enum ScaleMode
    {
        /// <summary>
        /// Uses the simplest symbology possible, but can draw quickly
        /// </summary>
        Simple = 0,

        /// <summary>
        /// Symbol sizing parameters are based in world coordinates and will get smaller when zooming out like a real object.
        /// </summary>
        Geographic = 1,

        /// <summary>
        /// The symbols approximately preserve their size as you zoom
        /// </summary>
        Symbolic = 2
    }
}