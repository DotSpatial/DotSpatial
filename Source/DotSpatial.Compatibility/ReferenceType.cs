// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 4:15:41 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// ReferenceTypes
    /// </summary>
    public enum ReferenceType
    {
        /// <summary>
        /// The coordinates are drawn in screen coordinates on the layer, and stay fixed as the map
        /// zooms and pans
        /// </summary>
        Screen,

        /// <summary>
        /// The drawing layer is geographically referenced and will move with the other spatially
        /// referenced map content.
        /// </summary>
        Geographic
    }
}