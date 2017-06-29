namespace DotSpatial.Plugins.SpatiaLite
{
    public class GeometryColumnInfo
    {
        public string TableName { get; set; }

        public string GeometryColumnName { get; set; }

        public string GeometryType { get; set; }

        public int CoordDimension { get; set; }

        public int SRID { get; set; }

        public bool SpatialIndexEnabled { get; set; }
    }
}