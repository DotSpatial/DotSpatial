// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:05:31 PM
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
    /// This class contains predefined CoordinateSystems for StateSystems.
    /// </summary>
    public class StateSystems : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo NAD1927AlaskaAlbersMeters;
        public readonly ProjectionInfo NAD1927AlaskaAlbersUSFeet;
        public readonly ProjectionInfo NAD1927CaliforniaTealeAlbersMeters;
        public readonly ProjectionInfo NAD1927GeorgiaStatewideAlbersUSFeet;
        public readonly ProjectionInfo NAD1927MichiganGeoRefMeters;
        public readonly ProjectionInfo NAD1927MichiganGeoRefUSfeet;
        public readonly ProjectionInfo NAD1927TexasStatewideMappingSystemIntlFeet;
        public readonly ProjectionInfo NAD1927WisconsinTMMeters;
        public readonly ProjectionInfo NAD1983CaliforniaTealeAlbersMeters;
        public readonly ProjectionInfo NAD1983CORS96AlaskaAlbersMeters;
        public readonly ProjectionInfo NAD1983CORS96OregonStatewideLambertIntlFeet;
        public readonly ProjectionInfo NAD1983CORS96OregonStatewideLambertMeters;
        public readonly ProjectionInfo NAD1983FloridaGDLAlbersMeters;
        public readonly ProjectionInfo NAD1983GeorgiaStatewideLambertUSFeet;
        public readonly ProjectionInfo NAD1983HARNCaliforniaTealeAlbersMeters;
        public readonly ProjectionInfo NAD1983HARNFloridaGDLAlbersMeters;
        public readonly ProjectionInfo NAD1983HARNMichiganGeoRefMeters;
        public readonly ProjectionInfo NAD1983HARNMississippiTMMeters;
        public readonly ProjectionInfo NAD1983HARNOregonStatewideLambertIntlFeet;
        public readonly ProjectionInfo NAD1983HARNOregonStatewideLambertMeters;
        public readonly ProjectionInfo NAD1983HARNTexasCentricMappingSystemAlbersMeters;
        public readonly ProjectionInfo NAD1983HARNTexasCentricMappingSystemLambertMeters;
        public readonly ProjectionInfo NAD1983HARNVirginiaLambertMeters;
        public readonly ProjectionInfo NAD1983HARNWisconsinTMMeters;
        public readonly ProjectionInfo NAD1983HARNWisconsinTMUSFeet;
        public readonly ProjectionInfo NAD1983IdahoTMMeters;
        public readonly ProjectionInfo NAD1983MichiganGeoRefMeters;
        public readonly ProjectionInfo NAD1983MichiganGeoRefUSfeet;
        public readonly ProjectionInfo NAD1983MississippiTMMeters;
        public readonly ProjectionInfo NAD1983NSRS2007AlaskaAlbersMeters;
        public readonly ProjectionInfo NAD1983NSRS2007FloridaGDLAlbersMeters;
        public readonly ProjectionInfo NAD1983NSRS2007MichiganGeoRefMeters;
        public readonly ProjectionInfo NAD1983NSRS2007MississippiTMMeters;
        public readonly ProjectionInfo NAD1983NSRS2007OregonStatewideLambertIntlFeet;
        public readonly ProjectionInfo NAD1983NSRS2007OregonStatewideLambertMeters;
        public readonly ProjectionInfo NAD1983NSRS2007TexasCentricMappingSystemAlbersMeters;
        public readonly ProjectionInfo NAD1983NSRS2007TexasCentricMappingSystemLambertMeters;
        public readonly ProjectionInfo NAD1983NSRS2007VirginiaLambertMeters;
        public readonly ProjectionInfo NAD1983NSRS2007WisconsinTMMeters;
        public readonly ProjectionInfo NAD1983NSRS2007WisconsinTMUSFeet;
        public readonly ProjectionInfo NAD1983OregonStatewideLambertIntlFeet;
        public readonly ProjectionInfo NAD1983OregonStatewideLambertMeters;
        public readonly ProjectionInfo NAD1983TexasCentricMappingSystemAlbersMeters;
        public readonly ProjectionInfo NAD1983TexasCentricMappingSystemLambertMeters;
        public readonly ProjectionInfo NAD1983TexasStatewideMappingSystemMeters;
        public readonly ProjectionInfo NAD1983USFSR6AlbersMeters;
        public readonly ProjectionInfo NAD1983VirginiaLambertMeters;
        public readonly ProjectionInfo NAD1983WisconsinTMMeters;
        public readonly ProjectionInfo NAD1983WisconsinTMUSFeet;
        public readonly ProjectionInfo NAD1983WyLamMeters;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of StateSystems.
        /// </summary>
        public StateSystems()
        {
            NAD1927AlaskaAlbersMeters = ProjectionInfo.FromAuthorityCode("ESRI", 102117).SetNames("NAD_1927_Alaska_Albers_Meters", "GCS_North_American_1927", "D_North_American_1927"); // missing
            NAD1927AlaskaAlbersUSFeet = ProjectionInfo.FromEpsgCode(2964).SetNames("NAD_1927_Alaska_Albers_Feet", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927CaliforniaTealeAlbersMeters = ProjectionInfo.FromEpsgCode(3309).SetNames("NAD_1927_California_Teale_Albers", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927GeorgiaStatewideAlbersUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102118).SetNames("NAD_1927_Georgia_Statewide_Albers", "GCS_North_American_1927", "D_North_American_1927"); // missing
            NAD1927MichiganGeoRefMeters = ProjectionInfo.FromAuthorityCode("ESRI", 102122).SetNames("NAD_1927_Michigan_GeoRef_Meters", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927MichiganGeoRefUSfeet = ProjectionInfo.FromAuthorityCode("ESRI", 102120).SetNames("NAD_1927_Michigan_GeoRef_Feet_US", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927TexasStatewideMappingSystemIntlFeet = ProjectionInfo.FromEpsgCode(3080).SetNames("NAD_1927_Texas_Statewide_Mapping_System", "GCS_North_American_1927", "D_North_American_1927");
            NAD1927WisconsinTMMeters = ProjectionInfo.FromEpsgCode(3069).SetNames("NAD_1927_Wisconsin_TM", "GCS_North_American_1927", "D_North_American_1927");
            NAD1983CaliforniaTealeAlbersMeters = ProjectionInfo.FromEpsgCode(3310).SetNames("NAD_1983_California_Teale_Albers", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983CORS96AlaskaAlbersMeters = ProjectionInfo.FromAuthorityCode("ESRI", 102247).SetNames("NAD_1983_CORS96_Alaska_Albers", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96OregonStatewideLambertIntlFeet = ProjectionInfo.FromAuthorityCode("EPSG", 102381).SetNames("NAD_1983_CORS96_Oregon_Statewide_Lambert_Ft_Intl", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983CORS96OregonStatewideLambertMeters = ProjectionInfo.FromAuthorityCode("EPSG", 102380).SetNames("NAD_1983_CORS96_Oregon_Statewide_Lambert", "GCS_NAD_1983_CORS96", "D_NAD_1983_CORS96"); // missing
            NAD1983FloridaGDLAlbersMeters = ProjectionInfo.FromEpsgCode(3086).SetNames("NAD_1983_Florida_GDL_Albers", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983GeorgiaStatewideLambertUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102604).SetNames("NAD_1983_Georgia_Statewide_Lambert", "GCS_North_American_1983", "D_North_American_1983"); // missing
            NAD1983HARNCaliforniaTealeAlbersMeters = ProjectionInfo.FromEpsgCode(3311).SetNames("NAD_1983_HARN_California_Teale_Albers", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNFloridaGDLAlbersMeters = ProjectionInfo.FromEpsgCode(3087).SetNames("NAD_1983_HARN_Florida_GDL_Albers", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNMichiganGeoRefMeters = ProjectionInfo.FromEpsgCode(3079).SetNames("NAD_1983_HARN_Michigan_GeoRef_Meters", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNMississippiTMMeters = ProjectionInfo.FromEpsgCode(3815).SetNames("NAD_1983_HARN_Mississippi_TM", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNOregonStatewideLambertIntlFeet = ProjectionInfo.FromEpsgCode(2994).SetNames("NAD_1983_HARN_Oregon_Statewide_Lambert_Feet_Intl", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNOregonStatewideLambertMeters = ProjectionInfo.FromEpsgCode(2993).SetNames("NAD_1983_HARN_Oregon_Statewide_Lambert", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNTexasCentricMappingSystemAlbersMeters = ProjectionInfo.FromEpsgCode(3085).SetNames("NAD_1983_HARN_Texas_Centric_Mapping_System_Albers", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNTexasCentricMappingSystemLambertMeters = ProjectionInfo.FromEpsgCode(3084).SetNames("NAD_1983_HARN_Texas_Centric_Mapping_System_Lambert", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNVirginiaLambertMeters = ProjectionInfo.FromEpsgCode(3969).SetNames("NAD_1983_HARN_Virginia_Lambert", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNWisconsinTMMeters = ProjectionInfo.FromEpsgCode(3071).SetNames("NAD_1983_HARN_Wisconsin_TM", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN");
            NAD1983HARNWisconsinTMUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102220).SetNames("NAD_1983_HARN_Wisconsin_TM_US_Ft", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NAD1983IdahoTMMeters = ProjectionInfo.FromAuthorityCode("ESRI", 102605).SetNames("NAD_1983_Idaho_TM", "GCS_North_American_1983", "D_North_American_1983"); // missing
            NAD1983MichiganGeoRefMeters = ProjectionInfo.FromEpsgCode(3078).SetNames("NAD_1983_Michigan_GeoRef_Meters", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MichiganGeoRefUSfeet = ProjectionInfo.FromAuthorityCode("ESRI", 102121).SetNames("NAD_1983_Michigan_GeoRef_Feet_US", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983MississippiTMMeters = ProjectionInfo.FromEpsgCode(3814).SetNames("NAD_1983_Mississippi_TM", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983NSRS2007AlaskaAlbersMeters = ProjectionInfo.FromEpsgCode(3467).SetNames("NAD_1983_NSRS2007_Alaska_Albers", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007FloridaGDLAlbersMeters = ProjectionInfo.FromEpsgCode(3513).SetNames("NAD_1983_NSRS2007_Florida_GDL_Albers", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007MichiganGeoRefMeters = ProjectionInfo.FromEpsgCode(3591).SetNames("NAD_1983_NSRS2007_Michigan_GeoRef_Meters", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007MississippiTMMeters = ProjectionInfo.FromEpsgCode(3816).SetNames("NAD_1983_NSRS2007_Mississippi_TM", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007OregonStatewideLambertIntlFeet = ProjectionInfo.FromEpsgCode(3644).SetNames("NAD_1983_NSRS2007_Oregon_Statewide_Lambert_Ft_Intl", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007OregonStatewideLambertMeters = ProjectionInfo.FromEpsgCode(3643).SetNames("NAD_1983_NSRS2007_Oregon_Statewide_Lambert", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007TexasCentricMappingSystemAlbersMeters = ProjectionInfo.FromEpsgCode(3665).SetNames("NAD_1983_NSRS2007_Texas_Centric_Mapping_System_Albers", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007TexasCentricMappingSystemLambertMeters = ProjectionInfo.FromEpsgCode(3666).SetNames("NAD_1983_NSRS2007_Texas_Centric_Mapping_System_Lambert", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007VirginiaLambertMeters = ProjectionInfo.FromEpsgCode(3970).SetNames("NAD_1983_NSRS2007_Virginia_Lambert", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007WisconsinTMMeters = ProjectionInfo.FromEpsgCode(3701).SetNames("NAD_1983_NSRS2007_Wisconsin_TM", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007");
            NAD1983NSRS2007WisconsinTMUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102217).SetNames("NAD_1983_NSRS2007_Wisconsin_TM_US_Ft", "GCS_NAD_1983_NSRS2007", "D_NAD_1983_NSRS2007"); // missing
            NAD1983OregonStatewideLambertIntlFeet = ProjectionInfo.FromEpsgCode(2992).SetNames("NAD_1983_Oregon_Statewide_Lambert_Feet_Intl", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983OregonStatewideLambertMeters = ProjectionInfo.FromEpsgCode(2991).SetNames("NAD_1983_Oregon_Statewide_Lambert", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983TexasCentricMappingSystemAlbersMeters = ProjectionInfo.FromEpsgCode(3083).SetNames("NAD_1983_Texas_Centric_Mapping_System_Albers", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983TexasCentricMappingSystemLambertMeters = ProjectionInfo.FromEpsgCode(3082).SetNames("NAD_1983_Texas_Centric_Mapping_System_Lambert", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983TexasStatewideMappingSystemMeters = ProjectionInfo.FromEpsgCode(3081).SetNames("NAD_1983_Texas_Statewide_Mapping_System", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983USFSR6AlbersMeters = ProjectionInfo.FromAuthorityCode("ESRI", 102218).SetNames("NAD_1983_USFS_R6_Albers", "GCS_North_American_1983", "D_North_American_1983"); // missing
            NAD1983VirginiaLambertMeters = ProjectionInfo.FromEpsgCode(3968).SetNames("NAD_1983_Virginia_Lambert", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983WisconsinTMMeters = ProjectionInfo.FromEpsgCode(3070).SetNames("NAD_1983_Wisconsin_TM", "GCS_North_American_1983", "D_North_American_1983");
            NAD1983WisconsinTMUSFeet = ProjectionInfo.FromAuthorityCode("ESRI", 102219).SetNames("NAD_1983_Wisconsin_TM_US_Ft", "GCS_North_American_1983", "D_North_American_1983"); // missing
            NAD1983WyLamMeters = ProjectionInfo.FromAuthorityCode("ESRI", 102212).SetNames("NAD_1983_WyLAM", "GCS_North_American_1983", "D_North_American_1983"); // missing
        }

        #endregion
    }
}

#pragma warning restore 1591