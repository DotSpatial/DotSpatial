using System;
using DotSpatial.Analysis;
using DotSpatial.Data;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using ClassCleanup = NUnit.Framework.TestFixtureTearDownAttribute;
using ClassInitialize = NUnit.Framework.TestFixtureSetUpAttribute;
using Assert = NUnit.Framework.Assert;

namespace DotSpatial.Analysis.Test
{
    /// <summary>
    ///This is a test class for VoronoiTest and is intended
    ///to contain all VoronoiTest Unit Tests
    ///</summary>
    [TestClass()]
    public class VoronoiTest
    {
        /// <summary>
        ///A test for VoronoiPolygons
        ///</summary>
        [TestMethod()]
        public void VoronoiPolygonsTest()
        {
            string path = System.IO.Path.Combine(new String[]{System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Data", "VoronoiTest.shp"});
            IFeatureSet points = FeatureSet.Open(path);
            IFeatureSet actual = Voronoi.VoronoiPolygons(points, cropToExtent: true);

            Assert.AreEqual(points.Features.Count, actual.Features.Count);
            foreach (IFeature point in points.Features)
            {
                //TODO: Assert.IsTrue(point is contained in one (and not more than one) polygon of actual)
            }
            //TODO: Assert that each polygon in actual contains exactly one point from points
        }
    }
}