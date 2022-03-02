// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Used for manipulation of the menu system for the application.
    /// </summary>
    public interface IMenus
    {
        /// <summary>
        /// Gets a MenuItem by its name.
        /// </summary>
        /// <param name="menuName">Name of the item that shpuld be returned.</param>
        IMenuItem this[string menuName] { get; }

        #region Methods

        /// <summary>
        /// Adds a menu with the specified name and uses the name as the text.
        /// </summary>
        /// <param name="name">The string name of the menu item to add.</param>
        /// <returns>The menu item that was created.</returns>
        IMenuItem AddMenu(string name);

        /// <summary>
        /// Adds a menu with the specified name and image, and uses the name as the text.
        /// </summary>
        /// <param name="name">the string name of the menu item and text.</param>
        /// <param name="picture">The image to associate with the menu item.</param>
        /// <returns>The menu item that was created.</returns>
        IMenuItem AddMenu(string name, Image picture);

        /// <summary>
        /// Adds a menu with the specified name, icon and text.
        /// </summary>
        /// <param name="name">The name to use to identify this item later.</param>
        /// <param name="picture">An image to associate with this item.</param>
        /// <param name="text">The string text to appear for this item.</param>
        /// <returns>The menu item that was created.</returns>
        IMenuItem AddMenu(string name, Image picture, string text);

        /// <summary>
        /// Adds a menu with the specified name to the menu indicated by ParentMenu.
        /// </summary>
        /// <param name="name">The string name to use.</param>
        /// <param name="parentMenu">The string name of the parent to add the menu to.</param>
        /// <returns>The menu item that was created.</returns>
        IMenuItem AddMenu(string name, string parentMenu);

        /// <summary>
        /// Adds a menu with the specified name and icon to the menu indicated by ParentMenu.
        /// </summary>
        /// <param name="name">the name to use as a key to identify this item and as text.</param>
        /// <param name="parentMenu">the parent menu item.</param>
        /// <param name="picture">The image to use for this item.</param>
        /// <returns>The menu item that was created.</returns>
        IMenuItem AddMenu(string name, string parentMenu, Image picture);

        /// <summary>
        /// Adds a menu with the specified name, icon and text to the specified ParentMenu.
        /// </summary>
        /// <param name="name">The string name to use as a key for this item.</param>
        /// <param name="parentMenu">The name of the parent menu to add this new item to.</param>
        /// <param name="picture">The picture to add for this item.</param>
        /// <param name="text">The string text to add for this item.</param>
        /// <returns>The menu item that was created.</returns>
        IMenuItem AddMenu(string name, string parentMenu, Image picture, string text);

        /// <summary>
        /// Adds a menu with the specified name, icon and text to the specified ParentMenu and after the specifed item.
        /// </summary>
        /// <param name="name">The string name to use as a key for this item.</param>
        /// <param name="parentMenu">The name of the parent menu to add this new item to.</param>
        /// <param name="picture">The picture to add for this item.</param>
        /// <param name="text">The string text to add for this item.</param>
        /// <param name="after">The name of the sibling menu item to add this item directly after.</param>
        /// <returns>The menu item that was created.</returns>
        IMenuItem AddMenu(string name, string parentMenu, Image picture, string text, string after);

        /// <summary>
        /// Adds a menu with the specified name and text to the specified ParentMenu and before the specified item.
        /// </summary>
        /// <param name="name">The string name to use as a key for this item.</param>
        /// <param name="parentMenu">The name of the parent menu to add this new item to.</param>
        /// <param name="text">The string text to add for this item.</param>
        /// <param name="before">The name of the sibling to insert this menu item on top of.</param>
        /// <returns>The menu item that was created.</returns>
        IMenuItem AddMenu(string name, string parentMenu, string text, string before);

        /// <summary>
        /// Removes a MenuItem.
        /// </summary>
        /// <param name="name">Name of the item to remove.</param>
        /// <returns>true on success, false otherwise.</returns>
        bool Remove(string name);

        #endregion
    }
}