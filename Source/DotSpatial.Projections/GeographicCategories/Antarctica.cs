// ********************************************************************************************************
// Product Name: DotSpatial.Projection
// Description:  The basic module for MapWindow version 6.0
// ********************************************************************************************************
//
// The Original Code is from MapWindow.dll version 6.0
//
// The Initial Developer of this Original Code is Ted Dunsford. Created 8/14/2009 4:03:57 PM
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
    /// This class contains predefined CoordinateSystems for Antarctica.
    /// </summary>
    public class Antarctica : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo AustralianAntarctic1998;
        public readonly ProjectionInfo CampAreaAstro;
        public readonly ProjectionInfo DeceptionIsland;
        public readonly ProjectionInfo Petrels1972;
        public readonly ProjectionInfo PointeGeologiePerroud1950;
        public readonly ProjectionInfo RSRGD2000;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of Antarctica.
        /// </summary>
        public Antarctica()
        {
            AustralianAntarctic1998 = ProjectionInfo.FromEpsgCode(4176).SetNames("", "GCS_Australian_Antarctic_1998", "D_Australian_Antarctic_1998");
            CampAreaAstro = ProjectionInfo.FromEpsgCode(4715).SetNames("", "GCS_Camp_Area", "D_Camp_Area");
            DeceptionIsland = ProjectionInfo.FromEpsgCode(4736).SetNames("", "GCS_Deception_Island", "D_Deception_Island");
            Petrels1972 = ProjectionInfo.FromEpsgCode(4636).SetNames("", "GCS_Petrels_1972", "D_Petrels_1972");
            PointeGeologiePerroud1950 = ProjectionInfo.FromEpsgCode(4637).SetNames("", "GCS_Pointe_Geologie_Perroud_1950", "D_Pointe_Geologie_Perroud_1950");
            RSRGD2000 = ProjectionInfo.FromEpsgCode(4764).SetNames("", "GCS_RSRGD2000", "D_Ross_Sea_Region_Geodetic_Datum_2000");
        }

        #endregion
    }
}

#pragma warning restore 1591