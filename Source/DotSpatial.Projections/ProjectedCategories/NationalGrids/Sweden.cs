// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:54:34 PM
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
    /// This class contains predefined CoordinateSystems for Sweden.
    /// </summary>
    public class Sweden : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo RT380gon;
        public readonly ProjectionInfo RT3825gonO;
        public readonly ProjectionInfo RT3825gonV;
        public readonly ProjectionInfo RT385gonO;
        public readonly ProjectionInfo RT385gonV;
        public readonly ProjectionInfo RT3875gonV;
        public readonly ProjectionInfo RT900gon;
        public readonly ProjectionInfo RT9025gonO;
        public readonly ProjectionInfo RT9025gonV;
        public readonly ProjectionInfo RT905gonO;
        public readonly ProjectionInfo RT905gonV;
        public readonly ProjectionInfo RT9075gonV;
        public readonly ProjectionInfo SWEREF991200;
        public readonly ProjectionInfo SWEREF991330;
        public readonly ProjectionInfo SWEREF991415;
        public readonly ProjectionInfo SWEREF991500;
        public readonly ProjectionInfo SWEREF991545;
        public readonly ProjectionInfo SWEREF991630;
        public readonly ProjectionInfo SWEREF991715;
        public readonly ProjectionInfo SWEREF991800;
        public readonly ProjectionInfo SWEREF991845;
        public readonly ProjectionInfo SWEREF992015;
        public readonly ProjectionInfo SWEREF992145;
        public readonly ProjectionInfo SWEREF992315;
        public readonly ProjectionInfo SWEREF99TM;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Sweden.
        /// </summary>
        public Sweden()
        {
            RT380gon = ProjectionInfo.FromEpsgCode(3028).SetNames("RT38_0_gon", "GCS_RT38", "D_Stockholm_1938");
            RT3825gonO = ProjectionInfo.FromEpsgCode(3029).SetNames("RT38_25_gon_O", "GCS_RT38", "D_Stockholm_1938");
            RT3825gonV = ProjectionInfo.FromEpsgCode(3027).SetNames("RT38_25_gon_V", "GCS_RT38", "D_Stockholm_1938");
            RT385gonO = ProjectionInfo.FromEpsgCode(3030).SetNames("RT38_5_gon_O", "GCS_RT38", "D_Stockholm_1938");
            RT385gonV = ProjectionInfo.FromEpsgCode(3026).SetNames("RT38_5_gon_V", "GCS_RT38", "D_Stockholm_1938");
            RT3875gonV = ProjectionInfo.FromEpsgCode(3025).SetNames("RT38_75_gon_V", "GCS_RT38", "D_Stockholm_1938");
            RT900gon = ProjectionInfo.FromEpsgCode(3022).SetNames("RT90_0_gon", "GCS_RT_1990", "D_RT_1990");
            RT9025gonO = ProjectionInfo.FromEpsgCode(3023).SetNames("RT90_25_gon_O", "GCS_RT_1990", "D_RT_1990");
            RT9025gonV = ProjectionInfo.FromEpsgCode(3021).SetNames("RT90_25_gon_V", "GCS_RT_1990", "D_RT_1990");
            RT905gonO = ProjectionInfo.FromEpsgCode(3024).SetNames("RT90_5_gon_O", "GCS_RT_1990", "D_RT_1990");
            RT905gonV = ProjectionInfo.FromEpsgCode(3020).SetNames("RT90_5_gon_V", "GCS_RT_1990", "D_RT_1990");
            RT9075gonV = ProjectionInfo.FromEpsgCode(3019).SetNames("RT90_75_gon_V", "GCS_RT_1990", "D_RT_1990");
            SWEREF991200 = ProjectionInfo.FromEpsgCode(3007).SetNames("SWEREF99_12_00", "GCS_SWEREF99", "D_SWEREF99");
            SWEREF991330 = ProjectionInfo.FromEpsgCode(3008).SetNames("SWEREF99_13_30", "GCS_SWEREF99", "D_SWEREF99");
            SWEREF991415 = ProjectionInfo.FromEpsgCode(3012).SetNames("SWEREF99_14_15", "GCS_SWEREF99", "D_SWEREF99");
            SWEREF991500 = ProjectionInfo.FromEpsgCode(3009).SetNames("SWEREF99_15_00", "GCS_SWEREF99", "D_SWEREF99");
            SWEREF991545 = ProjectionInfo.FromEpsgCode(3013).SetNames("SWEREF99_15_45", "GCS_SWEREF99", "D_SWEREF99");
            SWEREF991630 = ProjectionInfo.FromEpsgCode(3010).SetNames("SWEREF99_16_30", "GCS_SWEREF99", "D_SWEREF99");
            SWEREF991715 = ProjectionInfo.FromEpsgCode(3014).SetNames("SWEREF99_17_15", "GCS_SWEREF99", "D_SWEREF99");
            SWEREF991800 = ProjectionInfo.FromEpsgCode(3011).SetNames("SWEREF99_18_00", "GCS_SWEREF99", "D_SWEREF99");
            SWEREF991845 = ProjectionInfo.FromEpsgCode(3015).SetNames("SWEREF99_18_45", "GCS_SWEREF99", "D_SWEREF99");
            SWEREF992015 = ProjectionInfo.FromEpsgCode(3016).SetNames("SWEREF99_20_15", "GCS_SWEREF99", "D_SWEREF99");
            SWEREF992145 = ProjectionInfo.FromEpsgCode(3017).SetNames("SWEREF99_21_45", "GCS_SWEREF99", "D_SWEREF99");
            SWEREF992315 = ProjectionInfo.FromEpsgCode(3018).SetNames("SWEREF99_23_15", "GCS_SWEREF99", "D_SWEREF99");
            SWEREF99TM = ProjectionInfo.FromEpsgCode(3006).SetNames("SWEREF99_TM", "GCS_SWEREF99", "D_SWEREF99");
        }

        #endregion
    }
}

#pragma warning restore 1591