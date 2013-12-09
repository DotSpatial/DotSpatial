
using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the StatePlaneNad1983 category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class StatePlaneNad1983
    {

        /// <summary>
        /// Creates a new instance of the Africa Class
        /// </summary>
        [TestFixtureSetUp]
        public void Initialize()
        {
            
        }

        [Test]
        public void MichiganGeoRef2008()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.MichiganGeoRef2008;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983Maine2000CentralZone()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983Maine2000CentralZone;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983Maine2000EastZone()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983Maine2000EastZone;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983Maine2000WestZone()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983Maine2000WestZone;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlabamaEastFIPS0101()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneAlabamaEastFIPS0101;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlabamaWestFIPS0102()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneAlabamaWestFIPS0102;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska10FIPS5010()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneAlaska10FIPS5010;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska1FIPS5001()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneAlaska1FIPS5001;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska2FIPS5002()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneAlaska2FIPS5002;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska3FIPS5003()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneAlaska3FIPS5003;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska4FIPS5004()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneAlaska4FIPS5004;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska5FIPS5005()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneAlaska5FIPS5005;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska6FIPS5006()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneAlaska6FIPS5006;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska7FIPS5007()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneAlaska7FIPS5007;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska8FIPS5008()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneAlaska8FIPS5008;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneAlaska9FIPS5009()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneAlaska9FIPS5009;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneArizonaCentralFIPS0202()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneArizonaCentralFIPS0202;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneArizonaEastFIPS0201()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneArizonaEastFIPS0201;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneArizonaWestFIPS0203()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneArizonaWestFIPS0203;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneArkansasNorthFIPS0301()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneArkansasNorthFIPS0301;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneArkansasSouthFIPS0302()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneArkansasSouthFIPS0302;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneCaliforniaIFIPS0401()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneCaliforniaIFIPS0401;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneCaliforniaIIFIPS0402()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneCaliforniaIIFIPS0402;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneCaliforniaIIIFIPS0403()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneCaliforniaIIIFIPS0403;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneCaliforniaIVFIPS0404()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneCaliforniaIVFIPS0404;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneCaliforniaVFIPS0405()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneCaliforniaVFIPS0405;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneCaliforniaVIFIPS0406()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneCaliforniaVIFIPS0406;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneColoradoCentralFIPS0502()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneColoradoCentralFIPS0502;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneColoradoNorthFIPS0501()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneColoradoNorthFIPS0501;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneColoradoSouthFIPS0503()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneColoradoSouthFIPS0503;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneConnecticutFIPS0600()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneConnecticutFIPS0600;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneDelawareFIPS0700()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneDelawareFIPS0700;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneFloridaEastFIPS0901()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneFloridaEastFIPS0901;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneFloridaNorthFIPS0903()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneFloridaNorthFIPS0903;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneFloridaWestFIPS0902()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneFloridaWestFIPS0902;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneGeorgiaEastFIPS1001()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneGeorgiaEastFIPS1001;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneGeorgiaWestFIPS1002()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneGeorgiaWestFIPS1002;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneGuamFIPS5400()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneGuamFIPS5400;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneHawaii1FIPS5101()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneHawaii1FIPS5101;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneHawaii2FIPS5102()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneHawaii2FIPS5102;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneHawaii3FIPS5103()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneHawaii3FIPS5103;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneHawaii4FIPS5104()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneHawaii4FIPS5104;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneHawaii5FIPS5105()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneHawaii5FIPS5105;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneIdahoCentralFIPS1102()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneIdahoCentralFIPS1102;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneIdahoEastFIPS1101()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneIdahoEastFIPS1101;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneIdahoWestFIPS1103()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneIdahoWestFIPS1103;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneIllinoisEastFIPS1201()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneIllinoisEastFIPS1201;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneIllinoisWestFIPS1202()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneIllinoisWestFIPS1202;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneIndianaEastFIPS1301()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneIndianaEastFIPS1301;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneIndianaWestFIPS1302()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneIndianaWestFIPS1302;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneIowaNorthFIPS1401()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneIowaNorthFIPS1401;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneIowaSouthFIPS1402()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneIowaSouthFIPS1402;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneKansasNorthFIPS1501()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneKansasNorthFIPS1501;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneKansasSouthFIPS1502()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneKansasSouthFIPS1502;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneKentuckyFIPS1600()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneKentuckyFIPS1600;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneKentuckyNorthFIPS1601()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneKentuckyNorthFIPS1601;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneKentuckySouthFIPS1602()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneKentuckySouthFIPS1602;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneLouisianaNorthFIPS1701()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneLouisianaNorthFIPS1701;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneLouisianaSouthFIPS1702()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneLouisianaSouthFIPS1702;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMaineEastFIPS1801()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneMaineEastFIPS1801;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMaineWestFIPS1802()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneMaineWestFIPS1802;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMarylandFIPS1900()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneMarylandFIPS1900;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMassachusettsIslandFIPS2002()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneMassachusettsIslandFIPS2002;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMassachusettsMainlandFIPS2001()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneMassachusettsMainlandFIPS2001;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMichiganCentralFIPS2112()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneMichiganCentralFIPS2112;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMichiganNorthFIPS2111()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneMichiganNorthFIPS2111;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMichiganSouthFIPS2113()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneMichiganSouthFIPS2113;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMinnesotaCentralFIPS2202()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneMinnesotaCentralFIPS2202;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMinnesotaNorthFIPS2201()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneMinnesotaNorthFIPS2201;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMinnesotaSouthFIPS2203()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneMinnesotaSouthFIPS2203;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMississippiEastFIPS2301()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneMississippiEastFIPS2301;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMississippiWestFIPS2302()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneMississippiWestFIPS2302;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMissouriCentralFIPS2402()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneMissouriCentralFIPS2402;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMissouriEastFIPS2401()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneMissouriEastFIPS2401;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMissouriWestFIPS2403()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneMissouriWestFIPS2403;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMontanaFIPS2500()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneMontanaFIPS2500;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNebraskaFIPS2600()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneNebraskaFIPS2600;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNevadaCentralFIPS2702()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneNevadaCentralFIPS2702;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNevadaEastFIPS2701()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneNevadaEastFIPS2701;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNevadaWestFIPS2703()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneNevadaWestFIPS2703;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNewHampshireFIPS2800()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneNewHampshireFIPS2800;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNewJerseyFIPS2900()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneNewJerseyFIPS2900;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNewMexicoCentralFIPS3002()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneNewMexicoCentralFIPS3002;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNewMexicoEastFIPS3001()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneNewMexicoEastFIPS3001;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNewMexicoWestFIPS3003()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneNewMexicoWestFIPS3003;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNewYorkCentralFIPS3102()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneNewYorkCentralFIPS3102;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNewYorkEastFIPS3101()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneNewYorkEastFIPS3101;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNewYorkLongIslandFIPS3104()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneNewYorkLongIslandFIPS3104;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNewYorkWestFIPS3103()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneNewYorkWestFIPS3103;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNorthCarolinaFIPS3200()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneNorthCarolinaFIPS3200;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNorthDakotaNorthFIPS3301()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneNorthDakotaNorthFIPS3301;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNorthDakotaSouthFIPS3302()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneNorthDakotaSouthFIPS3302;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneOhioNorthFIPS3401()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneOhioNorthFIPS3401;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneOhioSouthFIPS3402()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneOhioSouthFIPS3402;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneOklahomaNorthFIPS3501()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneOklahomaNorthFIPS3501;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneOklahomaSouthFIPS3502()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneOklahomaSouthFIPS3502;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneOregonNorthFIPS3601()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneOregonNorthFIPS3601;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneOregonSouthFIPS3602()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneOregonSouthFIPS3602;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlanePennsylvaniaNorthFIPS3701()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlanePennsylvaniaNorthFIPS3701;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlanePennsylvaniaSouthFIPS3702()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlanePennsylvaniaSouthFIPS3702;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlanePuertoRicoVirginIslandsFIPS5200()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlanePuertoRicoVirginIslandsFIPS5200;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneRhodeIslandFIPS3800()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneRhodeIslandFIPS3800;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneSouthCarolinaFIPS3900()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneSouthCarolinaFIPS3900;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneSouthDakotaNorthFIPS4001()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneSouthDakotaNorthFIPS4001;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneSouthDakotaSouthFIPS4002()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneSouthDakotaSouthFIPS4002;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneTennesseeFIPS4100()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneTennesseeFIPS4100;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneTexasCentralFIPS4203()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneTexasCentralFIPS4203;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneTexasNorthCentralFIPS4202()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneTexasNorthCentralFIPS4202;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneTexasNorthFIPS4201()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneTexasNorthFIPS4201;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneTexasSouthCentralFIPS4204()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneTexasSouthCentralFIPS4204;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneTexasSouthFIPS4205()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneTexasSouthFIPS4205;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneUtahCentralFIPS4302()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneUtahCentralFIPS4302;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneUtahNorthFIPS4301()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneUtahNorthFIPS4301;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneUtahSouthFIPS4303()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneUtahSouthFIPS4303;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneVermontFIPS4400()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneVermontFIPS4400;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneVirginiaNorthFIPS4501()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneVirginiaNorthFIPS4501;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneVirginiaSouthFIPS4502()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneVirginiaSouthFIPS4502;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWashingtonNorthFIPS4601()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneWashingtonNorthFIPS4601;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWashingtonSouthFIPS4602()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneWashingtonSouthFIPS4602;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWestVirginiaNorthFIPS4701()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneWestVirginiaNorthFIPS4701;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWestVirginiaSouthFIPS4702()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneWestVirginiaSouthFIPS4702;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWisconsinCentralFIPS4802()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneWisconsinCentralFIPS4802;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWisconsinNorthFIPS4801()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneWisconsinNorthFIPS4801;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWisconsinSouthFIPS4803()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneWisconsinSouthFIPS4803;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWyomingEastCentralFIPS4902()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneWyomingEastCentralFIPS4902;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWyomingEastFIPS4901()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneWyomingEastFIPS4901;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWyomingWestCentralFIPS4903()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneWyomingWestCentralFIPS4903;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneWyomingWestFIPS4904()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983.NAD1983StatePlaneWyomingWestFIPS4904;
            Tester.TestProjection(pStart);
        }
    }
}