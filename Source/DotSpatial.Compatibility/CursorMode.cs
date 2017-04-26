// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 2:03:03 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
#pragma warning disable SA1300 // Element must begin with upper-case letter

    /// <summary>
    /// CursorModes
    /// </summary>
    public enum CursorMode
    {
        /// <summary>
        /// No built in behavior
        /// </summary>
        // ReSharper disable InconsistentNaming
        cmNone,

        /// <summary>
        /// Left click to Pan
        /// </summary>
        cmPan,

        /// <summary>
        /// Select
        /// </summary>
        cmSelection,

        /// <summary>
        /// Left click to zoom in
        /// </summary>
        cmZoomIn,

        /// <summary>
        /// Right click to zoom in
        /// </summary>
        cmZoomOut
    }
#pragma warning restore SA1300 // Element must begin with upper-case letter
}