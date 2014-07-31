using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the StatePlaneNad1927 category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    [Ignore("Tests fails only on x64. It seems proj.dll issue.")]
    public class StatePlaneNad1927
    {
        /// <summary>
        /// Creates a new instance of the Africa Class
        /// </summary>
        [TestFixtureSetUp]
        public void Initialize()
        {
            
        }

        [Test]
        public void NAD1927StatePlaneAlabamaEastFIPS0101()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneAlabamaEastFIPS0101;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927StatePlaneAlabamaWestFIPS0102()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneAlabamaWestFIPS0102;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927StatePlaneAlaska10FIPS5010()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneAlaska10FIPS5010;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore("Verify")]
        public void NAD1927StatePlaneAlaska1FIPS5001()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneAlaska1FIPS5001;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927StatePlaneAlaska2FIPS5002()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneAlaska2FIPS5002;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927StatePlaneAlaska3FIPS5003()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneAlaska3FIPS5003;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927StatePlaneAlaska4FIPS5004()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneAlaska4FIPS5004;
            Tester.TestProjection(pStart);
        }

        [Test]
        public void NAD1927StatePlaneAlaska5FIPS5005()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneAlaska5FIPS5005;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927StatePlaneAlaska6FIPS5006()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneAlaska6FIPS5006;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927StatePlaneAlaska7FIPS5007()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneAlaska7FIPS5007;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927StatePlaneAlaska8FIPS5008()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneAlaska8FIPS5008;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927StatePlaneAlaska9FIPS5009()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneAlaska9FIPS5009;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927StatePlaneArizonaCentralFIPS0202()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneArizonaCentralFIPS0202;
            Tester.TestProjection(pStart);
        }


        [Test]
        
        public void NAD1927StatePlaneArizonaEastFIPS0201()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneArizonaEastFIPS0201;
            Tester.TestProjection(pStart);
        }


        [Test]
        
        public void NAD1927StatePlaneArizonaWestFIPS0203()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneArizonaWestFIPS0203;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneArkansasNorthFIPS0301()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneArkansasNorthFIPS0301;
            Tester.TestProjection(pStart);
        }


        [Test]
        
        public void NAD1927StatePlaneArkansasSouthFIPS0302()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneArkansasSouthFIPS0302;
            Tester.TestProjection(pStart);
        }


        [Test]
        
        public void NAD1927StatePlaneCaliforniaIFIPS0401()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneCaliforniaIFIPS0401;
            Tester.TestProjection(pStart);
        }


        [Test]
        
        public void NAD1927StatePlaneCaliforniaIIFIPS0402()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneCaliforniaIIFIPS0402;
            Tester.TestProjection(pStart);
        }


        [Test]
        
        public void NAD1927StatePlaneCaliforniaIIIFIPS0403()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneCaliforniaIIIFIPS0403;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneCaliforniaIVFIPS0404()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneCaliforniaIVFIPS0404;
            Tester.TestProjection(pStart);
        }


        [Test]
        
        public void NAD1927StatePlaneCaliforniaVFIPS0405()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneCaliforniaVFIPS0405;
            Tester.TestProjection(pStart);
        }


        [Test]
        
        public void NAD1927StatePlaneCaliforniaVIFIPS0406()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneCaliforniaVIFIPS0406;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneCaliforniaVIIFIPS0407()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneCaliforniaVIIFIPS0407;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneColoradoCentralFIPS0502()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneColoradoCentralFIPS0502;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneColoradoNorthFIPS0501()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneColoradoNorthFIPS0501;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneColoradoSouthFIPS0503()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneColoradoSouthFIPS0503;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneConnecticutFIPS0600()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneConnecticutFIPS0600;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneDelawareFIPS0700()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneDelawareFIPS0700;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneFloridaEastFIPS0901()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneFloridaEastFIPS0901;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneFloridaNorthFIPS0903()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneFloridaNorthFIPS0903;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneFloridaWestFIPS0902()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneFloridaWestFIPS0902;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneGeorgiaEastFIPS1001()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneGeorgiaEastFIPS1001;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneGeorgiaWestFIPS1002()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneGeorgiaWestFIPS1002;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneGuamFIPS5400()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneGuamFIPS5400;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneIdahoCentralFIPS1102()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneIdahoCentralFIPS1102;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneIdahoEastFIPS1101()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneIdahoEastFIPS1101;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneIdahoWestFIPS1103()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneIdahoWestFIPS1103;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneIllinoisEastFIPS1201()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneIllinoisEastFIPS1201;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneIllinoisWestFIPS1202()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneIllinoisWestFIPS1202;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneIndianaEastFIPS1301()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneIndianaEastFIPS1301;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneIndianaWestFIPS1302()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneIndianaWestFIPS1302;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneIowaNorthFIPS1401()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneIowaNorthFIPS1401;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneIowaSouthFIPS1402()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneIowaSouthFIPS1402;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneKansasNorthFIPS1501()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneKansasNorthFIPS1501;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneKansasSouthFIPS1502()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneKansasSouthFIPS1502;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneKentuckyNorthFIPS1601()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneKentuckyNorthFIPS1601;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneKentuckySouthFIPS1602()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneKentuckySouthFIPS1602;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneLouisianaNorthFIPS1701()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneLouisianaNorthFIPS1701;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneLouisianaSouthFIPS1702()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneLouisianaSouthFIPS1702;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneMaineEastFIPS1801()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMaineEastFIPS1801;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneMaineWestFIPS1802()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMaineWestFIPS1802;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneMarylandFIPS1900()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMarylandFIPS1900;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneMassachusettsIslandFIPS2002()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMassachusettsIslandFIPS2002;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneMassachusettsMainlandFIPS2001()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMassachusettsMainlandFIPS2001;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneMichiganCentralFIPS2112()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMichiganCentralFIPS2112;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneMichiganNorthFIPS2111()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMichiganNorthFIPS2111;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneMichiganSouthFIPS2113()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMichiganSouthFIPS2113;
            Tester.TestProjection(pStart);
        }
       
        [Test]
        public void NAD1927StatePlaneMinnesotaCentralFIPS2202()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMinnesotaCentralFIPS2202;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneMinnesotaNorthFIPS2201()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMinnesotaNorthFIPS2201;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneMinnesotaSouthFIPS2203()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMinnesotaSouthFIPS2203;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneMississippiEastFIPS2301()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMississippiEastFIPS2301;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneMississippiWestFIPS2302()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMississippiWestFIPS2302;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneMissouriCentralFIPS2402()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMissouriCentralFIPS2402;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneMissouriEastFIPS2401()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMissouriEastFIPS2401;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneMissouriWestFIPS2403()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMissouriWestFIPS2403;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneMontanaCentralFIPS2502()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMontanaCentralFIPS2502;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneMontanaNorthFIPS2501()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMontanaNorthFIPS2501;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneMontanaSouthFIPS2503()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneMontanaSouthFIPS2503;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneNebraskaNorthFIPS2601()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneNebraskaNorthFIPS2601;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneNebraskaSouthFIPS2602()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneNebraskaSouthFIPS2602;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneNevadaCentralFIPS2702()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneNevadaCentralFIPS2702;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneNevadaEastFIPS2701()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneNevadaEastFIPS2701;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneNevadaWestFIPS2703()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneNevadaWestFIPS2703;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneNewHampshireFIPS2800()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneNewHampshireFIPS2800;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneNewJerseyFIPS2900()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneNewJerseyFIPS2900;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneNewMexicoCentralFIPS3002()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneNewMexicoCentralFIPS3002;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneNewMexicoEastFIPS3001()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneNewMexicoEastFIPS3001;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneNewMexicoWestFIPS3003()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneNewMexicoWestFIPS3003;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneNewYorkCentralFIPS3102()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneNewYorkCentralFIPS3102;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneNewYorkEastFIPS3101()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneNewYorkEastFIPS3101;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneNewYorkLongIslandFIPS3104()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneNewYorkLongIslandFIPS3104;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneNewYorkWestFIPS3103()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneNewYorkWestFIPS3103;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneNorthCarolinaFIPS3200()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneNorthCarolinaFIPS3200;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneNorthDakotaNorthFIPS3301()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneNorthDakotaNorthFIPS3301;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneNorthDakotaSouthFIPS3302()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneNorthDakotaSouthFIPS3302;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneOhioNorthFIPS3401()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneOhioNorthFIPS3401;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneOhioSouthFIPS3402()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneOhioSouthFIPS3402;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneOklahomaNorthFIPS3501()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneOklahomaNorthFIPS3501;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneOklahomaSouthFIPS3502()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneOklahomaSouthFIPS3502;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneOregonNorthFIPS3601()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneOregonNorthFIPS3601;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneOregonSouthFIPS3602()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneOregonSouthFIPS3602;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlanePennsylvaniaNorthFIPS3701()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlanePennsylvaniaNorthFIPS3701;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlanePennsylvaniaSouthFIPS3702()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlanePennsylvaniaSouthFIPS3702;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlanePuertoRicoFIPS5201()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlanePuertoRicoFIPS5201;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneRhodeIslandFIPS3800()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneRhodeIslandFIPS3800;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneSouthCarolinaNorthFIPS3901()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneSouthCarolinaNorthFIPS3901;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneSouthCarolinaSouthFIPS3902()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneSouthCarolinaSouthFIPS3902;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneSouthDakotaNorthFIPS4001()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneSouthDakotaNorthFIPS4001;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneSouthDakotaSouthFIPS4002()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneSouthDakotaSouthFIPS4002;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneTennesseeFIPS4100()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneTennesseeFIPS4100;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneTexasCentralFIPS4203()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneTexasCentralFIPS4203;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneTexasNorthCentralFIPS4202()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneTexasNorthCentralFIPS4202;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneTexasNorthFIPS4201()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneTexasNorthFIPS4201;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneTexasSouthCentralFIPS4204()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneTexasSouthCentralFIPS4204;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneTexasSouthFIPS4205()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneTexasSouthFIPS4205;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneUtahCentralFIPS4302()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneUtahCentralFIPS4302;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneUtahNorthFIPS4301()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneUtahNorthFIPS4301;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneUtahSouthFIPS4303()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneUtahSouthFIPS4303;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneVermontFIPS3400()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneVermontFIPS3400;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneVirginiaNorthFIPS4501()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneVirginiaNorthFIPS4501;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneVirginiaSouthFIPS4502()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneVirginiaSouthFIPS4502;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneWashingtonNorthFIPS4601()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneWashingtonNorthFIPS4601;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneWashingtonSouthFIPS4602()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneWashingtonSouthFIPS4602;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneWestVirginiaNorthFIPS4701()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneWestVirginiaNorthFIPS4701;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneWestVirginiaSouthFIPS4702()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneWestVirginiaSouthFIPS4702;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneWisconsinCentralFIPS4802()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneWisconsinCentralFIPS4802;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneWisconsinNorthFIPS4801()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneWisconsinNorthFIPS4801;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneWisconsinSouthFIPS4803()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneWisconsinSouthFIPS4803;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneWyomingEastCentralFIPS4902()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneWyomingEastCentralFIPS4902;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneWyomingEastFIPS4901()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneWyomingEastFIPS4901;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneWyomingWestCentralFIPS4903()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneWyomingWestCentralFIPS4903;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927StatePlaneWyomingWestFIPS4904()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1927.NAD1927StatePlaneWyomingWestFIPS4904;
            Tester.TestProjection(pStart);
        }
    }
}