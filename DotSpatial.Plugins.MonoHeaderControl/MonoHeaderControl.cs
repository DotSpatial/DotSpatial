using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using DotSpatial.Controls.Header;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace DemoMap
{
    [Export(typeof(IHeaderControl))]
    class MonoHeaderControl : HeaderControl, IPartImportsSatisfiedNotification
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

            Form form = Shell as Form;
            form.Menu = mainmenu;

            container = new FlowLayoutPanel();
            container.Name = "Default Group";
            container.Dock = DockStyle.Top;
            Shell.Controls.Add(container);
        }

        public override void SelectRoot(string key)
        {
            // we won't do anything here.
        }

        public override void Add(SimpleActionItem item)
        {
            MenuItem menu = new MenuItem(item.Caption);

            menu.Name = item.Key;
            menu.Enabled = item.Enabled;
            menu.Visible = item.Visible;
            menu.Click += (sender, e) => item.OnClick(e);
            item.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(SimpleActionItem_PropertyChanged);

            MenuItem root = null;

            if (!mainmenu.MenuItems.ContainsKey(item.RootKey))
            {
                root = new MenuItem(item.RootKey);
            }
            else
            {
                root = mainmenu.MenuItems.Find(item.RootKey, true).ElementAt(0);
            }

            try
            {
                root.MenuItems.Add(menu);
            }
            catch (Exception e)
            {
                Debug.Print(e.StackTrace);
            }

        }

        public override void Add(MenuContainerItem item)
        {
            throw new NotImplementedException();
        }

        public override void Add(RootItem item)
        {
            MenuItem submenu = null;
            if (!this.mainmenu.MenuItems.ContainsKey(item.Key))
            {
                submenu = new MenuItem();
                submenu.Name = item.Key;
                submenu.Visible = item.Visible;
                submenu.Text = item.Caption;
                submenu.MergeOrder = item.SortOrder;
                item.PropertyChanged += new PropertyChangedEventHandler(RootItem_PropertyChanged);
                mainmenu.MenuItems.Add(submenu);
            }
        }

        public override void Add(DropDownActionItem item)
        {
            ComboBox combo = new ComboBox();
            combo.Name = item.Key;

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

            //addLabel(item.ToolTipText);

            container.Controls.Add(combo);
        }

        private void addLabel(String text)
        {
            Label label = new Label();
            label.Text = text;
            container.Controls.Add(label);
        }

        public override void Add(SeparatorItem item)
        {
            //throw new NotImplementedException();
        }

        public override void Add(TextEntryActionItem item)
        {
            TextBox textBox = new TextBox();
            textBox.Name = item.Key;
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
            //addLabel(item.ToolTipText);
            container.Controls.Add(textBox);
            item.PropertyChanged += TextEntryActionItem_PropertyChanged;
        }

        private Control GetItem(string key)
        {
            Control item = container.Controls.Find(key, true).FirstOrDefault();
            return item;
        }

        private MenuItem GetMenuItem(string key)
        {
            MenuItem item = mainmenu.MenuItems.Find(key, true).FirstOrDefault();
            return item;
        }

        private void DropDownActionItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = sender as DropDownActionItem;
            var guiItem = this.GetItem(item.Key) as ComboBox;

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
            var item = sender as TextEntryActionItem;
            var guiItem = this.GetItem(item.Key) as TextBox;

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
            if (item.GetType().Equals(typeof(SimpleActionItem)) || item.GetType().Equals(typeof(RootItem)))
            {
                MenuItem guiItem = GetMenuItem(item.Key);

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
                        //guiItem.ToolTipText = item.ToolTipText;
                        break;

                    case "GroupCaption":
                        // todo: change group
                        break;

                    case "RootKey":
                        // todo: change root
                        // note, this case will also be selected in the case that we set the Root key in our code.
                        break;

                    case "Key":
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
                        //guiItem.ToolTipText = item.ToolTipText;
                        break;

                    case "GroupCaption":
                        // todo: change group
                        break;

                    case "RootKey":
                        // todo: change root
                        // note, this case will also be selected in the case that we set the Root key in our code.
                        break;

                    case "Key":
                    default:
                        throw new NotSupportedException(" This Header Control implementation doesn't have an implemenation for or has banned modifying that property.");
                }
            }
        }

        private void RootItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = sender as RootItem;
            var guiItem = this.GetItem(item.Key);

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
                default:
                    break;
            }
        }

        void SimpleActionItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = sender as SimpleActionItem;
            var guiItem = this.GetItem(item.Key);

            switch (e.PropertyName)
            {
                case "SmallImage":
                    break;

                case "LargeImage":
                    break;

                case "MenuContainerKey":
                    Trace.WriteLine("MenuContainerKey must not be changed after item is added to header.");
                    break;

                case "ToggleGroupKey":
                    Trace.WriteLine("ToggleGroupKey must not be changed after item is added to header.");
                    break;

                default:
                    ActionItem_PropertyChanged(item, e);
                    break;
            }
        }


        private void ParseAllowEditingProperty(DropDownActionItem item, ComboBox guiItem)
        {
            if (item.AllowEditingText)
            {
                guiItem.DropDownStyle = ComboBoxStyle.DropDown;
            }
            else
            {
                guiItem.DropDownStyle = ComboBoxStyle.DropDownList;
            }
        }
    }
}
