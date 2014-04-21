using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;

namespace DotSpatial.Plugins.SimpleMap
{
    public class SimpleMapPlugin : Extension
    {
        private Map _map;

        public SimpleMapPlugin()
        {
            DeactivationAllowed = false;
        }

        public override int Priority
        {
            get { return -10000; }
        }

        public override void Activate()
        {
            ShowMap();
            base.Activate();
        }

        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            App.DockManager.Remove("kMap");
            if (App.Map == _map) App.Map = null;
            base.Deactivate();
        }

        private void ShowMap()
        {
            _map = new Map {Text = "Map", Name = "map1", Legend = App.Legend};
            App.Map = _map;
            App.DockManager.Add(new DockablePanel("kMap", "Map", _map, DockStyle.Fill));
        }
    }
}
