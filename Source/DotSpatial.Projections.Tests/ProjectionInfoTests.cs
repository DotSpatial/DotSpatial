using NUnit.Framework;

namespace DotSpatial.Projections.Tests
{
    /// <summary>
    ///This is a test class for ProjectionInfoTest and is intended
    ///to contain all ProjectionInfoTest Unit Tests
    ///</summary>
    [TestFixture]
    public class ProjectionInfoTests
    {
        /// <summary>
        /// Proj4 string esri comparison test.
        /// </summary>
        [Test]
        public void Proj4EsriComparisonTest()
        {
            var infoBuiltIn = KnownCoordinateSystems.Geographic.World.WGS1984;

            const string Esri = "GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137,298.257223562997]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.0174532925199433]]";
            var infoFromEsri = ProjectionInfo.FromEsriString(Esri);

            var expected = infoFromEsri.ToProj4String();
            var actual = infoBuiltIn.ToProj4String();

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for ToProj4String
        ///</summary>
        [Test]
        public void ToProj4StringTestDoesReadMatchToString()
        {
            var proj4String = "+proj=longlat +ellps=WGS84 +no_defs ";

            var target = ProjectionInfo.FromProj4String(proj4String);

            Assert.AreEqual(proj4String.Trim(), target.ToProj4String().Trim());
        }

        /// <summary>
        ///A test for http://dotspatial.codeplex.com/workitem/188 WGS1984
        ///</summary>
        [Test]
        public void ToEsriStringWgs1984Test()
        {
            var p1 = KnownCoordinateSystems.Geographic.World.WGS1984;

            const string Esri = "GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137,298.257223562997]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.0174532925199433]]";
            var p2 = ProjectionInfo.FromEsriString(Esri);

            var expected = p2.ToEsriString();
            var actual = p1.ToEsriString();

            Assert.AreEqual(expected, actual);

            Assert.IsTrue(p1.Equals(p2));
        }

        /// <summary>
        ///A test for ProjectionInfo Constructor
        ///</summary>
        [Test]
        public void ProjectionInfoConstructorTest()
        {
            var proj4String = string.Empty;
            var actual = ProjectionInfo.FromProj4String(proj4String);
            var expected = new ProjectionInfo();

            Assert.AreEqual(expected.ToProj4String(), actual.ToProj4String());
        }

        /// <summary>
        ///A test for ProjectionInfo Constructor
        ///</summary>
        [Test]
        public void ProjectionInfoConstructorTest2()
        {
            var actual = ProjectionInfo.FromProj4String(null);
            var expected = new ProjectionInfo();

            Assert.AreEqual(expected.ToProj4String(), actual.ToProj4String());
        }

        /// <summary>
        ///A test for ProjectionInfo Constructor
        ///</summary>
        [Test]
        public void ProjectionInfoConstructorTest1()
        {
            var proj4String = "+proj=longlat +ellps=WGS84 +no_defs ";
            var expected = ProjectionInfo.FromProj4String(proj4String);

            var actual = new ProjectionInfo();
            actual.GeographicInfo.Datum.Spheroid = new Spheroid("WGS84");
            actual.NoDefs = true;
            actual.IsLatLon = true;

            Assert.AreEqual(expected.ToProj4String(), actual.ToProj4String());
        }

        /// <summary>
        ///A test for ToEsriString NorthAmericanDatum1983
        ///</summary>
        [Test]
        public void ToEsriStringNorthAmericanDatum1983Test()
        {
            var p1 = KnownCoordinateSystems.Geographic.NorthAmerica.NAD1983;

            const string Esri = "GEOGCS[\"GCS_North_American_1983\",DATUM[\"D_North_American_1983\",SPHEROID[\"GRS_1980\",6378137,298.257222101004]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.0174532925199433]]";
            var p2 = ProjectionInfo.FromEsriString(Esri);

            var expected = p2.ToEsriString();
            var actual = p1.ToEsriString();

            Assert.AreEqual(expected, actual);

            Assert.IsTrue(p1.Equals(p2));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void OverProj4ExportImport(bool expected)
        {
            var target = ProjectionInfo.FromProj4String(KnownCoordinateSystems.Geographic.World.WGS1984.ToProj4String());
            target.Over = expected;
            var proj4Str = target.ToProj4String();
            var actual = ProjectionInfo.FromProj4String(proj4Str);
            Assert.AreEqual(target.Over, actual.Over);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void GeocProj4ExportImport(bool expected)
        {
            var target = ProjectionInfo.FromProj4String(KnownCoordinateSystems.Geographic.World.WGS1984.ToProj4String());
            target.Geoc = expected;
            var proj4Str = target.ToProj4String();
            var actual = ProjectionInfo.FromProj4String(proj4Str);
            Assert.AreEqual(target.Geoc, actual.Geoc);
        }

        [Test]
        public void EsriCentralParallelParse()
        {
            // Test for https://dotspatial.codeplex.com/workitem/22934
            const string EsriStr = "PROJCS[\"Albers Equal-Area Conic [SHG]\",GEOGCS[\"GCS_North_American_1983\",DATUM[\"D_North_American_1983\",SPHEROID[\"GRS_1980\",6378137.0,298.257222101]],PRIMEM[\";Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Albers\"],PARAMETER[\"False_Easting\",0.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",-96.0],PARAMETER[\"Standard_Parallel_1\",29.5],PARAMETER[\"Standard_Parallel_2\",45.5],PARAMETER[\"Central_Parallel\",23.0],UNIT[\"Meter\",1.0]]";
            var pi = ProjectionInfo.FromEsriString(EsriStr);
            Assert.AreEqual(23.0, pi.LatitudeOfOrigin);
            var proj4Str = pi.ToProj4String();
            Assert.IsTrue(proj4Str.Contains("+lat_0=23"));
        }

        [Test]
        [TestCase("FormattedProjectionFile")]
        [TestCase("StandardProjectionFile")]
        public void ReadProjectionFile(string resourceName)
        {
            string prjFile = System.IO.Path.GetTempFileName();
            try
            {
                System.IO.File.WriteAllText(prjFile, Properties.Resources.ResourceManager.GetString(resourceName, Properties.Resources.Culture));
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
