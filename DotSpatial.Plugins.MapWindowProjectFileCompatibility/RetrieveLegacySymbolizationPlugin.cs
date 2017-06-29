using DotSpatial.Controls;
using DotSpatial.Symbology;

namespace DotSpatial.Plugins.MapWindowProjectFileCompatibility
{
    public class RetrieveLegacySymbolizationPlugin : Extension
    {
        public override void Activate()
        {
            App.Map.LayerAdded += Map_LayerAdded;

            base.Activate();
        }

        public override void Deactivate()
        {
            App.Map.LayerAdded -= Map_LayerAdded;
            base.Deactivate();
        }

        private void Map_LayerAdded(object sender, LayerEventArgs e)
        {
            IterateThroughAnyGroupsToFindLayers(e.Layer);
        }

        private void IterateThroughAnyGroupsToFindLayers(ILayer layer)
        {
            MapGroup g = (layer as MapGroup);
            if (g == null)
            {
                LegacyLayerDeserializer.TryDeserialization(layer, App.Map);
            }
            else
            {
                foreach (var l in g.Layers)
                {
                    IterateThroughAnyGroupsToFindLayers(l);
                }
            }
        }
    }
}