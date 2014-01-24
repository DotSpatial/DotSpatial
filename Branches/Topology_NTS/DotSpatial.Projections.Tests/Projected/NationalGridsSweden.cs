using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the NationalGridsSweden category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class NationalGridsSweden
    {
        /// <summary>
        /// Creates a new instance of the Africa Class
        /// </summary>
        [TestFixtureSetUp]
        public void Initialize()
        {
            
        }

        [Test]
        public void RT380gon()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.RT380gon;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void RT3825gonO()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.RT3825gonO;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void RT3825gonV()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.RT3825gonV;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void RT385gonO()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.RT385gonO;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void RT385gonV()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.RT385gonV;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void RT3875gonV()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.RT3875gonV;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void RT900gon()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.RT900gon;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void RT9025gonO()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.RT9025gonO;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void RT9025gonV()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.RT9025gonV;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void RT905gonO()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.RT905gonO;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void RT905gonV()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.RT905gonV;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void RT9075gonV()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.RT9075gonV;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SWEREF991200()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.SWEREF991200;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SWEREF991330()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.SWEREF991330;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SWEREF991415()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.SWEREF991415;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SWEREF991500()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.SWEREF991500;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SWEREF991545()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.SWEREF991545;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SWEREF991630()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.SWEREF991630;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SWEREF991715()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.SWEREF991715;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SWEREF991800()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.SWEREF991800;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SWEREF991845()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.SWEREF991845;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SWEREF992015()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.SWEREF992015;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SWEREF992145()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.SWEREF992145;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SWEREF992315()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.SWEREF992315;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SWEREF99TM()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsSweden.SWEREF99TM;
            Tester.TestProjection(pStart);
        }
    }
}