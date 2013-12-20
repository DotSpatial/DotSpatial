
using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the GausKrugerOther category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class GausKrugerOther
    {
        /// <summary>
        /// Creates a new instance of the Africa Class
        /// </summary>
        [TestFixtureSetUp]
        public void Initialize()
        {
            
        }

        [Test]
        public void Albanian1987GKZone4()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.GausKrugerOther.Albanian1987GKZone4;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ED19503DegreeGKZone10()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.GausKrugerOther.ED19503DegreeGKZone10;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ED19503DegreeGKZone11()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.GausKrugerOther.ED19503DegreeGKZone11;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ED19503DegreeGKZone12()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.GausKrugerOther.ED19503DegreeGKZone12;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ED19503DegreeGKZone13()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.GausKrugerOther.ED19503DegreeGKZone13;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ED19503DegreeGKZone14()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.GausKrugerOther.ED19503DegreeGKZone14;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ED19503DegreeGKZone15()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.GausKrugerOther.ED19503DegreeGKZone15;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ED19503DegreeGKZone9()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.GausKrugerOther.ED19503DegreeGKZone9;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Hanoi1972GKZone18()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.GausKrugerOther.Hanoi1972GKZone18;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Hanoi1972GKZone19()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.GausKrugerOther.Hanoi1972GKZone19;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Pulkovo1942Adj19833DegreeGKZone3()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.GausKrugerOther.Pulkovo1942Adj19833DegreeGKZone3;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Pulkovo1942Adj19833DegreeGKZone4()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.GausKrugerOther.Pulkovo1942Adj19833DegreeGKZone4;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Pulkovo1942Adj19833DegreeGKZone5()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.GausKrugerOther.Pulkovo1942Adj19833DegreeGKZone5;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthYemenGKZone8()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.GausKrugerOther.SouthYemenGKZone8;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthYemenGKZone9()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.GausKrugerOther.SouthYemenGKZone9;
            Tester.TestProjection(pStart);
        }
    }
}