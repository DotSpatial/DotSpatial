// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Projections;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This class contains methods for re-projecting layers in the map frame.
    /// </summary>
    public static class MapFrameProjectionHelper
    {
        #region Methods

        /// <summary>
        /// Reprojects all layers in the map frame so that they use the new projection Esri string.
        /// </summary>
        /// <param name="mapFrame">The map frame that contains all layers that should be reprojected.</param>
        /// <param name="newProjEsriString">The Esri WKT string of the new projection.</param>
        public static void ReprojectMapFrame(this IMapFrame mapFrame, string newProjEsriString)
        {
            if (mapFrame == null) throw new ArgumentNullException(nameof(mapFrame));
            if (newProjEsriString == null) throw new ArgumentNullException(nameof(newProjEsriString));

            // parse the projection
            var newProjection = ProjectionInfo.FromEsriString(newProjEsriString);
            mapFrame.ReprojectMapFrame(newProjection);
        }

        /// <summary>
        /// Reprojects all layers in the map frame so that they use new projection.
        /// </summary>
        /// <param name="mapFrame">The map frame that contains all layers that should be reprojected.</param>
        /// <param name="newProjection">New projection.</param>
        /// <param name="onCantReproject">Callback when layer can't be reprojected. Maybe null.</param>
        public static void ReprojectMapFrame(this IMapFrame mapFrame, ProjectionInfo newProjection, Action<ILayer> onCantReproject = null)
        {
            if (mapFrame == null) throw new ArgumentNullException(nameof(mapFrame));
            if (newProjection == null) throw new ArgumentNullException(nameof(newProjection));

            foreach (var layer in mapFrame.GetAllLayers())
            {
                if (layer.CanReproject)
                {
                    layer.Reproject(newProjection);
                }
                else
                {
                    onCantReproject?.Invoke(layer);
                }
            }

            foreach (var grp in mapFrame.GetAllGroups())
            {
                grp.Projection = newProjection;
            }

            mapFrame.Projection = newProjection;

            var parent = mapFrame.Parent as IMap;
            if (parent != null)
            {
                // this is needed to fire the Map.ProjectionChanged event
                parent.Projection = newProjection;
            }
        }

        #endregion
    }
}