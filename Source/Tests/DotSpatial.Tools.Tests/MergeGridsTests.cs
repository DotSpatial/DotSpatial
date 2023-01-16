// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DotSpatial.Data;
using DotSpatial.Data.Rasters.GdalExtension;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Tools.Tests
{
    /// <summary>
    ///This is a test class for intended to test the modified MergeGrids methods.
    ///</summary>
    [TestFixture]
    internal class MergeGridsTests
    {
        /// <summary>
        /// Tests the method that determines if a Type is of the flaoting point type (ie, float, double, decimal, etc).
        /// </summary>
        [Test]
        public void FloatPointTest()
        {
            System.Diagnostics.Debug.Print("-->FloatPointTest");
            System.Diagnostics.Debug.Print("========================================================================");
            List<Type> types = new List<Type> { typeof(byte), typeof(Int16), typeof(Int32), typeof(Int64), typeof(float), typeof(double), typeof(decimal), typeof(TextBox), null };

            foreach (Type typ in types)
            {
                bool isFloatPnt = MergeGrids.IsFloatingPoint(typ);
                System.Diagnostics.Debug.Print("type={0} IsFloatingPoint={1}", typ, isFloatPnt);
            }

            System.Diagnostics.Debug.Print("========================================================================");
        }


        /// <summary>
        /// Tests the method that determines the best output type based on two input types.
        /// </summary>
        [Test]
        public void BestOutputTypeTest()
        {
            System.Diagnostics.Debug.Print("-->BestOutputTypeTest");
            System.Diagnostics.Debug.Print("========================================================================");

            List<Type> typesA = new List<Type> { typeof(byte), typeof(Int16), typeof(Int32), typeof(Int64), typeof(float), typeof(double), typeof(decimal), null };
            List<Type> typesB = new List<Type> { typeof(byte), typeof(Int16), typeof(Int32), typeof(Int64), typeof(float), typeof(double), typeof(decimal), null };

            foreach (var typA in typesA)
            {
                foreach (var typB in typesB)
                {
                    System.Diagnostics.Debug.Print("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                    System.Diagnostics.Debug.Print("Input: typA={0} typB={1}", typA, typB);
                    var outTyp = MergeGrids.GetClosestType(typA, typB);
                    System.Diagnostics.Debug.Print("Output: typA={0} typB={1} outTyp={2}", typA, typB, outTyp);
                }
            }
            System.Diagnostics.Debug.Print("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            System.Diagnostics.Debug.Print("========================================================================");
        }

        /// <summary>
        /// Tests the method that merges two rasters into one raster.
        /// </summary>
        [Test]
        public void SimpleMergeGridsTest()
        {
            ////////////////////////////////////////////////////////////////
            // grid A
            ////////////////////////////////////////////////////////////////
            string gridDataFolder = Common.AbsolutePath(@"Data\Grids\");
            Debug.Print("gridDataFolder={0}", gridDataFolder);
            var p = new GdalRasterProvider();
            var fileNameA = Path.Combine(gridDataFolder, "TIFF", @"GTOPO30.tif");
            Debug.Print("fileNameA={0}", fileNameA);
            if (!File.Exists(fileNameA))
            {
                throw new Exception("Grid A does not exist.");
            }
            var gridA = p.Open(fileNameA);
            Debug.Print("gridA: min={0} max={1} cols={2} rows={3}", gridA.Minimum, gridA.Maximum, gridA.NumColumns, gridA.NumRows);
            Debug.Print("gridA: FileType={0} DataType={1}", gridA.FileType, gridA.DataType);

            ////////////////////////////////////////////////////////////////
            //grid B
            ////////////////////////////////////////////////////////////////
            var fileNameB = Path.Combine(gridDataFolder, "BIL", @"GTOPO30.bil");
            Debug.Print("fileNameB={0}", fileNameB);
            if (!File.Exists(fileNameB))
            {
                throw new Exception("Grid B does not exist.");
            }
            var gridB = p.Open(fileNameB);
            Debug.Print("gridB: min={0} max={1} cols={2} rows={3}", gridB.Minimum, gridB.Maximum, gridB.NumColumns, gridB.NumRows);
            Debug.Print("gridB: FileType={0} DataType={1}", gridB.FileType, gridB.DataType);

            ////////////////////////////////////////////////////////////////
            //merge the grids
            ////////////////////////////////////////////////////////////////
            var mergeGrids = new MergeGrids();
            var fileNameOut = Path.Combine(gridDataFolder, @"merged.bgd");
            Debug.Print("fileNameOut={0}", fileNameOut);
            IRaster outGrid = new Raster { Filename = fileNameOut };
            bool isMerged = mergeGrids.Execute(gridA, gridB, outGrid, new MockProgressHandler());
            if (isMerged)
            {
                Debug.Print("outGrid: min={0} max={1} cols={2} rows={3}", outGrid.Minimum, outGrid.Maximum, outGrid.NumColumns, outGrid.NumRows);
                Debug.Print("outGrid: FileType={0} DataType={1}", outGrid.FileType, outGrid.DataType);
                MessageBox.Show("The grids were merged.", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("The grids were not merged.", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            }








            //outRaster = Raster.Open(outRaster.Filename);
            //File.Delete(outRaster.Filename);




            //string savedGridName = Path.Combine(gridDataFolder, @"merged.tif");
            //var gridout = p.Open(savedGridName);
            //ICancelProgressHandler cancelProgressHandler = null;

            //IRaster input2 = _inputParam[1].Value as IRaster;

            //IRaster output = _outputParam[0].Value as IRaster;

            //return Execute(input1, input2, output, cancelProgressHandler);





            //string savedGridName = Path.Combine(gridDataFolder, @"elev_cm.tif");
            //sourceGrid.SaveAs(savedGridName);

            //Assert.AreEqual(sourceGrid.Maximum, sourceGridMaximum, 0.0001);

            //var savedSourceGrid = Raster.Open(savedGridName);

            //Assert.AreEqual(sourceGridMaximum, savedSourceGrid.Maximum, 0.0001);

            //sourceGrid.Close();
            //savedSourceGrid.Close();
            //File.Delete(savedGridName);

        }
    }
}
