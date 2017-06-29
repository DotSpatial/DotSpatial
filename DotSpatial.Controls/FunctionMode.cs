// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
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