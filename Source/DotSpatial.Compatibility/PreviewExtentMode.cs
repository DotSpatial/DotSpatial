// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:47:50 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Enumeration of possible preview map update types.
    /// </summary>
    public enum PreviewExtentMode
    {
        /// <summary>
        /// Update using full exents.
        /// </summary>
        FullExtents = 0,
        /// <summary>
        /// Update using current map view.
        /// </summary>
        CurrentMapView = 1
    }
}