using System;
using System.Linq;
using NUnit.Framework;


namespace DotSpatial.Topology.Tests
{
    /// <summary>
    ///
    /// </summary>
    [TestFixture]
    public class PointsLinesPolygons
    {
        [Test]
        public void LineLength()
        {
            Coordinate[] coords = new Coordinate[36];
            Random rnd = new Random();
            GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[36];
            for (int i = 0; i < 36; i++)
            {
                coords[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                double x = coords[i].X;
                double y = coords[i].Y;
                GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILineString lscheck = gf.CreateLineString(coordscheck);
            LineString ls = new LineString(coords);
            double length = ls.Length;
            double lengthcheck = lscheck.Length;
            Assert.AreEqual(length, lengthcheck);
        }

        [Test]
        public void PolygonArea()
        {
            Coordinate[] coords = new Coordinate[20];
            Random rnd = new Random();
            Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (int i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                double x = coords[i].X;
                double y = coords[i].Y;
                GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coordscheck[0].X, coordscheck[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
            GeoAPI.Geometries.IPolygon polygonCheck = gf.CreatePolygon(ring, null);
            Polygon pg = new Polygon(coords);
            double areaCheck = polygonCheck.Area;
            double area = pg.Area;
            Assert.AreEqual(area, areaCheck);
        }
        [Test]
        public void Buffer()
        {
            Random rnd = new Random();
            Coordinate coords = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            double x = coords.X;
            double y = coords.Y;
            GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
            //coordscheck[i] = c;
            Point p = new Point(coords);
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.IPoint ps = gf.CreatePoint(c);
            double area = p.Buffer(500).Area;
            double areacheck = ps.Buffer(500).Area;
            Assert.AreEqual(area, areacheck);
        }

        [Test]
        public void PolygonHoles()
        {
            Coordinate[] coords = new Coordinate[20];
            Random rnd = new Random();
            Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);

            // Shell Coordinates
            GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (int i = 0; i < 19; i++)
            {
                double x = center.X + Math.Cos((i * 10) * Math.PI / 10);
                double y = center.Y + (i * 10) * Math.PI / 10;
                coords[i] = new Coordinate(x, y);
                coordscheck[i] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);


            // Shell Rings
            LinearRing ring = new LinearRing(coords);
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILinearRing ringCheck = gf.CreateLinearRing(coordscheck);


            // Hole Coordinates
            GeoAPI.Geometries.ICoordinate[] coordsholecheck = new GeoAPI.Geometries.ICoordinate[20];
            Coordinate[] coordshole = new Coordinate[20];
            for (int i = 0; i < 20; i++)
            {
                double x = center.X + Math.Cos((i * 10) * Math.PI / 20);
                double y = center.Y + (i * 10) * Math.PI / 20;
                coordshole[i] = new Coordinate(x, y);
                coordsholecheck[i] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
            }
            coordshole[19] = new Coordinate(coordshole[0].X, coordshole[0].Y);
            coordsholecheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coordshole[0].X, coordshole[0].Y);

            // Hole LinearRing Arrays
            LinearRing hole = new LinearRing(coordshole);
            ILinearRing[] holes = new ILinearRing[1];
            GeoAPI.Geometries.ILinearRing holeCheck = gf.CreateLinearRing(coordsholecheck);
            GeoAPI.Geometries.ILinearRing[] holescheck = new GeoAPI.Geometries.ILinearRing[1];
            holes[0] = hole;
            holescheck[0] = holeCheck;


            Polygon pg = new Polygon(ring, holes);
            GeoAPI.Geometries.IPolygon polygonCheck = gf.CreatePolygon(ringCheck, holescheck);
            double areaCheck = polygonCheck.Area;
            double area = pg.Area;
            Assert.AreEqual(area, areaCheck);
        }


        [Test]
        public void BufferLength()
        {
            Random rnd = new Random();
            Coordinate coords = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            double x = coords.X;
            double y = coords.Y;
            GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
            //coordscheck[i] = c;
            Point p = new Point(coords);
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.IPoint ps = gf.CreatePoint(c);
            double boundary = p.Buffer(500).Length;
            double boundarycheck = ps.Buffer(500).Length;
            AssertExt.AreEqual15(boundary, boundarycheck);
        }

        [Test]
        public void PolgygonParimeterLength()
        {
            Coordinate[] coords = new Coordinate[20];
            Random rnd = new Random();
            Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (int i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                double x = coords[i].X;
                double y = coords[i].Y;
                GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
            GeoAPI.Geometries.IPolygon polygonCheck = gf.CreatePolygon(ring, null);
            Polygon pg = new Polygon(coords);
            double lengthCheck = polygonCheck.Length;
            double length = pg.Length;
            Assert.AreEqual(length, lengthCheck);

        }

        [Test]
        public void PolygonBoundaryCentroid()
        {
            Coordinate[] coords = new Coordinate[20];
            Random rnd = new Random();
            Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (int i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                double x = coords[i].X;
                double y = coords[i].Y;
                GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
            GeoAPI.Geometries.IPolygon polygonCheck = gf.CreatePolygon(ring, null);
            Polygon pg = new Polygon(coords);
            double x1 = pg.Boundary.Centroid.X;
            double x2 = polygonCheck.Boundary.Centroid.X;
            AssertExt.AreEqual15(x1, x2);
            double y1 = pg.Boundary.Centroid.Y;
            double y2 = polygonCheck.Boundary.Centroid.Y;
            AssertExt.AreEqual15(y1, y2);
        }

        [Test]
        public void PolygonBoundaryArea()
        {
            Coordinate[] coords = new Coordinate[20];
            Random rnd = new Random();
            Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (int i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                double x = coords[i].X;
                double y = coords[i].Y;
                GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
            GeoAPI.Geometries.IPolygon polygonCheck = gf.CreatePolygon(ring, null);
            Polygon pg = new Polygon(coords);
            AssertExt.AreEqual15(pg.Boundary.Area, polygonCheck.Boundary.Area);
        }

        [Test]
        public void PolygonCentroid()
        {
            Coordinate[] coords = new Coordinate[20];
            Random rnd = new Random();
            Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (int i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                double x = coords[i].X;
                double y = coords[i].Y;
                GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
            GeoAPI.Geometries.IPolygon polygonCheck = gf.CreatePolygon(ring, null);
            Polygon pg = new Polygon(coords);
            double x1 = pg.Centroid.X;
            double x2 = polygonCheck.Centroid.X;
            AssertExt.AreEqual15(x1, x2);
            double y1 = pg.Centroid.Y;
            double y2 = polygonCheck.Centroid.Y;
            AssertExt.AreEqual15(y1, y2);
        }

        [Test]
        public void PolygonNumPoints()
        {
            Coordinate[] coords = new Coordinate[20];
            Random rnd = new Random();
            Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (int i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                double x = coords[i].X;
                double y = coords[i].Y;
                GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
            GeoAPI.Geometries.IPolygon polygonCheck = gf.CreatePolygon(ring, null);
            Polygon pg = new Polygon(coords);
            Assert.AreEqual(pg.NumPoints, polygonCheck.NumPoints);
        }

        [Test]
        public void PolygonShellGeometryType()
        {
            Coordinate[] coords = new Coordinate[20];
            Random rnd = new Random();
            Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (int i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                double x = coords[i].X;
                double y = coords[i].Y;
                GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
            GeoAPI.Geometries.IPolygon polygonCheck = gf.CreatePolygon(ring, null);
            Polygon pg = new Polygon(coords);
            Assert.AreEqual(pg.Shell.GeometryType, polygonCheck.Shell.GeometryType);
        }

        [Test]
        public void PolygonShellStartPoint()
        {
            Coordinate[] coords = new Coordinate[20];
            Random rnd = new Random();
            Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (int i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                double x = coords[i].X;
                double y = coords[i].Y;
                GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
            GeoAPI.Geometries.IPolygon polygonCheck = gf.CreatePolygon(ring, null);
            Polygon pg = new Polygon(coords);
            AssertExt.AreEqual15(pg.Shell.StartPoint.X, polygonCheck.Shell.StartPoint.X);
            AssertExt.AreEqual15(pg.Shell.StartPoint.Y, polygonCheck.Shell.StartPoint.Y);
        }

        [Test]
        public void PolygonShellEndPoint()
        {
            Coordinate[] coords = new Coordinate[20];
            Random rnd = new Random();
            Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (int i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                double x = coords[i].X;
                double y = coords[i].Y;
                GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
            GeoAPI.Geometries.IPolygon polygonCheck = gf.CreatePolygon(ring, null);
            Polygon pg = new Polygon(coords);
            AssertExt.AreEqual15(pg.Shell.EndPoint.X, polygonCheck.Shell.EndPoint.X);
            AssertExt.AreEqual15(pg.Shell.EndPoint.Y, polygonCheck.Shell.EndPoint.Y);
        }

        [Test]
        public void LineStartPoint()
        {
            Coordinate[] coords = new Coordinate[36];
            Random rnd = new Random();
            GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[36];
            for (int i = 0; i < 36; i++)
            {
                coords[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                double x = coords[i].X;
                double y = coords[i].Y;
                GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILineString lscheck = gf.CreateLineString(coordscheck);
            LineString ls = new LineString(coords);
            AssertExt.AreEqual15(ls.StartPoint.X, lscheck.StartPoint.X);
            AssertExt.AreEqual15(ls.StartPoint.Y, lscheck.StartPoint.Y);
        }

        [Test]
        public void LineEndPoint()
        {
            Coordinate[] coords = new Coordinate[36];
            Random rnd = new Random();
            GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[36];
            for (int i = 0; i < 36; i++)
            {
                coords[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                double x = coords[i].X;
                double y = coords[i].Y;
                GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILineString lscheck = gf.CreateLineString(coordscheck);
            LineString ls = new LineString(coords);
            AssertExt.AreEqual15(ls.EndPoint.X, lscheck.EndPoint.X);
            AssertExt.AreEqual15(ls.EndPoint.Y, lscheck.EndPoint.Y);
        }

        [Test]
        public void LineStringCoordiantesCount()
        {
            Coordinate[] coords = new Coordinate[36];
            Random rnd = new Random();
            GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[36];
            for (int i = 0; i < 36; i++)
            {
                coords[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                double x = coords[i].X;
                double y = coords[i].Y;
                GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILineString lscheck = gf.CreateLineString(coordscheck);
            LineString ls = new LineString(coords);
            Assert.AreEqual(ls.Coordinates.Count, lscheck.Coordinates.Count());
        }

        [Test]
        public void LineStringCentroid()
        {
            Coordinate[] coords = new Coordinate[36];
            Random rnd = new Random();
            GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[36];
            for (int i = 0; i < 36; i++)
            {
                coords[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                double x = coords[i].X;
                double y = coords[i].Y;
                GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILineString lscheck = gf.CreateLineString(coordscheck);
            LineString ls = new LineString(coords);
            AssertExt.AreEqual15(ls.Centroid.X, lscheck.Centroid.X);
            AssertExt.AreEqual15(ls.Centroid.Y, lscheck.Centroid.Y);
        }

    }
}
