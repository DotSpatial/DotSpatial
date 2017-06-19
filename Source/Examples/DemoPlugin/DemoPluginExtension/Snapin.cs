using System;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;
using DotSpatial.Controls.Header;

namespace DemoPlugin.DemoPluginExtension
{
    public class Snapin : Extension
    {
        private const string UniqueKeyPluginStoredValueDate = "UniqueKey-PluginStoredValueDate";
        private const string AboutPanelKey = "kAboutPanel";
        private  DateTime _storedValue;

        public override void Activate()
        {
            // add some menu items...
            AddMenuItems(App.HeaderControl);

            // code for saving plugin settings...
            App.SerializationManager.Serializing += ManagerSerializing;
            App.SerializationManager.Deserializing += ManagerDeserializing;

            AddDockingPane();

            base.Activate();
        }

        public override void Deactivate()
        {
            // Do not forget to unsubscribe event handlers
            App.SerializationManager.Serializing -= ManagerSerializing;
            App.SerializationManager.Deserializing -= ManagerDeserializing;

            // Remove all GUI components which were added by plugin
            App.DockManager.Remove(AboutPanelKey);
            App.HeaderControl.RemoveAll();

            base.Deactivate();
        }

        private void AddMenuItems(IHeaderControl header)
        {
            const string SampleMenuKey = "kSample";

            // Root menu
            header.Add(new RootItem(SampleMenuKey, "MyPlugin"));

            // Add some child menus
            header.Add(new SimpleActionItem(SampleMenuKey, "Alpha", null) { Enabled = false });
            header.Add(new SimpleActionItem(SampleMenuKey, "Bravo", OnMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "Charlie", OnMenuClickEventHandler));

            // Add sub menus
            header.Add(new MenuContainerItem(SampleMenuKey, "submenu", "B"));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenu", "1", OnMenuClickEventHandler));
            header.Add(new SimpleActionItem(SampleMenuKey, "submenu", "2", OnMenuClickEventHandler));
        }

        private void OnMenuClickEventHandler(object sender, EventArgs e)
        {
            MessageBox.Show("Clicked " + ((SimpleActionItem) sender).Caption);
        }

        private void AddDockingPane()
        {
            var form = new AboutBox();
            form.okButton.Click += (o, args) => App.DockManager.HidePanel(AboutPanelKey);

            var aboutPanel = new DockablePanel(AboutPanelKey, "About", form.tableLayoutPanel, DockStyle.Right);
            App.DockManager.Add(aboutPanel);
        }

        private void ManagerDeserializing(object sender, SerializingEventArgs e)
        {
            var manager = (SerializationManager)sender;
            _storedValue = manager.GetCustomSetting(UniqueKeyPluginStoredValueDate, DateTime.Now);
        }

        private void ManagerSerializing(object sender, SerializingEventArgs e)
        {
            var manager = (SerializationManager)sender;
            manager.SetCustomSetting(UniqueKeyPluginStoredValueDate, _storedValue);
        }
    }
}