// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Data;
using DotSpatial.Projections;
using DotSpatial.Symbology;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This class contains methods for re-projecting
    /// layers in the map frame
    /// </summary>
    public static class MapFrameProjectionHelper
    {
        #region Methods

        /// <summary>
        /// Reprojects all layers in the map frame so that they use the new
        /// projection Esri string
        /// </summary>
        /// <param name="mapFrame">The map frame that contains all layers that should be reprojected</param>
        /// <param name="newProjEsriString">The Esri WKT string of the new projection</param>
        public static void ReprojectMapFrame(this IMapFrame mapFrame, string newProjEsriString)
        {
            if (mapFrame == null) throw new ArgumentNullException("mapFrame");
            if (newProjEsriString == null) throw new ArgumentNullException("newProjEsriString");

            // parse the projection
            var newProjection = ProjectionInfo.FromEsriString(newProjEsriString);
            mapFrame.ReprojectMapFrame(newProjection);
        }

        /// <summary>
        /// Reprojects all layers in the map frame so that they use new projection
        /// </summary>
        /// <param name="mapFrame">The map frame that contains all layers that should be reprojected</param>
        /// <param name="newProjection">New projection</param>
        /// <param name="onCantReproject">Callback when layer can't be reprojected. Maybe null.</param>
        public static void ReprojectMapFrame(this IMapFrame mapFrame, ProjectionInfo newProjection, Action<ILayer> onCantReproject = null)
        {
            if (mapFrame == null) throw new ArgumentNullException("mapFrame");
            if (newProjection == null) throw new ArgumentNullException("newProjection");

            foreach (var layer in mapFrame.GetAllLayers())
            {
                if (layer.CanReproject)
                {
                    layer.Reproject(newProjection);
                }
                else
                {
                    if (onCantReproject != null) onCantReproject(layer);
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
                // this need to fire Map.ProjectionChanged event
                parent.Projection = newProjection;
            }
        }

        /// <summary>
        /// Reprojects all layers in the map frame so that they use new projection. Before they are reprojected the layers
        /// will get reloaded from the original source if available. This is useful when doing multiple reprojections on a
        /// layer as the reprojection will occur on the original vertices and not the reprojected vertices. For certain
        /// workflows multiple reprojections will tend to lose the original vertices and result in unexpected coordinates.
        /// This method attempts to alleviate the issue.
        /// </summary>
        /// <param name="mapFrame">The map frame that contains all layers that should be reprojected</param>
        /// <param name="newProjEsriString">The Esri WKT string of the new projection</param>
        /// <remarks>
        /// This method also includes drawing layers, although they may lose their original vertices since they dont have a
        /// source from where we can reload.
        /// </remarks>
        public static void ReprojectMapFrameWithReload(this IMapFrame mapFrame, string newProjEsriString)
        {
            if (mapFrame == null) throw new ArgumentNullException("mapFrame");
            if (newProjEsriString == null) throw new ArgumentNullException("newProjEsriString");

            // parse the projection
            var newProjection = ProjectionInfo.FromEsriString(newProjEsriString);
            mapFrame.ReprojectMapFrameWithReload(newProjection);
        }

        /// <summary>
        /// Reprojects all layers in the map frame so that they use new projection. Before they are reprojected the layers
        /// will get reloaded from the original source if available. This is useful when doing multiple reprojections on a
        /// layer as the reprojection will occur on the original vertices and not the reprojected vertices. For certain
        /// workflows multiple reprojections will tend to lose the original vertices and result in unexpected coordinates.
        /// This method attempts to alleviate the issue.
        /// </summary>
        /// <param name="mapFrame">The map frame that contains all layers that should be reprojected</param>
        /// <param name="newProjection">New projection</param>
        /// <param name="onCantReproject">Callback when layer can't be reprojected. Maybe null.</param>
        /// <remarks>
        /// This method also includes drawing layers, although they may lose their original vertices since they dont have a
        /// source from where we can reload.
        /// </remarks>
        public static void ReprojectMapFrameWithReload(this IMapFrame mapFrame, ProjectionInfo newProjection, Action<ILayer> onCantReproject = null)
        {
            if (mapFrame == null) throw new ArgumentNullException("mapFrame");
            if (newProjection == null) throw new ArgumentNullException("newProjection");

            foreach (var layer in mapFrame.GetAllLayers())
            {
                if (layer.CanReproject)
                {
                    // check the DataSet of the layer and determine if it is sourced from a file.
                    string fileName = layer.DataSet.Filename;
                    if (string.IsNullOrWhiteSpace(fileName))
                    {
                        // this layer does not have a source file so it is probably an in-ram featureset. so just reproject
                        // it as usual.
                        layer.Reproject(newProjection);
                    }
                    else
                    {
                        // cache the label layer. we assume only feature layers can contain a label layer.
                        ILabelLayer lblLyr = null;
                        IFeatureLayer featLyr = layer as IFeatureLayer;
                        if (featLyr != null)
                        {
                            lblLyr = featLyr.LabelLayer;
                        }

                        // the layer has a source file so reload it into the layer. reload the data depending on the layer type.
                        if (layer is IMapFeatureLayer)
                        {
                            IFeatureSet fs = DotSpatial.Data.FeatureSet.OpenFile(fileName);
                            IMapFeatureLayer lyrFeat = (IMapFeatureLayer)layer;
                            lyrFeat.DataSet = fs;
                        }
                        else if (layer is IMapRasterLayer)
                        {
                            IRaster rs = DotSpatial.Data.Raster.OpenFile(fileName);
                            IMapRasterLayer lyrRst = (MapRasterLayer)layer;
                            lyrRst.DataSet = rs;
                        }
                        else if (layer is IMapImageLayer)
                        {
                            IImageData img = DotSpatial.Data.ImageData.Open(fileName);
                            IMapImageLayer lyrImg = (IMapImageLayer)layer;
                            lyrImg.Image = img;
                        }
                        else
                        {
                            // any other type of layer does not get its data reloaded.
                        }

                        // reproject the layer. if data was reloaded then we are reprojecting from the original source, otherwise
                        // if we do not reload the data then we are reprojecting data that could potentially be reprojected several
                        // times and thus could lose the original vertices.
                        layer.Reproject(newProjection);

                        // labels need to be re-attached if it's a featurelayer and it has a label layer.
                        if (featLyr != null && lblLyr != null)
                        {
                            featLyr.LabelLayer = lblLyr;
                        }
                    }
                }
                else
                {
                    if (onCantReproject != null) onCantReproject(layer);
                }
            }

            // we have to reproject drawing layers separately. drawing layers typically do not have a source file.
            int ignoredDrawLayerCount = 0;
            foreach (Layer layer in mapFrame.DrawingLayers)
            {
                if (layer.CanReproject)
                {
                    layer.Reproject(newProjection);
                }
                else
                {
                    ignoredDrawLayerCount += 1;
                    if (onCantReproject != null) onCantReproject(layer);
                }
            }

            if (ignoredDrawLayerCount > 0)
            {
                System.Diagnostics.Debug.Print(ignoredDrawLayerCount + " drawing layer(s) were not reprojected.");
            }

            foreach (var grp in mapFrame.GetAllGroups())
            {
                grp.Projection = newProjection;
            }

            mapFrame.Projection = newProjection;

            var parent = mapFrame.Parent as IMap;
            if (parent != null)
            {
                // this need to fire Map.ProjectionChanged event
                parent.Projection = newProjection;
            }
        }

        #endregion
    }
}