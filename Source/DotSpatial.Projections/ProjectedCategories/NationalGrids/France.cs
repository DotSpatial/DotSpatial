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
    /// This class contains predefined CoordinateSystems for France.
    /// </summary>
    public class France : CoordinateSystemCategory
    {
        #region Private Variables
        // ReSharper disable InconsistentNaming
        public readonly ProjectionInfo ED1950FranceEuroLambert;
        public readonly ProjectionInfo FranceI;
        public readonly ProjectionInfo FranceII;
        public readonly ProjectionInfo FranceIII;
        public readonly ProjectionInfo FranceIV;
        public readonly ProjectionInfo NorddeGuerre;
        public readonly ProjectionInfo NTFFranceIDegrees;
        public readonly ProjectionInfo NTFFranceIIDegrees;
        public readonly ProjectionInfo NTFFranceIIIDegrees;
        public readonly ProjectionInfo NTFFranceIVDegrees;
        public readonly ProjectionInfo NTFLambertZoneI;
        public readonly ProjectionInfo NTFLambertZoneII;
        public readonly ProjectionInfo NTFLambertZoneIII;
        public readonly ProjectionInfo NTFLambertZoneIV;
        public readonly ProjectionInfo NTFParisCentreFrance;
        public readonly ProjectionInfo NTFParisCorse;
        public readonly ProjectionInfo NTFParisLambertCentreFrance;
        public readonly ProjectionInfo NTFParisLambertCorse;
        public readonly ProjectionInfo NTFParisLambertNordFrance;
        public readonly ProjectionInfo NTFParisLambertSudFrance;
        public readonly ProjectionInfo NTFParisLambertZoneI;
        public readonly ProjectionInfo NTFParisLambertZoneII;
        public readonly ProjectionInfo NTFParisLambertZoneIII;
        public readonly ProjectionInfo NTFParisLambertZoneIV;
        public readonly ProjectionInfo NTFParisNordFrance;
        public readonly ProjectionInfo NTFParisSudFrance;
        public readonly ProjectionInfo RGF1993CC42;
        public readonly ProjectionInfo RGF1993CC43;
        public readonly ProjectionInfo RGF1993CC44;
        public readonly ProjectionInfo RGF1993CC45;
        public readonly ProjectionInfo RGF1993CC46;
        public readonly ProjectionInfo RGF1993CC47;
        public readonly ProjectionInfo RGF1993CC48;
        public readonly ProjectionInfo RGF1993CC49;
        public readonly ProjectionInfo RGF1993CC50;
        public readonly ProjectionInfo RGF1993Lambert93;
        // ReSharper restore InconsistentNaming
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of France.
        /// </summary>
        public France()
        {
            ED1950FranceEuroLambert = ProjectionInfo.FromEpsgCode(2192).SetNames("ED_1950_France_EuroLambert", "GCS_European_1950", "D_European_1950");
            FranceI = ProjectionInfo.FromEpsgCode(27581).SetNames("NTF_Paris_France_I", "GCS_NTF_Paris", "D_NTF");
            FranceII = ProjectionInfo.FromEpsgCode(27582).SetNames("NTF_Paris_France_II", "GCS_NTF_Paris", "D_NTF");
            FranceIII = ProjectionInfo.FromEpsgCode(27583).SetNames("NTF_Paris_France_III", "GCS_NTF_Paris", "D_NTF");
            FranceIV = ProjectionInfo.FromEpsgCode(27584).SetNames("NTF_Paris_France_IV", "GCS_NTF_Paris", "D_NTF");
            NorddeGuerre = ProjectionInfo.FromEpsgCode(27500).SetNames("Nord_de_Guerre", "GCS_ATF_Paris", "D_ATF");
            NTFFranceIDegrees = ProjectionInfo.FromAuthorityCode("ESRI", 102581).SetNames("NTF_France_I_degrees", "GCS_NTF", "D_NTF");
            NTFFranceIIDegrees = ProjectionInfo.FromAuthorityCode("ESRI", 102582).SetNames("NTF_France_II_degrees", "GCS_NTF", "D_NTF");
            NTFFranceIIIDegrees = ProjectionInfo.FromAuthorityCode("ESRI", 102583).SetNames("NTF_France_III_degrees", "GCS_NTF", "D_NTF");
            NTFFranceIVDegrees = ProjectionInfo.FromAuthorityCode("ESRI", 102584).SetNames("NTF_France_IV_degrees", "GCS_NTF", "D_NTF");
            NTFLambertZoneI = ProjectionInfo.FromAuthorityCode("ESRI", 102585).SetNames("NTF_Lambert_Zone_I", "GCS_NTF", "D_NTF"); // missing
            NTFLambertZoneII = ProjectionInfo.FromAuthorityCode("ESRI", 102586).SetNames("NTF_Lambert_Zone_II", "GCS_NTF", "D_NTF"); // missing
            NTFLambertZoneIII = ProjectionInfo.FromAuthorityCode("ESRI", 102587).SetNames("NTF_Lambert_Zone_III", "GCS_NTF", "D_NTF"); // missing
            NTFLambertZoneIV = ProjectionInfo.FromAuthorityCode("ESRI", 102588).SetNames("NTF_Lambert_Zone_IV", "GCS_NTF", "D_NTF"); // missing
            NTFParisCentreFrance = ProjectionInfo.FromEpsgCode(27592).SetNames("NTF_Paris_Centre_France", "GCS_NTF_Paris", "D_NTF");
            NTFParisCorse = ProjectionInfo.FromEpsgCode(27594).SetNames("NTF_Paris_Corse", "GCS_NTF_Paris", "D_NTF");
            NTFParisLambertCentreFrance = ProjectionInfo.FromEpsgCode(27562).SetNames("NTF_Paris_Lambert_Centre_France", "GCS_NTF_Paris", "D_NTF");
            NTFParisLambertCorse = ProjectionInfo.FromEpsgCode(27564).SetNames("NTF_Paris_Lambert_Corse", "GCS_NTF_Paris", "D_NTF");
            NTFParisLambertNordFrance = ProjectionInfo.FromEpsgCode(27561).SetNames("NTF_Paris_Lambert_Nord_France", "GCS_NTF_Paris", "D_NTF");
            NTFParisLambertSudFrance = ProjectionInfo.FromEpsgCode(27563).SetNames("NTF_Paris_Lambert_Sud_France", "GCS_NTF_Paris", "D_NTF");
            NTFParisLambertZoneI = ProjectionInfo.FromEpsgCode(27571).SetNames("NTF_Paris_Lambert_Zone_I", "GCS_NTF_Paris", "D_NTF");
            NTFParisLambertZoneII = ProjectionInfo.FromEpsgCode(27572).SetNames("NTF_Paris_Lambert_Zone_II", "GCS_NTF_Paris", "D_NTF");
            NTFParisLambertZoneIII = ProjectionInfo.FromEpsgCode(27573).SetNames("NTF_Paris_Lambert_Zone_III", "GCS_NTF_Paris", "D_NTF");
            NTFParisLambertZoneIV = ProjectionInfo.FromEpsgCode(27574).SetNames("NTF_Paris_Lambert_Zone_IV", "GCS_NTF_Paris", "D_NTF");
            NTFParisNordFrance = ProjectionInfo.FromEpsgCode(27591).SetNames("NTF_Paris_Nord_France", "GCS_NTF_Paris", "D_NTF");
            NTFParisSudFrance = ProjectionInfo.FromEpsgCode(27593).SetNames("NTF_Paris_Sud_France", "GCS_NTF_Paris", "D_NTF");
            RGF1993CC42 = ProjectionInfo.FromEpsgCode(3942).SetNames("RGF_1993_CC42", "GCS_RGF_1993", "D_RGF_1993");
            RGF1993CC43 = ProjectionInfo.FromEpsgCode(3943).SetNames("RGF_1993_CC43", "GCS_RGF_1993", "D_RGF_1993");
            RGF1993CC44 = ProjectionInfo.FromEpsgCode(3944).SetNames("RGF_1993_CC44", "GCS_RGF_1993", "D_RGF_1993");
            RGF1993CC45 = ProjectionInfo.FromEpsgCode(3945).SetNames("RGF_1993_CC45", "GCS_RGF_1993", "D_RGF_1993");
            RGF1993CC46 = ProjectionInfo.FromEpsgCode(3946).SetNames("RGF_1993_CC46", "GCS_RGF_1993", "D_RGF_1993");
            RGF1993CC47 = ProjectionInfo.FromEpsgCode(3947).SetNames("RGF_1993_CC47", "GCS_RGF_1993", "D_RGF_1993");
            RGF1993CC48 = ProjectionInfo.FromEpsgCode(3948).SetNames("RGF_1993_CC48", "GCS_RGF_1993", "D_RGF_1993");
            RGF1993CC49 = ProjectionInfo.FromEpsgCode(3949).SetNames("RGF_1993_CC49", "GCS_RGF_1993", "D_RGF_1993");
            RGF1993CC50 = ProjectionInfo.FromEpsgCode(3950).SetNames("RGF_1993_CC50", "GCS_RGF_1993", "D_RGF_1993");
            RGF1993Lambert93 = ProjectionInfo.FromEpsgCode(2154).SetNames("RGF_1993_Lambert_93", "GCS_RGF_1993", "D_RGF_1993");
        }

        #endregion
    }
}

#pragma warning restore 1591