using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DotSpatial.Topology;
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

            var addedFeature = parent.AddFeature(new Point());
            var actual = addedFeature.DataRow;

            Assert.AreEqual(expected, actual);
        }
    }
}
