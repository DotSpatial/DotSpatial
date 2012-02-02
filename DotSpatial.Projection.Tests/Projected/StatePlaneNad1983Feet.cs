
using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the StatePlaneNad1983Feet category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class StatePlaneNad1983Feet
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
        public void NAD1983StatePlaneAlabamaEastFIPS0101Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneAlabamaEastFIPS0101Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlabamaWestFIPS0102Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneAlabamaWestFIPS0102Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska10FIPS5010Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneAlaska10FIPS5010Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska1FIPS5001Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneAlaska1FIPS5001Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska2FIPS5002Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneAlaska2FIPS5002Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska3FIPS5003Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneAlaska3FIPS5003Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska4FIPS5004Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneAlaska4FIPS5004Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska5FIPS5005Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneAlaska5FIPS5005Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska6FIPS5006Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneAlaska6FIPS5006Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska7FIPS5007Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneAlaska7FIPS5007Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska8FIPS5008Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneAlaska8FIPS5008Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska9FIPS5009Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneAlaska9FIPS5009Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneArizonaCentralFIPS0202Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneArizonaCentralFIPS0202Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneArizonaEastFIPS0201Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneArizonaEastFIPS0201Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneArizonaWestFIPS0203Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneArizonaWestFIPS0203Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneArkansasNorthFIPS0301Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneArkansasNorthFIPS0301Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneArkansasSouthFIPS0302Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneArkansasSouthFIPS0302Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneCaliforniaIFIPS0401Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneCaliforniaIFIPS0401Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneCaliforniaIIFIPS0402Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneCaliforniaIIFIPS0402Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneCaliforniaIIIFIPS0403Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneCaliforniaIIIFIPS0403Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneCaliforniaIVFIPS0404Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneCaliforniaIVFIPS0404Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneCaliforniaVFIPS0405Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneCaliforniaVFIPS0405Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneCaliforniaVIFIPS0406Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneCaliforniaVIFIPS0406Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneColoradoCentralFIPS0502Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneColoradoCentralFIPS0502Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneColoradoNorthFIPS0501Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneColoradoNorthFIPS0501Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneColoradoSouthFIPS0503Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneColoradoSouthFIPS0503Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneConnecticutFIPS0600Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneConnecticutFIPS0600Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneDelawareFIPS0700Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneDelawareFIPS0700Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneFloridaEastFIPS0901Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneFloridaEastFIPS0901Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneFloridaNorthFIPS0903Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneFloridaNorthFIPS0903Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneFloridaWestFIPS0902Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneFloridaWestFIPS0902Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneGeorgiaEastFIPS1001Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneGeorgiaEastFIPS1001Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneGeorgiaWestFIPS1002Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneGeorgiaWestFIPS1002Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneGuamFIPS5400Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneGuamFIPS5400Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneHawaii1FIPS5101Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneHawaii1FIPS5101Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneHawaii2FIPS5102Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneHawaii2FIPS5102Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneHawaii3FIPS5103Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneHawaii3FIPS5103Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneHawaii4FIPS5104Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneHawaii4FIPS5104Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneHawaii5FIPS5105Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneHawaii5FIPS5105Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneIdahoCentralFIPS1102Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneIdahoCentralFIPS1102Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneIdahoEastFIPS1101Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneIdahoEastFIPS1101Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneIdahoWestFIPS1103Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneIdahoWestFIPS1103Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneIllinoisEastFIPS1201Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneIllinoisEastFIPS1201Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneIllinoisWestFIPS1202Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneIllinoisWestFIPS1202Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneIndianaEastFIPS1301Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneIndianaEastFIPS1301Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneIndianaWestFIPS1302Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneIndianaWestFIPS1302Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneIowaNorthFIPS1401Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneIowaNorthFIPS1401Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneIowaSouthFIPS1402Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneIowaSouthFIPS1402Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneKansasNorthFIPS1501Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneKansasNorthFIPS1501Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneKansasSouthFIPS1502Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneKansasSouthFIPS1502Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneKentuckyFIPS1600Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneKentuckyFIPS1600Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneKentuckyNorthFIPS1601Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneKentuckyNorthFIPS1601Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneKentuckySouthFIPS1602Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneKentuckySouthFIPS1602Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneLouisianaNorthFIPS1701Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneLouisianaNorthFIPS1701Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneLouisianaSouthFIPS1702Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneLouisianaSouthFIPS1702Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMaineEastFIPS1801Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneMaineEastFIPS1801Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMaineWestFIPS1802Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneMaineWestFIPS1802Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMarylandFIPS1900Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneMarylandFIPS1900Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMassachusettsIslandFIPS2002Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneMassachusettsIslandFIPS2002Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMassachusettsMainlandFIPS2001Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneMassachusettsMainlandFIPS2001Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMichiganCentralFIPS2112Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneMichiganCentralFIPS2112Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMichiganNorthFIPS2111Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneMichiganNorthFIPS2111Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMichiganSouthFIPS2113Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneMichiganSouthFIPS2113Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMinnesotaCentralFIPS2202Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneMinnesotaCentralFIPS2202Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMinnesotaNorthFIPS2201Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneMinnesotaNorthFIPS2201Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMinnesotaSouthFIPS2203Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneMinnesotaSouthFIPS2203Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMississippiEastFIPS2301Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneMississippiEastFIPS2301Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMississippiWestFIPS2302Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneMississippiWestFIPS2302Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMissouriCentralFIPS2402Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneMissouriCentralFIPS2402Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMissouriEastFIPS2401Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneMissouriEastFIPS2401Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMissouriWestFIPS2403Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneMissouriWestFIPS2403Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMontanaFIPS2500Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneMontanaFIPS2500Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNebraskaFIPS2600Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneNebraskaFIPS2600Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNevadaCentralFIPS2702Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneNevadaCentralFIPS2702Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNevadaEastFIPS2701Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneNevadaEastFIPS2701Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNevadaWestFIPS2703Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneNevadaWestFIPS2703Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNewHampshireFIPS2800Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneNewHampshireFIPS2800Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNewJerseyFIPS2900Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneNewJerseyFIPS2900Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNewMexicoCentralFIPS3002Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneNewMexicoCentralFIPS3002Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNewMexicoEastFIPS3001Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneNewMexicoEastFIPS3001Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNewMexicoWestFIPS3003Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneNewMexicoWestFIPS3003Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNewYorkCentralFIPS3102Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneNewYorkCentralFIPS3102Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNewYorkEastFIPS3101Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneNewYorkEastFIPS3101Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNewYorkLongIslandFIPS3104Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneNewYorkLongIslandFIPS3104Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNewYorkWestFIPS3103Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneNewYorkWestFIPS3103Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNorthCarolinaFIPS3200Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneNorthCarolinaFIPS3200Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNorthDakotaNorthFIPS3301Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneNorthDakotaNorthFIPS3301Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNorthDakotaSouthFIPS3302Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneNorthDakotaSouthFIPS3302Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneOhioNorthFIPS3401Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneOhioNorthFIPS3401Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneOhioSouthFIPS3402Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneOhioSouthFIPS3402Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneOklahomaNorthFIPS3501Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneOklahomaNorthFIPS3501Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneOklahomaSouthFIPS3502Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneOklahomaSouthFIPS3502Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneOregonNorthFIPS3601Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneOregonNorthFIPS3601Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneOregonSouthFIPS3602Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneOregonSouthFIPS3602Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlanePennsylvaniaNorthFIPS3701Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlanePennsylvaniaNorthFIPS3701Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlanePennsylvaniaSouthFIPS3702Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlanePennsylvaniaSouthFIPS3702Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlanePRVirginIslandsFIPS5200Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlanePRVirginIslandsFIPS5200Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneRhodeIslandFIPS3800Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneRhodeIslandFIPS3800Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneSouthCarolinaFIPS3900Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneSouthCarolinaFIPS3900Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneSouthDakotaNorthFIPS4001Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneSouthDakotaNorthFIPS4001Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneSouthDakotaSouthFIPS4002Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneSouthDakotaSouthFIPS4002Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneTennesseeFIPS4100Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneTennesseeFIPS4100Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneTexasCentralFIPS4203Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneTexasCentralFIPS4203Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneTexasNorthCentralFIPS4202Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneTexasNorthCentralFIPS4202Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneTexasNorthFIPS4201Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneTexasNorthFIPS4201Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneTexasSouthCentralFIPS4204Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneTexasSouthCentralFIPS4204Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneTexasSouthFIPS4205Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneTexasSouthFIPS4205Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneUtahCentralFIPS4302Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneUtahCentralFIPS4302Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneUtahNorthFIPS4301Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneUtahNorthFIPS4301Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneUtahSouthFIPS4303Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneUtahSouthFIPS4303Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneVermontFIPS4400Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneVermontFIPS4400Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneVirginiaNorthFIPS4501Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneVirginiaNorthFIPS4501Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneVirginiaSouthFIPS4502Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneVirginiaSouthFIPS4502Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWashingtonNorthFIPS4601Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneWashingtonNorthFIPS4601Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWashingtonSouthFIPS4602Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneWashingtonSouthFIPS4602Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWestVirginiaNorthFIPS4701Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneWestVirginiaNorthFIPS4701Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWestVirginiaSouthFIPS4702Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneWestVirginiaSouthFIPS4702Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWisconsinCentralFIPS4802Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneWisconsinCentralFIPS4802Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWisconsinNorthFIPS4801Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneWisconsinNorthFIPS4801Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWisconsinSouthFIPS4803Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneWisconsinSouthFIPS4803Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWyomingEastCentralFIPS4902Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneWyomingEastCentralFIPS4902Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWyomingEastFIPS4901Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneWyomingEastFIPS4901Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWyomingWestCentralFIPS4903Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneWyomingWestCentralFIPS4903Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWyomingWestFIPS4904Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Feet.NAD1983StatePlaneWyomingWestFIPS4904Feet;
            Tester.TestProjection(pStart);
        }
    }
}