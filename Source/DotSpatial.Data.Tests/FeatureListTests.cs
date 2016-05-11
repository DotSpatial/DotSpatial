using System.Data;
using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace DotSpatial.Data.Tests
{
    [TestFixture]
    public class FeatureListTests
    {
        [Test]
        public void Add_MustNotReplaceFeatureRow()
        {
            var parent = new FeatureSet(FeatureType.Point);
            var target = parent.Features;
            DataRow expected = null;
            target.FeatureAdded += delegate(object sender, FeatureEventArgs args)
                                       {
                                           expected = args.Feature.DataRow;
                                       };

            var addedFeature = parent.AddFeature(Point.Empty);
            var actual = addedFeature.DataRow;

            Assert.AreEqual(expected, actual);
        }
    }
}
