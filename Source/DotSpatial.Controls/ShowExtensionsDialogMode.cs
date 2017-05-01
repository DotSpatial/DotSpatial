// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
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