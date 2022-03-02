// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// During the Initialize method, the.
    /// </summary>
    public interface IPlugin
    {
        #region Properties

        /// <summary>
        /// Gets the Author of the plugin.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Gets the Build date.
        /// </summary>
        string BuildDate { get; }

        /// <summary>
        /// Gets the Short description of the plugin.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the Name of the plugin.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the Plugin serial number.  NO LONGER NEEDED; kept for backward compatibility.
        /// </summary>
        string SerialNumber { get; }

        /// <summary>
        /// Gets the Plugin version.
        /// </summary>
        string Version { get; }

        #endregion

        #region Methods

        /// <summary>
        /// This method is called by the DotSpatial when the plugin is loaded.
        /// </summary>
        /// <param name="mapWin">The <c>IMapWin</c> interface to use when interacting with the DotSpatial.</param>
        /// <param name="parentHandle">The windows handle of the DotSpatial form.  This can be used to make the DotSpatial the parent of any forms in the plugin.</param>
        void Initialize(IMapWin mapWin, int parentHandle);

        /// <summary>
        /// This method is called by the DotSpatial when a toolbar or menu item is clicked.
        /// </summary>
        /// <param name="itemName">Name of the item that was clicked.</param>
        /// <param name="handled">Set this parameter to true if your plugin handles this so that no other plugins recieve this message.</param>
        void ItemClicked(string itemName, ref bool handled);

        /// <summary>
        /// This method is called by the DotSpatial when a layer is removed from the map.
        /// </summary>
        /// <param name="handle">Handle of the layer that was removed.</param>
        void LayerRemoved(int handle);

        /// <summary>
        /// This method is called by the DotSpatial when one or more layer(s) is/are added.
        /// </summary>
        /// <param name="layers">An array of <c>Layer</c> objects that were added.</param>
        void LayersAdded(ILayerOld[] layers);

        /// <summary>
        /// This method is called by the DotSpatial when all layers are cleared from the map.
        /// </summary>
        void LayersCleared();

        /// <summary>
        /// This method is called by the DotSpatial when a layer is selected in code or by the legend.
        /// </summary>
        /// <param name="handle">Handle of the newly selected layer.</param>
        void LayerSelected(int handle);

        /// <summary>
        /// This method is called by the DotSpatial when the user double-clicks on the legend.
        /// </summary>
        /// <param name="handle">Handle of the layer or group that was double-clicked.</param>
        /// <param name="location">Location that was clicked.  Either a layer or a group.</param>
        /// <param name="handled">Set this parameter to true if your plugin handles this so that no other plugins recieve this message.</param>
        void LegendDoubleClick(int handle, ClickLocation location, ref bool handled);

        /// <summary>
        /// This method is called by the DotSpatial when the user presses a mouse button on the legend.
        /// </summary>
        /// <param name="handle">Handle of the layer or group that was under the cursor.</param>
        /// <param name="button">The mouse button that was pressed.  You can use the <c>vb6Buttons</c> enumeration to determine which button was pressed.</param>
        /// <param name="location">Location that was clicked.  Either a layer or a group.</param>
        /// <param name="handled">Set this parameter to true if your plugin handles this so that no other plugins recieve this message.</param>
        void LegendMouseDown(int handle, int button, ClickLocation location, ref bool handled);

        /// <summary>
        /// This method is called by the DotSpatial when the user releases a mouse button on the legend.
        /// </summary>
        /// <param name="handle">Handle of the layer or group that was under the cursor.</param>
        /// <param name="button">The mouse button that was released.  You can use the <c>vb6Buttons</c> enumeration to determine which button it was.</param>
        /// <param name="location">Location that was clicked.  Either a layer or a group.</param>
        /// <param name="handled">Set this parameter to true if your plugin handles this so that no other plugins recieve this message.</param>
        void LegendMouseUp(int handle, int button, ClickLocation location, ref bool handled);

        /// <summary>
        /// This method is called by the DotSpatial when the user completes a dragging operation on the map.
        /// </summary>
        /// <param name="bounds">The rectangle that was selected, in pixel coordinates.</param>
        /// <param name="handled">Set this parameter to true if your plugin handles this so that no other plugins recieve this message.</param>
        void MapDragFinished(Rectangle bounds, ref bool handled);

        /// <summary>
        /// This method is called by the DotSpatial when the extents of the map have changed.
        /// </summary>
        void MapExtentsChanged();

        /// <summary>
        /// This method is called by the DotSpatial when the user presses a mouse button over the map.
        /// </summary>
        /// <param name="button">The button that was pressed.  These values can be compared to the <c>vb6Butttons</c> enumerations.</param>
        /// <param name="shift">This value represents the state of the <c>ctrl</c>, <c>alt</c> and <c>shift</c> keyboard buttons.  The values use VB6 values.</param>
        /// <param name="x">The x coordinate in pixels.</param>
        /// <param name="y">The y coordinate in pixels.</param>
        /// <param name="handled">Set this parameter to true if your plugin handles this so that no other plugins recieve this message.</param>
        void MapMouseDown(int button, int shift, int x, int y, ref bool handled);

        /// <summary>
        /// This method is called by the DotSpatial when the user moves the mouse over the map display.
        /// </summary>
        /// <param name="screenX">The x coordinate in pixels.</param>
        /// <param name="screenY">The y coordinate in pixels.</param>
        /// <param name="handled">Set this parameter to true if your plugin handles this so that no other plugins recieve this message.</param>
        void MapMouseMove(int screenX, int screenY, ref bool handled);

        /// <summary>
        /// This method is called by the DotSpatial when the user releases a mouse button over the map.
        /// </summary>
        /// <param name="button">The button that was released.  These values can be compared to the <c>vb6Butttons</c> enumerations.</param>
        /// <param name="shift">This value represents the state of the <c>ctrl</c>, <c>alt</c> and <c>shift</c> keyboard buttons.  The values use VB6 values.</param>
        /// <param name="x">The x coordinate in pixels.</param>
        /// <param name="y">The y coordinate in pixels.</param>
        /// <param name="handled">Set this parameter to true if your plugin handles this so that no other plugins recieve this message.</param>
        void MapMouseUp(int button, int shift, int x, int y, ref bool handled);

        /// <summary>
        /// This message is relayed by the DotSpatial when another plugin broadcasts a message.  Messages can be used to send messages between plugins.
        /// </summary>
        /// <param name="msg">The message being relayed.</param>
        /// <param name="handled">Set this parameter to true if your plugin handles this so that no other plugins recieve this message.</param>
        void Message(string msg, ref bool handled);

        /// <summary>
        /// This method is called by the DotSpatial when a project is being loaded.
        /// </summary>
        /// <param name="projectFile">Filename of the project file.</param>
        /// <param name="settingsString">Settings string for this plugin from the project file.</param>
        void ProjectLoading(string projectFile, string settingsString);

        /// <summary>
        /// This method is called by the DotSpatial when a project is being saved.
        /// </summary>
        /// <param name="projectFile">Filename of the project file.</param>
        /// <param name="settingsString">Reference parameter.  Set this value in order to save your plugin's settings string in the project file.</param>
        void ProjectSaving(string projectFile, ref string settingsString);

        /// <summary>
        /// This method is called by the DotSpatial when shapes are selected by the user.
        /// </summary>
        /// <param name="handle">Handle of the shapefile layer that was selected on.</param>
        /// <param name="selectInfo">The <c>SelectInfo</c> object containing information about the selected shapes.</param>
        void ShapesSelected(int handle, ISelectInfo selectInfo);

        /// <summary>
        /// This method is called by the DotSpatial when the plugin is unloaded.
        /// </summary>
        void Terminate();

        #endregion
    }
}