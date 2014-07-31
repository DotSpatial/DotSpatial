using System;
using NUnit.Framework;


namespace DotSpatial.Topology.Tests
{
    
    [TestFixture]
    public class OverLayUnitTests
    {
        [Test]
        public void Intersection()
        {
            var rnd = new Random();
            var pg = new Polygon[50];
            var pgcheck = new GeoAPI.Geometries.IPolygon[50];
            var gf = new NetTopologySuite.Geometries.GeometryFactory();
            var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);

            for (var i = 0; i < 50; i++)
            {

                var coord = new Coordinate[36];
                var coordscheck = new GeoAPI.Geometries.Coordinate[36];
                for (var ii = 0; ii < 36; ii++)
                {
                    coord[ii] = new Coordinate(center.X + Math.Cos((ii * 10) * Math.PI / 10), center.Y + (ii * 10) * Math.PI / 10);
                    var x = coord[ii].X;
                    var y = coord[ii].Y;
                    var c = new GeoAPI.Geometries.Coordinate(x, y);
                    coordscheck[ii] = c;
                }
                coord[35] = new Coordinate(coord[0].X, coord[0].Y);
                coordscheck[35] = new GeoAPI.Geometries.Coordinate(coordscheck[0].X, coordscheck[0].Y);
                GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
                pgcheck[i] = gf.CreatePolygon(ring, null);
                pg[i] = new Polygon(coord);

            }
            for (var t = 0; t < 49; t++)
            {
                var g = pg[t].Intersection(pg[t + 1]);
                var gcheck = pgcheck[t].Intersection(pgcheck[t + 1]);
                for (var j = 0; j < g.Coordinates.Count; j++)
                {
                    Assert.AreEqual(g.Coordinates[j].X, gcheck.Coordinates[j].X);
                    Assert.AreEqual(g.Coordinates[j].Y, gcheck.Coordinates[j].Y);
                }
            }
        }

        [Test]
        public void Overlaps()
        {
            var rnd = new Random();
            var pg = new Polygon[50];
            var pgcheck = new GeoAPI.Geometries.IPolygon[50];
            var gf = new NetTopologySuite.Geometries.GeometryFactory();
            var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);

            for (var i = 0; i < 50; i++)
            {

                var coord = new Coordinate[36];
                var coordscheck = new GeoAPI.Geometries.Coordinate[36];
                for (var ii = 0; ii < 36; ii++)
                {
                    coord[ii] = new Coordinate(center.X + Math.Cos((ii * 10) * Math.PI / 10), center.Y + (ii * 10) * Math.PI / 10);
                    var x = coord[ii].X;
                    var y = coord[ii].Y;
                    var c = new GeoAPI.Geometries.Coordinate(x, y);
                    coordscheck[ii] = c;
                }
                coord[35] = new Coordinate(coord[0].X, coord[0].Y);
                coordscheck[35] = new GeoAPI.Geometries.Coordinate(coordscheck[0].X, coordscheck[0].Y);
                GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
                pgcheck[i] = gf.CreatePolygon(ring, null);
                pg[i] = new Polygon(coord);

            }
            for (var t = 0; t < 49; t++)
            {
                var g = pg[t].Overlaps(pg[t + 1]);
                var gcheck = pgcheck[t].Overlaps(pgcheck[t + 1]);
                Assert.AreEqual(g, gcheck);

            }
        }
    }
}
