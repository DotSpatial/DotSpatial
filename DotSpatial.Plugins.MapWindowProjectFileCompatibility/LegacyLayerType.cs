namespace DotSpatial.Plugins.MapWindowProjectFileCompatibility
{
    /// <summary>
    /// LayerType
    /// </summary>
    public enum LegacyLayerType
    {
        /// <summary>
        /// Raster Layer
        /// </summary>
        Grid = 4,

        /// <summary>
        /// Image Layer
        /// </summary>
        Image = 0,

        /// <summary>
        /// Not a valid layer format
        /// </summary>
        Invalid = -1,

        /// <summary>
        /// Line FeatureSet Layer
        /// </summary>
        LineShapefile = 2,

        /// <summary>
        /// Point FeatureSet Layer
        /// </summary>
        PointShapefile = 1,

        /// <summary>
        /// Polygon FeatureSet Layer
        /// </summary>
        PolygonShapefile = 3,
    }
}