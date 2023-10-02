using NUnit.Framework;

namespace DotSpatial.Projections.Tests.Projected
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
        [SetUp]
        public void Initialize()
        {

        }

        /// <summary>
        /// Test for Aitoffworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void Aitoffworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Aitoffworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Behrmannworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void Behrmannworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Behrmannworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Bonneworld       
        /// </summary>
        [Test, Category("Projection")]
        public void Bonneworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Bonneworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for CrasterParabolicworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void CrasterParabolicworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.CrasterParabolicworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Cubeworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void Cubeworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Cubeworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for CylindricalEqualAreaworld       
        /// </summary>
        [Test, Category("Projection")]
        public void CylindricalEqualAreaworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.CylindricalEqualAreaworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for EckertIIIworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void EckertIIIworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EckertIIIworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for EckertIIworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void EckertIIworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EckertIIworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for EckertIVworld       
        /// </summary>
        [Test, Category("Projection")]
        public void EckertIVworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EckertIVworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for EckertIworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void EckertIworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EckertIworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for EckertVIworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void EckertVIworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EckertVIworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for EckertVworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void EckertVworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EckertVworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for EquidistantConicworld       
        /// </summary>
        [Test, Category("Projection")]
        public void EquidistantConicworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EquidistantConicworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for EquidistantCylindricalworld       
        /// </summary>
        [Test, Category("Projection")]
        public void EquidistantCylindricalworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.EquidistantCylindricalworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for FlatPolarQuarticworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void FlatPolarQuarticworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.FlatPolarQuarticworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Fullerworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void Fullerworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Fullerworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for GallStereographicworld       
        /// </summary>
        [Test, Category("Projection")]
        public void GallStereographicworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.GallStereographicworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for HammerAitoffworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void HammerAitoffworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.HammerAitoffworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Loximuthalworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void Loximuthalworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Loximuthalworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Mercatorworld       
        /// </summary>
        [Test, Category("Projection")]
        public void Mercatorworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Mercatorworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for MillerCylindricalworld       
        /// </summary>
        [Test, Category("Projection")]
        public void MillerCylindricalworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.MillerCylindricalworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Mollweideworld       
        /// </summary>
        [Test, Category("Projection")]
        public void Mollweideworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Mollweideworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for PlateCarreeworld       
        /// </summary>
        [Test, Category("Projection")]
        public void PlateCarreeworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.PlateCarreeworld;
            Tester.TestProjection(pStart);
        }

        /// <summary>
        /// Test for PlateCarreeFromEsriString       
        /// </summary>
        [Test, Category("Projection")]
        public void PlateCarreeFromEsriString()
        {
            ProjectionInfo pStart = ProjectionInfo.FromEsriString("PROJCS[\"WGS_1984_Plate_Carree\",GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\",SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]],PROJECTION[\"Plate_Carree\"],PARAMETER[\"False_Easting\",0.0],PARAMETER[\"False_Northing\",0.0],PARAMETER[\"Central_Meridian\",0.0],UNIT[\"Meter\",1.0]]");
            Tester.TestProjection(pStart);
        }

        /// <summary>
        /// Test for Polyconicworld       
        /// </summary>
        [Test, Category("Projection")]
        public void Polyconicworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Polyconicworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for QuarticAuthalicworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void QuarticAuthalicworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.QuarticAuthalicworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Robinsonworld       
        /// </summary>
        [Test, Category("Projection")]
        public void Robinsonworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Robinsonworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Sinusoidalworld       
        /// </summary>
        [Test, Category("Projection")]
        public void Sinusoidalworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Sinusoidalworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for TheWorldfromSpace       
        /// </summary>
        [Test, Category("Projection")]
        public void TheWorldfromSpace()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.TheWorldfromSpace;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for Timesworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void Timesworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.Timesworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for VanderGrintenIworld       
        /// </summary>
        [Test, Category("Projection")]
        public void VanderGrintenIworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.VanderGrintenIworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for VerticalPerspectiveworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void VerticalPerspectiveworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.VerticalPerspectiveworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for WebMercator       
        /// </summary>
        [Test, Category("Projection")]
        public void WebMercator()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.WebMercator;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for WinkelIIworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void WinkelIIworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.WinkelIIworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for WinkelIworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void WinkelIworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.WinkelIworld;
            Tester.TestProjection(pStart);
        }


        /// <summary>
        /// Test for WinkelTripelNGSworld       
        /// </summary>
        [Test, Category("Projection")]
        [Ignore("")]
        public void WinkelTripelNGSworld()
        {
            ProjectionInfo pStart = KnownCoordinateSystems.Projected.World.WinkelTripelNGSworld;
            Tester.TestProjection(pStart);
        }
    }
}