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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:44:58 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// An object that represents a menu item within the Main Menu
    /// </summary>
    public interface IMenuItem
    {
        /// <summary>
        /// Gets/Sets the Text shown for the MenuItem
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets/Sets the icon for the menu item
        /// </summary>
        Image Picture { get; set; }

        /// <summary>
        /// Gets/Sets the category for this item (used when the user customizes the menu)
        /// </summary>
        string Category { get; set; }

        /// <summary>
        /// Gets/Sets the checked state of the item
        /// </summary>
        bool Checked { get; set; }

        /// <summary>
        /// Gets/Sets the tool tip text that will pop up for the item when a mouse over event occurs
        /// </summary>
        string Tooltip { get; set; }

        /// <summary>
        /// Gets/Sets whether or not this item should draw a dividing line between itself and any
        /// items before this item
        /// </summary>
        bool BeginsGroup { get; set; }

        /// <summary>
        /// Gets/Sets the cursor used when the mouse is over this control
        /// </summary>
        Cursor Cursor { get; set; }

        /// <summary>
        /// Gets/Sets the description of this menu item, used in customization of menu by the user
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets/Sets the Displayed state of this item
        /// </summary>
        bool Displayed { get; set; }

        /// <summary>
        ///	Gets/Sets the enabled state of this item
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets the Name of this item
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets/Sets the visibility state of this item
        /// </summary>
        bool Visible { get; set; }

        /// <summary>
        /// Gets the count of the submenu items contained within this item
        /// </summary>
        int NumSubItems { get; }

        /// <summary>
        /// Returns whether this menu item is the first visible submenu item.
        /// This is only valid in submenus, i.e. menus which have a parent.
        /// </summary>
        bool IsFirstVisibleSubmenuItem { get; }

        /// <summary>
        /// Gets a submenu item by its 0-based index
        /// </summary>
        IMenuItem SubItem(int index);

        /// <summary>
        /// Gets a submenu item by its string name
        /// </summary>
        IMenuItem SubItem(string name);
    }
}