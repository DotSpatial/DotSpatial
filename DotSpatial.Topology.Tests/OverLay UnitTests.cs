using System;
using NUnit.Framework;
using DotSpatial.Topology;


namespace DotSpatial.Topology.Tests
{

    /// <summary>
    /// 
    /// </summary>
    [TestFixture]
    public class OverLayUnitTests
    {



        //[Test]
        //public void Union()
        //{
        //    Random rnd = new Random();
        //    Polygon[] pg = new Polygon[50];
        //    GeoAPI.Geometries.IPolygon[] pgcheck = new GeoAPI.Geometries.IPolygon[50];
        //    GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
        //    for (int i = 0; i < 50; i++)
        //    {
        //        Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);
        //        Coordinate[] coord = new Coordinate[36];
        //        GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[36];
        //        for (int ii = 0; ii < 36; ii++)
        //        {
        //            coord[ii] = new Coordinate(center.X + Math.Cos((ii * 10) * Math.PI / 10), center.Y + (ii * 10) * Math.PI / 10);
        //            double x = coord[ii].X;
        //            double y = coord[ii].Y;
        //            GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
        //            coordscheck[ii] = c;
        //        }
        //        coord[35] = new Coordinate(coord[0].X, coord[0].Y);
        //        coordscheck[35] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coordscheck[0].X, coordscheck[0].Y);
        //        GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
        //        pgcheck[i] = gf.CreatePolygon(ring, null);
        //        pg[i] = new Polygon(coord);

        //    }
        //    for (int shp = 0; shp < 50; shp++)
        //    {
        //        IGeometry g = pg[shp].Union(pg[shp + 1]);
        //        GeoAPI.Geometries.IGeometry gcheck = pgcheck[shp].Union(pgcheck[shp + 1]);
        //        var crdsA = g.Coordinates;
        //        var crdsB = gcheck.Coordinates;
        //        for (int i = 0; i < crdsB.Length; i++)
        //        {
        //            Assert.AreEqual(crdsA[i].X, crdsB[i].X);
        //            Assert.AreEqual(crdsA[i].Y, crdsB[i].Y);
        //        }
        //    }

        //}

        [Test]
        public void Intersection()
        {
            Random rnd = new Random();
            Polygon[] pg = new Polygon[50];
            GeoAPI.Geometries.IPolygon[] pgcheck = new GeoAPI.Geometries.IPolygon[50];
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);

            for (int i = 0; i < 50; i++)
            {

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
            for (int t = 0; t < 49; t++)
            {
                IGeometry g = pg[t].Intersection(pg[t + 1]);
                GeoAPI.Geometries.IGeometry gcheck = pgcheck[t].Intersection(pgcheck[t + 1]);
                for (int j = 0; j < g.Coordinates.Count; j++)
                {
                    Assert.AreEqual(g.Coordinates[j].X, gcheck.Coordinates[j].X);
                    Assert.AreEqual(g.Coordinates[j].Y, gcheck.Coordinates[j].Y);
                }
            }
        }

        [Test]
        public void Overlaps()
        {
            Random rnd = new Random();
            Polygon[] pg = new Polygon[50];
            GeoAPI.Geometries.IPolygon[] pgcheck = new GeoAPI.Geometries.IPolygon[50];
            GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();
            Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);

            for (int i = 0; i < 50; i++)
            {

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
            for (int t = 0; t < 49; t++)
            {
                bool g = pg[t].Overlaps(pg[t + 1]);
                bool gcheck = pgcheck[t].Overlaps(pgcheck[t + 1]);
                Assert.AreEqual(g, gcheck);

            }
        }

        //[Test]
        //public void SymmetricDifference()
        //{
        //    Random rnd = new Random();
        //    Polygon[] pg = new Polygon[50];
        //    GeoAPI.Geometries.IPolygon[] pgcheck = new GeoAPI.Geometries.IPolygon[50];
        //    GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();

        //    for (int i = 0; i < 50; i++)
        //    {
        //       Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);

        //        Coordinate[] coord = new Coordinate[36];
        //        GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[36];
        //        for (int ii = 0; ii < 36; ii++)
        //        {
        //            coord[ii] = new Coordinate(center.X + Math.Cos((ii * 10) * Math.PI / 10), center.Y + (ii * 10) * Math.PI / 10);
        //            double x = coord[ii].X;
        //            double y = coord[ii].Y;
        //            GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
        //            coordscheck[ii] = c;
        //        }
        //        coord[35] = new Coordinate(coord[0].X, coord[0].Y);
        //        coordscheck[35] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coordscheck[0].X, coordscheck[0].Y);
        //        GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
        //        pgcheck[i] = gf.CreatePolygon(ring, null);
        //        pg[i] = new Polygon(coord);

        //    }
        //    for (int t = 0; t < 49; t++)
        //    {
        //        IGeometry g = pg[t].SymmetricDifference(pg[t + 1]);
        //        GeoAPI.Geometries.IGeometry gcheck = pgcheck[t].SymmetricDifference(pgcheck[t + 1]);
        //        for (int j = 0; j < g.Coordinates.Count; j++)
        //        {
        //            Assert.AreEqual(g.Coordinates[j].X, gcheck.Coordinates[j].X);
        //            Assert.AreEqual(g.Coordinates[j].Y, gcheck.Coordinates[j].Y);
        //        }
        //    }
        //}

        //[Test]
        //public void Difference()
        //{
        //    Random rnd = new Random();
        //    Polygon[] pg = new Polygon[50];
        //    GeoAPI.Geometries.IPolygon[] pgcheck = new GeoAPI.Geometries.IPolygon[50];
        //    GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory gf = new GisSharpBlog.NetTopologySuite.Geometries.GeometryFactory();

        //    for (int i = 0; i < 50; i++)
        //    {
        //        Coordinate center = new Coordinate((rnd.NextDouble() * 360) - 180, (rnd.NextDouble() * 180) - 90);

        //        Coordinate[] coord = new Coordinate[36];
        //        GeoAPI.Geometries.ICoordinate[] coordscheck = new GeoAPI.Geometries.ICoordinate[36];
        //        for (int ii = 0; ii < 36; ii++)
        //        {
        //            coord[ii] = new Coordinate(center.X + Math.Cos((ii * 10) * Math.PI / 10), center.Y + (ii * 10) * Math.PI / 10);
        //            double x = coord[ii].X;
        //            double y = coord[ii].Y;
        //            GisSharpBlog.NetTopologySuite.Geometries.Coordinate c = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(x, y);
        //            coordscheck[ii] = c;
        //        }
        //        coord[35] = new Coordinate(coord[0].X, coord[0].Y);
        //        coordscheck[35] = new GisSharpBlog.NetTopologySuite.Geometries.Coordinate(coordscheck[0].X, coordscheck[0].Y);
        //        GeoAPI.Geometries.ILinearRing ring = gf.CreateLinearRing(coordscheck);
        //        pgcheck[i] = gf.CreatePolygon(ring, null);
        //        pg[i] = new Polygon(coord);
        //    }
        //    for (int t = 0; t < 49; t++)
        //    {
        //        IGeometry g = pg[t].Difference(pg[t + 1]);
        //        GeoAPI.Geometries.IGeometry gcheck = pgcheck[t].Difference(pgcheck[t + 1]);
        //        for (int j = 0; j < g.Coordinates.Count; j++)
        //        {
        //            Assert.AreEqual(g.Coordinates[j].X, gcheck.Coordinates[j].X);
        //            Assert.AreEqual(g.Coordinates[j].Y, gcheck.Coordinates[j].Y);
        //        }
        //    }
        //}





    }


}
