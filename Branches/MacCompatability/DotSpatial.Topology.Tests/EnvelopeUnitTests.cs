using System;
using System.Diagnostics;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Topology.Tests
{
    [TestFixture]
    public class EnvelopeUnitTests
    {

        [Test]
        public void EnvelopeSizeCheck()
        {
            double w = 0;
            // Measure starting point memory use
            var start = GC.GetTotalMemory(true);

            // Allocate a new array of 100000 Extent classes.
            // If methods don't count, should be about 3,200,000 bytes.
            var memhog = new Envelope[100000];

            for (var i = 0; i < 100000; i++)
            {
                memhog[i] = new Envelope(new Coordinate(0, 0, 0), new Coordinate(1, 1, 1));
            }

            // Obtain measurements after creating the new byte[]
            var end = GC.GetTotalMemory(true);
            Debug.WriteLine("width: " + w);
            var size = (end - start) / 100000;

            // Ensure that the Array stays in memory and doesn't get optimized away
            Debug.WriteLine("Memory size of Extent = " + size);
            // Size of Envelope = 104

        }


        [Test]
        public void EnvelopeCoordinate()
        {
            var rnd = new Random();
            var c = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            var ev = new Envelope(c);
            var x = c.X;
            var y = c.Y;
            var ccheck = new GeoAPI.Geometries.Coordinate(x, y);
            var evcheck = new GeoAPI.Geometries.Envelope(ccheck);
            AssertExt.AreEqual15(ev.Maximum.Y, evcheck.MaxY);
            AssertExt.AreEqual15(ev.Maximum.X, evcheck.MaxX);
            AssertExt.AreEqual15(ev.Minimum.Y, evcheck.MinY);
            AssertExt.AreEqual15(ev.Minimum.X, evcheck.MinX);
        }

        [Test]
        public void TwoCoordinates()
        {
            var rnd = new Random();
            var c1 = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            var c2 = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            var ev = new Envelope(c1, c2);
            var x1 = c1.X;
            var y1 = c1.Y;
            var x2 = c2.X;
            var y2 = c2.Y;
            var c1Check = new GeoAPI.Geometries.Coordinate(x1, y1);
            var c2Check = new GeoAPI.Geometries.Coordinate(x2, y2);
            var evcheck = new GeoAPI.Geometries.Envelope(c1Check, c2Check);
            AssertExt.AreEqual15(ev.Maximum.Y, evcheck.MaxY);
            AssertExt.AreEqual15(ev.Maximum.X, evcheck.MaxX);
            AssertExt.AreEqual15(ev.Minimum.Y, evcheck.MinY);
            AssertExt.AreEqual15(ev.Minimum.X, evcheck.MinX);

        }

        [Test]
        public void Center()
        {
            var rnd = new Random();
            var c1 = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            var c2 = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            var ev = new Envelope(c1, c2);
            var x1 = c1.X;
            var y1 = c1.Y;
            var x2 = c2.X;
            var y2 = c2.Y;
            var c1Check = new GeoAPI.Geometries.Coordinate(x1, y1);
            var c2Check = new GeoAPI.Geometries.Coordinate(x2, y2);
            var evcheck = new GeoAPI.Geometries.Envelope(c1Check, c2Check);
            var center = new Coordinate(ev.Center());
            var centercheck = new GeoAPI.Geometries.Coordinate(evcheck.Centre);
            AssertExt.AreEqual15(center.X, centercheck.X);
            AssertExt.AreEqual15(center.Y, centercheck.Y);
        }

        [Test]
        public void HeightWidth()
        {
            var rnd = new Random();
            var x1 = ((rnd.NextDouble() * 360) - 180);
            var x2 = ((rnd.NextDouble() * 360) - 180);
            var y1 = ((rnd.NextDouble() * 360) - 180);
            var y2 = ((rnd.NextDouble() * 360) - 180);
            var ev = new Envelope(x1, x2, y1, y2);
            var evcheck = new GeoAPI.Geometries.Envelope(x1, x2, y1, y2);
            AssertExt.AreEqual15(ev.Height, evcheck.Height);
            AssertExt.AreEqual15(ev.Width, evcheck.Width);
        }

        [Test]
        public void LineStringEnvelopeMaxMin()
        {
            var coords = new Coordinate[36];
            var rnd = new Random();
            var coordscheck = new GeoAPI.Geometries.Coordinate[36];
            for (var i = 0; i < 36; i++)
            {
                coords[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GeoAPI.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            var gf = new NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILineString lscheck = gf.CreateLineString(coordscheck);
            var ls = new LineString(coords);
            AssertExt.AreEqual15(ls.Envelope.Maximum.X, lscheck.EnvelopeInternal.MaxX);
            AssertExt.AreEqual15(ls.Envelope.Maximum.Y, lscheck.EnvelopeInternal.MaxY);
            AssertExt.AreEqual15(ls.Envelope.Minimum.X, lscheck.EnvelopeInternal.MinX);
            AssertExt.AreEqual15(ls.Envelope.Minimum.Y, lscheck.EnvelopeInternal.MinY);
        }

        [Test]
        public void LineStringEnvelopeHeightWidth()
        {
            var coords = new Coordinate[36];
            var rnd = new Random();
            var coordscheck = new GeoAPI.Geometries.Coordinate[36];
            for (var i = 0; i < 36; i++)
            {
                coords[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GeoAPI.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            var gf = new NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILineString lscheck = gf.CreateLineString(coordscheck);
            var ls = new LineString(coords);
            AssertExt.AreEqual15(ls.Envelope.Width, lscheck.EnvelopeInternal.Width);
            AssertExt.AreEqual15(ls.Envelope.Height, lscheck.EnvelopeInternal.Height);

        }

        [Test]
        public void LineStringEnvelopeArea()
        {
            var coords = new Coordinate[36];
            var rnd = new Random();
            var coordscheck = new GeoAPI.Geometries.Coordinate[36];
            for (var i = 0; i < 36; i++)
            {
                coords[i] = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GeoAPI.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            var gf = new NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILineString lscheck = gf.CreateLineString(coordscheck);
            var ls = new LineString(coords);
            var area = ls.Envelope.Area();
            var areacheck = lscheck.Envelope.Area;
            AssertExt.AreEqual15(area, areacheck);
        }

        [Test]
        public void PolygonEnvelopeMaxMin()
        {
            var coords = new Coordinate[20];
            var rnd = new Random();
            var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            var coordscheck = new GeoAPI.Geometries.Coordinate[20];
            for (var i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GeoAPI.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GeoAPI.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            var gf = new NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
            GeoAPI.Geometries.IPolygon pgcheck = gf.CreatePolygon(ring, null);
            var pg = new Polygon(coords);
            AssertExt.AreEqual15(pg.Envelope.Maximum.X, pgcheck.EnvelopeInternal.MaxX);
            AssertExt.AreEqual15(pg.Envelope.Maximum.Y, pgcheck.EnvelopeInternal.MaxY);
            AssertExt.AreEqual15(pg.Envelope.Minimum.X, pgcheck.EnvelopeInternal.MinX);
            AssertExt.AreEqual15(pg.Envelope.Minimum.Y, pgcheck.EnvelopeInternal.MinY);
        }

        [Test]
        public void PolygonEnvelopeArea()
        {
            var coords = new Coordinate[20];
            var rnd = new Random();
            var center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
            var coordscheck = new GeoAPI.Geometries.Coordinate[20];
            for (var i = 0; i < 19; i++)
            {
                coords[i] = new Coordinate(center.X + Math.Cos((i * 10) * Math.PI / 10), center.Y + (i * 10) * Math.PI / 10);
                var x = coords[i].X;
                var y = coords[i].Y;
                var c = new GeoAPI.Geometries.Coordinate(x, y);
                coordscheck[i] = c;
            }
            coordscheck[19] = new GeoAPI.Geometries.Coordinate(coords[0].X, coords[0].Y);
            coords[19] = new Coordinate(coords[0].X, coords[0].Y);
            var gf = new NetTopologySuite.Geometries.GeometryFactory();
            GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
            GeoAPI.Geometries.IPolygon pgcheck = gf.CreatePolygon(ring, null);
            var pg = new Polygon(coords);
            var area = pg.Envelope.Area();
            AssertExt.AreEqual15(area, pgcheck.Envelope.Area);
        }
    }
}
