using DotSpatial.Data;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Tools.Tests
{
    [TestFixture]
    public class ClipPolygonWithLineTests
    {

        [Test]
        public void ExcecuteDoesntLoseOutputFilename()
        {
            var target = new ClipPolygonWithLine();

            IFeatureSet fsInput1 = FeatureSet.Open(@"Data\ClipPolygonWithLineTests\polygon.shp");
            IFeatureSet fsInput2 = FeatureSet.Open(@"Data\ClipPolygonWithLineTests\line.shp");
            IFeatureSet fsOutput = new FeatureSet { Filename = FileTools.GetTempFileName(".shp") };

            Assert.DoesNotThrow(() => target.Execute(fsInput1, fsInput2, fsOutput, new MockProgressHandler()));
            Assert.AreNotEqual(string.IsNullOrWhiteSpace(fsOutput.Filename), true);
            
            FileTools.DeleteShapeFile(fsOutput.Filename);
        }


    }
}
