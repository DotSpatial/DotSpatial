// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using DotSpatial.Controls;
using DotSpatial.Symbology;

namespace DotSpatial.Plugins.MapWindowProjectFileCompatibility
{
    /// <summary>
    /// Plugin that allows to retrieve legacy symbolization.
    /// </summary>
    public class RetrieveLegacySymbolizationPlugin : Extension
    {
        #region Methods

        /// <inheritdoc />
        public override void Activate()
        {
            App.Map.LayerAdded += MapLayerAdded;

            base.Activate();
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            App.Map.LayerAdded -= MapLayerAdded;
            base.Deactivate();
        }

        private void IterateThroughAnyGroupsToFindLayers(ILayer layer)
        {
            MapGroup g = layer as MapGroup;
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

        private void MapLayerAdded(object sender, LayerEventArgs e)
        {
            IterateThroughAnyGroupsToFindLayers(e.Layer);
        }

        #endregion
    }
}