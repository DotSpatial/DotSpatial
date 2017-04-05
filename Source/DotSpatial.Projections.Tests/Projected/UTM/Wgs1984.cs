using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected.Utm
{
    /// <summary>
    /// This class contains all the tests for the Utm.Wgs1984 category of Projected coordinate systems.
    /// </summary>
    [TestFixture]
    public class Wgs1984
    {
        [Test]
        [TestCaseSource(nameof(GetNorthernHemisphereProjections))]
        [TestCaseSource(nameof(GetSouthernHemisphereProjections))]
        public void ProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
        }

        private static IEnumerable<ProjectionInfoDesc> GetNorthernHemisphereProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.Utm.Wgs1984.NorthernHemisphere);
        }

        private static IEnumerable<ProjectionInfoDesc> GetSouthernHemisphereProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.Utm.Wgs1984.SouthernHemisphere);
        }

        //[Test]
        //[Ignore]
        //public void WGS1984ComplexUTMZone20N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984ComplexUTMZone20N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void WGS1984ComplexUTMZone21N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984ComplexUTMZone21N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void WGS1984ComplexUTMZone22N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984ComplexUTMZone22N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void WGS1984ComplexUTMZone23N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984ComplexUTMZone23N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void WGS1984ComplexUTMZone24N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984ComplexUTMZone24N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void WGS1984ComplexUTMZone25N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984ComplexUTMZone25N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void WGS1984ComplexUTMZone26N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984ComplexUTMZone26N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void WGS1984ComplexUTMZone27N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984ComplexUTMZone27N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void WGS1984ComplexUTMZone28N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984ComplexUTMZone28N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void WGS1984ComplexUTMZone29N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984ComplexUTMZone29N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void WGS1984ComplexUTMZone30N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984ComplexUTMZone30N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone10N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone10N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone10S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone10S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone11N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone11N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone11S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone11S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone12N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone12N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone12S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone12S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone13N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone13N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone13S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone13S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone14N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone14N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone14S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone14S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone15N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone15N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone15S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone15S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone16N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone16N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone16S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone16S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone17N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone17N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone17S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone17S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone18N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone18N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone18S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone18S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone19N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone19N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone19S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone19S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone1N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone1N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone1S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone1S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone20N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone20N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone20S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone20S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone21N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone21N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone21S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone21S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone22N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone22N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone22S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone22S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone23N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone23N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone23S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone23S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone24N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone24N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone24S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone24S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone25N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone25N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone25S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone25S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone26N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone26N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone26S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone26S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone27N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone27N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone27S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone27S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone28N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone28N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone28S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone28S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone29N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone29N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone29S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone29S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone2N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone2N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone2S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone2S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone30N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone30N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone30S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone30S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone31N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone31N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone31S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone31S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone32N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone32N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone32S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone32S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone33N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone33N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone33S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone33S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone34N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone34N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone34S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone34S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone35N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone35N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone35S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone35S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone36N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone36N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone36S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone36S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone37N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone37N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone37S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone37S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone38N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone38N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone38S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone38S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone39N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone39N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone39S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone39S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone3N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone3N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone3S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone3S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone40N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone40N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone40S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone40S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone41N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone41N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone41S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone41S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone42N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone42N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone42S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone42S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone43N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone43N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone43S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone43S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone44N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone44N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone44S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone44S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone45N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone45N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone45S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone45S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone46N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone46N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone46S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone46S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone47N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone47N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone47S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone47S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone48N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone48N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone48S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone48S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone49N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone49N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone49S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone49S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone4N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone4N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone4S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone4S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone50N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone50N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone50S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone50S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone51N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone51N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone51S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone51S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone52N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone52N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone52S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone52S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone53N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone53N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone53S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone53S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone54N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone54N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone54S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone54S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone55N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone55N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone55S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone55S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone56N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone56N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone56S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone56S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone57N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone57N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone57S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone57S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone58N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone58N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone58S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone58S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone59N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone59N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone59S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone59S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone5N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone5N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone5S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone5S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone60N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone60N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone60S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone60S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone6N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone6N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone6S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone6S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone7N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone7N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone7S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone7S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone8N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone8N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone8S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmWgs1984.WGS1984UTMZone8S;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone9N()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.Utm.Wgs1984.NorthernHemisphere.WGS1984UTMZone9N;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WGS1984UTMZone9S()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.Utm.Wgs1984.SouthernHemisphere.WGS1984UTMZone9S;
        //    Tester.TestProjection(pStart);
        //}
    }
}