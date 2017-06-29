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
        #region Private Variables

        private Rectangle _bounds;
        private Rectangle _checkbox;
        private Rectangle _expandBox;
        private int _indent;
        private ILegendItem _item;
        private Rectangle _symbolBox;
        private Rectangle _textBox;

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the bounds for this LegendBox
        /// </summary>
        public Rectangle Bounds
        {
            get { return _bounds; }
            set { _bounds = value; }
        }

        /// <summary>
        /// Gets or sets the actual item that this bounds is associated with
        /// </summary>
        public ILegendItem Item
        {
            get { return _item; }
            set { _item = value; }
        }

        /// <summary>
        /// If this item is in checkbox mode, then this is the physical location of the checkbox
        /// </summary>
        public Rectangle CheckBox
        {
            get { return _checkbox; }
            set { _checkbox = value; }
        }

        /// <summary>
        /// IF this item is a groupable item, this is the region for the expanding box for the item
        /// </summary>
        public Rectangle ExpandBox
        {
            get { return _expandBox; }
            set { _expandBox = value; }
        }

        /// <summary>
        /// Gets or sets the rectangle that corresponds with text.
        /// </summary>
        public Rectangle Textbox
        {
            get { return _textBox; }
            set { _textBox = value; }
        }

        /// <summary>
        /// gets or sets the symbol box
        /// </summary>
        public Rectangle SymbolBox
        {
            get { return _symbolBox; }
            set { _symbolBox = value; }
        }

        /// <summary>
        /// Gets or sets the integer number of indentations.  This should be used
        /// in coordination with whatever the indentation amount is for the specific legend.
        /// </summary>
        public int Indent
        {
            get { return _indent; }
            set { _indent = value; }
        }

        #endregion
    }
}