using System;
using NUnit.Framework;
using DotSpatial.Topology.IO;
using Assert = NUnit.Framework.Assert;
using DotSpatial.Topology.Geometries;

namespace DotSpatial.Topology.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class DataTypesTransfromations
    {


        [Test]
        public void PointToByteArray()
        {
            var rnd = new Random();
            var c = new Coordinate((rnd.NextDouble() + 360) - 180, (rnd.NextDouble() * 180) - 90);
            var p = new Point(c);
            var vals = p.ToBinary();
            var wkr = new WKBReader();
            var g = wkr.Read(vals);
            var pt = g as Point;
            if (pt != null)
            {
                Assert.AreEqual(p.X, pt.X);
                Assert.AreEqual(p.Y, pt.Y);
            }
            else
            {
                Assert.Fail("The test failed bc the check pt was null.");
            }
        }

        [Test]
        public void LsToByteArray()
        {
            var coords = new Coordinate[36];
            var rnd = new Random();
            for (var i = 0; i < 36; i++)
            {
                coords[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            }
            var ls = new LineString(coords);
            var vals = ls.ToBinary();
            var wkr = new WKBReader();
            var g = wkr.Read(vals);
            var lscheck = g as LineString;
            if (lscheck != null)
            {
                for (var i = 0; i < ls.Count; i++)
                {
                    Assert.AreEqual(ls.Coordinates[i].X, lscheck.Coordinates[i].X);
                    Assert.AreEqual(ls.Coordinates[i].Y, lscheck.Coordinates[i].Y);
                }
                Assert.AreEqual(ls.Length, lscheck.Length);
                Assert.AreEqual(ls.Envelope.Height, lscheck.Envelope.Height);
                Assert.AreEqual(ls.Envelope.Width, lscheck.Envelope.Width);
            }
            else
            {
                Assert.Fail("The test failed bc the check lscheck was null.");
            }

        }

        [Test]
        public void PolygonToByteArray()
        {
            var coords = new Coordinate[20];
            var rnd = new Random();
            var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            for (var i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
            }
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            var pg = new Polygon(coords);
            var vals = pg.ToBinary();
            var wkr = new WKBReader();
            var g = wkr.Read(vals);
            var pgcheck = g as Polygon;
            if (pgcheck != null)
            {
                for (var i = 0; i < pg.NumGeometries; i++)
                {
                    Assert.AreEqual(pg.Coordinates[i].X, pgcheck.Coordinates[i].X);
                    Assert.AreEqual(pg.Coordinates[i].Y, pgcheck.Coordinates[i].Y);
                }
                Assert.AreEqual(pg.Area, pgcheck.Area);
                Assert.AreEqual(pg.Centroid.X, pgcheck.Centroid.X);
                Assert.AreEqual(pg.Centroid.Y, pgcheck.Centroid.Y);
                Assert.AreEqual(pg.Length, pgcheck.Length);
                Assert.AreEqual(pg.Envelope.Width, pgcheck.Envelope.Width);
                Assert.AreEqual(pg.Envelope.Height, pgcheck.Envelope.Height);
            }
            else
            {
                Assert.Fail("The test failed bc the check pgcheck was null.");
            }

        }

        [Test]
        public void MultipolygonToByteArray()
        {
            var rnd = new Random();
            var pg = new Polygon[50];
            for (var i = 0; i < 50; i++)
            {
                var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                var coord = new Coordinate[36];
                for (var ii = 0; ii < 36; ii++)
                {
                    coord[ii] = new Coordinate(center.X + Math.Cos((ii * 10) * Math.PI / 10), center.Y + (ii * 10) * Math.PI / 10);
                }
                coord[35] = new Coordinate(coord[0].X, coord[0].Y);
                pg[i] = new Polygon(coord);
            }
            var mpg = new MultiPolygon(pg);
            var vals = mpg.ToBinary();
            var wkr = new WKBReader();
            var g = wkr.Read(vals);
            var mpgcheck = g as MultiPolygon;
            if (mpgcheck != null)
            {
                for (var ii = 0; ii < mpg.Coordinates.Count; ii++)
                {
                    Assert.AreEqual(mpg.Coordinates[ii].X, mpgcheck.Coordinates[ii].X);
                    Assert.AreEqual(mpg.Coordinates[ii].Y, mpgcheck.Coordinates[ii].Y);
                }
            }
            else
            {
                Assert.Fail("The test failed bc the check mpgcheck was null.");
            }

        }

        [Test]
        public void MultiLsToByteArray()
        {
            var rnd = new Random();
            var ls = new LineString[40];
            for (var ii = 0; ii < 40; ii++)
            {
                var coord = new Coordinate[36];
                for (var i = 0; i < 36; i++)
                {
                    coord[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                }
                ls[ii] = new LineString(coord);
            }
            var mls = new MultiLineString(ls);
            var vals = mls.ToBinary();
            var wkr = new WKBReader();
            var g = wkr.Read(vals);
            var mlscheck = g as MultiLineString;
            if (mlscheck != null)
            {
                for (var ii = 0; ii < mls.Coordinates.Count; ii++)
                {
                    Assert.AreEqual(mls.Coordinates[ii].X, mlscheck.Coordinates[ii].X);
                    Assert.AreEqual(mls.Coordinates[ii].Y, mlscheck.Coordinates[ii].Y);
                }
            }
            else
            {
                Assert.Fail("The test failed bc the check multilinestring was null.");
            }
        }

        [Test]
        public void MultiPointToByteArray()
        {
            var c = new Coordinate[36];
            var rnd = new Random();
            for (var i = 0; i < 36; i++)
            {
                c[i] = new Coordinate((rnd.NextDouble() + 360) - 180, (rnd.NextDouble() * 180) - 90);
            }
            var mps = new MultiPoint(c);
            var vals = mps.ToBinary();
            var wkb = new WKBReader();
            var g = wkb.Read(vals);
            var mpsCheck = g as MultiPoint;
            if (mpsCheck != null)
            {
                for (var ii = 0; ii < mps.Coordinates.Count; ii++)
                {
                    Assert.AreEqual(mps.Coordinates[ii].X, mpsCheck.Coordinates[ii].X);
                    Assert.AreEqual(mps.Coordinates[ii].Y, mpsCheck.Coordinates[ii].Y);
                }
            }
            else
            {
                Assert.Fail("The test failed because the MpsCheck  was null.");
            }
        }

        [Test]
        public void Buffer()
        {

            var c = new Coordinate[36];
            var rnd = new Random();
            for (var i = 0; i < 36; i++)
            {
                c[i] = new Coordinate((rnd.NextDouble() + 360) - 180, (rnd.NextDouble() * 180) - 90);
            }
            var mps = new MultiPoint(c);
            var vals = mps.ToBinary();
            var wkb = new WKBReader();
            var g = wkb.Read(vals);
            var mpsCheck = g as MultiPoint;
            if (mpsCheck != null)
            {

                Assert.AreEqual(mps.Buffer(200).Area, mpsCheck.Buffer(200).Area);

            }
            else
            {
                Assert.Fail("The test failed because the MpsCheck  was null.");
            }

        }

    }
}
