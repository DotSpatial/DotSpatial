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
            var coords = new Coordinate[36];
            var rnd = new Random();
            var coordscheck = new GeoAPI.Geometries.ICoordinate[36];
            for (var i = 0; i < 36; i++)
            {
                coords[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            var gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            var lscheck = gf.CreateLineString(coordscheck);
            var ls = new LineString(coords);
            var length = ls.Length;
            var lengthcheck = lscheck.Length;
            Assert.AreEqual(length, lengthcheck);
        }

        [Test]
        public void PolygonArea()
        {
            var coords = new Coordinate[20];
            var rnd = new Random();
            var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            var coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (var i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coordscheck[0].X, coordscheck[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            var gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            var ring = gf.CreateLinearRing(coordscheck);
            var polygonCheck = gf.CreatePolygon(ring, null);
            var pg = new Polygon(coords);
            var areaCheck = polygonCheck.Area;
            var area = pg.Area;
            Assert.AreEqual(area, areaCheck);
        }
        [Test]
        public void Buffer()
        {
            var rnd = new Random();
            var coords = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            var x = coords.X;
            var y = coords.Y;
            var c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
            //coordscheck[i] = c;
            var p = new Point(coords);
            var gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            var ps = gf.CreatePoint(c);
            var area = p.Buffer(500).Area;
            var areacheck = ps.Buffer(500).Area;
            Assert.IsTrue(Math.Abs(area - areacheck) < 1e-6);
        }

        [Test]
        public void PolygonHoles()
        {
            var coords = new Coordinate[20];
            var rnd = new Random();
            var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);

            // Shell Coordinates
            var coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (var i = 0; i < 19; i++)
            {
                var x = center.X + Math.Cos((i * 10) * Math.PI / 10);
                var y = center.Y + (i * 10) * Math.PI / 10;
                coords[i] = new Coordinate(x, y);
                coordscheck[i] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);


            // Shell Rings
            var ring = new LinearRing(coords);
            var gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            var ringCheck = gf.CreateLinearRing(coordscheck);


            // Hole Coordinates
            var coordsholecheck = new GeoAPI.Geometries.ICoordinate[20];
            var coordshole = new Coordinate[20];
            for (var i = 0; i < 20; i++)
            {
                var x = center.X + Math.Cos((i * 10) * Math.PI / 20);
                var y = center.Y + (i * 10) * Math.PI / 20;
                coordshole[i] = new Coordinate(x, y);
                coordsholecheck[i] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
            }
            coordshole[19] = new Coordinate(coordshole[0].X, coordshole[0].Y);
            coordsholecheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coordshole[0].X, coordshole[0].Y);

            // Hole LinearRing Arrays
            var hole = new LinearRing(coordshole);
            var holes = new ILinearRing[1];
            var holeCheck = gf.CreateLinearRing(coordsholecheck);
            var holescheck = new GeoAPI.Geometries.ILinearRing[1];
            holes[0] = hole;
            holescheck[0] = holeCheck;


            var pg = new Polygon(ring, holes);
            var polygonCheck = gf.CreatePolygon(ringCheck, holescheck);
            var areaCheck = polygonCheck.Area;
            var area = pg.Area;
            Assert.AreEqual(area, areaCheck);
        }


        [Test]
        public void BufferLength()
        {
            var rnd = new Random();
            var coords = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            var x = coords.X;
            var y = coords.Y;
            var c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
            //coordscheck[i] = c;
            var p = new Point(coords);
            var gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            var ps = gf.CreatePoint(c);
            var boundary = p.Buffer(500).Length;
            var boundarycheck = ps.Buffer(500).Length;
            AssertExt.AreEqual15(boundary, boundarycheck);
        }

        [Test]
        public void PolgygonParimeterLength()
        {
            var coords = new Coordinate[20];
            var rnd = new Random();
            var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            var coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (var i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            var gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            var ring = gf.CreateLinearRing(coordscheck);
            var polygonCheck = gf.CreatePolygon(ring, null);
            var pg = new Polygon(coords);
            var lengthCheck = polygonCheck.Length;
            var length = pg.Length;
            Assert.AreEqual(length, lengthCheck);

        }

        [Test]
        public void PolygonBoundaryCentroid()
        {
            var coords = new Coordinate[20];
            var rnd = new Random();
            var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            var coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (var i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            var gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            var ring = gf.CreateLinearRing(coordscheck);
            var polygonCheck = gf.CreatePolygon(ring, null);
            var pg = new Polygon(coords);
            var x1 = pg.Boundary.Centroid.X;
            var x2 = polygonCheck.Boundary.Centroid.X;
            AssertExt.AreEqual15(x1, x2);
            var y1 = pg.Boundary.Centroid.Y;
            var y2 = polygonCheck.Boundary.Centroid.Y;
            AssertExt.AreEqual15(y1, y2);
        }

        [Test]
        public void PolygonBoundaryArea()
        {
            var coords = new Coordinate[20];
            var rnd = new Random();
            var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            var coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (var i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            var gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            var ring = gf.CreateLinearRing(coordscheck);
            var polygonCheck = gf.CreatePolygon(ring, null);
            var pg = new Polygon(coords);
            AssertExt.AreEqual15(pg.Boundary.Area, polygonCheck.Boundary.Area);
        }

        [Test]
        public void PolygonCentroid()
        {
            var coords = new Coordinate[20];
            var rnd = new Random();
            var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            var coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (var i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            var gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            var ring = gf.CreateLinearRing(coordscheck);
            var polygonCheck = gf.CreatePolygon(ring, null);
            var pg = new Polygon(coords);
            var x1 = pg.Centroid.X;
            var x2 = polygonCheck.Centroid.X;
            AssertExt.AreEqual15(x1, x2);
            var y1 = pg.Centroid.Y;
            var y2 = polygonCheck.Centroid.Y;
            AssertExt.AreEqual15(y1, y2);
        }

        [Test]
        public void PolygonNumPoints()
        {
            var coords = new Coordinate[20];
            var rnd = new Random();
            var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            var coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (var i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            var gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            var ring = gf.CreateLinearRing(coordscheck);
            var polygonCheck = gf.CreatePolygon(ring, null);
            var pg = new Polygon(coords);
            Assert.AreEqual(pg.NumPoints, polygonCheck.NumPoints);
        }

        [Test]
        public void PolygonShellGeometryType()
        {
            var coords = new Coordinate[20];
            var rnd = new Random();
            var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            var coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (var i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            var gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            var ring = gf.CreateLinearRing(coordscheck);
            var polygonCheck = gf.CreatePolygon(ring, null);
            var pg = new Polygon(coords);
            Assert.AreEqual(pg.Shell.GeometryType, polygonCheck.Shell.GeometryType);
        }

        [Test]
        public void PolygonShellStartPoint()
        {
            var coords = new Coordinate[20];
            var rnd = new Random();
            var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            var coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (var i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            var gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            var ring = gf.CreateLinearRing(coordscheck);
            var polygonCheck = gf.CreatePolygon(ring, null);
            var pg = new Polygon(coords);
            AssertExt.AreEqual15(pg.Shell.StartPoint.X, polygonCheck.Shell.StartPoint.X);
            AssertExt.AreEqual15(pg.Shell.StartPoint.Y, polygonCheck.Shell.StartPoint.Y);
        }

        [Test]
        public void PolygonShellEndPoint()
        {
            var coords = new Coordinate[20];
            var rnd = new Random();
            var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            var coordscheck = new GeoAPI.Geometries.ICoordinate[20];
            for (var i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            var gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            var ring = gf.CreateLinearRing(coordscheck);
            var polygonCheck = gf.CreatePolygon(ring, null);
            var pg = new Polygon(coords);
            AssertExt.AreEqual15(pg.Shell.EndPoint.X, polygonCheck.Shell.EndPoint.X);
            AssertExt.AreEqual15(pg.Shell.EndPoint.Y, polygonCheck.Shell.EndPoint.Y);
        }

        [Test]
        public void LineStartPoint()
        {
            var coords = new Coordinate[36];
            var rnd = new Random();
            var coordscheck = new GeoAPI.Geometries.ICoordinate[36];
            for (var i = 0; i < 36; i++)
            {
                coords[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            var gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            var lscheck = gf.CreateLineString(coordscheck);
            var ls = new LineString(coords);
            AssertExt.AreEqual15(ls.StartPoint.X, lscheck.StartPoint.X);
            AssertExt.AreEqual15(ls.StartPoint.Y, lscheck.StartPoint.Y);
        }

        [Test]
        public void LineEndPoint()
        {
            var coords = new Coordinate[36];
            var rnd = new Random();
            var coordscheck = new GeoAPI.Geometries.ICoordinate[36];
            for (var i = 0; i < 36; i++)
            {
                coords[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            var gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            var lscheck = gf.CreateLineString(coordscheck);
            var ls = new LineString(coords);
            AssertExt.AreEqual15(ls.EndPoint.X, lscheck.EndPoint.X);
            AssertExt.AreEqual15(ls.EndPoint.Y, lscheck.EndPoint.Y);
        }

        [Test]
        public void LineStringCoordiantesCount()
        {
            var coords = new Coordinate[36];
            var rnd = new Random();
            var coordscheck = new GeoAPI.Geometries.ICoordinate[36];
            for (var i = 0; i < 36; i++)
            {
                coords[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            var gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            var lscheck = gf.CreateLineString(coordscheck);
            var ls = new LineString(coords);
            Assert.AreEqual(ls.Coordinates.Count, lscheck.Coordinates.Count());
        }

        [Test]
        public void LineStringCentroid()
        {
            var coords = new Coordinate[36];
            var rnd = new Random();
            var coordscheck = new GeoAPI.Geometries.ICoordinate[36];
            for (var i = 0; i < 36; i++)
            {
                coords[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            var gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            var lscheck = gf.CreateLineString(coordscheck);
            var ls = new LineString(coords);
            AssertExt.AreEqual15(ls.Centroid.X, lscheck.Centroid.X);
            AssertExt.AreEqual15(ls.Centroid.Y, lscheck.Centroid.Y);
        }

    }
}
