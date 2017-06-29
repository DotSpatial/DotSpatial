// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MenuBarHeaderControl.cs" company="">
//
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;

namespace DotSpatial.Controls.Header
{
    /// <summary>
    /// Implementation of toolbar header.
    /// </summary>
    public class MenuBarHeaderControl : HeaderControl
    {
        #region Constants and Fields

        internal const string DEFAULT_GROUP_NAME = "Default Group";
        private ToolStripPanel _tsPanel;
        private MenuStrip _menuStrip;
        private List<ToolStrip> _strips;
        private Boolean _toolstripsLoaded; // indicates whether toolstrips were loaded after programstart
        private readonly List<ToolstripPosition> _stripPosList = new List<ToolstripPosition>(); // List to remember ToolstripPositions for saving on exit.

        #endregion

        #region Properties

        /// <summary>
        /// If set the userdefined toolstrip-order won't be saved/loaded;
        /// </summary>
        /// <remarks>Enables the user to ignore Toolstrippositionsaving (e.g. in DesignMode)</remarks>
        public bool IgnoreToolstripPositionSaving { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        public override void Add(MenuContainerItem item)
        {
            var menu = new ToolStripMenuItem(item.Caption) { Name = item.Key };

            var root = _menuStrip.Items[item.RootKey] as ToolStripDropDownButton;
            if (root != null)
            {
                root.DropDownItems.Add(menu);
                root.Visible = true;
            }
        }

        /// <summary>
        /// Adds the separator.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void Add(SeparatorItem item)
        {
            var separator = new ToolStripSeparator();
            var strip = GetOrCreateStrip(item.GroupCaption);

            if (strip != null)
            {
                strip.Items.Add(separator);
            }
        }

        /// <summary>
        /// Adds the specified root item.
        /// </summary>
        /// <param name="item">
        /// The root item.
        /// </param>
        /// <remarks>
        /// </remarks>
        public override void Add(RootItem item)
        {
            // The root may have already been created.
            var root = _menuStrip.Items[item.Key] as ToolStripDropDownButton;
            if (root == null)
            {
                // if not we need to create it.
                CreateToolStripDropDownButton(item);
            }
            else
            {
                // We have already created the RootItem in anticipation of it being needed. (As it was specified by some HeaderItem already)
                // Update the caption and sort order of the root.
                root.Text = item.Caption;
                root.MergeIndex = item.SortOrder;
            }

            RefreshRootItemOrder();
        }

        /// <summary>
        /// This will add a new item that will appear on the standard toolbar or ribbon control.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <remarks>
        /// </remarks>
        public override void Add(SimpleActionItem item)
        {
            ToolStripItem menu;

            if (IsForMenuStrip(item) || item.GroupCaption == ApplicationMenuKey)
            {
                menu = new ToolStripMenuItem(item.Caption) { Image = item.SmallImage };
            }
            else
            {
                menu = new ToolStripButton(item.Caption)
                {
                    DisplayStyle = ToolStripItemDisplayStyle.Image,
                    Image = item.SmallImage
                };

                // we're grouping all Toggle buttons together into the same group.
                if (item.ToggleGroupKey != null)
                {
                    var button = (ToolStripButton)menu;
                    button.CheckOnClick = true;
                    button.CheckedChanged += button_CheckedChanged;

                    item.Toggling += (sender, args) =>
                    {
                        UncheckButtonsExcept(button);
                        button.Checked = !button.Checked;
                    };
                }
            }

            menu.Name = item.Key;
            menu.Enabled = item.Enabled;
            menu.Visible = item.Visible;
            menu.Click += (sender, e) => item.OnClick(e);

            EnsureNonNullRoot(item);
            var root = _menuStrip.Items[item.RootKey] as ToolStripDropDownButton;
            if (root == null)
            {
                // Temporarily create the root.
                root = CreateToolStripDropDownButton(new RootItem(item.RootKey, "AddRootItemWithKey " + item.RootKey));
            }

            if (item.MenuContainerKey == null)
            {
                if (IsForMenuStrip(item) || item.GroupCaption == ApplicationMenuKey || item.GroupCaption == null)
                {
                    root.DropDownItems.Add(menu);
                    root.Visible = true;
                }
                else
                {
                    var strip = GetOrCreateStrip(item.GroupCaption);
                    if (strip != null)
                    {
                        strip.Items.Add(menu);
                    }
                    menu.ToolTipText = String.IsNullOrWhiteSpace(item.ToolTipText) == false ? item.ToolTipText : item.Caption;
                }
            }
            else
            {
                var subMenu = root.DropDownItems[item.MenuContainerKey] as ToolStripMenuItem;
                if (subMenu != null)
                {
                    subMenu.DropDownItems.Add(menu);
                }
            }

            item.PropertyChanged += SimpleActionItem_PropertyChanged;
        }

        /// <summary>
        /// Adds a combo box style item
        /// </summary>
        /// <param name="item">The item.</param>
        public override void Add(DropDownActionItem item)
        {
            var strip = GetOrCreateStrip(item.GroupCaption);
            var combo = new ToolStripComboBox(item.Key);

            ParseAllowEditingProperty(item, combo);

            combo.ToolTipText = item.Caption;
            if (item.Width != 0)
            {
                combo.Width = item.Width;
            }

            combo.Items.AddRange(item.Items.ToArray());
            combo.SelectedIndexChanged += delegate
                                              {
                                                  item.PropertyChanged -= DropDownActionItem_PropertyChanged;
                                                  item.SelectedItem = combo.SelectedItem;
                                                  item.PropertyChanged += DropDownActionItem_PropertyChanged;
                                              };

            if (strip != null)
            {
                strip.Items.Add(combo);
            }

            item.PropertyChanged += DropDownActionItem_PropertyChanged;
        }

        /// <summary>
        /// Adds the specified textbox item.
        /// </summary>
        /// <param name="item">The item.</param>
        public override void Add(TextEntryActionItem item)
        {
            var strip = GetOrCreateStrip(item.GroupCaption);
            var textBox = new ToolStripTextBox(item.Key);
            if (item.Width != 0)
            {
                textBox.Width = item.Width;
            }

            textBox.TextChanged += delegate
                                       {
                                           item.PropertyChanged -= TextEntryActionItem_PropertyChanged;
                                           item.Text = textBox.Text;
                                           item.PropertyChanged += TextEntryActionItem_PropertyChanged;
                                       };
            if (strip != null)
            {
                strip.Items.Add(textBox);
            }

            item.PropertyChanged += TextEntryActionItem_PropertyChanged;
        }

        /// <summary>
        /// Initializes the specified container.
        /// </summary>
        /// <param name="toolStripPanel">The tool strip panel.</param>
        /// <param name="menuStrip">Menu strip.</param>
        public void Initialize(ToolStripPanel toolStripPanel, MenuStrip menuStrip)
        {
            if (toolStripPanel == null) throw new ArgumentNullException("toolStripPanel");
            if (menuStrip == null) throw new ArgumentNullException("menuStrip");

            if (_tsPanel != null)
            {
                RemoveAll();
                _menuStrip.ItemClicked -= MenuStrip_ItemClicked;
                _tsPanel.Layout -= RememberLayout;
                Application.ApplicationExit -= SaveToolstripPositions;
                _stripPosList.Clear();
            }

            _tsPanel = toolStripPanel;
            _tsPanel.Layout += RememberLayout;
            LoadToolstripPositions();
            Application.ApplicationExit += SaveToolstripPositions;
            _menuStrip = menuStrip;
            _menuStrip.ItemClicked += MenuStrip_ItemClicked;
            _strips = new List<ToolStrip> { _menuStrip };
            if (_stripPosList.Count == 0) //set the default position of the menustrip for it to show on top
                _stripPosList.Add(new ToolstripPosition(_menuStrip.Name, 0, 0));
        }

        /// <summary>
        /// Loads the toolstrips in the order, that was saved on the last exit.
        /// </summary>
        public void LoadToolstrips()
        {
            _toolstripsLoaded = false;
            _tsPanel.Controls.Clear();
            foreach (var i in _stripPosList.OrderBy(tsp => tsp.Row).ThenByDescending(tsp => tsp.Column)) //add toolstrips after saved order
            {
                var ts = _strips.Find(_ => _.Name == i.Name);
                if (ts != null)
                {
                    _tsPanel.Join(ts, i.Row);
                }
            }
            _toolstripsLoaded = true; //all toolstrips in _StripPosList are added, any missing strips can now be added

            var index = Math.Max(1, _tsPanel.Rows.Length - 1); //add toolstrips that were added after saving (e.g. Plugins dropped into Plugin-folder)
            foreach (var strip in Enumerable.Reverse(_strips)) //Reverse order because join adds at the beginning of the ToolStripPanelRow
            {
                if (!_stripPosList.Exists(_ => _.Name == strip.Name))
                {
                    _tsPanel.Join(strip, index);
                }
            }
        }
        
        /// <summary>
        /// Remove item from the standard toolbar or ribbon control
        /// </summary>
        /// <param name="key">
        /// The string itemName to remove from the standard toolbar or ribbon control
        /// </param>
        /// <remarks>
        /// </remarks>
        public override void Remove(string key)
        {
            var item = GetItem(key);
            if (item != null)
            {
                var toolStrip = item.Owner;
                item.Dispose();
                if (toolStrip.Items.Count == 0)
                {
                    _strips.Remove(toolStrip);
                    toolStrip.Dispose();
                }

                // Unsubscribe from events
                var headerItem = GetHeaderItemByKey(key);
                if (headerItem != null)
                {
                    headerItem.PropertyChanged -= SimpleActionItem_PropertyChanged;
                    headerItem.PropertyChanged -= DropDownActionItem_PropertyChanged;
                    headerItem.PropertyChanged -= TextEntryActionItem_PropertyChanged;
                }
            }
            base.Remove(key);
        }

        /// <summary>
        /// Selects the root. (Does nothing.)
        /// </summary>
        /// <param name="key">The key.</param>
        public override void SelectRoot(string key)
        {
            // we won't do anything here.
        }

        #endregion

        #region Methods

        /// <summary>
        /// Determines whether [is for tool strip]  being that it has an icon. Otherwise it should go on a menu.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// <c>true</c> if [is for tool strip] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsForMenuStrip(SimpleActionItem item)
        {
            return item.SmallImage == null;
        }

        private static void ParseAllowEditingProperty(DropDownActionItem item, ToolStripComboBox combo)
        {
            combo.DropDownStyle = item.AllowEditingText ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
        }

        private void RefreshRootItemOrder()
        {
            // Get a list of all the menus
            var pages = new List<ToolStripItem>(_menuStrip.Items.Cast<ToolStripItem>().OrderBy(_ => _.MergeIndex));

            // Re add all of the items in the new order.
            _menuStrip.Items.Clear();
            foreach (var sortedPage in pages)
            {
                _menuStrip.Items.Add(sortedPage);
            }
        }

        private void MenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            OnRootItemSelected(e.ClickedItem.Name);
        }

        private void ActionItem_PropertyChanged(ActionItem item, PropertyChangedEventArgs e)
        {
            var guiItem = GetItem(item.Key);

            switch (e.PropertyName)
            {
                case "Caption":
                    guiItem.Text = item.Caption;
                    break;

                case "Enabled":
                    guiItem.Enabled = item.Enabled;
                    break;

                case "Visible":
                    guiItem.Visible = item.Visible;
                    break;

                case "ToolTipText":
                    guiItem.ToolTipText = item.ToolTipText;
                    break;

                case "GroupCaption":
                    break;

                case "RootKey":
                    // note, this case will also be selected in the case that we set the Root key in our code.
                    break;

                default:
                    throw new NotSupportedException("This Header Control implementation doesn't have an implemenation for or has banned modifying that property.");
            }
        }

        /// <summary>
        /// Loads the saved toolstrippositions from file.
        /// </summary>
        private void LoadToolstripPositions()
        {
            if (IgnoreToolstripPositionSaving) return;
            var path = GetMenuBarHeaderControlConfigPath();
            if (!File.Exists(path)) return;
            using (var stream = File.Open(path, FileMode.Open))
            {
                var bin = new BinaryFormatter();
                _stripPosList.Clear();
                _stripPosList.AddRange((List<ToolstripPosition>)bin.Deserialize(stream));
            }
        }

        /// <summary>
        /// When the user moves toolstrips, the new position gets remembered for saving.
        /// </summary>
        private void RememberLayout(object sender, EventArgs e)
        {
            if (IgnoreToolstripPositionSaving) return;
            // Because we don't know what was moved where we correct all positions. 
            // This is done in reverse, because we don't want to repeat the first rows while the last row throws an exception.
            for (var i = _tsPanel.Rows.Length - 1; i >= 0; i--)
            {
                try
                {
                    var c = _tsPanel.Rows[i].Controls;
                    for (var k = 0; k < c.Length; k++)
                    {
                        var ts = (ToolStrip)c[k];
                        var erg = _stripPosList.Find(_ => _.Name == ts.Name);
                        if (erg != null)
                        {
                            erg.Row = i;
                            erg.Column = k;
                        }
                        else
                        {
                            _stripPosList.Add(new ToolstripPosition(ts.Name, i, k));
                        }
                    }
                }
                catch (ArgumentException) // The Controls of newly added rows throw this error. Then we ignore this RememberLayout-event because it gets raised again with correct Controls.
                { }
            }
        }

        /// <summary>
        /// Gets the path to the MenuBarHeaderControl.config-file in which the toolstrip-order is saved.
        /// </summary>
        private static string GetMenuBarHeaderControlConfigPath()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.ProductName);
            path = Path.Combine(path, "MenuBarHeaderControl.config");
            return path;
        }
        
        /// <summary>
        /// Saves the toolstrips positions so that the user doesn't have to reorder them on next start.
        /// </summary>
        private void SaveToolstripPositions(object sender, EventArgs e)
        {
            if (IgnoreToolstripPositionSaving) return;
            var path = GetMenuBarHeaderControlConfigPath();
            var dir = Path.GetDirectoryName(path);
            Debug.Assert(dir != null);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (var stream = File.Open(path, FileMode.Create))
            {
                var bin = new BinaryFormatter();
                bin.Serialize(stream, _stripPosList);
            }
        }

        /// <summary>
        /// Adds a toolstrip with the given name to the _strips-List. If the toolbars were already loaded it is added to the _tsPanel as well.
        /// </summary>
        /// <param name="groupName">Name of the toolstrip that will be added.</param>
        /// <returns>ToolStrip that was created.</returns>
        private ToolStrip AddToolStrip(string groupName)
        {
            var strip = new ToolStrip { Name = groupName };
            _strips.Add(strip);
            if (_toolstripsLoaded)
            {
                if (!_stripPosList.Exists(_ => _.Name == groupName))
                { _tsPanel.Join(strip); }
                else { LoadToolstrips(); } //if toolstrip exists, reload all for it to appear at the remembered position
            }
            return strip;
        }

        private ToolStripDropDownButton CreateToolStripDropDownButton(RootItem item)
        {
            var menu = new ToolStripDropDownButton(item.Caption)
            {
                Name = item.Key,
                ShowDropDownArrow = false,
                Visible = false,
                MergeIndex = item.SortOrder
            };
            _menuStrip.Items.Add(menu);
            return menu;
        }

        private void DropDownActionItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = (DropDownActionItem)sender;
            var guiItem = (ToolStripComboBox)GetItem(item.Key);

            switch (e.PropertyName)
            {
                case "AllowEditingText":
                    ParseAllowEditingProperty(item, guiItem);
                    break;

                case "Width":
                    guiItem.Width = item.Width;
                    break;

                case "SelectedItem":
                    guiItem.SelectedItem = item.SelectedItem;
                    break;

                case "FontColor":
                    guiItem.ForeColor = item.FontColor;
                    break;

                case "ToggleGroupKey":
                    break;

                case "MultiSelect":
                    break;

                default:
                    ActionItem_PropertyChanged(item, e);
                    break;
            }
        }

        /// <summary>
        /// Ensure the extensions tab exists.
        /// </summary>
        private void EnsureExtensionsTabExists()
        {
            var exists = _menuStrip.Items.ContainsKey(ExtensionsRootKey);
            if (!exists)
            {
                Add(new RootItem(ExtensionsRootKey, "Extensions"));
            }
        }

        /// <summary>
        /// Make sure the root key is present or use a default.
        /// </summary>
        /// <param name="item">
        /// </param>
        private void EnsureNonNullRoot(ActionItem item)
        {
            if (item.RootKey == null)
            {
                EnsureExtensionsTabExists();
                item.RootKey = ExtensionsRootKey;
            }
        }

        private ToolStripItem GetItem(string key)
        {
            foreach (var strip in _strips)
            {
                var item = strip.Items.Find(key, true).FirstOrDefault();
                if (item != null)
                {
                    return item;
                }
            }

            return null;
        }

        private ToolStrip GetOrCreateStrip(string groupCaption)
        {
            var query = from s in _strips
                        where s.Name == (groupCaption ?? DEFAULT_GROUP_NAME)
                        select s;

            return query.FirstOrDefault() ?? AddToolStrip(groupCaption);
        }

        private void SimpleActionItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = (SimpleActionItem)sender;
            var guiItem = GetItem(item.Key);

            switch (e.PropertyName)
            {
                case "SmallImage":
                    guiItem.Image = item.SmallImage;
                    break;

                case "LargeImage":
                    guiItem.Image = item.LargeImage;
                    break;

                case "MenuContainerKey":
                    break;
                case "ToggleGroupKey":
                    break;

                default:
                    ActionItem_PropertyChanged(item, e);
                    break;
            }
        }

        private void TextEntryActionItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = (TextEntryActionItem)sender;
            var guiItem = (ToolStripTextBox)GetItem(item.Key);

            switch (e.PropertyName)
            {
                case "Width":
                    guiItem.Width = item.Width;
                    break;

                case "Text":
                    guiItem.Text = item.Text;
                    break;

                case "FontColor":
                    guiItem.TextBox.ForeColor = item.FontColor;
                    break;

                default:
                    ActionItem_PropertyChanged(item, e);
                    break;
            }
        }

        /// <summary>
        /// Unchecks all toolstrip buttons except the current button
        /// </summary>
        /// <param name="checkedButton">
        /// The toolstrip button which should
        /// stay checked
        /// </param>
        private void UncheckButtonsExcept(ToolStripButton checkedButton)
        {
            foreach (var strip in _strips)
            {
                foreach (ToolStripItem item in strip.Items)
                {
                    var buttonItem = item as ToolStripButton;
                    if (buttonItem != null)
                    {
                        if (buttonItem.Name != checkedButton.Name && buttonItem.Checked)
                        {
                            buttonItem.Checked = false;
                        }
                    }
                }
            }
        }

        private void button_CheckedChanged(object sender, EventArgs e)
        {
            var button = (ToolStripButton)sender;
            if (button.Checked)
            {
                UncheckButtonsExcept(button);
            }
        }

        #endregion
    }

    /// <summary>
    /// Helper-Class to remember the Toolstrip-Positions to save to file on exit.
    /// </summary>
    [Serializable]
    internal class ToolstripPosition
    {
        public string Name { get; private set; }
        public int Row { get; set; }
        public int Column { get; set; }

        public ToolstripPosition(String name, int row, int column)
        {
            Name = name;
            Row = row;
            Column = column;
        }
    }
}
