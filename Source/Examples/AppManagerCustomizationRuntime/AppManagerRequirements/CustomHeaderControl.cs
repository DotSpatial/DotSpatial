// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls.Header;

namespace DotSpatial.Examples.AppManagerCustomizationRuntime.AppManagerRequirements
{
    /// <summary>
    /// Sample implmenentation of IHeaderControl.
    /// It shows how you can create your own header control as an extension.
    /// </summary>
    [Export(typeof(IHeaderControl))]
    internal class CustomHeaderControl : HeaderControl, IPartImportsSatisfiedNotification
    {
        private MenuStrip _menustrip;
        private FlowLayoutPanel _container;

        [Import("Shell", typeof(ContainerControl))]
        private ContainerControl Shell { get; set; }

        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use. (Shell will have a value)
        /// </summary>
        public void OnImportsSatisfied()
        {
            _menustrip = new MenuStrip();

            Form form = (Form)Shell;
            form.MainMenuStrip = _menustrip;

            _container = new FlowLayoutPanel
            {
                Name = "Default Group",
                Height = 0,
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            Shell.Controls.Add(_container);
            Shell.Controls.Add(_menustrip);
        }

        public override void SelectRoot(string key)
        {
            // we won't do anything here.
        }

        public override object Add(SimpleActionItem item)
        {
            ToolStripMenuItem menu = new(item.Caption, item.SmallImage, (sender, e) => item.OnClick(e), item.Key)
            {
                Enabled = item.Enabled,
                Visible = item.Visible
            };

            item.PropertyChanged += SimpleActionItemPropertyChanged;

            EnsureNonNullRoot(item);

            ToolStripItem root;

            if (item.MenuContainerKey == null)
            {
                if (!_menustrip.Items.ContainsKey(item.RootKey))
                {
                    root = _menustrip.Items[_menustrip.Items.Add(new ToolStripMenuItem(item.RootKey) { Name = item.RootKey })];
                }
                else
                {
                    root = _menustrip.Items.Find(item.RootKey, true)[0];
                }
            }
            else
            {
                root = _menustrip.Items.Find(item.MenuContainerKey, true)[0];
            }

            (root as ToolStripMenuItem)?.DropDownItems.Add(menu);
            return menu;
        }

        /// <summary>
        /// Ensure the extensions tab exists.
        /// </summary>
        private void EnsureExtensionsTabExists()
        {
            if (!_menustrip.Items.ContainsKey(ExtensionsRootKey))
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

        public override object Add(MenuContainerItem item)
        {
            ToolStripMenuItem submenu = new()
            {
                Name = item.Key,
                Visible = item.Visible,
                Text = item.Caption
            };

            item.PropertyChanged += RootItemPropertyChanged;
            ToolStripItem root = _menustrip.Items.Find(item.RootKey, true)[0];
            (root as ToolStripMenuItem)?.DropDownItems.Add(submenu);
            return submenu;
        }

        public override object Add(RootItem item)
        {
            if (!_menustrip.Items.ContainsKey(item.Key))
            {
                ToolStripMenuItem submenu = new()
                {
                    Name = item.Key,
                    Visible = item.Visible,
                    Text = item.Caption,
                    MergeIndex = item.SortOrder
                };

                item.PropertyChanged += RootItemPropertyChanged;
                _menustrip.Items.Add(submenu);
                return submenu;
            }

            ToolStripItem root = _menustrip.Items.Find(item.Key, true)[0];
            root.Text = item.Caption;
            root.MergeIndex = item.SortOrder;
            return root;
        }

        public override object Add(DropDownActionItem item)
        {
            ComboBox combo = new() { Name = item.Key };

            ParseAllowEditingProperty(item, combo);

            if (item.Width != 0)
            {
                combo.Width = item.Width;
            }

            combo.Items.AddRange(item.Items.ToArray());
            combo.SelectedIndexChanged += delegate
            {
                item.PropertyChanged -= DropDownActionItemPropertyChanged;
                item.SelectedItem = combo.SelectedItem;
                item.PropertyChanged += DropDownActionItemPropertyChanged;
            };
            item.PropertyChanged += DropDownActionItemPropertyChanged;
            _container.Controls.Add(combo);
            return combo;
        }

        public override object Add(SeparatorItem item)
        {
            return null;
        }

        public override object Add(TextEntryActionItem item)
        {
            TextBox textBox = new() { Name = item.Key };

            if (item.Width != 0)
            {
                textBox.Width = item.Width;
            }

            textBox.TextChanged += delegate
            {
                item.PropertyChanged -= TextEntryActionItemPropertyChanged;
                item.Text = textBox.Text;
                item.PropertyChanged += TextEntryActionItemPropertyChanged;
            };

            _container.Controls.Add(textBox);
            item.PropertyChanged += TextEntryActionItemPropertyChanged;
            return textBox;
        }

        private Control GetItem(string key)
        {
            Control item = _container.Controls.Find(key, true).FirstOrDefault();
            return item;
        }

        private ToolStripItem GetMenuItem(string key)
        {
            ToolStripItem item = _menustrip.Items.Find(key, true).FirstOrDefault();
            return item;
        }

        private void DropDownActionItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            DropDownActionItem item = (DropDownActionItem)sender;
            ComboBox guiItem = (ComboBox)GetItem(item.Key);

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
                    ActionItemPropertyChanged(item, e);
                    break;
            }
        }

        private void TextEntryActionItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TextEntryActionItem item = (TextEntryActionItem)sender;
            TextBox guiItem = (TextBox)GetItem(item.Key);

            switch (e.PropertyName)
            {
                case "Width":
                    guiItem.Width = item.Width;
                    break;

                case "Text":
                    guiItem.Text = item.Text;
                    break;

                case "FontColor":
                    guiItem.ForeColor = item.FontColor;
                    break;

                default:
                    ActionItemPropertyChanged(item, e);
                    break;
            }
        }

        private void ActionItemPropertyChanged(ActionItem item, PropertyChangedEventArgs e)
        {
            if (item.GetType() == typeof(SimpleActionItem) || item.GetType() == typeof(RootItem))
            {
                ToolStripItem guiItem = GetMenuItem(item.Key);

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
                        break;

                    case "GroupCaption":
                        break;

                    case "RootKey":
                        // note, this case will also be selected in the case that we set the Root key in our code.
                        break;

                    default:
                        throw new NotSupportedException(" This Header Control implementation doesn't have an implemenation for or has banned modifying that property.");
                }
            }
            else
            {
                Control guiItem = GetItem(item.Key);
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
                        break;

                    case "GroupCaption":
                        break;

                    case "RootKey":
                        // note, this case will also be selected in the case that we set the Root key in our code.
                        break;

                    default:
                        throw new NotSupportedException(" This Header Control implementation doesn't have an implemenation for or has banned modifying that property.");
                }
            }
        }

        private void RootItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RootItem item = (RootItem)sender;
            Control guiItem = GetItem(item.Key);

            switch (e.PropertyName)
            {
                case "Caption":
                    guiItem.Text = item.Caption;
                    break;

                case "Visible":
                    guiItem.Visible = item.Visible;
                    break;

                case "SortOrder":
                    break;
            }
        }

        private void SimpleActionItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SimpleActionItem item = (SimpleActionItem)sender;
            switch (e.PropertyName)
            {
                case "SmallImage":
                    break;

                case "LargeImage":
                    break;

                case "MenuContainerKey":
                    break;

                case "ToggleGroupKey":
                    break;
                default:
                    ActionItemPropertyChanged(item, e);
                    break;
            }
        }

        private static void ParseAllowEditingProperty(DropDownActionItem item, ComboBox guiItem)
        {
            guiItem.DropDownStyle = item.AllowEditingText ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
        }
    }
}
