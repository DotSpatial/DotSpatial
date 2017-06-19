// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// MenuItem
    /// </summary>
    public class MenuItem : IMenuItem
    {
        #region Fields

        private readonly ToolStripMenuItem _menuItem;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MenuItem"/> class.
        /// </summary>
        /// <param name="inMenuItem">The ToolStripMenuItem to wrap with this item</param>
        public MenuItem(ToolStripMenuItem inMenuItem)
        {
            _menuItem = inMenuItem;
        }

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not this item should draw a dividing line between itself and any
        /// items before this item
        /// </summary>
        public bool BeginsGroup
        {
            get
            {
                return PreviousItemIsSeparator;
            }

            set
            {
                if (value)
                {
                    if (PreviousItemIsSeparator == false)
                    {
                        InsertSeparator();
                    }
                }
                else
                {
                    if (PreviousItemIsSeparator)
                    {
                        ToolStripMenuItem tm = _menuItem.OwnerItem as ToolStripMenuItem;
                        if (tm != null)
                        {
                            tm.DropDownItems.Remove(PreviousItem);
                        }
                        else
                        {
                            _menuItem.Owner?.Items.Remove(PreviousItem);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the category for this item (used when the user customizes the menu)
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is checked.
        /// </summary>
        public bool Checked
        {
            get
            {
                return _menuItem.Checked;
            }

            set
            {
                _menuItem.Checked = value;
            }
        }

        /// <summary>
        /// Gets or sets the cursor used when the mouse is over this control
        /// </summary>
        public Cursor Cursor
        {
            get
            {
                return _menuItem.Owner.Cursor;
            }

            set
            {
                _menuItem.Owner.Cursor = value;
            }
        }

        /// <summary>
        /// Gets or sets the description of this menu item, used in customization of menu by the user
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the item is displayed.
        /// ... Side Note
        /// ... I have no idea what this is supposed to do, so it is redundant with visible for now
        /// and possibly should be made obsolete.
        /// </summary>
        public bool Displayed
        {
            get
            {
                return _menuItem.Visible;
            }

            set
            {
                _menuItem.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item is enabled.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return _menuItem.Enabled;
            }

            set
            {
                _menuItem.Enabled = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this menu item is the first visible submenu item.
        /// This is only valid in submenus, i.e. menus which have a parent.
        /// </summary>
        public bool IsFirstVisibleSubmenuItem
        {
            get
            {
                ToolStripMenuItem parent = _menuItem.OwnerItem as ToolStripMenuItem;
                if (parent != null)
                {
                    foreach (ToolStripItem tsi in parent.DropDownItems)
                    {
                        if (tsi == _menuItem) return true;
                        if (tsi.Visible) return false;
                    }
                }

                ToolStrip cont = _menuItem.Owner;
                foreach (ToolStripItem tsi in cont.Items)
                {
                    if (tsi == _menuItem) return true;
                    if (tsi.Visible) return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets the Name of this item
        /// </summary>
        public string Name => _menuItem.Name;

        /// <summary>
        /// Gets the count of the submenu items contained within this item
        /// </summary>
        public int NumSubItems => _menuItem.DropDownItems.Count;

        /// <summary>
        /// Gets or sets the icon for the menu item
        /// </summary>
        public Image Picture
        {
            get
            {
                return _menuItem.Image;
            }

            set
            {
                _menuItem.Image = value;
            }
        }

        /// <summary>
        /// Gets or sets the Text shown for the MenuItem
        /// </summary>
        public string Text
        {
            get
            {
                return _menuItem.Text;
            }

            set
            {
                _menuItem.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the tool tip text that will pop up for the item when a mouse over event occurs
        /// </summary>
        public string Tooltip
        {
            get
            {
                return _menuItem.ToolTipText;
            }

            set
            {
                _menuItem.ToolTipText = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the item is visible.
        /// </summary>
        public bool Visible
        {
            get
            {
                return _menuItem.Visible;
            }

            set
            {
                _menuItem.Visible = value;
            }
        }

        private ToolStripItem PreviousItem
        {
            get
            {
                int indx;
                ToolStripMenuItem parent = _menuItem.OwnerItem as ToolStripMenuItem;
                if (parent != null)
                {
                    indx = parent.DropDownItems.IndexOf(_menuItem) - 1;
                    if (indx > -1)
                    {
                        return parent.DropDownItems[indx];
                    }
                }

                ToolStrip cont = _menuItem.Owner;
                indx = cont.Items.IndexOf(_menuItem) - 1;
                if (indx > -1)
                {
                    return cont.Items[indx];
                }

                return null;
            }
        }

        private bool PreviousItemIsSeparator
        {
            get
            {
                ToolStripSeparator ts = PreviousItem as ToolStripSeparator;
                if (ts != null) return true;
                return false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a submenu item by its 0-based index
        /// </summary>
        /// <param name="index">Index of the item that should be returned.</param>
        /// <returns>The MenuItem with the given index.</returns>
        public IMenuItem SubItem(int index)
        {
            return new MenuItem(_menuItem.DropDownItems[index] as ToolStripMenuItem);
        }

        /// <summary>
        /// Gets a submenu item by its string name
        /// </summary>
        /// <param name="name">Name of the item that should be returned.</param>
        /// <returns>The MenuItem with the given name.</returns>
        public IMenuItem SubItem(string name)
        {
            return new MenuItem(_menuItem.DropDownItems[name] as ToolStripMenuItem);
        }

        private void InsertSeparator()
        {
            int indx;
            ToolStripMenuItem parent = _menuItem.OwnerItem as ToolStripMenuItem;
            if (parent != null)
            {
                indx = parent.DropDownItems.IndexOf(_menuItem);
                parent.DropDownItems.Insert(indx, new ToolStripSeparator());
            }

            ToolStrip cont = _menuItem.Owner;
            indx = cont.Items.IndexOf(_menuItem);
            cont.Items.Insert(indx, new ToolStripSeparator());
        }

        #endregion
    }
}