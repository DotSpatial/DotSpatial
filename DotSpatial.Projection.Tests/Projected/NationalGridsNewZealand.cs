using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the NationalGridsNewZealand category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class NationalGridsNewZealand
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
        public void ChathamIslands1979MapGrid()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.ChathamIslands1979MapGrid;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void NewZealandMapGrid()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NewZealandMapGrid;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NewZealandNorthIsland()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NewZealandNorthIsland;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NewZealandSouthIsland()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NewZealandSouthIsland;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949AmuriCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949AmuriCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949BayofPlentyCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949BayofPlentyCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949BluffCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949BluffCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949BullerCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949BullerCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949CollingwoodCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949CollingwoodCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949GawlerCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949GawlerCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949GreyCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949GreyCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949HawkesBayCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949HawkesBayCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949HokitikaCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949HokitikaCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949JacksonsBayCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949JacksonsBayCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949KarameaCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949KarameaCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949LindisPeakCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949LindisPeakCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949MarlboroughCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949MarlboroughCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949MountEdenCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949MountEdenCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949MountNicholasCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949MountNicholasCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949MountPleasantCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949MountPleasantCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949MountYorkCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949MountYorkCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949NelsonCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949NelsonCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949NorthTaieriCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949NorthTaieriCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949ObservationPointCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949ObservationPointCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949OkaritoCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949OkaritoCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949PovertyBayCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949PovertyBayCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949TaranakiCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949TaranakiCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949TimaruCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949TimaruCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949TuhirangiCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949TuhirangiCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949UTMZone58S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949UTMZone58S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949UTMZone59S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949UTMZone59S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949UTMZone60S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949UTMZone60S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949WairarapaCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949WairarapaCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949WanganuiCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949WanganuiCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949WellingtonCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD1949WellingtonCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000AmuriCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000AmuriCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000BayofPlentyCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000BayofPlentyCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000BluffCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000BluffCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000BullerCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000BullerCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000ChathamIslandCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000ChathamIslandCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000CollingwoodCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000CollingwoodCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000GawlerCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000GawlerCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000GreyCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000GreyCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000HawkesBayCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000HawkesBayCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000HokitikaCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000HokitikaCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000JacksonsBayCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000JacksonsBayCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000KarameaCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000KarameaCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000LindisPeakCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000LindisPeakCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000MarlboroughCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000MarlboroughCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000MountEdenCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000MountEdenCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000MountNicholasCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000MountNicholasCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000MountPleasantCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000MountPleasantCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000MountYorkCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000MountYorkCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000NelsonCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000NelsonCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000NewZealandTransverseMercator()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000NewZealandTransverseMercator;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000NorthTaieriCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000NorthTaieriCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000ObservationPointCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000ObservationPointCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000OkaritoCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000OkaritoCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000PovertyBayCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000PovertyBayCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000TaranakiCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000TaranakiCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000TimaruCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000TimaruCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000TuhirangiCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000TuhirangiCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000UTMZone58S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000UTMZone58S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000UTMZone59S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000UTMZone59S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000UTMZone60S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000UTMZone60S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000WairarapaCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000WairarapaCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000WanganuiCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000WanganuiCircuit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000WellingtonCircuit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsNewZealand.NZGD2000WellingtonCircuit;
            Tester.TestProjection(pStart);
        }
    }
}