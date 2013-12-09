
using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Geographic
{
    /// <summary>
    /// This class contains all the tests for the SouthAmerica category of Geographic coordinate systems
    /// </summary>
    [TestFixture]
    public class SouthAmerica
    {
        /// <summary>
        /// Creates a new instance of the Africa Class
        /// </summary>
        [TestFixtureSetUp]
        public void Initialize()
        {
            
        }

        [Test]
        public void Aratu()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.Aratu;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Bogota()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.Bogota;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void BogotaBogota()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.BogotaBogota;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void CampoInchauspe()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.CampoInchauspe;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ChosMalal1914()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.ChosMalal1914;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Chua()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.Chua;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void CorregoAlegre()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.CorregoAlegre;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void GuyaneFrancaise()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.GuyaneFrancaise;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void HitoXVIII1963()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.HitoXVIII1963;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void LaCanoa()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.LaCanoa;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Lake()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.Lake;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void LomaQuintana()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.LomaQuintana;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void MountDillon()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.MountDillon;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Naparima1955()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.Naparima1955;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Naparima1972()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.Naparima1972;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void PampadelCastillo()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.PampadelCastillo;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void POSGAR()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.POSGAR;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void POSGAR1998()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.POSGAR1998;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ProvisionalSouthAmer()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.ProvisionalSouthAmer;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void REGVEN()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.REGVEN;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SapperHill1943()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.SapperHill1943;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SIRGAS()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.SIRGAS;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthAmericanDatum1969()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.SouthAmericanDatum1969;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Trinidad1903()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.Trinidad1903;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Yacare()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.Yacare;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Zanderij()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SouthAmerica.Zanderij;
            Tester.TestProjection(pStart);
        }
    }
}
