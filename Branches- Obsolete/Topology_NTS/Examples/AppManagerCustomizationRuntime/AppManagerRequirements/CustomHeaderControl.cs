using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Header;

namespace DotSpatial.Examples.AppManagerCustomizationRuntime.AppManagerRequirements
{
    /// <summary>
    /// Sample implmenentation of IHeaderControl.
    /// It shows a technique how to create own header control as extension.
    /// </summary>
    [Export(typeof(IHeaderControl))]
    internal class CustomHeaderControl : HeaderControl, IPartImportsSatisfiedNotification
    {
        private MainMenu mainmenu;
        private FlowLayoutPanel container;

        [Import("Shell", typeof(ContainerControl))]
        private ContainerControl Shell { get; set; }

        /// <summary>
        /// Called when a part's imports have been satisfied and it is safe to use. (Shell will have a value)
        /// </summary>
        public void OnImportsSatisfied()
        {
            mainmenu = new MainMenu();

            var form = (Form)Shell;
            form.Menu = mainmenu;

            container = new FlowLayoutPanel
            {
                Name = "Default Group",
                Height = 0,
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink
            };
            Shell.Controls.Add(container);
        }

        public override void SelectRoot(string key)
        {
            // we won't do anything here.
        }

        public override void Add(SimpleActionItem item)
        {
            var menu = new IconMenuItem(item.Caption, item.SmallImage, (sender, e) => item.OnClick(e))
            {Name = item.Key, Enabled = item.Enabled, Visible = item.Visible,};
            item.PropertyChanged += SimpleActionItem_PropertyChanged;

            EnsureNonNullRoot(item);

            MenuItem root;
            if (item.MenuContainerKey == null)
            {
                root = !mainmenu.MenuItems.ContainsKey(item.RootKey)
                    ? mainmenu.MenuItems[mainmenu.MenuItems.Add(new MenuItem(item.RootKey) {Name = item.RootKey})]
                    : mainmenu.MenuItems.Find(item.RootKey, true)[0];
            }
            else
            {
                root = mainmenu.MenuItems.Find(item.MenuContainerKey, true)[0];
            }

            root.MenuItems.Add(menu);
        }

        /// <summary>
        /// Ensure the extensions tab exists.
        /// </summary>
        private void EnsureExtensionsTabExists()
        {
            var exists = mainmenu.MenuItems.ContainsKey(ExtensionsRootKey);
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

        public override void Add(MenuContainerItem item)
        {
            var submenu = new MenuItem
            {
                Name = item.Key,
                Visible = item.Visible,
                Text = item.Caption,
            };

            item.PropertyChanged += RootItem_PropertyChanged;
            var root = mainmenu.MenuItems.Find(item.RootKey, true)[0];
            root.MenuItems.Add(submenu);
        }

        public override void Add(RootItem item)
        {
            if (!mainmenu.MenuItems.ContainsKey(item.Key))
            {
                var submenu = new MenuItem
                {
                    Name = item.Key,
                    Visible = item.Visible,
                    Text = item.Caption,
                    MergeOrder = item.SortOrder
                };
                item.PropertyChanged += RootItem_PropertyChanged;
                mainmenu.MenuItems.Add(submenu);
            }
            else
            {
                var root = mainmenu.MenuItems.Find(item.Key, true)[0];
                root.Text = item.Caption;
                root.MergeOrder = item.SortOrder;
            }
        }

        public override void Add(DropDownActionItem item)
        {
            var combo = new ComboBox {Name = item.Key};

            ParseAllowEditingProperty(item, combo);

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
            item.PropertyChanged += DropDownActionItem_PropertyChanged;
            container.Controls.Add(combo);
        }
       
        public override void Add(SeparatorItem item)
        {
            
        }

        public override void Add(TextEntryActionItem item)
        {
            var textBox = new TextBox {Name = item.Key};
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
            container.Controls.Add(textBox);
            item.PropertyChanged += TextEntryActionItem_PropertyChanged;
        }

        private Control GetItem(string key)
        {
            var item = container.Controls.Find(key, true).FirstOrDefault();
            return item;
        }

        private MenuItem GetMenuItem(string key)
        {
            var item = mainmenu.MenuItems.Find(key, true).FirstOrDefault();
            return item;
        }

        private void DropDownActionItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = (DropDownActionItem)sender;
            var guiItem = (ComboBox)GetItem(item.Key);

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

        private void TextEntryActionItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = (TextEntryActionItem)sender;
            var guiItem = (TextBox)GetItem(item.Key);

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
                    ActionItem_PropertyChanged(item, e);
                    break;
            }
        }

        private void ActionItem_PropertyChanged(ActionItem item, PropertyChangedEventArgs e)
        {
            if (item.GetType() == typeof(SimpleActionItem) || item.GetType() == typeof(RootItem))
            {
                var guiItem = GetMenuItem(item.Key);

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

        private void RootItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = (RootItem)sender;
            var guiItem = GetItem(item.Key);

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

        private void SimpleActionItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = (SimpleActionItem)sender;
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
                    ActionItem_PropertyChanged(item, e);
                    break;
            }
        }

        private void ParseAllowEditingProperty(DropDownActionItem item, ComboBox guiItem)
        {
            guiItem.DropDownStyle = item.AllowEditingText ? ComboBoxStyle.DropDown : ComboBoxStyle.DropDownList;
        }
    }
}
