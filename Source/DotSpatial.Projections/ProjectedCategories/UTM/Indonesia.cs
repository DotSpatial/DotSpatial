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

namespace DotSpatial.Projections.ProjectedCategories.UTM
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for Indonesia.
    /// </summary>
    public class Indonesia : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo BataviaUTMZone48S;
        public readonly ProjectionInfo BataviaUTMZone49S;
        public readonly ProjectionInfo BataviaUTMZone50S;
        public readonly ProjectionInfo DGN1995UTMZone46N;
        public readonly ProjectionInfo DGN1995UTMZone47N;
        public readonly ProjectionInfo DGN1995UTMZone47S;
        public readonly ProjectionInfo DGN1995UTMZone48N;
        public readonly ProjectionInfo DGN1995UTMZone48S;
        public readonly ProjectionInfo DGN1995UTMZone49N;
        public readonly ProjectionInfo DGN1995UTMZone49S;
        public readonly ProjectionInfo DGN1995UTMZone50N;
        public readonly ProjectionInfo DGN1995UTMZone50S;
        public readonly ProjectionInfo DGN1995UTMZone51N;
        public readonly ProjectionInfo DGN1995UTMZone51S;
        public readonly ProjectionInfo DGN1995UTMZone52N;
        public readonly ProjectionInfo DGN1995UTMZone52S;
        public readonly ProjectionInfo DGN1995UTMZone53S;
        public readonly ProjectionInfo DGN1995UTMZone54S;
        public readonly ProjectionInfo GunungSegaraUTMZone50S;
        public readonly ProjectionInfo Indonesia1974UTMZone46N;
        public readonly ProjectionInfo Indonesia1974UTMZone46S;
        public readonly ProjectionInfo Indonesia1974UTMZone47N;
        public readonly ProjectionInfo Indonesia1974UTMZone47S;
        public readonly ProjectionInfo Indonesia1974UTMZone48N;
        public readonly ProjectionInfo Indonesia1974UTMZone48S;
        public readonly ProjectionInfo Indonesia1974UTMZone49N;
        public readonly ProjectionInfo Indonesia1974UTMZone49S;
        public readonly ProjectionInfo Indonesia1974UTMZone50N;
        public readonly ProjectionInfo Indonesia1974UTMZone50S;
        public readonly ProjectionInfo Indonesia1974UTMZone51N;
        public readonly ProjectionInfo Indonesia1974UTMZone51S;
        public readonly ProjectionInfo Indonesia1974UTMZone52N;
        public readonly ProjectionInfo Indonesia1974UTMZone52S;
        public readonly ProjectionInfo Indonesia1974UTMZone53N;
        public readonly ProjectionInfo Indonesia1974UTMZone53S;
        public readonly ProjectionInfo Indonesia1974UTMZone54S;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Indonesia.
        /// </summary>
        public Indonesia()
        {
            BataviaUTMZone48S = ProjectionInfo.FromEpsgCode(21148).SetNames("Batavia_UTM_Zone_48S", "GCS_Batavia", "D_Batavia");
            BataviaUTMZone49S = ProjectionInfo.FromEpsgCode(21149).SetNames("Batavia_UTM_Zone_49S", "GCS_Batavia", "D_Batavia");
            BataviaUTMZone50S = ProjectionInfo.FromEpsgCode(21150).SetNames("Batavia_UTM_Zone_50S", "GCS_Batavia", "D_Batavia");
            DGN1995UTMZone46N = ProjectionInfo.FromEpsgCode(23866).SetNames("DGN_1995_UTM_Zone_46N", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995UTMZone47N = ProjectionInfo.FromEpsgCode(23867).SetNames("DGN_1995_UTM_Zone_47N", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995UTMZone47S = ProjectionInfo.FromEpsgCode(23877).SetNames("DGN_1995_UTM_Zone_47S", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995UTMZone48N = ProjectionInfo.FromEpsgCode(23868).SetNames("DGN_1995_UTM_Zone_48N", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995UTMZone48S = ProjectionInfo.FromEpsgCode(23878).SetNames("DGN_1995_UTM_Zone_48S", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995UTMZone49N = ProjectionInfo.FromEpsgCode(23869).SetNames("DGN_1995_UTM_Zone_49N", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995UTMZone49S = ProjectionInfo.FromEpsgCode(23879).SetNames("DGN_1995_UTM_Zone_49S", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995UTMZone50N = ProjectionInfo.FromEpsgCode(23870).SetNames("DGN_1995_UTM_Zone_50N", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995UTMZone50S = ProjectionInfo.FromEpsgCode(23880).SetNames("DGN_1995_UTM_Zone_50S", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995UTMZone51N = ProjectionInfo.FromEpsgCode(23871).SetNames("DGN_1995_UTM_Zone_51N", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995UTMZone51S = ProjectionInfo.FromEpsgCode(23881).SetNames("DGN_1995_UTM_Zone_51S", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995UTMZone52N = ProjectionInfo.FromEpsgCode(23872).SetNames("DGN_1995_UTM_Zone_52N", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995UTMZone52S = ProjectionInfo.FromEpsgCode(23882).SetNames("DGN_1995_UTM_Zone_52S", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995UTMZone53S = ProjectionInfo.FromEpsgCode(23883).SetNames("DGN_1995_UTM_Zone_53S", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            DGN1995UTMZone54S = ProjectionInfo.FromEpsgCode(23884).SetNames("DGN_1995_UTM_Zone_54S", "GCS_DGN_1995", "D_Datum_Geodesi_Nasional_1995");
            GunungSegaraUTMZone50S = ProjectionInfo.FromEpsgCode(2933).SetNames("Gunung_Segara_UTM_Zone_50S", "GCS_Gunung_Segara", "D_Gunung_Segara");
            Indonesia1974UTMZone46N = ProjectionInfo.FromEpsgCode(23846).SetNames("Indonesian_1974_UTM_Zone_46N", "GCS_Indonesian_1974", "D_Indonesian_1974");
            Indonesia1974UTMZone46S = ProjectionInfo.FromEpsgCode(23886).SetNames("Indonesian_1974_UTM_Zone_46S", "GCS_Indonesian_1974", "D_Indonesian_1974");
            Indonesia1974UTMZone47N = ProjectionInfo.FromEpsgCode(23847).SetNames("Indonesian_1974_UTM_Zone_47N", "GCS_Indonesian_1974", "D_Indonesian_1974");
            Indonesia1974UTMZone47S = ProjectionInfo.FromEpsgCode(23887).SetNames("Indonesian_1974_UTM_Zone_47S", "GCS_Indonesian_1974", "D_Indonesian_1974");
            Indonesia1974UTMZone48N = ProjectionInfo.FromEpsgCode(23848).SetNames("Indonesian_1974_UTM_Zone_48N", "GCS_Indonesian_1974", "D_Indonesian_1974");
            Indonesia1974UTMZone48S = ProjectionInfo.FromEpsgCode(23888).SetNames("Indonesian_1974_UTM_Zone_48S", "GCS_Indonesian_1974", "D_Indonesian_1974");
            Indonesia1974UTMZone49N = ProjectionInfo.FromEpsgCode(23849).SetNames("Indonesian_1974_UTM_Zone_49N", "GCS_Indonesian_1974", "D_Indonesian_1974");
            Indonesia1974UTMZone49S = ProjectionInfo.FromEpsgCode(23889).SetNames("Indonesian_1974_UTM_Zone_49S", "GCS_Indonesian_1974", "D_Indonesian_1974");
            Indonesia1974UTMZone50N = ProjectionInfo.FromEpsgCode(23850).SetNames("Indonesian_1974_UTM_Zone_50N", "GCS_Indonesian_1974", "D_Indonesian_1974");
            Indonesia1974UTMZone50S = ProjectionInfo.FromEpsgCode(23890).SetNames("Indonesian_1974_UTM_Zone_50S", "GCS_Indonesian_1974", "D_Indonesian_1974");
            Indonesia1974UTMZone51N = ProjectionInfo.FromEpsgCode(23851).SetNames("Indonesian_1974_UTM_Zone_51N", "GCS_Indonesian_1974", "D_Indonesian_1974");
            Indonesia1974UTMZone51S = ProjectionInfo.FromEpsgCode(23891).SetNames("Indonesian_1974_UTM_Zone_51S", "GCS_Indonesian_1974", "D_Indonesian_1974");
            Indonesia1974UTMZone52N = ProjectionInfo.FromEpsgCode(23852).SetNames("Indonesian_1974_UTM_Zone_52N", "GCS_Indonesian_1974", "D_Indonesian_1974");
            Indonesia1974UTMZone52S = ProjectionInfo.FromEpsgCode(23892).SetNames("Indonesian_1974_UTM_Zone_52S", "GCS_Indonesian_1974", "D_Indonesian_1974");
            Indonesia1974UTMZone53N = ProjectionInfo.FromEpsgCode(23853).SetNames("Indonesian_1974_UTM_Zone_53N", "GCS_Indonesian_1974", "D_Indonesian_1974");
            Indonesia1974UTMZone53S = ProjectionInfo.FromEpsgCode(23893).SetNames("Indonesian_1974_UTM_Zone_53S", "GCS_Indonesian_1974", "D_Indonesian_1974");
            Indonesia1974UTMZone54S = ProjectionInfo.FromEpsgCode(23894).SetNames("Indonesian_1974_UTM_Zone_54S", "GCS_Indonesian_1974", "D_Indonesian_1974");
        }

        #endregion
    }
}

#pragma warning restore 1591