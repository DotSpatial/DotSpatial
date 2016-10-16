// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:52:19 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// The object given back when a panel is added to the status bar.  This object can
    /// be used to
    /// </summary>
    public interface IStatusBarItem
    {
        /// <summary>
        /// Gets/Sets the Alignment of the text
        /// </summary>
        HJustification Alignment { get; set; }

        /// <summary>
        /// Gets/Sets whether or not this StatusBarItem should auto size itself
        /// </summary>
        bool AutoSize { get; set; }

        /// <summary>
        /// Gets/Sets the minimum allowed width for this StatusBarItem
        /// </summary>
        int MinWidth { get; set; }

        /// <summary>
        /// Gets/Sets the Text within the StatusBarItem
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets/Sets the width of the StatusBarItem
        /// </summary>
        int Width { get; set; }
    }
}