using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Geographic
{
    /// <summary>
    /// This class contains all the tests for the Australia category of Geographic coordinate systems
    /// </summary>
    [TestFixture]
    public class Australia
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
        public void AustralianGeodeticDatum1966()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Australia.AustralianGeodeticDatum1966;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AustralianGeodeticDatum1984()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Australia.AustralianGeodeticDatum1984;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ChathamIslands1979()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Australia.ChathamIslands1979;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void GeocentricDatumofAustralia1994()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Australia.GeocentricDatumofAustralia1994;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NewZealandGeodeticDatum1949()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Australia.NewZealandGeodeticDatum1949;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Australia.NZGD2000;
            Tester.TestProjection(pStart);
        }
    }
}
