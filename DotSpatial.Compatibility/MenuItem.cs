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
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/21/2009 2:44:39 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Drawing;
using System.Windows.Forms;

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// MenuItem
    /// </summary>
    public class MenuItem : IMenuItem
    {
        #region Private Variables

        private readonly ToolStripMenuItem _menuItem;
        private string _category;
        private string _description;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of MenuItem
        /// </summary>
        /// <param name="inMenuItem">The ToolStripMenuItem to wrap with this item</param>
        public MenuItem(ToolStripMenuItem inMenuItem)
        {
            _menuItem = inMenuItem;
        }

        #endregion

        #region Methods

        #endregion

        #region Properties

        /// <summary>
        /// Gets/Sets the Text shown for the MenuItem
        /// </summary>
        public string Text
        {
            get { return _menuItem.Text; }
            set { _menuItem.Text = value; }
        }

        /// <summary>
        /// Gets/Sets the icon for the menu item
        /// </summary>
        public Image Picture
        {
            get { return _menuItem.Image; }
            set { _menuItem.Image = value; }
        }

        /// <summary>
        /// Gets/Sets the category for this item (used when the user customizes the menu)
        /// </summary>
        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }

        /// <summary>
        /// Gets/Sets the checked state of the item
        /// </summary>
        public bool Checked
        {
            get { return _menuItem.Checked; }
            set { _menuItem.Checked = value; }
        }

        /// <summary>
        /// Gets/Sets the tool tip text that will pop up for the item when a mouse over event occurs
        /// </summary>
        public string Tooltip
        {
            get { return _menuItem.ToolTipText; }
            set { _menuItem.ToolTipText = value; }
        }

        /// <summary>
        /// Gets/Sets whether or not this item should draw a dividing line between itself and any
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
                            if (_menuItem.Owner != null)
                            {
                                _menuItem.Owner.Items.Remove(PreviousItem);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets/Sets the cursor used when the mouse is over this control
        /// </summary>
        public Cursor Cursor
        {
            get { return _menuItem.Owner.Cursor; }
            set { _menuItem.Owner.Cursor = value; }
        }

        /// <summary>
        /// Gets/Sets the description of this menu item, used in customization of menu by the user
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Gets/Sets the Displayed state of this item
        /// ... Side Note
        /// ... I have no idea what this is supposed to do, so it is redundant with visible for now
        /// and possibly should be made obsolete.
        /// </summary>
        public bool Displayed
        {
            get { return _menuItem.Visible; }
            set { _menuItem.Visible = value; }
        }

        /// <summary>
        ///	Gets/Sets the enabled state of this item
        /// </summary>
        public bool Enabled
        {
            get { return _menuItem.Enabled; }
            set { _menuItem.Enabled = value; }
        }

        /// <summary>
        /// Gets the Name of this item
        /// </summary>
        public string Name
        {
            get { return _menuItem.Name; }
        }

        /// <summary>
        /// Gets/Sets the visibility state of this item
        /// </summary>
        public bool Visible
        {
            get { return _menuItem.Visible; }
            set { _menuItem.Visible = true; }
        }

        /// <summary>
        /// Gets the count of the submenu items contained within this item
        /// </summary>
        public int NumSubItems
        {
            get { return _menuItem.DropDownItems.Count; }
        }

        /// <summary>
        /// Gets a submenu item by its 0-based index
        /// </summary>
        public IMenuItem SubItem(int index)
        {
            return new MenuItem(_menuItem.DropDownItems[index] as ToolStripMenuItem);
        }

        /// <summary>
        /// Gets a submenu item by its string name
        /// </summary>
        public IMenuItem SubItem(string name)
        {
            return new MenuItem(_menuItem.DropDownItems[name] as ToolStripMenuItem);
        }

        /// <summary>
        /// Returns whether this menu item is the first visible submenu item.
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

        #endregion

        private bool PreviousItemIsSeparator
        {
            get
            {
                ToolStripSeparator ts = PreviousItem as ToolStripSeparator;
                if (ts != null) return true;
                return false;
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
            return;
        }
    }
}