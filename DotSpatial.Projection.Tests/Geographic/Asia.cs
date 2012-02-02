
using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Geographic
{
    /// <summary>
    /// This class contains all the tests for the Asia category of Geographic coordinate systems
    /// </summary>
    [TestFixture]
    public class Asia
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
        public void AinelAbd1970()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.AinelAbd1970;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Batavia()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Batavia;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void BataviaJakarta()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.BataviaJakarta;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Beijing1954()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Beijing1954;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void BukitRimpah()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.BukitRimpah;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void DeirezZor()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.DeirezZor;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void European1950ED77()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.European1950ED77;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EuropeanDatum1950()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.EuropeanDatum1950;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EverestBangladesh()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.EverestBangladesh;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EverestIndiaandNepal()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.EverestIndiaandNepal;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Everestdef1962()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Everestdef1962;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Everestdef1967()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Everestdef1967;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Everestdef1975()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Everestdef1975;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Everest1830()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Everest1830;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EverestModified()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.EverestModified;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Fahud()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Fahud;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void FD1958()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.FD1958;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Gandajika1970()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Gandajika1970;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void GunungSegara()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.GunungSegara;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void GunungSegaraJakarta()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.GunungSegaraJakarta;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Hanoi1972()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Hanoi1972;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void HeratNorth()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.HeratNorth;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void HongKong1963()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.HongKong1963;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void HongKong1980()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.HongKong1980;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void HuTzuShan()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.HuTzuShan;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void IGM1995()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.IGM1995;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void IKBD1992()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.IKBD1992;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indian1954()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Indian1954;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indian1960()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Indian1960;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indian1975()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Indian1975;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void IndonesianDatum1974()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.IndonesianDatum1974;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Israel()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Israel;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void JGD2000()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.JGD2000;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Jordan()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Jordan;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Kalianpur1880()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Kalianpur1880;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Kalianpur1937()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Kalianpur1937;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Kalianpur1962()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Kalianpur1962;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Kalianpur1975()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Kalianpur1975;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Kandawala()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Kandawala;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Kertau()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Kertau;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void KoreanDatum1985()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.KoreanDatum1985;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void KoreanDatum1995()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.KoreanDatum1995;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void KuwaitOilCompany()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.KuwaitOilCompany;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void KuwaitUtility()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.KuwaitUtility;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Luzon1911()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Luzon1911;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Makassar()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Makassar;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void MakassarJakarta()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.MakassarJakarta;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Nahrwan1967()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Nahrwan1967;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NationalGeodeticNetworkKuwait()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.NationalGeodeticNetworkKuwait;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ObservatorioMeteorologico1965()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.ObservatorioMeteorologico1965;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Oman()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Oman;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Padang1884()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Padang1884;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Padang1884Jakarta()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Padang1884Jakarta;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Palestine1923()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Palestine1923;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Pulkovo1942()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Pulkovo1942;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Pulkovo1995()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Pulkovo1995;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Qatar()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Qatar;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Qatar1948()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Qatar1948;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void QND1995()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.QND1995;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Rassadiran()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Rassadiran;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Samboja()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Samboja;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Segora()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Segora;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Serindung()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Serindung;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthAsiaSingapore()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.SouthAsiaSingapore;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Timbalai1948()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Timbalai1948;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Tokyo()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Tokyo;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void TrucialCoast1948()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.TrucialCoast1948;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Xian1980()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Asia.Xian1980;
            Tester.TestProjection(pStart);
        }
    }
}
