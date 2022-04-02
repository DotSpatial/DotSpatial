using NUnit.Framework;

namespace DotSpatial.Projections.Tests
{
    [TestFixture]
    class SpheroidTests
    {
        /// <summary>
        /// Checks that parsing an ESRI string with [ and ] in the spheroid name gets the complete name and doesn't throw an exception.
        /// </summary>
        [Test(Description = @"Checks that parsing an ESRI string with [ and ] in the spheroid name gets the complete name and doesn't throw an exception. (https://github.com/DotSpatial/DotSpatial/issues/1142)")]
        public void ParseEsriStringWithBracketInSpheroidName()
        {
            Spheroid s = new();
            Assert.DoesNotThrow(() => s.ParseEsriString("GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984[EPSG ID 7030]\",6378137,298.257223563]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]]"));
            Assert.AreEqual("WGS_1984[EPSG ID 7030]", s.Name, "The Spheroid name does not equal WGS_1984[EPSG ID 7030].");
        }

        /// <summary>
        /// Checks that parsing an ESRI string without [ and ] in the spheroid name gets the complete name and doesn't throw an exception.
        /// </summary>
        [Test(Description = @"Checks that parsing an ESRI string without [ and ] in the spheroid name gets the complete name and doesn't throw an exception. (https://github.com/DotSpatial/DotSpatial/issues/1142)")]
        public void ParseEsriStringWithoutBracketInSpheroidName()
        {
            Spheroid s = new();
            Assert.DoesNotThrow(() => s.ParseEsriString("GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137,298.257223563]],PRIMEM[\"Greenwich\",0],UNIT[\"Degree\",0.017453292519943295]]"));
            Assert.AreEqual("WGS_1984", s.Name, "The Spheroid name does not equal WGS_1984.");
        }
    }
}