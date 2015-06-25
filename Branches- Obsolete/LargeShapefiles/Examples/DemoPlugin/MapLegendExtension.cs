using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;

namespace DemoPlugin
{
    /// <summary>
    /// Adds map and legend to the application
    /// </summary>
    public class MapLegendExtension : Extension
    {
        public MapLegendExtension()
        {
            DeactivationAllowed = false;
        }

        public override int Priority
        {
            get { return -100; }
        }

        public override void Activate()
        {
            base.Activate();

            // Add legend
            var legend = new Legend{Text = "Legend"};
            App.Legend = legend;
            App.DockManager.Add(new DockablePanel("kLegend", "Legend", legend, DockStyle.Left));

            // Add map
            var map = new Map {Text = "Map", Legend = App.Legend};
            App.Map = map;
            App.DockManager.Add(new DockablePanel("kMap", "Map", map, DockStyle.Fill));
        }

        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            App.DockManager.Remove("kLegend");
            App.DockManager.Remove("kMap");
            base.Deactivate();
        }
    }
}