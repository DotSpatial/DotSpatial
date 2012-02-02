using System;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;
using DotSpatial.Controls.Header;
using DotSpatial.Data.Properties;

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
            //to test slow loading
            //System.Threading.Thread.Sleep(20000);

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
            var manager = sender as SerializationManager;

            _storedValue = manager.GetCustomSetting(UniqueKeyPluginStoredValueDate, DateTime.Now);
        }

        private void manager_Serializing(object sender, SerializingEventArgs e)
        {
            var manager = sender as SerializationManager;

            manager.SetCustomSetting(UniqueKeyPluginStoredValueDate, _storedValue);
        }

        private void myButton_Click(object sender, EventArgs e)
        {
            foreach (var recentFile in Settings.Default.RecentFiles)
            {
                MessageBox.Show("Recent File: " + recentFile);
            }

            IMapRasterLayer[] layers = App.Map.GetRasterLayers();
            if (layers.Length == 0)
            {
                MessageBox.Show("Please add a raster layer.");
                return;
            }
            IMapRasterLayer layer = layers[0];
            layer.Symbolizer.ShadedRelief.ElevationFactor = 1;
            layer.Symbolizer.ShadedRelief.IsUsed = true;
            layer.Symbolizer.CreateHillShade();

            layer.WriteBitmap();

            //Raster myRaster = new Raster();
            //myRaster.Create(
        }
    }
}