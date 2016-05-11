using System.IO;
using DotSpatial.Data;
using NUnit.Framework;

namespace DotSpatial.Analysis.Tests
{
    [TestFixture]
    class SlopeTests
    {
        [Test]
        public void SlopeIsWorking()
        {
            var raster = Raster.Open(Path.Combine("Data", "kriging.bgd"));
            var actual = Slope.GetSlope(raster, 1, true,  null);
            try
            {
                Assert.IsNotNull(actual);
                Assert.AreEqual(raster.Extent, actual.Extent);
                Assert.AreEqual(raster.NumColumns, actual.NumColumns);
                Assert.AreEqual(raster.NumRows, actual.NumRows);
                Assert.AreEqual(raster.CellHeight, actual.CellHeight);
                Assert.AreEqual(raster.CellWidth, actual.CellWidth);
                // Test that some output values are non zeros and non NoData
                var existsSomeValidValues = false;
                for (int i = 0; i < raster.NumRows; i++)
                    for (int j = 0; j < raster.NumColumns; j++)
                    {
                        if (raster.Value[i, j] != 0 &&
                            raster.Value[i, j] != raster.NoDataValue)
                        {
                            existsSomeValidValues = true;
                            goto finCycle;
                        }
                    }
                finCycle:
                Assert.IsTrue(existsSomeValidValues);
            }
            finally
            {
                File.Delete(actual.Filename);
            }
        }
    }
}
