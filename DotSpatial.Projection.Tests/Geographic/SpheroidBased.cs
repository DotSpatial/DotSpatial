
using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projections.Tests.Geographic
{
    /// <summary>
    /// This class contains all the tests for the SpheroidBased category of Geographic coordinate systems
    /// </summary>
    [TestFixture]
    public class SpheroidBased
    {
        /// <summary>
        /// Creates a new instance of the Africa Class
        /// </summary>
        [TestFixtureSetUp]
        public void Initialize()
        {
            
        }

        [Test]
        public void Airy1830()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Airy1830;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Airymodified()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Airymodified;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AustralianNational()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.AustralianNational;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Authalicsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Authalicsphere;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AuthalicsphereARCINFO()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.AuthalicsphereARCINFO;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void AverageTerrestrialSystem1977()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.AverageTerrestrialSystem1977;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Bessel1841()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Bessel1841;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Besselmodified()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Besselmodified;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore("Verify this test")]
        public void BesselNamibia()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.BesselNamibia;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Clarke1858()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Clarke1858;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Clarke1866()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Clarke1866;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Clarke1866Michigan()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Clarke1866Michigan;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Clarke1880()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Clarke1880;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Clarke1880Arc()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Clarke1880Arc;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Clarke1880Benoit()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Clarke1880Benoit;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Clarke1880IGN()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Clarke1880IGN;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Clarke1880RGS()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Clarke1880RGS;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Clarke1880SGA()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Clarke1880SGA;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Everestdefinition1967()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Everestdefinition1967;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Everestdefinition1975()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Everestdefinition1975;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Everest1830()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Everest1830;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Everestmodified()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Everestmodified;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Everestmodified1969()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Everestmodified1969;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Fischer1960()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Fischer1960;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Fischer1968()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Fischer1968;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Fischermodified()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Fischermodified;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void GRS1967()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.GRS1967;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void GRS1980()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.GRS1980;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Helmert1906()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Helmert1906;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Hough1960()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Hough1960;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void IndonesianNational()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.IndonesianNational;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void International1924()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.International1924;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void International1967()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.International1967;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Krasovsky1940()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Krasovsky1940;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void OSU1986geoidalmodel()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.OSU1986geoidalmodel;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void OSU1991geoidalmodel()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.OSU1991geoidalmodel;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Plessis1817()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Plessis1817;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void SphereEMEP()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.SphereEMEP;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Struve1860()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Struve1860;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Transitpreciseephemeris()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Transitpreciseephemeris;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Walbeck()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.Walbeck;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void WarOffice()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.WarOffice;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void WGS1966()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Geographic.SpheroidBased.WGS1966;
            Tester.TestProjection(pStart);
        }
    }
}
