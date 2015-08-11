namespace DotSpatial.Topology.Geometries
{
    /// <summary>
    /// Enumeration of OGC Geometry Types
    /// </summary>
    public enum OgcGeometryType
    {
        /// <summary>
        /// Point.
        /// </summary>
        Point = 1,

        /// <summary>
        /// LineString.
        /// </summary>
        LineString = 2,

        /// <summary>
        /// Polygon.
        /// </summary>
        Polygon = 3,

        /// <summary>
        /// MultiPoint.
        /// </summary>
        MultiPoint = 4,

        /// <summary>
        /// MultiLineString.
        /// </summary>
        MultiLineString = 5,

        /// <summary>
        /// MultiPolygon.
        /// </summary>
        MultiPolygon = 6,

        /// <summary>
        /// GeometryCollection.
        /// </summary>
        GeometryCollection = 7,

        /// <summary>
        /// CircularString
        /// </summary>
        CircularString = 8,
        
        /// <summary>
        /// CompoundCurve
        /// </summary>
        CompoundCurve = 9,
        
        /// <summary>
        /// CurvePolygon
        /// </summary>
        CurvePolygon = 10,
        
        /// <summary>
        /// MultiCurve
        /// </summary>
        MultiCurve = 11,
        
        /// <summary>
        /// MultiSurface
        /// </summary>
        MultiSurface = 12,
        
        /// <summary>
        /// Curve
        /// </summary>
        Curve = 13,
        
        /// <summary>
        /// Surface
        /// </summary>
        Surface = 14,
        
        /// <summary>
        /// PolyhedralSurface
        /// </summary>
        PolyhedralSurface = 15,
        
        /// <summary>
        /// TIN
        /// </summary>
// ReSharper disable InconsistentNaming
        TIN = 16
// ReSharper restore InconsistentNaming
    };
}