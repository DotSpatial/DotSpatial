// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:12:33 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// Jiri Kadlec         | 11/20/2010 |  Updated the proj4 string definition of Web Mercator Auxiliary Sphere
// ********************************************************************************************************

using DotSpatial.Projections.Transforms;

#pragma warning disable 1591

namespace DotSpatial.Projections.ProjectedCategories
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for World.
    /// </summary>
    public class World : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AitoffWorld;
        public readonly ProjectionInfo AzimuthalEquidistantWorld;
        public readonly ProjectionInfo BehrmannWorld;
        //public readonly ProjectionInfo BerghausStarAAG;
        public readonly ProjectionInfo BonneWorld;
        public readonly ProjectionInfo CassiniWorld;
        public readonly ProjectionInfo CrasterParabolicWorld;
        //  public readonly ProjectionInfo CubeWorld;
        public readonly ProjectionInfo CylindricalEqualAreaWorld;
        public readonly ProjectionInfo EckertIIIWorld;
        public readonly ProjectionInfo EckertIIWorld;
        public readonly ProjectionInfo EckertIVWorld;
        public readonly ProjectionInfo EckertIWorld;
        public readonly ProjectionInfo EckertVIWorld;
        public readonly ProjectionInfo EckertVWorld;
        public readonly ProjectionInfo EquidistantConicWorld;
        public readonly ProjectionInfo EquidistantCylindricalWorld;
        // public readonly ProjectionInfo FlatPolarQuarticWorld;
        // public readonly ProjectionInfo FullerWorld;
        public readonly ProjectionInfo GallStereographicWorld;
        public readonly ProjectionInfo GoodeHomolosineLand;
        public readonly ProjectionInfo GoodeHomolosineOcean;
        public readonly ProjectionInfo HammerAitoffWorld;
        public readonly ProjectionInfo HotineWorld;
        public readonly ProjectionInfo LoximuthalWorld;
        public readonly ProjectionInfo MercatorWorld;
        public readonly ProjectionInfo MillerCylindricalWorld;
        public readonly ProjectionInfo MollweideWorld;
        public readonly ProjectionInfo NSIDCEASEGridGlobal;
        public readonly ProjectionInfo PlateCarreeWorld;
        public readonly ProjectionInfo PolyconicWorld;
        public readonly ProjectionInfo QuarticAuthalicWorld;
        public readonly ProjectionInfo RobinsonWorld;
        public readonly ProjectionInfo SinusoidalWorld;
        public readonly ProjectionInfo StereographicWorld;
        public readonly ProjectionInfo TheWorldfromSpace;
        // public readonly ProjectionInfo TimesWorld;
        public readonly ProjectionInfo TwoPointEquidistantWorld;
        public readonly ProjectionInfo VanderGrintenIWorld;
        // public readonly ProjectionInfo VerticalPerspectiveWorld;
        public readonly ProjectionInfo WGS1984EASEGridGlobal;
        public readonly ProjectionInfo WGS1984PDCMercator;
        public readonly ProjectionInfo WGS1984PlateCarree;
        public readonly ProjectionInfo WGS1984WebMercator;
        public readonly ProjectionInfo WGS1984WebMercatorAuxiliarySphere;
        public readonly ProjectionInfo WGS1984WorldMercator;
        public readonly ProjectionInfo WinkelIIWorld;
        public readonly ProjectionInfo WinkelIWorld;
        public readonly ProjectionInfo WinkelTripelNGSWorld;

        public readonly ProjectionInfo WebMercator;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of World.
        /// </summary>
        public World()
        {
            AitoffWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54043).SetNames("World_Aitoff", "GCS_WGS_1984", "D_WGS_1984"); // missing
            AzimuthalEquidistantWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54032).SetNames("World_Azimuthal_Equidistant", "GCS_WGS_1984", "D_WGS_1984");
            BehrmannWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54017).SetNames("World_Behrmann", "GCS_WGS_1984", "D_WGS_1984");
            // BerghausStarAAG = ProjectionInfo.FromAuthorityCode("ESRI", 102299).SetNames("Berghaus_Star_AAG", "GCS_WGS_1984", "D_WGS_1984"); // projection not found
            BonneWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54024).SetNames("World_Bonne", "GCS_WGS_1984", "D_WGS_1984");
            CassiniWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54028).SetNames("World_Cassini", "GCS_WGS_1984", "D_WGS_1984");
            CrasterParabolicWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54046).SetNames("World_Craster_Parabolic", "GCS_WGS_1984", "D_WGS_1984"); // missing
            // CubeWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54051).SetNames("World_Cube", "GCS_WGS_1984", "D_WGS_1984"); // projection not found
            CylindricalEqualAreaWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54034).SetNames("World_Cylindrical_Equal_Area", "GCS_WGS_1984", "D_WGS_1984"); // missing
            EckertIIIWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54013).SetNames("World_Eckert_III", "GCS_WGS_1984", "D_WGS_1984");
            EckertIIWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54014).SetNames("World_Eckert_II", "GCS_WGS_1984", "D_WGS_1984");
            EckertIVWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54012).SetNames("World_Eckert_IV", "GCS_WGS_1984", "D_WGS_1984");
            EckertIWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54015).SetNames("World_Eckert_I", "GCS_WGS_1984", "D_WGS_1984");
            EckertVIWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54010).SetNames("World_Eckert_VI", "GCS_WGS_1984", "D_WGS_1984");
            EckertVWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54011).SetNames("World_Eckert_V", "GCS_WGS_1984", "D_WGS_1984");
            EquidistantConicWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54027).SetNames("World_Equidistant_Conic", "GCS_WGS_1984", "D_WGS_1984");
            EquidistantCylindricalWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54002).SetNames("World_Equidistant_Cylindrical", "GCS_WGS_1984", "D_WGS_1984");
            // FlatPolarQuarticWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54045).SetNames("World_Flat_Polar_Quartic", "GCS_WGS_1984", "D_WGS_1984"); // projection not found
            // FullerWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54050).SetNames("World_Fuller", "GCS_WGS_1984", "D_WGS_1984"); // projection not found
            GallStereographicWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54016).SetNames("World_Gall_Stereographic", "GCS_WGS_1984", "D_WGS_1984");
            GoodeHomolosineLand = ProjectionInfo.FromAuthorityCode("ESRI", 54052).SetNames("World_Goode_Homolosine_Land", "GCS_WGS_1984", "D_WGS_1984"); // missing
            GoodeHomolosineOcean = ProjectionInfo.FromAuthorityCode("ESRI", 54053).SetNames("World_Goode_Homolosine_Ocean", "GCS_WGS_1984", "D_WGS_1984"); // missing
            HammerAitoffWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54044).SetNames("World_Hammer_Aitoff", "GCS_WGS_1984", "D_WGS_1984"); // missing
            HotineWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54025).SetNames("World_Hotine", "GCS_WGS_1984", "D_WGS_1984");
            LoximuthalWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54023).SetNames("World_Loximuthal", "GCS_WGS_1984", "D_WGS_1984");
            MercatorWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54004).SetNames("World_Mercator", "GCS_WGS_1984", "D_WGS_1984");
            MillerCylindricalWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54003).SetNames("World_Miller_Cylindrical", "GCS_WGS_1984", "D_WGS_1984");
            MollweideWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54009).SetNames("World_Mollweide", "GCS_WGS_1984", "D_WGS_1984");
            NSIDCEASEGridGlobal = ProjectionInfo.FromEpsgCode(3410).SetNames("NSIDC_EASE_Grid_Global", "GCS_Sphere_International_1924_Authalic", "D_Sphere_International_1924_Authalic");
            PlateCarreeWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54001).SetNames("World_Plate_Carree", "GCS_WGS_1984", "D_WGS_1984");
            PolyconicWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54021).SetNames("World_Polyconic", "GCS_WGS_1984", "D_WGS_1984");
            QuarticAuthalicWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54022).SetNames("World_Quartic_Authalic", "GCS_WGS_1984", "D_WGS_1984");
            RobinsonWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54030).SetNames("World_Robinson", "GCS_WGS_1984", "D_WGS_1984");
            SinusoidalWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54008).SetNames("World_Sinusoidal", "GCS_WGS_1984", "D_WGS_1984");
            StereographicWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54026).SetNames("World_Stereographic", "GCS_WGS_1984", "D_WGS_1984");
            TheWorldfromSpace = ProjectionInfo.FromAuthorityCode("ESRI", 102038).SetNames("The_World_From_Space", "GCS_Sphere_ARC_INFO", "D_Sphere_ARC_INFO"); // missing
            // TimesWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54048).SetNames("World_Times", "GCS_WGS_1984", "D_WGS_1984"); // projection not found
            TwoPointEquidistantWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54031).SetNames("World_Two_Point_Equidistant", "GCS_WGS_1984", "D_WGS_1984");
            VanderGrintenIWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54029).SetNames("World_Van_der_Grinten_I", "GCS_WGS_1984", "D_WGS_1984");
            // VerticalPerspectiveWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54049).SetNames("World_Vertical_Perspective", "GCS_WGS_1984", "D_WGS_1984"); // projection not found
            WGS1984EASEGridGlobal = ProjectionInfo.FromEpsgCode(3975).SetNames("WGS_1984_EASE_Grid_Global", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984PDCMercator = ProjectionInfo.FromEpsgCode(3832).SetNames("WGS_1984_PDC_Mercator", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984PlateCarree = ProjectionInfo.FromEpsgCode(32662).SetNames("WGS_1984_Plate_Carree", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984WebMercator = ProjectionInfo.FromAuthorityCode("EPSG", 102113).SetNames("WGS_1984_Web_Mercator", "GCS_WGS_1984_Major_Auxiliary_Sphere", "D_WGS_1984_Major_Auxiliary_Sphere"); // missing
            WGS1984WebMercatorAuxiliarySphere = ProjectionInfo.FromEpsgCode(3857).SetNames("WGS_1984_Web_Mercator_Auxiliary_Sphere", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984WorldMercator = ProjectionInfo.FromEpsgCode(3395).SetNames("WGS_1984_World_Mercator", "GCS_WGS_1984", "D_WGS_1984");
            WinkelIIWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54019).SetNames("World_Winkel_II", "GCS_WGS_1984", "D_WGS_1984");
            WinkelIWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54018).SetNames("World_Winkel_I", "GCS_WGS_1984", "D_WGS_1984");
            WinkelTripelNGSWorld = ProjectionInfo.FromAuthorityCode("ESRI", 54042).SetNames("World_Winkel_Tripel_NGS", "GCS_WGS_1984", "D_WGS_1984"); // missing

            WebMercator = ProjectionInfo.FromProj4String("+proj=merc +a=6378137 +b=6378137 +lat_ts=0.0 +lon_0=0.0 +x_0=0.0 +y_0=0.0 +k=1.0 +units=m +nadgrids=@null +no_defs ");
            WebMercator.Transform = new MercatorAuxiliarySphere();
            WebMercator.ScaleFactor = 1;
            WebMercator.AuxiliarySphereType = AuxiliarySphereType.SemimajorAxis;
            WebMercator.GeographicInfo.Datum.Spheroid = new Spheroid(WebMercator.GeographicInfo.Datum.Spheroid.EquatorialRadius);
            WebMercator.Transform.Init(WebMercator);
        }

        #endregion
    }
}

#pragma warning restore 1591