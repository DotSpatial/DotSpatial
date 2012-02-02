using DotSpatial.Projections;

namespace DotSpatial.Controls
{
    /// <summary>
    /// This class contains methods for re-projecting
    /// layers in the map frame
    /// </summary>
    public static class MapFrameProjectionHelper
    {
        /// <summary>
        /// Reprojects all layers in the map frame so that they use the new
        /// projection ESRI string
        /// </summary>
        /// <param name="mapFrame">The map frame that contains all layers that should be reprojected</param>
        /// <param name="newProjEsriString">The ESRI WKT string of the new projection</param>
        public static void ReprojectMapFrame(IMapFrame mapFrame, string newProjEsriString)
        {
            //parse the projection
            ProjectionInfo newProjection = ProjectionInfo.FromEsriString(newProjEsriString);

            foreach (IMapLayer layer in mapFrame.GetAllLayers())
            {
                if (layer.DataSet.CanReproject)
                {
                    layer.DataSet.Reproject(newProjection);
                }
            }
            foreach (IMapGroup grp in mapFrame.GetAllGroups())
            {
                grp.Projection = ProjectionInfo.FromEsriString(newProjEsriString);
            }
            mapFrame.Projection = newProjection;

            var parent = mapFrame.Parent as IMap;
            if (parent != null)
            {
                // this need to fire Map.ProjectionChanged event
                parent.Projection = newProjection;
            } 
        }
    }
}