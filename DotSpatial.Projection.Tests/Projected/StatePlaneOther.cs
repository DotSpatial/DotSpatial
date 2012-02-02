using System.IO;
using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the StatePlaneOther category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class StatePlaneOther
    {


        /// <summary>
        /// Creates a new instance of the Africa Class
        /// </summary>
        [TestFixtureSetUp]
        public void Initialize()
        {
            TestSetupHelper.CopyProj4();
        }

        [Test]
        public void AmericanSamoa1962StatePlaneAmericanSamoaFIPS5300()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneOther.AmericanSamoa1962StatePlaneAmericanSamoaFIPS5300;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNGuamMapGrid()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneOther.NAD1983HARNGuamMapGrid;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNUTMZone2S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneOther.NAD1983HARNUTMZone2S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NADMichiganStatePlaneMichiganCentralFIPS2112()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneOther.NADMichiganStatePlaneMichiganCentralFIPS2112;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NADMichiganStatePlaneMichiganCentralOldFIPS2102()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneOther.NADMichiganStatePlaneMichiganCentralOldFIPS2102;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NADMichiganStatePlaneMichiganEastOldFIPS2101()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneOther.NADMichiganStatePlaneMichiganEastOldFIPS2101;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NADMichiganStatePlaneMichiganNorthFIPS2111()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneOther.NADMichiganStatePlaneMichiganNorthFIPS2111;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NADMichiganStatePlaneMichiganSouthFIPS2113()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneOther.NADMichiganStatePlaneMichiganSouthFIPS2113;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NADMichiganStatePlaneMichiganWestOldFIPS2103()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneOther.NADMichiganStatePlaneMichiganWestOldFIPS2103;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void OldHawaiianStatePlaneHawaii1FIPS5101()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneOther.OldHawaiianStatePlaneHawaii1FIPS5101;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void OldHawaiianStatePlaneHawaii2FIPS5102()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneOther.OldHawaiianStatePlaneHawaii2FIPS5102;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void OldHawaiianStatePlaneHawaii3FIPS5103()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneOther.OldHawaiianStatePlaneHawaii3FIPS5103;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void OldHawaiianStatePlaneHawaii4FIPS5104()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneOther.OldHawaiianStatePlaneHawaii4FIPS5104;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void OldHawaiianStatePlaneHawaii5FIPS5105()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneOther.OldHawaiianStatePlaneHawaii5FIPS5105;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void PuertoRicoStatePlanePuertoRicoFIPS5201()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneOther.PuertoRicoStatePlanePuertoRicoFIPS5201;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void PuertoRicoStatePlaneVirginIslandsStCroixFIPS5202()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneOther.PuertoRicoStatePlaneVirginIslandsStCroixFIPS5202;
            Tester.TestProjection(pStart);
        }
    }
}