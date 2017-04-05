// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:13:38 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.ProjectedCategories
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for WorldSphereBased.
    /// </summary>
    public class WorldSphereBased : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AitoffSphere;
        public readonly ProjectionInfo AzimuthalEquidistantSphere;
        public readonly ProjectionInfo BehrmannSphere;
        public readonly ProjectionInfo BonneSphere;
        public readonly ProjectionInfo CassiniSphere;
        public readonly ProjectionInfo CrasterParabolicSphere;
        public readonly ProjectionInfo CylindricalEqualAreaSphere;
        public readonly ProjectionInfo EckertIIISphere;
        public readonly ProjectionInfo EckertIISphere;
        public readonly ProjectionInfo EckertISphere;
        public readonly ProjectionInfo EckertIVSphere;
        public readonly ProjectionInfo EckertVISphere;
        public readonly ProjectionInfo EckertVSphere;
        public readonly ProjectionInfo EquidistantConicSphere;
        public readonly ProjectionInfo EquidistantCylindricalSphere;
        // public readonly ProjectionInfo FlatPolarQuarticSphere;
        public readonly ProjectionInfo GallStereographicSphere;
        public readonly ProjectionInfo HammerAitoffSphere;
        public readonly ProjectionInfo HotineSphere;
        public readonly ProjectionInfo LoximuthalSphere;
        public readonly ProjectionInfo MercatorSphere;
        public readonly ProjectionInfo MillerCylindricalSphere;
        public readonly ProjectionInfo MollweideSphere;
        public readonly ProjectionInfo PlateCarreeSphere;
        public readonly ProjectionInfo PolyconicSphere;
        public readonly ProjectionInfo QuarticAuthalicSphere;
        public readonly ProjectionInfo RobinsonSphere;
        public readonly ProjectionInfo SinusoidalSphere;
        public readonly ProjectionInfo StereographicSphere;
        // public readonly ProjectionInfo TimesSphere;
        public readonly ProjectionInfo TwoPointEquidistantSphere;
        public readonly ProjectionInfo VanderGrintenISphere;
        // public readonly ProjectionInfo VerticalPerspectiveSphere;
        public readonly ProjectionInfo WinkelIISphere;
        public readonly ProjectionInfo WinkelISphere;
        public readonly ProjectionInfo WinkelTripelNGSSphere;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of WorldSphereBased.
        /// </summary>
        public WorldSphereBased()
        {
            AitoffSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53043).SetNames("Sphere_Aitoff", "GCS_Sphere", "D_Sphere"); // missing
            AzimuthalEquidistantSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53032).SetNames("Sphere_Azimuthal_Equidistant", "GCS_Sphere", "D_Sphere");
            BehrmannSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53017).SetNames("Sphere_Behrmann", "GCS_Sphere", "D_Sphere");
            BonneSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53024).SetNames("Sphere_Bonne", "GCS_Sphere", "D_Sphere");
            CassiniSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53028).SetNames("Sphere_Cassini", "GCS_Sphere", "D_Sphere");
            CrasterParabolicSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53046).SetNames("Sphere_Craster_Parabolic", "GCS_Sphere", "D_Sphere"); // missing
            CylindricalEqualAreaSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53034).SetNames("Sphere_Cylindrical_Equal_Area", "GCS_Sphere", "D_Sphere"); // missing
            EckertIIISphere = ProjectionInfo.FromAuthorityCode("ESRI", 53013).SetNames("Sphere_Eckert_III", "GCS_Sphere", "D_Sphere");
            EckertIISphere = ProjectionInfo.FromAuthorityCode("ESRI", 53014).SetNames("Sphere_Eckert_II", "GCS_Sphere", "D_Sphere");
            EckertISphere = ProjectionInfo.FromAuthorityCode("ESRI", 53015).SetNames("Sphere_Eckert_I", "GCS_Sphere", "D_Sphere");
            EckertIVSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53012).SetNames("Sphere_Eckert_IV", "GCS_Sphere", "D_Sphere");
            EckertVISphere = ProjectionInfo.FromAuthorityCode("ESRI", 53010).SetNames("Sphere_Eckert_VI", "GCS_Sphere", "D_Sphere");
            EckertVSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53011).SetNames("Sphere_Eckert_V", "GCS_Sphere", "D_Sphere");
            EquidistantConicSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53027).SetNames("Sphere_Equidistant_Conic", "GCS_Sphere", "D_Sphere");
            EquidistantCylindricalSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53002).SetNames("Sphere_Equidistant_Cylindrical", "GCS_Sphere", "D_Sphere");
            // FlatPolarQuarticSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53045).SetNames("Sphere_Flat_Polar_Quartic", "GCS_Sphere", "D_Sphere"); // projection not found
            GallStereographicSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53016).SetNames("Sphere_Gall_Stereographic", "GCS_Sphere", "D_Sphere");
            HammerAitoffSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53044).SetNames("Sphere_Hammer_Aitoff", "GCS_Sphere", "D_Sphere"); // missing
            HotineSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53025).SetNames("Sphere_Hotine", "GCS_Sphere", "D_Sphere");
            LoximuthalSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53023).SetNames("Sphere_Loximuthal", "GCS_Sphere", "D_Sphere");
            MercatorSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53004).SetNames("Sphere_Mercator", "GCS_Sphere", "D_Sphere");
            MillerCylindricalSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53003).SetNames("Sphere_Miller_Cylindrical", "GCS_Sphere", "D_Sphere");
            MollweideSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53009).SetNames("Sphere_Mollweide", "GCS_Sphere", "D_Sphere");
            PlateCarreeSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53001).SetNames("Sphere_Plate_Carree", "GCS_Sphere", "D_Sphere");
            PolyconicSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53021).SetNames("Sphere_Polyconic", "GCS_Sphere", "D_Sphere");
            QuarticAuthalicSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53022).SetNames("Sphere_Quartic_Authalic", "GCS_Sphere", "D_Sphere");
            RobinsonSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53030).SetNames("Sphere_Robinson", "GCS_Sphere", "D_Sphere");
            SinusoidalSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53008).SetNames("Sphere_Sinusoidal", "GCS_Sphere", "D_Sphere");
            StereographicSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53026).SetNames("Sphere_Stereographic", "GCS_Sphere", "D_Sphere");
            // TimesSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53048).SetNames("Sphere_Times", "GCS_Sphere", "D_Sphere"); // projection not found
            TwoPointEquidistantSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53031).SetNames("Sphere_Two_Point_Equidistant", "GCS_Sphere", "D_Sphere");
            VanderGrintenISphere = ProjectionInfo.FromAuthorityCode("ESRI", 53029).SetNames("Sphere_Van_der_Grinten_I", "GCS_Sphere", "D_Sphere");
            // VerticalPerspectiveSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53049).SetNames("Sphere_Vertical_Perspective", "GCS_WGS_1984", "D_WGS_1984"); // projection not found
            WinkelIISphere = ProjectionInfo.FromAuthorityCode("ESRI", 53019).SetNames("Sphere_Winkel_II", "GCS_Sphere", "D_Sphere");
            WinkelISphere = ProjectionInfo.FromAuthorityCode("ESRI", 53018).SetNames("Sphere_Winkel_I", "GCS_Sphere", "D_Sphere");
            WinkelTripelNGSSphere = ProjectionInfo.FromAuthorityCode("ESRI", 53042).SetNames("Sphere_Winkel_Tripel_NGS", "GCS_Sphere", "D_Sphere"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591