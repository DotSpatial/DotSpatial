// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 5:04:41 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.ProjectedCategories.StatePlane
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for OtherGCS.
    /// </summary>
    public class OtherGCS : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AmericanSamoa1962StatePlaneAmerSamoaFIPS5300USFeet;
        public readonly ProjectionInfo GuamGeodeticNetwork1993Meters;
        public readonly ProjectionInfo GuamGeodeticTriangulationNetwork1963Meters;
        public readonly ProjectionInfo NAD1983HARNGuamMapGridMeters;
        public readonly ProjectionInfo NADMichiganStatePlaneMichiganCentOldFIPS2102USFeet;
        public readonly ProjectionInfo NADMichiganStatePlaneMichiganCentralFIPS2112USFeet;
        public readonly ProjectionInfo NADMichiganStatePlaneMichiganEastOldFIPS2101USFeet;
        public readonly ProjectionInfo NADMichiganStatePlaneMichiganNorthFIPS2111USFeet;
        public readonly ProjectionInfo NADMichiganStatePlaneMichiganSouthFIPS2113USFeet;
        public readonly ProjectionInfo NADMichiganStatePlaneMichiganWestOldFIPS2103USFeet;
        public readonly ProjectionInfo OldHawaiianStatePlaneHawaii1FIPS5101USFeet;
        public readonly ProjectionInfo OldHawaiianStatePlaneHawaii2FIPS5102USFeet;
        public readonly ProjectionInfo OldHawaiianStatePlaneHawaii3FIPS5103USFeet;
        public readonly ProjectionInfo OldHawaiianStatePlaneHawaii4FIPS5104USFeet;
        public readonly ProjectionInfo OldHawaiianStatePlaneHawaii5FIPS5105USFeet;
        public readonly ProjectionInfo PuertoRicoStatePlanePuertoRicoFIPS5201USFeet;
        public readonly ProjectionInfo PuertoRicoStatePlaneVirginIslStCroixFIPS5202USFeet;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of OtherGCS.
        /// </summary>
        public OtherGCS()
        {
            AmericanSamoa1962StatePlaneAmerSamoaFIPS5300USFeet = ProjectionInfo.FromAuthorityCode("ESRI", 65062).SetNames("American_Samoa_1962_StatePlane_American_Samoa_FIPS_5300", "GCS_American_Samoa_1962", "D_American_Samoa_1962"); // missing
            GuamGeodeticNetwork1993Meters = ProjectionInfo.FromAuthorityCode("ESRI", 102240).SetNames("Guam_Geodetic_Network_1993", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            GuamGeodeticTriangulationNetwork1963Meters = ProjectionInfo.FromAuthorityCode("ESRI", 102239).SetNames("Guam_Geodetic_Triangulation_Network_1963", "GCS_Guam_1963", "D_Guam_1963"); // missing
            NAD1983HARNGuamMapGridMeters = ProjectionInfo.FromAuthorityCode("EPSG", 102201).SetNames("NAD_1983_HARN_Guam_Map_Grid", "GCS_North_American_1983_HARN", "D_North_American_1983_HARN"); // missing
            NADMichiganStatePlaneMichiganCentOldFIPS2102USFeet = ProjectionInfo.FromEpsgCode(26802).SetNames("NAD_Michigan_StatePlane_Michigan_Central_Old_FIPS_2102", "GCS_North_American_Michigan", "D_North_American_Michigan");
            NADMichiganStatePlaneMichiganCentralFIPS2112USFeet = ProjectionInfo.FromEpsgCode(26812).SetNames("NAD_Michigan_StatePlane_Michigan_Central_FIPS_2112", "GCS_North_American_Michigan", "D_North_American_Michigan");
            NADMichiganStatePlaneMichiganEastOldFIPS2101USFeet = ProjectionInfo.FromEpsgCode(26801).SetNames("NAD_Michigan_StatePlane_Michigan_East_Old_FIPS_2101", "GCS_North_American_Michigan", "D_North_American_Michigan");
            NADMichiganStatePlaneMichiganNorthFIPS2111USFeet = ProjectionInfo.FromEpsgCode(26811).SetNames("NAD_Michigan_StatePlane_Michigan_North_FIPS_2111", "GCS_North_American_Michigan", "D_North_American_Michigan");
            NADMichiganStatePlaneMichiganSouthFIPS2113USFeet = ProjectionInfo.FromEpsgCode(26813).SetNames("NAD_Michigan_StatePlane_Michigan_South_FIPS_2113", "GCS_North_American_Michigan", "D_North_American_Michigan");
            NADMichiganStatePlaneMichiganWestOldFIPS2103USFeet = ProjectionInfo.FromEpsgCode(26803).SetNames("NAD_Michigan_StatePlane_Michigan_West_Old_FIPS_2103", "GCS_North_American_Michigan", "D_North_American_Michigan");
            OldHawaiianStatePlaneHawaii1FIPS5101USFeet = ProjectionInfo.FromEpsgCode(3561).SetNames("Old_Hawaiian_StatePlane_Hawaii_1_FIPS_5101", "GCS_Old_Hawaiian", "D_Old_Hawaiian");
            OldHawaiianStatePlaneHawaii2FIPS5102USFeet = ProjectionInfo.FromEpsgCode(3562).SetNames("Old_Hawaiian_StatePlane_Hawaii_2_FIPS_5102", "GCS_Old_Hawaiian", "D_Old_Hawaiian");
            OldHawaiianStatePlaneHawaii3FIPS5103USFeet = ProjectionInfo.FromEpsgCode(3563).SetNames("Old_Hawaiian_StatePlane_Hawaii_3_FIPS_5103", "GCS_Old_Hawaiian", "D_Old_Hawaiian");
            OldHawaiianStatePlaneHawaii4FIPS5104USFeet = ProjectionInfo.FromEpsgCode(3564).SetNames("Old_Hawaiian_StatePlane_Hawaii_4_FIPS_5104", "GCS_Old_Hawaiian", "D_Old_Hawaiian");
            OldHawaiianStatePlaneHawaii5FIPS5105USFeet = ProjectionInfo.FromEpsgCode(3565).SetNames("Old_Hawaiian_StatePlane_Hawaii_5_FIPS_5105", "GCS_Old_Hawaiian", "D_Old_Hawaiian");
            PuertoRicoStatePlanePuertoRicoFIPS5201USFeet = ProjectionInfo.FromEpsgCode(3991).SetNames("Puerto_Rico_StatePlane_Puerto_Rico_FIPS_5201", "GCS_Puerto_Rico", "D_Puerto_Rico");
            PuertoRicoStatePlaneVirginIslStCroixFIPS5202USFeet = ProjectionInfo.FromEpsgCode(3992).SetNames("Puerto_Rico_StatePlane_Virgin_Islands_St_Croix_FIPS_5202", "GCS_Puerto_Rico", "D_Puerto_Rico");
        }

        #endregion
    }
}

#pragma warning restore 1591