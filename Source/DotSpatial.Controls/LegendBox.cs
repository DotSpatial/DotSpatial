// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 12/3/2008 1:28:26 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// A Legend Box encapsulates the basic drawing information for an item.
    /// This will not capture information about sub-items.
    /// </summary>
    public class LegendBox
    {
        #region Properties

        /// <summary>
        /// Gets or sets the bounds for this LegendBox.
        /// </summary>
        public Rectangle Bounds { get; set; }

        /// <summary>
        /// Gets or sets the physical location of the checkbox.
        /// </summary>
        public Rectangle CheckBox { get; set; }

        /// <summary>
        /// Gets or sets the region for the expanding box for the item.
        /// </summary>
        public Rectangle ExpandBox { get; set; }

        /// <summary>
        /// Gets or sets the integer number of indentations. This should be used
        /// in coordination with whatever the indentation amount is for the specific legend.
        /// </summary>
        public int Indent { get; set; }

        /// <summary>
        /// Gets or sets the actual item that this bounds is associated with.
        /// </summary>
        public ILegendItem Item { get; set; }

        /// <summary>
        /// Gets or sets the symbol box.
        /// </summary>
        public Rectangle SymbolBox { get; set; }

        /// <summary>
        /// Gets or sets the rectangle that corresponds with text.
        /// </summary>
        public Rectangle Textbox { get; set; }

        #endregion
    }
}