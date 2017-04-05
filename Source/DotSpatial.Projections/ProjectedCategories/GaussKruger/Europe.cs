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

namespace DotSpatial.Projections.ProjectedCategories.GaussKruger
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for Europe.
    /// </summary>
    public class Europe : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Albanian1987GKZone4;
        public readonly ProjectionInfo ED19503DegreeGKZone10;
        public readonly ProjectionInfo ED19503DegreeGKZone11;
        public readonly ProjectionInfo ED19503DegreeGKZone12;
        public readonly ProjectionInfo ED19503DegreeGKZone13;
        public readonly ProjectionInfo ED19503DegreeGKZone14;
        public readonly ProjectionInfo ED19503DegreeGKZone15;
        public readonly ProjectionInfo ED19503DegreeGKZone9;
        public readonly ProjectionInfo ETRS1989ETRSGK19FIN;
        public readonly ProjectionInfo ETRS1989ETRSGK20FIN;
        public readonly ProjectionInfo ETRS1989ETRSGK21FIN;
        public readonly ProjectionInfo ETRS1989ETRSGK22FIN;
        public readonly ProjectionInfo ETRS1989ETRSGK23FIN;
        public readonly ProjectionInfo ETRS1989ETRSGK24FIN;
        public readonly ProjectionInfo ETRS1989ETRSGK25FIN;
        public readonly ProjectionInfo ETRS1989ETRSGK26FIN;
        public readonly ProjectionInfo ETRS1989ETRSGK27FIN;
        public readonly ProjectionInfo ETRS1989ETRSGK28FIN;
        public readonly ProjectionInfo ETRS1989ETRSGK29FIN;
        public readonly ProjectionInfo ETRS1989ETRSGK30FIN;
        public readonly ProjectionInfo ETRS1989ETRSGK31FIN;
        public readonly ProjectionInfo PD83GKZone3;
        public readonly ProjectionInfo PD83GKZone4;
        public readonly ProjectionInfo Pulkovo1942Adj19583DegreeGKZone10;
        public readonly ProjectionInfo Pulkovo1942Adj19583DegreeGKZone3;
        public readonly ProjectionInfo Pulkovo1942Adj19583DegreeGKZone4;
        public readonly ProjectionInfo Pulkovo1942Adj19583DegreeGKZone5;
        public readonly ProjectionInfo Pulkovo1942Adj19583DegreeGKZone6;
        public readonly ProjectionInfo Pulkovo1942Adj19583DegreeGKZone7;
        public readonly ProjectionInfo Pulkovo1942Adj19583DegreeGKZone8;
        public readonly ProjectionInfo Pulkovo1942Adj19583DegreeGKZone9;
        public readonly ProjectionInfo Pulkovo1942Adj1958GKZone2;
        public readonly ProjectionInfo Pulkovo1942Adj1958GKZone3;
        public readonly ProjectionInfo Pulkovo1942Adj1958GKZone4;
        public readonly ProjectionInfo Pulkovo1942Adj1958GKZone5;
        public readonly ProjectionInfo Pulkovo1942Adj19833DegreeGKZone3;
        public readonly ProjectionInfo Pulkovo1942Adj19833DegreeGKZone4;
        public readonly ProjectionInfo Pulkovo1942Adj19833DegreeGKZone5;
        public readonly ProjectionInfo Pulkovo1942Adj19833DegreeGKZone6;
        public readonly ProjectionInfo Pulkovo1942Adj19833DegreeGKZone7;
        public readonly ProjectionInfo Pulkovo1942Adj19833DegreeGKZone8;
        public readonly ProjectionInfo Pulkovo1942Adj1983GKZone2;
        public readonly ProjectionInfo Pulkovo1942Adj1983GKZone3;
        public readonly ProjectionInfo Pulkovo1942Adj1983GKZone4;
        public readonly ProjectionInfo RD83GKZone4;
        public readonly ProjectionInfo RD83GKZone5;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Europe.
        /// </summary>
        public Europe()
        {
            Albanian1987GKZone4 = ProjectionInfo.FromEpsgCode(2462).SetNames("Albanian_1987_GK_Zone_4", "GCS_Albanian_1987", "D_Albanian_1987");
            ED19503DegreeGKZone10 = ProjectionInfo.FromEpsgCode(2207).SetNames("ED_1950_3_Degree_GK_Zone_10", "GCS_European_1950", "D_European_1950");
            ED19503DegreeGKZone11 = ProjectionInfo.FromEpsgCode(2208).SetNames("ED_1950_3_Degree_GK_Zone_11", "GCS_European_1950", "D_European_1950");
            ED19503DegreeGKZone12 = ProjectionInfo.FromEpsgCode(2209).SetNames("ED_1950_3_Degree_GK_Zone_12", "GCS_European_1950", "D_European_1950");
            ED19503DegreeGKZone13 = ProjectionInfo.FromEpsgCode(2210).SetNames("ED_1950_3_Degree_GK_Zone_13", "GCS_European_1950", "D_European_1950");
            ED19503DegreeGKZone14 = ProjectionInfo.FromEpsgCode(2211).SetNames("ED_1950_3_Degree_GK_Zone_14", "GCS_European_1950", "D_European_1950");
            ED19503DegreeGKZone15 = ProjectionInfo.FromEpsgCode(2212).SetNames("ED_1950_3_Degree_GK_Zone_15", "GCS_European_1950", "D_European_1950");
            ED19503DegreeGKZone9 = ProjectionInfo.FromEpsgCode(2206).SetNames("ED_1950_3_Degree_GK_Zone_9", "GCS_European_1950", "D_European_1950");
            ETRS1989ETRSGK19FIN = ProjectionInfo.FromEpsgCode(3126).SetNames("ETRS_1989_ETRS-GK19FIN", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSGK20FIN = ProjectionInfo.FromEpsgCode(3127).SetNames("ETRS_1989_ETRS-GK20FIN", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSGK21FIN = ProjectionInfo.FromEpsgCode(3128).SetNames("ETRS_1989_ETRS-GK21FIN", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSGK22FIN = ProjectionInfo.FromEpsgCode(3129).SetNames("ETRS_1989_ETRS-GK22FIN", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSGK23FIN = ProjectionInfo.FromEpsgCode(3130).SetNames("ETRS_1989_ETRS-GK23FIN", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSGK24FIN = ProjectionInfo.FromEpsgCode(3131).SetNames("ETRS_1989_ETRS-GK24FIN", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSGK25FIN = ProjectionInfo.FromEpsgCode(3132).SetNames("ETRS_1989_ETRS-GK25FIN", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSGK26FIN = ProjectionInfo.FromEpsgCode(3133).SetNames("ETRS_1989_ETRS-GK26FIN", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSGK27FIN = ProjectionInfo.FromEpsgCode(3134).SetNames("ETRS_1989_ETRS-GK27FIN", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSGK28FIN = ProjectionInfo.FromEpsgCode(3135).SetNames("ETRS_1989_ETRS-GK28FIN", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSGK29FIN = ProjectionInfo.FromEpsgCode(3136).SetNames("ETRS_1989_ETRS-GK29FIN", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSGK30FIN = ProjectionInfo.FromEpsgCode(3137).SetNames("ETRS_1989_ETRS-GK30FIN", "GCS_ETRS_1989", "D_ETRS_1989");
            ETRS1989ETRSGK31FIN = ProjectionInfo.FromEpsgCode(3138).SetNames("ETRS_1989_ETRS-GK31FIN", "GCS_ETRS_1989", "D_ETRS_1989");
            PD83GKZone3 = ProjectionInfo.FromEpsgCode(3396).SetNames("PD/83_GK_Zone_3", "GCS_PD/83", "D_Potsdam_1983");
            PD83GKZone4 = ProjectionInfo.FromEpsgCode(3397).SetNames("PD/83_GK_Zone_4", "GCS_PD/83", "D_Potsdam_1983");
            Pulkovo1942Adj19583DegreeGKZone10 = ProjectionInfo.FromEpsgCode(3840).SetNames("Pulkovo_1942_Adj_1958_3_Degree_GK_Zone_10", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            Pulkovo1942Adj19583DegreeGKZone3 = ProjectionInfo.FromEpsgCode(3837).SetNames("Pulkovo_1942_Adj_1958_3_Degree_GK_Zone_3", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            Pulkovo1942Adj19583DegreeGKZone4 = ProjectionInfo.FromEpsgCode(3838).SetNames("Pulkovo_1942_Adj_1958_3_Degree_GK_Zone_4", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            Pulkovo1942Adj19583DegreeGKZone5 = ProjectionInfo.FromEpsgCode(3329).SetNames("Pulkovo_1942_Adj_1958_3_Degree_GK_Zone_5", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            Pulkovo1942Adj19583DegreeGKZone6 = ProjectionInfo.FromEpsgCode(3330).SetNames("Pulkovo_1942_Adj_1958_3_Degree_GK_Zone_6", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            Pulkovo1942Adj19583DegreeGKZone7 = ProjectionInfo.FromEpsgCode(3331).SetNames("Pulkovo_1942_Adj_1958_3_Degree_GK_Zone_7", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            Pulkovo1942Adj19583DegreeGKZone8 = ProjectionInfo.FromEpsgCode(3332).SetNames("Pulkovo_1942_Adj_1958_3_Degree_GK_Zone_8", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            Pulkovo1942Adj19583DegreeGKZone9 = ProjectionInfo.FromEpsgCode(3839).SetNames("Pulkovo_1942_Adj_1958_3_Degree_GK_Zone_9", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            Pulkovo1942Adj1958GKZone2 = ProjectionInfo.FromEpsgCode(3833).SetNames("Pulkovo_1942_Adj_1958_GK_Zone_2", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            Pulkovo1942Adj1958GKZone3 = ProjectionInfo.FromEpsgCode(3333).SetNames("Pulkovo_1942_Adj_1958_GK_Zone_3", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            Pulkovo1942Adj1958GKZone4 = ProjectionInfo.FromEpsgCode(3334).SetNames("Pulkovo_1942_Adj_1958_GK_Zone_4", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            Pulkovo1942Adj1958GKZone5 = ProjectionInfo.FromEpsgCode(3335).SetNames("Pulkovo_1942_Adj_1958_GK_Zone_5", "GCS_Pulkovo_1942_Adj_1958", "D_Pulkovo_1942_Adj_1958");
            Pulkovo1942Adj19833DegreeGKZone3 = ProjectionInfo.FromEpsgCode(2397).SetNames("Pulkovo_1942_Adj_1983_3_Degree_GK_Zone_3", "GCS_Pulkovo_1942_Adj_1983", "D_Pulkovo_1942_Adj_1983");
            Pulkovo1942Adj19833DegreeGKZone4 = ProjectionInfo.FromEpsgCode(2398).SetNames("Pulkovo_1942_Adj_1983_3_Degree_GK_Zone_4", "GCS_Pulkovo_1942_Adj_1983", "D_Pulkovo_1942_Adj_1983");
            Pulkovo1942Adj19833DegreeGKZone5 = ProjectionInfo.FromEpsgCode(2399).SetNames("Pulkovo_1942_Adj_1983_3_Degree_GK_Zone_5", "GCS_Pulkovo_1942_Adj_1983", "D_Pulkovo_1942_Adj_1983");
            Pulkovo1942Adj19833DegreeGKZone6 = ProjectionInfo.FromEpsgCode(3841).SetNames("Pulkovo_1942_Adj_1983_3_Degree_GK_Zone_6", "GCS_Pulkovo_1942_Adj_1983", "D_Pulkovo_1942_Adj_1983");
            Pulkovo1942Adj19833DegreeGKZone7 = ProjectionInfo.FromAuthorityCode("EPSG", 102764).SetNames("Pulkovo_1942_Adj_1983_3_Degree_GK_Zone_7", "GCS_Pulkovo_1942_Adj_1983", "D_Pulkovo_1942_Adj_1983"); // missing
            Pulkovo1942Adj19833DegreeGKZone8 = ProjectionInfo.FromAuthorityCode("EPSG", 102765).SetNames("Pulkovo_1942_Adj_1983_3_Degree_GK_Zone_8", "GCS_Pulkovo_1942_Adj_1983", "D_Pulkovo_1942_Adj_1983"); // missing
            Pulkovo1942Adj1983GKZone2 = ProjectionInfo.FromEpsgCode(3834).SetNames("Pulkovo_1942_Adj_1983_GK_Zone_2", "GCS_Pulkovo_1942_Adj_1983", "D_Pulkovo_1942_Adj_1983");
            Pulkovo1942Adj1983GKZone3 = ProjectionInfo.FromEpsgCode(3835).SetNames("Pulkovo_1942_Adj_1983_GK_Zone_3", "GCS_Pulkovo_1942_Adj_1983", "D_Pulkovo_1942_Adj_1983");
            Pulkovo1942Adj1983GKZone4 = ProjectionInfo.FromEpsgCode(3836).SetNames("Pulkovo_1942_Adj_1983_GK_Zone_4", "GCS_Pulkovo_1942_Adj_1983", "D_Pulkovo_1942_Adj_1983");
            RD83GKZone4 = ProjectionInfo.FromEpsgCode(3398).SetNames("RD/83_GK_Zone_4", "GCS_RD/83", "D_Rauenberg_1983");
            RD83GKZone5 = ProjectionInfo.FromEpsgCode(3399).SetNames("RD/83_GK_Zone_5", "GCS_RD/83", "D_Rauenberg_1983");
        }

        #endregion
    }
}

#pragma warning restore 1591