using System.IO;
using DotSpatial.Data;
using NUnit.Framework;

namespace DotSpatial.Symbology.Tests
{
    [TestFixture]
    class ImageLayerTests
    {
        [Test]
        public void LegendTextIsEqualsToFileNameWhenDataSetIsRealFile()
        {
            var target = new ImageLayer();
            Assert.IsNull(target.LegendText);
            var imageData = new ImageData
            {
                Filename = Path.GetTempFileName(),
            };
            target.DataSet = imageData;
            // Legend should be equals to filename
            Assert.AreEqual(Path.GetFileName(imageData.Filename), target.LegendText);
        }

        [Test]
        public void LegendTextIsNotChangedWhenDataSetIsNotRealFile()
        {
            const string legendText = "Custom Legend Text";
            var target = new ImageLayer { LegendText = legendText };
            Assert.AreEqual(legendText, target.LegendText);
            var imageData = new ImageData {Filename = ""};
            target.DataSet = imageData; // assign dataset
            // Legend should be same
            Assert.AreEqual(legendText, target.LegendText);
        }
    }
}
