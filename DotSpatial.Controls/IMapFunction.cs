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
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/5/2008 1:53:08 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System;
using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Interface for map functions.
    /// </summary>
    public interface IMapFunction
    {
        #region Events

        /// <summary>
        /// Occurs when the function is activated
        /// </summary>
        event EventHandler FunctionActivated;

        /// <summary>
        /// Occurs when the function is deactivated.
        /// </summary>
        event EventHandler FunctionDeactivated;

        #endregion

        #region Methods

        /// <summary>
        /// Forces activation
        /// </summary>
        void Activate();

        /// <summary>
        /// Forces deactivation.
        /// </summary>
        void Deactivate();

        /// <summary>
        /// This is the method that is called by the drawPanel.  The graphics coordinates are
        /// in pixels relative to the image being edited.
        /// </summary>
        void Draw(MapDrawArgs e);

        /// <summary>
        /// Forces this tool to execute whatever behavior should occur during a double click even on the panel
        /// </summary>
        /// <param name="e"></param>
        void DoMouseDoubleClick(GeoMouseArgs e);

        /// <summary>
        /// Instructs this tool to perform any actions that should occur on the MouseDown event
        /// </summary>
        /// <param name="e">A MouseEventArgs relative to the drawing panel</param>
        void DoMouseDown(GeoMouseArgs e);

        /// <summary>
        /// Instructs this tool to perform any actions that should occur on the MouseUp event
        /// </summary>
        /// <param name="e">A MouseEventArgs relative to the drawing panel</param>
        void DoMouseUp(GeoMouseArgs e);

        /// <summary>
        /// Instructs this tool to perform any actions that should occur on the MouseMove event
        /// </summary>
        /// <param name="e">A MouseEventArgs relative to the drawing panel</param>
        void DoMouseMove(GeoMouseArgs e);

        /// <summary>
        /// Instructs this tool to perform any actions that should occur on the MouseWheel event
        /// </summary>
        /// <param name="e">A MouseEventArgs relative to the drawing panel</param>
        void DoMouseWheel(GeoMouseArgs e);

        /// <summary>
        /// Here, the entire plugin is unloading, so if there are any residual states
        /// that are not taken care of, this should remove them.
        /// </summary>
        void Unload();

        /// <summary>
        /// When a key is pressed while the map has the focus, this occurs.
        /// </summary>
        /// <param name="e"></param>
        void DoKeyDown(KeyEventArgs e);

        /// <summary>
        /// When a key returns to the up position, this occurs.
        /// </summary>
        /// <param name="e"></param>
        void DoKeyUp(KeyEventArgs e);

        #endregion

        #region Properties

        /// <summary>
        /// This controls the cursor that this tool uses, unless the action has been cancelled by attempting
        /// to use the tool outside the bounds of the image.
        /// </summary>
        Bitmap CursorBitmap
        {
            get;
            set;
        }

        /// <summary>
        /// If this is false, then the typical contents from the map's back buffer are drawn first,
        /// followed by the contents of this tool.
        /// </summary>
        bool PreventBackBuffer
        {
            get;
        }

        /// <summary>
        /// Gets or sets a boolean that is true if this tool should be active.  If it is false,
        /// then this tool will not be sent mouse movement information.
        /// </summary>
        bool Enabled
        {
            get;
        }

        /// <summary>
        /// Different Pathways that allow functions to deactivate if another function that uses
        /// the specified UI domain activates.  This allows a scrolling zoom function to stay
        /// active while changing between pan and select functions which use the left mouse
        /// button.  The enumeration is flagged, and so can support multiple options.
        /// </summary>
        YieldStyles YieldStyle { get; set; }

        /// <summary>
        /// Organizes the map that this tool will work with.
        /// </summary>
        /// <param name="inMap"></param>
        void Init(IMap inMap);

        #endregion

        #region Properties

        /// <summary>
        /// Describes a button image
        /// </summary>
        Image ButtonImage
        {
            get;
        }

        /// <summary>
        /// Gets or sets the basic map that this tool interacts with.  This can alternately be set using
        /// the Init method.
        /// </summary>
        IMap Map
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name that attempts to identify this plugin uniquely.  If the
        /// name is already in the tools list, this will modify the name set here by
        /// appending a number.
        /// </summary>
        string Name
        {
            get;
            set;
        }

        #endregion
    }
}