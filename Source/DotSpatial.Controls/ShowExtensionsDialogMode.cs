// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

namespace DotSpatial.Controls
{
    /// <summary>
    /// A list of options for enabling Apps.
    /// </summary>
    public enum ShowExtensionsDialogMode
    {
        /// <summary>
        /// The "Extensions" menu item will appear on the HeaderControl. Clicking it launches the AppDialog.
        /// </summary>
        Default = 0,

        /// <summary>
        /// A "plugin" glyph will appear suspended in the lower right corner of the map. Clicking it launches the AppDialog.
        /// </summary>
        MapGlyph,

        /// <summary>
        /// The AppDialog button will not be shown. This allows the application developer to provide a custom implementation.
        /// </summary>
        None
    }
}