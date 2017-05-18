// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT license. See License.txt file in the project root for full license information.

using System;
using System.IO;
using DotSpatial.Data.Rasters.GdalExtension;
using DotSpatial.Tests.Common;
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;

namespace DotSpatial.Data.Tests
{
    /// <summary>
    /// This is a test class for RasterTest and is intended to contain all RasterTest Unit Tests.
    /// </summary>
    [TestClass]
    public class RasterTest
    {
        #region Methods

        /// <summary>
        /// Checks whether a raster created with GdalRasterProvider can be opened after closing.
        /// </summary>
        [TestMethod]
        public void CanOpenRasterAfterClose()
        {
            var rasterFileName = FileTools.GetTempFileName(".tif");
            var p = new GdalRasterProvider();
            var raster = p.Create(rasterFileName, null, 20, 20, 1, typeof(float), new[] { string.Empty });
            raster.Close();

            try
            {
                using (var openTif = File.Open(rasterFileName, FileMode.Open)) Assert.IsNotNull(openTif);
            }
            finally
            {
                File.Delete(rasterFileName);
            }
        }

        /// <summary>
        /// A test for GetNoDataCellCount
        /// </summary>
        [TestMethod]
        public void GetNoDataCellCountTest()
        {
            var path = FileTools.GetTempFileName(".bgd");
            const double Xllcorner = 3267132.224761;
            const double Yllcorner = 5326939.203029;
            const int Ncols = 512;
            const int Nrows = 128;
            const int FrequencyOfNoValue = 5;

            const double Cellsize = 500;
            var x2 = Xllcorner + (Cellsize * Ncols);
            var y2 = Yllcorner + (Cellsize * Nrows);
            var myExtent = new Extent(Xllcorner, Yllcorner, x2, y2);
            var target = (Raster)Raster.Create(path, string.Empty, Ncols, Nrows, 1, typeof(double), new[] { string.Empty });
            target.Bounds = new RasterBounds(Nrows, Ncols, myExtent);
            target.NoDataValue = -9999;
            var mRow = target.Bounds.NumRows;
            var mCol = target.Bounds.NumColumns;

            for (var row = 0; row < mRow; row++)
            {
                for (var col = 0; col < mCol; col++)
                {
                    if (row % FrequencyOfNoValue == 0) target.Value[row, col] = -9999d;
                    else target.Value[row, col] = 2d;
                }
            }

            target.Save();

            const long Expected = ((Nrows / FrequencyOfNoValue) * Ncols) + Ncols;
            var actual = target.GetNoDataCellCount();
            Assert.AreEqual(Expected, actual);

            try
            {
                File.Delete(path);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// A test for SaveAs
        /// </summary>
        [TestMethod]
        public void SaveAsTest()
        {
            const string GridDataFolder = @"Data\Grids\";
            var p = new GdalRasterProvider();
            var sourceGrid = p.Open(GridDataFolder + @"elev_cm_ESRI\elev_cm_clip2\hdr.adf");
            var sourceGridMaximum = sourceGrid.Maximum;

            const string SavedGridName = GridDataFolder + @"elev_cm.tif";
            sourceGrid.SaveAs(SavedGridName);

            Assert.AreEqual(sourceGrid.Maximum, sourceGridMaximum, 0.0001);

            var savedSourceGrid = Raster.Open(SavedGridName);

            Assert.AreEqual(sourceGridMaximum, savedSourceGrid.Maximum, 0.0001);

            sourceGrid.Close();
            savedSourceGrid.Close();
            File.Delete(SavedGridName);
        }

        #endregion
    }
}