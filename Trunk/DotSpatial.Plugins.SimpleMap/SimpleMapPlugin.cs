using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;

namespace DotSpatial.Plugins.SimpleMap
{
    public class SimpleMapPlugin : Extension
    {
        public override void Activate()
        {
            ShowMap();
            base.Activate();
        }

        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            App.DockManager.Remove("kMap");
            base.Deactivate();
        }

        private void ShowMap()
        {
            var map = new Map {Text = "Map", Name = "map1", Legend = App.Legend};
            App.Map = map;
            App.DockManager.Add(new DockablePanel("kMap", "Map", map, DockStyle.Fill));
        }
    }
}
