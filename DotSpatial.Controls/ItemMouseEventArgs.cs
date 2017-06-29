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
// The Initial Developer of this Original Code is Ted Dunsford. Created 12/3/2008 1:56:51 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Windows.Forms;

namespace DotSpatial.Controls
{
    /// <summary>
    /// ItemMouseEventArgs
    /// </summary>
    public class ItemMouseEventArgs : MouseEventArgs
    {
        #region Private Variables

        LegendBox _box;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of an ItemMouseEventArgs
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
            _box = inItemBox;
        }

        /// <summary>
        /// Creates a new instance of ItemMouseEventArgs from an existing MouseEventArgs.
        /// </summary>
        /// <param name="args">The existing arguments</param>
        /// <param name="inItemBox">A LegendBox for comparison</param>
        public ItemMouseEventArgs(MouseEventArgs args, LegendBox inItemBox)
            : base(args.Button, args.Clicks, args.X, args.Y, args.Delta)
        {
            _box = inItemBox;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the item that received the mouse down, plus the various rectangular extents encoded in the various boxes.
        /// </summary>
        public LegendBox ItemBox
        {
            get { return _box; }
            protected set { _box = value; }
        }

        #endregion
    }
}