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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:51:46 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Windows.Forms;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Used to manipulate the status bar at the bottom of Mapwindow.
    /// </summary>
    public interface IStatusBar
    {
        /// <summary>
        /// Gets/Sets the enabled state of the StatusBar
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets/Sets whether or not the StatusBar's ProgressBar should be shown
        /// </summary>
        bool ShowProgressBar { get; set; }

        /// <summary>
        /// Gets/Sets the value of the StatusBar's ProgressBar
        /// </summary>
        int ProgressBarValue { get; set; }

        /// <summary>
        /// Iterator for all panels within the StatusBar
        /// </summary>
        /// <param name="index">Index of the StatusBarItem to retrieve</param>
        IStatusBarItem this[int index] { get; }

        /// <summary>
        /// Returns the number of panels in the <c>StatusBar</c>.
        /// </summary>
        /// <returns>Number of panels in the <c>StatusBar</c>.</returns>
        int NumPanels { get; }

        /// <summary>
        /// Adds a new panel to the status bar.  This function has been deprecated.  Please use the
        /// <c>AddPanel(Text)</c> overload.
        /// </summary>
        /// <returns>The StatusBarItem that was just added</returns>
        IStatusBarItem AddPanel();

        /// <summary>
        ///	Adds a new panel to the status bar.  This function has been deprecated.  Please use the
        /// <c>AddPanel(Text)</c> overload.
        /// </summary>
        /// <param name="insertAt">The index at which the panel should be added</param>
        /// <returns>The StatusBarItem that was just added</returns>
        IStatusBarItem AddPanel(int insertAt);

        /// <summary>
        /// This is the preferred method to use to add a statusbar panel.
        /// </summary>
        /// <param name="text">Text to display in the panel.</param>
        /// <param name="position">Position to insert panel at.</param>
        /// <param name="width">Width of the panel in pixels.</param>
        /// <param name="autoSize">Panel <c>AutoSize</c> property.</param>
        /// <returns>A <c>StatusBarPanel</c> object.</returns>
        StatusBarPanel AddPanel(string text, int position, int width, StatusBarPanelAutoSize autoSize);

        /// <summary>
        /// Removes the specified Panel.  There must always be one panel.  If you remove the last panel, the <c>DotSpatial</c> will automatically add one.
        /// </summary>
        /// <param name="index">Zero-Based index of the panel to be removed</param>
        void RemovePanel(int index);

        /// <summary>
        /// Removes the panel object specified.  There must always be one panel.  If you remove the last panel, the <c>DotSpatial</c> will automatically add one.
        /// </summary>
        /// <param name="panel"><c>StatusBarPanel</c> to remove.</param>
        void RemovePanel(ref StatusBarPanel panel);

        /// <summary>
        /// This function makes the progress bar fit into the last panel of the status bar. Call this function whenever you change the size of ANY panel in the status bar.  You do not need to call this on <c>AddPanel</c> or <c>RemovePanel</c>.
        /// </summary>
        void ResizeProgressBar();
    }
}