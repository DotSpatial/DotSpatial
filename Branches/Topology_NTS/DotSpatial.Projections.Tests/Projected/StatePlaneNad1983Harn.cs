using System.IO;
using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the StatePlaneNad1983Harn category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class StatePlaneNad1983Harn
    {
        /// <summary>
        /// Creates a new instance of the Africa Class
        /// </summary>
        [TestFixtureSetUp]
        public void Initialize()
        {
            
        }

        [Test]
        public void NAD1983HARNMaine2000CentralZone()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNMaine2000CentralZone;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNMaine2000EastZone()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNMaine2000EastZone;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNMaine2000WestZone()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNMaine2000WestZone;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneAlabamaEastFIPS0101()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneAlabamaEastFIPS0101;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneAlabamaWestFIPS0102()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneAlabamaWestFIPS0102;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneArizonaCentralFIPS0202()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneArizonaCentralFIPS0202;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneArizonaEastFIPS0201()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneArizonaEastFIPS0201;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneArizonaWestFIPS0203()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneArizonaWestFIPS0203;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneArkansasNorthFIPS0301()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneArkansasNorthFIPS0301;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneArkansasSouthFIPS0302()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneArkansasSouthFIPS0302;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneCaliforniaIFIPS0401()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneCaliforniaIFIPS0401;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneCaliforniaIIFIPS0402()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneCaliforniaIIFIPS0402;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneCaliforniaIIIFIPS0403()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneCaliforniaIIIFIPS0403;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneCaliforniaIVFIPS0404()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneCaliforniaIVFIPS0404;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneCaliforniaVFIPS0405()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneCaliforniaVFIPS0405;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneCaliforniaVIFIPS0406()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneCaliforniaVIFIPS0406;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneColoradoCentralFIPS0502()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneColoradoCentralFIPS0502;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneColoradoNorthFIPS0501()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneColoradoNorthFIPS0501;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneColoradoSouthFIPS0503()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneColoradoSouthFIPS0503;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneConnecticutFIPS0600()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneConnecticutFIPS0600;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneDelawareFIPS0700()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneDelawareFIPS0700;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneFloridaEastFIPS0901()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneFloridaEastFIPS0901;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneFloridaNorthFIPS0903()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneFloridaNorthFIPS0903;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneFloridaWestFIPS0902()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneFloridaWestFIPS0902;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneGeorgiaEastFIPS1001()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneGeorgiaEastFIPS1001;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneGeorgiaWestFIPS1002()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneGeorgiaWestFIPS1002;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneHawaii1FIPS5101()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneHawaii1FIPS5101;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneHawaii2FIPS5102()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneHawaii2FIPS5102;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneHawaii3FIPS5103()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneHawaii3FIPS5103;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneHawaii4FIPS5104()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneHawaii4FIPS5104;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneHawaii5FIPS5105()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneHawaii5FIPS5105;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneIdahoCentralFIPS1102()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneIdahoCentralFIPS1102;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneIdahoEastFIPS1101()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneIdahoEastFIPS1101;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneIdahoWestFIPS1103()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneIdahoWestFIPS1103;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneIllinoisEastFIPS1201()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneIllinoisEastFIPS1201;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneIllinoisWestFIPS1202()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneIllinoisWestFIPS1202;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneIndianaEastFIPS1301()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneIndianaEastFIPS1301;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneIndianaWestFIPS1302()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneIndianaWestFIPS1302;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneIowaNorthFIPS1401()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneIowaNorthFIPS1401;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneIowaSouthFIPS1402()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneIowaSouthFIPS1402;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneKansasNorthFIPS1501()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneKansasNorthFIPS1501;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneKansasSouthFIPS1502()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneKansasSouthFIPS1502;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneKentuckyNorthFIPS1601()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneKentuckyNorthFIPS1601;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneKentuckySouthFIPS1602()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneKentuckySouthFIPS1602;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneLouisianaNorthFIPS1701()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneLouisianaNorthFIPS1701;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneLouisianaSouthFIPS1702()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneLouisianaSouthFIPS1702;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMaineEastFIPS1801()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneMaineEastFIPS1801;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMaineWestFIPS1802()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneMaineWestFIPS1802;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMarylandFIPS1900()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneMarylandFIPS1900;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMassachusettsIslandFIPS2002()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneMassachusettsIslandFIPS2002;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMassachusettsMainlandFIPS2001()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneMassachusettsMainlandFIPS2001;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMichiganCentralFIPS2112()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneMichiganCentralFIPS2112;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMichiganNorthFIPS2111()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneMichiganNorthFIPS2111;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMichiganSouthFIPS2113()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneMichiganSouthFIPS2113;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMinnesotaCentralFIPS2202()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneMinnesotaCentralFIPS2202;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMinnesotaNorthFIPS2201()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneMinnesotaNorthFIPS2201;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMinnesotaSouthFIPS2203()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneMinnesotaSouthFIPS2203;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMississippiEastFIPS2301()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneMississippiEastFIPS2301;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMississippiWestFIPS2302()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneMississippiWestFIPS2302;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMissouriCentralFIPS2402()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneMissouriCentralFIPS2402;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMissouriEastFIPS2401()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneMissouriEastFIPS2401;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMissouriWestFIPS2403()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneMissouriWestFIPS2403;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMontanaFIPS2500()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneMontanaFIPS2500;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNebraskaFIPS2600()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneNebraskaFIPS2600;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNevadaCentralFIPS2702()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneNevadaCentralFIPS2702;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNevadaEastFIPS2701()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneNevadaEastFIPS2701;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNevadaWestFIPS2703()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneNevadaWestFIPS2703;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNewHampshireFIPS2800()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneNewHampshireFIPS2800;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNewJerseyFIPS2900()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneNewJerseyFIPS2900;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNewMexicoCentralFIPS3002()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneNewMexicoCentralFIPS3002;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNewMexicoEastFIPS3001()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneNewMexicoEastFIPS3001;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNewMexicoWestFIPS3003()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneNewMexicoWestFIPS3003;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNewYorkCentralFIPS3102()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneNewYorkCentralFIPS3102;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNewYorkEastFIPS3101()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneNewYorkEastFIPS3101;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNewYorkLongIslandFIPS3104()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneNewYorkLongIslandFIPS3104;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNewYorkWestFIPS3103()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneNewYorkWestFIPS3103;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNorthDakotaNorthFIPS3301()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneNorthDakotaNorthFIPS3301;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNorthDakotaSouthFIPS3302()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneNorthDakotaSouthFIPS3302;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneOhioNorthFIPS3401()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneOhioNorthFIPS3401;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneOhioSouthFIPS3402()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneOhioSouthFIPS3402;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneOklahomaNorthFIPS3501()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneOklahomaNorthFIPS3501;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneOklahomaSouthFIPS3502()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneOklahomaSouthFIPS3502;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneOregonNorthFIPS3601()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneOregonNorthFIPS3601;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneOregonSouthFIPS3602()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneOregonSouthFIPS3602;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlanePRVirginIslandsFIPS5200()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlanePRVirginIslandsFIPS5200;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneRhodeIslandFIPS3800()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneRhodeIslandFIPS3800;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneSouthDakotaNorthFIPS4001()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneSouthDakotaNorthFIPS4001;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneSouthDakotaSouthFIPS4002()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneSouthDakotaSouthFIPS4002;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneTennesseeFIPS4100()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneTennesseeFIPS4100;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneTexasCentralFIPS4203()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneTexasCentralFIPS4203;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneTexasNorthCentralFIPS4202()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneTexasNorthCentralFIPS4202;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneTexasNorthFIPS4201()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneTexasNorthFIPS4201;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneTexasSouthCentralFIPS4204()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneTexasSouthCentralFIPS4204;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneTexasSouthFIPS4205()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneTexasSouthFIPS4205;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneUtahCentralFIPS4302()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneUtahCentralFIPS4302;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneUtahNorthFIPS4301()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneUtahNorthFIPS4301;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneUtahSouthFIPS4303()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneUtahSouthFIPS4303;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneVermontFIPS4400()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneVermontFIPS4400;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneVirginiaNorthFIPS4501()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneVirginiaNorthFIPS4501;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneVirginiaSouthFIPS4502()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneVirginiaSouthFIPS4502;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneWashingtonNorthFIPS4601()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneWashingtonNorthFIPS4601;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneWashingtonSouthFIPS4602()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneWashingtonSouthFIPS4602;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneWestVirginiaNorthFIPS4701()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneWestVirginiaNorthFIPS4701;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneWestVirginiaSouthFIPS4702()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneWestVirginiaSouthFIPS4702;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneWisconsinCentralFIPS4802()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneWisconsinCentralFIPS4802;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneWisconsinNorthFIPS4801()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneWisconsinNorthFIPS4801;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneWisconsinSouthFIPS4803()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneWisconsinSouthFIPS4803;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneWyomingEastFIPS4901()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneWyomingEastFIPS4901;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneWyomingECFIPS4902()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneWyomingECFIPS4902;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneWyomingWCFIPS4903()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneWyomingWCFIPS4903;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneWyomingWestFIPS4904()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983Harn.NAD1983HARNStatePlaneWyomingWestFIPS4904;
            Tester.TestProjection(pStart);
        }
    }
}