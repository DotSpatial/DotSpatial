// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
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
// Completely Re-conceived March 2008 by Ted Dunsford
// ********************************************************************************************************

using System.Collections.Generic;
using System.Drawing;
using DotSpatial.Data;

namespace DotSpatial.Symbology
{
    /// <summary>
    /// The LegendItem interface controls specifically the
    /// properties associated with the legend.  It is assumed that
    /// All layers will support one legend item, but this way
    /// ostensibly we can eventually add things to the legend that
    /// are not technically layers.
    /// </summary>
    public interface ILegendItem : IDescriptor, IChangeItem, IParentItem<ILegendItem>
    {
        #region Methods

        /// <summary>
        /// Tests the specified legend item to determine whether or not
        /// it can be dropped into the current item.
        /// </summary>
        /// <param name="item">Any object that implements ILegendItem</param>
        /// <returns>Boolean that is true if a drag-drop of the specified item will be allowed.</returns>
        bool CanReceiveItem(ILegendItem item);

        /// <summary>
        /// Instructs this legend item to perform custom drawing for any symbols.
        /// </summary>
        /// <param name="g">A Graphics surface to draw on</param>
        /// <param name="box">The rectangular coordinates that confine the symbol.</param>
        void LegendSymbol_Painted(Graphics g, Rectangle box);

        /// <summary>
        /// Prints the formal legend content without any resize boxes or other notations.
        /// </summary>
        /// <param name="g">The graphics object to print to</param>
        /// <param name="font">The system.Drawing.Font to use for the lettering</param>
        /// <param name="fontColor">The color of the font</param>
        /// <param name="maxExtent">Assuming 0, 0 is the top left, this is the maximum extent</param>
        void PrintLegendItem(Graphics g, Font font, Color fontColor, SizeF maxExtent);

        /// <summary>
        /// Gets the size of the symbol to be drawn to the legend
        /// </summary>
        Size GetLegendSymbolSize();

        #endregion

        #region Properties

        /// <summary>
        /// This is a list of menu items that should appear for this layer.
        /// These are in addition to any that are supported by default.
        /// Handlers should be added to this list before it is retrieved.
        /// </summary>
        List<SymbologyMenuItem> ContextMenuItems
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether or not this legend item should be visible.
        /// This will not be altered unless the LegendSymbolMode is set
        /// to CheckBox.
        /// </summary>
        bool Checked
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether this legend item is expanded.
        /// </summary>
        bool IsExpanded
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets whether this legend item is currently selected (and therefore drawn differently)
        /// </summary>
        bool IsSelected
        {
            get;
            set;
        }

        /// <summary>
        /// Gets whatever the child collection is and returns it as an IEnumerable set of legend items
        /// in order to make it easier to cycle through those values.
        /// </summary>
        IEnumerable<ILegendItem> LegendItems
        {
            get;
        }

        /// <summary>
        /// Gets or sets a boolean, that if false will prevent this item, or any of its child items
        /// from appearing in the legend when the legend is drawn.
        /// </summary>
        bool LegendItemVisible
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the symbol mode for this legend item.
        /// </summary>
        SymbolMode LegendSymbolMode
        {
            get;
        }

        /// <summary>
        /// The text that will appear in the legend
        /// </summary>
        string LegendText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a pre-defined behavior in the legend when referring to drag and drop functionality.
        /// </summary>
        LegendType LegendType
        {
            get;
        }

        #endregion
    }
}