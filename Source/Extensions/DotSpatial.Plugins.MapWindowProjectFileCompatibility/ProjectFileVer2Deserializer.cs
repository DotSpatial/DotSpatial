// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using DotSpatial.Controls;
using DotSpatial.Data;
using DotSpatial.Symbology;

namespace DotSpatial.Plugins.MapWindowProjectFileCompatibility
{
    /// <summary>
    /// Deserializer for version 2 project files.
    /// </summary>
    public class ProjectFileVer2Deserializer
    {
        #region Fields

        private readonly IMap _map;

        #endregion

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectFileVer2Deserializer"/> class.
        /// </summary>
        /// <param name="map">The map.</param>
        public ProjectFileVer2Deserializer(IMap map)
        {
            _map = map;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deserializes beginning from the given root.
        /// </summary>
        /// <param name="xmlRoot">Root to use as string point.</param>
        public void Deserialize(dynamic xmlRoot)
        {
            var mapwin4Section = xmlRoot.MapWindow4;
            var mapwingisSection = xmlRoot.MapWinGIS;

            _map.MapFrame.ProjectionString = mapwin4Section["ProjectProjection"];

            if (!Convert.ToBoolean(mapwin4Section["ViewBackColor_UseDefault"]))
            {
                if (_map is Control mapControl)
                    mapControl.BackColor = LegacyDeserializer.GetColor(mapwin4Section["ViewBackColor"]);

                _map.Invalidate();
            }

            // Deserialize layers
            var layersDescs = mapwingisSection.Layers.Elements();
            var allLayers = new Dictionary<int, List<ILayer>>(); // key: Group Name. Value: layers
            foreach (var layer in mapwin4Section.Layers.Elements())
            {
                var name = (string)layer["Name"];
                var groupInd = Convert.ToInt32(layer["GroupIndex"]);
                if (!allLayers.ContainsKey(groupInd)) allLayers[groupInd] = new List<ILayer>();
                var listLayers = allLayers[groupInd];

                IMapLayer layerToAdd = null;
                foreach (var layersDesc in layersDescs)
                {
                    if (layersDesc["LayerName"] == name)
                    {
                        var lt = (string)layersDesc["LayerType"];
                        switch (lt)
                        {
                            case "Image":
                                layerToAdd = new MapImageLayer(ImageData.Open(layersDesc["Filename"]));
                                break;
                            case "Shapefile":
                                var fs = FeatureSet.OpenFile(layersDesc["Filename"]);
                                if (fs is PointShapefile)
                                {
                                    layerToAdd = new MapPointLayer(fs);
                                }
                                else if (fs is PolygonShapefile)
                                {
                                    layerToAdd = new MapPolygonLayer(fs);
                                }
                                else if (fs is LineShapefile)
                                {
                                    layerToAdd = new MapLineLayer(fs);
                                }
                                else
                                {
                                    var type = (object)fs.GetType().ToString();
                                    Trace.WriteLine("Unsupported FeatureSet Type: " + type);
                                }

                                break;
                            default:
                                Trace.WriteLine("Unsupported LayerType: " + lt);
                                break;
                        }

                        break;
                    }
                }

                if (layerToAdd != null)
                {
                    layerToAdd.IsExpanded = Convert.ToBoolean(layer["Expanded"]);
                    listLayers.Add(layerToAdd);
                }
            }

            // Deserialize groups
            foreach (var group in mapwin4Section.Groups.Elements())
            {
                var gInd = Convert.ToInt32(group["Position"]);
                var g = new MapGroup
                {
                    LegendText = group["Name"],
                    IsExpanded = Convert.ToBoolean(group["Expanded"])
                };
                if (allLayers.TryGetValue(gInd, out List<ILayer> gl))
                {
                    foreach (var layer in gl)
                    {
                        g.Add(layer);
                    }
                }

                _map.MapFrame.Layers.Add(g);
            }
        }

        #endregion
    }
}