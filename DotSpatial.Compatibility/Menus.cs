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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/21/2009 1:35:36 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// Menus
    /// </summary>
    public class Menus : IMenus
    {
        #region Private Variables

        private readonly MenuStrip _menuStrip;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Menus
        /// </summary>
        public Menus(MenuStrip inMenuStrip)
        {
            _menuStrip = inMenuStrip;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a menu with the specified name and uses the name as the text.
        /// </summary>
        /// <param name="name">The string name of the menu item to add.</param>
        public IMenuItem AddMenu(string name)
        {
            ToolStripMenuItem mi = new ToolStripMenuItem { Name = name, Text = name };
            _menuStrip.Items.Add(mi);
            return new MenuItem(mi);
        }

        /// <summary>
        /// Adds a menu with the specified name and image, and uses the name as the text.
        /// </summary>
        /// <param name="name">the string name of the menu item and text</param>
        /// <param name="picture">The image to associate with the menu item</param>
        public IMenuItem AddMenu(string name, Image picture)
        {
            ToolStripMenuItem mi = new ToolStripMenuItem { Name = name, Text = name, Image = picture };
            _menuStrip.Items.Add(mi);
            return new MenuItem(mi);
        }

        /// <summary>
        /// Adds a menu with the specified name, icon and text
        /// </summary>
        /// <param name="name">The name to use to identify this item later</param>
        /// <param name="picture">An image to associate with this item</param>
        /// <param name="text">The string text to appear for this item</param>
        public IMenuItem AddMenu(string name, Image picture, string text)
        {
            ToolStripMenuItem mi = new ToolStripMenuItem { Name = name, Text = text, Image = picture };
            _menuStrip.Items.Add(mi);
            return new MenuItem(mi);
        }

        /// <summary>
        /// Adds a menu with the specified name to the menu indicated by ParentMenu
        /// </summary>
        /// <param name="name">The string name to use</param>
        /// <param name="parentMenu">The string name of the parent to add the menu to</param>
        public IMenuItem AddMenu(string name, string parentMenu)
        {
            ToolStripMenuItem mi = new ToolStripMenuItem { Name = name, Text = name };
            _menuStrip.Items.Add(mi);
            return new MenuItem(mi);
        }

        /// <summary>
        /// Adds a menu with the specified name and icon to the menu indicated by ParentMenu
        /// </summary>
        /// <param name="name">the name to use as a key to identify this item and as text</param>
        /// <param name="parentMenu">the parent menu item</param>
        /// <param name="picture">The image to use for this item</param>
        public IMenuItem AddMenu(string name, string parentMenu, Image picture)
        {
            ToolStripMenuItem parent = _menuStrip.Items.Find(parentMenu, true)[0] as ToolStripMenuItem;
            ToolStripMenuItem mi = new ToolStripMenuItem { Name = name, Text = name, Image = picture };
            if (parent != null) parent.DropDownItems.Add(mi);
            return new MenuItem(mi);
        }

        /// <summary>
        /// Adds a menu with the specified name, icon and text to the specified ParentMenu
        /// </summary>
        /// <param name="name">The string name to use as a key for this item</param>
        /// <param name="parentMenu">The name of the parent menu to add this new item to</param>
        /// <param name="picture">The picture to add for this item</param>
        /// <param name="text">The string text to add for this item</param>
        public IMenuItem AddMenu(string name, string parentMenu, Image picture, string text)
        {
            ToolStripMenuItem parent = _menuStrip.Items.Find(parentMenu, true)[0] as ToolStripMenuItem;
            ToolStripMenuItem mi = new ToolStripMenuItem { Name = name, Text = text, Image = picture };
            if (parent != null) parent.DropDownItems.Add(mi);
            return new MenuItem(mi);
        }

        /// <summary>
        /// Adds a menu with the specified name, icon and text to the specified ParentMenu and after the specifed item
        /// </summary>
        /// <param name="name">The string name to use as a key for this item</param>
        /// <param name="parentMenu">The name of the parent menu to add this new item to</param>
        /// <param name="picture">The picture to add for this item</param>
        /// <param name="text">The string text to add for this item</param>
        /// <param name="after">The name of the sibling menu item to add this item directly after</param>
        public IMenuItem AddMenu(string name, string parentMenu, Image picture, string text, string after)
        {
            ToolStripMenuItem parent = _menuStrip.Items.Find(parentMenu, true)[0] as ToolStripMenuItem;
            ToolStripMenuItem mi = new ToolStripMenuItem { Name = name, Text = text, Image = picture };
            if (parent != null)
                parent.DropDownItems.Insert(parent.DropDownItems.IndexOf(parent.DropDownItems.Find(after, false)[0]) + 1, mi);
            return new MenuItem(mi);
        }

        /// <summary>
        /// Adds a menu with the specified name and text to the specified ParentMenu and before the specifed item
        /// </summary>
        /// <param name="name">The string name to use as a key for this item</param>
        /// <param name="parentMenu">The name of the parent menu to add this new item to</param>
        /// <param name="text">The string text to add for this item</param>
        /// <param name="before">The name of the sibling to insert this menu item on top of</param>
        public IMenuItem AddMenu(string name, string parentMenu, string text, string before)
        {
            ToolStripMenuItem parent = _menuStrip.Items.Find(parentMenu, true)[0] as ToolStripMenuItem;
            ToolStripMenuItem mi = new ToolStripMenuItem { Name = name, Text = text };
            if (parent != null)
                parent.DropDownItems.Insert(parent.DropDownItems.IndexOf(parent.DropDownItems.Find(before, false)[0]), mi);
            return new MenuItem(mi);
        }

        /// <summary>
        /// Removes a MenuItem
        /// </summary>
        /// <param name="name">Name of the item to remove</param>
        /// <returns>true on success, false otherwise</returns>
        public bool Remove(string name)
        {
            ToolStripMenuItem tsmi = _menuStrip.Items.Find(name, true)[0] as ToolStripMenuItem;
            if (tsmi == null) return false;
            _menuStrip.Items.Remove(tsmi);
            return true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a MenuItem by its name
        /// </summary>
        public IMenuItem this[string menuName]
        {
            get
            {
                return new MenuItem(_menuStrip.Items.Find(menuName, true)[0] as ToolStripMenuItem);
            }
        }

        #endregion
    }
}