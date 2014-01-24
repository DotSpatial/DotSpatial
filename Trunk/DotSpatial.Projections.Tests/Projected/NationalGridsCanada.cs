using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the NationalGridsCanada category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class NationalGridsCanada
    {
        /// <summary>
        /// Creates a new instance of the Africa Class
        /// </summary>
        [TestFixtureSetUp]
        public void Initialize()
        {
            
        }

        [Test]
        public void ATS1977MTM4NovaScotia()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.ATS1977MTM4NovaScotia;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void ATS1977MTM5NovaScotia()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.ATS1977MTM5NovaScotia;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore("Verify")]
        public void ATS1977NewBrunswickStereographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.ATS1977NewBrunswickStereographic;
            Tester.TestProjection(pStart);
        }

        [Test]
        public void NAD192710TMAEPForest()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD192710TMAEPForest;
            Tester.TestProjection(pStart);
        }

        [Test]
        public void NAD192710TMAEPResource()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD192710TMAEPResource;
            Tester.TestProjection(pStart);
        }

        [Test]
        public void NAD19273TM111()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD19273TM111;
            Tester.TestProjection(pStart);
        }

        [Test]
        public void NAD19273TM114()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD19273TM114;
            Tester.TestProjection(pStart);
        }

        [Test]
        public void NAD19273TM117()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD19273TM117;
            Tester.TestProjection(pStart);
        }

        [Test]
        public void NAD19273TM120()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD19273TM120;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927CGQ77MTM10SCoPQ()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927CGQ77MTM10SCoPQ;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927CGQ77MTM2SCoPQ()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927CGQ77MTM2SCoPQ;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927CGQ77MTM3SCoPQ()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927CGQ77MTM3SCoPQ;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927CGQ77MTM4SCoPQ()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927CGQ77MTM4SCoPQ;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927CGQ77MTM5SCoPQ()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927CGQ77MTM5SCoPQ;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927CGQ77MTM6SCoPQ()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927CGQ77MTM6SCoPQ;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927CGQ77MTM7SCoPQ()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927CGQ77MTM7SCoPQ;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927CGQ77MTM8SCoPQ()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927CGQ77MTM8SCoPQ;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927CGQ77MTM9SCoPQ()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927CGQ77MTM9SCoPQ;
            Tester.TestProjection(pStart);
        }

        [Ignore("Doesn't work on x64 TeamCity. x86 is fine.")]
        [Test]
        public void NAD1927CGQ77QuebecLambert()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927CGQ77QuebecLambert;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927CGQ77UTMZone17N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927CGQ77UTMZone17N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927CGQ77UTMZone18N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927CGQ77UTMZone18N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927CGQ77UTMZone19N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927CGQ77UTMZone19N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927CGQ77UTMZone20N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927CGQ77UTMZone20N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927CGQ77UTMZone21N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927CGQ77UTMZone21N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927DEF1976MTM10()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927DEF1976MTM10;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927DEF1976MTM11()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927DEF1976MTM11;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927DEF1976MTM12()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927DEF1976MTM12;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927DEF1976MTM13()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927DEF1976MTM13;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927DEF1976MTM14()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927DEF1976MTM14;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927DEF1976MTM15()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927DEF1976MTM15;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927DEF1976MTM16()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927DEF1976MTM16;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927DEF1976MTM17()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927DEF1976MTM17;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927DEF1976MTM8()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927DEF1976MTM8;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927DEF1976MTM9()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927DEF1976MTM9;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927DEF1976UTMZone15N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927DEF1976UTMZone15N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927DEF1976UTMZone16N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927DEF1976UTMZone16N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927DEF1976UTMZone17N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927DEF1976UTMZone17N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1927DEF1976UTMZone18N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927DEF1976UTMZone18N;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927MTM1()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927MTM1;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927MTM2()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927MTM2;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927MTM3()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927MTM3;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927MTM4()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927MTM4;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927MTM5()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927MTM5;
            Tester.TestProjection(pStart);
        }

        
        [Test]
        public void NAD1927MTM6()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927MTM6;
            Tester.TestProjection(pStart);
        }


        [Test]
        
        public void NAD1927QuebecLambert()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1927QuebecLambert;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD198310TMAEPForest()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD198310TMAEPForest;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD198310TMAEPResource()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD198310TMAEPResource;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD19833TM111()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD19833TM111;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD19833TM114()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD19833TM114;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD19833TM117()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD19833TM117;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD19833TM120()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD19833TM120;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983BCEnvironmentAlbers()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983BCEnvironmentAlbers;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983CSRS98MTM10()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98MTM10;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983CSRS98MTM2SCoPQ()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98MTM2SCoPQ;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983CSRS98MTM3()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98MTM3;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983CSRS98MTM4()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98MTM4;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983CSRS98MTM5()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98MTM5;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983CSRS98MTM6()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98MTM6;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983CSRS98MTM7()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98MTM7;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983CSRS98MTM8()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98MTM8;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983CSRS98MTM9()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98MTM9;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore("Verify")]
        public void NAD1983CSRS98NewBrunswickStereographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98NewBrunswickStereographic;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore("Verify")]
        public void NAD1983CSRS98PrinceEdwardIsland()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98PrinceEdwardIsland;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983CSRS98UTMZone11N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98UTMZone11N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983CSRS98UTMZone12N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98UTMZone12N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983CSRS98UTMZone13N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98UTMZone13N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983CSRS98UTMZone17N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98UTMZone17N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983CSRS98UTMZone18N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98UTMZone18N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983CSRS98UTMZone19N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98UTMZone19N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983CSRS98UTMZone20N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98UTMZone20N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983CSRS98UTMZone21N()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983CSRS98UTMZone21N;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983MTM1()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983MTM1;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983MTM10()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983MTM10;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983MTM11()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983MTM11;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983MTM12()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983MTM12;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983MTM13()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983MTM13;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983MTM14()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983MTM14;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983MTM15()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983MTM15;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983MTM16()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983MTM16;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983MTM17()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983MTM17;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983MTM2()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983MTM2;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983MTM2SCoPQ()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983MTM2SCoPQ;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983MTM3()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983MTM3;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983MTM4()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983MTM4;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983MTM5()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983MTM5;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983MTM6()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983MTM6;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983MTM7()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983MTM7;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983MTM8()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983MTM8;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983MTM9()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983MTM9;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void NAD1983QuebecLambert()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.NAD1983QuebecLambert;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore("Verify")]
        public void PrinceEdwardIslandStereographic()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.NationalGridsCanada.PrinceEdwardIslandStereographic;
            Tester.TestProjection(pStart);
        }
    }
}