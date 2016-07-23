using System;
using System.IO;
using GeoAPI.Geometries;
using DotSpatial.Data.Rasters.GdalExtension;
using DotSpatial.Tests.Common;
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using ClassCleanup = NUnit.Framework.TestFixtureTearDownAttribute;
using ClassInitialize = NUnit.Framework.TestFixtureSetUpAttribute;

namespace DotSpatial.Data.Tests
{
    /// <summary>
    ///This is a class testing the implementation of affine coefficients.
    ///</summary>
    [TestClass()]
    public class AffineCoefficientsTests
    {
        /// <summary>
        ///A test for affine coefficients in AffineTransform: generate random points with 
        ///known (row, column) and check whether AffineTransform.ProjToCell returns the correct values.
        ///This test fails when using Math.Floor on lines 168-169 of AffineTransform.cs.
        ///</summary>
        [TestMethod]
        public void AffineTransformTest()
        {
            var c = new double[6];
            c[0] = -179.75; // x-coordinate of the *center* of the top-left cell
            c[1] = 0.5;     // column width
            c[2] = 0;       // rotation term
            c[3] = 89.75;   // y-coordinate of the *center* of the top-left cell
            c[4] = 0;       // rotation term
            c[5] = -0.5;    // negative row height
            var at = new AffineTransform(c);

            var rand = new Random();

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Coordinate corner = at.CellBottomLeft_ToProj(row, col);
                    double dX = Math.Abs(c[1]) * rand.NextDouble();
                    double dY = Math.Abs(c[5]) * rand.NextDouble();
                    Coordinate point = new Coordinate(corner.X + dX, corner.Y + dY);
                    Assert.AreEqual(at.ProjToCell(point), new RcIndex(row, col));
                }
            }
        }

        /// <summary>
        ///A test for affine coefficients in GdalRaster: use gdal to load a raster file with
        ///known geolocation and test the location of the center of the first grid cell.
        ///Without a half-cell shift applied to the origin of the grid (lines 587-588 in GdalRaster.cs),
        ///this test fails because affine coefficients are defined differently in gdal and dotspatial.
        ///</summary>
        [TestMethod]
        public void GdalRasterTest()
        {
            var rp = new GdalRasterProvider();
            var raster = rp.Open(@"Data\Grids\sample_geotiff.tif");
            var at = new AffineTransform(raster.Bounds.AffineCoefficients);
            Assert.AreEqual(at.CellCenter_ToProj(0, 0), new Coordinate(-179.9499969, 89.9499969));// correct location from sample_geotiff.tfw
        }
    }
}