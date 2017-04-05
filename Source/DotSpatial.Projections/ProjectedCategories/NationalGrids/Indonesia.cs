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
    /// This class contains predefined CoordinateSystems for Indonesia.
    /// </summary>
    public class Indonesia : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo BataviaNEIEZ;
        public readonly ProjectionInfo BataviaTM109SE;
        public readonly ProjectionInfo DGN1995IndonesiaTM3Zone462;
        public readonly ProjectionInfo DGN1995IndonesiaTM3Zone471;
        public readonly ProjectionInfo DGN1995IndonesiaTM3Zone472;
        public readonly ProjectionInfo DGN1995IndonesiaTM3Zone481;
        public readonly ProjectionInfo DGN1995IndonesiaTM3Zone482;
        public readonly ProjectionInfo DGN1995IndonesiaTM3Zone491;
        public readonly ProjectionInfo DGN1995IndonesiaTM3Zone492;
        public readonly ProjectionInfo DGN1995IndonesiaTM3Zone501;
        public readonly ProjectionInfo DGN1995IndonesiaTM3Zone502;
        public readonly ProjectionInfo DGN1995IndonesiaTM3Zone511;
        public readonly ProjectionInfo DGN1995IndonesiaTM3Zone512;
        public readonly ProjectionInfo DGN1995IndonesiaTM3Zone521;
        public readonly ProjectionInfo DGN1995IndonesiaTM3Zone522;
        public readonly ProjectionInfo DGN1995IndonesiaTM3Zone531;
        public readonly ProjectionInfo DGN1995IndonesiaTM3Zone532;
        public readonly ProjectionInfo DGN1995IndonesiaTM3Zone541;
        public readonly ProjectionInfo GunungSegaraNEIEZ;
        public readonly ProjectionInfo MakassarNEIEZ;
        public readonly ProjectionInfo SegaraJakartaNEIEZ;
        public readonly ProjectionInfo WGS1984TM116SE;
        public readonly ProjectionInfo WGS1984TM132SE;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Indonesia.
        /// </summary>
        public Indonesia()
        {
            BataviaNEIEZ = ProjectionInfo.FromEpsgCode(3001).SetNames("Batavia_NEIEZ", "GCS_Batavia", "D_Batavia");
            BataviaTM109SE = ProjectionInfo.FromEpsgCode(2308).SetNames("Batavia_TM_109_SE", "GCS_Batavia", "D_Batavia");
            DGN1995IndonesiaTM3Zone462 = ProjectionInfo.FromEpsgCode(23830).SetNames("DGN_1995_Indonesia_TM-3_Zone_46.2", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995IndonesiaTM3Zone471 = ProjectionInfo.FromEpsgCode(23831).SetNames("DGN_1995_Indonesia_TM-3_Zone_47.1", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995IndonesiaTM3Zone472 = ProjectionInfo.FromEpsgCode(23832).SetNames("DGN_1995_Indonesia_TM-3_Zone_47.2", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995IndonesiaTM3Zone481 = ProjectionInfo.FromEpsgCode(23833).SetNames("DGN_1995_Indonesia_TM-3_Zone_48.1", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995IndonesiaTM3Zone482 = ProjectionInfo.FromEpsgCode(23834).SetNames("DGN_1995_Indonesia_TM-3_Zone_48.2", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995IndonesiaTM3Zone491 = ProjectionInfo.FromEpsgCode(23835).SetNames("DGN_1995_Indonesia_TM-3_Zone_49.1", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995IndonesiaTM3Zone492 = ProjectionInfo.FromEpsgCode(23836).SetNames("DGN_1995_Indonesia_TM-3_Zone_49.2", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995IndonesiaTM3Zone501 = ProjectionInfo.FromEpsgCode(23837).SetNames("DGN_1995_Indonesia_TM-3_Zone_50.1", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995IndonesiaTM3Zone502 = ProjectionInfo.FromEpsgCode(23838).SetNames("DGN_1995_Indonesia_TM-3_Zone_50.2", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995IndonesiaTM3Zone511 = ProjectionInfo.FromEpsgCode(23839).SetNames("DGN_1995_Indonesia_TM-3_Zone_51.1", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995IndonesiaTM3Zone512 = ProjectionInfo.FromEpsgCode(23840).SetNames("DGN_1995_Indonesia_TM-3_Zone_51.2", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995IndonesiaTM3Zone521 = ProjectionInfo.FromEpsgCode(23841).SetNames("DGN_1995_Indonesia_TM-3_Zone_52.1", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995IndonesiaTM3Zone522 = ProjectionInfo.FromEpsgCode(23842).SetNames("DGN_1995_Indonesia_TM-3_Zone_52.2", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995IndonesiaTM3Zone531 = ProjectionInfo.FromEpsgCode(23843).SetNames("DGN_1995_Indonesia_TM-3_Zone_53.1", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995IndonesiaTM3Zone532 = ProjectionInfo.FromEpsgCode(23844).SetNames("DGN_1995_Indonesia_TM-3_Zone_53.2", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995IndonesiaTM3Zone541 = ProjectionInfo.FromEpsgCode(23845).SetNames("DGN_1995_Indonesia_TM-3_Zone_54.1", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            GunungSegaraNEIEZ = ProjectionInfo.FromEpsgCode(3000).SetNames("Gunung_Segara_NEIEZ", "GCS_Gunung_Segara", "D_Gunung_Segara");
            MakassarNEIEZ = ProjectionInfo.FromEpsgCode(3002).SetNames("Makassar_NEIEZ", "GCS_Makassar", "D_Makassar");
            SegaraJakartaNEIEZ = ProjectionInfo.FromEpsgCode(2934).SetNames("Gunung_Segara_Jakarta_NEIEZ", "GCS_Gunung_Segara_Jakarta", "D_Gunung_Segara");
            WGS1984TM116SE = ProjectionInfo.FromEpsgCode(2309).SetNames("WGS_1984_TM_116_SE", "GCS_WGS_1984", "D_WGS_1984");
            WGS1984TM132SE = ProjectionInfo.FromEpsgCode(2310).SetNames("WGS_1984_TM_132_SE", "GCS_WGS_1984", "D_WGS_1984");
        }

        #endregion
    }
}

#pragma warning restore 1591