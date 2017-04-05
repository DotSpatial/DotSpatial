using System.Collections.Generic;

using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the World category of Projected coordinate systems.
    /// </summary>
    [TestFixture]
    public class World
    {
        [Test]
        [TestCaseSource(nameof(GetProjections))]
        public void ProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.World);
        }

        //[Test]
        //[Ignore]
        //public void AitoffWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.AitoffWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void BehrmannWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.BehrmannWorld;
        //    Tester.TestProjection(pStart);
        //}


        [Test]
        public void BonneWorld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.BonneWorld;
            Tester.TestProjection(pStart);
        }


        //[Test]
        //[Ignore]
        //public void CrasterParabolicWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.CrasterParabolicWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void CubeWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.CubeWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void CylindricalEqualAreaWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.CylindricalEqualAreaWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void EckertIiiWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EckertIIIWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void EckertIiWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EckertIIWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void EckertIvWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EckertIVWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void EckertIWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EckertIWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void EckertViWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EckertVIWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void EckertVWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EckertVWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void EquidistantConicWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EquidistantConicWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void EquidistantCylindricalWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EquidistantCylindricalWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void FlatPolarQuarticWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.FlatPolarQuarticWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void FullerWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.FullerWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void GallStereographicWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.GallStereographicWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void HammerAitoffWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.HammerAitoffWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void LoximuthalWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.LoximuthalWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void MercatorWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.MercatorWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void MillerCylindricalWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.MillerCylindricalWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void MollweideWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.MollweideWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void PlateCarreeWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.PlateCarreeWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void PolyconicWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.PolyconicWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void QuarticAuthalicWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.QuarticAuthalicWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void RobinsonWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.RobinsonWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void SinusoidalWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.SinusoidalWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void TheWorldfromSpace()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.TheWorldfromSpace;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void TimesWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.TimesWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void VanderGrintenIWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.VanderGrintenIWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void VerticalPerspectiveWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.VerticalPerspectiveWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //public void WebMercator()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.WebMercator;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void WinkelIiWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.WinkelIIWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void WinkelIWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.WinkelIWorld;
        //    Tester.TestProjection(pStart);
        //}


        //[Test]
        //[Ignore]
        //public void WinkelTripelNgsWorld()
        //{
        //    ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.WinkelTripelNGSWorld;
        //    Tester.TestProjection(pStart);
        //}
    }
}