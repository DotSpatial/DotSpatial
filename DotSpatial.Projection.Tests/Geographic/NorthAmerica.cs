
using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Geographic
{
    /// <summary>
    /// This class contains all the tests for the NorthAmerica category of Geographic coordinate systems
    /// </summary>
    [TestFixture]
    public class NorthAmerica
    {

        /// <summary>
        /// Creates a new instance of the Africa Class
        /// </summary>
        [TestFixtureSetUp]
        public void Initialize()
        {
            
        }

        [Test]
        public void AlaskanIslands()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.AlaskanIslands;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AmericanSamoa1962()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.AmericanSamoa1962;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Ammassalik1958()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.Ammassalik1958;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ATS1977()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.ATS1977;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Barbados()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.Barbados;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Bermuda1957()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.Bermuda1957;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Bermuda2000()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.Bermuda2000;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void CapeCanaveral()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.CapeCanaveral;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Guam1963()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.Guam1963;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Helle1954()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.Helle1954;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Jamaica1875()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.Jamaica1875;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Jamaica1969()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.Jamaica1969;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927CGQ77()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.NAD1927CGQ77;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927Definition1976()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.NAD1927Definition1976;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NADMichigan()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.NADMichigan;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NorthAmerican1983CSRS98()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.NorthAmerican1983CSRS98;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NorthAmerican1983HARN()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.NorthAmerican1983HARN;
            Tester.TestProjection(pStart);
        }

        [Ignore]
        [Test]
        public void NorthAmericanDatum1927()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.NorthAmericanDatum1927;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NorthAmericanDatum1983()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.NorthAmericanDatum1983;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void OldHawaiian()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.OldHawaiian;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void PuertoRico()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.PuertoRico;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Qornoq()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.Qornoq;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Qornoq1927()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.Qornoq1927;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Scoresbysund1952()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.Scoresbysund1952;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void StGeorgeIsland()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.StGeorgeIsland;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void StLawrenceIsland()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.StLawrenceIsland;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void StPaulIsland()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.NorthAmerica.StPaulIsland;
            Tester.TestProjection(pStart);
        }
    }
}
