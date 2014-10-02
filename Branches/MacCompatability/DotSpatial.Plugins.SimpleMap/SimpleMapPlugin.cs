using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.MonoMac;
using DotSpatial.Controls.Docking;

namespace DotSpatial.Plugins.SimpleMap
{
    public class SimpleMapPlugin : Extension
    {
        private IMap _map;

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
            if (!Mono.Mono.IsRunningOnMonoMac ())
                _map = new DotSpatial.Controls.Map { Text = "Map", Name = "map1", Legend = App.Legend };
            else
                SetMap ();
            App.Map = _map;
            App.DockManager.Add(new DockablePanel("kMap", "Map", _map, DockStyle.Fill));
        }

        private void SetMap()
        {
            _map = new DotSpatial.Controls.MonoMac.Map { Legend = App.Legend };
        }
    }
}
