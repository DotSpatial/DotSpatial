// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:39:12 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Location of a click event within the legend.
    /// </summary>
    public enum ClickLocation
    {
        /// <summary>The user clicked outside of any group or layer.</summary>
        None = 0,
        /// <summary>The user clicked on a layer.</summary>
        Layer = 1,
        /// <summary>The user clicked on a group.</summary>
        Group = 2
    }
}