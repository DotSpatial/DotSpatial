using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Controls.Docking;

namespace DotSpatial.Plugins.SimpleMap
{
    /// <summary>
    /// Adds a simple map to the program.
    /// </summary>
    public class SimpleMapPlugin : Extension
    {
        private Map _map;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMapPlugin"/> class.
        /// </summary>
        public SimpleMapPlugin()
        {
            DeactivationAllowed = false;
        }

        /// <summary>
        /// Gets the priority. It's a very low value so it will be loaded at the beginning.
        /// </summary>
        public override int Priority => -10000;

        /// <inheritdoc />
        public override void Activate()
        {
            ShowMap();
            base.Activate();
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            App.HeaderControl.RemoveAll();
            App.DockManager.Remove("kMap");
            if (App.Map == _map) App.Map = null;
            base.Deactivate();
        }

        private void ShowMap()
        {
            _map = new Map { Text = "Map", Name = "map1", Legend = App.Legend };
            App.Map = _map;
            App.DockManager.Add(new DockablePanel("kMap", "Map", _map, DockStyle.Fill));
        }
    }
}
