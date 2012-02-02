using System.IO;
using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the StatePlaneNad1983HarnFeet category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class StatePlaneNad1983HarnFeet
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
        public void NAD1983HARNStatePlaneArizonaCentralFIPS0202FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneArizonaCentralFIPS0202FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneArizonaEastFIPS0201FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneArizonaEastFIPS0201FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneArizonaWestFIPS0203FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneArizonaWestFIPS0203FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneCaliforniaIFIPS0401Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneCaliforniaIFIPS0401Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneCaliforniaIIFIPS0402Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneCaliforniaIIFIPS0402Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneCaliforniaIIIFIPS0403Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneCaliforniaIIIFIPS0403Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneCaliforniaIVFIPS0404Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneCaliforniaIVFIPS0404Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneCaliforniaVFIPS0405Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneCaliforniaVFIPS0405Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneCaliforniaVIFIPS0406Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneCaliforniaVIFIPS0406Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneColoradoCentralFIPS0502Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneColoradoCentralFIPS0502Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneColoradoNorthFIPS0501Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneColoradoNorthFIPS0501Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneColoradoSouthFIPS0503Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneColoradoSouthFIPS0503Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneConnecticutFIPS0600Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneConnecticutFIPS0600Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneDelawareFIPS0700Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneDelawareFIPS0700Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneFloridaEastFIPS0901Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneFloridaEastFIPS0901Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneFloridaNorthFIPS0903Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneFloridaNorthFIPS0903Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneFloridaWestFIPS0902Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneFloridaWestFIPS0902Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneGeorgiaEastFIPS1001Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneGeorgiaEastFIPS1001Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneGeorgiaWestFIPS1002Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneGeorgiaWestFIPS1002Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneHawaii1FIPS5101Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneHawaii1FIPS5101Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneHawaii2FIPS5102Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneHawaii2FIPS5102Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneHawaii3FIPS5103Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneHawaii3FIPS5103Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneHawaii4FIPS5104Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneHawaii4FIPS5104Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneHawaii5FIPS5105Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneHawaii5FIPS5105Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneIdahoCentralFIPS1102Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneIdahoCentralFIPS1102Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneIdahoEastFIPS1101Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneIdahoEastFIPS1101Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneIdahoWestFIPS1103Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneIdahoWestFIPS1103Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneIndianaEastFIPS1301Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneIndianaEastFIPS1301Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneIndianaWestFIPS1302Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneIndianaWestFIPS1302Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneKentuckyNorthFIPS1601Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneKentuckyNorthFIPS1601Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneKentuckySouthFIPS1602Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneKentuckySouthFIPS1602Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMarylandFIPS1900Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneMarylandFIPS1900Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMassachusettsIslandFIPS2002Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneMassachusettsIslandFIPS2002Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMassachusettsMainlandFIPS2001Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneMassachusettsMainlandFIPS2001Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMichiganCentralFIPS2112FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneMichiganCentralFIPS2112FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMichiganNorthFIPS2111FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneMichiganNorthFIPS2111FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMichiganSouthFIPS2113FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneMichiganSouthFIPS2113FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMississippiEastFIPS2301Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneMississippiEastFIPS2301Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMississippiWestFIPS2302Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneMississippiWestFIPS2302Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneMontanaFIPS2500FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneMontanaFIPS2500FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNewMexicoCentralFIPS3002Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneNewMexicoCentralFIPS3002Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNewMexicoEastFIPS3001Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneNewMexicoEastFIPS3001Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNewMexicoWestFIPS3003Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneNewMexicoWestFIPS3003Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNewYorkCentralFIPS3102Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneNewYorkCentralFIPS3102Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNewYorkEastFIPS3101Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneNewYorkEastFIPS3101Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNewYorkLongIslandFIPS3104Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneNewYorkLongIslandFIPS3104Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNewYorkWestFIPS3103Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneNewYorkWestFIPS3103Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNorthDakotaNorthFIPS3301FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneNorthDakotaNorthFIPS3301FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneNorthDakotaSouthFIPS3302FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneNorthDakotaSouthFIPS3302FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneOklahomaNorthFIPS3501Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneOklahomaNorthFIPS3501Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneOklahomaSouthFIPS3502Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneOklahomaSouthFIPS3502Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneOregonNorthFIPS3601FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneOregonNorthFIPS3601FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneOregonSouthFIPS3602FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneOregonSouthFIPS3602FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneTennesseeFIPS4100Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneTennesseeFIPS4100Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneTexasCentralFIPS4203Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneTexasCentralFIPS4203Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneTexasNorthCentralFIPS4202Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneTexasNorthCentralFIPS4202Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneTexasNorthFIPS4201Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneTexasNorthFIPS4201Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneTexasSouthCentralFIPS4204Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneTexasSouthCentralFIPS4204Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneTexasSouthFIPS4205Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneTexasSouthFIPS4205Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneUtahCentralFIPS4302FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneUtahCentralFIPS4302FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneUtahNorthFIPS4301FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneUtahNorthFIPS4301FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneUtahSouthFIPS4303FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneUtahSouthFIPS4303FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneVirginiaNorthFIPS4501Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneVirginiaNorthFIPS4501Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneVirginiaSouthFIPS4502Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneVirginiaSouthFIPS4502Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneWashingtonNorthFIPS4601Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneWashingtonNorthFIPS4601Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneWashingtonSouthFIPS4602Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneWashingtonSouthFIPS4602Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneWisconsinCentralFIPS4802Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneWisconsinCentralFIPS4802Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneWisconsinNorthFIPS4801Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneWisconsinNorthFIPS4801Feet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNStatePlaneWisconsinSouthFIPS4803Feet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.StatePlaneNad1983HarnFeet.NAD1983HARNStatePlaneWisconsinSouthFIPS4803Feet;
            Tester.TestProjection(pStart);
        }
    }
}