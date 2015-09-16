using System;
using NUnit.Framework;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Tests
{
    [TestFixture]
    public class MultiShape
    {
        [Test]
        public void Multils()
        {
            var rnd = new Random();
            var ls = new LineString[40];
            var lscheck = new GeoAPI.Geometries.ILineString[40];
            var gf = new NetTopologySuite.Geometries.GeometryFactory();
            for (var ii = 0; ii < 40; ii++)
            {
                var coord = new Coordinate[36];
                var coordcheck = new GeoAPI.Geometries.Coordinate[36];
                for (var i = 0; i < 36; i++)
                {
                    coord[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                    var x = coord[i].X;
                    var y = coord[i].Y;
                    var c = new GeoAPI.Geometries.Coordinate(x, y);
                    coordcheck[i] = c;
                }
                ls[ii] = new LineString(coord);
                lscheck[ii] = gf.CreateLineString(coordcheck);
            }
            var mls = new MultiLineString(ls);
            var mlscheck = gf.CreateMultiLineString(lscheck);
            for (var ii = 0; ii < mls.Coordinates.Count; ii++)
            {
                Assert.AreEqual(mls.Coordinates[ii].X, mlscheck.Coordinates[ii].X);
                Assert.AreEqual(mls.Coordinates[ii].Y, mlscheck.Coordinates[ii].Y);
            }
            Assert.AreEqual(mls.NumGeometries, mlscheck.NumGeometries);
        }

        [Test]
        public void MultiPs()
        {
            var c = new Coordinate[36];
            var rnd = new Random();
            var ccheck = new GeoAPI.Geometries.Coordinate[36];
            var gf = new NetTopologySuite.Geometries.GeometryFactory();
            for (var i = 0; i < 36; i++)
            {
                c[i] = new Coordinate((rnd.NextDouble() + 360) - 180, (rnd.NextDouble() * 180) - 90);
                var x = c[i].X;
                var y = c[i].Y;
                var ctemp = new GeoAPI.Geometries.Coordinate(x, y);
                ccheck[i] = ctemp;
            }
            GeoAPI.Geometries.IMultiPoint mpsCheck = gf.CreateMultiPoint(ccheck);
            var mps = new MultiPoint(c);
            for (var ii = 0; ii < mps.Coordinates.Count; ii++)
            {
                Assert.AreEqual(mps.Coordinates[ii].X, mpsCheck.Coordinates[ii].X);
                Assert.AreEqual(mps.Coordinates[ii].Y, mpsCheck.Coordinates[ii].Y);
            }
        }

        [Test]
        public void Multipg()
        {
            var rnd = new Random();
            var pg = new Polygon[50];
            var pgcheck = new GeoAPI.Geometries.IPolygon[50];
            var gf = new NetTopologySuite.Geometries.GeometryFactory();
            for (var i = 0; i < 50; i++)
            {
                var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
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
                var ring = gf.CreateLinearRing(coordscheck);
                pgcheck[i] = gf.CreatePolygon(ring, null);
                pg[i] = new Polygon(coord);

            }
            var mpg = new MultiPolygon(pg);
            var mpgcheck = gf.CreateMultiPolygon(pgcheck);
            for (var ii = 0; ii < mpg.Coordinates.Count; ii++)
            {
                Assert.AreEqual(mpg.Coordinates[ii].X, mpgcheck.Coordinates[ii].X);
                Assert.AreEqual(mpg.Coordinates[ii].Y, mpgcheck.Coordinates[ii].Y);
            }
        }


        [Test]
        public void EnvelopeMpg()
        {
            var rnd = new Random();
            var pg = new Polygon[50];
            var pgcheck = new GeoAPI.Geometries.IPolygon[50];
            var gf = new NetTopologySuite.Geometries.GeometryFactory();
            for (var i = 0; i < 50; i++)
            {
                var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
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
            var mpg = new MultiPolygon(pg);
            var mpgcheck = gf.CreateMultiPolygon(pgcheck);
            Assert.AreEqual(mpg.Envelope.Width, mpgcheck.EnvelopeInternal.Width);
            Assert.AreEqual(mpg.Envelope.Height, mpgcheck.EnvelopeInternal.Height);
        }

        [Test]
        public void EnveloptMls()
        {
            var rnd = new Random();
            var ls = new LineString[40];
            var lscheck = new GeoAPI.Geometries.ILineString[40];
            var gf = new NetTopologySuite.Geometries.GeometryFactory();
            for (var ii = 0; ii < 40; ii++)
            {
                var coord = new Coordinate[36];
                var coordcheck = new GeoAPI.Geometries.Coordinate[36];
                for (var i = 0; i < 36; i++)
                {
                    coord[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                    var x = coord[i].X;
                    var y = coord[i].Y;
                    var c = new GeoAPI.Geometries.Coordinate(x, y);
                    coordcheck[i] = c;
                }
                ls[ii] = new LineString(coord);
                lscheck[ii] = gf.CreateLineString(coordcheck);
            }
            var mls = new MultiLineString(ls);
            var mlscheck = gf.CreateMultiLineString(lscheck);
            Assert.AreEqual(mls.Envelope.Width, mlscheck.EnvelopeInternal.Width);
            Assert.AreEqual(mls.Envelope.Height, mlscheck.EnvelopeInternal.Height);
        }

        [Test]
        public void MpgEnvelopMinMax()
        {
            var rnd = new Random();
            var pg = new Polygon[50];
            var pgcheck = new GeoAPI.Geometries.IPolygon[50];
            var gf = new NetTopologySuite.Geometries.GeometryFactory();
            for (var i = 0; i < 50; i++)
            {
                var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
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
            var mpg = new MultiPolygon(pg);
            var mpgcheck = gf.CreateMultiPolygon(pgcheck);
            Assert.AreEqual(mpg.EnvelopeInternal.Maximum.X, mpgcheck.EnvelopeInternal.MaxX);
            Assert.AreEqual(mpg.EnvelopeInternal.Maximum.Y, mpgcheck.EnvelopeInternal.MaxY);
            Assert.AreEqual(mpg.EnvelopeInternal.Minimum.X, mpgcheck.EnvelopeInternal.MinX);
            Assert.AreEqual(mpg.EnvelopeInternal.Minimum.Y, mpgcheck.EnvelopeInternal.MinY);
        }

        [Test]
        public void EnvelopeHeight()
        {
            var rnd = new Random();
            var pg = new Polygon[50];
            var pgcheck = new GeoAPI.Geometries.IPolygon[50];
            var gf = new NetTopologySuite.Geometries.GeometryFactory();
            for (var i = 0; i < 50; i++)
            {
                var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
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
            var mpg = new MultiPolygon(pg);
            var mpgcheck = gf.CreateMultiPolygon(pgcheck);
            Assert.AreEqual(mpg.Envelope.Height, mpgcheck.EnvelopeInternal.Height);
        }

        [Test]
        public void MlsLength()
        {
            var rnd = new Random();
            var ls = new LineString[40];
            var lscheck = new GeoAPI.Geometries.ILineString[40];
            var gf = new NetTopologySuite.Geometries.GeometryFactory();
            for (var ii = 0; ii < 40; ii++)
            {
                var coord = new Coordinate[36];
                var coordcheck = new GeoAPI.Geometries.Coordinate[36];
                for (var i = 0; i < 36; i++)
                {
                    coord[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                    var x = coord[i].X;
                    var y = coord[i].Y;
                    var c = new GeoAPI.Geometries.Coordinate(x, y);
                    coordcheck[i] = c;
                }
                ls[ii] = new LineString(coord);
                lscheck[ii] = gf.CreateLineString(coordcheck);
            }
            var mls = new MultiLineString(ls);
            var mlscheck = gf.CreateMultiLineString(lscheck);
            Assert.AreEqual(mls.Length, mlscheck.Length);
        }

        [Test]
        public void MpsBufferArea()
        {
            var c = new Coordinate[36];
            var rnd = new Random();
            var ccheck = new GeoAPI.Geometries.Coordinate[36];
            var gf = new NetTopologySuite.Geometries.GeometryFactory();
            for (var i = 0; i < 36; i++)
            {
                c[i] = new Coordinate((rnd.NextDouble() + 360) - 180, (rnd.NextDouble() * 180) - 90);
                var x = c[i].X;
                var y = c[i].Y;
                var ctemp = new GeoAPI.Geometries.Coordinate(x, y);
                ccheck[i] = ctemp;
            }
            GeoAPI.Geometries.IMultiPoint mpsCheck = gf.CreateMultiPoint(ccheck);
            var mps = new MultiPoint(c);
            var area = mps.Buffer(500).Area;
            var areacheck = mpsCheck.Buffer(500).Area;
            Assert.IsTrue(Math.Abs(area - areacheck) < 1e-6);
        }

        [Test]
        public void IntersectInternalTest()
        {
            var c = new Coordinate[50];
            var rnd = new Random();
            var cc = new Coordinate[50];
            for (var i = 0; i < 50; i++)
            {
                c[i] = new Coordinate((rnd.NextDouble() + 360) - 180, (rnd.NextDouble() * 180) - 90);
                var x = c[i].X;
                var y = c[i].Y;
                cc[i] = new Coordinate(x, y);
            }
            var mps = new MultiPoint(cc);
            var mpsCheck = new MultiPoint(c);
            var intersects = mpsCheck.Intersects(mps);
            if (intersects != true)
            {
                Assert.Fail("The two multipoint features did not intersect.");
            }

        }
    }
}
