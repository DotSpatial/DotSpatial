using System;
using System.Diagnostics;
using NUnit.Framework;

namespace DotSpatial.Topology.Tests
{
    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class EnvelopeUnitTests
    {

        [Test]
        public void EnvelopeSizeCheck()
        {
            double w = 0;
            // Measure starting point memory use
            long start = GC.GetTotalMemory(true);

            // Allocate a new array of 100000 Extent classes.
            // If methods don't count, should be about 3,200,000 bytes.
            Envelope[] memhog = new Envelope[100000];

            for (int i = 0; i < 100000; i++)
            {
                memhog[i] = new Envelope(new Coordinate(0, 0, 0), new Coordinate(1, 1, 1));
            }

            // Obtain measurements after creating the new byte[]
            long end = GC.GetTotalMemory(true);
            Debug.WriteLine("width: " + w);
            long size = (end - start) / 100000;

            // Ensure that the Array stays in memory and doesn't get optimized away
            Debug.WriteLine("Memory size of Extent = " + size);
            // Size of Envelope = 104

        }


        [Test]
        public void EnvelopeCoordinate()
        {
            Random rnd = new Random();
            Coordinate c = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            Envelope ev = new Envelope(c);
            double x = c.X;
            double y = c.Y;
            GisSharpBlog.NetTopologySuite.Geometries.Coordinate ccheck = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
            GisSharpBlog.NetTopologySuite.Geometries.Envelope evcheck = new GisSharpBlog.NetTopologySuite.Geometries.Envelope(ccheck);
            AssertExt.AreEqual15(ev.Maximum.Y, evcheck.MaxY);
            AssertExt.AreEqual15(ev.Maximum.X, evcheck.MaxX);
            AssertExt.AreEqual15(ev.Minimum.Y, evcheck.MinY);
            AssertExt.AreEqual15(ev.Minimum.X, evcheck.MinX);
        }

        [Test]
        public void TwoCoordinates()
        {
            Random rnd = new Random();
            Coordinate c1 = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            Coordinate c2 = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            Envelope ev = new Envelope(c1, c2);
            double x1 = c1.X;
            double y1 = c1.Y;
            double x2 = c2.X;
            double y2 = c2.Y;
            GisSharpBlog.NetTopologySuite.Geometries.Coordinate c1Check = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x1, y1);
            GisSharpBlog.NetTopologySuite.Geometries.Coordinate c2Check = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x2, y2);
            GisSharpBlog.NetTopologySuite.Geometries.Envelope evcheck = new GisSharpBlog.NetTopologySuite.Geometries.Envelope(c1Check, c2Check);
            AssertExt.AreEqual15(ev.Maximum.Y, evcheck.MaxY);
            AssertExt.AreEqual15(ev.Maximum.X, evcheck.MaxX);
            AssertExt.AreEqual15(ev.Minimum.Y, evcheck.MinY);
            AssertExt.AreEqual15(ev.Minimum.X, evcheck.MinX);

        }

        [Test]
        public void Center()
        {
            Random rnd = new Random();
            Coordinate c1 = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            Coordinate c2 = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            Envelope ev = new Envelope(c1, c2);
            double x1 = c1.X;
            double y1 = c1.Y;
            double x2 = c2.X;
            double y2 = c2.Y;
            GisSharpBlog.NetTopologySuite.Geometries.Coordinate c1Check = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x1, y1);
            GisSharpBlog.NetTopologySuite.Geometries.Coordinate c2Check = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x2, y2);
            GisSharpBlog.NetTopologySuite.Geometries.Envelope evcheck = new GisSharpBlog.NetTopologySuite.Geometries.Envelope(c1Check, c2Check);
            Coordinate center = new Coordinate(ev.Center());
            GisSharpBlog.NetTopologySuite.Geometries.Coordinate centercheck = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(evcheck.Centre);
            AssertExt.AreEqual15(center.X, centercheck.X);
            AssertExt.AreEqual15(center.Y, centercheck.Y);
        }

        [Test]
        public void HeightWidth()
        {
            Random rnd = new Random();
            double x1 = ((rnd.NextDouble() * 360) - 180);
            double x2 = ((rnd.NextDouble() * 360) - 180);
            double y1 = ((rnd.NextDouble() * 360) - 180);
            double y2 = ((rnd.NextDouble() * 360) - 180);
            Envelope ev = new Envelope(x1, x2, y1, y2);
            GisSharpBlog.NetTopologySuite.Geometries.Envelope evcheck = new GisSharpBlog.NetTopologySuite.Geometries.Envelope(x1, x2, y1, y2);
            AssertExt.AreEqual15(ev.Height, evcheck.Height);
            AssertExt.AreEqual15(ev.Width, evcheck.Width);
        }

        [Test]
        public void LineStringEnvelopeMaxMin()
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
            AssertExt.AreEqual15(ls.Envelope.Maximum.X, lscheck.EnvelopeInternal.MaxX);
            AssertExt.AreEqual15(ls.Envelope.Maximum.Y, lscheck.EnvelopeInternal.MaxY);
            AssertExt.AreEqual15(ls.Envelope.Minimum.X, lscheck.EnvelopeInternal.MinX);
            AssertExt.AreEqual15(ls.Envelope.Minimum.Y, lscheck.EnvelopeInternal.MinY);
        }

        [Test]
        public void LineStringEnvelopeHeightWidth()
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
            AssertExt.AreEqual15(ls.Envelope.Width, lscheck.EnvelopeInternal.Width);
            AssertExt.AreEqual15(ls.Envelope.Height, lscheck.EnvelopeInternal.Height);

        }

        [Test]
        public void LineStringEnvelopeArea()
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
            double area = ls.Envelope.Area();
            double areacheck = lscheck.Envelope.Area;
            AssertExt.AreEqual15(area, areacheck);
        }

        [Test]
        public void PolygonEnvelopeMaxMin()
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
            GeoAPI.Geometries.IPolygon pgcheck = gf.CreatePolygon(ring, null);
            Polygon pg = new Polygon(coords);
            AssertExt.AreEqual15(pg.Envelope.Maximum.X, pgcheck.EnvelopeInternal.MaxX);
            AssertExt.AreEqual15(pg.Envelope.Maximum.Y, pgcheck.EnvelopeInternal.MaxY);
            AssertExt.AreEqual15(pg.Envelope.Minimum.X, pgcheck.EnvelopeInternal.MinX);
            AssertExt.AreEqual15(pg.Envelope.Minimum.Y, pgcheck.EnvelopeInternal.MinY);
        }

        [Test]
        public void PolygonEnvelopeArea()
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
            GeoAPI.Geometries.IPolygon pgcheck = gf.CreatePolygon(ring, null);
            Polygon pg = new Polygon(coords);
            Double area = pg.Envelope.Area();
            AssertExt.AreEqual15(area, pgcheck.Envelope.Area);


        }


    }

}
