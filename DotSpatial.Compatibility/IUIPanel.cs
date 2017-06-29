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