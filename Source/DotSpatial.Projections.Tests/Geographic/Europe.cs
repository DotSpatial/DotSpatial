using System;
using System.Collections.Generic;

using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Geographic
{
    /// <summary>
    /// This class contains all the tests for the Europe category of Geographic coordinate systems
    /// </summary>
    [TestFixture]
    public class Europe
    {
        [Test]
        public void Belge1972Epsg31370()
        {
            // see https://dotspatial.codeplex.com/discussions/548133

            var source = ProjectionInfo.FromEpsgCode(31370);
            var dest = KnownCoordinateSystems.Geographic.World.WGS1984;
            double[] vertices = { 156117.21, 133860.06 };
            Reproject.ReprojectPoints(vertices, null, source, dest, 0, 1);

            Assert.IsTrue(Math.Abs(vertices[0] - 4.455) < 1e-7);
            Assert.IsTrue(Math.Abs(vertices[1] - 50.515485) < 1e-7);
        }

        [Test]
        [TestCaseSource(nameof(GetProjections))]
        public void EuropeTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Geographic.Europe);
        }



        //[Test]
        //public void Albanian1987()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Albanian1987;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void AtfParis()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.ATFParis;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Belge1950Brussels()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Belge1950Brussels;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Belge1972()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Belge1972;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Bern1898()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Bern1898;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Bern1898Bern()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Bern1898Bern;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Bern1938()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Bern1938;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Ch1903()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.CH1903;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Datum73()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Datum73;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void DatumLisboaBessel()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.DatumLisboaBessel;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void DatumLisboaHayford()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.DatumLisboaHayford;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void DealulPiscului1933Romania()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.DealulPiscului1933Romania;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void DealulPiscului1970Romania()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.DealulPiscului1970Romania;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void DeutscheHauptdreiecksnetz()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.DeutscheHauptdreiecksnetz;
        //    Tester.TestProjection(pStart);
        //}

        //[Test]
        //public void Amersfoort()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Amersfoort;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Estonia1937()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Estonia1937;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Estonia1992()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Estonia1992;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Estonia1997()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Estonia1997;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Etrf1989()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.ETRF1989;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Etrs1989()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.ETRS1989;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Eureffin()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.EUREFFIN;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void European1979()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.European1979;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void EuropeanDatum1950()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.EuropeanDatum1950;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void EuropeanDatum1987()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.EuropeanDatum1987;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Greek()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Greek;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void GreekAthens()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.GreekAthens;
        //    Tester.TestProjection(pStart);
        //}

        //[Test]
        //public void Hermannskogel()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Hermannskogel;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Hjorsey1955()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Hjorsey1955;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void HungarianDatum1972()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.HungarianDatum1972;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Irenet95()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.IRENET95;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Isn1993()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.ISN1993;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Kartastokoordinaattijarjestelma()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Kartastokoordinaattijarjestelma;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Lisbon()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Lisbon;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void LisbonLisbon()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.LisbonLisbon;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Lisbon1890()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Lisbon1890;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Lisbon1890Lisbon()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Lisbon1890Lisbon;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Lks1992()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.LKS1992;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Lks1994()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.LKS1994;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Luxembourg1930()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Luxembourg1930;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Madrid1870Madrid()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Madrid1870Madrid;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void MgiFerro()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.MGIFerro;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void MilitarGeographischeInstitut()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.MilitarGeographischeInstitut;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void MonteMario()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.MonteMario;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void MonteMarioRome()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.MonteMarioRome;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Ngo1948()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.NGO1948;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Ngo1948Oslo()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.NGO1948Oslo;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void NorddeGuerreParis()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.NorddeGuerreParis;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void NouvelleTriangulationFrancaise()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.NouvelleTriangulationFrancaise;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void NtfParis()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.NTFParis;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Ossn1980()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.OSSN1980;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Osgb1936()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.OSGB1936;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Osgb1970Sn()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.OSGB1970SN;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Osni1952()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.OSNI1952;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Pulkovo1942()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Pulkovo1942;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Pulkovo1942Adj1958()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Pulkovo1942Adj1958;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Pulkovo1942Adj1983()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Pulkovo1942Adj1983;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Pulkovo1995()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Pulkovo1995;
        //    Tester.TestProjection(pStart);
        //}
        
        //[Test]
        //public void ReseauNationalBelge1950()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.ReseauNationalBelge1950;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Reykjavik1900()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Reykjavik1900;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Rgf1993()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.RGF1993;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Roma1940()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.Roma1940;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Rt1990()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.RT1990;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Rt38()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.RT38;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Rt38Stockholm()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.RT38Stockholm;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void S42Hungary()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.S42Hungary;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Sjtsk()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.SJTSK;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Sweref99()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.SWEREF99;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void SwissTrf1995()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.SwissTRF1995;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Tm65()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.TM65;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void Tm75()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Geographic.Europe.TM75;
        //    Tester.TestProjection(pStart);
        //}
    }
}
