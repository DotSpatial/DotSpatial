// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Controls
{
    /// <summary>
    /// DefaultTools.
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