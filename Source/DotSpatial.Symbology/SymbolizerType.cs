// ********************************************************************************************************
// Product Name: DotSpatial.Symbology.dll
// Description:  The basic module for DotSpatial.Drawing.PredefinedSymbols.SymbolizerTypes version 6.0
// ********************************************************************************************************
//
// The Original Code is from DotSpatial.Drawing.PredefinedSymbols.dll version 6.0
//
// The Initial Developer of this Original Code is Jiri Kadlec. Created 5/15/2009 10:20:48 AM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

namespace DotSpatial.Symbology
{
    /// <summary>
    /// The available feature symbolizer types
    /// </summary>
    public enum SymbolizerType
    {
        /// <summary>
        /// The type is PointSymbolizer
        /// </summary>
        Point,
        /// <summary>
        /// The type is LineSymbolizer
        /// </summary>
        Line,
        /// <summary>
        /// The type is PolygonSymbolizer
        /// </summary>
        Polygon,
        /// <summary>
        /// The type is RasterSymbolizer
        /// </summary>
        Raster,
        /// <summary>
        /// The type of the symbolizer is unknown
        /// </summary>
        Unknown
    }
}