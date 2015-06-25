using System;
using DotSpatial.Tests.Common;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    class ShapefileTests
    {
        [Test]
        public void SelectByAttribute()
        {
            var file = FileTools.PathToTestFile(@"Shapefiles\Lakes\lakes.shp");
            var shapefile = Shapefile.OpenFile(file);
            var features = shapefile.SelectByAttribute("[NAME]='Great Salt Lake'");
            Assert.AreEqual(1, features.Count);
        }

        [Test]
        public void SelectIndexByAttribute()
        {
            var file = FileTools.PathToTestFile(@"Shapefiles\Lakes\lakes.shp");
            var shapeFile = Shapefile.OpenFile(file);
            var features = shapeFile.SelectIndexByAttribute("[NAME]='Great Salt Lake'");
            Assert.AreEqual(1, features.Count);
        }

        [Test]
        [TestCase(@"Shapefiles\Cities\cities.shp", typeof(PointShapefile))]
        [TestCase(@"Shapefiles\multipoint.shape\multipoint.shp", typeof(MultiPointShapefile))]
        [TestCase(@"Shapefiles\Rivers\Rivers.shp", typeof(LineShapefile))]
        [TestCase(@"Shapefiles\Lakes\Lakes.shp", typeof(PolygonShapefile))]
        public void CanOpen(string path, Type expectedType)
        {
            path = FileTools.PathToTestFile(path);
            var target = FeatureSet.OpenFile(path);
            Assert.IsTrue(target.Count > 0);
            Assert.AreEqual(expectedType, target.GetType());

            for (var i = 0; i < target.Count; i++)
            {
                var shape = target.GetShape(i, false);
                Assert.IsNotNull(shape);
            }

            // Additional test for UpdateExtent
            target.Extent = null;
            target.UpdateExtent();
            Assert.IsNotNull(target.Extent);
        }

        [Test]
        [TestCase(@"Shapefiles\Cities\cities.shp", typeof(PointShapefile))]
        [TestCase(@"Shapefiles\multipoint.shape\multipoint.shp", typeof(MultiPointShapefile))]
        [TestCase(@"Shapefiles\Rivers\Rivers.shp", typeof(LineShapefile))]
        [TestCase(@"Shapefiles\Archi\ARCHI_13-01-01.shp", typeof(LineShapefile))]
        [TestCase(@"Shapefiles\Lakes\Lakes.shp", typeof(PolygonShapefile))]
        public void CanSave(string path, Type expectedType)
        {
            foreach (var featuresInRam in new[]{false, true})
            {
                path = FileTools.PathToTestFile(path);
                var expected = (Shapefile)FeatureSet.OpenFile(path);
                Assert.IsTrue(expected.Count > 0);
                Assert.AreEqual(expectedType, expected.GetType());
                if (featuresInRam)
                {
                    var count = expected.Features.Count; // Force to load all features into memory
                    Assert.AreEqual(count, expected.Count);
                }
                Assert.AreEqual(featuresInRam, expected.FeaturesInRam);

                var newFile = FileTools.GetTempFileName(".shp");
                expected.SaveAs(newFile, true);
                try
                {
                    var actual = FeatureSet.OpenFile(path);
                    Assert.AreEqual(expected.Count, actual.Count);
                    for (var i = 0; i < expected.Count; i++)
                    {
                        var targetF = expected.GetShape(i, true);
                        var actualF = actual.GetShape(i, true);

                        Assert.AreEqual(targetF.Range.ShapeType, actualF.Range.ShapeType);
                        Assert.AreEqual(targetF.ToGeometry().Coordinates, actualF.ToGeometry().Coordinates);
                        Assert.AreEqual(targetF.Attributes, actualF.Attributes);
                    }
                }
                finally
                {
                    FileTools.DeleteShapeFile(newFile);
                }
            }
        }

        [Test]
        [TestCase(@"Shapefiles\Archi\ARCHI_13-01-01.shp", typeof(LineShapefile))]
        [TestCase(@"Shapefiles\Yield\Yield 2012.shp", typeof(PointShapefile))]
        public void CanLoadNullShapes(string path, Type expectedType)
        {
            path = FileTools.PathToTestFile(path);
            var target = FeatureSet.OpenFile(path);
            Assert.AreEqual(expectedType, target.GetType());
            Assert.IsNotNull(target);
            Assert.IsTrue(target.Count > 0);

            Shape nullShape = null;
            for (var i = 0; i < target.Count; i++)
            {
                var shape = target.GetShape(i, false);
                if (shape.Range.ShapeType == ShapeType.NullShape)
                {
                    nullShape = shape;
                    break;
                }
            }
            Assert.IsNotNull(nullShape);
            Assert.IsNull(nullShape.Vertices);
        }
    }
}
