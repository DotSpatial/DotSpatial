using System;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;
using DotSpatial.Controls.Header;

namespace DemoPlugin
{
    public class Snapin : Extension
    {
        private const string UniqueKeyPluginStoredValueDate = "UniqueKey-PluginStoredValueDate";
        private const string AboutPanelKey = "kAboutPanel";
        DateTime _storedValue;

        public override void Deactivate()
        {
            App.DockManager.Remove(AboutPanelKey);
            if (App.HeaderControl != null) { App.HeaderControl.RemoveAll(); }
            base.Deactivate();
        }

        public override void Activate()
        {
            AddMenuItems(App.HeaderControl);

            //code for saving plugin settings...
            App.SerializationManager.Serializing += manager_Serializing;
            App.SerializationManager.Deserializing += manager_Deserializing;

            AddDockingPane();

            base.Activate();
        }

        private void AddDockingPane()
        {
            var form = new AboutBox();
            DockablePanel aboutPanel = new DockablePanel(AboutPanelKey, "About", form.tableLayoutPanel, DockStyle.Right);
            App.DockManager.Add(aboutPanel);
            form.okButton.Click += okButton_Click;
            App.DockManager.ActivePanelChanged += DockManager_ActivePanelChanged;
        }

        private void DockManager_ActivePanelChanged(object sender, DockablePanelEventArgs e)
        {
            App.HeaderControl.SelectRoot("kHome");
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            App.DockManager.Remove(AboutPanelKey);
        }

        private void AddMenuItems(IHeaderControl header)
        {
            // add sample menu items...
            if (header == null) return;

            const string SampleMenuKey = "kSample";

            header.Add(new RootItem(SampleMenuKey, "MyPlugin"));
            SimpleActionItem alphaItem = new SimpleActionItem(SampleMenuKey, "Alpha", null) { Key = "kA" };
            header.Add(alphaItem);
            header.Add(new SimpleActionItem(SampleMenuKey, "Bravo", null));
            header.Add(new SimpleActionItem(SampleMenuKey, "Charlie", null));
            header.Add(new MenuContainerItem(SampleMenuKey, "submenu", "B"));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenu", "1", null));
            SimpleActionItem item = new SimpleActionItem(SampleMenuKey, "submenu", "2", null);
            header.Add(item);
            header.Add(new SimpleActionItem(SampleMenuKey, "submenu", "3", null));

            alphaItem.Enabled = false;
            header.Remove(item.Key);
        }

        private void manager_Deserializing(object sender, SerializingEventArgs e)
        {
            var manager = (SerializationManager)sender;
            _storedValue = manager.GetCustomSetting(UniqueKeyPluginStoredValueDate, DateTime.Now);
        }

        private void manager_Serializing(object sender, SerializingEventArgs e)
        {
            var manager = (SerializationManager)sender;
            manager.SetCustomSetting(UniqueKeyPluginStoredValueDate, _storedValue);
        }
    }
}