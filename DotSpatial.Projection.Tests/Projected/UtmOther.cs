using System.IO;
using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the UtmOther category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class UtmOther
    {
        /// <summary>
        /// Creates a new instance of the Africa Class
        /// </summary>
        [TestFixtureSetUp]
        public void Initialize()
        {
            
        }

        [Test]
        public void Abidjan1987UTMZone29N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Abidjan1987UTMZone29N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Abidjan1987UTMZone30N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Abidjan1987UTMZone30N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AdindanUTMZone37N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.AdindanUTMZone37N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AdindanUTMZone38N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.AdindanUTMZone38N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AfgooyeUTMZone38N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.AfgooyeUTMZone38N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AfgooyeUTMZone39N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.AfgooyeUTMZone39N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AinelAbd1970UTMZone37N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.AinelAbd1970UTMZone37N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AinelAbd1970UTMZone38N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.AinelAbd1970UTMZone38N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AinelAbd1970UTMZone39N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.AinelAbd1970UTMZone39N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AmericanSamoa1962UTMZone2S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.AmericanSamoa1962UTMZone2S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AratuUTMZone22S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.AratuUTMZone22S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AratuUTMZone23S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.AratuUTMZone23S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AratuUTMZone24S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.AratuUTMZone24S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Arc1950UTMZone34S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Arc1950UTMZone34S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Arc1950UTMZone35S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Arc1950UTMZone35S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Arc1950UTMZone36S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Arc1950UTMZone36S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Arc1960UTMZone35N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Arc1960UTMZone35N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Arc1960UTMZone35S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Arc1960UTMZone35S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Arc1960UTMZone36N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Arc1960UTMZone36N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Arc1960UTMZone36S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Arc1960UTMZone36S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Arc1960UTMZone37N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Arc1960UTMZone37N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Arc1960UTMZone37S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Arc1960UTMZone37S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ATS1977UTMZone19N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ATS1977UTMZone19N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ATS1977UTMZone20N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ATS1977UTMZone20N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AzoresCentral1995UTMZone26N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.AzoresCentral1995UTMZone26N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AzoresOriental1995UTMZone26N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.AzoresOriental1995UTMZone26N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void BataviaUTMZone48S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.BataviaUTMZone48S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void BataviaUTMZone49S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.BataviaUTMZone49S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void BataviaUTMZone50S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.BataviaUTMZone50S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void BissauUTMZone28N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.BissauUTMZone28N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void BogotaUTMZone17N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.BogotaUTMZone17N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void BogotaUTMZone18N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.BogotaUTMZone18N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void CamacupaUTMZone32S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.CamacupaUTMZone32S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void CamacupaUTMZone33S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.CamacupaUTMZone33S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void CampoInchauspeUTM19S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.CampoInchauspeUTM19S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void CampoInchauspeUTM20S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.CampoInchauspeUTM20S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void CapeUTMZone34S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.CapeUTMZone34S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void CapeUTMZone35S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.CapeUTMZone35S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void CapeUTMZone36S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.CapeUTMZone36S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void CarthageUTMZone32N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.CarthageUTMZone32N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Combani1950UTMZone38S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Combani1950UTMZone38S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Conakry1905UTMZone28N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Conakry1905UTMZone28N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Conakry1905UTMZone29N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Conakry1905UTMZone29N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void CorregoAlegreUTMZone23S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.CorregoAlegreUTMZone23S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void CorregoAlegreUTMZone24S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.CorregoAlegreUTMZone24S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void CSG1967UTMZone22N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.CSG1967UTMZone22N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void DabolaUTMZone28N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.DabolaUTMZone28N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void DabolaUTMZone29N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.DabolaUTMZone29N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Datum73UTMZone29N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Datum73UTMZone29N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void DoualaUTMZone32N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.DoualaUTMZone32N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ED1950ED77UTMZone38N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ED1950ED77UTMZone38N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ED1950ED77UTMZone39N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ED1950ED77UTMZone39N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ED1950ED77UTMZone40N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ED1950ED77UTMZone40N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ED1950ED77UTMZone41N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ED1950ED77UTMZone41N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ELD1979UTMZone32N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ELD1979UTMZone32N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ELD1979UTMZone33N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ELD1979UTMZone33N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ELD1979UTMZone34N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ELD1979UTMZone34N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ELD1979UTMZone35N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ELD1979UTMZone35N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRF1989UTMZone28N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRF1989UTMZone28N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRF1989UTMZone29N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRF1989UTMZone29N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRF1989UTMZone30N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRF1989UTMZone30N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRF1989UTMZone31N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRF1989UTMZone31N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRF1989UTMZone32N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRF1989UTMZone32N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRF1989UTMZone33N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRF1989UTMZone33N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRF1989UTMZone34N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRF1989UTMZone34N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRF1989UTMZone35N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRF1989UTMZone35N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRF1989UTMZone36N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRF1989UTMZone36N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRF1989UTMZone37N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRF1989UTMZone37N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRF1989UTMZone38N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRF1989UTMZone38N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRS1989UTMZone26N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRS1989UTMZone26N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRS1989UTMZone27N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRS1989UTMZone27N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRS1989UTMZone28N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRS1989UTMZone28N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRS1989UTMZone29N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRS1989UTMZone29N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRS1989UTMZone30N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRS1989UTMZone30N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRS1989UTMZone31N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRS1989UTMZone31N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRS1989UTMZone32N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRS1989UTMZone32N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRS1989UTMZone33N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRS1989UTMZone33N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRS1989UTMZone34N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRS1989UTMZone34N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRS1989UTMZone35N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRS1989UTMZone35N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRS1989UTMZone36N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRS1989UTMZone36N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRS1989UTMZone37N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRS1989UTMZone37N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRS1989UTMZone38N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRS1989UTMZone38N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ETRS1989UTMZone39N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ETRS1989UTMZone39N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EuropeanDatum1950UTMZone28N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.EuropeanDatum1950UTMZone28N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EuropeanDatum1950UTMZone29N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.EuropeanDatum1950UTMZone29N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EuropeanDatum1950UTMZone30N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.EuropeanDatum1950UTMZone30N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EuropeanDatum1950UTMZone31N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.EuropeanDatum1950UTMZone31N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EuropeanDatum1950UTMZone32N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.EuropeanDatum1950UTMZone32N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EuropeanDatum1950UTMZone33N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.EuropeanDatum1950UTMZone33N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EuropeanDatum1950UTMZone34N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.EuropeanDatum1950UTMZone34N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EuropeanDatum1950UTMZone35N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.EuropeanDatum1950UTMZone35N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EuropeanDatum1950UTMZone36N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.EuropeanDatum1950UTMZone36N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EuropeanDatum1950UTMZone37N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.EuropeanDatum1950UTMZone37N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EuropeanDatum1950UTMZone38N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.EuropeanDatum1950UTMZone38N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void FahudUTMZone39N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.FahudUTMZone39N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void FahudUTMZone40N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.FahudUTMZone40N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void FortDesaixUTMZone20N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.FortDesaixUTMZone20N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void FortMarigotUTMZone20N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.FortMarigotUTMZone20N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void GarouaUTMZone33N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.GarouaUTMZone33N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void GraciosaBaseSW1948UTMZone26N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.GraciosaBaseSW1948UTMZone26N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void GrandComorosUTMZone38S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.GrandComorosUTMZone38S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void HitoXVIII1963UTMZone19S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.HitoXVIII1963UTMZone19S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Hjorsey1955UTMZone26N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Hjorsey1955UTMZone26N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Hjorsey1955UTMZone27N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Hjorsey1955UTMZone27N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Hjorsey1955UTMZone28N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Hjorsey1955UTMZone28N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void HongKong1980UTMZone49N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.HongKong1980UTMZone49N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void HongKong1980UTMZone50N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.HongKong1980UTMZone50N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void IGM1995UTMZone32N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.IGM1995UTMZone32N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void IGM1995UTMZone33N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.IGM1995UTMZone33N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void IGN53MareUTMZone58S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.IGN53MareUTMZone58S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void IGN56LifouUTMZone58S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.IGN56LifouUTMZone58S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void IGN72GrandeTerreUTMZone58S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.IGN72GrandeTerreUTMZone58S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void IGN72NukuHivaUTMZone7S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.IGN72NukuHivaUTMZone7S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indian1954UTMZone46N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indian1954UTMZone46N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indian1954UTMZone47N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indian1954UTMZone47N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indian1954UTMZone48N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indian1954UTMZone48N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indian1960UTMZone48N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indian1960UTMZone48N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indian1960UTMZone49N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indian1960UTMZone49N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indian1975UTMZone47N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indian1975UTMZone47N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indian1975UTMZone48N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indian1975UTMZone48N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indonesia1974UTMZone46N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indonesia1974UTMZone46N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indonesia1974UTMZone46S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indonesia1974UTMZone46S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indonesia1974UTMZone47N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indonesia1974UTMZone47N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indonesia1974UTMZone47S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indonesia1974UTMZone47S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indonesia1974UTMZone48N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indonesia1974UTMZone48N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indonesia1974UTMZone48S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indonesia1974UTMZone48S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indonesia1974UTMZone49N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indonesia1974UTMZone49N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indonesia1974UTMZone49S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indonesia1974UTMZone49S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indonesia1974UTMZone50N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indonesia1974UTMZone50N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indonesia1974UTMZone50S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indonesia1974UTMZone50S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indonesia1974UTMZone51N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indonesia1974UTMZone51N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indonesia1974UTMZone51S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indonesia1974UTMZone51S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indonesia1974UTMZone52N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indonesia1974UTMZone52N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indonesia1974UTMZone52S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indonesia1974UTMZone52S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indonesia1974UTMZone53N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indonesia1974UTMZone53N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indonesia1974UTMZone53S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indonesia1974UTMZone53S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Indonesia1974UTMZone54S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Indonesia1974UTMZone54S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void IRENET95UTMZone29N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.IRENET95UTMZone29N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void JGD2000UTMZone51N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.JGD2000UTMZone51N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void JGD2000UTMZone52N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.JGD2000UTMZone52N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void JGD2000UTMZone53N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.JGD2000UTMZone53N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void JGD2000UTMZone54N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.JGD2000UTMZone54N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void JGD2000UTMZone55N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.JGD2000UTMZone55N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void JGD2000UTMZone56N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.JGD2000UTMZone56N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void K01949UTMZone42S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.K01949UTMZone42S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void KertauUTMZone47N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.KertauUTMZone47N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void KertauUTMZone48N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.KertauUTMZone48N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void KousseriUTMZone33N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.KousseriUTMZone33N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void LaCanoaUTMZone18N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.LaCanoaUTMZone18N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void LaCanoaUTMZone19N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.LaCanoaUTMZone19N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void LaCanoaUTMZone20N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.LaCanoaUTMZone20N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void LaCanoaUTMZone21N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.LaCanoaUTMZone21N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Locodjo1965UTMZone29N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Locodjo1965UTMZone29N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Locodjo1965UTMZone30N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Locodjo1965UTMZone30N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void LomeUTMZone31N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.LomeUTMZone31N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Malongo1987UTMZone32S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Malongo1987UTMZone32S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Manoca1962UTMZone32N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Manoca1962UTMZone32N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void MassawaUTMZone37N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.MassawaUTMZone37N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void MhastUTMZone32S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.MhastUTMZone32S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void MinnaUTMZone31N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.MinnaUTMZone31N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void MinnaUTMZone32N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.MinnaUTMZone32N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void MOP78UTMZone1S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.MOP78UTMZone1S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void MoznetUTMZone36S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.MoznetUTMZone36S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void MoznetUTMZone37S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.MoznetUTMZone37S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void MporalokoUTMZone32N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.MporalokoUTMZone32N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void MporalokoUTMZone32S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.MporalokoUTMZone32S;
            Tester.TestProjection(pStart);
        }

        [Ignore]
        [Test]
        public void NAD1927BLMZone14N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NAD1927BLMZone14N;
            Tester.TestProjection(pStart);
        }

        [Ignore]
        [Test]
        public void NAD1927BLMZone15N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NAD1927BLMZone15N;
            Tester.TestProjection(pStart);
        }

        [Ignore]
        [Test]
        public void NAD1927BLMZone16N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NAD1927BLMZone16N;
            Tester.TestProjection(pStart);
        }

        [Ignore]
        [Test]
        public void NAD1927BLMZone17N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NAD1927BLMZone17N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNUTMZone11N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NAD1983HARNUTMZone11N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNUTMZone12N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NAD1983HARNUTMZone12N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNUTMZone13N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NAD1983HARNUTMZone13N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNUTMZone18N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NAD1983HARNUTMZone18N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNUTMZone2S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NAD1983HARNUTMZone2S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNUTMZone4N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NAD1983HARNUTMZone4N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983HARNUTMZone5N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NAD1983HARNUTMZone5N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Nahrwan1967UTMZone38N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Nahrwan1967UTMZone38N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Nahrwan1967UTMZone39N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Nahrwan1967UTMZone39N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Nahrwan1967UTMZone40N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Nahrwan1967UTMZone40N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Naparima1955UTMZone20N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Naparima1955UTMZone20N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Naparima1972UTMZone20N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Naparima1972UTMZone20N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NEA74NoumeaUTMZone58S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NEA74NoumeaUTMZone58S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NGNUTMZone38N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NGNUTMZone38N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NGNUTMZone39N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NGNUTMZone39N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NGO1948UTMZone32N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NGO1948UTMZone32N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NGO1948UTMZone33N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NGO1948UTMZone33N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NGO1948UTMZone34N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NGO1948UTMZone34N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NGO1948UTMZone35N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NGO1948UTMZone35N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NordSahara1959UTMZone29N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NordSahara1959UTMZone29N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NordSahara1959UTMZone30N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NordSahara1959UTMZone30N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NordSahara1959UTMZone31N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NordSahara1959UTMZone31N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NordSahara1959UTMZone32N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NordSahara1959UTMZone32N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949UTMZone58S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NZGD1949UTMZone58S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949UTMZone59S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NZGD1949UTMZone59S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD1949UTMZone60S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NZGD1949UTMZone60S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000UTMZone58S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NZGD2000UTMZone58S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000UTMZone59S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NZGD2000UTMZone59S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NZGD2000UTMZone60S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.NZGD2000UTMZone60S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ObservMeteorologico1939UTMZone25N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ObservMeteorologico1939UTMZone25N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void OldHawaiianUTMZone4N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.OldHawaiianUTMZone4N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void OldHawaiianUTMZone5N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.OldHawaiianUTMZone5N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void PDO1993UTMZone39N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.PDO1993UTMZone39N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void PDO1993UTMZone40N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.PDO1993UTMZone40N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void PointeNoireUTMZone32S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.PointeNoireUTMZone32S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void PortoSanto1936UTMZone28N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.PortoSanto1936UTMZone28N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void PortoSanto1995UTMZone28N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.PortoSanto1995UTMZone28N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ProvSAmerDatumUTMZone17s()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ProvSAmerDatumUTMZone17s;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ProvSAmerDatumUTMZone18N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ProvSAmerDatumUTMZone18N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ProvSAmerDatumUTMZone18S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ProvSAmerDatumUTMZone18S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ProvSAmerDatumUTMZone19N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ProvSAmerDatumUTMZone19N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ProvSAmerDatumUTMZone19S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ProvSAmerDatumUTMZone19S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ProvSAmerDatumUTMZone20N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ProvSAmerDatumUTMZone20N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ProvSAmerDatumUTMZone20S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ProvSAmerDatumUTMZone20S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ProvSAmerDatumUTMZone21N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ProvSAmerDatumUTMZone21N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ProvSAmerDatumUTMZone22S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ProvSAmerDatumUTMZone22S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void PuertoRicoUTMZone20N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.PuertoRicoUTMZone20N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Qornoq1927UTMZone22N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Qornoq1927UTMZone22N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Qornoq1927UTMZone23N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Qornoq1927UTMZone23N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void REGVENUTMZone18N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.REGVENUTMZone18N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void REGVENUTMZone19N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.REGVENUTMZone19N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void REGVENUTMZone20N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.REGVENUTMZone20N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void RGFG1995UTMZone22N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.RGFG1995UTMZone22N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void RGR1992UTMZone40S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.RGR1992UTMZone40S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void RRAF1991UTMZone20N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.RRAF1991UTMZone20N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SainteAnneUTMZone20N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SainteAnneUTMZone20N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SaintPierreetMiquelon1950UTMZone21N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SaintPierreetMiquelon1950UTMZone21N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SambojaUTMZone50S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SambojaUTMZone50S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SaoBrazUTMZone26N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SaoBrazUTMZone26N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SapperHill1943UTMZone20S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SapperHill1943UTMZone20S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SapperHill1943UTMZone21S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SapperHill1943UTMZone21S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SchwarzeckUTMZone33S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SchwarzeckUTMZone33S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SelvagemGrande1938UTMZone28N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SelvagemGrande1938UTMZone28N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SierraLeone1968UTMZone28N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SierraLeone1968UTMZone28N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SierraLeone1968UTMZone29N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SierraLeone1968UTMZone29N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SIRGASUTMZone17N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SIRGASUTMZone17N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SIRGASUTMZone17S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SIRGASUTMZone17S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SIRGASUTMZone18N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SIRGASUTMZone18N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SIRGASUTMZone18S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SIRGASUTMZone18S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SIRGASUTMZone19N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SIRGASUTMZone19N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SIRGASUTMZone19S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SIRGASUTMZone19S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SIRGASUTMZone20N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SIRGASUTMZone20N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SIRGASUTMZone20S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SIRGASUTMZone20S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SIRGASUTMZone21N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SIRGASUTMZone21N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SIRGASUTMZone21S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SIRGASUTMZone21S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SIRGASUTMZone22N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SIRGASUTMZone22N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SIRGASUTMZone22S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SIRGASUTMZone22S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SIRGASUTMZone23S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SIRGASUTMZone23S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SIRGASUTMZone24S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SIRGASUTMZone24S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SIRGASUTMZone25S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SIRGASUTMZone25S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthAmerican1969UTMZone17S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SouthAmerican1969UTMZone17S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthAmerican1969UTMZone18N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SouthAmerican1969UTMZone18N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthAmerican1969UTMZone18S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SouthAmerican1969UTMZone18S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthAmerican1969UTMZone19N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SouthAmerican1969UTMZone19N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthAmerican1969UTMZone19S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SouthAmerican1969UTMZone19S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthAmerican1969UTMZone20N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SouthAmerican1969UTMZone20N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthAmerican1969UTMZone20S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SouthAmerican1969UTMZone20S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthAmerican1969UTMZone21N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SouthAmerican1969UTMZone21N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthAmerican1969UTMZone21S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SouthAmerican1969UTMZone21S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthAmerican1969UTMZone22N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SouthAmerican1969UTMZone22N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthAmerican1969UTMZone22S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SouthAmerican1969UTMZone22S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthAmerican1969UTMZone23S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SouthAmerican1969UTMZone23S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthAmerican1969UTMZone24S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SouthAmerican1969UTMZone24S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SouthAmerican1969UTMZone25S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SouthAmerican1969UTMZone25S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ST71BelepUTMZone58S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ST71BelepUTMZone58S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ST84IledesPinsUTMZone58S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ST84IledesPinsUTMZone58S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ST87OuveaUTMZone58S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.ST87OuveaUTMZone58S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SudanUTMZone35N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SudanUTMZone35N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SudanUTMZone36N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.SudanUTMZone36N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void TahaaUTMZone5S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.TahaaUTMZone5S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void TahitiUTMZone6S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.TahitiUTMZone6S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Tananarive1925UTMZone38S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Tananarive1925UTMZone38S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Tananarive1925UTMZone39S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Tananarive1925UTMZone39S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void TeteUTMZone36S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.TeteUTMZone36S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void TeteUTMZone37S()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.TeteUTMZone37S;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Timbalai1948UTMZone49N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Timbalai1948UTMZone49N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Timbalai1948UTMZone50N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Timbalai1948UTMZone50N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void TokyoUTMZone51N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.TokyoUTMZone51N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void TokyoUTMZone52N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.TokyoUTMZone52N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void TokyoUTMZone53N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.TokyoUTMZone53N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void TokyoUTMZone54N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.TokyoUTMZone54N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void TokyoUTMZone55N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.TokyoUTMZone55N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void TokyoUTMZone56N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.TokyoUTMZone56N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void TrucialCoast1948UTMZone39N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.TrucialCoast1948UTMZone39N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void TrucialCoast1948UTMZone40N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.TrucialCoast1948UTMZone40N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void YemenNGN1996UTMZone38N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.YemenNGN1996UTMZone38N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void YemenNGN1996UTMZone39N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.YemenNGN1996UTMZone39N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Yoff1972UTMZone28N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Yoff1972UTMZone28N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Zanderij1972UTMZone21N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.UtmOther.Zanderij1972UTMZone21N;
            Tester.TestProjection(pStart);
        }
    }
}