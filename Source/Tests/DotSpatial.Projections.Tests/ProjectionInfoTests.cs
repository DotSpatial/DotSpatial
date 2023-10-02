// Copyright (c) DotSpatial Team. All rights reserved.
// Licensed under the MIT, license. See License.txt file in the project root for full license information.

using NUnit.Framework;

namespace DotSpatial.Projections.Tests
{
    /// <summary>
    /// This is a test class for ProjectionInfoTest and is intended to contain all ProjectionInfoTest Unit Tests
    ///</summary>
    [TestFixture]
    public class ProjectionInfoTests
    {
        /// <summary>
        /// Proj4 string esri comparison test.
        /// </summary>
        [Test, Category("Projection")]
        public void Proj4EsriComparisonTest()
        {
            ProjectionInfo infoBuiltIn = KnownCoordinateSystems.Geographic.World.WGS1984;

            const string esri = "GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137,298.257223562997]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.0174532925199433]]";
            ProjectionInfo infoFromEsri = ProjectionInfo.FromEsriString(esri);

            string expected = infoFromEsri.ToProj4String();
            string actual = infoBuiltIn.ToProj4String();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToProj4String
        ///</summary>
        [Test, Category("Projection")]
        public void ToProj4StringTestDoesReadMatchToString()
        {
            string proj4String = "+proj=longlat +ellps=WGS84 +no_defs ";

            ProjectionInfo target = ProjectionInfo.FromProj4String(proj4String);

            Assert.AreEqual(proj4String.Trim(), target.ToProj4String().Trim());
        }

        /// <summary>
        ///A test for http://dotspatial.codeplex.com/workitem/188 WGS1984
        ///</summary>
        [Test, Category("Projection")]
        public void ToEsriStringWGS1984Test()
        {
            ProjectionInfo p1 = KnownCoordinateSystems.Geographic.World.WGS1984;

            const string esri = "GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137,298.257223562997]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]]";
            ProjectionInfo p2 = ProjectionInfo.FromEsriString(esri);

            string expected = p2.ToEsriString();
            string actual = p1.ToEsriString();

            Assert.AreEqual(expected, actual);

            Assert.IsTrue(p1.Equals(p2));
        }

        /// <summary>
        ///A test for ProjectionInfo Constructor
        ///</summary>
        [Test, Category("Projection")]
        public void ProjectionInfoConstructorTest()
        {
            string proj4String = string.Empty;
            ProjectionInfo actual = ProjectionInfo.FromProj4String(proj4String);
            ProjectionInfo expected = new ProjectionInfo();

            Assert.AreEqual(expected.ToProj4String(), actual.ToProj4String());
        }

        /// <summary>
        ///A test for ProjectionInfo Constructor
        ///</summary>
        [Test, Category("Projection")]
        public void ProjectionInfoConstructorTest2()
        {
            string proj4String = null;
            ProjectionInfo actual = ProjectionInfo.FromProj4String(proj4String);
            ProjectionInfo expected = new ProjectionInfo();

            Assert.AreEqual(expected.ToProj4String(), actual.ToProj4String());
        }

        /// <summary>
        ///A test for ProjectionInfo Constructor
        ///</summary>
        [Test, Category("Projection")]
        public void ProjectionInfoConstructorTest1()
        {
            string proj4String = "+proj=longlat +ellps=WGS84 +no_defs ";
            ProjectionInfo expected = ProjectionInfo.FromProj4String(proj4String);

            ProjectionInfo actual = new ProjectionInfo();
            actual.GeographicInfo.Datum.Spheroid = new Spheroid("WGS84");
            actual.NoDefs = true;
            actual.IsLatLon = true;

            Assert.AreEqual(expected.ToProj4String(), actual.ToProj4String());
        }

        /// <summary>
        ///A test for ToEsriString NorthAmericanDatum1983
        ///</summary>
        [Test, Category("Projection")]
        public void ToEsriStringNorthAmericanDatum1983Test()
        {
            ProjectionInfo p1 = KnownCoordinateSystems.Geographic.NorthAmerica.NorthAmericanDatum1983;

            const string esri = "GEOGCS[\"GCS_North_American_1983\",DATUM[\"D_North_American_1983\",SPHEROID[\"GRS_1980\",6378137,298.257222101004]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]]";
            ProjectionInfo p2 = ProjectionInfo.FromEsriString(esri);

            string expected = p2.ToEsriString();
            string actual = p1.ToEsriString();

            Assert.AreEqual(expected, actual);

            Assert.IsTrue(p1.Equals(p2));
        }

        /// <summary>
        /// Test for Geoc_Proj4ExportImport
        /// </summary>
        /// <param name="expected">value that should be returned.</param>
        [Test, Category("Projection")]
        [TestCase(true)]
        [TestCase(false)]
        public void Over_Proj4ExportImport(bool expected)
        {
            ProjectionInfo target = ProjectionInfo.FromProj4String(KnownCoordinateSystems.Geographic.World.WGS1984.ToProj4String());
            target.Over = expected;
            string proj4str = target.ToProj4String();
            ProjectionInfo actual = ProjectionInfo.FromProj4String(proj4str);
            Assert.AreEqual(target.Over, actual.Over);
        }

        /// <summary>
        /// Test for Geoc_Proj4ExportImport
        /// </summary>
        /// <param name="expected">value that should be returned.</param>
        [Test, Category("Projection")]
        [TestCase(true)]
        [TestCase(false)]
        public void Geoc_Proj4ExportImport(bool expected)
        {
            ProjectionInfo target = ProjectionInfo.FromProj4String(KnownCoordinateSystems.Geographic.World.WGS1984.ToProj4String());
            target.Geoc = expected;
            string proj4str = target.ToProj4String();
            ProjectionInfo actual = ProjectionInfo.FromProj4String(proj4str);
            Assert.AreEqual(target.Geoc, actual.Geoc);
        }

        /// <summary>
        /// Test for EsriCentral_ParallelParse       
        /// </summary>
        [Test, Category("Projection")]
        public void EsriCentral_ParallelParse()
        {
            // Test for https://dotspatial.codeplex.com/workitem/22934
            const string esriStr = "PROJCS[\"Albers Equal-Area Conic [SHG]\",GEOGCS[\"GCS_North_American_1983\",DATUM[\"D_North_American_1983\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\";Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Albers\"],PARAMETER[\"False_Easting\",0.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",-96.0],PARAMETER[\"Standard_Parallel_1\",29.5],PARAMETER[\"Standard_Parallel_2\",45.5],PARAMETER[\"Central_Parallel\",23.0],UNIT[\"Meter\",1.0]]";
            ProjectionInfo pi = ProjectionInfo.FromEsriString(esriStr);
            Assert.AreEqual(23.0, pi.LatitudeOfOrigin);
            string proj4Str = pi.ToProj4String();
            Assert.IsTrue(proj4Str.Contains("+lat_0=23"));
        }

        /// <summary>
        /// Test for ReadProjectionFile       
        /// </summary>
        [Test, Category("Projection")]
        [TestCase("FormattedProjectionFile")]
        [TestCase("StandardProjectionFile")]
        public void ReadProjectionFile(string resourceName)
        {
            string prjFile = System.IO.Path.GetTempFileName();
            try
            {
                System.IO.File.WriteAllBytes(prjFile, (byte[])Resources.ResourceManager.GetObject(resourceName, Resources.Culture));
                ProjectionInfo info = ProjectionInfo.Open(prjFile);

                Assert.AreEqual("WGS_1984_Web_Mercator_Auxiliary_Sphere", info.Name);
                Assert.AreEqual("GCS_WGS_1984", info.GeographicInfo.Name);
                Assert.AreEqual("D_WGS_1984", info.GeographicInfo.Datum.Name);
                Assert.AreEqual("WGS_1984", info.GeographicInfo.Datum.Spheroid.Name);
                Assert.AreEqual(6378137, info.GeographicInfo.Datum.Spheroid.EquatorialRadius, 1e-10);
                Assert.AreEqual(298.257223562997, info.GeographicInfo.Datum.Spheroid.InverseFlattening, 1e-10);
                Assert.AreEqual("Greenwich", info.GeographicInfo.Meridian.Name);
                Assert.AreEqual(0, info.GeographicInfo.Meridian.Longitude, 1e-10);
                Assert.AreEqual("Degree", info.GeographicInfo.Unit.Name);
                Assert.AreEqual(0.0174532925199433, info.GeographicInfo.Unit.Radians, 1e-10);
                Assert.AreEqual("Mercator_Auxiliary_Sphere", info.Transform.Name);
                Assert.AreEqual("Meter", info.Unit.Name);
                Assert.AreEqual(1, info.Unit.Meters, 1e-10);
            }
            finally
            {
                System.IO.File.Delete(prjFile);
            }
        }
    }
}
