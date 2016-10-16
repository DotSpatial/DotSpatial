// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 1:44:15 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Used for manipulation of the menu system for the application
    /// </summary>
    public interface IMenus
    {
        /// <summary>
        /// Gets a MenuItem by its name
        /// </summary>
        IMenuItem this[string menuName] { get; }

        /// <summary>
        /// Adds a menu with the specified name
        /// </summary>
        IMenuItem AddMenu(string name);

        /// <summary>
        /// Adds a menu with the specified name and icon
        /// </summary>
        IMenuItem AddMenu(string name, Image picture);

        /// <summary>
        /// Adds a menu with the specified name, icon and text
        /// </summary>
        IMenuItem AddMenu(string name, Image picture, string text);

        /// <summary>
        /// Adds a menu with the specified name to the menu indicated by ParentMenu
        /// </summary>
        IMenuItem AddMenu(string name, string parentMenu);

        /// <summary>
        /// Adds a menu with the specified name and icon to the menu indicated by ParentMenu
        /// </summary>
        IMenuItem AddMenu(string name, string parentMenu, Image picture);

        /// <summary>
        /// Adds a menu with the specified name, icon and text to the specified ParentMenu
        /// </summary>
        IMenuItem AddMenu(string name, string parentMenu, Image picture, string text);

        /// <summary>
        /// Adds a menu with the specified name, icon and text to the specified ParentMenu and after the specifed item
        /// </summary>
        IMenuItem AddMenu(string name, string parentMenu, Image picture, string text, string after);

        /// <summary>
        /// Adds a menu with the specified name and text to the specified ParentMenu and before the specifed item
        /// </summary>
        IMenuItem AddMenu(string name, string parentMenu, string text, string before);

        /// <summary>
        /// Removes a MenuItem
        /// </summary>
        /// <param name="name">Name of the item to remove</param>
        /// <returns>true on success, false otherwise</returns>
        bool Remove(string name);
    }
}