// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 2:03:03 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// CursorModes
    /// </summary>
    public enum CursorMode
    {
        /// <summary>
        /// No built in behavior
        /// </summary>
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
}