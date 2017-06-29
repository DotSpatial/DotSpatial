// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The core libraries for the DotSpatial project.
//
// ********************************************************************************************************
// The contents of this file are subject to the MIT License (MIT)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is DotSpatial.dll for the DotSpatial project
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in August, 2007.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// The extended and complete set of events associated with the PreviewMap
    /// Implementing this avoids having to manually add handlers for every event.
    /// </summary>
    [Obsolete("Do not use it. This interface is not used in DotSpatial anymore.")] // Marked in 1.7
    public interface IPreviewMapHandler
    {
        /// <summary>
        /// Occurs when a drag-and-drop operation is completed.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A DragEventArgs with the drag parameters</param>
        void PreviewMap_DragDrop(object sender, DragEventArgs e);

        /// <summary>
        /// Occurs when the visible region being displayed on the map changes
        /// </summary>
        void Map_ExtentsChanged(object sender, EventArgs e);

        /// <summary>
        /// Occurs when the control is clicked by the mouse.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A MouseEventArgs with any parameters</param>
        void PreviewMap_MouseClick(object sender, MouseEventArgs e);

        /// <summary>
        /// Occurs when the control is double clicked by the mouse.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A MouseEventArgs with any parameters</param>
        void PreviewMap_MouseDoubleClick(object sender, MouseEventArgs e);

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a mouse button is pressed.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A MouseEventArgs with any parameters</param>
        void PreviewMap_MouseDown(object sender, MouseEventArgs e);

        /// <summary>
        /// Occurs when the mouse pointer is moved over the control.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A MouseEventArgs with any parameters</param>
        void PreviewMap_MouseMove(object sender, MouseEventArgs e);

        /// <summary>
        /// Occurs when the mouse pointer is over the control and a mouse button is released.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A MouseEventArgs with any parameters</param>
        void PreviewMap_MouseUp(object sender, MouseEventArgs e);

        /// <summary>
        /// Occurs when the mouse wheel moves while the control has focus.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A MouseEventArgs with any parameters</param>
        void PreviewMap_MouseWheel(object sender, MouseEventArgs e);

        /// <summary>
        /// Occurs when the control is redrawn.
        /// </summary>
        /// <param name="sender">An instance of the calling object</param>
        /// <param name="e">A PaintEventArgs with any parameters</param>
        void PreviewMap_Paint(object sender, PaintEventArgs e);
    }
}