// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:06:28 PM
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
//        Name         |    Date    |        Comment
// --------------------|------------|------------------------------------------------------------
// Ted Dunsford        |   5/3/2010 |  Updated project to DotSpatial.Projection and license to LGPL
// ********************************************************************************************************

#pragma warning disable 1591

namespace DotSpatial.Projections.GeographicCategories
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for AustraliaNewZealand.
    /// </summary>
    public class AustraliaNewZealand : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AustralianGeodeticDatum1966;
        public readonly ProjectionInfo AustralianGeodeticDatum1984;
        public readonly ProjectionInfo ChathamIslands1979;
        public readonly ProjectionInfo GeocentricDatumofAustralia1994;
        public readonly ProjectionInfo NewZealandGeodeticDatum1949;
        public readonly ProjectionInfo NZGD2000;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of AustraliaNewZealand.
        /// </summary>
        public AustraliaNewZealand()
        {
            AustralianGeodeticDatum1966 = ProjectionInfo.FromEpsgCode(4202).SetNames("", "GCS_Australian_1966", "D_Australian_1966");
            AustralianGeodeticDatum1984 = ProjectionInfo.FromEpsgCode(4203).SetNames("", "GCS_Australian_1984", "D_Australian_1984");
            ChathamIslands1979 = ProjectionInfo.FromEpsgCode(4673).SetNames("", "GCS_Chatham_Islands_1979", "D_Chatham_Islands_1979");
            GeocentricDatumofAustralia1994 = ProjectionInfo.FromEpsgCode(4283).SetNames("", "GCS_GDA_1994", "D_GDA_1994");
            NewZealandGeodeticDatum1949 = ProjectionInfo.FromEpsgCode(4272).SetNames("", "GCS_New_Zealand_1949", "D_New_Zealand_1949");
            NZGD2000 = ProjectionInfo.FromEpsgCode(4167).SetNames("", "GCS_NZGD_2000", "D_NZGD_2000");
        }

        #endregion
    }
}

#pragma warning restore 1591