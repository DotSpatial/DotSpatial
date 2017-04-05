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

namespace DotSpatial.Projections.GeographicCategories
{
    /// <summary>
    /// This class contains predefined CoordinateSystems for IndianOcean.
    /// </summary>
    public class IndianOcean : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo Anna1Astro1965;
        public readonly ProjectionInfo Combani1950;
        public readonly ProjectionInfo Gan1970;
        public readonly ProjectionInfo GrandComoros;
        public readonly ProjectionInfo ISTS073Astro1969;
        public readonly ProjectionInfo KerguelenIsland1949;
        public readonly ProjectionInfo Mahe1971;
        public readonly ProjectionInfo Reunion1947;
        public readonly ProjectionInfo RGR1992;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of IndianOcean.
        /// </summary>
        public IndianOcean()
        {
            Anna1Astro1965 = ProjectionInfo.FromEpsgCode(4708).SetNames("", "GCS_Anna_1_1965", "D_Anna_1_1965");
            Combani1950 = ProjectionInfo.FromEpsgCode(4632).SetNames("", "GCS_Combani_1950", "D_Combani_1950");
            Gan1970 = ProjectionInfo.FromEpsgCode(4684).SetNames("", "GCS_Gan_1970", "D_Gan_1970");
            GrandComoros = ProjectionInfo.FromEpsgCode(4646).SetNames("", "GCS_Grand_Comoros", "D_Grand_Comoros");
            ISTS073Astro1969 = ProjectionInfo.FromEpsgCode(4724).SetNames("", "GCS_ISTS_073_1969", "D_ISTS_073_1969");
            KerguelenIsland1949 = ProjectionInfo.FromEpsgCode(4698).SetNames("", "GCS_Kerguelen_Island_1949", "D_Kerguelen_Island_1949");
            Mahe1971 = ProjectionInfo.FromEpsgCode(4256).SetNames("", "GCS_Mahe_1971", "D_Mahe_1971");
            Reunion1947 = ProjectionInfo.FromEpsgCode(4626).SetNames("", "GCS_Reunion_1947", "D_Reunion_1947");
            RGR1992 = ProjectionInfo.FromEpsgCode(4627).SetNames("", "GCS_RGR_1992", "D_RGR_1992");
        }

        #endregion
    }
}

#pragma warning restore 1591