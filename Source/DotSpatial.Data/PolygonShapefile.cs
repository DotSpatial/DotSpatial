// ********************************************************************************************************
// Product Name: DotSpatial.Data.dll
// Description:  The data access libraries for the DotSpatial project.
//
// ********************************************************************************************************
//
// The Original Code is DotSpatial
//
// The Initial Developer of this Original Code is Ted Dunsford. Created in February, 2008.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//
// ********************************************************************************************************

using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeoAPI.Geometries;
using NetTopologySuite.Algorithm;

namespace DotSpatial.Data
{
    /// <summary>
    /// A shapefile class that handles the special case where the data is made up of polygons
    /// </summary>
    public class PolygonShapefile : Shapefile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonShapefile"/> class for in-ram handling only.
        /// </summary>
        public PolygonShapefile()
            : base(FeatureType.Polygon, ShapeType.Polygon)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonShapefile"/> class that is loaded from the supplied fileName.
        /// </summary>
        /// <param name="fileName">The string fileName of the polygon shapefile to load</param>
        public PolygonShapefile(string fileName)
            : this()
        {
            Open(fileName, null);
        }

        /// <summary>
        /// Opens a shapefile.
        /// </summary>
        /// <param name="fileName">The string fileName of the polygon shapefile to load.</param>
        /// <param name="progressHandler">Any valid implementation of the DotSpatial.Data.IProgressHandler</param>
        public void Open(string fileName, IProgressHandler progressHandler)
        {
            if (!File.Exists(fileName)) return;

            Filename = fileName;
            IndexMode = true;
            Header = new ShapefileHeader(Filename);

            switch (Header.ShapeType)
            {
                case ShapeType.PolygonM:
                    CoordinateType = CoordinateType.M;
                    break;
                case ShapeType.PolygonZ:
                    CoordinateType = CoordinateType.Z;
                    break;
                default:
                    CoordinateType = CoordinateType.Regular;
                    break;
            }

            Extent = Header.ToExtent();
            Name = Path.GetFileNameWithoutExtension(fileName);
            Attributes.Open(Filename);

            LineShapefile.FillLines(Filename, progressHandler, this, FeatureType.Polygon);
            ReadProjection();
        }

        // X Y Poly Lines: Total Length = 28 Bytes
        // ---------------------------------------------------------
        // Position     Value               Type        Number      Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big
        // Byte 8       Shape Type 3        Integer     1           Little
        // Byte 12      Xmin                Double      1           Little
        // Byte 20      Ymin                Double      1           Little
        // Byte 28      Xmax                Double      1           Little
        // Byte 36      Ymax                Double      1           Little
        // Byte 44      NumParts            Integer     1           Little
        // Byte 48      NumPoints           Integer     1           Little
        // Byte 52      Parts               Integer     NumParts    Little
        // Byte X       Points              Point       NumPoints   Little
        // X = 52 + 4 * NumParts

        // X Y M Poly Lines: Total Length = 34 Bytes
        // ---------------------------------------------------------
        // Position     Value               Type        Number      Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big
        // Byte 8       Shape Type 23       Integer     1           Little
        // Byte 12      Box                 Double      4           Little
        // Byte 44      NumParts            Integer     1           Little
        // Byte 48      NumPoints           Integer     1           Little
        // Byte 52      Parts               Integer     NumParts    Little
        // Byte X       Points              Point       NumPoints   Little
        // Byte Y*      Mmin                Double      1           Little
        // Byte Y + 8*  Mmax                Double      1           Little
        // Byte Y + 16* Marray              Double      NumPoints   Little
        // X = 52 + (4 * NumParts)
        // Y = X + (16 * NumPoints)
        // * = optional

        // X Y Z M Poly Lines: Total Length = 44 Bytes
        // ---------------------------------------------------------
        // Position     Value               Type        Number  Byte Order
        // ---------------------------------------------------------
        // Byte 0       Record Number       Integer     1           Big
        // Byte 4       Content Length      Integer     1           Big
        // Byte 8       Shape Type 13       Integer     1           Little
        // Byte 12      Box                 Double      4           Little
        // Byte 44      NumParts            Integer     1           Little
        // Byte 48      NumPoints           Integer     1           Little
        // Byte 52      Parts               Integer     NumParts    Little
        // Byte X       Points              Point       NumPoints   Little
        // Byte Y       Zmin                Double      1           Little
        // Byte Y + 8   Zmax                Double      1           Little
        // Byte Y + 16  Zarray              Double      NumPoints   Little
        // Byte Z*      Mmin                Double      1           Little
        // Byte Z+8*    Mmax                Double      1           Little
        // Byte Z+16*   Marray              Double      NumPoints   Little
        // X = 52 + (4 * NumParts)
        // Y = X + (16 * NumPoints)
        // Z = Y + 16 + (8 * NumPoints)
        // * = optional

        /// <summary>
        /// Gets the specified feature by constructing it from the vertices, rather
        /// than requiring that all the features be created. (which takes up a lot of memory).
        /// </summary>
        /// <param name="index">The integer index</param>
        /// <returns>The polygon feature belonging to the given index.</returns>
        public override IFeature GetFeature(int index)
        {
            IFeature f;
            if (!IndexMode)
            {
                f = Features[index];
            }
            else
            {
                f = GetPolygon(index);
                f.DataRow = AttributesPopulated ? DataTable.Rows[index] : Attributes.SupplyPageOfData(index, 1).Rows[0];
            }

            return f;
        }

        /// <inheritdoc />
        protected override void SetHeaderShapeType()
        {
            if (CoordinateType == CoordinateType.Regular)
                Header.ShapeType = ShapeType.Polygon;
            else if (CoordinateType == CoordinateType.M)
                Header.ShapeType = ShapeType.PolygonM;
            else if (CoordinateType == CoordinateType.Z)
                Header.ShapeType = ShapeType.PolygonZ;
        }

        /// <inheritdoc />
        protected override StreamLengthPair PopulateShpAndShxStreams(Stream shpStream, Stream shxStream, bool indexed)
        {
            if (indexed)
                return LineShapefile.PopulateStreamsIndexed(shpStream, shxStream, this, ShapeType.PolygonZ, ShapeType.PolygonM, true);

            return LineShapefile.PopulateStreamsNotIndexed(shpStream, shxStream, this, AddPoints, ShapeType.PolygonZ, ShapeType.PolygonM, true);
        }

        /// <summary>
        /// Adds the parts and points of the given feature to the given parts and points lists.
        /// </summary>
        /// <param name="parts">List of parts, where the features parts get added.</param>
        /// <param name="points">List of points, where the features points get added.</param>
        /// <param name="f">Feature, whose parts and points get added to the lists.</param>
        private static void AddPoints(List<int> parts, List<Coordinate> points, IFeature f)
        {
            for (int iPart = 0; iPart < f.Geometry.NumGeometries; iPart++)
            {
                parts.Add(points.Count);
                IPolygon pg = f.Geometry.GetGeometryN(iPart) as IPolygon;
                if (pg == null) continue;

                ILineString bl = pg.Shell;

                // Exterior rings need to be clockwise
                IEnumerable<Coordinate> coords = CGAlgorithms.IsCCW(bl.Coordinates) ? bl.Coordinates.Reverse() : bl.Coordinates;
                points.AddRange(coords);

                foreach (var hole in pg.Holes)
                {
                    parts.Add(points.Count);

                    // Interior rings need to be counter-clockwise
                    IEnumerable<Coordinate> holeCoords = CGAlgorithms.IsCCW(hole.Coordinates) ? hole.Coordinates : hole.Coordinates.Reverse();
                    points.AddRange(holeCoords);
                }
            }
        }
    }
}