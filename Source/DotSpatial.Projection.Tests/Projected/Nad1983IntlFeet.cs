using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the Nad1983IntlFeet category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class Nad1983IntlFeet
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
        public void NAD1983StatePlaneArizonaCentralFIPS0202FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Nad1983IntlFeet.NAD1983StatePlaneArizonaCentralFIPS0202FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneArizonaEastFIPS0201FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Nad1983IntlFeet.NAD1983StatePlaneArizonaEastFIPS0201FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneArizonaWestFIPS0203FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Nad1983IntlFeet.NAD1983StatePlaneArizonaWestFIPS0203FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMichiganCentralFIPS2112FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Nad1983IntlFeet.NAD1983StatePlaneMichiganCentralFIPS2112FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMichiganNorthFIPS2111FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Nad1983IntlFeet.NAD1983StatePlaneMichiganNorthFIPS2111FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMichiganSouthFIPS2113FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Nad1983IntlFeet.NAD1983StatePlaneMichiganSouthFIPS2113FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneMontanaFIPS2500FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Nad1983IntlFeet.NAD1983StatePlaneMontanaFIPS2500FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNorthDakotaNorthFIPS3301FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Nad1983IntlFeet.NAD1983StatePlaneNorthDakotaNorthFIPS3301FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneNorthDakotaSouthFIPS3302FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Nad1983IntlFeet.NAD1983StatePlaneNorthDakotaSouthFIPS3302FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneOregonNorthFIPS3601FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Nad1983IntlFeet.NAD1983StatePlaneOregonNorthFIPS3601FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneOregonSouthFIPS3602FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Nad1983IntlFeet.NAD1983StatePlaneOregonSouthFIPS3602FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneSouthCarolinaFIPS3900FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Nad1983IntlFeet.NAD1983StatePlaneSouthCarolinaFIPS3900FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneUtahCentralFIPS4302FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Nad1983IntlFeet.NAD1983StatePlaneUtahCentralFIPS4302FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneUtahNorthFIPS4301FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Nad1983IntlFeet.NAD1983StatePlaneUtahNorthFIPS4301FeetIntl;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983StatePlaneUtahSouthFIPS4303FeetIntl()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.Nad1983IntlFeet.NAD1983StatePlaneUtahSouthFIPS4303FeetIntl;
            Tester.TestProjection(pStart);
        }
    }
}