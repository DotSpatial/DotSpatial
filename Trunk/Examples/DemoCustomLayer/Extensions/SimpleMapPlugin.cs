using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;

namespace DemoCustomLayer.Extensions
{
    /// <summary>
    /// Simple plugin which adds map to the dock manager
    /// </summary>
    public class SimpleMapPlugin : Extension
    {
        public SimpleMapPlugin()
        {
            DeactivationAllowed = false;
        }

        public override int Priority
        {
            get { return -100; }
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
