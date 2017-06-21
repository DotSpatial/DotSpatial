using System;
using NUnit.Framework;
using DotSpatial.Topology;



namespace DotSpatial.Topology.Tests
{

    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class MultiShape
    {


        [Test]
        public void Multils()
        {
            Random rnd = new Random();
            LineString[] ls = new LineString[40];
            GeoAPI.Geometries.ILineString[] lscheck = new GeoAPI.Geometries.ILineString[40];
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            for (int ii = 0; ii < 40; ii++)
            {
                Coordinate[] coord = new Coordinate[36];
                GeoAPI.Geometries.ICoordinate[] coordcheck = new GeoAPI.Geometries.ICoordinate[36];
                for (int i = 0; i < 36; i++)
                {
                    coord[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                    double x = coord[i].X;
                    double y = coord[i].Y;
                    GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                    coordcheck[i] = c;
                }
                ls[ii] = new LineString(coord);
                lscheck[ii] = gf.CreateLineString(coordcheck);
            }
            MultiLineString mls = new MultiLineString(ls);
            GeoAPI.Geometries.IMultiLineString mlscheck = gf.CreateMultiLineString(lscheck);
            for (int ii = 0; ii < mls.Coordinates.Count; ii++)
            {
                Assert.AreEqual(mls.Coordinates[ii].X, mlscheck.Coordinates[ii].X);
                Assert.AreEqual(mls.Coordinates[ii].Y, mlscheck.Coordinates[ii].Y);
            }
            Assert.AreEqual(mls.NumGeometries, mlscheck.NumGeometries);
        }

        [Test]
        public void MultiPs()
        {
            Coordinate[] c = new Coordinate[36];
            Random rnd = new Random();
            GeoAPI.Geometries.ICoordinate[] ccheck = new GeoAPI.Geometries.ICoordinate[36];
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            for (int i = 0; i < 36; i++)
            {
                c[i] = new Coordinate((rnd.NextDouble() + 360) - 180, (rnd.NextDouble() * 180) - 90);
                double x = c[i].X;
                double y = c[i].Y;
                GisSharpBlog.NetTopologySuite.Geometries.Coordinate ctemp = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                ccheck[i] = ctemp;
            }
            GeoAPI.Geometries.IMultiPoint mpsCheck = gf.CreateMultiPoint(ccheck);
            MultiPoint mps = new MultiPoint(c);
            for (int ii = 0; ii < mps.Coordinates.Count; ii++)
            {
                Assert.AreEqual(mps.Coordinates[ii].X, mpsCheck.Coordinates[ii].X);
                Assert.AreEqual(mps.Coordinates[ii].Y, mpsCheck.Coordinates[ii].Y);
            }
        }

        [Test]
        public void Multipg()
        {
            Random rnd = new Random();
            Polygon[] pg = new Polygon[50];
            GeoAPI.Geometries.IPolygon[] pgcheck = new GeoAPI.Geometries.IPolygon[50];
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            for (int i = 0; i < 50; i++)
            {
                Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                Coordinate[] coord = new Coordinate[36];
                GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[36];
                for (int ii = 0; ii < 36; ii++)
                {
                    coord[ii] = new Coordinate(center.X + Math.Cos((ii * 10) * Math.PI / 10), center.Y + (ii * 10) * Math.PI / 10);
                    double x = coord[ii].X;
                    double y = coord[ii].Y;
                    GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                    coordscheck[ii] = c;
                }
                coord[35] = new Coordinate(coord[0].X, coord[0].Y);
                coordscheck[35] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coordscheck[0].X, coordscheck[0].Y);
                GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
                pgcheck[i] = gf.CreatePolygon(ring, null);
                pg[i] = new Polygon(coord);

            }
            MultiPolygon mpg = new MultiPolygon(pg);
            GeoAPI.Geometries.IMultiPolygon mpgcheck = gf.CreateMultiPolygon(pgcheck);
            for (int ii = 0; ii < mpg.Coordinates.Count; ii++)
            {
                Assert.AreEqual(mpg.Coordinates[ii].X, mpgcheck.Coordinates[ii].X);
                Assert.AreEqual(mpg.Coordinates[ii].Y, mpgcheck.Coordinates[ii].Y);
            }
        }


        //[Test]
        //public void EnvelopeMps()
        //{
        //    Coordinate[] c = new Coordinate[36];
        //    Random rnd = new Random();
        //    GeoAPI.Geometries.ICoordinate[] ccheck = new GeoAPI.Geometries.ICoordinate[36];
        //    GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
        //    double y;
        //    for (int i = 0; i < 36; i++)
        //    {
        //        c[i] = new Coordinate((rnd.NextDouble() + 360) - 180, (rnd.NextDouble() * 180) - 90);
        //        double x = c[i].X;
        //        y = c[i].Y;
        //        GisSharpBlog.NetTopologySuite.Geometries.Coordinate ctemp = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
        //        ccheck[i] = ctemp;
        //    }
        //    GeoAPI.Geometries.IMultiPoint mpsCheck = gf.CreateMultiPoint(ccheck);
        //    MultiPoint Mps = new MultiPoint(c);
        //    Assert.AreEqual(Mps.Envelope.Width, mpsCheck.Envelope.);
        //    Assert.AreEqual(Mps.Envelope.Height, mpsCheck.Envelope.Height);

        //}

        [Test]
        public void EnvelopeMpg()
        {
            Random rnd = new Random();
            Polygon[] pg = new Polygon[50];
            GeoAPI.Geometries.IPolygon[] pgcheck = new GeoAPI.Geometries.IPolygon[50];
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            for (int i = 0; i < 50; i++)
            {
                Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                Coordinate[] coord = new Coordinate[36];
                GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[36];
                for (int ii = 0; ii < 36; ii++)
                {
                    coord[ii] = new Coordinate(center.X + Math.Cos((ii * 10) * Math.PI / 10), center.Y + (ii * 10) * Math.PI / 10);
                    double x = coord[ii].X;
                    double y = coord[ii].Y;
                    GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                    coordscheck[ii] = c;
                }
                coord[35] = new Coordinate(coord[0].X, coord[0].Y);
                coordscheck[35] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coordscheck[0].X, coordscheck[0].Y);
                GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
                pgcheck[i] = gf.CreatePolygon(ring, null);
                pg[i] = new Polygon(coord);
            }
            MultiPolygon mpg = new MultiPolygon(pg);
            GeoAPI.Geometries.IMultiPolygon mpgcheck = gf.CreateMultiPolygon(pgcheck);
            Assert.AreEqual(mpg.Envelope.Width, mpgcheck.EnvelopeInternal.Width);
            Assert.AreEqual(mpg.Envelope.Height, mpgcheck.EnvelopeInternal.Height);
        }

        [Test]
        public void EnveloptMls()
        {
            Random rnd = new Random();
            LineString[] ls = new LineString[40];
            GeoAPI.Geometries.ILineString[] lscheck = new GeoAPI.Geometries.ILineString[40];
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            for (int ii = 0; ii < 40; ii++)
            {
                Coordinate[] coord = new Coordinate[36];
                GeoAPI.Geometries.ICoordinate[] coordcheck = new GeoAPI.Geometries.ICoordinate[36];
                for (int i = 0; i < 36; i++)
                {
                    coord[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                    double x = coord[i].X;
                    double y = coord[i].Y;
                    GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                    coordcheck[i] = c;
                }
                ls[ii] = new LineString(coord);
                lscheck[ii] = gf.CreateLineString(coordcheck);
            }
            MultiLineString mls = new MultiLineString(ls);
            GeoAPI.Geometries.IMultiLineString mlscheck = gf.CreateMultiLineString(lscheck);
            Assert.AreEqual(mls.Envelope.Width, mlscheck.EnvelopeInternal.Width);
            Assert.AreEqual(mls.Envelope.Height, mlscheck.EnvelopeInternal.Height);
        }

        [Test]
        public void MpgEnvelopMinMax()
        {
            Random rnd = new Random();
            Polygon[] pg = new Polygon[50];
            GeoAPI.Geometries.IPolygon[] pgcheck = new GeoAPI.Geometries.IPolygon[50];
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            for (int i = 0; i < 50; i++)
            {
                Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                Coordinate[] coord = new Coordinate[36];
                GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[36];
                for (int ii = 0; ii < 36; ii++)
                {
                    coord[ii] = new Coordinate(center.X + Math.Cos((ii * 10) * Math.PI / 10), center.Y + (ii * 10) * Math.PI / 10);
                    double x = coord[ii].X;
                    double y = coord[ii].Y;
                    GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                    coordscheck[ii] = c;
                }
                coord[35] = new Coordinate(coord[0].X, coord[0].Y);
                coordscheck[35] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coordscheck[0].X, coordscheck[0].Y);
                GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
                pgcheck[i] = gf.CreatePolygon(ring, null);
                pg[i] = new Polygon(coord);
            }
            MultiPolygon mpg = new MultiPolygon(pg);
            GeoAPI.Geometries.IMultiPolygon mpgcheck = gf.CreateMultiPolygon(pgcheck);
            Assert.AreEqual(mpg.EnvelopeInternal.Maximum.X, mpgcheck.EnvelopeInternal.MaxX);
            Assert.AreEqual(mpg.EnvelopeInternal.Maximum.Y, mpgcheck.EnvelopeInternal.MaxY);
            Assert.AreEqual(mpg.EnvelopeInternal.Minimum.X, mpgcheck.EnvelopeInternal.MinX);
            Assert.AreEqual(mpg.EnvelopeInternal.Minimum.Y, mpgcheck.EnvelopeInternal.MinY);
        }

        [Test]
        public void EnvelopeHeight()
        {
            Random rnd = new Random();
            Polygon[] pg = new Polygon[50];
            GeoAPI.Geometries.IPolygon[] pgcheck = new GeoAPI.Geometries.IPolygon[50];
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            for (int i = 0; i < 50; i++)
            {
                Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                Coordinate[] coord = new Coordinate[36];
                GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[36];
                for (int ii = 0; ii < 36; ii++)
                {
                    coord[ii] = new Coordinate(center.X + Math.Cos((ii * 10) * Math.PI / 10), center.Y + (ii * 10) * Math.PI / 10);
                    double x = coord[ii].X;
                    double y = coord[ii].Y;
                    GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                    coordscheck[ii] = c;
                }
                coord[35] = new Coordinate(coord[0].X, coord[0].Y);
                coordscheck[35] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coordscheck[0].X, coordscheck[0].Y);
                GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
                pgcheck[i] = gf.CreatePolygon(ring, null);
                pg[i] = new Polygon(coord);
            }
            MultiPolygon mpg = new MultiPolygon(pg);
            GeoAPI.Geometries.IMultiPolygon mpgcheck = gf.CreateMultiPolygon(pgcheck);
            Assert.AreEqual(mpg.Envelope.Height, mpgcheck.EnvelopeInternal.Height);
        }

        [Test]
        public void MlsLength()
        {
            Random rnd = new Random();
            LineString[] ls = new LineString[40];
            GeoAPI.Geometries.ILineString[] lscheck = new GeoAPI.Geometries.ILineString[40];
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            for (int ii = 0; ii < 40; ii++)
            {
                Coordinate[] coord = new Coordinate[36];
                GeoAPI.Geometries.ICoordinate[] coordcheck = new GeoAPI.Geometries.ICoordinate[36];
                for (int i = 0; i < 36; i++)
                {
                    coord[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                    double x = coord[i].X;
                    double y = coord[i].Y;
                    GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                    coordcheck[i] = c;
                }
                ls[ii] = new LineString(coord);
                lscheck[ii] = gf.CreateLineString(coordcheck);
            }
            MultiLineString mls = new MultiLineString(ls);
            GeoAPI.Geometries.IMultiLineString mlscheck = gf.CreateMultiLineString(lscheck);
            Assert.AreEqual(mls.Length, mlscheck.Length);
        }

        [Test]
        public void MpsBufferArea()
        {
            Coordinate[] c = new Coordinate[36];
            Random rnd = new Random();
            GeoAPI.Geometries.ICoordinate[] ccheck = new GeoAPI.Geometries.ICoordinate[36];
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            for (int i = 0; i < 36; i++)
            {
                c[i] = new Coordinate((rnd.NextDouble() + 360) - 180, (rnd.NextDouble() * 180) - 90);
                double x = c[i].X;
                double y = c[i].Y;
                GisSharpBlog.NetTopologySuite.Geometries.Coordinate ctemp = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
                ccheck[i] = ctemp;
            }
            GeoAPI.Geometries.IMultiPoint mpsCheck = gf.CreateMultiPoint(ccheck);
            MultiPoint mps = new MultiPoint(c);
            double area = mps.Buffer(500).Area;
            double areacheck = mpsCheck.Buffer(500).Area;
            if (Math.Abs(area - areacheck) > 0.000000001)
            {
                Assert.AreEqual(mps.Buffer(500).Area, mpsCheck.Buffer(500).Area);
            }


        }

        [Test]
        public void IntersectInternalTest()
        {
            Coordinate[] c = new Coordinate[50];
            Random rnd = new Random();
            Coordinate[] cc = new Coordinate[50];
            for (int i = 0; i < 50; i++)
            {
                c[i] = new Coordinate((rnd.NextDouble() + 360) - 180, (rnd.NextDouble() * 180) - 90);
                double x = c[i].X;
                double y = c[i].Y;
                cc[i] = new Coordinate(x, y);
            }
            MultiPoint mps = new MultiPoint(cc);
            MultiPoint mpsCheck = new MultiPoint(c);
            bool intersects = mpsCheck.Intersects(mps);
            if (intersects != true)
            {
                Assert.Fail("The two multipoint features did not intersect.");
            }

        }



    }
}
