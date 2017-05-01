// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 11/6/2008 11:17:32 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Controls
{
    /// <summary>
    /// DefaultTools
    /// </summary>
    public enum FunctionMode
    {
        /// <summary>
        /// Mousewheel still zooms the map, but left clicking brings items into the view.
        /// </summary>
        Info,

        /// <summary>
        /// Zooms into the map with the left mouse button and zooms out with the right mouse button.
        /// </summary>
        ZoomIn,

        /// <summary>
        /// Zooms out by clicking the left mouse button.
        /// </summary>
        ZoomOut,

        /// <summary>
        /// Zooms by scrolling the mouse wheel and pans by pressing the mouse wheel and moving the mouse.
        /// </summary>
        ZoomPan,

        /// <summary>
        /// Pans the map with the left mouse button, context with the right and zooms with the mouse wheel
        /// </summary>
        Pan,

        /// <summary>
        /// Selects shapes with the left mouse button, context with the right and zooms with the mouse wheel
        /// </summary>
        Select,

        /// <summary>
        /// Left button selects, moves or edits, right produces a context menu
        /// </summary>
        Label,

        /// <summary>
        /// Disables all the tools
        /// </summary>
        None,
    }
}