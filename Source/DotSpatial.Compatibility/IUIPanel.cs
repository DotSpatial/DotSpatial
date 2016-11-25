// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 11:38:33 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Windows.Forms;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// A function that is called upon close of a panel. The
    /// name (caption) of the closed panel is passed into the
    /// OnPanelClose function.
    /// </summary>
    public delegate void OnPanelClose(string caption);

    /// <summary>
    /// UiPanel
    /// </summary>
    public interface IUIPanel
    {
        #region Methods

        /// <summary>
        /// Adds a function (onCloseFunction) which
        /// is called when the panel specified by caption is closed.
        /// </summary>
        void AddOnCloseHandler(string caption, OnPanelClose onCloseFunction);

        /// <summary>
        /// Returns a Panel that can be used to add dockable content to DotSpatial.
        /// </summary>
        Panel CreatePanel(string caption, SpatialDockStyle dockStyle);

        /// <summary>
        /// Returns a Panel that can be used to add dockable content to DotSpatial.
        /// </summary>
        Panel CreatePanel(string caption, DockStyle dockStyle);

        /// <summary>
        /// Deletes the specified panel.
        /// </summary>
        void DeletePanel(string caption);

        /// <summary>
        /// Hides or shows a panel without necessarily deleting it.
        /// </summary>
        void SetPanelVisible(string caption, bool visible);

        #endregion
    }
}