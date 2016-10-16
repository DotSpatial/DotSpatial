// ********************************************************************************************************
// Product Name: DotSpatial.Compatibility.dll
// Description:  Supports DotSpatial interfaces organized for a MapWindow 4 plugin context.
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 1/20/2009 11:56:31 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Compatibility
{
    /// <summary>
    /// LayerType
    /// </summary>
    public enum LayerType
    {
        /// <summary>
        /// Raster Layer
        /// </summary>
        Grid,

        /// <summary>
        /// Image Layer
        /// </summary>
        Image,

        /// <summary>
        /// Not a valid layer format
        /// </summary>
        Invalid,

        /// <summary>
        /// Line FeatureSet Layer
        /// </summary>
        LineShapefile,

        /// <summary>
        /// Point FeatureSet Layer
        /// </summary>
        PointShapefile,

        /// <summary>
        /// Polygon FeatureSet Layer
        /// </summary>
        PolygonShapefile,
    }
}