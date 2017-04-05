// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:23:00 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.ProjectedCategories.NationalGrids
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for Argentina.
    /// </summary>
    public class Argentina : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo ArgentinaZone1;
        public readonly ProjectionInfo ArgentinaZone2;
        public readonly ProjectionInfo ArgentinaZone3;
        public readonly ProjectionInfo ArgentinaZone4;
        public readonly ProjectionInfo ArgentinaZone5;
        public readonly ProjectionInfo ArgentinaZone6;
        public readonly ProjectionInfo ArgentinaZone7;
        public readonly ProjectionInfo ChosMalal1914Argentina2;
        public readonly ProjectionInfo HitoXVIII1963Argentina2;
        public readonly ProjectionInfo PampadelCastilloArgentina2;
        public readonly ProjectionInfo POSGAR1994ArgentinaZone1;
        public readonly ProjectionInfo POSGAR1994ArgentinaZone2;
        public readonly ProjectionInfo POSGAR1994ArgentinaZone3;
        public readonly ProjectionInfo POSGAR1994ArgentinaZone4;
        public readonly ProjectionInfo POSGAR1994ArgentinaZone5;
        public readonly ProjectionInfo POSGAR1994ArgentinaZone6;
        public readonly ProjectionInfo POSGAR1994ArgentinaZone7;
        public readonly ProjectionInfo POSGAR1998ArgentinaZone1;
        public readonly ProjectionInfo POSGAR1998ArgentinaZone2;
        public readonly ProjectionInfo POSGAR1998ArgentinaZone3;
        public readonly ProjectionInfo POSGAR1998ArgentinaZone4;
        public readonly ProjectionInfo POSGAR1998ArgentinaZone5;
        public readonly ProjectionInfo POSGAR1998ArgentinaZone6;
        public readonly ProjectionInfo POSGAR1998ArgentinaZone7;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Argentina.
        /// </summary>
        public Argentina()
        {
            ArgentinaZone1 = ProjectionInfo.FromEpsgCode(22191).SetNames("Argentina_Zone_1", "GCS_Campo_Inchauspe", "D_Campo_Inchauspe");
            ArgentinaZone2 = ProjectionInfo.FromEpsgCode(22192).SetNames("Argentina_Zone_2", "GCS_Campo_Inchauspe", "D_Campo_Inchauspe");
            ArgentinaZone3 = ProjectionInfo.FromEpsgCode(22193).SetNames("Argentina_Zone_3", "GCS_Campo_Inchauspe", "D_Campo_Inchauspe");
            ArgentinaZone4 = ProjectionInfo.FromEpsgCode(22194).SetNames("Argentina_Zone_4", "GCS_Campo_Inchauspe", "D_Campo_Inchauspe");
            ArgentinaZone5 = ProjectionInfo.FromEpsgCode(22195).SetNames("Argentina_Zone_5", "GCS_Campo_Inchauspe", "D_Campo_Inchauspe");
            ArgentinaZone6 = ProjectionInfo.FromEpsgCode(22196).SetNames("Argentina_Zone_6", "GCS_Campo_Inchauspe", "D_Campo_Inchauspe");
            ArgentinaZone7 = ProjectionInfo.FromEpsgCode(22197).SetNames("Argentina_Zone_7", "GCS_Campo_Inchauspe", "D_Campo_Inchauspe");
            ChosMalal1914Argentina2 = ProjectionInfo.FromEpsgCode(2081).SetNames("Chos_Malal_1914_Argentina_2", "GCS_Chos_Malal_1914", "D_Chos_Malal_1914");
            HitoXVIII1963Argentina2 = ProjectionInfo.FromEpsgCode(2083).SetNames("Hito_XVIII_1963_Argentina_2", "GCS_Hito_XVIII_1963", "D_Hito_XVIII_1963");
            PampadelCastilloArgentina2 = ProjectionInfo.FromEpsgCode(2082).SetNames("Pampa_del_Castillo_Argentina_2", "GCS_Pampa_del_Castillo", "D_Pampa_del_Castillo");
            POSGAR1994ArgentinaZone1 = ProjectionInfo.FromEpsgCode(22181).SetNames("POSGAR_1994_Argentina_Zone_1", "GCS_POSGAR_1994", "D_POSGAR_1994");
            POSGAR1994ArgentinaZone2 = ProjectionInfo.FromEpsgCode(22182).SetNames("POSGAR_1994_Argentina_Zone_2", "GCS_POSGAR_1994", "D_POSGAR_1994");
            POSGAR1994ArgentinaZone3 = ProjectionInfo.FromEpsgCode(22183).SetNames("POSGAR_1994_Argentina_Zone_3", "GCS_POSGAR_1994", "D_POSGAR_1994");
            POSGAR1994ArgentinaZone4 = ProjectionInfo.FromEpsgCode(22184).SetNames("POSGAR_1994_Argentina_Zone_4", "GCS_POSGAR_1994", "D_POSGAR_1994");
            POSGAR1994ArgentinaZone5 = ProjectionInfo.FromEpsgCode(22185).SetNames("POSGAR_1994_Argentina_Zone_5", "GCS_POSGAR_1994", "D_POSGAR_1994");
            POSGAR1994ArgentinaZone6 = ProjectionInfo.FromEpsgCode(22186).SetNames("POSGAR_1994_Argentina_Zone_6", "GCS_POSGAR_1994", "D_POSGAR_1994");
            POSGAR1994ArgentinaZone7 = ProjectionInfo.FromEpsgCode(22187).SetNames("POSGAR_1994_Argentina_Zone_7", "GCS_POSGAR_1994", "D_POSGAR_1994");
            POSGAR1998ArgentinaZone1 = ProjectionInfo.FromEpsgCode(22171).SetNames("POSGAR_1998_Argentina_Zone_1", "GCS_POSGAR_1998", "D_POSGAR_1998");
            POSGAR1998ArgentinaZone2 = ProjectionInfo.FromEpsgCode(22172).SetNames("POSGAR_1998_Argentina_Zone_2", "GCS_POSGAR_1998", "D_POSGAR_1998");
            POSGAR1998ArgentinaZone3 = ProjectionInfo.FromEpsgCode(22173).SetNames("POSGAR_1998_Argentina_Zone_3", "GCS_POSGAR_1998", "D_POSGAR_1998");
            POSGAR1998ArgentinaZone4 = ProjectionInfo.FromEpsgCode(22174).SetNames("POSGAR_1998_Argentina_Zone_4", "GCS_POSGAR_1998", "D_POSGAR_1998");
            POSGAR1998ArgentinaZone5 = ProjectionInfo.FromEpsgCode(22175).SetNames("POSGAR_1998_Argentina_Zone_5", "GCS_POSGAR_1998", "D_POSGAR_1998");
            POSGAR1998ArgentinaZone6 = ProjectionInfo.FromEpsgCode(22176).SetNames("POSGAR_1998_Argentina_Zone_6", "GCS_POSGAR_1998", "D_POSGAR_1998");
            POSGAR1998ArgentinaZone7 = ProjectionInfo.FromEpsgCode(22177).SetNames("POSGAR_1998_Argentina_Zone_7", "GCS_POSGAR_1998", "D_POSGAR_1998");
        }

        #endregion
    }
}

#pragma warning restore 1591