// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 11:52:10 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// FillStipple
    /// </summary>
    public enum Stipple
    {
        /// <summary>
        /// Use a custom stipple pattern
        /// </summary>
        Custom,
        /// <summary>
        /// A dashes and dots
        /// </summary>
        DashDotDash,
        /// <summary>
        /// Dashes only
        /// </summary>
        Dashed,
        /// <summary>
        /// Dots only
        /// </summary>
        Dotted,
        /// <summary>
        /// No stipple pattern should be used
        /// </summary>
        None
    }
}