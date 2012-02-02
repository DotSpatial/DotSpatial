using System;
using DotSpatial.Analysis;
using DotSpatial.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotSpatial.Analysis.Test
{
    /// <summary>
    ///This is a test class for VoronoiTest and is intended
    ///to contain all VoronoiTest Unit Tests
    ///</summary>
    [TestClass()]
    public class VoronoiTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        /// <summary>
        ///A test for VoronoiPolygons
        ///</summary>
        [TestMethod()]
        public void VoronoiPolygonsTest()
        {
            string path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"Data\VoronoiTest.shp");
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