
using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
{
    /// <summary>
    /// This class contains all the tests for the WorldSpheroid category of Projected coordinate systems
    /// </summary>
    [TestFixture]
    public class WorldSpheroid
    {
        /// <summary>
        /// Creates a new instance of the Africa Class
        /// </summary>
        [SetUp]
        public void Initialize()
        {

        }

        /// <summary>
        /// Test for Aitoffsphere       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void Aitoffsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.Aitoffsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Behrmannsphere       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void Behrmannsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.Behrmannsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Bonnesphere       
        /// </summary>
        [Test, Category("Projection")]
        public void Bonnesphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.Bonnesphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for CrasterParabolicsphere       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void CrasterParabolicsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.CrasterParabolicsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for CylindricalEqualAreasphere       
        /// </summary>
        [Test, Category("Projection")]
        public void CylindricalEqualAreasphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.CylindricalEqualAreasphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for EckertIIIsphere       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void EckertIIIsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.EckertIIIsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for EckertIIsphere       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void EckertIIsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.EckertIIsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for EckertIsphere       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void EckertIsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.EckertIsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for EckertIVsphere       
        /// </summary>
        [Test, Category("Projection")]
        public void EckertIVsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.EckertIVsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for EckertVIsphere       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void EckertVIsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.EckertVIsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for EckertVsphere       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void EckertVsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.EckertVsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for EquidistantConicsphere       
        /// </summary>
        [Test, Category("Projection")]
        public void EquidistantConicsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.EquidistantConicsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for EquidistantCylindricalsphere       
        /// </summary>
        [Test, Category("Projection")]
        public void EquidistantCylindricalsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.EquidistantCylindricalsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for FlatPolarQuarticsphere       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void FlatPolarQuarticsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.FlatPolarQuarticsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for GallStereographicsphere       
        /// </summary>
        [Test, Category("Projection")]
        public void GallStereographicsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.GallStereographicsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for HammerAitoffsphere       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void HammerAitoffsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.HammerAitoffsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Loximuthalsphere       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void Loximuthalsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.Loximuthalsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Mercatorsphere       
        /// </summary>
        [Test, Category("Projection")]
        public void Mercatorsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.Mercatorsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for MillerCylindricalsphere       
        /// </summary>
        [Test, Category("Projection")]
        public void MillerCylindricalsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.MillerCylindricalsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Mollweidesphere       
        /// </summary>
        [Test, Category("Projection")]
        public void Mollweidesphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.Mollweidesphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for PlateCarreesphere       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void PlateCarreesphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.PlateCarreesphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Polyconicsphere       
        /// </summary>
        [Test, Category("Projection")]
        public void Polyconicsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.Polyconicsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for QuarticAuthalicsphere       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void QuarticAuthalicsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.QuarticAuthalicsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Robinsonsphere       
        /// </summary>
        [Test, Category("Projection")]
        public void Robinsonsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.Robinsonsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Sinusoidalsphere       
        /// </summary>
        [Test, Category("Projection")]
        public void Sinusoidalsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.Sinusoidalsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Timessphere       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void Timessphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.Timessphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for VanderGrintenIsphere       
        /// </summary>
        [Test, Category("Projection")]
        public void VanderGrintenIsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.VanderGrintenIsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for VerticalPerspectivesphere       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void VerticalPerspectivesphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.VerticalPerspectivesphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for WinkelIIsphere       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void WinkelIIsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.WinkelIIsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for WinkelIsphere       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void WinkelIsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.WinkelIsphere;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for WinkelTripelNGSsphere       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void WinkelTripelNGSsphere()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.WorldSpheroid.WinkelTripelNGSsphere;
            Tester.TestProjection(pStart);
        }
    }
}