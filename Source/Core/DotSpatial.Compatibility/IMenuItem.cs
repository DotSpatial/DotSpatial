// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// An object that represents a menu item within the Main Menu.
    /// </summary>
    public interface IMenuItem
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not this item should draw a dividing line between itself and any
        /// items before this item.
        /// </summary>
        bool BeginsGroup { get; set; }

        /// <summary>
        /// Gets or sets the category for this item (used when the user customizes the menu).
        /// </summary>
        string Category { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is checked.
        /// </summary>
        bool Checked { get; set; }

        /// <summary>
        /// Gets or sets the cursor used when the mouse is over this control.
        /// </summary>
        Cursor Cursor { get; set; }

        /// <summary>
        /// Gets or sets the description of this menu item, used in customization of menu by the user.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is displayed.
        /// </summary>
        bool Displayed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is enabled.
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        /// Gets a value indicating whether this menu item is the first visible submenu item.
        /// This is only valid in submenus, i.e. menus which have a parent.
        /// </summary>
        bool IsFirstVisibleSubmenuItem { get; }

        /// <summary>
        /// Gets the Name of this item.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the count of the submenu items contained within this item.
        /// </summary>
        int NumSubItems { get; }

        /// <summary>
        /// Gets or sets the icon for the menu item.
        /// </summary>
        Image Picture { get; set; }

        /// <summary>
        /// Gets or sets the Text shown for the MenuItem.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets the tool tip text that will pop up for the item when a mouse over event occurs.
        /// </summary>
        string Tooltip { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this item is visible.
        /// </summary>
        bool Visible { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a submenu item by its 0-based index.
        /// </summary>
        /// <param name="index">Index of the item that should be returned.</param>
        /// <returns>The MenuItem with the given index.</returns>
        IMenuItem SubItem(int index);

        /// <summary>
        /// Gets a submenu item by its string name.
        /// </summary>
        /// <param name="name">Name of the item that should be returned.</param>
        /// <returns>The MenuItem with the given name.</returns>
        IMenuItem SubItem(string name);

        #endregion
    }
}