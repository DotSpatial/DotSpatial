﻿// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System.Collections.Generic;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// Extension methods for accessing the layers (including nested layers) and groups (including nested groups) of the map frame.
    /// </summary>
    public static class MapFrameExt
    {
        #region Methods

        /// <summary>
        /// Gets all feature layers of the map frame including feature layers which are nested
        /// within groups. The group objects themselves are not included in this list.
        /// </summary>
        /// <param name="mapFrame">this</param>
        /// <returns>The list of the feature layers</returns>
        public static List<IMapFeatureLayer> GetAllFeatureLayers(this IMapFrame mapFrame)
        {
            return GetAllTypeLayers<IMapFeatureLayer>(mapFrame);
        }

        /// <summary>
        /// Gets all map groups in the map including the nested groups.
        /// </summary>
        /// <param name="mapFrame">this</param>
        /// <returns>the list of the groups</returns>
        public static List<IMapGroup> GetAllGroups(this IMapFrame mapFrame)
        {
            var groupList = new List<IMapGroup>();
            GetNestedGroups(mapFrame, groupList);
            return groupList;
        }

        /// <summary>
        /// Gets all image layers of the map frame including image layers which are nested
        /// within groups. The group objects themselves are not included in this list.
        /// </summary>
        /// <param name="mapFrame">this</param>
        /// <returns>The list of the image layers</returns>
        public static List<IMapImageLayer> GetAllImageLayers(this IMapFrame mapFrame)
        {
            return GetAllTypeLayers<IMapImageLayer>(mapFrame);
        }

        /// <summary>
        /// Gets all layers of the map frame including layers which are nested
        /// within groups. The group objects themselves are not included in this list,
        /// but all FeatureLayers, RasterLayers, ImageLayers and other layers are included.
        /// </summary>
        /// <param name="mapFrame">this</param>
        /// <returns>The list of the layers</returns>
        public static List<ILayer> GetAllLayers(this IMapFrame mapFrame)
        {
            return GetAllTypeLayers<ILayer>(mapFrame);
        }

        /// <summary>
        /// Gets all line layers of the map frame including line layers which are nested
        /// within groups. The group objects themselves are not included in this list.
        /// </summary>
        /// <param name="mapFrame">this</param>
        /// <returns>The list of the line layers</returns>
        public static List<IMapLineLayer> GetAllLineLayers(this IMapFrame mapFrame)
        {
            return GetAllTypeLayers<IMapLineLayer>(mapFrame);
        }

        /// <summary>
        /// Gets all point layers of the map frame including point layers which are nested
        /// within groups. The group objects themselves are not included in this list.
        /// </summary>
        /// <param name="mapFrame">this</param>
        /// <returns>The list of the point layers</returns>
        public static List<IMapPointLayer> GetAllPointLayers(this IMapFrame mapFrame)
        {
            return GetAllTypeLayers<IMapPointLayer>(mapFrame);
        }

        /// <summary>
        /// Gets all polygon layers of the map frame including polygon layers which are nested
        /// within groups. The group objects themselves are not included in this list.
        /// </summary>
        /// <param name="mapFrame">this</param>
        /// <returns>The list of the polygon layers</returns>
        public static List<IMapPolygonLayer> GetAllPolygonLayers(this IMapFrame mapFrame)
        {
            return GetAllTypeLayers<IMapPolygonLayer>(mapFrame);
        }

        /// <summary>
        /// Gets all raster layers of the map frame including raster layers which are nested
        /// within groups. The group objects themselves are not included in this list.
        /// </summary>
        /// <param name="mapFrame">this</param>
        /// <returns>The list of the raster layers</returns>
        public static List<IMapRasterLayer> GetAllRasterLayers(this IMapFrame mapFrame)
        {
            return GetAllTypeLayers<IMapRasterLayer>(mapFrame);
        }

        /// <summary>
        /// Gets all the layers of the given type.
        /// </summary>
        /// <typeparam name="T">Type of the layers that should be included.</typeparam>
        /// <param name="mapFrame">mapFrame that contains the layers.</param>
        /// <returns>The list of the layers with the given type.</returns>
        private static List<T> GetAllTypeLayers<T>(this IMapFrame mapFrame)
            where T : class
        {
            var layerList = new List<T>();
            GetNestedLayers(mapFrame, layerList);
            return layerList;
        }

        /// <summary>
        /// Recursively adds all the groups to groupList.
        /// </summary>
        /// <param name="grp">Group to search through.</param>
        /// <param name="groupList">The list the groups should be added to.</param>
        private static void GetNestedGroups(IMapGroup grp, List<IMapGroup> groupList)
        {
            // initialize the layer list if required
            if (groupList == null) groupList = new List<IMapGroup>();

            // recursive function -- all nested groups and layers are considered
            foreach (var lyr in grp.Layers)
            {
                grp = lyr as IMapGroup;
                if (grp != null)
                {
                    GetNestedGroups(grp, groupList);
                    groupList.Add(grp);
                }
            }
        }

        /// <summary>
        /// Recursively adds all the layers of the given type that are found in group to layerList.
        /// </summary>
        /// <typeparam name="T">Type of the layers that should be included.</typeparam>
        /// <param name="group">Group that contains the layers.</param>
        /// <param name="layerList">The list the layers should be added to.</param>
        private static void GetNestedLayers<T>(IMapGroup group, List<T> layerList)
            where T : class
        {
            if (layerList == null) layerList = new List<T>();
            foreach (var layer in group.Layers)
            {
                var grp = layer as IMapGroup;
                if (grp != null)
                {
                    GetNestedLayers(grp, layerList);
                }
                else
                {
                    var tlayer = layer as T;
                    if (tlayer != null)
                    {
                        layerList.Add(tlayer);
                    }
                }
            }
        }

        #endregion
    }
}