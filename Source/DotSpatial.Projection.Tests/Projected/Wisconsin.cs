using System.IO;
using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the Wisconsin category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class Wisconsin
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
        public void NAD1983HARNAdjWIAdamsFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIAdamsFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIAdamsMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIAdamsMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIAshlandFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIAshlandFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIAshlandMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIAshlandMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIBarronFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIBarronFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIBarronMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIBarronMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIBayfieldFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIBayfieldFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIBayfieldMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIBayfieldMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIBrownFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIBrownFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIBrownMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIBrownMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIBuffaloFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIBuffaloFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIBuffaloMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIBuffaloMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIBurnettFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIBurnettFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIBurnettMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIBurnettMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWICalumetFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWICalumetFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWICalumetMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWICalumetMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIChippewaFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIChippewaFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIChippewaMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIChippewaMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIClarkFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIClarkFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIClarkMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIClarkMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIColumbiaFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIColumbiaFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIColumbiaMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIColumbiaMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWICrawfordFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWICrawfordFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWICrawfordMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWICrawfordMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIDaneFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIDaneFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIDaneMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIDaneMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIDodgeFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIDodgeFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIDodgeMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIDodgeMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIDoorFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIDoorFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIDoorMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIDoorMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIDouglasFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIDouglasFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIDouglasMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIDouglasMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIDunnFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIDunnFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIDunnMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIDunnMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIEauClaireFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIEauClaireFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIEauClaireMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIEauClaireMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIFlorenceFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIFlorenceFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIFlorenceMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIFlorenceMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIFondduLacFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIFondduLacFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIFondduLacMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIFondduLacMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIForestFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIForestFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIForestMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIForestMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIGrantFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIGrantFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIGrantMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIGrantMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIGreenFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIGreenFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIGreenLakeFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIGreenLakeFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIGreenLakeMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIGreenLakeMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIGreenMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIGreenMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIIowaFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIIowaFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIIowaMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIIowaMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIIronFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIIronFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIIronMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIIronMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIJacksonFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIJacksonFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIJacksonMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIJacksonMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIJeffersonFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIJeffersonFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIJeffersonMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIJeffersonMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIJuneauFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIJuneauFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIJuneauMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIJuneauMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIKenoshaFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIKenoshaFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIKenoshaMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIKenoshaMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIKewauneeFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIKewauneeFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIKewauneeMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIKewauneeMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWILaCrosseFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWILaCrosseFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWILaCrosseMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWILaCrosseMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWILafayetteFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWILafayetteFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWILafayetteMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWILafayetteMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWILangladeFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWILangladeFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWILangladeMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWILangladeMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWILincolnFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWILincolnFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWILincolnMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWILincolnMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIManitowocFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIManitowocFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIManitowocMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIManitowocMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIMarathonFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIMarathonFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIMarathonMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIMarathonMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIMarinetteFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIMarinetteFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIMarinetteMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIMarinetteMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIMarquetteFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIMarquetteFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIMarquetteMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIMarquetteMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIMenomineeFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIMenomineeFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIMenomineeMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIMenomineeMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIMilwaukeeFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIMilwaukeeFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIMilwaukeeMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIMilwaukeeMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIMonroeFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIMonroeFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIMonroeMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIMonroeMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIOcontoFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIOcontoFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIOcontoMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIOcontoMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIOneidaFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIOneidaFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIOneidaMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIOneidaMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIOutagamieFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIOutagamieFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIOutagamieMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIOutagamieMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIOzaukeeFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIOzaukeeFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIOzaukeeMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIOzaukeeMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIPepinFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIPepinFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIPepinMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIPepinMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIPierceFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIPierceFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIPierceMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIPierceMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIPolkFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIPolkFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIPolkMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIPolkMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIPortageFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIPortageFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIPortageMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIPortageMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIPriceFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIPriceFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIPriceMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIPriceMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIRacineFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIRacineFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIRacineMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIRacineMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIRichlandFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIRichlandFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIRichlandMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIRichlandMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIRockFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIRockFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIRockMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIRockMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIRuskFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIRuskFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIRuskMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIRuskMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWISaukFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWISaukFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWISaukMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWISaukMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWISawyerFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWISawyerFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWISawyerMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWISawyerMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIShawanoFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIShawanoFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIShawanoMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIShawanoMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWISheboyganFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWISheboyganFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWISheboyganMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWISheboyganMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIStCroixFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIStCroixFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIStCroixMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIStCroixMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWITaylorFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWITaylorFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWITaylorMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWITaylorMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWITrempealeauFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWITrempealeauFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWITrempealeauMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWITrempealeauMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIVernonFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIVernonFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIVernonMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIVernonMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIVilasFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIVilasFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIVilasMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIVilasMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIWalworthFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIWalworthFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIWalworthMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIWalworthMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIWashburnFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIWashburnFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIWashburnMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIWashburnMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIWashingtonFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIWashingtonFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIWashingtonMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIWashingtonMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIWaukeshaFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIWaukeshaFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIWaukeshaMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIWaukeshaMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIWaupacaFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIWaupacaFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIWaupacaMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIWaupacaMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIWausharaFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIWausharaFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIWausharaMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIWausharaMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIWinnebagoFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIWinnebagoFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIWinnebagoMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIWinnebagoMeters;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIWoodFeet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIWoodFeet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNAdjWIWoodMeters()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Wisconsin.NAD1983HARNAdjWIWoodMeters;
            Tester.TestProjection(pStart);
        }
    }
}