using NUnit.Framework;
using DotSpatial.Projections;

namespace DotSpatial.Projection.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the World category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class ProjectedWorld
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
        [Ignore]
        public void Aitoffworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Aitoffworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void Behrmannworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Behrmannworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Bonneworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Bonneworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void CrasterParabolicworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.CrasterParabolicworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void Cubeworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Cubeworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void CylindricalEqualAreaworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.CylindricalEqualAreaworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void EckertIIIworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EckertIIIworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void EckertIIworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EckertIIworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EckertIVworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EckertIVworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void EckertIworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EckertIworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void EckertVIworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EckertVIworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void EckertVworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EckertVworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EquidistantConicworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EquidistantConicworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void EquidistantCylindricalworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EquidistantCylindricalworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void FlatPolarQuarticworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.FlatPolarQuarticworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void Fullerworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Fullerworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void GallStereographicworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.GallStereographicworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void HammerAitoffworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.HammerAitoffworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void Loximuthalworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Loximuthalworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Mercatorworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Mercatorworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void MillerCylindricalworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.MillerCylindricalworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Mollweideworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Mollweideworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void PlateCarreeworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.PlateCarreeworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Polyconicworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Polyconicworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void QuarticAuthalicworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.QuarticAuthalicworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Robinsonworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Robinsonworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void Sinusoidalworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Sinusoidalworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void TheWorldfromSpace()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.TheWorldfromSpace;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void Timesworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Timesworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void VanderGrintenIworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.VanderGrintenIworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void VerticalPerspectiveworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.VerticalPerspectiveworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        public void WebMercator()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.WebMercator;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void WinkelIIworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.WinkelIIworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void WinkelIworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.WinkelIworld;
            Tester.TestProjection(pStart);
        }


        [Test]
        [Ignore]
        public void WinkelTripelNGSworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.WinkelTripelNGSworld;
            Tester.TestProjection(pStart);
        }
    }
}