// ********************************************************************************************************
// Product Name: DotSpatial.Controls.dll
// Description:  The Windows Forms user interface controls like the map, legend, toolbox, ribbon and others.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 12/3/2008 1:56:51 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Event args of a mouse event on an item.
    /// </summary>
    public class ItemMouseEventArgs : MouseEventArgs
    {
        #region Fields

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemMouseEventArgs"/> class.
        /// </summary>
        /// <param name="inButton">The Mouse Buttons</param>
        /// <param name="inClicks">The number of clicks</param>
        /// <param name="inX">The X coordinate</param>
        /// <param name="inY">The Y coordinate</param>
        /// <param name="inDelta">The delta of the mouse wheel</param>
        /// <param name="inItemBox">A LegendBox for comparision</param>
        public ItemMouseEventArgs(MouseButtons inButton, int inClicks, int inX, int inY, int inDelta, LegendBox inItemBox)
            : base(inButton, inClicks, inX, inY, inDelta)
        {
            ItemBox = inItemBox;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemMouseEventArgs"/> class from an existing MouseEventArgs.
        /// </summary>
        /// <param name="args">The existing arguments</param>
        /// <param name="inItemBox">A LegendBox for comparison</param>
        public ItemMouseEventArgs(MouseEventArgs args, LegendBox inItemBox)
            : base(args.Button, args.Clicks, args.X, args.Y, args.Delta)
        {
            ItemBox = inItemBox;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the item that received the mouse down, plus the various rectangular extents encoded in the various boxes.
        /// </summary>
        public LegendBox ItemBox { get; protected set; }

        #endregion
    }
}