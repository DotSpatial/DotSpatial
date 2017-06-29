using System;
using DotSpatial.Topology.Utilities;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;


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
            Random rnd = new Random();
            Coordinate c = new Coordinate((rnd.NextDouble() + 360) - 180, (rnd.NextDouble() * 180) - 90);
            Point p = new Point(c);
            byte[] vals = p.ToBinary();
            WkbReader wkr = new WkbReader();
            IGeometry g = wkr.Read(vals);
            Point pt = g as Point;
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
            Coordinate[] coords = new Coordinate[36];
            Random rnd = new Random();
            for (int i = 0; i < 36; i++)
            {
                coords[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            }
            LineString ls = new LineString(coords);
            byte[] vals = ls.ToBinary();
            WkbReader wkr = new WkbReader();
            IGeometry g = wkr.Read(vals);
            LineString lscheck = g as LineString;
            if (lscheck != null)
            {
                for (int i = 0; i < ls.Count; i++)
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
            Coordinate[] coords = new Coordinate[20];
            Random rnd = new Random();
            Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            for (int i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
            }
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            Polygon pg = new Polygon(coords);
            byte[] vals = pg.ToBinary();
            WkbReader wkr = new WkbReader();
            IGeometry g = wkr.Read(vals);
            Polygon pgcheck = g as Polygon;
            if (pgcheck != null)
            {
                for (int i = 0; i < pg.NumGeometries; i++)
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
            Random rnd = new Random();
            Polygon[] pg = new Polygon[50];
            for (int i = 0; i < 50; i++)
            {
                Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                Coordinate[] coord = new Coordinate[36];
                for (int ii = 0; ii < 36; ii++)
                {
                    coord[ii] = new Coordinate(center.X + Math.Cos((ii * 10) * Math.PI / 10), center.Y + (ii * 10) * Math.PI / 10);
                }
                coord[35] = new Coordinate(coord[0].X, coord[0].Y);
                pg[i] = new Polygon(coord);
            }
            MultiPolygon mpg = new MultiPolygon(pg);
            Byte[] vals = mpg.ToBinary();
            WkbReader wkr = new WkbReader();
            IGeometry g = wkr.Read(vals);
            MultiPolygon mpgcheck = g as MultiPolygon;
            if (mpgcheck != null)
            {
                for (int ii = 0; ii < mpg.Coordinates.Count; ii++)
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
            Random rnd = new Random();
            LineString[] ls = new LineString[40];
            for (int ii = 0; ii < 40; ii++)
            {
                Coordinate[] coord = new Coordinate[36];
                for (int i = 0; i < 36; i++)
                {
                    coord[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                }
                ls[ii] = new LineString(coord);
            }
            MultiLineString mls = new MultiLineString(ls);
            Byte[] vals = mls.ToBinary();
            WkbReader wkr = new WkbReader();
            IGeometry g = wkr.Read(vals);
            MultiLineString mlscheck = g as MultiLineString;
            if (mlscheck != null)
            {
                for (int ii = 0; ii < mls.Coordinates.Count; ii++)
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
            Coordinate[] c = new Coordinate[36];
            Random rnd = new Random();
            for (int i = 0; i < 36; i++)
            {
                c[i] = new Coordinate((rnd.NextDouble() + 360) - 180, (rnd.NextDouble() * 180) - 90);
            }
            MultiPoint mps = new MultiPoint(c);
            byte[] vals = mps.ToBinary();
            WkbReader wkb = new WkbReader();
            IGeometry g = wkb.Read(vals);
            MultiPoint mpsCheck = g as MultiPoint;
            if (mpsCheck != null)
            {
                for (int ii = 0; ii < mps.Coordinates.Count; ii++)
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

            Coordinate[] c = new Coordinate[36];
            Random rnd = new Random();
            for (int i = 0; i < 36; i++)
            {
                c[i] = new Coordinate((rnd.NextDouble() + 360) - 180, (rnd.NextDouble() * 180) - 90);
            }
            MultiPoint mps = new MultiPoint(c);
            byte[] vals = mps.ToBinary();
            WkbReader wkb = new WkbReader();
            IGeometry g = wkb.Read(vals);
            MultiPoint mpsCheck = g as MultiPoint;
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
