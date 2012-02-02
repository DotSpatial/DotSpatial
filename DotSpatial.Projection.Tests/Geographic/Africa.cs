using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Geographic
{
    /// <summary>
    /// This class contains all the tests for the Africa category of Geographic coordinate systems
    /// </summary>
    [TestFixture]
    public class Africa
    {
        /// <summary>
        /// Creates a new instance of the Africa Class
        /// </summary>
        [TestFixtureSetUp]
        public void  Initialize()
        {
            TestSetupHelper.CopyProj4();
        }


        [Test]
        public void Abidjan1987()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Abidjan1987;
            Tester.TestProjection(pStart);
        }


  
        [Test]
        public void Accra()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Accra;
            Tester.TestProjection(pStart);
        }



        [Test]
        public void Adindan()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Adindan;
            Tester.TestProjection(pStart);
        }



        [Test]
        public void Afgooye()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Afgooye;
            Tester.TestProjection(pStart);
        }



        [Test]
        public void Agadez()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Agadez;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AinelAbd1970()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.AinelAbd1970;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Arc1950()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Arc1950;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Arc1960()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Arc1960;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AyabelleLighthouse()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.AyabelleLighthouse;
            Tester.TestProjection(pStart);
        }

        [Test]
        public void Beduaram()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Beduaram;
            Tester.TestProjection(pStart);
        }

        [Test]
        public void Bissau()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Bissau;
            Tester.TestProjection(pStart);
        }

        [Test]
        public void Camacupa()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Camacupa;
            Tester.TestProjection(pStart);
        }

        [Test]
        public void Cape()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Cape;
            Tester.TestProjection(pStart);
        }

        [Test]
        public void Carthage()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Carthage;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Carthagedegrees()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Carthagedegrees;
            Tester.TestProjection(pStart);
        }

        [Test]
        public void CarthageParis()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.CarthageParis;
            Tester.TestProjection(pStart);
        }

        [Test]
        public void Conakry1905()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Conakry1905;
            Tester.TestProjection(pStart);
        }

        [Test]
        public void CotedIvoire()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.CotedIvoire;
            Tester.TestProjection(pStart);
        }

        [Test]
        public void Dabola()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Dabola;
            Tester.TestProjection(pStart);
        }

        [Test]
        public void Douala()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Douala;
            Tester.TestProjection(pStart);
        }
        [Test]
        public void Douala1948()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Douala1948;
            Tester.TestProjection(pStart);
        }

        [Test]
        public void Egypt1907()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Egypt1907;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Egypt1930()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Egypt1930;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EuropeanDatum1950()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.EuropeanDatum1950;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EuropeanLibyanDatum1979()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.EuropeanLibyanDatum1979;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Garoua()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Garoua;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Hartebeesthoek1994()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Hartebeesthoek1994;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Kousseri()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Kousseri;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void KuwaitOilCompany()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.KuwaitOilCompany;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void KuwaitUtility()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.KuwaitUtility;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Leigon()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Leigon;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Liberia1964()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Liberia1964;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Locodjo1965()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Locodjo1965;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Lome()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Lome;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Madzansua()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Madzansua;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Mahe1971()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Mahe1971;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Malongo1987()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Malongo1987;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Manoca()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Manoca;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Manoca1962()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Manoca1962;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Massawa()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Massawa;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Merchich()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Merchich;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Merchichdegrees()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Merchichdegrees;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Mhast()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Mhast;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Minna()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Minna;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Moznet()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Moznet;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Mporaloko()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Mporaloko;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Nahrwan1967()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Nahrwan1967;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NationalGeodeticNetworkKuwait()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.NationalGeodeticNetworkKuwait;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NordSahara1959()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.NordSahara1959;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NordSahara1959Paris()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.NordSahara1959Paris;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Observatario()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Observatario;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Oman()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Oman;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Palestine1923()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Palestine1923;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Pdo1993()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.PDO1993;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Point58()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Point58;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void PointeNoire()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.PointeNoire;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Qatar()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Qatar;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Qatar1948()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Qatar1948;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Schwarzeck()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Schwarzeck;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SierraLeone1924()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.SierraLeone1924;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SierraLeone1960()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.SierraLeone1960;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SierraLeone1968()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.SierraLeone1968;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthYemen()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.SouthYemen;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Sudan()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Sudan;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Tananarive1925()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Tananarive1925;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Tananarive1925Paris()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Tananarive1925Paris;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Tete()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Tete;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void TrucialCoast1948()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.TrucialCoast1948;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Voirol1875()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Voirol1875;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Voirol1875Degrees()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Voirol1875degrees;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Voirol1875Paris()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Voirol1875Paris;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void VoirolUnifie1960()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.VoirolUnifie1960;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void VoirolUnifie1960Degrees()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.VoirolUnifie1960degrees;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void VoirolUnifie1960Paris()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.VoirolUnifie1960Paris;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void YemenNgn1996()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.YemenNGN1996;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Yoff()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Africa.Yoff;
            Tester.TestProjection(pStart);
        }
    }
}
