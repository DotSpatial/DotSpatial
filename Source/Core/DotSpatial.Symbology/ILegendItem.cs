// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using System.Drawing;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// The LegendItem interface controls specifically the properties associated with the legend.
    /// It is assumed that all layers will support one legend item, but this way ostensibly we
    /// can eventually add things to the legend that are not technically layers.
    /// </summary>
    public interface ILegendItem : IDescriptor, IChangeItem, IParentItem<ILegendItem>
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not this legend item should be visible.
        /// This will not be altered unless the LegendSymbolMode is set to CheckBox.
        /// </summary>
        bool Checked { get; set; }

        /// <summary>
        /// Gets or sets a list of menu items that should appear for this layer.
        /// These are in addition to any that are supported by default.
        /// Handlers should be added to this list before it is retrieved.
        /// </summary>
        List<SymbologyMenuItem> ContextMenuItems { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this legend item is expanded.
        /// </summary>
        bool IsExpanded { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this legend item is currently selected (and therefore drawn differently).
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// Gets whatever the child collection is and returns it as an IEnumerable set of legend items
        /// in order to make it easier to cycle through those values.
        /// </summary>
        IEnumerable<ILegendItem> LegendItems { get; }

        /// <summary>
        /// Gets or sets a value indicating whether this item and its child items
        /// appear in the legend when the legend is drawn.
        /// </summary>
        bool LegendItemVisible { get; set; }

        /// <summary>
        /// Gets the symbol mode for this legend item.
        /// </summary>
        SymbolMode LegendSymbolMode { get; }

        /// <summary>
        /// Gets or sets the text that will appear in the legend.
        /// </summary>
        string LegendText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user can change the legend text in GUI.
        /// </summary>
        bool LegendTextReadOnly { get; set; }

        /// <summary>
        /// Gets a pre-defined behavior in the legend when referring to drag and drop functionality.
        /// </summary>
        LegendType LegendType { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Tests the specified legend item to determine whether or not
        /// it can be dropped into the current item.
        /// </summary>
        /// <param name="item">Any object that implements ILegendItem.</param>
        /// <returns>Boolean that is true if a drag-drop of the specified item will be allowed.</returns>
        bool CanReceiveItem(ILegendItem item);

        /// <summary>
        /// Gets the size of the symbol to be drawn to the legend.
        /// </summary>
        /// <returns>The size of the symbol to be drawn to the legend.</returns>
        Size GetLegendSymbolSize();

        /// <summary>
        /// Instructs this legend item to perform custom drawing for any symbols.
        /// </summary>
        /// <param name="g">A Graphics surface to draw on.</param>
        /// <param name="box">The rectangular coordinates that confine the symbol.</param>
        void LegendSymbolPainted(Graphics g, Rectangle box);

        /// <summary>
        /// Prints the formal legend content without any resize boxes or other notations.
        /// </summary>
        /// <param name="g">The graphics object to print to.</param>
        /// <param name="font">The system.Drawing.Font to use for the lettering.</param>
        /// <param name="fontColor">The color of the font.</param>
        /// <param name="maxExtent">Assuming 0, 0 is the top left, this is the maximum extent.</param>
        void PrintLegendItem(Graphics g, Font font, Color fontColor, SizeF maxExtent);

        #endregion
    }
}