// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Compatibility
{
#pragma warning disable SA1300 // Element must begin with upper-case letter

    /// <summary>
    /// CursorModes.
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