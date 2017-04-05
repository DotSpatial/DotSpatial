// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:49:00 PM
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
    /// This class contains predefined CoordinateSystems for Indiansubcontinent.
    /// </summary>
    public class Indiansubcontinent : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Kalianpur1880IndiaZone0;
        public readonly ProjectionInfo Kalianpur1880IndiaZoneI;
        public readonly ProjectionInfo Kalianpur1880IndiaZoneIIa;
        public readonly ProjectionInfo Kalianpur1880IndiaZoneIIb;
        public readonly ProjectionInfo Kalianpur1880IndiaZoneIII;
        public readonly ProjectionInfo Kalianpur1880IndiaZoneIV;
        public readonly ProjectionInfo Kalianpur1937IndiaZoneIIb;
        public readonly ProjectionInfo Kalianpur1937UTMZone45N;
        public readonly ProjectionInfo Kalianpur1937UTMZone46N;
        public readonly ProjectionInfo Kalianpur1962IndiaZoneI;
        public readonly ProjectionInfo Kalianpur1962IndiaZoneIIa;
        public readonly ProjectionInfo Kalianpur1962UTMZone41N;
        public readonly ProjectionInfo Kalianpur1962UTMZone42N;
        public readonly ProjectionInfo Kalianpur1962UTMZone43N;
        public readonly ProjectionInfo Kalianpur1975IndiaZoneI;
        public readonly ProjectionInfo Kalianpur1975IndiaZoneIIa;
        public readonly ProjectionInfo Kalianpur1975IndiaZoneIIb;
        public readonly ProjectionInfo Kalianpur1975IndiaZoneIII;
        public readonly ProjectionInfo Kalianpur1975IndiaZoneIV;
        public readonly ProjectionInfo Kalianpur1975UTMZone42N;
        public readonly ProjectionInfo Kalianpur1975UTMZone43N;
        public readonly ProjectionInfo Kalianpur1975UTMZone44N;
        public readonly ProjectionInfo Kalianpur1975UTMZone45N;
        public readonly ProjectionInfo Kalianpur1975UTMZone46N;
        public readonly ProjectionInfo Kalianpur1975UTMZone47N;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Indiansubcontinent.
        /// </summary>
        public Indiansubcontinent()
        {
            Kalianpur1880IndiaZone0 = ProjectionInfo.FromEpsgCode(24370).SetNames("Kalianpur_1880_India_Zone_0", "GCS_Kalianpur_1880", "D_Kalianpur_1880");
            Kalianpur1880IndiaZoneI = ProjectionInfo.FromEpsgCode(24371).SetNames("Kalianpur_1880_India_Zone_I", "GCS_Kalianpur_1880", "D_Kalianpur_1880");
            Kalianpur1880IndiaZoneIIa = ProjectionInfo.FromEpsgCode(24372).SetNames("Kalianpur_1880_India_Zone_IIa", "GCS_Kalianpur_1880", "D_Kalianpur_1880");
            Kalianpur1880IndiaZoneIIb = ProjectionInfo.FromEpsgCode(24382).SetNames("Kalianpur_1880_India_Zone_IIb", "GCS_Kalianpur_1880", "D_Kalianpur_1880");
            Kalianpur1880IndiaZoneIII = ProjectionInfo.FromEpsgCode(24373).SetNames("Kalianpur_1880_India_Zone_III", "GCS_Kalianpur_1880", "D_Kalianpur_1880");
            Kalianpur1880IndiaZoneIV = ProjectionInfo.FromEpsgCode(24374).SetNames("Kalianpur_1880_India_Zone_IV", "GCS_Kalianpur_1880", "D_Kalianpur_1880");
            Kalianpur1937IndiaZoneIIb = ProjectionInfo.FromEpsgCode(24375).SetNames("Kalianpur_1937_India_Zone_IIb", "GCS_Kalianpur_1937", "D_Kalianpur_1937");
            Kalianpur1937UTMZone45N = ProjectionInfo.FromEpsgCode(24305).SetNames("Kalianpur_1937_UTM_Zone_45N", "GCS_Kalianpur_1937", "D_Kalianpur_1937");
            Kalianpur1937UTMZone46N = ProjectionInfo.FromEpsgCode(24306).SetNames("Kalianpur_1937_UTM_Zone_46N", "GCS_Kalianpur_1937", "D_Kalianpur_1937");
            Kalianpur1962IndiaZoneI = ProjectionInfo.FromEpsgCode(24376).SetNames("Kalianpur_1962_India_Zone_I", "GCS_Kalianpur_1962", "D_Kalianpur_1962");
            Kalianpur1962IndiaZoneIIa = ProjectionInfo.FromEpsgCode(24377).SetNames("Kalianpur_1962_India_Zone_IIa", "GCS_Kalianpur_1962", "D_Kalianpur_1962");
            Kalianpur1962UTMZone41N = ProjectionInfo.FromEpsgCode(24311).SetNames("Kalianpur_1962_UTM_Zone_41N", "GCS_Kalianpur_1962", "D_Kalianpur_1962");
            Kalianpur1962UTMZone42N = ProjectionInfo.FromEpsgCode(24312).SetNames("Kalianpur_1962_UTM_Zone_42N", "GCS_Kalianpur_1962", "D_Kalianpur_1962");
            Kalianpur1962UTMZone43N = ProjectionInfo.FromEpsgCode(24313).SetNames("Kalianpur_1962_UTM_Zone_43N", "GCS_Kalianpur_1962", "D_Kalianpur_1962");
            Kalianpur1975IndiaZoneI = ProjectionInfo.FromEpsgCode(24378).SetNames("Kalianpur_1975_India_Zone_I", "GCS_Kalianpur_1975", "D_Kalianpur_1975");
            Kalianpur1975IndiaZoneIIa = ProjectionInfo.FromEpsgCode(24379).SetNames("Kalianpur_1975_India_Zone_IIa", "GCS_Kalianpur_1975", "D_Kalianpur_1975");
            Kalianpur1975IndiaZoneIIb = ProjectionInfo.FromEpsgCode(24380).SetNames("Kalianpur_1975_India_Zone_IIb", "GCS_Kalianpur_1975", "D_Kalianpur_1975");
            Kalianpur1975IndiaZoneIII = ProjectionInfo.FromEpsgCode(24381).SetNames("Kalianpur_1975_India_Zone_III", "GCS_Kalianpur_1975", "D_Kalianpur_1975");
            Kalianpur1975IndiaZoneIV = ProjectionInfo.FromEpsgCode(24383).SetNames("Kalianpur_1975_India_Zone_IV", "GCS_Kalianpur_1975", "D_Kalianpur_1975");
            Kalianpur1975UTMZone42N = ProjectionInfo.FromEpsgCode(24342).SetNames("Kalianpur_1975_UTM_Zone_42N", "GCS_Kalianpur_1975", "D_Kalianpur_1975");
            Kalianpur1975UTMZone43N = ProjectionInfo.FromEpsgCode(24343).SetNames("Kalianpur_1975_UTM_Zone_43N", "GCS_Kalianpur_1975", "D_Kalianpur_1975");
            Kalianpur1975UTMZone44N = ProjectionInfo.FromEpsgCode(24344).SetNames("Kalianpur_1975_UTM_Zone_44N", "GCS_Kalianpur_1975", "D_Kalianpur_1975");
            Kalianpur1975UTMZone45N = ProjectionInfo.FromEpsgCode(24345).SetNames("Kalianpur_1975_UTM_Zone_45N", "GCS_Kalianpur_1975", "D_Kalianpur_1975");
            Kalianpur1975UTMZone46N = ProjectionInfo.FromEpsgCode(24346).SetNames("Kalianpur_1975_UTM_Zone_46N", "GCS_Kalianpur_1975", "D_Kalianpur_1975");
            Kalianpur1975UTMZone47N = ProjectionInfo.FromEpsgCode(24347).SetNames("Kalianpur_1975_UTM_Zone_47N", "GCS_Kalianpur_1975", "D_Kalianpur_1975");
        }

        #endregion
    }
}

#pragma warning restore 1591