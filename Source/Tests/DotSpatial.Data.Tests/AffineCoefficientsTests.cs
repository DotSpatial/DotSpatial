// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using DotSpatial.Data.Rasters.GdalExtension;
using DotSpatial.Tests.Common;
using NetTopologySuite.Geometries;
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;

namespace DotSpatial.Data.Tests
{
    /// <summary>
    /// This is a class testing the implementation of affine coefficients.
    /// </summary>
    [TestClass]
    public class AffineCoefficientsTests
    {
        #region Methods

        /// <summary>
        /// A test for affine coefficients in AffineTransform: generate random points with
        /// known (row, column) and check whether AffineTransform.ProjToCell returns the correct values.
        /// This test fails when using Math.Floor in AffineTransform.ProjToCell.
        /// </summary>
        [TestMethod]
        public void AffineTransformTest()
        {
            var c = new double[6];
            c[0] = -179.75; // x-coordinate of the *center* of the top-left cell
            c[1] = 0.5; // column width
            c[2] = 0.0; // rotation/skew term
            c[3] = 89.75; // y-coordinate of the *center* of the top-left cell
            c[4] = 0.0; // rotation/skew term
            c[5] = -0.5; // negative row height
            var at = new AffineTransform(c);

            var rand = new Random();

            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    Coordinate corner = at.CellTopLeftToProj(row, col);
                    double fracCol = rand.NextDouble();
                    double fracRow = rand.NextDouble();
                    double dX = (c[1] * fracCol) + (c[2] * fracRow);
                    double dY = (c[4] * fracCol) + (c[5] * fracRow);
                    Coordinate point = new(corner.X + dX, corner.Y + dY);
                    Assert.AreEqual(at.ProjToCell(point), new RcIndex(row, col));
                }
            }
        }

        /// <summary>
        /// A test for affine coefficients in GdalRaster: use gdal to load a raster file with
        /// known geolocation and test the location of the center of the first grid cell.
        /// Without a half-cell shift applied to the origin of the grid in GdalRaster.ReadHeader,
        /// this test fails because affine coefficients are defined differently in gdal and dotspatial.
        /// </summary>
        [TestMethod]
        public void GdalRasterTest()
        {
            var rp = new GdalRasterProvider();
            var raster = rp.Open(Common.AbsolutePath(@"Data\Grids\sample_geotiff.tif"));
            var at = new AffineTransform(raster.Bounds.AffineCoefficients);
            Assert.AreEqual(at.CellCenterToProj(0, 0), new Coordinate(-179.9499969, 89.9499969)); // correct location from sample_geotiff.tfw
        }

        #endregion
    }
}