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
// The Original Code is DotSpatial.dll
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/29/2010 11:41:10 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
//   Name            |    Date    |                    Comments
// ------------------|------------|---------------------------------------------------------------
// ********************************************************************************************************

using System;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A list of options for enabling Apps.
    /// </summary>
    [Obsolete("Use ShowExtensionsDialogMode instead")] // Marked obsolete in 1.7
    public enum ShowExtensionsDialog
    {
        /// <summary>
        /// The "Extensions" menu item will appear on the HeaderControl.  Clicking it launches the AppDialog.
        /// </summary>
        Default = 0,

        /// <summary>
        /// A "plugin" glyph will appear suspended in the lower right corner of the map.  Clicking it launches
        /// the AppDialog.
        /// </summary>
        MapGlyph,

        /// <summary>
        /// The AppDialog will button not be shown. This allows the application developer to provide a custom implementation.
        /// </summary>
        None,
    }

    /// <summary>
    /// A list of options for enabling Apps.
    /// </summary>
    public enum ShowExtensionsDialogMode
    {
        /// <summary>
        /// The "Extensions" menu item will appear on the HeaderControl.  Clicking it launches the AppDialog.
        /// </summary>
        Default = 0,

        /// <summary>
        /// A "plugin" glyph will appear suspended in the lower right corner of the map.  Clicking it launches
        /// the AppDialog.
        /// </summary>
        MapGlyph,

        /// <summary>
        /// The AppDialog will button not be shown. This allows the application developer to provide a custom implementation.
        /// </summary>
        None,
    }
}