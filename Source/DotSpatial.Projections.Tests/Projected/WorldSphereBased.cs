using System.Collections.Generic;
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the WorldSphereBased category of Projected coordinate systems.
    /// </summary>
    [TestFixture]
    public class WorldSphereBased
    {
        [Test]
        [TestCaseSource(nameof(GetProjections))]
        public void ProjectedTests(ProjectionInfoDesc pInfo)
        {
            Tester.TestProjection(pInfo.ProjectionInfo);
            Assert.AreEqual(false, pInfo.ProjectionInfo.IsLatLon);
        }

        private static IEnumerable<ProjectionInfoDesc> GetProjections()
        {
            return ProjectionInfoDesc.GetForCoordinateSystemCategory(KnownCoordinateSystems.Projected.WorldSphereBased);
        }

        //    [Test]
        //    [Ignore]
        //    public void AitoffSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.AitoffSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    [Ignore]
        //    public void BehrmannSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.BehrmannSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    public void BonneSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.BonneSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    [Ignore]
        //    public void CrasterParabolicSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.CrasterParabolicSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    public void CylindricalEqualAreaSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.CylindricalEqualAreaSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    [Ignore]
        //    public void EckertIiiSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.EckertIIISphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    [Ignore]
        //    public void EckertIiSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.EckertIISphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    [Ignore]
        //    public void EckertISphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.EckertISphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    public void EckertIvSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.EckertIVSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    [Ignore]
        //    public void EckertViSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.EckertVISphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    [Ignore]
        //    public void EckertVSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.EckertVSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    public void EquidistantConicSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.EquidistantConicSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    public void EquidistantCylindricalSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.EquidistantCylindricalSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    [Ignore]
        //    public void FlatPolarQuarticSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.FlatPolarQuarticSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    public void GallStereographicSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.GallStereographicSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    [Ignore]
        //    public void HammerAitoffSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.HammerAitoffSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    [Ignore]
        //    public void LoximuthalSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.LoximuthalSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    public void MercatorSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.MercatorSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    public void MillerCylindricalSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.MillerCylindricalSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    public void MollweideSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.MollweideSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    [Ignore]
        //    public void PlateCarreeSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.PlateCarreeSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    public void PolyconicSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.PolyconicSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    [Ignore]
        //    public void QuarticAuthalicSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.QuarticAuthalicSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    public void RobinsonSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.RobinsonSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    public void SinusoidalSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.SinusoidalSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    [Ignore]
        //    public void TimesSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.TimesSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    public void VanderGrintenISphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.VanderGrintenISphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    [Ignore]
        //    public void VerticalPerspectiveSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.VerticalPerspectiveSphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    [Ignore]
        //    public void WinkelIiSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.WinkelIISphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    [Ignore]
        //    public void WinkelISphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.WinkelISphere;
        //        Tester.TestProjection(pStart);
        //    }


        //    [Test]
        //    [Ignore]
        //    public void WinkelTripelNgsSphere()
        //    {
        //        ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSphereBased.WinkelTripelNGSSphere;
        //        Tester.TestProjection(pStart);
        //    }
    }
}